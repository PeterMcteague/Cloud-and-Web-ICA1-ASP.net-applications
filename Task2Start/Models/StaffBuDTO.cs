using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HebbraCoDbfModel;

namespace Task2Start.Models
{
    public class StaffBuDTO
    {
        ///<summary>The name of the staff member.</summary>
        public string fullName { get; set; }
        ///<summary>The staff code of the staff member.</summary>
        public string staffCode { get; set; }

        //Takes in a list of staff for the businessunit.
        public static IEnumerable<StaffBuDTO> buildList(IEnumerable<Staff> staffIn)
        {
            var staff = staffIn.Select(b =>
                       new Models.StaffBuDTO()
                       {
                           fullName = (b.firstName + " " + b.middleName + " " + b.lastName),
                           staffCode = b.staffCode.Trim(),
                       }).AsEnumerable();
            return staff;
        }
    }
}