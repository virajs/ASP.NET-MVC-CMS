<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.EntityDetailsViewModel>" %>
<%@ Import Namespace="ComLib.Authentication" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("Controls/EntityMenu", Model); %>
    <% Html.RenderPartial(Model.ControlPath, Model.Entity); %>
</asp:Content>