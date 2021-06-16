using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004DD RID: 1245
	internal abstract class StartData : ControlInfoData
	{
		// Token: 0x0600362E RID: 13870 RVA: 0x00125DD4 File Offset: 0x00123FD4
		internal override void Deserialize(PSObject so, FormatObjectDeserializer deserializer)
		{
			base.Deserialize(so, deserializer);
			this.shapeInfo = (ShapeInfo)deserializer.DeserializeMemberObject(so, "shapeInfo");
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x00125DF5 File Offset: 0x00123FF5
		public StartData(string clsid) : base(clsid)
		{
		}

		// Token: 0x04001B9C RID: 7068
		public ShapeInfo shapeInfo;
	}
}
