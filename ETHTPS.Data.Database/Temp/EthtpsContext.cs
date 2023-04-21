﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ETHTPS.Data.Integrations.MSSQL.Temp
{
    public partial class ETHTPSContext : DbContext
    {
        public ETHTPSContext()
        {
        }

        public ETHTPSContext(DbContextOptions<ETHTPSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AggregatedCounter> AggregatedCounters { get; set; } = null!;
        public virtual DbSet<AggregatedEnpointStat> AggregatedEnpointStats { get; set; } = null!;
        public virtual DbSet<Apikey> Apikeys { get; set; } = null!;
        public virtual DbSet<ApikeyExperimentBinding> ApikeyExperimentBindings { get; set; } = null!;
        public virtual DbSet<ApikeyGroup> ApikeyGroups { get; set; } = null!;
        public virtual DbSet<CachedResponse> CachedResponses { get; set; } = null!;
        public virtual DbSet<ConfigurationString> ConfigurationStrings { get; set; } = null!;
        public virtual DbSet<Counter> Counters { get; set; } = null!;
        public virtual DbSet<DataUpdater> DataUpdaters { get; set; } = null!;
        public virtual DbSet<DataUpdaterStatus> DataUpdaterStatuses { get; set; } = null!;
        public virtual DbSet<DataUpdaterType> DataUpdaterTypes { get; set; } = null!;
        public virtual DbSet<DetailedAccessStat> DetailedAccessStats { get; set; } = null!;
        public virtual DbSet<Environment> Environments { get; set; } = null!;
        public virtual DbSet<Experiment> Experiments { get; set; } = null!;
        public virtual DbSet<ExperimentFeedback> ExperimentFeedbacks { get; set; } = null!;
        public virtual DbSet<ExperimentResult> ExperimentResults { get; set; } = null!;
        public virtual DbSet<ExperimentRunParameter> ExperimentRunParameters { get; set; } = null!;
        public virtual DbSet<ExperimentTarget> ExperimentTargets { get; set; } = null!;
        public virtual DbSet<ExperimentTargetType> ExperimentTargetTypes { get; set; } = null!;
        public virtual DbSet<ExperimentalSession> ExperimentalSessions { get; set; } = null!;
        public virtual DbSet<ExternalWebsite> ExternalWebsites { get; set; } = null!;
        public virtual DbSet<ExternalWebsiteCateopry> ExternalWebsiteCateopries { get; set; } = null!;
        public virtual DbSet<Feature> Features { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<GroupRole> GroupRoles { get; set; } = null!;
        public virtual DbSet<Hash> Hashes { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<JobParameter> JobParameters { get; set; } = null!;
        public virtual DbSet<JobQueue> JobQueues { get; set; } = null!;
        public virtual DbSet<List> Lists { get; set; } = null!;
        public virtual DbSet<LiveDataUpdaterStatus> LiveDataUpdaterStatuses { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<MarkdownPage> MarkdownPages { get; set; } = null!;
        public virtual DbSet<Microservice> Microservices { get; set; } = null!;
        public virtual DbSet<MicroserviceConfigurationString> MicroserviceConfigurationStrings { get; set; } = null!;
        public virtual DbSet<Network> Networks { get; set; } = null!;
        public virtual DbSet<OldestLoggedHistoricalEntry> OldestLoggedHistoricalEntries { get; set; } = null!;
        public virtual DbSet<OldestLoggedTimeWarpBlock> OldestLoggedTimeWarpBlocks { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<PermissionsForRole> PermissionsForRoles { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Provider> Providers { get; set; } = null!;
        public virtual DbSet<ProviderConfigurationString> ProviderConfigurationStrings { get; set; } = null!;
        public virtual DbSet<ProviderDetailsMarkdownPage> ProviderDetailsMarkdownPages { get; set; } = null!;
        public virtual DbSet<ProviderLink> ProviderLinks { get; set; } = null!;
        public virtual DbSet<ProviderType> ProviderTypes { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Schema> Schemas { get; set; } = null!;
        public virtual DbSet<Server> Servers { get; set; } = null!;
        public virtual DbSet<Set> Sets { get; set; } = null!;
        public virtual DbSet<StarkwareTransactionCountDatum> StarkwareTransactionCountData { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;
        public virtual DbSet<TimeWarpDataDay> TimeWarpDataDays { get; set; } = null!;
        public virtual DbSet<TimeWarpDataHour> TimeWarpDataHours { get; set; } = null!;
        public virtual DbSet<TimeWarpDataMinute> TimeWarpDataMinutes { get; set; } = null!;
        public virtual DbSet<TimeWarpDataWeek> TimeWarpDataWeeks { get; set; } = null!;
        public virtual DbSet<TimeWarpDatum> TimeWarpData { get; set; } = null!;
        public virtual DbSet<TpsandGasDataAll> TpsandGasDataAlls { get; set; } = null!;
        public virtual DbSet<TpsandGasDataDay> TpsandGasDataDays { get; set; } = null!;
        public virtual DbSet<TpsandGasDataHour> TpsandGasDataHours { get; set; } = null!;
        public virtual DbSet<TpsandGasDataLatest> TpsandGasDataLatests { get; set; } = null!;
        public virtual DbSet<TpsandGasDataMax> TpsandGasDataMaxes { get; set; } = null!;
        public virtual DbSet<TpsandGasDataMinute> TpsandGasDataMinutes { get; set; } = null!;
        public virtual DbSet<TpsandGasDataMonth> TpsandGasDataMonths { get; set; } = null!;
        public virtual DbSet<TpsandGasDataWeek> TpsandGasDataWeeks { get; set; } = null!;
        public virtual DbSet<TpsandGasDataYear> TpsandGasDataYears { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=dbos.dhcp.rb2011i.cce.co;Initial Catalog=ETHTPS;User Id=ethtps_prod;Password=EB4s8ESTQxbnDPUcaMHP5m7yVRucR8j86kufFvmLuVb8gdsjAS6xBXJL5T73AEQy;Trusted_Connection=True;Integrated Security=False;MultipleActiveResultSets=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AggregatedCounter>(entity =>
            {
                entity.HasKey(e => e.Key)
                    .HasName("PK_HangFire_CounterAggregated");

                entity.ToTable("AggregatedCounter", "HangFire");

                entity.HasIndex(e => e.ExpireAt, "IX_HangFire_AggregatedCounter_ExpireAt");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<AggregatedEnpointStat>(entity =>
            {
                entity.ToTable("AggregatedEnpointStats", "Statistics");

                entity.HasIndex(e => e.Path, "UQ__Aggregat__A15FA6CB9DE75FA6")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Path).HasMaxLength(255);
            });

            modelBuilder.Entity<Apikey>(entity =>
            {
                entity.ToTable("APIKeys", "Security");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.KeyHash).HasMaxLength(255);

                entity.Property(e => e.RequesterIpaddress)
                    .HasMaxLength(255)
                    .HasColumnName("RequesterIPAddress");
            });

            modelBuilder.Entity<ApikeyExperimentBinding>(entity =>
            {
                entity.ToTable("APIKeyExperimentBinding", "ABTesting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApikeyId).HasColumnName("APIKeyID");

                entity.Property(e => e.ExperimentId).HasColumnName("ExperimentID");

                entity.HasOne(d => d.Apikey)
                    .WithMany(p => p.ApikeyExperimentBindings)
                    .HasForeignKey(d => d.ApikeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__APIKeyExp__APIKe__0A688BB1");

                entity.HasOne(d => d.Experiment)
                    .WithMany(p => p.ApikeyExperimentBindings)
                    .HasForeignKey(d => d.ExperimentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__APIKeyExp__Exper__09746778");
            });

            modelBuilder.Entity<ApikeyGroup>(entity =>
            {
                entity.ToTable("APIKeyGroups", "Security");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApikeyId).HasColumnName("APIKeyID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Apikey)
                    .WithMany(p => p.ApikeyGroups)
                    .HasForeignKey(d => d.ApikeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__APIKeyGro__APIKe__2AD55B43");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.ApikeyGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__APIKeyGro__Group__2BC97F7C");
            });

            modelBuilder.Entity<CachedResponse>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__CachedRe__737584F69ACC9E33")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.KeyJson)
                    .IsUnicode(false)
                    .HasColumnName("KeyJSON");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ValueJson)
                    .IsUnicode(false)
                    .HasColumnName("ValueJSON");
            });

            modelBuilder.Entity<ConfigurationString>(entity =>
            {
                entity.ToTable("ConfigurationStrings", "Configuration");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Value).IsUnicode(false);
            });

            modelBuilder.Entity<Counter>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Id })
                    .HasName("PK_HangFire_Counter");

                entity.ToTable("Counter", "HangFire");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<DataUpdater>(entity =>
            {
                entity.ToTable("DataUpdaters", "DataUpdaters");

                entity.HasIndex(e => new { e.TypeId, e.ProviderId }, "UNIQUE_TID_PID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.DataUpdaters)
                    .HasForeignKey(d => d.ProviderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DataUpdat__Provi__5D60DB10");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.DataUpdaters)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DataUpdat__TypeI__5C6CB6D7");
            });

            modelBuilder.Entity<DataUpdaterStatus>(entity =>
            {
                entity.ToTable("DataUpdaterStatuses", "DataUpdaters");

                entity.HasIndex(e => e.Name, "UQ__DataUpda__737584F68C26E905")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<DataUpdaterType>(entity =>
            {
                entity.ToTable("DataUpdaterTypes", "DataUpdaters");

                entity.HasIndex(e => e.TypeName, "UQ__DataUpda__D4E7DFA88563416A")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TypeName).HasMaxLength(255);
            });

            modelBuilder.Entity<DetailedAccessStat>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Ipaddress)
                    .HasMaxLength(255)
                    .HasColumnName("IPAddress");

                entity.Property(e => e.Path).HasMaxLength(255);
            });

            modelBuilder.Entity<Environment>(entity =>
            {
                entity.ToTable("Environments", "Configuration");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Experiment>(entity =>
            {
                entity.ToTable("Experiments", "ABTesting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Experiments)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Experimen__Proje__03BB8E22");

                entity.HasOne(d => d.RunParametersNavigation)
                    .WithMany(p => p.Experiments)
                    .HasForeignKey(d => d.RunParameters)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Experimen__RunPa__01D345B0");

                entity.HasOne(d => d.TargetNavigation)
                    .WithMany(p => p.Experiments)
                    .HasForeignKey(d => d.Target)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Experimen__Targe__02C769E9");
            });

            modelBuilder.Entity<ExperimentFeedback>(entity =>
            {
                entity.ToTable("ExperimentFeedback", "ABTesting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Text).HasMaxLength(255);

                entity.HasOne(d => d.ExperimentNavigation)
                    .WithMany(p => p.ExperimentFeedbacks)
                    .HasForeignKey(d => d.Experiment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Experimen__Exper__0D44F85C");
            });

            modelBuilder.Entity<ExperimentResult>(entity =>
            {
                entity.ToTable("ExperimentResults", "ABTesting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SdpercentageReturnVisitors).HasColumnName("SDPercentageReturnVisitors");

                entity.Property(e => e.SdretentionSeconds).HasColumnName("SDRetentionSeconds");

                entity.HasOne(d => d.ExperimentNavigation)
                    .WithMany(p => p.ExperimentResults)
                    .HasForeignKey(d => d.Experiment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Experimen__Exper__00DF2177");
            });

            modelBuilder.Entity<ExperimentRunParameter>(entity =>
            {
                entity.ToTable("ExperimentRunParameters", "ABTesting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DisplayToNpeopleBeforeEnd).HasColumnName("DisplayToNPeopleBeforeEnd");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ExperimentTarget>(entity =>
            {
                entity.ToTable("ExperimentTargets", "ABTesting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.ExperimentTargets)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Experiment__Type__7EF6D905");
            });

            modelBuilder.Entity<ExperimentTargetType>(entity =>
            {
                entity.ToTable("ExperimentTargetTypes", "ABTesting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TargetTypeName).HasMaxLength(255);

                entity.Property(e => e.TargetTypeValue).HasMaxLength(255);
            });

            modelBuilder.Entity<ExperimentalSession>(entity =>
            {
                entity.ToTable("ExperimentalSessions", "ABTesting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TargetIpaddress)
                    .HasMaxLength(255)
                    .HasColumnName("TargetIPAddress");
            });

            modelBuilder.Entity<ExternalWebsite>(entity =>
            {
                entity.ToTable("ExternalWebsites", "Info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IconBase64).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.ExternalWebsites)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ExternalW__Categ__2EA5EC27");
            });

            modelBuilder.Entity<ExternalWebsiteCateopry>(entity =>
            {
                entity.ToTable("ExternalWebsiteCateopry", "Info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.ToTable("Features", "ProjectManagement");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Details).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Features)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Features__Projec__6BE40491");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Groups", "Security");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<GroupRole>(entity =>
            {
                entity.ToTable("GroupRoles", "Security");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupRoles)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupRole__Group__2334397B");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.GroupRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupRole__RoleI__24285DB4");
            });

            modelBuilder.Entity<Hash>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Field })
                    .HasName("PK_HangFire_Hash");

                entity.ToTable("Hash", "HangFire");

                entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Hash_ExpireAt");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.Field).HasMaxLength(100);
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job", "HangFire");

                entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Job_ExpireAt");

                entity.HasIndex(e => e.StateName, "IX_HangFire_Job_StateName");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.StateName).HasMaxLength(20);
            });

            modelBuilder.Entity<JobParameter>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.Name })
                    .HasName("PK_HangFire_JobParameter");

                entity.ToTable("JobParameter", "HangFire");

                entity.Property(e => e.Name).HasMaxLength(40);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobParameters)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_HangFire_JobParameter_Job");
            });

            modelBuilder.Entity<JobQueue>(entity =>
            {
                entity.HasKey(e => new { e.Queue, e.Id })
                    .HasName("PK_HangFire_JobQueue");

                entity.ToTable("JobQueue", "HangFire");

                entity.Property(e => e.Queue).HasMaxLength(50);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.FetchedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<List>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Id })
                    .HasName("PK_HangFire_List");

                entity.ToTable("List", "HangFire");

                entity.HasIndex(e => e.ExpireAt, "IX_HangFire_List_ExpireAt");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<LiveDataUpdaterStatus>(entity =>
            {
                entity.ToTable("LiveDataUpdaterStatuses", "DataUpdaters");

                entity.HasIndex(e => e.UpdaterId, "UQ_UID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastRunTime).HasColumnType("datetime");

                entity.Property(e => e.LastSuccessfulRunTime).HasColumnType("datetime");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UpdaterId).HasColumnName("UpdaterID");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.LiveDataUpdaterStatuses)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LiveDataU__Statu__5F492382");

                entity.HasOne(d => d.Updater)
                    .WithOne(p => p.LiveDataUpdaterStatus)
                    .HasForeignKey<LiveDataUpdaterStatus>(d => d.UpdaterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LiveDataU__Updat__5E54FF49");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Logs", "Logging");

                entity.Property(e => e.Level).HasMaxLength(50);

                entity.Property(e => e.Logged).HasColumnType("datetime");

                entity.Property(e => e.Logger).HasMaxLength(250);

                entity.Property(e => e.MachineName).HasMaxLength(50);
            });

            modelBuilder.Entity<MarkdownPage>(entity =>
            {
                entity.ToTable("MarkdownPages", "Info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RawMarkdown).IsUnicode(false);
            });

            modelBuilder.Entity<Microservice>(entity =>
            {
                entity.ToTable("Microservices", "Microservices");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<MicroserviceConfigurationString>(entity =>
            {
                entity.ToTable("MicroserviceConfigurationStrings", "Configuration");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConfigurationStringId).HasColumnName("ConfigurationStringID");

                entity.Property(e => e.EnvironmentId).HasColumnName("EnvironmentID");

                entity.Property(e => e.MicroserviceId).HasColumnName("MicroserviceID");

                entity.HasOne(d => d.ConfigurationString)
                    .WithMany(p => p.MicroserviceConfigurationStrings)
                    .HasForeignKey(d => d.ConfigurationStringId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Microserv__Confi__4959E263");

                entity.HasOne(d => d.Environment)
                    .WithMany(p => p.MicroserviceConfigurationStrings)
                    .HasForeignKey(d => d.EnvironmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Microserv__Envir__4A4E069C");

                entity.HasOne(d => d.Microservice)
                    .WithMany(p => p.MicroserviceConfigurationStrings)
                    .HasForeignKey(d => d.MicroserviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Microserv__Micro__4865BE2A");
            });

            modelBuilder.Entity<Network>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Networks__737584F6490B5C53")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<OldestLoggedHistoricalEntry>(entity =>
            {
                entity.HasIndex(e => new { e.Network, e.Provider }, "UNIQUE_NID_PID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OldestBlockDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.OldestLoggedHistoricalEntries)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OldestLog__Netwo__5D95E53A");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.OldestLoggedHistoricalEntries)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OldestLog__Provi__5CA1C101");
            });

            modelBuilder.Entity<OldestLoggedTimeWarpBlock>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.OldestLoggedTimeWarpBlocks)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OldestLog__Netwo__690797E6");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.OldestLoggedTimeWarpBlocks)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OldestLog__Provi__681373AD");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions", "Security");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<PermissionsForRole>(entity =>
            {
                entity.ToTable("PermissionsForRoles", "Security");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.PermissionsForRoles)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Permissio__Permi__22401542");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PermissionsForRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Permissio__RoleI__214BF109");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects", "ProjectManagement");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Details).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Website).HasMaxLength(255);

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.Projects)
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

            modelBuilder.Entity<ProviderConfigurationString>(entity =>
            {
                entity.ToTable("ProviderConfigurationStrings", "Configuration");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ConfigurationStringId).HasColumnName("ConfigurationStringID");

                entity.Property(e => e.EnvironmentId).HasColumnName("EnvironmentID");

                entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

                entity.HasOne(d => d.ConfigurationString)
                    .WithMany(p => p.ProviderConfigurationStrings)
                    .HasForeignKey(d => d.ConfigurationStringId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProviderC__Confi__3A179ED3");

                entity.HasOne(d => d.Environment)
                    .WithMany(p => p.ProviderConfigurationStrings)
                    .HasForeignKey(d => d.EnvironmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProviderC__Envir__3B0BC30C");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.ProviderConfigurationStrings)
                    .HasForeignKey(d => d.ProviderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProviderC__Provi__39237A9A");
            });

            modelBuilder.Entity<ProviderDetailsMarkdownPage>(entity =>
            {
                entity.ToTable("ProviderDetailsMarkdownPages", "Info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MarkdownPageId).HasColumnName("MarkdownPageID");

                entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

                entity.HasOne(d => d.MarkdownPage)
                    .WithMany(p => p.ProviderDetailsMarkdownPages)
                    .HasForeignKey(d => d.MarkdownPageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProviderD__Markd__27F8EE98");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.ProviderDetailsMarkdownPages)
                    .HasForeignKey(d => d.ProviderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProviderD__Provi__2704CA5F");
            });

            modelBuilder.Entity<ProviderLink>(entity =>
            {
                entity.ToTable("ProviderLinks", "Info");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExternalWebsiteId).HasColumnName("ExternalWebsiteID");

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

                entity.HasOne(d => d.ExternalWebsite)
                    .WithMany(p => p.ProviderLinks)
                    .HasForeignKey(d => d.ExternalWebsiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProviderL__Exter__2610A626");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.ProviderLinks)
                    .HasForeignKey(d => d.ProviderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProviderL__Provi__251C81ED");
            });

            modelBuilder.Entity<ProviderType>(entity =>
            {
                entity.HasIndex(e => e.Name, "UQ__Provider__737584F6D29891F3")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Color)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles", "Security");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Schema>(entity =>
            {
                entity.HasKey(e => e.Version)
                    .HasName("PK_HangFire_Schema");

                entity.ToTable("Schema", "HangFire");

                entity.Property(e => e.Version).ValueGeneratedNever();
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.ToTable("Server", "HangFire");

                entity.HasIndex(e => e.LastHeartbeat, "IX_HangFire_Server_LastHeartbeat");

                entity.Property(e => e.Id).HasMaxLength(200);

                entity.Property(e => e.LastHeartbeat).HasColumnType("datetime");
            });

            modelBuilder.Entity<Set>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.Value })
                    .HasName("PK_HangFire_Set");

                entity.ToTable("Set", "HangFire");

                entity.HasIndex(e => e.ExpireAt, "IX_HangFire_Set_ExpireAt");

                entity.HasIndex(e => new { e.Key, e.Score }, "IX_HangFire_Set_Score");

                entity.Property(e => e.Key).HasMaxLength(100);

                entity.Property(e => e.Value).HasMaxLength(256);

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<StarkwareTransactionCountDatum>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LastUpdateTime).HasColumnType("datetime");

                entity.Property(e => e.LastUpdateTps).HasColumnName("LastUpdateTPS");

                entity.Property(e => e.Product).HasMaxLength(255);

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.StarkwareTransactionCountData)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Starkware__Netwo__69FBBC1F");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.Id })
                    .HasName("PK_HangFire_State");

                entity.ToTable("State", "HangFire");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Reason).HasMaxLength(100);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.States)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_HangFire_State_Job");
            });

            modelBuilder.Entity<TimeWarpDataDay>(entity =>
            {
                entity.ToTable("TimeWarpData_Day");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TimeWarpDataDays)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeWarpD__Netwo__65370702");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TimeWarpDataDays)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeWarpD__Provi__6442E2C9");
            });

            modelBuilder.Entity<TimeWarpDataHour>(entity =>
            {
                entity.ToTable("TimeWarpData_Hour");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TimeWarpDataHours)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeWarpD__Netwo__634EBE90");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TimeWarpDataHours)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeWarpD__Provi__625A9A57");
            });

            modelBuilder.Entity<TimeWarpDataMinute>(entity =>
            {
                entity.ToTable("TimeWarpData_Minute");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TimeWarpDataMinutes)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeWarpD__Netwo__6166761E");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TimeWarpDataMinutes)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeWarpD__Provi__607251E5");
            });

            modelBuilder.Entity<TimeWarpDataWeek>(entity =>
            {
                entity.ToTable("TimeWarpData_Week");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TimeWarpDataWeeks)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeWarpD__Netwo__671F4F74");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TimeWarpDataWeeks)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeWarpD__Provi__662B2B3B");
            });

            modelBuilder.Entity<TimeWarpDatum>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TimeWarpData)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeWarpD__Netwo__5F7E2DAC");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TimeWarpData)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TimeWarpD__Provi__5E8A0973");
            });

            modelBuilder.Entity<TpsandGasDataAll>(entity =>
            {
                entity.ToTable("TPSAndGasData_All");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TpsandGasDataAlls)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Netwo__5BAD9CC8");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TpsandGasDataAlls)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Provi__5AB9788F");
            });

            modelBuilder.Entity<TpsandGasDataDay>(entity =>
            {
                entity.ToTable("TPSAndGasData_Day");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.OclhJson)
                    .HasMaxLength(255)
                    .HasColumnName("OCLH_JSON");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TpsandGasDataDays)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Netwo__55F4C372");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TpsandGasDataDays)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Provi__55009F39");
            });

            modelBuilder.Entity<TpsandGasDataHour>(entity =>
            {
                entity.ToTable("TPSAndGasData_Hour");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.OclhJson)
                    .HasMaxLength(255)
                    .HasColumnName("OCLH_JSON");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TpsandGasDataHours)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Netwo__540C7B00");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TpsandGasDataHours)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Provi__531856C7");
            });

            modelBuilder.Entity<TpsandGasDataLatest>(entity =>
            {
                entity.ToTable("TPSAndGasData_Latest");

                entity.HasIndex(e => e.Provider, "UQ__TPSAndGa__9944610B51A58108")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Gps).HasColumnName("GPS");

                entity.Property(e => e.Tps).HasColumnName("TPS");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TpsandGasDataLatests)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Netwo__503BEA1C");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithOne(p => p.TpsandGasDataLatest)
                    .HasForeignKey<TpsandGasDataLatest>(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Provi__4F47C5E3");
            });

            modelBuilder.Entity<TpsandGasDataMax>(entity =>
            {
                entity.ToTable("TPSAndGasData_Max");

                entity.HasIndex(e => e.Provider, "UQ__TPSAndGa__9944610B00C7220E")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.MaxGps).HasColumnName("MaxGPS");

                entity.Property(e => e.MaxGpsblockNumber).HasColumnName("MaxGPSBlockNumber");

                entity.Property(e => e.MaxTps).HasColumnName("MaxTPS");

                entity.Property(e => e.MaxTpsblockNumber).HasColumnName("MaxTPSBlockNumber");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TpsandGasDataMaxes)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Netwo__5224328E");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithOne(p => p.TpsandGasDataMax)
                    .HasForeignKey<TpsandGasDataMax>(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Provi__51300E55");
            });

            modelBuilder.Entity<TpsandGasDataMinute>(entity =>
            {
                entity.ToTable("TPSAndGasData_Minute");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TpsandGasDataMinutes)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Netwo__32767D0B");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TpsandGasDataMinutes)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Provi__318258D2");
            });

            modelBuilder.Entity<TpsandGasDataMonth>(entity =>
            {
                entity.ToTable("TPSAndGasData_Month");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.OclhJson)
                    .HasMaxLength(255)
                    .HasColumnName("OCLH_JSON");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TpsandGasDataMonths)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Netwo__6FB49575");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TpsandGasDataMonths)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Provi__6EC0713C");
            });

            modelBuilder.Entity<TpsandGasDataWeek>(entity =>
            {
                entity.ToTable("TPSAndGasData_Week");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.OclhJson)
                    .HasMaxLength(255)
                    .HasColumnName("OCLH_JSON");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TpsandGasDataWeeks)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Netwo__57DD0BE4");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TpsandGasDataWeeks)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Provi__56E8E7AB");
            });

            modelBuilder.Entity<TpsandGasDataYear>(entity =>
            {
                entity.ToTable("TPSAndGasData_Year");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AverageGps).HasColumnName("AverageGPS");

                entity.Property(e => e.AverageTps).HasColumnName("AverageTPS");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.NetworkNavigation)
                    .WithMany(p => p.TpsandGasDataYears)
                    .HasForeignKey(d => d.Network)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Netwo__59C55456");

                entity.HasOne(d => d.ProviderNavigation)
                    .WithMany(p => p.TpsandGasDataYears)
                    .HasForeignKey(d => d.Provider)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TPSAndGas__Provi__58D1301D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
