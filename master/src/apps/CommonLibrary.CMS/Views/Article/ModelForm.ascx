<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Articles.Article>" %>


            <p>
                <%= Html.ResourceFor(model => model.Title) %>
                <%= Html.TextBoxFor(model => model.Title)%>
                <%= Html.ValidationMessageFor(model => model.Title)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.PublishDate) %>
                <%= Html.TextBoxFor(model => model.PublishDate)%>
                <%= Html.ValidationMessageFor(model => model.PublishDate)%>
            </p>

