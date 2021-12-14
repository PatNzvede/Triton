using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class DepotVehicleDetail
    {
        public int Id { get; set; }
        public Depot Depot { get; set; }
        public bool WaitingForLoad { get; set; }
        public string VehicleId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
