using System.Web.Routing;
using StitchIt.Builder;

namespace StitchIt
{
    public static class RouteCollectionExtensions
    {
        public static IPathBuilder StitchIt(this RouteCollection routeCollection)
        {
            return new StitchItBuilder(routeCollection);
        }
    }
}