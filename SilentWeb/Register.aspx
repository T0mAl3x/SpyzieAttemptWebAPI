<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SilentWeb.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron text-center">
        <div class="row">
            <div class="col-md-6 col-md-offset-3 text-center">
                <asp:Image runat="server" ImageUrl="~/Pictures/logo.ico" Width="200px" Height="200px"/>
                <br />
                <div class="input-group">
                    <input type="text" class="form-control" placeholder="Firstname" name="firstname" />
                    <input type="text" class="form-control" placeholder="Lastname" name="lastname" />
                    <input type="text" class="form-control" placeholder="Username" name="username" />
                    <input type="password" class="form-control" placeholder="Password" name="password" />
                    <input type="password" class="form-control" placeholder="Retype Password" name="rpassword" />
                    <input type="email" class="form-control" placeholder="Email" name="email" />
                </div>
                <br />
                <asp:Button runat="server" Text="Register" CssClass="btn btn-success" OnClick="Submit" />
            </div>
        </div>
    </div>
</asp:Content>
