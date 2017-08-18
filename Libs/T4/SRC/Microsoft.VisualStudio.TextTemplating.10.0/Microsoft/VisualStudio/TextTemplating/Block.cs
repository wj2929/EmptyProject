namespace Microsoft.VisualStudio.TextTemplating
{
    using System;
    using System.Diagnostics;

    internal sealed class Block
    {
        private int endColumnNumber;
        private int endLineNumber;
        private string fileName;
        private int startColumnNumber;
        private int startLineNumber;
        private string text;
        private BlockType type;

        public Block()
        {
            this.text = "";
            this.fileName = "";
        }

        public Block(BlockType type, string text)
        {
            this.text = "";
            this.fileName = "";
            this.type = type;
            this.text = text;
        }

        public int EndColumnNumber
        {
            [DebuggerStepThrough]
            get
            {
                return this.endColumnNumber;
            }
            [DebuggerStepThrough]
            set
            {
                this.endColumnNumber = value;
            }
        }

        public int EndLineNumber
        {
            [DebuggerStepThrough]
            get
            {
                return this.endLineNumber;
            }
            [DebuggerStepThrough]
            set
            {
                this.endLineNumber = value;
            }
        }

        public string FileName
        {
            [DebuggerStepThrough]
            get
            {
                return this.fileName;
            }
            [DebuggerStepThrough]
            set
            {
                this.fileName = value;
            }
        }

        public int StartColumnNumber
        {
            [DebuggerStepThrough]
            get
            {
                return this.startColumnNumber;
            }
            [DebuggerStepThrough]
            set
            {
                this.startColumnNumber = value;
            }
        }

        public int StartLineNumber
        {
            [DebuggerStepThrough]
            get
            {
                return this.startLineNumber;
            }
            [DebuggerStepThrough]
            set
            {
                this.startLineNumber = value;
            }
        }

        public string Text
        {
            [DebuggerStepThrough]
            get
            {
                return this.text;
            }
            [DebuggerStepThrough]
            set
            {
                this.text = value;
            }
        }

        public BlockType Type
        {
            [DebuggerStepThrough]
            get
            {
                return this.type;
            }
            [DebuggerStepThrough]
            set
            {
                this.type = value;
            }
        }
    }
}

