using HebbraCoDbfModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Task1Start.Models;

namespace Task1Start.Controllers
{
    public class StaffController : Controller
    {
        //Defines an object HebbraCo16Model, which instantiates the HebbraCo16Model class defined in HebbraCoDbfModel.
        private HebbraCo16Model db = new HebbraCo16Model();

        // GET: Staff
        public ActionResult Index(String id)
        {
            //If id is an empty string
            if (String.IsNullOrEmpty(id))
            {
                //Get all active staff from db, with validation checks.
                var allStaff = db.Staffs.Where(s => s.Active == true && s.staffCode.Length == 8 && (s.startDate.Year != 0001));
                //Build a list of them.
                var viewModel = Models.StaffVM.buildList(allStaff);
                //Return them to be made into a viewmodel.
                return View(viewModel);
            }
            else
            {
                //Get all staff with that businessunitcode
                var staffs = db.Staffs.Where(s => s.Active == true && s.staffCode.Length == 8 && s.BusinessUnit.businessUnitCode == id && (s.startDate.Year != 0001));
                var viewModel = StaffVM.buildList(staffs);
                //Return them to be made into a viewmodel.
                return View(viewModel);
            }     
        }

        // GET: Staff/Edit/5
        //Edit /Staff/Edit/String
        public ActionResult Edit(string id)
        {
            //If the id given is empty or null
            if (String.IsNullOrEmpty(id))
            {
                //Return a badrequest http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Otherwise
            //Select the staff member from the database with the staffId equal to the id entered and assign to thisStaff
            var thisStaff = db.Staffs.SingleOrDefault(s => s.staffCode.Equals(id, StringComparison.OrdinalIgnoreCase));

            //If thisStaff isn't active or doesn't exist
            if (thisStaff == null || thisStaff.Active == false )
            {
                ////Return a badrequest http error
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //Replacing this with an error view
                return View("Error");

            }
            //else
            else
            {
                //Build a viewmodel of that staff
                var viewModel = Models.StaffDetailVM.buildViewModel(thisStaff);
                //Add the dropdown to the viewbag
                List<SelectListItem> items = new List<SelectListItem>();
                var allBU = db.BusinessUnits.Where(s => s.Active == true);
                foreach (var bu in allBU)
                {
                    if (thisStaff.BusinessUnit.businessUnitCode == bu.businessUnitCode)
                    {
                        items.Add(new SelectListItem { Text = bu.businessUnitCode.Trim(), Value = bu.businessUnitCode, Selected = true });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = bu.businessUnitCode.Trim(), Value = bu.businessUnitCode});
                    }
                }
                ViewBag.Dropdown = items;
                //return it
                return View(viewModel);
            }
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "businessUnitCode,staffCode,firstName,middleName,lastName,fullName,dob,startDate,profile,emailAddress,PhotoUrl")] StaffDetailVM staffVM)
        {
            //If it's a valid model state dictionary object (If it has required fields).
            if (ModelState.IsValid)
            {
                var efmodel = db.Staffs.SingleOrDefault(s => s.staffCode.Equals(staffVM.staffCode, StringComparison.OrdinalIgnoreCase));
                //Build a staffviewmodel from efmodel
                var model = StaffDetailVM.buildModel(staffVM, efmodel);
                //Set the database entries state to modified
                db.Entry(model).State = EntityState.Modified;
                //Save
                db.SaveChanges();
                //Redirect to the index action method
                return RedirectToAction("Index");
            }
            staffVM.PhotoUrl = "URL";
            List<SelectListItem> items = new List<SelectListItem>();
            var allBU = db.BusinessUnits.Where(s => s.Active == true);
            foreach (var bu in allBU)
            {
                if (staffVM.businessUnitCode == bu.businessUnitCode)
                {
                    items.Add(new SelectListItem { Text = bu.businessUnitCode, Value = bu.businessUnitCode, Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = bu.businessUnitCode, Value = bu.businessUnitCode });
                }
            }
            ViewBag.Dropdown = items;
            //Else return a view of the business unit view model
            return View(staffVM);
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, [Bind(Include = "businessUnitCode,staffCode,firstName,middleName,lastName,fullName,dob,startDate,profile,emailAddress,PhotoUrl")] StaffDetailVM staff)
        {
            //Image upload
            if (file != null)
            {
                string path = System.IO.Path.Combine(Server.MapPath("~/Images"), System.IO.Path.GetFileName(file.FileName));
                file.SaveAs(path);
                staff.PhotoUrl = file.FileName;
                ViewBag.Message = "File uploaded successfully";
            }
            else
            {
                var thisStaff = db.Staffs.Where(s => s.staffCode.Equals(staff.staffCode));
                staff.PhotoUrl = thisStaff.First().PhotoUrl;
            }

            staff.businessUnitCode = staff.businessUnitCode.Trim();
            staff.BusinessUnitID = db.BusinessUnits.Where(b => b.businessUnitCode.Equals(staff.businessUnitCode)).First().businessUnitId;

            //Profile edit
            if (ModelState.IsValid)
            {
                var efmodel = db.Staffs.SingleOrDefault(s => s.staffCode.Equals(staff.staffCode, StringComparison.OrdinalIgnoreCase));
                //Build a staffviewmodel from efmodel
                var model = StaffDetailVM.buildModel(staff, efmodel);
                //Set the database entries state to modified
                db.Entry(model).State = EntityState.Modified;
                //Save
                //CATCH THIS INCASE DB Dies
                try
                { 
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    //Do nothing i.e. don't update the db
                }
                return RedirectToAction("Index");
            }
            List<SelectListItem> items = new List<SelectListItem>();
            var allBU = db.BusinessUnits.Where(s => s.Active == true);
            foreach (var bu in allBU)
            {
                if (staff.businessUnitCode == bu.businessUnitCode)
                {
                    items.Add(new SelectListItem { Text = bu.businessUnitCode, Value = bu.businessUnitCode, Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = bu.businessUnitCode, Value = bu.businessUnitCode });
                }
            }
            ViewBag.Dropdown = items;
            //Else return a view of the business unit view model
            return View(staff);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            //Add the dropdown to the viewbag
            List<SelectListItem> items = new List<SelectListItem>();
            var allBU = db.BusinessUnits.Where(s => s.Active == true);
            foreach (var bu in allBU)
            {
                    items.Add(new SelectListItem { Text = bu.businessUnitCode.Trim(), Value = bu.businessUnitCode.Trim().ToString() });
            }
            ViewBag.Dropdown = items;
            //return it
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        //Ensured the method uses anti-forgery tokens
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include = "staffCode,businessUnitCode,firstName,middleName,lastName,dob,startDate,profile,emailAddress,PhotoUrl,Active")] Task1Start.Models.StaffDetailVM staff)
        {
            staff.businessUnitCode = staff.businessUnitCode.Trim();

            //Setting businessUnitID
            staff.BusinessUnitID = db.BusinessUnits.Where(b => b.Active == true && b.businessUnitCode == staff.businessUnitCode).First().businessUnitId;

            //Generate staffcode
            staff.staffCode = "u0000000";
            var staffCodeInt = Int32.Parse(staff.staffCode.Substring(1));
            var allStaff = db.Staffs;
            Boolean validCode = false;
            while (validCode == false)
            {
                Boolean changed = false;
                foreach (var s in allStaff)
                {
                    //If u + the number is in there.
                    if (("u" + new String('0', (7 - staffCodeInt.ToString().Length)) + staffCodeInt.ToString()).Equals(s.staffCode))
                    {
                        Debug.Print("EXISTS");
                        staffCodeInt++;
                        changed = true;
                    }
                }
                if (changed == false)
                {
                    validCode = true;
                }
            }
            staff.staffCode = ("u" + new String('0', 7 - staffCodeInt.ToString().Length) + staffCodeInt.ToString());

            //Image upload
            if (file != null)
            {
                string path = System.IO.Path.Combine(Server.MapPath("~/Images"), System.IO.Path.GetFileName(file.FileName));
                file.SaveAs(path);
                staff.PhotoUrl = file.FileName;
                ViewBag.Message = "File uploaded successfully";
            }
            else
            {
                var thisStaff = db.Staffs.Where(s => s.staffCode.Equals(staff.staffCode));
                try
                {
                    if (thisStaff.First() != null)
                    {
                        staff.PhotoUrl = thisStaff.First().PhotoUrl;
                    }
                }
                catch(InvalidOperationException)
                {
                    staff.PhotoUrl = "url";
                }
            }

            Debug.Print(staff.staffCode);

            //This needs removing as it's using the value when post is called, which is nothing as it doesn't yet exist.
            ModelState.Remove("staffCode");

            //Profile edit
            if (ModelState.IsValid)
            {
                var efmodel = db.Staffs.SingleOrDefault(s => s.staffCode.Equals(staff.staffCode, StringComparison.OrdinalIgnoreCase));
                //Build a staffviewmodel from efmodel
                var model = StaffDetailVM.buildModel(staff);
                //Save
                //CATCH THIS INCASE DB Dies
                try
                {
                    // Build a model of that business unit viewmodel
                    //Set it as active
                    model.Active = true;
                    //Add it to the db
                    db.Staffs.Add(model);
                    //Save it
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    //Do nothing i.e. don't update the db
                    Debug.Print("ERROR UPDATING DB");
                }
                return RedirectToAction("Index");
            }
            else
            {
                List<SelectListItem> items = new List<SelectListItem>();
                var allBU = db.BusinessUnits.Where(s => s.Active == true);
                foreach (var bu in allBU)
                {
                    if (staff.businessUnitCode == bu.businessUnitCode)
                    {
                        items.Add(new SelectListItem { Text = bu.businessUnitCode, Value = bu.businessUnitCode, Selected = true });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = bu.businessUnitCode, Value = bu.businessUnitCode });
                    }
                }
                ViewBag.Dropdown = items;
                //Else return a view of the business unit view model
                return View(staff);
            }
        }

        // GET: Staff/Delete/5
        public ActionResult Delete(string id)
        {
            //If the id passed in is null or empty
            if (String.IsNullOrEmpty(id))
            {
                //Return a bad request http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var thisStaff = db.Staffs.SingleOrDefault(s => s.staffCode.Equals(id, StringComparison.OrdinalIgnoreCase));
            //If it's inactive or doesnt exist
            if (thisStaff.Active == false || thisStaff == null)
            {
                //Return a bad request http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); ;
            }
            //else
            else
            {
                //Set var viewmodel to a viewmodel of the business unit
                var viewModel = StaffDetailVM.buildViewModel(thisStaff);
                //Return the viewmodel.
                return View(viewModel);
            }
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var thisStaff = db.Staffs.SingleOrDefault(s => s.staffCode.Equals(id, StringComparison.OrdinalIgnoreCase));
            //Set the active property of it to false.
            thisStaff.Active = false;
            //Set the state of it to modified.
            db.Entry(thisStaff).State = EntityState.Modified;
            //Save the changes made to the database.
            db.SaveChanges();
            //Return the user to the index.
            return RedirectToAction("Index");
        }

        // GET: Staff/Details/5
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
            var thisStaff = db.Staffs.SingleOrDefault(s => s.staffCode.Equals(id, StringComparison.OrdinalIgnoreCase));
            //If it can't find that businessunit or if it's inactive:
            if (thisStaff.Active == false || thisStaff == null)
            {
                //Return a badrequest http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); ;
            }
            //Otherwise
            else
            {
                //Build a viewmodel of that businessunit and assign it to viewmodel
                var viewModel = StaffDetailVM.buildViewModel(thisStaff);
                //Return that viewmodel as a view.
                return View(viewModel);
            }
        }

    }
}