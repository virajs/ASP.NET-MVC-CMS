/* *********************************************************************
* Author: Kishore Reddy
* Application: CommonLibrary.NET CMS
* Url: http://commonlibrarynetcms.codeplex.com
* Description: Javascript class for loading Recents items asynchronously 
* using Ajax/Json.
********************************************************************** */


///@summary: RecentsControl class to encapsulate the behaviour of the ajax/json based loading
///@param group: e.g. "blog" | "event" 
///@param refid: Id of the post these comments apply to.
///@param baseurl: Url of the ajax/json request. e.g. "/comment/ByGroupAndRefId?" 
///@param author: Author of the post these comments apply to.
///@param commentsdiv: The Id of the div tag to use to display the comments html.
///@param onLoadCompleteCallback: The callback function when data is loaded.
function JsonCrud(jsObjectName, modelname, columnNames, columnProps, columnWidths, page, pageSize, divId, onLoadCompleteCallback) {
    this.JsObjectName = jsObjectName;
    this.UrlCopy = "/" + modelname + "/copy/";
    this.UrlEdit = "/" + modelname + "/edit/";
    this.UrlDelete = "/" + modelname + "/delete/"
    this.UrlManage = "/" + modelname + "/FindByRecentAsJson?page="
    this.ColumnNames = columnNames.split(",");
    this.ColumnProps = columnProps.split(",");
    this.ColumnWidths = columnWidths;
    this.Page = page;
    this.PageSize = pageSize;
    this.ViewId = divId;
    this.TopLevelCssClass = "post";
    this.OnLoadComplete = onLoadCompleteCallback;
    this.ImageCopy   = "<img src=\"/content/images/generic/item_copy.png\"   alt=\"copy\" />";
    this.ImageEdit   = "<img src=\"/content/images/generic/item_edit.png\"   alt=\"edit\" />";
    this.ImageDelete = "<img src=\"/content/images/generic/item_delete.png\" alt=\"delete\" />";
}


///@summary: Loads the comments into the div view.
///@param pagenumber: Pagenumber of the comments to display.
JsonCrud.prototype.Load = function () {
    // 1. Setup url (keywords, searchtype, pagenumber).
    // "/comment/ByGroupAndRefId?group=blog&refid=23&pagenumber=1
    // 2. Get the search results via ajax based json query.
    // 3. set the comment local variable since "this" is not avaialble in the anonymous function.
    var url = this.UrlManage + this.Page + "&pagesize=" + this.PageSize;
    var manager = this;
    $.getJSON(url, function (data) {
        var finaldata = jQuery.parseJSON(data);
        manager.Display(finaldata);
        manager.OnLoadComplete(finaldata);
    });
}


/// @summary: Handles an edit action on an entity.
/// @param id: The id of the entity
JsonCrud.prototype.Edit = function (id) {
    this.Redirect(this.UrlEdit, id);
}


/// @summary: Handles a copy action on an entity.
/// @param id: The id of the entity
JsonCrud.prototype.Copy = function (id) {
    this.Redirect(this.UrlCopy, id);
}


/// @summary: Deletes an entity and display a resulting message.
/// @param id: The id of the entity
JsonCrud.prototype.Delete = function (id) {
    // /article/delete/54
    var url = this.UrlDelete + id
    var answer = confirm("Are you sure you want to delete this item?")
    if (answer)
        alert("Ok to delete")
    else
        alert("Keep it live")
    var manager = this;
    /*$.getJSON(url, function (data) {
        alert(data);
    });
    */
}


/// @summary: Handles a copy action on an entity.
/// @param url: The url to redirecto to.
/// @param id: The id of the entity.
JsonCrud.prototype.Redirect = function (url, id) {
    var finalUrl = url + id;
    document.location = url;
}


/// @summary: Displays the recent data in the UI(div)
/// @param data: Json data representing recent items.
JsonCrud.prototype.Display = function (data) {
    // Build the html for recent items.
    var html = this.Build(data);
    // Finally show the results.
    $('#' + this.ViewId).html(html);
    $('#' + this.ViewId).show();
}


/// @summary: Builds the Html content representing the recent items.
/// @param data: Json data
/// Contains: 
///      Success :   true | false
///      Message :   string( status/error message )
///      TotalPages: int( total number of pages of data )
///      Items :     Array of entities.
///
JsonCrud.prototype.Build = function (data) {
    var html = '<div class="' + this.TopLevelCssClass + '">';
    var len = data.Items.length;
    var contentCss = "content";

    // Add an extra <th> for the copy,edit,delete actions.
    html += "<table class=\"systemlist\"><tr><th></th>";
    for (var colnameNdx = 0; colnameNdx < this.ColumnNames.length; colnameNdx++)
        html += "<th>" + this.ColumnNames[colnameNdx] + "</th>";

    html += "</tr>";
    // Build up the list of links
    for (var i = 0; i < len; i++) {
        var entity = data.Items[i];
        var recordText = "";
        var colname = "";
        var actionlinks = this.BuildActionLinks(entity.Id);
        html += "<tr><td>" + actionlinks + "</td>";
        for (var col = 0; col < this.ColumnProps.length; col++) {
            colname = this.ColumnProps[col];
            recordText += "<td>" + entity[colname] + "</td>";
        }
        html += recordText + "</tr>";
    }
    html += "</table></div>";
    return html;
}


/// @summary: Builds the copy,edit,delete action images
JsonCrud.prototype.BuildActionLinks = function (id) {
    var links = "";
    // <td style="border-style:none"><a href="/Link/Copy/46"><img src="/content/images/generic/item_copy.png" alt="copy"/></a></td>
    var deleteaction = this.JsObjectName + ".Delete(" + id + "); return false;";
    if (id == 1) alert(deleteaction);
    links += "<table><tr>";
    links += "<td style=\"border-style:none\"><a href=\"" + this.UrlCopy + id + "\">" + this.ImageCopy + "</a></td>";
    links += "<td style=\"border-style:none\"><a href=\"" + this.UrlEdit + id + "\">" + this.ImageEdit + "</a></td>";
    links += "<td style=\"border-style:none\"><a href=\"#\" onclick=\"" + deleteaction + "\">" + this.ImageDelete + "</a></td>";
    links += "</tr></table>";
    return links;
}
