<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.EntityFormViewModel>" %>
<%@ Import Namespace="ComLib.Authentication" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="contentItem">
     
    <%= Html.HeadingForCreateEdit(Model.Name, true) %> 
    <%= Html.ValidationSummary("Errors while saving. Please correct the errors and try again.") %>
        
    <% using (Html.BeginForm(Model.FormActionName, Model.ControllerName, Model.RouteValues, FormMethod.Post, new { enctype = "multipart/form-data" }))
       {    %>
        <div class="form">
            <fieldset>
                <% Html.RenderPartial(Model.ControlPath, Model.Entity); %>
                <br />           
            </fieldset>
        </div><br /><br />
        <div class="centered"><input type="submit" class="action" value="Create" />
        <span class="actionspacer" />
        <input type="button" class="action" value="Cancel" onclick="window.location = '<%= Model.UrlCancel %>';" /></div>      
    <% } %>    
    </div>  
</asp:Content>