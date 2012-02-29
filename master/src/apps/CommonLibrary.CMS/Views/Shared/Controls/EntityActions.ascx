<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PostActionsViewModel>" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>
<%@ Import Namespace="ComLib.Web.Modules.Posts" %>


<span class="actions">
    <a rel="nofollow" href="mailto:?subject=<%=Html.Encode(Model.EmailSubject) %>&amp;body=<%= Html.Encode(Model.EmailBody) %>">E-mail</a> | 
    <a href="<%= Model.PermaLink %>">Permalink</a> 
    <% if (Model.EnableComments) { %>| <a href="<%= Model.PermaLink %>">Comments (<%=Model.CommentCount %>)</a> <% } %>
    <% if (Model.EnableFavorites){ %>| <a href="#" onclick="SendEntityActionAndDisplayResult('favorite', '<%= Model.EntityId %>', '<%= Html.Encode(Model.EmailSubject) %>', '<%= Html.Encode(Model.PermaLink) %>'); return false;">Favorite</a> <% } %>
    <% if (Model.EnableFlaging)  { %>| <a href="#" onclick="SendEntityActionAndDisplayResult('flag', '<%= Model.EntityId %>', '<%= Html.Encode(Model.EmailSubject) %>', '<%= Html.Encode(Model.PermaLink) %>'); return false;">Flag</a> <% } %>
</span><br />
<div id="entityActionResult_<%= Model.EntityId %>"></div>

<% if (this.Context.Items["addedEntityActionJS"] == null)
   { %>
       
    <script type="text/javascript">
        ///<summary>Sends the entity action either "adding favorite" or "adding a flag/reporting"
        /// to the server and then displays the servers result in a divId.
        ///</summary>
        ///<param name="entityAction">Corresponds to the controller name. (e.g. "Flag" or "Favorite".</param>
        ///<param name="refid">The id of the entity to perform the action on.</param>
        ///<param name="title">A descriptive title for the entity to perform the action on.</param>
        ///<param name="permalink">The absolute url for the entity to perform the action on.</param> 
        function SendEntityActionAndDisplayResult(entityAction, refid, title, permalink) {
            var model = "<%= Model.EntityName %>";
            var finalUrl = "/" + entityAction + "/add?model=" + encodeURIComponent(model)
                         + "&refid=" + encodeURIComponent(refid)
                         + "&title=" + encodeURIComponent(title)
                         + "&url=" + encodeURIComponent(permalink);
            SendActionAndDisplayResultInDiv(finalUrl, "entityActionResult_" + refid);
        }
    </script>
   
   <% this.Context.Items["addedEntityActionJS"] = true;
   }%>