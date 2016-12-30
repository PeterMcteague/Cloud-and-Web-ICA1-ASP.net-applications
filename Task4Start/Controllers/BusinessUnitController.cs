using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Task4Start.Controllers
{
    public class BusinessUnitController : Controller
    {
        //For sending requests to the api.
        private static HttpResponseMessage ApiRequest(String address)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri("http://localhost:65026/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            HttpResponseMessage response = client.GetAsync(address).Result;
            return response;
        }

        // GET: BusinessUnit
        public ActionResult Index()
        {
            HttpResponseMessage response = ApiRequest("/api/BusinessUnit");
            if (response.IsSuccessStatusCode)
            {
                var businessUnits = response.Content.ReadAsAsync<IEnumerable<Models.BusinessUnitDTO>>().Result;
                var viewModel = Models.BusinessUnitDTO.buildList(businessUnits);
                //Return them to be made into a viewmodel.
                return View(viewModel);
            }
            else
            {
                Debug.WriteLine("Index received a bad response from the web service.");
                return View();
            }
        }

        // GET: BusinessUnit/Skills/5
        public ActionResult Skills(string id)
        {
            //If id is an empty string
            if (String.IsNullOrEmpty(id))
            {
                //Return a bad request http error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewModel = Models.BusinessUnitSkillVM.buildListFromBUCode(id);
            return View(viewModel);
        }
    }
}