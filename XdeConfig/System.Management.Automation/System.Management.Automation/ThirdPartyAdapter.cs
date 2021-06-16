using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x02000114 RID: 276
	internal class ThirdPartyAdapter : PropertyOnlyAdapter
	{
		// Token: 0x06000EDD RID: 3805 RVA: 0x000524FB File Offset: 0x000506FB
		internal ThirdPartyAdapter(Type adaptedType, PSPropertyAdapter externalAdapter)
		{
			this.adaptedType = adaptedType;
			this.externalAdapter = externalAdapter;
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000EDE RID: 3806 RVA: 0x00052511 File Offset: 0x00050711
		internal Type AdaptedType
		{
			get
			{
				return this.adaptedType;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x00052519 File Offset: 0x00050719
		internal Type ExternalAdapterType
		{
			get
			{
				return this.externalAdapter.GetType();
			}
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00052528 File Offset: 0x00050728
		protected override IEnumerable<string> GetTypeNameHierarchy(object obj)
		{
			Collection<string> collection = null;
			try
			{
				collection = this.externalAdapter.GetTypeNameHierarchy(obj);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new ExtendedTypeSystemException("PSPropertyAdapter.GetTypeNameHierarchyError", ex, ExtendedTypeSystem.GetTypeNameHierarchyError, new object[]
				{
					obj.ToString()
				});
			}
			if (collection == null)
			{
				throw new ExtendedTypeSystemException("PSPropertyAdapter.NullReturnValueError", null, ExtendedTypeSystem.NullReturnValueError, new object[]
				{
					"PSPropertyAdapter.GetTypeNameHierarchy"
				});
			}
			return collection;
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x000525A4 File Offset: 0x000507A4
		protected override void DoAddAllProperties<T>(object obj, PSMemberInfoInternalCollection<T> members)
		{
			Collection<PSAdaptedProperty> collection = null;
			try
			{
				collection = this.externalAdapter.GetProperties(obj);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new ExtendedTypeSystemException("PSPropertyAdapter.GetProperties", ex, ExtendedTypeSystem.GetProperties, new object[]
				{
					obj.ToString()
				});
			}
			if (collection == null)
			{
				throw new ExtendedTypeSystemException("PSPropertyAdapter.NullReturnValueError", null, ExtendedTypeSystem.NullReturnValueError, new object[]
				{
					"PSPropertyAdapter.GetProperties"
				});
			}
			foreach (PSAdaptedProperty psadaptedProperty in collection)
			{
				this.InitializeProperty(psadaptedProperty, obj);
				members.Add(psadaptedProperty as T);
			}
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00052670 File Offset: 0x00050870
		protected override PSProperty DoGetProperty(object obj, string propertyName)
		{
			PSAdaptedProperty psadaptedProperty = null;
			try
			{
				psadaptedProperty = this.externalAdapter.GetProperty(obj, propertyName);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new ExtendedTypeSystemException("PSPropertyAdapter.GetProperty", ex, ExtendedTypeSystem.GetProperty, new object[]
				{
					propertyName,
					obj.ToString()
				});
			}
			if (psadaptedProperty != null)
			{
				this.InitializeProperty(psadaptedProperty, obj);
			}
			return psadaptedProperty;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x000526D8 File Offset: 0x000508D8
		private void InitializeProperty(PSAdaptedProperty property, object baseObject)
		{
			if (property.adapter == null)
			{
				property.adapter = this;
				property.baseObject = baseObject;
			}
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x000526F0 File Offset: 0x000508F0
		protected override bool PropertyIsSettable(PSProperty property)
		{
			PSAdaptedProperty adaptedProperty = property as PSAdaptedProperty;
			bool result;
			try
			{
				result = this.externalAdapter.IsSettable(adaptedProperty);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new ExtendedTypeSystemException("PSPropertyAdapter.PropertyIsSettableError", ex, ExtendedTypeSystem.PropertyIsSettableError, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00052750 File Offset: 0x00050950
		protected override bool PropertyIsGettable(PSProperty property)
		{
			PSAdaptedProperty adaptedProperty = property as PSAdaptedProperty;
			bool result;
			try
			{
				result = this.externalAdapter.IsGettable(adaptedProperty);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new ExtendedTypeSystemException("PSPropertyAdapter.PropertyIsGettableError", ex, ExtendedTypeSystem.PropertyIsGettableError, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x000527B0 File Offset: 0x000509B0
		protected override object PropertyGet(PSProperty property)
		{
			PSAdaptedProperty adaptedProperty = property as PSAdaptedProperty;
			object propertyValue;
			try
			{
				propertyValue = this.externalAdapter.GetPropertyValue(adaptedProperty);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new ExtendedTypeSystemException("PSPropertyAdapter.PropertyGetError", ex, ExtendedTypeSystem.PropertyGetError, new object[]
				{
					property.Name
				});
			}
			return propertyValue;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00052810 File Offset: 0x00050A10
		protected override void PropertySet(PSProperty property, object setValue, bool convertIfPossible)
		{
			PSAdaptedProperty adaptedProperty = property as PSAdaptedProperty;
			try
			{
				this.externalAdapter.SetPropertyValue(adaptedProperty, setValue);
			}
			catch (SetValueException)
			{
				throw;
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new ExtendedTypeSystemException("PSPropertyAdapter.PropertySetError", ex, ExtendedTypeSystem.PropertySetError, new object[]
				{
					property.Name
				});
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0005287C File Offset: 0x00050A7C
		protected override string PropertyType(PSProperty property, bool forDisplay)
		{
			PSAdaptedProperty adaptedProperty = property as PSAdaptedProperty;
			string text = null;
			try
			{
				text = this.externalAdapter.GetPropertyTypeName(adaptedProperty);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				throw new ExtendedTypeSystemException("PSPropertyAdapter.PropertyTypeError", ex, ExtendedTypeSystem.PropertyTypeError, new object[]
				{
					property.Name
				});
			}
			return text ?? "System.Object";
		}

		// Token: 0x04000682 RID: 1666
		private Type adaptedType;

		// Token: 0x04000683 RID: 1667
		private PSPropertyAdapter externalAdapter;
	}
}
