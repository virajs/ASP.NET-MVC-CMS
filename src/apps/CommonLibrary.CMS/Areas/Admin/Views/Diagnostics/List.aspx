<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="ComLib.Diagnostics" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Diagnostics</h2>
    <div>
    
    
    <% 
       var allData = this.Model as List<KeyValuePair<string, IDictionary>>;
        
       // Each DiagnosticGroup : MachineInfo, EnvUser, Modules, etc. from ComLib.Diagnostics        
       foreach (var entry in allData)
       {
           var section = entry.Key;
           var data = entry.Value;
           IDictionary sectionData = data;
    %>
    <br /><br /><div><h2><% = section%></h2></div>
    
    <table class="systemlist">
    <%
        foreach (DictionaryEntry dataEntry in sectionData)
        {
            // Key : Value
            // OS Version Platform : Win32NT
            string key = dataEntry.Key.ToString();
            object val = dataEntry.Value;
            string displayVal = val.ToString();
            bool is2Column = true;
            if (val is LoadedModule)
            {
                LoadedModule mod = val as LoadedModule;
                displayVal = string.Format(@"{0}<br/>{1}", mod.FullPath, mod.Version);
                key = mod.Name;
                is2Column = false;
            }
            if (is2Column)
            {%>                        
            <tr><td><%= key%></td><td><%= displayVal%></td></tr>
            <% }
            else
            { %>
                <tr><td><%= key%></td><td><%= displayVal%></td></tr>
            <%
            }
                
        }
       }
    %>
    </table>    
    </div>
</asp:Content>
