<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<KeyValuePair<string, string>>" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="System.Collections.Generic" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Model.Key %></h2>
    <p>
        <%= Model.Value %>
    </p>
</asp:Content>