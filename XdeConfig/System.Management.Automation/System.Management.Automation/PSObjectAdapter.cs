using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x0200010F RID: 271
	internal class PSObjectAdapter : MemberRedirectionAdapter
	{
		// Token: 0x06000EB3 RID: 3763 RVA: 0x00050FDC File Offset: 0x0004F1DC
		protected override IEnumerable<string> GetTypeNameHierarchy(object obj)
		{
			return ((PSObject)obj).InternalTypeNames;
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00050FE9 File Offset: 0x0004F1E9
		protected override T GetMember<T>(object obj, string memberName)
		{
			return ((PSObject)obj).Members[memberName] as T;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00051008 File Offset: 0x0004F208
		protected override PSMemberInfoInternalCollection<T> GetMembers<T>(object obj)
		{
			PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
			PSObject psobject = (PSObject)obj;
			foreach (PSMemberInfo psmemberInfo in psobject.Members)
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
