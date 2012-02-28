<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Widgets.WidgetInstance>" %>

<fieldset>
<p>
    <%= Html.ResourceFor(model => model.DefName) %>
    <%= Html.Encode(Model.DefName) %>
</p>        
<p>
    <%= Html.ResourceFor(model => model.Header) %>
    <%= Html.Encode(Model.Header) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.Zone) %>
    <%= Html.Encode(Model.Zone) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.Group) %>
    <%= Html.Encode(Model.Group) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.Roles) %>
    <%= Html.Encode(Model.Roles) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.Args) %>
    <%= Html.Encode(Model.Args) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.SortIndex) %>
    <%= Html.Encode(Model.SortIndex) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.MaxRecords) %>
    <%= Html.Encode(Model.MaxRecords) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.IsActive) %>
    <%= Html.Encode(Model.IsActive) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.IsPublic) %>
    <%= Html.Encode(Model.IsPublic) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.IsHighlighted) %>
    <%= Html.Encode(Model.IsHighlighted) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.IsRolesBased) %>
    <%= Html.Encode(Model.IsRolesBased) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefModel) %>
    <%= Html.Encode(Model.RefModel) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefName) %>
    <%= Html.Encode(Model.RefName) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefGroup) %>
    <%= Html.Encode(Model.RefGroup) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefEmail) %>
    <%= Html.Encode(Model.RefEmail) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefUrl) %>
    <%= Html.Encode(Model.RefUrl) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefSource) %>
    <%= Html.Encode(Model.RefSource) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefKey) %>
    <%= Html.Encode(Model.RefKey) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefContent) %>
    <%= Html.Encode(Model.RefContent) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefId1) %>
    <%= Html.Encode(Model.RefId1) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefId2) %>
    <%= Html.Encode(Model.RefId2) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefDate1) %>
    <%= Html.Encode(String.Format("{0:g}", Model.RefDate1)) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefDate2) %>
    <%= Html.Encode(String.Format("{0:g}", Model.RefDate2)) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefFlag1) %>
    <%= Html.Encode(Model.RefFlag1) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefFlag2) %>
    <%= Html.Encode(Model.RefFlag2) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefInt1) %>
    <%= Html.Encode(Model.RefInt1) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.RefInt2) %>
    <%= Html.Encode(Model.RefInt2) %>
</p>
<p>
    <%= Html.ResourceFor(model => model.WidgetLength) %>
    <%= Html.Encode(Model.WidgetLength) %>
</p>
</fieldset>