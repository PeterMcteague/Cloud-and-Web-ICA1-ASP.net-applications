//Imports the namespace System
using System;
//Imports the generic collections namespace from System
using System.Collections.Generic;
//Imports the linq namespace from System
using System.Linq;
//Imports the web namespace from System
using System.Web;
//Imports the MVC namespace from web from system
using System.Web.Mvc;

//Puts the below into the controllers namespace
namespace Task1Start.Controllers
{
    //Defines a class HomeController which inherits controller
    public class HomeController : Controller
    {
        //Defines an action method index()
        public ActionResult Index()
        {
            //When ran returns a view
            return View();
        }

        //Defines an action method about()
        public ActionResult About()
        {
            //Sets the message property of the view to the below
            ViewBag.Message = "Your application description page.";
            //Returns the view
            return View();
        }

        //Defines an action method contact
        public ActionResult Contact()
        {
            //Sets the message property of the view to the below
            ViewBag.Message = "Your contact page.";
            //Returns the view
            return View();
        }
    }
}