using System.Reflection;
using ProjectManager.Core.ProjectAggregate;
using ProjectManager.SharedKernel;
using ProjectManager.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Infrastructure.Data.Config;

namespace ProjectManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options)
  {
  }

  public DbSet<Project2> Projects2 => Set<Project2>();
  public DbSet<User> Users => Set<User>();
  public DbSet<Assignment> Assignments => Set<Assignment>();
  public DbSet<Message> Messages => Set<Message>();
  public DbSet<AssignmentStage> AssignmentStages => Set<AssignmentStage>();
  public DbSet<ChatChannel> ChatChannels => Set<ChatChannel>();

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseLazyLoadingProxies();

    if (optionsBuilder.IsConfigured) return;

    if (AppDbContextOptions.IsTesting)
    {
      optionsBuilder.UseInMemoryDatabase("projectmanagerDb");
    }
    else
    {
      optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=projectmanagerDb;User Id=postgres;Password=postgrespw;Pooling=false",
        b => b.MigrationsAssembly("ProjectManager.Infrastructure"));

      optionsBuilder.EnableSensitiveDataLogging();
    }
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    //modelBuilder.Entity<User>()
    //            .HasMany(u => u.Projects)
    //            .WithMany(p => p.Users)
    //            .UsingEntity(j => j.ToTable("UserProjects"));

    //modelBuilder.Entity<Project2>()
    //            .HasOne(p => p.Manager)
    //            .WithMany(u => u.ManagedProjects)
    //            .HasForeignKey(p => p.ManagerId);

    //modelBuilder.Entity<Assignment>()
    //            .HasOne(a => a.Project)
    //            .WithMany(p => p.Assignments)
    //            .HasForeignKey(a => a.ProjectId);

    //modelBuilder.Entity<Assignment>()
    //            .HasMany(a => a.Users)
    //            .WithMany(u => u.Assignments)
    //            .UsingEntity(j => j.ToTable("AssignmentUser"));

    //modelBuilder.Entity<Message>()
    //            .HasOne(m => m.User)
    //            .WithMany(u => u.Messages)
    //            .HasForeignKey(m => m.UserId);

    //modelBuilder.Entity<Message>()
    //            .HasOne(m => m.Project)
    //            .WithMany(p => p.Messages)
    //            .HasForeignKey(m => m.ProjectId);
    
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
