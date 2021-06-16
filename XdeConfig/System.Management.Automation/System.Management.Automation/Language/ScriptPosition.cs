using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005BC RID: 1468
	public sealed class ScriptPosition : IScriptPosition
	{
		// Token: 0x06003E89 RID: 16009 RVA: 0x0014BC12 File Offset: 0x00149E12
		public ScriptPosition(string scriptName, int scriptLineNumber, int offsetInLine, string line)
		{
			this._scriptName = scriptName;
			this._scriptLineNumber = scriptLineNumber;
			this._offsetInLine = offsetInLine;
			if (string.IsNullOrEmpty(line))
			{
				this._line = string.Empty;
				return;
			}
			this._line = line;
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x0014BC4C File Offset: 0x00149E4C
		public ScriptPosition(string scriptName, int scriptLineNumber, int offsetInLine, string line, string fullScript) : this(scriptName, scriptLineNumber, offsetInLine, line)
		{
			this._fullScript = fullScript;
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x06003E8B RID: 16011 RVA: 0x0014BC61 File Offset: 0x00149E61
		public string File
		{
			get
			{
				return this._scriptName;
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06003E8C RID: 16012 RVA: 0x0014BC69 File Offset: 0x00149E69
		public int LineNumber
		{
			get
			{
				return this._scriptLineNumber;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06003E8D RID: 16013 RVA: 0x0014BC71 File Offset: 0x00149E71
		public int ColumnNumber
		{
			get
			{
				return this._offsetInLine;
			}
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06003E8E RID: 16014 RVA: 0x0014BC79 File Offset: 0x00149E79
		public int Offset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06003E8F RID: 16015 RVA: 0x0014BC7C File Offset: 0x00149E7C
		public string Line
		{
			get
			{
				return this._line;
			}
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x0014BC84 File Offset: 0x00149E84
		public string GetFullScript()
		{
			return this._fullScript;
		}

		// Token: 0x04001F3A RID: 7994
		private readonly int _offsetInLine;

		// Token: 0x04001F3B RID: 7995
		private readonly int _scriptLineNumber;

		// Token: 0x04001F3C RID: 7996
		private readonly string _scriptName;

		// Token: 0x04001F3D RID: 7997
		private readonly string _line;

		// Token: 0x04001F3E RID: 7998
		private readonly string _fullScript;
	}
}
