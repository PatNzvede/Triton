using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class WayBillCapturesController : Controller
    {
        string Baseurl = "https://localhost:44363/";
        // Get Search for waybill status
        public async Task<IActionResult> Index(bool? search)
        {
            IEnumerable<WayBillCapture> waybillList;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/WayBillCaptures?search=" + search);

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var WayBillsResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    waybillList = JsonConvert.DeserializeObject<IEnumerable<WayBillCapture>>(WayBillsResponse);
                    return View(waybillList);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WayBillCapture waybill)
        {
            var way = new Vehicle();
            waybill.CreatedBy = 1;
            //vehicle.IsActive = true;
            waybill.CreatedOn = DateTime.Now;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync("api/WayBillCaptures", waybill);

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var WaybillsResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing 
                    way = JsonConvert.DeserializeObject<Models.Vehicle>(WaybillsResponse);


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