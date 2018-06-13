using DataLayer;
using SilentWeb.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
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
            Response.Write("<img id='loadingImage' width='100' height='100' style='position:absolute;margin-top:-100px;margin-left:-100px;top:50% !important;left:50% !important;' src='../Pictures/loading.gif'/>");
            Response.Write("<div id='content' class='container' style='display:none'>");

            if (cookie["select"] == null)
            {
                Response.Write("<div class='alert alert-warning'><strong>Warning!</strong>You haven't selected any smartphone.</div>");
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
                

                String graphVizString = Graphviz.GenerateInput(connectionString, cookie["username"]);
                Bitmap bm = new Bitmap(Graphviz.RenderImage(graphVizString, "jpg"));
                ImageConverter converter = new ImageConverter();
                Response.Write("<img style='display:block;margin:0 auto;' src='data:image/png;base64, " + Convert.ToBase64String((byte[])converter.ConvertTo(bm, typeof(byte[]))) + "'/></span>");

                DataTable table;
                DataTable tableNr = SqlHelper.GetSpecificInformation(connectionString, cookie["username"], "GetRegisteredPhones");
                Response.Write("<div class='row'>");
                Response.Write("<div class='col-md-12'>");
                Response.Write("<div class='jumbotron'>");
                if (tableNr != null && tableNr.Rows.Count != 0)
                {
                    Response.Write("<h2>Contact agenda between registered smartphones</h2>");
                    Response.Write("<br>");
                    foreach (DataRow row in tableNr.Rows)
                    {
                        Response.Write("<h4>"+ row["Manufacturer"].ToString() + " " + row["Model"].ToString() + " " + row["IMEI"].ToString() +"</h4>");
                        table = SqlHelper.GetSpecificInformation(connectionString, row["IMEI"].ToString(), "CompareContacts");
                        Response.Write("<table class='table table-hover table-responsive'>");
                        Response.Write("<thead><tr><th>Manufacturer</th><th>Model</th><th>IMEI</th><th>Check</th></tr></thead>");
                        Response.Write("<tbody>");
                        foreach (DataRow roww in table.Rows)
                        {
                            Response.Write("<tr class='success'>");
                            Response.Write("<td>" + roww["Manufacturer"].ToString() + "</td>");
                            Response.Write("<td>" + roww["Model"].ToString() + "</td>");
                            Response.Write("<td>" + roww["IMEI"].ToString() + "</td>");
                            if (roww["Checkk"].ToString() == "1")
                            {
                                Response.Write("<td><img width='30' height='30' src='../Pictures/check.png'/></td>");
                            }
                            else
                            {
                                Response.Write("<td><img width='30' height='30' src='../Pictures/X.png'/></td>");
                            }
                            Response.Write("</tr>");
                        }
                        Response.Write("</tbody>");
                        Response.Write("</table>");
                    }
                }
                else
                {
                    Response.Write("<div class='alert alert-warning'><strong>Info!</strong>You don't have any phones registered.</div>");
                }
                Response.Write("</div>");
                Response.Write("</div>");
                Response.Write("</div>");


                table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetMostCalled");

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
                        if (counter == 5)
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
                        if (counter == 5)
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
                    Response.Write("<table class='table table-hover'>");
                    Response.Write("<tr><td>Contact name: </td><td>" + table.Rows[0]["Name"].ToString() + "</td></tr>");
                    Response.Write("<tr><td>Date: </td><td>" + table.Rows[0]["Date"].ToString() + "</td></tr>");
                    Response.Write("</table>");
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