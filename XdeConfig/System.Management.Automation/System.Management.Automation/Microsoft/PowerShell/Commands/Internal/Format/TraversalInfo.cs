using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004AA RID: 1194
	internal class TraversalInfo
	{
		// Token: 0x0600353B RID: 13627 RVA: 0x001212E0 File Offset: 0x0011F4E0
		internal TraversalInfo(int level, int maxDepth)
		{
			this._level = level;
			this._maxDepth = maxDepth;
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x0600353C RID: 13628 RVA: 0x001212F6 File Offset: 0x0011F4F6
		internal int Level
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x0600353D RID: 13629 RVA: 0x001212FE File Offset: 0x0011F4FE
		internal int MaxDepth
		{
			get
			{
				return this._maxDepth;
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x0600353E RID: 13630 RVA: 0x00121306 File Offset: 0x0011F506
		internal TraversalInfo NextLevel
		{
			get
			{
				return new TraversalInfo(this._level + 1, this._maxDepth);
			}
		}

		// Token: 0x04001B3A RID: 6970
		private int _level;

		// Token: 0x04001B3B RID: 6971
		private int _maxDepth;
	}
}
