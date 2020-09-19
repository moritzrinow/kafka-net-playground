namespace Kafka.Common.Services
{
  using Microsoft.Extensions.DependencyInjection;

  public static class MetricsServiceExtensions
  {
    public static IServiceCollection AddKafkaMetrics(this IServiceCollection services)
    {
      services.AddSingleton<KafkaMetrics>();
      services.AddHostedService<MetricsService>();

      return services;
    }
  }
}