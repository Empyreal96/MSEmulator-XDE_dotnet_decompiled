using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000010 RID: 16
	internal sealed class Bitmap : IDisposable
	{
		// Token: 0x06000054 RID: 84 RVA: 0x0000354F File Offset: 0x0000174F
		public Bitmap(Stream stream, long maxIndex)
		{
			this._stream = stream;
			this._maxIndex = maxIndex;
			this._bitmap = new BlockCacheStream(SparseStream.FromStream(stream, Ownership.None), Ownership.None);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003578 File Offset: 0x00001778
		public void Dispose()
		{
			if (this._bitmap != null)
			{
				this._bitmap.Dispose();
				this._bitmap = null;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003594 File Offset: 0x00001794
		public bool IsPresent(long index)
		{
			long index2 = index / 8L;
			int num = 1 << (int)(index % 8L);
			return ((int)this.GetByte(index2) & num) != 0;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000035C0 File Offset: 0x000017C0
		public void MarkPresent(long index)
		{
			long num = index / 8L;
			byte b = (byte)(1 << (int)((byte)(index % 8L)));
			if (num >= this._bitmap.Length)
			{
				this._bitmap.Position = MathUtilities.RoundUp(num + 1L, 8L) - 1L;
				this._bitmap.WriteByte(0);
			}
			this.SetByte(num, this.GetByte(num) | b);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003624 File Offset: 0x00001824
		public void MarkPresentRange(long index, long count)
		{
			if (count <= 0L)
			{
				return;
			}
			long num = index / 8L;
			long num2 = (index + count - 1L) / 8L;
			if (num2 >= this._bitmap.Length)
			{
				this._bitmap.Position = MathUtilities.RoundUp(num2 + 1L, 8L) - 1L;
				this._bitmap.WriteByte(0);
			}
			byte[] array = new byte[num2 - num + 1L];
			array[0] = this.GetByte(num);
			if (array.Length != 1)
			{
				array[array.Length - 1] = this.GetByte(num2);
			}
			for (long num3 = index; num3 < index + count; num3 += 1L)
			{
				long num4 = num3 / 8L - num;
				byte b = (byte)(1 << (int)((byte)(num3 % 8L)));
				byte[] array2 = array;
				IntPtr intPtr = checked((IntPtr)num4);
				array2[(int)intPtr] = (array2[(int)intPtr] | b);
			}
			this.SetBytes(num, array);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000036E0 File Offset: 0x000018E0
		public void MarkAbsent(long index)
		{
			long num = index / 8L;
			byte b = (byte)(1 << (int)((byte)(index % 8L)));
			if (num < this._stream.Length)
			{
				this.SetByte(num, this.GetByte(num) & ~b);
			}
			if (index < this._nextAvailable)
			{
				this._nextAvailable = index;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003730 File Offset: 0x00001930
		internal void MarkAbsentRange(long index, long count)
		{
			if (count <= 0L)
			{
				return;
			}
			long num = index / 8L;
			long num2 = (index + count - 1L) / 8L;
			if (num2 >= this._bitmap.Length)
			{
				this._bitmap.Position = MathUtilities.RoundUp(num2 + 1L, 8L) - 1L;
				this._bitmap.WriteByte(0);
			}
			byte[] array = new byte[num2 - num + 1L];
			array[0] = this.GetByte(num);
			if (array.Length != 1)
			{
				array[array.Length - 1] = this.GetByte(num2);
			}
			for (long num3 = index; num3 < index + count; num3 += 1L)
			{
				long num4 = num3 / 8L - num;
				byte b = (byte)(1 << (int)((byte)(num3 % 8L)));
				byte[] array2 = array;
				IntPtr intPtr = checked((IntPtr)num4);
				array2[(int)intPtr] = (array2[(int)intPtr] & ~b);
			}
			this.SetBytes(num, array);
			if (index < this._nextAvailable)
			{
				this._nextAvailable = index;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003800 File Offset: 0x00001A00
		internal long AllocateFirstAvailable(long minValue)
		{
			long num = Math.Max(minValue, this._nextAvailable);
			while (this.IsPresent(num) && num < this._maxIndex)
			{
				num += 1L;
			}
			if (num < this._maxIndex)
			{
				this.MarkPresent(num);
				this._nextAvailable = num + 1L;
				return num;
			}
			return -1L;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003854 File Offset: 0x00001A54
		internal long SetTotalEntries(long numEntries)
		{
			long num = MathUtilities.RoundUp(MathUtilities.Ceil(numEntries, 8L), 8L);
			this._stream.SetLength(num);
			return num * 8L;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003881 File Offset: 0x00001A81
		internal long Size
		{
			get
			{
				return this._bitmap.Length;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003890 File Offset: 0x00001A90
		internal byte GetByte(long index)
		{
			if (index >= this._bitmap.Length)
			{
				return 0;
			}
			byte[] array = new byte[1];
			this._bitmap.Position = index;
			if (this._bitmap.Read(array, 0, 1) != 0)
			{
				return array[0];
			}
			return 0;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000038D8 File Offset: 0x00001AD8
		internal int GetBytes(long index, byte[] buffer, int offset, int count)
		{
			if (index + (long)count >= this._bitmap.Length)
			{
				count = (int)(this._bitmap.Length - index);
			}
			if (count <= 0)
			{
				return 0;
			}
			this._bitmap.Position = index;
			return this._bitmap.Read(buffer, offset, count);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x0000392C File Offset: 0x00001B2C
		private void SetByte(long index, byte value)
		{
			byte[] buffer = new byte[]
			{
				value
			};
			this._bitmap.Position = index;
			this._bitmap.Write(buffer, 0, 1);
			this._bitmap.Flush();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003969 File Offset: 0x00001B69
		private void SetBytes(long index, byte[] buffer)
		{
			this._bitmap.Position = index;
			this._bitmap.Write(buffer, 0, buffer.Length);
			this._bitmap.Flush();
		}

		// Token: 0x0400005A RID: 90
		private BlockCacheStream _bitmap;

		// Token: 0x0400005B RID: 91
		private readonly long _maxIndex;

		// Token: 0x0400005C RID: 92
		private long _nextAvailable;

		// Token: 0x0400005D RID: 93
		private readonly Stream _stream;
	}
}
