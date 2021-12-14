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
    public class WayBillItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public WayBillItemsController(AppDbContext _context)
        {
            this._context = _context;
        }
        // GET: api/WayBills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WayBillItem>>> GetWayBillItems(string? search)
        {
            return await _context.WayBillItems.Where(a => a.WayBillno == search || search == null).ToListAsync();
        }

        // GET: api/WayBills/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<WayBillItem>> GetWayBillItem(int id)
        {
            var waybill = await _context.WayBillItems.FindAsync(id);

            if (waybill == null)
            {
                return NotFound();
            }
            return waybill;
        }

        // PUT: api/WayBills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWayBillItem(int id, WayBillItem waybill)
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
        public async Task<ActionResult<WayBillItem>> PostWayBillItem(WayBillItem waybill)
        {
            string code = "";
            if(waybill.WayBillno != null)
            {
                var uwh = _context.WayBillCaptures.Where(a => a.CapturingStage == true &&
                            a.Waybillno == waybill.WayBillno).FirstOrDefault();
                if(uwh == null)
                {
                   
                    return BadRequest("The waybill you are using is not active");
                }
            }


                WayBillItem cc = _context.WayBillItems.OrderByDescending(a => a.Id).FirstOrDefault();
                List<WayBillCapture> wbc = _context.WayBillCaptures.Where(a => a.CapturingStage == true).ToList();
                if(cc== null && wbc.Count() ==0 )
                {
                    code = "TRIT0001";
                }
                foreach (WayBillCapture vv in wbc)
                {
                    int bb = _context.WayBillItems.Where(a => a.WayBillno == vv.Waybillno).Sum(a => a.Weight);
                    if (bb + waybill.Weight < 34000)
                    {
                        code = vv.Waybillno;
                        waybill.WayBillno = code;
                        break;
                    }
                }
                if(waybill.WayBillno == null)
                { 
                    string th = Regex.Match(cc.WayBillno, @"\d+").Value;
                    int ccd = Int32.Parse(th);
                    int idd = Int32.Parse(th);
                    idd++;
                    code = "TRIT" + idd.ToString("D4");

                    WayBillCapture wc = new WayBillCapture();
                    wc.CapturingStage = true;
                    wc.CreatedBy = 1;
                    wc.CreatedOn = DateTime.Now;
                    wc.Waybillno = code;
                    _context.WayBillCaptures.Add(wc);
                }                
                waybill.WayBillno = code;
           //
            waybill.CreatedBy = 1;
            waybill.CreatedOn = DateTime.Now;
            _context.WayBillItems.Add(waybill);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetWayBillItem", new { id = waybill.Id }, waybill);
        }

        // DELETE: api/WayBills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWayBillItem(int id)
        {
            var waybill = await _context.WayBillItems.FindAsync(id);
            if (waybill == null)
            {
                return NotFound();
            }
            _context.WayBillItems.Remove(waybill);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}