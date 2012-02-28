<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.CMS.Areas.Admin.EmailViewModel>" %>
<%@ Import Namespace="ComLib.Web.Lib" %>
<%@ Import Namespace="ComLib.EmailSupport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Email</h2>

    <%= Html.MessageSummary() %>
    <%= Html.ValidationSummary() %>

    <% using (Html.BeginForm())
       { %>   
        <div class="form">
            <fieldset>
                <p>
                    <%= Html.ResourceFor(model => model.To)%>
                    <%= Html.TextBoxFor(model => model.To)%>
                    <%= Html.ValidationMessageFor(model => model.To)%>
                </p>
                <p>
                    <%= Html.ResourceFor(model => model.From)%>
                    <%= Html.TextBoxFor(model => model.From)%>
                </p>
                <p>
                    <%= Html.ResourceFor(model => model.Subject)%>
                    <%= Html.TextBoxFor(model => model.Subject)%>
                </p> 
                <p>
                    <%= Html.ResourceFor(model => model.Body)%>
                    <%= Html.TextAreaFor(model => model.Body, new { @class = "wide" })%>
                </p>       
                <p>
                    <%= Html.ResourceFor(model => model.SmtpServer)%>
                    <%= Html.TextBoxFor(model => model.SmtpServer)%>
                </p>
                <p>
                    <%= Html.ResourceFor(model => model.Port)%>
                    <%= Html.TextBoxFor(model => model.Port)%>
                </p>
                <p>
                    <%= Html.ResourceFor(model => model.AuthUser)%>
                    <%= Html.TextBoxFor(model => model.AuthUser)%>
                </p> 
                <p>
                    <%= Html.ResourceFor(model => model.AuthPassword)%>
                    <%= Html.TextBoxFor(model => model.AuthPassword)%>
                </p>       
            </fieldset>
        </div><br /><br />
        <input type="submit" class="action" value="Send" />        
    <% } %>
</asp:Content>