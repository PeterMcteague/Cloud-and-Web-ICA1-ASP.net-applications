using HebbraCoDbfModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Task2Start.Models;
using System.Data.Entity;
using System.Diagnostics;

namespace Task2Start.Controllers
{
    [RoutePrefix("api/Staff")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StaffController : ApiController
    {
        private HebbraCoDbfModel.HebbraCo16Model context = new HebbraCo16Model();

        ///<summary>Gets the details of the staff member with the staffcode entered.</summary>
        ///<param name="id">The staffcode of the staff member you want the details of.</param>
        ///<returns>Returns the details of the staff member with the entered staffcode.</returns>  
        // GET: api/Staff/5
        [Route("{id}")]
        public StaffDTO GetStaffDetail(string id)
        {
            //Select the one staff member with the staff code that is put in.
            var staff = context.Staffs.Where(b => b.Active == true && b.staffCode == id).First();
            //Build a StaffDTO from the result.
            var dto = Task2Start.Models.StaffDTO.buildList(staff);
            return dto;
        }

        ///<summary>Gets all the staff members that belong to the entered business unit.</summary>
        ///<param name="id">The business unit to get the staff for.</param>
        ///<returns>Returns a list of the staff in the business unit.</returns>  
        // GET: api/Staff/BusinessUnit/5
        [Route("BusinessUnit/{id}")]
        public IEnumerable<StaffBuDTO> GetStaffForBU(string id)
        {
            //Select all the active staff in the database with this businessUnitCode
            var allStaff = context.Staffs.Where(b => b.Active == true && b.BusinessUnit.businessUnitCode == id);    
            //Build a list of them and return them.
            var dto = Task2Start.Models.StaffBuDTO.buildList(allStaff);
            return dto;
        }
    }
}
