using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Services;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly BackupService _backupService;
        private readonly ISchedulerFactory _schedulerFactory;

        public BackupController(BackupService backupService, ISchedulerFactory schedulerFactory)
        {
            _backupService = backupService;
            _schedulerFactory = schedulerFactory;
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleBackup([FromBody] BackupScheduleRequest request)
        {
            try
            {
                Console.WriteLine($"Received schedule request: Frequency = {request.Frequency}");

                // Hard-coded cron expressions for specific frequencies and times
                string cronExpression = request.Frequency.ToLower() switch
                {
                    "hourly" => "0 0 * * * ?",                // At the top of every hour
                    "daily" => "0 34 2 * * ?",                // Every day at 2:34 AM
                    "weekly" => "0 34 2 ? * MON",             // Every Monday at 2:34 AM
                    "bi-weekly" => "0 34 2 ? * MON#2",        // Every two weeks on the first Monday at 2:34 AM
                    "monthly" => "0 34 2 1 * ?",              // The first day of every month at 2:34 AM
                    "yearly" => "0 34 2 1 1 ?",               // January 1st at 2:34 AM
                    "every-weekday" => "0 0 9 ? * MON-FRI",   // Every weekday at 9:00 AM
                    "weekend" => "0 0 10 ? * SAT,SUN",        // Every weekend at 10:00 AM
                    _ => throw new ArgumentException("Invalid frequency")
                };

                Console.WriteLine($"Using hard-coded cron expression: {cronExpression}");

                if (!CronExpression.IsValidExpression(cronExpression))
                {
                    return BadRequest(new { message = "Invalid cron expression" });
                }

                var scheduler = await _schedulerFactory.GetScheduler();

                var jobKey = new JobKey("BackupJob");
                var triggerKey = new TriggerKey("BackupJob-trigger");

                // Check if the job already exists
                if (await scheduler.CheckExists(jobKey))
                {
                    await scheduler.DeleteJob(jobKey);
                    Console.WriteLine("Deleted existing job with key: 'BackupJob'");
                }

                var job = JobBuilder.Create<BackupJob>()
                    .WithIdentity(jobKey)
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(triggerKey)
                    .WithCronSchedule(cronExpression)
                    .ForJob(jobKey)
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

                Console.WriteLine($"Backup scheduled with frequency {request.Frequency}");

                return Ok(new { message = "Backup scheduled successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error scheduling backup: {ex.Message}");
                return StatusCode(500, new { message = "Error scheduling backup", details = ex.Message });
            }
        }

        [HttpPost("trigger")]
        public async Task<IActionResult> TriggerBackup()
        {
            try
            {
                await _backupService.BackupDatabaseAsync();
                return Ok(new { message = "Backup triggered successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error triggering backup: {ex.Message}");
                return StatusCode(500, new { message = "Error triggering backup", details = ex.Message });
            }
        }

        [HttpPost("restore")]
        public async Task<IActionResult> RestoreDatabase([FromBody] RestoreDatabaseRequest request)
        {
            try
            {
                await _backupService.RestoreDatabaseAsync(request.BackupFileName);
                return Ok(new { message = "Database restore triggered successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error triggering restore: {ex.Message}");
                return StatusCode(500, new { message = "Error triggering restore", details = ex.Message });
            }
        }
    }

    public class BackupScheduleRequest
    {
        public string Time { get; set; }
        public string Frequency { get; set; }
    }

    public class RestoreDatabaseRequest
    {
        public string BackupFileName { get; set; }
    }
}
