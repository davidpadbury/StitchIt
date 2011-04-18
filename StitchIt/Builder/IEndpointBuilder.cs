namespace StitchIt.Builder
{
    public interface IEndpointBuilder
    {
        /// <summary>
        /// Sets where the root of the stiched client-side javascript files to be published.
        /// </summary>
        /// <param name="virtualPath">The virtual path to the directory.</param>
        /// <returns>Endpoint builder</returns>
        void PublishAt(string virtualPath);
    }
}