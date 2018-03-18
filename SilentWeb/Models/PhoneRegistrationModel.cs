using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilentWeb.Models
{
    public class PhoneRegistrationModel
    {
        public string Username { get; set; }
        public string IMEI { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
    }
}