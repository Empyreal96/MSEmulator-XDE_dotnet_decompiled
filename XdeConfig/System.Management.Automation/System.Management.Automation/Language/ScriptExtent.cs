using System;
using System.Globalization;

namespace System.Management.Automation.Language
{
	// Token: 0x020005BD RID: 1469
	public sealed class ScriptExtent : IScriptExtent
	{
		// Token: 0x06003E91 RID: 16017 RVA: 0x0014BC8C File Offset: 0x00149E8C
		private ScriptExtent()
		{
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x0014BC94 File Offset: 0x00149E94
		public ScriptExtent(ScriptPosition startPosition, ScriptPosition endPosition)
		{
			this._startPosition = startPosition;
			this._endPosition = endPosition;
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06003E93 RID: 16019 RVA: 0x0014BCAA File Offset: 0x00149EAA
		public string File
		{
			get
			{
				return this._startPosition.File;
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06003E94 RID: 16020 RVA: 0x0014BCB7 File Offset: 0x00149EB7
		public IScriptPosition StartScriptPosition
		{
			get
			{
				return this._startPosition;
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06003E95 RID: 16021 RVA: 0x0014BCBF File Offset: 0x00149EBF
		public IScriptPosition EndScriptPosition
		{
			get
			{
				return this._endPosition;
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06003E96 RID: 16022 RVA: 0x0014BCC7 File Offset: 0x00149EC7
		public int StartLineNumber
		{
			get
			{
				return this._startPosition.LineNumber;
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06003E97 RID: 16023 RVA: 0x0014BCD4 File Offset: 0x00149ED4
		public int StartColumnNumber
		{
			get
			{
				return this._startPosition.ColumnNumber;
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06003E98 RID: 16024 RVA: 0x0014BCE1 File Offset: 0x00149EE1
		public int EndLineNumber
		{
			get
			{
				return this._endPosition.LineNumber;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06003E99 RID: 16025 RVA: 0x0014BCEE File Offset: 0x00149EEE
		public int EndColumnNumber
		{
			get
			{
				return this._endPosition.ColumnNumber;
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06003E9A RID: 16026 RVA: 0x0014BCFB File Offset: 0x00149EFB
		public int StartOffset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06003E9B RID: 16027 RVA: 0x0014BCFE File Offset: 0x00149EFE
		public int EndOffset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06003E9C RID: 16028 RVA: 0x0014BD04 File Offset: 0x00149F04
		public string Text
		{
			get
			{
				if (this.EndColumnNumber <= 0)
				{
					return string.Empty;
				}
				if (this.StartLineNumber == this.EndLineNumber)
				{
					return this._startPosition.Line.Substring(this._startPosition.ColumnNumber - 1, this._endPosition.ColumnNumber - this._startPosition.ColumnNumber);
				}
				return string.Format(CultureInfo.InvariantCulture, "{0}...{1}", new object[]
				{
					this._startPosition.Line.Substring(this._startPosition.ColumnNumber),
					this._endPosition.Line.Substring(0, this._endPosition.ColumnNumber)
				});
			}
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0014BDE4 File Offset: 0x00149FE4
		internal void ToPSObjectForRemoting(PSObject dest)
		{
			RemotingEncoder.AddNoteProperty<string>(dest, "ScriptExtent_File", () => this.File);
			RemotingEncoder.AddNoteProperty<int>(dest, "ScriptExtent_StartLineNumber", () => this.StartLineNumber);
			RemotingEncoder.AddNoteProperty<int>(dest, "ScriptExtent_StartColumnNumber", () => this.StartColumnNumber);
			RemotingEncoder.AddNoteProperty<int>(dest, "ScriptExtent_EndLineNumber", () => this.EndLineNumber);
			RemotingEncoder.AddNoteProperty<int>(dest, "ScriptExtent_EndColumnNumber", () => this.EndColumnNumber);
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x0014BE64 File Offset: 0x0014A064
		private void PopulateFromSerializedInfo(PSObject serializedScriptExtent)
		{
			string propertyValue = RemotingDecoder.GetPropertyValue<string>(serializedScriptExtent, "ScriptExtent_File");
			int propertyValue2 = RemotingDecoder.GetPropertyValue<int>(serializedScriptExtent, "ScriptExtent_StartLineNumber");
			int propertyValue3 = RemotingDecoder.GetPropertyValue<int>(serializedScriptExtent, "ScriptExtent_StartColumnNumber");
			int propertyValue4 = RemotingDecoder.GetPropertyValue<int>(serializedScriptExtent, "ScriptExtent_EndLineNumber");
			int propertyValue5 = RemotingDecoder.GetPropertyValue<int>(serializedScriptExtent, "ScriptExtent_EndColumnNumber");
			ScriptPosition startPosition = new ScriptPosition(propertyValue, propertyValue2, propertyValue3, null);
			ScriptPosition endPosition = new ScriptPosition(propertyValue, propertyValue4, propertyValue5, null);
			this._startPosition = startPosition;
			this._endPosition = endPosition;
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x0014BED8 File Offset: 0x0014A0D8
		internal static ScriptExtent FromPSObjectForRemoting(PSObject serializedScriptExtent)
		{
			ScriptExtent scriptExtent = new ScriptExtent();
			scriptExtent.PopulateFromSerializedInfo(serializedScriptExtent);
			return scriptExtent;
		}

		// Token: 0x04001F3F RID: 7999
		private ScriptPosition _startPosition;

		// Token: 0x04001F40 RID: 8000
		private ScriptPosition _endPosition;
	}
}
