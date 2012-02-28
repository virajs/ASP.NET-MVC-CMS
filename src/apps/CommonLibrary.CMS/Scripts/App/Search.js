/* *********************************************************************
* Author: Kishore Reddy
* Application: CommonLibrary.NET CMS
* Url: http://commonlibrarynetcms.codeplex.com
* Description: Javascript class for an AJAX based search request.
********************************************************************** */

///<summary>Initializes a new instance of the javascript Search class.
///</summary>
///<param name="pageSize">15</param>
///<param name="keywordsControlId">The id of the input control for the keywords.</param>
///<param name="searchTypeName">The name of the radio group indicating search by "site" or "web"</param>
///<param name="searchResultsDivId">The id of the div to populate w/ the searh results</param>
///<param name="baseurl">/search/DoOpenSearch?</param>
///<param name="onLoadCompleteCallback">The callback function to call when the data is loaded.</param>
function Search(pageSize, keywordsControlId, searchTypeName, searchResultsDivId, baseurl, onLoadCompleteCallback) {
    
    // Data for page and for calculations.
    this.PageSize = parseInt(pageSize);
    this.KeywordsControlId = keywordsControlId;
    this.SearchTypeName = searchTypeName;
    this.SearchResultsControlId = searchResultsDivId;
    this.Baseurl = baseurl;
    this.OnLoadComplete = onLoadCompleteCallback;
}


///<summary> Performs a websearch in the background via ajax, gets back json data
/// representing the search results and then displays them on the page.
/// This supports paging of the results. However, unlike using a regular pager, the page
/// number must be entered in the text box. This is the current implementation for the time being.
/// This uses 3 variables ( keywords, searchtype, pagenumber ).
///</summary>
Search.prototype.DoSearch = function(pageNumber) {
    var page = parseInt(pageNumber);
    var keywords = encodeURI($('#' + this.KeywordsControlId).val());
    var searchType = $("input[@name='" + this.SearchTypeName + "']:checked").val();
    
    // 1. Setup url (keywords, searchtype, pagenumber).
    var url = this.Baseurl + "keywords=" + keywords + "&searchtype=" + searchType + "&pagenumber=" + page;
    var search = this;

    // 2. Get the search results via ajax based json query.
    $.getJSON(url, function(data) {
        search.Display(data);
        search.OnLoadComplete(data);
    });
}


///<summary>Builds a single anchor link</summary>
Search.prototype.Build = function(data) {
    var html = "<div>";
    var len = data.Items.length;

    // Build up the list of links 
    for (var i = 0; i < len; i++) {
        html += "<a href=\"" + data.Items[i].Uri + "\">" + data.Items[i].Title + "</a><br/>"
                 + data.Items[i].Description + "<br/>"
                 + data.Items[i].Uri + "<br/><br/>";
    }
    html += "</div>";
    return html;
}


///<summary>Builds a single anchor link</summary>
Search.prototype.GetInfo = function() {
var info = "Current: " + this.CurrentPage + ", Total : " + this.TotalPages + ", Middle : " + this.PagesInMiddle + ", Starting : " + this._startingPage 
         + ", Ending : " + this._endingPage;
    return info;
}


///<summary>Display the html for the search results.</summary>
Search.prototype.Display = function(data) {
    var html = this.Build(data);
    $('#' + this.SearchResultsControlId).html(html);
}
