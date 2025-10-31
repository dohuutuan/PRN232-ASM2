    using FUNewsManagement_AnalyticsAPI.Services;

    namespace FUNewsManagement_AnalyticsAPI.Background
    {
        public class AnalyticsCacheWorker : BackgroundService
        {
            private readonly AnalyticsCacheService _cacheService;
            private readonly ILogger<AnalyticsCacheWorker> _logger;

            public AnalyticsCacheWorker(AnalyticsCacheService cacheService, ILogger<AnalyticsCacheWorker> logger)
            {
                _cacheService = cacheService;
                _logger = logger;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                _logger.LogInformation("✅ AnalyticsCacheWorker started.");

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        await _cacheService.RefreshAllAsync();
                        _logger.LogInformation("🟢 Cache refreshed at {time}", DateTime.Now);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Error while refreshing analytics cache");
                    }

                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // refresh mỗi 5 phút
                }
            }
        }
    }
