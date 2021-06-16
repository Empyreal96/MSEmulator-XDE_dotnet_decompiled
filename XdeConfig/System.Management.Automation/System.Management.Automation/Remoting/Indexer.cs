using System;
using System.Collections;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002B0 RID: 688
	internal class Indexer : IEnumerable, IEnumerator
	{
		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002146 RID: 8518 RVA: 0x000BFC53 File Offset: 0x000BDE53
		public object Current
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x000BFC5B File Offset: 0x000BDE5B
		internal Indexer(int[] lengths)
		{
			this._lengths = lengths;
			this._current = new int[lengths.Length];
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x000BFC78 File Offset: 0x000BDE78
		private bool CheckLengthsNonNegative(int[] lengths)
		{
			for (int i = 0; i < lengths.Length; i++)
			{
				if (lengths[i] < 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x000BFC9C File Offset: 0x000BDE9C
		public IEnumerator GetEnumerator()
		{
			this.Reset();
			return this;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x000BFCA8 File Offset: 0x000BDEA8
		public void Reset()
		{
			for (int i = 0; i < this._current.Length; i++)
			{
				this._current[i] = 0;
			}
			if (this._current.Length > 0)
			{
				this._current[this._current.Length - 1] = -1;
			}
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x000BFCF0 File Offset: 0x000BDEF0
		public bool MoveNext()
		{
			for (int i = this._lengths.Length - 1; i >= 0; i--)
			{
				if (this._current[i] < this._lengths[i] - 1)
				{
					this._current[i]++;
					return true;
				}
				this._current[i] = 0;
			}
			return false;
		}

		// Token: 0x04000EBA RID: 3770
		private int[] _current;

		// Token: 0x04000EBB RID: 3771
		private int[] _lengths;
	}
}
