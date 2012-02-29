<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.EntityFormViewModel>" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="ComLib.Configuration" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="contentItem">
     <%= Html.HeadingForCreateEdit(Model.Name, false) %> 
     <%= Html.ValidationSummary("Errors while saving. Please correct the errors and try again.") %>
     
     <% 
         var htmlattribs = ViewData[ComLib.Web.Lib.Core.ViewDataConstants.IsMultiPartFormData] != null ? new { enctype = "multipart/form-data" } : null;
         using (Html.BeginForm(Model.FormActionName, Model.ControllerName, Model.RouteValues, FormMethod.Post, htmlattribs)) { %>
        <div class="form">
            <fieldset>
                <% Html.RenderPartial(Model.ControlPath, Model.Entity); %>
                <br />                
            </fieldset>
        </div><br /><br />
        <input type="submit" class="action" value="Save" />
        <span class="actionspacer" />
        <input type="button" class="action" value="Cancel" onclick="window.location = '<%= Model.UrlCancel %>';" /> <br /><br />              
    <% } %> 
    </div>
</asp:Content>