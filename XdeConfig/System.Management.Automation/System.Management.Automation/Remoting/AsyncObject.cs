using System;
using System.Threading;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002C8 RID: 712
	internal class AsyncObject<T> where T : class
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x000C1A90 File Offset: 0x000BFC90
		// (set) Token: 0x060021E2 RID: 8674 RVA: 0x000C1ABE File Offset: 0x000BFCBE
		internal T Value
		{
			get
			{
				if (!this._valueWasSet.WaitOne())
				{
					this._value = default(T);
				}
				return this._value;
			}
			set
			{
				this._value = value;
				this._valueWasSet.Set();
			}
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x000C1AD3 File Offset: 0x000BFCD3
		internal AsyncObject()
		{
			this._valueWasSet = new ManualResetEvent(false);
		}

		// Token: 0x0400100D RID: 4109
		private T _value;

		// Token: 0x0400100E RID: 4110
		private ManualResetEvent _valueWasSet;
	}
}
