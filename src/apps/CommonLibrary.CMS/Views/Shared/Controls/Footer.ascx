<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="ComLib.Web.Modules.Pages" %>

    <% 
       foreach (var page in ComLib.Web.Modules.Pages.Page.FrontPages())
       { %>
          <a href="<%= page.UrlAbsolute %>"><%= page.Title %></a>
    <% } %>
    <a href="/termsofuse">terms of use</a> <a href="/privacy">privacy</a>
    <span class="copyright">@2010 CopyRight - CodeHelix Solutions - Powered by CommonLibrary.NET CMS</span> 