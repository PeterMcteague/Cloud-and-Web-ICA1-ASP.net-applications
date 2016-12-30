namespace HebbraCoDbfModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HebbraCo16Model : DbContext
    {
        public HebbraCo16Model()
            : base("name=HebbraCo16Model")
        {
        }

        public virtual DbSet<BusinessUnit> BusinessUnits { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusinessUnit>()
                .Property(e => e.businessUnitCode)
                .IsFixedLength();

            modelBuilder.Entity<BusinessUnit>()
                .Property(e => e.officePostCode)
                .IsFixedLength();

            modelBuilder.Entity<BusinessUnit>()
                .HasMany(e => e.Staffs)
                .WithRequired(e => e.BusinessUnit)
                .WillCascadeOnDelete(false);
        }
    }
}
