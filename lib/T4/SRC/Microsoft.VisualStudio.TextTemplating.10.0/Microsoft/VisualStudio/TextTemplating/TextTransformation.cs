namespace Microsoft.VisualStudio.TextTemplating
{
    using Microsoft.VisualStudio.TextTemplating.CodeDom;
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;

    public abstract class TextTransformation : IDisposable
    {
        private static CodeTypeMemberCollection baseClassMembers;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private CompilerErrorCollection errorsField;
        private StringBuilder generationEnvironmentField;
        private List<int> indentLengthsField;
        private IDictionary<string, object> sessionField;

        protected TextTransformation()
        {
        }

        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.generationEnvironmentField = null;
            this.errorsField = null;
        }

        public void Error(string message)
        {
            CompilerError error = new CompilerError {
                ErrorText = message
            };
            this.Errors.Add(error);
        }

        ~TextTransformation()
        {
            this.Dispose(false);
        }

        public virtual void Initialize()
        {
        }

        public string PopIndent()
        {
            string str = "";
            if (this.indentLengths.Count > 0)
            {
                int num = this.indentLengths[this.indentLengths.Count - 1];
                this.indentLengths.RemoveAt(this.indentLengths.Count - 1);
                if (num > 0)
                {
                    str = this.currentIndentField.Substring(this.currentIndentField.Length - num);
                    this.currentIndentField = this.currentIndentField.Remove(this.currentIndentField.Length - num);
                }
            }
            return str;
        }

        internal static CodeTypeDeclaration ProvideBaseClass(string name)
        {
            CodeTypeDeclaration member = new CodeTypeDeclaration(name);
            member.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Base class"));
            member.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Base class"));
            member.IsPartial = false;
            member.IsClass = true;
            member.AddSummaryComment("Base class for this transformation");
            if (baseClassMembers == null)
            {
                baseClassMembers = ProvideBaseClassMembers();
            }
            member.Members.AddRange(baseClassMembers);
            return member;
        }

        private static CodeTypeMemberCollection ProvideBaseClassMembers()
        {
            CodeTypeMemberCollection members = new CodeTypeMemberCollection();
            CodeMemberField field = new CodeMemberField(typeof(StringBuilder), "generationEnvironmentField");
            field.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Fields"));
            field.Type.Options = CodeTypeReferenceOptions.GlobalReference;
            members.Add(field);
            CodeMemberField field2 = new CodeMemberField(typeof(CompilerErrorCollection), "errorsField") {
                Type = { Options = CodeTypeReferenceOptions.GlobalReference }
            };
            members.Add(field2);
            CodeMemberField field3 = new CodeMemberField(typeof(List<int>), "indentLengthsField") {
                Type = { Options = CodeTypeReferenceOptions.GlobalReference }
            };
            members.Add(field3);
            CodeMemberField field4 = new CodeMemberField(typeof(string), "currentIndentField") {
                Type = { Options = CodeTypeReferenceOptions.GlobalReference },
                InitExpression = string.Empty.Prim()
            };
            members.Add(field4);
            CodeMemberField field5 = new CodeMemberField(typeof(bool), "endsWithNewline") {
                Type = { Options = CodeTypeReferenceOptions.GlobalReference }
            };
            members.Add(field5);
            CodeMemberField field6 = new CodeMemberField(typeof(IDictionary<string, object>), "sessionField") {
                Type = { Options = CodeTypeReferenceOptions.GlobalReference }
            };
            field6.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Fields"));
            members.Add(field6);
            CodeMemberProperty generationEnvironment = ProvideGenerationEnvironmentProperty(members, field);
            CodeMemberProperty errors = ProvideErrorsProperty(members, field2);
            CodeMemberProperty indentLengths = ProvideIndentLengthsProperty(members, field3);
            ProvideCurrentIndentProperty(members, field4);
            ProvideSessionProperty(members, field6);
            CodeVariableReferenceExpression textToAppend = new CodeVariableReferenceExpression("textToAppend");
            CodeParameterDeclarationExpression argsParam = new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(object[])), "args");
            CodeVariableReferenceExpression error = new CodeVariableReferenceExpression("error");
            ProvideWriteMethod1(members, field4, field5, generationEnvironment, textToAppend);
            ProvideWriteLineMethod1(members, field5, generationEnvironment, textToAppend);
            ProvideWriteMethod2(members, argsParam);
            ProvideWriteLineMethod2(members, argsParam);
            ProvideErrorMethod(members, errors, error);
            ProvideWarningMethod(members, errors, error);
            ProvidePushIndentMethod(members, field4, indentLengths);
            ProvidePopIndentMethod(members, field4, indentLengths);
            ProvideClearIndentMethod(members, field4, indentLengths);
            return members;
        }

        private static void ProvideClearIndentMethod(CodeTypeMemberCollection members, CodeMemberField currentIndent, CodeMemberProperty indentLengths)
        {
            CodeMemberMethod method = CodeDomHelpers.CreateMethod(null, "ClearIndent", "Remove any indentation", MemberAttributes.Public | MemberAttributes.Final, new CodeObject[] { indentLengths.Ref().CallS("Clear", new CodeExpression[0]), currentIndent.Ref().Assign(string.Empty.Prim()) });
            method.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Transform-time helpers"));
            members.Add(method);
        }

        private static void ProvideCurrentIndentProperty(CodeTypeMemberCollection members, CodeMemberField currentIndent)
        {
            CodeMemberProperty member = new CodeMemberProperty {
                Type = new CodeTypeReference(typeof(string)),
                Name = "CurrentIndent"
            };
            member.AddSummaryComment("Gets the current indent we use when adding lines to the output");
            member.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            member.GetStatements.Add(new CodeMethodReturnStatement(currentIndent.Ref()));
            members.Add(member);
        }

        private static void ProvideErrorMethod(CodeTypeMemberCollection members, CodeMemberProperty Errors, CodeVariableReferenceExpression error)
        {
            CodeMemberMethod method = CodeDomHelpers.CreateMethod(null, "Error", "Raise an error", MemberAttributes.Public | MemberAttributes.Final, new CodeObject[] { new CodeVariableDeclarationStatement(typeof(CompilerError), "error", typeof(CompilerError).New()), error.Prop("ErrorText").Assign(new CodeVariableReferenceExpression("message")), Errors.Ref().CallS("Add", new CodeExpression[] { error }) });
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), "message"));
            members.Add(method);
        }

        private static CodeMemberProperty ProvideErrorsProperty(CodeTypeMemberCollection members, CodeMemberField errors)
        {
            CodeMemberProperty member = new CodeMemberProperty {
                Type = new CodeTypeReference(typeof(CompilerErrorCollection)),
                Name = "Errors"
            };
            member.AddSummaryComment("The error collection for the generation process");
            member.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            member.GetStatements.Add(CodeDomHelpers.IfVariableNullThenInstantiateType(errors.Ref(), typeof(CompilerErrorCollection)));
            member.GetStatements.Add(new CodeMethodReturnStatement(errors.Ref()));
            members.Add(member);
            return member;
        }

        private static CodeMemberProperty ProvideGenerationEnvironmentProperty(CodeTypeMemberCollection members, CodeMemberField generationTimeBuilder)
        {
            CodeMemberProperty member = new CodeMemberProperty();
            member.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Properties"));
            member.Type = new CodeTypeReference(typeof(StringBuilder));
            member.Name = "GenerationEnvironment";
            member.AddSummaryComment("The string builder that generation-time code is using to assemble generated output");
            member.Attributes = MemberAttributes.Family | MemberAttributes.Final;
            member.GetStatements.Add(CodeDomHelpers.IfVariableNullThenInstantiateType(generationTimeBuilder.Ref(), typeof(StringBuilder)));
            member.GetStatements.Add(new CodeMethodReturnStatement(generationTimeBuilder.Ref()));
            member.SetStatements.Add(new CodeAssignStatement(generationTimeBuilder.Ref(), new CodePropertySetValueReferenceExpression()));
            members.Add(member);
            return member;
        }

        private static CodeMemberProperty ProvideIndentLengthsProperty(CodeTypeMemberCollection members, CodeMemberField indentLengthsField)
        {
            CodeMemberProperty member = new CodeMemberProperty {
                Type = new CodeTypeReference(typeof(List<int>)),
                Name = "indentLengths"
            };
            member.AddSummaryComment("A list of the lengths of each indent that was added with PushIndent");
            member.Attributes = MemberAttributes.Private;
            member.GetStatements.Add(CodeDomHelpers.IfVariableNullThenInstantiateType(indentLengthsField.Ref(), typeof(List<int>)));
            member.GetStatements.Add(new CodeMethodReturnStatement(indentLengthsField.Ref()));
            members.Add(member);
            return member;
        }

        private static void ProvidePopIndentMethod(CodeTypeMemberCollection members, CodeMemberField currentIndent, CodeMemberProperty indentLengths)
        {
            CodeVariableReferenceExpression lhs = new CodeVariableReferenceExpression("indentLength");
            CodeVariableReferenceExpression expression2 = new CodeVariableReferenceExpression("returnValue");
            CodeMemberMethod method = CodeDomHelpers.CreateMethod(typeof(string), "PopIndent", "Remove the last indent that was added with PushIndent", MemberAttributes.Public | MemberAttributes.Final, new CodeObject[] { new CodeVariableDeclarationStatement(typeof(string), expression2.VariableName, string.Empty.Prim()), new CodeConditionStatement(indentLengths.Ref().Prop("Count").Gt(0.Prim()), new CodeStatement[] { new CodeVariableDeclarationStatement(typeof(int), lhs.VariableName, indentLengths.Ref().Index(new CodeExpression[] { indentLengths.Ref().Prop("Count").Subtract(1.Prim()) })), indentLengths.Ref().CallS("RemoveAt", new CodeExpression[] { indentLengths.Ref().Prop("Count").Subtract(1.Prim()) }), new CodeConditionStatement(lhs.Gt(0.Prim()), new CodeStatement[] { expression2.Assign(currentIndent.Ref().Call("Substring", new CodeExpression[] { currentIndent.Ref().Prop("Length").Subtract(lhs) })), currentIndent.Ref().Assign(currentIndent.Ref().Call("Remove", new CodeExpression[] { currentIndent.Ref().Prop("Length").Subtract(lhs) })) }) }), new CodeMethodReturnStatement(expression2) });
            members.Add(method);
        }

        private static void ProvidePushIndentMethod(CodeTypeMemberCollection members, CodeMemberField currentIndent, CodeMemberProperty indentLengths)
        {
            CodeVariableReferenceExpression lhs = new CodeVariableReferenceExpression("indent");
            CodeMemberMethod method = CodeDomHelpers.CreateMethod(null, "PushIndent", "Increase the indent", MemberAttributes.Public | MemberAttributes.Final, new CodeObject[] { new CodeConditionStatement(lhs.VEquals(CodeDomHelpers.nullEx), new CodeStatement[] { new CodeThrowExceptionStatement(typeof(ArgumentNullException).New(new CodeExpression[] { "indent".Prim() })) }), currentIndent.Ref().Assign(currentIndent.Ref().Add(lhs)), indentLengths.Ref().CallS("Add", new CodeExpression[] { lhs.Prop("Length") }) });
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), "indent"));
            members.Add(method);
        }

        private static void ProvideSessionProperty(CodeTypeMemberCollection members, CodeMemberField session)
        {
            CodeMemberProperty member = new CodeMemberProperty {
                Type = new CodeTypeReference(typeof(IDictionary<string, object>))
            };
            member.Type.Options = CodeTypeReferenceOptions.GlobalReference;
            member.Name = "Session";
            member.AddSummaryComment("Current transformation session");
            member.Attributes = MemberAttributes.Public;
            member.GetStatements.Add(new CodeMethodReturnStatement(session.Ref()));
            member.SetStatements.Add(session.Ref().Assign(new CodePropertySetValueReferenceExpression()));
            member.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, "Properties"));
            members.Add(member);
        }

        private static void ProvideWarningMethod(CodeTypeMemberCollection members, CodeMemberProperty Errors, CodeVariableReferenceExpression error)
        {
            CodeMemberMethod method = CodeDomHelpers.CreateMethod(null, "Warning", "Raise a warning", MemberAttributes.Public | MemberAttributes.Final, new CodeObject[] { new CodeVariableDeclarationStatement(typeof(CompilerError), "error", typeof(CompilerError).New()), error.Prop("ErrorText").Assign(new CodeVariableReferenceExpression("message")), error.Prop("IsWarning").Assign(true.Prim()), Errors.Ref().CallS("Add", new CodeExpression[] { error }) });
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), "message"));
            members.Add(method);
        }

        private static void ProvideWriteLineMethod1(CodeTypeMemberCollection members, CodeMemberField endsWithNewline, CodeMemberProperty GenerationEnvironment, CodeVariableReferenceExpression textToAppend)
        {
            CodeMemberMethod method = CodeDomHelpers.CreateMethod(null, "WriteLine", "Write text directly into the generated output", MemberAttributes.Public | MemberAttributes.Final, new CodeObject[] { CodeDomHelpers.Call("Write", new CodeExpression[] { textToAppend }), GenerationEnvironment.Ref().CallS("AppendLine", new CodeExpression[0]), endsWithNewline.Ref().Assign(true.Prim()) });
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), textToAppend.VariableName));
            members.Add(method);
        }

        private static void ProvideWriteLineMethod2(CodeTypeMemberCollection members, CodeParameterDeclarationExpression argsParam)
        {
            CodeMemberMethod method = CodeDomHelpers.CreateMethod(null, "WriteLine", "Write formatted text directly into the generated output", MemberAttributes.Public | MemberAttributes.Final, new CodeObject[] { CodeDomHelpers.Call("WriteLine", new CodeExpression[] { CodeDomHelpers.Call(typeof(string), "Format", new CodeExpression[] { typeof(CultureInfo).Expr().Prop("CurrentCulture"), new CodeVariableReferenceExpression("format"), new CodeVariableReferenceExpression("args") }) }) });
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), "format"));
            method.Parameters.Add(argsParam);
            members.Add(method);
        }

        private static void ProvideWriteMethod1(CodeTypeMemberCollection members, CodeMemberField currentIndent, CodeMemberField endsWithNewline, CodeMemberProperty GenerationEnvironment, CodeVariableReferenceExpression textToAppend)
        {
            CodeExpression lhs = typeof(Environment).Expr().Prop("NewLine");
            CodeMemberMethod method = CodeDomHelpers.CreateMethod(null, "Write", "Write text directly into the generated output", MemberAttributes.Public | MemberAttributes.Final, new CodeObject[] { new CodeConditionStatement(CodeDomHelpers.Call(typeof(string), "IsNullOrEmpty", new CodeExpression[] { textToAppend }), new CodeStatement[] { new CodeMethodReturnStatement() }), new CodeCommentStatement("If we're starting off, or if the previous text ended with a newline,"), new CodeCommentStatement("we have to append the current indent first."), new CodeConditionStatement(new CodeBinaryOperatorExpression(GenerationEnvironment.Ref().Prop("Length").VEquals(0.Prim()), CodeBinaryOperatorType.BooleanOr, endsWithNewline.Ref()), new CodeStatement[] { GenerationEnvironment.Ref().CallS("Append", new CodeExpression[] { currentIndent.Ref() }), endsWithNewline.Ref().Assign(false.Prim()) }), new CodeCommentStatement("Check if the current text ends with a newline"), new CodeConditionStatement(textToAppend.Call("EndsWith", new CodeExpression[] { lhs, typeof(StringComparison).Expr().Prop("CurrentCulture") }), new CodeStatement[] { new CodeAssignStatement(endsWithNewline.Ref(), true.Prim()) }), new CodeCommentStatement("This is an optimization. If the current indent is \"\", then we don't have to do any"), new CodeCommentStatement("of the more complex stuff further down."), new CodeConditionStatement(currentIndent.Ref().Prop("Length").VEquals(0.Prim()), new CodeStatement[] { GenerationEnvironment.Ref().CallS("Append", new CodeExpression[] { textToAppend }), new CodeMethodReturnStatement() }), new CodeCommentStatement("Everywhere there is a newline in the text, add an indent after it"), textToAppend.Assign(textToAppend.Call("Replace", new CodeExpression[] { lhs, lhs.Add(currentIndent.Ref()) })), new CodeCommentStatement("If the text ends with a newline, then we should strip off the indent added at the very end"), new CodeCommentStatement("because the appropriate indent will be added when the next time Write() is called"), new CodeConditionStatement(endsWithNewline.Ref(), new CodeStatement[] { GenerationEnvironment.Ref().CallS("Append", new CodeExpression[] { textToAppend, 0.Prim(), textToAppend.Prop("Length").Subtract(currentIndent.Ref().Prop("Length")) }) }, new CodeStatement[] { GenerationEnvironment.Ref().CallS("Append", new CodeExpression[] { textToAppend }) }) });
            method.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Transform-time helpers"));
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), textToAppend.VariableName));
            members.Add(method);
        }

        private static void ProvideWriteMethod2(CodeTypeMemberCollection members, CodeParameterDeclarationExpression argsParam)
        {
            CodeMemberMethod method = CodeDomHelpers.CreateMethod(null, "Write", "Write formatted text directly into the generated output", MemberAttributes.Public | MemberAttributes.Final, new CodeObject[] { CodeDomHelpers.Call("Write", new CodeExpression[] { CodeDomHelpers.Call(typeof(string), "Format", new CodeExpression[] { typeof(CultureInfo).Expr().Prop("CurrentCulture"), new CodeVariableReferenceExpression("format"), new CodeVariableReferenceExpression("args") }) }) });
            method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(string)), "format"));
            argsParam.CustomAttributes.Add(new CodeAttributeDeclaration("System.ParamArrayAttribute"));
            method.Parameters.Add(argsParam);
            members.Add(method);
        }

        public void PushIndent(string indent)
        {
            if (indent == null)
            {
                throw new ArgumentNullException("indent");
            }
            this.currentIndentField = this.currentIndentField + indent;
            this.indentLengths.Add(indent.Length);
        }

        public abstract string TransformText();
        public void Warning(string message)
        {
            CompilerError error = new CompilerError {
                ErrorText = message,
                IsWarning = true
            };
            this.Errors.Add(error);
        }

        public void Write(string textToAppend)
        {
            if (!string.IsNullOrEmpty(textToAppend))
            {
                if ((this.GenerationEnvironment.Length == 0) || this.endsWithNewline)
                {
                    this.GenerationEnvironment.Append(this.currentIndentField);
                    this.endsWithNewline = false;
                }
                if (textToAppend.EndsWith(Environment.NewLine, StringComparison.CurrentCulture))
                {
                    this.endsWithNewline = true;
                }
                if (this.currentIndentField.Length == 0)
                {
                    this.GenerationEnvironment.Append(textToAppend);
                }
                else
                {
                    textToAppend = textToAppend.Replace(Environment.NewLine, Environment.NewLine + this.currentIndentField);
                    if (this.endsWithNewline)
                    {
                        this.GenerationEnvironment.Append(textToAppend, 0, textToAppend.Length - this.currentIndentField.Length);
                    }
                    else
                    {
                        this.GenerationEnvironment.Append(textToAppend);
                    }
                }
            }
        }

        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }

        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(CultureInfo.CurrentCulture, format, args));
        }

        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }

        public CompilerErrorCollection Errors
        {
            [DebuggerStepThrough]
            get
            {
                if (this.errorsField == null)
                {
                    this.errorsField = new CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }

        protected StringBuilder GenerationEnvironment
        {
            [DebuggerStepThrough]
            get
            {
                if (this.generationEnvironmentField == null)
                {
                    this.generationEnvironmentField = new StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            [DebuggerStepThrough]
            set
            {
                this.generationEnvironmentField = value;
            }
        }

        private List<int> indentLengths
        {
            get
            {
                if (this.indentLengthsField == null)
                {
                    this.indentLengthsField = new List<int>();
                }
                return this.indentLengthsField;
            }
        }

        public virtual IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
    }
}

