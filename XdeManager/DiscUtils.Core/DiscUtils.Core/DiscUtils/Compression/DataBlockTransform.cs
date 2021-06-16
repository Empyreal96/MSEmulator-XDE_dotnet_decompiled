using System;
using System.Globalization;

namespace DiscUtils.Compression
{
	// Token: 0x02000084 RID: 132
	internal abstract class DataBlockTransform
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600049F RID: 1183
		protected abstract bool BuffersMustNotOverlap { get; }

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000DB20 File Offset: 0x0000BD20
		public int Process(byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset)
		{
			if ((long)output.Length < (long)outputOffset + (long)this.MinOutputCount(inputCount))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Output buffer to small, must be at least {0} bytes may need to be {1} bytes", new object[]
				{
					this.MinOutputCount(inputCount),
					this.MaxOutputCount(inputCount)
				}));
			}
			if (this.BuffersMustNotOverlap)
			{
				int num = this.MaxOutputCount(inputCount);
				if (input == output && (long)inputOffset + (long)inputCount > (long)outputOffset && (long)inputOffset <= (long)outputOffset + (long)num)
				{
					byte[] array = new byte[num];
					int num2 = this.DoProcess(input, inputOffset, inputCount, array, 0);
					Array.Copy(array, 0, output, outputOffset, num2);
					return num2;
				}
			}
			return this.DoProcess(input, inputOffset, inputCount, output, outputOffset);
		}

		// Token: 0x060004A1 RID: 1185
		protected abstract int DoProcess(byte[] input, int inputOffset, int inputCount, byte[] output, int outputOffset);

		// Token: 0x060004A2 RID: 1186
		protected abstract int MaxOutputCount(int inputCount);

		// Token: 0x060004A3 RID: 1187
		protected abstract int MinOutputCount(int inputCount);
	}
}
