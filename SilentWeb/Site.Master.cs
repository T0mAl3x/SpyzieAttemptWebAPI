using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace SilentWeb
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void LogOut(object sender, System.EventArgs e)
        {
            if (Request.Cookies["Logged"] != null)
            {
                HttpCookie cookie = Request.Cookies["Logged"];
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                Response.Redirect("LogIn.aspx");
            }
        }
    }
}