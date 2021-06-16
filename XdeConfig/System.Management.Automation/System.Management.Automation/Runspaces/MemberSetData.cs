using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000165 RID: 357
	public class MemberSetData : TypeMemberData
	{
		// Token: 0x06001229 RID: 4649 RVA: 0x0006439D File Offset: 0x0006259D
		public MemberSetData(string name, Collection<TypeMemberData> members) : base(name)
		{
			this.Members = members;
			this.InheritMembers = true;
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600122A RID: 4650 RVA: 0x000643B4 File Offset: 0x000625B4
		// (set) Token: 0x0600122B RID: 4651 RVA: 0x000643BC File Offset: 0x000625BC
		public Collection<TypeMemberData> Members { get; private set; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600122C RID: 4652 RVA: 0x000643C5 File Offset: 0x000625C5
		// (set) Token: 0x0600122D RID: 4653 RVA: 0x000643CD File Offset: 0x000625CD
		public bool IsHidden { get; set; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600122E RID: 4654 RVA: 0x000643D6 File Offset: 0x000625D6
		// (set) Token: 0x0600122F RID: 4655 RVA: 0x000643DE File Offset: 0x000625DE
		public bool InheritMembers { get; set; }

		// Token: 0x06001230 RID: 4656 RVA: 0x000643E8 File Offset: 0x000625E8
		internal override TypeMemberData Copy()
		{
			return new MemberSetData(base.Name, this.Members)
			{
				IsHidden = this.IsHidden,
				InheritMembers = this.InheritMembers
			};
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x00064422 File Offset: 0x00062622
		internal override void Process(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			TypeTable.ProcessMemberSetData(errors, typeName, this, membersCollection, isOverride);
		}
	}
}
