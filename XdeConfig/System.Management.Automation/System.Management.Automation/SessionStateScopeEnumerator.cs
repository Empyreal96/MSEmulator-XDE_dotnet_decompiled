using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x0200083D RID: 2109
	internal sealed class SessionStateScopeEnumerator : IEnumerator<SessionStateScope>, IDisposable, IEnumerator, IEnumerable<SessionStateScope>, IEnumerable
	{
		// Token: 0x0600515F RID: 20831 RVA: 0x001B1FEB File Offset: 0x001B01EB
		internal SessionStateScopeEnumerator(SessionStateScope scope)
		{
			this._initialScope = scope;
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x001B1FFA File Offset: 0x001B01FA
		public bool MoveNext()
		{
			this._currentEnumeratedScope = ((this._currentEnumeratedScope == null) ? this._initialScope : this._currentEnumeratedScope.Parent);
			return this._currentEnumeratedScope != null;
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x001B2029 File Offset: 0x001B0229
		public void Reset()
		{
			this._currentEnumeratedScope = null;
		}

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x06005162 RID: 20834 RVA: 0x001B2032 File Offset: 0x001B0232
		SessionStateScope IEnumerator<SessionStateScope>.Current
		{
			get
			{
				if (this._currentEnumeratedScope == null)
				{
					throw PSTraceSource.NewInvalidOperationException();
				}
				return this._currentEnumeratedScope;
			}
		}

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x06005163 RID: 20835 RVA: 0x001B2048 File Offset: 0x001B0248
		object IEnumerator.Current
		{
			get
			{
				return ((IEnumerator<SessionStateScope>)this).Current;
			}
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x001B2050 File Offset: 0x001B0250
		IEnumerator<SessionStateScope> IEnumerable<SessionStateScope>.GetEnumerator()
		{
			return this;
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x001B2053 File Offset: 0x001B0253
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x001B2056 File Offset: 0x001B0256
		public void Dispose()
		{
			this.Reset();
		}

		// Token: 0x0400298B RID: 10635
		private readonly SessionStateScope _initialScope;

		// Token: 0x0400298C RID: 10636
		private SessionStateScope _currentEnumeratedScope;
	}
}
