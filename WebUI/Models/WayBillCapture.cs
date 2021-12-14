using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class WayBillCapture
    {
        public int Id { get; set; }
        [Display(Name = "Way Bill")]
        public string Waybillno { get; set; }
        [Display(Name = "Capturing Stage")]
        public bool CapturingStage { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
