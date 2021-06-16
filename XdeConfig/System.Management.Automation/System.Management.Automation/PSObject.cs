using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Microsoft.Management.Infrastructure;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Cim;

namespace System.Management.Automation
{
	// Token: 0x0200012E RID: 302
	[TypeDescriptionProvider(typeof(PSObjectTypeDescriptionProvider))]
	[Serializable]
	public class PSObject : IFormattable, IComparable, ISerializable, IDynamicMetaObjectProvider
	{
		// Token: 0x06000FDA RID: 4058 RVA: 0x0005A3E8 File Offset: 0x000585E8
		internal TypeTable GetTypeTable()
		{
			TypeTable result;
			if (this._typeTable != null && this._typeTable.TryGetTarget(out result))
			{
				return result;
			}
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			if (executionContextFromTLS != null)
			{
				return executionContextFromTLS.TypeTable;
			}
			return null;
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0005A420 File Offset: 0x00058620
		internal static T TypeTableGetMemberDelegate<T>(PSObject msjObj, string name) where T : PSMemberInfo
		{
			TypeTable typeTable = msjObj.GetTypeTable();
			return PSObject.TypeTableGetMemberDelegate<T>(msjObj, typeTable, name);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0005A43C File Offset: 0x0005863C
		private static T TypeTableGetMemberDelegate<T>(PSObject msjObj, TypeTable typeTableToUse, string name) where T : PSMemberInfo
		{
			if (typeTableToUse == null)
			{
				return default(T);
			}
			PSMemberInfoInternalCollection<PSMemberInfo> members = typeTableToUse.GetMembers<PSMemberInfo>(msjObj.InternalTypeNames);
			PSMemberInfo psmemberInfo = members[name];
			if (psmemberInfo == null)
			{
				PSObject.memberResolution.WriteLine("\"{0}\" NOT present in type table.", new object[]
				{
					name
				});
				return default(T);
			}
			T t = psmemberInfo as T;
			if (t != null)
			{
				PSObject.memberResolution.WriteLine("\"{0}\" present in type table.", new object[]
				{
					name
				});
				return t;
			}
			PSObject.memberResolution.WriteLine("\"{0}\" from types table ignored because it has type {1} instead of {2}.", new object[]
			{
				name,
				psmemberInfo.GetType(),
				typeof(T)
			});
			return default(T);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0005A50C File Offset: 0x0005870C
		internal static PSMemberInfoInternalCollection<T> TypeTableGetMembersDelegate<T>(PSObject msjObj) where T : PSMemberInfo
		{
			TypeTable typeTable = msjObj.GetTypeTable();
			return PSObject.TypeTableGetMembersDelegate<T>(msjObj, typeTable);
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0005A528 File Offset: 0x00058728
		internal static PSMemberInfoInternalCollection<T> TypeTableGetMembersDelegate<T>(PSObject msjObj, TypeTable typeTableToUse) where T : PSMemberInfo
		{
			if (typeTableToUse == null)
			{
				return new PSMemberInfoInternalCollection<T>();
			}
			PSMemberInfoInternalCollection<T> members = typeTableToUse.GetMembers<T>(msjObj.InternalTypeNames);
			PSObject.memberResolution.WriteLine("Type table members: {0}.", new object[]
			{
				members.Count
			});
			return members;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0005A574 File Offset: 0x00058774
		private static T AdapterGetMemberDelegate<T>(PSObject msjObj, string name) where T : PSMemberInfo
		{
			if (!msjObj.isDeserialized)
			{
				T t = msjObj.InternalAdapter.BaseGetMember<T>(msjObj.immediateBaseObject, name);
				PSObject.memberResolution.WriteLine("Adapted member: {0}.", new object[]
				{
					(t == null) ? "not found" : t.Name
				});
				return t;
			}
			if (msjObj.adaptedMembers == null)
			{
				return default(T);
			}
			T t2 = msjObj.adaptedMembers[name] as T;
			PSObject.memberResolution.WriteLine("Serialized adapted member: {0}.", new object[]
			{
				(t2 == null) ? "not found" : t2.Name
			});
			return t2;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0005A638 File Offset: 0x00058838
		internal static PSMemberInfoInternalCollection<U> TransformMemberInfoCollection<T, U>(PSMemberInfoCollection<T> source) where T : PSMemberInfo where U : PSMemberInfo
		{
			if (typeof(T) == typeof(U))
			{
				return source as PSMemberInfoInternalCollection<U>;
			}
			PSMemberInfoInternalCollection<U> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<U>();
			foreach (T t in source)
			{
				U u = t as U;
				if (u != null)
				{
					psmemberInfoInternalCollection.Add(u);
				}
			}
			return psmemberInfoInternalCollection;
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0005A6C4 File Offset: 0x000588C4
		private static PSMemberInfoInternalCollection<T> AdapterGetMembersDelegate<T>(PSObject msjObj) where T : PSMemberInfo
		{
			if (!msjObj.isDeserialized)
			{
				PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = msjObj.InternalAdapter.BaseGetMembers<T>(msjObj.immediateBaseObject);
				PSObject.memberResolution.WriteLine("Adapted members: {0}.", new object[]
				{
					psmemberInfoInternalCollection.VisibleCount
				});
				return psmemberInfoInternalCollection;
			}
			if (msjObj.adaptedMembers == null)
			{
				return new PSMemberInfoInternalCollection<T>();
			}
			PSObject.memberResolution.WriteLine("Serialized adapted members: {0}.", new object[]
			{
				msjObj.adaptedMembers.Count
			});
			return PSObject.TransformMemberInfoCollection<PSPropertyInfo, T>(msjObj.adaptedMembers);
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0005A758 File Offset: 0x00058958
		private static PSMemberInfoInternalCollection<T> DotNetGetMembersDelegate<T>(PSObject msjObj) where T : PSMemberInfo
		{
			if (msjObj.InternalAdapterSet.DotNetAdapter != null)
			{
				PSMemberInfoInternalCollection<T> psmemberInfoInternalCollection = msjObj.InternalAdapterSet.DotNetAdapter.BaseGetMembers<T>(msjObj.immediateBaseObject);
				PSObject.memberResolution.WriteLine("DotNet members: {0}.", new object[]
				{
					psmemberInfoInternalCollection.VisibleCount
				});
				return psmemberInfoInternalCollection;
			}
			return new PSMemberInfoInternalCollection<T>();
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0005A7B8 File Offset: 0x000589B8
		private static T DotNetGetMemberDelegate<T>(PSObject msjObj, string name) where T : PSMemberInfo
		{
			if (msjObj.InternalAdapterSet.DotNetAdapter != null)
			{
				T t = msjObj.InternalAdapterSet.DotNetAdapter.BaseGetMember<T>(msjObj.immediateBaseObject, name);
				PSObject.memberResolution.WriteLine("DotNet member: {0}.", new object[]
				{
					(t == null) ? "not found" : t.Name
				});
				return t;
			}
			return default(T);
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0005A82B File Offset: 0x00058A2B
		internal static Collection<CollectionEntry<PSMemberInfo>> GetMemberCollection(PSMemberViewTypes viewType)
		{
			return PSObject.GetMemberCollection(viewType, null);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0005A85C File Offset: 0x00058A5C
		internal static Collection<CollectionEntry<PSMemberInfo>> GetMemberCollection(PSMemberViewTypes viewType, TypeTable backupTypeTable)
		{
			Collection<CollectionEntry<PSMemberInfo>> collection = new Collection<CollectionEntry<PSMemberInfo>>();
			if ((viewType & PSMemberViewTypes.Extended) == PSMemberViewTypes.Extended)
			{
				if (backupTypeTable == null)
				{
					collection.Add(new CollectionEntry<PSMemberInfo>(new CollectionEntry<PSMemberInfo>.GetMembersDelegate(PSObject.TypeTableGetMembersDelegate<PSMemberInfo>), new CollectionEntry<PSMemberInfo>.GetMemberDelegate(PSObject.TypeTableGetMemberDelegate<PSMemberInfo>), true, true, "type table members"));
				}
				else
				{
					collection.Add(new CollectionEntry<PSMemberInfo>((PSObject msjObj) => PSObject.TypeTableGetMembersDelegate<PSMemberInfo>(msjObj, backupTypeTable), (PSObject msjObj, string name) => PSObject.TypeTableGetMemberDelegate<PSMemberInfo>(msjObj, backupTypeTable, name), true, true, "type table members"));
				}
			}
			if ((viewType & PSMemberViewTypes.Adapted) == PSMemberViewTypes.Adapted)
			{
				collection.Add(new CollectionEntry<PSMemberInfo>(new CollectionEntry<PSMemberInfo>.GetMembersDelegate(PSObject.AdapterGetMembersDelegate<PSMemberInfo>), new CollectionEntry<PSMemberInfo>.GetMemberDelegate(PSObject.AdapterGetMemberDelegate<PSMemberInfo>), false, false, "adapted members"));
			}
			if ((viewType & PSMemberViewTypes.Base) == PSMemberViewTypes.Base)
			{
				collection.Add(new CollectionEntry<PSMemberInfo>(new CollectionEntry<PSMemberInfo>.GetMembersDelegate(PSObject.DotNetGetMembersDelegate<PSMemberInfo>), new CollectionEntry<PSMemberInfo>.GetMemberDelegate(PSObject.DotNetGetMemberDelegate<PSMemberInfo>), false, false, "clr members"));
			}
			return collection;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0005A950 File Offset: 0x00058B50
		private static Collection<CollectionEntry<PSMethodInfo>> GetMethodCollection()
		{
			return new Collection<CollectionEntry<PSMethodInfo>>
			{
				new CollectionEntry<PSMethodInfo>(new CollectionEntry<PSMethodInfo>.GetMembersDelegate(PSObject.TypeTableGetMembersDelegate<PSMethodInfo>), new CollectionEntry<PSMethodInfo>.GetMemberDelegate(PSObject.TypeTableGetMemberDelegate<PSMethodInfo>), true, true, "type table members"),
				new CollectionEntry<PSMethodInfo>(new CollectionEntry<PSMethodInfo>.GetMembersDelegate(PSObject.AdapterGetMembersDelegate<PSMethodInfo>), new CollectionEntry<PSMethodInfo>.GetMemberDelegate(PSObject.AdapterGetMemberDelegate<PSMethodInfo>), false, false, "adapted members"),
				new CollectionEntry<PSMethodInfo>(new CollectionEntry<PSMethodInfo>.GetMembersDelegate(PSObject.DotNetGetMembersDelegate<PSMethodInfo>), new CollectionEntry<PSMethodInfo>.GetMemberDelegate(PSObject.DotNetGetMemberDelegate<PSMethodInfo>), false, false, "clr members")
			};
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0005A9E2 File Offset: 0x00058BE2
		internal static Collection<CollectionEntry<PSPropertyInfo>> GetPropertyCollection(PSMemberViewTypes viewType)
		{
			return PSObject.GetPropertyCollection(viewType, null);
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0005AA10 File Offset: 0x00058C10
		internal static Collection<CollectionEntry<PSPropertyInfo>> GetPropertyCollection(PSMemberViewTypes viewType, TypeTable backupTypeTable)
		{
			Collection<CollectionEntry<PSPropertyInfo>> collection = new Collection<CollectionEntry<PSPropertyInfo>>();
			if ((viewType & PSMemberViewTypes.Extended) == PSMemberViewTypes.Extended)
			{
				if (backupTypeTable == null)
				{
					collection.Add(new CollectionEntry<PSPropertyInfo>(new CollectionEntry<PSPropertyInfo>.GetMembersDelegate(PSObject.TypeTableGetMembersDelegate<PSPropertyInfo>), new CollectionEntry<PSPropertyInfo>.GetMemberDelegate(PSObject.TypeTableGetMemberDelegate<PSPropertyInfo>), true, true, "type table members"));
				}
				else
				{
					collection.Add(new CollectionEntry<PSPropertyInfo>((PSObject msjObj) => PSObject.TypeTableGetMembersDelegate<PSPropertyInfo>(msjObj, backupTypeTable), (PSObject msjObj, string name) => PSObject.TypeTableGetMemberDelegate<PSPropertyInfo>(msjObj, backupTypeTable, name), true, true, "type table members"));
				}
			}
			if ((viewType & PSMemberViewTypes.Adapted) == PSMemberViewTypes.Adapted)
			{
				collection.Add(new CollectionEntry<PSPropertyInfo>(new CollectionEntry<PSPropertyInfo>.GetMembersDelegate(PSObject.AdapterGetMembersDelegate<PSPropertyInfo>), new CollectionEntry<PSPropertyInfo>.GetMemberDelegate(PSObject.AdapterGetMemberDelegate<PSPropertyInfo>), false, false, "adapted members"));
			}
			if ((viewType & PSMemberViewTypes.Base) == PSMemberViewTypes.Base)
			{
				collection.Add(new CollectionEntry<PSPropertyInfo>(new CollectionEntry<PSPropertyInfo>.GetMembersDelegate(PSObject.DotNetGetMembersDelegate<PSPropertyInfo>), new CollectionEntry<PSPropertyInfo>.GetMemberDelegate(PSObject.DotNetGetMemberDelegate<PSPropertyInfo>), false, false, "clr members"));
			}
			return collection;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0005AB04 File Offset: 0x00058D04
		private void CommonInitialization(object obj)
		{
			if (obj is PSCustomObject)
			{
				this.immediateBaseObjectIsEmpty = true;
			}
			this.immediateBaseObject = obj;
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			this._typeTable = ((executionContextFromTLS != null) ? new WeakReference(executionContextFromTLS.TypeTable) : null);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0005AB44 File Offset: 0x00058D44
		internal static void RegisterAdapterMapping(Func<object, PSObject.AdapterSet> mapper)
		{
			lock (PSObject._adapterSetMappers)
			{
				PSObject._adapterSetMappers.Add(mapper);
			}
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0005AB88 File Offset: 0x00058D88
		private static PSObject.AdapterSet MappedInternalAdapterSet(object obj)
		{
			if (obj is PSMemberSet)
			{
				return PSObject.mshMemberSetAdapter;
			}
			if (obj is PSObject)
			{
				return PSObject.mshObjectAdapter;
			}
			if (obj is CimInstance)
			{
				return PSObject.cimInstanceAdapter;
			}
			if (obj is ManagementClass)
			{
				return PSObject.managementClassAdapter;
			}
			if (obj is ManagementBaseObject)
			{
				return PSObject.managementObjectAdapter;
			}
			if (obj is DirectoryEntry)
			{
				return PSObject.directoryEntryAdapter;
			}
			if (obj is DataRowView)
			{
				return PSObject.dataRowViewAdapter;
			}
			if (obj is DataRow)
			{
				return PSObject.dataRowAdapter;
			}
			if (obj is XmlNode)
			{
				return PSObject.xmlNodeAdapter;
			}
			return null;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0005AC14 File Offset: 0x00058E14
		internal static PSObject.AdapterSet GetMappedAdapter(object obj, TypeTable typeTable)
		{
			Type type = obj.GetType();
			if (typeTable != null)
			{
				PSObject.AdapterSet typeAdapter = typeTable.GetTypeAdapter(type);
				if (typeAdapter != null)
				{
					return typeAdapter;
				}
			}
			PSObject.AdapterSet adapterSet;
			if (PSObject._adapterMapping.TryGetValue(type, out adapterSet))
			{
				return adapterSet;
			}
			lock (PSObject._adapterSetMappers)
			{
				foreach (Func<object, PSObject.AdapterSet> func in PSObject._adapterSetMappers)
				{
					adapterSet = func(obj);
					if (adapterSet != null)
					{
						break;
					}
				}
			}
			if (adapterSet == null)
			{
				if (type.IsComObject())
				{
					if (WinRTHelper.IsWinRTType(type))
					{
						adapterSet = PSObject.dotNetInstanceAdapterSet;
					}
					else if (type.FullName.Equals("System.__ComObject"))
					{
						ComTypeInfo dispatchTypeInfo = ComTypeInfo.GetDispatchTypeInfo(obj);
						if (dispatchTypeInfo == null)
						{
							return PSObject.dotNetInstanceAdapterSet;
						}
						return new PSObject.AdapterSet(new ComAdapter(dispatchTypeInfo), PSObject.dotNetInstanceAdapter);
					}
					else
					{
						ComTypeInfo dispatchTypeInfo2 = ComTypeInfo.GetDispatchTypeInfo(obj);
						adapterSet = ((dispatchTypeInfo2 != null) ? new PSObject.AdapterSet(new DotNetAdapterWithComTypeName(dispatchTypeInfo2), null) : PSObject.dotNetInstanceAdapterSet);
					}
				}
				else
				{
					adapterSet = PSObject.dotNetInstanceAdapterSet;
				}
			}
			PSObject._adapterMapping.GetOrAdd(type, adapterSet);
			return adapterSet;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0005AD48 File Offset: 0x00058F48
		internal static PSObject.AdapterSet CreateThirdPartyAdapterSet(Type adaptedType, PSPropertyAdapter adapter)
		{
			return new PSObject.AdapterSet(new ThirdPartyAdapter(adaptedType, adapter), PSObject.baseAdapterForAdaptedObjects);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0005AD5B File Offset: 0x00058F5B
		public PSObject()
		{
			this.CommonInitialization(PSCustomObject.SelfInstance);
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0005AD79 File Offset: 0x00058F79
		public PSObject(object obj)
		{
			if (obj == null)
			{
				throw PSTraceSource.NewArgumentNullException("obj");
			}
			this.CommonInitialization(obj);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0005ADA4 File Offset: 0x00058FA4
		protected PSObject(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			string text = info.GetValue("CliXml", typeof(string)) as string;
			if (text == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			PSObject psobject = PSObject.AsPSObject(PSSerializer.Deserialize(text));
			this.CommonInitialization(psobject.ImmediateBaseObject);
			PSObject.CopyDeserializerFields(psobject, this);
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0005AE18 File Offset: 0x00059018
		internal static PSObject ConstructPSObjectFromSerializationInfo(SerializationInfo info, StreamingContext context)
		{
			return new PSObject(info, context);
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x0005AE21 File Offset: 0x00059021
		// (set) Token: 0x06000FF3 RID: 4083 RVA: 0x0005AE2E File Offset: 0x0005902E
		internal Adapter InternalAdapter
		{
			get
			{
				return this.InternalAdapterSet.OriginalAdapter;
			}
			set
			{
				this.InternalAdapterSet.OriginalAdapter = value;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x0005AE3C File Offset: 0x0005903C
		internal Adapter InternalBaseDotNetAdapter
		{
			get
			{
				return this.InternalAdapterSet.DotNetAdapter;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x0005AE4C File Offset: 0x0005904C
		private PSObject.AdapterSet InternalAdapterSet
		{
			get
			{
				if (this.adapterSet == null)
				{
					lock (this.lockObject)
					{
						if (this.adapterSet == null)
						{
							this.adapterSet = PSObject.GetMappedAdapter(this.immediateBaseObject, this.GetTypeTable());
						}
					}
				}
				return this.adapterSet;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x0005AEBC File Offset: 0x000590BC
		// (set) Token: 0x06000FF7 RID: 4087 RVA: 0x0005AF40 File Offset: 0x00059140
		internal PSMemberInfoInternalCollection<PSMemberInfo> InstanceMembers
		{
			get
			{
				if (this._instanceMembers == null)
				{
					lock (this.lockObject)
					{
						if (this._instanceMembers == null)
						{
							this._instanceMembers = PSObject._instanceMembersResurrectionTable.GetValue(PSObject.GetKeyForResurrectionTables(this), (object _) => new PSMemberInfoInternalCollection<PSMemberInfo>());
						}
					}
				}
				return this._instanceMembers;
			}
			set
			{
				this._instanceMembers = value;
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0005AF4C File Offset: 0x0005914C
		internal static bool HasInstanceMembers(object obj, out PSMemberInfoInternalCollection<PSMemberInfo> instanceMembers)
		{
			PSObject psobject = obj as PSObject;
			if (psobject != null)
			{
				lock (psobject)
				{
					if (psobject._instanceMembers == null)
					{
						PSObject._instanceMembersResurrectionTable.TryGetValue(PSObject.GetKeyForResurrectionTables(psobject), out psobject._instanceMembers);
					}
				}
				instanceMembers = psobject._instanceMembers;
			}
			else if (obj != null)
			{
				PSObject._instanceMembersResurrectionTable.TryGetValue(PSObject.GetKeyForResurrectionTables(obj), out instanceMembers);
			}
			else
			{
				instanceMembers = null;
			}
			return instanceMembers != null && instanceMembers.Count > 0;
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x0005AFE0 File Offset: 0x000591E0
		public PSMemberInfoCollection<PSMemberInfo> Members
		{
			get
			{
				if (this._members == null)
				{
					lock (this.lockObject)
					{
						if (this._members == null)
						{
							this._members = new PSMemberInfoIntegratingCollection<PSMemberInfo>(this, PSObject.memberCollection);
						}
					}
				}
				return this._members;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x0005B044 File Offset: 0x00059244
		public PSMemberInfoCollection<PSPropertyInfo> Properties
		{
			get
			{
				if (this._properties == null)
				{
					lock (this.lockObject)
					{
						if (this._properties == null)
						{
							this._properties = new PSMemberInfoIntegratingCollection<PSPropertyInfo>(this, PSObject.propertyCollection);
						}
					}
				}
				return this._properties;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x0005B0A8 File Offset: 0x000592A8
		public PSMemberInfoCollection<PSMethodInfo> Methods
		{
			get
			{
				if (this._methods == null)
				{
					lock (this.lockObject)
					{
						if (this._methods == null)
						{
							this._methods = new PSMemberInfoIntegratingCollection<PSMethodInfo>(this, PSObject.methodCollection);
						}
					}
				}
				return this._methods;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x0005B10C File Offset: 0x0005930C
		public object ImmediateBaseObject
		{
			get
			{
				return this.immediateBaseObject;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0005B114 File Offset: 0x00059314
		public object BaseObject
		{
			get
			{
				PSObject psobject = this;
				object obj;
				do
				{
					obj = psobject.immediateBaseObject;
					psobject = (obj as PSObject);
				}
				while (psobject != null);
				return obj;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x0005B144 File Offset: 0x00059344
		public Collection<string> TypeNames
		{
			get
			{
				ConsolidatedString internalTypeNames = this.InternalTypeNames;
				if (internalTypeNames.IsReadOnly)
				{
					lock (this.lockObject)
					{
						if (internalTypeNames.IsReadOnly)
						{
							this._typeNames = PSObject._typeNamesResurrectionTable.GetValue(PSObject.GetKeyForResurrectionTables(this), (object _) => new ConsolidatedString(this._typeNames));
							object baseObject = this.BaseObject;
							if (baseObject != null)
							{
								PSVariableAssignmentBinder.NoteTypeHasInstanceMemberOrTypeName(baseObject.GetType());
							}
							return this._typeNames;
						}
					}
					return internalTypeNames;
				}
				return internalTypeNames;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000FFF RID: 4095 RVA: 0x0005B1E4 File Offset: 0x000593E4
		// (set) Token: 0x06001000 RID: 4096 RVA: 0x0005B264 File Offset: 0x00059464
		internal ConsolidatedString InternalTypeNames
		{
			get
			{
				if (this._typeNames == null)
				{
					lock (this.lockObject)
					{
						if (this._typeNames == null && !PSObject._typeNamesResurrectionTable.TryGetValue(PSObject.GetKeyForResurrectionTables(this), out this._typeNames))
						{
							this._typeNames = this.InternalAdapter.BaseGetTypeNameHierarchy(this.immediateBaseObject);
						}
					}
				}
				return this._typeNames;
			}
			set
			{
				this._typeNames = value;
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0005B270 File Offset: 0x00059470
		internal static ConsolidatedString GetTypeNames(object obj)
		{
			PSObject psobject = obj as PSObject;
			if (psobject != null)
			{
				return psobject.InternalTypeNames;
			}
			ConsolidatedString result;
			if (PSObject.HasInstanceTypeName(obj, out result))
			{
				return result;
			}
			return PSObject.GetMappedAdapter(obj, null).OriginalAdapter.BaseGetTypeNameHierarchy(obj);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0005B2AC File Offset: 0x000594AC
		internal static bool HasInstanceTypeName(object obj, out ConsolidatedString result)
		{
			return PSObject._typeNamesResurrectionTable.TryGetValue(PSObject.GetKeyForResurrectionTables(obj), out result);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0005B2BF File Offset: 0x000594BF
		public static implicit operator PSObject(int valueToConvert)
		{
			return PSObject.AsPSObject(valueToConvert);
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0005B2CC File Offset: 0x000594CC
		public static implicit operator PSObject(string valueToConvert)
		{
			return PSObject.AsPSObject(valueToConvert);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0005B2D4 File Offset: 0x000594D4
		public static implicit operator PSObject(Hashtable valueToConvert)
		{
			return PSObject.AsPSObject(valueToConvert);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0005B2DC File Offset: 0x000594DC
		public static implicit operator PSObject(double valueToConvert)
		{
			return PSObject.AsPSObject(valueToConvert);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0005B2E9 File Offset: 0x000594E9
		public static implicit operator PSObject(bool valueToConvert)
		{
			return PSObject.AsPSObject(valueToConvert);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0005B2F8 File Offset: 0x000594F8
		internal static object Base(object obj)
		{
			PSObject psobject = obj as PSObject;
			if (psobject == null)
			{
				return obj;
			}
			if (psobject == AutomationNull.Value)
			{
				return null;
			}
			if (psobject.immediateBaseObjectIsEmpty)
			{
				return obj;
			}
			object obj2;
			do
			{
				obj2 = psobject.immediateBaseObject;
				psobject = (obj2 as PSObject);
			}
			while (psobject != null && !psobject.immediateBaseObjectIsEmpty);
			return obj2;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0005B344 File Offset: 0x00059544
		internal static PSMemberInfo GetStaticCLRMember(object obj, string methodName)
		{
			obj = PSObject.Base(obj);
			if (obj == null || methodName == null || methodName.Length == 0)
			{
				return null;
			}
			Type obj2 = (obj as Type) ?? obj.GetType();
			return PSObject.dotNetStaticAdapter.BaseGetMember<PSMemberInfo>(obj2, methodName);
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0005B386 File Offset: 0x00059586
		public static PSObject AsPSObject(object obj)
		{
			return PSObject.AsPSObject(obj, false);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0005B390 File Offset: 0x00059590
		internal static PSObject AsPSObject(object obj, bool storeTypeNameAndInstanceMembersLocally)
		{
			if (obj == null)
			{
				throw PSTraceSource.NewArgumentNullException("obj");
			}
			PSObject psobject = obj as PSObject;
			if (psobject != null)
			{
				return psobject;
			}
			return new PSObject(obj)
			{
				_storeTypeNameAndInstanceMembersLocally = storeTypeNameAndInstanceMembersLocally
			};
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0005B3C8 File Offset: 0x000595C8
		internal static object GetKeyForResurrectionTables(object obj)
		{
			PSObject psobject = obj as PSObject;
			if (psobject == null)
			{
				return obj;
			}
			PSObject psobject2 = psobject;
			while (psobject2.ImmediateBaseObject is PSObject)
			{
				psobject2 = (PSObject)psobject2.ImmediateBaseObject;
			}
			if (psobject2.ImmediateBaseObject is PSCustomObject || psobject2.ImmediateBaseObject is string || psobject._storeTypeNameAndInstanceMembersLocally)
			{
				return psobject2;
			}
			return psobject2.ImmediateBaseObject;
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0005B428 File Offset: 0x00059628
		private static string GetSeparator(ExecutionContext context, string separator)
		{
			if (separator != null)
			{
				return separator;
			}
			if (context != null)
			{
				object variableValue = context.GetVariableValue(SpecialVariables.OFSVarPath);
				if (variableValue != null)
				{
					return variableValue.ToString();
				}
			}
			return " ";
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0005B458 File Offset: 0x00059658
		internal static string ToStringEnumerator(ExecutionContext context, IEnumerator enumerator, string separator, string format, IFormatProvider formatProvider)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string separator2 = PSObject.GetSeparator(context, separator);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				stringBuilder.Append(PSObject.ToString(context, obj, separator, format, formatProvider, false, false));
				stringBuilder.Append(separator2);
			}
			if (stringBuilder.Length == 0)
			{
				return string.Empty;
			}
			int length = separator2.Length;
			stringBuilder.Remove(stringBuilder.Length - length, length);
			return stringBuilder.ToString();
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0005B4CC File Offset: 0x000596CC
		internal static string ToStringEnumerable(ExecutionContext context, IEnumerable enumerable, string separator, string format, IFormatProvider formatProvider)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string separator2 = PSObject.GetSeparator(context, separator);
			foreach (object obj in enumerable)
			{
				if (obj != null)
				{
					PSObject obj2 = PSObject.AsPSObject(obj);
					stringBuilder.Append(PSObject.ToString(context, obj2, separator, format, formatProvider, false, false));
				}
				stringBuilder.Append(separator2);
			}
			if (stringBuilder.Length == 0)
			{
				return string.Empty;
			}
			int length = separator2.Length;
			stringBuilder.Remove(stringBuilder.Length - length, length);
			return stringBuilder.ToString();
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0005B580 File Offset: 0x00059780
		private static string ToStringEmptyBaseObject(ExecutionContext context, PSObject mshObj, string separator, string format, IFormatProvider formatProvider)
		{
			StringBuilder stringBuilder = new StringBuilder("@{");
			bool flag = true;
			foreach (PSPropertyInfo pspropertyInfo in mshObj.Properties)
			{
				if (!flag)
				{
					stringBuilder.Append("; ");
				}
				flag = false;
				stringBuilder.Append(pspropertyInfo.Name);
				stringBuilder.Append("=");
				object obj;
				if (pspropertyInfo is PSScriptProperty)
				{
					obj = pspropertyInfo.GetType().FullName;
				}
				else
				{
					obj = pspropertyInfo.Value;
				}
				stringBuilder.Append(PSObject.ToString(context, obj, separator, format, formatProvider, false, false));
			}
			if (flag)
			{
				return string.Empty;
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0005B650 File Offset: 0x00059850
		internal static string ToStringParser(ExecutionContext context, object obj)
		{
			string result;
			try
			{
				result = PSObject.ToString(context, obj, null, null, CultureInfo.InvariantCulture, true, true);
			}
			catch (ExtendedTypeSystemException ex)
			{
				throw new PSInvalidCastException("InvalidCastFromAnyTypeToString", ex.InnerException, ExtendedTypeSystem.InvalidCastCannotRetrieveString, new object[0]);
			}
			return result;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0005B6A0 File Offset: 0x000598A0
		internal static string ToString(ExecutionContext context, object obj, string separator, string format, IFormatProvider formatProvider, bool recurse, bool unravelEnumeratorOnRecurse)
		{
			PSObject psobject = obj as PSObject;
			if (psobject == null)
			{
				if (obj == null)
				{
					return string.Empty;
				}
				Type type = obj.GetType();
				switch (type.GetTypeCode())
				{
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
					return obj.ToString();
				case TypeCode.Single:
					return ((float)obj).ToString(formatProvider);
				case TypeCode.Double:
					return ((double)obj).ToString(formatProvider);
				case TypeCode.Decimal:
					return ((decimal)obj).ToString(formatProvider);
				case TypeCode.DateTime:
					return ((DateTime)obj).ToString(formatProvider);
				case TypeCode.String:
					return (string)obj;
				}
				if (recurse)
				{
					IEnumerable enumerable = LanguagePrimitives.GetEnumerable(obj);
					if (enumerable != null)
					{
						try
						{
							return PSObject.ToStringEnumerable(context, enumerable, separator, format, formatProvider);
						}
						catch (Exception e)
						{
							CommandProcessorBase.CheckForSevereException(e);
						}
					}
					if (unravelEnumeratorOnRecurse)
					{
						IEnumerator enumerator = LanguagePrimitives.GetEnumerator(obj);
						if (enumerator != null)
						{
							try
							{
								return PSObject.ToStringEnumerator(context, enumerator, separator, format, formatProvider);
							}
							catch (Exception e2)
							{
								CommandProcessorBase.CheckForSevereException(e2);
							}
						}
					}
				}
				IFormattable formattable = obj as IFormattable;
				try
				{
					if (formattable != null)
					{
						return formattable.ToString(format, formatProvider);
					}
					Type type2 = obj as Type;
					if (type2 != null)
					{
						return ToStringCodeMethods.Type(type2, false);
					}
					return obj.ToString();
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					throw new ExtendedTypeSystemException("ToStringObjectBasicException", ex, ExtendedTypeSystem.ToStringException, new object[]
					{
						ex.Message
					});
				}
			}
			PSMethodInfo psmethodInfo = null;
			PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection;
			if (PSObject.HasInstanceMembers(psobject, out psmemberInfoInternalCollection))
			{
				psmethodInfo = (psmemberInfoInternalCollection["ToString"] as PSMethodInfo);
			}
			if (psmethodInfo == null && psobject.InternalTypeNames.Count != 0)
			{
				TypeTable typeTable = psobject.GetTypeTable();
				if (typeTable != null)
				{
					psmethodInfo = typeTable.GetMembers<PSMethodInfo>(psobject.InternalTypeNames)["ToString"];
					if (psmethodInfo != null)
					{
						psmethodInfo = (PSMethodInfo)psmethodInfo.Copy();
						psmethodInfo.instance = psobject;
					}
				}
			}
			if (psmethodInfo != null)
			{
				try
				{
					object obj2;
					if (formatProvider != null && psmethodInfo.OverloadDefinitions.Count > 1)
					{
						obj2 = psmethodInfo.Invoke(new object[]
						{
							format,
							formatProvider
						});
						return (obj2 != null) ? obj2.ToString() : string.Empty;
					}
					obj2 = psmethodInfo.Invoke(new object[0]);
					return (obj2 != null) ? obj2.ToString() : string.Empty;
				}
				catch (MethodException ex2)
				{
					throw new ExtendedTypeSystemException("MethodExceptionNullFormatProvider", ex2, ExtendedTypeSystem.ToStringException, new object[]
					{
						ex2.Message
					});
				}
			}
			if (recurse)
			{
				if (psobject.immediateBaseObjectIsEmpty)
				{
					try
					{
						return PSObject.ToStringEmptyBaseObject(context, psobject, separator, format, formatProvider);
					}
					catch (Exception e3)
					{
						CommandProcessorBase.CheckForSevereException(e3);
					}
				}
				IEnumerable enumerable2 = LanguagePrimitives.GetEnumerable(psobject);
				if (enumerable2 != null)
				{
					try
					{
						return PSObject.ToStringEnumerable(context, enumerable2, separator, format, formatProvider);
					}
					catch (Exception e4)
					{
						CommandProcessorBase.CheckForSevereException(e4);
					}
				}
				if (unravelEnumeratorOnRecurse)
				{
					IEnumerator enumerator2 = LanguagePrimitives.GetEnumerator(psobject);
					if (enumerator2 != null)
					{
						try
						{
							return PSObject.ToStringEnumerator(context, enumerator2, separator, format, formatProvider);
						}
						catch (Exception e5)
						{
							CommandProcessorBase.CheckForSevereException(e5);
						}
					}
				}
			}
			if (psobject.TokenText != null)
			{
				return psobject.TokenText;
			}
			object obj3 = psobject.immediateBaseObject;
			IFormattable formattable2 = obj3 as IFormattable;
			string result;
			try
			{
				string text;
				if (formattable2 == null)
				{
					text = obj3.ToString();
				}
				else
				{
					text = formattable2.ToString(format, formatProvider);
				}
				if (text == null)
				{
					text = string.Empty;
				}
				result = text;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				throw new ExtendedTypeSystemException("ToStringPSObjectBasicException", ex3, ExtendedTypeSystem.ToStringException, new object[]
				{
					ex3.Message
				});
			}
			return result;
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0005BAB8 File Offset: 0x00059CB8
		public override string ToString()
		{
			if (this.toStringFromDeserialization != null)
			{
				return this.toStringFromDeserialization;
			}
			return PSObject.ToString(null, this, null, null, null, true, false);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0005BAD5 File Offset: 0x00059CD5
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (this.toStringFromDeserialization != null)
			{
				return this.toStringFromDeserialization;
			}
			return PSObject.ToString(null, this, null, format, formatProvider, true, false);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0005BAF4 File Offset: 0x00059CF4
		private string PrivateToString()
		{
			string result;
			try
			{
				result = this.ToString();
			}
			catch (ExtendedTypeSystemException)
			{
				result = this.BaseObject.GetType().FullName;
			}
			return result;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0005BB30 File Offset: 0x00059D30
		public virtual PSObject Copy()
		{
			PSObject psobject = (PSObject)base.MemberwiseClone();
			if (this.BaseObject is PSCustomObject)
			{
				psobject.immediateBaseObject = PSCustomObject.SelfInstance;
				psobject.immediateBaseObjectIsEmpty = true;
			}
			else
			{
				psobject.immediateBaseObject = this.immediateBaseObject;
				psobject.immediateBaseObjectIsEmpty = false;
			}
			psobject._instanceMembers = null;
			psobject._typeNames = null;
			psobject._members = new PSMemberInfoIntegratingCollection<PSMemberInfo>(psobject, PSObject.memberCollection);
			psobject._properties = new PSMemberInfoIntegratingCollection<PSPropertyInfo>(psobject, PSObject.propertyCollection);
			psobject._methods = new PSMemberInfoIntegratingCollection<PSMethodInfo>(psobject, PSObject.methodCollection);
			psobject.adapterSet = PSObject.GetMappedAdapter(psobject.immediateBaseObject, psobject.GetTypeTable());
			ICloneable cloneable = psobject.immediateBaseObject as ICloneable;
			if (cloneable != null)
			{
				psobject.immediateBaseObject = cloneable.Clone();
			}
			if (psobject.immediateBaseObject is ValueType)
			{
				psobject.immediateBaseObject = PSObject.CopyValueType(psobject.immediateBaseObject);
			}
			bool flag = !object.ReferenceEquals(PSObject.GetKeyForResurrectionTables(this), PSObject.GetKeyForResurrectionTables(psobject));
			if (flag)
			{
				foreach (PSMemberInfo psmemberInfo in this.InstanceMembers)
				{
					if (!psmemberInfo.IsHidden)
					{
						psobject.Members.Add(psmemberInfo);
					}
				}
				psobject.TypeNames.Clear();
				foreach (string item in this.InternalTypeNames)
				{
					psobject.TypeNames.Add(item);
				}
			}
			psobject.hasGeneratedReservedMembers = false;
			return psobject;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0005BCD8 File Offset: 0x00059ED8
		internal static object CopyValueType(object obj)
		{
			Array array = Array.CreateInstance(obj.GetType(), 1);
			array.SetValue(obj, 0);
			return array.GetValue(0);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0005BD04 File Offset: 0x00059F04
		public int CompareTo(object obj)
		{
			if (object.ReferenceEquals(this, obj))
			{
				return 0;
			}
			int result;
			try
			{
				result = LanguagePrimitives.Compare(this.BaseObject, obj);
			}
			catch (ArgumentException innerException)
			{
				throw new ExtendedTypeSystemException("PSObjectCompareTo", innerException, ExtendedTypeSystem.NotTheSameTypeOrNotIcomparable, new object[]
				{
					this.PrivateToString(),
					PSObject.AsPSObject(obj).ToString(),
					"IComparable"
				});
			}
			return result;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0005BD78 File Offset: 0x00059F78
		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj) || (!object.ReferenceEquals(this.BaseObject, PSCustomObject.SelfInstance) && LanguagePrimitives.Equals(this.BaseObject, obj));
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0005BDA5 File Offset: 0x00059FA5
		public override int GetHashCode()
		{
			return this.BaseObject.GetHashCode();
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0005BDB4 File Offset: 0x00059FB4
		internal void AddOrSetProperty(string memberName, object value)
		{
			PSMemberInfo psmemberInfo;
			if (PSGetMemberBinder.TryGetInstanceMember(this, memberName, out psmemberInfo) && psmemberInfo is PSPropertyInfo)
			{
				psmemberInfo.Value = value;
				return;
			}
			this.Properties.Add(new PSNoteProperty(memberName, value));
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0005BDF0 File Offset: 0x00059FF0
		internal void AddOrSetProperty(PSNoteProperty property)
		{
			PSMemberInfo psmemberInfo;
			if (PSGetMemberBinder.TryGetInstanceMember(this, property.Name, out psmemberInfo) && psmemberInfo is PSPropertyInfo)
			{
				psmemberInfo.Value = property.Value;
				return;
			}
			this.Properties.Add(property);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0005BE30 File Offset: 0x0005A030
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw PSTraceSource.NewArgumentNullException("info");
			}
			string value;
			if (this.immediateBaseObjectIsEmpty)
			{
				PSObject source = new PSObject(this);
				value = PSSerializer.Serialize(source);
			}
			else
			{
				value = PSSerializer.Serialize(this);
			}
			info.AddValue("CliXml", value);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0005BE78 File Offset: 0x0005A078
		internal int GetReferenceHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0005BE80 File Offset: 0x0005A080
		internal static object GetNoteSettingValue(PSMemberSet settings, string noteName, object defaultValue, Type expectedType, bool shouldReplicateInstance, PSObject ownerObject)
		{
			if (settings == null)
			{
				return defaultValue;
			}
			if (shouldReplicateInstance)
			{
				settings.ReplicateInstance(ownerObject);
			}
			PSNoteProperty psnoteProperty = settings.Members[noteName] as PSNoteProperty;
			if (psnoteProperty == null)
			{
				return defaultValue;
			}
			object value = psnoteProperty.Value;
			if (value == null || value.GetType() != expectedType)
			{
				return defaultValue;
			}
			return psnoteProperty.Value;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0005BED8 File Offset: 0x0005A0D8
		internal int GetSerializationDepth(TypeTable backupTypeTable)
		{
			int result = 0;
			TypeTable typeTable = backupTypeTable ?? this.GetTypeTable();
			if (typeTable != null)
			{
				PSMemberSet settings = PSObject.TypeTableGetMemberDelegate<PSMemberSet>(this, typeTable, "PSStandardMembers");
				result = (int)PSObject.GetNoteSettingValue(settings, "SerializationDepth", 0, typeof(int), true, this);
			}
			return result;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0005BF28 File Offset: 0x0005A128
		internal PSPropertyInfo GetStringSerializationSource(TypeTable backupTypeTable)
		{
			PSMemberInfo psstandardMember = this.GetPSStandardMember(backupTypeTable, "StringSerializationSource");
			return psstandardMember as PSPropertyInfo;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0005BF48 File Offset: 0x0005A148
		internal SerializationMethod GetSerializationMethod(TypeTable backupTypeTable)
		{
			SerializationMethod result = SerializationMethod.AllPublicProperties;
			TypeTable typeTable = (backupTypeTable != null) ? backupTypeTable : this.GetTypeTable();
			if (typeTable != null)
			{
				PSMemberSet settings = PSObject.TypeTableGetMemberDelegate<PSMemberSet>(this, typeTable, "PSStandardMembers");
				result = (SerializationMethod)PSObject.GetNoteSettingValue(settings, "SerializationMethod", SerializationMethod.AllPublicProperties, typeof(SerializationMethod), true, this);
			}
			return result;
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x0005BF98 File Offset: 0x0005A198
		internal PSMemberSet PSStandardMembers
		{
			get
			{
				PSMemberSet psmemberSet = PSObject.TypeTableGetMemberDelegate<PSMemberSet>(this, "PSStandardMembers");
				if (psmemberSet != null)
				{
					psmemberSet = (PSMemberSet)psmemberSet.Copy();
					psmemberSet.ReplicateInstance(this);
					return psmemberSet;
				}
				return this.InstanceMembers["PSStandardMembers"] as PSMemberSet;
			}
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0005BFE4 File Offset: 0x0005A1E4
		internal PSMemberInfo GetPSStandardMember(TypeTable backupTypeTable, string memberName)
		{
			PSMemberInfo psmemberInfo = null;
			TypeTable typeTable = (backupTypeTable != null) ? backupTypeTable : this.GetTypeTable();
			if (typeTable != null)
			{
				PSMemberSet psmemberSet = PSObject.TypeTableGetMemberDelegate<PSMemberSet>(this, typeTable, "PSStandardMembers");
				if (psmemberSet != null)
				{
					psmemberSet.ReplicateInstance(this);
					PSMemberInfoIntegratingCollection<PSMemberInfo> psmemberInfoIntegratingCollection = new PSMemberInfoIntegratingCollection<PSMemberInfo>(psmemberSet, PSObject.GetMemberCollection(PSMemberViewTypes.All, backupTypeTable));
					psmemberInfo = psmemberInfoIntegratingCollection[memberName];
				}
			}
			if (psmemberInfo == null)
			{
				psmemberInfo = (this.InstanceMembers["PSStandardMembers"] as PSMemberSet);
			}
			return psmemberInfo;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0005C04C File Offset: 0x0005A24C
		internal Type GetTargetTypeForDeserialization(TypeTable backupTypeTable)
		{
			PSMemberInfo psstandardMember = this.GetPSStandardMember(backupTypeTable, "TargetTypeForDeserialization");
			if (psstandardMember != null)
			{
				return psstandardMember.Value as Type;
			}
			return null;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0005C078 File Offset: 0x0005A278
		internal Collection<string> GetSpecificPropertiesToSerialize(TypeTable backupTypeTable)
		{
			TypeTable typeTable = (backupTypeTable != null) ? backupTypeTable : this.GetTypeTable();
			if (typeTable != null)
			{
				return typeTable.GetSpecificProperties(this.InternalTypeNames);
			}
			return new Collection<string>(new List<string>());
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0005C0AE File Offset: 0x0005A2AE
		internal bool ShouldSerializeAdapter()
		{
			if (this.isDeserialized)
			{
				return this.adaptedMembers != null;
			}
			return !this.immediateBaseObjectIsEmpty;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0005C0CE File Offset: 0x0005A2CE
		internal bool ShouldSerializeBase()
		{
			if (this.isDeserialized)
			{
				return this.adaptedMembers != this.clrMembers;
			}
			return !this.immediateBaseObjectIsEmpty && this.InternalAdapter.GetType() != typeof(DotNetAdapter);
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0005C10E File Offset: 0x0005A30E
		internal PSMemberInfoInternalCollection<PSPropertyInfo> GetAdaptedProperties()
		{
			return this.GetProperties(this.adaptedMembers, this.InternalAdapter);
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0005C122 File Offset: 0x0005A322
		internal PSMemberInfoInternalCollection<PSPropertyInfo> GetBaseProperties()
		{
			return this.GetProperties(this.clrMembers, PSObject.dotNetInstanceAdapter);
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0005C138 File Offset: 0x0005A338
		private PSMemberInfoInternalCollection<PSPropertyInfo> GetProperties(PSMemberInfoInternalCollection<PSPropertyInfo> serializedMembers, Adapter particularAdapter)
		{
			if (this.isDeserialized)
			{
				return serializedMembers;
			}
			PSMemberInfoInternalCollection<PSPropertyInfo> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<PSPropertyInfo>();
			foreach (PSPropertyInfo member in particularAdapter.BaseGetMembers<PSPropertyInfo>(this.immediateBaseObject))
			{
				psmemberInfoInternalCollection.Add(member);
			}
			return psmemberInfoInternalCollection;
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0005C19C File Offset: 0x0005A39C
		internal static void CopyDeserializerFields(PSObject source, PSObject target)
		{
			if (!target.isDeserialized)
			{
				target.isDeserialized = source.isDeserialized;
				target.adaptedMembers = source.adaptedMembers;
				target.clrMembers = source.clrMembers;
			}
			if (target.toStringFromDeserialization == null)
			{
				target.toStringFromDeserialization = source.toStringFromDeserialization;
				target.TokenText = source.TokenText;
			}
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0005C1F5 File Offset: 0x0005A3F5
		internal void SetCoreOnDeserialization(object value, bool overrideTypeInfo)
		{
			this.immediateBaseObjectIsEmpty = false;
			this.immediateBaseObject = value;
			this.adapterSet = PSObject.GetMappedAdapter(this.immediateBaseObject, this.GetTypeTable());
			if (overrideTypeInfo)
			{
				this.InternalTypeNames = this.InternalAdapter.BaseGetTypeNameHierarchy(value);
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x0005C231 File Offset: 0x0005A431
		internal bool PreserveToString
		{
			get
			{
				if (this.preserveToStringSet)
				{
					return this.preserveToString;
				}
				this.preserveToStringSet = true;
				if (this.InternalTypeNames.Count == 0)
				{
					return false;
				}
				this.preserveToString = false;
				return this.preserveToString;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x0005C265 File Offset: 0x0005A465
		// (set) Token: 0x06001030 RID: 4144 RVA: 0x0005C26D File Offset: 0x0005A46D
		internal string ToStringFromDeserialization
		{
			get
			{
				return this.toStringFromDeserialization;
			}
			set
			{
				this.toStringFromDeserialization = value;
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0005C276 File Offset: 0x0005A476
		DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
		{
			return new PSObject.PSDynamicMetaObject(parameter, this);
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x0005C27F File Offset: 0x0005A47F
		// (set) Token: 0x06001033 RID: 4147 RVA: 0x0005C287 File Offset: 0x0005A487
		internal bool IsHelpObject
		{
			get
			{
				return this.isHelpObject;
			}
			set
			{
				this.isHelpObject = value;
			}
		}

		// Token: 0x040006DB RID: 1755
		public const string AdaptedMemberSetName = "psadapted";

		// Token: 0x040006DC RID: 1756
		public const string ExtendedMemberSetName = "psextended";

		// Token: 0x040006DD RID: 1757
		public const string BaseObjectMemberSetName = "psbase";

		// Token: 0x040006DE RID: 1758
		internal const string PSObjectMemberSetName = "psobject";

		// Token: 0x040006DF RID: 1759
		internal const string PSTypeNames = "pstypenames";

		// Token: 0x040006E0 RID: 1760
		private static Collection<CollectionEntry<PSMemberInfo>> memberCollection = PSObject.GetMemberCollection(PSMemberViewTypes.All);

		// Token: 0x040006E1 RID: 1761
		private static Collection<CollectionEntry<PSMethodInfo>> methodCollection = PSObject.GetMethodCollection();

		// Token: 0x040006E2 RID: 1762
		private static Collection<CollectionEntry<PSPropertyInfo>> propertyCollection = PSObject.GetPropertyCollection(PSMemberViewTypes.All);

		// Token: 0x040006E3 RID: 1763
		internal static readonly DotNetAdapter dotNetInstanceAdapter = new DotNetAdapter();

		// Token: 0x040006E4 RID: 1764
		private static readonly DotNetAdapter baseAdapterForAdaptedObjects = new BaseDotNetAdapterForAdaptedObjects();

		// Token: 0x040006E5 RID: 1765
		internal static readonly DotNetAdapter dotNetStaticAdapter = new DotNetAdapter(true);

		// Token: 0x040006E6 RID: 1766
		private static readonly PSObject.AdapterSet dotNetInstanceAdapterSet = new PSObject.AdapterSet(PSObject.dotNetInstanceAdapter, null);

		// Token: 0x040006E7 RID: 1767
		private static readonly PSObject.AdapterSet mshMemberSetAdapter = new PSObject.AdapterSet(new PSMemberSetAdapter(), null);

		// Token: 0x040006E8 RID: 1768
		private static readonly PSObject.AdapterSet mshObjectAdapter = new PSObject.AdapterSet(new PSObjectAdapter(), null);

		// Token: 0x040006E9 RID: 1769
		private static PSObject.AdapterSet cimInstanceAdapter = new PSObject.AdapterSet(new ThirdPartyAdapter(typeof(CimInstance), new CimInstanceAdapter()), PSObject.dotNetInstanceAdapter);

		// Token: 0x040006EA RID: 1770
		private static readonly PSObject.AdapterSet managementObjectAdapter = new PSObject.AdapterSet(new ManagementObjectAdapter(), PSObject.dotNetInstanceAdapter);

		// Token: 0x040006EB RID: 1771
		private static readonly PSObject.AdapterSet managementClassAdapter = new PSObject.AdapterSet(new ManagementClassApdapter(), PSObject.dotNetInstanceAdapter);

		// Token: 0x040006EC RID: 1772
		private static readonly PSObject.AdapterSet directoryEntryAdapter = new PSObject.AdapterSet(new DirectoryEntryAdapter(), PSObject.dotNetInstanceAdapter);

		// Token: 0x040006ED RID: 1773
		private static readonly PSObject.AdapterSet dataRowViewAdapter = new PSObject.AdapterSet(new DataRowViewAdapter(), PSObject.baseAdapterForAdaptedObjects);

		// Token: 0x040006EE RID: 1774
		private static readonly PSObject.AdapterSet dataRowAdapter = new PSObject.AdapterSet(new DataRowAdapter(), PSObject.baseAdapterForAdaptedObjects);

		// Token: 0x040006EF RID: 1775
		private static readonly PSObject.AdapterSet xmlNodeAdapter = new PSObject.AdapterSet(new XmlNodeAdapter(), PSObject.baseAdapterForAdaptedObjects);

		// Token: 0x040006F0 RID: 1776
		private static readonly ConcurrentDictionary<Type, PSObject.AdapterSet> _adapterMapping = new ConcurrentDictionary<Type, PSObject.AdapterSet>();

		// Token: 0x040006F1 RID: 1777
		private static readonly List<Func<object, PSObject.AdapterSet>> _adapterSetMappers = new List<Func<object, PSObject.AdapterSet>>
		{
			new Func<object, PSObject.AdapterSet>(PSObject.MappedInternalAdapterSet)
		};

		// Token: 0x040006F2 RID: 1778
		private object lockObject = new object();

		// Token: 0x040006F3 RID: 1779
		internal string TokenText;

		// Token: 0x040006F4 RID: 1780
		private object immediateBaseObject;

		// Token: 0x040006F5 RID: 1781
		private WeakReference _typeTable;

		// Token: 0x040006F6 RID: 1782
		private PSObject.AdapterSet adapterSet;

		// Token: 0x040006F7 RID: 1783
		internal bool hasGeneratedReservedMembers;

		// Token: 0x040006F8 RID: 1784
		private PSMemberInfoInternalCollection<PSMemberInfo> _instanceMembers;

		// Token: 0x040006F9 RID: 1785
		private static readonly ConditionalWeakTable<object, PSMemberInfoInternalCollection<PSMemberInfo>> _instanceMembersResurrectionTable = new ConditionalWeakTable<object, PSMemberInfoInternalCollection<PSMemberInfo>>();

		// Token: 0x040006FA RID: 1786
		private bool _storeTypeNameAndInstanceMembersLocally;

		// Token: 0x040006FB RID: 1787
		internal PSMemberInfoInternalCollection<PSPropertyInfo> adaptedMembers;

		// Token: 0x040006FC RID: 1788
		internal PSMemberInfoInternalCollection<PSPropertyInfo> clrMembers;

		// Token: 0x040006FD RID: 1789
		internal bool immediateBaseObjectIsEmpty;

		// Token: 0x040006FE RID: 1790
		internal static PSTraceSource memberResolution = PSTraceSource.GetTracer("MemberResolution", "Traces the resolution from member name to the member. A member can be a property, method, etc.", false);

		// Token: 0x040006FF RID: 1791
		private PSMemberInfoIntegratingCollection<PSMemberInfo> _members;

		// Token: 0x04000700 RID: 1792
		private PSMemberInfoIntegratingCollection<PSPropertyInfo> _properties;

		// Token: 0x04000701 RID: 1793
		private PSMemberInfoIntegratingCollection<PSMethodInfo> _methods;

		// Token: 0x04000702 RID: 1794
		private ConsolidatedString _typeNames;

		// Token: 0x04000703 RID: 1795
		private static readonly ConditionalWeakTable<object, ConsolidatedString> _typeNamesResurrectionTable = new ConditionalWeakTable<object, ConsolidatedString>();

		// Token: 0x04000704 RID: 1796
		internal bool isDeserialized;

		// Token: 0x04000705 RID: 1797
		private string toStringFromDeserialization;

		// Token: 0x04000706 RID: 1798
		internal bool preserveToString;

		// Token: 0x04000707 RID: 1799
		internal bool preserveToStringSet;

		// Token: 0x04000708 RID: 1800
		private bool isHelpObject;

		// Token: 0x0200012F RID: 303
		internal class AdapterSet
		{
			// Token: 0x170003E6 RID: 998
			// (get) Token: 0x06001037 RID: 4151 RVA: 0x0005C3F8 File Offset: 0x0005A5F8
			// (set) Token: 0x06001038 RID: 4152 RVA: 0x0005C400 File Offset: 0x0005A600
			internal Adapter OriginalAdapter
			{
				get
				{
					return this.originalAdapter;
				}
				set
				{
					this.originalAdapter = value;
				}
			}

			// Token: 0x170003E7 RID: 999
			// (get) Token: 0x06001039 RID: 4153 RVA: 0x0005C409 File Offset: 0x0005A609
			internal DotNetAdapter DotNetAdapter
			{
				get
				{
					return this.ultimatedotNetAdapter;
				}
			}

			// Token: 0x0600103A RID: 4154 RVA: 0x0005C411 File Offset: 0x0005A611
			internal AdapterSet(Adapter adapter, DotNetAdapter dotnetAdapter)
			{
				this.originalAdapter = adapter;
				this.ultimatedotNetAdapter = dotnetAdapter;
			}

			// Token: 0x0400070A RID: 1802
			private Adapter originalAdapter;

			// Token: 0x0400070B RID: 1803
			private DotNetAdapter ultimatedotNetAdapter;
		}

		// Token: 0x02000130 RID: 304
		internal class PSDynamicMetaObject : DynamicMetaObject
		{
			// Token: 0x0600103B RID: 4155 RVA: 0x0005C427 File Offset: 0x0005A627
			internal PSDynamicMetaObject(Expression expression, PSObject value) : base(expression, BindingRestrictions.Empty, value)
			{
			}

			// Token: 0x170003E8 RID: 1000
			// (get) Token: 0x0600103C RID: 4156 RVA: 0x0005C436 File Offset: 0x0005A636
			private new PSObject Value
			{
				get
				{
					return (PSObject)base.Value;
				}
			}

			// Token: 0x0600103D RID: 4157 RVA: 0x0005C443 File Offset: 0x0005A643
			private DynamicMetaObject GetUnwrappedObject()
			{
				return new DynamicMetaObject(Expression.Call(CachedReflectionInfo.PSObject_Base, base.Expression), base.Restrictions, PSObject.Base(this.Value));
			}

			// Token: 0x0600103E RID: 4158 RVA: 0x0005C473 File Offset: 0x0005A673
			public override IEnumerable<string> GetDynamicMemberNames()
			{
				return from member in this.Value.Members
				select member.Name;
			}

			// Token: 0x0600103F RID: 4159 RVA: 0x0005C4A4 File Offset: 0x0005A6A4
			private bool MustDeferIDMOP()
			{
				object obj = PSObject.Base(this.Value);
				return obj is IDynamicMetaObjectProvider && !(obj is PSObject);
			}

			// Token: 0x06001040 RID: 4160 RVA: 0x0005C4D4 File Offset: 0x0005A6D4
			private DynamicMetaObject DeferForIDMOP(DynamicMetaObjectBinder binder, params DynamicMetaObject[] args)
			{
				Expression[] array = new Expression[args.Length + 1];
				BindingRestrictions bindingRestrictions = (base.Restrictions == BindingRestrictions.Empty) ? this.PSGetTypeRestriction() : base.Restrictions;
				array[0] = Expression.Call(CachedReflectionInfo.PSObject_Base, base.Expression.Cast(typeof(object)));
				for (int i = 0; i < args.Length; i++)
				{
					array[i + 1] = args[i].Expression;
					bindingRestrictions = bindingRestrictions.Merge((args[i].Restrictions == BindingRestrictions.Empty) ? args[i].PSGetTypeRestriction() : args[i].Restrictions);
				}
				return new DynamicMetaObject(DynamicExpression.Dynamic(binder, binder.ReturnType, array), bindingRestrictions);
			}

			// Token: 0x06001041 RID: 4161 RVA: 0x0005C580 File Offset: 0x0005A780
			public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, new DynamicMetaObject[]
					{
						arg
					});
				}
				return binder.FallbackBinaryOperation(this.GetUnwrappedObject(), arg);
			}

			// Token: 0x06001042 RID: 4162 RVA: 0x0005C5B6 File Offset: 0x0005A7B6
			public override DynamicMetaObject BindConvert(ConvertBinder binder)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, new DynamicMetaObject[0]);
				}
				return binder.FallbackConvert(this);
			}

			// Token: 0x06001043 RID: 4163 RVA: 0x0005C5D5 File Offset: 0x0005A7D5
			public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, indexes);
				}
				return binder.FallbackDeleteIndex(this.GetUnwrappedObject(), indexes);
			}

			// Token: 0x06001044 RID: 4164 RVA: 0x0005C5F5 File Offset: 0x0005A7F5
			public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, new DynamicMetaObject[0]);
				}
				return binder.FallbackDeleteMember(this.GetUnwrappedObject());
			}

			// Token: 0x06001045 RID: 4165 RVA: 0x0005C619 File Offset: 0x0005A819
			public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, indexes);
				}
				return binder.FallbackGetIndex(this.GetUnwrappedObject(), indexes);
			}

			// Token: 0x06001046 RID: 4166 RVA: 0x0005C639 File Offset: 0x0005A839
			public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, args);
				}
				return binder.FallbackInvoke(this.GetUnwrappedObject(), args);
			}

			// Token: 0x06001047 RID: 4167 RVA: 0x0005C659 File Offset: 0x0005A859
			public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, indexes.Append(value).ToArray<DynamicMetaObject>());
				}
				return binder.FallbackSetIndex(this.GetUnwrappedObject(), indexes, value);
			}

			// Token: 0x06001048 RID: 4168 RVA: 0x0005C685 File Offset: 0x0005A885
			public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, new DynamicMetaObject[0]);
				}
				return binder.FallbackUnaryOperation(this.GetUnwrappedObject());
			}

			// Token: 0x06001049 RID: 4169 RVA: 0x0005C6AC File Offset: 0x0005A8AC
			public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, args);
				}
				PSInvokeMemberBinder psinvokeMemberBinder;
				if ((psinvokeMemberBinder = (binder as PSInvokeMemberBinder)) == null)
				{
					psinvokeMemberBinder = ((binder as PSInvokeBaseCtorBinder) ?? PSInvokeMemberBinder.Get(binder.Name, binder.CallInfo, false, false, null, null));
				}
				return psinvokeMemberBinder.FallbackInvokeMember(this, args);
			}

			// Token: 0x0600104A RID: 4170 RVA: 0x0005C6FA File Offset: 0x0005A8FA
			public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, new DynamicMetaObject[0]);
				}
				return ((binder as PSGetMemberBinder) ?? PSGetMemberBinder.Get(binder.Name, null, false)).FallbackGetMember(this);
			}

			// Token: 0x0600104B RID: 4171 RVA: 0x0005C730 File Offset: 0x0005A930
			public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
			{
				if (this.MustDeferIDMOP())
				{
					return this.DeferForIDMOP(binder, new DynamicMetaObject[]
					{
						value
					});
				}
				return ((binder as PSSetMemberBinder) ?? PSSetMemberBinder.Get(binder.Name, null, false)).FallbackSetMember(this, value);
			}
		}
	}
}
