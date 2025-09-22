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
    private const string SupportProjectForeignKeyName = "SupportProjectId";

    public DbSet<SupportProject> SupportProjects { get; set; } = null!;

    public DbSet<SupportProjectNote> ProjectNotes { get; set; } = null!;

    public DbSet<SupportProjectContact> Contacts { get; set; } = null!;

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
        modelBuilder.Entity<SupportProjectContact>(ConfigureSupportProjectContacts);
        modelBuilder.Entity<FundingHistory>(ConfigureFundingHistory);
        modelBuilder.Entity<ImprovementPlan>(ConfigureImprovementPlan);
        modelBuilder.Entity<ImprovementPlanObjective>(ConfigureImprovementPlanObjective);
        modelBuilder.Entity<ImprovementPlanReview>(ConfigureImprovementPlanReview);
        modelBuilder.Entity<ImprovementPlanObjectiveProgress>(ConfigureImprovementPlanObjectiveProgress);
        modelBuilder.Entity<EngagementConcern>(ConfigureEngagementConcerns);

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
            .HasForeignKey(SupportProjectForeignKeyName)
            .IsRequired();

        supportProjectConfiguration
            .HasMany(a => a.Contacts)
            .WithOne()
            .HasForeignKey(SupportProjectForeignKeyName)
            .IsRequired();

        supportProjectConfiguration
            .HasMany(a => a.FundingHistories)
            .WithOne()
            .HasForeignKey(SupportProjectForeignKeyName)
            .IsRequired();

        supportProjectConfiguration
            .HasMany(a => a.ImprovementPlans)
            .WithOne()
            .HasForeignKey(SupportProjectForeignKeyName)
            .IsRequired();
        
        supportProjectConfiguration
            .HasMany(a => a.EngagementConcerns)
            .WithOne()
            .HasForeignKey(SupportProjectForeignKeyName)
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
        fundingHistoryConfiguration.Property(e => e.ReadableId).UseIdentityColumn();
        fundingHistoryConfiguration.Property(e => e.FundingAmount).HasColumnType("decimal(18,2)");
        fundingHistoryConfiguration.Property(e => e.Id)
            .HasConversion(
                v => v!.Value,
                v => new FundingHistoryId(v));
    }

    private static void ConfigureSupportProjectContacts(EntityTypeBuilder<SupportProjectContact> supportProjectContactsConfiguration)
    {
        supportProjectContactsConfiguration.ToTable("SupportProjectContacts", DefaultSchema, b => b.IsTemporal());
        supportProjectContactsConfiguration.HasKey(a => a.Id);

        supportProjectContactsConfiguration.Property(e => e.Id)
            .HasConversion(
                v => v!.Value,
                v => new SupportProjectContactId(v));
    }

    private static void ConfigureImprovementPlanObjective(EntityTypeBuilder<ImprovementPlanObjective> builder)
    {
        builder.ToTable("ImprovementPlanObjectives", DefaultSchema, b => b.IsTemporal());
        builder.HasKey(a => a.Id);
        builder.Property(e => e.ReadableId).UseIdentityColumn();
        builder.Property(e => e.Id)
            .HasConversion(
                v => v!.Value,
                v => new ImprovementPlanObjectiveId(v));
    }

    private static void ConfigureImprovementPlan(EntityTypeBuilder<ImprovementPlan> builder)
    {
        builder.ToTable("ImprovementPlans", DefaultSchema, b => b.IsTemporal());
        builder.HasKey(a => a.Id);
        builder.Property(e => e.ReadableId).UseIdentityColumn();
        builder.Property(e => e.Id)
            .HasConversion(
                v => v!.Value,
                v => new ImprovementPlanId(v));

        builder.HasMany(a => a.ImprovementPlanObjectives)
            .WithOne()
            .HasForeignKey("ImprovementPlanId")
            .IsRequired();

        builder.HasMany(a => a.ImprovementPlanReviews)
            .WithOne()
            .HasForeignKey("ImprovementPlanId")
            .IsRequired();
    }

    private static void ConfigureImprovementPlanObjectiveProgress(EntityTypeBuilder<ImprovementPlanObjectiveProgress> builder)
    {
        builder.ToTable("ImprovementPlanObjectiveProgresses", DefaultSchema, b => b.IsTemporal());
        builder.HasKey(a => a.Id);
        builder.Property(e => e.ReadableId).UseIdentityColumn();
        builder.Property(e => e.Id)
            .HasConversion(
                v => v!.Value,
                v => new ImprovementPlanObjectiveProgressId(v));
        builder.Property(e => e.ImprovementPlanObjectiveId)
            .HasConversion(
                v => v!.Value,
                v => new ImprovementPlanObjectiveId(v));
        builder.Property(e => e.ImprovementPlanReviewId)
            .HasConversion(
                v => v!.Value,
                v => new ImprovementPlanReviewId(v));
    }

    private static void ConfigureImprovementPlanReview(EntityTypeBuilder<ImprovementPlanReview> builder)
    {
        builder.ToTable("ImprovementPlanReviews", DefaultSchema, b => b.IsTemporal());
        builder.HasKey(a => a.Id);
        builder.Property(e => e.ReadableId).UseIdentityColumn();
        builder.Property(e => e.Id)
            .HasConversion(
                v => v!.Value,
                v => new ImprovementPlanReviewId(v));

        builder.HasMany(a => a.ImprovementPlanObjectiveProgresses)
            .WithOne()
            .HasForeignKey("ImprovementPlanReviewId")
            .IsRequired();
    }
    
    private static void ConfigureEngagementConcerns(EntityTypeBuilder<EngagementConcern> builder)
    {
        builder.ToTable("EngagementConcerns", DefaultSchema, b => b.IsTemporal());
        builder.HasKey(a => a.Id);

        builder.Property(e => e.Id)
            .HasConversion(
                v => v!.Value,
                v => new EngagementConcernId(v));
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
