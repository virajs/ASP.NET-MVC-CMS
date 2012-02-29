<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="ComLib.CaptchaSupport" %>

<% string captchaText = Captcha.GetRandomTextEncoded();
   string captchaTextUrlEncoded = HttpUtility.UrlEncode(captchaText); 
%>
<%= Html.Hidden("CaptchaGeneratedText", captchaText) %>

<div class="Captcha">
    <table>
        <tr><td>Verification Code:</td></tr>
        <tr><td><img id="imgCaptcha" src="/captcha.ashx?CaptchaText=<%=captchaTextUrlEncoded %>" /></td></tr>
        <tr><td><%= Html.TextBox("CaptchaUserInput") %></td></tr>
        <tr><td>Please enter the verification code above.</td></tr>
    </table>
</div>