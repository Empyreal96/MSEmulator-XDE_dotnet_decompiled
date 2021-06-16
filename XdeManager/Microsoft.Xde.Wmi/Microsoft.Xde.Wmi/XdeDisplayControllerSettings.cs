using System;
using System.Drawing;
using System.Management;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Wmi
{
	// Token: 0x0200000C RID: 12
	public class XdeDisplayControllerSettings : IXdeDisplayControllerSettings
	{
		// Token: 0x0600007A RID: 122 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public XdeDisplayControllerSettings(ManagementObject managementObject)
		{
			ValidationUtilities.CheckNotNull(managementObject, "managementObject");
			if ((string)managementObject["__CLASS"] != "Msvm_SyntheticDisplayControllerSettingData")
			{
				throw new ArgumentException(XdeVmExceptions.InvalidWmiObject, "managementObject");
			}
			this.managementObject = managementObject;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003BF5 File Offset: 0x00001DF5
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003C0C File Offset: 0x00001E0C
		public ResolutionType ResolutionType
		{
			get
			{
				return (ResolutionType)((byte)this.managementObject["ResolutionType"]);
			}
			set
			{
				this.managementObject["ResolutionType"] = (byte)value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003C28 File Offset: 0x00001E28
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00003C66 File Offset: 0x00001E66
		public Size Resolution
		{
			get
			{
				int width = (int)((ushort)this.managementObject["HorizontalResolution"]);
				int height = (int)((ushort)this.managementObject["VerticalResolution"]);
				return new Size(width, height);
			}
			set
			{
				this.managementObject["HorizontalResolution"] = (ushort)value.Width;
				this.managementObject["VerticalResolution"] = (ushort)value.Height;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003CA2 File Offset: 0x00001EA2
		public ManagementObject ManagementObject
		{
			get
			{
				return this.managementObject;
			}
		}

		// Token: 0x04000010 RID: 16
		private const string ResolutionTypeName = "ResolutionType";

		// Token: 0x04000011 RID: 17
		private const string HorizontalResolutionName = "HorizontalResolution";

		// Token: 0x04000012 RID: 18
		private const string VerticalResolutionName = "VerticalResolution";

		// Token: 0x04000013 RID: 19
		private ManagementObject managementObject;
	}
}
