using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x02000152 RID: 338
	internal static class ReservedNameMembers
	{
		// Token: 0x0600115E RID: 4446 RVA: 0x0005FA60 File Offset: 0x0005DC60
		private static object GenerateMemberSet(string name, object obj)
		{
			PSObject psobject = PSObject.AsPSObject(obj);
			PSMemberInfo psmemberInfo = psobject.InstanceMembers[name];
			if (psmemberInfo == null)
			{
				psmemberInfo = new PSInternalMemberSet(name, psobject)
				{
					ShouldSerialize = false,
					IsHidden = true,
					IsReservedMember = true
				};
				psobject.InstanceMembers.Add(psmemberInfo);
				psmemberInfo.instance = psobject;
			}
			return psmemberInfo;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0005FAB7 File Offset: 0x0005DCB7
		internal static object GeneratePSBaseMemberSet(object obj)
		{
			return ReservedNameMembers.GenerateMemberSet("psbase", obj);
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0005FAC4 File Offset: 0x0005DCC4
		internal static object GeneratePSAdaptedMemberSet(object obj)
		{
			return ReservedNameMembers.GenerateMemberSet("psadapted", obj);
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0005FAD1 File Offset: 0x0005DCD1
		internal static object GeneratePSObjectMemberSet(object obj)
		{
			return ReservedNameMembers.GenerateMemberSet("psobject", obj);
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0005FAE0 File Offset: 0x0005DCE0
		internal static object GeneratePSExtendedMemberSet(object obj)
		{
			PSObject psobject = PSObject.AsPSObject(obj);
			PSMemberInfo psmemberInfo = psobject.InstanceMembers["psextended"];
			if (psmemberInfo == null)
			{
				psmemberInfo = new PSMemberSet("psextended", psobject)
				{
					ShouldSerialize = false,
					IsHidden = true,
					IsReservedMember = true
				};
				psmemberInfo.ReplicateInstance(psobject);
				psmemberInfo.instance = psobject;
				psobject.InstanceMembers.Add(psmemberInfo);
			}
			return psmemberInfo;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0005FB46 File Offset: 0x0005DD46
		public static Collection<string> PSTypeNames(PSObject o)
		{
			return o.TypeNames;
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0005FB50 File Offset: 0x0005DD50
		internal static void GeneratePSTypeNames(object obj)
		{
			PSObject psobject = PSObject.AsPSObject(obj);
			if (psobject.InstanceMembers["pstypenames"] != null)
			{
				return;
			}
			PSCodeProperty member = new PSCodeProperty("pstypenames", CachedReflectionInfo.ReservedNameMembers_PSTypeNames)
			{
				ShouldSerialize = false,
				instance = psobject,
				IsHidden = true,
				IsReservedMember = true
			};
			psobject.InstanceMembers.Add(member);
		}
	}
}
