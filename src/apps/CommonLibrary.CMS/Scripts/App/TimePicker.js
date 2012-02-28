/* 
@summary: TimePicker to display a simple ui for picking time.
@example:
Hours                 Minutes:                  Am / Pm
1     5     9         00      25    45          am    pm
2     6     10        10      30    50   
3     7     11        15      35    55
4     8     12        20      40    60
*/
function TimePicker(jsObjectName, viewId, controlId, triggerId, settings) {
 
    this.ViewId = viewId;
    this.ControlId = controlId;
    this.TriggerId = triggerId;
    this.JsObjectName = jsObjectName;
    this.Settings = settings;
    this.Hour = 9;
    this.Minutes = 0;
    this.IsPm = false;
    this.Time = "";
   
    this.Init = function() {
    };
 
 
    /*
    @summary: Callback when hours are selected
    @param {number} hour: The hour.
    */
    this.OnHourSelected = function(hour) {
        this.Hour = hour;
        this.Update();
    };
 
 
    /*
    @summary: Callback when minutes are selected
    @param {number} minutes: The minutes.
    */
    this.OnMinutesSelected = function(minutes) {
        this.Minutes = minutes;
        this.Update();
    };
 
 
    /*
    @summary: Callback when minutes are selected
    @param {number} minutes: The minutes.
    */
    this.OnAmPmSelected = function(isPm) {
        this.IsPm = isPm;
        this.Update();
    };
 
 
    this.Update = function() {
        var time = this.Hour + ":"
        time += (this.Minutes == 0) ? "00" : this.Minutes;
        time += (this.IsPm) ? " pm" : " am";
        var elem = document.getElementById(this.ControlId);
        if (elem)
            elem.value = time;
    };
   
   
    /*@summary: Builds the html for this control */
    this.Build = function() {
        // Build the html for hour.
        var hours = "<table>";
        var picker = this;
        this.ForEachRow(3, 12, 1, 1, function(start, end) {
            hours += "<tr>";
            for (var ndx = start; ndx <= end; ndx++) {
                var link = picker.BuildLink("OnHourSelected", ndx, ndx);
                hours += "<td>" + link + "</td>";
            }
            hours += "<tr>";
        });
        hours += "</table>";
 
        // Build the html for minutes.
        var minutes = "<table>";
        this.ForEachRow(3, 55, 0, 5, function(start, end) {
            minutes += "<tr>";
            for (var ndx = start; ndx <= end; ndx += 5) {
                var link = picker.BuildLink("OnMinutesSelected", ndx, ndx);
                minutes += "<td>" + link + "</td>";
            }
            minutes += "<tr>";
        });
        minutes += "</table>";
 
        // Build the html for the am/pm
        var ampm = "";
        ampm += this.BuildLink("OnAmPmSelected", false, "am");
        ampm += "&nbsp;&nbsp;" + this.BuildLink("OnAmPmSelected", true, "pm");
        var data = "<table><tr>"
                 + "<td valign=\"top\">" + "<h5>Hours</h5>" + hours + "</td>"
                 + "<td>&nbsp;&nbsp;&nbsp;</td>"
                 + "<td valign=\"top\">" + "<h5>Minutes</h5>" + minutes + "</td>"
                 + "<td>&nbsp;&nbsp;&nbsp;</td>"
                 + "<td valign=\"top\">" + "<h5>Am / Pm</h5>" + ampm + "</td>"
                 + "</tr></table>";
        return data;
    };
 
   
    /*@summary: Displays the control. */
    this.Display = function() {
        var elem = document.getElementById(this.ViewId);
        if (elem) {
            var html = this.Build();
            elem.innerHTML = html;
        }
    };
 
 
    this.ForEachRow = function(countItems, length, start, increment, callBack) {
        // break up into cols
        // 1  .. 2  .. 3
        // 4  .. 5  .. 6
        //      OR       
        // 1  .. 5  .. 10
        // 15 .. 20 .. 25       
 
        // 1, 12
        // 5, 60
        var span = Math.min(countItems, length);
        var endIndex = start < 1 ? (span - 1) * increment : span * increment;
        while (start < length) {
            callBack(start, endIndex);
            start = endIndex + increment;
            endIndex = endIndex + (span * increment);
            endIndex = endIndex > length ? length : endIndex;
        }
    }
 
    this.BuildLink = function(method, val, text) {
        var html = '<a href="#" onclick="{0}.{1}(' + val + '); return false;" >' + text + '</a>';
        html = html.replace("{0}", this.JsObjectName);
        html = html.replace("{1}", method);
        return html;
    };
   
}