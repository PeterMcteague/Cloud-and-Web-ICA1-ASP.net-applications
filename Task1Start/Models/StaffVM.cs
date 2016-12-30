using HebbraCoDbfModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Task1Start.Models
{
    public class StaffVM
    {
        [Key]
        [Required]
        [StringLength(8)]
        [Display(Name = "Staff Code")]
        public string staffCode { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength =1)]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Display(Name = "Full Name")]
        public string fullName
        {
            get
            {return firstName + " " + lastName; }
            set
            { fullName = firstName + " " + lastName; }
        }

        [Required(ErrorMessage = "Business unit code is required.")]
        [ForeignKey("BusinessUnitCode")]
        [Display(Name = "Business Unit Code")]
        public string businessUnitCode { get; set; }

        //Builds a list of actives. Might also filter by other things.
        public static IEnumerable<Models.StaffVM> buildList(IEnumerable<HebbraCoDbfModel.Staff> allStaff)
        {
            //Selects all business units in the list and creates a new businessunitviewmodel
            var staff = allStaff.Select(b =>
                        new Models.StaffVM()
                        {
                            //Staff code
                            staffCode = b.staffCode.Trim(),
                            //Name
                            firstName = b.firstName,
                            lastName = b.lastName,
                            //Business unit code where the businessunitid is the same
                            businessUnitCode = b.BusinessUnit.businessUnitCode,
                    }).AsEnumerable(); //Casts it to an enumerable
            return staff;
        }
    }
}