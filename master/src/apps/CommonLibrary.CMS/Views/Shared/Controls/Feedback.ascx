<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Feedbacks.Feedback>" %>

            <p>
                <%= Html.ResourceFor(model => model.Name) %>
                <%= Html.TextBoxFor(model => model.Name)%>
                <%= Html.ValidationMessageFor(model => model.Name)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Email) %>
                <%= Html.TextBoxFor(model => model.Email)%>
                <br /><span class="example">e.g. myname@gmail.com</span>
                <%= Html.ValidationMessageFor(model => model.Email)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Url) %>
                <%= Html.TextBoxFor(model => model.Url)%>
                <br /><span class="example">e.g. http://www.google.com</span>
                <%= Html.ValidationMessageFor(model => model.Url)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Title, "Subject") %>
                <%= Html.TextBoxFor(model => model.Title)%>
                <%= Html.ValidationMessageFor(model => model.Title)%>
            </p>
            <p>
                <%= Html.ResourceFor(model => model.Content) %>
                <%= Html.TextAreaFor(model => model.Content, new { @class = "text-area" })%>
                <%= Html.ValidationMessageFor(model => model.Content) %>
            </p>
            

