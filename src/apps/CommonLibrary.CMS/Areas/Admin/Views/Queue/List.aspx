<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<IList<ComLib.Queue.QueueStatus>>" %>
<%@ Import Namespace="ComLib.Queue" %>
<%@ Import Namespace="ComLib.Extensions" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Queues</h2>
    <table class="systemlist">
         <tr>           
            <th>Name</th>
            <th>Remaining</th>
            <th>Date</th>
            <th>Elapsed</th>
            <th>Dequeue</th>
            <th>Process #</th>
            <th>Total</th>
            <th>State</th>
         </tr>
         <% foreach (var item in Model)
            { %>
            <tr>
                <td>
                    <%= Html.Encode(item.Name.TruncateWithText(15, "..."))%>
                </td>
                <td>
                    <%= Html.Encode(item.Count)%>
                </td>
                <td>
                    <%= Html.Encode(item.LastProcessDate.ToString("hh:mm:ss"))%>
                </td>
                <td>
                    <%= Html.Encode(item.ElapsedTimeSinceLastProcessDate.ToMilitaryString())%>
                </td>
                <td>
                    <%= Html.Encode(item.DequeueSize)%>
                </td>
                <td>
                    <%= Html.Encode(item.NumberOfTimesProcessed)%>
                </td>
                <td>
                    <%= Html.Encode(item.TotalProcessed)%>
                </td>
                <td>
                    <%= Html.Encode(item.State.ToString())%>
                </td>
            </tr>
         <% } %>
     </table>
</asp:Content>
