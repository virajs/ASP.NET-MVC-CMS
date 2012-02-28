<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<ComLib.Web.Lib.Models.ChangePasswordModel>" %>


<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Change Password</h2>
    <p>
        Use the form below to change your password. 
    </p>
    <p>
        New passwords are required to be a minimum of <%= Html.Encode(ViewData["PasswordLength"]) %> characters in length.
    </p>
    <%= Html.ValidationSummary("Password change was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Account Information</legend>
                <p>
                    <%= Html.ResourceFor(m => m.OldPassword) %>
                    <%= Html.Password("OldPassword") %>
                    <%= Html.ValidationMessageFor(m => m.OldPassword) %>
                </p>
                <p>
                    <%= Html.ResourceFor(m => m.NewPassword) %>
                    <%= Html.Password("NewPassword") %>
                    <%= Html.ValidationMessageFor(m => m.NewPassword) %>
                </p>
                <p>
                    <%= Html.ResourceFor(m => m.ConfirmPassword) %>
                    <%= Html.Password("ConfirmPassword") %>
                    <%= Html.ValidationMessageFor(m => m.ConfirmPassword) %>
                </p>
                <p>
                    <input type="submit" class="action" value="Change Password" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
