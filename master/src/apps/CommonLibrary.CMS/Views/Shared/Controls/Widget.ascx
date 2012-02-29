<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Widgets.WidgetInstance>" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>
<div class="widget">
    <%if(Auth.IsAdmin()) { %>
    <div class="weditarea">
        <a href="/widget/edit/<%= Model.Id %>">Edit</a>&nbsp;&nbsp;&nbsp;<a href="/widget/delete/<%= Model.Id %>">X</a>
    </div>
    <% } %>
    
    <div class="wheader"> <%= Html.Encode(Model.Header) %></div><br />
    <% if (Model.IsSelfRenderable)
       {
           string widgetHtml = Model.Render();
           %>
        <div class="wbody"> <%= widgetHtml %></div><br /><br />
    <% }
       else
       {
           string path = Model.Definition.Path;
           path = "~/views/shared/" + path + ".ascx";
    %>            
        <div class="wbody"> <% Html.RenderPartial(path, Model); %></div><br /><br />
    <% } %>
</div>

