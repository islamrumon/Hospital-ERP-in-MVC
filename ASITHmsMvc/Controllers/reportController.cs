using ASITFunLib;
using ASITHmsEntity;
using ASITHmsRpt2Inventory;
using ASITHmsRpt3Manpower;
using ASITHmsViewMan.Inventory;
using ASITHmsViewMan.Manpower; //ASITHmsWeb.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASITHmsMvc.Controllers
{
    public class reportController : Controller
    {
        // GET: report

        #region //this is report data list
        private vmEntryStoreReq1 REQ = new vmEntryStoreReq1();
        private vmReportStore1 REQ1 = new vmReportStore1();
        private vmEntryStoreIssue1 ESS = new vmEntryStoreIssue1();
        private vmEntryPurReq1 PRREQ = new vmEntryPurReq1();
        private vmEntryItemRcv1 ITMREC = new vmEntryItemRcv1();
        private vmReportStore1 ITMREC1R = new vmReportStore1();
        private const int defaultPageSize = 20;
        #endregion

        #region //this report paramiter
        //string reportType = print;
     
  
        #endregion
        public ActionResult Index()
        {
            return View();
        }

        #region //this is report for AttnRptSingle
        [HttpGet]
        public ActionResult AttnRptSingle (string monthid1, string hccode1a,  string reportType ) {

            //check the conditon
            if (hccode1a == null || monthid1 == null || reportType == null) {
                return RedirectToAction("Index");
            }
           

            vmEntryAttnLeav1 vm2 = new vmEntryAttnLeav1();
            var pap1r = vm2.SetParamShowScheduledAttnInfo1(AspSession.Current.CompInfList[0].comcpcod, monthid1, hccode1a, "PRINT");
            DataSet ds1r = AspProcessAccess.GetHmsDataSet(pap1r);
            

            
            string RptType = "Attendance";

            List<HmsEntityManpower.RptAttnSchInfo> Rptlst = HcmGeneralClass1.GetIndRosterAttendance(monthid1: monthid1, hccode1a: hccode1a, RptType: RptType);
           
           

            vmReportHCM1 vmr1 = new vmReportHCM1();
            vmEntryHRGenral1 vm1 = new vmEntryHRGenral1();

            var pap1 = vmr1.SetHRMList(AspSession.Current.CompInfList[0].comcpcod, hccode1a, "EXISTSTAFFS");
            DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
            

            var Staff1 = ds1.Tables[0].DataTableToList<vmReportHCM1.Stafflist>();

            decimal sumLate1 = Rptlst.Sum(x => x.confirmlate);
            string Notes1 = (sumLate1 > 0 ? "Late Point = " + sumLate1.ToString("##") : "");

            decimal sumEout1 = Rptlst.Sum(x => x.confirmearly);
            Notes1 = Notes1 + (Notes1.Length > 0 && sumEout1 > 0 ? ", " : "") + (sumEout1 > 0 ? "Early Out Point = " + sumEout1.ToString("##") : "");
            Notes1 = (Notes1.Length > 0 ? "Confirm " : "") + Notes1;

            var pap2 = vm1.SetParamShowHCInfo(AspSession.Current.CompInfList[0].comcpcod, hccode1a, "PHOTO");
            DataSet dss2 = AspProcessAccess.GetHmsDataSet(pap2);
            

            byte[] bytes12 = null;
            if (!(dss2.Tables[0].Rows[0]["hcphoto"] is DBNull))
            {
                bytes12 = (byte[])dss2.Tables[0].Rows[0]["hcphoto"];

            }

            // create hasetable and send in to report methode
            Hashtable rptParam = new Hashtable();
            rptParam["Comlogo"] = (bytes12 == null ? "" : Convert.ToBase64String(bytes12));
            rptParam["empId"] = hccode1a.ToString().Trim();
            rptParam["empName"] = "Employee : " + hccode1a.Substring(6, 6) + " - " + Staff1[0].hcname.Trim() + ", " + Staff1[0].designame.Trim(); //& AtxtEmpAll.Text.ToString();
            rptParam["slMnth"] = (RptType == "Attendance" ? "Monthly Attendence" : "Duty Roster") + " - " + monthid1;
            rptParam["ParmNotes1"] = Notes1;
            rptParam["ParmBrnDept1"] = "Department : " + Staff1[0].deptname.Trim() + ", Joining Date : " + Staff1[0].joindat.Trim() +
                                       ", Reporting Date : " + Convert.ToDateTime(ds1r.Tables[2].Rows[0]["ServerTime"]).ToString("dd-MMM-yyyy hh:mm tt");


            var list3 = AspProcessAccess.GetRptGenInfo(ServerTime: Convert.ToDateTime(ds1r.Tables[2].Rows[0]["ServerTime"]));
            //send and get data form Repoert dll
            
            TempData["rName"] = "Payroll.RptAttenSchedule01";
            TempData["Rptlist"] = Rptlst;
            TempData["rptParam"] = rptParam;
            TempData["reportType"] = reportType;
            TempData["list3"] = list3;
                return RedirectToAction("Report");
          
        }
        #endregion

        public ActionResult Report() {
            //report name
            var reportName = TempData["rName"].ToString(); ;
            //report list
            var Rptlist = TempData["Rptlist"];
            //report paramiters
            var rptparam = TempData["rptParam"];
            //list3
            var list3 = TempData["list3"];
           // Rpt1 = HcmReportSetup.GetLocalReport("Payroll.RptAttenSchedule01", Rptlst, rptParam, null, null);

            //report generat paramiters
            string reportType = TempData["reportType"].ToString();
            string mimeType;
            string encoding;
            string fileNameExtension;
            string[] streams;
            Warning[] warning;
            string deviceInfo =
              "<DeviceInfo>" +
              "  <OutputFormat>" + reportType + "</OutputFormat>" +
              "</DeviceInfo>";
            //intial the localreport
            LocalReport Rpt1 = new LocalReport();

            #region //all code to get retunr rdlc 

            switch (reportName) {

                case "Store.RptClosingStock2":
                case "Store.RptClosingStock1L":
                case "Store.RptClosingStock1":
                case "Store.RptTransectionList":
                case "Store.RptStoreIssueSum1":
                case "Store.RptPurMrrSum1":
                case "Store.RptMrrDetails1":
                case "Store.RptMrrDetails2":
                case "Store.RptIssueDetails1":
                case "Store.RptStoreReqDetails1":
                case "Store.RptPurReqDetails1":
                case "Store.RptItemStatus1":
                    Rpt1 = StoreReportSetup.GetLocalReport(reportName, Rptlist, null, list3);
                    break;
                case "Payroll.RptAttenSchedule01":
                    Rpt1 = HcmReportSetup.GetLocalReport(reportName, Rptlist, rptparam, null, list3);
                    break;
                default:
                    return RedirectToAction("index");
            }
            //if (reportName == "")
            //{
                
            //}
            //else if (reportName == "") {
            //    Rpt1 = StoreReportSetup.GetLocalReport(reportName, Rptlist, null, list3);
            //} else if (reportName == "" || reportName == "") {
                
            //}
            

            #endregion

            //this is the code to print theall report
            var renderedByte = Rpt1.Render(
                  reportType,
                 deviceInfo,
                 out mimeType,
                 out encoding,
                 out fileNameExtension,
                 out streams,
                 out warning);

            if (reportType == "xlsx")
            {


                string extension = "xlsx";
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename= Report" + "." + extension);
                Response.OutputStream.Write(renderedByte, 0, renderedByte.Length); //Create the File
                Response.Flush(); //Send it to client for download
                Response.End();

            }
            else if (reportType == "docx")
            {



                string extension = "docx";
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename= Report" + "." + extension);
                Response.OutputStream.Write(renderedByte, 0, renderedByte.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();

            }
            return File(renderedByte, mimeType);
        }
    }
}