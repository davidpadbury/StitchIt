using System;

namespace StitchIt.Handlers
{
    public class JavaScriptFileHandler : IFileHandler
    {
        public string Extension
        {
            get { return "js"; }
        }

        public string Build(string content)
        {
            return content;
        }
    }
}