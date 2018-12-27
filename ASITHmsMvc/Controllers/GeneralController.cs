using ASITHmsMvc.App_Start;
using ASITHmsMvc.Models;
using ASITHmsMvc.Models.DtoModels;
using ASITFunLib;
using ASITHmsEntity;
using ASITHmsRpt3Manpower;
using ASITHmsRpt4Commercial;
using ASITHmsViewMan.Commercial;
using ASITHmsViewMan.Manpower;
using Microsoft.Reporting.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ASITHmsMvc.Controllers
{
    [Authorize]

    public class GeneralController : Controller
    {
        public string todate = "";
        public string fromDate = "";
        private List<HmsEntityManpower.RptAttnSchInfo> Rptlst1 = new List<HmsEntityManpower.RptAttnSchInfo>();
        private List<HmsEntityCommercial.CommInvSummInf> RptFrontDesk1 = new List<HmsEntityCommercial.CommInvSummInf>();
        private List<HmsEntityCommercial.GroupWiseTrans01> RptListc = new List<HmsEntityCommercial.GroupWiseTrans01>();
        private List<HmsEntityCommercial.GroupWiseTrans01> RptListd = new List<HmsEntityCommercial.GroupWiseTrans01>();
        private List<HmsEntityCommercial.FDeskCollSumm01> RptListColSum1 = new List<HmsEntityCommercial.FDeskCollSumm01>();
        private List<HmsEntityCommercial.FDeskSalesSumm01> RptDateWise = new List<HmsEntityCommercial.FDeskSalesSumm01>();
        private List<HmsEntityCommercial.FDeskDiscount01> RptDisRef = new List<HmsEntityCommercial.FDeskDiscount01>();
        private Hashtable rptParam = new Hashtable();

        private List<HmsEntityManpower.HcmLeaveDetailsReport01> RptYearLeaveList1 = new List<HmsEntityManpower.HcmLeaveDetailsReport01>();
        #region //for invoice
        //private List<HmsEntityCommercial.CommInvSummInf> RptFrontDesk1 = new List<HmsEntityCommercial.CommInvSummInf>();
        private List<HmsEntityCommercial.CommInv01.CommInv01TblItem> list1a = new List<HmsEntityCommercial.CommInv01.CommInv01TblItem>();
        private List<HmsEntityCommercial.CommInv01.CommInv01TblSum> list1c = new List<HmsEntityCommercial.CommInv01.CommInv01TblSum>();
        private List<HmsEntityCommercial.CommInv01.CommInv01TblCol> list1d = new List<HmsEntityCommercial.CommInv01.CommInv01TblCol>();
        private List<HmsEntityCommercial.CommInv01.CommInv01GenInf> list1b = new List<HmsEntityCommercial.CommInv01.CommInv01GenInf>();
        #endregion

        private List<HmsEntityManpower.HcmDayWiseAttanReport> RptGrpList1 = new List<HmsEntityManpower.HcmDayWiseAttanReport>();
        // GET: General

        [HttpGet]
        public ActionResult FdeskRpt() {

            //GetReportList rList = new GetReportList();
            ViewBag.branch = new SelectList(GetDeparttList(), "Value", "Text");
            ViewBag.report = new SelectList(GetReportList(), "Value", "Text");


            return View();
        }
        #region report generet
        string reportType;
        string mimeType;
        string encoding;
        string fileNameExtension;
        string[] streams;
        Warning[] warning;
        string toDate;
        string fromDate2;
        string report;
        string branch;
        [HttpPost]
        public ActionResult reports(fdeskRpt fRe)
        {
           
            if (ModelState.IsValid)
            {
                //assin the report paramiter
                LocalReport rpt = new LocalReport();
                this.reportType = fRe.reportType;
                this.fileNameExtension = reportType;
                string deviceInfo =
                   "<DeviceInfo>" +
                   "  <OutputFormat>" + reportType + "</OutputFormat>" +
                   "</DeviceInfo>";

                //this.todate = fRe.toDate;
                //this.fromDate = fRe.fromDate;
                //var dt1 = DateTime.Parse(fRe.fromDate);
                //var dt2 = DateTime.Parse(fRe.toDate);

                var dt1 = fRe.fromDate;
                var dt2 = fRe.toDate;
                this.report = fRe.report;
                this.branch = fRe.branch;
                string fromDate = (dt1 <= dt2 ? dt1 : dt2).ToString("dd-MMM-yyyy");// this.fromDate;
                this.fromDate2 = fromDate;
                string toDate = (dt1 > dt2 ? dt1 : dt2).ToString("dd-MMM-yyyy");// this.todate;
                this.todate = toDate;
                string rptItem = fRe.report;

                string SectCod = fRe.branch;
                SectCod = (SectCod.Substring(2, 2) == "00" ? SectCod.Substring(0, 0) : SectCod);
                SectCod = (SectCod.Length == 2 && SectCod.Substring(0, 2) == "00" ? SectCod.Substring(0, 2) : SectCod);  // Not knowning
                SectCod = (SectCod.Length == 2 && SectCod.Substring(2, 2) == "00" ? SectCod.Substring(2, 2) : SectCod);  //
                SectCod = (SectCod.Length != 4 ? SectCod + "%" : SectCod);
                string InvNum1 = "CSI";
                string InvStatus1 = "A";
                string SignInID1 = "%";
                string TerminalName1 = "%";
                string Option1 = "%";
                string SessionID1 = "%";
                string OrderBy1 = "DEFAULT";
                string processID = "";

                switch (rptItem)
                {
                    case "001":
                        processID = "COMMINVLIST01";
                        break;
                    case "002":
                        processID = "COMMINVDETAILS01";
                        break;
                    case "003":
                        processID = "GROUPDETAILS01";
                        break;
                    case "004":
                        processID = "CCDETAILSLIST01";
                        break;
                    case "005":
                        processID = "DISCOUNTLIST01";
                        break;
                    case "006":
                        processID = "DUESREFLIST01";
                        break;
                    case "007":
                        processID = "DUESREFBYLIST01";
                        break;
                    case "008":
                        processID = "GROUPSUMMARY01";
                        break;
                    case "009":
                        processID = "Unknown";
                        break;
                    case "010":
                        processID = "COLLSUMMARY01";
                        break;
                    case "011":
                        processID = "DAYSALCOLSUMMARY01";
                        break;
                }

                if (processID == "COLLSUMMARY01")
                {
                    this.CollectionSummary(RptProcID1: processID, BrnCode1: SectCod, StartDate1: fromDate, EndDate1: toDate, TerminalName1: TerminalName1);

                    ////loop for encripted url
                    //foreach (var i in this.RptListColSum1) {
                    //    this.RptListColSum1 s = new this.RptListColSum1();
                    //}
                    var jsonresult = Json(new { list = this.RptListColSum1 }, JsonRequestBehavior.AllowGet);
                    jsonresult.MaxJsonLength = int.MaxValue;
                    return jsonresult;
                }
                else if (processID == "DAYSALCOLSUMMARY01")
                {
                    this.DayWiseSalesCollectionSummary(RptProcID1: processID, BrnCode1: SectCod, StartDate1: fromDate, EndDate1: toDate, Option1: Option1);
                    
                    
                    var getdatewisecoll = Json(new { list = this.RptDateWise}, JsonRequestBehavior.AllowGet);
                    getdatewisecoll.MaxJsonLength = int.MaxValue;
                    return getdatewisecoll;
                }

                vmReportFrontDesk1 vmr = new vmReportFrontDesk1();
                var pap1 = vmr.SetParamFrontDeskReport(CompCode: AspSession.Current.CompInfList[0].comcpcod, ProcessID: processID, BrnchCod: SectCod, startDate: fromDate, EndDate: toDate,
                                           InvNum: InvNum1, PreparedBy: SignInID1, InvStatus: InvStatus1, TerminalName: TerminalName1, SessionID: SessionID1, OrderBy: OrderBy1);
                DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
                if (ds1 == null)
                    return Json("There are no Data", JsonRequestBehavior.AllowGet);
                if (ds1.Tables[0].Rows.Count == 0)
                {
                    return Json("There are No Data", JsonRequestBehavior.AllowGet);
                }

                this.RptListColSum1 = ds1.Tables[0].DataTableToList<HmsEntityCommercial.FDeskCollSumm01>();
                this.RptDisRef = ds1.Tables[0].DataTableToList<HmsEntityCommercial.FDeskDiscount01>();
                this.RptDateWise = ds1.Tables[0].DataTableToList<HmsEntityCommercial.FDeskSalesSumm01>(); // Date wise Collection
                this.RptListc = ds1.Tables[0].DataTableToList<HmsEntityCommercial.GroupWiseTrans01>();
                this.RptListd = ds1.Tables[1].DataTableToList<HmsEntityCommercial.GroupWiseTrans01>();
                this.RptFrontDesk1 = ds1.Tables[0].DataTableToList<HmsEntityCommercial.CommInvSummInf>();


                //list for reporting
                DateTime ServerTime1 = Convert.ToDateTime(ds1.Tables[1].Rows[0]["ServerTime"]);
                var list3 = AspProcessAccess.GetRptGenInfo(ServerTime: ServerTime1);

                //this date of 181106
                if (rptItem == "001" || rptItem == "002")
                {

                    if (rptItem == "002")
                    {
                        var RptLista1 = RptFrontDesk1.FindAll(x => x.ptinvnum2.Trim().Length == 0);
                        int i = 1;
                        string Pinv1 = "XXXXXXXXXXXXXXXXXX";
                        foreach (var item in RptLista1)
                        {
                            if (item.ptinvnum != Pinv1)
                                i = 1;
                            item.ptname = i.ToString("00") + ". " + item.ptname;
                            Pinv1 = item.ptinvnum;

                            i++;
                        }
                    }
                    //return the List
                    var gettransinvlist = Json(new { list = this.RptFrontDesk1 }, JsonRequestBehavior.AllowGet);

                    //check the report acton
                    if (fRe.reportType != null) {
                        rpt = CommReportSetup.GetLocalReport("Hospital.RptCommInvList1", RptFrontDesk1, null, list3);

                        byte[] renderedByte = rpt.Render(
                  reportType,
                 deviceInfo,
                 out mimeType,
                 out encoding,
                 out fileNameExtension,
                 out streams,
                 out warning);

                        return File(renderedByte, mimeType);
                    }
                    gettransinvlist.MaxJsonLength = int.MaxValue;
                    return gettransinvlist;

                }
                else if (rptItem == "003" || rptItem == "008")
                {
                    // "01. Group sales summary" 
                    var getGropSalesSumm = Json(new { list = this.RptListc }, JsonRequestBehavior.AllowGet);
                    getGropSalesSumm.MaxJsonLength = int.MaxValue;
                    return getGropSalesSumm;

                }
                else if (rptItem == "004")
                {
                    // "04. CC Charge Details"   
                    var getCCchargeDetails = Json(new { list = this.RptListColSum1 }, JsonRequestBehavior.AllowGet);
                    getCCchargeDetails.MaxJsonLength = int.MaxValue;
                    return getCCchargeDetails;

                }
                else if (rptItem == "005")
                {
                    // "05. Discount Reference List"  
                    var getDiscRefList = Json(new { list = this.RptDisRef }, JsonRequestBehavior.AllowGet);
                    getDiscRefList.MaxJsonLength = int.MaxValue;
                    return getDiscRefList;
                }
                else if (rptItem == "006")
                {
                    // "06. Dues Reference List"
                    var getDuesRefList = Json(new { list = this.RptDisRef }, JsonRequestBehavior.AllowGet);
                    getDuesRefList.MaxJsonLength = int.MaxValue;
                    return getDuesRefList;

                }
                else if (rptItem == "007")
                {
                    // "07. Reference wise Due Collection"  
                    var getRefWiseDueCol = Json(new { list = this.RptDisRef }, JsonRequestBehavior.AllowGet);
                    getRefWiseDueCol.MaxJsonLength = int.MaxValue;
                    return getRefWiseDueCol;
                }
                else if (rptItem == "011")
                {
                    // "11. Date Wise Collection"   
                    var getDateWiseColl = Json(new { list = this.RptDateWise }, JsonRequestBehavior.AllowGet);
                    getDateWiseColl.MaxJsonLength = int.MaxValue;
                    return getDateWiseColl;
                }
                else {
                    return Json("there are some problems", JsonRequestBehavior.AllowGet);
                }

                #region //this is report section

                if (fRe.reportType != null) {

                }
                #endregion

            }
            else {
                var st = "inpute data is In valide try again";
                return Json(st, JsonRequestBehavior.AllowGet);
            }
            

           
        }

        //helper method for return Report List
        public List<GetReportList> GetReportList() {
            List<GetReportList> Items = new List<GetReportList>();
           
            Items.Add(new GetReportList() { Text = "TRANCATION DETAILS", Value = " " });
            Items.Add(new GetReportList() { Text = "01. Sales Invoice List", Value = "001" });
            Items.Add(new GetReportList() { Text = "02. Invoice Wise Sales Details", Value = "002" });
            Items.Add(new GetReportList() { Text = "03. Group Wise Sales Details", Value = "003" });
            Items.Add(new GetReportList() { Text = "04. CC Charge Details", Value = "004" });
            Items.Add(new GetReportList() { Text = "05. Discount Reference List", Value = "005" });
            Items.Add(new GetReportList() { Text = "06. Dues Reference List", Value = "006" });
            Items.Add(new GetReportList() { Text = "07. Reference wise Due collection", Value = "007" });

            Items.Add(new GetReportList() { Text = "", Value = "" });
            Items.Add(new GetReportList() { Text = "SUMMERY REPORTS", Value = " " });
            Items.Add(new GetReportList() { Text = "01. Group Sales Summery", Value = "008" });  //07  //09
            Items.Add(new GetReportList() { Text = "02. Collection Due Summery", Value = "009" }); //08
            Items.Add(new GetReportList() { Text = "03. Invoice Wise Summery", Value = "010" }); //09
            Items.Add(new GetReportList() { Text = "04. Date Wise Collection", Value = "011" }); //10 //012
            return Items;
        }

        //this method return me DepertmentList
        protected List<GetDeparttList> GetDeparttList()
        {
            //calMonth1.EndDate = DateTime.Now;
            //calMonth2.EndDate = DateTime.Now;
            ////     calMonth1.SelectedDate = DateTime.Now;
            //this.fromDate = DateTime.Today.ToString("dd-MMM-yyy");
            //this.todate = DateTime.Today.ToString("dd-MMM-yyyy");
            List<GetDeparttList> ItemsDep = new List<GetDeparttList>();
            var lockList1 = AspSession.Current.CompInfList[0].BranchList.FindAll(x => x.brncod.Substring(2, 2) != "00").ToList();
            ItemsDep.Add(new GetDeparttList() { Text = "ALL BRANCHES", Value = "0000" });
            foreach (var item in lockList1)
            {
                ItemsDep.Add(new GetDeparttList() { Text = item.brnnam, Value = item.brncod });
            }

            return ItemsDep;

        }
        #endregion

        #region TRANSACTION DETAILS
        public String getTransInvList()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            data = data + @"<div class='JStableOuter' ><table id='mytable' class='table table-bordered table-hover Atable'>
                          <thead>  <tr class='TransInvRow' style='top: 0px; ' >
                                <th>SL</th>                             
                                <th>Date & Time</th>
                                <th>Invoice</th>
                                <th>User Name</th>
                                <th>Terminal</th>
                                <th>Description</th>  

                                <th>Item Qty</th>
                             <!-- <th>Total Amount</th> -->
                                <th>Discount Amount</th> 
                             <!-- <th>Net Amount</th>   -->
                             <!-- <th>Vat Amount</th>   -->
                                <th>Total Bill</th>
                                <th>Collection Amount</th>
                                <th>Due Amount</th>
                            </tr></thead><tbody>";
            string brnid1 = "xxxxxxxxxxxx";
            string ptinvdatOld = "xxxxxxxxxxxx";

            foreach (var item in this.RptFrontDesk1)
            {
                if (brnid1 != item.brncod)
                {
                    data += @"<tr><td colspan = '11'><b>" + @item.brnnam + "  (From " + this.fromDate + " to " + this.todate + ")" + "</b></td></tr>";
                    //data += @"<tr class=''><td colspan = '11'><b>" + @item.brnnam + this.RptListd[0]. + "</b></td></tr>";
                }
                brnid1 = item.brncod;
                string ptinvdat2 = item.ptinvdat.ToString("dd-MMM-yyyy hh:mmtt");
                if (ptinvdatOld == item.ptinvdat.ToString("dd-MMM-yyyy hh:mmtt"))
                    ptinvdat2 = "";
                else
                    ptinvdatOld = ptinvdat2;




                data += @"<tr><td class='text-right'>" + item.slnum.ToString("0#'.';(#,##0); ")// "0#'.'") //SL
                     + "</td><td class='text-nowrap'>" + ptinvdat2 //Date & Time             
                     + "</td><td class='text-nowrap'>" + (item.slnum == 0 ? "" : "<abbr title='Ref- " + item.rfFullName.ToString() + "' > ")
                     + "<a href='InvoiceWiseRpt?memoNum=" + item.ptinvnum + "&MemoDate=" + item.ptinvdat.ToString("dd-MMM-yyyy")
                     //+ "<a href='InvoiceWiseRpt?memoNum=" + item.ptinvnum + "&MemoDate=" + item.ptinvdat.ToString("dd-MMM-yyyy") 
                     + "' target='_blank'>" + item.ptinvnum2.ToString() + "</a></abbr>"  //Invoice
                     + "</td><td>" + item.signinnam.ToString() //User Name
                     + "</td><td>" + item.preparetrm.ToString() //Terminal               
                     + "</td><td>" + item.ptname.ToString()  //Description

                     + "</td><td class='text-right'>" + item.titemqty.ToString("##")     // Item Qty
                                                                                         //  + "</td><td " + styler1 + ">" + item.titmam.ToString("#,##0.00;-#,##0.00; ")       // Total Amount
                     + "</td><td class='text-right'>" + item.tidisam.ToString("#,##0.00;-#,##0.00; ")      // Discount Amount
                                                                                                           //  + "</td><td " + styler1 + ">" + item.tinetam.ToString("#,##0.00;-#,##0.00; ")      // Net Amount
                                                                                                           //  + "</td><td " + styler1 + ">" + item.tivatam.ToString("#,##0.00;-#,##0.00; ")      // Vat Amount
                     + "</td><td class='text-right'>" + item.tbillam.ToString("#,##0.00;-#,##0.00; ")      // Total Bill 
                     + "</td><td class='text-right'>" + item.tbilcolam.ToString("#,##0.00;-#,##0.00; ")    // Collection Amount
                     + "</td><td class='text-right'>" + item.tdueam.ToString("#,##0.00;-#,##0.00; ")       // Due amount              
                    + "</td></tr>";
            }
            var titemqty = this.RptFrontDesk1.Sum(x => x.titemqty).ToString("##");
            var tidisam = this.RptFrontDesk1.Sum(x => x.tidisam).ToString("#,##0.00;-#,##0.00; ");
            var tbillam = this.RptFrontDesk1.Sum(x => x.tbillam).ToString("#,##0.00;-#,##0.00; ");
            var tbilcolam = this.RptFrontDesk1.Sum(x => x.tbilcolam).ToString("#,##0.00;-#,##0.00; ");
            var tdueam = this.RptFrontDesk1.Sum(x => x.tdueam).ToString("#,##0.00;-#,##0.00; ");

            data += @"<tr><td colspan='6'>Grand Total : </td><td>" + titemqty + "</td>" +
              "<td>" + tidisam + " </td>" +
              "<td class='text-right'> " + tbillam + "</td>" +
              "<td class='text-right'> " + tbilcolam + "</td>" +
              "<td class='text-right'> " + tdueam + "</td>" +
               "</tr>";
            data += "</tbody></table></div>";
            return data;
        }

        public String getDiscRefList()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            //  string stylel1 = "style = 'text-align:left; padding-bottom:5px; padding-top:5px; font-size:11px ; min-width:130px'";
            // string styler1 = "style = 'text-align:right; padding-bottom:5px; padding-top:5px; font-size:12px;'";
            // string styler2 = "style = 'text-align:right; color:blue ; padding-bottom:5px; padding-top:5px; font-size:14px;'";
            data = data + @"<tr><td  colspan='11'><b> " + "DISCOUNT INFORMATION DETAILS - " + "(From " + this.fromDate + " to " + this.todate + ")" + " </b></td></tr>";
            data = data + @"<div class='JStableOuter'><table id='mytable' class='table table-bordered table-hover Atable'>
                            <thead>  <tr class='TransInvRow' style='top: 0px; '>
                                <th>Date & Time</th>                             
                                <th>User Name</th>
                                <th>Invoice No</th>
                                <th>Patient Name</th>
                                <th>Total Amount</th>
                                <th>Discount Amount</th>
                                <th>Net Bill Amount</th>

                                <th>New Coll. Amount</th>
                                <th>Due Amount</th>

                                <th>Discount Reference</th>
                            </tr></thead><tbody>";
            string brnid1 = "xxxxxxxxxxxx";

            //  <th>Last Coll. Date & Time</th>
            // <th>Delay (Pay)</th>
            foreach (var item in RptDisRef)
            {

                if (brnid1 != item.terminalid)
                {
                    data += @"<tr><td colspan = '12'><b>" + "Terminal : " + item.terminalid + " - " + item.brnnam + "<b></td></tr>";

                }
                brnid1 = item.terminalid;


                data += @"<tr><td class='text-nowrap'>" + item.invdat.ToString("dd-MMM-yyyy hh:mm tt") + "</td>" +
                        "<td>" + item.username + "</td>" +
                    //   "<td>" + item.ptinvnum2 + "</td>" +
                    " <td class='text-nowrap'><u><a href='InvoiceWiseRpt?memoNum=" + item.ptinvnum + "&MemoDate=" + item.invdat.ToString("dd-MMM-yyyy")

                         + "' target='_blank'>" + item.ptinvnum2.ToString() + "</u></a></td>" +
                        "<td>" + item.ptname + "</td>" +
                        "<td class='text-right'>" + item.totam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        "<td class='text-right'> " + item.disam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        "<td class='text-right'>" + item.netam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        //   "<td " + styler1 + ">" + item.coldat.ToString("dd-MMM-yyyy hh:mm tt") + "</td>" +
                        "<td class='text-right'>" + item.collam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        "<td class='text-right'>" + item.dueam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        //   "<td " + styler1 + ">" + item.daydiff + "</td>" +
                        "<td>" + item.ptinvnote + "</td></tr>";


            }

            var totam = this.RptDisRef.Sum(x => x.totam).ToString("#,##;-#,##0.00; ");
            var disam = this.RptDisRef.Sum(x => x.disam).ToString("#,##;-#,##0.00; ");
            var netam = this.RptDisRef.Sum(x => x.netam).ToString("#,##;-#,##0.00; ");

            var collam = this.RptDisRef.Sum(x => x.collam).ToString("#,##;-#,##0.00; ");
            var dueam = this.RptDisRef.Sum(x => x.dueam).ToString("#,##;-#,##0.00; ");

            data += @"<tr><td colspan='2'>" + "Record " + "</td>" +
                "<td>" + "" + "</td>" +
                "<td class='text-right'>" + " Grand Total" + "</td>" +
                "<td class='text-right'>" + totam + "</td>" +
                "<td class='text-right'>" + disam + "</td>" +
                "<td class='text-right'>" + netam + "</td>" +

                "<td class='text-right'>" + collam + "</td>" +
                "<td class='text-right'>" + dueam + "</td>" +

                "<td>" + "  " + "</td></tr>";

            data += "</tbody></table></div>";
            return data;
        }
        public String getDuesRefList()
        {

            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            data = data + @"<tr><td  colspan='11'><b> " + "DUES INFORMATION DETAILS - " + "(From " + this.fromDate + " to " + this.todate + ")" + " </b></td></tr>";
            data = data + @"<div class='JStableOuter' ><table id='mytable' class='table table-bordered table-hover Atable'>
                           <thead>  <tr class='TransInvRow' style='top: 0px; '>
                                <th>Date & Time</th>                             
                                <th>User Name</th>
                                <th>Invoice</th>
                                <th>Patient Name</th>
                                <th>Total Amount</th>
                                <th>Discount Amount</th>
                                <th>Net Bill Amount</th>
                               
                                <th>New Coll. Amount</th>
                                <th>Due Amount</th>
                               
                                <th>Discount Reference</th>
                            </tr></thead><tbody>";
            string brnid1 = "xxxxxxxxxxxx";
            // <th>Last Coll. Date & Time</th>
            // <th>Delay (Pay)</th>
            //  string stylel1 = "style = 'text-align:left; padding-bottom:5px; padding-top:5px; font-size:11px ; min-width:130px'";
            //  string styler1 = "style = 'text-align:right; padding-bottom:5px; padding-top:5px; font-size:12px;'";
            // string styler2 = "style = 'text-align:right; color:blue ; padding-bottom:5px; padding-top:5px; font-size:14px;'";
            foreach (var item in RptDisRef)
            {

                if (brnid1 != item.terminalid)
                {
                    data += @"<tr><td colspan = '12'><b>" + "Terminal : " + item.terminalid + " - " + item.brnnam + "<b></td></tr>";

                }
                brnid1 = item.terminalid;


                data += @"<tr><td class='text-nowrap'>" + item.invdat.ToString("dd-MMM-yyyy hh:mm tt") + "</td>" +
                        "<td>" + item.username + "</td>" +
                    //   "<td>" + item.ptinvnum2 + "</td>" +
                    " <td class='text-nowrap'><u><a href='InvoiceWiseRpt?memoNum=" + item.ptinvnum + "&MemoDate=" + item.invdat.ToString("dd-MMM-yyyy")

                         + "' target='_blank'>" + item.ptinvnum2.ToString() + "</u></a></td>" +
                        "<td>" + item.ptname + "</td>" +
                        "<td class='text-right'>" + item.totam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        "<td class='text-right'>" + item.disam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        "<td class='text-right'>" + item.netam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        //       "<td>" + item.coldat.ToString("dd-MMM-yyyy hh:mm tt") + "</td>" +
                        "<td class='text-right'>" + item.collam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        "<td class='text-right'>" + item.dueam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        //      "<td>" + item.daydiff + "</td>" +
                        "<td>" + item.ptinvnote + "</td></tr>";


            }

            var totam = this.RptDisRef.Sum(x => x.totam).ToString("#,##;-#,##0.00; ");
            var disam = this.RptDisRef.Sum(x => x.disam).ToString("#,##;-#,##0.00; ");
            var netam = this.RptDisRef.Sum(x => x.netam).ToString("#,##;-#,##0.00; ");

            var collam = this.RptDisRef.Sum(x => x.collam).ToString("#,##;-#,##0.00; ");
            var dueam = this.RptDisRef.Sum(x => x.dueam).ToString("#,##;-#,##0.00; ");

            data += @"<tr><td colspan='2' class='text-right'>" + "Record " + "</td>" +
                "<td>" + "" + "</td>" +
                "<td class='text-right'>" + " Grand Total" + "</td>" +
                "<td class='text-right'>" + totam + "</td>" +
                "<td class='text-right'>" + disam + "</td>" +
                "<td class='text-right'>" + netam + "</td>" +
                //    "<td>" + "  " + "</td>" +
                "<td class='text-right'>" + collam + "</td>" +
                "<td class='text-right'>" + dueam + "</td>" +
                //      "<td>" + "  " + "</td>" +
                "<td>" + "  " + "</td></tr>";

            data += "</tbody></table></div>";
            return data;
        }
        public String getRefWiseDueCol()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            // string stylel1 = "style = 'text-align:left; padding-bottom:5px; padding-top:5px; font-size:11px ; min-width:130px'";

            // string styler1 = "style = 'text-align:right; padding-bottom:5px; padding-top:5px; font-size:12px; width: 50px;'";
            // string styler2 = "style = 'text-align:right; color:blue ; padding-bottom:5px; padding-top:5px; font-size:14px;'";
            data = data + @"<tr><td  colspan='11'><b> " + "REFERENCE WISE DUE COLLECTION - " + "(From " + this.fromDate + " to " + this.todate + ")" + " </b></td></tr>";
            data = data + @"<div class='JStableOuter'><table id='mytable' class='table table-bordered table-hover Atable'>
                             <thead>  <tr class='TransInvRow' style='top: 0px; '>
                                <th>Date & Time</th>                             
                                <th>User Name</th>
                                <th>Invoice </th>
                                <th>Patient Name</th>
                                <th>Total Amnt</th>
                                <th>Dis Amnt</th>
                                <th>Net Bill Amnt</th>
                               
                                <th>New Coll. Amt</th>
                                <th>Due Amount</th>
                                
                                <th>Dis. Ref.</th>
                            </tr></thead><tbody>";
            string brnid1 = "xxxxxxxxxxxx";

            //  <th>Last Coll. Date & Time</th>
            // <th>Delay (Pay)</th>
            foreach (var item in RptDisRef)
            {

                if (brnid1 != item.brncod)
                {
                    data += @"<tr><td style='color:blue' colspan = '10'><b>" + item.brnnam + "<b></td></tr>";

                }

                brnid1 = item.brncod;
                data += @"<tr><td class='text-nowrap'>" + item.invdat.ToString("dd.MM.yyyy hh:mm tt") + "</td>" +
                        "<td>" + item.username + "</td>" +
                    //  "<td>" + item.ptinvnum2 + "</td>" +
                    " <td class='text-nowrap'><u><a href='InvoiceWiseRpt?memoNum=" + item.ptinvnum + "&MemoDate=" + item.invdat.ToString("dd-MMM-yyyy")

                         + "' target='_blank'>" + item.ptinvnum2.ToString() + "</u></a></td>" +
                        "<td>" + item.ptname + "</td>" +
                        "<td class='text-right'>" + item.totam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        "<td class='text-right'>" + item.disam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        "<td class='text-right'>" + item.netam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        //   "<td " + styler1 + ">" + item.coldat.ToString("dd-MMM-yyyy hh:mm tt") + "</td>" +
                        "<td class='text-right'>" + item.collam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        "<td class='text-right'>" + item.dueam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        //   "<td " + styler1 + ">" + item.daydiff + "</td>" +
                        "<td>" + item.ptinvnote + "</td></tr>";


            }



            var totam = this.RptDisRef.Sum(x => x.totam).ToString("#,##;-#,##0.00; ");
            var disam = this.RptDisRef.Sum(x => x.disam).ToString("#,##;-#,##0.00; ");
            var netam = this.RptDisRef.Sum(x => x.netam).ToString("#,##;-#,##0.00; ");

            var collam = this.RptDisRef.Sum(x => x.collam).ToString("#,##;-#,##0.00; ");
            var dueam = this.RptDisRef.Sum(x => x.dueam).ToString("#,##;-#,##0.00; ");

            data += @"<tr><td colspan='2' class='text-right'>" + "Record " + "</td>" +
                "<td>" + "" + "</td>" +
                "<td class='text-right'>" + " Grand Total" + "</td>" +
                "<td class='text-right'>" + totam + "</td>" +
                "<td class='text-right'>" + disam + "</td>" +
                "<td class='text-right'>" + netam + "</td>" +
                "<td class='text-right'>" + collam + "</td>" +
                "<td class='text-right'>" + dueam + "</td>" +
                "<td>" + "  " + "</td></tr>";

            data += "</tbody></table></div>";
            return data;
        }
        public String getCCchargeDetails()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            //     string stylel1 = "style = 'text-align:left; padding-bottom:5px; padding-top:5px; font-size:11px ; min-width:130px'";
            //    string styler1 = "style = 'text-align:right; padding-bottom:5px; padding-top:5px; font-size:12px;'";
            //   string styler2 = "style = 'text-align:right; color:blue ; padding-bottom:5px; padding-top:5px; font-size:14px;'";
            data += "<tr><td  colspan='12'><b> " + "CC CHARGE DETAILS - " + "(From " + this.fromDate + " to " + this.todate + ")" + " </b></td></tr>";
            data += "<div class='JStableOuter' ><table id='mytable' class='table table-bordered table-hover Atable'>" +
                            "<thead>  <tr class='TransInvRow' style='top: 0px; '>" +
                                "<th>Date & Time</th>" +
                                "<th>User Name</th>" +
                                "<th>Invoice No</th> " +
                                "<th>Patient Name</th> " +
                                "<th>CC Amount</th> " +
                                "<th>CC Paid Amount</th> " +
                                "<th>Balance Amount</th> " +
                                "</tr></thead><tbody>";
            string brnid1 = "xxxxxxxxxxxx";


            foreach (var item in RptListColSum1)
            {

                if (brnid1 != item.terminalid)
                {
                    data += "<tr><td colspan = '7'><b>" + "Terminal : " + item.terminalid + " - " + item.brnnam + "<b></td></tr>";

                }
                brnid1 = item.terminalid;


                data += "<tr><td class='text-nowrap'>" + item.invdat.ToString("dd-MM-yy hh:mm tt") + "</td>" +
                        "<td>" + item.username + "</td>" +
                    //   "<td>" + item.ptinvnum2 + "</td>" +
                    " <td class='text-nowrap'><u><a href='InvoiceWiseRpt?memoNum=" + item.ptinvnum + "&MemoDate=" + item.invdat.ToString("dd-MMM-yyyy")

                         + "' target='_blank'>" + item.ptinvnum2.ToString() + "</u></a></td>" +
                        "<td>" + item.ptname + "</td>" +
                        "<td class='text-right'>" + item.totam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                        "<td class='text-right'>" + item.ncolam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +

                        "<td class='text-right'>" + item.ocolam.ToString("#,##0.00;-#,##0.00; ") + "</td></tr>";


            }
            // var trows -this.RptDisRef.Count(x => x.ptinvnum.).ToString();
            var totam = this.RptListColSum1.Sum(x => x.totam).ToString("#,##;-#,##0.00; ");
            var ncolam = this.RptListColSum1.Sum(x => x.ncolam).ToString("#,##;-#,##0.00; ");
            var ocolam = this.RptListColSum1.Sum(x => x.ocolam).ToString("#,##;-#,##0.00; ");

            data += "<tr><td colspan='2'>" + "Record " + "</td>" +
                "<td colspan='2 class='text-right''>" + " Grand Total" + "</td>" +
                "<td class='text-right'>" + totam + "</td>" +
                "<td class='text-right'>" + ncolam + "</td>" +
                "<td class='text-right'>" + ocolam + "</td></tr>";

            data += "</tbody></table></div>";
            return data;
        }

        #endregion


        #region SUMMERY REPORTS
        public String getGropSalesSumm()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            string brnid1 = "xxxxxxxxxxxx";
            data = data + @"<tr><td  colspan='11'><b> " + "Group Sales Summery - " + "(From " + this.fromDate + " to " + this.todate + ")" + " </b></td></tr>";

            data = data + @"<div class='JStableOuter'><table id='mytable' class='table table-bordered table-hover Atable'>
                          <thead>  <tr class='TransInvRow' style='top: 0px; '>
                            <th>SL</th>                             
                            <th>Item Name & Trancsaction Description</th>
                            <th>Quantity</th>      
                            <th>Amount</th>      
                            <th>Discount</th>      
                            <th>Net Amount</th>      
                        </tr></thead><tbody>";

            foreach (var item in this.RptListc)
            {
                //if (brnid1 != this.RptFrontDesk1[0].brncod)
                //{
                //    //data += @"<tr class=''><td colspan = '11'><b>" + "Group Sales Summery " + "  (From " + this.fromDate + " to " + this.todate + ")" + "</b></td></tr>";
                //   // data += @"<tr class=''><td colspan = '11'><b>" + @item.slnum + this.RptListd[0]. + "</b></td></tr>";
                //}
                //brnid1 = this.RptFrontDesk1[0].brncod;
                //  string rowid = (item.trcode.Trim().Contains(item.trcode) ? "" : "" ) ;
                string color1 = (item.trdesc.Trim().Contains("GRAND ") ? "Blue" : "Black");
                color1 = "style = 'color:" + color1 + "; '";
                //string styleL1 = "style = 'text-align:left; color:" + color1 + ";  padding-bottom:5px; padding-top:5px; font-size:12px'";
                //string styleR1 = "style = 'text-align:right; color:" + color1 + "; padding-bottom:5px; padding-top:5px; font-size:12px'";
                // string stylec1 = "style = 'text-align:center; padding-bottom:5px; padding-top:5px; font-size:12px'";
                data += @"<tr>
                        <td colspan='2' " + color1 + ">" + item.slnum.ToString(" ") + item.trdesc
                           + "</td><td " + color1 + " class='text-right'>" + item.itemqty.ToString("##")
                           + "</td><td " + color1 + " class='text-right'>" + item.titmam.ToString("#,##0.00;-#,##0.00; ")
                           + "</td><td " + color1 + " class='text-right'>" + item.idisam.ToString("#,##0.00;-#,##0.00; ")
                           + "</td><td " + color1 + " class='text-right'>" + item.inetam.ToString("#,##0.00;-#,##0.00; ")
                           + "</td></tr>";
            }
            data += "</tbody></table></div>";
            return data;
        }

        public String getInvSumm()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";

            //data = data + @"<tr><td  colspan='11'><b> " + "SALES & COLLECTION SUMMERY - " + "(From " + this.fromDate + " to " + this.todate + ")" + " </b></td></tr>";
            //data = data + @"<div class='JStableOuter' ><table id='mytable' class='table table-bordered table-hover Atable'>
            //                <thead>  <tr class='TransInvRow' style='top: 0px; ' >      
            //                    <th>Collection Date & Time</th>                             
            //                    <th>Date & Time</th>
            //                    <th>User Name</th>
            //                    <th>Invoice</th>
            //                    <th>Patient Name</th>
            //                    <th>Total Amount</th>
            //                    <th>Discount Name</th>  

            //                    <th>Net Bill Amount</th>
            //                    <th>New Coll. Amount</th>
            //                    <th>Due Coll. Amount</th>
            //                </tr></thead><tbody>";
            //string brnid1 = "xxxxxxxxxxxx";

            //foreach (var item in this.RptListColSum1)
            //{
            //    if (brnid1 != item.terminalid)
            //    {

            //        data += @"<tr><td colspan = '2'><b>" + item.terminalid + " - " + item.brnnam + "</b></td>" +
            //            "<td colspan = '3'></td>" +
            //            "<td> " + this.RptListColSum1[2].totam + " </td>" +
            //            "<td></td>" +
            //            "<td></td>" +
            //            "<td></td>" +
            //            "<td></td>" +
            //            "</tr>";
            //    }
            //    brnid1 = item.terminalid;
            //    data += @"<tr><td class='text-nowrap'>" + item.coldat.ToString("dd.MM.yy hh:mm tt") //Collection Date               
            //         + "</td><td class='text-nowrap'>" + item.invdat.ToString("dd.MM.yy hh:mm tt")  //Invoice Date
            //         + "</td><td>" + item.username.ToString()  //Invoice Date
            //                                                   //+ "</td><td " + stylel2 + ">" + item.ptinvnum2.ToString()  //Invoice No

            //         + "</td><td class='text-nowrap'><u><a href='InvoiceWiseRpt?memoNum=" + item.ptinvnum + "&MemoDate=" + item.invdat.ToString("dd-MMM-yyyy")

            //         + "' target='_blank'>" + item.ptinvnum2.ToString() + "</u></a>"  //Invoice
            //         + "</td><td>" + item.ptname.ToString()      // Patient Name Amount      
            //         + "</td><td class='text-right'>" + item.totam.ToString("#,##0.00;-#,##0.00; ")      // Total Amount      
            //         + "</td><td class='text-right'>" + item.disam.ToString("#,##;-#,##0.00; ")    // Discount Amount
            //         + "</td><td class='text-right'>" + item.netam.ToString("#,##0.00;-#,##0.00; ")       // Net Bill amount              
            //         + "</td><td class='text-right'>" + item.ncolam.ToString("#,##0.00;-#,##0.00; ")       // New Coll amount              
            //         + "</td><td class='text-right'>" + item.ocolam.ToString("#,##0.00;-#,##0.00; ")       // Due Coll amount              
            //        + "</td></tr>";


            //}


            //var totalAmount = this.RptListColSum1.Sum(x => x.totam).ToString("#,##0.00;-#,##0.00; ");
            //var discountAmt = this.RptListColSum1.Sum(x => x.disam).ToString("#,##0.00;-#,##0.00; ");
            //var netBillAmt = this.RptListColSum1.Sum(x => x.netam).ToString("#,##0.00;-#,##0.00; ");
            //var newCollAmt = this.RptListColSum1.Sum(x => x.ncolam).ToString("#,##0.00;-#,##0.00; ");
            //var dueCollAmt = this.RptListColSum1.Sum(x => x.ocolam).ToString("#,##0.00;-#,##0.00; ");
            //var totalCollection = this.RptListColSum1.Sum(x => x.ocolam + x.ncolam).ToString("#,##;-#,##0.00; ");



            //data += @"<tr><td colspan='4'></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //   "</tr>" +

            //   "<tr><td colspan='4'></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //   "</tr>" +

            //   "<tr><td colspan='4'></td>" +
            //  "<td>" + "Grand Total " + "</td>" +
            //  "<td>" + totalAmount + "</td>" +
            //  "<td>" + discountAmt + "</td>" +
            //  "<td>" + netBillAmt + "</td>" +
            //  "<td>" + newCollAmt + "</td>" +
            //  "<td>" + dueCollAmt + "</td>" +
            //   "</tr>" +

            //   "<tr><td> </td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //  "<td></td>" +
            //  "<td colspan='6'>" + "TOTAL COLLECTION = " + totalCollection + "</td>" +
            //   "</tr>";
            //data += "</tbody></table></div>";
            return data;
        }
        public String getDateWiseColl()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            data = data + @"<tr><td  colspan='11'><b> " + "DATE WISE SALES & COLLECTION SUMMERY - " + "(From " + this.fromDate + " to " + this.todate + ")" + " </b></td></tr>";
            data = data + @"<div class='JStableOuter' ><table id='mytable' class='table table-bordered table-hover Atable'>
                             <thead>  <tr class='TransInvRow' style='top: 0px; '>
                                <th>Description</th>                             
                                <th>New Invoice Quantity</th>
                                <th>Gross Sales Amount</th>
                                <th>Discount Amount</th>
                                <th>Net Sales Amount</th>
                                <th>Collecton Amount</th>
                                <th>Due Amoount</th>
                            </tr></thead><tbody>";
            string brnid1 = "xxxxxxxxxxxx";
            foreach (var item in RptDateWise)
            {
                if (brnid1 != item.grp1cod)
                {
                    data += @"<tr><td colspan = '7'><b>" + item.grp1desc + "<b></td></tr>";
                }

                brnid1 = item.grp1cod;
                data += @"<tr><td>" + item.grp2desc + "</td>" +
                    "<td class='text-right'>" + item.invqty.ToString("##") + "</td>" +
                    "<td class='text-right'>" + item.gsalam.ToString("#,##;-#,##0.00; ") + "</td>" +
                    "<td class='text-right'>" + item.disam.ToString("#,##;-#,##0.00; ") + "</td>" +
                    "<td class='text-right'>" + item.nsalam.ToString("#,##;-#,##0.00; ") + "</td>" +
                    "<td class='text-right'>" + item.collam.ToString("#,##;-#,##0.00; ") + "</td>" +
                    "<td class='text-right'>" + item.dueam.ToString("#,##;-#,##0.00; ") + "</td>" +
                    "</tr>";
            }

            var tinvQty = this.RptDateWise.Sum(x => x.invqty).ToString("##");
            var tgsalam = this.RptDateWise.Sum(x => x.gsalam).ToString("#,##;-#,##0.00; ");
            var tdisam = this.RptDateWise.Sum(x => x.disam).ToString("#,##;-#,##0.00; ");
            var tnsalam = this.RptDateWise.Sum(x => x.nsalam).ToString("#,##;-#,##0.00; ");
            var tcollam = this.RptDateWise.Sum(x => x.collam).ToString("#,##;-#,##0.00; ");
            var tdueam = this.RptDateWise.Sum(x => x.dueam).ToString("#,##;-#,##0.00; ");
            data += @"<tr><td>" + "Grand Total" + "</td>" +
                "<td  class='text-right'>" + tinvQty + "</td>" +
                "<td class='text-right'>" + tgsalam + "</td>" +
                "<td class='text-right'>" + tdisam + "</td>" +
                "<td class='text-right'>" + tnsalam + "</td>" +
                "<td class='text-right'>" + tcollam + "</td>" +
                "<td class='text-right'>" + tdueam + "</td></tr>";

            data += "</tbody></table></div>";
            return data;
        }
        #endregion



        private void CollectionSummary(string RptProcID1, string BrnCode1, string StartDate1, string EndDate1, string TerminalName1)
        {
            vmReportFrontDesk1 vmr = new vmReportFrontDesk1();
            var pap1 = vmr.SetParamFrontDeskSumReport(CompCode: AspSession.Current.CompInfList[0].comcpcod, ProcessID: RptProcID1, BrnchCod: BrnCode1, startDate: StartDate1, EndDate: EndDate1, Option1: TerminalName1);
            DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
            if (ds1 == null)
                return;
            if (ds1 == null)
                return;
            if (ds1.Tables[0].Rows.Count == 0)
            {
                return;
            }
            this.RptListColSum1 = ds1.Tables[0].DataTableToList<HmsEntityCommercial.FDeskCollSumm01>();
            //this.RptListColSum2 = ds1.Tables[1].DataTableToList<HmsEntityCommercial.FDeskCollSumm01>();
            //this.RptListColSum3 = ds1.Tables[2].DataTableToList<HmsEntityCommercial.FDeskCollSumm01>(); //Branch Details
            //this.RptListColSum4 = ds1.Tables[3].DataTableToList<HmsEntityCommercial.FDeskCollSumm01>(); //Branch Terminal Details
        }
        private void DayWiseSalesCollectionSummary(string RptProcID1, string BrnCode1, string StartDate1, string EndDate1, string Option1 = "ALLBRANCH")
        {
            Option1 = "ALLBRANCH";
            vmReportFrontDesk1 vmr = new vmReportFrontDesk1();

            var pap1 = vmr.SetParamFrontDeskSumReport(CompCode: AspSession.Current.CompInfList[0].comcpcod, ProcessID: RptProcID1, BrnchCod: BrnCode1, startDate: StartDate1, EndDate: EndDate1, Option1: Option1);
            DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
            if (ds1 == null)
                return;
            this.RptDateWise = ds1.Tables[0].DataTableToList<HmsEntityCommercial.FDeskSalesSumm01>(); // Date wise Collection

        }

        #region this is invoise section
        [HttpGet]
        public ActionResult InvoiceWiseRpt(invoiceDTO inv) {


            if (inv.memoNum == null)
            {
                inv.memoDate = "01-Jan-2017";
                inv.memoNum = "CSI201611110100012";
            }
            else {
                if (inv.memoNum.Length == 11)
                {
                  
                    var lockList = AspSession.Current.CompInfList[0].BranchList.FindAll(a => a.brnsnam.Trim() == inv.memoNum.ToUpper().Substring(0, 2)).ToList();
                    if (lockList.Count != 0)
                    {
                        string branch = lockList[0].brncod;
                        string yearMonth = inv.memoNum.Substring(2, 4);
                        string invNum = inv.memoNum.Substring(6, 5);

                        string transID2 = "CSI20" + yearMonth + branch + invNum;
                        string date = DateTime.Today.ToString("dd-MMM-yyyy");
                        inv.memoNum = transID2;
                       inv.memoDate = date;
                    }



                }if (inv.memoNum.Length < 11) {
                    
                    ModelState.AddModelError("", "invoice number IS Invalied");
                    return View(inv);
                }

                string BrnCode1 = "%";
                string SignInID1 = "%";
                string StartDate1 = DateTime.Parse(inv.memoDate).AddDays(-365).ToString("dd-MMM-yyy hh:mm tt");// this.xctk_dtpSrchDat1.Text; // DateTime.Today.AddDays(-60).ToString("dd-MMM-yyyy");
                string EndDate1 = DateTime.Parse(inv.memoDate).AddDays(365).ToString("dd-MMM-yyy hh:mm tt"); //this.xctk_dtpSrchDat2.Text; // DateTime.Today.ToString("dd-MMM-yyyy");
                string InvNum1 = inv.memoNum;//"CSI";
                string InvStatus1 = "A";
                string TerminalName1 = "%";
                string SessionID1 = "%";
                string OrderBy1 = "DESCENDING";
                string RptProcID1 = "COMMINVMEMO01N";
                vmReportFrontDesk1 vmr = new vmReportFrontDesk1();

                try
                {
                    var pap1 = vmr.SetParamFrontDeskReport(CompCode: AspSession.Current.CompInfList[0].comcpcod, ProcessID: RptProcID1, BrnchCod: BrnCode1, startDate: StartDate1, EndDate: EndDate1,
                                                     InvNum: InvNum1, PreparedBy: SignInID1, InvStatus: InvStatus1, TerminalName: TerminalName1, SessionID: SessionID1, OrderBy: OrderBy1);
                    //var pap1 = vmr.SetParamCommInvoice(WpfProcessAccess.CompInfList[0].comcod, memoNum);
                    DataSet ds1 = ASITHmsMvc.AspProcessAccess.GetHmsDataSet(pap1);
                    //DataSet ds1 = WpfProcessAccess.GetHmsDataSet(pap1, "JSON");
                    if (ds1 == null)

                        return View();
                    string inputSource = ds1.Tables[1].Rows[0]["preparetrm"].ToString().Trim() + ", " + ds1.Tables[1].Rows[0]["preparebynam"].ToString().Trim()
                                      + ", " + ds1.Tables[1].Rows[0]["prepareses"].ToString().Trim() + ", " + Convert.ToDateTime(ds1.Tables[1].Rows[0]["rowtime"]).ToString("dd-MMM-yyyy hh:mm:ss tt");
                    //var list3 = WpfProcessAccess.GetRptGenInfo(ServerTime: Convert.ToDateTime(ds1.Tables[1].Rows[0]["ServerTime"]), InputSource: inputSource);
                    //list3[0].RptFooter1 = list3[0].RptFooter1.Replace(" Source", "");

                    this.list1a = ds1.Tables[0].DataTableToList<HmsEntityCommercial.CommInv01.CommInv01TblItem>();  // Table Data
                    this.list1b = ds1.Tables[1].DataTableToList<HmsEntityCommercial.CommInv01.CommInv01GenInf>();  //GeneralInfo
                    this.list1c = ds1.Tables[2].DataTableToList<HmsEntityCommercial.CommInv01.CommInv01TblSum>(); // Table Summery
                    this.list1d = ds1.Tables[3].DataTableToList<HmsEntityCommercial.CommInv01.CommInv01TblCol>(); // Table Collection

                    var list1e = "";
                    if (ds1.Tables[4].Rows.Count > 0)
                    {
                        if (!(ds1.Tables[4].Rows[0]["ptphoto"] is DBNull))
                        {
                            byte[] imge1 = (byte[])ds1.Tables[4].Rows[0]["ptphoto"];
                            list1e = Convert.ToBase64String(imge1);
                        }
                    }

                    //send data in view 
                    ViewBag.lblPaymentDetails = getPaymentDetails();
                    ViewBag.lbltbl = getTableData();
                    ViewBag.lblTblsum = getTableSumData();
                    ViewBag.lblGenerelInfo = getGeneralData();
                    ViewBag.lblGenerelInfoRight = getGeneralDataRight();
                }
                catch (Exception es) {
                    ModelState.AddModelError("", "invoice is invalied");
                    return View(inv);
                }
                
            }
           
            return View(inv);
        }


        //public void printinvoice(invoiceDTO inv)
        //{

        //    string BrnCode1 = "%";
        //    string SignInID1 = "%";
        //    string StartDate1 = DateTime.Parse(inv.memoDate).AddDays(-30).ToString("dd-MMM-yyy hh:mm tt");// this.xctk_dtpSrchDat1.Text; // DateTime.Today.AddDays(-60).ToString("dd-MMM-yyyy");
        //    string EndDate1 = DateTime.Parse(inv.memoDate).AddDays(30).ToString("dd-MMM-yyy hh:mm tt"); //this.xctk_dtpSrchDat2.Text; // DateTime.Today.ToString("dd-MMM-yyyy");
        //    string InvNum1 = inv.memoNum;//"CSI";
        //    string InvStatus1 = "A";
        //    string TerminalName1 = "%";
        //    string SessionID1 = "%";
        //    string OrderBy1 = "DESCENDING";
        //    string RptProcID1 = "COMMINVMEMO01N";
        //    vmReportFrontDesk1 vmr = new vmReportFrontDesk1();
        //    var pap1 = vmr.SetParamFrontDeskReport(CompCode: AspSession.Current.CompInfList[0].comcpcod, ProcessID: RptProcID1, BrnchCod: BrnCode1, startDate: StartDate1, EndDate: EndDate1,
        //                                         InvNum: InvNum1, PreparedBy: SignInID1, InvStatus: InvStatus1, TerminalName: TerminalName1, SessionID: SessionID1, OrderBy: OrderBy1);
        //    //var pap1 = vmr.SetParamCommInvoice(WpfProcessAccess.CompInfList[0].comcod, memoNum);
        //    DataSet ds1 = ASITHmsAsp.AspProcessAccess.GetHmsDataSet(pap1);
        //    //DataSet ds1 = WpfProcessAccess.GetHmsDataSet(pap1, "JSON");
        //    if (ds1 == null)
        //        return;
        //    string inputSource = ds1.Tables[1].Rows[0]["preparetrm"].ToString().Trim() + ", " + ds1.Tables[1].Rows[0]["preparebynam"].ToString().Trim()
        //                      + ", " + ds1.Tables[1].Rows[0]["prepareses"].ToString().Trim() + ", " + Convert.ToDateTime(ds1.Tables[1].Rows[0]["rowtime"]).ToString("dd-MMM-yyyy hh:mm:ss tt");
        //    //var list3 = WpfProcessAccess.GetRptGenInfo(ServerTime: Convert.ToDateTime(ds1.Tables[1].Rows[0]["ServerTime"]), InputSource: inputSource);
        //    //list3[0].RptFooter1 = list3[0].RptFooter1.Replace(" Source", "");

        //    this.list1a = ds1.Tables[0].DataTableToList<HmsEntityCommercial.CommInv01.CommInv01TblItem>();  // Table Data
        //    this.list1b = ds1.Tables[1].DataTableToList<HmsEntityCommercial.CommInv01.CommInv01GenInf>();  //GeneralInfo
        //    this.list1c = ds1.Tables[2].DataTableToList<HmsEntityCommercial.CommInv01.CommInv01TblSum>(); // Table Summery
        //    this.list1d = ds1.Tables[3].DataTableToList<HmsEntityCommercial.CommInv01.CommInv01TblCol>(); // Table Collection

        //    var list1e = "";
        //    if (ds1.Tables[4].Rows.Count > 0)
        //    {
        //        if (!(ds1.Tables[4].Rows[0]["ptphoto"] is DBNull))
        //        {
        //            byte[] imge1 = (byte[])ds1.Tables[4].Rows[0]["ptphoto"];
        //            list1e = Convert.ToBase64String(imge1);
        //        }
        //    }

        //    //send data in view 
        //    ViewBag.lblPaymentDetails = getPaymentDetails();
        //    ViewBag.lbltbl = getTableData();
        //    ViewBag.lblTblsum = getTableSumData();
        //    ViewBag.lblGenerelInfo = getGeneralData();
        //    ViewBag.lblGenerelInfoRight = getGeneralDataRight();
        //}
        #region invoice body
        public String getGeneralData()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;

            string data = "";

            string style1 = "style = 'text-align:left; font-size:13px;'";
            string style2 = "style = 'text-align:left; font-size:11px;'";
            string style3 = "style = 'text-align:left; font-size:12px;'";

            data = data + @"<table class='table-responsive'>
                         <tr class=''><td " + style1 + " > " + "Trans.Id : " + this.list1b[0].ptinvnum2 + "</td></tr>" +
                            "<tr><td " + style2 + "> " + "Branch : " + "<b>" + this.list1b[0].brnnam + "</b></td></tr>" +
                            "<tr><td " + style2 + "> " + "Name : " + "<b>" + this.list1b[0].ptname + "</b></td></tr>" +
                            "<tr><td " + style3 + ">  " + "Age : " + this.list1b[0].ptage + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;" + "Sex : " + this.list1b[0].ptgender + "</td></tr>" +
                            "<tr><td colspan='2' " + style2 + "> " + "Ref.by : " + this.list1b[0].rfFullName + "</td></tr>" +
                            //"<tr><td> " + (this.list1b[0].ptrefnote.Trim().Length == 0 ? "" : +"Notes : " + this.list1b[0].ptrefnote.Trim()) + "</td></tr>";
                            "<tr><td " + style3 + "> " + (this.list1b[0].ptrefnote.Trim().Length == 0 ? "" : "Notes : " + this.list1b[0].ptrefnote.Trim()) + "</td></tr>";

            data += "</table>";
            return data;
        }

        public String getGeneralDataRight()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";

            string style1 = "style = 'text-align:left; font-size:13px;'";
            string style2 = "style = 'text-align:left; font-size:11px;'";
            string style3 = "style = 'text-align:left; font-size:12px;'";



            data = data + @"<table class='table-responsive'>
                         <tr><td " + style1 + ">" + "Date : " + this.list1b[0].ptinvdat.ToString("dd-MMM-yyy hh:mm tt") + "</td></tr>" +
                            "<tr><td " + style3 + "> " + "Delivery : " + this.list1b[0].delivartime.ToString("dd-MMM-yyy hh:mm tt") + "</td></tr>" +
                            "<tr><td  " + style1 + "> " + "Tel/Cell : " + this.list1b[0].ptphone + "</td></tr>";

            data += "</table>";
            return data;
        }
        public String getTableData()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";

            data = data + @"<table class=' table table-bordered  table-hover table-responsive Atable'>
                         <tr style='height:20px; font-size=12px ; background: #eeeeee;'>
                            <th class='thpayment'>SL</th>                             
                            <th class='thpayment col-md-4' > Item Description</th>
                            <th class='thpayment'>Amount(Tk.)</th>      
                        </tr>";

            foreach (var item in this.list1a)
            {
                string styleL1 = "style = 'text-align:left; padding-bottom:5px; padding-top:5px; font-size:12px'";
                string styleR1 = "style = 'text-align:right; padding-bottom:5px; padding-top:5px; font-size:12px'";

                data += @"<tr>
                        <td " + styleR1 + ">" + item.slnum.ToString("00") + "."// "0#'.'") //SL
                    + "</td><td " + styleL1 + ">" + item.isirdesc + " " + item.itemrmrk
                       //+ "(" + item.itemqty.ToString("##0;") + ")".ToString()
                       + (item.itemqty == 1 ? "" : "- Qty :" + item.itemqty.ToString("####")).ToString()
                    + "</td><td " + styleR1 + ">" + item.itmam.ToString("#,##0.00;-#,##0.00; ")
                    + "</td></tr>";
            }

            data += "</table>";
            return data;
        }

        public String getPaymentDetails()
        {
            if (AspSession.Current.AspFormsList == null)

                return null;
            string data = "";

            string style1 = "style = 'padding-bottom: 2px; padding-top: 2px; font-size:12px; '";
            string style2 = "style = 'text-align:center; min-width:80px; padding-bottom: 2px; padding-top: 2px; font-size:11px;'";
            string style3 = "style = 'text-align:center; color:red; padding-bottom: 0px; padding-top: 3px; font-size:16px; font-weight: bold'";

            string inword = "Inword : " + ASITUtility.Trans(Convert.ToDouble(list1c[0].SumAmt), 2);
            string paid = this.list1b[0].dueam > 0 ? "Due Tk. " + this.list1b[0].dueam.ToString("#,##0.00;-#,##0.00; ") : "P A I D";


            data = data + @"<table class='table-responsive Atable'>
                    <tr " + style1 + ">   " + inword + "</tr>" +
            @"<tr style='background: #eeeeee;' ><h5><i><b><u>Payment Details </u></b></i></h5>
                               <th class='thpayment' style='text-align:center;'>Date & Time</th>
                               <th class='thpayment' style='text-align:center'> Amount</th>
                               <th class='thpayment' style='text-align:center'>User</th>
          </tr>";
            foreach (var item in list1d)
            {
                data = data + @"<tr><td " + style2 + "> " + item.bcnote + "</td>" +
                           "<td " + style2 + "> " + item.bilcolam.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                           "<td " + style2 + "> " + item.preparebynam + "</td></tr>";
            }
            data = data + @"<tr><td><table class='table table hover ' border='2'><td " + style3 + ">" + paid + "</table></td></tr>";

            data += "</table>";
            return data;
        }

        public String getTableSumData()
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            data = data + @"<table class='table table-bordered'>";
            foreach (var item in this.list1c)
            {
                string color1 = (item.SumHead.Trim().Contains("Total") ? "Blue" : "Black");
                string styleL1 = "style = ' min-width:80px; text-align: left; color:" + color1 + "; padding-bottom:5px; padding-right:10px; padding-top:5px; font-size:12px'";
                string styleR1 = "style = ' text-align: right; color:" + color1 + "; padding-bottom:5px; padding-right:10px; padding-top:5px; font-size:12px'";
                data += @"<tr><td " +
                    styleL1 + ">" + item.SumHead.Trim()
                    + "</td><td " + styleR1 + ">" + item.SumAmt.ToString("#,##0.00;-#,##0.00; ")
                    + "</td></tr>";
            }
            data += "</table>";
            return data;
        }
        #endregion

        #endregion


        #region  staff attendence single
        [HttpGet]
        
        public ActionResult AttnRptSingle(EmployeSingle employeSingle)
        {
            //if (employeSingle.query != null) {
            //    UrlEncription ur = new UrlEncription();
            //    string url = ur.Decrypt(employeSingle.query); //EmpId=950100401353/&monthId=201811Time=26-Nov-18 10:03:33 AM;
            //    employeSingle.EmpId = url.Substring(6, 12);
            //    employeSingle.monthId =  url.Substring(28, 6);

            //    //get time
            //    //DateTime time1 = DateTime.Parse(url.Substring(49, 8)).AddMinutes(1);
            //    //DateTime time2 = DateTime.Now.ToLocalTime();
            //    //if ( time2 >time1 ) {
            //    //    return RedirectToAction("logout", "login");
            //    //}
                
                
            //}
            EmployeSingle es = new EmployeSingle();
            if (employeSingle.EmpId != null && employeSingle.monthId != null)
            {
                string hccode1a = "";
                string monthid1 = "";
                if (employeSingle.EmpId.Trim().Length > 6)
                {
                    hccode1a = employeSingle.EmpId.ToString().Trim();
                    monthid1 = employeSingle.monthId;
                }
                else {
                    hccode1a = "950100" + employeSingle.EmpId.ToString().Trim();
                    

                    string monthName1 = employeSingle.monthId;
                    DateTime Date1 = DateTime.Parse("01" + monthName1);
                    monthid1 = Date1.ToString("yyyyMM");
                }

                
                
           

                ViewBag.lbltbl1 = getAttendantData_2(hccode1a, monthid1);


                List<time> items = new List<time>();
                for (int i = -12; i < 1; i++)
                {
                    items.Add(new time() { timeId = i, datetime = DateTime.Now.AddMonths(i).ToString("MMMM, yyyy") });
                }

                ViewBag.ddlType = new SelectList(items.OrderByDescending(a => a.timeId), "datetime", "datetime");
                ViewBag.mount = monthid1;
                ViewBag.id = hccode1a;
                return View(es);
            }
            else
            {

                ViewBag.error = "invalid input";
                List<time> items = new List<time>();
                for (int i = -12; i < 1; i++)
                {
                    items.Add(new time() { timeId = i, datetime = DateTime.Now.AddMonths(i).ToString("MMMM, yyyy") });
                }
                
                ViewBag.ddlType = new SelectList(items.OrderByDescending(a=>a.timeId), "datetime", "datetime");

                
                return View(es);
            }
        }

        


        //print the single employe
        public String getAttendantData_2(string hccode1a = "", string monthid1 = "")
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            this.GetAttendancel(hccode1a, monthid1);
            //var n = AspSession.Current.AspFormsList;
            //string data = "";
            string data = @"<table class='table table-bordered Atable'>
                            <tr>
                                <th>Day</th>
                                <th>Attendent Reports</th>
                                <th>Status</th>         
                                <th>Sch Hour</th>
                                <th>Act Hour</th>
                                <th>Office Time</th>
                                
                            </tr>";
            //this.GetAttendancel();
            //int x = (data.Length > 0 ? 1 : 0);



            foreach (var item in this.Rptlst1)
            {
                string color1 = (item.attnrmrk.Trim().Contains("Day off") ? "Blue" : "Black");
                string styleR1 = "style = ' text-align: left; color:" + color1 + "; font-weight:bold; font-size:12px'";

                if (item.schworkhr == 0)
                {
                    data += @"<tr><td>"
                    + item.attndate.ToString("dd.ddd")
                    + "</td><td " + styleR1 + ">" + item.attnrmrk
                    + "</td><td " + styleR1 + ">" + item.attnstat
                    + "</td><td>" + item.schworkhr.ToString("##0.00;-##0.00; ")
                    + "</td><td>" + item.actworkhr.ToString("##0.00;X; ")
                    + "</td><td " + styleR1 + " class='txtCenter'>" + item.attnstat
                    + "</td></tr>";
                }
                else
                {
                    data += @"<tr><td>"
                   + item.attndate.ToString("dd.ddd")
                   + "</td><td>" + item.attnrmrk
                   + "</td><td>" + item.attnstat
                   + "</td><td>" + item.schworkhr.ToString("##0.00;-##0.00; ")
                   + "</td><td>" + item.actworkhr.ToString("##0.00;X; ")
                   + "</td><td>" + item.intime1.ToString("hh:mm tt")
                   + " - " + item.outtime1.ToString("hh:mm tt")
                   + (item.outtime1 == item.intime2 ? "" :
                   " / " + item.intime2.ToString("hh:mm tt")
                   + " - " + item.outtime2.ToString("hh:mm tt")) + "</td></tr>";
                }
            }
            data += "</table>";
            return data;
        }


        protected void GetAttendancel(string hccode1a = "", string monthid1 = "")
        {
            vmEntryAttnLeav1 vm2 = new vmEntryAttnLeav1();
            var pap1r = vm2.SetParamShowScheduledAttnInfo1(AspSession.Current.CompInfList[0].comcpcod, monthid1, hccode1a, "PRINT");
            DataSet ds1r = AspProcessAccess.GetHmsDataSet(pap1r);
            if (ds1r == null)
                return;

            if (ds1r.Tables[0].Rows.Count == 0)
            {
                return;
            }
            string RptType = "Attendance";

            List<HmsEntityManpower.RptAttnSchInfo> Rptlst = ASITHmsMvc.HcmGeneralClass1.GetIndRosterAttendance(monthid1: monthid1, hccode1a: hccode1a, RptType: RptType);
            ViewBag.Rptlst = Rptlst;
            if (Rptlst == null)
            {
                return;
            }
            this.Rptlst1 = Rptlst;

            vmReportHCM1 vmr1 = new vmReportHCM1();
            vmEntryHRGenral1 vm1 = new vmEntryHRGenral1();

            var pap1 = vmr1.SetHRMList(AspSession.Current.CompInfList[0].comcpcod, hccode1a, "EXISTSTAFFS");
            DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
            if (ds1 == null)
                return;

            var Staff1 = ds1.Tables[0].DataTableToList<vmReportHCM1.Stafflist>();

            decimal sumLate1 = Rptlst.Sum(x => x.confirmlate);
            string Notes1 = (sumLate1 > 0 ? "Late Point = " + sumLate1.ToString("##") : "");

            decimal sumEout1 = Rptlst.Sum(x => x.confirmearly);
            Notes1 = Notes1 + (Notes1.Length > 0 && sumEout1 > 0 ? ", " : "") + (sumEout1 > 0 ? "Early Out Point = " + sumEout1.ToString("##") : "");
            Notes1 = (Notes1.Length > 0 ? "Confirm " : "") + Notes1;

            var pap2 = vm1.SetParamShowHCInfo(AspSession.Current.CompInfList[0].comcpcod, hccode1a, "PHOTO");
            DataSet dss2 = AspProcessAccess.GetHmsDataSet(pap2);
            if (dss2 == null)
                return;

            byte[] bytes12 = null;
            if (!(dss2.Tables[0].Rows[0]["hcphoto"] is DBNull))
            {
                bytes12 = (byte[])dss2.Tables[0].Rows[0]["hcphoto"];

            }
            String imgStr1 = (bytes12 == null ? "" : Convert.ToBase64String(bytes12));
            ViewBag.img1 = "data:image/jpg;base64," + imgStr1;

            ViewBag.lblName = "Employee : " + hccode1a.Substring(6) + " " + " - " + Staff1[0].hcname.Trim() + ", " + Staff1[0].designame.Trim();
            ViewBag.lblDpName = "Department: " + Staff1[0].deptname.Trim() + ", Joining Date : " + Staff1[0].joindat.Trim() +
                                       ", Reporting Date : " + Convert.ToDateTime(ds1r.Tables[2].Rows[0]["ServerTime"]).ToString("dd-MMM-yyyy hh:mm tt");
            string[] mn1 = { "", "January", "February", "March", "Aprill", "May", "Jun", "July", "August", "September", "October", "November", "December" };
            string monthName1 = mn1[int.Parse(monthid1.Substring(4, 2))] + ", " + monthid1.Substring(0, 4);
            ViewBag.lblAtten = (RptType == "Attendance" ? "Attendence" : "Duty Roster") + " - " + monthName1;

            rptParam["empId"] = hccode1a;//  this.AtxtEmpAll.Value.ToString().Trim();
            rptParam["empName"] = "Employee : " + hccode1a.Substring(6, 6) + " - " + Staff1[0].hcname.Trim() + ", " + Staff1[0].designame.Trim();

            rptParam["ParmNotes1"] = Notes1;

            
        }

        #endregion

        #region  staff attendence group
        [HttpGet]
      
        public ActionResult AttnRptGroup(EmpGroup empGroup)
        {
            EmpGroup eg = new EmpGroup();
            //get tow date or nedd dropdown menu for 
            if (empGroup.type != null)
            {
                var dt1 = DateTime.Parse(empGroup.toDate);
                var dt2 = DateTime.Parse(empGroup.fromDate);

                
                string AttnDate1 = (dt1 <= dt2 ? dt1 : dt2).ToString("dd-MMM-yyyy");// this.txtMonth1.Text;
                string AttnDate2 = (dt1 > dt2 ? dt1 : dt2).ToString("dd-MMM-yyyy");// this.txtMonth2.Text;
                string monthid1 = DateTime.Parse(AttnDate1).ToString("yyyyMM");
             
                ViewBag.days = ((DateTime.Parse(AttnDate2) - DateTime.Parse(AttnDate1)).TotalDays) + 3;
                string SectCod = empGroup.type;

                SectCod = (SectCod.Substring(9, 3) == "000" ? SectCod.Substring(0, 9) : SectCod);
                SectCod = (SectCod.Length == 9 && SectCod.Substring(7, 2) == "00" ? SectCod.Substring(0, 7) : SectCod);
                SectCod = (SectCod.Length == 7 && SectCod.Substring(4, 3) == "000" ? SectCod.Substring(4, 3) : SectCod) + "%";
                string hccode1 = "%";

                vmEntryAttnLeav1 vm2 = new vmEntryAttnLeav1();
                var pap1r = vm2.SetParamShowActualAttnInfo1(AspSession.Current.CompInfList[0].comcpcod, monthid1, AttnDate1, AttnDate2, hccode1, SectCod);
                DataSet ds1r = AspProcessAccess.GetHmsDataSet(pap1r);
                if (ds1r == null)
                    return View();

                if (ds1r.Tables[0].Rows.Count == 0)
                {
                    return View();
                }
                this.RptGrpList1 = ds1r.Tables[0].DataTableToList<HmsEntityManpower.HcmDayWiseAttanReport>();
               
                ViewBag.lbltbl1 =this.getGropAttendantData(empGroup);
                //ViewBag.lbltbl2 = JsonConvert.SerializeObject(this.RptGrpList1);




            }
         

            ViewBag.ddlType = new SelectList(AspSession.Current.CompInfList[0].SectionList, "sectcod", "sectname");
            return View();
        }

        public String getGropAttendantData(EmpGroup empGroup)
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            data = data + @"<table class='table table-bordered table-hover Atable' id='datatable'><thead>
                            <tr>
<th>SL</th>
                                <th>Date</th>
                                <th>Name & Machine Responses"
                                    + " <span class='hidden-xs'>(From " + empGroup.fromDate + " to " + empGroup.toDate + ") </span>" + @"</th>
                                <th>Office Time</th>                               
                            </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            string empid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in this.RptGrpList1)
            {
                i++;
                if (sectid1 != item.sectcod)
                {
                    data += @"<tr class='grpStaff'><td></td><td></td><td><b>" + @item.sectname + "</b></td><td></td></tr>";
                }
                sectid1 = item.sectcod;
                if (empid1 != item.hccode)
                {
                    string encUrl = "EmpId=950100" + item.staffid + "/&monthId=" + item.monthid +"Time="+DateTime.Now;
                    UrlEncription ur = new UrlEncription();
                 
                    data += @"<tr><td></td><td></td><td><b><u><a href='AttnRptSingle?query=" + ur.Encrypt(encUrl)+"' target='_blank'>" + item.staffid + " - " + @item.hcnamdsg + "</a></u></b></td><td></td></tr>";
                    //data += @"<tr><td colspan = '3'><b><u><a href='AttnRptSingle?EmpId=950100" + item.staffid + "&monthId=" + item.monthid + "' target='_blank'>" + item.staffid + " - " + @item.hcnamdsg + "</a></u></b></td></tr>";
                }
                empid1 = item.hccode;


                data += @"<tr><td>"+i.ToString("00")+"</td><td>" + item.attndate.ToString("dd.ddd")
                            + "</td><td>" + item.Rmrks.Trim() + " " + item.atndtl.Trim()
                            + "</td><td>" + item.Rmrks.Trim() + " " + item.InTime1.Trim() +
                            (item.OutTime1 == item.InTime2 ? "" : " - " + item.OutTime1 + " / " + item.InTime2)
                            + (item.OutTime2 == item.InTime2 ? "" : " - " + item.OutTime2) + "</td></tr>";
            }


            data += "</tbody></table>";
            return data;

        }

       
        #endregion

        #region Yearly Leave
        [HttpGet]
        public ActionResult yearlyLeave(yearlyLeave yleave1) {

            if (yleave1.ddlLocation != null && yleave1.ddlMonth != null) {
                ViewBag.getYearlyLeaveData = this.getYearlyLeaveData(yleave1);
            }

            List<ListItem> items = new List<ListItem>();
            for (int i = -2; i < 2; i++)
            {
                items.Add(new ListItem(DateTime.Now.AddYears(i).ToString("yyyy")));
            }
            ViewBag.ddlMonth = new SelectList(items, "text", "text");

            //show depertmwnt list
            ViewBag.ddlLocation = new SelectList(AspSession.Current.CompInfList[0].SectionList, "sectcod", "sectname");
            //check the report is called
            if (yleave1.reportType != null) {

                //send and get data form Repoert dll
                TempData["rName"] = "Payroll.RptAttenSchedule01";
                TempData["Rptlist"] = this.RptYearLeaveList1;
                TempData["rptParam"] = rptParam;
                TempData["reportType"] = yleave1.reportType;
                TempData["list3"] = null;
                return RedirectToAction("Report", "report");
            }

            //this is print table
            
            return View(); 
        }

        public String getYearlyLeaveData(yearlyLeave yleave1)
        {
            string data = "";
            if (AspSession.Current.AspFormsList == null)
                return null;
            this.PrepareLeaveDetailsReport01(yleave1);

            //return to report generet controller
            if (yleave1.reportType != null) {
                return null; 
            }
            data = @"<table id='datatable' class='table table-bordered Atable'><thead>
                            <tr>
                                <th class='lID' >L.ID</th>
                                <th class='lDes'>Description</th>
                                <th class='lPeriod'>Leave Period</th>
                                <th class='lDays'>Day(s)</th>
                                <th class='Apply'>Apply Date</th>
                                <th class='Apply'>Approve Date</th>                               
                            </tr></thead><tbody>";
            //string sectid1 = "xxxxxxxxxxxx";
            string empid1 = "xxxxxxxxxxxx";
            foreach (var item in this.RptYearLeaveList1)
            {
                if (empid1 != item.hccode)
                {
                    data += @"<tr class='grpStaff'><td></td><td><b>" + item.hccode.Substring(6) +
                        " - " + item.hcnamdsg.Trim() + " " + item.deptdesc.Trim() + "</b></td><td></td><td></td><td></td><td></td></tr>";
                }

                empid1 = item.hccode;
                if (item.leavcod == "000000000000")
                {
                    data += @"<tr><td style='text-align: center'><b>" + item.leavid + "</b></td><td><b>" + item.lreason.Trim()
                          + "</b></td><td white-space:nowrap><b>" + item.begndat.ToString("dd.MMM.yyyy") +
                        (item.begndat1.Trim().Length == 0 ? "" : (item.begndat == item.enddat ? "" :
                        " <span class='hidden-xs'>To</span> " + item.enddat.ToString("dd.MMM.yyyy")))
                        + "</b></td><td class=''><center><b>" + item.totday.ToString("##0.0;-##0.0; ")
                        + "</b></center></td><td><b>" + (item.submitdat.Year == 1900 ? "" : item.submitdat.ToString("dd.MMM.yyyy"))
                        + "</b></td><td><b>" + (item.aprvdat.Year == 1900 ? "" : item.aprvdat.ToString("dd.MMM.yyyy")) + "</b></td></tr>";
                }
                else
                {
                    data += @"<tr><td></td><td>" + item.leavdesc.Trim() + " [ " +
                        item.begndat.ToString("dd.MMM.yyyy") + (item.begndat1.Trim().Length == 0 ? "" :
                        (item.begndat == item.enddat ? "" : " To " + item.enddat.ToString("dd.MMM.yyyy"))) + " ]</td><td></td><td></td><td></td><td></td></tr>";
                }
            }
            data += "</tbody></table>";
            return data;
        }

        private void PrepareLeaveDetailsReport01(yearlyLeave yleave1,string hccode1 = "", string SectCod = "", string TrTyp = "", string PrintId = "")
        {
            SectCod = yleave1.ddlLocation.ToString();
            SectCod = (SectCod.Substring(9, 3) == "000" ? SectCod.Substring(0, 9) : SectCod);
            SectCod = (SectCod.Length == 9 && SectCod.Substring(7, 2) == "00" ? SectCod.Substring(0, 7) : SectCod);
            SectCod = (SectCod.Length == 7 && SectCod.Substring(4, 3) == "000" ? SectCod.Substring(4, 3) : SectCod) + "%";
            hccode1 = "%";

            string monthName1 = "January, " + yleave1.ddlMonth.ToString(); //"February, 2018";// ((ComboBoxItem)this.cmbInfoMonth.SelectedItem).Content.ToString().Trim();
            DateTime Date1 = DateTime.Parse("01" + monthName1);
            string yearid1 = Date1.ToString("yyyy");
            vmReportHCM1 vmr1 = new vmReportHCM1();
            var pap = vmr1.SetParamShowLeaveDetails(AspSession.Current.CompInfList[0].comcpcod, hccode1, yearid1, SectCod);

            DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap);
            if (ds1 == null)
                return;

            var list1 = ds1.Tables[0].DataTableToList<HmsEntityManpower.HcmLeaveDetailsReport01>();
            this.RptYearLeaveList1 = list1;



        }
        #endregion
    }
}