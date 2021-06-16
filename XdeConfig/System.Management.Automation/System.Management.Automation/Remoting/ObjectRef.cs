using System;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000256 RID: 598
	internal class ObjectRef<T> where T : class
	{
		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001C57 RID: 7255 RVA: 0x000A5012 File Offset: 0x000A3212
		internal T OldValue
		{
			get
			{
				return this._oldValue;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001C58 RID: 7256 RVA: 0x000A501A File Offset: 0x000A321A
		internal T Value
		{
			get
			{
				if (this._newValue == null)
				{
					return this._oldValue;
				}
				return this._newValue;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001C59 RID: 7257 RVA: 0x000A5036 File Offset: 0x000A3236
		internal bool IsOverridden
		{
			get
			{
				return this._newValue != null;
			}
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000A5049 File Offset: 0x000A3249
		internal ObjectRef(T oldValue)
		{
			this._oldValue = oldValue;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x000A5058 File Offset: 0x000A3258
		internal void Override(T newValue)
		{
			this._newValue = newValue;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x000A5061 File Offset: 0x000A3261
		internal void Revert()
		{
			this._newValue = default(T);
		}

		// Token: 0x04000BB6 RID: 2998
		private T _newValue;

		// Token: 0x04000BB7 RID: 2999
		private T _oldValue;
	}
}
