using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x02000061 RID: 97
	internal class ParameterCollectionTypeInformation
	{
		// Token: 0x06000537 RID: 1335 RVA: 0x00019260 File Offset: 0x00017460
		internal ParameterCollectionTypeInformation(Type type)
		{
			this.ParameterCollectionType = ParameterCollectionType.NotCollection;
			TypeInfo typeInfo = type.GetTypeInfo();
			if (type.IsSubclassOf(typeof(Array)))
			{
				this.ParameterCollectionType = ParameterCollectionType.Array;
				this.ElementType = type.GetElementType();
				return;
			}
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				return;
			}
			Type[] interfaces = type.GetInterfaces();
			if (interfaces.Any((Type i) => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<, >)) || (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<, >)))
			{
				return;
			}
			bool flag = type.GetInterface(typeof(IList).Name) != null;
			if (flag && typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Collection<>))
			{
				this.ParameterCollectionType = ParameterCollectionType.IList;
				Type[] genericArguments = type.GetGenericArguments();
				this.ElementType = genericArguments[0];
				return;
			}
			Type type2 = interfaces.FirstOrDefault((Type i) => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));
			if (type2 != null)
			{
				this.ParameterCollectionType = ParameterCollectionType.ICollectionGeneric;
				Type[] genericArguments2 = type2.GetGenericArguments();
				this.ElementType = genericArguments2[0];
				return;
			}
			if (flag)
			{
				this.ParameterCollectionType = ParameterCollectionType.IList;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x000193B0 File Offset: 0x000175B0
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x000193B8 File Offset: 0x000175B8
		internal ParameterCollectionType ParameterCollectionType { get; private set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x000193C1 File Offset: 0x000175C1
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x000193C9 File Offset: 0x000175C9
		internal Type ElementType { get; set; }
	}
}
