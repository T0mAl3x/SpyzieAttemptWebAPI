using SilentWeb.Models;
using System;
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
            securityToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(securityToken));

            return MakeUrlSafe(securityToken);
        }

        private string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, 32)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string MakeUrlSafe(string securityToken)
        {
            return securityToken.Replace('+', '-').Replace('/', '_');
        }
    }
}