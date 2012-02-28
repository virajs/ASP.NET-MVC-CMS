<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Comments.Comment>" %>
<%if( Model != null)
  {
      CommentsViewModel commentInfo = this.ViewData["CommentsViewModel"] as CommentsViewModel;     
      %>
<h2>Add a comment</h2><br />
<div class="contentItem">
    <div class="form">
        <fieldset>
            <p>
                <%= Html.ResourceFor(model => model.Name) %>
                <%= Html.TextBoxFor(model => model.Name)%>
                <%= Html.ValidationMessageFor(model => model.Name)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Email) %>
                <%= Html.TextBoxFor(model => model.Email)%>
                <br /><span class="example">e.g. myname@gmail.com</span>
                <%= Html.ValidationMessageFor(model => model.Email)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Url) %>
                <%= Html.TextBoxFor(model => model.Url)%>
                <br /><span class="example">e.g. http://www.google.com</span>
                <%= Html.ValidationMessageFor(model => model.Url)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Content) %>
                <%= Html.TextAreaFor(model => model.Content, new { @class = "text-area" })%>
                <%= Html.ValidationMessageFor(model => model.Content) %>
            </p>
            <br />
            <% Html.RenderPartial("Controls/Captcha"); %>
        </fieldset>
   </div>
   <br /><br />
   
   <input type="button" id="commentAdd" value="Add Comment" class="action" onclick="CommentAdd('<%= commentInfo.Group %>', <%= commentInfo.RefId %>);" />  
    <script type="text/javascript">
        ///<summary>Add the comment to the system via ajax/json</summary>
        ///<param name="model">The type of the model for which this comment applies to. e.g. "post" | "event".</param>
        ///<param name="refid">The id of the model for which this comment applies to.
        function CommentAdd(model, refid) {
            var model = "post";
            var name = $('#Name').val();
            var email = $('#Email').val();
            var url = $('#Url').val();
            var content = $('#Content').val();
            var finalUrl = "/comment/add?captchatext=" + encodeURIComponent($('#CaptchaUserInput').val())
                        + "&reftext=" + encodeURIComponent($("#CaptchaGeneratedText").val())
                        + "&model=" + encodeURIComponent(model)
                        + "&refid=" + refid
                        + "&name=" + encodeURIComponent(name)
                        + "&email=" + encodeURIComponent(email)
                        + "&url=" + encodeURIComponent(url)
                        + "&content=" + encodeURIComponent(content)
                        + "&rating=" + 5;
                       
            //alert(finalUrl);
            SendActionAndDisplayResultInDivWithCallback(finalUrl, "commentActionResult", OnCommentSaveComplete);
        }


        ///<summary>Callback for when the comment is saved.</summary>
        function OnCommentSaveComplete(success)
        {
            if(success)
            {
                // Clear the existing values.
                $('#Name').val("");
                $('#Email').val("");
                $('#Url').val("");
                $('#Content').val("");
                <%= this.ViewData["CommentsReloadCallback"] %>();
            }
        }
    </script>
    <div id="commentActionResult"></div>
</div>     
<% } %>      