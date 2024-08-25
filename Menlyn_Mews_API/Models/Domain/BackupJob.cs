using Quartz;
using Menlyn_Mews_API.Services;

namespace Menlyn_Mews_API.Models.Domain
{
    public class BackupJob : IJob
    {
        private readonly BackupService _backupService;

        public BackupJob(BackupService backupService)
        {
            _backupService = backupService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _backupService.BackupDatabaseAsync();
        }
    }
}
