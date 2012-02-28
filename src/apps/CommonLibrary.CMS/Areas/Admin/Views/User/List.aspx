<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<EntityListViewModel<ComLib.Account.User>>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Users</h2>
    <table class="systemlist">
         <tr>         
            <th>Id</th>  
            <th>Name</th>
            <th>Email</th>
            <th>Roles</th>
            <th>Approved</th>
            <th>Locked</th>
            <th>Last Login</th>
    <% foreach (var item in Model.Items)
       { %>
    
        <tr>
            <td><%= Html.Encode(item.Id) %></td>
            <td>
                <%= Html.Encode(item.UserName)%>
            </td>
            <td>
                <%= Html.Encode(item.Email)%>
            </td>
            <td>
                <%= Html.Encode(item.Roles)%>
            </td>
            <td>
                <%= Html.Encode(item.IsApproved)%>
            </td>
            <td>
                <%= Html.Encode(item.IsLockedOut)%>
            </td>
            <td>
                <%= Html.Encode(item.LastLoginDate.ToShortDateString())%>
            </td>       
        </tr>
        <tr>
            <td colspan="6" class="post">
                <span class="actions">Actions: 
                    <%= Html.ActionLink("Approve", "Approve", new { userName = item.UserName })%> | <%= Html.ActionLink("Warn", "Warn", new { userName = item.UserName })%> |
                    <%= Html.ActionLink("Lock", "LockOut", new { userName = item.UserName })%> | <%= Html.ActionLink("Unlock", "UndoLockOut", new { userName = item.UserName })%> |
                    <%= Html.ActionLink("Send Pswd", "LockOut", new { userName = item.UserName })%> |                    
                    <a href="/account/remove?username=<%=item.UserName %>">Remove</a>
                </span><br /><br />
            </td>
        </tr>
    <% } %>
    </table>
    <br /><br /><% Html.RenderPartial("Controls/Pager", Model); %><br /><br /><br />
</asp:Content>
