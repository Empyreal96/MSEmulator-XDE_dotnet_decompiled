using System;

namespace System.Management.Automation
{
	// Token: 0x02000066 RID: 102
	[Serializable]
	public class PSSnapInSpecification
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x0001A18B File Offset: 0x0001838B
		internal PSSnapInSpecification(string psSnapinName)
		{
			PSSnapInInfo.VerifyPSSnapInFormatThrowIfError(psSnapinName);
			this.Name = psSnapinName;
			this.Version = null;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0001A1A7 File Offset: 0x000183A7
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x0001A1AF File Offset: 0x000183AF
		public string Name { get; internal set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001A1B8 File Offset: 0x000183B8
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x0001A1C0 File Offset: 0x000183C0
		public Version Version { get; internal set; }
	}
}
