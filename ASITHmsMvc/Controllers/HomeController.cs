using ASITHmsMvc.App_Start;
using ASITHmsMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASITHmsMvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //this action for dynamic menu
        public ActionResult Sidemenu() {

          // var m = menuItems.menuParts();

            //this is  menu 
            string data = "";
            data += "<div id='sidebar-menu' class='main-menu-wrapper'> <div class='menu-section'>  <ul class='nav side-menu flex-column'>";
            var filter2m = menuitems1.menuParts().FindAll(x => x.menuId.Substring(2, 2) == "00").OrderBy(y => y.menuId);
            int i = 0;
            //var sitname = HostingEnvironment.ApplicationHost.GetSiteName();
            //sitname = sitname.Substring(0, sitname.IndexOf("\\"));

            foreach (var item in filter2m)
            {
                data = data + "<li class='nav-item has-child'> <a href='javascript: void(0); ' class='ripple'>" + item.menuDesc.ToString() + "<span class='fa fa-chevron-right'></span></a>";
                var filter2d1 = menuitems1.menuParts().FindAll(x => x.menuId.Substring(0, 2) == item.menuId.Substring(0, 2) && x.menuId.Substring(2, 2) != "00").OrderBy(y => y.menuId);

                data += " <ul class='nav child-menu'>";

                foreach (var item2 in filter2d1)
                {
                    string c = item2.menuUrlC.Trim();
                    string a = item2.menuUrlA.Trim();
                    data += " <li> &nbsp;&nbsp;<a href='"+Url.Action(a,c)+"' style='color:#2d0069'>" + item2.menuDesc.Trim()+ "</a></li>";
                    //data += @" <li> &nbsp;&nbsp;<a href='" + HostingEnvironment.ApplicationPhysicalPath + item2.menuUrl.Trim() + "' style='color:#2d0069'>" + item2.menuDesc.Trim() + "</a></li>";
                }
                data += " </ul>";

                data += " </li>";

            }

            data += " </ul></div></div>";

            //create class or proprety for pass row html data
            rawMenu rm = new rawMenu();
            rm.description = data;
    
            return PartialView("SideMenu", rm);
        }

        public ActionResult header() {

            header h = new header();
            try {
                if ((AspSession.Current.CompInfList[0].comlabel != null))
                {
                    byte[] bytes12 = AspSession.Current.CompInfList[0].comlabel;
                    String imgStr1 = (bytes12 == null ? "" : Convert.ToBase64String(bytes12));
                    h.imgTitle1 = "data:image/jpg;base64," + imgStr1;
                }

                if ((AspSession.Current.CompInfList[0].comlogo != null))
                {
                    byte[] bytes12 = AspSession.Current.CompInfList[0].comlogo;
                    String imgStr1 = (bytes12 == null ? "" : Convert.ToBase64String(bytes12));
                    h.imgLogo1 = "data:image/jpg;base64," + imgStr1;
                }
                if ((AspSession.Current.SignedInUserList[0].hcphoto != null))
                {
                    byte[] bytes12 = (byte[])AspSession.Current.SignedInUserList[0].hcphoto;
                    String imgStr1 = (bytes12 == null ? "" : Convert.ToBase64String(bytes12));
                    h.imgAdmin = "data:image/jpg;base64," + imgStr1;

                    //MemoryStream mem = new MemoryStream(bytes);
                    //BitmapImage bmp3 = new BitmapImage();
                    //bmp3.BeginInit();
                    //bmp3.StreamSource = mem;
                    //bmp3.EndInit();
                    //this.imgSignInUser.Source = bmp3;
                    //this.UserPhoto.Source = bmp3;
                }
                if (AspSession.Current.SignedInUserList[0].signinnam != null) {
                    h.signname = AspSession.Current.SignedInUserList[0].signinnam + " [" + AspSession.Current.SignedInUserList[0].sessionID + "] ";
                }
                return PartialView("header", h);
            }
            catch (Exception ex) {
                return RedirectToAction("index", "login");
            }

            
        }
    }
}