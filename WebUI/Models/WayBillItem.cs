using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class WayBillItem
    {
        public int Id { get; set; }
        [Display(Name = "Way Bill")]
        public string WayBillno { get; set; }
        [Display(Name = "Item Details")]
        public string ItemDetails { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int Weight { get; set; }
    }
}
