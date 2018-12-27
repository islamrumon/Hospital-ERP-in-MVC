using ASITHmsMvc.Models.DtoModels;
using ASITFunLib;
using ASITHmsEntity;
using ASITHmsViewMan.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ASITHmsMvc.Controllers
{


    public class Location {
        public string sectname { get; set; }
        public string sectcod { get; set; }
    }
    public class Group {
        public string text { get; set; }
        public string value { get; set; }
    }

    public class Report {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class InventoryController : Controller
    {
        private List<HmsEntityInventory.InvStockList> RptStockList = new List<HmsEntityInventory.InvStockList>();  // A
        private List<HmsEntityInventory.InvStockList02> RptStockList02 = new List<HmsEntityInventory.InvStockList02>(); // A
        private List<HmsEntityInventory.StoreIssueSummary1> RptStoreIssue = new List<HmsEntityInventory.StoreIssueSummary1>(); //B
        private List<HmsEntityInventory.PurMrrSummary1> RptPurMrrLst = new List<HmsEntityInventory.PurMrrSummary1>(); //B
        private List<HmsEntityInventory.PurReqSummary1> RptPurReqLst = new List<HmsEntityInventory.PurReqSummary1>(); //B
        private List<HmsEntityInventory.InvTransectionList> RptTranLst = new List<HmsEntityInventory.InvTransectionList>();  //C
        private List<HmsEntityInventory.StoreReqMemoDetails> RptStReqDetail = new List<HmsEntityInventory.StoreReqMemoDetails>(); //D
        private List<HmsEntityInventory.IssueMemoDetails> RptIssuMemDetail = new List<HmsEntityInventory.IssueMemoDetails>(); //D
        private List<HmsEntityInventory.PurReqMemoDetails> RptpurreqMemDetail = new List<HmsEntityInventory.PurReqMemoDetails>(); //D
        private List<HmsEntityInventory.MrrMemoDetails> RptMrrMemoDetail = new List<HmsEntityInventory.MrrMemoDetails>(); // D
        private List<HmsEntityInventory.ItemStatusDetails> RptItemStatusDe = new List<HmsEntityInventory.ItemStatusDetails>(); // E


        //this list for report generet info
        private List<HmsEntityGeneral.ReportGeneralInfo> list3 = new List<HmsEntityGeneral.ReportGeneralInfo>();

        string rName;
        object list;
        private vmReportStore1 vm1 = new vmReportStore1();
        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }

       


        [HttpGet]
        public ActionResult StoreReport(storereportDTO storereport)
        {
            #region //common Fontend data
            //get location list
            var locationList = AspSession.Current.CompInfList[0].SectionList.FindAll(x => x.sectcod.Substring(9, 3) != "000").ToList();

            List<Location> lo = new List<Location>();

            Location l = new Location();
            l.sectname = "All Location";
            l.sectcod = "000000000000";
            lo.Add(l);
            foreach (var item in locationList)
            {
                Location l1 = new Location();
                l1.sectcod = item.sectcod;
                l1.sectname = item.sectname;
                lo.Add(l1);

            }

            ViewBag.Location = new SelectList(lo, "sectcod", "sectname");
            //end location list
            if (AspSession.Current.StaffList == null)
                AspProcessAccess.GetCompanyStaffList();
            if (AspSession.Current.InvItemGroupList == null)
                AspProcessAccess.GetInventoryItemGroupList();


            //if (WpfProcessAccess.AccSirCodeList == null)
            //    WpfProcessAccess.GetAccSirCodeList();

            if (AspSession.Current.InvItemList == null)
                AspProcessAccess.GetInventoryItemList();

            //if (WpfProcessAccess.SupplierContractorList == null)
            //    WpfProcessAccess.GetSupplierContractorList();

            List<Group> gp = new List<Group>();

            Group g = new Group();
            g.text = "ALL GROUP OF ITEMS";
            g.value = "000000000000";
            gp.Add(g);

            foreach (var itemd1 in AspSession.Current.InvItemGroupList)
            {
                var GrpList1 = AspSession.Current.InvItemList.FindAll(x => x.sircode.Substring(0, 7) == itemd1.sircode.Substring(0, 7));
                if (GrpList1.Count > 0)
                {
                    Group g1 = new Group();
                    g1.text = itemd1.sircode.Substring(0, 7) + ": " + itemd1.sirtype + " - " + itemd1.sirdesc.Trim();
                    g1.value = itemd1.sircode;
                    gp.Add(g1);
                }

            }

            ViewBag.ItemGroup = new SelectList(gp, "value", "text");


            //report List
            List<Report> rp = new List<Report>();

            rp.Add(new Report() { Text = "A. OVERAL SUMMERY REPORTS", Value = "000" });

            rp.Add(new Report() { Text = "01. Stock Balance Summery", Value = "A01STOCK01" });
            rp.Add(new Report() { Text = "02. Stock Value Summery", Value = "A01STOCK01VAL" });
            rp.Add(new Report() { Text = "03. Stock Balance With Label", Value = "A01STOCK01L" });
            rp.Add(new Report() { Text = "04. Stock Value With Label", Value = "A01STOCK01LVAL" });
            rp.Add(new Report() { Text = "05. Stock Balance Summery-2", Value = "A02STOCK02" });
            rp.Add(new Report() { Text = "06. Stock Value Summery-2", Value = "A02STOCK02VAL" });

            rp.Add(new Report() { Text = "B. GROUP WISE SUMMERY REPORTS", Value = "000" });
            rp.Add(new Report() { Text = "01. Store requisition", Value = "B01SRQ" });
            rp.Add(new Report() { Text = "02. Store issue/transfer", Value = "B02SIR" });
            rp.Add(new Report() { Text = "03. Purchase requisition", Value = "B03REQ" });
            rp.Add(new Report() { Text = "04. Item receive summary", Value = "B04MRR" });

            rp.Add(new Report() { Text = "C. TRANSACTION MEMO LIST", Value = "" });
            rp.Add(new Report() { Text = "01. Store requisition", Value = "C01SRQ" });
            rp.Add(new Report() { Text = "02. Store issue/transfer", Value = "C02SIR" });
            rp.Add(new Report() { Text = "03. Purchase requisition", Value = "C03REQ" });
            rp.Add(new Report() { Text = "04. Item receive (MRR)", Value = "C04MRR" });

            rp.Add(new Report() { Text = "D. TRANSACTION DETAILS", Value = "" });
            rp.Add(new Report() { Text = "01. Store requisition", Value = "D01SRQ" });
            rp.Add(new Report() { Text = "02. Store issue/transfer", Value = "D02SIR" });
            rp.Add(new Report() { Text = "03. Purchase requisition", Value = "D03REQ" });
            rp.Add(new Report() { Text = "04. Item receive (MRR)", Value = "D04MRR" });
            rp.Add(new Report() { Text = "05. MRR with batch info", Value = "D05MRR" });

            rp.Add(new Report() { Text = "E. TRANSACTION DETAILS", Value = "" });
            rp.Add(new Report() { Text = "01. ITEMS STATUS DETAILS", Value = "E01ISTAT01" });
            rp.Add(new Report() { Text = "02. ITEMS STATUS SUMMERY", Value = "E01ISTAT02" });
            rp.Add(new Report() { Text = "03. L/C STATUS SUMMERY", Value = "E03LCSTAT03" });

            //report list
            ViewBag.ReportTitles = new SelectList(rp, "Value", "Text");
            #endregion

            if (storereport.DatedForm != null && storereport.DatedTo != null)
            {
                var dt1 = DateTime.Parse(storereport.DatedForm);
                var dt2 = DateTime.Parse(storereport.DatedTo);

                string fromDate = (dt1 <= dt2 ? dt1 : dt2).ToString("dd-MMM-yyyy");
                string toDate = (dt1 > dt2 ? dt1 : dt2).ToString("dd-MMM-yyyy");
                string SectCod = storereport.Location;
                string rptType = storereport.ReportTitles;
                SectCod = (SectCod.Substring(9, 3) == "000" ? SectCod.Substring(0, 3) : SectCod);
                SectCod = (SectCod.Length == 9 && SectCod.Substring(7, 2) == "00" ? SectCod.Substring(0, 7) : SectCod);
                SectCod = (SectCod.Length == 7 && SectCod.Substring(4, 3) == "00" ? SectCod.Substring(4, 3) : SectCod);
                SectCod = (SectCod.Length != 12 ? (SectCod.ToString() == "000" ? "%" : SectCod + "%") : SectCod);
                //string ItemGrp1 = ((ComboBoxItem)(this.cmbItemGroup.SelectedItem)).Tag.ToString().Trim().Substring(0, 7);

                string ItemGrp1a = storereport.ItemGroup.ToString().Trim().Substring(0, 7);
                string ItemGrp1desa = storereport.ItemGroup.ToString().Trim();

                string ItemGrp1 = storereport.ItemGroup.ToString();//.Trim().Substring(0, 7);

                string groupItem = "";
               

                foreach (var item in gp) {
                    if (item.value == ItemGrp1) {
                        groupItem = item.text;
                    }
                }
                string ItemGrp1des = groupItem.ToString().Trim();
                string itemCode1 = "";
                string itemCode1des ="";

                if (storereport.ItemName != null) {
                     itemCode1 = storereport.ItemName.Trim();
                     itemCode1des = storereport.ItemName.Trim();
                   
                }
                ItemGrp1a = (ItemGrp1a == "0000000" ? "%" : (itemCode1.Length > 0 ? itemCode1 : ItemGrp1a));
                ItemGrp1desa = (ItemGrp1a == "%" ? "" : (itemCode1.Length > 0 ? itemCode1des : ItemGrp1desa));



                switch (rptType.Substring(0, 2))
                {
                    case "A0": this.GetStockReport(rptType, fromDate, toDate, SectCod, ItemGrp1a, ItemGrp1desa, storereport.reportType); break;
                    case "B0": this.GetSummaryReport(rptType, fromDate, toDate, SectCod, "%", "%", "%", "%", storereport.reportType); break;
                    case "C0": this.GetTransecList(rptType, fromDate, toDate, SectCod, "%", "%", storereport.reportType); break;
                    case "D0": this.GetTransDetails(rptType, fromDate, toDate, SectCod, "%", "%", "%", "%", storereport.reportType); break;
                    case "E0": this.GetItemSpecialTrans(rptType, fromDate, toDate, SectCod, "%", "%", "%", "%", storereport.reportType); break;
                }
                //switch (rptType.Substring(0, 2))
                //{
                //    case "A0": this.GetStockReport(rptType, fromDate, toDate, SectCod, ItemGrp1a, ItemGrp1desa); break;
                //    case "B0": this.GetSummaryReport(rptType, fromDate, toDate, SectCod, "%", "%", "%", "%"); break;
                //    case "C0": this.GetTransecList(rptType, fromDate, toDate, SectCod, "%", "%", storereport.reportType); break;
                //    case "D0": this.GetTransDetails(rptType, fromDate, toDate, SectCod, "%", "%", "%", "%"); break;
                //    case "E0": this.GetItemSpecialTrans(rptType, fromDate, toDate, SectCod, "%", "%", "%", "%"); break;

                //}

                //pass data for create link in view for print report 
                ViewBag.DatedForml = storereport.DatedForm;
                ViewBag.DatedTol = storereport.DatedTo;
                ViewBag.ItemGroupl = storereport.ItemGroup;
                ViewBag.Locationl = storereport.Location;
                ViewBag.ReportTitlesl = storereport.ReportTitles;

             
                

                //check that report comand have or not
                if (storereport.reportType != null) {
                //send and get data form Repoert dll
                TempData["rName"] = this.rName;
                TempData["Rptlist"] = this.list;
                TempData["rptParam"] = null;
                TempData["reportType"] = storereport.reportType;
                TempData["list3"] = this.list3;
                return RedirectToAction("Report", "report");
            }

            }

            return View();

        }


       


        ///report show 
        ///
        #region GetStockReport   A. OVERALL SUMMERY REPORTS rc
        public void GetStockReport(string TrTyp, string fromDate, string toDate, string SectCod, string ItemGrp1, string ItemGrp1des , string reportType)
        {
            var pap1 = vm1.SetParamInvSumReport(AspSession.Current.CompInfList[0].comcod, TrTyp.Substring(3), fromDate, toDate, SectCod, ItemGrp1, ItemGrp1des);
            DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
            string rptType = TrTyp.ToString();
            if (ds1 == null)
                return;
            
            this.RptStockList = ds1.Tables[0].DataTableToList<HmsEntityInventory.InvStockList>();
            this.RptStockList02 = ds1.Tables[0].DataTableToList<HmsEntityInventory.InvStockList02>();
            //this is for reporting visual
            if (reportType != null)
            {
                //if have report command the assign this paramiter
                string serverTime1 = Convert.ToDateTime(ds1.Tables[1].Rows[0]["ServerTime"]).ToString("dd-MMM-yyyy hh:mm:ss tt");
                this.list3 = AspProcessAccess.GetRptGenInfo(ServerTime: Convert.ToDateTime(serverTime1));
                string RptName = (TrTyp.Contains("A01STOCK01L") ? "Store.RptClosingStock1L" : "Store.RptClosingStock1");
                if (rptType.Substring(0, 3) == "A01")
                {
                    //assign the values 
                    this.rName = RptName;
                    this.list = this.RptStockList;
                    //ViewBag.lbltbl1 = (TrTyp.Contains("A01STOCK01L") ? getRptClosingStock1L() : getRptClosingStock1());
                }
                else if (rptType.Substring(0, 3) == "A02")
                {
                    this.rName = "Store.RptClosingStock2";
                    this.list = RptStockList02;
                    //ViewBag.lbltbl1 = getRptClosingStock2();

                }
            }
            else {
                if (rptType.Substring(0, 3) == "A01")
                {

                    ViewBag.lbltbl1 = (TrTyp.Contains("A01STOCK01L") ? getRptClosingStock1L() : getRptClosingStock1());
                }
                else if (rptType.Substring(0, 3) == "A02")
                {
                    ViewBag.lbltbl1 = getRptClosingStock2();

                }

                if (rptType.ToString() == "A01STOCK01")
                {
                    ViewBag.lbltitle1 = "01. Stock Balance Summery" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                }
                else if (rptType.ToString() == "A01STOCK01VAL")
                {
                    ViewBag.lbltitle1 = "02. Stock Value Summery" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                }
                else if (rptType.ToString() == "A01STOCK01L")
                {
                    ViewBag.lbltitle1 = "03. Stock Balance With Label" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                }
                else if (rptType.ToString() == "A01STOCK01LVAL")
                {
                    ViewBag.lbltitle1 = "04. Stock Value With Label" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                }
                else if (rptType.ToString() == "A02STOCK02")
                {
                    ViewBag.lbltitle1 = "05. Stock Balance Summery-2" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                }
                else if (rptType.ToString() == "A02STOCK02VAL")
                {
                    ViewBag.lbltitle1 = "06. Stock Value Summery-2" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                }
            }
            

            
        }
        public String getRptClosingStock1()  // "01. Stock Balance Summery " & "02. Stock Value Summery"   
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            int i = 0;
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                            <thead>  <tr class='TransInvRow' style='top: 0px; '>
                                <th>SL</th>
                                <th>Item Description</th>
                                <th>Unit</th>
                                <th>Opening</th>
                                <th>Receive</th>
                                <th>Issue</th>
                                <th>Closing</th>
                                </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            foreach (var item in this.RptStockList)
            {
                if (sectid1 != item.sectcod)
                {
                    i = 1;
                    data += @"<tr class='grpstaff'>"+
                        "<td><b>" + @item.sectname + "</b></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td></tr>";
                }
                sectid1 = item.sectcod;

                data += @"<tr class='rowstyle-table'><td  class='text-center'>" + i
                            + "</td><td><span class='hidden-xs'> " + item.sircode.Trim() + " - " + "</span>" + item.sirdesc.Trim()
                            + @"</td><td>" + item.sirunit.Trim()
                            + "</td><td class='text-right'>" + item.opnqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.recvqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.isuqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.clsqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td></tr>";
                i++;
            }
            data += "</tbody></table></div>";
            return data;
        }
        public String getRptClosingStock1L() // "03. Stock Balance With Label" & "04. Stock Value With Label"  
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            int i = 0;
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                             <thead>  <tr class='TransInvRow' style='top: 0px; '>
                                <th>SL</th>
                                <th>Item Description</th>
                                <th>Unit</th>
                                <th>Opening</th>
                                <th>Recv./in.</th>
                                <th>Issue/Out</th>
                                <th>Net. Pos</th>
                                <th>Closing </th>
                                <th>Min Label</th>
                                <th>Less Exist</th>
                                <th>Max. Label</th>
                                <th>Excess</th>
                                <th>Re-Order Level</th>   
                                <th>Req. Required</th>
                            </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            foreach (var item in this.RptStockList)
            {
                if (sectid1 != item.sectcod)
                {
                    i = 1;
                    data += @"<tr class=''><td></td>"+
                        "<td><b>" + @item.sectname + "</b></td>" +
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
                }
                sectid1 = item.sectcod;

                data += @"<tr class='rowstyle-table'><td style= 'text-align: center'>" + item.slnum.ToString("0#'.'")

                            + "</td><td><span class='hidden-xs'> " + item.sircode.Trim() + " - " + "</span>" + item.sirdesc.Trim()
                            + @"</td><td>" + item.sirunit.Trim()
                            + "</td><td class='text-right'>" + item.opnqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.recvqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.isuqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.netqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.clsqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.minstock.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.lessqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.maxstock.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.excesqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.reordrlvl.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.rreqqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td></tr>";
                i++;
            }
            var opnqty = this.RptStockList.Sum(x => x.opnqty).ToString("#,##0;-#,##0; ");
            var clsqty = this.RptStockList.Sum(x => x.clsqty).ToString("#,##0;-#,##0; ");
            var excesqty = this.RptStockList.Sum(x => x.excesqty).ToString("#,##0;-#,##0; ");


            data = data + @"<tr><td></td><td></td><td> Grand Total : </td>
                                       <td class='tdgrandt'>" + opnqty + "</td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td class='tdgrandt'>" + clsqty + "</td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td class='tdgrandt'>" + excesqty + "</td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "</tr>";



            data += "</tbody></table></div>";
            return data;
        }
        public String getRptClosingStock2()  // "05. Stock Balance Summery-2" & "06. Stock Value Summery-2" 
        {
            if (AspSession.Current.AspFormsList == null)
                return null;
            string data = "";
            int i = 0;
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                            <thead>  <tr class='TransInvRow' style='top: 0px; '>
                                <th>SL</th>
                                <th>Item Description</th>
                                <th>Unit</th>
                                <th>Opening Balance</th>
                                <th>MRR</th>
                                <th>Other Rcv</th>
                                <th>Total Rcv</th>
                                <th>Issue Qty</th>
                                <th>Consum.</th>
                                <th>Sales</th>
                                <th>Total Out</th>
                                <th>Report Period</th>   
                                <th>Closing Balance</th>
                            </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            foreach (var item in this.RptStockList02)
            {
                if (sectid1 != item.sectcod)
                {
                    i = 1;
                    data += @"<tr class=''><td></td>"+
                        "<td><b>" + @item.sectname + "</b></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
                }
                sectid1 = item.sectcod;

                data += @"<tr class='rowstyle-table'><td style= 'text-align: center'>" + i

                            + "</td><td><span class='hidden-xs'> " + item.sircode.Trim() + " - " + "</span>" + item.sirdesc.Trim()

                            + @"</td><td>" + item.sirunit.Trim()
                            + "</td><td class='text-right'>" + item.opnqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.mrrqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.orcvqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.trcvqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.isuqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.conqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.salqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.tisuqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.netqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td><td class='text-right'>" + item.clsqty.ToString("#,##0.00;-#,##0.00; ")
                            + "</td></tr>";
                i++;
            }

            var opnqty = this.RptStockList.Sum(x => x.opnqty).ToString("#,##0;-#,##0; ");
            var clsqty = this.RptStockList.Sum(x => x.clsqty).ToString("#,##0;-#,##0; ");


            data = data + @"<tr><td></td><td></td><td> Grand Total : </td>
                                       <td>" + opnqty + "</td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td></td>" +
                                           "<td>" + clsqty + "</td>" +
                                           "</tr>";
            data += "</tbody></table></div>";
            return data;
        }

        #endregion 
        // 
        #region GetSummaryReport  B. GROUP WISE SUMMERY REPORTS rc
        public void GetSummaryReport(string TrTyp, string fromDate, string toDate, string SectCod, string ItemGrp1, string ItemGrp1des, string SupId1, string StaffId1,string reportType)
        {
            var pap1 = vm1.SetParamInvSumReport(AspSession.Current.CompInfList[0].comcod, TrTyp.Substring(3), fromDate, toDate, SectCod);
            DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
            string rptItem = TrTyp.ToString();
            if (ds1 == null)
                return;
            //this is all data list
            this.RptStoreIssue = ds1.Tables[0].DataTableToList<HmsEntityInventory.StoreIssueSummary1>();
            this.RptPurReqLst = ds1.Tables[0].DataTableToList<HmsEntityInventory.PurReqSummary1>();
            this.RptPurMrrLst = ds1.Tables[0].DataTableToList<HmsEntityInventory.PurMrrSummary1>();

            //this is for report paramiter
            string serverTime1 = Convert.ToDateTime(ds1.Tables[1].Rows[0]["ServerTime"]).ToString("dd-MMM-yyyy hh:mm:ss tt");
            this.list3 = AspProcessAccess.GetRptGenInfo(ServerTime: Convert.ToDateTime(serverTime1));

            
            
                switch (TrTyp.Substring(0, 3))
                {
                    case "B01":

                        this.RptStoreIssue = ds1.Tables[0].DataTableToList<HmsEntityInventory.StoreIssueSummary1>();

                        if (reportType != null)
                        {
                        this.rName = "Store.RptStoreIssueSum1";
                        this.list = RptStoreIssue;
                    }
                        else {
                            ViewBag.lbltitle1 = "01. Store requisition" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                            ViewBag.lbltbl1 = getStoreRequsiRpt1();
                        }
                        
                        break;
                    case "B02":
                        this.RptStoreIssue = ds1.Tables[0].DataTableToList<HmsEntityInventory.StoreIssueSummary1>();
                        if (reportType != null)
                        {
                        this.rName = "Store.RptStoreIssueSum1";
                        this.list = RptStoreIssue;

                    }
                        else {
                            ViewBag.lbltitle1 = "02. Store issue/transfer" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";

                            ViewBag.lbltbl1 = getStoreRequsiRpt2();
                        }
                    
                        break;
                    case "B03":


                    //--------------there are probelm whene report generet
                            //this.RptPurReqLst = ds1.Tables[0].DataTableToList<HmsEntityInventory.PurReqSummary1>();
                            if (reportType != null)
                            {
                        this.rName = "Store.RptPurReqSum1";
                        this.list = RptPurReqLst;
                    }
                            else {
                                ViewBag.lbltitle1 = "03. Purchase requisition" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";

                                ViewBag.lbltbl1 = getStorePurReqRpt();
                            }
                  
                        break;
                    case "B04":
                        this.RptPurMrrLst = ds1.Tables[0].DataTableToList<HmsEntityInventory.PurMrrSummary1>();
                        if (reportType != null)
                        {
                        this.rName = "Store.RptPurMrrSum1";
                        this.list = RptPurMrrLst;
                    }
                        else {
                            ViewBag.lbltitle1 = "04. Item receive summary " + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";

                            ViewBag.lbltbl1 = getStorePurMrrRpt();
                        }
                        break;

                
                }
           
           
           

        }
        public String getStoreRequsiRpt1()    // "B01". GROUP WISE SUMMERY REPORTS
        {
            if (AspSession.Current.AspFormsList == null)
                return null;

            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                             <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th style='width: 50px'>SL</th>
                          <th style='width: 200px'>Item Description</th>
                         
                          <th style='width: 50px'>Unit</th>
                          <th style='width: 50px'>Quantity</th>
                        </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in RptStoreIssue)
            {

                if (sectid1 != item.sectcod)
                {
                    i = 1;
                    //data += @"<tr><td colspan='2'><span class='pull-left'>From : " + item.sectName + "</span>" +
                    //    "<span class='pull-right'> To : " + item.sectName2 + "</span></td><td colspan='2'></td></tr>";
                    data += @"<tr><td></td><td><span class='tdhead'>FROM : " + item.sectName + "&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp" + " TO :" + item.sectName2 + "</span></td><td></td><td></td></tr>";
                }
                sectid1 = item.sectcod;
                data += @"<tr><td>" + i++ + "</td>" +
                    "<td>" + item.rsircode + " " + item.sirdesc + "</td>" +
                    "" +
                    "<td>" + item.sirunit.ToString() + "</td>" +
                    "<td class='text-right'>" + item.trnqty.ToString("#,##0.00;-#,##0.00; ") + "</td></tr>";
            }
            data += "</tbody></table></div>";

            return data;

        }
        public String getStoreRequsiRpt2()    // "B02". GROUP WISE SUMMERY REPORTS
        {
            if (AspSession.Current.AspFormsList == null)
                return null;

            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                           <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th style='width: 50px'>SL</th>
                          <th style='width: 200px'>Item Description</th>
                         
                          <th style='width: 50px'>Unit</th>
                          <th style='width: 50px'>Quantity</th>
                        </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in RptStoreIssue)
            {

                if (sectid1 != item.sectcod2)
                {
                    i = 1;
                    data += @"<tr><td><span class='tdhead'>FROM : " + item.sectName + "&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp" + " TO :" + item.sectName2 + "</span></td><td></td><td></td><td></td></tr>";

                }
                sectid1 = item.sectcod2;
                data += @"<tr><td>" + i++ + "</td>" +
                    "<td>" + item.rsircode + " " + item.sirdesc + "</td>" +
                    "" +
                    "<td>" + item.sirunit.ToString() + "</td>" +
                    "<td class='text-right'>" + item.trnqty.ToString("#,##0.00;-#,##0.00; ") + "</td></tr>";
            }
            data += "</tbody></table></div>";

            return data;

        }
        public String getStorePurReqRpt()     // "B03. Purchase requisition" 
        {

            if (AspSession.Current.AspFormsList == null)
                return null;

            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                            <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th>SL</th>
                          <th>Item Description</th>
                          <th>Unit</th>
                          <th>Quantity</th>
                          <th>Rate</th>
                          <th>Amount</th>
                        </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in RptPurReqLst)
            {
                if (sectid1 != item.sectcod)
                {
                    i = 1;
                    data += @"<tr><td class='tdhead'>Location : " + item.sectName + "</td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "</tr>";
                }
                sectid1 = item.sectcod;
                data += @"<tr><td>" + i++ + "</td>" +
                    "<td>" + item.rsircode + " " + item.sirdesc + "</td>" +
                     "<td>" + item.sirunit.ToString() + "</td>" +
                    "<td>" + item.reqqty.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                    "<td class='text-right'>" + item.reqamt.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                    "<td class='text-right'>" + item.reqrat.ToString("#,##0.00;-#,##0.00; ") + "</td></tr>";
            }

            var reqamt = this.RptPurReqLst.Sum(x => x.reqamt).ToString("#,##0.00");
            var reqrat = this.RptPurReqLst.Sum(x => x.reqrat).ToString("#,##0.00");

            data += @"<tr><td class='tdgrandt'>GRAND TOTAL : </td><td></td><td></td><td></td><td class='tdgrandt'>" + reqamt + "</td>" +
                "<td class='tdgrandt'>" + reqrat + "</td></tr>";

            data += "</tbody></table></div>";

            return data;

        }
        public String getStorePurMrrRpt()     // "B04. Item receive summary" 
        {
            if (AspSession.Current.AspFormsList == null)
                return null;

            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                          <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th>SL</th>
                          <th>Item Description</th>
                          <th>Unit</th>
                          <th>Quantity</th>
                          <th>Rate</th>
                          <th>Amount</th>
                        </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in RptPurMrrLst)
            {
                if (sectid1 != item.sectcod)
                {
                    i = 1;
                    data += @"<tr><td class='tdhead'>Location : " + item.sectName + "</td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td>"+
                        "<td></td></tr>";
                }
                sectid1 = item.sectcod;
                data += @"<tr><td>" + i++ + "</td>" +
                    "<td>" + item.rsircode + " " + item.sirdesc + "</td>" +
                    "<td>" + item.sirunit.ToString() + "</td>" +
                    "<td>" + item.mrrqty.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                    "<td class='text-right'>" + item.mrrrat.ToString("#,##0.00;-#,##0.00; ") + "</td>" +
                    "<td class='text-right'>" + item.mrramt.ToString("#,##0.00;-#,##0.00; ") + "</td></tr>";
            }

            // var mrrrat = this.RptPurMrrLst.Sum(x => x.mrrrat).ToString("#,##0.00");
            var mrramt = this.RptPurMrrLst.Sum(x => x.mrramt).ToString("#,##0.00");

            data += @"<tr><td></td><td></td><td></td><td></td><td class='tdgrandt'>GRAND TOTAL :</td>" +
                "<td class='tdgrandt'>" + mrramt + "</td></tr>";

            data += "</tbody></table></div>";

            return data;
        }

        #endregion

        #region GetTransecList  C. TRANSACTION MEMO LIST rc
        public void GetTransecList(string TrTyp, string fromDate, string toDate, string SectCod, string SupId1, string StaffId1 ,string reportType)
        {
            var pap1 = vm1.SetParamStoreTransList(AspSession.Current.CompInfList[0].comcod, TrTyp.Substring(3), fromDate, toDate, SectCod);
            DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
            string rptItem = TrTyp.ToString();
            if (ds1 == null)
                return;
            this.RptTranLst = ds1.Tables[0].DataTableToList<HmsEntityInventory.InvTransectionList>();
            if (reportType != null)
            {
                //if have report command the assign this paramiter
                string ServerTime1 = Convert.ToDateTime(ds1.Tables[1].Rows[0]["ServerTime"]).ToString("dd-MMM-yyyy hh:mm:ss tt");
                this.list3= AspProcessAccess.GetRptGenInfo(ServerTime: Convert.ToDateTime(ServerTime1));
                this.rName = "Store.RptTransectionList";
                list = this.RptTranLst;
            }
            else {

                //if report command is not execuit ther print the table in view 
                // if execuit then generet the report in report pdf,doc etc
                switch (TrTyp.Substring(0, 3))
                {
                    case "C01":
                        ViewBag.lbltitle1 = "01. STORE REQUISITION " + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getTransLstRpt();
                        break;
                    case "C02":
                        ViewBag.lbltitle1 = "02. STORE ISSUE/TRANSFER " + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getTransLstRpt();
                        break;
                    case "C03":
                        ViewBag.lbltitle1 = "03. PURCHASE REQUISITION " + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getTransLstRpt();
                        break;
                    case "C04":
                        ViewBag.lbltitle1 = "04. ITEM RECEIVE (MRR) " + @" <span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getTransLstRpt();
                        break;


                }
            }

            
            
        }
        public String getTransLstRpt()        // "C01. Store requisition" 
        {
            if (AspSession.Current.AspFormsList == null)
                return null;

            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                           <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th>SL</th>
                          <th>Date</th>
                          <th>Memo No</th>
                          <th>Department Name</th>
                          <th>Store/Supply Source</th>
                          <th>Prepare/Rcv/AppRv.By</th>
                        </tr></thead><tbody>";
            // string sectid1 = "xxxxxxxxxxxx";

            foreach (var item in RptTranLst)
            {

                string ssirname = item.sectname2.Trim() == "" ? item.ssirname : item.sectname2;
                string recvbyName = item.PreparByName.Trim() == "" ? item.recvbyName.Trim() == "" ? item.approvbyName : item.recvbyName : item.PreparByName;
                data += @"<tr><td>" + item.slnum.ToString() + "</td>" +
                    "<td>" + item.memoDate1 + "</td>" +
                    "<td>" + item.memonum1 + "</td>" +
                    "<td>" + item.sectname + "</td>" +
                    "<td>" + ssirname + "</td>" +
                    "<td>" + recvbyName + "</td></tr>";
            }
            data += "</tbody></table></div>";

            return data;
        }

        #endregion

        #region GetTransDetails  D. TRANSACTION DETAILS
        public void GetTransDetails(string TrTyp, string fromDate, string toDate, string SectCod, string ItemGrp1, string ItemGrp1des, string SupId1, string StaffId1, string reportType)
        {
            var pap1 = vm1.SetParamStoreTransDetails(AspSession.Current.CompInfList[0].comcod, TrTyp.Substring(3), fromDate, toDate, SectCod);
            DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
            if (ds1 == null)
                return;
            //this is data list
            this.RptStReqDetail = ds1.Tables[0].DataTableToList<HmsEntityInventory.StoreReqMemoDetails>();    // D01
            this.RptIssuMemDetail = ds1.Tables[0].DataTableToList<HmsEntityInventory.IssueMemoDetails>();     // D02
            this.RptpurreqMemDetail = ds1.Tables[0].DataTableToList<HmsEntityInventory.PurReqMemoDetails>();  // D03
            this.RptMrrMemoDetail = ds1.Tables[0].DataTableToList<HmsEntityInventory.MrrMemoDetails>();       // D04 & D05


            //this is for visual report
            string ServerTime1 = Convert.ToDateTime(ds1.Tables[1].Rows[0]["ServerTime"]).ToString("dd-MMM-yyyy hh:mm:ss tt");
            this.list3 = AspProcessAccess.GetRptGenInfo(ServerTime: Convert.ToDateTime(ServerTime1));
            //  string datefromTo = @"<span class='hidden-xs pull-right'> From " + this.txtMonth1.Text + " To " + this.txtMonth2.Text + "</span>";
            switch (TrTyp.Substring(0, 3))
            {
                case "D01":

                    if (reportType != null)
                    {
                        this.list = RptStReqDetail;
                        this.rName = "Store.RptStoreReqDetails1";
                    }
                    else {
                        ViewBag.lbltitle1 = "01. STORE REQUISION TRANSACTION LIST" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getStorReqDetails();
                    }
                    break;
                case "D02":
                    if (reportType != null)
                    {
                        this.list = RptIssuMemDetail;
                        this.rName = "Store.RptIssueDetails1";
                    }
                    else {
                        ViewBag.lbltitle1 = "02. ISSUE MEMO DETAILS LIST " + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getIssuMemDetails();
                    }

                    break;
                case "D03":
                    if (reportType != null)
                    {
                        this.list = RptpurreqMemDetail;
                        this.rName = "Store.RptPurReqDetails1";
                    }
                    else {
                        ViewBag.lbltitle1 = "03. PURCHASE REQUSITION MEMO DETAILS " + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getPurReqMemDetails();
                    }

                    break;
                case "D04":
                    if (reportType != null)
                    {
                        this.list = RptMrrMemoDetail;
                        this.rName = "Store.RptMrrDetails1";
                    }
                    else {
                        ViewBag.lbltitle1 = "04. ITEM RECEIVE MRR LIST " + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getMrrmemoDetails1();
                    }
                    
                    break;
                case "D05":
                    if (reportType != null)
                    {
                        this.list = RptMrrMemoDetail;
                        this.rName = "Store.RptMrrDetails2";
                    }
                    else {
                        ViewBag.lbltitle1 = "05. MRR WITH BATCH INFO LIST " + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getMrrmemoDetails2();


                    }
                    break;

            }
           
        }
        public string getStorReqDetails()   // "D01"
        {
            if (AspSession.Current.AspFormsList == null)
                return null;

            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                          <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th>SL</th>
                          <th>Item Description</th>
                          <th>Unit</th>
                          <th>Quantity</th>
                        </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in RptStReqDetail)
            {
                string narration = item.srfnar.Trim().Length == 0 ? " " : " Narration : " + item.srfnar.ToString();
                string reference = item.srfref.Trim().Length == 0 ? " " : " Reference : " + item.srfref.ToString();


                if (sectid1 != item.srfno1)
                {
                    i = 1;
                    data += @"<tr><td> <span class='pull-left tdhead'>Date: " + item.srfdat1 + " " + "&nbsp &nbsp &nbsp" + " Req. No : " + item.srfno1 + "&nbsp &nbsp &nbsp" + " Store : " + item.sectname + " " + "</span>" +
                        "<span class='pull-right tdhead'> Req.By : " + item.srfbyName + " </span></td><td></td><td></td><td></td></tr>";
                    data += @"<tr><td><span class='pull-left tdhead' >" + reference + " </span>" +
                        "<span class='pull-right tdhead'>" + narration + " </span></td><td></td><td></td><td></td></tr>";
                }
                sectid1 = item.srfno1;
                data += @"<tr><td>" + i + "</td>" +
                    "<td>" + item.rsircode + " " + item.sirdesc + "</td>" +
                    "<td>" + item.sirunit.ToString() + "</td>" +
                    "<td class='text-right'>" + item.srfqty.ToString("#,##0.00;-#,##0.00; ") + "</td></tr>";
                i++;
            }

            data += "</tbody></table></div>";

            return data;
        }
        public string getIssuMemDetails()   // "D02"
        {
            if (AspSession.Current.AspFormsList == null)
                return null;


            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                           <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th>SL</th>
                          <th>Item Description</th>
                          <th>Unit</th>
                          <th>Quantity</th>
                        </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in RptIssuMemDetail)
            {
                string sectname = item.sectname.Trim().Length == 0 ? " " : " From : " + item.sectname.ToString();
                string sectname2 = item.sectname2.Trim().Length == 0 ? " " : " To : " + item.sectname2.ToString();
                string otherref = item.sirref.Trim().Length == 0 ? " " : " Other Ref. " + item.sirref.ToString();
                string issueby = item.sirbyName.Trim().Length == 0 ? " " : " Issued By : " + item.sirbyName.ToString();
                string reciveby = item.recvbyName.Trim().Length == 0 ? " " : " Received By : " + item.recvbyName.ToString();
                if (sectid1 != item.srfno1)
                {
                    i = 1;
                    data += @"<tr><td> <span class='pull-left'>Date: " + item.sirdat1 + " " + " Issue No : " + item.sirno1 + "</span>" +
                        "<span class='pull-center'>" + sectname + " </span>" +
                        "<span class='pull-right'>" + sectname2 + " </span></td><td></td><td></td><td></td></tr>";
                    data += @"<tr><td><span class='pull-left'>" + issueby + " </span>" +
                        "<span class='pull-center'>" + otherref + " </span>" +
                        "<span class='pull-right'>" + reciveby + " </span></td><td></td><td></td><td></td></tr>";

                }
                sectid1 = item.srfno1;
                data += @"<tr><td>" + i + "</td>" +
                    "<td>" + item.rsircode + " " + item.sirdesc + "</td>" +
                    "<td>" + item.sirunit.ToString() + "</td>" +
                    "<td class='text-right'>" + item.sirqty.ToString("#,##0.00;-#,##0.00; ") + "</td></tr>";
                i++;
            }

            data += "</tbody></table></div>";

            return data;
        }
        public string getPurReqMemDetails() // "D03"
        {
            if (AspSession.Current.AspFormsList == null)
                return null;


            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                           <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th>SL</th>
                          <th>Item Description</th>
                          <th>Quantity</th>
                          <th>Unit</th>
                          <th>Rate</th>
                          <th>Amount</th>
                        </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in RptpurreqMemDetail)
            {
                string reqbyName = item.reqbyName.Trim().Length == 0 ? " " : " Req.By : " + item.reqbyName.ToString();
                string reqnar = item.reqnar.Trim().Length == 0 ? " " : " Narration : " + item.reqnar.ToString();
                string reqref = item.reqref.Trim().Length == 0 ? " " : " Reference : " + item.reqref.ToString();
                //      string reqqty = item.reqqty.Trim().Length == 0 ? " " : " Reference : " + item.reqref.ToString();
                if (sectid1 != item.reqno1)
                {
                    i = 1;
                    data += @"<tr><td> <span class='pull-left'>Date: " + item.reqdat1 + " " + " Req. No : " + item.reqno1 + "</span>" +
                        "<span class='pull-right'>" + reqbyName + " </span></td><td></td><td></td><td></td><td></td><td></td></tr>";
                    data += @"<tr><td><span class='pull-right'>" + reqnar + " </span>" +
                        "<span class='pull-left'>" + reqref + " </span></td><td></td><td></td><td></td><td></td><td></td></tr>";
                }
                sectid1 = item.reqno1;
                data += @"<tr><td>" + i + "</td>" +
                    "<td>" + item.rsircode + " " + item.sirdesc + "</td>" +
                    "<td>" + item.reqqty.ToString("#,##0.00") + "</td>" + // *
                    "<td>" + item.sirunit.ToString() + "</td>" +
                    "<td class='text-right'>" + item.reqrate.ToString("#,##0.00") + "</td>" + // *
                    "<td class='text-right'>" + item.reqamt.ToString("#,##0.00;-#,##0.00; ") + "</td></tr>"; // *
                i++;

            }
            var reqrate = this.RptpurreqMemDetail.Sum(x => x.reqrate).ToString("#,##0");
            var reqamt = this.RptpurreqMemDetail.Sum(x => x.reqrate).ToString("#,##0");

            data += @"<tr><td></td><td></td><td></td><td class='tdgrandt'> GRAND TOTAL </td><td class='tdgrandt'> " + reqrate + "</td>" +
                "<td class='tdgrandt'>" + reqamt + "</td></tr>";

            data += "</tbody></table></div>";
            return data;
        }
        public string getMrrmemoDetails1()  // "D04" 
        {
            if (AspSession.Current.AspFormsList == null)
                return null;


            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                           <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th>SL</th>
                          <th>Item Description</th>
                          <th>Unit</th>
                          <th>Quantity</th>
                          <th>Rate</th>
                          <th>Amount</th>
                        </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in RptMrrMemoDetail)
            {

                string ssirname = item.ssirname.Trim().Length == 0 ? " " : " Supply Source : " + item.ssirname.ToString();
                //  string orderno1 = item.orderno1.Trim().Contains("000000-0000-00000") ? " " : " Order No : " + item.orderno1.ToString();
                string chlnno = item.chlnno.Trim().Length == 0 ? " " : " Challan Ref. : " + item.chlnno.ToString();
                string mrrref = item.mrrref.Trim().Length == 0 ? " " : " Other Ref. : " + item.mrrref.ToString();
                string mrrnar = item.mrrnar.Trim().Length == 0 ? " " : " Narration. : " + item.mrrnar.ToString();
                if (sectid1 != item.mrrno1)
                {
                    i = 1;
                    data += @"<tr><td> <span class='pull-left tdhead'>Date: " + item.mrrdat1 + "  &nbsp &nbsp &nbsp &nbsp" +
                        " Mrr. No : " + item.mrrno1 + " &nbsp &nbsp &nbsp &nbsp" + "</span>" +
                        "<span class='text-right'>LOCATION : " + item.sectname + " &nbsp &nbsp " + mrrnar + " </span></td><td></td><td></td><td></td><td></td><td></td></tr>";
                    data += @"<tr><td><span class='tdhead'>" + ssirname +
                        " &nbsp &nbsp" + chlnno +
                        " &nbsp &nbsp " + mrrref + " </span></td><td></td><td></td><td></td><td></td><td></td></tr>";
                }
                sectid1 = item.mrrno1;
                data += @"<tr><td>" + i + "</td>" +
                    "<td>" + item.rsircode + " " + item.sirdesc + "</td>" +
                    "<td>" + item.sirunit.ToString() + "</td>" + // *
                    "<td class='text-right'>" + item.mrrqty.ToString("#,##0.00") + "</td>" +
                    "<td class='text-right'>" + item.mrrrate.ToString("#,##0.00") + "</td>" + // *
                    "<td class='text-right'>" + item.mrramt.ToString("#,##0.00;-#,##0.00; ") + "</td></tr>"; // *
                i++;
            }
            var mrrrate = this.RptMrrMemoDetail.Sum(x => x.mrrrate).ToString("#,##0");
            var mrramt = this.RptMrrMemoDetail.Sum(x => x.mrramt).ToString("#,##0");

            data += @"<tr><td></td><td></td><td></td><td class='tdgrandt'> GRAND TOTAL </td><td class='tdgrandt'> " + mrrrate + "</td>" +
                "<td class='tdgrandt'>" + mrramt + "</td></tr>";

            data += "</tbody></table></div>";
            return data;
        }
        public string getMrrmemoDetails2()  // "D05" 
        {
            if (AspSession.Current.AspFormsList == null)
                return null;


            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                           <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th>SL</th>
                          <th>Item Description</th>
                          <th>Quantity</th>
                          <th>Unit</th>
                          <th>Rate</th>
                          <th>Amount</th>
                          <th>Mfg. Date</th>
                          <th>Exp. Date</th>
                          <th>Batch No</th>
                        </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in RptMrrMemoDetail)
            {

                string ssirname = item.ssirname.Trim().Length == 0 ? " " : " Supply Source : " + item.ssirname.ToString();
                //  string orderno1 = item.orderno1.Trim().Contains("000000-0000-00000") ? " " : " Order No : " + item.orderno1.ToString();
                string chlnno = item.chlnno.Trim().Length == 0 ? " " : " Challan Ref. : " + item.chlnno.ToString();
                string mrrref = item.mrrref.Trim().Length == 0 ? " " : " Other Ref. : " + item.mrrref.ToString();
                string mrrnar = item.mrrnar.Trim().Length == 0 ? " " : " Narration. : " + item.mrrnar.ToString();
                if (sectid1 != item.mrrno1)
                {
                    i = 1;
                    data += @"<tr><td></td><td> <span class='pull-left tdhead'>Date: " + item.mrrdat1 + "  &nbsp &nbsp &nbsp &nbsp" +
                        " Mrr. No : " + item.mrrno1 + " &nbsp &nbsp &nbsp &nbsp" + "</span>" +
                        "<span class='text-right'>LOCATION : " + item.sectname + " &nbsp &nbsp " + mrrnar + " </span></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
                    data += @"<tr><td></td><td><span class='tdhead'>" + ssirname +
                        " &nbsp &nbsp" + chlnno +
                        " &nbsp &nbsp " + mrrref + " </span></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
                }
                sectid1 = item.mrrno1;
                data += @"<tr><td>" + i + "</td>" +
                    "<td>" + item.rsircode + " " + item.sirdesc + "</td>" +
                    "<td class='text-right'>" + item.mrrqty.ToString("#,##0.00") + "</td>" +
                    "<td>" + item.sirunit.ToString() + "</td>" + // *
                    "<td class='text-right'>" + item.mrrrate.ToString("#,##0.00") + "</td>" + // *
                    "<td class='text-right'>" + item.mrramt.ToString("#,##0.00") + "</td>" + // *
                    "<td class='text-right'>" + item.mfgdat.ToString("dd-MMM-yyyy") + "</td>" +
                    "<td class='text-right'>" + item.expdat.ToString("dd-MMM-yyyy") + "</td>" +
                    "<td class='text-right'>" + item.batchno.ToString() + "</td>" +
                    "</tr>"; // *
                i++;
            }
            var mrrrate = this.RptMrrMemoDetail.Sum(x => x.mrrrate).ToString("#,##0");
            var mrramt = this.RptMrrMemoDetail.Sum(x => x.mrramt).ToString("#,##0");

            data += @"<tr><td class='tdgrandt'> GRAND TOTAL </td><td class='tdgrandt'> " + mrrrate + "</td>" +
                "<td class='tdgrandt'>" + mrramt + "</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";

            data += "</tbody></table></div>";
            return data;
        }

        #endregion

        #region GetItemSpecialTrans  E. SPECIAL REPORTS
        public void GetItemSpecialTrans(string TrTyp, string fromDate, string toDate, string SectCod, string ItemGrp1, string ItemGrp1des, string SupId1, string StaffId1,string reportType)
        {
            var pap1 = vm1.SetParamItemSpecialTrans(AspSession.Current.CompInfList[0].comcod, TrTyp.Substring(3), fromDate, toDate, SectCod);
            DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
            if (ds1 == null)
                return;
            this.RptItemStatusDe = ds1.Tables[0].DataTableToList<HmsEntityInventory.ItemStatusDetails>();    // E01
            if (reportType != null)
            {
                //if have report command the assign this paramiter
                string ServerTime1 = Convert.ToDateTime(ds1.Tables[1].Rows[0]["ServerTime"]).ToString("dd-MMM-yyyy hh:mm:ss tt");
                this.list3 = AspProcessAccess.GetRptGenInfo(ServerTime: Convert.ToDateTime(ServerTime1));
                this.rName = "Store.RptItemStatus1";
                list = RptItemStatusDe;
            }
            else {
                //  string datefromTo = @"<span class='hidden-xs pull-right'> From " + this.txtMonth1.Text + " To " + this.txtMonth2.Text + "</span>";
                switch (TrTyp.Substring(0, 3))
                {
                    case "E01":
                        ViewBag.lbltitle1 = "01. ITEMS STATUS DETAILS" + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getItemsStatus1();
                        break;
                    case "E02":
                        ViewBag.lbltitle1 = "02. ITEMS STATUS SUMMERY " + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = getItemsStatus1();
                        break;
                    case "E03":
                        ViewBag.lbltitle1 = "03. L/C STATUS SUMMERY " + @"<span class='hidden-xs pull-right'> From " + fromDate + " To " + toDate + "</span>";
                        ViewBag.lbltbl1 = "Not Found";
                        break;

                }
            }
           
            
        }

        public string getItemsStatus1()  // "E01" 
        {
            if (AspSession.Current.AspFormsList == null)
                return null;


            string data = "";
            data = data + @"<div class=''><table id='mytable' class='table table-bordered table-hover Atable'>
                 <thead>  <tr class='TransInvRow' style='top: 0px; '>
                          <th>SL</th>
                          <th>Date</th>
                          <th>Memo</th>
                          <th>Trans. Name</th>
                          <th>Description</th>
                          <th>In-Qty</th>
                          <th>Out-Qty</th>
                          <th>Balance</th>
                          <th>Rate</th>
                          <th>Amount</th>
                        </tr></thead><tbody>";
            string sectid1 = "xxxxxxxxxxxx";
            int i = 0;
            foreach (var item in RptItemStatusDe)
            {

                //  string ssirname = item.ssirname.Trim().Length == 0 ? " " : " Supply Source : " + item.ssirname.ToString();
                //  string orderno1 = item.orderno1.Trim().Contains("000000-0000-00000") ? " " : " Order No : " + item.orderno1.ToString();

                if (sectid1 != item.sircode)
                {
                    i = 1;
                    data += @"<tr><td><span class='tdhead'>" + "LOCATION : " + item.sectname + "</span></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
                    data += @"<tr><td> <span class='pull-left tdhead'>Item Name : " + item.sircode + "-" + item.sirdesc +
                        " Unit : " + item.sirunit + "</span></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";

                }
                sectid1 = item.sircode;
                data += @"<tr><td>" + i + "</td>" +
                    "<td class='text-nowrap'>" + item.trndat.ToString("dd-MMM-yyyy") + "</td>" +
                    "<td class='text-right'>" + item.memonum1 + "</td>" +
                    "<td>" + item.memoname + "</td>" + // *
                    "<td class=''>" + item.trndesc + "</td>" + // *
                    "<td class='text-right'>" + item.inqty.ToString("#,##0.00") + "</td>" + // *
                    "<td class='text-right'>" + item.outqty.ToString("#,##0.00") + "</td>" +
                    "<td class='text-right'>" + item.balqty.ToString("#,##0.00") + "</td>" +
                    "<td class='text-right'>" + item.itmrat.ToString("#,##0.00") + "</td>" +
                    "<td class='text-right'>" + item.itmamt.ToString("#,##0.00") + "</td>" +
                    "</tr>"; // *
                i++;
            }
            var inqty = this.RptItemStatusDe.Sum(x => x.inqty).ToString("#,##0");
            var outqty = this.RptItemStatusDe.Sum(x => x.outqty).ToString("#,##0");
            var balqty = this.RptItemStatusDe.Sum(x => x.balqty).ToString("#,##0");
            var itmrat = this.RptItemStatusDe.Sum(x => x.itmrat).ToString("#,##0");
            var itmamt = this.RptItemStatusDe.Sum(x => x.itmamt).ToString("#,##0");

            data += @"<tr><td></td><td></td><td></td><td></td><td class='tdgrandt'> GRAND TOTAL </td>
             <td class='tdgrandt'> " + inqty + "</td>" +
                "<td class='tdgrandt'>" + outqty + "</td>" +
                "<td class='tdgrandt'>" + balqty + "</td>" +
                "<td class='tdgrandt'>" + itmrat + "</td>" +
                "<td class='tdgrandt'>" + itmamt + "</td>" +
                "</tr>";

            data += "</tbody></table></div>";
            return data;
        }

        #endregion
    }
}