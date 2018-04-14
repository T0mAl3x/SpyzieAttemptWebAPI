using DataLayer;
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
    public partial class Metadata : System.Web.UI.Page
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
                DataTable table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetMetadata");

                if (table.Rows.Count == 0)
                {
                    Response.Write("<div class='alert alert-info'><strong>Info!</strong>You don't have any metadata saved.</div>");
                }
                else
                {
                    Response.Write("<table class='table table-hover table-responsive'>");
                    Response.Write("<thead><tr><th>Absolute Path</th><th>Last Accessed</th><th>Last Modified</th></tr></thead>");
                    Response.Write("<tbody>");
                    foreach (DataRow row in table.Rows)
                    {
                        Response.Write("<tr>");
                        Response.Write("<td>" + row["AbsolutePath"].ToString() + "</td>");
                        Response.Write("<td>" + row["LastAccessed"].ToString() + "</td>");
                        Response.Write("<td>" + row["LastModified"].ToString() + "</td>");
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