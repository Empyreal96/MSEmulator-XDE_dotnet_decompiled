using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005B8 RID: 1464
	internal sealed class InternalScriptPosition : IScriptPosition
	{
		// Token: 0x06003E60 RID: 15968 RVA: 0x0014B9C0 File Offset: 0x00149BC0
		internal InternalScriptPosition(PositionHelper _positionHelper, int offset)
		{
			this._positionHelper = _positionHelper;
			this._offset = offset;
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06003E61 RID: 15969 RVA: 0x0014B9D6 File Offset: 0x00149BD6
		public string File
		{
			get
			{
				return this._positionHelper.File;
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06003E62 RID: 15970 RVA: 0x0014B9E3 File Offset: 0x00149BE3
		public int LineNumber
		{
			get
			{
				return this._positionHelper.LineFromOffset(this._offset);
			}
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x06003E63 RID: 15971 RVA: 0x0014B9F6 File Offset: 0x00149BF6
		public int ColumnNumber
		{
			get
			{
				return this._positionHelper.ColumnFromOffset(this._offset);
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06003E64 RID: 15972 RVA: 0x0014BA09 File Offset: 0x00149C09
		public string Line
		{
			get
			{
				return this._positionHelper.Text(this.LineNumber);
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06003E65 RID: 15973 RVA: 0x0014BA1C File Offset: 0x00149C1C
		public int Offset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x0014BA24 File Offset: 0x00149C24
		internal InternalScriptPosition CloneWithNewOffset(int offset)
		{
			return new InternalScriptPosition(this._positionHelper, offset);
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x0014BA32 File Offset: 0x00149C32
		public string GetFullScript()
		{
			return this._positionHelper.ScriptText;
		}

		// Token: 0x04001F35 RID: 7989
		private readonly PositionHelper _positionHelper;

		// Token: 0x04001F36 RID: 7990
		private readonly int _offset;
	}
}
