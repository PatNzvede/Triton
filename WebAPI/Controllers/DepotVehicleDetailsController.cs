using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepotVehicleDetailsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DepotVehicleDetailsController(AppDbContext _context)
        {
            this._context = _context;
        }
        // GET: api/DepotVehicleDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepotVehicleDetail>>> GetDepotVehicleDetails(bool? search)
        {

            return await _context.DepotVehicleDetails.Where(a => a.WaitingForLoad == search || search == null).ToListAsync();
        }

        // GET: api/WayBills/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DepotVehicleDetail>> GetDepotVehicleDetail(int id)
        {
            var vehicle = await _context.DepotVehicleDetails.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }
            return vehicle;
        }

        // PUT: api/WayBills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepotVehicleDetail(int id, DepotVehicleDetail vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest();
            }
            _context.Entry(vehicle).State = EntityState.Modified;

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
        public async Task<ActionResult<DepotVehicleDetail>> PostDepotVehicleDetail(DepotVehicleDetail vehicle)
        {
            DepotVehicleDetail dvh = _context.DepotVehicleDetails.Where(a => a.VehicleId == vehicle.VehicleId
                                    && a.WaitingForLoad == true).FirstOrDefault();

            WayBill wb = _context.WayBills.Where(a => a.VehicleId == vehicle.VehicleId
                                   && a.Status == Status.Transit).FirstOrDefault();
            if (dvh == null && wb == null)
            {
                vehicle.CreatedBy = 1;
                vehicle.CreatedOn = DateTime.Now;

                _context.DepotVehicleDetails.Add(vehicle);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("Vehicle already in the queque, cannot be added again");
            }

            return CreatedAtAction("GetDepotVehicleDetail", new { id = vehicle.Id }, vehicle);
        }

        // DELETE: api/WayBills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepotVehicleDetail(int id)
        {
            var vehicle = await _context.DepotVehicleDetails.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            _context.DepotVehicleDetails.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}