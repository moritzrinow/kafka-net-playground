namespace Kafka.Producer
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using Confluent.Kafka;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  public class Worker : BackgroundService
  {
    private readonly ILogger<Worker> _logger;
    private readonly IProducer<Null, string> producer;
    private readonly IAdminClient admin;

    public Worker(ILogger<Worker> logger, IOptions<ProducerConfig> config, IOptions<AdminClientConfig> adminConfig)
    {
      this._logger = logger;
      this.producer = new ProducerBuilder<Null, string>(config.Value).Build();
      this.admin = new AdminClientBuilder(adminConfig.Value).Build();
    }

    public override void Dispose()
    {
      this.producer.Dispose();
      base.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          DeliveryResult<Null, string> result = await this.producer.ProduceAsync("hello-topic", new Message<Null, string>
          {
            Value = $"Hello subscribers, the current time is {DateTimeOffset.UtcNow}"
          }, stoppingToken);

          this._logger.LogInformation("Message sent");

          await Task.Delay(500, stoppingToken);
        }
        catch (ProduceException<Null, string> ex)
        {
          this._logger.LogError($"Produce error: {ex.Error.Reason}");
        }
      }
    }
  }
}