namespace StaffSkillsModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StaffSkillsModelContext : DbContext
    {
        public StaffSkillsModelContext()
            : base("name=StaffSkillsModel")
        {
        }

        public virtual DbSet<staffSkill> staffSkills { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<staffSkill>()
                .Property(e => e.staffCode)
                .IsFixedLength();

            modelBuilder.Entity<staffSkill>()
                .Property(e => e.skillCode)
                .IsFixedLength();
        }
    }
}
