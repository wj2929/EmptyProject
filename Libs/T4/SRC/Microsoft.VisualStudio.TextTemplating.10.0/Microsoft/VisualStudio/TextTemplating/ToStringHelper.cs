namespace Microsoft.VisualStudio.TextTemplating
{
    using Microsoft.VisualStudio.TextTemplating.CodeDom;
    using System;
    using System.CodeDom;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    public static class ToStringHelper
    {
        private static IFormatProvider formatProviderField = CultureInfo.InvariantCulture;

        internal static CodeTypeMemberCollection ProvideHelpers(CultureInfo formatProvider)
        {
            CodeTypeMemberCollection members = new CodeTypeMemberCollection();
            CodeTypeReference nestRef = new CodeTypeReference(ProvideNestedClass(formatProvider, members).Name);
            CodeMemberField toStringHelperField = ProvideNestedClassField(members, nestRef);
            ProvideNestedClassProperty(members, nestRef, toStringHelperField);
            return members;
        }

        private static CodeTypeDeclaration ProvideNestedClass(CultureInfo formatProvider, CodeTypeMemberCollection members)
        {
            CodeTypeDeclaration member = new CodeTypeDeclaration("ToStringInstanceHelper") {
                IsClass = true
            };
            member.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "ToString Helpers"));
            member.Attributes = MemberAttributes.Public;
            member.AddSummaryComment("Utility class to produce culture-oriented representation of an object as a string.");
            CodeMemberField field = ProvideNestedFormatProviderField(member, formatProvider);
            CodeFieldReferenceExpression formatProviderFieldRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), field.Name);
            ProvideNestedFormatProviderProperty(member, formatProviderFieldRef);
            ProvideNestedToStringWithCultureMethod(member, formatProviderFieldRef);
            members.Add(member);
            return member;
        }

        private static CodeMemberField ProvideNestedClassField(CodeTypeMemberCollection members, CodeTypeReference nestRef)
        {
            CodeMemberField field = new CodeMemberField(nestRef, "toStringHelperField") {
                Attributes = MemberAttributes.Private,
                InitExpression = new CodeObjectCreateExpression(nestRef, new CodeExpression[0])
            };
            members.Add(field);
            return field;
        }

        private static void ProvideNestedClassProperty(CodeTypeMemberCollection members, CodeTypeReference nestRef, CodeMemberField toStringHelperField)
        {
            CodeMemberProperty property = new CodeMemberProperty {
                Type = nestRef,
                Name = "ToStringHelper",
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), toStringHelperField.Name)));
            property.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "ToString Helpers"));
            members.Add(property);
        }

        private static CodeMemberField ProvideNestedFormatProviderField(CodeTypeDeclaration nest, CultureInfo formatProvider)
        {
            CodeMemberField field = new CodeMemberField(typeof(IFormatProvider), "formatProviderField ") {
                Attributes = MemberAttributes.Private
            };
            if (formatProvider == CultureInfo.InvariantCulture)
            {
                field.InitExpression = typeof(CultureInfo).Expr().Prop("InvariantCulture");
            }
            else
            {
                field.InitExpression = new CodeObjectCreateExpression(typeof(CultureInfo), new CodeExpression[] { formatProvider.Name.Prim() });
            }
            nest.Members.Add(field);
            return field;
        }

        private static void ProvideNestedFormatProviderProperty(CodeTypeDeclaration nest, CodeFieldReferenceExpression formatProviderFieldRef)
        {
            CodeMemberProperty member = new CodeMemberProperty {
                Name = "FormatProvider",
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Type = new CodeTypeReference(typeof(IFormatProvider))
            };
            member.AddSummaryComment("Gets or sets format provider to be used by ToStringWithCulture method.");
            member.GetStatements.Add(new CodeMethodReturnStatement(formatProviderFieldRef));
            member.SetStatements.Add(new CodeConditionStatement(new CodeBinaryOperatorExpression(new CodePropertySetValueReferenceExpression(), CodeBinaryOperatorType.IdentityInequality, CodeDomHelpers.nullEx), new CodeStatement[] { formatProviderFieldRef.Assign(new CodePropertySetValueReferenceExpression()) }));
            nest.Members.Add(member);
        }

        private static void ProvideNestedToStringWithCultureMethod(CodeTypeDeclaration nest, CodeFieldReferenceExpression formatProviderRef)
        {
            CodeParameterDeclarationExpression parameter = new CodeParameterDeclarationExpression(typeof(object), "objectToConvert");
            CodeVariableReferenceExpression callSite = new CodeVariableReferenceExpression("t");
            CodeVariableReferenceExpression left = new CodeVariableReferenceExpression("method");
            CodeMemberMethod method = CodeDomHelpers.CreateMethod(typeof(string), "ToStringWithCulture", "This is called from the compile/run appdomain to convert objects within an expression block to a string", MemberAttributes.Public | MemberAttributes.Final, new CodeObject[] { CodeDomHelpers.CheckNullParameter(parameter.Name), new CodeVariableDeclarationStatement(typeof(Type), callSite.VariableName, parameter.Ref().Call("GetType", new CodeExpression[0])), new CodeVariableDeclarationStatement(typeof(MethodInfo), left.VariableName, callSite.Call("GetMethod", new CodeExpression[] { "ToString".Prim(), new CodeArrayCreateExpression(typeof(Type), new CodeExpression[] { new CodeTypeOfExpression(typeof(IFormatProvider)) }) })), new CodeConditionStatement(new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.IdentityEquality, CodeDomHelpers.nullEx), new CodeStatement[] { new CodeMethodReturnStatement(parameter.Ref().Call("ToString", new CodeExpression[0])) }, new CodeStatement[] { new CodeMethodReturnStatement(new CodeCastExpression(typeof(string), left.Call("Invoke", new CodeExpression[] { parameter.Ref(), new CodeArrayCreateExpression(typeof(object), new CodeExpression[] { formatProviderRef }) }))) }) });
            method.Parameters.Add(parameter);
            nest.Members.Add(method);
        }

        public static string ToStringWithCulture(object objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            MethodInfo method = objectToConvert.GetType().GetMethod("ToString", new Type[] { typeof(IFormatProvider) });
            if (method == null)
            {
                return objectToConvert.ToString();
            }
            return (method.Invoke(objectToConvert, new object[] { formatProviderField }) as string);
        }

        public static IFormatProvider FormatProvider
        {
            [DebuggerStepThrough]
            get
            {
                return formatProviderField;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null)
                {
                    formatProviderField = value;
                }
            }
        }
    }
}

