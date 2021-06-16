using System;
using System.Runtime.InteropServices;

namespace MS.WindowsAPICodePack.Internal
{
	// Token: 0x02000008 RID: 8
	public abstract class ZeroInvalidHandle : SafeHandle
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00002E7E File Offset: 0x0000107E
		protected ZeroInvalidHandle() : base(IntPtr.Zero, true)
		{
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002E8C File Offset: 0x0000108C
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}
	}
}
