using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Authentication;
using ComLib.Web.Templating;
using ComLib.Web.Lib.Services.Macros;


namespace ComLib.Web.Modules.Macros
{
    [Macro( Name = "showforroles", DisplayName = "Show For Roles", Description = "Shows the content supplied for the roles specified", IsReusable = true, HasInnerContent = true)]
    [MacroParameter( Name = "roles", Description = "Name of the roles to show the content for", DataType = typeof(string), Example = "Admin", ExampleMultiple = "? | * | Admin | Users,Moderators" )]
    public class ShowForRolesMacro : IMacro
    {
        /// <summary>
        /// Process the tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string Process(Tag tag)
        {
            if (!tag.Attributes.Contains("roles"))
                return string.Empty;

            var roles = tag.Attributes["roles"] as string;
            if (roles == "?")
                return tag.InnerContent;

            bool isAuthenticated = Auth.IsAuthenticated();
            if (roles == "*" && isAuthenticated)
                return tag.InnerContent;

            string result = Auth.IsUserInRoles(roles)
                          ? tag.InnerContent
                          : string.Empty;
            return result;
        }
    }
}
