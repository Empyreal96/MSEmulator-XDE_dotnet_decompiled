using System;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x0200005E RID: 94
	internal sealed class DiskGroupRecord : DatabaseRecord
	{
		// Token: 0x060003CC RID: 972 RVA: 0x0000A5B4 File Offset: 0x000087B4
		protected override void DoReadFrom(byte[] buffer, int offset)
		{
			base.DoReadFrom(buffer, offset);
			int num = offset + 24;
			this.Id = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.Name = DatabaseRecord.ReadVarString(buffer, ref num);
			if ((this.Flags & 240U) == 64U)
			{
				this.GroupGuidString = DatabaseRecord.ReadBinaryGuid(buffer, ref num).ToString();
			}
			else
			{
				this.GroupGuidString = DatabaseRecord.ReadVarString(buffer, ref num);
			}
			this.Unknown1 = DatabaseRecord.ReadUInt(buffer, ref num);
		}

		// Token: 0x04000124 RID: 292
		public string GroupGuidString;

		// Token: 0x04000125 RID: 293
		public uint Unknown1;
	}
}
