namespace Microsoft.VisualStudio.TextTemplating
{
    using Microsoft.VisualStudio.TextTemplating.Properties;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal sealed class TransformationRunner : MarshalByRefObject
    {
        private CompilerErrorCollection errors;
        internal const string ExceptionProgressSlot = "TextTemplatingProgress";
        private static HashSet<string> shadowCopyPaths = null;
        private static object shadowCopySync = new object();

        private Assembly AttemptAssemblyLoad(string assemblyName)
        {
            try
            {
                return Assembly.LoadFrom(assemblyName);
            }
            catch (Exception exception)
            {
                if (Engine.IsCriticalException(exception))
                {
                    throw;
                }
                this.LogError(string.Format(CultureInfo.CurrentCulture, Resources.AssemblyLoadError, new object[] { assemblyName }) + string.Format(CultureInfo.CurrentCulture, Resources.Exception, new object[] { exception.ToString() }), false);
                return null;
            }
        }

        private Assembly Compile(string source, string inputFile, IEnumerable<string> references, bool debug, CodeDomProvider provider, string compilerOptions)
        {
            CompilerParameters options = new CompilerParameters();
            foreach (string str in references)
            {
                options.ReferencedAssemblies.Add(str);
            }
            options.WarningLevel = 4;
            if (debug)
            {
                options.GenerateInMemory = false;
                options.IncludeDebugInformation = true;
                options.TempFiles.KeepFiles = true;
            }
            else
            {
                options.GenerateInMemory = true;
                options.IncludeDebugInformation = false;
                options.TempFiles.KeepFiles = false;
            }
            options.CompilerOptions = compilerOptions;
            CompilerResults results = null;
            Assembly compiledAssembly = null;
            try
            {
                results = provider.CompileAssemblyFromSource(options, new string[] { source });
                if (results.Errors.Count > 0)
                {
                    foreach (CompilerError error in results.Errors)
                    {
                        error.ErrorText = Resources.CompilerErrorPrepend + error.ErrorText;
                        if (string.IsNullOrEmpty(error.FileName))
                        {
                            error.FileName = inputFile;
                        }
                    }
                    this.Errors.AddRange(results.Errors);
                }
                if (!results.Errors.HasErrors)
                {
                    compiledAssembly = results.CompiledAssembly;
                }
            }
            catch (Exception exception)
            {
                if (Engine.IsCriticalException(exception))
                {
                    throw;
                }
                this.LogError(Resources.CompilerErrors + string.Format(CultureInfo.CurrentCulture, Resources.Exception, new object[] { exception.ToString() }), false);
            }
            return compiledAssembly;
        }

        private TextTransformation CreateTextTransformation(string fullClassName, ITextTemplatingEngineHost host, Assembly assembly, ITextTemplatingSession userSession)
        {
            object obj2 = null;
            try
            {
                obj2 = assembly.CreateInstance(fullClassName);
                if (obj2 == null)
                {
                    this.LogError(Resources.ExceptionInstantiatingTransformationObject, false);
                    return null;
                }
                Type transformationType = obj2.GetType();
                if (host != null)
                {
                    try
                    {
                        transformationType.GetProperty("Host").SetValue(obj2, host, null);
                    }
                    catch (Exception exception)
                    {
                        if (Engine.IsCriticalException(exception))
                        {
                            throw;
                        }
                        this.LogError(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionSettingHost, new object[] { transformationType.FullName }), false);
                    }
                }
                try
                {
                    PropertyInfo mostDerivedProperty = GetMostDerivedProperty(transformationType, "Session");
                    if (mostDerivedProperty != null)
                    {
                        mostDerivedProperty.SetValue(obj2, userSession, null);
                    }
                }
                catch (Exception exception2)
                {
                    if (Engine.IsCriticalException(exception2))
                    {
                        throw;
                    }
                    this.LogError(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionSettingSession, new object[] { transformationType.FullName }), false);
                }
                return (TextTransformation) obj2;
            }
            catch (Exception exception3)
            {
                if (Engine.IsCriticalException(exception3))
                {
                    IDisposable disposable = obj2 as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                    throw;
                }
                this.LogError(Resources.ExceptionInstantiatingTransformationObject + string.Format(CultureInfo.CurrentCulture, Resources.Exception, new object[] { exception3.ToString() }), false);
            }
            return null;
        }

        private static void EnsureShadowCopyPaths(IEnumerable<string> paths)
        {
            if (AppDomain.CurrentDomain.ShadowCopyFiles)
            {
                string path = string.Empty;
                lock (shadowCopySync)
                {
                    if (shadowCopyPaths == null)
                    {
                        shadowCopyPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    }
                    foreach (string str2 in paths)
                    {
                        if (!shadowCopyPaths.Contains(str2))
                        {
                            shadowCopyPaths.Add(str2);
                        }
                    }
                    path = string.Join(";", shadowCopyPaths.ToArray<string>());
                }
                AppDomain.CurrentDomain.SetShadowCopyPath(path);
            }
        }

        private static PropertyInfo GetMostDerivedProperty(Type transformationType, string propertyName)
        {
            PropertyInfo property = null;
            while ((transformationType != typeof(object)) && (transformationType != null))
            {
                property = transformationType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (property != null)
                {
                    return property;
                }
                transformationType = transformationType.BaseType;
            }
            return null;
        }

        private void LoadExplicitAssemblyReferences(IEnumerable<string> references)
        {
            references = (from referenceAssembly in references
                where !string.IsNullOrEmpty(referenceAssembly) && File.Exists(referenceAssembly)
                select referenceAssembly).ToList<string>();
            List<string> source = new List<string>();
            if (AppDomain.CurrentDomain.ShadowCopyFiles)
            {
                foreach (string str in references)
                {
                    string directoryName = Path.GetDirectoryName(str);
                    if (string.IsNullOrEmpty(directoryName))
                    {
                        string currentDirectory = Directory.GetCurrentDirectory();
                        if (File.Exists(Path.Combine(currentDirectory, str)))
                        {
                            directoryName = currentDirectory;
                        }
                    }
                    source.Add(directoryName);
                }
                EnsureShadowCopyPaths(source.Distinct<string>(StringComparer.OrdinalIgnoreCase));
            }
            foreach (string str4 in references)
            {
                DateTime lastWriteTime = new DateTime();
                if (AppDomain.CurrentDomain.ShadowCopyFiles && File.Exists(str4))
                {
                    FileInfo info = new FileInfo(str4);
                    lastWriteTime = info.LastWriteTime;
                }
                Assembly assembly = this.AttemptAssemblyLoad(str4);
                if (((assembly != null) && AppDomain.CurrentDomain.ShadowCopyFiles) && !assembly.GlobalAssemblyCache)
                {
                    ShadowTimes.Insert(str4, lastWriteTime);
                }
            }
        }

        private Assembly LocateAssembly(bool cacheAssemblies, string fullClassName, string source, string inputFile, bool debug, CodeDomProvider provider, IEnumerable<string> compilerReferences, string compilerOptions)
        {
            Assembly assembly = null;
            if (cacheAssemblies)
            {
                assembly = AssemblyCache.Find(fullClassName);
            }
            if (assembly == null)
            {
                assembly = this.Compile(source, inputFile, compilerReferences, debug, provider, compilerOptions);
                if ((assembly != null) && cacheAssemblies)
                {
                    AssemblyCache.Insert(fullClassName, assembly);
                }
            }
            return assembly;
        }

        private void LogError(string errorText, bool isWarning)
        {
            CompilerError error = new CompilerError {
                ErrorText = errorText,
                IsWarning = isWarning
            };
            this.Errors.Add(error);
        }

        public void PreLoadAssemblies(string[] assemblies)
        {
            try
            {
                this.LoadExplicitAssemblyReferences(assemblies);
            }
            catch (Exception exception)
            {
                if (Engine.IsCriticalException(exception))
                {
                    throw;
                }
                this.LogError(Resources.ExceptionWhileRunningCode + string.Format(CultureInfo.CurrentCulture, Resources.Exception, new object[] { exception.ToString() }), false);
            }
        }

        public void RunTransformation(TemplateProcessingSession session, string source, ITextTemplatingEngineHost host, out string result)
        {
            ToStringHelper.FormatProvider = session.FormatProvider;
            CodeDomProvider codeDomProvider = session.CodeDomProvider;
            string errorOutput = Resources.ErrorOutput;
            bool validBaseClass = string.IsNullOrEmpty(session.BaseClassName);
            Assembly assembly = null;
            try
            {
                if (this.ValidateBaseClass(session.BaseClassName, session.ImportDirectives, validBaseClass))
                {
                    session.AssemblyDirectives.Add(base.GetType().Assembly.Location);
                    session.AssemblyDirectives.Add(typeof(ITextTemplatingEngineHost).Assembly.Location);
                    assembly = this.LocateAssembly(session.CacheAssemblies, session.ClassFullName, source, session.TemplateFile, session.Debug, codeDomProvider, session.AssemblyDirectives, session.CompilerOptions);
                }
                if (assembly != null)
                {
                    using (TextTransformation transformation = this.CreateTextTransformation(session.ClassFullName, host, assembly, session.UserTransformationSession))
                    {
                        if (transformation != null)
                        {
                            try
                            {
                                transformation.Initialize();
                            }
                            catch (Exception exception)
                            {
                                if (Engine.IsCriticalException(exception))
                                {
                                    throw;
                                }
                                this.LogError(Resources.ErrorInitializingTransformationObject + string.Format(CultureInfo.CurrentCulture, Resources.Exception, new object[] { exception.ToString() }), false);
                            }
                            if (!transformation.Errors.HasErrors && !this.Errors.HasErrors)
                            {
                                try
                                {
                                    errorOutput = transformation.TransformText();
                                }
                                catch (Exception exception2)
                                {
                                    if (Engine.IsCriticalException(exception2))
                                    {
                                        throw;
                                    }
                                    if (exception2.Data["TextTemplatingProgress"] != null)
                                    {
                                        errorOutput = exception2.Data["TextTemplatingProgress"].ToString();
                                    }
                                    ArgumentNullException exception3 = exception2 as ArgumentNullException;
                                    if ((exception3 != null) && (StringComparer.OrdinalIgnoreCase.Compare(exception3.ParamName, "objectToConvert") == 0))
                                    {
                                        this.LogError(Resources.ExpressionBlockNull + Environment.NewLine + exception3.StackTrace, false);
                                    }
                                    else
                                    {
                                        this.LogError(Resources.TransformationErrorPrepend + exception2.ToString(), false);
                                    }
                                }
                            }
                            foreach (CompilerError error in transformation.Errors)
                            {
                                error.ErrorText = Resources.TransformationErrorPrepend + error.ErrorText;
                            }
                            this.Errors.AddRange(transformation.Errors);
                        }
                    }
                }
            }
            catch (Exception exception4)
            {
                if (Engine.IsCriticalException(exception4))
                {
                    throw;
                }
                this.LogError(Resources.ExceptionWhileRunningCode + string.Format(CultureInfo.CurrentCulture, Resources.Exception, new object[] { exception4.ToString() }), false);
            }
            result = errorOutput;
        }

        private bool ValidateBaseClass(string baseClassName, IList<string> importedNamespaces, bool validBaseClass)
        {
            try
            {
                if (string.IsNullOrEmpty(baseClassName))
                {
                    return validBaseClass;
                }
                string[] strArray = new string[importedNamespaces.Count + 1];
                strArray[0] = baseClassName;
                for (int i = 1; i < strArray.Length; i++)
                {
                    strArray[i] = importedNamespaces[i - 1].Trim() + "." + baseClassName;
                }
                Type type = null;
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (string str in strArray)
                {
                    foreach (Assembly assembly in assemblies)
                    {
                        type = assembly.GetType(str, false, true);
                        if (type != null)
                        {
                            break;
                        }
                    }
                    if (type != null)
                    {
                        break;
                    }
                }
                if (type != null)
                {
                    if (!type.IsSubclassOf(typeof(TextTransformation)))
                    {
                        this.LogError(Resources.InvalidBaseClass, false);
                        return validBaseClass;
                    }
                    validBaseClass = true;
                    return validBaseClass;
                }
                this.LogError(string.Format(CultureInfo.CurrentCulture, Resources.BaseClassNotFound, new object[] { baseClassName }), false);
            }
            catch (Exception exception)
            {
                if (Engine.IsCriticalException(exception))
                {
                    throw;
                }
                this.LogError(string.Format(CultureInfo.CurrentCulture, Resources.BaseClassNotFound, new object[] { baseClassName }), false);
            }
            return validBaseClass;
        }

        public CompilerErrorCollection Errors
        {
            [DebuggerStepThrough]
            get
            {
                if (this.errors == null)
                {
                    this.errors = new CompilerErrorCollection();
                }
                return this.errors;
            }
        }
    }
}

