using System;
using System.Threading;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200087C RID: 2172
	public abstract class PipelineWriter
	{
		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x06005321 RID: 21281
		public abstract WaitHandle WaitHandle { get; }

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x06005322 RID: 21282
		public abstract bool IsOpen { get; }

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x06005323 RID: 21283
		public abstract int Count { get; }

		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x06005324 RID: 21284
		public abstract int MaxCapacity { get; }

		// Token: 0x06005325 RID: 21285
		public abstract void Close();

		// Token: 0x06005326 RID: 21286
		public abstract void Flush();

		// Token: 0x06005327 RID: 21287
		public abstract int Write(object obj);

		// Token: 0x06005328 RID: 21288
		public abstract int Write(object obj, bool enumerateCollection);
	}
}
