using System;
using System.ComponentModel;

namespace System.Management.Automation
{
	// Token: 0x02000135 RID: 309
	[Flags]
	[TypeConverter(typeof(LanguagePrimitives.EnumMultipleTypeConverter))]
	public enum PSMemberViewTypes
	{
		// Token: 0x04000720 RID: 1824
		Extended = 1,
		// Token: 0x04000721 RID: 1825
		Adapted = 2,
		// Token: 0x04000722 RID: 1826
		Base = 4,
		// Token: 0x04000723 RID: 1827
		All = 7
	}
}
