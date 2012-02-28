<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Entities" %>
<%@ Import Namespace="ComLib.Data" %>
<%@ Import Namespace="ComLib.Web.Modules.Profiles" %>
<%@ Import Namespace="ComLib.Extensions" %>

<% 
    // 1. Get recent events.
    int pageSize = 6;
    var recents = ComLib.Web.Modules.Profiles.Profile.Find(Query<Profile>.New().Where(t => t.Id).MoreThan(1).OrderByDescending(t => t.Id).Limit(pageSize));
    if (recents != null && recents.Count > 0)
    {   
        %><table><%
            EnumerableHelper.ForEachByCols(recents.Count, 4, (startNdx, endNdx) =>
            {   
                %><tr><%
                for (int ndx = startNdx; ndx <= endNdx; ndx++)
                { 
                    var profile = recents[ndx]; %>
                    <td><a href="/profile/DetailsByUser/<%= profile.UserId %>"><%= profile.Name %></a>&nbsp;&nbsp;&nbsp;<br /><br /></td> <%
                } 
                %></tr><%
           });
       %></table>
 <% } %>