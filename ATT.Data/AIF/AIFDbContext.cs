//namespace ATT.Data.AIF
//{
//    using System;
//    using System.Data.Entity;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Linq;

//    public partial class AIFDbContext : DbContext
//    {
//        public AIFDbContext()
//            : base("name=AIFDbContext") {
//        }
//        public virtual DbSet<Errors> Errors { get; set; }
//        public virtual DbSet<IDocNumbers> IDocNumbers { get; set; }
//        public virtual DbSet<IDocs> IDocs { get; set; }
//        public virtual DbSet<Interfaces> Interfaces { get; set; }
//        public virtual DbSet<Missions> Missions { get; set; }
//        public virtual DbSet<Tasks> Tasks { get; set; }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
//            modelBuilder.Entity<IDocs>()
//                .Property(e => e.Time)
//                .HasPrecision(0);

//            modelBuilder.Entity<Interfaces>()
//                .HasMany(e => e.Tasks)
//                .WithOptional(e => e.Interfaces)
//                .HasForeignKey(e => e.InterfaceId);

//            modelBuilder.Entity<Missions>()
//                .HasMany(e => e.Tasks)
//                .WithOptional(e => e.Missions)
//                .HasForeignKey(e => e.Mid);

//            modelBuilder.Entity<Tasks>()
//                .HasMany(e => e.IDocNumbers)
//                .WithOptional(e => e.Tasks)
//                .HasForeignKey(e => e.Tid);

//            modelBuilder.Entity<Tasks>()
//                .HasMany(e => e.IDocs)
//                .WithOptional(e => e.Tasks)
//                .HasForeignKey(e => e.Tid);
//        }
//    }
//}
