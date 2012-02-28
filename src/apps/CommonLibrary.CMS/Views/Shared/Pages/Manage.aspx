<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.EntityListViewModel>" %>
<%@ Import Namespace="ComLib.Web.Lib.Extensions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.HeadingForManage(Model.Name) %>
    <% Html.RenderPartial(Model.ControlPath, Model); %><br /><br /><br />
    <% Html.RenderPager(Model.Name, Model); %>
    <br /><br />
</asp:Content>