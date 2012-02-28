<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.MenuEntrys.MenuEntry>" %>


   
        <fieldset>
            <p>
                <%= Html.ResourceFor(model => model.Name) %>
                <%= Html.Encode(Model.Name)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Description) %>
                <%= Html.Encode(Model.Description) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Url) %>
                <%= Html.Encode(Model.Url)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Roles) %>
                <%= Html.Encode(Model.Roles)%>
            </p>                                   
            <p>
                <%= Html.ResourceFor(model => model.RefId) %>
                <%= Html.Encode(Model.RefId)%>
            </p>           
            <p>
                <%= Html.ResourceFor(model => model.SortIndex) %>
                <%= Html.Encode(Model.SortIndex)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.IsPublic) %>
                <%= Html.Encode(Model.IsPublic) %>
            </p>  
            <p>
                <%= Html.ResourceFor(model => model.IsRerouted) %>
                <%= Html.Encode(Model.IsRerouted)%>
            </p>
        </fieldset>

   

