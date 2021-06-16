using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x02000012 RID: 18
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public sealed class HcsNotificationWatcher<THandle> : IDisposable
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00003FC0 File Offset: 0x000021C0
		public HcsNotificationWatcher(THandle handle, RegisterHcsNotificationCallback<THandle> register, UnregisterHcsNotificationCallback unregister, HCS_NOTIFICATIONS[] notificationList)
		{
			this.n.TryAdd(HCS_NOTIFICATIONS.HcsNotificationSystemCreateCompleted, new TaskCompletionSource<NotificationResult>());
			this.n.TryAdd(HCS_NOTIFICATIONS.HcsNotificationSystemExited, new TaskCompletionSource<NotificationResult>());
			this.n.TryAdd(HCS_NOTIFICATIONS.HcsNotificationProcessExited, new TaskCompletionSource<NotificationResult>());
			this.callbackFunc = delegate(uint nType, IntPtr ctx, int nStatus, string nData)
			{
				if (nType == 16777216U)
				{
					HcsException ex = new HcsException(-2147467260, null);
					foreach (KeyValuePair<HCS_NOTIFICATIONS, TaskCompletionSource<NotificationResult>> keyValuePair in this.n)
					{
						if (this.IsSticky(keyValuePair.Key))
						{
							keyValuePair.Value.TrySetException(ex);
						}
						else
						{
							this.RemoveAndFailIf(keyValuePair.Key, ex);
						}
					}
					return;
				}
				if (nType == 1U)
				{
					HcsException ex2 = new HcsException(-1070137082, null);
					this.n[HCS_NOTIFICATIONS.HcsNotificationSystemCreateCompleted].TrySetException(ex2);
					this.RemoveAndFailIf(HCS_NOTIFICATIONS.HcsNotificationSystemStartCompleted, ex2);
					this.RemoveAndFailIf(HCS_NOTIFICATIONS.HcsNotificationSystemPauseCompleted, ex2);
					this.RemoveAndFailIf(HCS_NOTIFICATIONS.HcsNotificationSystemResumeCompleted, ex2);
				}
				this.n.AddOrUpdate((HCS_NOTIFICATIONS)nType, new TaskCompletionSource<NotificationResult>(), delegate(HCS_NOTIFICATIONS k, TaskCompletionSource<NotificationResult> existingTcs)
				{
					if (HcsException.Failed(nStatus))
					{
						existingTcs.TrySetException(new HcsException(nStatus, nData));
					}
					else
					{
						existingTcs.TrySetResult(new NotificationResult
						{
							Status = nStatus,
							Data = nData
						});
					}
					if (this.IsSticky(k))
					{
						return existingTcs;
					}
					return new TaskCompletionSource<NotificationResult>();
				});
			};
			register(handle, this.callbackFunc, IntPtr.Zero, out this.h);
			this.unreg = unregister;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000404C File Offset: 0x0000224C
		~HcsNotificationWatcher()
		{
			this.Dispose();
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004078 File Offset: 0x00002278
		public Task<NotificationResult> WatchAsync(HCS_NOTIFICATIONS notificationType)
		{
			return this.n.GetOrAdd(notificationType, new TaskCompletionSource<NotificationResult>()).Task;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004090 File Offset: 0x00002290
		public bool Wait(HCS_NOTIFICATIONS notificationType, int timeout = -1)
		{
			return this.WatchAsync(notificationType).Wait(timeout);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000040A0 File Offset: 0x000022A0
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (Interlocked.CompareExchange(ref this.disposed, 1, 0) == 0)
			{
				this.unreg(this.h);
				this.h = IntPtr.Zero;
				HcsException exception = new HcsException(-2147467260, null);
				TaskCompletionSource<NotificationResult>[] array = this.n.Values.ToArray<TaskCompletionSource<NotificationResult>>();
				this.n.Clear();
				TaskCompletionSource<NotificationResult>[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].TrySetException(exception);
				}
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004120 File Offset: 0x00002320
		private void RemoveAndFailIf(HCS_NOTIFICATIONS nType, HcsException e)
		{
			TaskCompletionSource<NotificationResult> taskCompletionSource;
			if (this.n.TryRemove(nType, out taskCompletionSource))
			{
				taskCompletionSource.TrySetException(e);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004145 File Offset: 0x00002345
		private bool IsSticky(HCS_NOTIFICATIONS nType)
		{
			return nType == HCS_NOTIFICATIONS.HcsNotificationSystemCreateCompleted || nType == HCS_NOTIFICATIONS.HcsNotificationSystemExited || nType == HCS_NOTIFICATIONS.HcsNotificationProcessExited;
		}

		// Token: 0x0400003E RID: 62
		private readonly ConcurrentDictionary<HCS_NOTIFICATIONS, TaskCompletionSource<NotificationResult>> n = new ConcurrentDictionary<HCS_NOTIFICATIONS, TaskCompletionSource<NotificationResult>>();

		// Token: 0x0400003F RID: 63
		private IntPtr h;

		// Token: 0x04000040 RID: 64
		private NotificationCallback callbackFunc;

		// Token: 0x04000041 RID: 65
		private UnregisterHcsNotificationCallback unreg;

		// Token: 0x04000042 RID: 66
		private int disposed;
	}
}
