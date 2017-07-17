using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACFUN.Controllers
{
    public class UserController : Controller
    {

        static string ORM = "adsfasdf";
        // GET: User
        public ActionResult Login()
        {
            ViewBag.DD = ORM;
            return View();
        }

        public ActionResult Change()
        {
            ORM = "1321313";
            return RedirectToAction("/Login");
        }
    }
}