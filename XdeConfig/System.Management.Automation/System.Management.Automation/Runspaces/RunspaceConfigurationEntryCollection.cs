using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000407 RID: 1031
	public sealed class RunspaceConfigurationEntryCollection<T> : IEnumerable<!0>, IEnumerable where T : RunspaceConfigurationEntry
	{
		// Token: 0x06002E4C RID: 11852 RVA: 0x000FEE35 File Offset: 0x000FD035
		public RunspaceConfigurationEntryCollection()
		{
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x000FEE5E File Offset: 0x000FD05E
		public RunspaceConfigurationEntryCollection(IEnumerable<T> items)
		{
			if (items == null)
			{
				throw PSTraceSource.NewArgumentNullException("item");
			}
			this.AddBuiltInItem(items);
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06002E4E RID: 11854 RVA: 0x000FEE9C File Offset: 0x000FD09C
		internal Collection<T> UpdateList
		{
			get
			{
				return this._updateList;
			}
		}

		// Token: 0x17000ABD RID: 2749
		public T this[int index]
		{
			get
			{
				T result;
				lock (this._syncObject)
				{
					result = this._data[index];
				}
				return result;
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06002E50 RID: 11856 RVA: 0x000FEEEC File Offset: 0x000FD0EC
		public int Count
		{
			get
			{
				int count;
				lock (this._syncObject)
				{
					count = this._data.Count;
				}
				return count;
			}
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000FEF34 File Offset: 0x000FD134
		public void Reset()
		{
			lock (this._syncObject)
			{
				for (int i = this._data.Count - 1; i >= 0; i--)
				{
					T t = this._data[i];
					if (!t.BuiltIn)
					{
						this.RecordRemove(this._data[i]);
						this._data.RemoveAt(i);
					}
				}
				this._builtInEnd = this._data.Count;
			}
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000FEFD4 File Offset: 0x000FD1D4
		public void RemoveItem(int index)
		{
			lock (this._syncObject)
			{
				if (index < 0 || index >= this._data.Count)
				{
					throw PSTraceSource.NewArgumentOutOfRangeException("index", index);
				}
				this.RecordRemove(this._data[index]);
				this._data.RemoveAt(index);
				if (index < this._builtInEnd)
				{
					this._builtInEnd--;
				}
			}
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x000FF068 File Offset: 0x000FD268
		public void RemoveItem(int index, int count)
		{
			lock (this._syncObject)
			{
				if (index < 0 || index + count > this._data.Count)
				{
					throw PSTraceSource.NewArgumentOutOfRangeException("index", index);
				}
				for (int i = index + count - 1; i >= index; i--)
				{
					this.RecordRemove(this._data[i]);
					this._data.RemoveAt(i);
				}
				int num = Math.Min(count, this._builtInEnd - index);
				if (num > 0)
				{
					this._builtInEnd -= num;
				}
			}
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000FF118 File Offset: 0x000FD318
		internal void Remove(T item)
		{
			lock (this._syncObject)
			{
				int num = this._data.IndexOf(item);
				if (num < 0 || num >= this._data.Count)
				{
					throw PSTraceSource.NewArgumentOutOfRangeException("index", num);
				}
				this.RecordRemove(item);
				this._data.Remove(item);
				if (num < this._builtInEnd)
				{
					this._builtInEnd--;
				}
			}
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000FF1B0 File Offset: 0x000FD3B0
		public void Prepend(T item)
		{
			lock (this._syncObject)
			{
				this.RecordAdd(item);
				item._builtIn = false;
				this._data.Insert(0, item);
				this._builtInEnd++;
			}
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x000FF218 File Offset: 0x000FD418
		public void Prepend(IEnumerable<T> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			lock (this._syncObject)
			{
				int num = 0;
				foreach (T t in items)
				{
					this.RecordAdd(t);
					t._builtIn = false;
					this._data.Insert(num++, t);
					this._builtInEnd++;
				}
			}
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x000FF2CC File Offset: 0x000FD4CC
		public void Append(T item)
		{
			lock (this._syncObject)
			{
				this.RecordAdd(item);
				item._builtIn = false;
				this._data.Add(item);
			}
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x000FF328 File Offset: 0x000FD528
		public void Append(IEnumerable<T> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			lock (this._syncObject)
			{
				foreach (T t in items)
				{
					this.RecordAdd(t);
					t._builtIn = false;
					this._data.Add(t);
				}
			}
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x000FF3C0 File Offset: 0x000FD5C0
		internal void AddBuiltInItem(T item)
		{
			lock (this._syncObject)
			{
				item._builtIn = true;
				this.RecordAdd(item);
				this._data.Insert(this._builtInEnd, item);
				this._builtInEnd++;
			}
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x000FF430 File Offset: 0x000FD630
		internal void AddBuiltInItem(IEnumerable<T> items)
		{
			lock (this._syncObject)
			{
				foreach (T t in items)
				{
					t._builtIn = true;
					this.RecordAdd(t);
					this._data.Insert(this._builtInEnd, t);
					this._builtInEnd++;
				}
			}
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x000FF4D0 File Offset: 0x000FD6D0
		internal void RemovePSSnapIn(string PSSnapinName)
		{
			lock (this._syncObject)
			{
				for (int i = this._data.Count - 1; i >= 0; i--)
				{
					T t = this._data[i];
					if (t.PSSnapIn != null)
					{
						T t2 = this._data[i];
						if (t2.PSSnapIn.Name.Equals(PSSnapinName, StringComparison.Ordinal))
						{
							this.RecordRemove(this._data[i]);
							this._data.RemoveAt(i);
							if (i < this._builtInEnd)
							{
								this._builtInEnd--;
							}
						}
					}
				}
			}
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x000FF5A0 File Offset: 0x000FD7A0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._data.GetEnumerator();
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x000FF5AD File Offset: 0x000FD7AD
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this._data.GetEnumerator();
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x000FF5BA File Offset: 0x000FD7BA
		public void Update()
		{
			this.Update(false);
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x000FF5C4 File Offset: 0x000FD7C4
		internal void Update(bool force)
		{
			lock (this._syncObject)
			{
				if (this.OnUpdate != null && (force || this._updateList.Count > 0))
				{
					this.OnUpdate();
					foreach (T t in this._updateList)
					{
						t._action = UpdateAction.None;
					}
					this._updateList.Clear();
				}
			}
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000FF670 File Offset: 0x000FD870
		private void RecordRemove(T t)
		{
			if (t.Action == UpdateAction.Add)
			{
				t._action = UpdateAction.None;
				this._updateList.Remove(t);
				return;
			}
			t._action = UpdateAction.Remove;
			this._updateList.Add(t);
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x000FF6C0 File Offset: 0x000FD8C0
		private void RecordAdd(T t)
		{
			if (t.Action == UpdateAction.Remove)
			{
				t._action = UpdateAction.None;
				this._updateList.Remove(t);
				return;
			}
			t._action = UpdateAction.Add;
			this._updateList.Add(t);
		}

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x06002E62 RID: 11874 RVA: 0x000FF710 File Offset: 0x000FD910
		// (remove) Token: 0x06002E63 RID: 11875 RVA: 0x000FF748 File Offset: 0x000FD948
		internal event RunspaceConfigurationEntryUpdateEventHandler OnUpdate;

		// Token: 0x0400185A RID: 6234
		private Collection<T> _data = new Collection<T>();

		// Token: 0x0400185B RID: 6235
		private int _builtInEnd;

		// Token: 0x0400185C RID: 6236
		private Collection<T> _updateList = new Collection<T>();

		// Token: 0x0400185D RID: 6237
		private object _syncObject = new object();
	}
}
