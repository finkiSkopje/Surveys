using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Domain
{
    public class Report1Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Generated { get; set; }
        public int Answered { get; set; }
    }
}
