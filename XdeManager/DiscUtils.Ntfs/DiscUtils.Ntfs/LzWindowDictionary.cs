using System;
using System.Collections.Generic;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000033 RID: 51
	internal sealed class LzWindowDictionary
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x0000A808 File Offset: 0x00008A08
		public LzWindowDictionary()
		{
			this.Initalize();
			this._offsetList = new List<int>[256];
			for (int i = 0; i < this._offsetList.Length; i++)
			{
				this._offsetList[i] = new List<int>();
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000A851 File Offset: 0x00008A51
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000A859 File Offset: 0x00008A59
		private int BlockSize { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000A862 File Offset: 0x00008A62
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000A86A File Offset: 0x00008A6A
		public int MaxMatchAmount { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000A873 File Offset: 0x00008A73
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000A87B File Offset: 0x00008A7B
		public int MinMatchAmount { get; set; }

		// Token: 0x060001FE RID: 510 RVA: 0x0000A884 File Offset: 0x00008A84
		public void Reset()
		{
			this.Initalize();
			for (int i = 0; i < this._offsetList.Length; i++)
			{
				this._offsetList[i].Clear();
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A8B8 File Offset: 0x00008AB8
		public int[] Search(byte[] decompressedData, int decompressedDataOffset, uint index, uint length)
		{
			this.RemoveOldEntries(decompressedData[(int)(checked((IntPtr)(unchecked((long)decompressedDataOffset + (long)((ulong)index)))))]);
			int[] array = new int[2];
			if (index < 1U || (ulong)(length - index) < (ulong)((long)this.MinMatchAmount))
			{
				return array;
			}
			for (int i = 0; i < this._offsetList[(int)decompressedData[(int)(checked((IntPtr)(unchecked((long)decompressedDataOffset + (long)((ulong)index)))))]].Count; i++)
			{
				int num = this._offsetList[(int)decompressedData[(int)(checked((IntPtr)(unchecked((long)decompressedDataOffset + (long)((ulong)index)))))]][i];
				int num2 = 1;
				if ((ulong)index - (ulong)((long)num) > (ulong)((long)this.BlockSize))
				{
					break;
				}
				int num3 = (int)Math.Min((long)Math.Min(this.MaxMatchAmount, this.BlockSize), Math.Min((long)((ulong)(length - index)), (long)((ulong)length - (ulong)((long)num))));
				while (num2 < num3 && decompressedData[(int)(checked((IntPtr)(unchecked((long)decompressedDataOffset + (long)((ulong)index) + (long)num2))))] == decompressedData[decompressedDataOffset + num + num2])
				{
					num2++;
				}
				if (num2 >= this.MinMatchAmount && num2 > array[1])
				{
					array = new int[]
					{
						(int)((ulong)index - (ulong)((long)num)),
						num2
					};
					if (num2 == this.MaxMatchAmount)
					{
						break;
					}
				}
			}
			return array;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A9B2 File Offset: 0x00008BB2
		public void AddEntry(byte[] decompressedData, int decompressedDataOffset, int index)
		{
			this._offsetList[(int)decompressedData[decompressedDataOffset + index]].Add(index);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A9C8 File Offset: 0x00008BC8
		public void AddEntryRange(byte[] decompressedData, int decompressedDataOffset, int index, int length)
		{
			for (int i = 0; i < length; i++)
			{
				this.AddEntry(decompressedData, decompressedDataOffset, index + i);
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A9ED File Offset: 0x00008BED
		private void Initalize()
		{
			this.MinMatchAmount = 3;
			this.MaxMatchAmount = 18;
			this.BlockSize = 4096;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000AA09 File Offset: 0x00008C09
		private void RemoveOldEntries(byte index)
		{
			while (this._offsetList[(int)index].Count > 256)
			{
				this._offsetList[(int)index].RemoveAt(0);
			}
		}

		// Token: 0x040000EB RID: 235
		private readonly List<int>[] _offsetList;
	}
}
