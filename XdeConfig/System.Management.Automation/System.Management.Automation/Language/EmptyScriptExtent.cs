using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005BB RID: 1467
	internal sealed class EmptyScriptExtent : IScriptExtent
	{
		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x06003E7C RID: 15996 RVA: 0x0014BB68 File Offset: 0x00149D68
		public string File
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06003E7D RID: 15997 RVA: 0x0014BB6B File Offset: 0x00149D6B
		public IScriptPosition StartScriptPosition
		{
			get
			{
				return PositionUtilities.EmptyPosition;
			}
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06003E7E RID: 15998 RVA: 0x0014BB72 File Offset: 0x00149D72
		public IScriptPosition EndScriptPosition
		{
			get
			{
				return PositionUtilities.EmptyPosition;
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06003E7F RID: 15999 RVA: 0x0014BB79 File Offset: 0x00149D79
		public int StartLineNumber
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06003E80 RID: 16000 RVA: 0x0014BB7C File Offset: 0x00149D7C
		public int StartColumnNumber
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06003E81 RID: 16001 RVA: 0x0014BB7F File Offset: 0x00149D7F
		public int EndLineNumber
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06003E82 RID: 16002 RVA: 0x0014BB82 File Offset: 0x00149D82
		public int EndColumnNumber
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06003E83 RID: 16003 RVA: 0x0014BB85 File Offset: 0x00149D85
		public int StartOffset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06003E84 RID: 16004 RVA: 0x0014BB88 File Offset: 0x00149D88
		public int EndOffset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06003E85 RID: 16005 RVA: 0x0014BB8B File Offset: 0x00149D8B
		public string Text
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x0014BB94 File Offset: 0x00149D94
		public override bool Equals(object obj)
		{
			IScriptExtent scriptExtent = obj as IScriptExtent;
			return scriptExtent != null && (string.IsNullOrEmpty(scriptExtent.File) && scriptExtent.StartLineNumber == this.StartLineNumber && scriptExtent.StartColumnNumber == this.StartColumnNumber && scriptExtent.EndLineNumber == this.EndLineNumber && scriptExtent.EndColumnNumber == this.EndColumnNumber && string.IsNullOrEmpty(scriptExtent.Text));
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x0014BC02 File Offset: 0x00149E02
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
