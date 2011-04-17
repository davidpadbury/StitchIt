using System;
using System.Data;
using System.IO;
using System.Web;
using Jurassic;
using Machine.Fakes;
using Machine.Specifications;

namespace StitchIt.Specs
{
    public class requiring_simple_export : PackagerSpecs
    {
        static string Message;

        Because of = () => Message = Evaluate<string>("SimpleRequire", @"stitchIt.require('calc').message");

        It returned_message = () => Message.ShouldEqual("OH HAI");
    }

    public class with_no_modules : PackagerSpecs
    {
        static Exception Error;

        Because of = () => Error = Catch.Exception(() => Evaluate("NoModules", "stitchIt.require('noModule')"));

        It should_throw_eror = () => Error.ShouldNotBeNull();
        It should_put_undefined_module_name_in_error = () => Error.Message.ShouldContain("noModule");
    }

    public class requiring_relative_modules : PackagerSpecs
    {
        static bool Result;

        Because of = () => Result = Evaluate<bool>("Relative", "stitchIt.require('program').sameFunction");

        It should_have_imported_the_same_function = () => Result.ShouldBeTrue();
    }

    public abstract class PackagerSpecs
    {
        protected static ScriptEngine Engine;

        protected static void Evaluate(string fixture, string expression)
        {
            Load(fixture);

            Engine.Evaluate(expression);
        }

        protected static T Evaluate<T>(string fixture, string expression)
        {
            Load(fixture);

            return Evaluate<T>(expression);
        }

        protected static T Evaluate<T>(string expression)
        {
            return Engine.Evaluate<T>(expression);
        }

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