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
    public class VehiclesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VehiclesController(AppDbContext _context)
        {
            this._context = _context;
        }

        // GET: api/Vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles(string? search)
        {
            return await _context.Vehicles.Where(a => a.Model == search || search == null
            ).ToListAsync();
        }

        // GET: api/Vehicles/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }
            return vehicle;
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle( Vehicle vehicle)
        {
            //if (id != vehicle.Id)
            //{
            //    return BadRequest();
            //}
            string th = vehicle.RegNumber;
            if (th != null)
            {
                string ccd = th.Substring(0, 2);
                if (ccd == "ND")
                {
                    vehicle.Depot = Depot.Durban;
                }
                else
                {
                    vehicle.Depot = Depot.Johannesburg;
                }
            }
            else {
                return BadRequest("Registration number should be either Durban or Johannesburg");
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

        // POST: api/Vehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            Vehicle veh = _context.Vehicles.OrderByDescending(a => a.VehicleId).FirstOrDefault();
            if (veh != null)
            {
                int test = int.Parse(veh.VehicleId);
                vehicle.VehicleId = (test + 1).ToString();
            }
            string th = vehicle.RegNumber;
            if (th != null)
            {
                string ccd = th.Substring(0, 2);
                if (ccd == "ND")
                {
                    vehicle.Depot = Depot.Durban;
                }
                else
                {
                    vehicle.Depot = Depot.Johannesburg;
                }
            }
            else
            {
                return BadRequest("Registration number should be either Durban or Johannesburg");
            }
            vehicle.IsActive = true;
            vehicle.CreatedBy = 1;
            vehicle.CreatedOn = DateTime.Now;
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicle);
        }

        // DELETE: api/Vehicles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}