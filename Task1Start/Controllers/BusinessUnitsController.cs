//Imports the system namespace
using System;
//Imports the generic collections namespace found in system
using System.Collections.Generic;
//Imports the data namespace found in system
using System.Data;
//Imports the entity namespace, inside data, inside system
using System.Data.Entity;
//Imports the linq namespace, inside system
using System.Linq;
//Imports the net namespace, found in system.
using System.Net;
//Imports the web namespace, found in system.
using System.Web;
//Imports the mvc namespace, found in web, found in system.
using System.Web.Mvc;
//Imports the database model
using HebbraCoDbfModel;
//Imports the models found in task1start/models
using Task1Start.Models;

//Puts the code below into the controllers namespace
namespace Task1Start.Controllers
{
    //Creates a class BusinessUnitsController , which inherits from the controller class found in System.Web.MVC
    public class BusinessUnitsController : Controller
    {
        //Defines an object HebbraCo16Model, which instantiates the HebbraCo16Model class defined in HebbraCoDbfModel.
        private HebbraCo16Model db = new HebbraCo16Model();

        // GET: BusinessUnits
        //Implements ActionResult , which it inherited from Controller. Just tells the controller how to handle the action method Index()
        public ActionResult Index()
        {
            //Gets all the active business units from the businessunits table in the db.
            var allBu = db.BusinessUnits.Where(b => b.Active == true);
            //Creates a variable viewmodel , which creates a list of all the active businessunits
            var viewModel = Models.BusinessUnitVM.buildList(allBu);
            //Return them to be made into a viewmodel.
            return View(viewModel);
        }

        // GET: BusinessUnits/Details/5
        //Implements ActionResult , which it inherited from Controller. Just tells the controller how to handle the action method Details() which takes id as a parameter.
        public ActionResult Details(string id)
        {
            //If id is an empty string
            if (String.IsNullOrEmpty(id))
            {
                //Return a bad request http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Continue here if not an empty string
            //Creates a var thisBu which is the business units with businessunitcode = id
            var thisBu = db.BusinessUnits.SingleOrDefault(bu => bu.businessUnitCode.Equals(id, StringComparison.OrdinalIgnoreCase));
            //If it can't find that businessunit or if it's inactive:
            if (thisBu.Active == false || thisBu == null)
            {
                //Return a badrequest http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); ;
            }
            //Otherwise
            else
            {
                //Build a viewmodel of that businessunit and assign it to viewmodel
                var viewModel = BusinessUnitDetailVM.buildViewModel(thisBu);
                //Return that viewmodel as a view.
                return View(viewModel);
            }   
        }

        // GET: BusinessUnits/Create
        //Creates an action for Create()
        public ActionResult Create()
        {
            //Returns a new viewresult object.
            return View();
        }

        // POST: BusinessUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Restricts the following method to only use post when working with http
        [HttpPost]
        //Ensured the method uses anti-forgery tokens
        [ValidateAntiForgeryToken]
        //Implements ActionResult , which it inherited from Controller. Just tells the controller how to handle the action method Create(). 
        //This time it's overloaded to take in a business unit
        public ActionResult Create([Bind(Include = "businessUnitCode,title,description,officeAddress1,officeAddresss2,officeAddress3,officePostCode,officeContact,officePhone,officeEmail")] Task1Start.Models.BusinessUnitDetailVM businessUnitVM)
        {
            //If it's a valid model state dictionary object (If it has required fields).
            if (ModelState.IsValid)
            {
                //Build a model of that business unit viewmodel
                var model = BusinessUnitDetailVM.buildModel(businessUnitVM);
                //Set it as active
                model.Active = true;
                //Add it to the db
                db.BusinessUnits.Add(model);
                //Save it
                db.SaveChanges();
                //Send a index action, which returns the user to index.
                return RedirectToAction("Index");
            }
            //Otherwise returmn the businessunitviewmodel as a view
            return View(businessUnitVM);
        }

        // GET: BusinessUnits/Edit/5
        //An action method to edit a businessunit
        public ActionResult Edit(string id)
        {
            //If the id given is empty or null
            if (String.IsNullOrEmpty(id))
            {
                //Return a badrequest http error
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("EditError");
            }
            //Otherwise
            //Select the businss unit from the database with the businessunitcode equal to the id entered and assign to thisBu
            var thisBu = db.BusinessUnits.SingleOrDefault(bu => bu.businessUnitCode.Equals(id, StringComparison.OrdinalIgnoreCase));

            //If thisBu isn't active or doesn't exit
            if (thisBu.Active == false || thisBu == null)
            {
                //Return a badrequest http error
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest); ;
                return RedirectToAction("EditError");
            }
            //else
            else
            {
                //Build a viewmodel of that business unit
                var viewModel = BusinessUnitDetailVM.buildViewModel(thisBu);
                //return it
                return View(viewModel);
            }
        }

        //GET: BusinessUnits/EditError
        public ActionResult EditError(string id)
        {
            return View();
        }

        // POST: BusinessUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Restricts the following method to only use post when working with http
        [HttpPost]
        //Ensured the method uses anti-forgery tokens
        [ValidateAntiForgeryToken]
        //Deals with the user sending an action method to edit a businessunit.
        //This time it's overloaded to take a business unit in , as a parameter.
        public ActionResult Edit([Bind(Include = "businessUnitCode,title,description,officeAddress1,officeAddresss2,officeAddress3,officePostCode,officeContact,officePhone,officeEmail")] BusinessUnitDetailVM businessUnitVM)
        {
            //If it's a valid model state dictionary object (If it has required fields).
            if (ModelState.IsValid)
            {
                //If the businessunit code is in the database, assign the businessunit to efmodel
                var efmodel = db.BusinessUnits.SingleOrDefault(bu => bu.businessUnitCode.Equals(businessUnitVM.businessUnitCode, StringComparison.OrdinalIgnoreCase));
                //Build a businessunitviewmodel from efmodel
                var model = BusinessUnitDetailVM.buildModel(businessUnitVM, efmodel);
                //Set the database entries state to modified
                db.Entry(model).State = EntityState.Modified;
                //Save
                db.SaveChanges();
                //Redirect to the index action method
                return RedirectToAction("Index");
            }
            //Else return a view of the business unit view model
            return View(businessUnitVM);
        }

        // GET: BusinessUnits/Delete/5
        //Handles an action method for deleting business unit. Takes an id as a parameter.
        public ActionResult Delete(string id)
        {
            //If the id passed in is null or empty
            if (String.IsNullOrEmpty(id))
            {
                //Return a bad request http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //otherwise
            //Set thisBu to the business unit in the database with businessunitcode equal to id
            var thisBu = db.BusinessUnits.SingleOrDefault(bu => bu.businessUnitCode.Equals(id, StringComparison.OrdinalIgnoreCase));
            //If it's inactive or doesnt exist
            if (thisBu.Active == false || thisBu == null)
            {
                //Return a bad request http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); ;
            }
            //else
            else
            {
                //Set var viewmodel to a viewmodel of the business unit
                var viewModel = BusinessUnitDetailVM.buildViewModel(thisBu);
                //Return the viewmodel.
                return View(viewModel);
            }
        }

        // POST: BusinessUnits/Delete/5
        //Restricts the following method to only use post when working with http. Also can only occur if a delete action method has been run first.
        [HttpPost, ActionName("Delete")]
        //Ensured the method uses anti-forgery tokens
        [ValidateAntiForgeryToken]
        //Defines an action method deletecomfirmed , which takes an id as a parameter.
        public ActionResult DeleteConfirmed(string id)
        {
            //thisBu becomes equal to the businessunit in the database with businessunitcode equal to the id entered as a parameter.
            var thisBu = db.BusinessUnits.SingleOrDefault(bu => bu.businessUnitCode.Equals(id, StringComparison.OrdinalIgnoreCase));
            //Set the active property of it to false.
            thisBu.Active = false;
            //Set the state of it to modified.
            db.Entry(thisBu).State = EntityState.Modified;
            //Save the changes made to the database.
            db.SaveChanges();
            //Return the user to the index.
            return RedirectToAction("Index");
        }

        //Defines a method Dispose which takes a boolean disposing.
        //Overrides the one from controller.
        protected override void Dispose(bool disposing)
        {
            //If disposing is true
            if (disposing)
            {
                //Cal the dispose method of the database.
                db.Dispose();
            }
            //Otherwise pass to the one in controller.
            base.Dispose(disposing);
        }
    }
}
