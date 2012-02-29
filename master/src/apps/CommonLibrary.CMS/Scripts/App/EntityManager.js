/* *********************************************************************
* Author: Kishore Reddy
* Application: CommonLibrary.NET CMS
* Url: http://commonlibrarynetcms.codeplex.com
* Description: Javascript class for loading Recents items asynchronously 
* using Ajax/Json.
********************************************************************** */


/*
@summary: EntityManager class to encapsulate create, copy, edit, delete of entities.
@param {String} jsObjectName: The name of the javascript object representing the instance of this class.
@param {String} modelName: The name of the model e.g. "Post".
@param {Number} page: The page of data to show the first time ( on pageload ).
@param {Number} pageSize: The number of enteries to show on each page.
@param {function(int page, int totalPages)} onLoadCompleteCallback: A function to call for notification purposes. Called after
loading of data is done, the function is passed the current page and the total pages that are available. This is useful for setting up a pager.
@param {String} jsonActionUrlPart: A string template representing the url to call to get the json data representing the list of managable entities.
This should have {0} and {1} as place holders for the page and pageSize. e.g. (/ManageUsingJson?page={0}&pagesize={1}&orderbycolumn{2}.
*/
function EntityManager(jsObjectName, modelname, page, pageSize, onLoadCompleteCallback, jsonActionUrlPart) {
    this.JsObjectName = jsObjectName;
    this.ModelName = modelname;    
    this.UrlCreate = "/" + modelname + "/create";
    this.UrlCopy = "/" + modelname + "/copy/";
    this.UrlEdit = "/" + modelname + "/edit/";
    this.UrlDelete = "/" + modelname + "/delete/";
    this.UrlManage = "/" + modelname + jsonActionUrlPart;
    this.Page = page;
    this.PageSize = pageSize;
    this.OrderByColumn = "";
    this.LastDataFetched = null;
    this.ActionResultViewId = "adminActionResult";
    this.TopLevelCssClass = "post";
    this.OnLoadCompleteCallback = onLoadCompleteCallback;
    this.IsOnLoadCompleteFunction = !onLoadCompleteCallback ? false : typeof onLoadCompleteCallback == 'function';
    this.ImageCopy   = "<img src=\"/content/images/generic/item_copy.png\"   alt=\"copy\" />";
    this.ImageEdit   = "<img src=\"/content/images/generic/item_edit.png\"   alt=\"edit\" />";
    this.ImageDelete = "<img src=\"/content/images/generic/item_delete.png\" alt=\"delete\" />";
}


///@summary: Loads the comments into the div view.
///@param pagenumber: Pagenumber of the comments to display.
EntityManager.prototype.Load = function (page, pageSize, orderByColumn) {

    this.Page = !page ? this.Page : page;
    this.PageSize = !pageSize ? 15 : pageSize;
    this.OrderByColumn = !orderByColumn ? "" : orderByColumn;

    // 1. Setup url (keywords, searchtype, pagenumber).
    // "/comment/ByGroupAndRefId?group=blog&refid=23&pagenumber=1
    // 2. Get the search results via ajax based json query.
    // 3. set the comment local variable since "this" is not avaialble in the anonymous function.
    var url = this.UrlManage.replace("{0}", this.Page);
    url = url.replace("{1}", this.PageSize);
    if (this.OrderByColumn == null || this.OrderByColumn == "")
        url = url.replace("{2}", " ");
    else
        url = url.replace("{2}", this.OrderByColumn);
    var manager = this;
    $.getJSON(url, function (data) {
        var finaldata = jQuery.parseJSON(data);
        manager.LastDataFetched = finaldata;
        manager.OnLoadCompleteCallback(finaldata.Page, finaldata.TotalPages);
    });
}


/// @summary: Handles an edit action on an entity.
/// @param {number} id: The id of the entity
/// @param {number} rowindex: A row index number ( if applicable ) that may come from the grid.
EntityManager.prototype.Create = function (id, rowindex) {
    this.Redirect(this.UrlCreate, "");
}


/// @summary: Handles a copy action on an entity.
/// @param {number} id: The id of the entity
/// @param {number} rowindex: A row index number ( if applicable ) that may come from the grid.
EntityManager.prototype.View = function (id, rowindex) {
    this.Redirect("/" + this.ModelName + "/Details/", id);
}


/// @summary: Handles an edit action on an entity.
/// @param {number} id: The id of the entity
/// @param {number} rowindex: A row index number ( if applicable ) that may come from the grid.
EntityManager.prototype.Edit = function (id, rowindex) {
    this.Redirect(this.UrlEdit, id);
}


/// @summary: Handles a copy action on an entity.
/// @param {number} id: The id of the entity
/// @param {number} rowindex: A row index number ( if applicable ) that may come from the grid.
EntityManager.prototype.Copy = function (id, rowindex) {
    this.Redirect(this.UrlCopy, id);
}


/// @summary: Deletes an entity and display a resulting message.
/// @param {number} id: The id of the entity
/// @param {number} rowindex: A row index number ( if applicable ) that may come from the grid.
/// @param {number} callback: Function to call after action has been performed.
EntityManager.prototype.Delete = function (id, rowindex, callback) {
    // /article/delete/54
    var url = this.UrlDelete + id
    var question = "Are you sure you want to delete this item?";
    this.ConfirmAndSend(url, question, id, rowindex, callback);
}


/// @summary: Deletes an entity and display a resulting message.
/// @param {number} id: The id of the entity
/// @param {number} rowindex: A row index number ( if applicable ) that may come from the grid.
/// @param {number} callback: Function to call after action has been performed.
EntityManager.prototype.Clone = function (id, rowindex, callback) {
    // /article/delete/54
    var url = "/" + this.ModelName + "/Clone/" + id;
    var question = "Are you sure you want to Clone this item?";
    this.ConfirmAndSend(url, question, id, rowindex, callback);
}


/// @summary: Deletes an entity and display a resulting message.
/// @param {number} id: The id of the entity
/// @param {number} rowindex: A row index number ( if applicable ) that may come from the grid.
/// @param {number} callback: Function to call after action has been performed.
EntityManager.prototype.OnOff = function (id, rowindex, callback) {
    // /article/delete/54
    var url = "/" + this.ModelName + "/OnOff/" + id;
    var question = "Are you sure you want to Activate/Deactivate this item?";
    this.ConfirmAndSend(url, question, id, rowindex, callback);
}



/// @summary: Handles a copy action on an entity.
/// @param {string} idToSortIndexDelimited: A delimited string of ids to their sortindex e.g. "<id>:<sortindex>,<id>:<sortindex>"
/// @param {number} id: The id of the entity.
EntityManager.prototype.Reorder = function (idToSortIndexDelimited) {
    var urlorderings = escape(idToSortIndexDelimited);
    var urlSaveOrderings = "/" + this.ModelName + "/Reorder?orderings=" + urlorderings;
    SendActionAndDisplayResult(urlSaveOrderings);
}


/// @summary: Deletes an entity and display a resulting message.
/// @param {number} id: The id of the entity
/// @param {number} rowindex: A row index number ( if applicable ) that may come from the grid.
/// @param {number} callback: Function to call after action has been performed.
EntityManager.prototype.ConfirmAndSend = function (url, confirmMessage, id, rowindex, callback) {
    // /article/delete/54
    this.ClearMessage();
    var answer = confirm(confirmMessage)
    if (answer) {
        var manager = this;
        $.getJSON(url, function (data) {
            try {
                manager.DisplayMessage(data);
                if (callback)
                    if (rowindex)
                        callback(id, rowindex);
            }
            catch (err) { }
        });
    }
}


/// @summary: Handles a copy action on an entity.
/// @param {string} url: The url to redirecto to.
/// @param {number} id: The id of the entity.
EntityManager.prototype.Redirect = function (url, id) {
    var finalUrl = url + id;
    document.location = finalUrl;
}


/// @summary: Builds the copy,edit,delete action images
EntityManager.prototype.BuildActionLinks = function (id, isCopyEnabled, isEditEnabled, isDeleteEnabled, codeCopy, codeEdit, codeDelete) {
    var links = "";
    // <td style="border-style:none"><a href="/Link/Copy/46"><img src="/content/images/generic/item_copy.png" alt="copy"/></a></td>
    var deleteaction = codeDelete ? codeDelete : this.JsObjectName + ".Delete(" + id + ");";
    deleteaction +=  "return false;";
    links += "<table><tr>";
    links += "<td style=\"border-style:none\"><a href=\"" + this.UrlCopy + id + "\">" + this.ImageCopy + "</a></td>";
    links += "<td style=\"border-style:none\"><a href=\"" + this.UrlEdit + id + "\">" + this.ImageEdit + "</a></td>";
    links += "<td style=\"border-style:none\"><a href=\"#\" onclick=\"" + deleteaction + "\">" + this.ImageDelete + "</a></td>";
    links += "</tr></table>";
    return links;
}


/* @summary: Displays the result message in a div, after applying the proper css class based on error/success. 
@param {object} result: The result object that contains a "Success"(bool) and "Message"(string) properties.
@param {string} divId: The div id that the message should appear in. */
EntityManager.prototype.DisplayMessage = function (result) {
    var msg = result.Success ? '<span class="message">' : '<span class="error">';
    msg = msg + result.Message + "</span>";
    var elem = document.getElementById(this.ActionResultViewId);
    if (elem)
        elem.innerHTML = msg;
}


/* @summary: Displays the result message in a div, after applying the proper css class based on error/success. 
@param {object} result: The result object that contains a "Success"(bool) and "Message"(string) properties.
@param {string} divId: The div id that the message should appear in. */
EntityManager.prototype.ClearMessage = function (result) {
    var elem = document.getElementById(this.ActionResultViewId);
    if (elem)
        elem.innerHTML = "";
}