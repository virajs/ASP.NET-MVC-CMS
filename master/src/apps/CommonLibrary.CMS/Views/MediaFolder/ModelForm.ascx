<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Media.MediaFolder>" %>


            <p>
                <%= Html.ResourceFor(model => model.Name) %>
                <%= Html.TextBoxFor(model => model.Name) %>
                <%= Html.ValidationMessageFor(model => model.Name) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Description) %>
                <%= Html.TextBoxFor(model => model.Description) %>
                <%= Html.ValidationMessageFor(model => model.Description) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.IsPublic)%>
                <%= Html.CheckBoxFor(model => model.IsPublic)%>
                <%= Html.ValidationMessageFor(model => model.IsPublic)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.SortIndex)%>
                <%= Html.TextBoxFor(model => model.SortIndex)%>
                <%= Html.ValidationMessageFor(model => model.SortIndex)%>
            </p>


