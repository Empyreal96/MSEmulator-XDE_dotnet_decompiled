using System;
using System.IO;

namespace DiscUtils.Compression
{
	// Token: 0x0200007F RID: 127
	internal class BZip2CombinedHuffmanTrees
	{
		// Token: 0x06000477 RID: 1143 RVA: 0x0000D2D4 File Offset: 0x0000B4D4
		public BZip2CombinedHuffmanTrees(BitStream bitstream, int maxSymbols)
		{
			this._bitstream = bitstream;
			this.Initialize(maxSymbols);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000D2EC File Offset: 0x0000B4EC
		public uint NextSymbol()
		{
			if (this._symbolsToNextSelector == 0)
			{
				this._symbolsToNextSelector = 50;
				this._activeTree = this._trees[(int)this._selectors[this._nextSelector]];
				this._nextSelector++;
			}
			this._symbolsToNextSelector--;
			return this._activeTree.NextSymbol(this._bitstream);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000D350 File Offset: 0x0000B550
		private void Initialize(int maxSymbols)
		{
			int num = (int)this._bitstream.Read(3);
			if (num < 2 || num > 6)
			{
				throw new InvalidDataException("Invalid number of tables");
			}
			int num2 = (int)this._bitstream.Read(15);
			if (num2 < 1)
			{
				throw new InvalidDataException("Invalid number of selectors");
			}
			this._selectors = new byte[num2];
			MoveToFront moveToFront = new MoveToFront(num, true);
			for (int i = 0; i < num2; i++)
			{
				this._selectors[i] = moveToFront.GetAndMove((int)this.CountSetBits(num));
			}
			this._trees = new HuffmanTree[num];
			for (int j = 0; j < num; j++)
			{
				uint[] array = new uint[maxSymbols];
				uint num3 = this._bitstream.Read(5);
				for (int k = 0; k < maxSymbols; k++)
				{
					if (num3 < 1U || num3 > 20U)
					{
						throw new InvalidDataException("Invalid length constructing Huffman tree");
					}
					while (this._bitstream.Read(1) != 0U)
					{
						num3 = ((this._bitstream.Read(1) == 0U) ? (num3 + 1U) : (num3 - 1U));
						if (num3 < 1U || num3 > 20U)
						{
							throw new InvalidDataException("Invalid length constructing Huffman tree");
						}
					}
					array[k] = num3;
				}
				this._trees[j] = new HuffmanTree(array);
			}
			this._symbolsToNextSelector = 0;
			this._nextSelector = 0;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000D490 File Offset: 0x0000B690
		private byte CountSetBits(int max)
		{
			byte b = 0;
			while (this._bitstream.Read(1) != 0U)
			{
				b += 1;
				if ((int)b >= max)
				{
					throw new InvalidDataException("Exceeded max number of consecutive bits");
				}
			}
			return b;
		}

		// Token: 0x04000195 RID: 405
		private HuffmanTree _activeTree;

		// Token: 0x04000196 RID: 406
		private readonly BitStream _bitstream;

		// Token: 0x04000197 RID: 407
		private int _nextSelector;

		// Token: 0x04000198 RID: 408
		private byte[] _selectors;

		// Token: 0x04000199 RID: 409
		private int _symbolsToNextSelector;

		// Token: 0x0400019A RID: 410
		private HuffmanTree[] _trees;
	}
}
