<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="ComLib.Web.Modules.MenuEntrys" %>
  

        <% 
           foreach (var menuItem in MenuEntry.FrontPages())
           { %>
              <a href="<%= menuItem.UrlAbsolute %>"><%= menuItem.Name %></a>
        <% } %> 