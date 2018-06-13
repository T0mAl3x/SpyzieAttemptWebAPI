using DataLayer;
using DataLayer.Models;
using SilentWeb.Services;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SilentWeb.Controllers
{
    public class ServiceController : ApiController
    {
        [HttpPost]
        public string RegisterPhone([FromBody] PhoneRegistrationModel value)
        {
            if (DataChecker.CheckPhoneRegistration(value))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
                return Base64Helper.Encode(SqlHelper.RegisterPhone(connectionString, Base64Helper.Decode(value.IMEI), Base64Helper.Decode(value.Manufacturer),
                Base64Helper.Decode(value.Model), Base64Helper.Decode(value.Username), Base64Helper.Decode(value.Number)));
            }
            else
            {
                return Base64Helper.Encode("fail");
            }
        }

        [HttpPost]
        public string AuthentificateUserFromPhone([FromBody] UserAuthenticationModel value)
        {
            if (DataChecker.CheckUserCredentials(value))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
                if (SqlHelper.LogInUser(connectionString, Base64Helper.Decode(value.Username), Base64Helper.Decode(value.Password)))
                {
                    return Base64Helper.Encode("success");
                }
                else
                {
                    return Base64Helper.Encode("fail");
                }
            }
            else
            {
                return Base64Helper.Encode("fail");
            }
            
        }

        [HttpPost]
        public string GetMask([FromBody] PhoneAuthenticationModel credentials)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            return Base64Helper.Encode(SqlHelper.GetMask(connectionString, Base64Helper.Decode(credentials.IMEI), Base64Helper.Decode(credentials.SecToken)));
        }

        [HttpPost]
        public IHttpActionResult GatherAllData([FromBody] BulkDataModel bulkData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (bulkData == null)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Data is null" };
                throw new HttpResponseException(msg);
            }

            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            string sqlResponse = SqlHelper.GatherData(connectionString, Base64Helper.DecodeBulk(bulkData));

            if (sqlResponse == "")
            {
                return Ok();
            }
            else
            {
                if (sqlResponse.Equals("Authentication failed"))
                {
                    //var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Authentication failed" };
                    //throw new HttpResponseException(msg);
                }
                return Ok();
            }
        }
    }
}