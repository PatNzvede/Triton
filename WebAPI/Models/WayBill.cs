using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class WayBill
    {
        public int Id { get; set; }
        public string Waybillno { get; set; }
        public Status Status { get; set; }
        public int Weight { get; set; }
        public string Destination { get; set; }
        public Depot LoadingFrom { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string VehicleId { get; set; }

    }
    public enum Depot
    {
        Johannesburg,
        Durban
    }
    public enum Status
    {
        Open,
        Transit,
        Delivered,
        Damaged,
        Closed
    }
}
