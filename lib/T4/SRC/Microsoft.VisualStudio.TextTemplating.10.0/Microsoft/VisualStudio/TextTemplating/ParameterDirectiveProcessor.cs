namespace Microsoft.VisualStudio.TextTemplating
{
    using Microsoft.VisualStudio.TextTemplating.CodeDom;
    using Microsoft.VisualStudio.TextTemplating.Properties;
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting.Messaging;
    using System.Text;

    public sealed class ParameterDirectiveProcessor : DirectiveProcessor, IRecognizeHostSpecific
    {
        private StringBuilder codeBuffer;
        internal const string DirectiveName = "parameter";
        private bool hostSpecific;
        private CodeDomProvider languageCodeDomProvider;
        private StringBuilder postInitializationBuffer;
        internal const string ProcessorName = "ParameterDirectiveProcessor";
        private ITextTemplatingEngineHost templatingHost;

        public override void FinishProcessingRun()
        {
        }

        private static void GenerateCallContextLookup(string nameValue, CodeMemberField field, CodeStatementCollection statements, CodeVariableReferenceExpression valueAcquired, CodeTypeReference typeReference, string typeValue)
        {
            CodeVariableDeclarationStatement declaration = typeof(object).Decl("data", typeof(CallContext).Expr().Call("LogicalGetData", new CodeExpression[] { nameValue.Prim() }));
            CodeVariableReferenceExpression expression = declaration.Ref();
            CodeAssignStatement statement2 = field.Ref().Assign(typeReference.Cast(expression));
            CodeConditionStatement statement3 = new CodeConditionStatement(expression.NotNull(), new CodeStatement[] { GenerateTypeMismatchCheck(expression, nameValue, typeReference, typeValue, new CodeStatement[] { statement2 }) });
            statements.Add(new CodeConditionStatement(valueAcquired.VEquals(false.Prim()), new CodeStatement[] { declaration, statement3 }));
        }

        private void GenerateClassCode(string nameValue, string typeValue, out CodeMemberField field)
        {
            field = new CodeMemberField(typeValue, string.Format(CultureInfo.InvariantCulture, "_{0}Field", new object[] { nameValue }));
            field.Attributes = MemberAttributes.Private;
            field.Type.Options = CodeTypeReferenceOptions.GlobalReference;
            CodeMemberProperty member = new CodeMemberProperty {
                Name = nameValue,
                Type = new CodeTypeReference(typeValue, CodeTypeReferenceOptions.GlobalReference)
            };
            member.GetStatements.Add(new CodeMethodReturnStatement(field.Ref()));
            member.AddSummaryComment("Access the " + nameValue + " parameter of the template.");
            CodeGeneratorOptions standardOptions = StandardOptions;
            using (StringWriter writer = new StringWriter(this.codeBuffer, CultureInfo.InvariantCulture))
            {
                this.languageCodeDomProvider.GenerateCodeFromMember(field, writer, standardOptions);
                this.languageCodeDomProvider.GenerateCodeFromMember(member, writer, standardOptions);
            }
        }

        private static void GenerateHostResolveParameterValueLookup(string nameValue, CodeMemberField field, CodeStatementCollection statements, CodeVariableReferenceExpression valueAcquired, CodeTypeReference typeReference, CodeStatement setValueAcquired, string typeValue)
        {
            CodePropertyReferenceExpression callSite = new CodeThisReferenceExpression().Prop("Host");
            CodeVariableDeclarationStatement declaration = new CodeVariableDeclarationStatement(typeof(string).Ref(), "parameterValue", callSite.Call("ResolveParameterValue", new CodeExpression[] { "Property".Prim(), "PropertyDirectiveProcessor".Prim(), nameValue.Prim() }));
            CodeVariableReferenceExpression expression2 = declaration.Ref();
            CodeVariableDeclarationStatement statement2 = new CodeVariableDeclarationStatement(typeof(TypeConverter).Ref(), "tc", typeof(TypeDescriptor).Expr().Call("GetConverter", new CodeExpression[] { new CodeTypeOfExpression(typeReference) }));
            statements.Add(new CodeConditionStatement(valueAcquired.VEquals(false.Prim()), new CodeStatement[] { declaration, new CodeConditionStatement(typeof(string).Expr().Call("IsNullOrEmpty", new CodeExpression[] { expression2 }).VEquals(false.Prim()), new CodeStatement[] { statement2, new CodeConditionStatement(statement2.Ref().NotNull().And(statement2.Ref().Call("CanConvertFrom", new CodeExpression[] { new CodeTypeOfExpression(typeof(string).Ref()) })), new CodeStatement[] { field.Ref().Assign(new CodeCastExpression(typeReference, statement2.Ref().Call("ConvertFrom", new CodeExpression[] { expression2 }))), setValueAcquired }, new CodeStatement[] { GenerateReportTypeMismatch(nameValue, typeValue) }) }) }));
        }

        private void GeneratePostInitCode(string nameValue, string typeValue, CodeMemberField field)
        {
            CodeStatementCollection statements = new CodeStatementCollection();
            CodeVariableDeclarationStatement declaration = new CodeVariableDeclarationStatement(typeof(bool).Ref(), nameValue + "ValueAcquired", false.Prim());
            CodeVariableReferenceExpression lhs = declaration.Ref();
            CodeTypeReference typeReference = new CodeTypeReference(typeValue, CodeTypeReferenceOptions.GlobalReference);
            CodeStatement setValueAcquired = lhs.Assign(true.Prim());
            statements.Add(declaration);
            GenerateSessionLookup(nameValue, field, statements, setValueAcquired, typeReference, typeValue);
            if (this.hostSpecific)
            {
                GenerateHostResolveParameterValueLookup(nameValue, field, statements, lhs, typeReference, setValueAcquired, typeValue);
            }
            GenerateCallContextLookup(nameValue, field, statements, lhs, typeReference, typeValue);
            CodeGeneratorOptions standardOptions = StandardOptions;
            using (StringWriter writer = new StringWriter(this.postInitializationBuffer, CultureInfo.InvariantCulture))
            {
                foreach (CodeStatement statement3 in statements)
                {
                    this.languageCodeDomProvider.GenerateCodeFromStatement(statement3, writer, standardOptions);
                }
            }
        }

        private static CodeStatement GenerateReportTypeMismatch(string nameValue, string typeValue)
        {
            return new CodeThisReferenceExpression().CallS("Error", new CodeExpression[] { string.Format(CultureInfo.CurrentCulture, Resources.ParameterDirectiveTypeMismatch, new object[] { nameValue, typeValue }).Prim() });
        }

        private static void GenerateSessionLookup(string nameValue, CodeMemberField field, CodeStatementCollection statements, CodeStatement setValueAcquired, CodeTypeReference typeReference, string typeValue)
        {
            CodeExpression variable = new CodeIndexerExpression(new CodeThisReferenceExpression().Prop("Session"), new CodeExpression[] { nameValue.Prim() });
            statements.Add(new CodeConditionStatement(new CodeThisReferenceExpression().Prop("Session").Call("ContainsKey", new CodeExpression[] { nameValue.Prim() }), new CodeStatement[] { GenerateTypeMismatchCheck(variable, nameValue, typeReference, typeValue, new CodeStatement[] { field.Ref().Assign(typeReference.Cast(variable)), setValueAcquired }) }));
        }

        private static CodeStatement GenerateTypeMismatchCheck(CodeExpression variable, string nameValue, CodeTypeReference typeReference, string typeValue, CodeStatement[] elseClause)
        {
            CodeStatement statement = GenerateReportTypeMismatch(nameValue, typeValue);
            return new CodeConditionStatement(new CodeTypeOfExpression(typeReference).Call("IsAssignableFrom", new CodeExpression[] { variable.Call("GetType", new CodeExpression[0]) }).UnaryNot(), new CodeStatement[] { statement }, elseClause);
        }

        public override string GetClassCodeForProcessingRun()
        {
            return this.codeBuffer.ToString();
        }

        public override string[] GetImportsForProcessingRun()
        {
            return new string[0];
        }

        public override string GetPostInitializationCodeForProcessingRun()
        {
            return this.postInitializationBuffer.ToString();
        }

        public override string GetPreInitializationCodeForProcessingRun()
        {
            return string.Empty;
        }

        public override string[] GetReferencesForProcessingRun()
        {
            return new string[0];
        }

        public override void Initialize(ITextTemplatingEngineHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            base.Initialize(host);
            this.templatingHost = host;
        }

        public override bool IsDirectiveSupported(string directiveName)
        {
            return (StringComparer.OrdinalIgnoreCase.Compare(directiveName, "parameter") == 0);
        }

        void IRecognizeHostSpecific.SetProcessingRunIsHostSpecific(bool isHostSpecific)
        {
            this.hostSpecific = isHostSpecific;
        }

        public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
        {
            string str;
            string str2;
            CodeMemberField field;
            if (!this.IsDirectiveSupported(directiveName))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.DirectiveNotSupported, new object[] { directiveName }), "directiveName");
            }
            if (!arguments.TryGetValue("name", out str))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.DirectiveMissingArgument, new object[] { directiveName, "name" }), "arguments");
            }
            if (!arguments.TryGetValue("type", out str2))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.DirectiveMissingArgument, new object[] { directiveName, "type" }), "arguments");
            }
            this.GenerateClassCode(str, str2, out field);
            this.GeneratePostInitCode(str, str2, field);
        }

        public override void StartProcessingRun(CodeDomProvider languageProvider, string templateContents, CompilerErrorCollection errors)
        {
            if (languageProvider == null)
            {
                throw new ArgumentNullException("languageProvider");
            }
            base.StartProcessingRun(languageProvider, templateContents, errors);
            this.languageCodeDomProvider = languageProvider;
            this.postInitializationBuffer = new StringBuilder();
            this.codeBuffer = new StringBuilder();
        }

        private ITextTemplatingEngineHost Host
        {
            [DebuggerStepThrough]
            get
            {
                return this.templatingHost;
            }
        }

        public bool RequiresProcessingRunIsHostSpecific
        {
            get
            {
                return false;
            }
        }

        private static CodeGeneratorOptions StandardOptions
        {
            get
            {
                return new CodeGeneratorOptions { BlankLinesBetweenMembers = true, IndentString = "    ", VerbatimOrder = true, BracingStyle = "C" };
            }
        }
    }
}

