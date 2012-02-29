<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.CMS" %>
<%@ Import Namespace="ComLib.Authentication" %>
<%@ Import Namespace="ComLib.Web.Modules.Services" %>
<%@ Import Namespace="ComLib.Web.Modules.Settings" %>
<%@ Import Namespace="System.Linq" %>

<script type="text/javascript">

    function ShowHide(name, useNameDirectly, handleShowCollapse) {
        if (!useNameDirectly) {
            name = "dash_" + name + "_body";
        }

        // First toggle.
        $("#" + name).toggle();

        if (handleShowCollapse) {
            // Check hidden state and set either the + (expand) or -(collapse) sign
            if ($("#" + name).is(':visible')) {
                $("#" + name + "_sh").text("-");
            }
            else {
                $("#" + name + "_sh").text("+");
            }
        }
    }


    var _allNames = '<%= CMS.Dashboard.SectionNames %>,Account,Models';

    function ShowHideAll(show) {
        var names = _allNames.split(',');
        for (i = 0; i < names.length; i++) {
            var name = names[i];
            if (show) {
                $("#dash_" + name + "_body").show();
                $("#dash_" + name + "_body_sh").text("-");
            }
            else {
                $("#dash_" + name + "_body").hide();
                $("#dash_" + name + "_body_sh").text("+");
            }
        }
    }
    $(function () {
        ShowHideAll();
        ShowHide('System', false, true);
    });
</script>
<% bool isAdmin = Auth.IsAdmin();        
%>

<div id="dashboard" class="dashboard">
    <a id="showall" onclick="ShowHideAll(true); return false;"  href="#">+ Show All</a>&nbsp;&nbsp;&nbsp;
    <a id="hideall" onclick="ShowHideAll(false); return false;" href="#">- Hide All</a><br /><br />

    <% if (isAdmin) 
       {   
           // E.g. System.
           foreach (var section in CMS.Dashboard.AdminSections)
           { %>
           <div id="dash_<%= section.Name %>" class="section">
                <div id="dash_<%= section.Name %>_header" class="header">
                    <a id="dash_<%= section.Name %>_body_sh" onclick="ShowHide('<%= section.Name %>', false, true); return false;" href="#">- </a>
                    <a onclick="ShowHide('<%= section.Name %>', false, true); return false;" href="#"><%= section.Name %></a></div>
                <div id="dash_<%= section.Name %>_body" class="body"> 
                    <ul><%  foreach (var menuitem in section.Items) { %>
                            <li><a href="<%= menuitem.NavigateUrl %>"><%= menuitem.Text %></a></li> 
                         <% } %>
                    </ul>        
                </div>
            </div>
           <% }
       }
    %>
    <% if (Auth.IsAuthenticated())
       { 
            // E.g. System.
           foreach (var authSection in CMS.Dashboard.AuthenticatedSections)
           { %>
           <div id="dash_<%= authSection.Name %>" class="section">
                <div id="dash_<%= authSection.Name %>_header" class="header">
                    <a id="dash_<%= authSection.Name %>_body_sh" onclick="ShowHide('<%= authSection.Name %>', false, true); return false;" href="#">- </a>
                    <a onclick="ShowHide('<%= authSection.Name %>', false, true); return false;" href="#"><%= authSection.Name%></a></div>
                <div id="dash_<%= authSection.Name %>_body" class="body"> 
                    <ul><%  foreach (var menuitem in authSection.Items) { %>
                            <li><a href="<%= menuitem.NavigateUrl %>"><%= menuitem.Text %></a></li> 
                         <% } %>
                    </ul>        
                </div>
            </div>
           <% } %>
        <div id="dash_account" class="section">
            <div id="dash_account_header" class="header">
                 <a id="dash_Account_body_sh" onclick="ShowHide('Account', false, true); return false;" href="#">- </a>
                 <a onclick="ShowHide('Account', false, true); return false;" href="#">Account</a></div>
            <div id="dash_Account_body" class="body">
                <ul>
                    <li><a href="/profile/editprofilebyname/<%= Auth.UserShortName %>">Profile</a></li>            
                    <li><a href="/account/changepassword">Password</a></li>                    
                </ul>             
            </div>
        </div> 
        <div id="dash_models" class="section">
            <div id="dash_models_header" class="header">
                <a id="dash_Models_body_sh" onclick="ShowHide('Models', false, true); return false;" href="#">- </a>
                <a onclick="ShowHide('Models', false, true); return false;" href="#">Manage</a></div><br />
            <div id="dash_Models_body" class="body">
            <%
                CMS.Dashboard.Models.ForEach((modelEntry) =>
                {
                    if(!modelEntry.IsSystemModel)
                    {
                        string modelName = modelEntry.ControllerName;
                        string alias = modelEntry.DisplayName;
                        if(CMS.Dashboard.CanCreateModel(modelEntry))
                        {  %>        
                        <div class="bodyheader"><a onclick="ShowHide('dash_<%= alias %>_area', true, false); return false;" href="#"><%= alias%></a></div>
                        <div id="dash_<%= alias %>_area" class="body">
                            <ul>        
                                <li><a href="/<%= modelName %>/create">add</a></li>
                                <li><a href="/<%= modelName %>/manage/">manage</a></li>
                            </ul>
                        </div>
                    <% }   
                    }
                     
                }); %>           
                </div>
            </div>
        </div>
    <% } %>