<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Pages.Page>" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>
<%@ Import Namespace="ComLib.CMS" %>

<% 
   var content = CMS.Macros.BuildContent(Model.Content);
    %>    
<%= content %>


