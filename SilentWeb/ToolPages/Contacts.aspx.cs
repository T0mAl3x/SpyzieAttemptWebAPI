using DataLayer;
using SilentWeb.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SilentWeb.ToolPages
{
    public partial class Contacts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Logged"];
            Response.Write("<div class='container text-center'>");

            if (cookie["select"] == null)
            {
                Response.Write("<div class='alert alert-warning'><strong>Warning!</strong>You haven't selected any smartphone.</div>");
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
                DataTable table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetContacts");

                if (table.Rows.Count == 0)
                {
                    Response.Write("<div class='alert alert-info'><strong>Info!</strong>You don't have any contacts.</div>");
                }
                else
                {
                    Response.Write("<table class='table table-hover table-responsive'>");
                    Response.Write("<thead><tr><th>Icon</th><th>Name</th><th>Number</th></tr></thead>");
                    Response.Write("<tbody>");
                    foreach (DataRow row in table.Rows)
                    {
                        Response.Write("<tr>");
                        Response.Write("<td><img class='img-responsive' width='150' height='150' src='data:image/png;base64, " + Base64Helper.MakeUrlUnsafe(row["Picture"].ToString()) + "'/></td>");
                        Response.Write("<td>" + row["ContactName"].ToString() + "</td>");
                        Response.Write("<td>" + row["Number"].ToString() + "</td>");
                        Response.Write("</tr>");
                    }
                    Response.Write("</tbody>");
                    Response.Write("</table>");
                }
            }
            Response.Write("</div>");
        }
    }
}