<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.CMS.Models.Widgets.Users>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Data" %>
<%@ Import Namespace="ComLib.Web.Services.GravatarSupport" %>
<%@ Import Namespace="ComLib.Web.Modules.Profiles" %>
<%
    IQuery<Profile> query = Query<Profile>.New().Where(p => p.IsGravatarEnabled).Is(true).Or(p => p.ImageRefId).MoreThan(0);
    PagedList<Profile> items = ComLib.Web.Modules.Profiles.Profile.Find(query, 1, Model.NumberOfEntries);
%>  
<div class="usercloud">
    <table>
<%
    // This dispalys photos in columns of 3.
    Gravatar gravatar = new Gravatar();
    EnumerableHelper.ForEachByCols(items.Count, Model.NumberOfEntriesAcross, (startNdx, endNdx) =>
    {                    
        %><tr><%
        for(int ndx = startNdx; ndx <= endNdx; ndx++)
        {
            ComLib.Web.Modules.Profiles.Profile profile = items[ndx];
            string imageUrl = profile.GetImageUrl(true, 40, true, "top", "border:0", true);
            /*
            if (profile.IsGravatarEnabled)
            {
                gravatar.Init(profile.Email, 40, Rating.g, IconType.none, ".png");
                imageUrl = gravatar.Url;
            }
            */
            %>
            <td><a href="/profile/DetailsByUser/<%= profile.UserId %>"><%= imageUrl %></a></td>
     <% } %>
        </tr>
<% }); %>
    </table>
</div>

