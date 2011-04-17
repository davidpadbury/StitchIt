using StitchIt.Handlers;

namespace StitchIt
{
    public class DefaultStitchItPackagerFactory
    {
        public StitchItPackager Build()
        {
            return new StitchItPackager(new[]
                {
                    new JavaScriptFileHandler()
                });
        }
    }
}