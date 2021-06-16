using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Threading;
using Microsoft.Management.Infrastructure;

namespace System.Management.Automation
{
	// Token: 0x02000A42 RID: 2626
	internal class InternalMISerializer
	{
		// Token: 0x06006961 RID: 26977 RVA: 0x00213244 File Offset: 0x00211444
		internal InternalMISerializer(int depth)
		{
			this._depth = depth;
			this._typeTable = new TypeTable();
		}

		// Token: 0x17001D8C RID: 7564
		// (get) Token: 0x06006962 RID: 26978 RVA: 0x0021325E File Offset: 0x0021145E
		// (set) Token: 0x06006963 RID: 26979 RVA: 0x00213266 File Offset: 0x00211466
		internal CimInstance CimInstance
		{
			get
			{
				return this._cimInstance;
			}
			set
			{
				this._cimInstance = value;
			}
		}

		// Token: 0x17001D8D RID: 7565
		// (get) Token: 0x06006964 RID: 26980 RVA: 0x0021326F File Offset: 0x0021146F
		private Collection<CollectionEntry<PSPropertyInfo>> AllPropertiesCollection
		{
			get
			{
				if (this.allPropertiesCollection == null)
				{
					this.allPropertiesCollection = PSObject.GetPropertyCollection(PSMemberViewTypes.All, this._typeTable);
				}
				return this.allPropertiesCollection;
			}
		}

		// Token: 0x17001D8E RID: 7566
		// (get) Token: 0x06006965 RID: 26981 RVA: 0x00213294 File Offset: 0x00211494
		private bool CanUseDefaultRunspaceInThreadSafeManner
		{
			get
			{
				if (this.canUseDefaultRunspaceInThreadSafeManner == null)
				{
					this.canUseDefaultRunspaceInThreadSafeManner = new bool?(false);
					RunspaceBase runspaceBase = Runspace.DefaultRunspace as RunspaceBase;
					if (runspaceBase != null)
					{
						Pipeline currentlyRunningPipeline = runspaceBase.GetCurrentlyRunningPipeline();
						LocalPipeline localPipeline = currentlyRunningPipeline as LocalPipeline;
						if (localPipeline != null && localPipeline.NestedPipelineExecutionThread != null)
						{
							this.canUseDefaultRunspaceInThreadSafeManner = new bool?(localPipeline.NestedPipelineExecutionThread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId);
						}
					}
				}
				return this.canUseDefaultRunspaceInThreadSafeManner.Value;
			}
		}

		// Token: 0x06006966 RID: 26982 RVA: 0x0021330E File Offset: 0x0021150E
		internal CimInstance Serialize(object o)
		{
			return this.CreateCimInstanceForOneTopLevelObject(o);
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x00213318 File Offset: 0x00211518
		internal CimInstance CreateCimInstanceForOneTopLevelObject(object source)
		{
			CimInstance result;
			this.CreateCimInstanceForOneObject(source, null, this._depth, out result);
			return result;
		}

		// Token: 0x06006968 RID: 26984 RVA: 0x00213336 File Offset: 0x00211536
		private void CreateCimInstanceForOneObject(object source, string property, int depth, out CimInstance result)
		{
			result = InternalMISerializer.CreateNullCimInstance();
			if (source == null)
			{
				return;
			}
			if (this.HandlePrimitiveKnownTypeByConvertingToPSObject(source, property, depth, out result))
			{
				return;
			}
			if (this.HandleKnownContainerTypes(source, property, depth, out result))
			{
				return;
			}
			this.HandleComplexTypePSObject(source, property, depth, out result);
		}

		// Token: 0x06006969 RID: 26985 RVA: 0x0021336C File Offset: 0x0021156C
		private bool HandlePrimitiveKnownTypeByConvertingToPSObject(object source, string property, int depth, out CimInstance result)
		{
			result = InternalMISerializer.CreateNullCimInstance();
			MITypeSerializationInfo typeSerializationInfo = KnownMITypes.GetTypeSerializationInfo(source.GetType());
			if (typeSerializationInfo != null)
			{
				PSObject source2 = PSObject.AsPSObject(source);
				return this.HandlePrimitiveKnownTypePSObject(source2, property, depth, out result);
			}
			return false;
		}

		// Token: 0x0600696A RID: 26986 RVA: 0x002133A4 File Offset: 0x002115A4
		private bool HandlePrimitiveKnownTypePSObject(object source, string property, int depth, out CimInstance result)
		{
			result = InternalMISerializer.CreateNullCimInstance();
			bool result2 = false;
			PSObject psobject = source as PSObject;
			if (psobject != null && !psobject.immediateBaseObjectIsEmpty)
			{
				object immediateBaseObject = psobject.ImmediateBaseObject;
				MITypeSerializationInfo typeSerializationInfo = KnownMITypes.GetTypeSerializationInfo(immediateBaseObject.GetType());
				if (typeSerializationInfo != null)
				{
					this.CreateCimInstanceForPrimitiveTypePSObject(psobject, immediateBaseObject, typeSerializationInfo, property, depth, out result);
					result2 = true;
				}
			}
			return result2;
		}

		// Token: 0x0600696B RID: 26987 RVA: 0x002133F4 File Offset: 0x002115F4
		private bool HandleKnownContainerTypes(object source, string property, int depth, out CimInstance result)
		{
			result = InternalMISerializer.CreateNullCimInstance();
			ContainerType containerType = ContainerType.None;
			PSObject psobject = source as PSObject;
			IEnumerable enumerable = null;
			IDictionary dictionary = null;
			if (psobject != null && psobject.immediateBaseObjectIsEmpty)
			{
				return false;
			}
			SerializationUtilities.GetKnownContainerTypeInfo((psobject != null) ? psobject.ImmediateBaseObject : source, out containerType, out dictionary, out enumerable);
			if (containerType == ContainerType.None)
			{
				return false;
			}
			result = InternalMISerializer.CreateCimInstanceForPSObject("PS_Object", psobject ?? PSObject.AsPSObject(source), false);
			List<CimInstance> list = null;
			switch (containerType)
			{
			case ContainerType.Dictionary:
				this.WriteDictionary(dictionary, depth, out list);
				break;
			case ContainerType.Queue:
				this.WriteEnumerable(enumerable, property, depth, out list);
				break;
			case ContainerType.Stack:
				this.WriteEnumerable(enumerable, property, depth, out list);
				break;
			case ContainerType.List:
				this.WriteEnumerable(enumerable, property, depth, out list);
				break;
			case ContainerType.Enumerable:
				this.WriteEnumerable(enumerable, property, depth, out list);
				break;
			}
			CimInstance[] value = list.ToArray();
			CimProperty newItem = CimProperty.Create("Value", value, CimType.InstanceArray, CimFlags.Property);
			result.CimInstanceProperties.Add(newItem);
			return true;
		}

		// Token: 0x0600696C RID: 26988 RVA: 0x002134E8 File Offset: 0x002116E8
		private void HandleComplexTypePSObject(object source, string property, int depth, out CimInstance result)
		{
			List<CimInstance> list = null;
			PSObject psobject = PSObject.AsPSObject(source);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (!psobject.immediateBaseObjectIsEmpty)
			{
				ErrorRecord errorRecord = psobject.ImmediateBaseObject as ErrorRecord;
				if (errorRecord == null)
				{
					InformationalRecord informationalRecord = psobject.ImmediateBaseObject as InformationalRecord;
					if (informationalRecord == null)
					{
						flag3 = (psobject.ImmediateBaseObject is Enum);
						flag4 = (psobject.ImmediateBaseObject is PSObject);
					}
					else
					{
						informationalRecord.ToPSObjectForRemoting(psobject);
						flag2 = true;
					}
				}
				else
				{
					errorRecord.ToPSObjectForRemoting(psobject);
					flag = true;
				}
			}
			bool writeToString = true;
			if (psobject.ToStringFromDeserialization == null && psobject.immediateBaseObjectIsEmpty)
			{
				writeToString = false;
			}
			result = InternalMISerializer.CreateCimInstanceForPSObject("PS_Object", psobject, writeToString);
			PSMemberInfoInternalCollection<PSPropertyInfo> specificPropertiesToSerialize = SerializationUtilities.GetSpecificPropertiesToSerialize(psobject, this.AllPropertiesCollection, this._typeTable);
			if (flag3)
			{
				CimInstance value = this.CreateCimInstanceForEnum(psobject, depth, property != null);
				CimProperty newItem = CimProperty.Create("Value", value, CimType.Reference, CimFlags.Property);
				result.CimInstanceProperties.Add(newItem);
			}
			else if (flag4)
			{
				CimInstance value2;
				this.CreateCimInstanceForOneObject(psobject.ImmediateBaseObject, property, depth, out value2);
				CimProperty newItem2 = CimProperty.Create("Value", value2, CimType.Reference, CimFlags.Property);
				result.CimInstanceProperties.Add(newItem2);
			}
			else if (!flag && !flag2)
			{
				this.CreateCimInstanceForPSObjectProperties(psobject, depth, specificPropertiesToSerialize, out list);
			}
			if (list != null && list.Count > 0)
			{
				CimInstance[] value3 = list.ToArray();
				CimProperty newItem3 = CimProperty.Create("Properties", value3, CimType.ReferenceArray, CimFlags.Property);
				result.CimInstanceProperties.Add(newItem3);
			}
		}

		// Token: 0x0600696D RID: 26989 RVA: 0x0021365C File Offset: 0x0021185C
		private void CreateCimInstanceForPrimitiveTypePSObject(PSObject source, object primitive, MITypeSerializationInfo pktInfo, string property, int depth, out CimInstance result)
		{
			result = InternalMISerializer.CreateNullCimInstance();
			string toStringForPrimitiveObject = SerializationUtilities.GetToStringForPrimitiveObject(source);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = toStringForPrimitiveObject != null;
			if (!flag2 && !flag)
			{
				if (flag3)
				{
					return;
				}
				if (primitive != null)
				{
					InternalMISerializer.CreateCimInstanceForOnePrimitiveKnownType(this, property, primitive, pktInfo, out result);
				}
			}
		}

		// Token: 0x0600696E RID: 26990 RVA: 0x002136A0 File Offset: 0x002118A0
		private static void CreateCimInstanceForOnePrimitiveKnownType(InternalMISerializer serializer, string property, object source, MITypeSerializationInfo entry, out CimInstance result)
		{
			if (entry != null && entry.Serializer == null)
			{
				string value = Convert.ToString(source, CultureInfo.InvariantCulture);
				result = InternalMISerializer.CreateRawStringCimInstance(property, value, entry);
				return;
			}
			result = entry.Serializer(property, source, entry);
		}

		// Token: 0x0600696F RID: 26991 RVA: 0x002136E4 File Offset: 0x002118E4
		private void WriteEnumerable(IEnumerable enumerable, string property, int depth, out List<CimInstance> enumerableInstances)
		{
			enumerableInstances = new List<CimInstance>();
			IEnumerator enumerator = null;
			try
			{
				enumerator = enumerable.GetEnumerator();
				try
				{
					enumerator.Reset();
				}
				catch (NotSupportedException)
				{
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_EnumerationFailed, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
				{
					enumerable.GetType().AssemblyQualifiedName,
					ex.ToString()
				});
				enumerator = null;
			}
			if (enumerator != null)
			{
				for (;;)
				{
					object source = null;
					try
					{
						if (!enumerator.MoveNext())
						{
							break;
						}
						source = enumerator.Current;
					}
					catch (Exception ex2)
					{
						CommandProcessorBase.CheckForSevereException(ex2);
						PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_EnumerationFailed, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
						{
							enumerable.GetType().AssemblyQualifiedName,
							ex2.ToString()
						});
						break;
					}
					CimInstance item;
					this.CreateCimInstanceForOneObject(source, property, depth, out item);
					enumerableInstances.Add(item);
				}
			}
		}

		// Token: 0x06006970 RID: 26992 RVA: 0x002137EC File Offset: 0x002119EC
		private void WriteDictionary(IDictionary dictionary, int depth, out List<CimInstance> listOfCimInstances)
		{
			listOfCimInstances = new List<CimInstance>();
			IDictionaryEnumerator dictionaryEnumerator = null;
			try
			{
				dictionaryEnumerator = dictionary.GetEnumerator();
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_EnumerationFailed, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
				{
					dictionary.GetType().AssemblyQualifiedName,
					ex.ToString()
				});
			}
			if (dictionaryEnumerator != null)
			{
				for (;;)
				{
					object obj = null;
					object value = null;
					try
					{
						if (!dictionaryEnumerator.MoveNext())
						{
							return;
						}
						obj = dictionaryEnumerator.Key;
						value = dictionaryEnumerator.Value;
					}
					catch (Exception ex2)
					{
						CommandProcessorBase.CheckForSevereException(ex2);
						PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_EnumerationFailed, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
						{
							dictionary.GetType().AssemblyQualifiedName,
							ex2.ToString()
						});
						return;
					}
					if (obj == null)
					{
						break;
					}
					CimInstance item = this.CreateCimInstanceForDictionaryEntry(obj, value, depth);
					listOfCimInstances.Add(item);
				}
				return;
			}
		}

		// Token: 0x06006971 RID: 26993 RVA: 0x002138EC File Offset: 0x00211AEC
		private CimInstance CreateCimInstanceForDictionaryEntry(object key, object value, int depth)
		{
			CimInstance cimInstance = InternalMISerializer.CreateCimInstance("PSObject_DictionaryEntry");
			CimInstance value2;
			this.CreateCimInstanceForOneObject(key, null, depth, out value2);
			CimInstance value3;
			this.CreateCimInstanceForOneObject(value, null, depth, out value3);
			CimProperty newItem = CimProperty.Create("Key", value2, CimType.Instance, CimFlags.Property);
			CimProperty newItem2 = CimProperty.Create("Value", value3, CimType.Instance, CimFlags.Property);
			cimInstance.CimInstanceProperties.Add(newItem);
			cimInstance.CimInstanceProperties.Add(newItem2);
			return cimInstance;
		}

		// Token: 0x06006972 RID: 26994 RVA: 0x00213958 File Offset: 0x00211B58
		internal static CimInstance CreateCimInstanceForPrimitiveType(string property, object source, MITypeSerializationInfo entry)
		{
			CimInstance cimInstance;
			if (property != null)
			{
				cimInstance = InternalMISerializer.CreateCimInstanceWhenPropertyNameExists(property, source, entry);
			}
			else
			{
				cimInstance = InternalMISerializer.CreateCimInstance(entry.CimClassName);
				CimProperty newItem = CimProperty.Create("Value", source, entry.CimType, CimFlags.Property);
				cimInstance.CimInstanceProperties.Add(newItem);
			}
			return cimInstance;
		}

		// Token: 0x06006973 RID: 26995 RVA: 0x002139A0 File Offset: 0x00211BA0
		internal static CimInstance CreateCimInstanceForString(string property, object source, MITypeSerializationInfo entry)
		{
			string text = InternalSerializer.EncodeString((string)source);
			CimInstance cimInstance;
			if (property != null)
			{
				cimInstance = InternalMISerializer.CreateCimInstanceWhenPropertyNameExists(property, text, entry);
			}
			else
			{
				cimInstance = InternalMISerializer.CreateCimInstance(entry.CimClassName);
				CimProperty newItem = CimProperty.Create("Value", text, entry.CimType, CimFlags.Property);
				cimInstance.CimInstanceProperties.Add(newItem);
			}
			return cimInstance;
		}

		// Token: 0x06006974 RID: 26996 RVA: 0x002139F4 File Offset: 0x00211BF4
		private static CimProperty CreateTypeNamesProperty(IEnumerable<string> types)
		{
			return CimProperty.Create("TypeNames", types, CimType.StringArray, CimFlags.Property);
		}

		// Token: 0x06006975 RID: 26997 RVA: 0x00213A05 File Offset: 0x00211C05
		internal static CimProperty CreateCimProperty(string propertyName, object propertyValue, CimType cimType)
		{
			return CimProperty.Create(propertyName, propertyValue, cimType, CimFlags.Property);
		}

		// Token: 0x06006976 RID: 26998 RVA: 0x00213A14 File Offset: 0x00211C14
		internal static CimInstance CreateCimInstance(string cimClassName)
		{
			return new CimInstance(cimClassName, "root/Microsoft/Windows/Powershellv4");
		}

		// Token: 0x06006977 RID: 26999 RVA: 0x00213A30 File Offset: 0x00211C30
		internal static CimInstance CreateCimInstanceForPSObject(string cimClassName, PSObject psObj, bool writeToString)
		{
			CimInstance cimInstance = new CimInstance(cimClassName, "root/Microsoft/Windows/Powershellv4");
			CimProperty newItem = InternalMISerializer.CreateTypeNamesProperty(psObj.TypeNames);
			cimInstance.CimInstanceProperties.Add(newItem);
			if (writeToString)
			{
				CimProperty newItem2 = CimProperty.Create("ToString", SerializationUtilities.GetToString(psObj), CimType.String, CimFlags.Property);
				cimInstance.CimInstanceProperties.Add(newItem2);
			}
			return cimInstance;
		}

		// Token: 0x06006978 RID: 27000 RVA: 0x00213A88 File Offset: 0x00211C88
		private static CimInstance CreateRawStringCimInstance(string property, string value, MITypeSerializationInfo entry)
		{
			CimInstance cimInstance;
			if (property != null)
			{
				cimInstance = InternalMISerializer.CreateCimInstanceWhenPropertyNameExists(property, value, entry);
			}
			else
			{
				cimInstance = InternalMISerializer.CreateCimInstance(entry.CimClassName);
				CimProperty newItem = CimProperty.Create("Value", value, entry.CimType, CimFlags.Property);
				cimInstance.CimInstanceProperties.Add(newItem);
			}
			return cimInstance;
		}

		// Token: 0x06006979 RID: 27001 RVA: 0x00213AD0 File Offset: 0x00211CD0
		private static CimInstance CreateCimInstanceWhenPropertyNameExists(string property, object source, MITypeSerializationInfo entry)
		{
			CimInstance cimInstance = InternalMISerializer.CreateCimInstance(entry.CimClassName);
			CimProperty newItem = CimProperty.Create("Value", source, entry.CimType, CimFlags.Property);
			cimInstance.CimInstanceProperties.Add(newItem);
			CimInstance cimInstance2 = InternalMISerializer.CreateCimInstance("PS_ObjectProperty");
			CimProperty newItem2 = CimProperty.Create("Name", property, CimType.String, CimFlags.Property);
			cimInstance2.CimInstanceProperties.Add(newItem2);
			CimProperty newItem3 = CimProperty.Create("Value", cimInstance, CimType.Reference, CimFlags.Property);
			cimInstance2.CimInstanceProperties.Add(newItem3);
			return cimInstance2;
		}

		// Token: 0x0600697A RID: 27002 RVA: 0x00213B50 File Offset: 0x00211D50
		private CimInstance CreateCimInstanceForEnum(PSObject mshSource, int depth, bool serializeAsString)
		{
			CimInstance result = null;
			object immediateBaseObject = mshSource.ImmediateBaseObject;
			this.CreateCimInstanceForOneObject(Convert.ChangeType(immediateBaseObject, Enum.GetUnderlyingType(immediateBaseObject.GetType()), CultureInfo.InvariantCulture), null, depth, out result);
			return result;
		}

		// Token: 0x0600697B RID: 27003 RVA: 0x00213B88 File Offset: 0x00211D88
		private void CreateCimInstanceForPSObjectProperties(PSObject source, int depth, IEnumerable<PSPropertyInfo> specificPropertiesToSerialize, out List<CimInstance> listOfCimInstances)
		{
			listOfCimInstances = new List<CimInstance>();
			if (specificPropertiesToSerialize != null)
			{
				this.SerializeProperties(specificPropertiesToSerialize, depth, out listOfCimInstances);
				return;
			}
			if (source.ShouldSerializeAdapter())
			{
				IEnumerable<PSPropertyInfo> adaptedProperties = source.GetAdaptedProperties();
				if (adaptedProperties != null)
				{
					this.SerializeProperties(adaptedProperties, depth, out listOfCimInstances);
				}
			}
		}

		// Token: 0x0600697C RID: 27004 RVA: 0x00213BCC File Offset: 0x00211DCC
		private void SerializeProperties(IEnumerable<PSPropertyInfo> propertyCollection, int depth, out List<CimInstance> listOfCimInstances)
		{
			listOfCimInstances = new List<CimInstance>();
			foreach (PSMemberInfo psmemberInfo in propertyCollection)
			{
				PSProperty psproperty = psmemberInfo as PSProperty;
				if (psproperty != null)
				{
					bool flag;
					object propertyValueInThreadSafeManner = SerializationUtilities.GetPropertyValueInThreadSafeManner(psproperty, this.CanUseDefaultRunspaceInThreadSafeManner, out flag);
					if (flag)
					{
						CimInstance item = null;
						this.CreateCimInstanceForOneObject(propertyValueInThreadSafeManner, psproperty.Name, depth, out item);
						listOfCimInstances.Add(item);
					}
				}
			}
		}

		// Token: 0x0600697D RID: 27005 RVA: 0x00213C54 File Offset: 0x00211E54
		private static CimInstance CreateNullCimInstance()
		{
			return new CimInstance("Null");
		}

		// Token: 0x0400327B RID: 12923
		private const string PowerShellRemotingProviderNamespace = "root/Microsoft/Windows/Powershellv4";

		// Token: 0x0400327C RID: 12924
		private int _depth;

		// Token: 0x0400327D RID: 12925
		private CimInstance _cimInstance;

		// Token: 0x0400327E RID: 12926
		private TypeTable _typeTable;

		// Token: 0x0400327F RID: 12927
		private Collection<CollectionEntry<PSPropertyInfo>> allPropertiesCollection;

		// Token: 0x04003280 RID: 12928
		private bool? canUseDefaultRunspaceInThreadSafeManner;
	}
}
