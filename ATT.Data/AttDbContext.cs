namespace ATT.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Entity;
    public partial class AttDbContext : DbContext
    {
        public AttDbContext()
            : base("name=AttDbContext")
        {
        }

        public virtual DbSet<IDocNumbers> IDocNumbers { get; set; }

        public virtual DbSet<IDocNumbers_ITG> IDocNumbers_ITG { get; set; }

        public virtual DbSet<MsgIDs> MsgIds { get; set; }
        public virtual DbSet<IDocType> IDocTypes { get; set; }
        public virtual DbSet<ParameterConfig> ParameterConfigs { get; set; }
        public virtual DbSet<ProAwsy> ProAwsys { get; set; }
        public virtual DbSet<SenderConfig> SenderConfigs { get; set; }
        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<XNode> XNodes { get; set; }
        public virtual DbSet<XPathConfig> XPathConfigs { get; set; }

        public virtual DbSet<MsgIDs_ITG> MsgIDs_ITG { get; set; }
        public virtual DbSet<SAPCompanyCodes> SAPCompanyCodes { get; set; }
        public virtual DbSet<SAPDocTypes> SAPDocTypes { get; set; }
        public virtual DbSet<SAPInterfaces> SAPInterfaces { get; set; }

        public virtual DbSet<TaskDataConfigs> TaskDataConfigs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {

            modelBuilder.Entity<SAPInterfaces>()
                .HasMany(e => e.SAPCompanyCodes)
                .WithOptional(e => e.SAPInterfaces)
                .HasForeignKey(e => e.InterfaceId);

            modelBuilder.Entity<SAPInterfaces>()
                .HasMany(e => e.SAPDocTypes)
                .WithOptional(e => e.SAPInterfaces)
                .HasForeignKey(e => e.InterfaceId);

            modelBuilder.Entity<ProAwsy>()
                .HasMany(e => e.EDIKeys)
                .WithOptional(e => e.ProAwsy)
                .HasForeignKey(e => e.ProAwsysId);

            modelBuilder.Entity<ProAwsy>()
                .HasMany(e => e.ParameterConfigs)
                .WithOptional(e => e.ProAwsy)
                .HasForeignKey(e => e.ProAwsysId);

            modelBuilder.Entity<XNode>()
                .HasMany(e => e.ParameterConfigs)
                .WithOptional(e => e.XNode)
                .HasForeignKey(e => e.NodeId);
        }
    }
}
