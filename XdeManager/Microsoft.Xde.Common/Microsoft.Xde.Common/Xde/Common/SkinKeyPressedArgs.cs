using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000064 RID: 100
	public class SkinKeyPressedArgs : EventArgs
	{
		// Token: 0x0600023D RID: 573 RVA: 0x000051A8 File Offset: 0x000033A8
		public SkinKeyPressedArgs(SkinButton skinButton)
		{
			this.Button = skinButton;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600023E RID: 574 RVA: 0x000051B7 File Offset: 0x000033B7
		// (set) Token: 0x0600023F RID: 575 RVA: 0x000051BF File Offset: 0x000033BF
		public SkinButton Button { get; private set; }
	}
}
