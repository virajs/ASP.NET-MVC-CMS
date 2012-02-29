<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Lib.Models.CommentsViewModel>" %>
<%@ Import Namespace="ComLib.Paging" %>

<%= Html.Javascript("/scripts/app/commentlist.js") %>
<%= Html.Javascript("/scripts/app/pagerajax.js") %>

<br /><h2>Comments</h2><br />
<div id="comments"></div>
<br /><br />
<!-- 2. Client side javascript ajax pager -->
<% Html.RenderPartial("Controls/PagerAjax", new PagerAjaxViewModel(){ JSObjectName = "_pager", PageSize = 15, CssClassCurrent = "current",  OnPageSelectedCallBack = "EntityManage_OnPageSelected" }); %>
<br /><br />

<script type="text/javascript">    
    var _comments = null;

    $(document).ready(function() {
        _comments = new CommentsListControl(<%=Model.PageSize %>, "<%=Model.Group %>", <%=Model.RefId %>, "<%=Model.Url %>", "<%=Model.PostAuthor %>", "comments", null);
        _comments.OnLoadCompleteCallback = function (page, total) { _pager.SetPageData(page, total); };
        _pager.OnPageSelectedCallback = function (page) { _comments.Load(page); };
        _comments.Load(1);
    });

        
    function Comments_Reload() {
        _comments.Load(_pager.CurrentPage);
    }
    
</script>


<%= Html.ValidationSummary("Errors while saving. Please correct the errors and try again.") %>    
<% using (Html.BeginForm(Model.SubmitActionName, Model.SubmitControllerName, new { id = Model.RefId }))
   { %>
    <%  // Comments may be disabled for the respective post, or duration for commenting may have expired.
    if (Model.EnableCommentAdding)
    {
        this.ViewData["CommentsViewModel"] = Model;
        this.ViewData["CommentsReloadCallback"] = "Comments_Reload";
        %>
        
        <% Html.RenderPartial("Controls/CommentAdd", new ComLib.Web.Modules.Comments.Comment()); %>
    <%} %>
<%} %>