using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Infrastructure.Database.Interceptors;
using Dfe.ManageSchoolImprovement.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Dfe.ManageSchoolImprovement.Infrastructure.Database;

public class RegionalImprovementForStandardsAndExcellenceContext(DbContextOptions<RegionalImprovementForStandardsAndExcellenceContext> options, IConfiguration configuration, IMediator mediator, IUserContextService userContextService) : DbContext(options)
{
    private readonly IConfiguration? _configuration = configuration;
    const string DefaultSchema = "RISE";

    public DbSet<SupportProject> SupportProjects { get; set; } = null!;

    public DbSet<SupportProjectNote> ProjectNotes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration!.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        optionsBuilder.AddInterceptors(new DomainEventDispatcherInterceptor(mediator));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SupportProject>(ConfigureSupportProject);
        modelBuilder.Entity<SupportProjectNote>(ConfigureSupportProjectNotes);
        modelBuilder.Entity<FundingHistory>(ConfigureFundingHistory);

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigureSupportProject(EntityTypeBuilder<SupportProject> supportProjectConfiguration)
    {
        supportProjectConfiguration.HasKey(s => s.Id);
        supportProjectConfiguration.ToTable("SupportProject", DefaultSchema, b => b.IsTemporal());
        supportProjectConfiguration.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(
                v => v!.Value,
                v => new SupportProjectId(v));

        supportProjectConfiguration
            .HasMany(a => a.Notes)
            .WithOne()
            .HasForeignKey("SupportProjectId")
            .IsRequired();

        supportProjectConfiguration
            .HasMany(a => a.FundingHistories)
            .WithOne()
            .HasForeignKey("SupportProjectId")
            .IsRequired();

        supportProjectConfiguration
            .HasQueryFilter(p => p.DeletedAt == null);
    }

    private static void ConfigureSupportProjectNotes(EntityTypeBuilder<SupportProjectNote> supportProjectNoteConfiguration)
    {
        supportProjectNoteConfiguration.ToTable("SupportProjectNotes", DefaultSchema, b => b.IsTemporal());
        supportProjectNoteConfiguration.HasKey(a => a.Id);
        supportProjectNoteConfiguration.Property(e => e.Id)
            .HasConversion(
                v => v!.Value,
                v => new SupportProjectNoteId(v));
    }
    private static void ConfigureFundingHistory(EntityTypeBuilder<FundingHistory> fundingHistoryConfiguration)
    {
        fundingHistoryConfiguration.ToTable("FundingHistories", DefaultSchema, b => b.IsTemporal());
        fundingHistoryConfiguration.HasKey(a => a.Id);
        fundingHistoryConfiguration.Property(e => e.Id)
            .HasConversion(
                v => v!.Value,
                v => new FundingHistoryId(v));
    }

    public override int SaveChanges()
    {
        SetAuditFields();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditFields()
    {
        var currentUsername = userContextService.GetCurrentUsername();

        // for new domain object mapped directly to the database
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditableEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (IAuditableEntity)entry.Entity;
            var utcNow = DateTime.UtcNow;
            entity.LastModifiedOn = utcNow;
            entity.LastModifiedBy = currentUsername;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedOn = utcNow;
                entity.CreatedBy = currentUsername;
            }
        }
    }
}
