namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true), CLSCompliant(false)]
    public sealed class ProvideIncludeFolderAttribute : RegistrationAttribute
    {
        private string extension;
        private string folder;
        public const string IncludeFoldersKeyName = "IncludeFolders";
        private int index;
        public const string TextTemplatingKeyName = "TextTemplating";

        public ProvideIncludeFolderAttribute(string extension, int index, string folder)
        {
            if (string.IsNullOrEmpty(extension))
            {
                throw new ArgumentNullException("extension");
            }
            if (!extension.StartsWith(".", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentOutOfRangeException("extension", Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ExtensionMalformed);
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", Microsoft.VisualStudio.TextTemplating.VSHost.Resources.IndexOutOfRange);
            }
            this.extension = extension;
            this.index = index;
            this.folder = folder;
        }

        public override void Register(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            string name = @"TextTemplating\IncludeFolders\" + this.Extension;
            using (RegistrationAttribute.Key key = context.CreateKey(name))
            {
                if (key != null)
                {
                    string str2 = context.EscapePath(this.Folder);
                    if (!Path.IsPathRooted(this.Folder) && !Regex.Match(this.Folder, @"^%\w+%", RegexOptions.Singleline).Success)
                    {
                        str2 = context.EscapePath(Path.Combine(context.ComponentPath, this.Folder));
                    }
                    key.SetValue(string.Format(CultureInfo.InvariantCulture, "Include{0}", new object[] { this.Index }), str2);
                    context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideIncludeFolderLogRegistered, new object[] { this.Extension, this.Index, str2 }));
                }
            }
        }

        public override void Unregister(RegistrationAttribute.RegistrationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.RemoveValue(@"TextTemplating\IncludeFolders\" + this.Extension, string.Format(CultureInfo.InvariantCulture, "Include{0}", new object[] { this.Index }));
            context.Log.WriteLine(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.VSHost.Resources.ProvideIncludeFolderLogUnregistered, new object[] { this.Extension, this.Index, this.Folder }));
        }

        public string Extension
        {
            [DebuggerStepThrough]
            get
            {
                return this.extension;
            }
        }

        public string Folder
        {
            [DebuggerStepThrough]
            get
            {
                return this.folder;
            }
        }

        public int Index
        {
            [DebuggerStepThrough]
            get
            {
                return this.index;
            }
        }
    }
}

