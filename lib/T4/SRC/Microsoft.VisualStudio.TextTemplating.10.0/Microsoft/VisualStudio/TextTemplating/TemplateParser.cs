namespace Microsoft.VisualStudio.TextTemplating
{
    using Microsoft.VisualStudio.TextTemplating.Properties;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    internal static class TemplateParser
    {
        private static Regex allNewlineRegex = new Regex("^(" + Environment.NewLine + ")*$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex directiveEscapeFindingRegex = new Regex("\\\\+(?=\")|\\\\+(?=$)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static Regex directiveParsingRegex = new Regex("(?<pname>\\S+?)\\s*=\\s*\"(?<pvalue>.*?)(?<=[^\\\\](\\\\\\\\)*)\"|(?<name>\\S+)", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static Regex eolEscapeFindingRegex = new Regex(@"\\+(?=$)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static Regex escapeFindingRegex = new Regex(@"\\+(?=<\#)|\\+(?=\#>)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static MatchEvaluator escapeReplacingEvaluator;
        private static Regex nameValidatingRegex = new Regex(@"^\s*[\w\.]+\s+", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static Regex newlineAtLineStartRegex = new Regex("^" + Environment.NewLine, RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex newlineFindingRegex = new Regex(Environment.NewLine, RegexOptions.Singleline | RegexOptions.Compiled);
        private static Regex paramValueValidatingRegex = new Regex("[\\w\\.]+\\s*=\\s*\"(.*?)(?<=[^\\\\](\\\\\\\\)*)\"\\s*", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static Regex templateParsingRegex = new Regex("\r\n\t\t\t# We check if we have an even number of \\ in the beginning of\t#\r\n\t\t\t# of the file preceeding an open tag. If we do, then it's\t\t#\r\n\t\t\t# boilerplate text. We check this in the very beginning because\t#\r\n\t\t\t# if not, the directives/classfeatures etc will match it and we\t#\r\n\t\t\t# won't get the initial backslashes as boilerplate code\t\t\t#\r\n\t\t\t(?<boilerplate>^(\\\\\\\\)+)(?=<\\#)|\r\n\r\n\t\t\t# Check for an unescaped (0 or even number of \\ preceeding\t\t#\r\n\t\t\t# it) directive start tag and it's accompanying end tag. Store\t#\r\n\t\t\t# text of the directive tag in a group named directive\t\t\t#\r\n\t\t\t(?<=([^\\\\]|^)(\\\\\\\\)*)<\\#@(?<directive>.*?)(?<=[^\\\\](\\\\\\\\)*)\\#>|\r\n\r\n\t\t\t# Check for an unescaped classfeature start tag and its end tag\t#\r\n\t\t\t# Store the text between the tags in group called classfeatures\t#\r\n\t\t\t(?<=([^\\\\]|^)(\\\\\\\\)*)<\\#\\+(?<classfeature>.*?)(?<=[^\\\\](\\\\\\\\)*)\\#>|\r\n\r\n\t\t\t# Check for an unescaped expression start tag and its end tag.\t#\r\n\t\t\t# Store the text between the tags in group called expression\t#\r\n\t\t\t(?<=([^\\\\]|^)(\\\\\\\\)*)<\\#=(?<expression>.*?)(?<=[^\\\\](\\\\\\\\)*)\\#>|\r\n\r\n\t\t\t# Check for an unescaped statement start tag and its end tag.\t#\r\n\t\t\t# Store the text between the tags in group called statement.\t#\r\n\t\t\t# We can only check for statements after checking for\t\t\t#\r\n\t\t\t# directives, expressions and classfeatures because the start\t#\r\n\t\t\t# tag for statements is a substring of the other start tags\t\t#\r\n\t\t\t(?<=([^\\\\]|^)(\\\\\\\\)*)<\\#(?<statement>.*?)(?<=[^\\\\](\\\\\\\\)*)\\#>|\r\n\r\n\t\t\t# Finally, check for boilerplate code that's not within a start\t#\r\n\t\t\t# or end tag (look for anything preceeding a start tag or an\t#\r\n\t\t\t# EOL) This has to be done at the ver end so that the .+ does\t#\r\n\t\t\t# not match other blocks\t\t\t\t\t\t\t\t\t\t#\r\n\t\t\t(?<boilerplate>.+?)(?=((?<=[^\\\\](\\\\\\\\)*)<\\#))|\r\n\t\t\t(?<boilerplate>.+)(?=$)", RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static Regex unescapedTagFindingRegex = new Regex(@"(^|[^\\])(\\\\)*(<\#|\#>)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        static TemplateParser()
        {
            escapeReplacingEvaluator = delegate (Match match) {
                if (match.Success && (match.Value != null))
                {
                    int length = (int) Math.Floor((double) (((double) match.Value.Length) / 2.0));
                    return match.Value.Substring(0, length);
                }
                return string.Empty;
            };
        }

        private static void CheckBlockSequence(List<Block> blocks, CompilerErrorCollection errors)
        {
            bool flag = false;
            bool flag2 = false;
            foreach (Block block in blocks)
            {
                if (!flag)
                {
                    if (block.Type == BlockType.ClassFeature)
                    {
                        flag = true;
                    }
                }
                else if ((block.Type == BlockType.Directive) || (block.Type == BlockType.Statement))
                {
                    CompilerError error = new CompilerError(block.FileName, block.StartLineNumber, block.StartColumnNumber, null, string.Format(CultureInfo.CurrentCulture, Resources.WrongBlockSequence, new object[] { Enum.GetName(typeof(BlockType), block.Type) })) {
                        IsWarning = false
                    };
                    errors.Add(error);
                    flag2 = true;
                }
            }
            if (flag && !flag2)
            {
                Block block2 = blocks[blocks.Count - 1];
                if ((block2.Type != BlockType.ClassFeature) && ((block2.Type != BlockType.BoilerPlate) || !allNewlineRegex.Match(block2.Text).Success))
                {
                    CompilerError error2 = new CompilerError(block2.FileName, block2.StartLineNumber, block2.StartColumnNumber, null, Resources.WrongFinalBlockType) {
                        IsWarning = false
                    };
                    errors.Add(error2);
                }
            }
        }

        private static void InsertPositionInformation(List<Block> blocks)
        {
            int num = 1;
            int num2 = 1;
            foreach (Block block in blocks)
            {
                if (((block.Type == BlockType.ClassFeature) || (block.Type == BlockType.Directive)) || (block.Type == BlockType.Expression))
                {
                    num2 += 3;
                }
                else if (block.Type == BlockType.Statement)
                {
                    num2 += 2;
                }
                block.StartLineNumber = num;
                block.StartColumnNumber = num2;
                MatchCollection matchs = newlineFindingRegex.Matches(block.Text);
                num += matchs.Count;
                if (matchs.Count > 0)
                {
                    num2 = ((block.Text.Length - matchs[matchs.Count - 1].Index) - Environment.NewLine.Length) + 1;
                }
                else
                {
                    num2 += block.Text.Length;
                }
                block.EndLineNumber = num;
                block.EndColumnNumber = num2;
                if (block.Type != BlockType.BoilerPlate)
                {
                    num2 += 2;
                }
            }
        }

        public static Directive ParseDirectiveBlock(Block block, CompilerErrorCollection errors)
        {
            if (block == null)
            {
                throw new ArgumentNullException("block");
            }
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }
            if (!ValidateDirectiveString(block))
            {
                CompilerError error = new CompilerError(block.FileName, block.StartLineNumber, block.StartColumnNumber, null, Resources.WrongDirectiveFormat) {
                    IsWarning = false
                };
                errors.Add(error);
                return null;
            }
            MatchCollection matchs = directiveParsingRegex.Matches(block.Text);
            string directiveName = null;
            Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (Match match in matchs)
            {
                Group group;
                if ((group = match.Groups["name"]).Success)
                {
                    directiveName = group.Value;
                }
                else
                {
                    string key = null;
                    string valueString = null;
                    if ((group = match.Groups["pname"]).Success)
                    {
                        key = group.Value;
                    }
                    if ((group = match.Groups["pvalue"]).Success)
                    {
                        valueString = group.Value;
                    }
                    if ((key != null) && (valueString != null))
                    {
                        if (parameters.ContainsKey(key))
                        {
                            CompilerError error2 = new CompilerError(block.FileName, block.StartLineNumber, block.StartColumnNumber, null, string.Format(CultureInfo.CurrentCulture, Resources.DuplicateDirectiveParameter, new object[] { key })) {
                                IsWarning = true
                            };
                            errors.Add(error2);
                            continue;
                        }
                        valueString = StripDirectiveEscapeCharacters(valueString);
                        parameters.Add(key, valueString);
                    }
                }
            }
            if (directiveName != null)
            {
                return new Directive(directiveName, parameters, block);
            }
            return null;
        }

        public static List<Block> ParseTemplateIntoBlocks(string content, CompilerErrorCollection errors)
        {
            return ParseTemplateIntoBlocks(content, "", errors);
        }

        public static List<Block> ParseTemplateIntoBlocks(string content, string fileName, CompilerErrorCollection errors)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }
            List<Block> blocks = new List<Block>();
            foreach (Match match in templateParsingRegex.Matches(content))
            {
                Block item = new Block();
                Group group = null;
                if ((group = match.Groups["boilerplate"]).Success)
                {
                    item.Type = BlockType.BoilerPlate;
                }
                else if ((group = match.Groups["directive"]).Success)
                {
                    item.Type = BlockType.Directive;
                }
                else if ((group = match.Groups["classfeature"]).Success)
                {
                    item.Type = BlockType.ClassFeature;
                }
                else if ((group = match.Groups["expression"]).Success)
                {
                    item.Type = BlockType.Expression;
                }
                else if ((group = match.Groups["statement"]).Success)
                {
                    item.Type = BlockType.Statement;
                }
                if ((group != null) && group.Success)
                {
                    item.Text = group.Value;
                    item.FileName = fileName;
                    blocks.Add(item);
                }
            }
            InsertPositionInformation(blocks);
            WarnAboutUnexpectedTags(blocks, errors);
            StripEscapeCharacters(blocks);
            CheckBlockSequence(blocks, errors);
            return blocks;
        }

        private static string StripDirectiveEscapeCharacters(string valueString)
        {
            return directiveEscapeFindingRegex.Replace(valueString, escapeReplacingEvaluator);
        }

        private static void StripEscapeCharacters(List<Block> blocks)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                Block block = blocks[i];
                block.Text = escapeFindingRegex.Replace(block.Text, escapeReplacingEvaluator);
                if (i != (blocks.Count - 1))
                {
                    block.Text = eolEscapeFindingRegex.Replace(block.Text, escapeReplacingEvaluator);
                }
            }
        }

        internal static void StripExtraNewlines(List<Block> blocks)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                Block block = blocks[i];
                if (((block.Type == BlockType.BoilerPlate) && (i > 0)) && ((blocks[i - 1].Type != BlockType.Expression) && (blocks[i - 1].Type != BlockType.BoilerPlate)))
                {
                    block.Text = newlineAtLineStartRegex.Replace(block.Text, string.Empty);
                }
                if ((((block.Type == BlockType.BoilerPlate) && (i > 0)) && (blocks[i - 1].Type == BlockType.ClassFeature)) && ((i == (blocks.Count - 1)) || (blocks[i + 1].Type == BlockType.ClassFeature)))
                {
                    block.Text = allNewlineRegex.Replace(block.Text, string.Empty);
                }
            }
            Predicate<Block> match = b => string.IsNullOrEmpty(b.Text);
            blocks.RemoveAll(match);
        }

        private static bool ValidateDirectiveString(Block block)
        {
            Match match = nameValidatingRegex.Match(block.Text);
            if (!match.Success)
            {
                return false;
            }
            int length = match.Length;
            MatchCollection matchs = paramValueValidatingRegex.Matches(block.Text);
            if (matchs.Count == 0)
            {
                return false;
            }
            foreach (Match match2 in matchs)
            {
                if (match2.Index != length)
                {
                    return false;
                }
                length += match2.Length;
            }
            if (length != block.Text.Length)
            {
                return false;
            }
            return true;
        }

        private static void WarnAboutUnexpectedTags(List<Block> blocks, CompilerErrorCollection errors)
        {
            foreach (Block block in blocks)
            {
                if (unescapedTagFindingRegex.Match(block.Text).Success)
                {
                    CompilerError error = new CompilerError(block.FileName, block.StartLineNumber, block.StartColumnNumber, null, Resources.UnexpectedTag) {
                        IsWarning = false
                    };
                    errors.Add(error);
                }
            }
        }
    }
}

