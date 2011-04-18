namespace StitchIt.Builder
{
    public interface IPathBuilder
    {
        /// <summary>
        /// Sets where the root of the client-side javascript files to be stitched.
        /// </summary>
        /// <param name="virtualPath">The virtual path to the directory.</param>
        /// <returns>Endpoint builder</returns>
        IEndpointBuilder RootedAt(string virtualPath);
    }
}