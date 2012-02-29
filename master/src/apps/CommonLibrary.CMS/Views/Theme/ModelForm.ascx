<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Themes.Theme>" %>


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
                <%= Html.ResourceFor(model => model.Path) %>
                <%= Html.TextBoxFor(model => model.Path)%>
                <%= Html.ValidationMessageFor(model => model.Path)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Layouts) %>
                <%= Html.TextBoxFor(model => model.Layouts)%>
                <%= Html.ValidationMessageFor(model => model.Layouts)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Author) %>
                <%= Html.TextBoxFor(model => model.Author)%>
                <%= Html.ValidationMessageFor(model => model.Author)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Version) %>
                <%= Html.TextBoxFor(model => model.Version)%>
                <%= Html.ValidationMessageFor(model => model.Version)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Email) %>
                <%= Html.TextBoxFor(model => model.Email)%>
                <%= Html.ValidationMessageFor(model => model.Email)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Url) %>
                <%= Html.TextBoxFor(model => model.Url)%>
                <br /><span class="example">http://www.google.com</span>
                <%= Html.ValidationMessageFor(model => model.Url)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.SortIndex)%>
                <%= Html.TextBoxFor(model => model.SortIndex)%>
                <%= Html.ValidationMessageFor(model => model.SortIndex)%>
            </p>


