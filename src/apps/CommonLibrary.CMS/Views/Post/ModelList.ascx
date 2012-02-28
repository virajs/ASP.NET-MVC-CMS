<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Posts.Post>>" %>
<%@ Import Namespace="ComLib.Web.Modules.Posts" %>
<%@ Import Namespace="ComLib" %>
    <br />
    <div class="posts">  
        <% 
        if (Model.Items != null && Model.Items.Count > 0)
        {
            foreach (var item in Model.Items)
            {    
                string permaLink = "/post/show/" + item.SlugUrl;
        %>
            <div class="post">
                    <span class="header"><a href="<%= permaLink  %>"><%= Html.Encode(item.Title)%></a></span><br />
                    <span class="date"><%= Html.Encode(item.PublishDate.ToString("MMMM d, yyyy  HH:mm"))%> </span> created by:
                    <span class="user"><%= Html.Encode(item.CreateUser)%> </span><br /><br />
                    <span class="content"><%= Html.Encode(item.Content)%></span><br /><br />
                    <% if (item.Category != null) { %>
                    category: <span class="category"><%= Html.Encode(item.Category.Name)%></span><br /> <% } %>        
                    tags: <span class="tags"><%= Html.Encode(item.Tags)%></span><br /><br />
                    <% Html.RenderPartial("~/views/shared/Controls/EntityActions.ascx", PostHelper.BuildActionsFor<Post>(item.IsCommentEnabled, item.CommentCount, item.Id, item.Title, true, false, true, this.Context, permaLink)); %>
                    <br />
                    <% if (Model.ShowEditDelete)
                       {%>
                            <% Html.RenderPartial("~/views/shared/Controls/EntityManage.ascx", new EntityListManageViewModel() { Id = item.Id, Item = item, ViewInfo = Model }); %>
                    <% } %>
                    <br /><br />
            </div>                        
        <%   }
        }
        %>  
    </div>

