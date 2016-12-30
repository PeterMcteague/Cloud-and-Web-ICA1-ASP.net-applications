using StaffSkillsModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Task4Start.Models;
using Task4Start.SkillService;

namespace Task4Start.Controllers
{
    public class StaffController : Controller
    {

        private StaffSkillsModelContext skilldb = new StaffSkillsModelContext();

        //For making api requests
        private static HttpResponseMessage ApiRequest(String address)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:65026/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            HttpResponseMessage response = client.GetAsync(address).Result;
            return response;
        }

        // GET: Staff/Index/5
        public ActionResult Index(string id)
        {
            //If id is an empty string
            if (String.IsNullOrEmpty(id))
            {
                //Return a bad request http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpResponseMessage response = ApiRequest("/api/Staff/BusinessUnit/" + id);
            var staffs = response.Content.ReadAsAsync<IEnumerable<Models.StaffDTO>>().Result;
            var viewModel = Models.StaffDTO.buildList(staffs);
            //Return them to be made into a viewmodel.
            return View(viewModel);
        }

        // GET: Staff/Skills/5
        public ActionResult Skills(String id)
        {
            //If id is an empty string
            if (String.IsNullOrEmpty(id))
            {
                //Return a bad request http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewModel = Models.StaffSkillVM.buildListFromStaffCode(id);
            return View(viewModel);
        }

        // GET: Staff/Skills/AddSkill?staffCode=staffCode
        public ActionResult AddSkill(String staffCode)
        {
            //If id is an empty string
            if (String.IsNullOrEmpty(staffCode))
            {
                //Return a bad request http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Get skills with that staffcode from the database
            StaffSkillsModelContext skilldb = new StaffSkillsModelContext();
            var staffSkills = skilldb.staffSkills.Where(s => s.staffCode == staffCode && s.active == true).ToList();

            //Pass them into the skillservice to get name and description
            //GetSkill(skillcode)
            var skillServiceClient = new SkillService.SkillServiceClient();
            var allSkills = skillServiceClient.GetAllSkills();
            var skillsToAdd = allSkills.ToList();
            var thisStaffSkills = new List<SkillService.SkillsDTO>();
            //Convert staffSkill to skillDTO
            foreach (var skill in staffSkills)
            {
                var thisSkill = skillServiceClient.GetSkill(skill.skillCode);
                if (thisSkill != null)
                {
                    thisStaffSkills.Add(skillServiceClient.GetSkill(skill.skillCode));
                }
            }
            //Remove all the staffs skills from the list of skills
            foreach (var skill in thisStaffSkills)
            {
                foreach (var innerSkill in allSkills)
                {
                    if (innerSkill.skillCode == skill.skillCode)
                    {
                        Debug.Print("This staff member already knows " + skill.skillCode);
                        skillsToAdd.Remove(innerSkill);
                    }
                }
            }
            //All skills is now just the ones the staff member doesn't have.
            var viewmodel = new StaffSkillVM();
            viewmodel.staffCode = staffCode;
            viewmodel.skills = thisStaffSkills;

            //Getting fullName for viewmodel
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:65026");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            HttpResponseMessage response = client.GetAsync("api/Staff/" + staffCode).Result;
            var staffDetails = response.Content.ReadAsAsync<Models.StaffDetailDTO>().Result;
            viewmodel.fullName = staffDetails.fullName;

            viewmodel.items = new List<SelectListItem>();
            //Add dropdown via viewbag
            foreach (var skill in skillsToAdd)
            {
                viewmodel.items.Add(new SelectListItem { Text = skill.skillDescription.Trim(), Value = skill.skillCode.Trim()});
            }

            return View(viewmodel);
        }

        // POST: Staff/Skills/AddSkill?staffCode=staffCode
        [HttpPost]
        public ActionResult AddSkill([Bind(Include ="selectedSkill,staffCode,fullName,items,skills")] StaffSkillVM viewmodel)
        {
            StaffSkillsModelContext db = new StaffSkillsModelContext();
            if (!ModelState.IsValid)
            {
                if (viewmodel.selectedSkill != null)
                {
                    //Add a new entry to the database of the staffcode and the selected skillcode
                    var skill = new staffSkill();
                    skill.skillCode = viewmodel.selectedSkill;
                    skill.staffCode = viewmodel.staffCode;
                    skill.active = true;
                    //Set the database entries state to modified
                    try
                    {
                        //Add it to the db
                        db.staffSkills.Add(skill);
                        //Save it
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        //Do nothing i.e. don't update the db
                        Debug.Print("ERROR UPDATING DB");
                    }
                    return RedirectToAction("Skills","Staff", new {@id = viewmodel.staffCode });
                }
            }
            return RedirectToAction("AddSkill","Staff",new{staffCode = viewmodel.staffCode});
        }
    }
}