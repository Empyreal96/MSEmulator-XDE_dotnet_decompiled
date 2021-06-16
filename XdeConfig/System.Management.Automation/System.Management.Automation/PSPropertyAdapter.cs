using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x02000115 RID: 277
	public abstract class PSPropertyAdapter
	{
		// Token: 0x06000EE9 RID: 3817 RVA: 0x000528E4 File Offset: 0x00050AE4
		public virtual Collection<string> GetTypeNameHierarchy(object baseObject)
		{
			if (baseObject == null)
			{
				throw new ArgumentNullException("baseObject");
			}
			Collection<string> collection = new Collection<string>();
			Type type = baseObject.GetType();
			while (type != null)
			{
				collection.Add(type.FullName);
				type = type.GetTypeInfo().BaseType;
			}
			return collection;
		}

		// Token: 0x06000EEA RID: 3818
		public abstract Collection<PSAdaptedProperty> GetProperties(object baseObject);

		// Token: 0x06000EEB RID: 3819
		public abstract PSAdaptedProperty GetProperty(object baseObject, string propertyName);

		// Token: 0x06000EEC RID: 3820
		public abstract bool IsSettable(PSAdaptedProperty adaptedProperty);

		// Token: 0x06000EED RID: 3821
		public abstract bool IsGettable(PSAdaptedProperty adaptedProperty);

		// Token: 0x06000EEE RID: 3822
		public abstract object GetPropertyValue(PSAdaptedProperty adaptedProperty);

		// Token: 0x06000EEF RID: 3823
		public abstract void SetPropertyValue(PSAdaptedProperty adaptedProperty, object value);

		// Token: 0x06000EF0 RID: 3824
		public abstract string GetPropertyTypeName(PSAdaptedProperty adaptedProperty);
	}
}
