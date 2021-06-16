using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000019 RID: 25
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class JsonExtensionDataAttribute : Attribute
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00002FC0 File Offset: 0x000011C0
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00002FC8 File Offset: 0x000011C8
		public bool WriteData { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00002FD1 File Offset: 0x000011D1
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00002FD9 File Offset: 0x000011D9
		public bool ReadData { get; set; }

		// Token: 0x06000093 RID: 147 RVA: 0x00002FE2 File Offset: 0x000011E2
		public JsonExtensionDataAttribute()
		{
			this.WriteData = true;
			this.ReadData = true;
		}
	}
}
