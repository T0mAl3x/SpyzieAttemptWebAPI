using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class LocationModel
    {
        public List<Location> Locations { get; set; }
        public string Hash { get; set; }
    }

    public class Location
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Date { get; set; }
    }
}
