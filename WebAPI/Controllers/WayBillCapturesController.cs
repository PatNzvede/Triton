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
    public class WayBillCapturesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WayBillCapturesController(AppDbContext _context)
        {
            this._context = _context;
        }

        // GET: api/WayBills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WayBillCapture>>> GetWayBillCaptures(bool? search)
        {
            return await _context.WayBillCaptures.Where(a => a.CapturingStage == search || search == null).ToListAsync();
        }

        // GET: api/WayBills/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<WayBillCapture>> GetWayBillCapture(int id)
        {
            var waybill = await _context.WayBillCaptures.FindAsync(id);

            if (waybill == null)
            {
                return NotFound();
            }
            return waybill;
        }

        // PUT: api/WayBills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWayBillCapture(int id, WayBillCapture waybill)
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
        public async Task<ActionResult<WayBillCapture>> PostWayBillCapture(WayBillCapture waybill)
        {
            var way = await _context.WayBillCaptures.FindAsync(waybill.Waybillno);
            if (way != null)
            {
                waybill.CreatedBy = 1;
                waybill.CreatedOn = DateTime.Now;
                _context.WayBillCaptures.Add(waybill);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetWayBillCapture", new { id = waybill.Id }, waybill);
            }
            else
            {
                return new WayBillCapture();

            }
        }

        // DELETE: api/WayBills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWayBillCapture(int id)
        {
            var waybill = await _context.WayBillCaptures.FindAsync(id);
            if (waybill == null)
            {
                return NotFound();
            }
            _context.WayBillCaptures.Remove(waybill);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
