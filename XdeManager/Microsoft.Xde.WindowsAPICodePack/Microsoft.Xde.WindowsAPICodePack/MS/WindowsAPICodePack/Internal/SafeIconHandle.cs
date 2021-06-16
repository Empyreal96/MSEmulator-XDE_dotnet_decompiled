using System;

namespace MS.WindowsAPICodePack.Internal
{
	// Token: 0x02000009 RID: 9
	public class SafeIconHandle : ZeroInvalidHandle
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00002E9E File Offset: 0x0000109E
		protected override bool ReleaseHandle()
		{
			return CoreNativeMethods.DestroyIcon(this.handle);
		}
	}
}
