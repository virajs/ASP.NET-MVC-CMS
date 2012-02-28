<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>
<%  
    // Determine if owner of widgets.
    bool isAdmin = false;
    string zone = Model as string;
    ComLib.ToDo.Implement(ComLib.ToDo.Priority.Normal, "Kishore", "This needs to use tentant id for multi-tenancy", () => isAdmin = Auth.IsAdmin());    
    
    // The widget loader can be slightly optimized to load all the user's widgets at one go instead of on each call to loading zone.    
    var widgetService = this.Context.Items["widget_loader"] as WidgetService;
    if (widgetService == null)
    {
        widgetService = new WidgetService();
        this.Context.Items["widget_loader"] = widgetService;
    }
    TimeSpan start = DateTime.Now.TimeOfDay;

    // This bypassas the call to load widgets individually via the widget.ascx control and insteads renders the widgets here.
    // This bypasses calling .RenderPartial('widget.ascx') for as many widgets as there are.
    bool bypassWidgetControl = true;
    if (bypassWidgetControl)
    {
        // Get all the widgets in the zone.
        IList<WidgetInstance> widgets = widgetService.GetWidgetsInZone(zone);
        foreach (WidgetInstance widget in widgets)
        { %>
        <div id="widget_<%=widget.Id %>" class="widget">
            <%if (isAdmin)
              { %>
            <div class="weditarea">
                <a href="#" onclick="_widgetManager.ToggleEditArea('weditarea_<%=widget.Id %>'); return false;">edit</a>
            </div>
            <% } %>
    
            <div class="wheader"> <%= Html.Encode(widget.Header)%></div><br />
            <%if (isAdmin)
              { %>                
                <div id="weditarea_<%=widget.Id %>" class="weditarea" style="display:none;text-align:left;">
                    <a href="/widget/edit/<%= widget.Id %>">edit</a>&nbsp;&nbsp;
                    <a href="#" onclick="_widgetManager.Save(); return false;">save</a>&nbsp;&nbsp;
                    <a href="#" onclick="_widgetManager.Delete(<%=widget.Id %>);">delete</a>&nbsp;&nbsp;
                    <a href="#" id="weditonoff_<%=widget.Id %>" onclick="_widgetManager.Toggle('weditonoff_<%=widget.Id %>', <%=widget.Id %>);" >off</a>
                </div>
            <% } %>
            <% if (widget.IsSelfRenderable)
               {
                   string widgetHtml = widget.Render();
                   %>
                <div id="wbody_<%=widget.Id %>" class="wbody"> <%= widgetHtml%></div><br /><br />
            <% }
               else
               {
                   string path = widget.Definition.Path;
                   path = "~/views/shared/" + path + ".ascx";
            %>            
                <div id="wbody_<%=widget.Id %>" class="wbody"> <% Html.RenderPartial(path, widget); %></div><br /><br />
            <% } %>
        </div>
  <% }
    }
    else
    {
        widgetService.ForEachInZone(zone, (widget) => Html.RenderPartial("~/views/shared/controls/widget.ascx", widget));
    }
      // Just for benchmarking purposes.
      TimeSpan finish = DateTime.Now.TimeOfDay;
      TimeSpan diff = finish - start;
      int seconds = diff.Milliseconds ;
    %>