using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000241 RID: 577
	internal sealed class PSDataCollectionEnumerator<W> : IBlockingEnumerator<W>, IEnumerator<!0>, IDisposable, IEnumerator
	{
		// Token: 0x06001B65 RID: 7013 RVA: 0x000A155C File Offset: 0x0009F75C
		internal PSDataCollectionEnumerator(PSDataCollection<W> collection, bool neverBlock)
		{
			this.collToEnumerate = collection;
			this.index = 0;
			this.currentElement = default(W);
			this.collToEnumerate.IsEnumerated = true;
			this._neverBlock = neverBlock;
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06001B66 RID: 7014 RVA: 0x000A1591 File Offset: 0x0009F791
		W IEnumerator<!0>.Current
		{
			get
			{
				return this.currentElement;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001B67 RID: 7015 RVA: 0x000A1599 File Offset: 0x0009F799
		public object Current
		{
			get
			{
				return this.currentElement;
			}
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x000A15A6 File Offset: 0x0009F7A6
		public bool MoveNext()
		{
			return this.MoveNext(!this._neverBlock);
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x000A15B8 File Offset: 0x0009F7B8
		public bool MoveNext(bool block)
		{
			bool result;
			lock (this.collToEnumerate.SyncObject)
			{
				while (this.index >= this.collToEnumerate.Count)
				{
					if (this.collToEnumerate.RefCount == 0 || !this.collToEnumerate.IsOpen)
					{
						return false;
					}
					if (!block)
					{
						return false;
					}
					if (this.collToEnumerate.PulseIdleEvent)
					{
						this.collToEnumerate.FireIdleEvent();
						Monitor.Wait(this.collToEnumerate.SyncObject);
					}
					else
					{
						Monitor.Wait(this.collToEnumerate.SyncObject);
					}
				}
				this.currentElement = this.collToEnumerate[this.index];
				if (this.collToEnumerate.ReleaseOnEnumeration)
				{
					this.collToEnumerate[this.index] = default(W);
				}
				this.index++;
				result = true;
			}
			return result;
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x000A16C4 File Offset: 0x0009F8C4
		public void Reset()
		{
			this.currentElement = default(W);
			this.index = 0;
		}

		// Token: 0x06001B6B RID: 7019 RVA: 0x000A16D9 File Offset: 0x0009F8D9
		void IDisposable.Dispose()
		{
		}

		// Token: 0x04000B36 RID: 2870
		private W currentElement;

		// Token: 0x04000B37 RID: 2871
		private int index;

		// Token: 0x04000B38 RID: 2872
		private PSDataCollection<W> collToEnumerate;

		// Token: 0x04000B39 RID: 2873
		private bool _neverBlock;
	}
}
