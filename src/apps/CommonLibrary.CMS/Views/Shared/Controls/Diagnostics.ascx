<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="ComLib.Environments" %>
<%@ Import Namespace="ComLib.Configuration" %>
<%@ Import Namespace="ComLib.Logging" %>
<%@ Import Namespace="ComLib.Notifications" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="System.Reflection" %>


 
<%  
    // Display diagnostics based on configsetting
    // But always display if admin is logged on.
    bool display = Config.Get<bool>("Diagnostics", "DisplayInHeader");
    if (!display && Auth.IsAdmin()) display = true;    
    if (display)
    {
        string version = typeof(ModuleMap).Assembly.GetName().Version.ToString();
        string envname = Env.EnvType.ToString().ToLower();
    
    %>
        <div class="<%= envname %>">
            <span class="dfield">Env: </span><%= Env.EnvType.ToString()%> &nbsp;&nbsp;&nbsp;
            <span class="dfield">Config: </span> <%= Env.RefPath %> &nbsp;&nbsp;&nbsp;
            <span class="dfield">Database: </span><%= Config.Get<string>("Database", "server")%>,<%= Config.Get<string>("Database", "database")%>&nbsp;&nbsp;&nbsp;
            <span class="dfield">Version: </span><%= version %> &nbsp;&nbsp;&nbsp;
            <span class="dfield">LogLevel: </span><%= Logger.Default.Level.ToString()%> &nbsp;&nbsp;&nbsp;
            <span class="dfield">Emails: </span><%= Notifier.Settings.EnableNotifications%> &nbsp;&nbsp;&nbsp;
            <span class="dfield">Machine: </span><%= Environment.MachineName%> &nbsp;&nbsp;&nbsp;
            <span class="dfield">Started: </span><%= ((DateTime)this.Application["start_time"]).ToString("MM-dd : HH:mm:ss")%> &nbsp;&nbsp;&nbsp;
            <span class="dfield">User: </span><%= Auth.UserShortName%> &nbsp;&nbsp;&nbsp;        
        </div>
    <% } %>
