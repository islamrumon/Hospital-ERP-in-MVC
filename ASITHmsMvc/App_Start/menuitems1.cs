using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASITHmsMvc.App_Start
{
    public class menuList
    {
        public string menuId { get; set; }
        public string menuDesc { get; set; }
        public string menuUrlC { get; set; }
        public string menuUrlA { get; set; }
    }
    public class menuitems1
    {
        public static List<menuList> menuParts()
        {
            //   <i class='fa fa- pages' aria-hidden='true'></i>


            List<menuList> parts = new List<menuList>();
            parts.Add(new menuList() { menuId = "0000", menuDesc = "<i class='fa fa-home' aria-hidden='true'></i> <span>My Shortcut</span>", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0001", menuDesc = "01. User Name", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0002", menuDesc = "02. Staff Attendence",  menuUrlC = "General", menuUrlA = "AttnRptSingle" });
            parts.Add(new menuList() { menuId = "0003", menuDesc = "03. Group Attendence", menuUrlC = "General", menuUrlA = "AttnRptGroup" });
            parts.Add(new menuList() { menuId = "0004", menuDesc = "04. Yearly Leave",  menuUrlC = "General", menuUrlA = "YearlyLeave" });
            //parts.Add(new menuList() { menuId = "0005", menuDesc = "05. Stock Report", menuUrlC = "Inventory", menuUrlA = "StockReport1" });


            parts.Add(new menuList() { menuId = "0100", menuDesc = "<i class='fa fa-book' aria-hidden='true'></i><span>Front Desk</span>", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0101", menuDesc = "01. Invoice Wise (FDR)", menuUrlC = "General", menuUrlA = "InvoiceWiseRpt" });
            parts.Add(new menuList() { menuId = "0102", menuDesc = "02. F.Desk Report",  menuUrlC = "General", menuUrlA = "FdeskRpt" });
            parts.Add(new menuList() { menuId = "0103", menuDesc = "03. Receiption (MR)", menuUrlC = "General", menuUrlA = "Reception" });
            parts.Add(new menuList() { menuId = "0104", menuDesc = "04. Patient Visit Token Entry", menuUrlC = "", menuUrlA = "" });


            //parts.Add(new menu1() { menuId = "0100", menuDesc = "Front Desk", menuUrl = "" });
            //parts.Add(new menu1() { menuId = "0101", menuDesc = "01. Receiption (MR)", menuUrl = "General/Reception.aspx" });

            //parts.Add(new menu1() { menuId = "0104", menuDesc = "02. F.Desk Report", menuUrl = "General/FdeskRpt.aspx" });
            //parts.Add(new menu1() { menuId = "0105", menuDesc = "03. Patient Visit Token Entry", menuUrl = "" });



            parts.Add(new menuList() { menuId = "0200", menuDesc = "<i class='fa fa-file-pdf-o' aria-hidden='true'></i><span>Lab Report</span>", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0201", menuDesc = "01. Sample Receive", menuUrlC = "" , menuUrlA =""});
            parts.Add(new menuList() { menuId = "0202", menuDesc = "02. Report Prepration", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0203", menuDesc = "03. Report Submission", menuUrlC = "", menuUrlA = "" });

            parts.Add(new menuList() { menuId = "0300", menuDesc = "<i class='fa fa-briefcase' aria-hidden='true'></i><span>Store Operation</span>", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0301", menuDesc = "01. Store Requisition", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0302", menuDesc = "02. Item Issue/Consumption", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0303", menuDesc = "03. Item Purchase/Recive", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0304", menuDesc = "04. Purchase Process", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0305", menuDesc = "05. Physical Stock Entry", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0306", menuDesc = "06. Inventory Reports", menuUrlC = "Inventory", menuUrlA = "StoreReport" });

            parts.Add(new menuList() { menuId = "0400", menuDesc = "<i class='fa fa-university' aria-hidden='true'></i><span>Accounts</span>", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0401", menuDesc = "01. Accounts Reports" , menuUrlC = "Accounting", menuUrlA = "AccountReport" });
            parts.Add(new menuList() { menuId = "0402", menuDesc = "02. Payment Voucher", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0403", menuDesc = "03. Receipt Voucher", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0404", menuDesc = "04. Journal Voucher", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0405", menuDesc = "05. Fund Transfer Voucher", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0406", menuDesc = "06. Bank Reconciliation", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0407", menuDesc = "07. Payment Proposal Entry", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0408", menuDesc = "08. Opening Voucher", menuUrlC = "", menuUrlA = "" });


            parts.Add(new menuList() { menuId = "0500", menuDesc = "<i class='fa fa-handshake-o' aria-hidden='true'></i><span>Marketing</span>", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0501", menuDesc = "01. Visit Reporting", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0502", menuDesc = "02. Job Assigning", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0503", menuDesc = "03. Evaluation", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0504", menuDesc = "04. Mkt.Reports", menuUrlC = "", menuUrlA = "" });

            parts.Add(new menuList() { menuId = "0600", menuDesc = "<i class='fa fa-users' aria-hidden='true'></i><span>HR and Payroll</span>", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0601", menuDesc = "01. Staff Attendence", menuUrlC = "General", menuUrlA = "AttnRptSingle" });
            parts.Add(new menuList() { menuId = "0602", menuDesc = "02. Group Attendence",  menuUrlC = "General", menuUrlA = "AttnRptGroup" });
            //parts.Add(new menu1() { menuId = "0603", menuDesc = "03. Recruitment", menuUrl = "" });
            //parts.Add(new menu1() { menuId = "0604", menuDesc = "04. HR General Information", menuUrl = "" });
            //parts.Add(new menu1() { menuId = "0605", menuDesc = "05. HR Reports", menuUrl = "" });
            //parts.Add(new menu1() { menuId = "0606", menuDesc = "06. HR Reports (Old)", menuUrl = "" });

            parts.Add(new menuList() { menuId = "0700", menuDesc = "<i class='fa fa-cogs' aria-hidden='true'></i><span>Setup and Utilities</span>", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0701", menuDesc = "01. Accounts Code",  menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0702", menuDesc = "02. Resource Code",   menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0703", menuDesc = "03. Location Code",  menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0704", menuDesc = "04. Other Details Code",  menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0705", menuDesc = "05. Configuration Setup", menuUrlC = "", menuUrlA = "" });

            parts.Add(new menuList() { menuId = "0800", menuDesc = "<i class='fa fa-file-text-o' aria-hidden='true'></i><span>MIS Report</span>", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0801", menuDesc = "01. MIS Reports", menuUrlC = "", menuUrlA = "" });
            parts.Add(new menuList() { menuId = "0802", menuDesc = "02. Doctors Ledger",   menuUrlC = "", menuUrlA = "" });
            return parts;
        }
    }
}