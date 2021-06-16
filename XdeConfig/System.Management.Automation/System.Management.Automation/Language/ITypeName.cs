using System;

namespace System.Management.Automation.Language
{
	// Token: 0x0200057E RID: 1406
	public interface ITypeName
	{
		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06003A41 RID: 14913
		string FullName { get; }

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06003A42 RID: 14914
		string Name { get; }

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06003A43 RID: 14915
		string AssemblyName { get; }

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06003A44 RID: 14916
		bool IsArray { get; }

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06003A45 RID: 14917
		bool IsGeneric { get; }

		// Token: 0x06003A46 RID: 14918
		Type GetReflectionType();

		// Token: 0x06003A47 RID: 14919
		Type GetReflectionAttributeType();

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06003A48 RID: 14920
		IScriptExtent Extent { get; }
	}
}
