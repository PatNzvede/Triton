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
    public class VehiclesController : Controller
    {
        string Baseurl = "https://localhost:44363/";
        //Get Search By model
        public async Task<IActionResult> Index(string? search)
        {
            IEnumerable<Vehicle> vehicleList;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Vehicles?search=" + search);

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var VehiclesResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    vehicleList = JsonConvert.DeserializeObject<IEnumerable<Vehicle>>(VehiclesResponse);
                    return View(vehicleList);
                }
            }
            return View();
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Vehicle vehicle = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync($"api/Vehicles/{id}");

                if (Res.IsSuccessStatusCode)
                {
                    vehicle = await Res.Content.ReadAsAsync<Vehicle>();
                    TempData["AlertMessage"] = "Item display successfully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,RegNumber,Depot,Model,NetWeight,IsActive,CreatedBy,CreatedOn")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
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
                       
                        HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(vehicle),Encoding.UTF8,"application/json");
                        HttpResponseMessage Res = await client.PutAsync($"api/Vehicles/{id}", httpContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            vehicle = await Res.Content.ReadAsAsync<Vehicle>();
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
            return RedirectToAction(nameof(Index)) ;  
        }
        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vehicle vehicle = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync($"api/Vehicles/details/{id}");

                if (Res.IsSuccessStatusCode)
                {
                    vehicle = await Res.Content.ReadAsAsync<Vehicle>();
                    TempData["AlertMessage"] = "Item display successfully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(vehicle);

          //  return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            var vh = new Models.Vehicle();
            vehicle.CreatedBy = 1;
            vehicle.IsActive = true;
            vehicle.CreatedOn = DateTime.Now;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.PostAsJsonAsync("api/Vehicles", vehicle);

                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var VehiclesResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing 
                    vh = JsonConvert.DeserializeObject<Models.Vehicle>(VehiclesResponse);
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