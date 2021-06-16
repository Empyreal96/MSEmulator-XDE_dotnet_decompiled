using System;
using System.Collections;

namespace System.Management.Automation
{
	// Token: 0x0200048B RID: 1163
	internal class RangeEnumerator : IEnumerator
	{
		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06003380 RID: 13184 RVA: 0x001197C0 File Offset: 0x001179C0
		internal int LowerBound
		{
			get
			{
				return this._lowerBound;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06003381 RID: 13185 RVA: 0x001197C8 File Offset: 0x001179C8
		internal int UpperBound
		{
			get
			{
				return this._upperBound;
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06003382 RID: 13186 RVA: 0x001197D0 File Offset: 0x001179D0
		public object Current
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06003383 RID: 13187 RVA: 0x001197DD File Offset: 0x001179DD
		internal int CurrentValue
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x001197E5 File Offset: 0x001179E5
		public RangeEnumerator(int lowerBound, int upperBound)
		{
			this._lowerBound = lowerBound;
			this._current = this._lowerBound;
			this._upperBound = upperBound;
			if (lowerBound > upperBound)
			{
				this.increment = -1;
			}
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x00119820 File Offset: 0x00117A20
		public void Reset()
		{
			this._current = this._lowerBound;
			this.firstElement = true;
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x00119835 File Offset: 0x00117A35
		public bool MoveNext()
		{
			if (this.firstElement)
			{
				this.firstElement = false;
				return true;
			}
			if (this._current == this._upperBound)
			{
				return false;
			}
			this._current += this.increment;
			return true;
		}

		// Token: 0x04001AB4 RID: 6836
		private int _lowerBound;

		// Token: 0x04001AB5 RID: 6837
		private int _upperBound;

		// Token: 0x04001AB6 RID: 6838
		private int _current;

		// Token: 0x04001AB7 RID: 6839
		private int increment = 1;

		// Token: 0x04001AB8 RID: 6840
		private bool firstElement = true;
	}
}
