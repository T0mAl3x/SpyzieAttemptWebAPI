using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SilentWeb.Controllers
{
    public class ServiceController : ApiController
    {
        [HttpPost]
        public void Post([FromBody] XElement value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Test));
            var test = serializer.Deserialize(value.CreateReader());
        }

        [XmlRoot("Test")]
        public class Test
        {
            [XmlElement("Id")]
            public int Id { get; set; }
            [XmlElement("Text")]
            public string Text { get; set; }
        }
    }
}