<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Feedbacks.Feedback>" %>

<fieldset>
    <div><h2><%= Model.Name %></h2></div>
    <p>
        <%= Html.ResourceFor(model => model.Title)%>
        <%= Html.Encode(Model.Title) %>
    </p>
    <p>
        <%= Html.ResourceFor(model => model.Name)%>
        <%= Html.Encode(Model.Name)%>
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
        <%= Html.ResourceFor(model => model.Content)%>
        <%= Html.Encode(Model.Content)%>
    </p>
</fieldset>