using System;
using System.Collections.Generic;

namespace DiscUtils.Streams
{
	// Token: 0x02000004 RID: 4
	public class BlockCache<T> where T : Block, new()
	{
		// Token: 0x06000011 RID: 17 RVA: 0x0000247C File Offset: 0x0000067C
		public BlockCache(int blockSize, int blockCount)
		{
			this._blockSize = blockSize;
			this._totalBlocks = blockCount;
			this._blocks = new Dictionary<long, T>();
			this._lru = new LinkedList<T>();
			this._freeBlocks = new List<T>(this._totalBlocks);
			this.FreeBlockCount = this._totalBlocks;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000024D0 File Offset: 0x000006D0
		// (set) Token: 0x06000013 RID: 19 RVA: 0x000024D8 File Offset: 0x000006D8
		public int FreeBlockCount { get; private set; }

		// Token: 0x06000014 RID: 20 RVA: 0x000024E1 File Offset: 0x000006E1
		public bool ContainsBlock(long position)
		{
			return this._blocks.ContainsKey(position);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024EF File Offset: 0x000006EF
		public bool TryGetBlock(long position, out T block)
		{
			if (this._blocks.TryGetValue(position, out block))
			{
				this._lru.Remove(block);
				this._lru.AddFirst(block);
				return true;
			}
			return false;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002528 File Offset: 0x00000728
		public T GetBlock(long position)
		{
			T freeBlock;
			if (this.TryGetBlock(position, out freeBlock))
			{
				return freeBlock;
			}
			freeBlock = this.GetFreeBlock();
			freeBlock.Position = position;
			freeBlock.Available = -1;
			this.StoreBlock(freeBlock);
			return freeBlock;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000256C File Offset: 0x0000076C
		public void ReleaseBlock(long position)
		{
			T t;
			if (this._blocks.TryGetValue(position, out t))
			{
				this._blocks.Remove(position);
				this._lru.Remove(t);
				this._freeBlocks.Add(t);
				int freeBlockCount = this.FreeBlockCount;
				this.FreeBlockCount = freeBlockCount + 1;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000025BF File Offset: 0x000007BF
		private void StoreBlock(T block)
		{
			this._blocks[block.Position] = block;
			this._lru.AddFirst(block);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025E8 File Offset: 0x000007E8
		private T GetFreeBlock()
		{
			T t;
			if (this._freeBlocks.Count > 0)
			{
				int index = this._freeBlocks.Count - 1;
				t = this._freeBlocks[index];
				this._freeBlocks.RemoveAt(index);
				int freeBlockCount = this.FreeBlockCount;
				this.FreeBlockCount = freeBlockCount - 1;
			}
			else if (this._blocksCreated < this._totalBlocks)
			{
				t = Activator.CreateInstance<T>();
				t.Data = new byte[this._blockSize];
				this._blocksCreated++;
				int freeBlockCount = this.FreeBlockCount;
				this.FreeBlockCount = freeBlockCount - 1;
			}
			else
			{
				t = this._lru.Last.Value;
				this._lru.RemoveLast();
				this._blocks.Remove(t.Position);
			}
			return t;
		}

		// Token: 0x04000007 RID: 7
		private readonly Dictionary<long, T> _blocks;

		// Token: 0x04000008 RID: 8
		private int _blocksCreated;

		// Token: 0x04000009 RID: 9
		private readonly int _blockSize;

		// Token: 0x0400000A RID: 10
		private readonly List<T> _freeBlocks;

		// Token: 0x0400000B RID: 11
		private readonly LinkedList<T> _lru;

		// Token: 0x0400000C RID: 12
		private readonly int _totalBlocks;
	}
}
