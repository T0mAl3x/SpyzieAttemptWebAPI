using DataLayer;
using System;
using System.Configuration;
using System.Data;
using System.Web;

namespace SilentWeb.ToolPages
{
    public partial class Smartphones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie cookie = Request.Cookies["Logged"];
            string username = "";
            if (cookie != null)
            {
                username = cookie["username"];
            }
            Response.Write("<div class='container text-center'>");
            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
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
            Response.Write("</div>");

            string input = Request.Form["select"];
            if (input != null)
            {
                cookie["select"] = input;
            }
            Response.Cookies.Add(cookie);
        }
    }
}