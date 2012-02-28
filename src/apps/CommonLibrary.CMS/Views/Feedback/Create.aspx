<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.EntityFormViewModel>" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="ComLib.Extensions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="contentItem">
    <div class="header"><h2>Submit Feedback</h2></div><br />    
    <%= Html.ValidationSummary("Errors while saving. Please correct the errors and try again.") %>
        
    <% using (Html.BeginForm()) { %>
        <div class="form">
            <fieldset>
                <% Html.RenderPartial(Model.ControlPath, Model.Entity); %>
                <br /> 
                <p>
                    <% Html.RenderPartial("Controls/Captcha"); %>
                </p>               
            </fieldset>
        </div><br /><br />
        <div class="centered"><input type="submit" class="action" value="Create" /></div>      
    <% } %>    
    </div>  
</asp:Content>