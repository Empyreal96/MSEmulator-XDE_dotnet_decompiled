using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A89 RID: 2697
	internal sealed class SplatCallSite
	{
		// Token: 0x06006B2A RID: 27434 RVA: 0x00219DDA File Offset: 0x00217FDA
		internal SplatCallSite(object callable)
		{
			this._callable = callable;
		}

		// Token: 0x06006B2B RID: 27435 RVA: 0x00219DEC File Offset: 0x00217FEC
		internal object Invoke(object[] args)
		{
			Delegate @delegate = this._callable as Delegate;
			if (@delegate != null)
			{
				return @delegate.DynamicInvoke(args);
			}
			if (this._site == null)
			{
				this._site = CallSite<Func<CallSite, object, object[], object>>.Create(SplatInvokeBinder.Instance);
			}
			return this._site.Target(this._site, this._callable, args);
		}

		// Token: 0x04003337 RID: 13111
		internal readonly object _callable;

		// Token: 0x04003338 RID: 13112
		internal CallSite<Func<CallSite, object, object[], object>> _site;
	}
}
