/* *********************************************************************
* Author: Kishore Reddy
* Application: CommonLibrary.NET CMS
* Url: http://commonlibrarynetcms.codeplex.com
* Description: Javascript class for loading Recents items asynchronously 
* using Ajax/Json.
********************************************************************** */


///<summary>RecentsControl class to encapsulate the behaviour of the ajax/json
/// based loading
///</summary>
///<param name="group">e.g. "blog" | "event" </param>
///<param name="refid">Id of the post these comments apply to.</param>
///<param name="baseurl">Url of the ajax/json request. e.g. "/comment/ByGroupAndRefId?" </param>
///<param name="author">Author of the post these comments apply to.</param>
///<param name="commentsdiv">The Id of the div tag to use to display the comments html.</param>
///<param name="onLoadCompleteCallback">The callback function when data is loaded.</param>
function RecentsControl(baseurl, isVertical, isRefreshable, refreshRate, pageSize, divId, topLevelCssClass, onLoadCompleteCallback) {
    this.Baseurl = baseurl;
    this.IsVertical = isVertical;
    this.IsRefreshable = isRefreshable;
    this.RefreshRate = refreshRate;
    this.PageSize = pageSize;
    this.ViewId = divId;
    this.TopLevelCssClass = topLevelCssClass;
    this.OnLoadComplete = onLoadCompleteCallback;
}


///<summary>Loads the comments into the div view.</summary>
///<param name="pagenumber">Pagenumber of the comments to display.</param>
RecentsControl.prototype.Load = function () {
    // 1. Setup url (keywords, searchtype, pagenumber).
    // "/comment/ByGroupAndRefId?group=blog&refid=23&pagenumber=1
    var url = this.Baseurl + "pagesize=" + this.PageSize;

    // 2. Get the search results via ajax based json query.
    // 3. set the comment local variable since "this" is not avaialble in the anonymous function.
    var recents = this;
    $.getJSON(url, function (data) {
        recents.Display(data);
        recents.OnLoadComplete(data);
    });
}


///<summary>Displays the recent data in the UI(div)</summary>
///<param name="data">Json data representing recent items.</param>
RecentsControl.prototype.Display = function (data) {
    // Build the html for recent items.
    var html = this.Build(data);

    // Finally show the results.
    $('#' + this.ViewId).html(html);
    $('#' + this.ViewId).show();
}


///<summary>Builds the Html content representing the recent items.</summary>
///<param name="data">Json data
/// Contains: 
///      Success :   true | false
///      Message :   string( status/error message )
///      TotalPages: int( total number of pages of data )
///      Items :     The comments.
///         comment: see CommentListViewModel in CommentsController.cs. 
///</param>
RecentsControl.prototype.Build = function (data) {
    var html = '<div class="' + this.TopLevelCssClass + '">';
    var len = data.Items.length;
    var contentCss = "content";

    // Build up the list of links
    for (var i = 0; i < len; i++) {
        var comment = data.Items[i];
        html += comment.Html;
    }
    html += "</div>";
    return html;
}
