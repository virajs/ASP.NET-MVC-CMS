<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Widgets.WidgetInstance>" %>


<script type="text/javascript">
    function WidgetDoSearch() {
        var rawkeywords = $('#widgetSearchText').val();
        var keywords = encodeURI(rawkeywords);
        var searchType = $("input[name='Search']:checked").val();
        window.location = "/search/search?keywords=" + keywords + "&searchtype=" + searchType;
    }
</script>
<input type="text" id="widgetSearchText" style="width:150px" /> <br />
Site <input type="radio" name="Search" checked="checked" value="site" /> Web <input type="radio" name="Search" value="web" /><br /><br />
<input type="button" class="action" id="widgetDoSearch" value="Search" onclick="javascript:WidgetDoSearch();"/>
