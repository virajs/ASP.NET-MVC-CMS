using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.Text;
using System.IO;

using ComLib.Extensions;
using ComLib.Web.Lib.Core;


namespace ComLib.Web.Lib.Extensions
{
    /// <summary>
    /// Class to provided localized resource strings for labels.
    /// This is currently in place but using default data for now.
    /// </summary>
    public static class HtmlHelperResourceExtensions
    {
        #region Resource/Localization
        /// <summary>
        /// Get the localized resource string for the supplied expression.
        /// e.g. "Post", "Create" => Create a new BlogPost
        /// </summary>
        /// <param name="htmlhelper">Class used for extensions methods</param>
        /// <param name="expression">The expression associated w/ the resource</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceFor<TModel>(this HtmlHelper<TModel> htmlhelper, Expression<Func<TModel, object>> expression)
        {
            return ResourceFor<TModel>(htmlhelper, expression, null, null);
        }


        /// <summary>
        /// Get the localized resource string for the supplied expression.
        /// e.g. "Post", "Create" => Create a new BlogPost
        /// </summary>
        /// <param name="htmlhelper">Class used for extensions methods</param>
        /// <param name="expression">The expression associated w/ the resource</param>
        /// <param name="defaultValue">The default value to use if the resource key is not there.</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceFor<TModel>(this HtmlHelper<TModel> htmlhelper, Expression<Func<TModel, object>> expression, string defaultValue)
        {
            return ResourceFor<TModel>(htmlhelper, expression, defaultValue, null);
        }


        /// <summary>
        /// Get the localized resource string for the supplied expression.
        /// e.g. "Post", "Create" => Create a new BlogPost
        /// </summary>
        /// <param name="htmlhelper">Class used for extensions methods</param>
        /// <param name="expression">The expression associated w/ the resource</param>
        /// <param name="defaultValue">The default value to use if the resource key is not there.</param>
        /// <param name="args">The args to use on the resource string.</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceFor<TModel>(this HtmlHelper<TModel> htmlhelper, Expression<Func<TModel, object>> expression, string defaultValue, params object[] args)
        {
            string section = typeof(TModel).Name;
            string key = ComLib.ExpressionHelper.GetPropertyName<TModel>(expression);
            MvcHtmlString text = !string.IsNullOrEmpty(defaultValue) ? htmlhelper.Label(defaultValue) : htmlhelper.Label(key);
            return text;
        }


        /// <summary>
        /// Get the localized resource string for the supplied expression.
        /// e.g. "Post", "Create" => Create a new BlogPost
        /// </summary>
        /// <param name="htmlhelper">Class used for extensions methods</param>
        /// <param name="key">The key associated w/ the resource</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceFor<TModel>(this HtmlHelper<TModel> htmlhelper, string key)
        {
            return ResourceFor<TModel>(htmlhelper, key, null, null);
        }


        /// <summary>
        /// Get the localized resource string for the supplied expression.
        /// e.g. "Post", "Create" => Create a new BlogPost
        /// </summary>
        /// <param name="htmlhelper">Class used for extensions methods</param>
        /// <param name="key">The key associated w/ the resource</param>
        /// <param name="defaultvalue">The default value to use if the resource string is not available.</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceFor<TModel>(this HtmlHelper<TModel> htmlhelper, string key, string defaultvalue)
        {
            return ResourceFor<TModel>(htmlhelper, key, defaultvalue, null);
        }


        /// <summary>
        /// Get the localized resource string for the supplied expression.
        /// e.g. "Post", "Create" => Create a new BlogPost
        /// </summary>
        /// <param name="htmlhelper">Class used for extensions methods</param>
        /// <param name="key">The key associated w/ the resource</param>
        /// <param name="defaultvalue">The default value to use if the resource string is not available.</param>
        /// <param name="args">The args to use on the resource string.</param>
        /// <returns></returns>
        public static MvcHtmlString ResourceFor<TModel>(this HtmlHelper<TModel> htmlhelper, string key, string defaultvalue, params object[] args)
        {
            string section = typeof(TModel).Name;
            MvcHtmlString text = htmlhelper.Label(key);
            return text;
        }        


        /// <summary>
        /// Get the localized resource string for the supplied section/key combination
        /// e.g. "Post", "Create" => Create a new BlogPost
        /// </summary>
        /// <param name="htmlhelper">Class used for extensions methods</param>
        /// <param name="section">The section associated w/ the resource</param>
        /// <param name="key">The key associated w/ the resource.</param>
        /// <returns></returns>
        public static MvcHtmlString Resource(this HtmlHelper htmlhelper, string section, string key)
        {
            return Resource(htmlhelper, section, key, null, null);
        }


        /// <summary>
        /// Get the localized resource string for the supplied section/key combination
        /// e.g. "Post", "Create" => Create a new BlogPost
        /// </summary>
        /// <param name="htmlhelper">Class used for extensions methods</param>
        /// <param name="section">The section associated w/ the resource</param>
        /// <param name="key">The key associated w/ the resource.</param>
        /// <param name="defaultvalue">The default value to use if the section/key is not there.</param>
        /// <returns></returns>
        public static MvcHtmlString Resource(this HtmlHelper htmlhelper, string section, string key, string defaultvalue)
        {
            return Resource(htmlhelper, section, key, defaultvalue, null);
        }


        /// <summary>
        /// Get the localized resource string for the supplied section/key combination
        /// e.g. "Post", "Create" => Create a new BlogPost
        /// </summary>
        /// <param name="htmlhelper">Class used for extensions methods</param>
        /// <param name="section">The section associated w/ the resource</param>
        /// <param name="key">The key associated w/ the resource.</param>
        /// <param name="defaultvalue">The default value to use if the section/key is not there.</param>
        /// <param name="args">The args to supply to the string.format() method to use the resource string.</param>
        /// <returns></returns>
        public static MvcHtmlString Resource(this HtmlHelper htmlhelper, string section, string key, string defaultvalue, params object[] args)
        {
            if (!string.IsNullOrEmpty(defaultvalue))
                return htmlhelper.Label(defaultvalue);

            return htmlhelper.Label(key);
        }
        #endregion


        #region JS, CSS
        /// <summary>
        /// CSSs the specified htmlhelper.
        /// </summary>
        /// <param name="htmlhelper">The htmlhelper.</param>
        /// <param name="csspath">The csspath.</param>
        /// <returns></returns>
        public static MvcHtmlString Css(this HtmlHelper htmlhelper, string csspath)
        {
            string format = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
            format = string.Format(format, csspath);
            return MvcHtmlString.Create(format);
        }


        /// <summary>
        /// Creates a script tag for javascript file as as string using the path supplied.
        /// </summary>
        /// <param name="htmlhelper">The htmlhelper.</param>
        /// <param name="csspath">The javascript file path.</param>
        /// <returns></returns>
        public static MvcHtmlString Javascript(this HtmlHelper htmlhelper, string path)
        {
            //<script src="/scripts/jquery-1.3.2.js" type="text/javascript"></script> 
            string format = "<script src=\"{0}\" type=\"text/javascript\"></script>";
            format = string.Format(format, path);
            return MvcHtmlString.Create(format);
        }
        #endregion


        #region UI
        /// <summary>
        /// Generates an Action Link using the image path.
        /// </summary>
        /// <param name="htmlhelper"></param>
        /// <param name="actionName">Action to run on the controller.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">Values for routes</param>
        /// <param name="htmlAttributes">The html attributes</param>
        /// <param name="imagePath">Path to the image</param>
        /// <param name="imageAltText">Alt text for the image.</param>
        /// <returns></returns>
        public static MvcHtmlString ActionLinkImage(this HtmlHelper htmlhelper, string actionName, string controllerName, object routeValues, object htmlAttributes, string imagePath, string imageAltText)
        {
            MvcHtmlString anchor = htmlhelper.ActionLink("REPLACE_ME", actionName, controllerName, routeValues, htmlAttributes);

            ToDo.WorkAround(ToDo.Priority.Low, "Kishore", "ActionLink creates a closed anchor, this method creates an open anchor to put in images", () =>
            {
                var openAnchor = anchor.ToString();
                var imgtext = string.Format("<img src=\"{0}\" alt=\"{1}\"/>", imagePath, imageAltText);
                openAnchor = openAnchor.Replace("REPLACE_ME", imgtext);
                anchor = MvcHtmlString.Create(openAnchor);
            });
            return anchor;
        }


        /// <summary>
        /// Controls the specified htmlhelper.
        /// </summary>
        /// <param name="htmlhelper">The htmlhelper.</param>
        /// <param name="id">The id.</param>
        /// <param name="dataTypeAsString">The data type as string.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static MvcHtmlString Control(this HtmlHelper htmlhelper, string id, string dataTypeAsString, string value, string cssClass)
        {
            if (string.IsNullOrEmpty(dataTypeAsString)
                || string.Compare("double", dataTypeAsString, true) == 0
                || string.Compare("int", dataTypeAsString, true) == 0
                || string.Compare("string", dataTypeAsString, true) == 0)
            {
                return htmlhelper.TextBox(id, value, new { @class = cssClass });
            }
            if (string.Compare("bool", dataTypeAsString, true) == 0)
            {
                bool isChecked = StringExtensions.ToBool(value);
                return htmlhelper.CheckBox(id, isChecked, new { @class = cssClass });
            }
            if (string.Compare("stringclob", dataTypeAsString, true) == 0)
            {
                return htmlhelper.TextArea(id, value, new { @class = cssClass });
            }
            return htmlhelper.TextBox(id, value, new { @class = cssClass });
        }


        /// <summary>
        /// Controls the specified htmlhelper.
        /// </summary>
        /// <param name="htmlhelper">The htmlhelper.</param>
        /// <param name="id">The id.</param>
        /// <param name="dataTypeAsString">The data type as string.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static MvcHtmlString Control(this HtmlHelper htmlhelper, string id, string dataTypeAsString, object obj, string propertyName, string cssClass)
        {
            object val = obj == null ? string.Empty : Reflection.ReflectionUtils.GetPropertyValue(obj, propertyName);
            string value = string.Empty;
            if (val != null) value = val.ToString();
            return Control(htmlhelper, id, dataTypeAsString, value, cssClass);
        }


        /// <summary>
        /// Convert the list of strings to a selectListItems.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IList<SelectListItem> ToDropDownList(this HtmlHelper helper, string selectedValue, string[] items)
        {
            IList<SelectListItem> selections = new List<SelectListItem>();
            if (items != null)
            {
                foreach (string item in items)
                {
                    bool selected = string.Compare(item, selectedValue) == 0;
                    selections.Add(new SelectListItem() { Selected = selected, Text = item, Value = item });
                }
            }
            return selections;
        }


        /// <summary>
        /// Convert the list of strings to a selectListItems.
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IList<SelectListItem> ToDropDownList(this HtmlHelper helper, string selectedValue, IList<string> items)
        {
            IList<SelectListItem> selections = new List<SelectListItem>();
            foreach (string item in items)
            {
                bool selected = string.Compare(item, selectedValue) == 0;
                selections.Add(new SelectListItem() { Selected = selected, Text = item, Value = item });
            }
            return selections;
        }


        /// <summary>
        /// Returns a unordered list of messages to display, these are obtained from calls to "FlashMessages" in CommonController.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString MessageSummary(this HtmlHelper helper)
        {
            IMessages messages = helper.ViewContext.HttpContext.Items[ViewDataConstants.FlashMessages] as IMessages;
            if (messages == null)
                return MvcHtmlString.Empty;

            StringBuilder buffer = new StringBuilder();
            buffer.Append("<ul>");
            foreach (string message in messages.MessageList)
                buffer.Append("<li>" + helper.Encode(message) + "</li>");

            buffer.Append("</ul>");
            return MvcHtmlString.Create(buffer.ToString());
        }

        
        /// <summary>
        /// Gets the heading for a model's index or manage page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelName"></param>
        /// <param name="htmlhelper"></param>
        /// <returns></returns>
        public static string HeadingForCreateEdit(this HtmlHelper html, string modelName, bool isCreate)
        {
            var config = HttpContext.Current.Application.Get("model.settings") as IDictionary;
            var actionText = isCreate ? "Create new " : "Edit ";
            var displayName = config.GetOrDefault<string>(modelName, "DisplayName", modelName);
            var defaultHeading = actionText + displayName;
            var heading = config.GetOrDefault<string>(modelName, "View.HeadingForCreate", defaultHeading);
            return "<div class=\"header\"><h2>" + actionText + displayName + "</h2></div><br />";
        }


        /// <summary>
        /// Gets the heading for a model's index or manage page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelName"></param>
        /// <param name="htmlhelper"></param>
        /// <returns></returns>
        public static string HeadingForIndex(this HtmlHelper html, string modelName)
        {
            var config = HttpContext.Current.Application.Get("model.settings") as IDictionary;
            var displayName = config.GetOrDefault<string>(modelName, "DisplayName", modelName);
            var defaultHeading = displayName + "(s)";
            var heading = config.GetOrDefault<string>(modelName, "View.HeadingForIndex", defaultHeading);
            return "<div class=\"header\"><h2>" + heading + "</h2></div><br />";
        }


        /// <summary>
        /// Gets the heading for a model's index or manage page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelName"></param>
        /// <param name="htmlhelper"></param>
        /// <returns></returns>
        public static string HeadingForManage(this HtmlHelper html, string modelName)
        {
            var config = HttpContext.Current.Application.Get("model.settings") as IDictionary;
            var actionText = "Manage ";
            var displayName = config.GetOrDefault<string>(modelName, "DisplayName", modelName);
            var defaultHeading = actionText + displayName + "(s)";
            var heading = config.GetOrDefault<string>(modelName, "View.HeadingForManage", defaultHeading);
            return "<div class=\"header\"><h2>" + heading + "</h2></div><br />";
        }
        

        ///<summary>Displays a textbox with a calendar picker.</summary>
        ///    $( "#datepicker" ).datepicker({
        ///        showOn: "button",
        ///        buttonImage: "images/calendar.gif",
        ///        buttonImageOnly: true,
        ///        showButtonPanel: true
        ///    });
        ///});
        public static string CalendarTextBox(this HtmlHelper helper, string id, string defaultValue)
        {
            var imagepath = "/content/images/generic/calendar.gif";
            var template = "<script type=\"text/javascript\">$(function() { $(\"#{0}\").datepicker({ showOn: \"button\", buttonImage: \"{1}\", buttonImageOnly: true, showButtonPanel: true }); });</script>";
            template = template.Replace("{0}", id);
            template = template.Replace("{1}", imagepath);

            var control = "<input type=\"text\" id=\"" + id + "\" name=\"" + id + "\" value=\"{0}\" />";
            control = control.Replace("{0}", defaultValue);
            return control + template;
        }
        #endregion


        #region RenderPartials
        /// <summary>
        /// Renders the pager.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="modelName"></param>
        public static string RenderPartialAsString(this HtmlHelper htmlHelper, string viewName, object model)
        {
            string content = "";
            var ctx = htmlHelper.ViewContext.Controller.ControllerContext;
            if (string.IsNullOrEmpty(viewName))
                viewName = ctx.RouteData.GetRequiredString("action");

            htmlHelper.ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ctx, viewName);
                ViewContext viewContext = new ViewContext(ctx, viewResult.View, htmlHelper.ViewData, htmlHelper.ViewContext.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                content = sw.GetStringBuilder().ToString();
            }
            return content;
        }


        /// <summary>
        /// Renders the pager.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="modelName"></param>
        public static void RenderPager(this HtmlHelper html, string modelName, object model)
        {
            var config = HttpContext.Current.Application.Get("model.settings") as IDictionary;
            if (config.GetOrDefault<bool>(modelName, "IsPagable", true))
                html.RenderPartial("Controls/Pager", model);
        }

        #endregion


        #region Ajax
        /// <summary>
        /// Creates an anchor action link that makes an ajax call to the url supplied and alerts (via alert box) the result.
        /// </summary>
        /// <param name="htmlhelper">The HtmlHelper</param>
        /// <param name="linkText">The text to display for the link</param>
        /// <param name="actionUrl">The url of the ajax call</param>
        /// <param name="id">Optional id for the anchor.</param>
        /// <returns></returns>
        public static string AjaxLinkWithAlert(this HtmlHelper htmlhelper, string linkText, string actionUrl, string id = "")
        {
            if (!string.IsNullOrEmpty(id))
                id = @"id=""" + id + @"""";

            System.Web.HttpUtility.UrlEncode(actionUrl);
            var code = "AjaxHelper.SendActionAndAlert('" + actionUrl + "');";
            var control = @"<a href=""#"" {0} onclick=""{1}; return false;"" class=""actionlink"" >{2}</a>";
            control = string.Format(control, id, code, linkText);
            return control;
        }


        /// <summary>
        /// Creates an anchor action link that makes an ajax call to the url supplied and alerts (via alert box) the result.
        /// </summary>
        /// <param name="htmlhelper">The HtmlHelper</param>
        /// <param name="linkText">The text to display for the link</param>
        /// <param name="actionUrl">The url of the ajax call</param>
        /// <param name="id">Optional id for the anchor.</param>
        /// <returns></returns>
        public static string AjaxLinkWithMessage(this HtmlHelper htmlhelper, string linkText, string actionUrl, string divIdForMessage, string id = "")
        {
            if (!string.IsNullOrEmpty(id))
                id = @"id=""" + id + @"""";

            System.Web.HttpUtility.UrlEncode(actionUrl);
            var code = "AjaxHelper.SendActionAndDisplayMessage('" + actionUrl + "', '" + divIdForMessage + "');";
            var control = @"<a href=""#"" {0} onclick=""{1}; return false;"" class=""actionlink"" >{2}</a>";
            control = string.Format(control, id, code, linkText);
            return control;
        }


        /// <summary>
        /// Creates an anchor action link that makes an ajax call to the url supplied and alerts (via alert box) the result.
        /// </summary>
        /// <param name="htmlhelper">The HtmlHelper</param>
        /// <param name="linkText">The text to display for the link</param>
        /// <param name="actionUrl">The url of the ajax call</param>
        /// <param name="id">Optional id for the anchor.</param>
        /// <returns></returns>
        public static string AjaxLinkWithCallback(this HtmlHelper htmlhelper, string linkText, string actionUrl, string callback, string id = "")
        {
            if (!string.IsNullOrEmpty(id))
                id = @"id=""" + id + @"""";

            System.Web.HttpUtility.UrlEncode(actionUrl);
            var code = "AjaxHelper.SendActionAndCallback('" + actionUrl + "', " + callback + ");";
            var control = @"<a href=""#"" {0} onclick=""{1}; return false;"" class=""actionlink"" >{2}</a>";
            control = string.Format(control, id, code, linkText);
            return control;
        }
        #endregion
    }
}
