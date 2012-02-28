using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using ComLib;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Account;
using ComLib.Web.Modules.Profiles;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Services;


namespace ComLib.Web.Modules.Services
{
    /// <summary>
    /// Account membership service
    /// </summary>
    public class MembershipService : IMembershipService
    {

        /// <summary>
        /// Whether or not notifications are enabled;
        /// </summary>
        public bool EnableNotifications { get; set; }


        /// <summary>
        /// Minimum Password Length;
        /// </summary>
        public int MinPasswordLength { get; set; }


        /// <summary>
        /// Validate the credentials.
        /// </summary>
        /// <param name="userName">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns></returns>
        public BoolMessage IsValidateUser(string userName, string password)
        {
            return User.VerifyUser(userName, password);
        }


        /// <summary>
        /// Validate the credentials and return a boolmessage if the validation
        /// was successful and also provides the roles of the user.
        /// </summary>
        /// <param name="userName">Username.</param>
        /// <param name="password">Password.</param>
        /// <returns></returns>
        public BoolMessageItem ValidateUser(string userName, string password, ref string roles)
        {
            BoolMessageItem<User> result = User.VerifyUser(userName, password);
            if (result.Success) roles = result.Item.Roles;

            return result;
        }


        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="email">Email Address.</param>
        /// <returns></returns>
        public BoolMessageItem CreateUser(string userName, string password, string email, ref MembershipCreateStatus status)
        {
            return CreateUser(userName, password, email, string.Empty, ref status);
        }


        /// <summary>
        /// Create the user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="roles"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public BoolMessageItem CreateUser(string userName, string password, string email, string roles, ref MembershipCreateStatus status)
        {
            // Create the User and Profile.
            BoolMessageItem<User> result = User.Create(userName, email, password, roles, ref status);
            BoolMessageItem<Profile> finalResult = new BoolMessageItem<Profile>(null, result.Success, result.Message);
            if (result.Success)
            {
                finalResult = Profile.Create(result.Item);

                // Rollback the user
                if (!finalResult.Success)
                {
                    Try.CatchLog("Unable to delete user : " + userName + " after error while creating profile.", () => User.Delete(result.Item.Id));
                    status = MembershipCreateStatus.UserRejected;
                }
                else // Send user welcome email.
                {
                    Profile profile = finalResult.Item;
                    string sitename = Configuration.Config.Get<string>("Notifications", "WebSite.Name");
                    string firstname = string.IsNullOrEmpty(profile.FirstName) ? profile.UserName : profile.FirstName;
                    if (EnableNotifications)
                    {
                        Notifications.Notifier.WelcomeNewUser(result.Item.Email, "Welcome to " + sitename, firstname, profile.UserName, password);
                    }
                }
            }
            return finalResult;
        }


        /// <summary>
        /// Delete the user with the associated username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public BoolMessage DeleteUser(string username)
        {
            if(string.IsNullOrEmpty(username)) return new BoolMessage(false, "User name not provided.");

            // Check user removing account is the same user or admin.
            if (!Authentication.Auth.IsUserOrAdmin(username))
                return new BoolMessage(false, "Not authorized to remove account.");

            // Can not delete admin account.
            if (Authentication.Auth.IsAdmin() && (string.Compare(Authentication.Auth.UserShortName, username, true) == 0))
                return new BoolMessage(false, "Can not delete admin account.");

            User user = User.Repository.First(Query<User>.New().Where(u => u.UserNameLowered).Is(username.ToLower()));
            if(user == null) return new BoolMessage(false, "User : " + username + " not found.");

            Profile profile = Profile.Repository.First(Query<Profile>.New().Where( p => p.UserId).Is(user.Id));
            if( profile == null) return new BoolMessage(false, "Profile does not exist for user : " + username);
                
            profile.Delete();
            if(profile.Errors.HasAny)
                return new BoolMessage(false, "Error removing profile : " + profile.Errors.Message());

            user.Delete();
            if( user.Errors.HasAny)
                return new BoolMessage(false, "Error removing user: " + username);

            return BoolMessage.True;
        }


        /// <summary>
        /// Change the password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {            
            return User.ChangePassword(userName, oldPassword, newPassword).Success;
        }


        /// <summary>
        /// Get the user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="useEmail"></param>
        /// <returns></returns>
        public BoolMessage SendPassword(string userName, string email, bool useEmail)
        {
            IList<User> users = User.Find(Query<User>.New().Where(u => u.EmailLowered).Is(email.ToLower()));
            User user = null;
            if (users != null && users.Count > 0) user = users[0];
            if (user == null) return new BoolMessage(false, "Email address not found.");

            Notifications.Notifier.RemindUserPassword(user.EmailLowered, "Password reminder", user.UserName, user.UserName, User.Decrypt(user.Password));
            return new BoolMessage(true, string.Empty);
        }
    }
}
