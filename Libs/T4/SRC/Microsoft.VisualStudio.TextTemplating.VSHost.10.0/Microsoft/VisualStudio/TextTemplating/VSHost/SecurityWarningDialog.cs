namespace Microsoft.VisualStudio.TextTemplating.VSHost
{
    using Microsoft.VisualStudio.Modeling.Shell;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal sealed class SecurityWarningDialog : DialogBase
    {
        private Button cancelButton;
        private IContainer components;
        private CheckBox doNotShowCheckBox;
        private Button okButton;
        private TableLayoutPanel tableLayoutPanel3;
        private Label Warning;

        private SecurityWarningDialog(IServiceProvider provider) : base(provider)
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SecurityWarningDialog));
            this.Warning = new Label();
            this.doNotShowCheckBox = new CheckBox();
            this.okButton = new Button();
            this.cancelButton = new Button();
            this.tableLayoutPanel3 = new TableLayoutPanel();
            this.tableLayoutPanel3.SuspendLayout();
            base.SuspendLayout();
            manager.ApplyResources(this.Warning, "Warning");
            this.Warning.CausesValidation = false;
            this.tableLayoutPanel3.SetColumnSpan(this.Warning, 3);
            this.Warning.MinimumSize = new Size(0x144, 0x4c);
            this.Warning.Name = "Warning";
            manager.ApplyResources(this.doNotShowCheckBox, "doNotShowCheckBox");
            this.tableLayoutPanel3.SetColumnSpan(this.doNotShowCheckBox, 3);
            this.doNotShowCheckBox.MinimumSize = new Size(0xb3, 0x11);
            this.doNotShowCheckBox.Name = "doNotShowCheckBox";
            this.doNotShowCheckBox.UseVisualStyleBackColor = true;
            this.okButton.DialogResult = DialogResult.OK;
            manager.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.cancelButton.DialogResult = DialogResult.Cancel;
            manager.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            manager.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.okButton, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.Warning, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cancelButton, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.doNotShowCheckBox, 0, 1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            base.AcceptButton = this.okButton;
            manager.ApplyResources(this, "$this");
            base.AccessibleRole = AccessibleRole.Dialog;
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.cancelButton;
            base.Controls.Add(this.tableLayoutPanel3);
            base.HelpButton = true;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SecurityWarningDialog";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            base.ResumeLayout(false);
        }

        internal static void ShowSecurityWarningDialog(IServiceProvider provider, out bool cancel, out bool showAgain)
        {
            cancel = true;
            showAgain = true;
            IUIService service = provider.GetService(typeof(IUIService)) as IUIService;
            if (service != null)
            {
                SecurityWarningDialog form = new SecurityWarningDialog(provider);
                if (service.ShowDialog(form) == DialogResult.OK)
                {
                    cancel = false;
                    showAgain = !form.doNotShowCheckBox.Checked;
                }
            }
        }

        protected override string F1Keyword
        {
            get
            {
                return "vs.dsl.t4.securitywarning";
            }
        }
    }
}

