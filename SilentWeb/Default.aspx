<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SilentWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- First Container -->
    <div class="container-fluid text-center">
      <h3 class="margin">Who Are We?</h3>
      <img src="./Pictures/logo.ico" class="img-responsive img-circle margin" style="display:inline" alt="Bird" width="350" height="350" runat="server">
      <h3>We are Spyer-Beta. The first free monitoring system for android.</h3>
    </div>

    <!-- Third Container (Grid) -->
    <div class="container-fluid text-center" style="background-color:#ffffff; color: #555555">    
      <h3 class="margin">Some of the interesting functionalities...</h3><br>
      <div class="row">
        <div class="col-sm-4">
          <p>You can easily track your device even with you location settings off!</p>
          <img src="./Pictures/location.jpg" class="img-responsive" style="width:100%; height:150px" alt="Image">
        </div>
        <div class="col-sm-4"> 
          <p>Find out what files have you recently accessed!</p>
          <img src="./Pictures/files.jpg" class="img-responsive" style="width:100%; height:150px" alt="Image">
        </div>
        <div class="col-sm-4"> 
          <p>Keep an eye on what is being typed on you smartphone device.</p>
          <img src="./Pictures/keylogger.jpeg" class="img-responsive" style="width:100%; height:150px" alt="Image">
        </div>
      </div>
    </div>

</asp:Content>
