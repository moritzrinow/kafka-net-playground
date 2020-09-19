namespace Kafka.Consumer
{
  using Confluent.Kafka;
  using Kafka.Common.Services;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;

  public class Program
  {
    public static void Main(string[] args)
    {
      Program.CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
                 .ConfigureServices((hostContext, services) =>
                 {
                   services.Configure<AdminClientConfig>(hostContext.Configuration.GetSection("AdminConfig"));
                   services.Configure<ConsumerConfig>(hostContext.Configuration.GetSection("ConsumerConfig"));
                   services.AddKafkaMetrics();
                   services.AddHostedService<Worker>();
                 });
    }
  }
}