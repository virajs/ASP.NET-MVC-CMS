<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Scripts.Master" Inherits="System.Web.Mvc.ViewPage<MediaUploadFrameViewModel>" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>

<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server"> 
    <form method="post" enctype="multipart/form-data" action="<%= Model.ActionUrl %>">       
        <div class="form">
            <fieldset>
                <% Html.RenderPartial("Controls/MediaUpload", Model); %>
            </fieldset>
        </div>  
        <input type="submit" value="Upload"  class="action" />
        <input type="button" value="Test Parent" onclick="parent.TestChildCallBack();" />
    </form>
</asp:Content>