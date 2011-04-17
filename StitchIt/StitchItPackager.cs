using System;
using System.IO;
using System.Text;

namespace StitchIt
{
    public class StitchItPackager
    {
        const string ResourceBase = @"StitchIt.Resources";
        const string StitchItWrapperName = ResourceBase + @".stitchIt.js";
        const string StitchItModuleWrapperName = ResourceBase + @".stitchItModule.js";

        public string Package(string path)
        {
            var jsFiles = Directory.EnumerateFiles(path, @"*.js", SearchOption.AllDirectories);
            var modulesBuilder = new StringBuilder();
            var moduleWrapper = GetStitchItModuleWrapper();
            var stitchItWrapper = GetStitchItWrapper();
            var separate = false;

            foreach (var filePath in jsFiles)
            {
                var identifer = GenerateIdentifier(filePath);
                var content = File.ReadAllText(filePath);

                var module = moduleWrapper
                    .Replace("**MODULE**", content)
                    .Replace("**IDENTIFIER**", identifer);

                if (separate)
                    modulesBuilder.Append(",");

                modulesBuilder.Append(module);

                separate = true;
            }

            var stitchIt = stitchItWrapper.Replace("**MODULES**", modulesBuilder.ToString());

            return stitchIt;
        }

        private string GenerateIdentifier(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        private String GetStitchItWrapper()
        {
            return GetResource(StitchItWrapperName);
        }

        private String GetStitchItModuleWrapper()
        {
            return GetResource(StitchItModuleWrapperName);
        }

        private String GetResource(string name)
        {
            var assembly = typeof(StitchItHandler).Assembly;

            using (var stream = assembly.GetManifestResourceStream(name))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}