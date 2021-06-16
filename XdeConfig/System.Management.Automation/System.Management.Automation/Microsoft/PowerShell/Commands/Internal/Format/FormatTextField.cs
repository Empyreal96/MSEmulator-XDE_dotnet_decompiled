using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004ED RID: 1261
	internal sealed class FormatTextField : FormatValue
	{
		// Token: 0x0600364C RID: 13900 RVA: 0x001261D8 File Offset: 0x001243D8
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.text = deserializer.DeserializeStringMemberVariable(so, "text");
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x001261F4 File Offset: 0x001243F4
		public FormatTextField() : base("b8d9e369024a43a580b9e0c9279e3354")
		{
		}

		// Token: 0x04001BC0 RID: 7104
		internal const string CLSID = "b8d9e369024a43a580b9e0c9279e3354";

		// Token: 0x04001BC1 RID: 7105
		public string text;
	}
}
