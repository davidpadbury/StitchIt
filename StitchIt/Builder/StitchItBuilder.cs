using System;
using System.Web.Routing;

namespace StitchIt.Builder
{
    public class StitchItBuilder : IPathBuilder, IEndpointBuilder
    {
        readonly RouteCollection _routeCollection;
        string _rootPath;
        string _publicationPath;

        public StitchItBuilder(RouteCollection routeCollection)
        {
            _routeCollection = routeCollection;
        }

        public IEndpointBuilder RootedAt(string virtualPath)
        {
            _rootPath = virtualPath;

            return this;
        }

        public void PublishAt(string virtualPath)
        {
            _publicationPath = virtualPath;

            InstallHandler();
        }

        private void InstallHandler()
        {
            var stitchItHandler = new StitchItHandler(_rootPath, _publicationPath);
            var route = new Route(_publicationPath, stitchItHandler);

            _routeCollection.Add("StitchIt", route);
        }
    }
}