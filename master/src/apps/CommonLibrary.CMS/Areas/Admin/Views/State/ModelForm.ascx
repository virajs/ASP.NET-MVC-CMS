<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.LocationSupport.State>" %>
<%@ Import Namespace="ComLib.LocationSupport" %>


<p>
    <%= Html.ResourceFor(model => model.Name) %>
    <%= Html.TextBoxFor(model => model.Name) %>
    <%= Html.ValidationMessageFor(model => model.Name) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.CountryName) %>
    <%= Html.TextBoxFor(model => model.CountryName) %>
    <%= Html.ValidationMessageFor(model => model.CountryName) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.IsActive) %>
    <%= Html.CheckBoxFor(model => model.IsActive)%>
    <%= Html.ValidationMessageFor(model => model.IsActive)%>
</p>
<p>
    <%= Html.ResourceFor(model => model.IsAlias) %>
    <%= Html.CheckBoxFor(model => model.IsAlias)%>
    <%= Html.ValidationMessageFor(model => model.IsAlias)%>
</p>
<p>
    <%= Html.ResourceFor(model => model.Abbreviation)%>
    <%= Html.TextBoxFor(model => model.Abbreviation)%>
    <%= Html.ValidationMessageFor(model => model.Abbreviation)%>
</p>


