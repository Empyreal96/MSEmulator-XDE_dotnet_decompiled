using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000105 RID: 261
	internal class DotNetAdapter : Adapter
	{
		// Token: 0x06000E58 RID: 3672 RVA: 0x0004E619 File Offset: 0x0004C819
		internal DotNetAdapter()
		{
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0004E621 File Offset: 0x0004C821
		internal DotNetAdapter(bool isStatic)
		{
			this.isStatic = isStatic;
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0004E630 File Offset: 0x0004C830
		private static bool SameSignature(MethodBase method1, MethodBase method2)
		{
			if (method1.GetGenericArguments().Length != method2.GetGenericArguments().Length)
			{
				return false;
			}
			ParameterInfo[] parameters = method1.GetParameters();
			ParameterInfo[] parameters2 = method2.GetParameters();
			if (parameters.Length != parameters2.Length)
			{
				return false;
			}
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].ParameterType != parameters2[i].ParameterType || parameters[i].IsOut != parameters2[i].IsOut || parameters[i].IsOptional != parameters2[i].IsOptional)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0004E6B8 File Offset: 0x0004C8B8
		private static void AddOverload(List<MethodBase> previousMethodEntry, MethodInfo method)
		{
			bool flag = true;
			for (int i = 0; i < previousMethodEntry.Count; i++)
			{
				if (DotNetAdapter.SameSignature(previousMethodEntry[i], method))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				previousMethodEntry.Add(method);
			}
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0004E6F8 File Offset: 0x0004C8F8
		private static void PopulateMethodReflectionTable(Type type, MethodInfo[] methods, CacheTable typeMethods)
		{
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.DeclaringType == type)
				{
					string name = methodInfo.Name;
					List<MethodBase> list = (List<MethodBase>)typeMethods[name];
					if (list == null)
					{
						List<MethodBase> member = new List<MethodBase>
						{
							methodInfo
						};
						typeMethods.Add(name, member);
					}
					else
					{
						DotNetAdapter.AddOverload(list, methodInfo);
					}
				}
			}
			TypeInfo typeInfo = type.GetTypeInfo();
			if (typeInfo.BaseType != null)
			{
				DotNetAdapter.PopulateMethodReflectionTable(typeInfo.BaseType, methods, typeMethods);
			}
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0004E788 File Offset: 0x0004C988
		private static void PopulateMethodReflectionTable(ConstructorInfo[] ctors, CacheTable typeMethods)
		{
			foreach (ConstructorInfo item in ctors)
			{
				List<MethodBase> list = (List<MethodBase>)typeMethods["new"];
				if (list == null)
				{
					typeMethods.Add("new", new List<MethodBase>
					{
						item
					});
				}
				else
				{
					list.Add(item);
				}
			}
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0004E7E4 File Offset: 0x0004C9E4
		private static void PopulateMethodReflectionTable(Type type, CacheTable typeMethods, BindingFlags bindingFlags)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			if (type != null)
			{
				MethodInfo[] methods = type.GetMethods(bindingFlags);
				DotNetAdapter.PopulateMethodReflectionTable(type, methods, typeMethods);
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				TypeInfo typeInfo2 = type2.GetTypeInfo();
				if (TypeResolver.IsPublic(typeInfo2) && (!typeInfo2.IsGenericType || !type.IsArray))
				{
					MethodInfo[] array;
					if (typeInfo.IsInterface)
					{
						array = type2.GetMethods(bindingFlags);
					}
					else
					{
						array = typeInfo.GetRuntimeInterfaceMap(type2).InterfaceMethods;
					}
					foreach (MethodInfo methodInfo in array)
					{
						if (methodInfo.IsPublic && methodInfo.IsStatic == ((BindingFlags.Static & bindingFlags) != BindingFlags.Default))
						{
							List<MethodBase> list = (List<MethodBase>)typeMethods[methodInfo.Name];
							if (list == null)
							{
								List<MethodBase> member = new List<MethodBase>
								{
									methodInfo
								};
								typeMethods.Add(methodInfo.Name, member);
							}
							else if (!list.Contains(methodInfo))
							{
								list.Add(methodInfo);
							}
						}
					}
				}
			}
			if ((bindingFlags & BindingFlags.Static) != BindingFlags.Default && TypeResolver.IsPublic(typeInfo) && (List<MethodBase>)typeMethods["new"] == null)
			{
				BindingFlags bindingFlags2 = bindingFlags & ~(BindingFlags.Static | BindingFlags.FlattenHierarchy);
				bindingFlags2 |= BindingFlags.Instance;
				ConstructorInfo[] constructors = type.GetConstructors(bindingFlags2);
				DotNetAdapter.PopulateMethodReflectionTable(constructors, typeMethods);
			}
			for (int k = 0; k < typeMethods.memberCollection.Count; k++)
			{
				typeMethods.memberCollection[k] = new DotNetAdapter.MethodCacheEntry(((List<MethodBase>)typeMethods.memberCollection[k]).ToArray());
			}
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0004E998 File Offset: 0x0004CB98
		private static void PopulateEventReflectionTable(Type type, Dictionary<string, DotNetAdapter.EventCacheEntry> typeEvents, BindingFlags bindingFlags)
		{
			if (type != null)
			{
				EventInfo[] events = type.GetEvents(bindingFlags);
				Dictionary<string, List<EventInfo>> dictionary = new Dictionary<string, List<EventInfo>>(StringComparer.OrdinalIgnoreCase);
				foreach (EventInfo eventInfo in events)
				{
					string name = eventInfo.Name;
					List<EventInfo> list;
					if (!dictionary.TryGetValue(name, out list))
					{
						List<EventInfo> value = new List<EventInfo>
						{
							eventInfo
						};
						dictionary.Add(name, value);
					}
					else
					{
						list.Add(eventInfo);
					}
				}
				foreach (KeyValuePair<string, List<EventInfo>> keyValuePair in dictionary)
				{
					typeEvents.Add(keyValuePair.Key, new DotNetAdapter.EventCacheEntry(keyValuePair.Value.ToArray()));
				}
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0004EA6C File Offset: 0x0004CC6C
		private static bool PropertyAlreadyPresent(List<PropertyInfo> previousProperties, PropertyInfo property)
		{
			bool result = false;
			ParameterInfo[] indexParameters = property.GetIndexParameters();
			int num = indexParameters.Length;
			for (int i = 0; i < previousProperties.Count; i++)
			{
				PropertyInfo propertyInfo = previousProperties[i];
				ParameterInfo[] indexParameters2 = propertyInfo.GetIndexParameters();
				if (indexParameters2.Length == num)
				{
					bool flag = true;
					for (int j = 0; j < indexParameters2.Length; j++)
					{
						ParameterInfo parameterInfo = indexParameters2[j];
						ParameterInfo parameterInfo2 = indexParameters[j];
						if (parameterInfo.ParameterType != parameterInfo2.ParameterType)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0004EAF8 File Offset: 0x0004CCF8
		private static void PopulatePropertyReflectionTable(Type type, CacheTable typeProperties, BindingFlags bindingFlags)
		{
			Dictionary<string, List<PropertyInfo>> dictionary = new Dictionary<string, List<PropertyInfo>>(StringComparer.OrdinalIgnoreCase);
			if (type != null)
			{
				PropertyInfo[] properties = type.GetProperties(bindingFlags);
				for (int i = 0; i < properties.Length; i++)
				{
					DotNetAdapter.PopulateSingleProperty(type, properties[i], dictionary, properties[i].Name);
				}
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				if (TypeResolver.IsPublic(type2))
				{
					PropertyInfo[] properties = type2.GetProperties(bindingFlags);
					for (int k = 0; k < properties.Length; k++)
					{
						DotNetAdapter.PopulateSingleProperty(type, properties[k], dictionary, properties[k].Name);
					}
				}
			}
			foreach (KeyValuePair<string, List<PropertyInfo>> keyValuePair in dictionary)
			{
				List<PropertyInfo> value = keyValuePair.Value;
				PropertyInfo propertyInfo = value[0];
				if (value.Count > 1 || propertyInfo.GetIndexParameters().Length != 0)
				{
					typeProperties.Add(keyValuePair.Key, new DotNetAdapter.ParameterizedPropertyCacheEntry(value));
				}
				else
				{
					typeProperties.Add(keyValuePair.Key, new DotNetAdapter.PropertyCacheEntry(propertyInfo));
				}
			}
			if (type != null)
			{
				foreach (FieldInfo fieldInfo in type.GetFields(bindingFlags))
				{
					string name = fieldInfo.Name;
					DotNetAdapter.PropertyCacheEntry propertyCacheEntry = (DotNetAdapter.PropertyCacheEntry)typeProperties[name];
					if (propertyCacheEntry == null)
					{
						typeProperties.Add(name, new DotNetAdapter.PropertyCacheEntry(fieldInfo));
					}
					else if (!string.Equals(propertyCacheEntry.member.Name, name))
					{
						throw new ExtendedTypeSystemException("NotACLSComplaintField", null, ExtendedTypeSystem.NotAClsCompliantFieldProperty, new object[]
						{
							name,
							type.FullName,
							propertyCacheEntry.member.Name
						});
					}
				}
			}
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0004ECE0 File Offset: 0x0004CEE0
		private static void PopulateSingleProperty(Type type, PropertyInfo property, Dictionary<string, List<PropertyInfo>> tempTable, string propertyName)
		{
			List<PropertyInfo> list;
			if (!tempTable.TryGetValue(propertyName, out list))
			{
				list = new List<PropertyInfo>
				{
					property
				};
				tempTable.Add(propertyName, list);
				return;
			}
			PropertyInfo propertyInfo = list[0];
			if (!string.Equals(property.Name, propertyInfo.Name, StringComparison.Ordinal))
			{
				throw new ExtendedTypeSystemException("NotACLSComplaintProperty", null, ExtendedTypeSystem.NotAClsCompliantFieldProperty, new object[]
				{
					property.Name,
					type.FullName,
					propertyInfo.Name
				});
			}
			if (DotNetAdapter.PropertyAlreadyPresent(list, property))
			{
				return;
			}
			list.Add(property);
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0004ED74 File Offset: 0x0004CF74
		private static CacheTable GetStaticPropertyReflectionTable(Type type)
		{
			CacheTable result;
			lock (DotNetAdapter.staticPropertyCacheTable)
			{
				CacheTable cacheTable = null;
				if (DotNetAdapter.staticPropertyCacheTable.TryGetValue(type, out cacheTable))
				{
					result = cacheTable;
				}
				else
				{
					cacheTable = new CacheTable();
					DotNetAdapter.PopulatePropertyReflectionTable(type, cacheTable, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					DotNetAdapter.staticPropertyCacheTable[type] = cacheTable;
					result = cacheTable;
				}
			}
			return result;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0004EDE0 File Offset: 0x0004CFE0
		private static CacheTable GetStaticMethodReflectionTable(Type type)
		{
			CacheTable result;
			lock (DotNetAdapter.staticMethodCacheTable)
			{
				CacheTable cacheTable = null;
				if (DotNetAdapter.staticMethodCacheTable.TryGetValue(type, out cacheTable))
				{
					result = cacheTable;
				}
				else
				{
					cacheTable = new CacheTable();
					DotNetAdapter.PopulateMethodReflectionTable(type, cacheTable, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					DotNetAdapter.staticMethodCacheTable[type] = cacheTable;
					result = cacheTable;
				}
			}
			return result;
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0004EE4C File Offset: 0x0004D04C
		private static Dictionary<string, DotNetAdapter.EventCacheEntry> GetStaticEventReflectionTable(Type type)
		{
			Dictionary<string, DotNetAdapter.EventCacheEntry> result;
			lock (DotNetAdapter.staticEventCacheTable)
			{
				Dictionary<string, DotNetAdapter.EventCacheEntry> dictionary;
				if (DotNetAdapter.staticEventCacheTable.TryGetValue(type, out dictionary))
				{
					result = dictionary;
				}
				else
				{
					dictionary = new Dictionary<string, DotNetAdapter.EventCacheEntry>();
					DotNetAdapter.PopulateEventReflectionTable(type, dictionary, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					DotNetAdapter.staticEventCacheTable[type] = dictionary;
					result = dictionary;
				}
			}
			return result;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0004EEB8 File Offset: 0x0004D0B8
		private static CacheTable GetInstancePropertyReflectionTable(Type type)
		{
			CacheTable result;
			lock (DotNetAdapter.instancePropertyCacheTable)
			{
				CacheTable cacheTable = null;
				if (DotNetAdapter.instancePropertyCacheTable.TryGetValue(type, out cacheTable))
				{
					result = cacheTable;
				}
				else
				{
					cacheTable = new CacheTable();
					DotNetAdapter.PopulatePropertyReflectionTable(type, cacheTable, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					DotNetAdapter.instancePropertyCacheTable[type] = cacheTable;
					result = cacheTable;
				}
			}
			return result;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0004EF24 File Offset: 0x0004D124
		private static CacheTable GetInstanceMethodReflectionTable(Type type)
		{
			CacheTable result;
			lock (DotNetAdapter.instanceMethodCacheTable)
			{
				CacheTable cacheTable = null;
				if (DotNetAdapter.instanceMethodCacheTable.TryGetValue(type, out cacheTable))
				{
					result = cacheTable;
				}
				else
				{
					cacheTable = new CacheTable();
					DotNetAdapter.PopulateMethodReflectionTable(type, cacheTable, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					DotNetAdapter.instanceMethodCacheTable[type] = cacheTable;
					result = cacheTable;
				}
			}
			return result;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0004F1B0 File Offset: 0x0004D3B0
		internal IEnumerable<object> GetPropertiesAndMethods(Type type, bool @static)
		{
			CacheTable propertyTable = @static ? DotNetAdapter.GetStaticPropertyReflectionTable(type) : DotNetAdapter.GetInstancePropertyReflectionTable(type);
			for (int i = 0; i < propertyTable.memberCollection.Count; i++)
			{
				DotNetAdapter.PropertyCacheEntry propertyCacheEntry = propertyTable.memberCollection[i] as DotNetAdapter.PropertyCacheEntry;
				if (propertyCacheEntry != null)
				{
					yield return propertyCacheEntry.member;
				}
			}
			CacheTable methodTable = @static ? DotNetAdapter.GetStaticMethodReflectionTable(type) : DotNetAdapter.GetInstanceMethodReflectionTable(type);
			for (int j = 0; j < methodTable.memberCollection.Count; j++)
			{
				DotNetAdapter.MethodCacheEntry method = methodTable.memberCollection[j] as DotNetAdapter.MethodCacheEntry;
				if (method != null && !method[0].method.IsSpecialName)
				{
					yield return method;
				}
			}
			yield break;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0004F1DC File Offset: 0x0004D3DC
		private static Dictionary<string, DotNetAdapter.EventCacheEntry> GetInstanceEventReflectionTable(Type type)
		{
			Dictionary<string, DotNetAdapter.EventCacheEntry> result;
			lock (DotNetAdapter.instanceEventCacheTable)
			{
				Dictionary<string, DotNetAdapter.EventCacheEntry> dictionary;
				if (DotNetAdapter.instanceEventCacheTable.TryGetValue(type, out dictionary))
				{
					result = dictionary;
				}
				else
				{
					dictionary = new Dictionary<string, DotNetAdapter.EventCacheEntry>(StringComparer.OrdinalIgnoreCase);
					DotNetAdapter.PopulateEventReflectionTable(type, dictionary, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					DotNetAdapter.instanceEventCacheTable[type] = dictionary;
					result = dictionary;
				}
			}
			return result;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0004F24C File Offset: 0x0004D44C
		internal static bool IsTypeParameterizedProperty(Type t)
		{
			return t == typeof(PSMemberInfo) || t == typeof(PSParameterizedProperty);
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0004F272 File Offset: 0x0004D472
		internal static bool IsRuntimeTypeInstance(object obj)
		{
			return obj.GetType() == DotNetAdapter.RuntimeType;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0004F284 File Offset: 0x0004D484
		internal T GetDotNetProperty<T>(object obj, string propertyName) where T : PSMemberInfo
		{
			bool flag = typeof(T).IsAssignableFrom(typeof(PSProperty));
			bool flag2 = DotNetAdapter.IsTypeParameterizedProperty(typeof(T));
			if (!flag && !flag2)
			{
				return default(T);
			}
			CacheTable cacheTable = this.isStatic ? DotNetAdapter.GetStaticPropertyReflectionTable((Type)obj) : DotNetAdapter.GetInstancePropertyReflectionTable(DotNetAdapter.IsRuntimeTypeInstance(obj) ? typeof(Type) : obj.GetType());
			object obj2 = cacheTable[propertyName];
			if (obj2 == null)
			{
				return default(T);
			}
			DotNetAdapter.PropertyCacheEntry propertyCacheEntry = obj2 as DotNetAdapter.PropertyCacheEntry;
			if (propertyCacheEntry != null && flag)
			{
				bool isHidden = propertyCacheEntry.member.GetCustomAttributes(typeof(HiddenAttribute), false).Any<object>();
				return new PSProperty(propertyCacheEntry.member.Name, this, obj, propertyCacheEntry)
				{
					IsHidden = isHidden
				} as T;
			}
			DotNetAdapter.ParameterizedPropertyCacheEntry parameterizedPropertyCacheEntry = obj2 as DotNetAdapter.ParameterizedPropertyCacheEntry;
			if (parameterizedPropertyCacheEntry != null && flag2)
			{
				return new PSParameterizedProperty(parameterizedPropertyCacheEntry.propertyName, this, obj, parameterizedPropertyCacheEntry) as T;
			}
			return default(T);
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0004F3A8 File Offset: 0x0004D5A8
		internal T GetDotNetMethod<T>(object obj, string methodName) where T : PSMemberInfo
		{
			if (!typeof(T).IsAssignableFrom(typeof(PSMethod)))
			{
				return default(T);
			}
			CacheTable cacheTable = this.isStatic ? DotNetAdapter.GetStaticMethodReflectionTable((Type)obj) : DotNetAdapter.GetInstanceMethodReflectionTable(DotNetAdapter.IsRuntimeTypeInstance(obj) ? typeof(Type) : obj.GetType());
			DotNetAdapter.MethodCacheEntry methodCacheEntry = (DotNetAdapter.MethodCacheEntry)cacheTable[methodName];
			if (methodCacheEntry == null)
			{
				return default(T);
			}
			bool isSpecial = !(methodCacheEntry[0].method is ConstructorInfo) && methodCacheEntry[0].method.IsSpecialName;
			bool isHidden = false;
			foreach (MethodInformation methodInformation in methodCacheEntry.methodInformationStructures)
			{
				if (methodInformation.method.GetCustomAttributes(typeof(HiddenAttribute), false).Any<object>())
				{
					isHidden = true;
					break;
				}
			}
			return new PSMethod(methodCacheEntry[0].method.Name, this, obj, methodCacheEntry, isSpecial, isHidden) as T;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0004F4C8 File Offset: 0x0004D6C8
		internal void AddAllProperties<T>(object obj, PSMemberInfoInternalCollection<T> members, bool ignoreDuplicates) where T : PSMemberInfo
		{
			bool flag = typeof(T).IsAssignableFrom(typeof(PSProperty));
			bool flag2 = DotNetAdapter.IsTypeParameterizedProperty(typeof(T));
			if (!flag && !flag2)
			{
				return;
			}
			CacheTable cacheTable = this.isStatic ? DotNetAdapter.GetStaticPropertyReflectionTable((Type)obj) : DotNetAdapter.GetInstancePropertyReflectionTable(DotNetAdapter.IsRuntimeTypeInstance(obj) ? typeof(Type) : obj.GetType());
			for (int i = 0; i < cacheTable.memberCollection.Count; i++)
			{
				DotNetAdapter.PropertyCacheEntry propertyCacheEntry = cacheTable.memberCollection[i] as DotNetAdapter.PropertyCacheEntry;
				if (propertyCacheEntry != null)
				{
					if (flag && (!ignoreDuplicates || members[propertyCacheEntry.member.Name] == null))
					{
						bool isHidden = propertyCacheEntry.member.GetCustomAttributes(typeof(HiddenAttribute), false).Any<object>();
						members.Add(new PSProperty(propertyCacheEntry.member.Name, this, obj, propertyCacheEntry)
						{
							IsHidden = isHidden
						} as T);
					}
				}
				else if (flag2)
				{
					DotNetAdapter.ParameterizedPropertyCacheEntry parameterizedPropertyCacheEntry = (DotNetAdapter.ParameterizedPropertyCacheEntry)cacheTable.memberCollection[i];
					if (!ignoreDuplicates || members[parameterizedPropertyCacheEntry.propertyName] == null)
					{
						members.Add(new PSParameterizedProperty(parameterizedPropertyCacheEntry.propertyName, this, obj, parameterizedPropertyCacheEntry) as T);
					}
				}
			}
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0004F634 File Offset: 0x0004D834
		internal void AddAllMethods<T>(object obj, PSMemberInfoInternalCollection<T> members, bool ignoreDuplicates) where T : PSMemberInfo
		{
			if (!typeof(T).IsAssignableFrom(typeof(PSMethod)))
			{
				return;
			}
			CacheTable cacheTable = this.isStatic ? DotNetAdapter.GetStaticMethodReflectionTable((Type)obj) : DotNetAdapter.GetInstanceMethodReflectionTable(DotNetAdapter.IsRuntimeTypeInstance(obj) ? typeof(Type) : obj.GetType());
			for (int i = 0; i < cacheTable.memberCollection.Count; i++)
			{
				DotNetAdapter.MethodCacheEntry methodCacheEntry = (DotNetAdapter.MethodCacheEntry)cacheTable.memberCollection[i];
				bool flag = methodCacheEntry[0].method is ConstructorInfo;
				string name = flag ? "new" : methodCacheEntry[0].method.Name;
				if (!ignoreDuplicates || members[name] == null)
				{
					bool isSpecial = !flag && methodCacheEntry[0].method.IsSpecialName;
					bool isHidden = false;
					foreach (MethodInformation methodInformation in methodCacheEntry.methodInformationStructures)
					{
						if (methodInformation.method.GetCustomAttributes(typeof(HiddenAttribute), false).Any<object>())
						{
							isHidden = true;
							break;
						}
					}
					members.Add(new PSMethod(name, this, obj, methodCacheEntry, isSpecial, isHidden) as T);
				}
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0004F784 File Offset: 0x0004D984
		internal void AddAllEvents<T>(object obj, PSMemberInfoInternalCollection<T> members, bool ignoreDuplicates) where T : PSMemberInfo
		{
			if (!typeof(T).IsAssignableFrom(typeof(PSEvent)))
			{
				return;
			}
			Dictionary<string, DotNetAdapter.EventCacheEntry> dictionary = this.isStatic ? DotNetAdapter.GetStaticEventReflectionTable((Type)obj) : DotNetAdapter.GetInstanceEventReflectionTable(DotNetAdapter.IsRuntimeTypeInstance(obj) ? typeof(Type) : obj.GetType());
			foreach (DotNetAdapter.EventCacheEntry eventCacheEntry in dictionary.Values)
			{
				if (!ignoreDuplicates || members[eventCacheEntry.events[0].Name] == null)
				{
					members.Add(new PSEvent(eventCacheEntry.events[0]) as T);
				}
			}
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0004F85C File Offset: 0x0004DA5C
		internal void AddAllDynamicMembers<T>(object obj, PSMemberInfoInternalCollection<T> members, bool ignoreDuplicates) where T : PSMemberInfo
		{
			IDynamicMetaObjectProvider dynamicMetaObjectProvider = obj as IDynamicMetaObjectProvider;
			if (dynamicMetaObjectProvider == null || obj is PSObject)
			{
				return;
			}
			if (!typeof(T).IsAssignableFrom(typeof(PSDynamicMember)))
			{
				return;
			}
			foreach (string name in dynamicMetaObjectProvider.GetMetaObject(Expression.Variable(dynamicMetaObjectProvider.GetType())).GetDynamicMemberNames())
			{
				members.Add(new PSDynamicMember(name) as T);
			}
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0004F8F8 File Offset: 0x0004DAF8
		private static bool PropertyIsStatic(PSProperty property)
		{
			DotNetAdapter.PropertyCacheEntry propertyCacheEntry = property.adapterData as DotNetAdapter.PropertyCacheEntry;
			return propertyCacheEntry != null && propertyCacheEntry.isStatic;
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x0004F91C File Offset: 0x0004DB1C
		internal override bool SiteBinderCanOptimize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0004F92D File Offset: 0x0004DB2D
		internal static ConsolidatedString GetInternedTypeNameHierarchy(Type type)
		{
			return DotNetAdapter.typeToTypeNameDictionary.GetOrAdd(type, (Type t) => new ConsolidatedString(Adapter.GetDotNetTypeNameHierarchy(t), true));
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0004F957 File Offset: 0x0004DB57
		protected override ConsolidatedString GetInternedTypeNameHierarchy(object obj)
		{
			return DotNetAdapter.GetInternedTypeNameHierarchy(obj.GetType());
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x0004F964 File Offset: 0x0004DB64
		protected override T GetMember<T>(object obj, string memberName)
		{
			T dotNetProperty = this.GetDotNetProperty<T>(obj, memberName);
			if (dotNetProperty != null)
			{
				return dotNetProperty;
			}
			return this.GetDotNetMethod<T>(obj, memberName);
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0004F98C File Offset: 0x0004DB8C
		protected override PSMemberInfoInternalCollection<T> GetMembers<T>(object obj)
		{
			PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<T>();
			this.AddAllProperties<T>(obj, psmemberInfoInternalCollection, false);
			this.AddAllMethods<T>(obj, psmemberInfoInternalCollection, false);
			this.AddAllEvents<T>(obj, psmemberInfoInternalCollection, false);
			this.AddAllDynamicMembers<T>(obj, psmemberInfoInternalCollection, false);
			return psmemberInfoInternalCollection;
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0004F9C4 File Offset: 0x0004DBC4
		protected override AttributeCollection PropertyAttributes(PSProperty property)
		{
			DotNetAdapter.PropertyCacheEntry propertyCacheEntry = (DotNetAdapter.PropertyCacheEntry)property.adapterData;
			return propertyCacheEntry.Attributes;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0004F9E4 File Offset: 0x0004DBE4
		protected override string PropertyToString(PSProperty property)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (DotNetAdapter.PropertyIsStatic(property))
			{
				stringBuilder.Append("static ");
			}
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

		// Token: 0x06000E7A RID: 3706 RVA: 0x0004FA7C File Offset: 0x0004DC7C
		protected override object PropertyGet(PSProperty property)
		{
			DotNetAdapter.PropertyCacheEntry propertyCacheEntry = (DotNetAdapter.PropertyCacheEntry)property.adapterData;
			PropertyInfo propertyInfo = propertyCacheEntry.member as PropertyInfo;
			if (propertyInfo != null)
			{
				if (propertyCacheEntry.writeOnly)
				{
					throw new GetValueException("WriteOnlyProperty", null, ExtendedTypeSystem.WriteOnlyProperty, new object[]
					{
						propertyInfo.Name
					});
				}
				if (propertyCacheEntry.useReflection)
				{
					return propertyInfo.GetValue(property.baseObject, null);
				}
				return propertyCacheEntry.getterDelegate(property.baseObject);
			}
			else
			{
				FieldInfo fieldInfo = propertyCacheEntry.member as FieldInfo;
				if (propertyCacheEntry.useReflection)
				{
					return fieldInfo.GetValue(property.baseObject);
				}
				return propertyCacheEntry.getterDelegate(property.baseObject);
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0004FB30 File Offset: 0x0004DD30
		protected override void PropertySet(PSProperty property, object setValue, bool convertIfPossible)
		{
			DotNetAdapter.PropertyCacheEntry propertyCacheEntry = (DotNetAdapter.PropertyCacheEntry)property.adapterData;
			if (propertyCacheEntry.readOnly)
			{
				throw new SetValueException("ReadOnlyProperty", null, ExtendedTypeSystem.ReadOnlyProperty, new object[]
				{
					propertyCacheEntry.member.Name
				});
			}
			PropertyInfo propertyInfo = propertyCacheEntry.member as PropertyInfo;
			if (propertyInfo != null)
			{
				if (convertIfPossible)
				{
					setValue = Adapter.PropertySetAndMethodArgumentConvertTo(setValue, propertyInfo.PropertyType, CultureInfo.InvariantCulture);
				}
				if (propertyCacheEntry.useReflection)
				{
					propertyInfo.SetValue(property.baseObject, setValue, null);
					return;
				}
				propertyCacheEntry.setterDelegate(property.baseObject, setValue);
				return;
			}
			else
			{
				FieldInfo fieldInfo = propertyCacheEntry.member as FieldInfo;
				if (convertIfPossible)
				{
					setValue = Adapter.PropertySetAndMethodArgumentConvertTo(setValue, fieldInfo.FieldType, CultureInfo.InvariantCulture);
				}
				if (propertyCacheEntry.useReflection)
				{
					fieldInfo.SetValue(property.baseObject, setValue);
					return;
				}
				propertyCacheEntry.setterDelegate(property.baseObject, setValue);
				return;
			}
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0004FC17 File Offset: 0x0004DE17
		protected override bool PropertyIsSettable(PSProperty property)
		{
			return !((DotNetAdapter.PropertyCacheEntry)property.adapterData).readOnly;
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0004FC2C File Offset: 0x0004DE2C
		protected override bool PropertyIsGettable(PSProperty property)
		{
			return !((DotNetAdapter.PropertyCacheEntry)property.adapterData).writeOnly;
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x0004FC44 File Offset: 0x0004DE44
		protected override string PropertyType(PSProperty property, bool forDisplay)
		{
			Type propertyType = ((DotNetAdapter.PropertyCacheEntry)property.adapterData).propertyType;
			if (!forDisplay)
			{
				return propertyType.FullName;
			}
			return ToStringCodeMethods.Type(propertyType, false);
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0004FC74 File Offset: 0x0004DE74
		internal static object AuxiliaryConstructorInvoke(MethodInformation methodInformation, object[] arguments, object[] originalArguments)
		{
			object result;
			try
			{
				result = ((ConstructorInfo)methodInformation.method).Invoke(arguments);
			}
			catch (TargetInvocationException ex)
			{
				Exception ex2 = (ex.InnerException == null) ? ex : ex.InnerException;
				throw new MethodInvocationException("DotNetconstructorTargetInvocation", ex2, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					".ctor",
					arguments.Length,
					ex2.Message
				});
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				throw new MethodInvocationException("DotNetconstructorException", ex3, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					".ctor",
					arguments.Length,
					ex3.Message
				});
			}
			Adapter.SetReferences(arguments, methodInformation, originalArguments);
			return result;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0004FD4C File Offset: 0x0004DF4C
		internal static object AuxiliaryMethodInvoke(object target, object[] arguments, MethodInformation methodInformation, object[] originalArguments)
		{
			object result;
			try
			{
				result = methodInformation.Invoke(target, arguments);
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException is FlowControlException || ex.InnerException is ScriptCallDepthException)
				{
					throw ex.InnerException;
				}
				if (ex.InnerException is ParameterBindingException)
				{
					throw ex.InnerException;
				}
				Exception ex2 = (ex.InnerException == null) ? ex : ex.InnerException;
				throw new MethodInvocationException("DotNetMethodTargetInvocation", ex2, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					methodInformation.method.Name,
					arguments.Length,
					ex2.Message
				});
			}
			catch (ParameterBindingException)
			{
				throw;
			}
			catch (FlowControlException)
			{
				throw;
			}
			catch (ScriptCallDepthException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				if (methodInformation.method.DeclaringType == typeof(SteppablePipeline) && (methodInformation.method.Name.Equals("Begin") || methodInformation.method.Name.Equals("Process") || methodInformation.method.Name.Equals("End")))
				{
					throw;
				}
				throw new MethodInvocationException("DotNetMethodException", ex3, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					methodInformation.method.Name,
					arguments.Length,
					ex3.Message
				});
			}
			Adapter.SetReferences(arguments, methodInformation, originalArguments);
			MethodInfo methodInfo = methodInformation.method as MethodInfo;
			if (methodInfo != null && methodInfo.ReturnType != typeof(void))
			{
				return result;
			}
			return AutomationNull.Value;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0004FF30 File Offset: 0x0004E130
		internal static MethodInformation[] GetMethodInformationArray(MethodBase[] methods)
		{
			int num = methods.Length;
			MethodInformation[] array = new MethodInformation[num];
			for (int i = 0; i < methods.Length; i++)
			{
				array[i] = new MethodInformation(methods[i], 0);
			}
			return array;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0004FF64 File Offset: 0x0004E164
		internal static object MethodInvokeDotNet(string methodName, object target, MethodInformation[] methodInformation, PSMethodInvocationConstraints invocationConstraints, object[] arguments)
		{
			object[] array;
			MethodInformation bestMethodAndArguments = Adapter.GetBestMethodAndArguments(methodName, methodInformation, invocationConstraints, arguments, out array);
			if (bestMethodAndArguments.method is ConstructorInfo)
			{
				return DotNetAdapter.InvokeResolvedConstructor(bestMethodAndArguments, array, arguments);
			}
			string methodDefinition = bestMethodAndArguments.methodDefinition;
			ScriptTrace.Trace(1, "TraceMethodCall", ParserStrings.TraceMethodCall, new object[]
			{
				methodDefinition
			});
			PSObject.memberResolution.WriteLine("Calling Method: {0}", new object[]
			{
				methodDefinition
			});
			return DotNetAdapter.AuxiliaryMethodInvoke(target, array, bestMethodAndArguments, arguments);
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0004FFE0 File Offset: 0x0004E1E0
		internal static object ConstructorInvokeDotNet(Type type, ConstructorInfo[] constructors, object[] arguments)
		{
			MethodInformation[] methodInformationArray = DotNetAdapter.GetMethodInformationArray(constructors);
			object[] newArguments;
			MethodInformation bestMethodAndArguments = Adapter.GetBestMethodAndArguments(type.Name, methodInformationArray, arguments, out newArguments);
			return DotNetAdapter.InvokeResolvedConstructor(bestMethodAndArguments, newArguments, arguments);
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0005000C File Offset: 0x0004E20C
		private static object InvokeResolvedConstructor(MethodInformation bestMethod, object[] newArguments, object[] arguments)
		{
			if ((PSObject.memberResolution.Options & PSTraceSourceOptions.WriteLine) != PSTraceSourceOptions.None)
			{
				PSObject.memberResolution.WriteLine("Calling Constructor: {0}", new object[]
				{
					DotNetAdapter.GetMethodInfoOverloadDefinition(null, bestMethod.method, 0)
				});
			}
			return DotNetAdapter.AuxiliaryConstructorInvoke(bestMethod, newArguments, arguments);
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0005005C File Offset: 0x0004E25C
		internal static void ParameterizedPropertyInvokeSet(string propertyName, object target, object valuetoSet, MethodInformation[] methodInformation, object[] arguments)
		{
			object[] array;
			MethodInformation bestMethodAndArguments = Adapter.GetBestMethodAndArguments(propertyName, methodInformation, arguments, out array);
			PSObject.memberResolution.WriteLine("Calling Set Method: {0}", new object[]
			{
				bestMethodAndArguments.methodDefinition
			});
			ParameterInfo[] parameters = bestMethodAndArguments.method.GetParameters();
			Type parameterType = parameters[parameters.Length - 1].ParameterType;
			object obj;
			try
			{
				obj = Adapter.PropertySetAndMethodArgumentConvertTo(valuetoSet, parameterType, CultureInfo.InvariantCulture);
			}
			catch (InvalidCastException ex)
			{
				throw new MethodException("PropertySetterConversionInvalidCastArgument", ex, ExtendedTypeSystem.MethodArgumentConversionException, new object[]
				{
					arguments.Length - 1,
					valuetoSet,
					propertyName,
					parameterType,
					ex.Message
				});
			}
			object[] array2 = new object[array.Length + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i];
			}
			array2[array.Length] = obj;
			DotNetAdapter.AuxiliaryMethodInvoke(target, array2, bestMethodAndArguments, arguments);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00050154 File Offset: 0x0004E354
		internal static string GetMethodInfoOverloadDefinition(string memberName, MethodBase methodEntry, int parametersToIgnore)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (methodEntry.IsStatic)
			{
				stringBuilder.Append("static ");
			}
			MethodInfo methodInfo = methodEntry as MethodInfo;
			if (methodInfo != null)
			{
				stringBuilder.Append(ToStringCodeMethods.Type(methodInfo.ReturnType, false));
				stringBuilder.Append(" ");
			}
			else
			{
				ConstructorInfo constructorInfo = methodEntry as ConstructorInfo;
				if (constructorInfo != null)
				{
					stringBuilder.Append(ToStringCodeMethods.Type(constructorInfo.DeclaringType, false));
					stringBuilder.Append(" ");
				}
			}
			if (methodEntry.DeclaringType.GetTypeInfo().IsInterface)
			{
				stringBuilder.Append(ToStringCodeMethods.Type(methodEntry.DeclaringType, true));
				stringBuilder.Append(".");
			}
			stringBuilder.Append(memberName ?? methodEntry.Name);
			if (methodEntry.IsGenericMethodDefinition)
			{
				stringBuilder.Append("[");
				Type[] genericArguments = methodEntry.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(ToStringCodeMethods.Type(genericArguments[i], false));
				}
				stringBuilder.Append("]");
			}
			stringBuilder.Append("(");
			ParameterInfo[] parameters = methodEntry.GetParameters();
			int num = parameters.Length - parametersToIgnore;
			if (num > 0)
			{
				for (int j = 0; j < num; j++)
				{
					ParameterInfo parameterInfo = parameters[j];
					Type type = parameterInfo.ParameterType;
					if (type.IsByRef)
					{
						stringBuilder.Append("[ref] ");
						type = type.GetElementType();
					}
					if (type.IsArray && j == num - 1)
					{
						object[] customAttributes = parameterInfo.GetCustomAttributes(typeof(ParamArrayAttribute), false);
						if (customAttributes != null && customAttributes.Any<object>())
						{
							stringBuilder.Append("Params ");
						}
					}
					stringBuilder.Append(ToStringCodeMethods.Type(type, false));
					stringBuilder.Append(" ");
					stringBuilder.Append(parameterInfo.Name);
					stringBuilder.Append(", ");
				}
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00050377 File Offset: 0x0004E577
		protected override object MethodInvoke(PSMethod method, object[] arguments)
		{
			return this.MethodInvoke(method, null, arguments);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00050384 File Offset: 0x0004E584
		protected override object MethodInvoke(PSMethod method, PSMethodInvocationConstraints invocationConstraints, object[] arguments)
		{
			DotNetAdapter.MethodCacheEntry methodCacheEntry = (DotNetAdapter.MethodCacheEntry)method.adapterData;
			return DotNetAdapter.MethodInvokeDotNet(method.Name, method.baseObject, methodCacheEntry.methodInformationStructures, invocationConstraints, arguments);
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x000503C0 File Offset: 0x0004E5C0
		protected override Collection<string> MethodDefinitions(PSMethod method)
		{
			DotNetAdapter.MethodCacheEntry methodCacheEntry = (DotNetAdapter.MethodCacheEntry)method.adapterData;
			IList<string> list = (from m in methodCacheEntry.methodInformationStructures
			select m.methodDefinition).Distinct(StringComparer.Ordinal).ToList<string>();
			return new Collection<string>(list);
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00050418 File Offset: 0x0004E618
		protected override string ParameterizedPropertyType(PSParameterizedProperty property)
		{
			DotNetAdapter.ParameterizedPropertyCacheEntry parameterizedPropertyCacheEntry = (DotNetAdapter.ParameterizedPropertyCacheEntry)property.adapterData;
			return parameterizedPropertyCacheEntry.propertyType.FullName;
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0005043C File Offset: 0x0004E63C
		protected override bool ParameterizedPropertyIsSettable(PSParameterizedProperty property)
		{
			return !((DotNetAdapter.ParameterizedPropertyCacheEntry)property.adapterData).readOnly;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00050451 File Offset: 0x0004E651
		protected override bool ParameterizedPropertyIsGettable(PSParameterizedProperty property)
		{
			return !((DotNetAdapter.ParameterizedPropertyCacheEntry)property.adapterData).writeOnly;
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00050468 File Offset: 0x0004E668
		protected override object ParameterizedPropertyGet(PSParameterizedProperty property, object[] arguments)
		{
			DotNetAdapter.ParameterizedPropertyCacheEntry parameterizedPropertyCacheEntry = (DotNetAdapter.ParameterizedPropertyCacheEntry)property.adapterData;
			return DotNetAdapter.MethodInvokeDotNet(property.Name, property.baseObject, parameterizedPropertyCacheEntry.getterInformation, null, arguments);
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0005049C File Offset: 0x0004E69C
		protected override void ParameterizedPropertySet(PSParameterizedProperty property, object setValue, object[] arguments)
		{
			DotNetAdapter.ParameterizedPropertyCacheEntry parameterizedPropertyCacheEntry = (DotNetAdapter.ParameterizedPropertyCacheEntry)property.adapterData;
			DotNetAdapter.ParameterizedPropertyInvokeSet(parameterizedPropertyCacheEntry.propertyName, property.baseObject, setValue, parameterizedPropertyCacheEntry.setterInformation, arguments);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x000504D0 File Offset: 0x0004E6D0
		protected override Collection<string> ParameterizedPropertyDefinitions(PSParameterizedProperty property)
		{
			DotNetAdapter.ParameterizedPropertyCacheEntry parameterizedPropertyCacheEntry = (DotNetAdapter.ParameterizedPropertyCacheEntry)property.adapterData;
			Collection<string> collection = new Collection<string>();
			for (int i = 0; i < parameterizedPropertyCacheEntry.propertyDefinition.Length; i++)
			{
				collection.Add(parameterizedPropertyCacheEntry.propertyDefinition[i]);
			}
			return collection;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00050514 File Offset: 0x0004E714
		protected override string ParameterizedPropertyToString(PSParameterizedProperty property)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Collection<string> collection = this.ParameterizedPropertyDefinitions(property);
			for (int i = 0; i < collection.Count; i++)
			{
				stringBuilder.Append(collection[i]);
				stringBuilder.Append(", ");
			}
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			return stringBuilder.ToString();
		}

		// Token: 0x0400065A RID: 1626
		private const BindingFlags instanceBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

		// Token: 0x0400065B RID: 1627
		private const BindingFlags staticBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy;

		// Token: 0x0400065C RID: 1628
		private bool isStatic;

		// Token: 0x0400065D RID: 1629
		private static Dictionary<Type, CacheTable> instancePropertyCacheTable = new Dictionary<Type, CacheTable>();

		// Token: 0x0400065E RID: 1630
		private static Dictionary<Type, CacheTable> staticPropertyCacheTable = new Dictionary<Type, CacheTable>();

		// Token: 0x0400065F RID: 1631
		private static Dictionary<Type, CacheTable> instanceMethodCacheTable = new Dictionary<Type, CacheTable>();

		// Token: 0x04000660 RID: 1632
		private static Dictionary<Type, CacheTable> staticMethodCacheTable = new Dictionary<Type, CacheTable>();

		// Token: 0x04000661 RID: 1633
		private static readonly Dictionary<Type, Dictionary<string, DotNetAdapter.EventCacheEntry>> instanceEventCacheTable = new Dictionary<Type, Dictionary<string, DotNetAdapter.EventCacheEntry>>();

		// Token: 0x04000662 RID: 1634
		private static readonly Dictionary<Type, Dictionary<string, DotNetAdapter.EventCacheEntry>> staticEventCacheTable = new Dictionary<Type, Dictionary<string, DotNetAdapter.EventCacheEntry>>();

		// Token: 0x04000663 RID: 1635
		private static readonly Type RuntimeType = typeof(object).GetType();

		// Token: 0x04000664 RID: 1636
		private static ConcurrentDictionary<Type, ConsolidatedString> typeToTypeNameDictionary = new ConcurrentDictionary<Type, ConsolidatedString>();

		// Token: 0x02000106 RID: 262
		internal class MethodCacheEntry
		{
			// Token: 0x06000E94 RID: 3732 RVA: 0x000505D7 File Offset: 0x0004E7D7
			internal MethodCacheEntry(MethodBase[] methods)
			{
				this.methodInformationStructures = DotNetAdapter.GetMethodInformationArray(methods);
			}

			// Token: 0x170003CC RID: 972
			internal MethodInformation this[int i]
			{
				get
				{
					return this.methodInformationStructures[i];
				}
			}

			// Token: 0x04000667 RID: 1639
			internal MethodInformation[] methodInformationStructures;
		}

		// Token: 0x02000107 RID: 263
		internal class EventCacheEntry
		{
			// Token: 0x06000E96 RID: 3734 RVA: 0x000505F5 File Offset: 0x0004E7F5
			internal EventCacheEntry(EventInfo[] events)
			{
				this.events = events;
			}

			// Token: 0x04000668 RID: 1640
			internal EventInfo[] events;
		}

		// Token: 0x02000108 RID: 264
		internal class ParameterizedPropertyCacheEntry
		{
			// Token: 0x06000E97 RID: 3735 RVA: 0x00050604 File Offset: 0x0004E804
			internal ParameterizedPropertyCacheEntry(List<PropertyInfo> properties)
			{
				PropertyInfo propertyInfo = properties[0];
				this.propertyName = propertyInfo.Name;
				this.propertyType = propertyInfo.PropertyType;
				List<MethodInfo> list = new List<MethodInfo>();
				List<MethodInfo> list2 = new List<MethodInfo>();
				List<string> list3 = new List<string>();
				for (int i = 0; i < properties.Count; i++)
				{
					PropertyInfo propertyInfo2 = properties[i];
					if (propertyInfo2.PropertyType != this.propertyType)
					{
						this.propertyType = typeof(object);
					}
					MethodInfo getMethod = propertyInfo2.GetGetMethod();
					StringBuilder stringBuilder = new StringBuilder();
					StringBuilder stringBuilder2 = new StringBuilder();
					if (getMethod != null)
					{
						stringBuilder2.Append("get;");
						stringBuilder.Append(DotNetAdapter.GetMethodInfoOverloadDefinition(this.propertyName, getMethod, 0));
						list.Add(getMethod);
					}
					MethodInfo setMethod = propertyInfo2.GetSetMethod();
					if (setMethod != null)
					{
						stringBuilder2.Append("set;");
						if (stringBuilder.Length == 0)
						{
							stringBuilder.Append(DotNetAdapter.GetMethodInfoOverloadDefinition(this.propertyName, setMethod, 1));
						}
						list2.Add(setMethod);
					}
					stringBuilder.Append(" {");
					stringBuilder.Append(stringBuilder2);
					stringBuilder.Append("}");
					list3.Add(stringBuilder.ToString());
				}
				this.propertyDefinition = list3.ToArray();
				this.writeOnly = (list.Count == 0);
				this.readOnly = (list2.Count == 0);
				this.getterInformation = new MethodInformation[list.Count];
				for (int j = 0; j < list.Count; j++)
				{
					this.getterInformation[j] = new MethodInformation(list[j], 0);
				}
				this.setterInformation = new MethodInformation[list2.Count];
				for (int k = 0; k < list2.Count; k++)
				{
					this.setterInformation[k] = new MethodInformation(list2[k], 1);
				}
			}

			// Token: 0x04000669 RID: 1641
			internal MethodInformation[] getterInformation;

			// Token: 0x0400066A RID: 1642
			internal MethodInformation[] setterInformation;

			// Token: 0x0400066B RID: 1643
			internal string propertyName;

			// Token: 0x0400066C RID: 1644
			internal bool readOnly;

			// Token: 0x0400066D RID: 1645
			internal bool writeOnly;

			// Token: 0x0400066E RID: 1646
			internal Type propertyType;

			// Token: 0x0400066F RID: 1647
			internal string[] propertyDefinition;
		}

		// Token: 0x02000109 RID: 265
		internal class PropertyCacheEntry
		{
			// Token: 0x06000E98 RID: 3736 RVA: 0x000507FC File Offset: 0x0004E9FC
			internal PropertyCacheEntry(PropertyInfo property)
			{
				this.member = property;
				this.propertyType = property.PropertyType;
				TypeInfo typeInfo = property.DeclaringType.GetTypeInfo();
				TypeInfo typeInfo2 = property.PropertyType.GetTypeInfo();
				if (typeInfo.IsValueType || typeInfo2.IsGenericType || typeInfo.IsGenericType || property.DeclaringType.IsComObject() || property.PropertyType.IsComObject())
				{
					this.readOnly = (property.GetSetMethod() == null);
					this.writeOnly = (property.GetGetMethod() == null);
					this.useReflection = true;
					return;
				}
				MethodInfo getMethod = property.GetGetMethod(true);
				if (getMethod != null && (getMethod.IsPublic || getMethod.IsFamily))
				{
					this.isStatic = getMethod.IsStatic;
					ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
					Expression expression = this.isStatic ? null : parameterExpression.Cast(getMethod.DeclaringType);
					this.getterDelegate = Expression.Lambda<DotNetAdapter.PropertyCacheEntry.GetterDelegate>(Expression.Property(expression, property).Cast(typeof(object)), new ParameterExpression[]
					{
						parameterExpression
					}).Compile();
				}
				else
				{
					this.writeOnly = true;
				}
				MethodInfo setMethod = property.GetSetMethod(true);
				if (setMethod != null && (setMethod.IsPublic || setMethod.IsFamily))
				{
					this.isStatic = setMethod.IsStatic;
					ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
					ParameterExpression parameterExpression3 = Expression.Parameter(typeof(object));
					Expression expression2 = this.isStatic ? null : parameterExpression2.Cast(setMethod.DeclaringType);
					this.setterDelegate = Expression.Lambda<DotNetAdapter.PropertyCacheEntry.SetterDelegate>(Expression.Assign(Expression.Property(expression2, property), Expression.Convert(parameterExpression3, property.PropertyType)), new ParameterExpression[]
					{
						parameterExpression2,
						parameterExpression3
					}).Compile();
					return;
				}
				this.readOnly = true;
			}

			// Token: 0x06000E99 RID: 3737 RVA: 0x000509EC File Offset: 0x0004EBEC
			internal PropertyCacheEntry(FieldInfo field)
			{
				this.member = field;
				this.isStatic = field.IsStatic;
				this.propertyType = field.FieldType;
				if (field.IsLiteral || field.IsInitOnly)
				{
					this.readOnly = true;
				}
				ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
				ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
				Expression expression = null;
				Type declaringType = field.DeclaringType;
				TypeInfo typeInfo = declaringType.GetTypeInfo();
				if (!field.IsStatic)
				{
					if (typeInfo.IsValueType)
					{
						expression = ((Nullable.GetUnderlyingType(declaringType) != null) ? Expression.Property(parameterExpression, "Value") : Expression.Unbox(parameterExpression, declaringType));
					}
					else
					{
						expression = parameterExpression.Cast(declaringType);
					}
				}
				Expression body;
				if (typeInfo.IsGenericTypeDefinition)
				{
					Expression innerException = Expression.New(CachedReflectionInfo.GetValueException_ctor, new Expression[]
					{
						Expression.Constant("PropertyGetException"),
						Expression.Constant(null, typeof(Exception)),
						Expression.Constant(ParserStrings.PropertyInGenericType),
						Expression.NewArrayInit(typeof(object), new Expression[]
						{
							Expression.Constant(field.Name)
						})
					});
					body = Compiler.ThrowRuntimeErrorWithInnerException("PropertyGetException", Expression.Constant(ParserStrings.PropertyInGenericType), innerException, typeof(object), new Expression[]
					{
						Expression.Constant(field.Name)
					});
				}
				else
				{
					body = Expression.Field(expression, field).Cast(typeof(object));
				}
				this.getterDelegate = Expression.Lambda<DotNetAdapter.PropertyCacheEntry.GetterDelegate>(body, new ParameterExpression[]
				{
					parameterExpression
				}).Compile();
				string text = null;
				Type type = field.FieldType;
				if (typeInfo.IsGenericTypeDefinition)
				{
					text = ParserStrings.PropertyInGenericType;
					if (type.GetTypeInfo().ContainsGenericParameters)
					{
						type = typeof(object);
					}
				}
				else if (this.readOnly)
				{
					text = ParserStrings.PropertyIsReadOnly;
				}
				Expression body2;
				if (text != null)
				{
					Expression innerException2 = Expression.New(CachedReflectionInfo.SetValueException_ctor, new Expression[]
					{
						Expression.Constant("PropertyAssignmentException"),
						Expression.Constant(null, typeof(Exception)),
						Expression.Constant(text),
						Expression.NewArrayInit(typeof(object), new Expression[]
						{
							Expression.Constant(field.Name)
						})
					});
					body2 = Compiler.ThrowRuntimeErrorWithInnerException("PropertyAssignmentException", Expression.Constant(text), innerException2, type, new Expression[]
					{
						Expression.Constant(field.Name)
					});
				}
				else
				{
					body2 = Expression.Assign(Expression.Field(expression, field), Expression.Convert(parameterExpression2, field.FieldType));
				}
				this.setterDelegate = Expression.Lambda<DotNetAdapter.PropertyCacheEntry.SetterDelegate>(body2, new ParameterExpression[]
				{
					parameterExpression,
					parameterExpression2
				}).Compile();
			}

			// Token: 0x170003CD RID: 973
			// (get) Token: 0x06000E9A RID: 3738 RVA: 0x00050CC8 File Offset: 0x0004EEC8
			internal AttributeCollection Attributes
			{
				get
				{
					if (this.attributes == null)
					{
						object[] customAttributes = this.member.GetCustomAttributes(true);
						this.attributes = new AttributeCollection(customAttributes.Cast<Attribute>().ToArray<Attribute>());
					}
					return this.attributes;
				}
			}

			// Token: 0x04000670 RID: 1648
			internal MemberInfo member;

			// Token: 0x04000671 RID: 1649
			internal DotNetAdapter.PropertyCacheEntry.GetterDelegate getterDelegate;

			// Token: 0x04000672 RID: 1650
			internal DotNetAdapter.PropertyCacheEntry.SetterDelegate setterDelegate;

			// Token: 0x04000673 RID: 1651
			internal bool useReflection;

			// Token: 0x04000674 RID: 1652
			internal bool readOnly;

			// Token: 0x04000675 RID: 1653
			internal bool writeOnly;

			// Token: 0x04000676 RID: 1654
			internal bool isStatic;

			// Token: 0x04000677 RID: 1655
			internal Type propertyType;

			// Token: 0x04000678 RID: 1656
			private AttributeCollection attributes;

			// Token: 0x0200010A RID: 266
			// (Invoke) Token: 0x06000E9C RID: 3740
			internal delegate object GetterDelegate(object instance);

			// Token: 0x0200010B RID: 267
			// (Invoke) Token: 0x06000EA0 RID: 3744
			internal delegate void SetterDelegate(object instance, object setValue);
		}
	}
}
