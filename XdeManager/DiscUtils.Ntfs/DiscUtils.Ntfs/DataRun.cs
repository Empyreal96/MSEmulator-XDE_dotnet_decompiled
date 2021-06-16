using System;
using System.Globalization;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000016 RID: 22
	internal class DataRun
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00004CB8 File Offset: 0x00002EB8
		public DataRun()
		{
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004CC0 File Offset: 0x00002EC0
		public DataRun(long offset, long length, bool isSparse)
		{
			this.RunOffset = offset;
			this.RunLength = length;
			this.IsSparse = isSparse;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00004CDD File Offset: 0x00002EDD
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00004CE5 File Offset: 0x00002EE5
		public bool IsSparse { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004CEE File Offset: 0x00002EEE
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00004CF6 File Offset: 0x00002EF6
		public long RunLength { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004CFF File Offset: 0x00002EFF
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00004D07 File Offset: 0x00002F07
		public long RunOffset { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004D10 File Offset: 0x00002F10
		internal int Size
		{
			get
			{
				int num = DataRun.VarLongSize(this.RunLength);
				int num2 = DataRun.VarLongSize(this.RunOffset);
				return 1 + num + num2;
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004D3C File Offset: 0x00002F3C
		public int Read(byte[] buffer, int offset)
		{
			int num = buffer[offset] >> 4 & 15;
			int num2 = (int)(buffer[offset] & 15);
			this.RunLength = DataRun.ReadVarLong(buffer, offset + 1, num2);
			this.RunOffset = DataRun.ReadVarLong(buffer, offset + 1 + num2, num);
			this.IsSparse = (num == 0);
			return 1 + num2 + num;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004D8A File Offset: 0x00002F8A
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0:+##;-##;0}[+{1}]", new object[]
			{
				this.RunOffset,
				this.RunLength
			});
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004DC0 File Offset: 0x00002FC0
		internal int Write(byte[] buffer, int offset)
		{
			int num = DataRun.WriteVarLong(buffer, offset + 1, this.RunLength);
			int num2 = this.IsSparse ? 0 : DataRun.WriteVarLong(buffer, offset + 1 + num, this.RunOffset);
			buffer[offset] = (byte)((num & 15) | (num2 << 4 & 240));
			return 1 + num + num2;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004E14 File Offset: 0x00003014
		private static long ReadVarLong(byte[] buffer, int offset, int size)
		{
			ulong num = 0UL;
			bool flag = false;
			for (int i = 0; i < size; i++)
			{
				byte b = buffer[offset + i];
				num |= (ulong)b << i * 8;
				flag = ((b & 128) > 0);
			}
			if (flag)
			{
				for (int j = size; j < 8; j++)
				{
					num |= 255UL << j * 8;
				}
			}
			return (long)num;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004E74 File Offset: 0x00003074
		private static int WriteVarLong(byte[] buffer, int offset, long val)
		{
			bool flag = val >= 0L;
			int num = 0;
			do
			{
				buffer[offset + num] = (byte)(val & 255L);
				val >>= 8;
				num++;
			}
			while (val != 0L && val != -1L);
			if (flag && (buffer[offset + num - 1] & 128) != 0)
			{
				buffer[offset + num] = 0;
				num++;
			}
			else if (!flag && (buffer[offset + num - 1] & 128) != 128)
			{
				buffer[offset + num] = byte.MaxValue;
				num++;
			}
			return num;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004EF0 File Offset: 0x000030F0
		private static int VarLongSize(long val)
		{
			bool flag = val >= 0L;
			int num = 0;
			bool flag2;
			do
			{
				flag2 = ((val & 128L) != 0L);
				val >>= 8;
				num++;
			}
			while (val != 0L && val != -1L);
			if ((flag && flag2) || (!flag && !flag2))
			{
				num++;
			}
			return num;
		}
	}
}
