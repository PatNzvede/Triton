using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WebUI.Models
{
    public class WayBill
    {
        public int Id { get; set; }
        [Display(Name = "Way Bill")]
        public string Waybillno { get; set; }
        public Status Status { get; set; }
        public int Weight { get; set; }
        public string Destination { get; set; }
        [Display(Name = "Loading From")]
        public Depot LoadingFrom { get; set; }
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
        [Display(Name = "Created On")]
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