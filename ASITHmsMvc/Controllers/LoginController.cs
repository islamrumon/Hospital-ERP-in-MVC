using ASITHmsMvc.Models.DtoModels;
using ASITFunLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ASITHmsMvc.Controllers
{
    public class LoginController : Controller
    {
        //this constrector for loding all requirment
        public LoginController() {
            this.LoadCompInf();
        }


        //this function for load all requried data
        protected void LoadCompInf()
        {
            AspProcessAccess.GetAppConfigInfo();
            AspProcessAccess.GetCompanyInfoList();
            if (AspSession.Current.CompInfList != null)
                AspProcessAccess.GetCompanyStaffList();

            if (AspSession.Current.DatabaseErrorInfoList != null)
            {
                AspProcessAccess.ShowDatabaseErrorMessage(AspSession.Current.DatabaseErrorInfoList[0].errormessage);
                return;
            }

            if (AspSession.Current.StaffList == null)
            {
                AspProcessAccess.ShowDatabaseErrorMessage("Database configuration error occured.\nPlease contact to System Administrator");
                return;
            }

            if (AspSession.Current.StaffList.Count == 0)
            {
                AspProcessAccess.ShowDatabaseErrorMessage("Database configuration error occured.\nPlease contact to System Administrator");
                //Application.Current.Shutdown();
                return;
            }

        }



        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginDto login) {
            //there are implement the login implementation 
            if (ModelState.IsValid) {

                string UserSignInName = login.username.Trim().ToUpper(); //this.UserName.Text.Trim().ToUpper();
                string UserSignInPass = login.password.Trim();
                string TerminalID = Environment.MachineName.ToString().Trim().ToUpper();
                string TerminalMAC = AspProcessAccess.GetMacAddress();// AspProcessAccess.GetMacAddress();

                if (UserSignInName.Length < 4 || UserSignInPass.Length < 4)
                {
                    if (AspSession.Current.VersionType == "0") // Published Version (1 for Development & Testing Version)
                    ModelState.AddModelError("", "There are Some Problems");
                    return View();
                    // For Temporary Use
                   // UserSignInName = "HAFIZ";// "HARUN";
                    //UserSignInPass = "2222a";// "1111";
                }
                string encodedPw = ASITUtility.EncodePassword(UserSignInName + UserSignInPass);

                AspProcessAccess.GetSignedInUserList(UserSignInName, encodedPw, TerminalID, "", "");

                
                if (AspSession.Current.SignedInUserList == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View();
                }
                else {
                    FormsAuthentication.RedirectFromLoginPage(UserSignInName, false);
                }
                //if (AspSession.Current.SignedInUserList == null)
                //{
                //    FailureText.Text = "";
                //    ErrorMessage.Visible = true;
                //    return;
                //}
                //if (AspSession.Current.SignedInUserList.Count == 0)
                //{
                //    FailureText.Text = "Invalid username or password.";
                //    ErrorMessage.Visible = true;
                //    return;
                //}
               
            }
            return RedirectToAction("index","home");
        }

        //logout method
        
        public ActionResult logout() {

            AspSession.Current.CompInfList = null;

            AspSession.Current.DatabaseErrorInfoList = null;

            AspSession.Current.SignedInUserList = null;
            AspSession.Current.SignedInUserAuthList = null;
            AspSession.Current.AspFormsList = null;
            AspSession.Current.StaffGroupList = null;
            AspSession.Current.StaffList = null;
            AspSession.Current.SupplierContractorList = null;
            AspSession.Current.InvItemGroupList = null;
            AspSession.Current.InvItemList = null;
            AspSession.Current.GenInfoTitleList = null;
            AspSession.Current.AccCodeList = null;
            AspSession.Current.AccSirCodeList = null;
            AspSession.Current.ServiceItemList = null;
            AspSession.Current.RefByInfList = null;
            AspSession.Current.CommInvSummList = null;
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }
    }
}