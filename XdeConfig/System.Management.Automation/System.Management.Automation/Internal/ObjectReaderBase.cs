using System;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200088A RID: 2186
	internal abstract class ObjectReaderBase<T> : PipelineReader<T>, IDisposable
	{
		// Token: 0x060053B2 RID: 21426 RVA: 0x001BB5A6 File Offset: 0x001B97A6
		public ObjectReaderBase([In] [Out] ObjectStreamBase stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream", "stream may not be null");
			}
			this._stream = stream;
		}

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x060053B3 RID: 21427 RVA: 0x001BB5D4 File Offset: 0x001B97D4
		// (remove) Token: 0x060053B4 RID: 21428 RVA: 0x001BB63C File Offset: 0x001B983C
		public override event EventHandler DataReady
		{
			add
			{
				lock (this._monitorObject)
				{
					bool flag2 = null == this.InternalDataReady;
					this.InternalDataReady += value;
					if (flag2)
					{
						this._stream.DataReady += this.OnDataReady;
					}
				}
			}
			remove
			{
				lock (this._monitorObject)
				{
					this.InternalDataReady -= value;
					if (this.InternalDataReady == null)
					{
						this._stream.DataReady -= this.OnDataReady;
					}
				}
			}
		}

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x060053B5 RID: 21429 RVA: 0x001BB69C File Offset: 0x001B989C
		// (remove) Token: 0x060053B6 RID: 21430 RVA: 0x001BB6D4 File Offset: 0x001B98D4
		public event EventHandler InternalDataReady;

		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x060053B7 RID: 21431 RVA: 0x001BB709 File Offset: 0x001B9909
		public override WaitHandle WaitHandle
		{
			get
			{
				return this._stream.ReadHandle;
			}
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x060053B8 RID: 21432 RVA: 0x001BB716 File Offset: 0x001B9916
		public override bool EndOfPipeline
		{
			get
			{
				return this._stream.EndOfPipeline;
			}
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x060053B9 RID: 21433 RVA: 0x001BB723 File Offset: 0x001B9923
		public override bool IsOpen
		{
			get
			{
				return this._stream.IsOpen;
			}
		}

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x060053BA RID: 21434 RVA: 0x001BB730 File Offset: 0x001B9930
		public override int Count
		{
			get
			{
				return this._stream.Count;
			}
		}

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x060053BB RID: 21435 RVA: 0x001BB73D File Offset: 0x001B993D
		public override int MaxCapacity
		{
			get
			{
				return this._stream.MaxCapacity;
			}
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x001BB74A File Offset: 0x001B994A
		public override void Close()
		{
			this._stream.Close();
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x001BB757 File Offset: 0x001B9957
		private void OnDataReady(object sender, EventArgs args)
		{
			this.InternalDataReady.SafeInvoke(this, args);
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x001BB766 File Offset: 0x001B9966
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060053BF RID: 21439
		protected abstract void Dispose(bool disposing);

		// Token: 0x04002B06 RID: 11014
		protected ObjectStreamBase _stream;

		// Token: 0x04002B07 RID: 11015
		private object _monitorObject = new object();
	}
}
