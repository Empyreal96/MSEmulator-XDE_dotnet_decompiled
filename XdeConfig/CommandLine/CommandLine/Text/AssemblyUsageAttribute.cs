using System;
using System.Runtime.InteropServices;

namespace CommandLine.Text
{
	// Token: 0x02000053 RID: 83
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(false)]
	public sealed class AssemblyUsageAttribute : MultilineTextAttribute
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x000084EC File Offset: 0x000066EC
		public AssemblyUsageAttribute(string line1) : base(line1)
		{
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000084F5 File Offset: 0x000066F5
		public AssemblyUsageAttribute(string line1, string line2) : base(line1, line2)
		{
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000084FF File Offset: 0x000066FF
		public AssemblyUsageAttribute(string line1, string line2, string line3) : base(line1, line2, line3)
		{
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000850A File Offset: 0x0000670A
		public AssemblyUsageAttribute(string line1, string line2, string line3, string line4) : base(line1, line2, line3, line4)
		{
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00008517 File Offset: 0x00006717
		public AssemblyUsageAttribute(string line1, string line2, string line3, string line4, string line5) : base(line1, line2, line3, line4, line5)
		{
		}
	}
}
