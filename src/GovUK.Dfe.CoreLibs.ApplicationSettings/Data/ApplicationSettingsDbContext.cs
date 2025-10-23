using GovUK.Dfe.CoreLibs.ApplicationSettings.Configuration;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GovUK.Dfe.CoreLibs.ApplicationSettings.Data;

public class ApplicationSettingsDbContext : DbContext
{
    private readonly ApplicationSettingsOptions _options;

    public ApplicationSettingsDbContext(DbContextOptions<ApplicationSettingsDbContext> options, IOptions<ApplicationSettingsOptions> settingsOptions)
        : base(options)
    {
        _options = settingsOptions.Value;
    }

    public DbSet<ApplicationSetting> ApplicationSettings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply schema if specified
        if (!string.IsNullOrEmpty(_options.Schema))
        {
            modelBuilder.HasDefaultSchema(_options.Schema);
        }

        modelBuilder.Entity<ApplicationSetting>(entity =>
        {
            // If schema is specified, apply it to the table
            if (!string.IsNullOrEmpty(_options.Schema))
            {
                entity.ToTable("ApplicationSettings", _options.Schema);
            }
            else
            {
                entity.ToTable("ApplicationSettings");
            }

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Key)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Value)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("General");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255);

            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255);

            // Create unique index on Key for fast lookups
            entity.HasIndex(e => e.Key)
                .IsUnique()
                .HasDatabaseName("IX_ApplicationSettings_Key");

            // Create index on Category for filtered queries
            entity.HasIndex(e => e.Category)
                .HasDatabaseName("IX_ApplicationSettings_Category");
        });

        base.OnModelCreating(modelBuilder);
    }
}