using System;
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
            var test = (Test)serializer.Deserialize(value.CreateReader());

            Console.WriteLine(test.Text);
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