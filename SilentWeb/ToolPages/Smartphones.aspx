<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Smartphones.aspx.cs" Inherits="SilentWeb.ToolPages.Smartphones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Modal -->
  <div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Modal Header</h4>
        </div>
        <div class="modal-body text-center">
            <div class="checkbox"><asp:CheckBox ID="photos" runat="server" />Photos</div>
            <div class="checkbox"><asp:CheckBox ID="contacts" runat="server" />Contacts</div>
            <div class="checkbox"><asp:CheckBox ID="callhistory" runat="server" />Call History</div>
            <div class="checkbox"><asp:CheckBox ID="messages" runat="server" />Messages</div>
            <div class="checkbox"><asp:CheckBox ID="trafic" runat="server" />Trafic</div>
            <div class="checkbox"><asp:CheckBox ID="apps" runat="server" />Apps</div>
            <div class="checkbox"><asp:CheckBox ID="location" runat="server" />Location</div>
            <div class="checkbox"><asp:CheckBox ID="keylogger" runat="server" />Keylogger</div>
            <div class="checkbox"><asp:CheckBox ID="battery" runat="server" />Battery</div>
            <br />
            <asp:Button runat="server" Text="Submit" CssClass="btn btn-success" OnClick="Submit" />
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
  </div>
    <script type="text/javascript">
        $(window).ready(function () {
            $('#loadingImage').hide();
            $('#content').show();
        });
    </script>
</asp:Content>
