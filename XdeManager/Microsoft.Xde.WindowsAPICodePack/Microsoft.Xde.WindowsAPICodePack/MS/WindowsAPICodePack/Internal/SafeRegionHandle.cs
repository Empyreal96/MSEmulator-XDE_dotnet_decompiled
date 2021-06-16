using System;

namespace MS.WindowsAPICodePack.Internal
{
	// Token: 0x0200000A RID: 10
	public class SafeRegionHandle : ZeroInvalidHandle
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00002EB8 File Offset: 0x000010B8
		protected override bool ReleaseHandle()
		{
			return CoreNativeMethods.DeleteObject(this.handle);
		}
	}
}
