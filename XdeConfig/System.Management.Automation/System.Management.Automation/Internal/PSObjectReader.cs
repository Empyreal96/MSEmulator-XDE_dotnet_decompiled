using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200088C RID: 2188
	internal class PSObjectReader : ObjectReaderBase<PSObject>
	{
		// Token: 0x060053C8 RID: 21448 RVA: 0x001BB7E3 File Offset: 0x001B99E3
		public PSObjectReader([In] [Out] ObjectStream stream) : base(stream)
		{
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x001BB7EC File Offset: 0x001B99EC
		public override Collection<PSObject> Read(int count)
		{
			return PSObjectReader.MakePSObjectCollection(this._stream.Read(count));
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x001BB7FF File Offset: 0x001B99FF
		public override PSObject Read()
		{
			return PSObjectReader.MakePSObject(this._stream.Read());
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x001BB811 File Offset: 0x001B9A11
		public override Collection<PSObject> ReadToEnd()
		{
			return PSObjectReader.MakePSObjectCollection(this._stream.ReadToEnd());
		}

		// Token: 0x060053CC RID: 21452 RVA: 0x001BB823 File Offset: 0x001B9A23
		public override Collection<PSObject> NonBlockingRead()
		{
			return PSObjectReader.MakePSObjectCollection(this._stream.NonBlockingRead(int.MaxValue));
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x001BB83A File Offset: 0x001B9A3A
		public override Collection<PSObject> NonBlockingRead(int maxRequested)
		{
			return PSObjectReader.MakePSObjectCollection(this._stream.NonBlockingRead(maxRequested));
		}

		// Token: 0x060053CE RID: 21454 RVA: 0x001BB84D File Offset: 0x001B9A4D
		public override PSObject Peek()
		{
			return PSObjectReader.MakePSObject(this._stream.Peek());
		}

		// Token: 0x060053CF RID: 21455 RVA: 0x001BB85F File Offset: 0x001B9A5F
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._stream.Close();
			}
		}

		// Token: 0x060053D0 RID: 21456 RVA: 0x001BB86F File Offset: 0x001B9A6F
		private static PSObject MakePSObject(object o)
		{
			if (o == null)
			{
				return null;
			}
			return PSObject.AsPSObject(o);
		}

		// Token: 0x060053D1 RID: 21457 RVA: 0x001BB87C File Offset: 0x001B9A7C
		private static Collection<PSObject> MakePSObjectCollection(Collection<object> coll)
		{
			if (coll == null)
			{
				return null;
			}
			Collection<PSObject> collection = new Collection<PSObject>();
			foreach (object o in coll)
			{
				collection.Add(PSObjectReader.MakePSObject(o));
			}
			return collection;
		}
	}
}
