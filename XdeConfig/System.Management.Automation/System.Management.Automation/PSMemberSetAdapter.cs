using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x02000110 RID: 272
	internal class PSMemberSetAdapter : MemberRedirectionAdapter
	{
		// Token: 0x06000EB7 RID: 3767 RVA: 0x00051154 File Offset: 0x0004F354
		protected override IEnumerable<string> GetTypeNameHierarchy(object obj)
		{
			yield return typeof(PSMemberSet).FullName;
			yield break;
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00051171 File Offset: 0x0004F371
		protected override T GetMember<T>(object obj, string memberName)
		{
			return ((PSMemberSet)obj).Members[memberName] as T;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00051190 File Offset: 0x0004F390
		protected override PSMemberInfoInternalCollection<T> GetMembers<T>(object obj)
		{
			PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
			foreach (PSMemberInfo psmemberInfo in ((PSMemberSet)obj).Members)
			{
				T t = psmemberInfo as T;
				if (t != null)
				{
					psmemberInfoInternalCollection.Add(t);
				}
			}
			return psmemberInfoInternalCollection;
		}
	}
}
