<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Lib.Models.EntityDetailsViewModel>" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>

hahahahaha!!! done.
<% Html.RenderPartial(Model.ControlPath, Model.Entity); %>
