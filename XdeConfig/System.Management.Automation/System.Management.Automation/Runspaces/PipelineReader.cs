using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Threading;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200087B RID: 2171
	public abstract class PipelineReader<T>
	{
		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06005311 RID: 21265
		// (remove) Token: 0x06005312 RID: 21266
		public abstract event EventHandler DataReady;

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x06005313 RID: 21267
		public abstract WaitHandle WaitHandle { get; }

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x06005314 RID: 21268
		public abstract bool EndOfPipeline { get; }

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x06005315 RID: 21269
		public abstract bool IsOpen { get; }

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x06005316 RID: 21270
		public abstract int Count { get; }

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x06005317 RID: 21271
		public abstract int MaxCapacity { get; }

		// Token: 0x06005318 RID: 21272
		public abstract void Close();

		// Token: 0x06005319 RID: 21273
		public abstract Collection<T> Read(int count);

		// Token: 0x0600531A RID: 21274
		public abstract T Read();

		// Token: 0x0600531B RID: 21275
		public abstract Collection<T> ReadToEnd();

		// Token: 0x0600531C RID: 21276
		public abstract Collection<T> NonBlockingRead();

		// Token: 0x0600531D RID: 21277
		public abstract Collection<T> NonBlockingRead(int maxRequested);

		// Token: 0x0600531E RID: 21278
		public abstract T Peek();

		// Token: 0x0600531F RID: 21279 RVA: 0x001BA014 File Offset: 0x001B8214
		internal IEnumerator<T> GetReadEnumerator()
		{
			while (!this.EndOfPipeline)
			{
				T t = this.Read();
				if (object.Equals(t, AutomationNull.Value))
				{
					break;
				}
				yield return t;
			}
			yield break;
		}
	}
}
