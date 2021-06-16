using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200015D RID: 349
	public abstract class TypeMemberData
	{
		// Token: 0x060011EB RID: 4587 RVA: 0x00063F98 File Offset: 0x00062198
		internal TypeMemberData(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.Name = name;
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00063FBA File Offset: 0x000621BA
		internal TypeMemberData()
		{
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x00063FC2 File Offset: 0x000621C2
		// (set) Token: 0x060011EE RID: 4590 RVA: 0x00063FCA File Offset: 0x000621CA
		public string Name { get; protected set; }

		// Token: 0x060011EF RID: 4591
		internal abstract TypeMemberData Copy();

		// Token: 0x060011F0 RID: 4592
		internal abstract void Process(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride);
	}
}
