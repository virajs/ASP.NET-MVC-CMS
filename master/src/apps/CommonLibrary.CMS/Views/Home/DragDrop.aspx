<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<script src="/scripts/jquery-ui-1.8.6.custom.min.js" type="text/javascript"></script>
<script src="/scripts/app/widgets.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(function () {
            $("#sortable1, #sortable2").sortable({
                connectWith: '.connectedSortable'
            }).disableSelection();
            $("#gridTableSortable").sortable().disableSelection();

        });
	</script>


<div class="demo">

<div class="connectedSortable" id="gridTableSortable">
	<div style="padding-top: 10px; padding-bottom: 10px;" id="ddgridrow_1">
		<table class="systemlist"><tbody>
			<tr id="gRow_0">
				<td><input type="button" onclick="_grid.RowMenu.ShowMenu(0);" value="Menu"></td>
				<td id="gRowCol_0_0">1</td><td id="gRowCol_0_1">Ayende Rahien 1</td>
				<td id="gRowCol_0_2">bloggers</td><td id="gRowCol_0_3">http://ayende.com</td>
			</tr>
			</tbody>
		</table>
	</div>
	<div style="padding-top: 10px; padding-bottom: 10px;" id="ddgridrow_2">
		<table class="systemlist"><tbody>
			<tr id="gRow_0">
				<td><input type="button" onclick="_grid.RowMenu.ShowMenu(0);" value="Menu"></td>
				<td id="gRowCol_0_0">1</td><td id="gRowCol_0_1">Ayende Rahien 2</td>
				<td id="gRowCol_0_2">bloggers</td><td id="gRowCol_0_3">http://ayende.com</td>
			</tr>
			</tbody>
		</table>
	</div>
	<div style="padding-top: 10px; padding-bottom: 10px;" id="ddgridrow_3">
		<table class="systemlist"><tbody>
			<tr id="gRow_0">
				<td><input type="button" onclick="_grid.RowMenu.ShowMenu(0);" value="Menu"></td>
				<td id="gRowCol_0_0">1</td><td id="gRowCol_0_1">Ayende Rahien 3</td>
				<td id="gRowCol_0_2">bloggers</td><td id="gRowCol_0_3">http://ayende.com</td>
			</tr>
			</tbody>
		</table>
	</div>
</div>

<div id="sortable1" class="connectedSortable">
	<div class="ui-state-default">A 1</div>
	<div class="ui-state-default">A 2</div>
	<div class="ui-state-default">A 3</div>
	<div class="ui-state-default">A 4</div>
	<div class="ui-state-default">A 5</div>
</div>
<br /><br />
<div id="sortable2" class="connectedSortable">
    <% int count = 5;
       for (int ndx = 1; ndx <= 5; ndx++)
       { %>
	    <div class="widget">
            <div class="wheader" onclick="ShowHide('weditcontent_<%= ndx%>');" onmouseover="ToggleW('weditcontent_<%= ndx%>');" onmouseout="ToggleW('weditcontent_<%= ndx%>');">B <%= ndx%></div>
            <div id="weditcontent_<%=ndx %>" style="visibility:hidden"><a onclick="">Save</a><a onclick="">Delete</a><a onclick="">On/Off</a></div>
            <input type="hidden" id="weditcontent_<%=ndx %>_showhide" value="on" />
            <div class="wbody">Contents</div>
        </div>
    <% } %>
</div>


<table>
    <tr>
        <td>
            <div id="w_zoneleft" class="connectedSortable">
            <% 
                for (int ndx = 1; ndx <= 5; ndx++)
                {
                    string id = "a_" + ndx.ToString();
                    %>
	            <div id="widget_<%=id %>" class="widget">
                    <div class="wheader">A <%= ndx%> <a onclick="Toggle2('weditarea_<%=id %>');">edit</a></div>
                    <div id="weditarea_<%=id %>" style="display:none">
                        <a onclick="_widgetManager.Save(); return false;">Save</a>
                        <input type="button" id="weditonoff_<%=id %>" onclick="_widgetManager.Toggle('weditonoff_<%=id %>', <%=id %>);" value="Off" />
                        <a onclick="_widgetManager.Delete(<%=id %>);">Delete</a>                
                        </div>
                    <div class="wbody">Contents</div>
                </div>
            <% } %>
            </div>
        </td>
        <td style="width:50px">&nbsp;</td>
        <td>
            <div id="w_zoneright" class="connectedSortable">
            <% 
                for (int ndx = 1; ndx <= 5; ndx++)
                {
                    string id = "b_" + ndx.ToString();
                    %>
	            <div id="widget_<%=id %>" class="widget">
                    <div class="wheader">B <%= ndx%> <a onclick="_widgetManager.ToggleEditArea('weditarea_<%=id %>');">edit</a></div>
                    <div id="weditarea_<%=id %>" style="display:none">
                        <a onclick="_widgetManager.Save(); return false;">Save</a>
                        <input type="button" id="weditonoff_<%=id %>" onclick="_widgetManager.Toggle('weditonoff_<%=id %>', <%=id %>);" value="Off" />
                        <a onclick="_widgetManager.Delete(<%=id %>);">Delete</a>                
                    </div>
                    <div class="wbody">Contents</div>
                </div>
            <% } %>
            </div>
        </td>
    </tr>
</table>
    <script type="text/javascript">

        function Toggle2(id) {
            var elem = document.getElementById(id);
            var val = elem.style.display == 'none' ? "block" : "none";
            elem.style.display = val;
        }

        function ToggleW(id) {
            if(document.getElementById(id + "_showhide").value == "off")
                return;

            var elem = document.getElementById(id);
            var val = elem.style.visibility == 'visible' ? 'hidden' : 'visible';
            elem.style.visibility = val;
        }

        function ShowHide(id) {
            var elemsh = document.getElementById(id + "_showhide");
            elemsh.value = elemsh.value == "on" ? "off" : "on";
            var elem = document.getElementById(id);
            elem.style.visibility = elemsh.value == "off" ? "visible" : "hidden";
        }

        var _widgetManager = null;

        // Set up the location fields.
        $(document).ready(function () {
            //alert("iinitializing");
            _widgetManager = new WidgetManager("zoneright,zoneleft", "connectedSortable", "w_");
        });
    </script>
</div><!-- End demo -->

<div class="demo-description">

<p>
	Sort items from one list into another and vice versa, by passing a selector into
	the <code>connectWith</code> option. The simplest way to do this is to
	group all related lists with a CSS class, and then pass that class into the
	sortable function (i.e., <code>connectWith: '.myclass'</code>).
</p>

</div><!-- End demo-description -->

</asp:Content>