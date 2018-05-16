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
    public partial class Photos : System.Web.UI.Page
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
                DataTable table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetPhotos");

                if (table.Rows.Count == 0)
                {
                    Response.Write("<div class='alert alert-info'><strong>Info!</strong>You don't have any photos.</div>");
                }
                else
                {
                    Response.Write("<table id='photosTable' class='table table-hover table-responsive'>");
                    Response.Write("<thead><tr><th>Photo</th><th>Date Taken</th><th>Latitude</th><th>Longitude</th></tr></thead>");
                    Response.Write("<tbody>");
                    foreach (DataRow row in table.Rows)
                    {
                        string latitude = row["Latitude"].ToString();
                        string longitude = row["Longitude"].ToString();
                        if (latitude.Equals("") || longitude.Equals(""))
                        {
                            latitude = "none";
                            longitude = "none";
                        }
                        Response.Write("<tr data-toggle='modal' data-target='#myModal'>");
                        Response.Write("<td><img class='img-responsive' width='150' height='150' src='data:image/png;base64, " + Base64Helper.MakeUrlUnsafe(row["Image"].ToString()) + "'/></td>");
                        Response.Write("<td>" + row["Date"].ToString() + "</td>");
                        Response.Write("<td>" + latitude + "</td>");
                        Response.Write("<td>" + longitude + "</td>");
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