using HebbraCoDbfModel;
using System;
using System.Activities.Statements;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace Task1Start.Models
{
    public class StaffDetailVM : StaffVM
    {
        [Display(Name = "Middle name")]
        public string middleName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime dob { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [Display(Name = "Start date")]
        public DateTime startDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Profile")]
        public string profile { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [Display(Name = "Email Address")]
        public string emailAddress { get; set; }

        [Display(Name = "Photo")]
        public string PhotoUrl { get; set; }

        [Display(Name = "Business Unit ID")]
        public int BusinessUnitID { get; set; }

        [Display(Name = "Full Name")]
        public new string fullName
        {
            get
            { return firstName + " " + middleName + " " + lastName; }
            set
            { fullName = firstName + " " + middleName + " " + lastName; }
        }

        public static Models.StaffDetailVM buildViewModel(HebbraCoDbfModel.Staff thisStaff)
        {
            //Creates a new viewmodel from the properties of the business unit
            StaffDetailVM vm = new StaffDetailVM
            {
                businessUnitCode = thisStaff.BusinessUnit.businessUnitCode.Trim(),
                firstName = thisStaff.firstName,
                staffCode = thisStaff.staffCode,
                middleName = thisStaff.middleName,
                lastName = thisStaff.lastName,
                dob = thisStaff.dob,
                startDate = thisStaff.startDate,
                profile = thisStaff.profile,
                emailAddress = thisStaff.emailAddress,
                PhotoUrl = thisStaff.PhotoUrl
            };
            //Returns the viewmodel.
            return vm;
        }

        public static HebbraCoDbfModel.Staff buildModel(StaffDetailVM staff)
        {
            //Creates a new business unit.
            HebbraCoDbfModel.Staff s = new HebbraCoDbfModel.Staff();
            HebbraCo16Model db = new HebbraCo16Model();

            //Assigns values of the new staff to the ones from the viewmodel
            s.businessUnitId = staff.BusinessUnitID;
            s.firstName = staff.firstName;
            s.staffCode = staff.staffCode;
            s.middleName = staff.middleName;
            s.lastName = staff.lastName;
            s.dob = staff.dob;
            s.startDate = staff.startDate;
            s.profile = staff.profile;
            s.emailAddress = staff.emailAddress;
            s.PhotoUrl = staff.PhotoUrl;
            //Sets the businessunit as active
            s.Active = true;
            //Returns it
            return s;
        }

        public static HebbraCoDbfModel.Staff buildModel(StaffDetailVM staff, HebbraCoDbfModel.Staff s)
        {
            //Sets the properties of the businessunit passed as a param to the one from the viewmodel.
            s.firstName = staff.firstName;
            s.staffCode = staff.staffCode;
            s.middleName = staff.middleName;
            s.lastName = staff.lastName;
            s.dob = staff.dob;
            s.startDate = staff.startDate;
            s.profile = staff.profile;
            s.emailAddress = staff.emailAddress;
            s.PhotoUrl = staff.PhotoUrl;
            s.businessUnitId = staff.BusinessUnitID;
            //Sets it as active
            s.Active = true;
            //Return it.
            return s;
        }
    }
}