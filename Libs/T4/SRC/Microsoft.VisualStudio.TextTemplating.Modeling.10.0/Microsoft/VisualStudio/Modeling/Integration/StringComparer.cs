namespace Microsoft.VisualStudio.Modeling.Integration
{
    using System;
    using System.CodeDom.Compiler;

    [GeneratedCode("CommonStringComparer", "1.0")]
    internal class StringComparer : System.StringComparer
    {
        public static readonly Microsoft.VisualStudio.Modeling.Integration.StringComparer AssemblyName = OrdinalIgnoreCase;
        public static readonly Microsoft.VisualStudio.Modeling.Integration.StringComparer CurrentCulture = new Microsoft.VisualStudio.Modeling.Integration.StringComparer(StringComparison.CurrentCulture);
        public static readonly Microsoft.VisualStudio.Modeling.Integration.StringComparer CurrentCultureIgnoreCase = new Microsoft.VisualStudio.Modeling.Integration.StringComparer(StringComparison.CurrentCultureIgnoreCase);
        public static readonly Microsoft.VisualStudio.Modeling.Integration.StringComparer FileName = OrdinalIgnoreCase;
        public static readonly Microsoft.VisualStudio.Modeling.Integration.StringComparer Ordinal = new Microsoft.VisualStudio.Modeling.Integration.StringComparer(StringComparison.Ordinal);
        public static readonly Microsoft.VisualStudio.Modeling.Integration.StringComparer OrdinalIgnoreCase = new Microsoft.VisualStudio.Modeling.Integration.StringComparer(StringComparison.OrdinalIgnoreCase);
        public static readonly Microsoft.VisualStudio.Modeling.Integration.StringComparer PathName = OrdinalIgnoreCase;
        public static readonly Microsoft.VisualStudio.Modeling.Integration.StringComparer ReferenceString = Ordinal;
        private System.StringComparer stringComparer;
        private StringComparison stringComparison;

        private StringComparer(StringComparison stringComparison)
        {
            this.stringComparison = stringComparison;
            this.stringComparer = GetMatchingStringComparer(stringComparison);
        }

        public override int Compare(string x, string y)
        {
            return this.stringComparer.Compare(x, y);
        }

        public int Compare(string x, int indexX, string y, int indexY, int length)
        {
            return string.Compare(x, indexX, y, indexY, length, this.stringComparison);
        }

        public bool Contains(string mainString, string subStringToCheckFor)
        {
            ValidatePublicArg.NotNull(mainString, "mainString");
            ValidatePublicArg.NotNull(subStringToCheckFor, "subStringToCheckFor");
            return (mainString.IndexOf(subStringToCheckFor, this.stringComparison) >= 0);
        }

        public bool EndsWith(string mainString, string subStringToCheckFor)
        {
            ValidatePublicArg.NotNull(mainString, "mainString");
            ValidatePublicArg.NotNull(subStringToCheckFor, "subStringToCheckFor");
            return mainString.EndsWith(subStringToCheckFor, this.stringComparison);
        }

        public override bool Equals(string x, string y)
        {
            return this.stringComparer.Equals(x, y);
        }

        public override int GetHashCode(string x)
        {
            return this.stringComparer.GetHashCode(x);
        }

        private static System.StringComparer GetMatchingStringComparer(StringComparison stringComparison)
        {
            switch (stringComparison)
            {
                case StringComparison.CurrentCulture:
                    return System.StringComparer.CurrentCulture;

                case StringComparison.CurrentCultureIgnoreCase:
                    return System.StringComparer.CurrentCultureIgnoreCase;

                case StringComparison.Ordinal:
                    return System.StringComparer.Ordinal;

                case StringComparison.OrdinalIgnoreCase:
                    return System.StringComparer.OrdinalIgnoreCase;
            }
            return System.StringComparer.Ordinal;
        }

        public int IndexOf(string mainString, char charToCheckFor)
        {
            ValidatePublicArg.NotNull(mainString, "mainString");
            return mainString.IndexOf(charToCheckFor);
        }

        public int IndexOf(string mainString, string subStringToCheckFor)
        {
            ValidatePublicArg.NotNull(mainString, "mainString");
            ValidatePublicArg.NotNull(subStringToCheckFor, "subStringToCheckFor");
            return mainString.IndexOf(subStringToCheckFor, this.stringComparison);
        }

        public int IndexOfAny(string mainString, char[] charsToCheckFor)
        {
            ValidatePublicArg.NotNull(mainString, "mainString");
            ValidatePublicArg.NotNull(charsToCheckFor, "charsToCheckFor");
            return mainString.IndexOfAny(charsToCheckFor);
        }

        public int LastIndexOf(string mainString, char charToCheckFor)
        {
            ValidatePublicArg.NotNull(mainString, "mainString");
            return mainString.LastIndexOf(charToCheckFor);
        }

        public int LastIndexOf(string mainString, string subStringToCheckFor)
        {
            ValidatePublicArg.NotNull(mainString, "mainString");
            ValidatePublicArg.NotNull(subStringToCheckFor, "subStringToCheckFor");
            return mainString.LastIndexOf(subStringToCheckFor, this.stringComparison);
        }

        public int LastIndexOfAny(string mainString, char[] charsToCheckFor)
        {
            ValidatePublicArg.NotNull(mainString, "mainString");
            ValidatePublicArg.NotNull(charsToCheckFor, "charsToCheckFor");
            return mainString.LastIndexOfAny(charsToCheckFor);
        }

        public bool StartsWith(string mainString, string subStringToCheckFor)
        {
            ValidatePublicArg.NotNull(mainString, "mainString");
            ValidatePublicArg.NotNull(subStringToCheckFor, "subStringToCheckFor");
            return mainString.StartsWith(subStringToCheckFor, this.stringComparison);
        }

        public static Microsoft.VisualStudio.Modeling.Integration.StringComparer InvariantCulture
        {
            get
            {
                throw new InvalidOperationException("InvariantCulture should never be used!");
            }
        }

        public static Microsoft.VisualStudio.Modeling.Integration.StringComparer InvariantCultureIgnoreCase
        {
            get
            {
                throw new InvalidOperationException("InvariantCultureIgnoreCase should never be used!");
            }
        }

        private static class ValidatePublicArg
        {
            public static void NotNull(object arg, string paramName)
            {
                if (arg == null)
                {
                    throw new ArgumentNullException(paramName);
                }
            }
        }
    }
}

