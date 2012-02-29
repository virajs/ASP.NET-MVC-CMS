<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.CMS.Areas.Admin.ConfigViewModel>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>
<%@ Import Namespace="ComLib.CMS.Areas.Admin" %>
<%@ Import Namespace="ComLib.Configuration" %>
<%@ Import Namespace="ComLib.Web.Modules.OptionDefs" %>
<%@ Import Namespace="ComLib.Web.Modules.Resources" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contentItem">    
        <div class="header"><h2>Edit <%= Model.Name %> Setting(s)</h2></div>
        <span class="description"><%= Model.Description %></span><br /><br />
        <% using (Html.BeginForm("Save", "Config"))
           { %>
            
            <%= Html.Hidden("confignameInForm", Model.Name) %>
            <table >
            <% foreach (OptionDef def in Model.OptionDefs)
               {
                   Resource res = Model.Resources.ContainsKey(def.Key) ? Model.Resources[def.Key] : null;
                   string label = res == null ? def.Key : res.Name;
                   string description = res == null ? string.Empty : res.Description;
                   string example = res == null ? string.Empty : res.Example;
               %>
               <tr>
                    <td><span class="label"><%= label %></span></td>
                    <td><%= Html.Control(def.Key, def.ValType, Model.Config, def.Key, def.DisplayStyle) %><br />
                        <span class="description"><%= description %></span><br />
                        <span class="example">e.g. <%= example %></span><br /><br />
                    </td>
               </tr>
            <% } %>
            </table>
            <input type="submit" class="action" value="Save" />
        <% } %>
    
    </div>
</asp:Content>