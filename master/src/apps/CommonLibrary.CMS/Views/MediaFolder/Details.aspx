<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.EntityDetailsViewModel>" %>
<%@ Import Namespace="ComLib.Authentication" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
        
    <% if (Auth.IsUserOrAdmin(Model.Entity.CreateUser))
       { %>    
        <div class="actionbuttonarea">
            <%= Html.ActionLink("Manage Files", "ManageByFolder", "MediaFile", new { id = Model.Entity.Id },     new { @class = "actionbutton" })%> 
        </div>
    <% } %>
    
        
    <% Html.RenderPartial(Model.ControlPath, Model.Entity); %>
</asp:Content>