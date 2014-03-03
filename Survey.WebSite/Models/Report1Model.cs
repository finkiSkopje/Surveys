using Survey.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.WebSite.Models
{
    public class Report1Model
    {
        public int? SemesterTypes { get; set; }
        public int? Years { get; set; }
        public IEnumerable<Report1Entity> list { get; set; }
    }
}