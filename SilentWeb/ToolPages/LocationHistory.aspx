<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocationHistory.aspx.cs" Inherits="SilentWeb.ToolPages.LocationHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Location</h4>
      </div>
      <div class="modal-body">
        <div id="map" style="width:100%;height:500px"></div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#loadingImage').hide();
        $('#content').show();
        $('#locationTable tr').click(function () {
            var counter = 0;
            var latitude;
            var longitude;
            $(this).find('td').each(function (column, td) {
                if (counter === 0) {
                    latitude = td.innerText;
                }
                if (counter === 1) {
                    longitude = td.innerText;
                }
                counter++;
            });
            var mapCanvas = document.getElementById("map");
            var myCenter = new google.maps.LatLng(Number(latitude), Number(longitude));
            var mapOptions = { center: myCenter, zoom: 18 };
            var map = new google.maps.Map(mapCanvas, mapOptions);
            var marker = new google.maps.Marker({
                position: myCenter,
                animation: google.maps.Animation.BOUNCE
            });
            marker.setMap(map);
            google.maps.event.trigger(map, "resize");
        });
    });
    $('#myModal').on('shown.bs.modal', function () {
        google.maps.event.trigger(map, "resize");
    });
</script>
</asp:Content>
