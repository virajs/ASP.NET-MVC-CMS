<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<IList<ComLib.Scheduling.TaskSummary>>" %>
<%@ Import Namespace="ComLib.Scheduling" %>
<%@ Import Namespace="ComLib.Extensions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Tasks</h2>
    <table class="systemlist">
         <tr>           
            <th>Name</th>
            <th>Last Run Date</th>
            <th>Elapsed</th>
            <th>Is Active</th>
            <th># Times Run</th>
            <th>Run Now</th>
         </tr>
         <% foreach (var item in Model)
            { %>
            <tr>
                <td>
                    <%= Html.Encode(item.Name.TruncateWithText(15, "..."))%>
                </td>
                <td>
                    <%= Html.Encode(item.LastProcessDate.ToString("hh:mm:ss"))%>
                </td>
                <td>
                    <%= Html.Encode(item.ElapsedTimeSinceLastProcessDate.ToMilitaryString())%>
                </td>
                <td>
                    <%= Html.Encode(item.IsActive)%>
                </td>
                <td>
                    <%= Html.Encode(item.NumberOfTimesProcessed)%>
                </td>
                <td>
                    <%= Html.ActionLink("Run", "Run", new { taskname = item.Name }, new { @class = "actionlink" }) %>
                </td>
            </tr>
         <% } %>
     </table>
</asp:Content>
