using System;
using System.Collections;
using System.Threading;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200087D RID: 2173
	internal class DiscardingPipelineWriter : PipelineWriter
	{
		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x0600532A RID: 21290 RVA: 0x001BA040 File Offset: 0x001B8240
		public override WaitHandle WaitHandle
		{
			get
			{
				return this.waitHandle;
			}
		}

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x0600532B RID: 21291 RVA: 0x001BA048 File Offset: 0x001B8248
		public override bool IsOpen
		{
			get
			{
				return this.isOpen;
			}
		}

		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x0600532C RID: 21292 RVA: 0x001BA050 File Offset: 0x001B8250
		public override int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x0600532D RID: 21293 RVA: 0x001BA058 File Offset: 0x001B8258
		public override int MaxCapacity
		{
			get
			{
				return int.MaxValue;
			}
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x001BA05F File Offset: 0x001B825F
		public override void Close()
		{
			this.isOpen = false;
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x001BA068 File Offset: 0x001B8268
		public override void Flush()
		{
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x001BA06C File Offset: 0x001B826C
		public override int Write(object obj)
		{
			int num = 1;
			this.count += num;
			return num;
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x001BA08C File Offset: 0x001B828C
		public override int Write(object obj, bool enumerateCollection)
		{
			if (!enumerateCollection)
			{
				return this.Write(obj);
			}
			int num = 0;
			IEnumerable enumerable = LanguagePrimitives.GetEnumerable(obj);
			if (enumerable != null)
			{
				using (IEnumerator enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						num++;
					}
					goto IL_4A;
				}
			}
			num++;
			IL_4A:
			this.count += num;
			return num;
		}

		// Token: 0x04002AB6 RID: 10934
		private ManualResetEvent waitHandle = new ManualResetEvent(true);

		// Token: 0x04002AB7 RID: 10935
		private bool isOpen = true;

		// Token: 0x04002AB8 RID: 10936
		private int count;
	}
}
