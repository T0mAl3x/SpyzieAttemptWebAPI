using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class PhoneRegistrationModel
    {
        public string Username { get; set; }
        public string IMEI { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Number { get; set; }
    }
}
