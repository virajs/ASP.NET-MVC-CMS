
/* ****************************************************************
@summary: Class for building the row for the manage grid. 
***************************************************************** */
function EventGridBuilder() {

    /* @summary: Configures the column headers and the column value builders.
    @param {Grid} grid: The grid object. */
    this.Configure = function (grid) {

        grid.Columns.Configure({ columnsToIncludeDelimited: "Id,Starts,Title,Category" });
        grid.Data.RegisterBuilder("Starts", function (grid, row, ndx) { return row["StartDate"] + "<br/>" + row["Starts"]; });
        grid.Data.RegisterBuilder("Title",  function (grid, row, ndx) { return "<b>" + row["Title"] + "</b><br/><br/><div class=\"example\">" + row["Description"] + "</div>"; });
    };
}



/* ****************************************************************
@summary: Class for building the row for the manage grid. 
***************************************************************** */
function FavoriteGridBuilder() {

    /* @summary: Configures the column headers and the column value builders.
    @param {Grid} grid: The grid object. */
    this.Configure = function (grid) {
        grid.Columns.Configure({ columnsToIncludeDelimited: "Id,Title,Model,RefId" });
        grid.Data.RegisterBuilder("Title", function( grid, row, ndx) { return '<a href="' + row["Url"] + '">' + row["Title"] + '</a>'; });
    };
}




/* ****************************************************************
@summary: Class for building the row for the manage grid. 
***************************************************************** */
function MediaFolderGridBuilder() {

    /* @summary: Configures the column headers and the column value builders.
    @param {Grid} grid: The grid object. */
    this.Configure = function (grid) {
        grid.Columns.Add(new Column("Actions", "Actions", "Actions", "Actions", "Actions", true, 7));
        grid.Data.RegisterBuilder("Name", function (grid, row, ndx) { return '<a href="/mediafolder/details/' + row.Id + '">' + row.Name + '</a>'; });
        grid.Data.RegisterBuilder("Actions", function (grid, row, ndx) {
            return '<a href="/mediafile/createinfolder/' + row.Id + '">Add</a>' + "&nbsp;&nbsp;"
                 + '<a href="/mediafile/managebyfolder/' + row.Id + '">Manage</a>';
        });
    };
}



/* ****************************************************************
@summary: Class for building the row for the manage grid. 
***************************************************************** */
function FeedbackGridBuilder() {

    /*@summary: Builds the headers for the manage grid. 
    @param {Grid} grid: The grid object. */
    this.Configure = function (grid) {
        var builder = this;
        grid.Columns.Add(new Column("Contact", "Contact", "Contact", "Contact", "Contact", true, 7));
        grid.Columns.Configure({ columnsToIncludeDelimited: "Id,Title,Name,Contact" });
        grid.Data.RegisterBuilder("Title", function (grid, row, ndx) { return row["Title"] + "<br/>" + row["Content"]; });
        grid.Data.RegisterBuilder("Content", function (grid, row, ndx) { return row["Email"] + "<br />" + row["Url"]; });
    };
}



/* ****************************************************************
@summary: Class for configuring the comment grid. 
***************************************************************** */
function CommentGridBuilder() {

    /* 
    @summary: Configures the column headers and the column value builders.
    @param {Grid} grid: The grid object. 
    */
    this.Configure = function (grid) {
        var builder = this;        
        grid.Columns.Configure({ columnsToIncludeDelimited: "Id,Content,Approved" });
        grid.Data.RegisterBuilder("Approved", function (grid, row, ndx) { return row["IsApproved"] ? "Yes" : "No"; });        
        grid.Data.RegisterBuilder("Content", function (grid, row, ndx) { return builder.BuildContent(row);  });
    };


    this.BuildContent = function (row) {
        return this.BuildImageLink(row) + row["Name"] + "<br />" + row["Email"] + "<br />"
               + '<h4><a href="' + row["Url"] + '">' + row["Title"] + '</a></h4>' + row["Content"];
    };


    this.BuildImageLink = function (row) {
        if (row["IsGravatarEnabled"])
            return '<img src="' + row["ImageUrl"] + '" alt="user" />' + "<br />";

        return "";
    };
}


/* ****************************************************************
@summary: Class for building the row for the manage grid.
***************************************************************** */
function UserGridBuilder() {
    
    /*@summary: Builds the headers for the manage grid. 
    @param {Grid} grid: The grid object. */
    this.Configure = function (grid) {
        var builder = this;
        grid.MenuRow.Remove("delete");
        grid.MenuRow.Remove("clone");
        grid.Columns.Add(new Column("Status", "Status", "Status", "Status", "Status", true, 7));
        grid.Columns.Add(new Column("Actions", "Actions", "Actions", "Actions", "string", true, 8));
        grid.Columns.Configure({ columnsToIncludeDelimited: "Id,Name,Email,Roles,IsApproved,Status,Actions" });
        grid.Data.RegisterBuilder("Status", function (grid, row, ndx) {
            return "Approved: " + row["IsApproved"] + "<br />"
                 + "Locked: " + row["IsLockedOut"] + "<br />"
                 + "Login: " + row["LastLoginDate"];
        });
        grid.Data.RegisterBuilder("Actions", function (grid, row, ndx) {
            var username = row["UserName"];
            return builder.BuildActionLink("CMSUserAction('" + username + "', 'Approve'); return false;", "Approve")
                   + builder.BuildActionLink("CMSUserAction('" + username + "', 'Lockout'); return false;", "Lockout")
                   + builder.BuildActionLink("CMSUserAction('" + username + "', 'UndoLockout'); return false;", "Undo Lockout")
                   + builder.BuildActionLink("CMSUserAction('" + username + "', 'SendPassword'); return false;", "Send Pswd")
        });
    };


    this.BuildActionLink = function (code, text) {
        var html = '<a href="#" onclick="' + code + '">' + text + '</a><br/>';
        return html;
    };
}


function CMSUserAction(username, action) {
    SendActionAndDisplayResult("/admin/user/" + action + "?username=" + username);
}