using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200002D RID: 45
	internal class IndexNode
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x00009390 File Offset: 0x00007590
		public IndexNode(IndexNodeSaveFn store, int storeOverhead, Index index, bool isRoot, uint allocatedSize)
		{
			this._store = store;
			this._storageOverhead = storeOverhead;
			this._index = index;
			this._isRoot = isRoot;
			this.Header = new IndexHeader(allocatedSize);
			this.TotalSpaceAvailable = (long)((ulong)allocatedSize);
			IndexEntry indexEntry = new IndexEntry(this._index.IsFileIndex);
			indexEntry.Flags |= IndexEntryFlags.End;
			this._entries = new List<IndexEntry>();
			this._entries.Add(indexEntry);
			this.Header.OffsetToFirstEntry = (uint)(16 + storeOverhead);
			this.Header.TotalSizeOfEntries = (uint)((ulong)this.Header.OffsetToFirstEntry + (ulong)((long)indexEntry.Size));
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000943C File Offset: 0x0000763C
		public IndexNode(IndexNodeSaveFn store, int storeOverhead, Index index, bool isRoot, byte[] buffer, int offset)
		{
			this._store = store;
			this._storageOverhead = storeOverhead;
			this._index = index;
			this._isRoot = isRoot;
			this.Header = new IndexHeader(buffer, offset);
			this.TotalSpaceAvailable = (long)((ulong)this.Header.AllocatedSizeOfEntries);
			this._entries = new List<IndexEntry>();
			int num = (int)this.Header.OffsetToFirstEntry;
			while ((long)num < (long)((ulong)this.Header.TotalSizeOfEntries))
			{
				IndexEntry indexEntry = new IndexEntry(index.IsFileIndex);
				indexEntry.Read(buffer, offset + num);
				this._entries.Add(indexEntry);
				if ((indexEntry.Flags & IndexEntryFlags.End) != IndexEntryFlags.None)
				{
					break;
				}
				num += indexEntry.Size;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001AA RID: 426 RVA: 0x000094ED File Offset: 0x000076ED
		public IEnumerable<IndexEntry> Entries
		{
			get
			{
				return this._entries;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000094F5 File Offset: 0x000076F5
		public IndexHeader Header { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00009500 File Offset: 0x00007700
		private long SpaceFree
		{
			get
			{
				long num = 0L;
				for (int i = 0; i < this._entries.Count; i++)
				{
					num += (long)this._entries[i].Size;
				}
				int num2 = MathUtilities.RoundUp(16 + this._storageOverhead, 8);
				return this.TotalSpaceAvailable - (num + (long)num2);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00009556 File Offset: 0x00007756
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000955E File Offset: 0x0000775E
		internal long TotalSpaceAvailable { get; set; }

		// Token: 0x060001AF RID: 431 RVA: 0x00009567 File Offset: 0x00007767
		public void AddEntry(byte[] key, byte[] data)
		{
			if (this.AddEntry(new IndexEntry(key, data, this._index.IsFileIndex)) != null)
			{
				throw new IOException("Error adding entry - root overflowed");
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009590 File Offset: 0x00007790
		public void UpdateEntry(byte[] key, byte[] data)
		{
			int i = 0;
			while (i < this._entries.Count)
			{
				IndexEntry indexEntry = this._entries[i];
				if (this._index.Compare(key, indexEntry.KeyBuffer) == 0)
				{
					IndexEntry indexEntry2 = new IndexEntry(indexEntry, key, data);
					if (this._entries[i].Size != indexEntry2.Size)
					{
						throw new NotImplementedException("Changing index entry sizes");
					}
					this._entries[i] = indexEntry2;
					this._store();
					return;
				}
				else
				{
					i++;
				}
			}
			throw new IOException("No such index entry");
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009624 File Offset: 0x00007824
		public bool TryFindEntry(byte[] key, out IndexEntry entry, out IndexNode node)
		{
			foreach (IndexEntry indexEntry in this._entries)
			{
				if ((indexEntry.Flags & IndexEntryFlags.End) != IndexEntryFlags.None)
				{
					if ((indexEntry.Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
					{
						return this._index.GetSubBlock(indexEntry).Node.TryFindEntry(key, out entry, out node);
					}
					break;
				}
				else
				{
					int num = this._index.Compare(key, indexEntry.KeyBuffer);
					if (num == 0)
					{
						entry = indexEntry;
						node = this;
						return true;
					}
					if (num < 0 && (indexEntry.Flags & (IndexEntryFlags.Node | IndexEntryFlags.End)) != IndexEntryFlags.None)
					{
						return this._index.GetSubBlock(indexEntry).Node.TryFindEntry(key, out entry, out node);
					}
				}
			}
			entry = null;
			node = null;
			return false;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000096FC File Offset: 0x000078FC
		public virtual ushort WriteTo(byte[] buffer, int offset)
		{
			bool flag = false;
			uint num = 0U;
			foreach (IndexEntry indexEntry in this._entries)
			{
				num += (uint)indexEntry.Size;
				flag |= ((indexEntry.Flags & IndexEntryFlags.Node) > IndexEntryFlags.None);
			}
			this.Header.OffsetToFirstEntry = (uint)MathUtilities.RoundUp(16 + this._storageOverhead, 8);
			this.Header.TotalSizeOfEntries = num + this.Header.OffsetToFirstEntry;
			this.Header.HasChildNodes = (flag ? 1 : 0);
			this.Header.WriteTo(buffer, offset);
			int num2 = (int)this.Header.OffsetToFirstEntry;
			foreach (IndexEntry indexEntry2 in this._entries)
			{
				indexEntry2.WriteTo(buffer, offset + num2);
				num2 += indexEntry2.Size;
			}
			return 16;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00009818 File Offset: 0x00007A18
		public int CalcEntriesSize()
		{
			int num = 0;
			foreach (IndexEntry indexEntry in this._entries)
			{
				num += indexEntry.Size;
			}
			return num;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00009870 File Offset: 0x00007A70
		public virtual int CalcSize()
		{
			return MathUtilities.RoundUp(16 + this._storageOverhead, 8) + this.CalcEntriesSize();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009888 File Offset: 0x00007A88
		public int GetEntry(byte[] key, out bool exactMatch)
		{
			for (int i = 0; i < this._entries.Count; i++)
			{
				IndexEntry indexEntry = this._entries[i];
				if ((indexEntry.Flags & IndexEntryFlags.End) != IndexEntryFlags.None)
				{
					exactMatch = false;
					return i;
				}
				int num = this._index.Compare(key, indexEntry.KeyBuffer);
				if (num <= 0)
				{
					exactMatch = (num == 0);
					return i;
				}
			}
			throw new IOException("Corrupt index node - no End entry");
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000098F0 File Offset: 0x00007AF0
		public bool RemoveEntry(byte[] key, out IndexEntry newParentEntry)
		{
			bool flag;
			int entry = this.GetEntry(key, out flag);
			IndexEntry indexEntry = this._entries[entry];
			if (flag)
			{
				if ((indexEntry.Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
				{
					IndexNode node = this._index.GetSubBlock(indexEntry).Node;
					IndexEntry indexEntry2 = node.FindLargestLeaf();
					byte[] keyBuffer = indexEntry2.KeyBuffer;
					byte[] dataBuffer = indexEntry2.DataBuffer;
					IndexEntry indexEntry3;
					node.RemoveEntry(keyBuffer, out indexEntry3);
					indexEntry.KeyBuffer = keyBuffer;
					indexEntry.DataBuffer = dataBuffer;
					if (indexEntry3 != null)
					{
						this.InsertEntryThisNode(indexEntry3);
					}
					indexEntry3 = this.LiftNode(entry);
					if (indexEntry3 != null)
					{
						this.InsertEntryThisNode(indexEntry3);
					}
					indexEntry3 = this.PopulateEnd();
					if (indexEntry3 != null)
					{
						this.InsertEntryThisNode(indexEntry3);
					}
					newParentEntry = this.EnsureNodeSize();
				}
				else
				{
					this._entries.RemoveAt(entry);
					newParentEntry = null;
				}
				this._store();
				return true;
			}
			IndexEntry indexEntry4;
			if ((indexEntry.Flags & IndexEntryFlags.Node) != IndexEntryFlags.None && this._index.GetSubBlock(indexEntry).Node.RemoveEntry(key, out indexEntry4))
			{
				if (indexEntry4 != null)
				{
					this.InsertEntryThisNode(indexEntry4);
				}
				indexEntry4 = this.LiftNode(entry);
				if (indexEntry4 != null)
				{
					this.InsertEntryThisNode(indexEntry4);
				}
				indexEntry4 = this.PopulateEnd();
				if (indexEntry4 != null)
				{
					this.InsertEntryThisNode(indexEntry4);
				}
				newParentEntry = this.EnsureNodeSize();
				this._store();
				return true;
			}
			newParentEntry = null;
			return false;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00009A30 File Offset: 0x00007C30
		internal bool Depose()
		{
			if (!this._isRoot)
			{
				throw new InvalidOperationException("Only valid on root node");
			}
			if (this._entries.Count == 1)
			{
				return false;
			}
			IndexEntry indexEntry = new IndexEntry(this._index.IsFileIndex);
			indexEntry.Flags = IndexEntryFlags.End;
			this._index.AllocateBlock(indexEntry).Node.SetEntries(this._entries, 0, this._entries.Count);
			this._entries.Clear();
			this._entries.Add(indexEntry);
			return true;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00009AB8 File Offset: 0x00007CB8
		private IndexEntry LiftNode(int entryIndex)
		{
			if ((this._entries[entryIndex].Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
			{
				IndexNode node = this._index.GetSubBlock(this._entries[entryIndex]).Node;
				if (node._entries.Count == 1)
				{
					long childrenVirtualCluster = this._entries[entryIndex].ChildrenVirtualCluster;
					this._entries[entryIndex].Flags = ((this._entries[entryIndex].Flags & ~IndexEntryFlags.Node) | (node._entries[0].Flags & IndexEntryFlags.Node));
					this._entries[entryIndex].ChildrenVirtualCluster = node._entries[0].ChildrenVirtualCluster;
					this._index.FreeBlock(childrenVirtualCluster);
				}
				if ((this._entries[entryIndex].Flags & (IndexEntryFlags.Node | IndexEntryFlags.End)) == IndexEntryFlags.None)
				{
					IndexEntry newEntry = this._entries[entryIndex];
					this._entries.RemoveAt(entryIndex);
					return this._index.GetSubBlock(this._entries[entryIndex]).Node.AddEntry(newEntry);
				}
			}
			return null;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00009BD8 File Offset: 0x00007DD8
		private IndexEntry PopulateEnd()
		{
			if (this._entries.Count > 1 && this._entries[this._entries.Count - 1].Flags == IndexEntryFlags.End && (this._entries[this._entries.Count - 2].Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
			{
				IndexEntry indexEntry = this._entries[this._entries.Count - 2];
				this._entries.RemoveAt(this._entries.Count - 2);
				this._entries[this._entries.Count - 1].ChildrenVirtualCluster = indexEntry.ChildrenVirtualCluster;
				this._entries[this._entries.Count - 1].Flags |= IndexEntryFlags.Node;
				indexEntry.ChildrenVirtualCluster = 0L;
				indexEntry.Flags = IndexEntryFlags.None;
				return this._index.GetSubBlock(this._entries[this._entries.Count - 1]).Node.AddEntry(indexEntry);
			}
			return null;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009CF8 File Offset: 0x00007EF8
		private void InsertEntryThisNode(IndexEntry newEntry)
		{
			bool flag;
			int entry = this.GetEntry(newEntry.KeyBuffer, out flag);
			if (flag)
			{
				throw new InvalidOperationException("Entry already exists");
			}
			this._entries.Insert(entry, newEntry);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009D30 File Offset: 0x00007F30
		private IndexEntry AddEntry(IndexEntry newEntry)
		{
			bool flag;
			int entry = this.GetEntry(newEntry.KeyBuffer, out flag);
			if (flag)
			{
				throw new InvalidOperationException("Entry already exists");
			}
			if ((this._entries[entry].Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
			{
				IndexEntry indexEntry = this._index.GetSubBlock(this._entries[entry]).Node.AddEntry(newEntry);
				if (indexEntry == null)
				{
					return null;
				}
				this.InsertEntryThisNode(indexEntry);
			}
			else
			{
				this._entries.Insert(entry, newEntry);
			}
			IndexEntry result = this.EnsureNodeSize();
			this._store();
			return result;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009DBE File Offset: 0x00007FBE
		private IndexEntry EnsureNodeSize()
		{
			if (this.SpaceFree < 0L)
			{
				if (!this._isRoot)
				{
					return this.Divide();
				}
				this.Depose();
			}
			return null;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00009DE4 File Offset: 0x00007FE4
		private IndexEntry FindLargestLeaf()
		{
			if ((this._entries[this._entries.Count - 1].Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
			{
				return this._index.GetSubBlock(this._entries[this._entries.Count - 1]).Node.FindLargestLeaf();
			}
			if (this._entries.Count > 1 && (this._entries[this._entries.Count - 2].Flags & IndexEntryFlags.Node) == IndexEntryFlags.None)
			{
				return this._entries[this._entries.Count - 2];
			}
			throw new IOException("Invalid index node found");
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009E94 File Offset: 0x00008094
		private IndexEntry Divide()
		{
			int num = this._entries.Count / 2;
			IndexEntry indexEntry = this._entries[num];
			IndexEntry indexEntry2 = new IndexEntry(this._index.IsFileIndex);
			indexEntry2.Flags |= IndexEntryFlags.End;
			List<IndexEntry> list = new List<IndexEntry>(num + 1);
			for (int i = 0; i < num; i++)
			{
				list.Add(this._entries[i]);
			}
			list.Add(indexEntry2);
			if ((indexEntry.Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
			{
				indexEntry2.ChildrenVirtualCluster = indexEntry.ChildrenVirtualCluster;
				indexEntry2.Flags |= IndexEntryFlags.Node;
			}
			this._index.AllocateBlock(indexEntry).Node.SetEntries(list, 0, list.Count);
			this._entries.RemoveRange(0, num + 1);
			return indexEntry;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009F60 File Offset: 0x00008160
		private void SetEntries(IList<IndexEntry> newEntries, int offset, int count)
		{
			this._entries.Clear();
			for (int i = 0; i < count; i++)
			{
				this._entries.Add(newEntries[i + offset]);
			}
			if (count == 0 || (this._entries[this._entries.Count - 1].Flags & IndexEntryFlags.End) == IndexEntryFlags.None)
			{
				IndexEntry indexEntry = new IndexEntry(this._index.IsFileIndex);
				indexEntry.Flags = IndexEntryFlags.End;
				this._entries.Add(indexEntry);
			}
			if (this.SpaceFree < 0L)
			{
				throw new IOException("Error setting node entries - oversized for node");
			}
			this._store();
		}

		// Token: 0x040000DA RID: 218
		private readonly List<IndexEntry> _entries;

		// Token: 0x040000DB RID: 219
		private readonly Index _index;

		// Token: 0x040000DC RID: 220
		private readonly bool _isRoot;

		// Token: 0x040000DD RID: 221
		private readonly int _storageOverhead;

		// Token: 0x040000DE RID: 222
		private readonly IndexNodeSaveFn _store;
	}
}
