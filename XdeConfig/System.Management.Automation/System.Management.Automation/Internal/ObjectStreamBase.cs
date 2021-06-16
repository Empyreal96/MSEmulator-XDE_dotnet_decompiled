using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200088F RID: 2191
	internal abstract class ObjectStreamBase : IDisposable
	{
		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x060053E6 RID: 21478 RVA: 0x001BBB8C File Offset: 0x001B9D8C
		// (remove) Token: 0x060053E7 RID: 21479 RVA: 0x001BBBC4 File Offset: 0x001B9DC4
		internal event EventHandler DataReady;

		// Token: 0x060053E8 RID: 21480 RVA: 0x001BBBF9 File Offset: 0x001B9DF9
		internal void FireDataReadyEvent(object source, EventArgs args)
		{
			this.DataReady.SafeInvoke(source, args);
		}

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x060053E9 RID: 21481
		internal abstract int MaxCapacity { get; }

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x060053EA RID: 21482 RVA: 0x001BBC08 File Offset: 0x001B9E08
		internal virtual WaitHandle ReadHandle
		{
			get
			{
				throw PSTraceSource.NewNotSupportedException();
			}
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x060053EB RID: 21483 RVA: 0x001BBC0F File Offset: 0x001B9E0F
		internal virtual WaitHandle WriteHandle
		{
			get
			{
				throw PSTraceSource.NewNotSupportedException();
			}
		}

		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x060053EC RID: 21484
		internal abstract bool EndOfPipeline { get; }

		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x060053ED RID: 21485
		internal abstract bool IsOpen { get; }

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x060053EE RID: 21486
		internal abstract int Count { get; }

		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x060053EF RID: 21487
		internal abstract PipelineReader<object> ObjectReader { get; }

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x060053F0 RID: 21488
		internal abstract PipelineReader<PSObject> PSObjectReader { get; }

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x060053F1 RID: 21489
		internal abstract PipelineWriter ObjectWriter { get; }

		// Token: 0x060053F2 RID: 21490 RVA: 0x001BBC16 File Offset: 0x001B9E16
		internal virtual object Read()
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x001BBC1D File Offset: 0x001B9E1D
		internal virtual Collection<object> Read(int count)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x001BBC24 File Offset: 0x001B9E24
		internal virtual Collection<object> ReadToEnd()
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x001BBC2B File Offset: 0x001B9E2B
		internal virtual Collection<object> NonBlockingRead(int maxRequested)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x001BBC32 File Offset: 0x001B9E32
		internal virtual object Peek()
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x060053F7 RID: 21495 RVA: 0x001BBC39 File Offset: 0x001B9E39
		internal virtual int Write(object value)
		{
			return this.Write(value, false);
		}

		// Token: 0x060053F8 RID: 21496 RVA: 0x001BBC43 File Offset: 0x001B9E43
		internal virtual int Write(object obj, bool enumerateCollection)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x001BBC4A File Offset: 0x001B9E4A
		internal virtual void Close()
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x001BBC51 File Offset: 0x001B9E51
		internal virtual void Flush()
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x001BBC58 File Offset: 0x001B9E58
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060053FC RID: 21500
		protected abstract void Dispose(bool disposing);
	}
}
