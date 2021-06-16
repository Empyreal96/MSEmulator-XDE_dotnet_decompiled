using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000022 RID: 34
	internal class EventPayload : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		// Token: 0x06000132 RID: 306 RVA: 0x0000A0A0 File Offset: 0x000082A0
		internal EventPayload(List<string> payloadNames, List<object> payloadValues)
		{
			this.m_names = payloadNames;
			this.m_values = payloadValues;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000A0B6 File Offset: 0x000082B6
		public ICollection<string> Keys
		{
			get
			{
				return this.m_names;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000A0BE File Offset: 0x000082BE
		public ICollection<object> Values
		{
			get
			{
				return this.m_values;
			}
		}

		// Token: 0x1700002D RID: 45
		public object this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				int num = 0;
				checked
				{
					foreach (string a in this.m_names)
					{
						if (a == key)
						{
							return this.m_values[num];
						}
						num++;
					}
					throw new KeyNotFoundException();
				}
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000A14F File Offset: 0x0000834F
		public void Add(string key, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000A156 File Offset: 0x00008356
		public void Add(KeyValuePair<string, object> payloadEntry)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000A15D File Offset: 0x0000835D
		public void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000A164 File Offset: 0x00008364
		public bool Contains(KeyValuePair<string, object> entry)
		{
			return this.ContainsKey(entry.Key);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000A174 File Offset: 0x00008374
		public bool ContainsKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			foreach (string a in this.m_names)
			{
				if (a == key)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000A1E0 File Offset: 0x000083E0
		public int Count
		{
			get
			{
				return this.m_names.Count;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000A1ED File Offset: 0x000083ED
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000A2C0 File Offset: 0x000084C0
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			checked
			{
				for (int i = 0; i < this.Keys.Count; i++)
				{
					yield return new KeyValuePair<string, object>(this.m_names[i], this.m_values[i]);
				}
				yield break;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000A2DC File Offset: 0x000084DC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, object>>)this).GetEnumerator();
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000A2F1 File Offset: 0x000084F1
		public void CopyTo(KeyValuePair<string, object>[] payloadEntries, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000A2F8 File Offset: 0x000084F8
		public bool Remove(string key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000A2FF File Offset: 0x000084FF
		public bool Remove(KeyValuePair<string, object> entry)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000A308 File Offset: 0x00008508
		public bool TryGetValue(string key, out object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int num = 0;
			checked
			{
				foreach (string a in this.m_names)
				{
					if (a == key)
					{
						value = this.m_values[num];
						return true;
					}
					num++;
				}
				value = null;
				return false;
			}
		}

		// Token: 0x040000A0 RID: 160
		private List<string> m_names;

		// Token: 0x040000A1 RID: 161
		private List<object> m_values;
	}
}
