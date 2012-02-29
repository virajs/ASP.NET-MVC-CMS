<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Lib.Models.EntityListManageViewModel>" %>
<%@ Import Namespace="ComLib.Entities" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>
<%@ Import Namespace="ComLib.Authentication" %>

<%  
    bool canEdit = Model.Item is IEntity
                 ? Auth.IsUserOrAdmin(((IEntity)Model.Item).CreateUser)
                 : Auth.IsAdmin();    
    if(canEdit){ %>
<table>
    <tr><% if (Model.ShowCopy)   { %><td style="border-style:none"><%= Html.ActionLinkImage("Copy",  Model.ViewInfo.ControllerName,  new { id = Model.Id }, null, "/content/images/generic/item_copy.png",    "copy")%></td> <% } %>
        <% if (Model.ShowEdit)   { %><td style="border-style:none"><%= Html.ActionLinkImage("Edit",   Model.ViewInfo.ControllerName, new { id = Model.Id },  null, "/content/images/generic/item_edit.png",   "copy")%></td> <% } %>
        <% if (Model.ShowDelete) { %><td style="border-style:none"><%= Html.ActionLinkImage("Delete", Model.ViewInfo.ControllerName, new { id = Model.Id },  null, "/content/images/generic/item_delete.png", "copy")%></td> <% } %>
    </tr>
</table>
<%} %>