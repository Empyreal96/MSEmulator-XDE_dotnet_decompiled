using System;
using System.Runtime.InteropServices;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x020007A9 RID: 1961
	internal sealed class SafeProcessHandle : SafeHandle
	{
		// Token: 0x06004CE9 RID: 19689 RVA: 0x0019591B File Offset: 0x00193B1B
		internal SafeProcessHandle() : base(IntPtr.Zero, true)
		{
		}

		// Token: 0x06004CEA RID: 19690 RVA: 0x00195929 File Offset: 0x00193B29
		internal SafeProcessHandle(IntPtr existingHandle) : base(IntPtr.Zero, true)
		{
			base.SetHandle(existingHandle);
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x0019593E File Offset: 0x00193B3E
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06004CEC RID: 19692 RVA: 0x0019594B File Offset: 0x00193B4B
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero || this.handle == new IntPtr(-1);
			}
		}
	}
}
