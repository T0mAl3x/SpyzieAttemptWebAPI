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
    public partial class LocationHistory : System.Web.UI.Page
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
                DataTable table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetLocationHistory");

                if (table.Rows.Count == 0)
                {
                    Response.Write("<div class='alert alert-info'><strong>Info!</strong>You don't have any location saved.</div>");
                }
                else
                {
                    Response.Write("<table id='locationTable' class='table table-hover table-responsive'>");
                    Response.Write("<thead><tr><th>Latitude</th><th>Longitude</th><th>Location Date</th></tr></thead>");
                    Response.Write("<tbody>");
                    foreach (DataRow row in table.Rows)
                    {
                        Response.Write("<tr data-toggle='modal' data-target='#myModal'>");
                        Response.Write("<td>" + row["Latitude"].ToString() + "</td>");
                        Response.Write("<td>" + row["Longitude"].ToString() + "</td>");
                        Response.Write("<td>" + row["LocationDate"].ToString() + "</td>");
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