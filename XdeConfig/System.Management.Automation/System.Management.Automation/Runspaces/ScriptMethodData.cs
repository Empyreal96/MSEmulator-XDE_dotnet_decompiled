using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000162 RID: 354
	[DebuggerDisplay("ScriptMethod: {Name,nq}")]
	public sealed class ScriptMethodData : TypeMemberData
	{
		// Token: 0x06001216 RID: 4630 RVA: 0x00064219 File Offset: 0x00062419
		public ScriptMethodData(string name, ScriptBlock scriptToInvoke) : base(name)
		{
			this.Script = scriptToInvoke;
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x00064229 File Offset: 0x00062429
		// (set) Token: 0x06001218 RID: 4632 RVA: 0x00064231 File Offset: 0x00062431
		public ScriptBlock Script { get; set; }

		// Token: 0x06001219 RID: 4633 RVA: 0x0006423C File Offset: 0x0006243C
		internal override TypeMemberData Copy()
		{
			return new ScriptMethodData(base.Name, this.Script);
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0006425C File Offset: 0x0006245C
		internal override void Process(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			TypeTable.ProcessScriptMethodData(errors, typeName, this, membersCollection, isOverride);
		}
	}
}
