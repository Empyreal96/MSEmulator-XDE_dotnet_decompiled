using System;

namespace MS.WindowsAPICodePack.Internal
{
	// Token: 0x0200000B RID: 11
	public class SafeWindowHandle : ZeroInvalidHandle
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00002ECA File Offset: 0x000010CA
		protected override bool ReleaseHandle()
		{
			return this.IsInvalid || CoreNativeMethods.DestroyWindow(this.handle) != 0;
		}
	}
}
