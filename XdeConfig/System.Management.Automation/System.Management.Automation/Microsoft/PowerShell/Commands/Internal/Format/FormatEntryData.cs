using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004E0 RID: 1248
	internal sealed class FormatEntryData : PacketInfoData
	{
		// Token: 0x06003634 RID: 13876 RVA: 0x00125EA0 File Offset: 0x001240A0
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.formatEntryInfo = (FormatEntryInfo)deserializer.DeserializeMandatoryMemberObject(so, "formatEntryInfo");
			this.outOfBand = deserializer.DeserializeBoolMemberVariable(so, "outOfBand");
			this.writeStream = deserializer.DeserializeWriteStreamTypeMemberVariable(so);
			this.isHelpObject = so.IsHelpObject;
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x00125EF7 File Offset: 0x001240F7
		public FormatEntryData() : base("27c87ef9bbda4f709f6b4002fa4af63c")
		{
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x00125F04 File Offset: 0x00124104
		internal void SetStreamTypeFromPSObject(PSObject so)
		{
			if (PSObjectHelper.IsWriteErrorStream(so))
			{
				this.writeStream = WriteStreamType.Error;
				return;
			}
			if (PSObjectHelper.IsWriteWarningStream(so))
			{
				this.writeStream = WriteStreamType.Warning;
				return;
			}
			if (PSObjectHelper.IsWriteVerboseStream(so))
			{
				this.writeStream = WriteStreamType.Verbose;
				return;
			}
			if (PSObjectHelper.IsWriteDebugStream(so))
			{
				this.writeStream = WriteStreamType.Debug;
				return;
			}
			if (PSObjectHelper.IsWriteInformationStream(so))
			{
				this.writeStream = WriteStreamType.Information;
				return;
			}
			this.writeStream = WriteStreamType.None;
		}

		// Token: 0x04001BA3 RID: 7075
		internal const string CLSID = "27c87ef9bbda4f709f6b4002fa4af63c";

		// Token: 0x04001BA4 RID: 7076
		public FormatEntryInfo formatEntryInfo;

		// Token: 0x04001BA5 RID: 7077
		public bool outOfBand;

		// Token: 0x04001BA6 RID: 7078
		public WriteStreamType writeStream;

		// Token: 0x04001BA7 RID: 7079
		internal bool isHelpObject;
	}
}
