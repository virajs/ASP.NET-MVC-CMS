<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Lib.Models.EntityDetailsViewModel>" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>
<%@ Import Namespace="ComLib.Web.Lib.Core" %>

<%
    bool isSortable = (Model.Entity is IEntitySortable);
    bool isCloneable = (Model.Entity is IEntityClonable);
    bool isActivatable = (Model.Entity is IEntityActivatable);
    bool isEditable = Auth.IsUserOrAdmin(Model.Entity.CreateUser);
%>
<% if(isEditable) { %>
<div id="entityMenu<%=Model.Name %>" ></div><br /><br />
<script type="text/javascript">

    var _entityMenu = null;
    var _entityManager = null;

    // Set up the location fields.
    $(document).ready(function () {
        _entityManager = new EntityManager("_entities", "<%=Model.Name %>", 1, 15, null, null);

        _entityMenu = new GridMenu();
        _entityMenu.IsVertical = true;
        _entityMenu.CssClass = "gridmenu";
        _entityMenu.HideOnLoad = true;
        _entityMenu.Name = "Actions";
        _entityMenu.MenuCssClass = "action";
        _entityMenu.OnMenuClickCodeTemplate = '_entityMenu.ShowMenu(${RowIndex});';
        <% if(isEditable){ %>
            <% if(isCloneable){ %>_entityMenu.Add(new GridMenuItem("copy", "copy", "/content/images/generic/item_copy.png", "", "_entityManager.Copy(${Id}, ${RowIndex}); return false;", 1));<%} %>
         _entityMenu.Add(new GridMenuItem("edit", "edit", "/content/images/generic/item_edit.png", "", "_entityManager.Edit(${Id}, ${RowIndex}); return false;", 2));
         _entityMenu.Add(new GridMenuItem("delete", "delete", "/content/images/generic/item_delete.png", "", "_entityManager.Delete(${Id}, ${RowIndex}); return false;", 3));
            <% if(isActivatable){ %>_entityMenu.Add(new GridMenuItem("onoff", "on/off", "/content/images/generic/item_disable.png", "", "_entityManager.OnOff(${Id}); return false;", 4));<% } %>
            <% if(isCloneable){ %>_entityMenu.Add(new GridMenuItem("clone", "clone", "/content/images/generic/item_clone.png", "", "_entityManager.Clone(${Id}, ${RowIndex}); return false;", 5));<% } %>        
        <% } %>
        _entityMenu.Init();
        _entityMenu.Context["RowIndex"] = <%=Model.Entity.Id %>;
        _entityMenu.Context["Id"] = <%=Model.Entity.Id %>;
        var html = _entityMenu.Build();
        var elem = document.getElementById("entityMenu<%=Model.Name %>");
        elem.innerHTML = html;
    });
    
</script>
<%= Html.Javascript("/scripts/app/EntityManager.js") %>
<%= Html.Javascript("/scripts/app/Menu.js") %>
<% } %>