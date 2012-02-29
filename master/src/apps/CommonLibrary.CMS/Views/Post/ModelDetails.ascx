<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Post>" %>
<%@ Import Namespace="ComLib.Web.Modules.Posts" %>
    
    <div class="post">
        <span class="header"><%= Html.Encode(Model.Title)%></span><br />
        <span class="date"><%= Html.Encode(Model.PublishDate.ToString("MMMM d, yyyy  HH:mm"))%> </span> created by:
        <span class="user"><%= Html.Encode(Model.CreateUser)%> </span><br /><br />
        <span class="content"><%= Model.Content%></span><br /><br />
        <% if (Model.Category != null) { %>
        category: <span class="category"><%= Html.Encode(Model.Category.Name)%></span><br /> <% } %>
        tags: <span class="tags"><%= Html.Encode(Model.Tags)%></span><br /><br /><br />
        
        <div class="actions">
            <% Html.RenderPartial("~/views/shared/Controls/EntityActions.ascx", PostHelper.BuildActionsFor<Post>(Model.IsCommentEnabled, Model.CommentCount, Model.Id, Model.Title, true, false, true, this.Context)); %>
        </div>
        
        <% Html.RenderPartial("~/views/shared/Controls/Comments.ascx", new ComLib.Web.Lib.Models.CommentsViewModel() 
           {    PostAuthor = Model.CreateUser, Group = "blog", 
                PageSize = 15, RefId = Model.Id, Url = "/comment/ByGroupAndRefId?",
                 SubmitControllerName = "post", SubmitActionName = "SubmitComment",
                EnablePaging = true, EnableCommentAdding = true
           }); %>
    </div>


