<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Comments.Comment>" %>


            <p>
                <%= Html.ResourceFor(model => model.Title) %>
                <%= Html.TextBoxFor(model => model.Title, new { @class = "wide" })%>
                <%= Html.ValidationMessageFor(model => model.Title)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Content) %>
                <%= Html.TextAreaFor(model => model.Content, new { @class = "text-area" })%>
                <%= Html.ValidationMessageFor(model => model.Content)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Name) %>
                <%= Html.TextBoxFor(model => model.Name) %>
                <%= Html.ValidationMessageFor(model => model.Name) %>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Email) %>
                <%= Html.TextBoxFor(model => model.Email)%>                
                <br /><span class="example">myname@gmail.com</span>
                <%= Html.ValidationMessageFor(model => model.Email)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Url) %>
                <%= Html.TextBoxFor(model => model.Url)%>
                <br /><span class="example">http://www.mywebsite.com</span>
                <%= Html.ValidationMessageFor(model => model.Url)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.IsGravatarEnabled)%>
                <%= Html.CheckBoxFor(model => model.IsGravatarEnabled)%>
                <%= Html.ValidationMessageFor(model => model.IsGravatarEnabled)%>
            </p>


