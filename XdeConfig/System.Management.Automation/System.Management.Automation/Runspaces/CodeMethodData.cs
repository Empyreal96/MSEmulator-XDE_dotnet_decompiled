using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000163 RID: 355
	[DebuggerDisplay("CodeMethod: {Name,nq}")]
	public sealed class CodeMethodData : TypeMemberData
	{
		// Token: 0x0600121B RID: 4635 RVA: 0x00064269 File Offset: 0x00062469
		public CodeMethodData(string name, MethodInfo methodToCall) : base(name)
		{
			this.CodeReference = methodToCall;
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x00064279 File Offset: 0x00062479
		// (set) Token: 0x0600121D RID: 4637 RVA: 0x00064281 File Offset: 0x00062481
		public MethodInfo CodeReference { get; set; }

		// Token: 0x0600121E RID: 4638 RVA: 0x0006428C File Offset: 0x0006248C
		internal override TypeMemberData Copy()
		{
			return new CodeMethodData(base.Name, this.CodeReference);
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x000642AC File Offset: 0x000624AC
		internal override void Process(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			TypeTable.ProcessCodeMethodData(errors, typeName, this, membersCollection, isOverride);
		}
	}
}
