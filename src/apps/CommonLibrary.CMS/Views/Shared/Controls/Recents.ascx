<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.ViewModels.RecentViewModel>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Extensions" %>


<%= Html.Javascript("/scripts/app/recents.js") %>
<script type="text/javascript">
    var <%=Model.JSObjectName %> = null;

    /// Loads the comments on page load.
    $(document).ready(function() {   
        <%=Model.JSObjectName %> = new RecentsControl("<%=Model.Url %>", <%=Model.IsVertical.ToString().ToLower() %>, <%=Model.IsRefreshable.ToString().ToLower() %>, <%=Model.RefreshRate %>, <%=Model.PageSize %>, "<%=Model.DivId %>", "<%=Model.TopLevelCssClass %>", null);
        <%=Model.JSObjectName %>.Load();
    });
</script>