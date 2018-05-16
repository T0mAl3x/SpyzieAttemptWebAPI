<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs" Inherits="SilentWeb.ToolPages.Contacts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(window).ready(function () {
            $('#loadingImage').hide();
            $('#content').show();
        });
    </script>
</asp:Content>
