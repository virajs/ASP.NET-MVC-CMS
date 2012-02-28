<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Themes.Theme>>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>
<%@ Import Namespace="ComLib.Web.Modules.Themes" %>

<div class="posts"> 
        
        <table>
            <%                
                // This will iterate over every 3 items.
                EnumerableHelper.ForEachByCols(Model.Items.Count, 3, (startNdx, endNdx) =>
                {         
                    %>
                    <tr>
                    <%           
                    for(int ndx = startNdx; ndx <= endNdx; ndx++)
                    {
                        Theme theme = Model.Items[ndx];                        
                    %>  
                        <td style="padding:5px 20px 10px 20px">
                        <img src="<%= theme.Path %>/theme.jpg" style="border:0" alt="<%= theme.Path %>"/><br />
                        <h2><%= Html.Encode(theme.Name) %></h2> <br />
                        <input type="button" class="action" value="Activate" onclick="SendActionAndDisplayResult('/theme/ActivateTheme?themeid=<%= theme.Id %>');" />
                        <%= Html.ActionLink("Preview", "PreviewTheme", "Home", new { themename = theme.Name }, new { @class = "actionlink" })%>
                        <br /><br />
                        <%= Html.Encode(theme.Description) %><br />
                        Author: <%= Html.Encode(theme.Author) %><br />
                        Version: <%= Html.Encode(theme.Version) %><br />
                        </td>
                    <% } %>
                    </tr>                    
             <%   }); %> 
        </table>
</div>
