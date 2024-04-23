using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.PerformanceData;
using CustomerDetails.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;




namespace CustomerDetails.Controllers
{

    public class HomeController : Controller
    {
        

  

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        //o
        private static List<CountryData> _countries = new List<CountryData>();

        public ActionResult Index(string searchString, int? page)
        {
            LoadCountriesFromJson(); 

            if (!string.IsNullOrEmpty(searchString))
            {
                _countries = FilterCountriesBySearchString(_countries, searchString);
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;
            var pagedCountries = GetPagedCountries(_countries, pageNumber, pageSize);

            var viewModel = new PagedListViewModel<CountryData>(
                pagedCountries,
                pageNumber,
                pageSize,
                _countries.Count
            );

            return View(viewModel);
        }

        private void LoadCountriesFromJson()
        {
            string filePath = Server.MapPath("~/countries.json");

            if (!System.IO.File.Exists(filePath))
            {
                return;
            }

            string jsonData = System.IO.File.ReadAllText(filePath);
            _countries = JsonConvert.DeserializeObject<List<CountryData>>(jsonData);
        }

        private List<CountryData> FilterCountriesBySearchString(List<CountryData> countries, string searchString)
        {
            return countries.Where(c =>
                c.Name.ToLower().Contains(searchString.ToLower()) ||
                c.Address.ToLower().Contains(searchString.ToLower()) ||
                c.City.ToLower().Contains(searchString.ToLower()) ||
                c.Pincode.ToLower().Contains(searchString.ToLower()) ||
                c.Country.ToLower().Contains(searchString.ToLower())
            ).ToList();
        }

        private IEnumerable<CountryData> GetPagedCountries(List<CountryData> countries, int pageNumber, int pageSize)
        {
            return countries.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
















        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CountryData country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    string filePath = Server.MapPath("~/countries.json");

                   
                    string jsonData = System.IO.File.ReadAllText(filePath);

                    
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<CountryData> countries = serializer.Deserialize<List<CountryData>>(jsonData);

                    
                    countries.Add(country);

                   
                    string newJsonData = serializer.Serialize(countries);

                    
                    System.IO.File.WriteAllText(filePath, newJsonData);

                    
                    _countries = countries;

                    
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError("", "An error occurred while saving data.");
                }
            }

     
            return View(country);
        }


       
        public ActionResult Details(int? id)
        {
            var country = _countries.FirstOrDefault(c => c.Id == id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }



        // GET: Country/Create
        public ActionResult Create()
        {
            return View();
        }


        
        public ActionResult Edit(int id)
        {
            var country = _countries.FirstOrDefault(c => c.Id == id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CountryData editedCountry)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   
                    string filePath = Server.MapPath("~/countries.json");
                    string jsonData = System.IO.File.ReadAllText(filePath);

                    
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<CountryData> countries = serializer.Deserialize<List<CountryData>>(jsonData);

                    
                    int index = countries.FindIndex(c => c.Id == editedCountry.Id);
                    if (index != -1)
                    {
                        
                        countries[index].Name = editedCountry.Name;
                        countries[index].Address = editedCountry.Address;
                        countries[index].City = editedCountry.City;
                        countries[index].Pincode = editedCountry.Pincode;
                        countries[index].Country = editedCountry.Country;

                        string newJsonData = serializer.Serialize(countries);

                       
                        System.IO.File.WriteAllText(filePath, newJsonData);

                        _countries = countries;

                        
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError("", "An error occurred while saving data.");
                }
            }
            
            return View(editedCountry);
        }

        
        public ActionResult Delete(int? id)
        {
            var country = _countries.FirstOrDefault(c => c.Id == id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCountry(int id)
        {
            
            var countryToDelete = _countries.FirstOrDefault(c => c.Id == id);

           
            if (countryToDelete == null)
            {
                
                return RedirectToAction("Error");
            }

           
            _countries.Remove(countryToDelete);

           
            string json = JsonConvert.SerializeObject(_countries);

            
            string filePath = Server.MapPath("~/countries.json");
            System.IO.File.WriteAllText(filePath, json);

            
            return RedirectToAction("Index");
        }


    }
}
