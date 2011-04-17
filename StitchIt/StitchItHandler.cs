using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace StitchIt
{
    public class StitchItHandler : IHttpHandler, IRouteHandler
    {
        const string ResourceBase = @"StitchIt.Resources";
        const string StitchItWrapperName = ResourceBase + @".stitchIt.js";
        const string StitchItModuleWrapperName = ResourceBase + @".stitchItModule.js";

        readonly string _rootPath;
        readonly string _publicationPath;

        public StitchItHandler(string rootPath, string publicationPath)
        {
            _rootPath = rootPath;
            _publicationPath = publicationPath;
        }

        public void ProcessRequest(HttpContext context)
        {
            var response = context.Response;
            var path = context.Server.MapPath(_rootPath);
            var jsFiles = Directory.EnumerateFiles(path, @"*.js", SearchOption.AllDirectories);
            var modulesBuilder = new StringBuilder();
            var moduleWrapper = GetStitchItModuleWrapper();
            var stitchItWrapper = GetStitchItWrapper();
            var separate = false;

            response.ContentType = "text/javascript";

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

            response.Write(stitchIt);
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
            var assembly = typeof (StitchItHandler).Assembly;

            using (var stream = assembly.GetManifestResourceStream(name))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }
    }
}