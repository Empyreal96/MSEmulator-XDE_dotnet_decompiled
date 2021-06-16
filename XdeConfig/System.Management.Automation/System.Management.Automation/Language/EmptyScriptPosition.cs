using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005BA RID: 1466
	internal sealed class EmptyScriptPosition : IScriptPosition
	{
		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06003E75 RID: 15989 RVA: 0x0014BB4A File Offset: 0x00149D4A
		public string File
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06003E76 RID: 15990 RVA: 0x0014BB4D File Offset: 0x00149D4D
		public int LineNumber
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06003E77 RID: 15991 RVA: 0x0014BB50 File Offset: 0x00149D50
		public int ColumnNumber
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06003E78 RID: 15992 RVA: 0x0014BB53 File Offset: 0x00149D53
		public int Offset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06003E79 RID: 15993 RVA: 0x0014BB56 File Offset: 0x00149D56
		public string Line
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x0014BB5D File Offset: 0x00149D5D
		public string GetFullScript()
		{
			return null;
		}
	}
}
