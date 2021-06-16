using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x02000820 RID: 2080
	internal abstract class ScopedItemSearcher<T> : IEnumerator<!0>, IDisposable, IEnumerator, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x06004FDD RID: 20445 RVA: 0x001A7813 File Offset: 0x001A5A13
		internal ScopedItemSearcher(SessionStateInternal sessionState, VariablePath lookupPath)
		{
			if (sessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("sessionState");
			}
			if (lookupPath == null)
			{
				throw PSTraceSource.NewArgumentNullException("lookupPath");
			}
			this.sessionState = sessionState;
			this.lookupPath = lookupPath;
			this.InitializeScopeEnumerator();
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x001A784B File Offset: 0x001A5A4B
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this;
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x001A784E File Offset: 0x001A5A4E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x001A7854 File Offset: 0x001A5A54
		public bool MoveNext()
		{
			bool result = true;
			if (!this.isInitialized)
			{
				this.InitializeScopeEnumerator();
			}
			while (this.scopeEnumerable.MoveNext())
			{
				T t;
				if (this.TryGetNewScopeItem(((IEnumerator<SessionStateScope>)this.scopeEnumerable).Current, out t))
				{
					this.currentScope = ((IEnumerator<SessionStateScope>)this.scopeEnumerable).Current;
					this.current = t;
					result = true;
					break;
				}
				result = false;
				if (this.isSingleScopeLookup)
				{
					break;
				}
			}
			return result;
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06004FE1 RID: 20449 RVA: 0x001A78BC File Offset: 0x001A5ABC
		T IEnumerator<!0>.Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06004FE2 RID: 20450 RVA: 0x001A78C4 File Offset: 0x001A5AC4
		public object Current
		{
			get
			{
				return this.current;
			}
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x001A78D1 File Offset: 0x001A5AD1
		public void Reset()
		{
			this.InitializeScopeEnumerator();
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x001A78D9 File Offset: 0x001A5AD9
		public void Dispose()
		{
			this.current = default(T);
			this.scopeEnumerable.Dispose();
			this.scopeEnumerable = null;
			this.isInitialized = false;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004FE5 RID: 20453
		protected abstract bool GetScopeItem(SessionStateScope scope, VariablePath name, out T newCurrentItem);

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06004FE6 RID: 20454 RVA: 0x001A7906 File Offset: 0x001A5B06
		internal SessionStateScope CurrentLookupScope
		{
			get
			{
				return this.currentScope;
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x001A790E File Offset: 0x001A5B0E
		internal SessionStateScope InitialScope
		{
			get
			{
				return this.initialScope;
			}
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x001A7918 File Offset: 0x001A5B18
		private bool TryGetNewScopeItem(SessionStateScope lookupScope, out T newCurrentItem)
		{
			return this.GetScopeItem(lookupScope, this.lookupPath, out newCurrentItem);
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x001A7938 File Offset: 0x001A5B38
		private void InitializeScopeEnumerator()
		{
			this.initialScope = this.sessionState.CurrentScope;
			if (this.lookupPath.IsGlobal)
			{
				this.initialScope = this.sessionState.GlobalScope;
				this.isSingleScopeLookup = true;
			}
			else if (this.lookupPath.IsLocal || this.lookupPath.IsPrivate)
			{
				this.initialScope = this.sessionState.CurrentScope;
				this.isSingleScopeLookup = true;
			}
			else if (this.lookupPath.IsScript)
			{
				this.initialScope = this.sessionState.ScriptScope;
				this.isSingleScopeLookup = true;
			}
			this.scopeEnumerable = new SessionStateScopeEnumerator(this.initialScope);
			this.isInitialized = true;
		}

		// Token: 0x040028DC RID: 10460
		private SessionStateScope currentScope;

		// Token: 0x040028DD RID: 10461
		private SessionStateScope initialScope;

		// Token: 0x040028DE RID: 10462
		private T current;

		// Token: 0x040028DF RID: 10463
		protected SessionStateInternal sessionState;

		// Token: 0x040028E0 RID: 10464
		private VariablePath lookupPath;

		// Token: 0x040028E1 RID: 10465
		private SessionStateScopeEnumerator scopeEnumerable;

		// Token: 0x040028E2 RID: 10466
		private bool isSingleScopeLookup;

		// Token: 0x040028E3 RID: 10467
		private bool isInitialized;
	}
}
