using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using StitchIt.Handlers;

namespace StitchIt
{
    public class StitchItPackager
    {
        const string StitchItWrapperName = "stitchIt.js";
        const string StitchItModuleWrapperName = "stitchItModule.js";

        readonly ResourceReader _resourceReader = new ResourceReader();
        readonly IEnumerable<IFileHandler> _fileHandlers;

        public StitchItPackager(IEnumerable<IFileHandler> fileHandlers)
        {
            _fileHandlers = fileHandlers;
        }

        public string Package(string rootPath)
        {
            if (!Directory.Exists(rootPath))
            {
                var message = string.Format("Directory not found: {0}", rootPath);
                throw new ArgumentException(message);
            }

            // Make sure the path ends with a directory separator
            if (!rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                rootPath += Path.DirectorySeparatorChar;

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
                        .Replace("**MODULE**", moduleContent)
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

        private static string GenerateIdentifier(string path)
        {
            var components = path.Split(Path.DirectorySeparatorChar);

            // Remove extension from last component
            var lastIndex = components.Length - 1;
            components[lastIndex] = Path.GetFileNameWithoutExtension(components[lastIndex]);

            var commonJsIdentifier = string.Join("/", components);

            return commonJsIdentifier;
        }

        private string GetStitchItWrapper()
        {
            return _resourceReader.Read(StitchItWrapperName);
        }

        private string GetStitchItModuleWrapper()
        {
            return _resourceReader.Read(StitchItModuleWrapperName);
        }
    }
}