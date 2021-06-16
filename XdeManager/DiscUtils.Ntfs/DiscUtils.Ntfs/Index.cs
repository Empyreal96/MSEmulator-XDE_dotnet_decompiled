using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000028 RID: 40
	internal class Index : IDisposable
	{
		// Token: 0x06000171 RID: 369 RVA: 0x00008384 File Offset: 0x00006584
		public Index(File file, string name, BiosParameterBlock bpb, UpperCase upCase)
		{
			this._file = file;
			this._name = name;
			this._bpb = bpb;
			this.IsFileIndex = (name == "$I30");
			this._blockCache = new ObjectCache<long, IndexBlock>();
			this._root = this._file.GetStream(AttributeType.IndexRoot, this._name).GetContent<IndexRoot>();
			this._comparer = this._root.GetCollator(upCase);
			using (Stream stream = this._file.OpenStream(AttributeType.IndexRoot, this._name, FileAccess.Read))
			{
				byte[] buffer = StreamUtilities.ReadExact(stream, (int)stream.Length);
				this._rootNode = new IndexNode(new IndexNodeSaveFn(this.WriteRootNodeToDisk), 0, this, true, buffer, 16);
				this._rootNode.TotalSpaceAvailable += (long)(this._file.MftRecordFreeSpace(AttributeType.IndexRoot, this._name) - 100);
			}
			if (this._file.StreamExists(AttributeType.IndexAllocation, this._name))
			{
				this.AllocationStream = this._file.OpenStream(AttributeType.IndexAllocation, this._name, FileAccess.ReadWrite);
			}
			if (this._file.StreamExists(AttributeType.Bitmap, this._name))
			{
				this._indexBitmap = new Bitmap(this._file.OpenStream(AttributeType.Bitmap, this._name, FileAccess.ReadWrite), long.MaxValue);
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008500 File Offset: 0x00006700
		private Index(AttributeType attrType, AttributeCollationRule collationRule, File file, string name, BiosParameterBlock bpb, UpperCase upCase)
		{
			this._file = file;
			this._name = name;
			this._bpb = bpb;
			this.IsFileIndex = (name == "$I30");
			this._blockCache = new ObjectCache<long, IndexBlock>();
			this._file.CreateStream(AttributeType.IndexRoot, this._name);
			this._root = new IndexRoot
			{
				AttributeType = (uint)attrType,
				CollationRule = collationRule,
				IndexAllocationSize = (uint)bpb.IndexBufferSize,
				RawClustersPerIndexRecord = bpb.RawIndexBufferSize
			};
			this._comparer = this._root.GetCollator(upCase);
			this._rootNode = new IndexNode(new IndexNodeSaveFn(this.WriteRootNodeToDisk), 0, this, true, 32U);
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000085C0 File Offset: 0x000067C0
		// (set) Token: 0x06000174 RID: 372 RVA: 0x000085C8 File Offset: 0x000067C8
		internal Stream AllocationStream { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000085D4 File Offset: 0x000067D4
		public int Count
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<byte[], byte[]> keyValuePair in this.Entries)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00008624 File Offset: 0x00006824
		public IEnumerable<KeyValuePair<byte[], byte[]>> Entries
		{
			get
			{
				foreach (IndexEntry indexEntry in this.Enumerate(this._rootNode))
				{
					yield return new KeyValuePair<byte[], byte[]>(indexEntry.KeyBuffer, indexEntry.DataBuffer);
				}
				IEnumerator<IndexEntry> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00008634 File Offset: 0x00006834
		internal uint IndexBufferSize
		{
			get
			{
				return this._root.IndexAllocationSize;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00008641 File Offset: 0x00006841
		internal bool IsFileIndex { get; }

		// Token: 0x1700005D RID: 93
		public byte[] this[byte[] key]
		{
			get
			{
				byte[] result;
				if (this.TryGetValue(key, out result))
				{
					return result;
				}
				throw new KeyNotFoundException();
			}
			set
			{
				this._rootNode.TotalSpaceAvailable = (long)(this._rootNode.CalcSize() + this._file.MftRecordFreeSpace(AttributeType.IndexRoot, this._name));
				IndexEntry indexEntry;
				IndexNode indexNode;
				if (this._rootNode.TryFindEntry(key, out indexEntry, out indexNode))
				{
					indexNode.UpdateEntry(key, value);
					return;
				}
				this._rootNode.AddEntry(key, value);
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000086CF File Offset: 0x000068CF
		public void Dispose()
		{
			if (this._indexBitmap != null)
			{
				this._indexBitmap.Dispose();
				this._indexBitmap = null;
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000086EB File Offset: 0x000068EB
		public static void Create(AttributeType attrType, AttributeCollationRule collationRule, File file, string name)
		{
			new Index(attrType, collationRule, file, name, file.Context.BiosParameterBlock, file.Context.UpperCase).WriteRootNodeToDisk();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00008711 File Offset: 0x00006911
		public IEnumerable<KeyValuePair<byte[], byte[]>> FindAll(IComparable<byte[]> query)
		{
			foreach (IndexEntry indexEntry in this.FindAllIn(query, this._rootNode))
			{
				yield return new KeyValuePair<byte[], byte[]>(indexEntry.KeyBuffer, indexEntry.DataBuffer);
			}
			IEnumerator<IndexEntry> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00008728 File Offset: 0x00006928
		public bool ContainsKey(byte[] key)
		{
			byte[] array;
			return this.TryGetValue(key, out array);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00008740 File Offset: 0x00006940
		public bool Remove(byte[] key)
		{
			this._rootNode.TotalSpaceAvailable = (long)(this._rootNode.CalcSize() + this._file.MftRecordFreeSpace(AttributeType.IndexRoot, this._name));
			IndexEntry indexEntry;
			bool result = this._rootNode.RemoveEntry(key, out indexEntry);
			if (indexEntry != null)
			{
				throw new IOException("Error removing entry, root overflowed");
			}
			return result;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008798 File Offset: 0x00006998
		public bool TryGetValue(byte[] key, out byte[] value)
		{
			IndexEntry indexEntry;
			IndexNode indexNode;
			if (this._rootNode.TryFindEntry(key, out indexEntry, out indexNode))
			{
				value = indexEntry.DataBuffer;
				return true;
			}
			value = null;
			return false;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000087C8 File Offset: 0x000069C8
		internal static string EntryAsString(IndexEntry entry, string fileName, string indexName)
		{
			IByteArraySerializable byteArraySerializable = null;
			IByteArraySerializable byteArraySerializable2 = null;
			if (indexName == "$I30")
			{
				byteArraySerializable = new FileNameRecord();
				byteArraySerializable2 = default(FileRecordReference);
			}
			else if (fileName == "$ObjId" && indexName == "$O")
			{
				byteArraySerializable = new ObjectIds.IndexKey();
				byteArraySerializable2 = new ObjectIdRecord();
			}
			else if (fileName == "$Reparse" && indexName == "$R")
			{
				byteArraySerializable = new ReparsePoints.Key();
				byteArraySerializable2 = new ReparsePoints.Data();
			}
			else if (fileName == "$Quota")
			{
				if (indexName == "$O")
				{
					byteArraySerializable = new Quotas.OwnerKey();
					byteArraySerializable2 = new Quotas.OwnerRecord();
				}
				else if (indexName == "$Q")
				{
					byteArraySerializable = new Quotas.OwnerRecord();
					byteArraySerializable2 = new Quotas.QuotaRecord();
				}
			}
			else if (fileName == "$Secure")
			{
				if (indexName == "$SII")
				{
					byteArraySerializable = new SecurityDescriptors.IdIndexKey();
					byteArraySerializable2 = new SecurityDescriptors.IdIndexData();
				}
				else if (indexName == "$SDH")
				{
					byteArraySerializable = new SecurityDescriptors.HashIndexKey();
					byteArraySerializable2 = new SecurityDescriptors.IdIndexData();
				}
			}
			try
			{
				if (byteArraySerializable != null && byteArraySerializable2 != null)
				{
					byteArraySerializable.ReadFrom(entry.KeyBuffer, 0);
					byteArraySerializable2.ReadFrom(entry.DataBuffer, 0);
					return string.Concat(new object[]
					{
						"{",
						byteArraySerializable,
						"-->",
						byteArraySerializable2,
						"}"
					});
				}
			}
			catch
			{
				return "{Parsing-Error}";
			}
			return "{Unknown-Index-Type}";
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008950 File Offset: 0x00006B50
		internal long IndexBlockVcnToPosition(long vcn)
		{
			if (vcn % (long)((ulong)this._root.RawClustersPerIndexRecord) != 0L)
			{
				throw new NotSupportedException(string.Concat(new object[]
				{
					"Unexpected vcn (not a multiple of clusters-per-index-record): vcn=",
					vcn,
					" rcpir=",
					this._root.RawClustersPerIndexRecord
				}));
			}
			if ((long)this._bpb.BytesPerCluster <= (long)((ulong)this._root.IndexAllocationSize))
			{
				return vcn * (long)this._bpb.BytesPerCluster;
			}
			if (this._root.RawClustersPerIndexRecord != 8)
			{
				throw new NotSupportedException("Unexpected RawClustersPerIndexRecord (multiple index blocks per cluster): " + this._root.RawClustersPerIndexRecord);
			}
			return vcn / (long)((ulong)this._root.RawClustersPerIndexRecord) * (long)((ulong)this._root.IndexAllocationSize);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008A1C File Offset: 0x00006C1C
		internal bool ShrinkRoot()
		{
			if (this._rootNode.Depose())
			{
				this.WriteRootNodeToDisk();
				this._rootNode.TotalSpaceAvailable = (long)(this._rootNode.CalcSize() + this._file.MftRecordFreeSpace(AttributeType.IndexRoot, this._name));
				return true;
			}
			return false;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008A70 File Offset: 0x00006C70
		internal IndexBlock GetSubBlock(IndexEntry parentEntry)
		{
			IndexBlock indexBlock = this._blockCache[parentEntry.ChildrenVirtualCluster];
			if (indexBlock == null)
			{
				indexBlock = new IndexBlock(this, false, parentEntry, this._bpb);
				this._blockCache[parentEntry.ChildrenVirtualCluster] = indexBlock;
			}
			return indexBlock;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008AB4 File Offset: 0x00006CB4
		internal IndexBlock AllocateBlock(IndexEntry parentEntry)
		{
			if (this.AllocationStream == null)
			{
				this._file.CreateStream(AttributeType.IndexAllocation, this._name);
				this.AllocationStream = this._file.OpenStream(AttributeType.IndexAllocation, this._name, FileAccess.ReadWrite);
			}
			if (this._indexBitmap == null)
			{
				this._file.CreateStream(AttributeType.Bitmap, this._name);
				this._indexBitmap = new Bitmap(this._file.OpenStream(AttributeType.Bitmap, this._name, FileAccess.ReadWrite), long.MaxValue);
			}
			long num = this._indexBitmap.AllocateFirstAvailable(0L);
			parentEntry.ChildrenVirtualCluster = num * (long)MathUtilities.Ceil(this._bpb.IndexBufferSize, (int)((ushort)this._bpb.SectorsPerCluster * this._bpb.BytesPerSector));
			parentEntry.Flags |= IndexEntryFlags.Node;
			IndexBlock indexBlock = IndexBlock.Initialize(this, false, parentEntry, this._bpb);
			this._blockCache[parentEntry.ChildrenVirtualCluster] = indexBlock;
			return indexBlock;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008BB8 File Offset: 0x00006DB8
		internal void FreeBlock(long vcn)
		{
			long index = vcn / (long)MathUtilities.Ceil(this._bpb.IndexBufferSize, (int)((ushort)this._bpb.SectorsPerCluster * this._bpb.BytesPerSector));
			this._indexBitmap.MarkAbsent(index);
			this._blockCache.Remove(vcn);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008C08 File Offset: 0x00006E08
		internal int Compare(byte[] x, byte[] y)
		{
			return this._comparer.Compare(x, y);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008C17 File Offset: 0x00006E17
		internal void Dump(TextWriter writer, string prefix)
		{
			this.NodeAsString(writer, prefix, this._rootNode, "R");
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008C2C File Offset: 0x00006E2C
		protected IEnumerable<IndexEntry> Enumerate(IndexNode node)
		{
			foreach (IndexEntry focus in node.Entries)
			{
				if ((focus.Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
				{
					IndexBlock subBlock = this.GetSubBlock(focus);
					foreach (IndexEntry indexEntry in this.Enumerate(subBlock.Node))
					{
						yield return indexEntry;
					}
					IEnumerator<IndexEntry> enumerator2 = null;
				}
				if ((focus.Flags & IndexEntryFlags.End) == IndexEntryFlags.None)
				{
					yield return focus;
				}
				focus = null;
			}
			IEnumerator<IndexEntry> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008C43 File Offset: 0x00006E43
		private IEnumerable<IndexEntry> FindAllIn(IComparable<byte[]> query, IndexNode node)
		{
			foreach (IndexEntry focus in node.Entries)
			{
				bool flag = true;
				bool matches = false;
				bool keepIterating = true;
				if ((focus.Flags & IndexEntryFlags.End) == IndexEntryFlags.None)
				{
					int num = query.CompareTo(focus.KeyBuffer);
					if (num == 0)
					{
						matches = true;
					}
					else if (num > 0)
					{
						flag = false;
					}
					else if (num < 0)
					{
						keepIterating = false;
					}
				}
				if (flag && (focus.Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
				{
					IndexBlock subBlock = this.GetSubBlock(focus);
					foreach (IndexEntry indexEntry in this.FindAllIn(query, subBlock.Node))
					{
						yield return indexEntry;
					}
					IEnumerator<IndexEntry> enumerator2 = null;
				}
				if (matches)
				{
					yield return focus;
				}
				if (!keepIterating)
				{
					yield break;
				}
				focus = null;
			}
			IEnumerator<IndexEntry> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008C64 File Offset: 0x00006E64
		private void WriteRootNodeToDisk()
		{
			this._rootNode.Header.AllocatedSizeOfEntries = (uint)this._rootNode.CalcSize();
			byte[] array = new byte[(ulong)this._rootNode.Header.AllocatedSizeOfEntries + (ulong)((long)this._root.Size)];
			this._root.WriteTo(array, 0);
			this._rootNode.WriteTo(array, this._root.Size);
			using (Stream stream = this._file.OpenStream(AttributeType.IndexRoot, this._name, FileAccess.Write))
			{
				stream.Position = 0L;
				stream.Write(array, 0, array.Length);
				stream.SetLength(stream.Position);
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008D2C File Offset: 0x00006F2C
		private void NodeAsString(TextWriter writer, string prefix, IndexNode node, string id)
		{
			writer.WriteLine(prefix + id + ":");
			foreach (IndexEntry indexEntry in node.Entries)
			{
				if ((indexEntry.Flags & IndexEntryFlags.End) != IndexEntryFlags.None)
				{
					writer.WriteLine(prefix + "      E");
				}
				else
				{
					writer.WriteLine(prefix + "      " + Index.EntryAsString(indexEntry, this._file.BestName, this._name));
				}
				if ((indexEntry.Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
				{
					this.NodeAsString(writer, prefix + "        ", this.GetSubBlock(indexEntry).Node, ":i" + indexEntry.ChildrenVirtualCluster);
				}
			}
		}

		// Token: 0x040000BA RID: 186
		private readonly ObjectCache<long, IndexBlock> _blockCache;

		// Token: 0x040000BB RID: 187
		protected BiosParameterBlock _bpb;

		// Token: 0x040000BC RID: 188
		private readonly IComparer<byte[]> _comparer;

		// Token: 0x040000BD RID: 189
		protected File _file;

		// Token: 0x040000BE RID: 190
		private Bitmap _indexBitmap;

		// Token: 0x040000BF RID: 191
		protected string _name;

		// Token: 0x040000C0 RID: 192
		private readonly IndexRoot _root;

		// Token: 0x040000C1 RID: 193
		private readonly IndexNode _rootNode;
	}
}
