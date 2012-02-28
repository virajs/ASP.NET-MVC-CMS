/* *********************************************************************
* Author: Kishore Reddy
* Application: CommonLibrary.NET CMS
* Url: http://commonlibrarynetcms.codeplex.com
* Description: Javascript class for loading comments asynchronously 
* using Ajax/Json.
********************************************************************** */


///@summary: Comments class to encapsulate the behaviour of the ajax/json based comments loading.
///@param group: e.g. "blog" | "event"
///@param refid: Id of the post these comments apply to.
///@param baseurl: Url of the ajax/json request. e.g. "/comment/ByGroupAndRefId?"
///@param author: Author of the post these comments apply to.
///@param commentsdiv: The Id of the div tag to use to display the comments html.
///@param onLoadCompleteCallback: The callback function when data is loaded.
function CommentsListControl(pagesize, group, refid, baseurl, author, commentsdiv, onLoadCompleteCallback) {
    
    this.PageSize = pagesize;
    this.Group = group;
    this.RefId = refid;
    this.Baseurl = baseurl;
    this.ViewId = commentsdiv;
    this.Author = author;
    this.OnLoadCompleteCallback = onLoadCompleteCallback;
}


///@summary: Loads the comments into the div view.
///@param pagenumber: Pagenumber of the comments to display.
CommentsListControl.prototype.Load = function(pageNumber) {

    // 1. Setup url (keywords, searchtype, pagenumber).
    // "/comment/ByGroupAndRefId?group=blog&refid=23&pagenumber=1
    var url = this.Baseurl + "group=" + this.Group + "&refid=" + this.RefId + "&pagenumber=" + pageNumber + "&pagesize=" + this.PageSize;

    // 2. Get the search results via ajax based json query.
    // 3. set the comment local variable since "this" is not avaialble in the anonymous function.
    var comments = this;
    $.getJSON(url, function(data) {
        comments.Display(data);
        comments.OnLoadCompleteCallback(pageNumber, data.TotalPages);
    });
}


///@summary: Displays the comments data in the UI(div)
///@param data: Json data representing comments.
CommentsListControl.prototype.Display = function(data) {
    // Build the html for comments.
    var html = this.Build(data);

    // Finally show the results.
    $('#' + this.ViewId).html(html);
    $('#' + this.ViewId).show();
}


///@summary: Builds the Html content representing the comments.
///@param data: Json data
/// Contains: 
///      Success :   true | false
///      Message :   string( status/error message )
///      TotalPages: int( total number of pages of data )
///      Items :     The comments.
///         comment: see CommentListViewModel in CommentsController.cs. 
///
CommentsListControl.prototype.Build = function (data) {
    this.LastResults = data;
    var html = '<div class="comments">';
    var len = data.Items.length;
    var contentCss = "content";

    // Build up the list of links
    for (var i = 0; i < len; i++) {
        var comment = data.Items[i];
        var commentDate = new Date();
        commentDate.setTime(comment.CreateDate);
        var dateStr = commentDate.getMonth() + "/" + commentDate.getDay() + "/" + commentDate.getFullYear();
        var ampam = commentDate.getHours() > 12 ? "pm" : "am";
        var hours = commentDate.getHours() > 12 ? commentDate.getHours() - 12 : commentDate.getHours();
        var mins = commentDate.getMinutes() > 9 ? commentDate.getMinutes() : "0" + commentDate.getMinutes();
        if (hours == 0)
            hours = 12;
        var timeStr = hours + ":" + mins + " " + ampam;

        html += '<div class="comment">';
        if (comment.IsRegisteredUser === true) {
            html += '<span class="user"><a href="/users/' + comment.User + '">' + comment.User + '</a></span>';
        }
        else {
            html += '<span class="user">' + comment.User + '</span>';
        }
        html += ' on ' + '<span class="date">' + dateStr + '</span> at <span class="date">' + timeStr + '</span><br/><br/>';
        html += "</div>";

        var contentClass = comment.User == this.Author ? "contentbold" : "content";
        html += '<div class="' + contentClass + '">';

        // Include image.
        html += '<div style="float:left; padding-right:20px"><img src="' + comment.ImageUrl + '" height="40" width="40" alt="user" /></div>';
        html += "<div>" + comment.Content + "</div>";

        html += '</div><br/>';
        html += '<a href="' + comment.Url + '" class="url">' + comment.Url + '</a><br/><br/><br/>';
        html += '<div class="separator"/><br/>';
    }
    html += "</div>";
    return html;
}
