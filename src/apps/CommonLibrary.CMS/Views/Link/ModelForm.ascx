<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Links.Link>" %>


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
                <%= Html.ResourceFor(model => model.Url) %>
                <%= Html.TextBoxFor(model => model.Url)%>
                <br /><span class="example">http://www.google.com</span>
                <%= Html.ValidationMessageFor(model => model.Url)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Group) %>
                <%= Html.TextBoxFor(model => model.Group)%>
                <%= Html.ValidationMessageFor(model => model.Group)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.SortIndex)%>
                <%= Html.TextBoxFor(model => model.SortIndex)%>
                <%= Html.ValidationMessageFor(model => model.SortIndex)%>
            </p>


