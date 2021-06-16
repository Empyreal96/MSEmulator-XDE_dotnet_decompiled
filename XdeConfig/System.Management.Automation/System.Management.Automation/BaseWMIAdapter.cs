using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000187 RID: 391
	internal abstract class BaseWMIAdapter : Adapter
	{
		// Token: 0x06001318 RID: 4888 RVA: 0x00076C88 File Offset: 0x00074E88
		private IEnumerable<string> GetTypeNameHierarchyFromDerivation(ManagementBaseObject managementObj, string dotnetBaseType, bool shouldIncludeNamespace)
		{
			StringBuilder type = new StringBuilder(200);
			type.Append(dotnetBaseType);
			type.Append("#");
			if (shouldIncludeNamespace)
			{
				type.Append(managementObj.SystemProperties["__NAMESPACE"].Value);
				type.Append("\\");
			}
			type.Append(managementObj.SystemProperties["__CLASS"].Value);
			yield return type.ToString();
			PropertyData derivationData = managementObj.SystemProperties["__Derivation"];
			if (derivationData != null)
			{
				string[] typeHierarchy = Adapter.PropertySetAndMethodArgumentConvertTo(derivationData.Value, typeof(string[]), CultureInfo.InvariantCulture) as string[];
				if (typeHierarchy != null)
				{
					foreach (string t in typeHierarchy)
					{
						type.Clear();
						type.Append(dotnetBaseType);
						type.Append("#");
						if (shouldIncludeNamespace)
						{
							type.Append(managementObj.SystemProperties["__NAMESPACE"].Value);
							type.Append("\\");
						}
						type.Append(t);
						yield return type.ToString();
					}
				}
			}
			yield break;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00077024 File Offset: 0x00075224
		protected override IEnumerable<string> GetTypeNameHierarchy(object obj)
		{
			ManagementBaseObject managementObj = obj as ManagementBaseObject;
			bool isLoopStarted = false;
			foreach (string baseType in Adapter.GetDotNetTypeNameHierarchy(obj))
			{
				if (!isLoopStarted)
				{
					isLoopStarted = true;
					foreach (string typeFromDerivation in this.GetTypeNameHierarchyFromDerivation(managementObj, baseType, true))
					{
						yield return typeFromDerivation;
					}
					foreach (string typeFromDerivation2 in this.GetTypeNameHierarchyFromDerivation(managementObj, baseType, false))
					{
						yield return typeFromDerivation2;
					}
				}
				yield return baseType;
			}
			yield break;
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00077048 File Offset: 0x00075248
		protected override T GetMember<T>(object obj, string memberName)
		{
			Adapter.tracer.WriteLine("Getting member with name {0}", new object[]
			{
				memberName
			});
			ManagementBaseObject managementBaseObject = obj as ManagementBaseObject;
			if (managementBaseObject == null)
			{
				return default(T);
			}
			PSProperty psproperty = this.DoGetProperty(managementBaseObject, memberName);
			if (typeof(T).IsAssignableFrom(typeof(PSProperty)) && psproperty != null)
			{
				return psproperty as T;
			}
			if (typeof(T).IsAssignableFrom(typeof(PSMethod)))
			{
				T managementObjectMethod = this.GetManagementObjectMethod<T>(managementBaseObject, memberName);
				if (managementObjectMethod != null && psproperty == null)
				{
					return managementObjectMethod;
				}
			}
			return default(T);
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x000770F4 File Offset: 0x000752F4
		protected override PSMemberInfoInternalCollection<T> GetMembers<T>(object obj)
		{
			ManagementBaseObject wmiObject = (ManagementBaseObject)obj;
			PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
			this.AddAllProperties<T>(wmiObject, psmemberInfoInternalCollection);
			this.AddAllMethods<T>(wmiObject, psmemberInfoInternalCollection);
			return psmemberInfoInternalCollection;
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00077120 File Offset: 0x00075320
		protected override object MethodInvoke(PSMethod method, object[] arguments)
		{
			ManagementObject obj = method.baseObject as ManagementObject;
			BaseWMIAdapter.WMIMethodCacheEntry mdata = (BaseWMIAdapter.WMIMethodCacheEntry)method.adapterData;
			return this.AuxillaryInvokeMethod(obj, mdata, arguments);
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00077150 File Offset: 0x00075350
		protected override Collection<string> MethodDefinitions(PSMethod method)
		{
			BaseWMIAdapter.WMIMethodCacheEntry wmimethodCacheEntry = (BaseWMIAdapter.WMIMethodCacheEntry)method.adapterData;
			return new Collection<string>
			{
				wmimethodCacheEntry.MethodDefinition
			};
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x0007717C File Offset: 0x0007537C
		protected override bool PropertyIsSettable(PSProperty property)
		{
			ManagementBaseObject mgmtBaseObject = property.baseObject as ManagementBaseObject;
			bool result;
			try
			{
				ManagementClass managementClass = BaseWMIAdapter.CreateClassFrmObject(mgmtBaseObject);
				result = (bool)managementClass.GetPropertyQualifierValue(property.Name, "Write");
			}
			catch (ManagementException)
			{
				result = true;
			}
			catch (UnauthorizedAccessException)
			{
				result = true;
			}
			catch (COMException)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x000771F0 File Offset: 0x000753F0
		protected override bool PropertyIsGettable(PSProperty property)
		{
			return true;
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x000771F4 File Offset: 0x000753F4
		protected override string PropertyType(PSProperty property, bool forDisplay)
		{
			PropertyData propertyData = property.adapterData as PropertyData;
			Type dotNetType = BaseWMIAdapter.GetDotNetType(propertyData);
			string text;
			if (propertyData.Type == CimType.Object)
			{
				text = BaseWMIAdapter.GetEmbeddedObjectTypeName(propertyData);
				if (propertyData.IsArray)
				{
					text += "[]";
				}
			}
			else
			{
				text = (forDisplay ? ToStringCodeMethods.Type(dotNetType, false) : dotNetType.ToString());
			}
			return text;
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00077250 File Offset: 0x00075450
		protected override object PropertyGet(PSProperty property)
		{
			PropertyData propertyData = property.adapterData as PropertyData;
			return propertyData.Value;
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00077270 File Offset: 0x00075470
		protected override void PropertySet(PSProperty property, object setValue, bool convertIfPossible)
		{
			if (!(property.baseObject is ManagementBaseObject))
			{
				throw new SetValueInvocationException("CannotSetNonManagementObjectMsg", null, ExtendedTypeSystem.CannotSetNonManagementObject, new object[]
				{
					property.Name,
					property.baseObject.GetType().FullName,
					typeof(ManagementBaseObject).FullName
				});
			}
			if (!this.PropertyIsSettable(property))
			{
				throw new SetValueException("ReadOnlyWMIProperty", null, ExtendedTypeSystem.ReadOnlyProperty, new object[]
				{
					property.Name
				});
			}
			PropertyData propertyData = property.adapterData as PropertyData;
			if (convertIfPossible && setValue != null)
			{
				Type dotNetType = BaseWMIAdapter.GetDotNetType(propertyData);
				setValue = Adapter.PropertySetAndMethodArgumentConvertTo(setValue, dotNetType, CultureInfo.InvariantCulture);
			}
			propertyData.Value = setValue;
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00077330 File Offset: 0x00075530
		protected override string PropertyToString(PSProperty property)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.PropertyType(property, true));
			stringBuilder.Append(" ");
			stringBuilder.Append(property.Name);
			stringBuilder.Append(" {");
			if (this.PropertyIsGettable(property))
			{
				stringBuilder.Append("get;");
			}
			if (this.PropertyIsSettable(property))
			{
				stringBuilder.Append("set;");
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x000773B3 File Offset: 0x000755B3
		protected override AttributeCollection PropertyAttributes(PSProperty property)
		{
			return null;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x000773B8 File Offset: 0x000755B8
		protected static CacheTable GetInstanceMethodTable(ManagementBaseObject wmiObject, bool staticBinding)
		{
			CacheTable result;
			lock (BaseWMIAdapter.instanceMethodCacheTable)
			{
				CacheTable cacheTable = null;
				ManagementPath classPath = wmiObject.ClassPath;
				string key = string.Format(CultureInfo.InvariantCulture, "{0}#{1}", new object[]
				{
					classPath.Path,
					staticBinding.ToString()
				});
				cacheTable = (CacheTable)BaseWMIAdapter.instanceMethodCacheTable[key];
				if (cacheTable != null)
				{
					Adapter.tracer.WriteLine("Returning method information from internal cache", new object[0]);
					result = cacheTable;
				}
				else
				{
					Adapter.tracer.WriteLine("Method information not found in internal cache. Constructing one", new object[0]);
					try
					{
						cacheTable = new CacheTable();
						ManagementClass managementClass = wmiObject as ManagementClass;
						if (managementClass == null)
						{
							managementClass = BaseWMIAdapter.CreateClassFrmObject(wmiObject);
						}
						BaseWMIAdapter.PopulateMethodTable(managementClass, cacheTable, staticBinding);
						BaseWMIAdapter.instanceMethodCacheTable[key] = cacheTable;
					}
					catch (ManagementException)
					{
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (COMException)
					{
					}
					result = cacheTable;
				}
			}
			return result;
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x000774D0 File Offset: 0x000756D0
		private static void PopulateMethodTable(ManagementClass mgmtClass, CacheTable methodTable, bool staticBinding)
		{
			MethodDataCollection methods = mgmtClass.Methods;
			if (methods != null)
			{
				ManagementPath classPath = mgmtClass.ClassPath;
				foreach (MethodData methodData in methods)
				{
					bool flag = BaseWMIAdapter.IsStaticMethod(methodData);
					if (flag == staticBinding)
					{
						string name = methodData.Name;
						BaseWMIAdapter.WMIMethodCacheEntry member = new BaseWMIAdapter.WMIMethodCacheEntry(name, classPath.Path, methodData);
						methodTable.Add(name, member);
					}
				}
			}
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x00077560 File Offset: 0x00075760
		private static ManagementClass CreateClassFrmObject(ManagementBaseObject mgmtBaseObject)
		{
			ManagementClass managementClass = mgmtBaseObject as ManagementClass;
			if (managementClass == null)
			{
				managementClass = new ManagementClass(mgmtBaseObject.ClassPath);
				ManagementObject managementObject = mgmtBaseObject as ManagementObject;
				if (managementObject != null)
				{
					managementClass.Scope = managementObject.Scope;
					managementClass.Options = managementObject.Options;
				}
			}
			return managementClass;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x000775A8 File Offset: 0x000757A8
		protected static string GetEmbeddedObjectTypeName(PropertyData pData)
		{
			string result = typeof(object).FullName;
			if (pData == null)
			{
				return result;
			}
			try
			{
				string text = (string)pData.Qualifiers["cimtype"].Value;
				result = string.Format(CultureInfo.InvariantCulture, "{0}#{1}", new object[]
				{
					typeof(ManagementObject).FullName,
					text.Replace("object:", "")
				});
			}
			catch (ManagementException)
			{
			}
			catch (COMException)
			{
			}
			return result;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00077648 File Offset: 0x00075848
		protected static Type GetDotNetType(PropertyData pData)
		{
			Adapter.tracer.WriteLine("Getting DotNet Type for CimType : {0}", new object[]
			{
				pData.Type
			});
			CimType type = pData.Type;
			string text;
			switch (type)
			{
			case CimType.SInt16:
				text = typeof(short).FullName;
				goto IL_1D4;
			case CimType.SInt32:
				text = typeof(int).FullName;
				goto IL_1D4;
			case CimType.Real32:
				text = typeof(float).FullName;
				goto IL_1D4;
			case CimType.Real64:
				text = typeof(double).FullName;
				goto IL_1D4;
			case (CimType)6:
			case (CimType)7:
			case (CimType)9:
			case (CimType)10:
			case (CimType)12:
			case CimType.Object:
			case (CimType)14:
			case (CimType)15:
				break;
			case CimType.String:
				text = typeof(string).FullName;
				goto IL_1D4;
			case CimType.Boolean:
				text = typeof(bool).FullName;
				goto IL_1D4;
			case CimType.SInt8:
				text = typeof(sbyte).FullName;
				goto IL_1D4;
			case CimType.UInt8:
				text = typeof(byte).FullName;
				goto IL_1D4;
			case CimType.UInt16:
				text = typeof(ushort).FullName;
				goto IL_1D4;
			case CimType.UInt32:
				text = typeof(uint).FullName;
				goto IL_1D4;
			case CimType.SInt64:
				text = typeof(long).FullName;
				goto IL_1D4;
			case CimType.UInt64:
				text = typeof(ulong).FullName;
				goto IL_1D4;
			default:
				switch (type)
				{
				case CimType.DateTime:
					text = typeof(string).FullName;
					goto IL_1D4;
				case CimType.Reference:
					text = typeof(string).FullName;
					goto IL_1D4;
				case CimType.Char16:
					text = typeof(char).FullName;
					goto IL_1D4;
				}
				break;
			}
			text = typeof(object).FullName;
			IL_1D4:
			if (pData.IsArray)
			{
				text += "[]";
			}
			return Type.GetType(text);
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00077844 File Offset: 0x00075A44
		protected static bool IsStaticMethod(MethodData mdata)
		{
			try
			{
				QualifierData qualifierData = mdata.Qualifiers["static"];
				if (qualifierData == null)
				{
					return false;
				}
				bool result = false;
				LanguagePrimitives.TryConvertTo<bool>(qualifierData.Value, out result);
				return result;
			}
			catch (ManagementException)
			{
			}
			catch (COMException)
			{
			}
			return false;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x000778A4 File Offset: 0x00075AA4
		private object AuxillaryInvokeMethod(ManagementObject obj, BaseWMIAdapter.WMIMethodCacheEntry mdata, object[] arguments)
		{
			MethodInformation[] methods = new MethodInformation[]
			{
				mdata.MethodInfoStructure
			};
			object[] array;
			Adapter.GetBestMethodAndArguments(mdata.Name, methods, arguments, out array);
			ParameterInformation[] parameters = mdata.MethodInfoStructure.parameters;
			Adapter.tracer.WriteLine("Parameters found {0}. Arguments supplied {0}", new object[]
			{
				parameters.Length,
				array.Length
			});
			ManagementClass managementClass = BaseWMIAdapter.CreateClassFrmObject(obj);
			ManagementBaseObject methodParameters = managementClass.GetMethodParameters(mdata.Name);
			for (int i = 0; i < parameters.Length; i++)
			{
				BaseWMIAdapter.WMIParameterInformation wmiparameterInformation = (BaseWMIAdapter.WMIParameterInformation)parameters[i];
				if (i < arguments.Length && arguments[i] == null)
				{
					array[i] = null;
				}
				methodParameters[wmiparameterInformation.Name] = array[i];
			}
			return this.InvokeManagementMethod(obj, mdata.Name, methodParameters);
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00077974 File Offset: 0x00075B74
		internal static void UpdateParameters(ManagementBaseObject parameters, SortedList parametersList)
		{
			if (parameters == null)
			{
				return;
			}
			foreach (PropertyData propertyData in parameters.Properties)
			{
				int num = -1;
				BaseWMIAdapter.WMIParameterInformation value = new BaseWMIAdapter.WMIParameterInformation(propertyData.Name, BaseWMIAdapter.GetDotNetType(propertyData));
				try
				{
					num = (int)propertyData.Qualifiers["ID"].Value;
				}
				catch (ManagementException)
				{
				}
				catch (COMException)
				{
				}
				if (num < 0)
				{
					num = parametersList.Count;
				}
				parametersList[num] = value;
			}
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00077A30 File Offset: 0x00075C30
		internal static MethodInformation GetMethodInformation(MethodData mData)
		{
			SortedList sortedList = new SortedList();
			BaseWMIAdapter.UpdateParameters(mData.InParameters, sortedList);
			BaseWMIAdapter.WMIParameterInformation[] array = new BaseWMIAdapter.WMIParameterInformation[sortedList.Count];
			if (sortedList.Count > 0)
			{
				sortedList.Values.CopyTo(array, 0);
			}
			return new MethodInformation(false, true, array);
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00077A7C File Offset: 0x00075C7C
		internal static string GetMethodDefinition(MethodData mData)
		{
			SortedList sortedList = new SortedList();
			BaseWMIAdapter.UpdateParameters(mData.InParameters, sortedList);
			StringBuilder stringBuilder = new StringBuilder();
			if (sortedList.Count > 0)
			{
				foreach (object obj in sortedList.Values)
				{
					BaseWMIAdapter.WMIParameterInformation wmiparameterInformation = (BaseWMIAdapter.WMIParameterInformation)obj;
					string text = wmiparameterInformation.parameterType.ToString();
					PropertyData propertyData = mData.InParameters.Properties[wmiparameterInformation.Name];
					if (propertyData.Type == CimType.Object)
					{
						text = BaseWMIAdapter.GetEmbeddedObjectTypeName(propertyData);
						if (propertyData.IsArray)
						{
							text += "[]";
						}
					}
					stringBuilder.Append(text);
					stringBuilder.Append(" ");
					stringBuilder.Append(wmiparameterInformation.Name);
					stringBuilder.Append(", ");
				}
			}
			if (stringBuilder.Length > 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			Adapter.tracer.WriteLine("Constructing method definition for method {0}", new object[]
			{
				mData.Name
			});
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("System.Management.ManagementBaseObject ");
			stringBuilder2.Append(mData.Name);
			stringBuilder2.Append("(");
			stringBuilder2.Append(stringBuilder.ToString());
			stringBuilder2.Append(")");
			string text2 = stringBuilder2.ToString();
			Adapter.tracer.WriteLine("Definition constructed: {0}", new object[]
			{
				text2
			});
			return text2;
		}

		// Token: 0x0600132F RID: 4911
		protected abstract void AddAllProperties<T>(ManagementBaseObject wmiObject, PSMemberInfoInternalCollection<T> members) where T : PSMemberInfo;

		// Token: 0x06001330 RID: 4912
		protected abstract void AddAllMethods<T>(ManagementBaseObject wmiObject, PSMemberInfoInternalCollection<T> members) where T : PSMemberInfo;

		// Token: 0x06001331 RID: 4913
		protected abstract object InvokeManagementMethod(ManagementObject wmiObject, string methodName, ManagementBaseObject inParams);

		// Token: 0x06001332 RID: 4914
		protected abstract T GetManagementObjectMethod<T>(ManagementBaseObject wmiObject, string methodName) where T : PSMemberInfo;

		// Token: 0x06001333 RID: 4915
		protected abstract PSProperty DoGetProperty(ManagementBaseObject wmiObject, string propertyName);

		// Token: 0x0400083A RID: 2106
		private static HybridDictionary instanceMethodCacheTable = new HybridDictionary();

		// Token: 0x02000188 RID: 392
		internal class WMIMethodCacheEntry
		{
			// Token: 0x1700048A RID: 1162
			// (get) Token: 0x06001336 RID: 4918 RVA: 0x00077C3C File Offset: 0x00075E3C
			public string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x1700048B RID: 1163
			// (get) Token: 0x06001337 RID: 4919 RVA: 0x00077C44 File Offset: 0x00075E44
			public string ClassPath
			{
				get
				{
					return this.classPath;
				}
			}

			// Token: 0x1700048C RID: 1164
			// (get) Token: 0x06001338 RID: 4920 RVA: 0x00077C4C File Offset: 0x00075E4C
			public MethodInformation MethodInfoStructure
			{
				get
				{
					return this.methodInfoStructure;
				}
			}

			// Token: 0x1700048D RID: 1165
			// (get) Token: 0x06001339 RID: 4921 RVA: 0x00077C54 File Offset: 0x00075E54
			public string MethodDefinition
			{
				get
				{
					return this.methodDefinition;
				}
			}

			// Token: 0x0600133A RID: 4922 RVA: 0x00077C5C File Offset: 0x00075E5C
			internal WMIMethodCacheEntry(string n, string cPath, MethodData mData)
			{
				this.name = n;
				this.classPath = cPath;
				this.methodInfoStructure = BaseWMIAdapter.GetMethodInformation(mData);
				this.methodDefinition = BaseWMIAdapter.GetMethodDefinition(mData);
			}

			// Token: 0x0400083B RID: 2107
			private string name;

			// Token: 0x0400083C RID: 2108
			private string classPath;

			// Token: 0x0400083D RID: 2109
			private MethodInformation methodInfoStructure;

			// Token: 0x0400083E RID: 2110
			private string methodDefinition;
		}

		// Token: 0x02000189 RID: 393
		internal class WMIParameterInformation : ParameterInformation
		{
			// Token: 0x1700048E RID: 1166
			// (get) Token: 0x0600133B RID: 4923 RVA: 0x00077C8A File Offset: 0x00075E8A
			public string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x0600133C RID: 4924 RVA: 0x00077C92 File Offset: 0x00075E92
			public WMIParameterInformation(string name, Type ty) : base(ty, true, null, false)
			{
				this.name = name;
			}

			// Token: 0x0400083F RID: 2111
			private string name;
		}
	}
}
