<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="ComLib.Authentication" %>
<% if (Auth.IsAdmin())
   { %>
    <script src="/scripts/jquery-ui-1.8.6.custom.min.js" type="text/javascript"></script>
    <script src="/scripts/app/widgets.js" type="text/javascript"></script>
    <script type="text/javascript">
        _widgetManager = new WidgetManager("zoneright,zoneleft", "connectedSortable", "", false);

        // Set up the location fields.
        $(document).ready(function () {
            _widgetManager = new WidgetManager("zoneright,zoneleft", "connectedSortable", "", false);
        });
    </script>
<% } %>
