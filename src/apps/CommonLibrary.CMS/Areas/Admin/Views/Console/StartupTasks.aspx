<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<IList<ComLib.BootStrapSupport.Task>>" %>
<%@ Import Namespace="ComLib.BootStrapSupport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Startup Tasks</h2><br />
    <table class="systemlist">
        <tr>
            <th>Task Name</th>
            <th>Executed</th>
            <th>Success</th>
            <th>Group</th>
            <th>Priority</th>
            <th>Message</th>
        </tr>
        <% 
            if (Model != null && Model.Count > 0)
            {
                foreach (var result in Model)
                { %>
                   <tr>
                        <td><%= result.Name%></td>
                        <td><%= result.ExecutedOn.ToShortTimeString() %></td>
                        <td><%= result.Status()%></td>
                        <td><%= result.Group %></td>
                        <td><%= result.Priority.ToString() %></td>
                        <td><%= result.Message %></td>
                   </tr>
             <% }
            } %>
    </table>
</asp:Content>
