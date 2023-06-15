using Microsoft.EntityFrameworkCore;

namespace ETHTPS.Data.Integrations.MSSQL;

public partial class EthtpsContext : ETHTPSContextBase
{
#pragma warning disable CS8618
    public EthtpsContext()
    {
    }

    public EthtpsContext(DbContextOptions<EthtpsContext> options)
        : base(options)
    {
    }
#pragma warning  restore CS8618

    public virtual DbSet<AggregatedCounter> AggregatedCounters { get; set; }

    public virtual DbSet<Apikey> Apikeys { get; set; }

    public virtual DbSet<ApikeyExperimentBinding> ApikeyExperimentBindings { get; set; }

    public virtual DbSet<ApikeyGroup> ApikeyGroups { get; set; }

    public virtual DbSet<AppConfigurationValue> AppConfigurationValues { get; set; }

    public virtual DbSet<CachedResponse> CachedResponses { get; set; }

    public virtual DbSet<Counter> Counters { get; set; }

    public virtual DbSet<DetailedAccessStat> DetailedAccessStats { get; set; }
    public virtual DbSet<AggregatedEnpointStat> AggregatedEnpointStats { get; set; }

    public virtual DbSet<Experiment> Experiments { get; set; }

    public virtual DbSet<ExperimentFeedback> ExperimentFeedbacks { get; set; }

    public virtual DbSet<ExperimentResult> ExperimentResults { get; set; }

    public virtual DbSet<ExperimentRunParameter> ExperimentRunParameters { get; set; }

    public virtual DbSet<ExperimentTarget> ExperimentTargets { get; set; }

    public virtual DbSet<ExperimentTargetType> ExperimentTargetTypes { get; set; }

    public virtual DbSet<ExperimentalSession> ExperimentalSessions { get; set; }

    public virtual DbSet<ExternalWebsite> ExternalWebsites { get; set; }

    public virtual DbSet<ExternalWebsiteCategory> ExternalWebsiteCateopries { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupRole> GroupRoles { get; set; }

    public virtual DbSet<Hash> Hashes { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobParameter> JobParameters { get; set; }

    public virtual DbSet<JobQueue> JobQueues { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<MarkdownPage> MarkdownPages { get; set; }

    public virtual DbSet<Network> Networks { get; set; }

    public virtual DbSet<OldestLoggedHistoricalEntry> OldestLoggedHistoricalEntries { get; set; }

    public virtual DbSet<OldestLoggedTimeWarpBlock> OldestLoggedTimeWarpBlocks { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionsForRole> PermissionsForRoles { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<ProviderDetailsMarkdownPage> ProviderDetailsMarkdownPages { get; set; }

    public virtual DbSet<ProviderLink> ProviderLinks { get; set; }

    public virtual DbSet<ProviderType> ProviderTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schema> Schemas { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<StarkwareTransactionCountDatum> StarkwareTransactionCountData { get; set; }

    public virtual DbSet<State> States { get; set; }
    public virtual DbSet<DataUpdater> DataUpdaters { get; set; }

    public virtual DbSet<DataUpdaterStatus> DataUpdaterStatuses { get; set; }

    public virtual DbSet<DataUpdaterType> DataUpdaterTypes { get; set; }
    public virtual DbSet<LiveDataUpdaterStatus> LiveDataUpdaterStatuses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<DataUpdater>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DataUpda__3214EC2739E457A8");

            entity.ToTable("DataUpdaters", "DataUpdaters");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.HasOne(d => d.Provider).WithMany(p => p.DataUpdaters)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DataUpdat__Provi__5D60DB10");

            entity.HasOne(d => d.Type).WithMany(p => p.DataUpdaters)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DataUpdat__TypeI__5C6CB6D7");
        });

        modelBuilder.Entity<DataUpdaterStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DataUpda__3214EC27FA247EA4");

            entity.ToTable("DataUpdaterStatuses", "DataUpdaters");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<LiveDataUpdaterStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LiveData__3214EC27242E551C");

            entity.ToTable("LiveDataUpdaterStatuses", "DataUpdaters");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(x => x.Enabled).HasColumnName("Enabled");
            entity.Property(e => e.LastSuccessfulRunTime).HasColumnType("datetime");
            entity.Property(e => e.LastRunTime).HasColumnType("datetime");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UpdaterId).HasColumnName("UpdaterID");

            entity.HasOne(d => d.Status).WithMany(p => p.LiveDataUpdaterStatuses)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LiveDataU__Statu__5F492382");

            entity.HasOne(d => d.Updater).WithMany(p => p.LiveDataUpdaterStatuses)
                .HasForeignKey(d => d.UpdaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LiveDataU__Updat__5E54FF49");
        });

        modelBuilder.Entity<DataUpdaterType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DataUpda__3214EC27C05DB102");

            entity.ToTable("DataUpdaterTypes", "DataUpdaters");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<AggregatedCounter>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PK_HangFire_CounterAggregated");

            entity.ToTable("AggregatedCounter", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_AggregatedCounter_ExpireAt");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Apikey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APIKeys__3214EC275054A916");

            entity.ToTable("APIKeys", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.KeyHash)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.RequesterIpaddress)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("RequesterIPAddress");
        });

        modelBuilder.Entity<ApikeyExperimentBinding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APIKeyEx__3214EC276D75AB55");

            entity.ToTable("APIKeyExperimentBinding", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApikeyId).HasColumnName("APIKeyID");
            entity.Property(e => e.ExperimentId).HasColumnName("ExperimentID");

            entity.HasOne(d => d.Apikey).WithMany(p => p.ApikeyExperimentBindings)
                .HasForeignKey(d => d.ApikeyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APIKeyExp__APIKe__0A688BB1");

            entity.HasOne(d => d.Experiment).WithMany(p => p.ApikeyExperimentBindings)
                .HasForeignKey(d => d.ExperimentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APIKeyExp__Exper__09746778");
        });

        modelBuilder.Entity<ApikeyGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APIKeyGr__3214EC2740A83882");

            entity.ToTable("APIKeyGroups", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApikeyId).HasColumnName("APIKeyID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.HasOne(d => d.Apikey).WithMany(p => p.ApikeyGroups)
                .HasForeignKey(d => d.ApikeyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APIKeyGro__APIKe__2AD55B43");

            entity.HasOne(d => d.Group).WithMany(p => p.ApikeyGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APIKeyGro__Group__2BC97F7C");
        });

        modelBuilder.Entity<AppConfigurationValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AppConfi__3214EC273CB2D48A");

            entity.ToTable("AppConfigurationValues", "Configuration");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<CachedResponse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CachedRe__3214EC27DAB81A21");

            entity.HasIndex(e => e.Name, "UQ__CachedRe__737584F69ACC9E33").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.KeyJson)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("KeyJSON");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ValueJson)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("ValueJSON");
        });

        modelBuilder.Entity<Counter>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_Counter");

            entity.ToTable("Counter", "HangFire");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<DetailedAccessStat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Detailed__3214EC270F45AD2B");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Ipaddress)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("IPAddress");
            entity.Property(e => e.Path)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Experiment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC279A250572");

            entity.ToTable("Experiments", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.Project).WithMany(p => p.Experiments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimen__Proje__03BB8E22");

            entity.HasOne(d => d.RunParametersNavigation).WithMany(p => p.Experiments)
                .HasForeignKey(d => d.RunParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimen__RunPa__01D345B0");

            entity.HasOne(d => d.TargetNavigation).WithMany(p => p.Experiments)
                .HasForeignKey(d => d.Target)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimen__Targe__02C769E9");
        });

        modelBuilder.Entity<ExperimentFeedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC27BF1DE34C");

            entity.ToTable("ExperimentFeedback", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Text).HasMaxLength(255);

            entity.HasOne(d => d.ExperimentNavigation).WithMany(p => p.ExperimentFeedbacks)
                .HasForeignKey(d => d.Experiment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimen__Exper__0D44F85C");
        });

        modelBuilder.Entity<ExperimentResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC270ABFB690");

            entity.ToTable("ExperimentResults", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SdpercentageReturnVisitors).HasColumnName("SDPercentageReturnVisitors");
            entity.Property(e => e.SdretentionSeconds).HasColumnName("SDRetentionSeconds");

            entity.HasOne(d => d.ExperimentNavigation).WithMany(p => p.ExperimentResults)
                .HasForeignKey(d => d.Experiment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimen__Exper__00DF2177");
        });

        modelBuilder.Entity<ExperimentRunParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC27F62F241F");

            entity.ToTable("ExperimentRunParameters", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DisplayToNpeopleBeforeEnd).HasColumnName("DisplayToNPeopleBeforeEnd");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ExperimentTarget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC27EC9FFF6B");

            entity.ToTable("ExperimentTargets", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.ExperimentTargets)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experiment__Type__7EF6D905");
        });

        modelBuilder.Entity<ExperimentTargetType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC27476B2C8F");

            entity.ToTable("ExperimentTargetTypes", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TargetTypeName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.TargetTypeValue)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<ExperimentalSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC27C40FB4B2");

            entity.ToTable("ExperimentalSessions", "ABTesting");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.TargetIpaddress)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("TargetIPAddress");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.ExperimentalSession)
                .HasForeignKey<ExperimentalSession>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimental__ID__7FEAFD3E");
        });

        modelBuilder.Entity<ExternalWebsite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__External__3214EC271E3E2D19");

            entity.ToTable("ExternalWebsites", "Info");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IconBase64)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.ExternalWebsites)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExternalW__Categ__2EA5EC27");
        });

        modelBuilder.Entity<ExternalWebsiteCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__External__3214EC27BF25D844");

            entity.ToTable("ExternalWebsiteCateopry", "Info");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Features__3214EC27FF1D60EB");

            entity.ToTable("Features", "ProjectManagement");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.Project).WithMany(p => p.Features)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Features__Projec__6BE40491");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Groups__3214EC274366C4EA");

            entity.ToTable("Groups", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<GroupRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GroupRol__3214EC27A868EA7C");

            entity.ToTable("GroupRoles", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupRoles)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupRole__Group__2334397B");

            entity.HasOne(d => d.Role).WithMany(p => p.GroupRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupRole__RoleI__24285DB4");
        });

        modelBuilder.Entity<Hash>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Field }).HasName("PK_HangFire_Hash");

            entity.ToTable("Hash", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Hash_ExpireAt");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Field).HasMaxLength(100);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HangFire_Job");

            entity.ToTable("Job", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Job_ExpireAt");

            entity.HasIndex(e => e.StateName, "IX_HangFire_Job_StateName");

            entity.Property(e => e.Arguments).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            entity.Property(e => e.InvocationData).IsRequired();
            entity.Property(e => e.StateName).HasMaxLength(20);
        });

        modelBuilder.Entity<JobParameter>(entity =>
        {
            entity.HasKey(e => new { e.JobId, e.Name }).HasName("PK_HangFire_JobParameter");

            entity.ToTable("JobParameter", "HangFire");

            entity.Property(e => e.Name).HasMaxLength(40);

            entity.HasOne(d => d.Job).WithMany(p => p.JobParameters)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_HangFire_JobParameter_Job");
        });

        modelBuilder.Entity<JobQueue>(entity =>
        {
            entity.HasKey(e => new { e.Queue, e.Id }).HasName("PK_HangFire_JobQueue");

            entity.ToTable("JobQueue", "HangFire");

            entity.Property(e => e.Queue).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FetchedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_List");

            entity.ToTable("List", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_List_ExpireAt");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<MarkdownPage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Markdown__3214EC2759E148E2");

            entity.ToTable("MarkdownPages", "Info");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RawMarkdown)
                .IsRequired()
                .IsUnicode(false);
        });

        modelBuilder.Entity<Network>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Networks__3214EC27444EB224");

            entity.HasIndex(e => e.Name, "UQ__Networks__737584F6490B5C53").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<OldestLoggedHistoricalEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OldestLo__3214EC2720C78346");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.NetworkNavigation).WithMany(p => p.OldestLoggedHistoricalEntries)
                .HasForeignKey(d => d.Network)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OldestLog__Netwo__5D95E53A");

            entity.HasOne(d => d.ProviderNavigation).WithMany(p => p.OldestLoggedHistoricalEntries)
                .HasForeignKey(d => d.Provider)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OldestLog__Provi__5CA1C101");
        });

        modelBuilder.Entity<OldestLoggedTimeWarpBlock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OldestLo__3214EC27D48D7158");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.NetworkNavigation).WithMany(p => p.OldestLoggedTimeWarpBlocks)
                .HasForeignKey(d => d.Network)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OldestLog__Netwo__690797E6");

            entity.HasOne(d => d.ProviderNavigation).WithMany(p => p.OldestLoggedTimeWarpBlocks)
                .HasForeignKey(d => d.Provider)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OldestLog__Provi__681373AD");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC2739068DB8");

            entity.ToTable("Permissions", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<PermissionsForRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC27BE57EBDC");

            entity.ToTable("PermissionsForRoles", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Permission).WithMany(p => p.PermissionsForRoles)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permissio__Permi__22401542");

            entity.HasOne(d => d.Role).WithMany(p => p.PermissionsForRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permissio__RoleI__214BF109");
        });

        modelBuilder.Entity<AggregatedEnpointStat>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("AggregatedEnpointStats", "Statistics");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Path).IsRequired().HasMaxLength(255);

            entity.Property(e => e.AverageRequestTimeMs)
            .IsRequired()
            .HasDefaultValue(0);
            entity.Property(e => e.RequestCount)
           .IsRequired()
           .HasDefaultValue(0);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Projects__3214EC270C3347BB");

            entity.ToTable("Projects", "ProjectManagement");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Website)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.ProviderNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Provider)
                .HasConstraintName("FK__Projects__Provid__6AEFE058");
        });
        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ__Provider__737584F60991B0F2")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.Color)
                .HasMaxLength(16)
                .IsUnicode(false);

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.Property(e => e.TheoreticalMaxTps).HasColumnName("TheoreticalMaxTPS");

            entity.HasOne(d => d.SubchainOfNavigation)
                .WithMany(p => p.InverseSubchainOfNavigation)
                .HasForeignKey(d => d.SubchainOf)
                .HasConstraintName("FK__Providers__Subch__4E53A1AA");

            entity.HasOne(d => d.TypeNavigation)
                .WithMany(p => p.Providers)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Providers__Type__4D5F7D71");
        });

        modelBuilder.Entity<ProviderDetailsMarkdownPage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC275B25AB19");

            entity.ToTable("ProviderDetailsMarkdownPages", "Info");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MarkdownPageId).HasColumnName("MarkdownPageID");
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

            entity.HasOne(d => d.MarkdownPage).WithMany(p => p.ProviderDetailsMarkdownPages)
                .HasForeignKey(d => d.MarkdownPageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderD__Markd__27F8EE98");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderDetailsMarkdownPages)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderD__Provi__2704CA5F");
        });

        modelBuilder.Entity<ProviderLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC2748D7D0A4");

            entity.ToTable("ProviderLinks", "Info");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ExternalWebsiteId).HasColumnName("ExternalWebsiteID");
            entity.Property(e => e.Link)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

            entity.HasOne(d => d.ExternalWebsite).WithMany(p => p.ProviderLinks)
                .HasForeignKey(d => d.ExternalWebsiteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderL__Exter__2610A626");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderLinks)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderL__Provi__251C81ED");
        });

        modelBuilder.Entity<ProviderType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC27AD4DB9BD");

            entity.HasIndex(e => e.Name, "UQ__Provider__737584F6D29891F3").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC27E1CED80E");

            entity.ToTable("Roles", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Schema>(entity =>
        {
            entity.HasKey(e => e.Version).HasName("PK_HangFire_Schema");

            entity.ToTable("Schema", "HangFire");

            entity.Property(e => e.Version).ValueGeneratedNever();
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HangFire_Server");

            entity.ToTable("Server", "HangFire");

            entity.HasIndex(e => e.LastHeartbeat, "IX_HangFire_Server_LastHeartbeat");

            entity.Property(e => e.Id).HasMaxLength(200);
            entity.Property(e => e.LastHeartbeat).HasColumnType("datetime");
        });

        modelBuilder.Entity<Set>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Value }).HasName("PK_HangFire_Set");

            entity.ToTable("Set", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Set_ExpireAt");

            entity.HasIndex(e => new { e.Key, e.Score }, "IX_HangFire_Set_Score");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Value).HasMaxLength(256);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<StarkwareTransactionCountDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Starkwar__3214EC277EE03B03");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastUpdateTime).HasColumnType("datetime");
            entity.Property(e => e.LastUpdateTps).HasColumnName("LastUpdateTPS");
            entity.Property(e => e.Product)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.NetworkNavigation).WithMany(p => p.StarkwareTransactionCountData)
                .HasForeignKey(d => d.Network)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Starkware__Netwo__69FBBC1F");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => new { e.JobId, e.Id }).HasName("PK_HangFire_State");

            entity.ToTable("State", "HangFire");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Reason).HasMaxLength(100);

            entity.HasOne(d => d.Job).WithMany(p => p.States)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_HangFire_State_Job");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
