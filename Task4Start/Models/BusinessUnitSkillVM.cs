using StaffSkillsModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Task4Start.Models
{

    public class BusinessUnitSkillVM
    {
        [Required]
        [StringLength(10)]
        [Display(Name = "Code")]
        public string businessUnitCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Title")]
        public string title { get; set; }

        public IEnumerable<SkillService.SkillsDTO> skills { get; set; }

        public static Models.BusinessUnitSkillVM buildListFromBUCode(String buCode)
        {
            BusinessUnitSkillVM viewmodel = new BusinessUnitSkillVM();
            viewmodel.businessUnitCode = buCode;
            var allStaffSkills = new List<staffSkill>();

            //Get Business Unit title from api/BusinessUnit
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:65026");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            HttpResponseMessage response = client.GetAsync("api/BusinessUnit/").Result;
            var allBU = response.Content.ReadAsAsync<IEnumerable<BusinessUnitDTO>>().Result;
            var thisBU = allBU.FirstOrDefault(b => b.businessUnitCode == buCode);
            viewmodel.title = thisBU.title;

            //Get all staff in BU from api/Staff/BusinessUnit/{buCode}
            response = client.GetAsync("api/Staff/BusinessUnit/" + buCode).Result;
            var allStaff = response.Content.ReadAsAsync<IEnumerable<StaffDTO>>().Result;

            //Get skills for each staff 
            StaffSkillsModelContext skilldb = new StaffSkillsModelContext();
            foreach (var staff in allStaff)
            {
                var staffSkills = skilldb.staffSkills.Where(s => s.staffCode == staff.staffCode && s.active == true).ToList();
                foreach (var skill in staffSkills)
                {
                    if (!allStaffSkills.Contains(skill))
                    {
                        allStaffSkills.Add(skill);
                    }
                }
            }

            //Now get each one from skillService and add to vm
            var skillServiceClient = new SkillService.SkillServiceClient();
            var thisStaffSkills = new List<SkillService.SkillsDTO>();
            foreach (var skill in allStaffSkills)
            {
                var thisSkill = skillServiceClient.GetSkill(skill.skillCode);
                if (thisSkill != null)
                {
                    thisStaffSkills.Add(skillServiceClient.GetSkill(skill.skillCode));
                }
            }
            viewmodel.skills = thisStaffSkills;

            return viewmodel;
        }
    }
}