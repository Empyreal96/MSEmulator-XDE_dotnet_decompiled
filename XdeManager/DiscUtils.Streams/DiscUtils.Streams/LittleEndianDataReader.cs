using System;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000024 RID: 36
	public class LittleEndianDataReader : DataReader
	{
		// Token: 0x06000117 RID: 279 RVA: 0x0000454F File Offset: 0x0000274F
		public LittleEndianDataReader(Stream stream) : base(stream)
		{
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00004558 File Offset: 0x00002758
		public override ushort ReadUInt16()
		{
			base.ReadToBuffer(2);
			return EndianUtilities.ToUInt16LittleEndian(this._buffer, 0);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000456D File Offset: 0x0000276D
		public override int ReadInt32()
		{
			base.ReadToBuffer(4);
			return EndianUtilities.ToInt32LittleEndian(this._buffer, 0);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00004582 File Offset: 0x00002782
		public override uint ReadUInt32()
		{
			base.ReadToBuffer(4);
			return EndianUtilities.ToUInt32LittleEndian(this._buffer, 0);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00004597 File Offset: 0x00002797
		public override long ReadInt64()
		{
			base.ReadToBuffer(8);
			return EndianUtilities.ToInt64LittleEndian(this._buffer, 0);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x000045AC File Offset: 0x000027AC
		public override ulong ReadUInt64()
		{
			base.ReadToBuffer(8);
			return EndianUtilities.ToUInt64LittleEndian(this._buffer, 0);
		}
	}
}
