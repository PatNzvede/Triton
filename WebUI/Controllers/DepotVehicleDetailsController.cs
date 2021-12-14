using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using WebUI.Models;
using System.Threading.Tasks;
using System.Text;

namespace WebUI.Controllers
{
    public class DepotVehicleDetailsController : Controller
    {
        string Baseurl = "https://localhost:44363/";
        //Get Search By waiting for load
        public async Task<IActionResult> Index(bool? search)
        {
            IEnumerable<DepotVehicleDetail> vehicleList;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/DepotVehicleDetails?search=" + search);

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var VehiclesResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    vehicleList = JsonConvert.DeserializeObject<IEnumerable<Models.DepotVehicleDetail>>(VehiclesResponse);
                    return View(vehicleList);
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
        public async Task<IActionResult> Create(DepotVehicleDetail vehicle)
        {
            var vh = new DepotVehicleDetail();
            vehicle.CreatedBy = 1;
            vehicle.CreatedOn = DateTime.Now;
            vehicle.UpdatedBy = 1;
            vehicle.UpdatedOn = DateTime.Now;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync("api/DepotVehicleDetails", vehicle);

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var VehiclesResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing 
                    vh = JsonConvert.DeserializeObject<DepotVehicleDetail>(VehiclesResponse);
                   // TempData["AlertMessage"] = "Item created successfully";
                    ViewBag.Result = "Saved Succesfully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    return View();
                }
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Details(string id)
        {
            DepotVehicleDetail vehicle = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync($"api/DepotVehicleDetails/details/{id}");

                if (Res.IsSuccessStatusCode)
                {
                    vehicle = await Res.Content.ReadAsAsync<DepotVehicleDetail>();
                    TempData["AlertMessage"] = "Item display successfully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(vehicle);
        }
       
       public async Task<ActionResult> Edit(int id)
        {
            DepotVehicleDetail vehicle = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync($"api/DepotVehicleDetails/{id}");

                if (Res.IsSuccessStatusCode)
                {
                    vehicle = await Res.Content.ReadAsAsync<DepotVehicleDetail>();
                    TempData["AlertMessage"] = "Item edit successfully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DepotVehicleDetail vehicle)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(vehicle), Encoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PutAsync($"api/DepotVehicleDetails/{vehicle.Id}", httpContent);
                    
                    if (Res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string id)
        {
            DepotVehicleDetail vehicle = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync($"api/DepotVehicleDetails/details/{id}");

                if (Res.IsSuccessStatusCode)
                {
                    vehicle = await Res.Content.ReadAsAsync<DepotVehicleDetail>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
           return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.DeleteAsync($"api/DepotVehicleDetails/delete/{id}");
                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View();
        }
    }
}
