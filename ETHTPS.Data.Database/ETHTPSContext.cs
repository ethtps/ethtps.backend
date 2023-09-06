using ETHTPS.Data.Integrations.MSSQL.RPC;

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
#pragma warning restore CS8618

    public virtual DbSet<Updater> RPCUpdaters { get; set; }

    public virtual DbSet<UpdaterConfiguration> RPCUpdaterConfigurations { get; set; }

    public virtual DbSet<Health> RPCHealth { get; set; }

    public virtual DbSet<Binding> RPCBindings { get; set; }

    public virtual DbSet<HealthStatus> RPCHealthStatuses { get; set; }
    public virtual DbSet<Endpoint> RPCEndpoints { get; set; }
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

    public virtual DbSet<Network> Networks { get; set; }

    public virtual DbSet<OldestLoggedHistoricalEntry> OldestLoggedHistoricalEntries { get; set; }

    public virtual DbSet<OldestLoggedTimeWarpBlock> OldestLoggedTimeWarpBlocks { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionsForRole> PermissionsForRoles { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<ProviderLink> ProviderLinks { get; set; }

    public virtual DbSet<ProviderType> ProviderTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schema> Schemas { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<Set> Sets { get; set; }

    public virtual DbSet<StarkwareTransactionCountDatum> StarkwareTransactionCountData { get; set; }

    public virtual DbSet<FeedbackTypes> FeedbackTypes { get; set; }

    public virtual DbSet<UserFeedback> UserFeedback { get; set; }

    public virtual DbSet<State> States { get; set; }
    public virtual DbSet<DataUpdater> DataUpdaters { get; set; }

    public virtual DbSet<DataUpdaterStatus> DataUpdaterStatuses { get; set; }

    public virtual DbSet<DataUpdaterType> DataUpdaterTypes { get; set; }
    public virtual DbSet<LiveDataUpdaterStatus> LiveDataUpdaterStatuses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AggregatedCounter>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("PK_HangFire_CounterAggregated");

            entity.ToTable("AggregatedCounter", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_AggregatedCounter_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<AggregatedEnpointStat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Aggregat__3214EC2779105DBD");

            entity.ToTable("AggregatedEnpointStats", "Statistics");

            entity.HasIndex(e => e.Path, "UQ__Aggregat__A15FA6CB989AC05C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Path).HasMaxLength(255);
        });

        modelBuilder.Entity<Apikey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APIKeys__3214EC273CF843BF");

            entity.ToTable("APIKeys", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.KeyHash).HasMaxLength(255);
            entity.Property(e => e.RequesterIpaddress)
                .HasMaxLength(255)
                .HasColumnName("RequesterIPAddress");
        });

        modelBuilder.Entity<ApikeyExperimentBinding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APIKeyEx__3214EC27CC570B3A");

            entity.ToTable("APIKeyExperimentBinding", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApikeyId).HasColumnName("APIKeyID");
            entity.Property(e => e.ExperimentId).HasColumnName("ExperimentID");

            entity.HasOne(d => d.Apikey).WithMany(p => p.ApikeyExperimentBindings)
                .HasForeignKey(d => d.ApikeyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APIKeyExp__APIKe__05C3D225");

            entity.HasOne(d => d.Experiment).WithMany(p => p.ApikeyExperimentBindings)
                .HasForeignKey(d => d.ExperimentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APIKeyExp__Exper__04CFADEC");
        });

        modelBuilder.Entity<ApikeyGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__APIKeyGr__3214EC270837A7C8");

            entity.ToTable("APIKeyGroups", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ApikeyId).HasColumnName("APIKeyID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");

            entity.HasOne(d => d.Apikey).WithMany(p => p.ApikeyGroups)
                .HasForeignKey(d => d.ApikeyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APIKeyGro__APIKe__7F16D496");

            entity.HasOne(d => d.Group).WithMany(p => p.ApikeyGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__APIKeyGro__Group__000AF8CF");
        });

        modelBuilder.Entity<Binding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bindings__3214EC276F70E733");

            entity.ToTable("Bindings", "RPC");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastError).HasMaxLength(255);

            entity.HasOne(d => d.EndpointNavigation).WithMany(p => p.Bindings)
                .HasForeignKey(d => d.Endpoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bindings__Endpoi__39CD8610");

            entity.HasOne(d => d.UpdaterNavigation).WithMany(p => p.Bindings)
                .HasForeignKey(d => d.Updater)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bindings__Update__3AC1AA49");
        });

        modelBuilder.Entity<Counter>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Id }).HasName("PK_HangFire_Counter");

            entity.ToTable("Counter", "HangFire");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<DataUpdater>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DataUpda__3214EC27813D11AA");

            entity.ToTable("DataUpdaters", "DataUpdaters");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Enabled)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Provider).WithMany(p => p.DataUpdaters)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DataUpdat__Provi__1BB31344");

            entity.HasOne(d => d.Type).WithMany(p => p.DataUpdaters)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DataUpdat__TypeI__1ABEEF0B");
        });

        modelBuilder.Entity<DataUpdaterStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DataUpda__3214EC270432F177");

            entity.ToTable("DataUpdaterStatuses", "DataUpdaters");

            entity.HasIndex(e => e.Name, "UQ__DataUpda__737584F686C5B42C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<DataUpdaterType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DataUpda__3214EC27056279FB");

            entity.ToTable("DataUpdaterTypes", "DataUpdaters");

            entity.HasIndex(e => e.TypeName, "UQ__DataUpda__D4E7DFA8FE05F0DA").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<Endpoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Endpoint__3214EC276EC9799F");

            entity.ToTable("Endpoints", "RPC");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.AuthType).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Enabled)
                .IsRequired()
                .HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Experiment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC278EC97317");

            entity.ToTable("Experiments", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

            entity.HasOne(d => d.Project).WithMany(p => p.Experiments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimen__Proje__670A40DB");

            entity.HasOne(d => d.RunParametersNavigation).WithMany(p => p.Experiments)
                .HasForeignKey(d => d.RunParameters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimen__RunPa__0A888742");

            entity.HasOne(d => d.TargetNavigation).WithMany(p => p.Experiments)
                .HasForeignKey(d => d.Target)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimen__Targe__0B7CAB7B");
        });

        modelBuilder.Entity<ExperimentFeedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC27416EA87C");

            entity.ToTable("ExperimentFeedback", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Text).HasMaxLength(255);

            entity.HasOne(d => d.ExperimentNavigation).WithMany(p => p.ExperimentFeedbacks)
                .HasForeignKey(d => d.Experiment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimen__Exper__07AC1A97");
        });

        modelBuilder.Entity<ExperimentResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC274B80E3D0");

            entity.ToTable("ExperimentResults", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SdpercentageReturnVisitors).HasColumnName("SDPercentageReturnVisitors");
            entity.Property(e => e.SdretentionSeconds).HasColumnName("SDRetentionSeconds");

            entity.HasOne(d => d.ExperimentNavigation).WithMany(p => p.ExperimentResults)
                .HasForeignKey(d => d.Experiment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimen__Exper__09946309");
        });

        modelBuilder.Entity<ExperimentRunParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC2770106B64");

            entity.ToTable("ExperimentRunParameters", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DisplayToNpeopleBeforeEnd).HasColumnName("DisplayToNPeopleBeforeEnd");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ExperimentTarget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC27E9621D6B");

            entity.ToTable("ExperimentTargets", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.ExperimentTargets)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experiment__Type__06B7F65E");
        });

        modelBuilder.Entity<ExperimentTargetType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC277841139F");

            entity.ToTable("ExperimentTargetTypes", "ABTesting");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TargetTypeName).HasMaxLength(255);
            entity.Property(e => e.TargetTypeValue).HasMaxLength(255);
        });

        modelBuilder.Entity<ExperimentalSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Experime__3214EC2761F71FE6");

            entity.ToTable("ExperimentalSessions", "ABTesting");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.TargetIpaddress)
                .HasMaxLength(255)
                .HasColumnName("TargetIPAddress");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.ExperimentalSession)
                .HasForeignKey<ExperimentalSession>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experimental__ID__08A03ED0");
        });

        modelBuilder.Entity<ExternalWebsite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__External__3214EC273580B394");

            entity.ToTable("ExternalWebsites", "Info");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IconBase64).IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.ExternalWebsites)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExternalW__Categ__0E591826");
        });

        modelBuilder.Entity<ExternalWebsiteCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__External__3214EC27028F4C4E");

            entity.ToTable("ExternalWebsiteCateopry", "Info");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Features__3214EC27BD73E8E2");

            entity.ToTable("Features", "ProjectManagement");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
        });

        modelBuilder.Entity<FeedbackTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC27A6B132D6");

            entity.ToTable("FeedbackTypes", "Feedback");

            entity.HasIndex(e => e.Name, "UQ__Feedback__737584F6DA6487B5").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Groups__3214EC270549E31F");

            entity.ToTable("Groups", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<GroupRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GroupRol__3214EC270E0EB6DF");

            entity.ToTable("GroupRoles", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupRoles)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupRole__Group__02E7657A");

            entity.HasOne(d => d.Role).WithMany(p => p.GroupRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupRole__RoleI__03DB89B3");
        });

        modelBuilder.Entity<Hash>(entity =>
        {
            entity.HasKey(e => new { e.Key, e.Field }).HasName("PK_HangFire_Hash");

            entity.ToTable("Hash", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Hash_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Field).HasMaxLength(100);
        });

        modelBuilder.Entity<Health>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Health__3214EC27411D5327");

            entity.ToTable("Health", "RPC");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ErrorDetails).HasMaxLength(255);
            entity.Property(e => e.LastCheck).HasColumnType("datetime");

            entity.HasOne(d => d.BindingNavigation).WithMany(p => p.Healths)
                .HasForeignKey(d => d.Binding)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Health__Binding__3BB5CE82");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Healths)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Health__Status__3CA9F2BB");
        });

        modelBuilder.Entity<HealthStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HealthSt__3214EC27CC89C25A");

            entity.ToTable("HealthStatuses", "RPC");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_HangFire_Job");

            entity.ToTable("Job", "HangFire");

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Job_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.HasIndex(e => e.StateName, "IX_HangFire_Job_StateName").HasFilter("([StateName] IS NOT NULL)");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
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

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_List_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<LiveDataUpdaterStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LiveData__3214EC272379C247");

            entity.ToTable("LiveDataUpdaterStatuses", "DataUpdaters");

            entity.HasIndex(e => e.UpdaterId, "UQ__LiveData__C23D31D34B0EEB86").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastRunTime).HasColumnType("datetime");
            entity.Property(e => e.LastSuccessfulRunTime).HasColumnType("datetime");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UpdaterId).HasColumnName("UpdaterID");

            entity.HasOne(d => d.Status).WithMany(p => p.LiveDataUpdaterStatuses)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LiveDataU__Statu__1D9B5BB6");

            entity.HasOne(d => d.Updater).WithOne(p => p.LiveDataUpdaterStatus)
                .HasForeignKey<LiveDataUpdaterStatus>(d => d.UpdaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LiveDataU__Updat__1CA7377D");
        });

        modelBuilder.Entity<MarkdownPage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Markdown__3214EC271CCA8E7E");

            entity.ToTable("MarkdownPages", "Info");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RawMarkdown).IsUnicode(false);
        });

        modelBuilder.Entity<Network>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Networks__3214EC27209F3397");

            entity.HasIndex(e => e.Name, "UQ__Networks__737584F68DD767EC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<OldestLoggedHistoricalEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OldestLo__3214EC270885419C");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.NetworkNavigation).WithMany(p => p.OldestLoggedHistoricalEntries)
                .HasForeignKey(d => d.Network)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OldestLog__Netwo__71BCD978");

            entity.HasOne(d => d.ProviderNavigation).WithMany(p => p.OldestLoggedHistoricalEntries)
                .HasForeignKey(d => d.Provider)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OldestLog__Provi__70C8B53F");
        });

        modelBuilder.Entity<OldestLoggedTimeWarpBlock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OldestLo__3214EC270F22E88F");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.NetworkNavigation).WithMany(p => p.OldestLoggedTimeWarpBlocks)
                .HasForeignKey(d => d.Network)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OldestLog__Netwo__7D2E8C24");

            entity.HasOne(d => d.ProviderNavigation).WithMany(p => p.OldestLoggedTimeWarpBlocks)
                .HasForeignKey(d => d.Provider)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OldestLog__Provi__7C3A67EB");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC2790E3370C");

            entity.ToTable("Permissions", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<PermissionsForRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC2709F1F4ED");

            entity.ToTable("PermissionsForRoles", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Permission).WithMany(p => p.PermissionsForRoles)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permissio__Permi__01F34141");

            entity.HasOne(d => d.Role).WithMany(p => p.PermissionsForRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permissio__RoleI__00FF1D08");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Projects__3214EC27CBA369D8");

            entity.ToTable("Projects", "ProjectManagement");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Website).HasMaxLength(255);

            entity.HasOne(d => d.ProviderNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Provider)
                .HasConstraintName("FK__Projects__Provid__0D64F3ED");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC272128D63A");

            entity.HasIndex(e => e.Name, "UQ__Provider__737584F61AA5F23A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Color)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.TheoreticalMaxTps).HasColumnName("TheoreticalMaxTPS");

            entity.HasOne(d => d.SubchainOfNavigation).WithMany(p => p.InverseSubchainOfNavigation)
                .HasForeignKey(d => d.SubchainOf)
                .HasConstraintName("FK__Providers__Subch__5EAA0504");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Providers)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Providers__Type__5DB5E0CB");
        });

        modelBuilder.Entity<ProviderDetailsMarkdownPage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC27AC4F3A92");

            entity.ToTable("ProviderDetailsMarkdownPages", "Info");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MarkdownPageId).HasColumnName("MarkdownPageID");
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

            entity.HasOne(d => d.MarkdownPage).WithMany(p => p.ProviderDetailsMarkdownPages)
                .HasForeignKey(d => d.MarkdownPageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderD__Markd__1229A90A");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderDetailsMarkdownPages)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderD__Provi__113584D1");
        });

        modelBuilder.Entity<ProviderLink>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC27CF6DC1F2");

            entity.ToTable("ProviderLinks", "Info");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ExternalWebsiteId).HasColumnName("ExternalWebsiteID");
            entity.Property(e => e.Link).HasMaxLength(255);
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

            entity.HasOne(d => d.ExternalWebsite).WithMany(p => p.ProviderLinks)
                .HasForeignKey(d => d.ExternalWebsiteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderL__Exter__10416098");

            entity.HasOne(d => d.Provider).WithMany(p => p.ProviderLinks)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProviderL__Provi__0F4D3C5F");
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

        modelBuilder.Entity<ProviderType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provider__3214EC27E3E25B89");

            entity.HasIndex(e => e.Name, "UQ__Provider__737584F6356FCCBE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Color)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC2785ED3E38");

            entity.ToTable("Roles", "Security");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
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

            entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Set_ExpireAt").HasFilter("([ExpireAt] IS NOT NULL)");

            entity.HasIndex(e => new { e.Key, e.Score }, "IX_HangFire_Set_Score");

            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Value).HasMaxLength(256);
            entity.Property(e => e.ExpireAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<StarkwareTransactionCountDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Starkwar__3214EC2737C90DDE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LastUpdateTime).HasColumnType("datetime");
            entity.Property(e => e.LastUpdateTps).HasColumnName("LastUpdateTPS");
            entity.Property(e => e.Product).HasMaxLength(255);

            entity.HasOne(d => d.NetworkNavigation).WithMany(p => p.StarkwareTransactionCountData)
                .HasForeignKey(d => d.Network)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Starkware__Netwo__7E22B05D");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => new { e.JobId, e.Id }).HasName("PK_HangFire_State");

            entity.ToTable("State", "HangFire");

            entity.HasIndex(e => e.CreatedAt, "IX_HangFire_State_CreatedAt");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Reason).HasMaxLength(100);

            entity.HasOne(d => d.Job).WithMany(p => p.States)
                .HasForeignKey(d => d.JobId)
                .HasConstraintName("FK_HangFire_State_Job");
        });

        modelBuilder.Entity<Updater>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Updaters__3214EC2763267566");

            entity.ToTable("Updaters", "RPC");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");

            entity.HasOne(d => d.NetworkNavigation).WithMany(p => p.Updaters)
                .HasForeignKey(d => d.Network)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Updaters__Networ__36F11965");

            entity.HasOne(d => d.ProviderNavigation).WithMany(p => p.Updaters)
                .HasForeignKey(d => d.Provider)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Updaters__Provid__37E53D9E");
        });

        modelBuilder.Entity<UpdaterConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UpdaterC__3214EC27AA1C983E");

            entity.ToTable("UpdaterConfiguration", "RPC");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AuthMethod).HasMaxLength(255);
            entity.Property(e => e.AuthMethodDetails).HasMaxLength(255);
            entity.Property(e => e.Enabled)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdateIntervalMs).HasDefaultValueSql("((5000))");

            entity.HasOne(d => d.UpdaterNavigation).WithMany(p => p.UpdaterConfigurations)
                .HasForeignKey(d => d.Updater)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UpdaterCo__Updat__38D961D7");
        });

        modelBuilder.Entity<UserFeedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserFeed__3214EC270350DF32");

            entity.ToTable("UserFeedback", "Feedback");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Details).HasMaxLength(255);
            entity.Property(e => e.ExtraData).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
