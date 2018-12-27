using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASITHmsEntity;
using ASITHmsViewMan;
using ASITHmsViewMan.General;
using ASITHmsViewMan.Commercial;

namespace ASITHmsMvc
{
    public class AspSession
    {
        public static AspSession Current
        {
            get
            {
                AspSession session = (AspSession)HttpContext.Current.Session["__AppWebSession__"];
                if (session == null)
                {
                    session = new AspSession();
                    HttpContext.Current.Session["__AppWebSession__"] = session;
                }
                return session;
            }
        }
        public string AppTitle
        {
            get { return "CentERPoint Healthcare Management System"; }

        }

        public string HmsVersion = "181121.1"; //"180627.1"; //"180602.1"; //"180514.1"; // "180503.1"; // "180422.1"; // "180409.1"; // "180324.1"; Product Version Control    // dbo.SP_REPORT_CODEBOOK_01 @ProcID = 'VERSIONCHK01'
        public string VersionType = "1"; // Development & Testing Version (0 from Published Version)
        public string AppComCode = "5600"; // Development & Testing Version (0 from Published Version)
        public string AppLocalImagePath = ""; // Development & Testing Version (0 from Published Version)
        public string AppRptViewStyle = "Dialog"; // Development & Testing Version (0 from Published Version)
        public List<HmsEntityGeneral.DatabaseErrorInfo> DatabaseErrorInfoList { get; set; }         // Back-End Database and Commpunication Realtes Error List
        public List<HmsEntityGeneral.CompInfCodeBook> CompInfList { get; set; }                     // User Company, Branches and Department List 
        public List<HmsEntityManpower.SignInInfo> SignedInUserList { get; set; }                   // Application User List from User Information Code Book (Sub-Set of Resource Code Book)
        public List<HmsEntityGeneral.UserInterfaceAuth.uiObjSignInAuth> SignedInUserAuthList;                 // Application User List from User Information Code Book (Sub-Set of Resource Code Book)
        public List<string> AspFormsList;
        public List<HmsEntityGeneral.SirInfCodeBook> StaffGroupList { get; set; }                  // Company's Human Resources Group List from Resource Code Book
        public List<HmsEntityGeneral.SirInfCodeBook> StaffList { get; set; }                       // Company's Human Resources List from Resource Code Book
        public List<HmsEntityGeneral.SirInfCodeBook> SupplierContractorList { get; set; }          // Supplier and Contructors List from Resource Code Book
        public List<HmsEntityGeneral.SirInfCodeBook> InvItemGroupList { get; set; }                // Inventory Items List from Resource Code Book
        public List<HmsEntityGeneral.SirInfCodeBook> InvItemList { get; set; }                    // Inventory Items List from Resource Code Book
        public List<HmsEntityGeneral.AcInfCodeBook> GenInfoTitleList { get; set; }                 // Others General Information Title List from Accounts Code Book
        public List<HmsEntityGeneral.AcInfCodeBook> AccCodeList { get; set; }                      // Chart of Accounts
        public List<HmsEntityGeneral.SirInfCodeBook> AccSirCodeList { get; set; }                  // Chart of Subsidiary Accounts
        public List<HmsEntityCommercial.HmsServiceItem> ServiceItemList { get; set; }             // Hospital/Diagnostic Centre Service Item List
        public List<HmsEntityCommercial.HmsRefByInf> RefByInfList { get; set; }                   // Hospital/Diagnostic Centre Service Item List
        public List<HmsEntityCommercial.CommInvSummInf> CommInvSummList { get; set; }             // Hospital/Diagnostic Centre Commercial Invoice Summary List
    }
}
