namespace Kafka.Producer
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using Confluent.Kafka;
  using Confluent.Kafka.Admin;
  using Kafka.Common;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.Options;

  public class Worker : BackgroundService
  {
    private readonly ILogger<Worker> _logger;
    private readonly IProducer<Null, string> producer;
    private readonly IAdminClient admin;
    private readonly KafkaMetrics metrics;

    public Worker(ILogger<Worker> logger, IOptions<ProducerConfig> config, IOptions<AdminClientConfig> adminConfig, KafkaMetrics metrics)
    {
      this._logger = logger;
      this.producer = new ProducerBuilder<Null, string>(config.Value).Build();
      this.admin = new AdminClientBuilder(adminConfig.Value).Build();
      this.metrics = metrics;
    }

    public override void Dispose()
    {
      this.producer.Dispose();
      base.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      await this.EnsureTopicExistsAsync(stoppingToken);

      while (!stoppingToken.IsCancellationRequested)
      {
        try
        {
          this.producer.Produce("hello-topic", new Message<Null, string>
          {
            Value = $"Hello subscribers, the current time is {DateTimeOffset.UtcNow}"
          });

          this.metrics.EventsPerSecond++;
        }
        catch (ProduceException<Null, string> ex)
        {
          this._logger.LogError($"Produce error: {ex.Error.Reason}");
        }
      }
    }

    protected async Task EnsureTopicExistsAsync(CancellationToken stoppingToken)
    {
      var metadata = this.admin.GetMetadata(TimeSpan.FromMilliseconds(2000));
      if (metadata.Topics.Any(p => p.Topic == "hello-topic"))
      {
        return;
      }

      var topics = new List<TopicSpecification>
      {
        new TopicSpecification
        {
          Name = "hello-topic",
          NumPartitions = 1
        }
      };

      await this.admin.CreateTopicsAsync(topics);

      this._logger.LogInformation("Created topic 'hello-topic'");
    }
  }
}