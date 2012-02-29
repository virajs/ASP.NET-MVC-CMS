<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="ComLib.Authentication" %>

<% if (Auth.IsAuthenticated()) 
   { %>
        Welcome <b><%= Html.Encode(Page.User.Identity.Name) %></b>&nbsp;&nbsp;&nbsp;&nbsp;<br /><br />
        <a class="dashboardlink" href="/admin/console/home" >Dashboard</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="/account/logoff">Log Off</a>&nbsp;&nbsp;&nbsp;&nbsp;
<% } else { %> 
        <br /><a href="/account/logon">Log On</a>&nbsp;&nbsp;&nbsp;&nbsp;<a href="/account/register">Register</a>&nbsp;&nbsp;&nbsp;&nbsp;
<%  } %>
