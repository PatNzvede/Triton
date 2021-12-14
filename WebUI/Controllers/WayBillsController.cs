using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebUI.Models;
namespace WebUI.Controllers
{
    public class WayBillsController : Controller
    {
        string Baseurl = "https://localhost:44363/";
        //Get Search By Status
        public async Task<IActionResult> Index(string? search)
        {
            IEnumerable<WayBill> waybillList;
            using (var client = new HttpClient())
            { 
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/WayBills?search=" + search);

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var WayBillsResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    waybillList = JsonConvert.DeserializeObject<IEnumerable<Models.WayBill>>(WayBillsResponse);
                    return View(waybillList);
                }
            }
            return View();
        }

        // GET: WayBills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            WayBill waybill = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync($"api/WayBills/{id}");

                if (Res.IsSuccessStatusCode)
                {
                    waybill = await Res.Content.ReadAsAsync<WayBill>();
                    TempData["AlertMessage"] = "Item display successfully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(waybill);
        }

        // POST: WayBills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,Destination,LoadingFrom,Status,Weight,Waybillno,CreatedBy,CreatedOn")] WayBill waybill)
        {
            if (id != waybill.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(waybill), Encoding.UTF8, "application/json");
                        HttpResponseMessage Res = await client.PutAsync($"api/WayBills/{id}", httpContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            waybill = await Res.Content.ReadAsAsync<WayBill>();
                            TempData["AlertMessage"] = "Item display successfully";
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error try after some time.");
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;

                }
            }
            return RedirectToAction(nameof(Index));
        }

       [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WayBill waybill)
        {
            var way = new WayBill();
            way.CreatedOn = DateTime.Now;
            way.CreatedBy = 1;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync("api/WayBills", waybill);

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var WayBillResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing 
                    way = JsonConvert.DeserializeObject<WayBill>(WayBillResponse);
                    TempData["AlertMessage"] = "Item created successfully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    return View();
                }
                return RedirectToAction("Index");
            }
        }
    }
}
