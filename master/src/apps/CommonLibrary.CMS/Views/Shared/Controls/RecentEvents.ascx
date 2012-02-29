<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Entities" %>
<%@ Import Namespace="ComLib.Data" %>
<%@ Import Namespace="ComLib.Web.Modules.Events" %>
<%@ Import Namespace="ComLib.Web.Modules.Categorys" %>
<%@ Import Namespace="ComLib.Extensions" %>

<% 
    // 1. Get recent events.
    int pageSize = 8;
    var recents = Event.Find(Query<Event>.New().Where(t => t.Id).MoreThan(1).OrderByDescending(t => t.Id).Limit(pageSize));
    
    // 2. Find uniuqe categories.
    var categoryGroups = from ev in recents group ev by CategoryHelper.FindParentCategoryNameFor<Event>(ev.CategoryId) into g select new { Category = g.Key, Events = g };
    
    // Iterate by category name.
    foreach (var group in categoryGroups)
    {
        IList<Event> events = group.Events.ToList();

        %>
        <h2><%= group.Category %></h2>
        <div class="posts"><table>
        <%
        // Now go over each event in the same category.
        foreach (var item in events)
        { %>
                <tr>
                    <td style="width:20px;">&nbsp;</td>
                    <td class="post" style="vertical-align:top; padding-top:5px;">
                        <span class="date"><%= Html.Encode(item.StartDate.ToString("MM/dd"))%></span><br />to<br />                  
                        <span class="date"><%= item.EndDate.ToString("MM/dd")%></span><br /><br />
                        <span class="time"><%= Html.Encode(item.Starts)%></span>
                    </td>
                    <td style="width:10px;">&nbsp;</td>
                    <td class="post">
                        <span class="headersmall"><a href="<%= "/event/show/" + item.SlugUrl %>"><%= Html.Encode(item.Title)%></a></span><br /> 
                        created by: <span class="user"><%= Html.Encode(item.CreateUser)%> </span><br /><br />                 
                        <span class="content"><%= Html.Encode(item.Description)%></span><br /><br />
                        <% if (item.Address != null)
                           { %>       
                           Address: <span class="address"><%= Html.Encode(item.Address.ToOneLine("venue"))%></span><br /><br />
                        <% } %>
                        <br />
                    </td>   
               </tr>
        <% } %>
       </table></div> 
    <% } %>