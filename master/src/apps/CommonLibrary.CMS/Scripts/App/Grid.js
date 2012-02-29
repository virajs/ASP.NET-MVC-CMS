/* ********************************************************************************************************************
Name: CommonLibrary.NET CMS Grid.
Author: Kishore Reddy
Description: Multiple classes to support a client-side / javascript Grid component.
Url: http://commonlibrarynetcms.codeplex.com
- Column: Represents an individual column.
- Columns: Collection class for columns with lookup methods.
- ColumnHelper: Helper class for column related functionality.
- Grid:  Grid component and controller for all parts of the grid.
- GridView: Default Table based view class for the grid that builds up an html table containing the grid data.
- GridData: Stores the data for the grid and also "builders" that can build the cell data for a specific column.
- GridMenu: Store a list of all the possible actions that can be done a row in the grid. Used to build up the row menu.
- GridMenuItem: Single menu action for a grid row.
********************************************************************************************************************** */
/*
@summary: Represents a selectable column.
@param {number} id: The id of the column
@param {string} name: The name of the column
@param {number} index: The index number for this column.
@param {string} formattedName: The formatted name of column
@param {string} propName: The name of an objects property associated w/ column
@param {string} type: The data type of the column
@param {bool} isSelected: Whether or not the column is selected by default
@param {number} sortIndex: The order number of the column
*/
function Column(id, name, index, formattedName, propName, type, isSelected, sortIndex, width) {
    this.Id = id;
    this.Name = name;
    this.Index = index;
    this.FormattedName = formattedName;
    this.DataType = type;
    this.MappedProperty = propName;
    this.IsSelected = isSelected;
    this.IsEnabled = true;
    this.SortIndex = sortIndex;
    this.Width = width;
}

/*
@summary: Class to represent column selections for a management/index page.
@param {String} columnName: Comma delimited list of column names
@param {String} columnProps: Comma delimited list of object properties associated with the column names.
@param {String} jsObjectName: Name of the javascript object representing the instance of this class
@param {String} columnSelections: Comma delimited list of column names that are selected.
*/
function Columns(columnNames, columnProps, columnTypes, columnWidths, idPrefix) {
    this.Cols = [];
    this.MapIds = {};
    this.MapNames = {};
    this.Names = columnNames;
    this.Props = columnProps;
    this.Types = columnTypes;
    this.Widths = columnWidths;
    this.SelectIdPrefix = idPrefix == null ? "mngCol_" : idPrefix;


    //@summary: Initialize the state of the columns.
    this.Init = function () {
        var names = this.Names.split(',');
        var props = this.Props.split(',');
        var types = this.Types.split(',');
        var widths = (this.Widths == null || this.Widths == "") ? [] : this.Widths.split(',');

        // Create the list of columns.
        for (var ndx = 0; ndx < names.length; ndx++) {
            var width = widths.length == 0 ? "" : widths[ndx];
            var column = new Column("", names[ndx], ndx, "", props[ndx], types[ndx], true, 0, width);
            this.Add(column);
        }
    };


    /*@summary: Configures the columns based on the settings.
    @param {Dictionary} settings: supports
    - columnsToIncludeDelimited
    - columnsToExcludeDelimited
    */
    this.Configure = function (settings) {
        var colsToInclude = settings.columnsToIncludeDelimited;
        if (colsToInclude) {
            var colNames = colsToInclude.split(",");
            if (colNames && colNames.length > 0) {
                var includeMap = {};
                for (var ndx = 0; ndx < colNames.length; ndx++)
                    includeMap[colNames[ndx]] = true;

                for (var ndx = 0; ndx < this.Cols.length; ndx++) {
                    var col = this.Cols[ndx];
                    if (includeMap[col.Name] == null)
                        col.IsEnabled = false;
                }
            }
        }
    };


    /*@summary: Adds a column to the list of columns. */
    this.Add = function (column) {
        if (column.Index == 0) column.Index = this.Cols.length;
        column.FormattedName = this.FormatName(column.Name);
        column.Id = this.SelectIdPrefix + column.FormattedName;
        this.MapIds[column.Id] = column;
        this.MapNames[column.Name] = column;
        this.Cols.push(column);
    };


    /*@summary: Adds a column to the list of columns. */
    this.Remove = function (colname) {
        var ndx = this.IndexOf(colname);
        this.Cols.splice(ndx, 1);
    };


    /*@summary: Gets the column at the specified index. */
    this.At = function (ndx) {
        return this.Cols[ndx];
    };


    /*@summary: Gets the column w/ the specified name. */
    this.Named = function (name) {
        return this.MapNames[name];
    };


    /*@summary: Gets a comma delimited string of the selected columns. */
    this.IndexOf = function (colname) {
        for (var ndx = 0; ndx < this.Cols.length; ndx++) {
            if (this.Cols[ndx].Name == colname)
                return ndx;
        }
        return -1;
    };


    /*@summary: Gets a comma delimited string of the selected columns. */
    this.ForEach = function (criteria, callback) {
        for (var ndx = 0; ndx < this.Cols.length; ndx++) {
            if (criteria == null)
                callback(this.Cols[ndx]);

            else if (criteria(this.Cols[ndx]))
                callback(this.Cols[ndx]);
        }
    };


    /*@summary: Gets a comma delimited string of the selected columns. */
    this.GetSelectedColumnsDelimited = function () {
        var colsDelimited = "";
        for (var ndx = 0; ndx < this.Cols.length; ndx++) {
            if (this.Cols[ndx].IsSelected)
                colsDelimited += "," + this.Cols[ndx].Name;
        }
        colsDelimited = colsDelimited.substring(1);
        return colsDelimited;
    };


    //@summary: Get the reformatted name from the raw name.
    // This group / offer names are reformatted to exclude spaces and dashes.
    this.FormatName = function (name) {
        var finalname = name.replace(" ", "");
        finalname = finalname.replace("-", "");
        finalname = finalname.replace(".", "");
        return finalname;
    };
}



/*@summary: Helper class for column related functionality.*/
var ColumnHelper =
{
    BuildColumnSelections: function (columns, jsObjectName, start, end, inputs) {
        for (var ndx = start; ndx < end; ndx++) {
            var col = columns[ndx];
            var input = ColumnHelper.BuildColumnSelection(col.Id, col.Name, jsObjectName, col.IsSelected);
            inputs.push(input);
        }
    },


    BuildColumnSelection: function (id, columnName, jsObjectName, checked) {
        return '<input type="checkbox" id="' + id
            + '" value="' + columnName
            + '" checked="' + (checked ? "true" : "false")
            + '" onclick="' + jsObjectName + ".OnChanged('" + id + '\');" />'
            + columnName + "<br />";
    },


    ForEachRow: function (numberOfColumns, length, callBack) {
        // break up into cols
        var startIndex = 0;
        var span = Math.min(numberOfColumns, length);
        var endIndex = startIndex + span;
        while (startIndex < length) {
            callBack(startIndex, endIndex);
            startIndex += span;
            endIndex = startIndex + span;
            endIndex = endIndex > length ? length : endIndex;
        }
    }
}
/*
@summary: Class to represent column selections for a management/index page.
@param {String} columns: Instance of the columns class
@param {String} jsObjectName: Name of the javascript object representing the instance of this class
@param {String} divDisplayId: The id of the div that will be populated w/ the html for the selection.
@param {number} numberOfDisplayColumns: The number of columns to show in the selection e.g. 3 columns of column names.
@param {function(Columns)} onChangedCallback: A function to call when a column has been selected or deselected.
@param {function(Columns)} onDoneCallback: A function to call when the column selection is complete and user hits done.
*/
function ColumnSelector(columns, jsObjectName, divDisplayId, numberOfDisplayColumns, onChangedCallback, onDoneCallback) {
    this.JsObjectName = jsObjectName;
    this.Columns = columns;
    this.LastDisplayHtml = "";
    this.DivDisplayId = divDisplayId;
    this.SelectIdPrefix = columns.SelectIdPrefix;
    this.NumberDisplayColumns = numberOfDisplayColumns;
    this.OnChangedCallback = onChangedCallback;
    this.OnDoneCallback = onDoneCallback;
    this.EnableDone = false;
    this.Grid = null;
    this.Name = "Columns";


    //@summary: Initialize the state of the columns.
    this.Init = function (grid) {
        this.Grid = grid;
    };


    //@summary: When column selection is done.
    this.OnDone = function () {
        var info = this.Columns.GetSelectedColumnsDelimited();
        if (this.OnDoneCallback)
            this.OnDoneCallback(this.Columns);
    };


    //@summary: Toggle the change in the selection of the column name.
    this.OnChanged = function (id) {
        var col = this.Columns.MapIds[id];
        if (col == null) return;

        col.IsSelected = !col.IsSelected;
        if (this.OnChangedCallback)
            this.OnChangedCallback(this.Columns);
    };


    //@summary: Build the html for the
    this.Build = function () {
        var html = "";
        var inputs = [];
        var selector = this;
        var columns = [];
        for (var ndx = 0; ndx < this.Columns.Cols.length; ndx++)
            if (this.Columns.Cols[ndx].IsEnabled)
                columns.push(this.Columns.Cols[ndx]);

        html += "<table>";
        ColumnHelper.ForEachRow(this.NumberDisplayColumns, columns.length, function (start, end) {
            ColumnHelper.BuildColumnSelections(columns, selector.JsObjectName, start, end, inputs);
            html += "<tr>";
            for (var ndx = 0; ndx < inputs.length; ndx++)
                html += "<td>" + inputs[ndx] + "</td>";
            html += "</tr>";
            inputs.length = 0;
        });
        html += "</table>";
        if (this.EnableDone) {
            html += '<br/><input type="button" id="colSelectorDone" value="Done" onclick="' + this.JsObjectName + '.OnDone();" />';
        }
        return html;
    };


    //@summary: Displays the UI for selecting the columns.
    this.Display = function () {
        var html = this.Build();
        this.LastDisplayHtml = html;
        var div = document.getElementById(this.DivDisplayId);
        if (div) div.innerHTML = this.LastDisplayHtml;
    };
}



/*@summary: Data class for the grid to store the data records and also store callbacks 
which are custom functions to build the value for a colum */
function GridData(grid) {
    this.Builders = {};
    this.Items = null;

    this.RegisterBuilder = function (colName, func) {
        this.Builders[colName] = func;
    };


    /* 
    @summary: Builds a list of items w/ their new sortindex.
    @example
    new row #   Id   SortIndex
    1   row 2:  3,   5
    2   row 5:  6,   8
    3   row 1:  2,   1

    Id: SortIndex
    3:1,6:5,2:8
    */
    this.GetNewOrder = function (newRowIndexes) {
        var newSortOrderDelimited = "";
        var sortindexes = [];

        var sorter = function (a, b) { return a - b; };

        // Get all sortindexes.
        for (var ndx = 0; ndx < this.Items.length; ndx++)
            sortindexes.push(this.Items[ndx].SortIndex);

        sortindexes.sort(sorter);
        var min = sortindexes[0];
        var max = sortindexes[sortindexes.length - 1];
        var areAllZeros = (min == max && min == 0);

        // Check for same values for sortindex if they are the same.
        for (var ndx = 0; ndx < sortindexes.length - 1; ndx++) {
            if (areAllZeros) {
                sortindexes[ndx] = ndx;
                sortindexes[ndx + 1] = ndx + 1;
            }
            else {
                var current = sortindexes[ndx];
                var next = sortindexes[ndx + 1];
                if (current == next)
                    sortindexes[ndx + 1] = current + 1;
            }
        }
        for (var ndx = 0; ndx < newRowIndexes.length; ndx++) {
            if (ndx < this.Items.length)
                newSortOrderDelimited += this.Items[newRowIndexes[ndx]].Id + ":" + sortindexes[ndx] + ",";
        }
        return newSortOrderDelimited;
    };
}



/*@summary: A lightweight grid with modular UI parts.
@param {string} jsObjectName: The name of the javascript instance of this grid.
@param {string} viewId: The div id to display the grid in.
@param {Columns} columns: An instance of the Columns class
@param {Object} settings: object with all the settings.
- @param {bool} IsPagingEnabled:
- @param {bool} IsSortingEnabled:
- @param {bool} IsReloadEnabled:
- @param {bool} IsCreateEnabled:
- @param {bool} IsEditEnabled:
- @param {bool} IsDeleteEnabled:
- @param {bool} IsSortable:
- @param {string} CssClassForTable:
- @param {string} CssClassForColumn:
- @param {string} CssClassForRow:
- @param {string} CssClassForCell:
- @param {string} CssClassForSortingRows:
- @param {function} OnColumnSelected:
- @param {function} OnReload: function(page, pageSize, orderBy)
- @param {function} OnPageSelected:
- @param {function} OnRowSelected:    
- @param {function} ActionLinkBuilder:
@param {array} parts: Array of UIParts. with "Init(), Display()" methods. */
function Grid(jsObjectName, viewId, columns, settings, customGridBuilder) {

    this.Columns = columns;
    this.Data = new GridData(this);
    this.Menu = new GridMenu();
    this.MenuRow = new GridMenu();    
    this.View = new GridView(this);
    this.UIParts = [];
    this.JsObjectName = jsObjectName;
    this.ViewId = viewId;
    this.Settings = settings;
    this.DataFetcher = settings.DataFetcher;
    this.DataDeleter = settings.DataDeleter;
    this.NewQueryFetcher = settings.NewQueryFetcher;
    this.CustomGridBuilder = customGridBuilder;
    this.DataCallbacks = {};
    this.MenuCallbacks = {};
    this.MenuRowCallbacks = {};
    

    /*@summary: Initializes the default settings. */
    this.Init = function () {
        this.CheckOrDefault("IsPagingEnabled", true);
        this.CheckOrDefault("IsReloadEnabled", true);
        this.CheckOrDefault("IsCreateEnabled", true);
        this.CheckOrDefault("IsEditEnabled", true);
        this.CheckOrDefault("IsDeleteEnabled", true);
        this.CheckOrDefault("IsSortable", false);
        this.CheckOrDefault("CssClassForTable", "gridtable");
        this.CheckOrDefault("CssClassForSortingRows", "connectedSortable");
        this.CheckOrDefault("ActionLinkBuilder", null);
        this.CheckOrDefault("PageSize", 15);
        if (this.Settings.IsSortable) {
            this.View = new GridViewDivBased(this);
        }
        this.Columns = new Columns(this.Settings.ColumnNames, this.Settings.ColumnProps, this.Settings.ColumnTypes, this.Settings.ColumnWidths, this.Settings.ColumnPrefix);
        this.Columns.Init();

        this.Menu.OnMenuItemClickCodeTemplate = this.JsObjectName + ".OnMenuAction('{0}','');";
        this.MenuRow.OnMenuClickCodeTemplate = this.JsObjectName + '.MenuRow.ShowMenu(${RowIndex});';
        this.MenuRow.OnMenuItemClickCodeTemplate = this.JsObjectName + ".OnMenuRowAction('{0}','${Id},${RowIndex}');";
        this.Menu.Init();
        this.MenuRow.Init();
        if (this.CustomGridBuilder != null)
            this.CustomGridBuilder.Configure(this);
    };


    this.RegisterUIPart = function (uiPart) {
        this.UIParts.push(uiPart);
    };


    this.RegisterCallbackForDataAction = function (action, func) {
        this.DataCallbacks[action] = func;
    };


    this.RegisterCallbackForMenuAction = function (action, func) {
        this.MenuCallbacks[action] = func;
    };


    this.RegisterCallbackForMenuRowAction = function (action, func) {
        this.MenuRowCallbacks[action] = func;
    };

    /* Display the grid. */
    this.Display = function () {
        var html = '<div id="gridHeader">' + this.BuildHeader() + '</div>'
                 + '<div id="gridTable">' + this.View.Render() + '</div>'
                 + '<div id="gridFooter">' + this.BuildFooter() + '</div>';
        var div = document.getElementById(this.ViewId);
        if (div) div.innerHTML = html;
        this.View.EnableDragDrop(false);
    };


    /*@summary: Reloads the data. */
    this.Reload = function () {
        var html = this.View.Render();
        var div = document.getElementById("gridTable");
        if(div) div.innerHTML = html;
    };


    /*@summary: Callback for all of the row menu actions. */
    this.OnMenuAction = function (action, context) {
        var func = this.MenuCallbacks[action];
        //alert(action + ", " + context);
        if (action == "sort") {
            this.View.EnableDragDrop(true);
        }
        else if (action == "savesort") {
            var newRowIndexes = this.View.GetReordedItems();
            var idToSortIndexDelimited = this.Data.GetNewOrder(newRowIndexes);
            if (func)
                func(idToSortIndexDelimited);
        }
        else
            if (func)
                func();
    };


    /*@summary: Callback for all of the row menu actions. */
    this.OnMenuRowAction = function (action, context) {
        var func = this.MenuRowCallbacks[action];
        if (func) {
            var tokens = context.split(",");
            var rowindex = Number(tokens[1]);
            var id = Number(tokens[0]);
            func(id, rowindex, null);
        }
    };


    /*@summary: Deletes a row by hiding it. */
    this.DeleteRow = function (id, rowindex) {
        var grid = this;
        var rowid = grid.BuildRowId(rowIndex);
        var row = document.getElementById(rowid);
        if (row) row.style.display = 'none';
    };


    /*@summary: Changes the columns that are displayed. */
    this.ChangeColumns = function () {
        for (var ndx = 0; ndx < this.Columns.Cols.length; ndx++) {
            var col = this.Columns.Cols[ndx];
            if(col.IsEnabled)
                this.ShowHideColumn(ndx, col.IsSelected);
        }
    };


    /*@summary: Hides the entire column.*/
    this.ShowHideColumn = function (colIndex, show) {
        var display = show ? '' : 'none';
        var col = this.Columns.At(colIndex);
        var colHeaderId = this.BuildColumnHeaderId(col);
        var cellHeader = document.getElementById(colHeaderId);
        if (cellHeader)
            cellHeader.style.display = display;

        for (var ndx = 0; ndx < this.Data.Items.length; ndx++) {
            var row = this.Data.Items[ndx];
            var colId = this.BuildColumnId(row, col, ndx);
            cell = document.getElementById(colId);
            if (cell)
                cell.style.display = display;
        }
    };


    this.OnColumnSelected = function (colIndex) {
        var col = this.Columns.At(colIndex);
        this.LastColumnOrdered = col;
        var colProperty = col.MappedProperty;
        this.NewQueryFetcher(1, this.Settings.PageSize, colProperty);
    };


    /*@summary: Builds the html for the header for the grid. */
    this.BuildHeader = function () {

        // Builds the menubar.
        var html = "";
        for (var ndx = 0; ndx < this.Menu.Items.length; ndx++) {
            var menuitem = this.Menu.Items[ndx];
            menuitem.CssClass = "actionbutton";
            html += menuitem.Build(null) + "&nbsp;&nbsp;";
        }
        html += "<br /><br />";
        html += this.BuildParts();
        return html;
    };


    this.BuildFooter = function () {
        return "";
    };


    this.BuildRowActions = function (row, ndx) {
        this.MenuRow.Context["RowIndex"] = ndx;
        this.MenuRow.Context["Id"] = row["Id"];
        var linkhtml = this.MenuRow.Build();
        return linkhtml;
    };


    this.ToggleUIPart = function (name) {
        var fullname = "gridPart_" + name;
        var elem = document.getElementById(fullname);
        if (elem) {
            var display = elem.style.display;
            elem.style.display = (display == "none" || display == "block") ? "" : "none";
        }
    };


    this.BuildParts = function () {
        var html = "";
        for (var ndx = 0; ndx < this.UIParts.length; ndx++) {
            var uiPart = this.UIParts[ndx];
            uiPart.Init(this);
            html += '<div id="gridPart_' + uiPart.Name + '" >';
            html += "<h4>" + uiPart.Name + "</h4>";
            html += uiPart.Build();
            html += "</div>";
        }
        return html + "<br />";
    };


    //@summary: Get the value of the setting if available or default value otherwise.
    this.CheckOrDefault = function (key, defaultValue) {
        if (this.Settings[key] == null)
            this.Settings[key] = defaultValue;
    };


    this.BuildColumnHeaderId = function(col) {
        return "gColHeader_" + col.Index;
    };


    this.BuildRowId = function (ndx) {
        return "gRow_" + ndx;
    };


    this.BuildColumnId = function (row, col, ndx) {
        return "gRowCol_" + ndx + "_" + col.Index;
    };
}



/*@summary: Default view for the grid. This is table based and does not support drag & drop. */
function GridView(grid) {
    this.Grid = grid;
    this.IncludeWidthsForEachRow = false;

    this.Render = function () {
        var html = '<table class="' + this.Grid.Settings.CssClassForTable + '">'
                 + this.BuildColumnHeaders()
                 + this.BuildRows()
                 + "</table>";
        return html;
    };


    /*@summary: Builds the columns ( headers ) html for the grid. */
    this.BuildColumnHeaders = function () {
        var view = this;
        var html = "<tr>";

        // For action links.        
        html += '<th ' + 'width="' + view.Grid.Settings.RowMenuWidth + '"></th>';

        var criteria = function (col) { return col.IsEnabled; };
        this.Grid.Columns.ForEach(criteria, function (col) {
            html += view.BuildColumnHeader(col);
        });
        html += "</tr>";
        return html;
    };


    /*
    @summary: Builds the html for the column header.
    @param {string|object} col: Name of the column or a column object. 
    */
    this.BuildColumnHeader = function (col) {

        if (typeof col == "string")
            col = this.Grid.Columns.Named(col);

        var colId = this.Grid.BuildColumnHeaderId(col);
        var onclickCode = this.Grid.JsObjectName + ".OnColumnSelected(" + col.Index + "); return false;";
        var width = (col.Width == null || col.Width == "") ? "" : ' style="width:' + col.Width + 'px" ';
        var html = '<th id="' + colId + '"' + width + '><a href="#" onclick="' + onclickCode + '" >' + col.Name + "</a></th>";
        return html;
    };


    this.BuildRows = function () {
        var data = this.Grid.DataFetcher(1);
        this.Grid.Data.Items = data.Items;
        var html = "";
        for (var ndx = 0; ndx < this.Grid.Data.Items.length; ndx++) {
            html += this.BuildRow(ndx);
        }
        return html;
    };


    this.BuildRow = function (ndx) {
        var html = "";
        var row = this.Grid.Data.Items[ndx];
        var rowId = this.Grid.BuildRowId(ndx);
        var width = ' style="width:' + this.Grid.Settings.RowMenuWidth + 'px" ';
        html += '<tr id="' + rowId + '">';
        html += (ndx == 0 ) ? "<td " + width + ">" : "<td>";
        html += this.Grid.BuildRowActions(row, ndx) + "</td>";
        html += this.BuildColumns(row, ndx);
        html += "</tr>";
        return html;
    };


    this.BuildColumns = function (row, ndx) {
        var html = "";
        var grid = this.Grid;
        var view = this;
        var criteria = function (col) { return col.IsEnabled; };
        grid.Columns.ForEach(criteria, function (col) {
            var builder = grid.Data.Builders[col.Name];
            var val = "";
            if (builder)
                val = builder(grid, row, ndx);
            else
                val = row[col.MappedProperty];
            html += view.BuildColumn(row, ndx, col, val);
        });
        return html;
    };


    this.BuildColumn = function (row, ndx, col, val) {
        if (typeof col == "string")
            col = this.Grid.Columns.Named(col);

        var colId = this.Grid.BuildColumnId(row, col, ndx);
        var width = "";
        if (ndx == 0 || this.IncludeWidthsForEachRow)
            width = (col.Width == null || col.Width == "") ? "" : ' style="width:' + col.Width + 'px" ';

        var html = '<td id="' + colId + '"' + width + '>' + val + "</td>";
        return html;
    };


    this.EnableDragDrop = function (enable) {
        // Not supported in default view.
    };


    this.GetReordedItems = function () {
        return null;
    };
}



/*
@summary: This grid view basically makes the default view compatible for jquery ui drag & drop for records in the grid.
@remarks: The jquery ui drag & drop does not support d&d for table row. 
To correct this... this grid view class reuses the default grid view and makes each record/row in the grid it's own table
and wraps this table inside a div. For this to look presentable however, the column widths must be specified for each column.
*/
function GridViewDivBased(grid) {
    this.DefaultView = new GridView(grid);
    this.GridSortViewId = "gridTableSortable";
    this.Grid = grid;

    this.Render = function () {
        var html = '<table class="' + this.Grid.Settings.CssClassForTable + '">'
                 + this.DefaultView.BuildColumnHeaders()
                 + "</table>"
                 + "<br/>"
                 + '<div id="' + this.GridSortViewId + '" class="' + this.Grid.Settings.CssClassForSortingRows + '">'
                 + this.BuildRows()
                 + '</div>';

        return html;
    };


    this.BuildRows = function () {
        var data = this.Grid.DataFetcher(1);
        this.DefaultView.IncludeWidthsForEachRow = true;
        this.Grid.Data.Items = data.Items;
        var html = "";
        for (var ndx = 0; ndx < this.Grid.Data.Items.length; ndx++) {
            html += '<div id="ddgridrow_' + ndx + '" style="padding-top:10px; padding-bottom: 10px;">';
            html += '<table class="' + this.Grid.Settings.CssClassForTable + '">';
            html += this.DefaultView.BuildRow(ndx);
            html += "</table>";
            html += "</div>";
        }
        return html;
    };


    // Enabled drag and drop.
    this.EnableDragDrop = function (enable) {

        var elem = document.getElementById(this.GridSortViewId);
        elem.className = enable ? this.Grid.Settings.CssClassForSortingRows : "";
        var viewId = "#" + this.GridSortViewId;
        if (enable) {
            // Jquery handles the sorting/drag-drop.
            $(viewId).sortable({
                connectWith: "." + this.ClassForSorting
            }).sortable('enable').disableSelection();
        }
        else {
            // Jquery handles the sorting/drag-drop.
            $(viewId).sortable('disable').enableSelection();
        }
    }


    this.GetReordedItems = function () {
        var children = $("#" + this.GridSortViewId).children();
        var view = this;
        var newRowIndexes = [];
        jQuery.each(children, function (i, val) {
            var rowindex = val["id"];
            rowindex = rowindex.replace("ddgridrow_", "");
            newRowIndexes.push(parseInt(rowindex));
        });
        return newRowIndexes;
    };
}