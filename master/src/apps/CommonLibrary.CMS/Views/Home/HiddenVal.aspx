<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <form method="post" action="/home/hiddenval2">
    <input type="hidden" id="hdnModelName" name="hdnModelName" value="a" />
    <input type="text" id="txtModelName" value="a" />
    <input type="button" id="btnModelName" value="Set" onclick="SetModelName();" />

    <input type="submit" value="Submit" />
        <script type="text/javascript">

            function SetModelName() {
                document.getElementById('hdnModelName').value = document.getElementById('txtModelName').value;
            }


            function TestChildCallBack() {     
                alert("Child callback handled in parent window");
            }
        </script>
    </form>
</asp:Content>