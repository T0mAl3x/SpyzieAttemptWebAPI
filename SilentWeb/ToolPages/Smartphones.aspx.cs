using DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace SilentWeb.ToolPages
{
    public partial class Smartphones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            HttpCookie cookie = Request.Cookies["Logged"];
            string username = "";
            if (cookie != null)
            {
                username = cookie["username"];

                Response.Write("<div class='container text-center'>");
                string userMask = SqlHelper.GetUserMask(connectionString);
                if (userMask != "" && userMask != null)
                {
                    Response.Write("<button type='button' class='btn btn - info btn - lg' data-toggle='modal' data-target='#myModal'>Set mask</button>");
                    Response.Write("<table class='table table-stripped table-responsive>'");
                    Response.Write("<thead><tr><th>Photos</th><th>Contacts</th><th>Call History</th><th>Messages</th><th>Trafic</th><th>Installed Apps</th><th>Location</th><th>Battery</th></tr></thead>");
                    Response.Write("<tbody>");
                    Response.Write("<tr>");
                    Response.Write("<td>" + getUserMaskColumn(userMask[0]) + "</td>");
                    Response.Write("<td>" + getUserMaskColumn(userMask[1]) + "</td>");
                    Response.Write("<td>" + getUserMaskColumn(userMask[2]) + "</td>");
                    Response.Write("<td>" + getUserMaskColumn(userMask[3]) + "</td>");
                    Response.Write("<td>" + getUserMaskColumn(userMask[4]) + "</td>");
                    Response.Write("<td>" + getUserMaskColumn(userMask[5]) + "</td>");
                    Response.Write("<td>" + getUserMaskColumn(userMask[6]) + "</td>");
                    Response.Write("<td>" + getUserMaskColumn(userMask[7]) + "</td>");
                    Response.Write("</tr>");
                    Response.Write("</tbody>");
                    Response.Write("</table>");


                    DataTable table = SqlHelper.GetSmartphones(connectionString, username);
                    if (table.Rows.Count == 0)
                    {
                        Response.Write("<div class='alert alert-info'><strong>Info!</strong>You don't have any registered smartphone.</div>");
                    }
                    else
                    {
                        Response.Write("<form method='POST' id='smartphonesForm'>");
                        Response.Write("<input type='hidden' id='smartphonesInput' name='select'/>");
                        Response.Write("</form>");
                        Response.Write("<table id='smartphonesTable' class='table table-hover table-responsive'>");
                        Response.Write("<thead><tr><th>Manufacturer</th><th>Model</th><th>IMEI</th><th>Battery level</th></tr></thead>");
                        Response.Write("<tbody>");
                        bool first = true;
                        foreach (DataRow row in table.Rows)
                        {
                            if (first)
                            {
                                cookie["select"] = row["IMEI"].ToString();
                                first = false;
                            }
                            Response.Write("<tr>");
                            Response.Write("<td>" + row["Manufacturer"].ToString() + "</td>");
                            Response.Write("<td>" + row["Model"].ToString() + "</td>");
                            Response.Write("<td>" + row["IMEI"].ToString() + "</td>");
                            Response.Write("<td>" + row["BatteryLevel"].ToString() + "</td>");
                            Response.Write("</tr>");
                        }
                        Response.Write("</tbody>");
                        Response.Write("</table>");
                    }

                    string input = Request.Form["select"];
                    if (input != null)
                    {
                        cookie["select"] = input;
                    }
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    Response.Write("<div class='alerboott alert-info'><strong>Info!</strong>You don't have any smartphones registered.</div>");
                }
                Response.Write("</div>");
            }
        }

        private string getUserMaskColumn(char c)
        {
            if (c == '1')
            {
                return "yes";
            }
            return "no";
        }

        protected void Submit(object sender, EventArgs e)
        {
            StringBuilder userMask = new StringBuilder();
            if (photos.Checked)
            {
                userMask.Append("1");
            }
            else
            {
                userMask.Append("0");
            }

            if (contacts.Checked)
            {
                userMask.Append("1");
            }
            else
            {
                userMask.Append("0");
            }

            if (callhistory.Checked)
            {
                userMask.Append("1");
            }
            else
            {
                userMask.Append("0");
            }

            if (messages.Checked)
            {
                userMask.Append("1");
            }
            else
            {
                userMask.Append("0");
            }

            if (trafic.Checked)
            {
                userMask.Append("1");
            }
            else
            {
                userMask.Append("0");
            }

            if (apps.Checked)
            {
                userMask.Append("1");
            }
            else
            {
                userMask.Append("0");
            }

            if (location.Checked)
            {
                userMask.Append("1");
            }
            else
            {
                userMask.Append("0");
            }

            if (battery.Checked)
            {
                userMask.Append("1");
            }
            else
            {
                userMask.Append("0");
            }

            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            SqlHelper.SetUserMask(connectionString, userMask);
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }
    }
}