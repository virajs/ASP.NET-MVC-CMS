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
using ComLib.Web.Modules;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Lib.Models;


namespace ComLib.Web.Lib.Models
{

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.
    public interface IMembershipService
    {
        int MinPasswordLength { get; }
        BoolMessage IsValidateUser(string userName, string password);
        BoolMessageItem ValidateUser(string userName, string password, ref string roles);
        BoolMessageItem CreateUser(string userName, string password, string email, ref MembershipCreateStatus status);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        BoolMessage SendPassword(string userName, string email, bool useEmail);
    }



    /// <summary>
    /// Forms Authentication Interface.
    /// </summary>
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignIn(string userName, string roles, bool createPersistentCookie);
        void SignOut();
    }



    /// <summary>
    /// Account membership service
    /// </summary>
    public class AccountMembershipService : IMembershipService
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public AccountMembershipService()
        {
        }


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
            BoolMessageItem<User> result = User.Create(userName, email, password, password, ref status);
            return result;
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
            IList<User> users = User.Find(Criteria<User>.New().Where(u => u.EmailLowered).Is(email.ToLower()));
            User user = null;
            if (users != null && users.Count > 0) user = users[0];
            if (user == null) return new BoolMessage(false, "Email address not found.");

            Notifications.Notifier.RemindUserPassword(user.UserName, "Password reminder", user.UserName, user.UserName, User.Decrypt(user.Password));
            return new BoolMessage(true, string.Empty);
        }
    }



    /// <summary>
    /// Forms Authentication Implementation
    /// http://msdn.microsoft.com/en-us/library/aa302397.aspx
    /// </summary>
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        /// <summary>
        /// Sign in issuing the Forms Authentication Ticket.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <param name="createPersistentCookie"></param>
        public void SignIn(string userName, bool createPersistentCookie)
        {
            SignIn(userName, string.Empty, createPersistentCookie);
        }


        /// <summary>
        /// Sign in issuing the Forms Authentication Ticket.
        /// http://msdn.microsoft.com/en-us/library/aa302397.aspx
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <param name="createPersistentCookie"></param>
        public void SignIn(string userName, string roles, bool createPersistentCookie)
        {
            SecurityHelper.SetCookieContainingUserData(userName, roles, 30);     
        }


        /// <summary>
        /// Sign out.
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();            
        }
    }



    internal static class ValidationUtil
    {
        private const string _stringRequiredErrorMessage = "Value cannot be null or empty.";

        public static void ValidateRequiredStringValue(string value, string parameterName)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentException(_stringRequiredErrorMessage, parameterName);
            }
        }
    }
    #endregion

    #region Models
    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Current password")]
        public string OldPassword { get; set; }

        [Required, ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm new password")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public class RegisterModel
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        public string Email { get; set; }

        [Required, ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }
    }
    #endregion

    #region Validation
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty
        {
            get;
            private set;
        }

        public string OriginalProperty
        {
            get;
            private set;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";

        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            if (String.IsNullOrEmpty(valueAsString))
            {
                return true;
            }

            return (valueAsString.Length >= _minCharacters);
        }
    }
    #endregion

}
