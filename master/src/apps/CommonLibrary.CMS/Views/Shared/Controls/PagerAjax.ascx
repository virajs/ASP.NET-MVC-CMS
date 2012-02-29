<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Paging.PagerAjaxViewModel>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Paging" %>

 <script type="text/javascript">

     var <%=Model.JSObjectName %> = null;

     /// Loads the comments on page load.
     $(document).ready(function () {
         <%=Model.JSObjectName %> = new PagerAjax("<%=Model.JSObjectName %>", 1, 7, 1, "pagerview", "<%=Model.OnPageSelectedCallBack %>", "", "<%=Model.CssClassCurrent %>", <%=Model.PageSize %>);
     });
</script>

<div id="pagerview" class="pager"></div>

