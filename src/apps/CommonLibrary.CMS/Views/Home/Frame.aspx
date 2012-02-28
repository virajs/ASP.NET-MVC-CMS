<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>
<%@ Import Namespace="ComLib.Web.Lib.Extensions" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

<% var model = MediaFolder.GetAll()[0]; %>
<% Html.RenderMediaUpload(model, 1, false, true, "no", javascriptCallbackOnLoadComplete: "TestChildCallBack"); %><br /><br />

    <script type="text/javascript">
        function TestChildCallBack() {            
            alert("Child callback handled in parent window");
        }
    </script>
</asp:Content>