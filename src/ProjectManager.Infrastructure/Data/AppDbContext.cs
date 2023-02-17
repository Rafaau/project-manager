using System.Reflection;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Infrastructure.Data.Config;
using Microsoft.Extensions.Options;

namespace ProjectManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options)
  {
  }

  public DbSet<Project2> Projects2 => Set<Project2>();
  public DbSet<User> Users => Set<User>();
  public DbSet<Assignment> Assignments => Set<Assignment>();
  public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
  public DbSet<AssignmentStage> AssignmentStages => Set<AssignmentStage>();
  public DbSet<ChatChannel> ChatChannels => Set<ChatChannel>();
  public DbSet<Appointment> Appointments => Set<Appointment>();
  public DbSet<Notification> Notifications => Set<Notification>();
  public DbSet<PrivateMessage> PrivateMessages => Set<PrivateMessage>();
  public DbSet<InvitationLink> InvitationLinks => Set<InvitationLink>();

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseLazyLoadingProxies();

    if (optionsBuilder.IsConfigured) return;

    if (AppDbContextOptions.IsTesting || AppDbContextOptions.IsE2ETesting)
      optionsBuilder.UseNpgsql(AppDbContextOptions.ConnectionString, 
      b => b.EnableRetryOnFailure(10, TimeSpan.FromSeconds(3), null)
            .MigrationsAssembly("ProjectManager.Infrastructure"));
    else
    {
      optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=projectManagerDb;User Id=postgres;Password=postgrespw;Pooling=false",
        b => b.MigrationsAssembly("ProjectManager.Infrastructure"));

      optionsBuilder.EnableSensitiveDataLogging();
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    return result;
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}
