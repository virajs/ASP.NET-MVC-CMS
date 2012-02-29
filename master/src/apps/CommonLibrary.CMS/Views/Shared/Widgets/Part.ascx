<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Widgets.WidgetInstance>" %>
<%@ Import Namespace="ComLib.Web.Modules.Parts" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>
<%@ Import Namespace="ComLib.Data" %>
<%
    ComLib.Web.Modules.Parts.Part item = null;
    string content = string.Empty;
    
    if (!string.IsNullOrEmpty(Model.RefGroup))
    {
        // WebPart name collision... hence the verbosity.
        var items = ComLib.Web.Modules.Parts.Part.Repository.Find(Query<ComLib.Web.Modules.Parts.Part>.New().Where(p => p.Title).Is(Model.RefGroup));
        if (items != null && items.Count > 0)
            item = items[0];
    }
    else
        item = ComLib.Web.Modules.Parts.Part.Get(Model.Id);

    content = item == null ? string.Empty : item.Content;
       
%>
    <%= content %>
    

