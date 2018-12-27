using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASITHmsMvc.Models
{
    public class AllHelperModels
    {
    }

    //this model for time setup
    public class time
    {
        public int timeId { get; set; }
        public string datetime { get; set; }
    }

    public class GetReportList {

        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class GetDeparttList {

        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class fdeskRpt
    {
        public DateTime toDate { get; set; }
        public DateTime fromDate { get; set; }
        public string report { get; set; }
        public string branch { get; set; }

        public string reportType { get; set; }
    }
}