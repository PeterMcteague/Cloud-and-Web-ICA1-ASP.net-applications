using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task4Start.Models
{
    public class BusinessUnitDTO
    {
        [Required]
        [StringLength(10)]
        [Display(Name = "Code")]
        public string businessUnitCode { get; set; }

         [Required]
        [StringLength(50)]
        [Display(Name = "Title")]
        public string title { get; set; }

        public static IEnumerable<Models.BusinessUnitDTO> buildList(IEnumerable<Models.BusinessUnitDTO> allBu)
        {
            //Selects all business units in the list and creates a new businessunitviewmodel
            var busUnits = allBu.Select(b =>
                        new Models.BusinessUnitDTO()
                        {
                            //Gets the businessunitcode (-whitespace) and assigns it as a property of the new businessunitvm
                            businessUnitCode = b.businessUnitCode.Trim(),
                            //Gets the title assigns it as a property of the new businessunitvm
                            title = b.title,
                        }).AsEnumerable(); //Casts it to an enumerable
            //Returns the list of business units.
            return busUnits;
        }
    }
}