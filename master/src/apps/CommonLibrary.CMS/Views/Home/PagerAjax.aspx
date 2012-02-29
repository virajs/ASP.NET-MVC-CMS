<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="ComLib.Paging" %>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
     <!-- 2. Client side javascript ajax pager -->
    <% Html.RenderPartial("Controls/PagerAjax", new PagerAjaxViewModel(){ JSObjectName = "_pager", PageSize = 15, CssClassCurrent = "current",  OnPageSelectedCallBack = "Dummy_OnPageSelected" }); %><br /><br />
    
    <script type="text/javascript">
        var _dummy = null;

        // Display the data.
        $(document).ready(function () {
            _dummy = new Dummy(function (page, total) { _pager.SetPageData(page, total); });
            _pager.OnPageSelectedCallback = function (page) { _dummy.Load(page); };
            _dummy.Load(1);
        });

        function Dummy(callback) {

            this.OnLoadCompleteCallback = callback;

            // Load callback.
            this.Load = function (page) {

                document.getElementById("pagerInfo").innerHTML = page;
                if (this.OnLoadCompleteCallback)
                    this.OnLoadCompleteCallback(page, 15);
            };
        }
        
    </script>
    <div id="pagerInfo"></div>

    <%= Html.Javascript("/scripts/app/PagerAjax.js") %>
</asp:Content>
