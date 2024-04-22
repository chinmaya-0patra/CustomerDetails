using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CustomerDetails.Models
{
    public class CountryData
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
     
        public string Pincode { get; set; }
        
        public string Country { get; set; }
        
    }
}