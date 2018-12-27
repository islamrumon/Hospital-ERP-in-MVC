using ASITHmsMvc.ASITHmsService1;
using ASITDataLib;
using ASITFunLib;

using ASITHmsEntity;
using ASITHmsViewMan.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;

/// <summary>
/// Summary description for AspProcessAccess
/// </summary>
/// 
namespace ASITHmsMvc
{
    public static class AspProcessAccess
    {
        public static ASITHmsMvc.ASITHmsService1.ASITHmsServiceClient HmsDataService = new ASITHmsMvc.ASITHmsService1.ASITHmsServiceClient();
        public static ProcessAccess HmsDataServicepa1 = null;   // For using WCF Service
        //public static ProcessAccess HmsDataServicepa1 = new ProcessAccess(@"Data Source=LOCALHOST\SQL2K12EXP;initial Catalog=ASITHMSDB;User ID=asitdev;Password=asitdev1234", "Fixed"); // ConnType="Web", ConnType="WPF", ConnType="Fixed"
        //public static ProcessAccess HmsDataServicepa1 = new ProcessAccess("DBConnStr", "WPF"); // ConnType="Web", ConnType="WPF", ConnType="Fixed"

        private static vmHmsGeneralList1 vmGenList1 = new vmHmsGeneralList1();
        public static string GetTestServiceData()
        {
            return HmsDataService.GetData(0);

        }
        //public static DataSet GetHmsDataSet(ASITFunParams.ProcessAccessParams pap1, string ParamPassType = "JSON")
        public static DataSet GetHmsDataSet(ASITFunParams.ProcessAccessParams pap1, string ParamPassType = "PARAMETERS") // string ParamPassType = "CLASS" --- used for WPF 
        {
            try
            {
                AspSession.Current.DatabaseErrorInfoList = null;
                string Comcod1 = (AspSession.Current.CompInfList == null ? AspSession.Current.AppComCode : AspSession.Current.CompInfList[0].comcod);
                DataSet ds1 = new DataSet();
                switch (ParamPassType.ToUpper())
                {
                    case "CLASS":
                        if (HmsDataServicepa1 == null)
                            ds1 = HmsDataService.GetDataSetResult(pap1: pap1, comcod1: Comcod1);
                        else
                            ds1 = HmsDataServicepa1.GetDataSetResult(pap1: pap1);
                        break;
                    case "PARAMETERS":
                        ds1 = HmsDataService.GetDataSetResultWeb(_comCod: pap1.comCod, _ProcName: pap1.ProcName, _ProcID: pap1.ProcID,
                       _parmXml01: pap1.parmXml01, _parmXml02: pap1.parmXml02, _parmBin01: pap1.parmBin01,
                       _parm01: pap1.parm01, _parm02: pap1.parm02, _parm03: pap1.parm03, _parm04: pap1.parm04, _parm05: pap1.parm05,
                       _parm06: pap1.parm06, _parm07: pap1.parm07, _parm08: pap1.parm08, _parm09: pap1.parm09, _parm10: pap1.parm10,
                       _parm11: pap1.parm11, _parm12: pap1.parm12, _parm13: pap1.parm13, _parm14: pap1.parm14, _parm15: pap1.parm15,
                       _parm16: pap1.parm16, _parm17: pap1.parm17, _parm18: pap1.parm18, _parm19: pap1.parm19, _parm20: pap1.parm20,
                       comcod1: Comcod1);
                        break;
                    case "XML":
                        string XmlPap1 = ASITUtility.XmlSerialize(pap1);
                        string XmlDs1;
                        if (HmsDataServicepa1 == null)
                            XmlDs1 = HmsDataService.GetXmlStrResult(XmlPap1, comcod1: Comcod1);
                        else
                            XmlDs1 = HmsDataServicepa1.GetXmlStrResult(XmlPap1);

                        ds1 = ASITUtility.XmlDeserialize<DataSet>(XmlDs1);
                        break;
                    case "JSON": // Can't keep DataSet Name, So Xml DataSet Update to SQL Server not possible
                                 // Recommended to Send/Receive DataSet without DataSet Name.
                        string JsonPap1 = JsonConvert.SerializeObject(pap1, Newtonsoft.Json.Formatting.Indented);
                        string parmXml01DsName = (pap1.parmXml01 == null ? "" : pap1.parmXml01.DataSetName);
                        string parmXml02DsName = (pap1.parmXml02 == null ? "" : pap1.parmXml02.DataSetName);
                        string JsonDs1;
                        if (HmsDataServicepa1 == null)
                            JsonDs1 = HmsDataService.GetJsonStrResult(JsonPap1: JsonPap1, parmXml01DsName: parmXml01DsName, parmXml02DsName: parmXml02DsName, comcod1: Comcod1);
                        else
                            JsonDs1 = HmsDataServicepa1.GetJsonStrResult(JsonPap1: JsonPap1, parmXml01DsName: parmXml01DsName, parmXml02DsName: parmXml02DsName);
                        ds1 = JsonConvert.DeserializeObject<DataSet>(JsonDs1);
                        break;
                    case "NONQUERY":
                        if (HmsDataServicepa1 == null)
                            ds1 = HmsDataService.GetDataSetNonQuerySQL(pap1: pap1, comcod1: Comcod1);
                        else
                            ds1 = HmsDataServicepa1.GetDataSetNonQuerySQL(pap1: pap1);
                        break;
                }
                if (ds1.Tables.Count == 0)
                {
                    AspSession.Current.DatabaseErrorInfoList.Add(new HmsEntityGeneral.DatabaseErrorInfo { errornumber = 0, errorseverity = 0, errorstate = 0, process_id = "", errorline = 0, errormessage = "Unknown Error Occured", errorprocedure = "" });
                    return null;
                }

                if (ds1.Tables[0].TableName.ToUpper().Contains("ERRORTABLE"))
                {
                    AspSession.Current.DatabaseErrorInfoList = ds1.Tables[0].DataTableToList<HmsEntityGeneral.DatabaseErrorInfo>();
                    return null;
                }
                return ds1;
            }
            catch (Exception exp)
            {
                return null;
            }
        }

      
        public static void ShowDatabaseErrorMessage(string customMsg = "")
        {
            // Error Logs to be written here further
            AspSession.Current.DatabaseErrorInfoList = null;
        }

        public static void GetSignedInUserList(string SignInName = "MILLAT", string hcPass = "XXXX", string TerminalID = "UNKNOWN", string newPass1 = "ABCD", string newPass2 = "EFGH")
        {
            if (AspSession.Current.CompInfList == null)
                return;

            if (AspSession.Current.SignedInUserList == null)
            {
                var pap1 = vmGenList1.SetParamSignIn(AspSession.Current.CompInfList[0].comcpcod, SignInName, hcPass, TerminalID, newPass1, newPass2);
                DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
                if (ds1 == null)
                    return;

                if (ds1.Tables[0].Rows.Count == 0)
                    return;

                string _comcod1 = ds1.Tables[0].Rows[0]["comcod"].ToString().Trim();
                string _hccode1 = ds1.Tables[0].Rows[0]["hccode"].ToString().Trim();

                AspSession.Current.SignedInUserList = ds1.Tables[0].DataTableToList<HmsEntityManpower.SignInInfo>(); ;
                if (!(ds1.Tables[0].Rows[0]["hcphoto"] is DBNull))
                    AspSession.Current.SignedInUserList[0].hcphoto = (byte[])ds1.Tables[0].Rows[0]["hcphoto"];

                if (!(ds1.Tables[0].Rows[0]["hcinisign"] is DBNull))
                    AspSession.Current.SignedInUserList[0].hcinisign = (byte[])ds1.Tables[0].Rows[0]["hcinisign"];

                if (!(ds1.Tables[0].Rows[0]["hcfullsign"] is DBNull))
                    AspSession.Current.SignedInUserList[0].hcfullsign = (byte[])ds1.Tables[0].Rows[0]["hcfullsign"];

                AspSession.Current.SignedInUserAuthList = null;
                AspSession.Current.AspFormsList = null;

                //For Temporary Activiting all Form for Testing Purpose
                //WpfProcessAccess.AppFormsList = WpfProcessAccess.FormsList();
                //return;


                // User Authentacion table
                if (ds1.Tables[1].Rows[0]["perdesc"] is DBNull)
                {
                    if (_hccode1.Substring(2) == "0100101001" || _hccode1.Substring(2) == "0100101002")     // FOR TOP MANAGEMENT
                    {
                        AspSession.Current.AspFormsList = AspProcessAccess.AspFormsList();
                        var uiObjList = new HmsEntityGeneral.UserInterfaceAuth().uiObjInfoList;
                        AspSession.Current.SignedInUserAuthList = new List<HmsEntityGeneral.UserInterfaceAuth.uiObjSignInAuth>();
                        foreach (var item in uiObjList)
                            AspSession.Current.SignedInUserAuthList.Add(new HmsEntityGeneral.UserInterfaceAuth.uiObjSignInAuth() { moduleid = item.moduleid, uicode = item.uicode, objallow = true });
                    }
                    return;
                }

                //string xmlbytostring = HmsHelper.ConvertBinaryToString((byte[])(ds1.Tables[1].Rows[0]["perdesc"]));
                string xmlbytostring = (System.Text.ASCIIEncoding.Default.GetString((byte[])(ds1.Tables[1].Rows[0]["perdesc"])));
                char[] xmlDSArray = xmlbytostring.ToCharArray().Reverse().ToArray();
                string xmlDS = new string(xmlDSArray);
                DataSet ds1a = new DataSet();
                System.IO.StringReader xmlSR = new System.IO.StringReader(xmlDS);
                //ds1a.ReadXml(xmlSR, XmlReadMode.IgnoreSchema);
                ds1a.ReadXml(xmlSR);

                string _comcod2 = ds1a.Tables[1].Rows[0]["comcod"].ToString().Trim();
                string _hccode2 = ds1a.Tables[1].Rows[0]["hccode"].ToString().Trim();

                if (!(_comcod1 == _comcod2 && _hccode1 == _hccode2))
                    return;
                DataView dv1 = ds1a.Tables[0].DefaultView;
                dv1.RowFilter = ("objallow=True");
                DataTable tbl1 = dv1.ToTable();
                if (tbl1.Rows.Count == 0)
                    return;

                AspSession.Current.SignedInUserAuthList = tbl1.DataTableToList<HmsEntityGeneral.UserInterfaceAuth.uiObjSignInAuth>();
                AspSession.Current.AspFormsList = new List<string>();
                var AllFormsList = AspProcessAccess.AspFormsList();
                foreach (var item in AspSession.Current.SignedInUserAuthList)
                {
                    string frmId1 = AllFormsList.Find(x => x == item.moduleid.Trim() + "." + item.uicode.Trim().Substring(4));
                    if (frmId1 != null)
                        AspSession.Current.AspFormsList.Add(item.moduleid.Trim() + "." + item.uicode.Trim().Substring(4));
                }
            }
        }

        public static void GetAppConfigInfo()
        {
            List<string> ConfigInfo1 = AspConfigInfo();
            AspSession.Current.AppComCode = ConfigInfo1[0].ToString();// jj[0].ToString();
            AspSession.Current.VersionType = ConfigInfo1[1].ToString();// jj[1].ToString();
            AspSession.Current.AppLocalImagePath = ConfigInfo1[2].ToString();// Config1.AppSettings.Settings["AppLocalImagePath"].Value.ToString().Trim();

        }

        public static List<string> AspConfigInfo()
        {
            List<string> configlst1 = new List<string>();

            /*
              * 
               string ii = System.IO.Path.Combine(Environment.CurrentDirectory, Application.ProductName + ".EXE");
                 Configuration Config1 = ConfigurationManager.OpenExeConfiguration(ii);
                 ii = Config1.AppSettings.Settings["COMPortConfig"].Value.ToString().Trim();
                 string[] jj = ii.Split(new string[] { "." }, StringSplitOptions.None);
                 string strCOMPortName = jj[0]; // "COM3";
                 string strBaudRate = jj[1];    // "19200";
                 string strParity = jj[2];      // "Even";
                 string strDataBits = jj[3];    // "8";
                 string strStopBits = jj[4];    //  "1";                 
              */
            //string ii = System.IO.Path.Combine(Environment.CurrentDirectory, System.Windows.Forms.Application.ProductName + ".EXE");
            //Configuration Config1 = ConfigurationManager.OpenExeConfiguration(ii);
            //Configuration Config1_test = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string ii = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath.ToString();
            var Config1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(ii);

            string[] jj = Config1.AppSettings.Settings["VersionType"].Value.ToString().Trim().Split(new string[] { "." }, StringSplitOptions.None);
            configlst1.Add(jj[0].ToString());
            configlst1.Add(jj[1].ToString());
            configlst1.Add(Config1.AppSettings.Settings["AppLocalImagePath"].Value.ToString().Trim());


            /*            
              var serviceModel = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
              var endpoints = serviceModel.Client.Endpoints;
              foreach (ChannelEndpointElement e in endpoints)
              {
                if (e.Name == "HTTP_Port")
                Console.WriteLine(e.Address);
              }
              Console.ReadLine();                         
             */

            return configlst1;
        }
        public static void GetCompanyInfoList()
        {
            if (AspSession.Current.CompInfList == null)
            {
                //ASITFunParams.ProcessAccessParams pap1 = vmGenList1.SetParamCompBrnSecCodeBook(jj[0].ToString(), jj[1].ToString(), AspSession.Current.HmsVersion); // Company Code 6521 for Digilab
                ASITFunParams.ProcessAccessParams pap1 = vmGenList1.SetParamCompBrnSecCodeBook(AspSession.Current.AppComCode, AspSession.Current.VersionType, AspSession.Current.HmsVersion); // 
                //DataSet ds1 = WpfProcessAccess.GetHmsDataSet(pap1);
                DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
                if (ds1 == null)
                    return;

                AspSession.Current.CompInfList = vmGenList1.PrepareCompBrnSecList(ds1);
            }
        }

        public static void GetCompanyStaffList()
        {
            if (AspSession.Current.CompInfList == null)
                return;

            if (AspSession.Current.StaffList == null)
            {
                var pap1 = vmGenList1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "9[56]%", "5");
                DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
                if (ds1 == null)
                    return;

                AspSession.Current.StaffList = ds1.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();
            }
        }


        public static void GetInventoryItemGroupList()
        {
            if (AspSession.Current.CompInfList == null)
                return;

            if (AspSession.Current.InvItemGroupList == null)
            {
                var pap1 = vmGenList1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "0[1-9]%", "3"); //"[0-4]%"
                DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
                if (ds1 == null)
                    return;

                var pap2 = vmGenList1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "4521%", "3"); //"[0-4]%"
                DataSet ds2 = AspProcessAccess.GetHmsDataSet(pap2);
                if (ds2 == null)
                    return;


                var pap3 = vmGenList1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "41%", "3"); //"[0-4]%"
                DataSet ds3 = AspProcessAccess.GetHmsDataSet(pap3);
                if (ds3 == null)
                    return;

                var pap4 = vmGenList1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "21%", "3"); //"[0-4]%"
                DataSet ds4 = AspProcessAccess.GetHmsDataSet(pap4);
                if (ds4 == null)
                    return;


                var list1 = ds1.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();
                var list2 = ds2.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();
                var list3 = ds3.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();
                var list4 = ds4.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();

                AspSession.Current.InvItemGroupList = list1.Union(list2).Union(list3).Union(list4).ToList();
            }
        }

        public static void GetInventoryItemList()
        {
            if (AspSession.Current.CompInfList == null)
                return;

            if (AspSession.Current.InvItemList == null)
            {
                var pap1 = vmGenList1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "0[1-9]%", "5"); //"[0-4]%"
                DataSet ds1 = AspProcessAccess.GetHmsDataSet(pap1);
                if (ds1 == null)
                    return;

                var pap2 = vmGenList1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "4521%", "5"); //"[0-4]%"
                DataSet ds2 = AspProcessAccess.GetHmsDataSet(pap2);
                if (ds2 == null)
                    return;

                var pap3 = vmGenList1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "41%", "5"); //"[0-4]%"
                DataSet ds3 = AspProcessAccess.GetHmsDataSet(pap3);
                if (ds3 == null)
                    return;

                var pap4 = vmGenList1.SetParamSirInfCodeBook(AspSession.Current.CompInfList[0].comcpcod, "21%", "5"); //"[0-4]%"
                DataSet ds4 = AspProcessAccess.GetHmsDataSet(pap4);
                if (ds4 == null)
                    return;


                var list1 = ds1.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();
                var list2 = ds2.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();
                var list3 = ds3.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();
                var list4 = ds4.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();
                AspSession.Current.InvItemList = list1.Union(list2).Union(list3).Union(list4).ToList();

                //WpfProcessAccess.InvItemList = ds1.Tables[0].DataTableToList<HmsEntityGeneral.SirInfCodeBook>();
            }
        }


        #region RDLC Report Related Functions
        public static List<HmsEntityGeneral.ReportGeneralInfo> GetRptGenInfo(DateTime ServerTime = default(DateTime), string InputSource = "")
        {
            var list3 = new List<HmsEntityGeneral.ReportGeneralInfo>();
            list3.Add(new HmsEntityGeneral.ReportGeneralInfo()
            {
                RptCompName = AspSession.Current.CompInfList[0].comnam,
                RptCompAdd1 = AspSession.Current.CompInfList[0].comadd1,
                RptCompAdd2 = AspSession.Current.CompInfList[0].comadd2,
                RptCompAdd3 = AspSession.Current.CompInfList[0].comadd3,
                RptCompAdd4 = AspSession.Current.CompInfList[0].comadd4,
                RptFooter1 = (InputSource.Length > 0 ? "Input Source: " + InputSource + " / " : "") + "Print Source: " 
                            + AspSession.Current.SignedInUserList[0].terminalID + ", " +
                             AspSession.Current.SignedInUserList[0].signinnam + ", " +
                             AspSession.Current.SignedInUserList[0].sessionID + ", " + (ServerTime.Year > 1900 ? ServerTime.ToString("dd-MMM-yyyy hh:mm:ss tt") : DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"))

            });
            return list3;
        }
        #endregion  // RDLC Report Related Functions
        #region Form List and Creating Forms
      
        public static List<string> AspFormsList()
        {

            List<string> frmlst1 = new List<string>();
            frmlst1.Add("Commercial.frmEntryDocVisit1");

            frmlst1.Add("Commercial.frmEntryFrontDesk1");
            frmlst1.Add("Commercial.frmEntryFrontDesk3");
            frmlst1.Add("Commercial.frmEntryHelpDesk1");
            frmlst1.Add("Commercial.frmReportFrontDesk1");

            frmlst1.Add("Commercial.frmEntryPharmaPOS1");
            frmlst1.Add("Commercial.frmEntryRestauPOS1");
            frmlst1.Add("Commercial.frmReportPharmaPOS1");
            frmlst1.Add("Commercial.frmEntryGenTrPOS1");


            frmlst1.Add("Diagnostic.frmEntryLabRegister1");
            frmlst1.Add("Diagnostic.frmEntryLabReport1");
            frmlst1.Add("Diagnostic.frmEntryLabMagt1");

            frmlst1.Add("Inventory.frmEntryInvMgt1");

            frmlst1.Add("Inventory.frmEntryStoreReq1");
            frmlst1.Add("Inventory.frmEntryStoreIssue1");
            frmlst1.Add("Inventory.frmEntryItemRcv1");
            frmlst1.Add("Inventory.frmEntryItemStock1");


            frmlst1.Add("Inventory.frmEntryPur01");
            frmlst1.Add("Inventory.frmReportStore1");

            frmlst1.Add("Accounting.frmEntryAccMgt1");
            frmlst1.Add("Accounting.frmEntryVoucher1");
            frmlst1.Add("Accounting.frmEntryBankRecon1");
            frmlst1.Add("Accounting.frmEntryPayPro1");
            frmlst1.Add("Accounting.frmReportAccounts1");

            frmlst1.Add("Marketing.frmEntryMarketing1");
            frmlst1.Add("Marketing.frmReportMarketing1");


            frmlst1.Add("Manpower.frmEntryAttn1");
            frmlst1.Add("Manpower.frmEntryPayroll1");
            frmlst1.Add("Manpower.frmEntryRecruit1");
            frmlst1.Add("Manpower.frmEntryHRGenral1");
            frmlst1.Add("Manpower.frmReportHCM1");
            frmlst1.Add("Manpower.frmReportHCM1Old");

            frmlst1.Add("General.frmAccCodeBook1");
            frmlst1.Add("General.frmSirCodeBook1");
            frmlst1.Add("General.frmOtherCodeBook1");
            frmlst1.Add("General.frmConfigSetup1");
            frmlst1.Add("General.frmReportAdmin1");
            frmlst1.Add("General.frmReportMIS1");

            return frmlst1;
        }
        public static string GetMacAddress()
        {
            string strMac1 = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                string strMac2 = "";
                //if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up)
                if ((nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet || nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    && nic.OperationalStatus == OperationalStatus.Up)
                {
                    //return nic.GetPhysicalAddress();
                    var address1 = nic.GetPhysicalAddress();

                    byte[] bytes = address1.GetAddressBytes();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        strMac2 += bytes[i].ToString("X2");
                        // Display the physical address in hexadecimal.
                        //Console.Write("{0}", bytes[i].ToString("X2"));
                        // Insert a hyphen after each byte, unless we are at the end of the
                        // address.
                        if (i != bytes.Length - 1)
                        {
                            //strMac2 += "-";
                            //Console.Write("-");
                        }
                    }
                    //return strMac2;
                }
                if (strMac2.Trim().Length > 0)
                    strMac1 += (strMac1.Trim().Length > 0 ? ", " : "") + strMac2;
            }
            return strMac1;
        }

        #endregion //Form List and Creating Forms
    }
}