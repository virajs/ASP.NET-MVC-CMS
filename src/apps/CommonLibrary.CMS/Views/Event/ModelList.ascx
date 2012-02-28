<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Events.Event>>" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>
<%@ Import Namespace="ComLib.Web.Modules.Posts" %>
<%@ Import Namespace="ComLib.Web.Modules.Events" %>
    <br />
    <div class="posts">
        <table>
        <% foreach (var item in Model.Items) {
               string permaLink = "/event/show/" + item.SlugUrl;  
            %>
            <tr>
                <td class="post" style="vertical-align:top; padding-top:5px;">
                    <span class="date"><%= Html.Encode(item.StartDate.ToString("MM/dd")) %></span><br />to<br />                  
                    <span class="date"><%= item.EndDate.ToString("MM/dd") %></span><br /><br />
                    <span class="time"><%= Html.Encode(item.Starts) %></span>
                </td>
                <td style="width:10px;">&nbsp;</td>
                <td class="post">
                    <span class="header"><a href="<%= permaLink %>"><%= Html.Encode(item.Title)%></a></span><br />                                         
                    <span class="createdate"><%= Html.Encode(item.UpdateDate.ToString("MMMM d, yyyy  HH:mm"))%> </span>&nbsp;&nbsp; posted by:
                    <span class="user"><a href="/profile/show/<%= Html.Encode(item.CreateUser) %>"><%= Html.Encode(item.CreateUser) %></a> </span><br /><br />                 
                    <span class="content"><%= Html.Encode(item.Description) %></span><br /><br />
                    <% if (item.Address != null)
                       { %>       
                       <b>Address:</b> <span class="address"><%= Html.Encode(item.Address.ToOneLine("city"))%></span><br /><br />
                    <% } %>
                    <% if (item.Category != null) { %>
                        Category: <span class="category"><%= Html.Encode(item.Category.Name)%></span><br /> 
                    <% } %>
                    <br />
                    <% Html.RenderPartial("~/views/shared/Controls/EntityActions.ascx", PostHelper.BuildActionsFor<Event>(false, 0, item.Id, item.Title, false, false, true, this.Context, permaLink)); %>
                    <br /><br />
                    <% if (Model.ShowEditDelete)
                   {%>                   
                    <% Html.RenderPartial("~/views/shared/Controls/EntityManage.ascx", new EntityListManageViewModel() { Id = item.Id, Item = item, ViewInfo = Model }); %>
                <% } %><br /><br />
                </td>   
           </tr>
        <% } %>
        </table>
    </div>
    
