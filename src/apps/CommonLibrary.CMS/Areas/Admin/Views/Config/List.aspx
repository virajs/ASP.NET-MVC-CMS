<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<EntityListViewModel<ComLib.Configuration.ConfigItem>>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List</h2>

    <table class="systemlist">
        <tr>
            <th></th>            
            <th>
                Id
            </th>
            <th>
                App
            </th>
            <th>
                Name
            </th>
            <th>
                Section
            </th>
            <th>
                Key
            </th>
            <th>
                Val
            </th>
            <th>
                ValType
            </th>
        </tr>

    <% foreach (var item in Model.Items) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id = item.Id })%> |
                <%= Html.ActionLink("Delete", "Delete", new { id = item.Id })%>
            </td>
            <td>
                <%= Html.Encode(item.Id) %>
            </td>
            <td>
                <%= Html.Encode(item.App) %>
            </td>
            <td>
                <%= Html.Encode(item.Name) %>
            </td>
            <td>
                <%= Html.Encode(item.Section) %>
            </td>
            <td>
                <%= Html.Encode(item.Key) %>
            </td>
            <td>
                <%= Html.Encode(item.Val) %>
            </td>
            <td>
                <%= Html.Encode(item.ValType) %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>