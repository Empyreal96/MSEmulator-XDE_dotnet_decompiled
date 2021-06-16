using System;
using System.Threading;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000755 RID: 1877
	internal class ThreadLocal<T>
	{
		// Token: 0x06004AF6 RID: 19190 RVA: 0x00188E99 File Offset: 0x00187099
		public ThreadLocal()
		{
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x00188EA1 File Offset: 0x001870A1
		public ThreadLocal(bool refCounted)
		{
			this._refCounted = refCounted;
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06004AF8 RID: 19192 RVA: 0x00188EB0 File Offset: 0x001870B0
		// (set) Token: 0x06004AF9 RID: 19193 RVA: 0x00188EBD File Offset: 0x001870BD
		public T Value
		{
			get
			{
				return this.GetStorageInfo().Value;
			}
			set
			{
				this.GetStorageInfo().Value = value;
			}
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x00188ECC File Offset: 0x001870CC
		public T GetOrCreate(Func<T> func)
		{
			ThreadLocal<T>.StorageInfo storageInfo = this.GetStorageInfo();
			T t = storageInfo.Value;
			if (t == null)
			{
				t = (storageInfo.Value = func());
			}
			return t;
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x00188F00 File Offset: 0x00187100
		public T Update(Func<T, T> updater)
		{
			ThreadLocal<T>.StorageInfo storageInfo = this.GetStorageInfo();
			return storageInfo.Value = updater(storageInfo.Value);
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x00188F2C File Offset: 0x0018712C
		public T Update(T newValue)
		{
			ThreadLocal<T>.StorageInfo storageInfo = this.GetStorageInfo();
			T value = storageInfo.Value;
			storageInfo.Value = newValue;
			return value;
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x00188F4F File Offset: 0x0018714F
		private static int GetCurrentThreadId()
		{
			return Thread.CurrentThread.ManagedThreadId;
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x00188F5B File Offset: 0x0018715B
		public ThreadLocal<T>.StorageInfo GetStorageInfo()
		{
			return this.GetStorageInfo(this._stores);
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x00188F6C File Offset: 0x0018716C
		private ThreadLocal<T>.StorageInfo GetStorageInfo(ThreadLocal<T>.StorageInfo[] curStorage)
		{
			int currentThreadId = ThreadLocal<T>.GetCurrentThreadId();
			if (curStorage != null && curStorage.Length > currentThreadId)
			{
				ThreadLocal<T>.StorageInfo storageInfo = curStorage[currentThreadId];
				if (storageInfo != null && (this._refCounted || storageInfo.Thread == Thread.CurrentThread))
				{
					return storageInfo;
				}
			}
			return this.RetryOrCreateStorageInfo(curStorage);
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x00188FAD File Offset: 0x001871AD
		private ThreadLocal<T>.StorageInfo RetryOrCreateStorageInfo(ThreadLocal<T>.StorageInfo[] curStorage)
		{
			if (curStorage == ThreadLocal<T>.Updating)
			{
				while ((curStorage = this._stores) == ThreadLocal<T>.Updating)
				{
					Thread.Sleep(0);
				}
				return this.GetStorageInfo(curStorage);
			}
			return this.CreateStorageInfo();
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x00188FE0 File Offset: 0x001871E0
		private ThreadLocal<T>.StorageInfo CreateStorageInfo()
		{
			Thread.BeginCriticalRegion();
			ThreadLocal<T>.StorageInfo[] array = ThreadLocal<T>.Updating;
			ThreadLocal<T>.StorageInfo result;
			try
			{
				int currentThreadId = ThreadLocal<T>.GetCurrentThreadId();
				ThreadLocal<T>.StorageInfo storageInfo = new ThreadLocal<T>.StorageInfo(Thread.CurrentThread);
				while ((array = Interlocked.Exchange<ThreadLocal<T>.StorageInfo[]>(ref this._stores, ThreadLocal<T>.Updating)) == ThreadLocal<T>.Updating)
				{
					Thread.Sleep(0);
				}
				if (array == null)
				{
					array = new ThreadLocal<T>.StorageInfo[currentThreadId + 1];
				}
				else if (array.Length <= currentThreadId)
				{
					ThreadLocal<T>.StorageInfo[] array2 = new ThreadLocal<T>.StorageInfo[currentThreadId + 1];
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null && array[i].Thread.IsAlive)
						{
							array2[i] = array[i];
						}
					}
					array = array2;
				}
				result = (array[currentThreadId] = storageInfo);
			}
			finally
			{
				if (array != ThreadLocal<T>.Updating)
				{
					Interlocked.Exchange<ThreadLocal<T>.StorageInfo[]>(ref this._stores, array);
				}
				Thread.EndCriticalRegion();
			}
			return result;
		}

		// Token: 0x0400243B RID: 9275
		private ThreadLocal<T>.StorageInfo[] _stores;

		// Token: 0x0400243C RID: 9276
		private static readonly ThreadLocal<T>.StorageInfo[] Updating = new ThreadLocal<T>.StorageInfo[0];

		// Token: 0x0400243D RID: 9277
		private readonly bool _refCounted;

		// Token: 0x02000756 RID: 1878
		internal sealed class StorageInfo
		{
			// Token: 0x06004B03 RID: 19203 RVA: 0x001890C1 File Offset: 0x001872C1
			internal StorageInfo(Thread curThread)
			{
				this.Thread = curThread;
			}

			// Token: 0x0400243E RID: 9278
			internal readonly Thread Thread;

			// Token: 0x0400243F RID: 9279
			public T Value;
		}
	}
}
