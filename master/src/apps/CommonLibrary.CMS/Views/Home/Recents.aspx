<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("controls/Recents", new ComLib.Web.Modules.ViewModels.RecentViewModel()
       {
           JSObjectName = "recentEvents",
           Url = "/event/recentItemsJson?",
           DivId = "recentEvents",
           IsVertical = true,
           IsRefreshable = false,
           TopLevelCssClass = "posts",
           PageSize = 10 
       }); %>
       <% Html.RenderPartial("controls/Recents", new ComLib.Web.Modules.ViewModels.RecentViewModel()
       {
           JSObjectName = "recentEvents",
           Url = "/profile/recentItemsJson?",
           DivId = "recentProfiles",
           IsVertical = true,
           IsRefreshable = false,
           TopLevelCssClass = "posts",
           PageSize = 10 
       }); %>
    <div id="recentEvents">
    </div>
    <div id="recentProfiles">
    </div>
</asp:Content>
