<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Mvc.Models.Conference>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
  <% using (Html.BeginForm("Save", "Home"))
     { %>
        <%= Html.EditorFor(m => Model) %>
        <input type="submit" value="Save" />
  <% } %>
</asp:Content>
