<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CallHistory.aspx.cs" Inherits="SilentWeb.ToolPages.CallHistory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(window).ready(function () {
            $('#loadingImage').hide();
            $('#content').show();
        });
    </script>
</asp:Content>
