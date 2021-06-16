using System;

namespace DiscUtils.Compression
{
	// Token: 0x02000086 RID: 134
	internal sealed class InverseBurrowsWheeler : DataBlockTransform
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x0000DD0F File Offset: 0x0000BF0F
		public InverseBurrowsWheeler(int bufferSize)
		{
			this._pointers = new int[bufferSize];
			this._nextPos = new int[256];
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0000DD33 File Offset: 0x0000BF33
		protected override bool BuffersMustNotOverlap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0000DD36 File Offset: 0x0000BF36
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x0000DD3E File Offset: 0x0000BF3E
		public int OriginalIndex { get; set; }

		// Token: 0x060004AD RID: 1197 RVA: 0x0000DD48 File Offset: 0x0000BF48
		protected override int DoProcess(byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset)
		{
			Array.Clear(this._nextPos, 0, this._nextPos.Length);
			for (int i = inputOffset; i < inputOffset + inputCount; i++)
			{
				this._nextPos[(int)input[i]]++;
			}
			int num = 0;
			for (int j = 0; j < 256; j++)
			{
				int num2 = num;
				num += this._nextPos[j];
				this._nextPos[j] = num2;
			}
			for (int k = 0; k < inputCount; k++)
			{
				int[] pointers = this._pointers;
				int[] nextPos = this._nextPos;
				byte b = input[inputOffset + k];
				int num3 = nextPos[(int)b];
				nextPos[(int)b] = num3 + 1;
				pointers[num3] = k;
			}
			int num4 = this._pointers[this.OriginalIndex];
			for (int l = 0; l < inputCount; l++)
			{
				output[outputOffset + l] = input[inputOffset + num4];
				num4 = this._pointers[num4];
			}
			return inputCount;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000DE26 File Offset: 0x0000C026
		protected override int MaxOutputCount(int inputCount)
		{
			return inputCount;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000DE29 File Offset: 0x0000C029
		protected override int MinOutputCount(int inputCount)
		{
			return inputCount;
		}

		// Token: 0x040001B7 RID: 439
		private readonly int[] _nextPos;

		// Token: 0x040001B8 RID: 440
		private readonly int[] _pointers;
	}
}
