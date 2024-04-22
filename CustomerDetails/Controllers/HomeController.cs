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



        private static List<CountryData> _countries = new List<CountryData>();

        // GET: Country
        //public ActionResult Index(string searchString, int? page)
        //{
        //    // Define JSON file path
        //    string filePath = Server.MapPath("~/countries.json");

        //    // Check if the file exists
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        // Handle the case where the file does not exist (e.g., create an empty file or return a view with an error message)
        //        return HttpNotFound();
        //    }

        //    // Read existing JSON data from file
        //    string jsonData = System.IO.File.ReadAllText(filePath);

        //    // Deserialize JSON data to list of CountryData objects
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    List<CountryData> countries = serializer.Deserialize<List<CountryData>>(jsonData);

        //    // Filter countries based on search string
        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        countries = countries.Where(c =>
        //            c.Name.ToLower().Contains(searchString.ToLower()) ||
        //            c.Address.ToLower().Contains(searchString.ToLower()) ||
        //            c.City.ToLower().Contains(searchString.ToLower()) ||
        //            c.Pincode.ToLower().Contains(searchString.ToLower()) ||
        //            c.Country.ToLower().Contains(searchString.ToLower())
        //        ).ToList();
        //    }

        //    // Determine page size
        //    int pageSize = 5;

        //    // Calculate total count and total pages for paging
        //    int totalCount = countries.Count;
        //    int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        //    // Set current page number
        //    int pageNumber = page ?? 1;

        //    // Retrieve records for the current page
        //    var pagedCountries = countries.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        //    // Create PagedListViewModel instance
        //    var viewModel = new PagedListViewModel<CountryData>(
        //        pagedCountries,
        //        pageNumber,
        //        pageSize,
        //        totalCount
        //    );

        //    return View(viewModel);
        //}
        // GET: Country
        public ActionResult Index(string searchString, int? page)
        {
            LoadCountriesFromJson(); // Load countries from JSON file

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
















        // POST: Country/Create
        // POST: Country/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CountryData country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Define JSON file path
                    string filePath = Server.MapPath("~/countries.json");

                    // Read existing JSON data from file
                    string jsonData = System.IO.File.ReadAllText(filePath);

                    // Deserialize JSON data to list of CountryData objects
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<CountryData> countries = serializer.Deserialize<List<CountryData>>(jsonData);

                    // Add new country to the list
                    countries.Add(country);

                    // Serialize list of CountryData objects to JSON format
                    string newJsonData = serializer.Serialize(countries);

                    // Write updated JSON data back to file
                    System.IO.File.WriteAllText(filePath, newJsonData);

                    // Update _countries list
                    _countries = countries;

                    // Redirect to the index page
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Handle exception (e.g., log error)
                    ModelState.AddModelError("", "An error occurred while saving data.");
                }
            }

            // If ModelState is not valid or an error occurred, return to the create view with validation errors
            return View(country);
        }


        // GET: Country/Details/5
        //GET: Country/Details/5
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


        // GET: Country/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    var country = _countries.FirstOrDefault(c => c.Id == id);
        //    if (country == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(country);
        //}

        //// POST: Edit
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(CountryData editedCountry)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Read existing JSON data from file
        //            string filePath = Server.MapPath("~/countries.json");
        //            string jsonData = System.IO.File.ReadAllText(filePath);

        //            // Deserialize JSON data to list of CountryData objects
        //            JavaScriptSerializer serializer = new JavaScriptSerializer();
        //            List<CountryData> countries = serializer.Deserialize<List<CountryData>>(jsonData);

        //            // Find the index of the country object to be updated
        //            int index = countries.FindIndex(c => c.Id == editedCountry.Id);
        //            if (index != -1)
        //            {
        //                // Update the properties of the country object in the list
        //                countries[index].Name = editedCountry.Name;
        //                countries[index].Address = editedCountry.Address;
        //                countries[index].City = editedCountry.City;
        //                countries[index].Pincode = editedCountry.Pincode;
        //                countries[index].Country = editedCountry.Country;

        //                // Serialize the updated list to JSON
        //                string newJsonData = serializer.Serialize(countries);

        //                // Write updated JSON data back to file
        //                System.IO.File.WriteAllText(filePath, newJsonData);

        //                // Redirect to the Index page
        //                return RedirectToAction("Index");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exception (e.g., log error)
        //            ModelState.AddModelError("", "An error occurred while saving data.");
        //        }
        //    }
        //    // If the model state is not valid or an error occurred, return to the Edit view with validation errors
        //    return View(editedCountry);
        //}




        //// GET: Country/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    var country = _countries.FirstOrDefault(c => c.Id == id);
        //    if (country == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(country);
        //}

        //// POST: Country/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteCountry(int id)
        //{
        //    // Find the country to delete
        //    var countryToDelete = _countries.FirstOrDefault(c => c.Id == id);

        //    // If countryToDelete is null, the country with the specified ID was not found
        //    if (countryToDelete == null)
        //    {
        //        // Handle the case where the country to delete was not found
        //        // For example, display an error message or redirect to an error page
        //        return RedirectToAction("Error");
        //    }

        //    // Remove the country from the list
        //    _countries.Remove(countryToDelete);

        //    // Serialize the updated list to JSON
        //    string json = JsonConvert.SerializeObject(_countries);

        //    // Write the JSON data back to the file
        //    string filePath = Server.MapPath("~/countries.json");
        //    System.IO.File.WriteAllText(filePath, json);

        //    // Redirect to the Index page
        //    return RedirectToAction("Index");
        //}
        // GET: Country/Edit/5
        
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
                    // Read existing JSON data from file
                    string filePath = Server.MapPath("~/countries.json");
                    string jsonData = System.IO.File.ReadAllText(filePath);

                    // Deserialize JSON data to list of CountryData objects
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<CountryData> countries = serializer.Deserialize<List<CountryData>>(jsonData);

                    // Find the index of the country object to be updated
                    int index = countries.FindIndex(c => c.Id == editedCountry.Id);
                    if (index != -1)
                    {
                        // Update the properties of the country object in the list
                        countries[index].Name = editedCountry.Name;
                        countries[index].Address = editedCountry.Address;
                        countries[index].City = editedCountry.City;
                        countries[index].Pincode = editedCountry.Pincode;
                        countries[index].Country = editedCountry.Country;

                        // Serialize the updated list to JSON
                        string newJsonData = serializer.Serialize(countries);

                        // Write updated JSON data back to file
                        System.IO.File.WriteAllText(filePath, newJsonData);

                        // Update _countries list
                        _countries = countries;

                        // Redirect to the Index page
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception (e.g., log error)
                    ModelState.AddModelError("", "An error occurred while saving data.");
                }
            }
            // If the model state is not valid or an error occurred, return to the Edit view with validation errors
            return View(editedCountry);
        }

        // GET: Country/Delete/5
        public ActionResult Delete(int? id)
        {
            var country = _countries.FirstOrDefault(c => c.Id == id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: Country/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCountry(int id)
        {
            // Find the country to delete
            var countryToDelete = _countries.FirstOrDefault(c => c.Id == id);

            // If countryToDelete is null, the country with the specified ID was not found
            if (countryToDelete == null)
            {
                // Handle the case where the country to delete was not found
                // For example, display an error message or redirect to an error page
                return RedirectToAction("Error");
            }

            // Remove the country from the list
            _countries.Remove(countryToDelete);

            // Serialize the updated list to JSON
            string json = JsonConvert.SerializeObject(_countries);

            // Write the JSON data back to the file
            string filePath = Server.MapPath("~/countries.json");
            System.IO.File.WriteAllText(filePath, json);

            // Redirect to the Index page
            return RedirectToAction("Index");
        }


    }
}
