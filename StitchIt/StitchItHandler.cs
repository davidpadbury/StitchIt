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
        readonly StitchItPackager _packager = new StitchItPackager();
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
            var stitchIt = _packager.Package(path);

            response.ContentType = "text/javascript";
            response.StatusCode = 200;
            
            response.Write(stitchIt);
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