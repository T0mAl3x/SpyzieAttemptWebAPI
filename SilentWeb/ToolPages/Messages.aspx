<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Messages.aspx.cs" Inherits="SilentWeb.ToolPages.Messages" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(window).ready(function () {
            $('#loadingImage').hide();
            $('#content').show();
        });
        function manageChatBox(i, total) {
            $('#' + i).toggleClass('hidden');

            if (!$('#' + i).hasClass('hidden'))
                for (var j = 0; j < total; j++) {
                    if (!$('#' + j).hasClass('hidden') && j != i) {
                        $('#' + j).addClass('hidden');
                    }
            }
        }
    </script>
</asp:Content>
