using System;
using System.IO;
using System.Web;
using Jurassic;
using Machine.Fakes;
using Machine.Specifications;

namespace StitchIt.Specs
{
    public class requiring_simple_export : PackagerSpecs
    {
        Because of = () => Load("SimpleRequire");

        It should_have_loaded = () => Engine.Evaluate<string>(@"stitchIt.require('calc').message").ShouldEqual("OH HAI");
    }

    public class PackagerSpecs
    {
        protected static ScriptEngine Engine;

        protected static void Load(string fixture)
        {
            var path = Path.Combine(@"..\..\Fixtures", fixture);

            var packager = new StitchItPackager();
            var content = packager.Package(path);

            Engine = new ScriptEngine();
            Engine.Evaluate(content);
        }
    }
}