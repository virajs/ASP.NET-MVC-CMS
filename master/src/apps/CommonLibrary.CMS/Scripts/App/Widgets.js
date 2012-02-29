//@Summary: This configures the loader with all the zones.
//@Param sortableZones: delmited list of all the zones.        
function WidgetManager(sortableZones, klassForSortable, zonePrefix, isAdminBased) {

    // [zoneright, zoneleft]
    this.Zones = sortableZones.split(",");
    this.ZonePrefix = zonePrefix;
    this.ClassForSorting = klassForSortable;
    this.IsAdminBasedUI = isAdminBased;
    this.NumberOfWidgetsExpanded = 0;

    // Build up the zone ids for jquery.
    // e.g. "#widget_zone_zoneright, #widget_zone_zoneleft"
    var zoneIds = "#" + this.GetZoneId(0);
    if (this.Zones.length > 1) {
        for (var ndx = 1; ndx < this.Zones.length; ndx++) {
            var zoneId = ", #" + this.GetZoneId(ndx);
            zoneIds += zoneId;
        }
    }
    this.ZoneIds = zoneIds;
    this.EnableDragDrop();
}



/// @summary: Deletes an entity and display a resulting message.
/// @param {number} id: The id of the entity
/// @param {number} rowindex: A row index number ( if applicable ) that may come from the grid.
/// @param {number} callback: Function to call after action has been performed.
WidgetManager.prototype.Delete = function (id) {
    // /article/delete/54
    var question = "Are you sure you want to delete this item?";
    var answer = confirm(question)
    if (answer) {
        SendActionAndCallback("/widget/delete/" + id, function (result) {
            var wedit = document.getElementById("weditarea_" + id);
            if (wedit) wedit.style.display = "none";
            var wbody = document.getElementById("wbody_" + id);
            if (wbody) wbody.innerHTML = result.Message;
        });
    }
}


// Allow toggling of the state of the widget from on/off.
// Note: This does not delete the widget but just sets it's IsActive flag.
WidgetManager.prototype.Toggle = function (controlId, id) {
    var elem = document.getElementById(controlId);
    var state = elem.innerHTML.toLowerCase();
    var newState = state == "on" ? "off" : "on";
    SendActionAndCallback("/widget/OnOff/" + id, function (result) {
        DisplayActionResultMessage(result, "adminActionResult");
        var wbody = document.getElementById("wbody_" + id);
        if (wbody) wbody.innerHTML = result.Message;
        elem.innerHTML = newState;
    });
}


// Displays the "action bar".
WidgetManager.prototype.ToggleEditArea = function (id) {
    var elem = document.getElementById(id);
    var val = elem.style.display == 'none' ? "block" : "none";
    elem.style.display = val;

    if (val == "block")
        this.NumberOfWidgetsExpanded++;
    else
        this.NumberOfWidgetsExpanded--;

    // Enable drag and drop if any 1 widget is "expanded" for editing.    
    this.EnableDragDrop();
}



// Displays the "action bar".
WidgetManager.prototype.GetZoneId = function (ndx) {
    if (this.ZonePrefix == "" || this.ZonePrefix == null || this.ZonePrefix == undefined)
        return this.Zones[ndx];

    return this.ZonePrefix + this.Zones[ndx];
}


// Displays the "action bar".
WidgetManager.prototype.IsDragDropEnabled = function () {

    if (this.IsAdminBasedUI) 
        return true;
    
    return this.NumberOfWidgetsExpanded > 0;
}

// Can enable drag and drop by setting the appropriate class name
// on the divs used for sorting via jquery.
WidgetManager.prototype.EnableDragDrop = function () {

    var enable = this.IsDragDropEnabled();
    for (var ndx = 0; ndx < this.Zones.length; ndx++) {
        var zoneId = this.GetZoneId(ndx);
        var elem = document.getElementById(zoneId);
        elem.className = enable ? this.ClassForSorting : "";
    }

    if (enable) {
        // Jquery handles the sorting/drag-drop.
        $(this.ZoneIds).sortable({
            connectWith: "." + this.ClassForSorting
        }).sortable('enable').disableSelection();
    }
    else {
        // Jquery handles the sorting/drag-drop.
        $(this.ZoneIds).sortable('disable').enableSelection();
    }

}


// Save the orderings of the widgets.
// e.g. widgetid,zone,ndx
//      5,zoneright,1;2,zoneright,2 etc.
WidgetManager.prototype.Save = function Save() {

    var entries = "";
    for (ndx = 0; ndx < this.Zones.length; ndx++) {
        var zone = this.GetZoneId(ndx);
        var children = $("#" + zone).children();
        var wManager = this;
        jQuery.each(children, function (i, val) {
            var id = val["id"];
            id = id.replace("widget_", "");
            var actualZone = zone.replace(wManager.ZonePrefix, "");
            var entrydata = id + "," + actualZone + "," + i + ";";
            entries += entrydata;
        });
    }
    // Sample data becomes:
    // 5, zoneright, 1; 2, zoneright, 2
    var urlorderings = escape(entries);
    SendActionAndDisplayResult('/widget/Reorder?orderings=' + urlorderings);
}