namespace Kafka.Consumer
{
  using System.Threading;
  using System.Threading.Tasks;
  using Confluent.Kafka;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  public class Worker : BackgroundService
  {
    private readonly ILogger<Worker> _logger;
    private readonly IConsumer<Ignore, string> consumer;

    public Worker(ILogger<Worker> logger, IOptions<ConsumerConfig> config)
    {
      this._logger = logger;
      this.consumer = new ConsumerBuilder<Ignore, string>(config.Value).Build();
      this.consumer.Subscribe("hello-topic");
    }

    public override void Dispose()
    {
      this.consumer.Dispose();
      base.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          ConsumeResult<Ignore, string> result = this.consumer.Consume(stoppingToken);
          this._logger.LogInformation($"Received message: '{result.Message.Value}' at: '{result.TopicPartitionOffset}'");
        }
        catch (ConsumeException ex)
        {
          this._logger.LogError($"Receive error: {ex.Error.Reason}");
        }

        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}