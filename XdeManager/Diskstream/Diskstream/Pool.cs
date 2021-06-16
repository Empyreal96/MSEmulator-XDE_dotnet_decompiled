using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Microsoft.Spaces.Diskstream
{
	// Token: 0x02000003 RID: 3
	public class Pool : IDisposable
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002420 File Offset: 0x00000620
		protected Pool(IntPtr handle)
		{
			bool flag = handle == IntPtr.Zero || handle == Pool.InvalidHandleValue;
			if (flag)
			{
				throw new ArgumentException("The handle is invalid.");
			}
			this.Handle = handle;
			this.IsDisposed = false;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002470 File Offset: 0x00000670
		~Pool()
		{
			this.Dispose(false);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000024A4 File Offset: 0x000006A4
		public Space[] Spaces
		{
			get
			{
				int num = (int)Pool.PoolHandleGetNumberOfSpaceHandles(this.GetHandle());
				bool flag = num == 0;
				Space[] result;
				if (flag)
				{
					result = null;
				}
				else
				{
					IntPtr[] array = new IntPtr[num];
					Pool.PoolHandleGetSpaceHandles(this.GetHandle(), (uint)num, array);
					Space[] array2 = new Space[num];
					for (int i = 0; i < num; i++)
					{
						array2[i] = Space.Open(array[i]);
					}
					result = array2;
				}
				return result;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002514 File Offset: 0x00000714
		// (set) Token: 0x06000020 RID: 32 RVA: 0x0000251C File Offset: 0x0000071C
		protected IntPtr Handle { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002525 File Offset: 0x00000725
		// (set) Token: 0x06000022 RID: 34 RVA: 0x0000252D File Offset: 0x0000072D
		protected bool IsDisposed { get; set; }

		// Token: 0x06000023 RID: 35 RVA: 0x00002538 File Offset: 0x00000738
		public static Pool Open(Disk disk)
		{
			return Pool.Open(new Disk[]
			{
				disk
			});
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000255C File Offset: 0x0000075C
		public static Pool Open(Disk[] disks)
		{
			bool flag = disks == null;
			if (flag)
			{
				throw new ArgumentNullException("disks");
			}
			bool flag2 = disks.Length < 1;
			if (flag2)
			{
				throw new ArgumentException("At least one disk is required.");
			}
			IntPtr[] array = new IntPtr[disks.Length];
			for (int i = 0; i < disks.Length; i++)
			{
				array[i] = disks[i].GetHandle();
			}
			IntPtr intPtr = Pool.PoolHandleOpen((uint)array.Length, array);
			bool flag3 = intPtr == Pool.InvalidHandleValue;
			if (flag3)
			{
				throw new Win32Exception();
			}
			return new Pool(intPtr);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000025F2 File Offset: 0x000007F2
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002604 File Offset: 0x00000804
		protected virtual void Dispose(bool disposing)
		{
			bool isDisposed = this.IsDisposed;
			if (!isDisposed)
			{
				Pool.PoolHandleClose(this.GetHandle());
				this.IsDisposed = true;
			}
		}

		// Token: 0x06000027 RID: 39
		[DllImport("diskhandle.dll", SetLastError = true)]
		private static extern IntPtr PoolHandleOpen(uint numberOfDiskHandles, IntPtr[] diskHandles);

		// Token: 0x06000028 RID: 40
		[DllImport("diskhandle.dll")]
		private static extern bool PoolHandleClose(IntPtr poolHandle);

		// Token: 0x06000029 RID: 41
		[DllImport("diskhandle.dll")]
		private static extern uint PoolHandleGetNumberOfSpaceHandles(IntPtr poolHandle);

		// Token: 0x0600002A RID: 42
		[DllImport("diskhandle.dll")]
		private static extern void PoolHandleGetSpaceHandles(IntPtr poolHandle, uint numberOfSpaceHandles, IntPtr[] spaceHandles);

		// Token: 0x0600002B RID: 43 RVA: 0x00002638 File Offset: 0x00000838
		private IntPtr GetHandle()
		{
			bool isDisposed = this.IsDisposed;
			if (isDisposed)
			{
				throw new ObjectDisposedException("Handle");
			}
			return this.Handle;
		}

		// Token: 0x04000005 RID: 5
		protected static readonly IntPtr InvalidHandleValue = new IntPtr(-1);
	}
}
