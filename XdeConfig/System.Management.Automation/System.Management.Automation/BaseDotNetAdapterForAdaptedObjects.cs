using System;

namespace System.Management.Automation
{
	// Token: 0x0200010C RID: 268
	internal class BaseDotNetAdapterForAdaptedObjects : DotNetAdapter
	{
		// Token: 0x06000EA3 RID: 3747 RVA: 0x00050D08 File Offset: 0x0004EF08
		protected override PSMemberInfoInternalCollection<T> GetMembers<T>(object obj)
		{
			PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
			base.AddAllProperties<T>(obj, psmemberInfoInternalCollection, true);
			base.AddAllMethods<T>(obj, psmemberInfoInternalCollection, true);
			base.AddAllEvents<T>(obj, psmemberInfoInternalCollection, true);
			return psmemberInfoInternalCollection;
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00050D38 File Offset: 0x0004EF38
		protected override T GetMember<T>(object obj, string memberName)
		{
			PSProperty dotNetProperty = base.GetDotNetProperty<PSProperty>(obj, memberName);
			if (typeof(T).IsAssignableFrom(typeof(PSProperty)) && dotNetProperty != null)
			{
				return dotNetProperty as T;
			}
			if (typeof(T) == typeof(PSMemberInfo))
			{
				T dotNetMethod = PSObject.dotNetInstanceAdapter.GetDotNetMethod<T>(obj, memberName);
				if (dotNetMethod != null && dotNetProperty == null)
				{
					return dotNetMethod;
				}
			}
			if (DotNetAdapter.IsTypeParameterizedProperty(typeof(T)))
			{
				PSParameterizedProperty dotNetProperty2 = PSObject.dotNetInstanceAdapter.GetDotNetProperty<PSParameterizedProperty>(obj, memberName);
				if (dotNetProperty2 != null && dotNetProperty == null)
				{
					return dotNetProperty2 as T;
				}
			}
			return default(T);
		}
	}
}
