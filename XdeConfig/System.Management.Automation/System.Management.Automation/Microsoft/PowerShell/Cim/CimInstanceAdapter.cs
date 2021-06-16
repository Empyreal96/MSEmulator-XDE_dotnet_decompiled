using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Reflection;
using Microsoft.Management.Infrastructure;

namespace Microsoft.PowerShell.Cim
{
	// Token: 0x020009F9 RID: 2553
	public sealed class CimInstanceAdapter : PSPropertyAdapter
	{
		// Token: 0x06005D88 RID: 23944 RVA: 0x001FE3C4 File Offset: 0x001FC5C4
		private static PSAdaptedProperty GetCimPropertyAdapter(CimProperty property, object baseObject, string propertyName)
		{
			return new PSAdaptedProperty(propertyName, property)
			{
				baseObject = baseObject
			};
		}

		// Token: 0x06005D89 RID: 23945 RVA: 0x001FE3E4 File Offset: 0x001FC5E4
		private static PSAdaptedProperty GetCimPropertyAdapter(CimProperty property, object baseObject)
		{
			PSAdaptedProperty result;
			try
			{
				string name = property.Name;
				result = CimInstanceAdapter.GetCimPropertyAdapter(property, baseObject, name);
			}
			catch (CimException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06005D8A RID: 23946 RVA: 0x001FE41C File Offset: 0x001FC61C
		private static PSAdaptedProperty GetPSComputerNameAdapter(CimInstance cimInstance)
		{
			return new PSAdaptedProperty(RemotingConstants.ComputerNameNoteProperty, cimInstance)
			{
				baseObject = cimInstance
			};
		}

		// Token: 0x06005D8B RID: 23947 RVA: 0x001FE440 File Offset: 0x001FC640
		public override Collection<PSAdaptedProperty> GetProperties(object baseObject)
		{
			CimInstance cimInstance = baseObject as CimInstance;
			if (cimInstance == null)
			{
				string message = string.Format(CultureInfo.InvariantCulture, CimInstanceTypeAdapterResources.BaseObjectNotCimInstance, new object[]
				{
					"baseObject",
					typeof(CimInstance).ToString()
				});
				throw new PSInvalidOperationException(message);
			}
			Collection<PSAdaptedProperty> collection = new Collection<PSAdaptedProperty>();
			if (cimInstance.CimInstanceProperties != null)
			{
				foreach (CimProperty property in cimInstance.CimInstanceProperties)
				{
					PSAdaptedProperty cimPropertyAdapter = CimInstanceAdapter.GetCimPropertyAdapter(property, baseObject);
					if (cimPropertyAdapter != null)
					{
						collection.Add(cimPropertyAdapter);
					}
				}
			}
			PSAdaptedProperty pscomputerNameAdapter = CimInstanceAdapter.GetPSComputerNameAdapter(cimInstance);
			if (pscomputerNameAdapter != null)
			{
				collection.Add(pscomputerNameAdapter);
			}
			return collection;
		}

		// Token: 0x06005D8C RID: 23948 RVA: 0x001FE50C File Offset: 0x001FC70C
		public override PSAdaptedProperty GetProperty(object baseObject, string propertyName)
		{
			if (propertyName == null)
			{
				throw new PSArgumentNullException("propertyName");
			}
			CimInstance cimInstance = baseObject as CimInstance;
			if (cimInstance == null)
			{
				string message = string.Format(CultureInfo.InvariantCulture, CimInstanceTypeAdapterResources.BaseObjectNotCimInstance, new object[]
				{
					"baseObject",
					typeof(CimInstance).ToString()
				});
				throw new PSInvalidOperationException(message);
			}
			CimProperty cimProperty = cimInstance.CimInstanceProperties[propertyName];
			if (cimProperty != null)
			{
				return CimInstanceAdapter.GetCimPropertyAdapter(cimProperty, baseObject, propertyName);
			}
			if (propertyName.Equals(RemotingConstants.ComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase))
			{
				return CimInstanceAdapter.GetPSComputerNameAdapter(cimInstance);
			}
			return null;
		}

		// Token: 0x06005D8D RID: 23949 RVA: 0x001FE5A4 File Offset: 0x001FC7A4
		internal static string CimTypeToTypeNameDisplayString(CimType cimType)
		{
			switch (cimType)
			{
			case CimType.DateTime:
			case CimType.Reference:
			case CimType.Instance:
				break;
			case CimType.String:
				goto IL_4C;
			default:
				switch (cimType)
				{
				case CimType.DateTimeArray:
				case CimType.ReferenceArray:
				case CimType.InstanceArray:
					break;
				case CimType.StringArray:
					goto IL_4C;
				default:
					goto IL_4C;
				}
				break;
			}
			return "CimInstance#" + cimType.ToString();
			IL_4C:
			return ToStringCodeMethods.Type(CimConverter.GetDotNetType(cimType), false);
		}

		// Token: 0x06005D8E RID: 23950 RVA: 0x001FE60C File Offset: 0x001FC80C
		public override string GetPropertyTypeName(PSAdaptedProperty adaptedProperty)
		{
			if (adaptedProperty == null)
			{
				throw new ArgumentNullException("adaptedProperty");
			}
			CimProperty cimProperty = adaptedProperty.Tag as CimProperty;
			if (cimProperty != null)
			{
				return CimInstanceAdapter.CimTypeToTypeNameDisplayString(cimProperty.CimType);
			}
			if (adaptedProperty.Name.Equals(RemotingConstants.ComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase))
			{
				return ToStringCodeMethods.Type(typeof(string), false);
			}
			throw new ArgumentNullException("adaptedProperty");
		}

		// Token: 0x06005D8F RID: 23951 RVA: 0x001FE670 File Offset: 0x001FC870
		public override object GetPropertyValue(PSAdaptedProperty adaptedProperty)
		{
			if (adaptedProperty == null)
			{
				throw new ArgumentNullException("adaptedProperty");
			}
			CimProperty cimProperty = adaptedProperty.Tag as CimProperty;
			if (cimProperty != null)
			{
				return cimProperty.Value;
			}
			if (adaptedProperty.Name.Equals(RemotingConstants.ComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase))
			{
				CimInstance cimInstance = (CimInstance)adaptedProperty.Tag;
				return cimInstance.GetCimSessionComputerName();
			}
			throw new ArgumentNullException("adaptedProperty");
		}

		// Token: 0x06005D90 RID: 23952 RVA: 0x001FE6D4 File Offset: 0x001FC8D4
		private void AddTypeNameHierarchy(IList<string> typeNamesWithNamespace, IList<string> typeNamesWithoutNamespace, string namespaceName, string className)
		{
			if (!string.IsNullOrEmpty(namespaceName))
			{
				string item = string.Format(CultureInfo.InvariantCulture, "Microsoft.Management.Infrastructure.CimInstance#{0}/{1}", new object[]
				{
					namespaceName,
					className
				});
				typeNamesWithNamespace.Add(item);
			}
			typeNamesWithoutNamespace.Add(string.Format(CultureInfo.InvariantCulture, "Microsoft.Management.Infrastructure.CimInstance#{0}", new object[]
			{
				className
			}));
		}

		// Token: 0x06005D91 RID: 23953 RVA: 0x001FE734 File Offset: 0x001FC934
		private List<CimClass> GetInheritanceChain(CimInstance cimInstance)
		{
			List<CimClass> list = new List<CimClass>();
			CimClass cimClass = cimInstance.CimClass;
			while (cimClass != null)
			{
				list.Add(cimClass);
				try
				{
					cimClass = cimClass.CimSuperClass;
				}
				catch (CimException)
				{
					break;
				}
			}
			return list;
		}

		// Token: 0x06005D92 RID: 23954 RVA: 0x001FE778 File Offset: 0x001FC978
		public override Collection<string> GetTypeNameHierarchy(object baseObject)
		{
			CimInstance cimInstance = baseObject as CimInstance;
			if (cimInstance == null)
			{
				throw new ArgumentNullException("baseObject");
			}
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			IList<CimClass> inheritanceChain = this.GetInheritanceChain(cimInstance);
			if (inheritanceChain == null || inheritanceChain.Count == 0)
			{
				this.AddTypeNameHierarchy(list, list2, cimInstance.CimSystemProperties.Namespace, cimInstance.CimSystemProperties.ClassName);
			}
			else
			{
				foreach (CimClass cimClass in inheritanceChain)
				{
					this.AddTypeNameHierarchy(list, list2, cimClass.CimSystemProperties.Namespace, cimClass.CimSystemProperties.ClassName);
					cimClass.Dispose();
				}
			}
			List<string> list3 = new List<string>();
			list3.AddRange(list);
			list3.AddRange(list2);
			if (baseObject != null)
			{
				Type type = baseObject.GetType();
				while (type != null)
				{
					list3.Add(type.FullName);
					type = type.GetTypeInfo().BaseType;
				}
			}
			return new Collection<string>(list3);
		}

		// Token: 0x06005D93 RID: 23955 RVA: 0x001FE88C File Offset: 0x001FCA8C
		public override bool IsGettable(PSAdaptedProperty adaptedProperty)
		{
			return true;
		}

		// Token: 0x06005D94 RID: 23956 RVA: 0x001FE890 File Offset: 0x001FCA90
		public override bool IsSettable(PSAdaptedProperty adaptedProperty)
		{
			if (adaptedProperty == null)
			{
				return false;
			}
			CimProperty cimProperty = adaptedProperty.Tag as CimProperty;
			if (cimProperty == null)
			{
				return false;
			}
			bool flag = CimFlags.ReadOnly == (cimProperty.Flags & CimFlags.ReadOnly);
			return !flag;
		}

		// Token: 0x06005D95 RID: 23957 RVA: 0x001FE8D0 File Offset: 0x001FCAD0
		public override void SetPropertyValue(PSAdaptedProperty adaptedProperty, object value)
		{
			if (adaptedProperty == null)
			{
				throw new ArgumentNullException("adaptedProperty");
			}
			if (!this.IsSettable(adaptedProperty))
			{
				throw new SetValueException("ReadOnlyCIMProperty", null, CimInstanceTypeAdapterResources.ReadOnlyCIMProperty, new object[]
				{
					adaptedProperty.Name
				});
			}
			CimProperty cimProperty = adaptedProperty.Tag as CimProperty;
			object obj = value;
			if (obj != null)
			{
				CimType cimType = cimProperty.CimType;
				Type resultType;
				if (cimType != CimType.DateTime)
				{
					if (cimType != CimType.DateTimeArray)
					{
						resultType = CimConverter.GetDotNetType(cimProperty.CimType);
					}
					else
					{
						resultType = typeof(object[]);
					}
				}
				else
				{
					resultType = typeof(object);
				}
				obj = Adapter.PropertySetAndMethodArgumentConvertTo(value, resultType, CultureInfo.InvariantCulture);
			}
			cimProperty.Value = obj;
		}
	}
}
