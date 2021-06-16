using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000161 RID: 353
	public sealed class CodePropertyData : TypeMemberData
	{
		// Token: 0x0600120C RID: 4620 RVA: 0x0006417D File Offset: 0x0006237D
		public CodePropertyData(string name, MethodInfo getMethod) : base(name)
		{
			this.GetCodeReference = getMethod;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0006418D File Offset: 0x0006238D
		public CodePropertyData(string name, MethodInfo getMethod, MethodInfo setMethod) : base(name)
		{
			this.GetCodeReference = getMethod;
			this.SetCodeReference = setMethod;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x000641A4 File Offset: 0x000623A4
		// (set) Token: 0x0600120F RID: 4623 RVA: 0x000641AC File Offset: 0x000623AC
		public MethodInfo GetCodeReference { get; set; }

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x000641B5 File Offset: 0x000623B5
		// (set) Token: 0x06001211 RID: 4625 RVA: 0x000641BD File Offset: 0x000623BD
		public MethodInfo SetCodeReference { get; set; }

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x000641C6 File Offset: 0x000623C6
		// (set) Token: 0x06001213 RID: 4627 RVA: 0x000641CE File Offset: 0x000623CE
		public bool IsHidden { get; set; }

		// Token: 0x06001214 RID: 4628 RVA: 0x000641D8 File Offset: 0x000623D8
		internal override TypeMemberData Copy()
		{
			return new CodePropertyData(base.Name, this.GetCodeReference, this.SetCodeReference)
			{
				IsHidden = this.IsHidden
			};
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x0006420C File Offset: 0x0006240C
		internal override void Process(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			TypeTable.ProcessCodePropertyData(errors, typeName, this, membersCollection, isOverride);
		}
	}
}
