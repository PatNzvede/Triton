using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class WayBillCapture
    {
        public int Id { get; set; }

        public string Waybillno { get; set; }
        public bool CapturingStage { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
