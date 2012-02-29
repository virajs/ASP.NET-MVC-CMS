<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.JsonManageViewModel>" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="ComLib.Configuration" %>
<%@ Import Namespace="ComLib.Paging" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <%= Html.HeadingForManage(Model.Name) %>

    <!-- 1. Below div ids is populated with the html/table of the row of entities. -->
    <div id="columnsView"></div><br />
    <div id="gridView"></div>
    
    <br /><br />
    <!-- 2. Client side javascript ajax pager -->
    <% Html.RenderPartial("Controls/PagerAjax", new PagerAjaxViewModel(){ JSObjectName = "_pager", PageSize = 2, CssClassCurrent = "current",  OnPageSelectedCallBack = "EntityManage_OnPageSelected" }); %><br /><br />
    <br /><br />

    <!-- 3. Now hook up the page load handler to build the html table of entities. -->
    <script type="text/javascript">

        var _entities = null;
        var _grid = null;
        var _columns = null;
        var _columnSelector = null;
        
        // Display the data.
        $(document).ready(function () {
            ActivateSorting();
            _entities = new EntityManager("_entities", "<%= Model.Name %>", 1, 15, null, "/ManageUsingJson?page={0}&pagesize={1}&orderbycolumn={2}"); 
            
            // 1. Create instance of grid with various settings.           
            _grid = new Grid("_grid", "gridView", _columns, 
                            { 
                                CssClassForTable: "systemlist",
                                CssClassForSortingRows: "connectedSortable",
                                IsSortable: <%= Model.IsSortable.ToString().ToLower() %>,
                                ColumnNames: "<%= Model.ColumnNames %>", 
                                ColumnProps:"<%= Model.ColumnProps %>", 
                                ColumnTypes: "<%= Model.ColumnTypes %>", 
                                ColumnWidths: "<%= Model.ColumnWidths %>",
                                RowMenuWidth: 70,
                                ColumnPrefix: "col_",
                                DataFetcher: function(page){ return _entities.LastDataFetched; },
                                DataDeleter: function(id, rowIndex, callback){ return _entities.Delete(id, rowIndex, callback); },
                                NewQueryFetcher: function(page, size, orderby) { _entities.Load(page, size, orderby); }
                            }, null);

            // Custom row builder available for this entity?
            <% if(Model.HasRowBuilder){ %>
            _grid.CustomGridBuilder = new <%=Model.RowBuilder %>(); 
            <% } %>

            // 2. Set up the all the row menu actions and icons and menu bar actions and icons.
            _grid.Menu.IsVertical = false;
            _grid.Menu.Add(new GridMenuItem("add", "Add", "", "", "", 0));
            _grid.Menu.Add(new GridMenuItem("reload", "Reload", "", "", "", 0));
            
            <% if(Model.IsSortable){ %>
            _grid.Menu.Add(new GridMenuItem("sort", "Start Sort", "", "", "", 0));
            _grid.Menu.Add(new GridMenuItem("savesort", "Save Sort", "", "", "", 0));            
            <% } %>

            _grid.Menu.Add(new GridMenuItem("columns", "Columns", "", "", "", 0));
            <% if(Model.HasDetailsPage){ %>
            _grid.MenuRow.Add(new GridMenuItem("view", "view", "/content/images/generic/item_view.png", "", "", 0));
            <% } %>            
            _grid.MenuRow.Add(new GridMenuItem("copy", "", "/content/images/generic/item_copy.png", "", "", 1));
            _grid.MenuRow.Add(new GridMenuItem("edit", "", "/content/images/generic/item_edit.png", "", "", 2));
            _grid.MenuRow.Add(new GridMenuItem("delete", "", "/content/images/generic/item_delete.png", "", "", 3));

            <% if(Model.IsActivatable){ %>
            _grid.MenuRow.Add(new GridMenuItem("onoff", "on/off", "/content/images/generic/item_disable.png", "", "", 4));
            <% } %>
            <% if(Model.IsCloneable){ %>
            _grid.MenuRow.Add(new GridMenuItem("clone", "", "/content/images/generic/item_clone.png", "", "", 5));
            <% } %>            
            
            // 3. Initialize the grid.
            _grid.Init();

            // 4. Setup the callbacks for the dataloading and rowmenu actions.
            _grid.RegisterCallbackForMenuAction("add", function() { return _entities.Create(); });
            _grid.RegisterCallbackForMenuAction("reload", function() { return _entities.Load(_pager.CurrentPage); });
            _grid.RegisterCallbackForMenuAction("savesort", function(idToSortIndexesDelimited) { return _entities.Reorder(idToSortIndexesDelimited); });
            _grid.RegisterCallbackForMenuAction("columns", function() { _grid.ToggleUIPart("Columns"); });
            _grid.RegisterCallbackForDataAction("load", function(page) { return _entities.LastDataFetched; });
            _grid.RegisterCallbackForDataAction("query", function(page, size, orderby) { _entities.Load(page, size, orderby); });             
            _grid.RegisterCallbackForMenuRowAction("view", function(id, rowindex, callback) { _entities.View(id, rowindex, callback); });
            _grid.RegisterCallbackForMenuRowAction("copy", function(id, rowindex, callback) { _entities.Copy(id, rowindex, callback); });
            _grid.RegisterCallbackForMenuRowAction("edit", function(id, rowindex, callback) { _entities.Edit(id, rowindex, callback); });
            _grid.RegisterCallbackForMenuRowAction("delete", function(id, rowindex, callback) { _entities.Delete(id, rowindex, function(id, rowindex){ _grid.DeleteRow(id, rowindex); }); });
            _grid.RegisterCallbackForMenuRowAction("onoff", function(id, rowindex, callback) { _entities.OnOff(id, rowindex, callback); });
            _grid.RegisterCallbackForMenuRowAction("clone", function(id, rowindex, callback) { _entities.Clone(id, rowindex, callback); });
            
            // 5. Register the UI parts for the grid.            
            _columnSelector = new ColumnSelector(_grid.Columns, "_columnSelector", "columnsView", 8, function(cols){ _grid.ChangeColumns();}, function(cols){ _grid.Display();});
            _grid.RegisterUIPart(_columnSelector);

            // 5. Integrate w/ the other components ( pager etc. )
            // Setup callbacks.
            // 1. Set callback on entity manager to update the pager when it loads data.
            // 2. Set callback on the pager to tell entity manager to fetch data for the selected page.
            _entities.OnLoadCompleteCallback = function (page, total) { _pager.SetPageData(page, total); _grid.Display();  };            
            _pager.OnPageSelectedCallback = function (page) { _entities.Load(page); };

            // Now finally load the initial set of data on the entity manager.
            _entities.Load();
        });

        function ActivateSorting()
        {   
            $( "#gridTableSortable" ).sortable();
		    $( "#gridTableSortable" ).disableSelection();
        }
    </script>
    <br /><br />

    <!-- Include all the required javascripts. -->
    <%= Html.Javascript("/scripts/app/EntityManager.js") %>
    <%= Html.Javascript("/scripts/app/PagerAjax.js") %>
    <%= Html.Javascript("/scripts/app/Menu.js") %>
    <%= Html.Javascript("/scripts/app/Grid.js") %>    
    <%= Html.Javascript("/scripts/app/RowBuilders.js") %>
</asp:Content>