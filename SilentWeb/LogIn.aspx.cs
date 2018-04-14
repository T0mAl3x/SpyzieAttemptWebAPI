using DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SilentWeb
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Submit(object sender, EventArgs e)
        {
            string username = Request.Form["username"];
            string password = Request.Form["password"];

            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            bool response = SqlHelper.LogInUser(connectionString, username, password);

            if (response)
            {
                HttpCookie cookie = new HttpCookie("Logged");
                cookie["username"] = username;
                cookie.Expires = DateTime.Now.AddDays(20);
                Response.Cookies.Add(cookie);
                Response.Redirect("Default.aspx");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "Oho!", "alert('Log in failed!')", true);
            }
        }
    }
}