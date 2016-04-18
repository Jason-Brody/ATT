namespace ATT.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AttDbContext : DbContext
    {
        public AttDbContext()
            : base("name=AttDbContext")
        {
        }

        public virtual DbSet<IDocNumbers> IDocNumbers { get; set; }

        public virtual DbSet<EDIKeys> EDIKeys { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
