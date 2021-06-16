using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x02000016 RID: 22
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public abstract class SafeHcsHandle<TWatcherHandle> : SafeHandle
	{
		// Token: 0x060000AA RID: 170 RVA: 0x00004286 File Offset: 0x00002486
		protected SafeHcsHandle(IHcs hcs) : base(IntPtr.Zero, true)
		{
			this.SetHcs(hcs);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000429B File Offset: 0x0000249B
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000042AD File Offset: 0x000024AD
		// (set) Token: 0x060000AD RID: 173 RVA: 0x000042B5 File Offset: 0x000024B5
		internal HcsNotificationWatcher<TWatcherHandle> Watcher { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AE RID: 174
		protected abstract HcsCloseHandleFunc CloseFunc { get; }

		// Token: 0x060000AF RID: 175 RVA: 0x000042BE File Offset: 0x000024BE
		public IHcs GetHcs()
		{
			return this.hcs;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000042C6 File Offset: 0x000024C6
		public void SetHcs(IHcs value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.hcs = value;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000042DD File Offset: 0x000024DD
		public Task<NotificationResult> WatchAsync(HCS_NOTIFICATIONS notificationType)
		{
			HcsNotificationWatcher<TWatcherHandle> watcher = this.Watcher;
			if (watcher == null)
			{
				return null;
			}
			return watcher.WatchAsync(notificationType);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000042F1 File Offset: 0x000024F1
		protected override bool ReleaseHandle()
		{
			HcsNotificationWatcher<TWatcherHandle> watcher = this.Watcher;
			if (watcher != null)
			{
				watcher.Dispose();
			}
			this.CloseFunc(this.handle);
			this.handle = IntPtr.Zero;
			return true;
		}

		// Token: 0x04000045 RID: 69
		private IHcs hcs;
	}
}
