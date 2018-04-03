using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class ApplicationsModel
    {
        public List<Application> Applications { get; set; }
        public string Hash { get; set; }
    }

    public class Application
    {
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
