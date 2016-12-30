namespace HebbraCoDbfModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BusinessUnit")]
    public partial class BusinessUnit
    {
        public BusinessUnit()
        {
            Staffs = new HashSet<Staff>();
        }

        public int businessUnitId { get; set; }

        [Required]
        [StringLength(10)]
        public string businessUnitCode { get; set; }

        [Required]
        [StringLength(50)]
        public string title { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public string officeAddress1 { get; set; }

        [Required]
        public string officeAddresss2 { get; set; }

        [Required]
        public string officeAddress3 { get; set; }

        [Required]
        [StringLength(10)]
        public string officePostCode { get; set; }

        [Required]
        public string officeContact { get; set; }

        [Required]
        [StringLength(50)]
        public string officePhone { get; set; }

        [Required]
        [StringLength(50)]
        public string officeEmail { get; set; }

        public bool? Active { get; set; }

        public virtual ICollection<Staff> Staffs { get; set; }
    }
}
