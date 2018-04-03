using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class PhotosModel
    {
        public List<Photo> Photos { get; set; }
        public string Hash { get; set; }
    }

    public class Photo
    {
        public string Image { get; set; }
        public string Date { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
