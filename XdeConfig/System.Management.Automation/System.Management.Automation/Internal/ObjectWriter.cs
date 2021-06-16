using System;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000892 RID: 2194
	internal class ObjectWriter : PipelineWriter
	{
		// Token: 0x06005428 RID: 21544 RVA: 0x001BCD44 File Offset: 0x001BAF44
		public ObjectWriter([In] [Out] ObjectStreamBase stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._stream = stream;
		}

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x06005429 RID: 21545 RVA: 0x001BCD61 File Offset: 0x001BAF61
		public override WaitHandle WaitHandle
		{
			get
			{
				return this._stream.WriteHandle;
			}
		}

		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x0600542A RID: 21546 RVA: 0x001BCD6E File Offset: 0x001BAF6E
		public override bool IsOpen
		{
			get
			{
				return this._stream.IsOpen;
			}
		}

		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x0600542B RID: 21547 RVA: 0x001BCD7B File Offset: 0x001BAF7B
		public override int Count
		{
			get
			{
				return this._stream.Count;
			}
		}

		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x0600542C RID: 21548 RVA: 0x001BCD88 File Offset: 0x001BAF88
		public override int MaxCapacity
		{
			get
			{
				return this._stream.MaxCapacity;
			}
		}

		// Token: 0x0600542D RID: 21549 RVA: 0x001BCD95 File Offset: 0x001BAF95
		public override void Close()
		{
			this._stream.Close();
		}

		// Token: 0x0600542E RID: 21550 RVA: 0x001BCDA2 File Offset: 0x001BAFA2
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x0600542F RID: 21551 RVA: 0x001BCDAF File Offset: 0x001BAFAF
		public override int Write(object obj)
		{
			return this._stream.Write(obj);
		}

		// Token: 0x06005430 RID: 21552 RVA: 0x001BCDBD File Offset: 0x001BAFBD
		public override int Write(object obj, bool enumerateCollection)
		{
			return this._stream.Write(obj, enumerateCollection);
		}

		// Token: 0x04002B25 RID: 11045
		private ObjectStreamBase _stream;
	}
}
