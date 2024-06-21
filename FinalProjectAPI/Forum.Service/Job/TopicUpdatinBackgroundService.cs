using Forum.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Forum.Service.Job
{
    public class TopicUpdatinBackgroundService : BackgroundService
    {

        private readonly ILogger<TopicUpdatinBackgroundService> _logger;
        private readonly int _daysToInactive;
        private readonly IServiceProvider _serviceProvider;

        public TopicUpdatinBackgroundService(IServiceProvider serviceProvider, ILogger<TopicUpdatinBackgroundService> logger, int days)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _daysToInactive = days;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Update is started . ");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateStatusAsync(stoppingToken);
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR WHILE HELLO EXECUTING BACKGROUND JOB");
                }
                finally
                {
                    _logger.LogInformation("Update is stopping.");
                }
            }
        }

        public async Task UpdateStatusAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var cutoffDate = DateTime.Now.AddDays(_daysToInactive);
                var topicsToUpdate = await dbContext.Topics
                    .Where(t => t.Status == true)
                    .Where(t => t.Comments.OrderByDescending(c => c.CreatedTime).FirstOrDefault().CreatedTime < cutoffDate)
                    .ToListAsync(stoppingToken);

                foreach (var topic in topicsToUpdate)
                {
                    topic.Status = false;
                }

                await dbContext.SaveChangesAsync(stoppingToken);

                _logger.LogInformation($"Updated {topicsToUpdate.Count} topics to inactive.");
            }
        }
    }
}
