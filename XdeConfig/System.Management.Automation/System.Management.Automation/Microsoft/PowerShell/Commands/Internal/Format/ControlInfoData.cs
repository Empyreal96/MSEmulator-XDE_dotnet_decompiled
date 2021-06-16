using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004DC RID: 1244
	internal abstract class ControlInfoData : PacketInfoData
	{
		// Token: 0x0600362C RID: 13868 RVA: 0x00125DAA File Offset: 0x00123FAA
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.groupingEntry = (GroupingEntry)deserializer.DeserializeMemberObject(so, "groupingEntry");
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x00125DCB File Offset: 0x00123FCB
		public ControlInfoData(string clsid) : base(clsid)
		{
		}

		// Token: 0x04001B9B RID: 7067
		public GroupingEntry groupingEntry;
	}
}
