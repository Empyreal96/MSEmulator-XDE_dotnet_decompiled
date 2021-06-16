using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000053 RID: 83
	internal sealed class UpperCase : IComparer<string>
	{
		// Token: 0x060003BE RID: 958 RVA: 0x00015038 File Offset: 0x00013238
		public UpperCase(File file)
		{
			using (Stream stream = file.OpenStream(AttributeType.Data, null, FileAccess.Read))
			{
				this._table = new char[stream.Length / 2L];
				byte[] buffer = StreamUtilities.ReadExact(stream, (int)stream.Length);
				for (int i = 0; i < this._table.Length; i++)
				{
					this._table[i] = (char)EndianUtilities.ToUInt16LittleEndian(buffer, i * 2);
				}
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000150BC File Offset: 0x000132BC
		public int Compare(string x, string y)
		{
			int num = Math.Min(x.Length, y.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = (int)(this._table[(int)x[i]] - this._table[(int)y[i]]);
				if (num2 != 0)
				{
					return num2;
				}
			}
			return x.Length - y.Length;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00015118 File Offset: 0x00013318
		public int Compare(byte[] x, int xOffset, int xLength, byte[] y, int yOffset, int yLength)
		{
			int num = Math.Min(xLength, yLength) / 2;
			for (int i = 0; i < num; i++)
			{
				char c = (char)((int)x[xOffset + i * 2] | (int)x[xOffset + i * 2 + 1] << 8);
				char c2 = (char)((int)y[yOffset + i * 2] | (int)y[yOffset + i * 2 + 1] << 8);
				int num2 = (int)(this._table[(int)c] - this._table[(int)c2]);
				if (num2 != 0)
				{
					return num2;
				}
			}
			return xLength - yLength;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00015188 File Offset: 0x00013388
		internal static UpperCase Initialize(File file)
		{
			byte[] array = new byte[131072];
			for (int i = 0; i <= 65535; i++)
			{
				EndianUtilities.WriteBytesLittleEndian((ushort)char.ToUpperInvariant((char)i), array, i * 2);
			}
			using (Stream stream = file.OpenStream(AttributeType.Data, null, FileAccess.ReadWrite))
			{
				stream.Write(array, 0, array.Length);
			}
			return new UpperCase(file);
		}

		// Token: 0x04000190 RID: 400
		private readonly char[] _table;
	}
}
