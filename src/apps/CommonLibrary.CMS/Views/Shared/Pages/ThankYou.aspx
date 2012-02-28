<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="ComLib.Authentication" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% if (Model != null && Model.GetType() == typeof(string))
       { %>
            <h2><%= Model%></h2>
    <% } %>
</asp:Content>