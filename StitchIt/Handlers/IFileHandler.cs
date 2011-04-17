namespace StitchIt.Handlers
{
    public interface IFileHandler
    {
        /// <summary>
        /// Get the file extension associated with this handler
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Build the file content into CommonJS Module
        /// </summary>
        /// <param name="content">The file content</param>
        /// <returns>Content suitable for a CommonJS module</returns>
        string Build(string content);
    }
}