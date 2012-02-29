<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.ChangePasswordModel>" %>


<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.ValidationSummary() %><br />
    <%= Html.MessageSummary() %><br />
    <h2>Account Removal</h2>        
    <p>
        You can remove yourself from the system by clicking the "Remove account" button below.
        Afterwards, you will no longer be able to login and/or create any content on the site.
    </p>
    <a href="/account/removeconfirmed?username=<%= this.Request.Params["username"] %>">Remove account</a>
</asp:Content>
