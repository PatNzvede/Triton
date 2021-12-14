using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class WayBillItem
    {
        public int Id { get; set; }
        public string WayBillno { get; set; }
        public string ItemDetails { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int Weight { get; set; }
    }
}
