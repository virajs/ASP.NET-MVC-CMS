<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Articles.Article>" %>


   
            <p>
                <%= Html.ResourceFor(model => model.Title)%>
                <%= Html.Encode(Model.Title) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.PublishDate) %>
                <%= Html.Encode(Model.PublishDate)%>
            </p>

