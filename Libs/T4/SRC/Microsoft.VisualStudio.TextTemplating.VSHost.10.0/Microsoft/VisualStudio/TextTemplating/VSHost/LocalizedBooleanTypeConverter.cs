namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    internal class LocalizedBooleanTypeConverter : BooleanConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            bool? nullable = this.StringToBool(value as string);
            if (nullable.HasValue)
            {
                return nullable.Value;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                bool? nullable = value as bool?;
                if (nullable.HasValue)
                {
                    if (!nullable.Value)
                    {
                        return Resources.BooleanFalse;
                    }
                    return Resources.BooleanTrue;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        private bool? StringToBool(string stringValue)
        {
            if (stringValue != null)
            {
                if (StringComparer.OrdinalIgnoreCase.Compare(stringValue, Resources.BooleanTrue) == 0)
                {
                    return true;
                }
                if (StringComparer.OrdinalIgnoreCase.Compare(stringValue, Resources.BooleanFalse) == 0)
                {
                    return false;
                }
            }
            return null;
        }
    }
}

