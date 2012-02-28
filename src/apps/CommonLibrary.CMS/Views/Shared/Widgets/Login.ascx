<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.CMS.Models.Widgets.Login>" %>
<%@ Import Namespace="ComLib.Web.Modules.Links" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>
<%@ Import Namespace="ComLib.Data" %>


<p>
    User:<br />
    <input type="text" id="w_user" />
</p>
<p>
    Password:<br />
    <input type="password" id="w_password" />
</p>
<p>
    <%= Html.CheckBox("rememberMe") %> <label class="inline" for="rememberMe">Remember me?</label>
    <%= Html.ActionLink("Forgot Password", "ForgotPassword", "Account") %>
</p>
<p>
    <input type="button" class="action" value="Log On" onclick="WDoLogin();" />
</p>

<script type="text/javascript">
    function WDoLogin() {
        var user = document.getElementById("w_user").value;
        var password = document.getElementById("w_password").value;
        var errors = [];
        if (user == null || user == "" || user == undefined)
            errors.push("User name is required");
        if (password == null || password == "" || password == undefined)
            errors.push("Password is required");

        var result = true;
        var resultMsg = "";
        var url = "<%= Model.LoginUrl %>" + "user=" + escape(user) + "&password=" + escape(password);

        if (result) {
            //$("#widget_<%= Model.Id %>").toggle();
            $("#widget_<%= Model.Id %>").hide('blind', null, "slow", null);
        }
    }
</script>