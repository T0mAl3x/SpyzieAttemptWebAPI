using DataLayer;
using SilentWeb.Models;
using SilentWeb.Services;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace SilentWeb.Controllers
{
    public class ServiceController : ApiController
    {
        [HttpPost]
        public string RegisterPhone([FromBody] PhoneRegistrationModel value)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            return Base64Helper.Encode(SqlHelper.RegisterPhone(connectionString, Base64Helper.Decode(value.IMEI), Base64Helper.Decode(value.Manufacturer),
            Base64Helper.Decode(value.Model), Base64Helper.Decode(value.Username)));
        }
    }
}