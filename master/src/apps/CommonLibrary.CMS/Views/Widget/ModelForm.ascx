<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Widgets.WidgetInstance>" %>
<%@ Import Namespace="ComLib.Web.Lib.Extensions" %>
<%@ Import Namespace="ComLib.Web.Modules.Services" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>
<%@ Import Namespace="ComLib.Web.Modules.Themes" %>
            <table>
            <%  // Dynamic model editor.
                //
                // 1. OPTION DEFINITIONS
                //    - This uses the option definitions stored in /config/data/optiondefs.csv
                //    - The optiondefs represent what fields/properties are applicable for displaying/editing for a specific model.
                //
                // 2. RESOURCE STRINGS
                //    - This also uses the localized resource strings 
                
                var editor = new ModelService(Model, Html);
                string language = "english";
                string configSections = string.Format("Widget,W_{0}", Model.GetType().Name);

                // 1. Exclude the "DefName".
                // 2. Load all the properties in the UI, as outlined by the optiondefs.( /config/data/OptionDefs.csv )
                // 3. Override the "Zone" Display field w/ a drop-down of the available zones for the theme.
                editor.Exclude("DefName");
                editor.Load(configSections, configSections, language);                
                editor.Override("Zone", () => Html.DropDownList("Zone", Html.ToDropDownList(Model.Zone, Theme.Current.Zones.Split(','))));                
                
                foreach (var property in editor.Properties)
                { %>
                <tr>
                    <td style="vertical-align:top"><label for="<%= editor.FriendlyName(property) %>"><%= editor.FriendlyName(property)%></label></td>
                    <td><%= editor.Control(property)%><br />
                        <span class="description"><%= editor.Description(property)%></span><br />
                        <span class="example"><%= editor.Example("e.g.", property)%></span><br /><br />
                    </td>
                </tr>
             <% } %>             
             </table>