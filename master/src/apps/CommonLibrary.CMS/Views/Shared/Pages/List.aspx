<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.EntityListViewModel>" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="ComLib.Configuration" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.HeadingForIndex(Model.Name) %>
    <% Html.RenderPartial(Model.ControlPath, Model); %>
    <% Html.RenderPager(Model.Name, Model); %>
    <br /><br />
</asp:Content>