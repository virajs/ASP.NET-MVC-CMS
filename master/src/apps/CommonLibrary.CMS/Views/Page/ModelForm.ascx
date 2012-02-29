<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Pages.Page>" %>


            <p>
                <%= Html.ResourceFor(model => model.Title) %>
                <%= Html.TextBoxFor(model => model.Title)%>
                <%= Html.ValidationMessageFor(model => model.Title)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Description) %>
                <%= Html.TextBoxFor(model => model.Description) %>
                <%= Html.ValidationMessageFor(model => model.Description) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Content) %>
                <%= Html.TextAreaFor(model => model.Content, new { @class = "text-area" })%>
                <%= Html.ValidationMessageFor(model => model.Content) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Keywords) %>
                <%= Html.TextBoxFor(model => model.Keywords)%>
                <%= Html.ValidationMessageFor(model => model.Keywords)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Slug) %>
                <%= Html.TextBoxFor(model => model.Slug)%>
                <%= Html.ValidationMessageFor(model => model.Slug)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.AccessRoles, "Access Roles") %>
                <%= Html.TextBoxFor(model => model.AccessRoles) %>
                <%= Html.ValidationMessageFor(model => model.AccessRoles) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Parent) %>
                <%= Html.TextBoxFor(model => model.Parent) %>
                <%= Html.ValidationMessageFor(model => model.Parent) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.IsPublic, "Is Public") %>
                <%= Html.CheckBoxFor(model => model.IsPublic) %>
                <%= Html.ValidationMessageFor(model => model.IsPublic) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.IsFrontPage, "Add To Menu") %>
                <%= Html.CheckBoxFor(model => model.IsFrontPage)%>
                <%= Html.ValidationMessageFor(model => model.IsFrontPage)%>
            </p>

