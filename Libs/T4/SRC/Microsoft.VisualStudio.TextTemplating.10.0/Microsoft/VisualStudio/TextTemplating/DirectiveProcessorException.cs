namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DirectiveProcessorException : Exception
    {
        public DirectiveProcessorException()
        {
        }

        public DirectiveProcessorException(string message) : base(message)
        {
        }

        protected DirectiveProcessorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DirectiveProcessorException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

