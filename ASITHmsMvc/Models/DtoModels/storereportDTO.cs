using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASITHmsMvc.Models.DtoModels
{
    public class storereportDTO
    {
            public string DatedForm { get; set; }
        public string DatedTo  { get; set; }
        public string ItemName { get; set; }
        public string SupplySource { get; set; }
        public string Location { get; set; }
        public string ItemGroup { get; set; }
        public string ReportTitles { get; set; }
        public string StaffName { get; set; }

        public string reportType { get; set; }
    }
}