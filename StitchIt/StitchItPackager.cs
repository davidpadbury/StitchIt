using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StitchIt.Handlers;
using System.Linq;

namespace StitchIt
{
    public class StitchItPackager
    {
        const string ResourceBase = @"StitchIt.Resources";
        const string StitchItWrapperName = ResourceBase + @".stitchIt.js";
        const string StitchItModuleWrapperName = ResourceBase + @".stitchItModule.js";

        readonly IEnumerable<IFileHandler> _fileHandlers;

        public StitchItPackager(IEnumerable<IFileHandler> fileHandlers)
        {
            _fileHandlers = fileHandlers;
        }

        public string Package(string rootPath)
        {
            // Directory should exist
            if (!Directory.Exists(rootPath))
            {
                var message = string.Format("Directory not found: {0}", rootPath);
                throw new ArgumentException(message);
            }

            // Make sure the path ends with a directory separator
            if (rootPath[rootPath.Length - 1] != Path.DirectorySeparatorChar)
                rootPath = rootPath + Path.DirectorySeparatorChar;

            var modulesBuilder = new StringBuilder();
            var moduleWrapper = GetStitchItModuleWrapper();
            var stitchItWrapper = GetStitchItWrapper();
            var separate = false;

            foreach (var handler in _fileHandlers)
            {
                var pattern = string.Format("*.{0}", handler.Extension);
                var files = Directory.EnumerateFiles(rootPath, pattern, SearchOption.AllDirectories);

                foreach (var filePath in files)
                {
                    // TODO: This is a really dumb way to find the relative path
                    var relativePath = filePath.Replace(rootPath, string.Empty);

                    var identifer = GenerateIdentifier(relativePath);
                    var fileContent = File.ReadAllText(filePath);
                    var moduleContent = handler.Build(fileContent);

                    var module = moduleWrapper
                        .Replace("**MODULE**", fileContent)
                        .Replace("**IDENTIFIER**", identifer);

                    if (separate)
                        modulesBuilder.Append(",");

                    modulesBuilder.Append(module);

                    separate = true;
                }
            }

            var stitchIt = stitchItWrapper.Replace("**MODULES**", modulesBuilder.ToString());

            return stitchIt;
        }

        private string GenerateIdentifier(string path)
        {
            var components = path.Split(Path.DirectorySeparatorChar);

            // Remove extension from last component
            var lastIndex = components.Length - 1;
            components[lastIndex] = Path.GetFileNameWithoutExtension(components[lastIndex]);

            var commonJsIdentifier = string.Join("/", components);

            return commonJsIdentifier;
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