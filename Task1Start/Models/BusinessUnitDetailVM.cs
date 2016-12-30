//Imports the system namespace
using System;
//Imports the generic collections namespace used for lists etc
using System.Collections.Generic;
//Imports the dataannotations namespace, which lets you set validation rules for data
//and set relationships, and specify how data is displayed
using System.ComponentModel.DataAnnotations;
//Imports the linq namespace, which supports linq queries
using System.Linq;
//Imports the web namespace which lets you communicate with browsers/servers
using System.Web;
//Imports the mcv namespace for obvious reasons
using System.Web.Mvc;

//Puts the below class into the models namespace
namespace Task1Start.Models
{
    //Defines a viewmodel for businessunitdetail that inherits businessunit
    public class BusinessUnitDetailVM : BusinessUnitVM
    {
        //Literally just all the fields in the database
        //[Required means it's a required field]
        [Required]
        //Gives it a name to display.
        [Display(Name = "Brief Description")]
        //Makes a var for it with getters and setters.
        public string description { get; set; }
        //Same
        [Required]
        [Display(Name = "Address Line 1")]
        public string officeAddress1 { get; set; }
        //Same
        [Required]
        [Display(Name = "Address Line 2")]
        public string officeAddresss2 { get; set; }
        //Same
        [Required]
        [Display(Name = "Address Line 3")]
        public string officeAddress3 { get; set; }
        //Same but cant have >10 chars.
        [Required]
        [StringLength(10)]
        [Display(Name = "Post Code")]
        [DataType(DataType.PostalCode)]
        public string officePostCode { get; set; }
        //Same
        [Required]
        [Display(Name = "Main Contact Full Name")]
        public string officeContact { get; set; }
        //Same
        [Required]
        [StringLength(50)]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string officePhone { get; set; }
        //Same
        [Required]
        [StringLength(50)]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string officeEmail { get; set; }
        //Same but this one is hidden at default.
        [HiddenInput(DisplayValue = false)]
        public string message { get; set; }

        //Defines a method for building the viewmodel which takes in a buisnessunit as a parameter.
        public static Models.BusinessUnitDetailVM buildViewModel(HebbraCoDbfModel.BusinessUnit thisBu)
        {
            //Creates a new viewmodel from the properties of the business unit
            BusinessUnitDetailVM vm = new BusinessUnitDetailVM
            {
                //Sets the text for each property of the viewmodel.
                businessUnitCode = thisBu.businessUnitCode.Trim(),
                title = thisBu.title,
                description = thisBu.description,
                officeAddress1 = thisBu.officeAddress1,
                officeAddresss2 = thisBu.officeAddresss2,
                officeAddress3 = thisBu.officeAddress3,
                officeContact = thisBu.officeContact,
                officeEmail = thisBu.officeEmail,
                officePhone = thisBu.officePhone,
                officePostCode = thisBu.officePostCode
            };
            //Returns the viewmodel.
            return vm;
        }

        //Defines a method that builds a model of a businessunit from a viewmodel
        public static HebbraCoDbfModel.BusinessUnit buildModel(BusinessUnitDetailVM businessUnit)
        {
            //Creates a new business unit.
            HebbraCoDbfModel.BusinessUnit Bu = new HebbraCoDbfModel.BusinessUnit();
          
            //Assigns values of the new businessunit to the ones from the viewmodel
            Bu.businessUnitCode = businessUnit.businessUnitCode;
            Bu.title = businessUnit.title;
            Bu.description = businessUnit.description;
            Bu.officeAddress1 = businessUnit.officeAddress1;
            Bu.officeAddresss2 = businessUnit.officeAddresss2;
            Bu.officeAddress3 = businessUnit.officeAddress3;
            Bu.officeContact = businessUnit.officeContact;
            Bu.officeEmail = businessUnit.officeEmail;
            Bu.officePhone = businessUnit.officePhone;
            Bu.officePostCode = businessUnit.officePostCode;
            //Sets the businessunit as active
            Bu.Active = true;
            //Returns it
            return Bu;
        }

        //Defines a method that builds a model of a businessunit from a viewmodel and businessunit
        public static HebbraCoDbfModel.BusinessUnit buildModel(BusinessUnitDetailVM businessUnit, HebbraCoDbfModel.BusinessUnit Bu)
        {
            //Sets the properties of the businessunit passed as a param to the one from the viewmodel.
            Bu.businessUnitCode = businessUnit.businessUnitCode;
            Bu.title = businessUnit.title;
            Bu.description = businessUnit.description;
            Bu.officeAddress1 = businessUnit.officeAddress1;
            Bu.officeAddresss2 = businessUnit.officeAddresss2;
            Bu.officeAddress3 = businessUnit.officeAddress3;
            Bu.officeContact = businessUnit.officeContact;
            Bu.officeEmail = businessUnit.officeEmail;
            Bu.officePhone = businessUnit.officePhone;
            Bu.officePostCode = businessUnit.officePostCode;
            //Sets it as active
            Bu.Active = true;
            //Return it.
            return Bu;
        }
    }
}