<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Events.Event>" %>
<%@ Import Namespace="ComLib.Web.Modules.Events" %>
<%@ Import Namespace="ComLib.Web.Modules.Posts" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="ComLib.Web.Lib" %>

<%if (Model != null)
  {
      ComLib.Maps.GoogleMapUrlBuilder google = new ComLib.Maps.GoogleMapUrlBuilder();
      ComLib.Maps.YahooMapUrlBuilder yahoo = new ComLib.Maps.YahooMapUrlBuilder();
      string mapUrlYahoo = yahoo.Build(Model.Address);
      string mapUrlGoogle = google.Build(Model.Address);
      string fullAddress = Model.Address.IsOnline ? "online" : Model.Address.ToOneLine();
      %>   
        <table>
            <tr>
                <td colspan="3"><h2><%= Html.Encode(Model.Title)%></h2><br /></td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td><span class="label">Date</span></td>
                            <td><%= Html.Encode(Model.StartDate.ToShortDateString())%>  to <%= Html.Encode(Model.EndDate.ToShortDateString())%></td>                        
                        </tr>
                        <tr>
                            <td><span class="label">Time</span></td>
                            <td><%= Html.Encode(Model.Times)%></td>                        
                        </tr>
                        <% if(Model.Category != null) { %>
                        <tr>
                            <td><span class="label">Category</span></td>
                            <td><%= Html.Encode(Model.Category.Name)%></td>
                        </tr>
                        <% } %>
                        <tr>
                            <td><span class="label">Email</span></td>
                            <td><%= Html.Encode(Model.Email)%></td>
                        </tr>
                        <tr>
                            <td><span class="label">Costs</span></td>
                            <td><%= Html.Encode(Model.Costs)%></td>            
                        </tr>
                        <tr>
                            <td><span class="label">Skill</span></td>
                            <td><%= Html.Encode(Model.SkillText)%></td>            
                        </tr>
                        <tr>
                            <td><span class="label">Ages</span></td>
                            <td><%= Html.Encode(Model.Ages) %></td>            
                        </tr>
                         <tr>
                            <td><span class="label">Seats</span></td>
                            <td><%= Html.Encode(Model.Seating) %></td>            
                        </tr>
                        <tr>
                            <td><span class="label">Phone</span></td>
                            <td><%= Html.Encode(Model.Phone)%></td>
                        </tr>                    
                        <tr>
                            <td><span class="label">Maps</span></td>
                            <td><a href="<%=mapUrlGoogle %>" target="_blank"><img src="/Content/images/generic/mapslink_google.jpg" alt="google maps" id="imgmapgoogle" /></a> &nbsp;&nbsp;&nbsp;&nbsp;
                                <a href="<%=mapUrlYahoo %>" target="_blank"><img src="/Content/images/generic/mapslink_yahoo.gif" alt="yahoo maps" id="imgmapyahoo" /></a>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:50px"></td>
                <td>
                    <% if( !Model.IsOnline ){ %>
                    <% Html.RenderPartial("~/views/shared/Controls/Maps.ascx", new ComLib.Maps.GeoAddressViewModel(Model.Address, 250, 250)); %>
                    <%} %>
                </td>
            </tr>                        
            <tr>
                <td colspan="3" class="post">
                    <span class="label">Url:</span><a href="<%= Html.Encode(Model.Url)%>"><%= Html.Encode(Model.Url)%></a>
                </td>
            </tr>
            <% if( !string.IsNullOrEmpty(Model.Tags)) { %>
            <tr>
                <td colspan="3">
                    <span class="label">Tags:</span> <span class="tags"><%= Html.Encode(Model.Tags)%></span>
                </td>
            </tr>    
            <% } %>
            <tr>
                <td colspan="3" class="post">
                    <span class="label">Address:</span><%= fullAddress%>
                </td>
            </tr>  
            <tr>
                <td colspan="3" class="post">
                    <br />
                    <div class="actions">
                        <% Html.RenderPartial("~/views/shared/Controls/EntityActions.ascx", PostHelper.BuildActionsFor<Event>(true, 0, Model.Id, Model.Title, false, false, true, this.Context)); %>
                    </div>
                </td>
            </tr>  
            <tr>
                <td colspan="3"><br /><br />
                   <span class="label">Description</span><br /><br />
                    <%= Html.Encode(Model.Content)%><br /><br /><br /><br />
                </td>
            </tr>   
            <tr>
                <td colspan="3"><span class="label">Posted by:</span><a href="/profile/show/<%= Html.Encode(Model.CreateUser)%>"><%= Html.Encode(Model.CreateUser)%></a></td>
            </tr>       
        </table>
        <br /><br />
        <% Html.RenderMediaGallery(Model); %>                   
     
<% } %>