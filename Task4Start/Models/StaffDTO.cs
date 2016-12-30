using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task4Start.Models
{
    public class StaffDTO
    {
        /// <summary>
        /// The full name of that staff member.
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string fullName { get; set; }

        /// <summary>
        /// The staff code for the staff member
        /// </summary>
        [Key]
        [Required]
        [StringLength(8)]
        [Display(Name = "Staff Code")]
        public string staffCode { get; set; }

        public static IEnumerable<Models.StaffDTO> buildList(IEnumerable<Models.StaffDTO> AllStaffs)
        {
            var staffs = AllStaffs.Select(b =>
                       new Models.StaffDTO()
                       {
                           staffCode = b.staffCode.Trim(),
                           fullName = b.fullName,
                       }).AsEnumerable();
            return staffs;
        }
    }
}