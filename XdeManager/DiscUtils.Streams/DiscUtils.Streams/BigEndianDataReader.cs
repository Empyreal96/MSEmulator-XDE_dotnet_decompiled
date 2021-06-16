using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000020 RID: 32
	public class BigEndianDataReader : DataReader
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x00004375 File Offset: 0x00002575
		public BigEndianDataReader(Stream stream) : base(stream)
		{
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000437E File Offset: 0x0000257E
		public override ushort ReadUInt16()
		{
			base.ReadToBuffer(2);
			return EndianUtilities.ToUInt16BigEndian(this._buffer, 0);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004393 File Offset: 0x00002593
		public override int ReadInt32()
		{
			base.ReadToBuffer(4);
			return EndianUtilities.ToInt32BigEndian(this._buffer, 0);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000043A8 File Offset: 0x000025A8
		public override uint ReadUInt32()
		{
			base.ReadToBuffer(4);
			return EndianUtilities.ToUInt32BigEndian(this._buffer, 0);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000043BD File Offset: 0x000025BD
		public override long ReadInt64()
		{
			base.ReadToBuffer(8);
			return EndianUtilities.ToInt64BigEndian(this._buffer, 0);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000043D2 File Offset: 0x000025D2
		public override ulong ReadUInt64()
		{
			base.ReadToBuffer(8);
			return EndianUtilities.ToUInt64BigEndian(this._buffer, 0);
		}
	}
}
