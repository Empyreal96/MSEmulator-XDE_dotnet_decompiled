using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200009E RID: 158
	public sealed class PSClassInfo
	{
		// Token: 0x0600079D RID: 1949 RVA: 0x00025C4C File Offset: 0x00023E4C
		internal PSClassInfo(string name)
		{
			this.Name = name;
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x00025C66 File Offset: 0x00023E66
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x00025C6E File Offset: 0x00023E6E
		public string Name { get; private set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00025C77 File Offset: 0x00023E77
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x00025C7F File Offset: 0x00023E7F
		public ReadOnlyCollection<PSClassMemberInfo> Members { get; private set; }

		// Token: 0x060007A2 RID: 1954 RVA: 0x00025C88 File Offset: 0x00023E88
		public void UpdateMembers(IList<PSClassMemberInfo> members)
		{
			if (members != null)
			{
				this.Members = new ReadOnlyCollection<PSClassMemberInfo>(members);
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x00025C99 File Offset: 0x00023E99
		// (set) Token: 0x060007A4 RID: 1956 RVA: 0x00025CA1 File Offset: 0x00023EA1
		public PSModuleInfo Module { get; internal set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x00025CAA File Offset: 0x00023EAA
		// (set) Token: 0x060007A6 RID: 1958 RVA: 0x00025CB2 File Offset: 0x00023EB2
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

		// Token: 0x0400037B RID: 891
		private string helpFilePath = string.Empty;
	}
}
