namespace Kafka.CassandraConsumer
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using Kafka.CassandraConsumer.Database;
  using Kafka.CassandraConsumer.Metadata;
  using Kafka.CassandraConsumer.Models;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  public class Worker : BackgroundService
  {
    private readonly ILogger<Worker> _logger;
    private readonly IOptions<CassandraSettings> settings;
    private readonly ILoggerFactory loggerFactory;

    public Worker(ILogger<Worker> logger, IOptions<CassandraSettings> settings, ILoggerFactory loggerFactory)
    {
      this._logger = logger;
      this.settings = settings;
      this.loggerFactory = loggerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        await using var context = new HelloContext(this.settings, this.loggerFactory);

        var events = await context.HelloEvents.AsNoTracking()
                                  .ToListAsync(stoppingToken);

        this._logger.LogInformation($"Current events persisted: {events.Count}");

        context.HelloEvents.Add(new HelloEvent
        {
          Id = Guid.NewGuid(),
          Message = "Hello to Cassandra cluster",
          Timestamp = DateTimeOffset.UtcNow,
        });

        await context.SaveChangesAsync(stoppingToken);

        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}