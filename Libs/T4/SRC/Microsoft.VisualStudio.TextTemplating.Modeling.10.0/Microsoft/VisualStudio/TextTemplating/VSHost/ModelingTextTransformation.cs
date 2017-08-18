namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Modeling;
    using Microsoft.VisualStudio.Modeling.Validation;
    using Microsoft.VisualStudio.TextTemplating;
    using Microsoft.VisualStudio.TextTemplating.Modeling.Properties;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Text;

    public abstract class ModelingTextTransformation : TextTransformation
    {
        private static Guid currentTemplateSessionId = Guid.NewGuid();
        private List<Type> domainModels = new List<Type>();
        private static ITextTemplatingSession session;
        private bool skipValidation;
        private Microsoft.VisualStudio.Modeling.Store store;

        protected void AddDomainModel(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }
            if (!this.domainModels.Contains(modelType))
            {
                this.domainModels.Add(modelType);
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.UnmanagedCode)]
        protected static string ConvertModelRelativePathToTemplateRelativePath(string modelPath, string templatePath, string path)
        {
            if (!File.Exists(modelPath))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.Modeling.Properties.Resources.ModelingFileNotFound, new object[] { modelPath }), "modelPath");
            }
            if (!File.Exists(templatePath))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.Modeling.Properties.Resources.ModelingFileNotFound, new object[] { templatePath }), "templatePath");
            }
            string fullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(modelPath), path));
            StringBuilder pszPath = new StringBuilder(260);
            templatePath = Path.GetFullPath(templatePath);
            if (!NativeMethods.PathRelativePathTo(pszPath, templatePath, 0, fullPath, 0))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Microsoft.VisualStudio.TextTemplating.Modeling.Properties.Resources.ModelingRelativePathFailed, new object[] { path }));
            }
            return pszPath.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (this.store != null)
                {
                    this.store.Dispose();
                    this.store = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            if (!base.Errors.HasErrors)
            {
                IServiceProvider serviceProvider = this.ServiceProvider;
                if (serviceProvider != null)
                {
                    this.store = new Microsoft.VisualStudio.Modeling.Store(serviceProvider, new Type[0]);
                }
                else
                {
                    this.store = new Microsoft.VisualStudio.Modeling.Store(new Type[0]);
                }
                Type[] domainModelTypes = this.domainModels.ToArray();
                this.Store.LoadDomainModels(domainModelTypes);
            }
        }

        protected virtual void OnSessionChanged(ITextTemplatingSession oldSession, ITextTemplatingSession newSession)
        {
        }

        protected bool ValidateStore(string categories, CompilerErrorCollection errors)
        {
            if (this.skipValidation)
            {
                return false;
            }
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }
            if (this.Store == null)
            {
                return true;
            }
            if (string.IsNullOrEmpty(categories))
            {
                return true;
            }
            string[] strArray = categories.Split(new char[] { '|' });
            ValidationCategories categories2 = 0;
            List<string> list = new List<string>();
            foreach (string str in strArray)
            {
                string str2 = str.Trim();
                if (!string.IsNullOrEmpty(str2))
                {
                    if (StringComparer.OrdinalIgnoreCase.Compare(str2, "Open") == 0)
                    {
                        categories2 |= ValidationCategories.Open;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Compare(str2, "Load") == 0)
                    {
                        categories2 |= ValidationCategories.Load;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Compare(str2, "Save") == 0)
                    {
                        categories2 |= ValidationCategories.Save;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Compare(str2, "Menu") == 0)
                    {
                        categories2 |= ValidationCategories.Menu;
                    }
                    else if (StringComparer.OrdinalIgnoreCase.Compare(str2, "Custom") == 0)
                    {
                        categories2 |= ValidationCategories.Custom;
                    }
                    else
                    {
                        list.Add(str2);
                    }
                }
            }
            bool flag = false;
            ValidationController controller = new ValidationController();
            if (categories2 != 0)
            {
                controller.Validate(this.store, categories2);
                foreach (ValidationMessage message in controller.ValidationMessages)
                {
                    CompilerError error = new CompilerError(message.File, message.Line, message.Column, message.Code, message.Description) {
                        IsWarning = message.Type != ViolationType.Error
                    };
                    flag |= !error.IsWarning;
                    errors.Add(error);
                }
            }
            if (list.Count > 0)
            {
                controller.ValidateCustom(this.store, list.ToArray());
                foreach (ValidationMessage message2 in controller.ValidationMessages)
                {
                    CompilerError error2 = new CompilerError(message2.File, message2.Line, message2.Column, message2.Code, message2.Description) {
                        IsWarning = message2.Type != ViolationType.Error
                    };
                    flag |= !error2.IsWarning;
                    errors.Add(error2);
                }
            }
            return flag;
        }

        protected virtual IServiceProvider ServiceProvider
        {
            get
            {
                return null;
            }
        }

        public ITextTemplatingSession Session
        {
            get
            {
                return session;
            }
            set
            {
                ITextTemplatingSession oldSession = session;
                session = value;
                if (session == null)
                {
                    if (oldSession != null)
                    {
                        this.OnSessionChanged(oldSession, session);
                    }
                }
                else if (!session.Equals(oldSession))
                {
                    this.OnSessionChanged(oldSession, session);
                }
            }
        }

        public static Guid SessionId
        {
            get
            {
                if (session != null)
                {
                    return session.Id;
                }
                return currentTemplateSessionId;
            }
        }

        protected bool SkipValidation
        {
            get
            {
                return this.skipValidation;
            }
            set
            {
                this.skipValidation = value;
            }
        }

        protected Microsoft.VisualStudio.Modeling.Store Store
        {
            [DebuggerStepThrough]
            get
            {
                return this.store;
            }
        }

        internal static class NativeMethods
        {
            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("shlwapi.dll", CharSet=CharSet.Unicode)]
            internal static extern bool PathRelativePathTo([Out] StringBuilder pszPath, [In] string pszFrom, [In] uint dwAttrFrom, [In] string pszTo, [In] uint dwAttrTo);
        }
    }
}

