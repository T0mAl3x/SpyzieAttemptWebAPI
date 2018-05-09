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
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Logged"];
            Response.Write("<div class='container'>");

            if (cookie["select"] == null)
            {
                Response.Write("<div class='alert alert-warning'><strong>Warning!</strong>You haven't selected any smartphone.</div>");
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
                DataTable table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetMostCalled");

                Response.Write("<div class='row'>");
                Response.Write("<div class='col-md-6'>");
                Response.Write("<div class='jumbotron'>");
                
                if (table != null && table.Rows.Count != 0)
                {
                    Response.Write("<h2>Five most called contacts</h2>");
                    Response.Write("<table class='table table-hover table-responsive'>");
                    Response.Write("<thead><tr><th>Name</th><th>Number</th></tr></thead>");
                    Response.Write("<tbody>");
                    int counter = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        Response.Write("<tr>");
                        Response.Write("<td>"+ row["Name"].ToString() +"</td>");
                        Response.Write("<td>" + row["Number"].ToString() + "</td>");
                        Response.Write("</tr>");
                        counter++;
                        if (counter == 4)
                        {
                            break;
                        }
                    }
                    Response.Write("</tbody>");
                    Response.Write("</table>");
                }
                else
                {
                    Response.Write("<div class='alert alert-warning'><strong>Info!</strong>You don't have a call history.</div>");
                }
                Response.Write("</div>");
                Response.Write("</div>");

                table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetMostMessaged");
                Response.Write("<div class='col-md-6'>");
                Response.Write("<div class='jumbotron'>");
                if (table != null && table.Rows.Count != 0)
                {
                    Response.Write("<h2>Five most messaged contacts</h2>");
                    Response.Write("<table class='table table-hover table-responsive'>");
                    Response.Write("<thead><tr><th>Name</th><th>Number</th></tr></thead>");
                    Response.Write("<tbody>");
                    int counter = 0;
                    foreach (DataRow row in table.Rows)
                    {
                        Response.Write("<tr>");
                        Response.Write("<td>" + row["Name"].ToString() + "</td>");
                        Response.Write("<td>" + row["Number"].ToString() + "</td>");
                        Response.Write("</tr>");
                        counter++;
                        if (counter == 4)
                        {
                            break;
                        }
                    }
                    Response.Write("</tbody>");
                    Response.Write("</table>");
                }
                else
                {
                    Response.Write("<div class='alert alert-warning'><strong>Info!</strong>You don't have any messages.</div>");
                }
                Response.Write("</div>");
                Response.Write("</div>");
                Response.Write("</div>");

                table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetLastCall");
                Response.Write("<div class='row'>");
                Response.Write("<div class='col-md-6'>");
                Response.Write("<div class='jumbotron'>");

                if (table != null && table.Rows.Count != 0)
                {
                    Response.Write("<h2>Last call</h2>");
                    Response.Write("<table class='table table-hover table-responsive'>");
                    Response.Write("<thead><tr><th>Name</th><th>Number</th><th>Date</th></tr></thead>");
                    Response.Write("<tbody>");
                    Response.Write("<tr>");
                    Response.Write("<td>" + table.Rows[0]["Name"].ToString() + "</td>");
                    Response.Write("<td>" + table.Rows[0]["Number"].ToString() + "</td>");
                    Response.Write("<td>" + table.Rows[0]["Date"].ToString() + "</td>");
                    Response.Write("</tr>");
                    Response.Write("</tbody>");
                    Response.Write("</table>");
                }
                else
                {
                    Response.Write("<div class='alert alert-warning'><strong>Info!</strong>You don't have a call history.</div>");
                }
                Response.Write("</div>");
                Response.Write("</div>");

                table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetLastMessage");
                Response.Write("<div class='col-md-6'>");
                Response.Write("<div class='jumbotron'>");
                if (table != null && table.Rows.Count != 0)
                {
                    Response.Write("<h2>Last message</h2>");
                    Response.Write("<p class='bg-info'>Contact name</p>");
                    Response.Write("<p>" + table.Rows[0]["Name"].ToString() +"</p>");
                    Response.Write("<p class='bg-info'>Date</p>");
                    Response.Write("<p>" + table.Rows[0]["Date"].ToString() + "</p>");
                    Response.Write("<p>"+ table.Rows[0]["Body"].ToString() + "</p>");
                }
                else
                {
                    Response.Write("<div class='alert alert-warning'><strong>Info!</strong>You don't have any messages.</div>");
                }
                Response.Write("</div>");
                Response.Write("</div>");
                Response.Write("</div>");

            }
            Response.Write("</div>");
        }
    }
}