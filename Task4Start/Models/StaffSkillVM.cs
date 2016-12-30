using Newtonsoft.Json;
using StaffSkillsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Task4Start.Models
{
    public class StaffSkillVM
    {
        public string fullName { get; set; }
        public string staffCode { get; set; }
        public List<SelectListItem> items { get; set; }
        public String selectedSkill { get; set; }
        public IEnumerable<SkillService.SkillsDTO> skills { get; set; }

        public static Models.StaffSkillVM buildListFromStaffCode(String staffCode)
        {
            StaffSkillVM viewmodel = new StaffSkillVM();

            //Get skills with that staffcode from the database
            StaffSkillsModelContext skilldb = new StaffSkillsModelContext();
            var staffSkills = skilldb.staffSkills.Where(s => s.staffCode == staffCode && s.active == true).ToList();

            //Pass them into the skillservice to get name and description
            //GetSkill(skillcode)
            var skillServiceClient = new SkillService.SkillServiceClient();
            var thisStaffSkills = new List<SkillService.SkillsDTO>();
            foreach(var skill in staffSkills)
            {
                var thisSkill = skillServiceClient.GetSkill(skill.skillCode);
                if (thisSkill != null)
                {
                    thisStaffSkills.Add(skillServiceClient.GetSkill(skill.skillCode));
                }
            }
            //Get name from api
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:65026");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            HttpResponseMessage response = client.GetAsync("api/Staff/" + staffCode).Result;
            var staffDetails = response.Content.ReadAsAsync<Models.StaffDetailDTO>().Result;
            viewmodel.fullName = staffDetails.fullName;

            viewmodel.skills = thisStaffSkills;
            viewmodel.staffCode = staffCode;
            return viewmodel;
        }
    }
}