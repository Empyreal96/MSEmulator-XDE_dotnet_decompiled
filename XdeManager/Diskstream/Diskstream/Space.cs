using System;
using System.Runtime.InteropServices;

namespace Microsoft.Spaces.Diskstream
{
	// Token: 0x02000004 RID: 4
	public class Space : Disk
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002673 File Offset: 0x00000873
		protected Space(IntPtr handle) : base(handle)
		{
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002680 File Offset: 0x00000880
		public string Name
		{
			get
			{
				return Marshal.PtrToStringUni(Space.SpaceHandleGetName(base.GetHandle()));
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000026A4 File Offset: 0x000008A4
		internal static Space Open(IntPtr handle)
		{
			return new Space(handle);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000026BC File Offset: 0x000008BC
		protected override void Dispose(bool disposing)
		{
			bool isDisposed = base.IsDisposed;
			if (!isDisposed)
			{
				base.IsDisposed = true;
			}
		}

		// Token: 0x06000031 RID: 49
		[DllImport("diskhandle.dll")]
		private static extern IntPtr SpaceHandleGetName(IntPtr spaceHandle);
	}
}
