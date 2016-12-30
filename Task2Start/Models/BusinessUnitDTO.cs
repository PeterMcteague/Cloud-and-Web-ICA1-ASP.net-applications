using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HebbraCoDbfModel;

namespace Task2Start.Models
{
    public class BusinessUnitDTO
    {
        ///<summary>The unique code for the business unit.</summary>
        public string businessUnitCode { get; set; }

        ///<summary>The title for the business unit.</summary>
        public string title { get; set; }

        //Builds a list of BusinessUnitDTO from a database query.
        public static IEnumerable<BusinessUnitDTO> buildList(IEnumerable<BusinessUnit> Allbu)
        {
            //Create businessunits from query.
            var busUnits = Allbu.Select(b =>
                       new Models.BusinessUnitDTO()
                       {
                           businessUnitCode = b.businessUnitCode.Trim(),
                           title = b.title.Trim(),
                       }).AsEnumerable();
            return busUnits;
        }
    }
}