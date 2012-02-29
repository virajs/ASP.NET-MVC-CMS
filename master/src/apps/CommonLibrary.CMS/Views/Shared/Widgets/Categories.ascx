<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.CMS.Models.Widgets.Categories>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Patterns" %>
<%@ Import Namespace="ComLib.Web.Modules.Categorys" %>
<%
    // The ToNodes helper method will convert the list of categories to a node<T> structure with parent / child 
    // relationships. This allows for easy building up of a tree like display for categories.
    // The Node<T> is a component in the ComLib.Patterns namespace.
    Node<Category> rootNode = Category.ToNodes(Model.Group, Model.ShowAllCategories);
%>
<a id="showall" onclick="ExpandCollapseAll(_categories<%= Model.Group %>All, true, '_sh'); return false;"  href="#">Show All</a>&nbsp;&nbsp;&nbsp;
<a id="hideall" onclick="ExpandCollapseAll(_categories<%= Model.Group %>All, false, '_sh'); return false;" href="#">Hide All</a><br /><br />
<%  string allDivs = string.Empty;        
    if (rootNode != null && rootNode.HasChildren)
    {
        // Handle sub-cateogories? 
        string jsShowCollaps = Model.ShowAllCategories ? "true" : "false";
        
        // 1. Iterate through all parent categories.
        for (int ndx = 0; ndx < rootNode.Children.Count; ndx++)
        {
            var category = rootNode.Children[ndx];
            
            // 2. Build up ids for sub-categories div and +/- (expand/collapse) link ids.
            string divId = "cat_" + category.Item.Id;
            string linkId = "cat_" + category.Item.Id + "_sh";
            allDivs += divId + ",";
            
            // 3. Build up the links "+" CategoryName
            string link =  Model.ShowAllCategories ? "<a id=\"" + linkId + "\" onclick=\"ExpandCollapse('" + divId + "', '" + linkId + "', " + jsShowCollaps + "); return false;\" href=\"#\">+ </a>" : string.Empty;
            link += string.Format("<a href=\"/{0}/category/{1}\">{2}</a><br />", Model.EntityName, category.Item.Id, category.Item.Name);
            %>
            <%= link %>
            <%  // 4. Sub-categories Applicable ?
                if (Model.ShowAllCategories && category.HasChildren)
                {
                    string subLinks = string.Empty; 
                    StringBuilder buffer = new StringBuilder();

                    // 5. Now iterate over and build up the sub-categories.
                    for (int subNdx = 0; subNdx < category.Children.Count; subNdx++)
                    {
                        var subCategory = category.Children[subNdx];
                        buffer.Append("&nbsp;&nbsp;&nbsp;<a href=\"/" + Model.EntityName + "/category/" + subCategory.Item.Id + "\">" + subCategory.Item.Name + "</a><br />");
                    }
                    subLinks = buffer.ToString();
                    
                    // 6. Wrap up the categories inside a div to expand/collapse them.
                    subLinks = "<div id=\"" + divId + "\" style=\"display:block\">" + subLinks + "</div>";
                    %>
                    <%= subLinks%>
            <% } %>
      <% }
          if (allDivs.EndsWith(","))
              allDivs = allDivs.Substring(0, allDivs.Length - 1);
    } %>  

<% if(!string.IsNullOrEmpty(Model.RequestLink)){ %>
    <br /><br /><a href="<%= Model.RequestLink %>">Suggestions?</a>
<% } %>


<script type="text/javascript">
    var _categories<%= Model.Group %>All = null;
    /// Hide all the sub-categories.
    $(document).ready(function () {
        _categories<%= Model.Group %>All = '<%= allDivs %>';
        ExpandCollapseAll(_categories<%= Model.Group %>All, false);
    });
</script>
