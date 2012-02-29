<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Lib.Models.EntityListViewModel>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Paging" %>
<% 
    string pagerHtml = string.Empty;
    if (Model.TotalPages > 1)
    {
        if (!Model.UrlIndex.EndsWith("/"))
            Model.UrlIndex += "/";
        pagerHtml = PagerBuilderWeb.Instance.Build(Model.PageIndex, Model.TotalPages, page => Model.UrlIndex + page);
    }
%>

<div class="pager">
    <%=pagerHtml %>
</div>

