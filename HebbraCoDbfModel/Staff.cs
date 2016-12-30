namespace HebbraCoDbfModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Staff")]
    public partial class Staff
    {
        public int staffId { get; set; }

        public int businessUnitId { get; set; }

        [Required]
        [StringLength(50)]
        public string staffCode { get; set; }

        [Required]
        public string firstName { get; set; }

      
        public string middleName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Column(TypeName = "date")]
        public DateTime dob { get; set; }

        [Column(TypeName = "date")]
        public DateTime startDate { get; set; }

        public string profile { get; set; }

        [Required]
        public string emailAddress { get; set; }

        public string PhotoUrl { get; set; }

        public bool Active { get; set; }

        public virtual BusinessUnit BusinessUnit { get; set; }
    }
}
