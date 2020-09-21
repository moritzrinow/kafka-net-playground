namespace Kafka.CassandraConsumer.Database
{
  using Kafka.CassandraConsumer.Metadata;
  using Kafka.CassandraConsumer.Models;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Migrations;
  using Microsoft.Extensions.Options;

  public class HelloContext : DbContext
  {
    private readonly CassandraSettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="HelloContext" /> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public HelloContext(IOptions<CassandraSettings> settings)
    {
      this.settings = settings.Value;
    }

    /// <summary>
    /// Gets or sets the hello events.
    /// </summary>
    /// <value>
    /// The hello events.
    /// </value>
    public virtual DbSet<HelloEvent> HelloEvents { get; set; }

    /// <summary>
    ///   <para>
    ///   Override this method to configure the database (and other options) to be used for this context.
    ///   This method is called for each instance of the context that is created.
    ///   The base implementation does nothing.
    ///   </para>
    ///   <para>
    ///   In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not
    ///   have been passed
    ///   to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" />
    ///   to determine if
    ///   the options have already been set, and skip some or all of the logic in
    ///   <see
    ///     cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />
    ///   .
    ///   </para>
    /// </summary>
    /// <param name="optionsBuilder">
    /// A builder used to create or modify options for this context. Databases (and other extensions)
    /// typically define extension methods on this object that allow you to configure the context.
    /// </param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseCassandra(this.settings.ConnectionString, options => { options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "master"); });

      base.OnConfiguring(optionsBuilder);
    }

    /// <summary>
    /// Override this method to further configure the model that was discovered by convention from the entity types
    /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting
    /// model may be cached
    /// and re-used for subsequent instances of your derived context.
    /// </summary>
    /// <param name="modelBuilder">
    /// The builder being used to construct the model for this context. Databases (and other extensions) typically
    /// define extension methods on this object that allow you to configure aspects of the model that are specific
    /// to a given database.
    /// </param>
    /// <remarks>
    /// If a model is explicitly set on the options for this context (via
    /// <see
    ///   cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />
    /// )
    /// then this method will not be run.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }
  }
}