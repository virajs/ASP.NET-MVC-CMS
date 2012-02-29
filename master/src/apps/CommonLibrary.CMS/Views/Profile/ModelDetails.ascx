<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Profiles.Profile>" %>
<%@ Import Namespace="ComLib.CMS.Models.Widgets" %>
<fieldset>
    
        <div><h2><%= Model.UserName %></h2></div>        
        <table>
            <tr>
                <td><%= Model.GetImageUrl(wrapInImageTag: true, size: 200) %></td>
                <% if(Model.IsAddressEnabled){ %>
                    <td><% Html.RenderPartial("~/views/shared/widgets/maps.ascx", new GeoMap() { FullAddress = Model.Address.ToOneLine(Model.AddressDisplayLevel), Width = 300, Height = 200 });  %></td>
                <% } %>
            </tr>
        </table>     
        
        <br /><br />
        <div>
            <% if(Model.EnableDisplayOfName){ %>
            <div>
                <%= Html.ResourceFor("Name") %>
                <%= Html.Encode(Model.FirstName)%> <%= Html.Encode(Model.LastName)%><br /><br />
            </div>
            <%} %>
            <% if (Model.IsAddressEnabled)
              { %>
            <div>
                <%= Html.ResourceFor(model => model.Address, "Address")%>
                <%= Html.Encode(Model.Address.ToOneLine(Model.AddressDisplayLevel))%><br /><br />
            </div>
            <%} %>
            <div>
                <%= Html.ResourceFor(model => model.WebSite, "WebSite")%>
                <%= Html.Encode(Model.WebSite)%><br /><br />
            </div>            
            <div>
                <%= Html.ResourceFor(model => model.About) %>
                <%= Html.Encode(Model.About)%><br />
            </div>
        </div>
</fieldset>
