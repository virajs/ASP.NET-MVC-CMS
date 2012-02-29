<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Posts.Post>" %>
<%@ Import Namespace="ComLib.Web.Modules.Posts" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>
        
    <p>                
        <%= Html.ResourceFor(model => model.Title) %>
        <%= Html.TextBoxFor(model => model.Title, new { @class = "wide" })%>
        <%= Html.ValidationMessageFor(model => model.Title) %>
    </p>
    <p>
        <%= Html.ResourceFor("Category") %>
        <%= ComLib.Web.Modules.Categorys.CategoryHelper.BuildCategoriesFor<Post>("CategoryId", "&nbsp;&nbsp;&nbsp;&nbsp;")%>
        <%= Html.ValidationMessageFor(model => model.CategoryId)%>
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
        <%= Html.ResourceFor(model => model.Tags) %>
        <%= Html.TextBoxFor(model => model.Tags)%>
        <%= Html.ValidationMessageFor(model => model.Tags)%>
    </p>
    <p>
        <%= Html.ResourceFor(model => model.Slug) %>
        <%= Html.TextBoxFor(model => model.Slug) %>
        <%= Html.ValidationMessageFor(model => model.Slug) %>
    </p>
    <p>
        <%= Html.ResourceFor(model => model.IsPublished, "Publish") %>
        <%= Html.CheckBoxFor(model => model.IsPublished)%>
        <%= Html.ValidationMessageFor(model => model.IsPublished)%>
    </p>
    <p>
        <%= Html.ResourceFor(model => model.IsFavorite, "Is Favorite") %>
        <%= Html.CheckBoxFor(model => model.IsFavorite) %>
        <%= Html.ValidationMessageFor(model => model.IsFavorite) %>
    </p>
    <p>
        <%= Html.ResourceFor(model => model.IsPublic, "Is Public") %>
        <%= Html.CheckBoxFor(model => model.IsPublic) %>
        <%= Html.ValidationMessageFor(model => model.IsPublic) %>
    </p>
    <p>
        <%= Html.ResourceFor(model => model.IsCommentEnabled, "Enable Comments") %>
        <%= Html.CheckBoxFor(model => model.IsCommentEnabled)%>
        <%= Html.ValidationMessageFor(model => model.IsCommentEnabled) %>
    </p>
    <p>
        <%= Html.ResourceFor(model => model.IsCommentModerated, "Moderate Comments") %>
        <%= Html.CheckBoxFor(model => model.IsCommentModerated)%>
        <%= Html.ValidationMessageFor(model => model.IsCommentModerated) %>
    </p>
    <p>
        <%= Html.ResourceFor(model => model.IsRatable, "Enable Ratings") %>
        <%= Html.CheckBoxFor(model => model.IsRatable)%>
        <%= Html.ValidationMessageFor(model => model.IsRatable) %>
    </p>
    <p>
        <% Html.RenderMediaUpload(Model, 1, true); %><br /><br />
    </p>
    <script type="text/javascript">
        // Set up the location fields.
        $(document).ready(function () {
            $("#CategoryId").val('<%= Model.CategoryId %>');
        });
    </script>
