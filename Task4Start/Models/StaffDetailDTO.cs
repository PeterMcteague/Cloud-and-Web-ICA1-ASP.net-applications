using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Task4Start.Models
{
    public class StaffDetailDTO
    {
        ///<summary>The name of the staff member.</summary>
        public string fullName { get; set; }
        ///<summary>The staff code for the staff member.</summary>
        public string staffCode { get; set; }
        ///<summary>The URL of the staff members photo.</summary>
        public string PhotoUrl { get; set; }
        ///<summary>The date of birth of the staff member.</summary>
        public string dob { get; set; }
        ///<summary>The date the staff member started.</summary>
        public string startDate { get; set; }
        ///<summary>The email address of the staff member.</summary>
        public string emailAddress { get; set; }
        ///<summary>The first name of the staff member.</summary>
        public string firstName { get; set; }
        ///<summary>The middle name of the staff member.</summary>
        public string middleName { get; set; }
        ///<summary>The last name of the staff member.</summary>
        public string lastName { get; set; }
        ///<summary>A profile written by the staff member.</summary>
        public string profile { get; set; }


        public static StaffDetailDTO buildList(StaffDetailDTO staffIn)
        {
            var staff = new StaffDetailDTO();
            //Set vars
            staff.fullName = (staffIn.firstName + " " + staffIn.middleName + " " + staffIn.lastName);
            staff.staffCode = staffIn.staffCode;
            staff.PhotoUrl = staffIn.PhotoUrl;
            staff.dob = staffIn.dob;
            staff.startDate = staffIn.startDate;
            staff.emailAddress = staffIn.emailAddress;
            staff.firstName = staffIn.firstName;
            staff.middleName = staffIn.middleName;
            staff.lastName = staffIn.lastName;
            staff.profile = staffIn.profile;
            //End of set vars
            return staff;
        }
    }
}