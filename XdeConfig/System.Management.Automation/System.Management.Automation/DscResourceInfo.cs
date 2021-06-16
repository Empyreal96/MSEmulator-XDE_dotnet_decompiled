using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200006B RID: 107
	public class DscResourceInfo
	{
		// Token: 0x060005BB RID: 1467 RVA: 0x0001A7B5 File Offset: 0x000189B5
		internal DscResourceInfo(string name, string friendlyName, string path, string parentPath, ExecutionContext context)
		{
			this.Name = name;
			this.FriendlyName = friendlyName;
			this.Path = path;
			this.ParentPath = parentPath;
			this.Properties = new ReadOnlyCollection<DscResourcePropertyInfo>(new List<DscResourcePropertyInfo>());
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0001A7F5 File Offset: 0x000189F5
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x0001A7FD File Offset: 0x000189FD
		public string Name { get; private set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0001A806 File Offset: 0x00018A06
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x0001A80E File Offset: 0x00018A0E
		public string ResourceType { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x0001A817 File Offset: 0x00018A17
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x0001A81F File Offset: 0x00018A1F
		public string FriendlyName { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0001A828 File Offset: 0x00018A28
		// (set) Token: 0x060005C3 RID: 1475 RVA: 0x0001A830 File Offset: 0x00018A30
		public string Path { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0001A839 File Offset: 0x00018A39
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x0001A841 File Offset: 0x00018A41
		public string ParentPath { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x0001A84A File Offset: 0x00018A4A
		// (set) Token: 0x060005C7 RID: 1479 RVA: 0x0001A852 File Offset: 0x00018A52
		public ImplementedAsType ImplementedAs { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x0001A85B File Offset: 0x00018A5B
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x0001A863 File Offset: 0x00018A63
		public string CompanyName { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0001A86C File Offset: 0x00018A6C
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x0001A874 File Offset: 0x00018A74
		public ReadOnlyCollection<DscResourcePropertyInfo> Properties { get; private set; }

		// Token: 0x060005CC RID: 1484 RVA: 0x0001A87D File Offset: 0x00018A7D
		public void UpdateProperties(IList<DscResourcePropertyInfo> properties)
		{
			if (properties != null)
			{
				this.Properties = new ReadOnlyCollection<DscResourcePropertyInfo>(properties);
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0001A88E File Offset: 0x00018A8E
		// (set) Token: 0x060005CE RID: 1486 RVA: 0x0001A896 File Offset: 0x00018A96
		public PSModuleInfo Module { get; internal set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0001A89F File Offset: 0x00018A9F
		// (set) Token: 0x060005D0 RID: 1488 RVA: 0x0001A8A7 File Offset: 0x00018AA7
		public string HelpFile
		{
			get
			{
				return this.helpFilePath;
			}
			internal set
			{
				this.helpFilePath = value;
			}
		}

		// Token: 0x04000246 RID: 582
		private string helpFilePath = string.Empty;
	}
}
