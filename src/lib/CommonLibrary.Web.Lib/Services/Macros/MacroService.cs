using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


using ComLib.Web.Templating;
using ComLib.Reflection;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Services;
using ComLib.Web.Lib.Services.Macros;

namespace ComLib.Web.Lib.Services.Macros
{
    
    /// <summary>
    /// Information Service
    /// </summary>
    public class MacroService : ExtensionService<MacroAttribute, IMacro>, IMacroService
    {
        private string _prefix;
        private string _openBracket;
        private string _closeBracket;


        public MacroService() : this("$", "[", "]")
        {
        }


        public MacroService(string prefix, string open, string close)
        {
            _prefix = prefix;
            _openBracket = open;
            _closeBracket = close;
            _onLoadCompleteCallback = () => OnLoadComplete();
        }


        /// <summary>
        /// Builds the content for the specified tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string BuildContent(Tag tag)
        {
            var macro = Create(tag.Name);
            string content = macro.Process(tag);
            return content;
        }


        /// <summary>
        /// Builds the content by replacing any custom cms tags with their actual content.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="components"></param>
        /// <returns></returns>
        public string BuildContent(string content)
        {
            var doc = new DomDocParser(_prefix, _openBracket, _closeBracket);
            var result = doc.Parse(content);
            if (result == null || result.Tags == null || result.Tags.Count == 0)
            {
                return content;
            }

            var buffer = new StringBuilder();
            foreach (var tag in result.Tags)
            {
                var macro = Create(tag.Name);
                buffer.Append(macro.Process(tag));
            }
            string finalText = buffer.ToString();
            return finalText;
        }



        private void OnLoadComplete()
        {
            foreach (var metaInfo in this._lookup)
            {
                var atts = metaInfo.Value.DataType.GetCustomAttributes(typeof(MacroParameter), false);
                foreach(var att in atts)
                {
                    if(att is MacroParameter)
                    {
                        metaInfo.Value.AdditionalAttributes.Add(att);
                    }
                }
            }
        }
    }
}
