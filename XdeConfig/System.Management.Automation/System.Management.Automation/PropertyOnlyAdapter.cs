using System;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000111 RID: 273
	internal abstract class PropertyOnlyAdapter : DotNetAdapter
	{
		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000EBB RID: 3771 RVA: 0x00051208 File Offset: 0x0004F408
		internal override bool SiteBinderCanOptimize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0005120B File Offset: 0x0004F40B
		protected override ConsolidatedString GetInternedTypeNameHierarchy(object obj)
		{
			return new ConsolidatedString(this.GetTypeNameHierarchy(obj), true);
		}

		// Token: 0x06000EBD RID: 3773
		protected abstract PSProperty DoGetProperty(object obj, string propertyName);

		// Token: 0x06000EBE RID: 3774
		protected abstract void DoAddAllProperties<T>(object obj, PSMemberInfoInternalCollection<T> members) where T : PSMemberInfo;

		// Token: 0x06000EBF RID: 3775 RVA: 0x0005121C File Offset: 0x0004F41C
		protected override T GetMember<T>(object obj, string memberName)
		{
			PSProperty psproperty = this.DoGetProperty(obj, memberName);
			if (typeof(T).IsAssignableFrom(typeof(PSProperty)) && psproperty != null)
			{
				return psproperty as T;
			}
			if (typeof(T).IsAssignableFrom(typeof(PSMethod)))
			{
				T dotNetMethod = PSObject.dotNetInstanceAdapter.GetDotNetMethod<T>(obj, memberName);
				if (dotNetMethod != null && psproperty == null)
				{
					return dotNetMethod;
				}
			}
			if (DotNetAdapter.IsTypeParameterizedProperty(typeof(T)))
			{
				PSParameterizedProperty dotNetProperty = PSObject.dotNetInstanceAdapter.GetDotNetProperty<PSParameterizedProperty>(obj, memberName);
				if (dotNetProperty != null && psproperty == null)
				{
					return dotNetProperty as T;
				}
			}
			return default(T);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x000512CC File Offset: 0x0004F4CC
		protected override PSMemberInfoInternalCollection<T> GetMembers<T>(object obj)
		{
			PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
			if (typeof(T).IsAssignableFrom(typeof(PSProperty)))
			{
				this.DoAddAllProperties<T>(obj, psmemberInfoInternalCollection);
			}
			PSObject.dotNetInstanceAdapter.AddAllMethods<T>(obj, psmemberInfoInternalCollection, true);
			if (DotNetAdapter.IsTypeParameterizedProperty(typeof(T)))
			{
				PSMemberInfoInternalCollection<PSParameterizedProperty> psmemberInfoInternalCollection2 = new PSMemberInfoInternalCollection<PSParameterizedProperty>();
				PSObject.dotNetInstanceAdapter.AddAllProperties<PSParameterizedProperty>(obj, psmemberInfoInternalCollection2, true);
				foreach (PSParameterizedProperty psparameterizedProperty in psmemberInfoInternalCollection2)
				{
					try
					{
						psmemberInfoInternalCollection.Add(psparameterizedProperty as T);
					}
					catch (ExtendedTypeSystemException)
					{
					}
				}
			}
			return psmemberInfoInternalCollection;
		}
	}
}
