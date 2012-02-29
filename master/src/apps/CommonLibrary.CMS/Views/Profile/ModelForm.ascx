<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Profiles.Profile>" %>


   
            <p>
                <%= Html.ResourceFor(model => model.Email, "Email")%> 
                <%= Html.TextBoxFor(model => model.Email, new { @class = "wide" })%> 
                <%= Html.ValidationMessageFor(model => model.Email)%>
            </p> 
            <p>
                <%= Html.ResourceFor(model => model.IsGravatarEnabled, "Is Gravatar Enabled") %>
                <%= Html.CheckBoxFor(model => model.IsGravatarEnabled)%>
                <%= Html.ValidationMessageFor(model => model.IsGravatarEnabled)%>
            </p>
            <p>
                <%= Html.ResourceFor("Enable Display Of Name") %>
                <%= Html.CheckBoxFor(model => model.EnableDisplayOfName)%>
                <%= Html.ValidationMessageFor(model => model.EnableDisplayOfName)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.FirstName, "First Name") %>
                <%= Html.TextBoxFor(model => model.FirstName)%>
                <%= Html.ValidationMessageFor(model => model.FirstName)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.LastName, "Last Name") %>
                <%= Html.TextBoxFor(model => model.LastName)%>
                <%= Html.ValidationMessageFor(model => model.LastName)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.WebSite, "WebSite")%>
                <%= Html.TextBoxFor(model => model.WebSite, new { @class = "wide" })%>
                <%= Html.ValidationMessageFor(model => model.WebSite)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.About) %>
                <%= Html.TextAreaFor(model => model.About, new { @class = "text-area" })%>
                <%= Html.ValidationMessageFor(model => model.About)%>
            </p>
            <p>
                <%= Html.ResourceFor("Enable Address") %>
                <%= Html.CheckBoxFor(model => model.IsAddressEnabled, new { @onclick = "ProfileAddress_Toggle();" })%>
                <%= Html.ValidationMessageFor(model => model.IsAddressEnabled)%>
            </p>
            <p> 
                <script type="text/javascript">
                    var addressDisplayLevel = "<%= Model.AddressDisplayLevel %>";
                    $(document).ready(function(){ ProfileAddress_Toggle(); });
                    
                    function ProfileAddress_Toggle() {                        
                        if($('#IsAddressEnabled').is(':checked'))
                            $('#addressArea').show();
                        else
                            $('#addressArea').hide();
                    }
                </script>          
                <div id="addressArea">
                    <p>
                        <%= Html.ResourceFor("Address Display Level") %>
                        <%= Html.DropDownList("AddressDisplayLevel", Html.ToDropDownList("City", new string[] {"Venue", "Street", "City", "State", "Country"})) %>
                        <%= Html.ValidationMessageFor(model => model.AddressDisplayLevel)%>
                    </p>
                    <table>
                        <tr>
                            <td>
                                <%= Html.ResourceFor(model => model.Address.Venue) %>
                                <%= Html.TextBoxFor(model => model.Address.Venue)%>
                                <br /><span class="example">e.g. Javits Convention Center</span>
                                <%= Html.ValidationMessageFor(model => model.Address.Venue)%>
                            </td>
                            <td>
                                <%= Html.ResourceFor(model => model.Address.Street) %>
                                <%= Html.TextBoxFor(model => model.Address.Street)%>
                                <br /><span class="example">e.g. 100 Broadway</span>
                                <%= Html.ValidationMessageFor(model => model.Address.Street)%>
                            </td>                        
                        </tr>
                       <tr>
                            <td><%= Html.ResourceFor(model => model.Address.City) %>
                                <%= Html.TextBoxFor(model => model.Address.City)%>
                                <br /><span class="example">e.g. Queens</span>
                                <%= Html.ValidationMessageFor(model => model.Address.City)%>
                            </td>
                            <td><%= Html.ResourceFor(model => model.Address.State) %>
                                <%= Html.TextBoxFor(model => model.Address.State)%>
                                <br /><span class="example">e.g. New York</span>
                                <%= Html.ValidationMessageFor(model => model.Address.State)%>
                            </td>                  
                        </tr>    
                        <tr>
                            <td>
                                <%= Html.ResourceFor(model => model.Address.Zip) %>
                                <%= Html.TextBoxFor(model => model.Address.Zip)%>
                                <br /><span class="example">e.g. 10017</span>
                                <%= Html.ValidationMessageFor(model => model.Address.Zip)%>
                            </td>
                            <td colspan="2"><%= Html.ResourceFor(model => model.Address.Country) %>
                                <%= Html.TextBoxFor(model => model.Address.Country)%>
                                <br /><span class="example">e.g. USA</span>
                                <%= Html.ValidationMessageFor(model => model.Address.Country)%>
                            </td> 
                        </tr>       
                    </table>
                </div>
            </p>
            <div><br />
                <div>
                    <% if(Model.HasImage) { %>
                            <h3>Profile Photo</h3>            
                            <%= Html.AjaxLinkWithAlert("Delete", "/profile/DeletePhoto/" + Model.Id)%> <br /><br />
                            <div id="profileImage"><%= Model.GetImageUrl(wrapInImageTag: true, size:45 ) %></div>
                    <% } %>
                </div>
                <div>
                    <% Html.RenderMediaUpload(Model, 1, Model.IsPersistant(), false, javascriptCallbackOnLoadComplete: "Profile_ImageApply"); %>
                </div>
            </div>


<script type="text/javascript">
    function Profile_ImageApply() {
        var id = <%= Model.Id %>;        
        AjaxHelper.SendActionAndCallback("/profile/ApplyProfilePhoto/" + id, function (res1) {
            if (res1.Success) {
                AjaxHelper.SendActionAndCallback("/profile/GetPhotoThumbnail/" + id, Profile_ImageRefresh);
            }
        });
    }


    function Profile_ImageRefresh(res)
    {
        if (res.Success)
            $("#profileImage").html(res.Item);
    }
</script>
