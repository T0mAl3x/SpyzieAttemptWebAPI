using DataLayer;
using DataLayer.Models;
using SilentWeb.Services;
using System.Configuration;
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

        [HttpPost]
        public string GetMask([FromBody] PhoneAuthenticationModel credentials)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            return Base64Helper.Encode(SqlHelper.GetMask(connectionString, Base64Helper.Decode(credentials.IMEI), Base64Helper.Decode(credentials.SecToken)));
        }

        [HttpPost]
        public IHttpActionResult GatherData([FromBody] BulkDataModel bulkData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            //SqlHelper.GatherData(connectionString, bulkData);

            return Ok();
        }
    }
}