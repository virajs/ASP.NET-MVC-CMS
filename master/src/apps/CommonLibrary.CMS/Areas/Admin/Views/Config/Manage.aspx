<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<EntityListViewModel<ComLib.Configuration.ConfigItem>>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form action="/admin/config/updatesettings" method="post">
    <h2>Configuration Settings</h2>
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
                <%= Html.ActionLink("Edit", "Edit", new { id = item.Id }) %> |
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
    
    Update <br />
    Setting : <input id="settingName" type="text" />
    Value : <input id="settingValue" type="text" />
    <p>
       <input type="submit" value="Update" class="action" />     
    </p>
   
    
    <p>
        <%= Html.ActionLink("Load", "LoadSettings", new { name = Model.Url }, new { @class = "actionlink" })%>
        <%= Html.ActionLink("Save", "SaveSettings", new { name = Model.Url }, new { @class = "actionlink" })%>
    </p>
      
      </form>
</asp:Content>