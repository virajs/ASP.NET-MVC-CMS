<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Parts.Part>" %>


    
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
                <%= Html.ResourceFor(model => model.AccessRoles, "Access Roles") %>
                <%= Html.TextBoxFor(model => model.AccessRoles) %>
                <%= Html.ValidationMessageFor(model => model.AccessRoles) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.IsPublic, "Is Public") %>
                <%= Html.TextBoxFor(model => model.IsPublic) %>
                <%= Html.ValidationMessageFor(model => model.IsPublic) %>
            </p>


