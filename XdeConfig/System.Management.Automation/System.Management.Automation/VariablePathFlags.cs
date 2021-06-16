using System;

namespace System.Management.Automation
{
	// Token: 0x02000846 RID: 2118
	[Flags]
	internal enum VariablePathFlags
	{
		// Token: 0x040029CC RID: 10700
		None = 0,
		// Token: 0x040029CD RID: 10701
		Local = 1,
		// Token: 0x040029CE RID: 10702
		Script = 2,
		// Token: 0x040029CF RID: 10703
		Global = 4,
		// Token: 0x040029D0 RID: 10704
		Private = 8,
		// Token: 0x040029D1 RID: 10705
		Variable = 16,
		// Token: 0x040029D2 RID: 10706
		Function = 32,
		// Token: 0x040029D3 RID: 10707
		DriveQualified = 64,
		// Token: 0x040029D4 RID: 10708
		Unqualified = 128,
		// Token: 0x040029D5 RID: 10709
		UnscopedVariableMask = 111
	}
}
