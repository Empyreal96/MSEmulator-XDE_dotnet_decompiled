using System;
using System.ComponentModel;

namespace System.Management.Automation
{
	// Token: 0x02000134 RID: 308
	[TypeConverter(typeof(LanguagePrimitives.EnumMultipleTypeConverter))]
	[Flags]
	public enum PSMemberTypes
	{
		// Token: 0x0400070F RID: 1807
		AliasProperty = 1,
		// Token: 0x04000710 RID: 1808
		CodeProperty = 2,
		// Token: 0x04000711 RID: 1809
		Property = 4,
		// Token: 0x04000712 RID: 1810
		NoteProperty = 8,
		// Token: 0x04000713 RID: 1811
		ScriptProperty = 16,
		// Token: 0x04000714 RID: 1812
		PropertySet = 32,
		// Token: 0x04000715 RID: 1813
		Method = 64,
		// Token: 0x04000716 RID: 1814
		CodeMethod = 128,
		// Token: 0x04000717 RID: 1815
		ScriptMethod = 256,
		// Token: 0x04000718 RID: 1816
		ParameterizedProperty = 512,
		// Token: 0x04000719 RID: 1817
		MemberSet = 1024,
		// Token: 0x0400071A RID: 1818
		Event = 2048,
		// Token: 0x0400071B RID: 1819
		Dynamic = 4096,
		// Token: 0x0400071C RID: 1820
		Properties = 31,
		// Token: 0x0400071D RID: 1821
		Methods = 448,
		// Token: 0x0400071E RID: 1822
		All = 8191
	}
}
