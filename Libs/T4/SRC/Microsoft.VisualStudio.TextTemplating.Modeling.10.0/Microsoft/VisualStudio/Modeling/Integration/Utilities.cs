namespace Microsoft.VisualStudio.Modeling.Integration
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    internal static class Utilities
    {
        internal static string ComposeUIMessage(string messageResource, params object[] arguments)
        {
            return string.Format(CultureInfo.CurrentCulture, messageResource, arguments);
        }

        [Conditional("DEBUG")]
        private static void ValidateMessageParameters(string message, int paramNumber)
        {
            for (int i = 0; Microsoft.VisualStudio.Modeling.Integration.StringComparer.Ordinal.Contains(message, "{" + i + "}") && (i < 0x7fffffff); i++)
            {
            }
        }
    }
}

