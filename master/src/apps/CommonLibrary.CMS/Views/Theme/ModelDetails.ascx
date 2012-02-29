<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Themes.Theme>" %>


   
            <p>
                <%= Html.ResourceFor(model => model.Name)%>
                <%= Html.Encode(Model.Name) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Description) %>
                <%= Html.Encode(Model.Description)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Path)%>
                <%= Html.Encode(Model.Path)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Layouts)%>
                <%= Html.Encode(Model.Layouts)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Author) %>
                <%= Html.Encode(Model.Author)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Version) %>
                <%= Html.Encode(Model.Version)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Email)%>
                <%= Html.Encode(Model.Email)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Url)%>
                <%= Html.Encode(Model.Url)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.SortIndex)%>
                <%= Html.Encode(Model.SortIndex)%>
            </p>


