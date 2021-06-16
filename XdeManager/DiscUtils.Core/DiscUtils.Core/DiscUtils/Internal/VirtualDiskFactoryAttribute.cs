using System;

namespace DiscUtils.Internal
{
	// Token: 0x02000075 RID: 117
	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class VirtualDiskFactoryAttribute : Attribute
	{
		// Token: 0x06000447 RID: 1095 RVA: 0x0000CE08 File Offset: 0x0000B008
		public VirtualDiskFactoryAttribute(string type, string fileExtensions)
		{
			this.Type = type;
			this.FileExtensions = fileExtensions.Replace(".", string.Empty).Split(new char[]
			{
				','
			});
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0000CE3D File Offset: 0x0000B03D
		public string[] FileExtensions { get; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0000CE45 File Offset: 0x0000B045
		public string Type { get; }
	}
}
