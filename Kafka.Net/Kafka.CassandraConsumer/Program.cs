namespace Kafka.CassandraConsumer
{
  using Kafka.CassandraConsumer.Database;
  using Kafka.CassandraConsumer.Metadata;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;

  public class Program
  {
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public static void Main(string[] args)
    {
      Program.CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Creates the host builder.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
                 .ConfigureServices((hostContext, services) =>
                 {
                   services.Configure<CassandraSettings>(hostContext.Configuration.GetSection("Cassandra"));
                   services.AddDbContext<HelloContext>();
                   services.AddHostedService<Worker>();
                 });
    }
  }
}