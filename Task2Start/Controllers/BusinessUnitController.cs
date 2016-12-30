using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using HebbraCoDbfModel;
using Task2Start.Models;

namespace Task2Start.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BusinessUnitController : ApiController
    {
        //Setting up a connection to the HebraCoDb
        private HebbraCoDbfModel.HebbraCo16Model context = new HebbraCo16Model();

        ///<summary>Gets a list of all the business units.</summary>
        ///<returns>Returns all the business units.</returns> 
        public IEnumerable<BusinessUnitDTO> Get()
        {
            //Select all the active business units.
            var allBu = context.BusinessUnits.Where(b => b.Active == true);
            //Build a list of BusinessUnitDTO's from them.
            var dto = BusinessUnitDTO.buildList(allBu);
            return dto;
        }
    }
}
