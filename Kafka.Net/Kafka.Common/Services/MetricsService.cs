namespace Kafka.Common.Services
{
  using System.Threading;
  using System.Threading.Tasks;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;

  public class MetricsService : BackgroundService
  {
    private readonly ILogger<MetricsService> logger;
    private volatile KafkaMetrics metrics;

    public MetricsService(ILogger<MetricsService> logger, KafkaMetrics metrics)
    {
      this.logger = logger;
      this.metrics = metrics;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        await Task.Delay(1000, stoppingToken);

        this.logger.LogInformation($"Events per second: {this.metrics.EventsPerSecond}");

        this.metrics.EventsPerSecond = 0L;
      }
    }
  }
}