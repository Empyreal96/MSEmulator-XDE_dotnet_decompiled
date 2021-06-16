using System;

namespace DiscUtils.LogicalDiskManager
{
	// Token: 0x0200005F RID: 95
	internal sealed class DiskRecord : DatabaseRecord
	{
		// Token: 0x060003CE RID: 974 RVA: 0x0000A63C File Offset: 0x0000883C
		protected override void DoReadFrom(byte[] buffer, int offset)
		{
			base.DoReadFrom(buffer, offset);
			int num = offset + 24;
			this.Id = DatabaseRecord.ReadVarULong(buffer, ref num);
			this.Name = DatabaseRecord.ReadVarString(buffer, ref num);
			if ((this.Flags & 240U) == 64U)
			{
				this.DiskGuidString = DatabaseRecord.ReadBinaryGuid(buffer, ref num).ToString();
				return;
			}
			this.DiskGuidString = DatabaseRecord.ReadVarString(buffer, ref num);
		}

		// Token: 0x04000126 RID: 294
		public string DiskGuidString;
	}
}
