<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="SilentWeb.LogIn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron text-center">
        <div class="row">
            <div class="col-md-6 col-md-offset-3 text-center" style="align-items:center !important">
                <asp:Image runat="server" ImageUrl="~/Pictures/logo.ico" Width="200px" Height="200px"/>
                <br />
                <div class="input-group" style="margin-left:50px">
                    <input type="text" class="form-control" placeholder="Username" name="username" />
                    <input type="password" class="form-control" placeholder="Password" name="password" />
                </div>
                <br />
                <asp:Button runat="server" Text="Log in" CssClass="btn btn-success" OnClick="Submit" />
            </div>
        </div>
    </div>
</asp:Content>
