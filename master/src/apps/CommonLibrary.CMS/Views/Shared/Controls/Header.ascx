<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="ComLib.Configuration" %>
<%@ Import Namespace="ComLib.Web.Modules.Settings" %>

            <div id="logoarea">
                <% if (SiteSettings.Instance.Site.IsLogoEnabled)
                { %> <img src="<%=SiteSettings.Instance.Site.LogoPath %>" id="imgLogo" alt="company logo " />        
                <% } %>
                &nbsp;
            </div>
            <div id="heading">
                <table style="margin-left: auto; margin-right: auto;">
                    <tr>
                        <td><h1><%= SiteSettings.Instance.Site.Title %></h1></td>
                        <td>
                            <%  bool displayType = Config.GetDefault<bool>("App", "displayversiontype", false);
                                string versiontype = Config.GetDefault<string>("App", "versiontype", string.Empty); 
                                if ( displayType ) {    
                            %>
                                <span class="versiontype">&nbsp;&nbsp;<%= versiontype %></span>
                            <% } %>
                        </td>
                    </tr>
                </table>                
                <h3><%= SiteSettings.Instance.Site.Description %></h3>
            </div>
            <div id="login"><% Html.RenderPartial("~/views/shared/Controls/LogOnUserControl.ascx"); %></div>   