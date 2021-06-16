using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200088B RID: 2187
	internal class ObjectReader : ObjectReaderBase<object>
	{
		// Token: 0x060053C0 RID: 21440 RVA: 0x001BB775 File Offset: 0x001B9975
		public ObjectReader([In] [Out] ObjectStream stream) : base(stream)
		{
		}

		// Token: 0x060053C1 RID: 21441 RVA: 0x001BB77E File Offset: 0x001B997E
		public override Collection<object> Read(int count)
		{
			return this._stream.Read(count);
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x001BB78C File Offset: 0x001B998C
		public override object Read()
		{
			return this._stream.Read();
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x001BB799 File Offset: 0x001B9999
		public override Collection<object> ReadToEnd()
		{
			return this._stream.ReadToEnd();
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x001BB7A6 File Offset: 0x001B99A6
		public override Collection<object> NonBlockingRead()
		{
			return this._stream.NonBlockingRead(int.MaxValue);
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x001BB7B8 File Offset: 0x001B99B8
		public override Collection<object> NonBlockingRead(int maxRequested)
		{
			return this._stream.NonBlockingRead(maxRequested);
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x001BB7C6 File Offset: 0x001B99C6
		public override object Peek()
		{
			return this._stream.Peek();
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x001BB7D3 File Offset: 0x001B99D3
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._stream.Close();
			}
		}
	}
}
