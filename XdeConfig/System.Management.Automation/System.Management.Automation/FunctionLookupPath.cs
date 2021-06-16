using System;

namespace System.Management.Automation
{
	// Token: 0x02000848 RID: 2120
	internal class FunctionLookupPath : VariablePath
	{
		// Token: 0x06005192 RID: 20882 RVA: 0x001B2799 File Offset: 0x001B0999
		internal FunctionLookupPath(string path) : base(path, VariablePathFlags.Function | VariablePathFlags.Unqualified)
		{
		}
	}
}
