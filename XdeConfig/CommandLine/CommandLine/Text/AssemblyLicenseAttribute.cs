using System;
using System.Runtime.InteropServices;

namespace CommandLine.Text
{
	// Token: 0x02000052 RID: 82
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(false)]
	public sealed class AssemblyLicenseAttribute : MultilineTextAttribute
	{
		// Token: 0x060001DD RID: 477 RVA: 0x000084B2 File Offset: 0x000066B2
		public AssemblyLicenseAttribute(string line1) : base(line1)
		{
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000084BB File Offset: 0x000066BB
		public AssemblyLicenseAttribute(string line1, string line2) : base(line1, line2)
		{
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000084C5 File Offset: 0x000066C5
		public AssemblyLicenseAttribute(string line1, string line2, string line3) : base(line1, line2, line3)
		{
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000084D0 File Offset: 0x000066D0
		public AssemblyLicenseAttribute(string line1, string line2, string line3, string line4) : base(line1, line2, line3, line4)
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000084DD File Offset: 0x000066DD
		public AssemblyLicenseAttribute(string line1, string line2, string line3, string line4, string line5) : base(line1, line2, line3, line4, line5)
		{
		}
	}
}
