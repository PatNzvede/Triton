using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string VehicleId { get; set; }
        [Display(Name = "Registration Number")]
        public string RegNumber { get; set; }
        public string Model { get; set; }
        [Display(Name = "Net Weight")]
        public int NetWeight { get; set; }
        public Depot Depot { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
