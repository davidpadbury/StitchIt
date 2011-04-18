using System;
using Jurassic;

namespace StitchIt.Handlers.CoffeeScript
{
    public class CoffeeScriptCompiler
    {
        const string CoffeeScriptResourceName = "coffee-script.js";

        /// <summary>
        /// JavaScript for calling CoffeeScript compiler and specifying that we don't want it wrapped in a module.
        /// </summary>
        const string CompileExpressionPattern = "CoffeeScript.compile('{0}', {{bare:true}})";

        static readonly object EngineLock = new object();
        static readonly ResourceReader _resourceReader = new ResourceReader();
        static volatile ScriptEngine Engine;

        private static void CreateEngine()
        {
            if (Engine == null)
            {
                lock (EngineLock)
                {
                    if (Engine == null)
                    {
                        var coffeeScriptSource = LoadCoffeeScriptSource();

                        Engine = new ScriptEngine();
                        
                        // Load coffeescript
                        Engine.Execute(coffeeScriptSource);
                    }
                }
            }
        }

        private static string LoadCoffeeScriptSource()
        {
            return _resourceReader.Read(CoffeeScriptResourceName);
        }

        public CoffeeScriptCompiler()
        {
            // The coffeescript engine takes quite so long (~8secs) to parse that we'll share an instance through
            // all instances of the CoffeeScriptCompiler. Kick off the lazy initialization at this point
            // so it will hopefully be ready by the time we actually need to use it
            CreateEngine();
        }

        public string Compile(string coffeeScript)
        {
            // I've read that the Jurassic engine isn't thread safe so be cautious 
            lock(EngineLock)
            {
                var escapeCoffeeScript = coffeeScript.Replace("'", @"\'").Replace(Environment.NewLine, @"\n");
                var compileExpression = string.Format(CompileExpressionPattern, escapeCoffeeScript);

                return Engine.Evaluate<string>(compileExpression);
            }
        }
    }
}