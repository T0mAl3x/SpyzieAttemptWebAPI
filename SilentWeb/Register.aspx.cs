using DataLayer;
using System;
using System.Configuration;
using System.Web.UI;

namespace SilentWeb
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Submit(object sender, EventArgs e)
        {
            string firstname = Request.Form["firstname"];
            string lastname = Request.Form["lastname"];
            string username = Request.Form["username"];
            string password = Request.Form["password"];
            string email = Request.Form["email"];

            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            string response = SqlHelper.RegisterUser(connectionString, firstname, lastname, username, password, email);

            if (response.Equals("Ok"))
            {
                Response.Redirect("LogIn.aspx");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "Oho!", "alert('This username is taken!')", true);
            }
        }
    }
}