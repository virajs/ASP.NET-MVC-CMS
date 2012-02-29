<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
    <br />
    <input />    <h3><a href="http://commonlibrarynet.codeplex.com" title="ASP.NET MVC Website">CommonLibrary.NET</a></h3>
    Available at : http://commonlibrarynet.codeplex.com.<br />
    The CommonLibrary.NET is an open-source library of re-usable code/component ranging from ActiveRecord, DomainModel, <br />
    Argument Parsing, CSV Parsing, Holiday Calendar support, Database Helpers, Email Notifications and so much more. 
    <br /><br />
    
    <h3><a href="http://commonlibrarynetmvc.codeplex.com" title="ASP.NET MVC Website">ASP.NET MVC 2 App Template Features</a>.</h3>
    Availale at http://commonlibrarynetmvc.codeplex.com.    
    
    <h3>Features</h3>
    <ul>
        <li>Uses ASP.NET MVC 2</li>
        <li>Active Record based entities</li>
        <li>Simple, Lightweight, Small codebase</li>
        <li>Plug in you're own repository ( NHibernate, EntityFramework etc )</li>
        <li>CodeGenerate your entities in seconds</li>
        <li>Powered by CommonLibrary.NET Framework</li>
        <li>Support for Widgets</li>
        <li>Extremely light-weight</li>
        <li>This home page is less than 350 lines of HTML !</li>
        <li>A lot of code coverage</li>
        <li>Included entities (Blog, Links, Event, BlogRoll, Tags, Pages, Parts, Profiles )</li>
        <li>Very easy to customize!</li>        
        <li>Admin/Dashboard included</li>
        <li>Paging supported on all entities</li>
        <li>Get up and running in seconds and start testing via the builtin In-Memory repositories.</li>
        <li>Repositories for Sql Server 2008 provided. Future support for MySql</li>
        <li>System features ( Cache, Logs, Users, Diagnostics viewer/manager )</li>        
        <li>Comments</li>
        <li>Rss generation of posts and events</li>
        <li>Abuse/Flagging support</li>        
        <li>Widget Runtime management</li>
    </ul>
    
    <br /><br />
    <h3>Limitations</h3>
    <ul>
        <li>No BlogML</li>
        <li>No WebLog API</li>
        <li>No trackbacks/pingbacks</li>
        <li>No HasOne/HasMany relational support in ActiveRecord yet</li>
        <li>No Akismet</li>
        <li>No Localization support</li>
    </ul>
    <br /><br />
</asp:Content>
