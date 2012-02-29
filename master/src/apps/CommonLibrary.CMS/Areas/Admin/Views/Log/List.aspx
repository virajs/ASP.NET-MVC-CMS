<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.CMS.Areas.Admin.LogViewModel>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>
<%@ Import Namespace="ComLib.Logging" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <% using (Html.BeginForm())
    {%>
    <h2>Logs</h2><br />
    <%= Html.Hidden("LogAction") %>    
    <table>
    <tr><td>Delete By Level</td><td><%= Html.DropDownList("LogLevel", Model.LogLevels)%></td><td><input class="action" type="button" id="delByLevel" value="Delete" onclick="OnLogDelete('DeleteByLevel');"/></td></tr>
    <tr><td>Delete By Date</td><td><input type="text" id="LogDate" /></td><td><input class="action" type="button" id="delByDate" value="Delete" onclick="OnLogDelete('DeleteByDate');" /></td></tr>    
    </table>
    
    <br />
    <%= Html.ActionLink("Delete All", "DeleteAll", null, new { @class = "actionlink" })%> 
    <%= Html.ActionLink("Flush Logs", "FlushAll", null, new { @class = "actionlink" })%> 
    <br />
    <script type="text/javascript">
        function OnLogDelete(actionName) 
        {
            //alert("deleting by " + actionName);
            document.getElementById('LogAction').value = actionName;
            document.forms[0].submit();
        }
    </script>
    <br />
        <table class="systemlist">
             <tr>
                <th></th>
                <th>Id</th>
                <th>Created</th>
                <th>Application</th>
                <th>Level</th>
                <th>Computer</th>
                <th>User</th>
                <th>Exception</th>
        <% foreach (var item in Model.PagerView.Items)
           {
               string error = string.IsNullOrEmpty(item.Exception)
                            ? ComLib.Extensions.StringExtensions.TruncateWithText(item.Message, 150, "...")
                            : ComLib.Extensions.StringExtensions.TruncateWithText(item.Exception, 150, "...");
               %>
        
            <tr>
                <td>
                    <%= Html.ActionLink("Delete", "DeleteById", new { id=item.Id }) %> 
                </td>
                <td>
                    <%= Html.Encode(item.Id)%>
                </td>
                <td>
                    <%= Html.Encode(item.CreateDate)%>
                </td>
                <td>
                    <%= Html.Encode(item.Application)%>
                </td>
                <td>
                    <%= Html.Encode(item.LogLevelName)%>
                </td>  
                <td>
                    <%= Html.Encode(item.Computer)%>
                </td>
                <td>
                    <%= Html.Encode(item.UserName)%>
                </td>
                <td>
                    <%= Html.Encode(error)%>
                </td>
            </tr>
        <% } %>
        </table>
    <br /><br /><% Html.RenderPartial("Controls/Pager", Model.PagerView); %><br /><br /><br /> 
    
 <% } %>
</asp:Content>
