<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Modules.Themes.LayoutsViewModel>" %>
<%@ Import Namespace="ComLib.CMS.Controllers" %>
<%@ Import Namespace="ComLib.Web.Modules.Themes" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Layout</h2><br />
    <span>NOTE: The layout of your dashboard will always have a single column even though you change the layout of your site.</span><br /><br />
    <h3><span class="error"><%= Model.Message %></span></h3>
    <% using (Html.BeginForm("ChangeLayout", "Theme")) { %>    
    <table class="systemlist">
        <tr>
            <th></th>
            <th>Example</th>
            <th>Layout Name</th>
        </tr>
    <%     
        // Can the layout be changed for the current theme?   
        if (Model.CanChangeLayout)
        {
            // Display each layout image and name.
            foreach (var layout in Model.Layouts)
            { 
                // Check off the current layout radio button. "checked="checked".
                bool isCurrentLayout = string.Compare(ComLib.Web.Modules.Themes.Theme.Current.SelectedLayout, layout.MasterName, true) == 0;
                string checkedAttr = isCurrentLayout ? "checked=\"checked\"" : string.Empty;
                %>
               <tr>
                    <td><input type="radio" name="layout_name" <%= checkedAttr %> value="<%= layout.MasterName %>" /></td>
                    <td><img src="<%= layout.ImagePage %>" alt="layout" /></td>
                    <td><%= layout.Name%></td>
               </tr>
    <%      }
        } 
        else { %>
        <tr>
            <td><img src="<%= Model.Layouts[0].ImagePage %>" alt="layout" /></td>
        </tr>
        <% } %>
    </table>
        <% if (Model.CanChangeLayout)
           { %> <br /><input type="button" value="Change" class="action" onclick="javascript:ChangeLayout();" />
        <% } %>
    <% } %>
     <script type="text/javascript">
         ///<summary>Sends an ajax get request to the url specified and displays the result message in the displayDivId supplied.
         ///</summary>
         ///<param name="url">The url to send the ajax action to.</param>
         ///<param name="displayDivId">The id of the div to show the resulting success/message combination result.</param>
         function ChangeLayout() {
             var layout = $("input[name='layout_name']:checked").val();
             SendActionAndDisplayResult('/theme/changelayout?layout=' + layout);
         }
       
    </script>
     
</asp:Content>