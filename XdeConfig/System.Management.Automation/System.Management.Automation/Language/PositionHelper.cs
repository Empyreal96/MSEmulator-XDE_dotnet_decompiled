using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005B7 RID: 1463
	internal class PositionHelper
	{
		// Token: 0x06003E59 RID: 15961 RVA: 0x0014B90C File Offset: 0x00149B0C
		internal PositionHelper(string filename, string scriptText)
		{
			this._filename = filename;
			this._scriptText = scriptText;
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06003E5A RID: 15962 RVA: 0x0014B922 File Offset: 0x00149B22
		internal string ScriptText
		{
			get
			{
				return this._scriptText;
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (set) Token: 0x06003E5B RID: 15963 RVA: 0x0014B92A File Offset: 0x00149B2A
		internal int[] LineStartMap
		{
			set
			{
				this._lineStartMap = value;
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06003E5C RID: 15964 RVA: 0x0014B933 File Offset: 0x00149B33
		public string File
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x0014B93C File Offset: 0x00149B3C
		internal int LineFromOffset(int offset)
		{
			int num = Array.BinarySearch<int>(this._lineStartMap, offset);
			if (num < 0)
			{
				num = ~num - 1;
			}
			return num + 1;
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x0014B962 File Offset: 0x00149B62
		internal int ColumnFromOffset(int offset)
		{
			return offset - this._lineStartMap[this.LineFromOffset(offset) - 1] + 1;
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x0014B978 File Offset: 0x00149B78
		internal string Text(int line)
		{
			int num = this._lineStartMap[line - 1];
			if (line < this._lineStartMap.Length)
			{
				int length = this._lineStartMap[line] - num;
				return this.ScriptText.Substring(num, length);
			}
			return this.ScriptText.Substring(num);
		}

		// Token: 0x04001F32 RID: 7986
		private readonly string _filename;

		// Token: 0x04001F33 RID: 7987
		private readonly string _scriptText;

		// Token: 0x04001F34 RID: 7988
		private int[] _lineStartMap;
	}
}
