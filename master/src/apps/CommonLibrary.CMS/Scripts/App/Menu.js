/* ********************************************************************************************************************
Name: CommonLibrary.NET CMS Grid.
Author: Kishore Reddy
Description: Multiple classes to support a client-side / javascript Grid component.
Url: http://commonlibrarynetcms.codeplex.com
- GridMenu: Store a list of all the possible actions that can be done a row in the grid. Used to build up the row menu.
- GridMenuItem: Single menu action for a grid row.
********************************************************************************************************************** */
/* @summary: Grid menu class to contain menu and build actions for each row. */
function GridMenu() {
    this.Name = "Menu";
    this.IsVertical = true;
    this.HideOnLoad = true;
    this.Items = [];
    this.OnMenuItemClickCodeTemplate = "";
    this.OnMenuClickCodeTemplate = "";
    this.IsVertical = true;
    this.Context = {};
    this.MenuItemNull = new GridMenuItem("", "", "", "", "", 0);
    this.MenuCssClass = "";
    this.CssClass = "gridmenu";

    this.Init = function () {

        for (var ndx = 0; ndx < this.Items.length; ndx++) {
            if(this.Items[ndx].OnClickCode == null || this.Items[ndx].OnClickCode == "")
                this.Items[ndx].OnClickCode = this.OnMenuItemClickCodeTemplate;
            if(this.Items[ndx].SortIndex != 0 )
                this.Items[ndx].SortIndex = ndx;
        }
    };

    this.Remove = function (name) {
        var ndx = this.IndexOf(name);
        this.Items.splice(ndx, 1);
    };

    this.Count = function () {
        return this.Items.length;
    };

    this.Named = function (name) {
        var ndx = this.IndexOf(name);
        if (ndx < 0) return this.MenuItemNull;
        return this.Items[ndx];
    };


    this.Add = function (menuItem) {
        this.Items.push(menuItem);
    };

    this.Build = function () {
        var html = "";
        for (var ndx = 0; ndx < this.Items.length; ndx++) {
            var menuitem = this.Items[ndx];
            var itemhtml = menuitem.Build(this.Context);
            html += "<li>" + itemhtml + "</li>";
        }
        var rowndx = this.Context["RowIndex"];
        var onclickCode = this.OnMenuClickCodeTemplate;
        for (var key in this.Context) {
            var replacement = this.Context[key];
            onclickCode = onclickCode.replace("${" + key + "}", replacement);
        }

        var linkhtml = this.IsVertical ? '<input type="button" value="' + this.Name + '" onclick="' + onclickCode + '" class="' + this.MenuCssClass + '" />' : "";
        var display = this.HideOnLoad ? "display:none;" : "";
        linkhtml += '<ul class="' + this.CssClass + '" id="gridrowmenu_' + rowndx + '" style="' + display + '">' +  html + '</ul>';
        return linkhtml;
    };


    this.IndexOf = function (name) {
        for (var ndx = 0; ndx < this.Items.length; ndx++) {
            if (this.Items[ndx].Name == name)
                return ndx;
        }
        return -1;
    };


    this.ShowMenu = function(id) {
        var elem = document.getElementById("gridrowmenu_" + id);
        var current = elem.style.display;
        elem.style.display = (current == "none" || current == "block") ? "" : "none";
    }
}



/* @summary: Class to represent a menu item for the grid. */
function GridMenuItem(name, text, image, alt, onclickcode, sortindex) {
    this.Name = name;
    this.Text = text ? text : name;
    this.Image = image ? image : "";
    this.AltText = alt ? alt : "";
    this.OnClickCode = onclickcode ? onclickcode : "";
    this.SortIndex = sortindex ? sortindex : 1;
    this.CssClass = "";

    /*@summary: Builds the html for this menu item. */
    this.Build = function (context) {
        return this.ToHtml(context, this.Name, this.Text, this.Image, this.AltText, this.OnClickCode);
    };


    /*@summary: Builds the html for this menu item. */
    this.ToHtml = function (context, name, text, image, alt, onclickcode) {
        if (context != null) {
            for (var key in context ) {
                var replacement = context[key];
                onclickcode = onclickcode.replace("${" + key + "}", replacement);
            }
        }

        var html = "";
        html = '<a href="#" onclick="' + onclickcode + '" class="' + this.CssClass + '" >';
        html += (image != null && image != "")
                          ? '<img src="' + image + '" alt="' + alt + '" />&nbsp;&nbsp;' + text
                          : text;
        html += "</a>";
        html = html.replace("{0}", name);

        if (context) {
            // Substitute values.
            for (var key in context) {
                if (context.hasOwnProperty(key)) {
                    var val = context[key];
                    html = html.replace("${" + key + "}", val);
                }
            }
        }
        return html;
    };
}