<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Lib.Models.PostActionsViewModel>" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>

<span class="actions">
    Actions: 
        <a rel="nofollow" href="mailto:?subject=<%=Model.EmailSubject %>&amp;body=<%= Model.EmailBody %>">E-mail</a> | 
        <a href="<%= Model.PermaLink %>">Permalink</a> 
        <% if (Model.CommentCountEnabled)
           { %>|
            <a href="<%= Model.PermaLink %>">Comments (<%=Model.CommentCount %>)</a>
        <% } %>
</span>