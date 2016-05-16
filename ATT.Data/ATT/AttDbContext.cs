namespace ATT.Data.ATT
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AttDbContext : DbContext
    {
        public AttDbContext()
            : base("name=AttDbContext") {
        }

        public virtual DbSet<IDocNumbers> IDocNumbers { get; set; }
        public virtual DbSet<IDocNumbers_ITG> IDocNumbers_ITG { get; set; }
        public virtual DbSet<IDocTypes> IDocTypes { get; set; }
        public virtual DbSet<Missions> Missions { get; set; }
        public virtual DbSet<MsgIDs> MsgIDs { get; set; }
        public virtual DbSet<MsgIDs_ITG> MsgIDs_ITG { get; set; }
        public virtual DbSet<ParameterConfigs> ParameterConfigs { get; set; }
        public virtual DbSet<ProAwsys> ProAwsys { get; set; }
        public virtual DbSet<SAPCompanyCodes> SAPCompanyCodes { get; set; }
        public virtual DbSet<SAPDocTypes> SAPDocTypes { get; set; }
        public virtual DbSet<SAPInterfaces> SAPInterfaces { get; set; }
        public virtual DbSet<SenderConfigs> SenderConfigs { get; set; }
        public virtual DbSet<Sources> Sources { get; set; }
        public virtual DbSet<TaskDataConfigs> TaskDataConfigs { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<XNodes> XNodes { get; set; }
        public virtual DbSet<XPathConfigs> XPathConfigs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<IDocNumbers>()
                .Property(e => e.Time)
                .HasPrecision(0);

            modelBuilder.Entity<IDocNumbers_ITG>()
                .Property(e => e.Time)
                .HasPrecision(0);

            modelBuilder.Entity<IDocTypes>()
                .HasMany(e => e.MsgIDs)
                .WithOptional(e => e.IDocTypes)
                .HasForeignKey(e => e.IDocTypeId);

            modelBuilder.Entity<IDocTypes>()
                .HasMany(e => e.SenderConfigs)
                .WithRequired(e => e.IDocTypes)
                .HasForeignKey(e => e.IDocTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IDocTypes>()
                .HasMany(e => e.XPathConfigs)
                .WithOptional(e => e.IDocTypes)
                .HasForeignKey(e => e.IDocTypeId);

            modelBuilder.Entity<Missions>()
                .HasMany(e => e.Tasks)
                .WithOptional(e => e.Missions)
                .HasForeignKey(e => e.Mid);

            modelBuilder.Entity<SAPInterfaces>()
                .HasMany(e => e.MsgIDs)
                .WithOptional(e => e.SAPInterfaces)
                .HasForeignKey(e => e.InterfaceId);

            modelBuilder.Entity<SAPInterfaces>()
                .HasMany(e => e.SAPCompanyCodes)
                .WithOptional(e => e.SAPInterfaces)
                .HasForeignKey(e => e.InterfaceId);

            modelBuilder.Entity<SAPInterfaces>()
                .HasMany(e => e.SAPDocTypes)
                .WithOptional(e => e.SAPInterfaces)
                .HasForeignKey(e => e.InterfaceId);

            modelBuilder.Entity<SAPInterfaces>()
                .HasMany(e => e.Tasks)
                .WithOptional(e => e.SAPInterfaces)
                .HasForeignKey(e => e.InterfaceId);

            modelBuilder.Entity<Sources>()
                .HasMany(e => e.ProAwsys)
                .WithOptional(e => e.Sources)
                .HasForeignKey(e => e.SourceId);

            modelBuilder.Entity<Sources>()
                .HasMany(e => e.SenderConfigs)
                .WithRequired(e => e.Sources)
                .HasForeignKey(e => e.SourceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<XNodes>()
                .HasMany(e => e.ParameterConfigs)
                .WithOptional(e => e.XNodes)
                .HasForeignKey(e => e.NodeId);

            modelBuilder.Entity<XNodes>()
                .HasMany(e => e.XPathConfigs)
                .WithOptional(e => e.XNodes)
                .HasForeignKey(e => e.XNodeId);
        }
    }
}
