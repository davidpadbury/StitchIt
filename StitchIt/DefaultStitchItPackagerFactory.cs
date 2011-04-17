using StitchIt.Handlers;

namespace StitchIt
{
    public class DefaultStitchItPackagerFactory
    {
        public StitchItPackager Build()
        {
            return new StitchItPackager(new IFileHandler[]
                {
                    new JavaScriptFileHandler(),
                    new HtmlTemplateFileHandler()
                });
        }
    }
}