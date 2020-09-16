namespace Kafka.Producer
{
  using Confluent.Kafka;
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
                   services.Configure<ProducerConfig>(hostContext.Configuration.GetSection("ProducerConfig"));
                   services.AddHostedService<Worker>();
                 });
    }
  }
}