<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.EntityDeletionViewModel>" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3><%= Model.Message %></h3>
    <br /><br />

    <a href="<%= Model.UrlManage %>">Manage <%= Model.Name %>(s)</a>
    
</asp:Content>