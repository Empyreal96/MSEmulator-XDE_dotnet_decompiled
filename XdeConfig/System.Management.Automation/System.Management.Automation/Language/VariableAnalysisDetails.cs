using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x020005EB RID: 1515
	internal class VariableAnalysisDetails
	{
		// Token: 0x060040D1 RID: 16593 RVA: 0x0015865E File Offset: 0x0015685E
		internal VariableAnalysisDetails()
		{
			this.AssociatedAsts = new List<Ast>();
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x060040D2 RID: 16594 RVA: 0x00158671 File Offset: 0x00156871
		// (set) Token: 0x060040D3 RID: 16595 RVA: 0x00158679 File Offset: 0x00156879
		public int BitIndex { get; set; }

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x060040D4 RID: 16596 RVA: 0x00158682 File Offset: 0x00156882
		// (set) Token: 0x060040D5 RID: 16597 RVA: 0x0015868A File Offset: 0x0015688A
		public int LocalTupleIndex { get; set; }

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x060040D6 RID: 16598 RVA: 0x00158693 File Offset: 0x00156893
		// (set) Token: 0x060040D7 RID: 16599 RVA: 0x0015869B File Offset: 0x0015689B
		public Type Type { get; set; }

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x060040D8 RID: 16600 RVA: 0x001586A4 File Offset: 0x001568A4
		// (set) Token: 0x060040D9 RID: 16601 RVA: 0x001586AC File Offset: 0x001568AC
		public string Name { get; set; }

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x060040DA RID: 16602 RVA: 0x001586B5 File Offset: 0x001568B5
		// (set) Token: 0x060040DB RID: 16603 RVA: 0x001586BD File Offset: 0x001568BD
		public bool Automatic { get; set; }

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x060040DC RID: 16604 RVA: 0x001586C6 File Offset: 0x001568C6
		// (set) Token: 0x060040DD RID: 16605 RVA: 0x001586CE File Offset: 0x001568CE
		public bool PreferenceVariable { get; set; }

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x060040DE RID: 16606 RVA: 0x001586D7 File Offset: 0x001568D7
		// (set) Token: 0x060040DF RID: 16607 RVA: 0x001586DF File Offset: 0x001568DF
		public bool Assigned { get; set; }

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x060040E0 RID: 16608 RVA: 0x001586E8 File Offset: 0x001568E8
		// (set) Token: 0x060040E1 RID: 16609 RVA: 0x001586F0 File Offset: 0x001568F0
		public List<Ast> AssociatedAsts { get; private set; }
	}
}
