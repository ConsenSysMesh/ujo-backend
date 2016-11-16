using System.IO;

namespace CCC.BlockchainProcessing
{
    public class TextWriterLogger : ILogger
    {
        private readonly TextWriter _textWriter;

        public TextWriterLogger(TextWriter textWriter)
        {
            _textWriter = textWriter;
        }
        public void WriteLine(string info)
        {
            _textWriter.WriteLine(info);
        }
    }
}