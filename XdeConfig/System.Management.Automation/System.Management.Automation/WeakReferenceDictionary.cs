using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Management.Automation
{
	// Token: 0x0200045B RID: 1115
	internal class WeakReferenceDictionary<T> : IDictionary<object, T>, ICollection<KeyValuePair<object, T>>, IEnumerable<KeyValuePair<object, T>>, IEnumerable
	{
		// Token: 0x060030AB RID: 12459 RVA: 0x0010A6FC File Offset: 0x001088FC
		public WeakReferenceDictionary()
		{
			this.weakEqualityComparer = new WeakReferenceDictionary<T>.WeakReferenceEqualityComparer();
			this.dictionary = new Dictionary<WeakReference, T>(this.weakEqualityComparer);
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x0010A72C File Offset: 0x0010892C
		private void CleanUp()
		{
			if (this.Count > this.cleanupTriggerSize)
			{
				Dictionary<WeakReference, T> dictionary = new Dictionary<WeakReference, T>(this.weakEqualityComparer);
				foreach (KeyValuePair<WeakReference, T> keyValuePair in this.dictionary)
				{
					object target = keyValuePair.Key.Target;
					if (target != null)
					{
						dictionary.Add(keyValuePair.Key, keyValuePair.Value);
					}
				}
				this.dictionary = dictionary;
				this.cleanupTriggerSize = 1000 + this.Count * 2;
			}
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x0010A7D4 File Offset: 0x001089D4
		public void Add(object key, T value)
		{
			this.dictionary.Add(new WeakReference(key), value);
			this.CleanUp();
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x0010A7EE File Offset: 0x001089EE
		public bool ContainsKey(object key)
		{
			return this.dictionary.ContainsKey(new WeakReference(key));
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x060030AF RID: 12463 RVA: 0x0010A804 File Offset: 0x00108A04
		public ICollection<object> Keys
		{
			get
			{
				List<object> list = new List<object>(this.dictionary.Keys.Count);
				foreach (WeakReference weakReference in this.dictionary.Keys)
				{
					object target = weakReference.Target;
					if (target != null)
					{
						list.Add(target);
					}
				}
				return list;
			}
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x0010A880 File Offset: 0x00108A80
		public bool Remove(object key)
		{
			return this.dictionary.Remove(new WeakReference(key));
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x0010A894 File Offset: 0x00108A94
		public bool TryGetValue(object key, out T value)
		{
			WeakReference key2 = new WeakReference(key);
			return this.dictionary.TryGetValue(key2, out value);
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x060030B2 RID: 12466 RVA: 0x0010A8B5 File Offset: 0x00108AB5
		public ICollection<T> Values
		{
			get
			{
				return this.dictionary.Values;
			}
		}

		// Token: 0x17000B26 RID: 2854
		public T this[object key]
		{
			get
			{
				return this.dictionary[new WeakReference(key)];
			}
			set
			{
				this.dictionary[new WeakReference(key)] = value;
				this.CleanUp();
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x060030B5 RID: 12469 RVA: 0x0010A8EF File Offset: 0x00108AEF
		private ICollection<KeyValuePair<WeakReference, T>> WeakCollection
		{
			get
			{
				return this.dictionary;
			}
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x0010A8F7 File Offset: 0x00108AF7
		private static KeyValuePair<WeakReference, T> WeakKeyValuePair(KeyValuePair<object, T> publicKeyValuePair)
		{
			return new KeyValuePair<WeakReference, T>(new WeakReference(publicKeyValuePair.Key), publicKeyValuePair.Value);
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x0010A911 File Offset: 0x00108B11
		public void Add(KeyValuePair<object, T> item)
		{
			this.WeakCollection.Add(WeakReferenceDictionary<T>.WeakKeyValuePair(item));
			this.CleanUp();
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x0010A92A File Offset: 0x00108B2A
		public void Clear()
		{
			this.WeakCollection.Clear();
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x0010A937 File Offset: 0x00108B37
		public bool Contains(KeyValuePair<object, T> item)
		{
			return this.WeakCollection.Contains(WeakReferenceDictionary<T>.WeakKeyValuePair(item));
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x0010A94C File Offset: 0x00108B4C
		public void CopyTo(KeyValuePair<object, T>[] array, int arrayIndex)
		{
			List<KeyValuePair<object, T>> list = new List<KeyValuePair<object, T>>(this.WeakCollection.Count);
			foreach (KeyValuePair<object, T> item in this)
			{
				list.Add(item);
			}
			list.CopyTo(array, arrayIndex);
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x060030BB RID: 12475 RVA: 0x0010A9B0 File Offset: 0x00108BB0
		public int Count
		{
			get
			{
				return this.WeakCollection.Count;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x060030BC RID: 12476 RVA: 0x0010A9BD File Offset: 0x00108BBD
		public bool IsReadOnly
		{
			get
			{
				return this.WeakCollection.IsReadOnly;
			}
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x0010A9CA File Offset: 0x00108BCA
		public bool Remove(KeyValuePair<object, T> item)
		{
			return this.WeakCollection.Remove(WeakReferenceDictionary<T>.WeakKeyValuePair(item));
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x0010AB4C File Offset: 0x00108D4C
		public IEnumerator<KeyValuePair<object, T>> GetEnumerator()
		{
			foreach (KeyValuePair<WeakReference, T> weakKeyValuePair in this.WeakCollection)
			{
				KeyValuePair<WeakReference, T> keyValuePair = weakKeyValuePair;
				object key = keyValuePair.Key.Target;
				if (key != null)
				{
					object key2 = key;
					KeyValuePair<WeakReference, T> keyValuePair2 = weakKeyValuePair;
					yield return new KeyValuePair<object, T>(key2, keyValuePair2.Value);
				}
			}
			yield break;
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x0010AB68 File Offset: 0x00108D68
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<object, T>>)this).GetEnumerator();
		}

		// Token: 0x04001A32 RID: 6706
		private const int initialCleanupTriggerSize = 1000;

		// Token: 0x04001A33 RID: 6707
		private readonly IEqualityComparer<WeakReference> weakEqualityComparer;

		// Token: 0x04001A34 RID: 6708
		private Dictionary<WeakReference, T> dictionary;

		// Token: 0x04001A35 RID: 6709
		private int cleanupTriggerSize = 1000;

		// Token: 0x0200045C RID: 1116
		private class WeakReferenceEqualityComparer : IEqualityComparer<WeakReference>
		{
			// Token: 0x060030C0 RID: 12480 RVA: 0x0010AB80 File Offset: 0x00108D80
			public bool Equals(WeakReference x, WeakReference y)
			{
				object target = x.Target;
				if (target == null)
				{
					return false;
				}
				object target2 = y.Target;
				return target2 != null && object.ReferenceEquals(target, target2);
			}

			// Token: 0x060030C1 RID: 12481 RVA: 0x0010ABAC File Offset: 0x00108DAC
			public int GetHashCode(WeakReference obj)
			{
				object target = obj.Target;
				if (target == null)
				{
					return RuntimeHelpers.GetHashCode(obj);
				}
				return RuntimeHelpers.GetHashCode(target);
			}
		}
	}
}
