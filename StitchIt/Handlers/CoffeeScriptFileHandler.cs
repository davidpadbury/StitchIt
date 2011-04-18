using System;
using StitchIt.Handlers.CoffeeScript;

namespace StitchIt.Handlers
{
    public class CoffeeScriptFileHandler : IFileHandler
    {
        private readonly CoffeeScriptCompiler _compiler = new CoffeeScriptCompiler();

        public string Extension
        {
            get { return "coffee"; }
        }

        public string Build(string content)
        {
            return _compiler.Compile(content);
        }
    }
}