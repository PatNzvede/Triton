using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string VehicleId { get; set; }
        public string RegNumber { get; set; }
        public string Model { get; set; }
        public int NetWeight { get; set; }
        public Depot Depot { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
