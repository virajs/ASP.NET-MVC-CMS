<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Comments.Comment>" %>


            <p>
                <%= Html.ResourceFor(model => model.GroupId) %>
                <%= Html.Encode(Model.GroupId)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.RefId)%>
                <%= Html.Encode(Model.RefId)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Title) %>
                <%= Html.Encode(Model.Title)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Content) %>
                <%= Html.Encode(Model.Content)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Name)%>
                <%= Html.Encode(Model.Name) %>
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
                <%= Html.ResourceFor(model => model.IsGravatarEnabled) %>
                <%= Html.Encode(Model.IsGravatarEnabled)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Rating)%>
                <%= Html.Encode(Model.Rating)%>
            </p>
            


