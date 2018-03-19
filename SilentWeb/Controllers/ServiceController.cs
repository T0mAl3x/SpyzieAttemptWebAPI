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
            string securityToken = RandomString();
            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            SqlHelper.RegisterPhone(connectionString, Base64Helper.Decode(value.IMEI), Base64Helper.Decode(value.Manufacturer),
                Base64Helper.Decode(value.Model), securityToken, Base64Helper.Decode(value.Username));

            return Base64Helper.Encode(securityToken);
        }

        private string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, 32)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}