<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.LogOnModel>" %>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Resource("Account", "ForgotPassword", "Forgot Password") %></h2>
    
    <%= Html.ValidationSummary("Invalid Email. Please correct the errors and try again.") %>
    <div class="message"><%= this.ViewData["Message"] %></div>
    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>                
                <p>
                    <%= Html.Resource("Account", "Email", "Email") %>
                    <%= Html.TextBox("emailaddress") %>
                </p>
                <p>
                    <input type="submit" class="action" value="Send Password" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
