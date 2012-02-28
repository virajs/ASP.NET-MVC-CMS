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
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Lib.Models;


namespace ComLib.Web.Lib.Services
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }
        bool EnableNotifications { get; set; }

        BoolMessage IsValidateUser(string userName, string password);
        BoolMessageItem ValidateUser(string userName, string password, ref string roles);
        BoolMessageItem CreateUser(string userName, string password, string email, ref MembershipCreateStatus status);
        BoolMessageItem CreateUser(string userName, string password, string email, string roles, ref MembershipCreateStatus status);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        BoolMessage DeleteUser(string username);
        BoolMessage SendPassword(string userName, string email, bool useEmail);
    }
}
