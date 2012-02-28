<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.BoolMessageItem<ComLib.KeyValue<ComLib.Web.Lib.Services.Information.InfoAttribute,ComLib.Web.Lib.Services.Information.IInformation>>>" %>
<%@ Import Namespace="ComLib.BootStrapSupport" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <% if (Model.Success)
      { %>
      <h2><%= Model.Item.Key.Name%></h2>
      <p><%= Model.Item.Key.Description%></p><br />
      <%= Model.Item.Value.GetInfo()%>    
   <% }
      else
      { %>
      <%= Model.Message %>
   <% } %>
   <br /><br /><br />
</asp:Content>
