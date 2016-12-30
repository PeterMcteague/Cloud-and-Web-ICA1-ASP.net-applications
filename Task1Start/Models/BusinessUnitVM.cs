//Imports the system namespace
using System;
//Imports the generic collections namespace from it (For lists etc)
using System.Collections.Generic;
//Imports the dataannotations namespace, which lets you set validation rules for data
//and set relationships, and specify how data is displayed
using System.ComponentModel.DataAnnotations;
//Imports the linq namespace, which supports linq queries
using System.Linq;
//Imports the web namespace which lets you communicate with browsers/servers
using System.Web;

//Puts the below into the models namespace
namespace Task1Start.Models
{
    //Defines a class businessunitviewmodel
    public class BusinessUnitVM
    {
        //Defines a string businessunitcode with a getter and setter
        //It's displayed as the viewmodel as code, is required and cant have >10 chars.
        [Required]
        [StringLength(10)]
        [Display(Name = "Code")]
        public string businessUnitCode { get; set; }

        //Defines a string title with a getter and setter
        //It's displayed as the viewmodel as "unit name", is required and cant have >10 chars.
        [Required]
        [StringLength(50)]
        [Display(Name = "Unit Name")]
        public string title { get; set; }

        public Boolean active { get; set; }

                //Defines a method buildlist. It takes in a list of businessunits and returns a list of businessunitviewmodels
        public static IEnumerable<Models.BusinessUnitVM> buildList(IEnumerable<HebbraCoDbfModel.BusinessUnit> allBu)
        {
            //Selects all business units in the list and creates a new businessunitviewmodel
            var busUnits = allBu.Select(b =>
                        new Models.BusinessUnitVM()
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