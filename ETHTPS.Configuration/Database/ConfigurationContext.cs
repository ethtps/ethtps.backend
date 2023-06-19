using ETHTPS.Configuration.Extensions;

using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Configuration.Database;

public partial class ConfigurationContext : ConfigurationContextBase
{
    public ConfigurationContext()
    {

    }

    public ConfigurationContext(DbContextOptions<ConfigurationContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Environment>? Environments { get; set; }

    public virtual DbSet<Microservice>? Microservices { get; set; }

    public virtual DbSet<MicroserviceConfigurationString>? MicroserviceConfigurationStrings { get; set; }


    public virtual DbSet<Provider>? Providers { get; set; }

    public virtual DbSet<ConfigurationString>? ConfigurationStrings { get; set; }


    public virtual DbSet<ProviderConfigurationString>? ProviderConfigurationStrings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfigurationString>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Configur__3214EC274EBBB2FD");

            entity.ToTable("ConfigurationStrings", "Configuration");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EncryptionAlgorithmOrHint).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Value).HasMaxLength(255);
        });

        modelBuilder.Entity<ConfigurationStringTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Configur__3214EC278BB1A05A");

            entity.ToTable("ConfigurationStringTags", "Configuration");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ConfigurationStringId).HasColumnName("ConfigurationStringID");
            entity.Property(e => e.TagId).HasColumnName("TagID");

            entity.HasOne(d => d.ConfigurationString).WithMany(p => p.ConfigurationStringTags)
                .HasForeignKey(d => d.ConfigurationStringId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Configura__Confi__5C57A83E");

            entity.HasOne(d => d.Tag).WithMany(p => p.ConfigurationStringTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Configura__TagID__5D4BCC77");
        });


        modelBuilder.Entity<Environment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Environm__3214EC27D353D7B2");

            entity.ToTable("Environments", "Configuration");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Microservice>(entity =>
        {
            modelBuilder.ConfigureEntityPrimaryKey<Microservice>();
            entity.HasKey(e => e.Id).HasName("PK__Microser__3214EC27B5F7310C");

            entity.ToTable("Microservices", "Microservices");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<MicroserviceConfigurationString>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Microser__3214EC276D4B1F06");

            entity.ToTable("MicroserviceConfigurationStrings", "Configuration");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ConfigurationStringId).HasColumnName("ConfigurationStringID");
            entity.Property(e => e.EnvironmentId).HasColumnName("EnvironmentID");
            entity.Property(e => e.MicroserviceId).HasColumnName("MicroserviceID");

            entity.HasOne(d => d.ConfigurationString).WithMany(p => p.MicroserviceConfigurationStrings)
                .HasForeignKey(d => d.ConfigurationStringId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Microserv__Confi__1411F17C");

            entity.HasOne(d => d.Environment).WithMany(p => p.MicroserviceConfigurationStrings)
                .HasForeignKey(d => d.EnvironmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Microserv__Envir__150615B5");

            entity.HasOne(d => d.Microservice).WithMany(p => p.MicroserviceConfigurationStrings)
                .HasForeignKey(d => d.MicroserviceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Microserv__Micro__131DCD43");
        });


        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC272CD5F6F0");

            entity.HasIndex(e => e.Name, "UQ__Tags__737584F69720953B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<MicroserviceTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Microser__3214EC2761621108");

            entity.ToTable("MicroserviceTags", "Microservices");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MicroserviceId).HasColumnName("MicroserviceID");
            entity.Property(e => e.TagId).HasColumnName("TagID");

            entity.HasOne(d => d.Microservice).WithMany(p => p.MicroserviceTags)
                .HasForeignKey(d => d.MicroserviceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Microserv__Micro__5E3FF0B0");

            entity.HasOne(d => d.Tag).WithMany(p => p.MicroserviceTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Microserv__TagID__5F3414E9");
        });

        modelBuilder.Entity<AllConfigurationStringsModel>(e => e.HasNoKey());

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC2720D7D6EB");

            entity.HasIndex(e => e.Name, "UQ__Provider__737584F60991B0F2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Color)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.TheoreticalMaxTps).HasColumnName("TheoreticalMaxTPS");

            entity.HasOne(d => d.SubchainOfNavigation).WithMany(p => p.InverseSubchainOfNavigation)
                .HasForeignKey(d => d.SubchainOf)
                .HasConstraintName("FK__Providers__Subch__4E53A1AA");
        });

        modelBuilder.Entity<ProviderConfigurationString>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC27D092728E");

            entity.ToTable("ProviderConfigurationStrings", "Configuration");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ConfigurationStringId).HasColumnName("ConfigurationStringID");
            entity.Property(e => e.EnvironmentId).HasColumnName("EnvironmentID");
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

            entity.HasOne(d => d.ConfigurationString).WithMany(p => p.ProviderConfigurationStrings)
                .HasForeignKey(d => d.ConfigurationStringId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderC__Confi__16EE5E27");

            entity.HasOne(d => d.Environment).WithMany(p => p.ProviderConfigurationStrings)
                .HasForeignKey(d => d.EnvironmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderC__Envir__17E28260");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderConfigurationStrings)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderC__Provi__15FA39EE");
        });
        modelBuilder.Entity<ProviderTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC273F7DF65E");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
            entity.Property(e => e.TagId).HasColumnName("TagID");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderTags)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderT__Provi__5A6F5FCC");

            entity.HasOne(d => d.Tag).WithMany(p => p.ProviderTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderT__TagID__5B638405");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
