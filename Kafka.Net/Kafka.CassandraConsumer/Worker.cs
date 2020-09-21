namespace Kafka.CassandraConsumer
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;

  public class Worker : BackgroundService
  {
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
      this._logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        this._logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}