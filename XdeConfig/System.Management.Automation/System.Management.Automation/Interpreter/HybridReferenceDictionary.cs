using System;
using System.Collections.Generic;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000752 RID: 1874
	internal class HybridReferenceDictionary<TKey, TValue> where TKey : class
	{
		// Token: 0x06004AE6 RID: 19174 RVA: 0x0018892E File Offset: 0x00186B2E
		public HybridReferenceDictionary()
		{
		}

		// Token: 0x06004AE7 RID: 19175 RVA: 0x00188936 File Offset: 0x00186B36
		public HybridReferenceDictionary(int initialCapicity)
		{
			if (initialCapicity > 10)
			{
				this._dict = new Dictionary<TKey, TValue>(initialCapicity);
				return;
			}
			this._keysAndValues = new KeyValuePair<TKey, TValue>[initialCapicity];
		}

		// Token: 0x06004AE8 RID: 19176 RVA: 0x0018895C File Offset: 0x00186B5C
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._dict != null)
			{
				return this._dict.TryGetValue(key, out value);
			}
			if (this._keysAndValues != null)
			{
				for (int i = 0; i < this._keysAndValues.Length; i++)
				{
					if (this._keysAndValues[i].Key == key)
					{
						value = this._keysAndValues[i].Value;
						return true;
					}
				}
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x06004AE9 RID: 19177 RVA: 0x001889DC File Offset: 0x00186BDC
		public bool Remove(TKey key)
		{
			if (this._dict != null)
			{
				return this._dict.Remove(key);
			}
			if (this._keysAndValues != null)
			{
				for (int i = 0; i < this._keysAndValues.Length; i++)
				{
					if (this._keysAndValues[i].Key == key)
					{
						this._keysAndValues[i] = default(KeyValuePair<TKey, TValue>);
						this._count--;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004AEA RID: 19178 RVA: 0x00188A5C File Offset: 0x00186C5C
		public bool ContainsKey(TKey key)
		{
			if (this._dict != null)
			{
				return this._dict.ContainsKey(key);
			}
			if (this._keysAndValues != null)
			{
				for (int i = 0; i < this._keysAndValues.Length; i++)
				{
					if (this._keysAndValues[i].Key == key)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06004AEB RID: 19179 RVA: 0x00188ABA File Offset: 0x00186CBA
		public int Count
		{
			get
			{
				if (this._dict != null)
				{
					return this._dict.Count;
				}
				return this._count;
			}
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x00188AD6 File Offset: 0x00186CD6
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (this._dict != null)
			{
				return this._dict.GetEnumerator();
			}
			return this.GetEnumeratorWorker();
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x00188BE0 File Offset: 0x00186DE0
		private IEnumerator<KeyValuePair<TKey, TValue>> GetEnumeratorWorker()
		{
			if (this._keysAndValues != null)
			{
				for (int i = 0; i < this._keysAndValues.Length; i++)
				{
					if (this._keysAndValues[i].Key != null)
					{
						yield return this._keysAndValues[i];
					}
				}
			}
			yield break;
		}

		// Token: 0x17000FB7 RID: 4023
		public TValue this[TKey key]
		{
			get
			{
				TValue result;
				if (this.TryGetValue(key, out result))
				{
					return result;
				}
				throw new KeyNotFoundException();
			}
			set
			{
				if (this._dict != null)
				{
					this._dict[key] = value;
					return;
				}
				int num;
				if (this._keysAndValues != null)
				{
					num = -1;
					for (int i = 0; i < this._keysAndValues.Length; i++)
					{
						if (this._keysAndValues[i].Key == key)
						{
							this._keysAndValues[i] = new KeyValuePair<TKey, TValue>(key, value);
							return;
						}
						if (this._keysAndValues[i].Key == null)
						{
							num = i;
						}
					}
				}
				else
				{
					this._keysAndValues = new KeyValuePair<TKey, TValue>[10];
					num = 0;
				}
				if (num != -1)
				{
					this._count++;
					this._keysAndValues[num] = new KeyValuePair<TKey, TValue>(key, value);
					return;
				}
				this._dict = new Dictionary<TKey, TValue>();
				for (int j = 0; j < this._keysAndValues.Length; j++)
				{
					this._dict[this._keysAndValues[j].Key] = this._keysAndValues[j].Value;
				}
				this._keysAndValues = null;
				this._dict[key] = value;
			}
		}

		// Token: 0x04002432 RID: 9266
		private const int _arraySize = 10;

		// Token: 0x04002433 RID: 9267
		private KeyValuePair<TKey, TValue>[] _keysAndValues;

		// Token: 0x04002434 RID: 9268
		private Dictionary<TKey, TValue> _dict;

		// Token: 0x04002435 RID: 9269
		private int _count;
	}
}
