using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200015F RID: 351
	[DebuggerDisplay("AliasProperty: {Name,nq} = {ReferencedMemberName,nq}")]
	public sealed class AliasPropertyData : TypeMemberData
	{
		// Token: 0x060011F8 RID: 4600 RVA: 0x00064043 File Offset: 0x00062243
		public AliasPropertyData(string name, string referencedMemberName) : base(name)
		{
			this.ReferencedMemberName = referencedMemberName;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00064053 File Offset: 0x00062253
		public AliasPropertyData(string name, string referencedMemberName, Type type) : base(name)
		{
			this.ReferencedMemberName = referencedMemberName;
			this.MemberType = type;
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x0006406A File Offset: 0x0006226A
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x00064072 File Offset: 0x00062272
		public string ReferencedMemberName { get; set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x0006407B File Offset: 0x0006227B
		// (set) Token: 0x060011FD RID: 4605 RVA: 0x00064083 File Offset: 0x00062283
		public Type MemberType { get; set; }

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x0006408C File Offset: 0x0006228C
		// (set) Token: 0x060011FF RID: 4607 RVA: 0x00064094 File Offset: 0x00062294
		public bool IsHidden { get; set; }

		// Token: 0x06001200 RID: 4608 RVA: 0x000640A0 File Offset: 0x000622A0
		internal override TypeMemberData Copy()
		{
			return new AliasPropertyData(base.Name, this.ReferencedMemberName, this.MemberType)
			{
				IsHidden = this.IsHidden
			};
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x000640D4 File Offset: 0x000622D4
		internal override void Process(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			TypeTable.ProcessAliasData(errors, typeName, this, membersCollection, isOverride);
		}
	}
}
