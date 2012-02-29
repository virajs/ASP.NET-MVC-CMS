<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.MenuEntrys.MenuEntry>" %>


            <p>
                <%= Html.ResourceFor(model => model.Name) %>
                <%= Html.TextBoxFor(model => model.Name)%>
                <%= Html.ValidationMessageFor(model => model.Name)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Description) %>
                <%= Html.TextBoxFor(model => model.Description) %>
                <%= Html.ValidationMessageFor(model => model.Description) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Url) %>
                <%= Html.TextBoxFor(model => model.Url)%>
                <%= Html.ValidationMessageFor(model => model.Url)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Roles) %>
                <%= Html.TextBoxFor(model => model.Roles)%>
                <%= Html.ValidationMessageFor(model => model.Roles)%>
            </p>                                   
            <p>
                <%= Html.ResourceFor(model => model.RefId) %>
                <%= Html.TextBoxFor(model => model.RefId)%>
                <%= Html.ValidationMessageFor(model => model.RefId)%>
            </p>           
            <p>
                <%= Html.ResourceFor(model => model.SortIndex) %>
                <%= Html.TextBoxFor(model => model.SortIndex)%>
                <%= Html.ValidationMessageFor(model => model.SortIndex)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.IsPublic) %>
                <%= Html.TextBoxFor(model => model.IsPublic) %>
                <%= Html.ValidationMessageFor(model => model.IsPublic) %>
            </p>  
            <p>
                <%= Html.ResourceFor(model => model.IsRerouted) %>
                <%= Html.TextBoxFor(model => model.IsRerouted)%>
                <%= Html.ValidationMessageFor(model => model.IsRerouted)%>
            </p>

