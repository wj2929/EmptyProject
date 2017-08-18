namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [CLSCompliant(true)]
    public interface ITextTemplatingSession : IEquatable<ITextTemplatingSession>, IEquatable<Guid>, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, ISerializable
    {
        Guid Id { get; }
    }
}

