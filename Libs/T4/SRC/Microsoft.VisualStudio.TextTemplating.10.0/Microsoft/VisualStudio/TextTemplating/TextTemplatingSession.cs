namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class TextTemplatingSession : Dictionary<string, object>, ITextTemplatingSession, IEquatable<ITextTemplatingSession>, IEquatable<Guid>, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, ISerializable
    {
        private Guid id;
        private const string idName = "__TextTransformationSession_Id";

        public TextTemplatingSession()
        {
            this.id = Guid.NewGuid();
        }

        public TextTemplatingSession(Guid id)
        {
            this.id = id;
        }

        private TextTemplatingSession(SerializationInfo information, StreamingContext context) : base(information, context)
        {
            this.id = (Guid) information.GetValue("__TextTransformationSession_Id", typeof(Guid));
        }

        public bool Equals(ITextTemplatingSession other)
        {
            if (other == null)
            {
                return false;
            }
            return this.Equals(other.Id);
        }

        public bool Equals(Guid other)
        {
            return (this.Id.CompareTo(other) == 0);
        }

        public override bool Equals(object obj)
        {
            ITextTemplatingSession other = obj as ITextTemplatingSession;
            if (other == null)
            {
                return false;
            }
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        void ISerializable.GetObjectData(SerializationInfo information, StreamingContext context)
        {
            information.AddValue("__TextTransformationSession_Id", this.id, typeof(Guid));
            base.GetObjectData(information, context);
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }
    }
}

