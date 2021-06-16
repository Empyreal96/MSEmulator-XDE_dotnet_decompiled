using System;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x02000011 RID: 17
	internal sealed class ValueCell : Cell
	{
		// Token: 0x06000098 RID: 152 RVA: 0x000051CC File Offset: 0x000033CC
		public ValueCell(string name) : this(-1)
		{
			this.Name = name;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000051DC File Offset: 0x000033DC
		public ValueCell(int index) : base(index)
		{
			this.DataIndex = -1;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000051EC File Offset: 0x000033EC
		// (set) Token: 0x0600009B RID: 155 RVA: 0x000051F4 File Offset: 0x000033F4
		public int DataIndex { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000051FD File Offset: 0x000033FD
		// (set) Token: 0x0600009D RID: 157 RVA: 0x00005205 File Offset: 0x00003405
		public int DataLength { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600009E RID: 158 RVA: 0x0000520E File Offset: 0x0000340E
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00005216 File Offset: 0x00003416
		public RegistryValueType DataType { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000521F File Offset: 0x0000341F
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00005227 File Offset: 0x00003427
		public string Name { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00005230 File Offset: 0x00003430
		public override int Size
		{
			get
			{
				return 20 + (string.IsNullOrEmpty(this.Name) ? 0 : this.Name.Length);
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005250 File Offset: 0x00003450
		public override int ReadFrom(byte[] buffer, int offset)
		{
			int num = (int)EndianUtilities.ToUInt16LittleEndian(buffer, offset + 2);
			this.DataLength = EndianUtilities.ToInt32LittleEndian(buffer, offset + 4);
			this.DataIndex = EndianUtilities.ToInt32LittleEndian(buffer, offset + 8);
			this.DataType = (RegistryValueType)EndianUtilities.ToInt32LittleEndian(buffer, offset + 12);
			this._flags = (ValueFlags)EndianUtilities.ToUInt16LittleEndian(buffer, offset + 16);
			if ((this._flags & ValueFlags.Named) != ~(ValueFlags.Named | ValueFlags.Unknown0002 | ValueFlags.Unknown0004 | ValueFlags.Unknown0008 | ValueFlags.Unknown0010 | ValueFlags.Unknown0020 | ValueFlags.Unknown0040 | ValueFlags.Unknown0080 | ValueFlags.Unknown0100 | ValueFlags.Unknown0200 | ValueFlags.Unknown0400 | ValueFlags.Unknown0800 | ValueFlags.Unknown1000 | ValueFlags.Unknown2000 | ValueFlags.Unknown4000 | ValueFlags.Unknown8000))
			{
				this.Name = EndianUtilities.BytesToString(buffer, offset + 20, num).Trim(new char[1]);
			}
			return 20 + num;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000052D0 File Offset: 0x000034D0
		public override void WriteTo(byte[] buffer, int offset)
		{
			int num;
			if (string.IsNullOrEmpty(this.Name))
			{
				this._flags &= ~ValueFlags.Named;
				num = 0;
			}
			else
			{
				this._flags |= ValueFlags.Named;
				num = this.Name.Length;
			}
			EndianUtilities.StringToBytes("vk", buffer, offset, 2);
			EndianUtilities.WriteBytesLittleEndian(num, buffer, offset + 2);
			EndianUtilities.WriteBytesLittleEndian(this.DataLength, buffer, offset + 4);
			EndianUtilities.WriteBytesLittleEndian(this.DataIndex, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian((int)this.DataType, buffer, offset + 12);
			EndianUtilities.WriteBytesLittleEndian((ushort)this._flags, buffer, offset + 16);
			if (num != 0)
			{
				EndianUtilities.StringToBytes(this.Name, buffer, offset + 20, num);
			}
		}

		// Token: 0x0400005B RID: 91
		private ValueFlags _flags;
	}
}
