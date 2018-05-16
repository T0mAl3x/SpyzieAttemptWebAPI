<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SilentWeb.ToolPages.Dashboard" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DataLayer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>Last location</h2>
        <div id="map" style="width:100%;height:500px"></div>
    </div>
    <%
        HttpCookie cookie = Request.Cookies["Logged"];
        bool ok = true;
        float latitude = 0;
        float longitude = 0;
        if (cookie["select"] != null)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SilentConnection"].ConnectionString;
            DataTable table = SqlHelper.GetSpecificInformation(connectionString, cookie["select"], "GetLastLocation");
            if (table != null && table.Rows.Count != 0)
            {
                latitude = float.Parse(table.Rows[0]["Lat"].ToString());
                longitude = float.Parse(table.Rows[0]["Long"].ToString());
            }
            else
            {
                ok = false;
                Response.Write("<div class='alert alert-info'><strong>Info!</strong>You don't have any locations.</div>");
            }

        }
        else
        {
            ok = false;
        }



    %>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#loadingImage').hide();
            $('#content').show();
        <%
        if (ok != false)
        {
            Response.Write("" +
            "var mapCanvas = document.getElementById('map');" +
            "var myCenter = new google.maps.LatLng(Number("+latitude+"), Number("+longitude+"));" +
            "var mapOptions = { center: myCenter, zoom: 18 };" +
            "var map = new google.maps.Map(mapCanvas, mapOptions);" +
            "var marker = new google.maps.Marker({" +
                "position: myCenter," +
                "animation: google.maps.Animation.BOUNCE" +
            "});" +
            "marker.setMap(map);");
        }
        else
        {
            Response.Write("$('#map').addClass('hidden');");
        }


        %>
    });
</script>
</asp:Content>
