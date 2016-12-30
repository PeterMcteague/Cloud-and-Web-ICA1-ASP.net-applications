namespace StaffSkillsModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("staffSkill")]
    public partial class staffSkill
    {
        public int staffSkillId { get; set; }

        [Required]
        [StringLength(8)]
        public string staffCode { get; set; }

        [Required]
        [StringLength(10)]
        public string skillCode { get; set; }

        public bool active { get; set; }
    }
}
