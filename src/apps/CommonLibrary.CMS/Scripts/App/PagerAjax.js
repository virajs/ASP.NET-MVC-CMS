/* *********************************************************************
* Author: Kishore Reddy
* Application: CommonLibrary.NET CMS
* Url: http://commonlibrarynetcms.codeplex.com
* Description: Javascript class for an AJAX based pager.
********************************************************************** */

///@summary: Initialize a new instance of the pager.
///@example: The best way to show an example of using the pager is w/ data.
/// Current Page = 1
/// Number of pages displayed in "middle" = 5 ( see example below ).
/// Total Pages = 12
///
/// Case 1:                       1   2   3   4   5    ..   next   12   ( CurrentPage = 1,  StartingPage = 1,  EndingPage = 5,  TotalPages = 12 )
/// Case 2:   1   ..   previous   6   7   8   9   10   ..   next   12   ( CurrentPage = 6,  StartingPage = 6,  EndingPage = 10, TotalPages = 12 )
/// Case 3:   1   ..   previous   11  12                                ( CurrentPage = 11, StartingPage = 11, EndingPage = 12, TotalPages = 12 )
///    
///    STEPS:
///     1. When starting at page 1 and the total pages = 12, there are 5 pages in the middle ( case 1 )
///     2. When on the 5th page and clicking "next", this changes the pages showed to starting at 6 ( case 2 )
///     3. When finally on page 10 and clicking "next", the last 2 pages are displayed.
function PagerAjax(jsObjectName, pageNumber, pagesInMiddle, totalPages, pagerDivId, onPageSelectedCallback, cssPage, cssCurrentPage, pageSize) {

    // Data for page and for calculations.
    this.JsObjectName = jsObjectName;
    this.PageSize = pageSize;
    this.CurrentPage = parseInt(pageNumber);
    this.PagesInMiddle = parseInt(pagesInMiddle);
    this.TotalPages = parseInt(totalPages);
    
    // UI reference to div tag id.
    this.ViewId = pagerDivId;
    
    // Callbacks.
    this.OnPageSelectedCallback = onPageSelectedCallback;
    
    // Range values 1-5, 6-10, 11-12
    this._startingPage = 1;
    this._endingPage = 1;
    this._previousPage = 1;
    this._nextPage = 1;
    
    // CSS values.
    this.CssCurrentPage = cssCurrentPage;
    this.CssPage = cssPage;

    // Display the current page.
    this.SetPageData(this.CurrentPage, this.TotalPages);
}


/// @summary: 
/// Set the current page and total pages. 
/// Will recalculate to determine what pages to display.
PagerAjax.prototype.SetPage = function (page) {
    // Notify callbacks.
    if (this.OnPageSelectedCallback != null)
        this.OnPageSelectedCallback(page);
}


/// @summary: Set the current page and total pages. Will recalculate to determine what pages to display.
PagerAjax.prototype.SetPageData = function (page, totalPages) {
    this.CurrentPage = parseInt(page);
    this.TotalPages = parseInt(totalPages);
    this.Calculate();
    this.Display();
}


/// @summary: Can show First page link?
PagerAjax.prototype.CanShowFirst = function() {
    return (this._startingPage != 1);
}


/// @summary: Can show previous link?
PagerAjax.prototype.CanShowPrevious = function() {
    return (this.CurrentPage > 1);
}


/// @summary: Can show Next page link?
PagerAjax.prototype.CanShowNext = function() {
    return (this.CurrentPage < (this.TotalPages ));
}


/// @summary: Can show Last page link?
PagerAjax.prototype.CanShowLast = function() {
    return (this._endingPage != this.TotalPages);
}


/// @summary: Performs some logic/calculation to determine which pages to display.
PagerAjax.prototype.Calculate = function() {
    // Validation & Bounds checking.
    if (this.CurrentPage <= 0) {
        this.CurrentPage = 1;
    }
    if (this.TotalPages <= 0) {
        this.TotalPages = 1;
    }

    if (this.CurrentPage > this.TotalPages) {
        this.CurrentPage = this.TotalPages;
    }
    // Only 1 page.
    if (this.TotalPages == 1) {
        this.CurrentPage = 1;
        this._startingPage = 1;
        this._endingPage = 1;
    }
    // TotalPages <= # Pages that can be displayed
    else if (this.TotalPages < this.PagesInMiddle) {
        this._startingPage = 1;
        this._endingPage = this.TotalPages;
    }
    // Last page
    else if (this.CurrentPage == this.TotalPages) {
        this._startingPage = (this.TotalPages - this.PagesInMiddle) + 1;
        this._endingPage = this.TotalPages;
    }
    else {
        var range = Math.ceil(this.CurrentPage / this.PagesInMiddle);
        this._startingPage = (range - 1) * this.PagesInMiddle + 1;
        this._endingPage = Math.min(range * this.PagesInMiddle, this.TotalPages);
    }
}


///@summary: Builds the html for the pager.
PagerAjax.prototype.PageUrlBuilder = function(page) {
    return "/post/index/" + page;
}


///@summary: Builds the html for the pager.
PagerAjax.prototype.Build = function() {
    var html = "";
    var url = "";

    // Handle case where there is 1 page of data.
    if (this.TotalPages == 1)
        return html;

    // Build the previous page link.
    if (this.CanShowPrevious()) {
        // Previous
        this._previousPage = this.CurrentPage - 1;
        url = this.PageUrlBuilder(this._previousPage);
        html += this.BuildLink(this.CssPage, url, "&#171;", this._previousPage);
    }

    // Build the starting page link.            
    if (this.CanShowFirst()) {
        // First
        url = this.PageUrlBuilder(1);
        html += this.BuildLink(this.CssPage, url, "1", 1);

        // This is to avoid putting ".." between 1 and 2 for example.
        // If 1 is the starting page and we want to display 2 as starting page.
        if (this.CanShowPrevious) {
            html += "&nbsp;&nbsp;&nbsp;";
        }
    }

    // Each page number.
    var start = this._startingPage;
    var end = this._endingPage;
    var ndx = start;
    while (ndx <= end) {
        var cssClass = (ndx == this.CurrentPage) ? this.CssCurrentPage : "";
        url = this.PageUrlBuilder(ndx);

        // Build page number link. <a href="<%=Url %>" class="<%=cssClass %>" ><%=ndx %></a>
        html += this.BuildLink(cssClass, url, ndx, ndx);
        ndx++;
    }

    // Build the  ending page link.
    if (this.CanShowLast()) {
        url = this.PageUrlBuilder(this.TotalPages);

        // This is to avoid putting ".." between 7 and 8 for example.
        // If 7 is the ending page and we want to display 8 as total pages.
        if (this.CanShowNext) {
            html += "&nbsp;&nbsp;&nbsp;";
        }
        html += this.BuildLink(this.CssPage, url, this.TotalPages, this.TotalPages);
    }

    // Build the next page link.
    if (this.CanShowNext()) {
        // Previous
        this._nextPage = this.CurrentPage + 1;
        url = this.PageUrlBuilder(this._nextPage);
        html += this.BuildLink(this.CssPage, url, "&#187;", this._nextPage);
    }
    return html;
}


///@summary: Builds a single anchor link
PagerAjax.prototype.BuildLink = function (cssClass, url, text, pagenumber) {
    var callBack = this.JsObjectName + ".SetPage(" + pagenumber + "); return false;";
    var link = '<a class="' + cssClass + '" href="' + "#" + '" onclick="' + callBack + '">' + text + "</a>";
    return link;
}


///@summary: Builds a single anchor link
PagerAjax.prototype.GetInfo = function() {
var info = "Current: " + this.CurrentPage + ", Total : " + this.TotalPages + ", Middle : " + this.PagesInMiddle + ", Starting : " + this._startingPage 
         + ", Ending : " + this._endingPage;
    return info;
}


///@summary: Display the html for the pager.
PagerAjax.prototype.Display = function() {
    var html = this.Build();
    $('#' + this.ViewId).html(html);
}
