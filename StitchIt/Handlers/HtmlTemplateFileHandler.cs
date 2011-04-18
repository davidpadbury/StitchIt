using System;

namespace StitchIt.Handlers
{
    public class HtmlTemplateFileHandler : IFileHandler
    {
        const string TemplateModule = @"module.exports = '{0}';";

        public string Extension
        {
            get { return "html"; }
        }

        public string Build(string content)
        {
            // We're enclosing the javascript in a string so escape any single quote string delimiters
            var escapedContent = content.Replace(@"'", @"\'").Replace(Environment.NewLine, @"\n");
            var moduleContent = string.Format(TemplateModule, escapedContent);

            return moduleContent;
        }
    }
}