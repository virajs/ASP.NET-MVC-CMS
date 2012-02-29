<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Scripts.Master" Inherits="System.Web.Mvc.ViewPage<MediaUploadFrameViewModel>" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>

<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server"> 
    
    <%= Html.ValidationSummary("Errors while saving. Please correct the errors and try again.") %>    
    <div class="messages"><%= Html.MessageSummary() %></div>

    <form method="post" enctype="multipart/form-data" action="/mediafile/doupload">
        
            <% if(Model.NumberOfUploadsAllowed > 1){ %><input type="submit" value="Upload"  class="action" /> <% } %>
            <div class="form">
                <fieldset>
                    <% Html.RenderPartial("Controls/MediaUpload", Model); %>
                </fieldset>
            </div>
            <input type="submit" value="Upload"  class="action" />
        
        <input type="hidden" id="MediaUploadFrameModel.IsFolderMode"          name="MediaUploadFrameModel.IsFolderMode" value="<%= Model.IsFolderMode %>" />
        <input type="hidden" id="MediaUploadFrameModel.ModelName"             name="MediaUploadFrameModel.ModelName" value="<%= Model.ModelName %>" />
        <input type="hidden" id="MediaUploadFrameModel.RefId"                 name="MediaUploadFrameModel.RefId" value="<%= Model.RefId %>" />
        <input type="hidden" id="MediaUploadFrameModel.NumberOfUploadsAllowed"name="MediaUploadFrameModel.NumberOfUploadsAllowed" value="<%= Model.NumberOfUploadsAllowed %>" />
        <input type="hidden" id="MediaUploadFrameModel.Width"                 name="MediaUploadFrameModel.Width" value="<%= Model.Width %>" />
        <input type="hidden" id="MediaUploadFrameModel.ShowDetailUI"          name="MediaUploadFrameModel.ShowDetailUI" value="<%= Model.ShowDetailUI %>" />
        <input type="hidden" id="MediaUploadFrameModel.JavascriptCallback"    name="MediaUploadFrameModel.JavascriptCallback" value="<%= Model.JavascriptCallback %>" />
        <input type="hidden" id="MediaUploadFrameModel.RunJavascriptCallBack" name="MediaUploadFrameModel.RunJavascriptCallBack" value="<%= Model.RunJavascriptCallBack %>" />        
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            var isUploadMode = <%=Model.IsUploadMode.ToString().ToLower() %>;
            var runCallBack = <%=Model.RunJavascriptCallBack.ToString().ToLower() %>;
            if(runCallBack)                
            {
                var code = "parent.<%=Model.JavascriptCallback %>();";
                eval(code);
            }

            if(isUploadMode)
                parent.frames[0].scrolling = "no";
        });
    </script>
</asp:Content>