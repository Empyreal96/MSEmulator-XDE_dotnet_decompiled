using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000160 RID: 352
	[DebuggerDisplay("ScriptProperty: {Name,nq}")]
	public sealed class ScriptPropertyData : TypeMemberData
	{
		// Token: 0x06001202 RID: 4610 RVA: 0x000640E1 File Offset: 0x000622E1
		public ScriptPropertyData(string name, ScriptBlock getScriptBlock) : base(name)
		{
			this.GetScriptBlock = getScriptBlock;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x000640F1 File Offset: 0x000622F1
		public ScriptPropertyData(string name, ScriptBlock getScriptBlock, ScriptBlock setScriptBlock) : base(name)
		{
			this.GetScriptBlock = getScriptBlock;
			this.SetScriptBlock = setScriptBlock;
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x00064108 File Offset: 0x00062308
		// (set) Token: 0x06001205 RID: 4613 RVA: 0x00064110 File Offset: 0x00062310
		public ScriptBlock GetScriptBlock { get; set; }

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x00064119 File Offset: 0x00062319
		// (set) Token: 0x06001207 RID: 4615 RVA: 0x00064121 File Offset: 0x00062321
		public ScriptBlock SetScriptBlock { get; set; }

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x0006412A File Offset: 0x0006232A
		// (set) Token: 0x06001209 RID: 4617 RVA: 0x00064132 File Offset: 0x00062332
		public bool IsHidden { get; set; }

		// Token: 0x0600120A RID: 4618 RVA: 0x0006413C File Offset: 0x0006233C
		internal override TypeMemberData Copy()
		{
			return new ScriptPropertyData(base.Name, this.GetScriptBlock, this.SetScriptBlock)
			{
				IsHidden = this.IsHidden
			};
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00064170 File Offset: 0x00062370
		internal override void Process(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			TypeTable.ProcessScriptPropertyData(errors, typeName, this, membersCollection, isOverride);
		}
	}
}
