using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Task4Start.Controllers
{
    public class SkillController : Controller
    {
        private Task4Start.SkillService.SkillServiceClient Client = new Task4Start.SkillService.SkillServiceClient();

        // GET: Skill
        public ActionResult Index()
        {
            var allSkills = Client.GetAllSkills();
            return View(allSkills);
        }
    }
}