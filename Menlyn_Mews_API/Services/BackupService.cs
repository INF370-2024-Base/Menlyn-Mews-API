using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Menlyn_Mews_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Services
{
    public class BackupService
    {
        private readonly string _projectId;
        private readonly string _bucketName;
        private readonly string _credentialsPath;
        private readonly AppDbContext _context;
        private readonly ILogger<BackupService> _logger;
        private readonly IConfiguration _configuration;

        public BackupService(IConfiguration configuration, AppDbContext context, ILogger<BackupService> logger)
        {
            _projectId = configuration["GoogleCloud:ProjectId"] ?? throw new ArgumentNullException(nameof(configuration), "ProjectId is not configured.");
            _bucketName = configuration["GoogleCloud:BucketName"] ?? throw new ArgumentNullException(nameof(configuration), "BucketName is not configured.");
            _credentialsPath = configuration["GoogleCloud:CredentialsPath"] ?? throw new ArgumentNullException(nameof(configuration), "CredentialsPath is not configured.");
            _context = context ?? throw new ArgumentNullException(nameof(context), "AppDbContext is not configured.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger is not configured.");
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Configuration is not configured.");
        }

        public async Task BackupDatabaseAsync()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var backupFileName = $"backup_{timestamp}.bak";

            // Ensure the backup directory exists
            var backupDirectory = "C:\\Backup";
            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
            }

            var backupFilePath = Path.Combine(backupDirectory, backupFileName);
            var sql = $"BACKUP DATABASE [{_context.Database.GetDbConnection().Database}] TO DISK = '{backupFilePath}'";

            try
            {
                _logger.LogInformation($"Starting database backup: {backupFilePath}");
                await _context.Database.ExecuteSqlRawAsync(sql);
                _logger.LogInformation($"Database backup completed: {backupFilePath}");

                // Upload to Google Cloud Storage
                _logger.LogInformation($"Uploading backup to Google Cloud Storage: {_bucketName}/{backupFileName}");
                var storageClient = StorageClient.Create(GoogleCredential.FromFile(_credentialsPath));
                using (var fileStream = new FileStream(backupFilePath, FileMode.Open))
                {
                    await storageClient.UploadObjectAsync(_bucketName, backupFileName, null, fileStream);
                }
                _logger.LogInformation($"Backup uploaded to Google Cloud Storage: {_bucketName}/{backupFileName}");

                // Clean up the temporary file
                File.Delete(backupFilePath);
                _logger.LogInformation($"Temporary backup file deleted: {backupFilePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the backup process.");
                throw;
            }
        }

        public async Task RestoreDatabaseAsync(string backupFileName)
        {
            var backupFilePath = Path.Combine("C:\\Backup", backupFileName);
            var databaseName = _context.Database.GetDbConnection().Database;

            try
            {
                var masterConnectionString = _configuration.GetConnectionString("MasterConnection");
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer(masterConnectionString);

                using (var masterContext = new AppDbContext(optionsBuilder.Options))
                {
                    // Close all existing connections to the database
                    var killConnectionsSql = @$"
                DECLARE @kill varchar(8000) = '';
                SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(5), session_id) + ';'
                FROM sys.dm_exec_sessions
                WHERE database_id  = DB_ID('{databaseName}')
                EXEC(@kill);
            ";
                    await masterContext.Database.ExecuteSqlRawAsync(killConnectionsSql);
                    _logger.LogInformation($"All connections to database {databaseName} have been killed.");

                    // Set the database to single-user mode
                    var setSingleUserSql = $"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                    await masterContext.Database.ExecuteSqlRawAsync(setSingleUserSql);
                    _logger.LogInformation($"Database {databaseName} set to SINGLE_USER mode.");

                    // Restore the database
                    var restoreSql = $"RESTORE DATABASE [{databaseName}] FROM DISK = '{backupFilePath}' WITH REPLACE;";
                    await masterContext.Database.ExecuteSqlRawAsync(restoreSql);
                    _logger.LogInformation($"Database restored from {backupFilePath}.");

                    // Set the database back to multi-user mode
                    var setMultiUserSql = $"ALTER DATABASE [{databaseName}] SET MULTI_USER;";
                    await masterContext.Database.ExecuteSqlRawAsync(setMultiUserSql);
                    _logger.LogInformation($"Database {databaseName} set to MULTI_USER mode.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the restore process.");
                throw;
            }
        }

    }
}
