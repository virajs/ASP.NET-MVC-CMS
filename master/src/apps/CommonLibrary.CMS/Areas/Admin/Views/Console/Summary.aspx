<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<Dictionary<string, object>>" %>
<%@ Import Namespace="ComLib.BootStrapSupport" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Environment</h2><br />
    <table class="systemlist">
        <tr>
            <th>Area</th>
            <th>Setting</th>
        </tr>
        <% 
            List<KeyValuePair<string, string>> summaries = Model["summary"] as List<KeyValuePair<string, string>>;

            if (summaries != null && summaries.Count > 0)
            {
                foreach (var result in summaries)
                { %>
                   <tr>
                        <td><%= result.Key%></td>
                        <td><%= result.Value %></td>
                   </tr>
             <% }
            } %>
    </table>
    
    <br /><br />
    <h2>Widgets</h2><br />    
    <table class="systemlist">
        <tr>
            <th>Name</th>
            <th>Author</th>
            <th>Email</th>
            <th>Version</th>
            <th>Url</th>
        </tr>
        <%
            IList<Widget> widgets = Model["widgets"] as IList<Widget>;
            if(widgets != null && widgets.Count > 0)
            {
                foreach(var widget in widgets)
                {
        %>
            <tr>
                <td><%= widget.Name %></td>
                <td><%= widget.Author %></td>
                <td><%= widget.Email %></td>
                <td><%= widget.Version %></td>
                <td><%= widget.Url %></td>
            </tr>
            <% 
                }            
            } %>
    </table>
</asp:Content>
