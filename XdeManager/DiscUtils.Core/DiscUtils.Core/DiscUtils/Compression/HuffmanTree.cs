using System;

namespace DiscUtils.Compression
{
	// Token: 0x02000085 RID: 133
	internal sealed class HuffmanTree
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x0000DBD8 File Offset: 0x0000BDD8
		public HuffmanTree(uint[] lengths)
		{
			this.Lengths = lengths;
			this._numSymbols = lengths.Length;
			uint num = 0U;
			for (int i = 0; i < this.Lengths.Length; i++)
			{
				if (this.Lengths[i] > num)
				{
					num = this.Lengths[i];
				}
			}
			this._numBits = (int)num;
			this._buffer = new uint[1 << this._numBits];
			this.Build();
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0000DC47 File Offset: 0x0000BE47
		public uint[] Lengths { get; }

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000DC50 File Offset: 0x0000BE50
		public uint NextSymbol(BitStream bitStream)
		{
			uint num = this._buffer[(int)bitStream.Peek(this._numBits)];
			bitStream.Consume((int)this.Lengths[(int)num]);
			return num;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000DC80 File Offset: 0x0000BE80
		private void Build()
		{
			int num = 0;
			for (int i = 1; i <= this._numBits; i++)
			{
				uint num2 = 0U;
				while ((ulong)num2 < (ulong)((long)this._numSymbols))
				{
					if ((ulong)this.Lengths[(int)num2] == (ulong)((long)i))
					{
						int num3 = 1 << this._numBits - i;
						for (int j = 0; j < num3; j++)
						{
							this._buffer[num + j] = num2;
						}
						num += num3;
					}
					num2 += 1U;
				}
			}
			for (int k = num; k < this._buffer.Length; k++)
			{
				this._buffer[k] = uint.MaxValue;
			}
		}

		// Token: 0x040001B3 RID: 435
		private readonly uint[] _buffer;

		// Token: 0x040001B4 RID: 436
		private readonly int _numBits;

		// Token: 0x040001B5 RID: 437
		private readonly int _numSymbols;
	}
}
