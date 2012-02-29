<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Links.Link>" %>


   
            <p>
                <%= Html.ResourceFor(model => model.Name)%>
                <%= Html.Encode(Model.Name) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Description) %>
                <%= Html.Encode(Model.Description)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Url)%>
                <%= Html.Encode(Model.Url)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Group) %>
                <%= Html.Encode(Model.Group)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.SortIndex)%>
                <%= Html.Encode(Model.SortIndex)%>
            </p>


