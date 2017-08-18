namespace Microsoft.VisualStudio.TextTemplating.Modeling
{
    using Microsoft.VisualStudio.Modeling.Integration;
    using Microsoft.VisualStudio.TextTemplating.Modeling.Properties;
    using System;

    internal static class ModelBusExceptionMessages
    {
        private static string unknownErrorOccurred;

        internal static string GetErrorLoadFailed(string path)
        {
            return Microsoft.VisualStudio.Modeling.Integration.Utilities.ComposeUIMessage(Resources.ErrorLoadFailed, new object[] { path });
        }

        internal static string GetErrorLocateSerializer(string path)
        {
            return Microsoft.VisualStudio.Modeling.Integration.Utilities.ComposeUIMessage(Resources.ErrorLocateSerializer, new object[] { path });
        }

        internal static string GetInvalidModelFilePath(string path)
        {
            return Microsoft.VisualStudio.Modeling.Integration.Utilities.ComposeUIMessage(Resources.ErrorInvalidModelFilePath, new object[] { path });
        }

        internal static string UnknownErrorOccurred
        {
            get
            {
                if (string.IsNullOrEmpty(unknownErrorOccurred))
                {
                    unknownErrorOccurred = Microsoft.VisualStudio.Modeling.Integration.Utilities.ComposeUIMessage(Resources.ErrorUnknownErrorOccurred, new object[0]);
                }
                return unknownErrorOccurred;
            }
        }
    }
}

