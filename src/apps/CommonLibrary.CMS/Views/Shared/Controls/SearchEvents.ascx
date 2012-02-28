<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Extensions" %>
<%@ Import Namespace="ComLib.Web.Modules.Categorys" %>
<%@ Import Namespace="ComLib.Web.Modules.Events" %>
<%
    string keywords = this.Request.Params["keywords"];
    string location = this.Request.Params["location"];
    string categoryid = this.Request.Params["category"];
    if (string.IsNullOrWhiteSpace(categoryid))
        categoryid = "-1";
     %>
<div class="search">
<table border="0">
    <tr>
        <td>
            <span class="label">Keywords</span><br />
            <input type="text" id="SearchKeywords" style="width:150px" value="" />&nbsp;&nbsp;&nbsp;<br />
            <span class="example">yoga cooking computer</span>
        </td>
        <td>
            <span class="label">Location</span><br />
            <input type="text" id="SearchLocation"  style="width:150px" value="" />&nbsp;&nbsp;&nbsp;<br />
            <span class="example">Brooklyn | New Jersey</span>
        </td>
        <td>
            <span class="label">Category</span><br />
            <%= ComLib.Web.Modules.Categorys.CategoryHelper.BuildCategoriesFor<Event>("CategoryId", "&nbsp;&nbsp;&nbsp;&nbsp;", includeEmptyCategory: true, emptyCategoryName: "All") %>&nbsp;
        </td>
        <td valign="middle"><br />
            <input type="button" id="searchGo" class="action"  value="search" onclick="_searchEventsCtrl.DoSearch();" />
        </td>
    </tr>
</table>
</div>


<script type="text/javascript">

    var _searchEventsCtrl = null;

    /// Loads the comments on page load.
    $(document).ready(function() {
        _searchEventsCtrl = new SearchEvents('/event/search');
        $("#SearchKeywords").val('<%= keywords %>');
        $("#SearchLocation").val('<%= location %>');
        $("#CategoryId").val('<%= categoryid %>');

        $("#SearchKeywords").keyup(function (event) {
            if (event.keyCode == 13) {
                $("#searchGo").click();
            }
        });
        $("#SearchLocation").keyup(function (event) {
            if (event.keyCode == 13) {
                $("#searchGo").click();
            }
        });
    });
    

    // Initialize the search events class.
    function SearchEvents(redirectUrl) {
        this.RedirectUrl = redirectUrl;
        this.Page = 1;
    }


    ///<summary>Handler for when the page is selected.</summary>
    ///<param name="page">The selected page number.</param>
    SearchEvents.prototype.OnEventsPageSelected = function(page) {
        this.Page = page;
        this.DoSearch();
    }


    ///<summary>Perform the search for events</summary>
    SearchEvents.prototype.DoSearch = function () {
        var keywords = $('#SearchKeywords').val();
        var location = $('#SearchLocation').val();
        var category = $('#CategoryId').val();
        var page = this.Page;
        keywords = encodeURI(keywords);
        location = encodeURI(location);
        category = encodeURI(category);
        var finalUrl = this.RedirectUrl + "?keywords=" + keywords + "&location=" + location + "&category=" + category + "&page=" + page;
        //alert(finalUrl);
        document.location = finalUrl;
    }
</script>
