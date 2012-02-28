<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Media.MediaUploadFrameViewModel>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Web.Lib.Core" %>
<%
    var manageurl = "/mediafile/managebyfolder/" + Model.RefId;
    if (!Model.IsFolderMode)
        manageurl = "/mediafile/managebyrefid/" + Model.RefId + "?refgroup=" + ModuleMap.Instance.GetId(Model.ModelName);
    
%>
<h3>Images</h3>
<a href="#" class="actionbutton" id="showimageuploadui" onclick="$('#mediaupload').toggle();return false;">Add</a>
<%if(Model.ShowManageLink) { %> <a href="<%=manageurl %>" class="actionbutton">Manage</a> <% } %>

<div id="mediaupload" style="display:none"><br /> 
    <iframe src="<%=Model.FullFrameSourceUrl %>" width="<%=Model.Width + 30 %>" height="<%=Model.Height %>" frameborder="0" scrolling="<%=Model.ScrollMode %>" marginheight="0" marginwidth="0" style="background-color:White">
        <p>Your browser does not support iframes.</p>
    </iframe>
</div>