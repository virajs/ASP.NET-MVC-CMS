<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Search</h2>
    <%Html.RenderPartial("SearchResults"); %>
</asp:Content>