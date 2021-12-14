using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaybillsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public WaybillsController(AppDbContext _context)
        {
            this._context = _context;
        }
        private static TEnum? GetEnum<TEnum>(string value) where TEnum : struct
        {
            TEnum result;

            return Enum.TryParse<TEnum>(value, out result) ? (TEnum?)result : null;
        }
        //Get api/WayBills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WayBill>>> Index(string? search)
        {
            return await _context.WayBills.Where(a => a.Status == GetEnum<Status>(search) || search == null).ToListAsync();
        }

        // GET: api/WayBills/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<WayBill>> GetWayBill(int id)
        {
            var waybill = await _context.WayBills.FindAsync(id);

            if (waybill == null)
            {
                return NotFound();
            }
            return waybill;
        }

        // PUT: api/WayBills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWayBill(int id, WayBill waybill)
        {
            if (id != waybill.Id)
            {
                return BadRequest();
            }
            _context.Entry(waybill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/WayBills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WayBill>> PostWayBill(WayBill waybill)
        {
            WayBillCapture wc = _context.WayBillCaptures.Where(a => a.Waybillno == waybill.Waybillno).FirstOrDefault();
            if (waybill.Waybillno != null && wc.CreatedBy != 0)
            {
                return BadRequest("Waybill is not active, verify your waybill is active");
            }
                int bb = _context.WayBillItems.Where(a => a.WayBillno == waybill.Waybillno).Sum(a => a.Weight);
            if (waybill.VehicleId == null)
            {
                List<DepotVehicleDetail> vhd = _context.DepotVehicleDetails.Where(a => a.WaitingForLoad == true &&
                a.Depot == waybill.LoadingFrom).ToList();   
                foreach (DepotVehicleDetail dd in vhd)
                {
                    Vehicle weight = _context.Vehicles.Where(a => a.VehicleId == dd.VehicleId).FirstOrDefault();
                    int verifyLoad = weight.NetWeight - bb;
                    if (verifyLoad <= 3000 && verifyLoad > 0)
                    {
                        waybill.VehicleId = dd.VehicleId;
                        break;
                    }
                }
            }

                  wc.CapturingStage = false;
                 _context.WayBillCaptures.Update(wc); //.State = EntityState.Modified;
                _context.SaveChanges();
            DepotVehicleDetail dvh = _context.DepotVehicleDetails.Where(a => a.VehicleId == waybill.VehicleId &&
               a.WaitingForLoad == true).FirstOrDefault();
            dvh.WaitingForLoad = false;
            dvh.UpdatedBy = 1;
            dvh.UpdatedOn = DateTime.Now;
            _context.DepotVehicleDetails.Update(dvh);
            _context.SaveChanges();

            waybill.CreatedBy = 1;
            waybill.CreatedOn = DateTime.Now;
            waybill.Weight = bb;
            waybill.Status = Status.Transit;
            _context.WayBills.Add(waybill);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWayBill", new { id = waybill.Id }, waybill);
        }

        // DELETE: api/WayBills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWayBill(int id)
        {
            var waybill = await _context.WayBills.FindAsync(id);
            if (waybill == null)
            {
                return NotFound();
            }
            _context.WayBills.Remove(waybill);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}