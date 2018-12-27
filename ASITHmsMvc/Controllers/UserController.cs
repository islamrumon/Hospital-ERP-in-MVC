using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASIT_Hospital_MVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // GET: User
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult About() {
            return View();
        }
        public ActionResult Contact() {
            return View();
        }
    }
}