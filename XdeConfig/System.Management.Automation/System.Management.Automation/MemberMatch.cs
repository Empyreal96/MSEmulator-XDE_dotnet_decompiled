using System;

namespace System.Management.Automation
{
	// Token: 0x0200014B RID: 331
	internal class MemberMatch
	{
		// Token: 0x0600112B RID: 4395 RVA: 0x0005F2EA File Offset: 0x0005D4EA
		internal static WildcardPattern GetNamePattern(string name)
		{
			if (name != null && WildcardPattern.ContainsWildcardCharacters(name))
			{
				return new WildcardPattern(name, WildcardOptions.IgnoreCase);
			}
			return null;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0005F300 File Offset: 0x0005D500
		internal static PSMemberInfoInternalCollection<T> Match<T>(PSMemberInfoInternalCollection<T> memberList, string name, WildcardPattern nameMatch, PSMemberTypes memberTypes) where T : PSMemberInfo
		{
			PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
			if (memberList == null)
			{
				throw PSTraceSource.NewArgumentNullException("memberList");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			if (nameMatch == null)
			{
				T t = memberList[name];
				if (t != null && (t.MemberType & memberTypes) != (PSMemberTypes)0)
				{
					psmemberInfoInternalCollection.Add(t);
				}
				return psmemberInfoInternalCollection;
			}
			foreach (T member in memberList)
			{
				if (nameMatch.IsMatch(member.Name) && (member.MemberType & memberTypes) != (PSMemberTypes)0)
				{
					psmemberInfoInternalCollection.Add(member);
				}
			}
			return psmemberInfoInternalCollection;
		}
	}
}
