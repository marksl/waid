using System.Web.Mvc;

namespace WaidWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/HowItWorks/
        public ActionResult HowItWorks()
        {
            return View();
        }

        // GET: /Home/WhatYouSee/
        public ActionResult WhatYouSee()
        {
            return View();
        }
    }
}
