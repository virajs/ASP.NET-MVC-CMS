<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Widgets.WidgetInstance>>" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>


<%= Html.DropDownList("widgettypes", Html.ToDropDownList(Model.Name, Widget.Names)) %>
<a href="#" class="actionbutton" onclick="Widget_Create(); return false;">Add</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<a href="#" class="actionbutton" onclick="_widgetManager.Save(); return false;">Save Widgets</a>
<br /><br /><br />
<span class="label">Ordering of widgets</span>
<span class="example">
<br />1. Drag and Drop a widget inside a zone to change order
<br />2. Drag and Drop a widget into a different zone to change zone
<br />3. Click the Save Widgets button to save the new ordering.</span><br /><br /><br />

<span class="label">Enabling / Disabling </span><br />
<span class="example">Click the Menu button and click on the On/Off link to enable/disable the widget</span><br />
        <%  
            var widgets = Model.Items;
            var widgetIds = "";
            List<string> zones = (from e in widgets select e.Zone).Distinct().ToList();
            foreach (string zonename in zones)
            {                
                string zone = "widget_zone_" + zonename;
            %>                
                <br /><br />
                <div><h4><%=zonename %></h4></div>
                <table>
                        <tr>
                            <th style="width:100px; text-align:left;">&nbsp;</th>
                            <th style="width:120px; text-align:left;">Type</th>
                            <th style="width:100px; text-align:left;">Header</th>
                            <th style="width:70px; text-align:left;">Enabled</th>
                            <th style="width:170px; text-align:left;">Tag</th>
                        </tr>
                    </table>
                <div id="<%=zone %>" class="connectedSortable">                  
                <%            
                    var widgetsInZone = (from e in widgets where e.Zone.CompareTo(zonename) == 0 select e).OrderBy(e => e.SortIndex).ToList();
                    foreach (var widget in widgetsInZone)
                    {
                        widgetIds += widget.Id.ToString() + ",";
                        %>        
                        <div id="widget_<%=widget.Id %>" class="widgetpart">
                            <table>
                                <tr>
                                    <td style="width:100px" id="widgetMenu_<%=widget.Id %>"></td>
                                    <td style="width:120px"><%= Html.Encode(widget.DefName) %></td>
                                    <td style="width:100px"><%= Html.Encode(widget.Header) %></td>
                                    <td style="width:70px; font-weight:bold"><span id="widgetOnOff_<%=widget.Id %>"><%= Html.Encode(widget.IsActive ? "on" : "off") %></span></td>    
                                    <td style="width:170px"><%= Html.Encode(widget.EmbedTag) %></td>                                 
                                </tr>
                            </table>            
                        </div>        
                     <%} %>
                 </div>
           <% } %>
           <input type="hidden" id="widgetIdsDelimited" value="<%=widgetIds %>" />

<div id="gridmenu"></div>
<script src="/scripts/jquery-ui-1.8.6.custom.min.js" type="text/javascript"></script>
<script src="/scripts/app/widgets.js" type="text/javascript"></script>
<script src="/scripts/app/EntityManager.js" type="text/javascript"></script>
<script src="/scripts/app/Menu.js" type="text/javascript"></script>
<script src="/scripts/app/Grid.js" type="text/javascript"></script>
<script type="text/javascript">

    var _menu = null;
    var _widgetManager = null;
    var _entities = null;

    //Creates a new widget.
    function Widget_Create() {
        var widget = $('#widgettypes').val();
        var url = "/widget/Create?widgettype=" + widget;
        window.location = url;
    }


    // Set up the location fields.
    $(document).ready(function () {
        _widgetManager = new WidgetManager("zoneright,zoneleft", "connectedSortable", "widget_zone_", true);
        _entities = new EntityManager("_entities", "Widget", 1, 1000, null, null);

        _menu = new GridMenu();
        _menu.IsVertical = true;
        _menu.OnMenuClickCodeTemplate = '_menu.ShowMenu(${RowIndex});';
        _menu.Add(new GridMenuItem("copy", "", "/content/images/generic/item_copy.png", "", "_entities.Copy(${Id}, ${RowIndex}); return false;", 1));
        _menu.Add(new GridMenuItem("edit", "", "/content/images/generic/item_edit.png", "", "_entities.Edit(${Id}, ${RowIndex}); return false;", 2));
        _menu.Add(new GridMenuItem("delete", "", "/content/images/generic/item_delete.png", "", "_entities.Delete(${Id}, ${RowIndex}); return false;", 3));
        _menu.Add(new GridMenuItem("onoff", "on/off", "/content/images/generic/item_disable.png", "", "_widgetManager.Toggle('${ControlId}', ${Id}); return false;", 4));
        _menu.Add(new GridMenuItem("clone", "", "/content/images/generic/item_clone.png", "", "_entities.Clone(${Id}, ${RowIndex}); return false;", 5));
        _menu.Init();

        var widgetIdsDelimited = document.getElementById("widgetIdsDelimited").value;
        var widgetIds = widgetIdsDelimited.split(',');
        for (var ndx = 0; ndx < widgetIds.length; ndx++) {
            var widgetId = widgetIds[ndx];
            if (widgetId != null && widgetId != "") {
                _menu.Context["RowIndex"] = widgetId;
                _menu.Context["Id"] = widgetId;
                _menu.Context["ControlId"] = "widgetOnOff_" + widgetId;
                var html = _menu.Build();
                var elem = document.getElementById("widgetMenu_" + widgetId);
                if (elem) elem.innerHTML = html;
            }
        }
    });
    
</script>

