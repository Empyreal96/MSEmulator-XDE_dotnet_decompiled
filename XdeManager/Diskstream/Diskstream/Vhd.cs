using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Microsoft.Spaces.Diskstream
{
	// Token: 0x02000005 RID: 5
	public class Vhd : Disk
	{
		// Token: 0x06000032 RID: 50 RVA: 0x000026DF File Offset: 0x000008DF
		protected Vhd(IntPtr handle) : base(handle)
		{
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000026EC File Offset: 0x000008EC
		public static List<Vhd> Open(List<string> vhdFilePaths, bool readOnly = false)
		{
			List<string> list = new List<string>(vhdFilePaths);
			list.Reverse();
			List<Vhd> list2 = new List<Vhd>();
			bool flag = false;
			try
			{
				Vhd vhd = null;
				for (int i = 0; i < list.Count; i++)
				{
					bool readOnly2 = true;
					bool flag2 = i == list.Count - 1;
					if (flag2)
					{
						readOnly2 = readOnly;
					}
					vhd = Vhd.Open(list[i], readOnly2, vhd);
					list2.Add(vhd);
				}
				list2.Reverse();
				flag = true;
			}
			finally
			{
				bool flag3 = !flag;
				if (flag3)
				{
					foreach (Vhd vhd2 in list2)
					{
						vhd2.Dispose();
					}
				}
			}
			return list2;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000027DC File Offset: 0x000009DC
		public static Vhd Open(string vhdFilePath, bool readOnly, Vhd parent)
		{
			IntPtr parentVhd = Disk.InvalidHandleValue;
			bool flag = parent != null;
			if (flag)
			{
				parentVhd = parent.GetHandle();
			}
			IntPtr intPtr = Vhd.VhdHandleOpen(vhdFilePath, readOnly, parentVhd);
			bool flag2 = intPtr == Disk.InvalidHandleValue;
			if (flag2)
			{
				throw new Win32Exception();
			}
			return new Vhd(intPtr);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000282C File Offset: 0x00000A2C
		protected override void Dispose(bool disposing)
		{
			bool isDisposed = base.IsDisposed;
			if (!isDisposed)
			{
				Vhd.VhdHandleClose(base.GetHandle());
				base.IsDisposed = true;
			}
		}

		// Token: 0x06000036 RID: 54
		[DllImport("diskhandle.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr VhdHandleOpen(string vhdFilePath, bool readOnly, IntPtr parentVhd);

		// Token: 0x06000037 RID: 55
		[DllImport("diskhandle.dll", SetLastError = true)]
		private static extern bool VhdHandleClose(IntPtr vhdHandle);

		// Token: 0x06000038 RID: 56
		[DllImport("diskhandle.dll", SetLastError = true)]
		private static extern bool DiskHandleFlush(IntPtr diskHandle);
	}
}
