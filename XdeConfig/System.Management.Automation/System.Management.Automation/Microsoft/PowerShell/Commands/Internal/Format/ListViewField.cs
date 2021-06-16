using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004E9 RID: 1257
	internal sealed class ListViewField : FormatInfoData
	{
		// Token: 0x06003645 RID: 13893 RVA: 0x001260E8 File Offset: 0x001242E8
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.label = deserializer.DeserializeStringMemberVariable(so, "label");
			this.propertyName = deserializer.DeserializeStringMemberVariable(so, "propertyName");
			this.formatPropertyField = (FormatPropertyField)deserializer.DeserializeMandatoryMemberObject(so, "formatPropertyField");
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x00126138 File Offset: 0x00124338
		public ListViewField() : base("b761477330ce4fb2a665999879324d73")
		{
		}

		// Token: 0x04001BB7 RID: 7095
		internal const string CLSID = "b761477330ce4fb2a665999879324d73";

		// Token: 0x04001BB8 RID: 7096
		public string label;

		// Token: 0x04001BB9 RID: 7097
		public string propertyName;

		// Token: 0x04001BBA RID: 7098
		public FormatPropertyField formatPropertyField = new FormatPropertyField();
	}
}
