using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005B9 RID: 1465
	internal sealed class InternalScriptExtent : IScriptExtent
	{
		// Token: 0x06003E68 RID: 15976 RVA: 0x0014BA3F File Offset: 0x00149C3F
		internal InternalScriptExtent(PositionHelper _positionHelper, int startOffset, int endOffset)
		{
			this._positionHelper = _positionHelper;
			this._startOffset = startOffset;
			this._endOffset = endOffset;
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06003E69 RID: 15977 RVA: 0x0014BA5C File Offset: 0x00149C5C
		public string File
		{
			get
			{
				return this._positionHelper.File;
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06003E6A RID: 15978 RVA: 0x0014BA69 File Offset: 0x00149C69
		public IScriptPosition StartScriptPosition
		{
			get
			{
				return new InternalScriptPosition(this._positionHelper, this._startOffset);
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06003E6B RID: 15979 RVA: 0x0014BA7C File Offset: 0x00149C7C
		public IScriptPosition EndScriptPosition
		{
			get
			{
				return new InternalScriptPosition(this._positionHelper, this._endOffset);
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06003E6C RID: 15980 RVA: 0x0014BA8F File Offset: 0x00149C8F
		public int StartLineNumber
		{
			get
			{
				return this._positionHelper.LineFromOffset(this._startOffset);
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06003E6D RID: 15981 RVA: 0x0014BAA2 File Offset: 0x00149CA2
		public int StartColumnNumber
		{
			get
			{
				return this._positionHelper.ColumnFromOffset(this._startOffset);
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06003E6E RID: 15982 RVA: 0x0014BAB5 File Offset: 0x00149CB5
		public int EndLineNumber
		{
			get
			{
				return this._positionHelper.LineFromOffset(this._endOffset);
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06003E6F RID: 15983 RVA: 0x0014BAC8 File Offset: 0x00149CC8
		public int EndColumnNumber
		{
			get
			{
				return this._positionHelper.ColumnFromOffset(this._endOffset);
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06003E70 RID: 15984 RVA: 0x0014BADC File Offset: 0x00149CDC
		public string Text
		{
			get
			{
				if (this._startOffset > this._positionHelper.ScriptText.Length)
				{
					return "";
				}
				return this._positionHelper.ScriptText.Substring(this._startOffset, this._endOffset - this._startOffset);
			}
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x0014BB2A File Offset: 0x00149D2A
		public override string ToString()
		{
			return this.Text;
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06003E72 RID: 15986 RVA: 0x0014BB32 File Offset: 0x00149D32
		internal PositionHelper PositionHelper
		{
			get
			{
				return this._positionHelper;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06003E73 RID: 15987 RVA: 0x0014BB3A File Offset: 0x00149D3A
		public int StartOffset
		{
			get
			{
				return this._startOffset;
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06003E74 RID: 15988 RVA: 0x0014BB42 File Offset: 0x00149D42
		public int EndOffset
		{
			get
			{
				return this._endOffset;
			}
		}

		// Token: 0x04001F37 RID: 7991
		private readonly PositionHelper _positionHelper;

		// Token: 0x04001F38 RID: 7992
		private readonly int _startOffset;

		// Token: 0x04001F39 RID: 7993
		private readonly int _endOffset;
	}
}
