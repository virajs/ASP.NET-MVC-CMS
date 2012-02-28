<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>


<%= Html.Javascript("/scripts/app/search.js") %>
<%= Html.Javascript("/scripts/app/pagerajax.js") %>

<% bool autosearch = (bool)ViewData["autosearch"]; 
   string autoSearchStr = autosearch ? "true" : "false";
%>
   
<script type="text/javascript">
    var searcher = null;
    var pager = null;
    var pageSelected = 0;
    var newTotalPages = 0;
    var doAutoSearch = false;
    
    
    // Set the page number to 1 and hide pagenumber textbox.
    // There is no reason to show it before performing the actual search.    
    $(document).ready(function() {          
        
        doAutoSearch = <%= autoSearchStr %>;
        searcher = new Search(15, "searchText", "searchType", "searchResults", "/search/DoOpenSearch?", Search_OnLoadComplete);        
        pager = new PagerAjax(1, 5, 1, "pagerview", "Search_OnPageSelected", "", "current");
        if(doAutoSearch)
        {
            Search_AutoSearch();
        }        
    });
    
    
    ///<summary> Automatically does a search on page load.
    /// This is for cases where the search is originally from another page. e.g. search widget.
    ///</summary>
    function Search_AutoSearch() {
        var searchkeywords = '<%= ViewData["searchkeywords"] %>';
        var searchtype = '<%=ViewData["searchtype"] %>'
        var isWeb = searchtype == 'web';
        var optionNdx = isWeb ? 1 : 0;       
        
        // Populate the view with the searchtype/keywords from originating page. 
        $('input[name="searchType"]')[optionNdx].checked = true;
        $('#searchText').val(searchkeywords);        
        searcher.DoSearch(1);                
    }
    
    
    ///<summary>Refresh the comments page based on the selected page.</summary>
    function Search_OnPageSelected(page)
    {
        pageSelected = page;
        searcher.DoSearch(page);          
    }
    
    
    ///<summary>Callback when the search data is loaded</summary>
    function Search_OnLoadComplete(data)
    {
        newTotalPages = data.TotalPages;
        pager = new PagerAjax(pager.CurrentPage, 7, newTotalPages, "pagerview", "Search_OnPageSelected", "", "current");
        pager.SetPageData(pageSelected, newTotalPages);       
    }
</script>

<input type="text" id="searchText" /> <input type="button" class="action" id="btnDoSearch" value="Search" onclick="javascript:DoSearch(1);"/><br />
Site <input type="radio" name="searchType" checked="checked" value="site" /> Web <input type="radio" name="searchType" value="web" /><br /><br />

<div id="searchResults"></div><br /><br />
<div id="pagerview" class="pager"></div>