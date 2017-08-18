namespace Microsoft.VisualStudio.TextTemplating.Modeling.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static string ErrorInvalidModelFilePath
        {
            get
            {
                return ResourceManager.GetString("ErrorInvalidModelFilePath", resourceCulture);
            }
        }

        internal static string ErrorLoadFailed
        {
            get
            {
                return ResourceManager.GetString("ErrorLoadFailed", resourceCulture);
            }
        }

        internal static string ErrorLocateSerializer
        {
            get
            {
                return ResourceManager.GetString("ErrorLocateSerializer", resourceCulture);
            }
        }

        internal static string ErrorUnknownErrorOccurred
        {
            get
            {
                return ResourceManager.GetString("ErrorUnknownErrorOccurred", resourceCulture);
            }
        }

        internal static string ExisitingCacheEntry
        {
            get
            {
                return ResourceManager.GetString("ExisitingCacheEntry", resourceCulture);
            }
        }

        internal static string ModelingFileNotFound
        {
            get
            {
                return ResourceManager.GetString("ModelingFileNotFound", resourceCulture);
            }
        }

        internal static string ModelingRelativePathFailed
        {
            get
            {
                return ResourceManager.GetString("ModelingRelativePathFailed", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("Microsoft.VisualStudio.TextTemplating.Modeling.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }
    }
}

