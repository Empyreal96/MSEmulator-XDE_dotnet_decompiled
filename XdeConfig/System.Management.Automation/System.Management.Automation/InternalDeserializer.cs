using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Serialization;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000453 RID: 1107
	internal class InternalDeserializer
	{
		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x06003047 RID: 12359 RVA: 0x001076E8 File Offset: 0x001058E8
		private bool UnknownTagsAllowed
		{
			get
			{
				return this._version.Minor > 1;
			}
		}

		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x06003048 RID: 12360 RVA: 0x001076F8 File Offset: 0x001058F8
		private bool DuplicateRefIdsAllowed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x001076FB File Offset: 0x001058FB
		internal InternalDeserializer(XmlReader reader, DeserializationContext context)
		{
			this._reader = reader;
			this._context = context;
			this.objectRefIdHandler = new ReferenceIdHandlerForDeserializer<object>();
			this.typeRefIdHandler = new ReferenceIdHandlerForDeserializer<ConsolidatedString>();
		}

		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x0600304A RID: 12362 RVA: 0x00107727 File Offset: 0x00105927
		// (set) Token: 0x0600304B RID: 12363 RVA: 0x0010772F File Offset: 0x0010592F
		internal TypeTable TypeTable
		{
			get
			{
				return this._typeTable;
			}
			set
			{
				this._typeTable = value;
			}
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x00107738 File Offset: 0x00105938
		internal void ValidateVersion(string version)
		{
			this._version = null;
			Exception ex = null;
			try
			{
				this._version = new Version(version);
			}
			catch (ArgumentException ex2)
			{
				ex = ex2;
			}
			catch (FormatException ex3)
			{
				ex = ex3;
			}
			if (ex != null)
			{
				throw this.NewXmlException(Serialization.InvalidVersion, ex, new object[0]);
			}
			if (this._version.Major != 1)
			{
				throw this.NewXmlException(Serialization.UnexpectedVersion, null, new object[]
				{
					this._version.Major
				});
			}
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x001077D0 File Offset: 0x001059D0
		private object ReadOneDeserializedObject(out string streamName, out bool isKnownPrimitiveType)
		{
			if (this._reader.NodeType != XmlNodeType.Element)
			{
				throw this.NewXmlException(Serialization.InvalidNodeType, null, new object[]
				{
					this._reader.NodeType.ToString(),
					XmlNodeType.Element.ToString()
				});
			}
			InternalDeserializer._trace.WriteLine("Processing start node {0}", new object[]
			{
				this._reader.LocalName
			});
			streamName = this._reader.GetAttribute("S");
			isKnownPrimitiveType = false;
			if (this.IsNextElement("Nil"))
			{
				this.Skip();
				return null;
			}
			if (this.IsNextElement("Ref"))
			{
				string attribute = this._reader.GetAttribute("RefId");
				if (attribute == null)
				{
					throw this.NewXmlException(Serialization.AttributeExpected, null, new object[]
					{
						"RefId"
					});
				}
				object referencedObject = this.objectRefIdHandler.GetReferencedObject(attribute);
				if (referencedObject == null)
				{
					throw this.NewXmlException(Serialization.InvalidReferenceId, null, new object[]
					{
						attribute
					});
				}
				this.Skip();
				return referencedObject;
			}
			else
			{
				TypeSerializationInfo typeSerializationInfoFromItemTag = KnownTypes.GetTypeSerializationInfoFromItemTag(this._reader.LocalName);
				if (typeSerializationInfoFromItemTag != null)
				{
					InternalDeserializer._trace.WriteLine("Primitive Knowntype Element {0}", new object[]
					{
						typeSerializationInfoFromItemTag.ItemTag
					});
					isKnownPrimitiveType = true;
					return this.ReadPrimaryKnownType(typeSerializationInfoFromItemTag);
				}
				if (this.IsNextElement("Obj"))
				{
					InternalDeserializer._trace.WriteLine("PSObject Element", new object[0]);
					return this.ReadPSObject();
				}
				InternalDeserializer._trace.TraceError("Invalid element {0} tag found", new object[]
				{
					this._reader.LocalName
				});
				throw this.NewXmlException(Serialization.InvalidElementTag, null, new object[]
				{
					this._reader.LocalName
				});
			}
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x001079A5 File Offset: 0x00105BA5
		internal void Stop()
		{
			this.isStopping = true;
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x001079AE File Offset: 0x00105BAE
		private void CheckIfStopping()
		{
			if (this.isStopping)
			{
				throw PSTraceSource.NewInvalidOperationException(Serialization.Stopping, new object[0]);
			}
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x001079CC File Offset: 0x00105BCC
		private bool RehydrateCimInstanceProperty(CimInstance cimInstance, PSPropertyInfo deserializedProperty, HashSet<string> namesOfModifiedProperties)
		{
			if (deserializedProperty.Name.Equals(RemotingConstants.ComputerNameNoteProperty, StringComparison.OrdinalIgnoreCase))
			{
				string text = deserializedProperty.Value as string;
				if (text != null)
				{
					cimInstance.SetCimSessionComputerName(text);
				}
				return true;
			}
			CimProperty cimProperty = cimInstance.CimInstanceProperties[deserializedProperty.Name];
			if (cimProperty == null)
			{
				return false;
			}
			object obj = deserializedProperty.Value;
			if (obj != null)
			{
				PSObject psobject = PSObject.AsPSObject(obj);
				if (psobject.BaseObject is ArrayList)
				{
					if (psobject.InternalTypeNames == null || psobject.InternalTypeNames.Count == 0)
					{
						return false;
					}
					string text2 = Deserializer.MaskDeserializationPrefix(psobject.InternalTypeNames[0]);
					if (text2 == null)
					{
						return false;
					}
					Type type;
					if (!LanguagePrimitives.TryConvertTo<Type>(text2, CultureInfo.InvariantCulture, out type))
					{
						return false;
					}
					if (!type.IsArray)
					{
						return false;
					}
					object obj2;
					if (!LanguagePrimitives.TryConvertTo(obj, type, CultureInfo.InvariantCulture, out obj2))
					{
						return false;
					}
					psobject = PSObject.AsPSObject(obj2);
				}
				obj = psobject.BaseObject;
			}
			try
			{
				cimProperty.Value = obj;
				if (!namesOfModifiedProperties.Contains(deserializedProperty.Name))
				{
					cimProperty.IsValueModified = false;
				}
			}
			catch (FormatException)
			{
				return false;
			}
			catch (InvalidCastException)
			{
				return false;
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (CimException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x00107B1C File Offset: 0x00105D1C
		private CimClass RehydrateCimClass(PSPropertyInfo classMetadataProperty)
		{
			if (classMetadataProperty == null || classMetadataProperty.Value == null)
			{
				return null;
			}
			IEnumerable enumerable = LanguagePrimitives.GetEnumerable(classMetadataProperty.Value);
			if (enumerable == null)
			{
				return null;
			}
			Stack<KeyValuePair<CimClassSerializationId, CimClass>> stack = new Stack<KeyValuePair<CimClassSerializationId, CimClass>>();
			CimClass cimClass = null;
			foreach (object obj in enumerable)
			{
				CimClass parentClass = cimClass;
				if (obj == null)
				{
					return null;
				}
				PSObject psobject = PSObject.AsPSObject(obj);
				PSPropertyInfo pspropertyInfo = psobject.InstanceMembers["Namespace"] as PSPropertyInfo;
				if (pspropertyInfo == null)
				{
					return null;
				}
				string namespaceName = pspropertyInfo.Value as string;
				PSPropertyInfo pspropertyInfo2 = psobject.InstanceMembers["ClassName"] as PSPropertyInfo;
				if (pspropertyInfo2 == null)
				{
					return null;
				}
				string className = pspropertyInfo2.Value as string;
				PSPropertyInfo pspropertyInfo3 = psobject.InstanceMembers["ServerName"] as PSPropertyInfo;
				if (pspropertyInfo3 == null)
				{
					return null;
				}
				string computerName = pspropertyInfo3.Value as string;
				PSPropertyInfo pspropertyInfo4 = psobject.InstanceMembers["Hash"] as PSPropertyInfo;
				if (pspropertyInfo4 == null)
				{
					return null;
				}
				object obj2 = pspropertyInfo4.Value;
				if (obj2 == null)
				{
					return null;
				}
				if (obj2 is PSObject)
				{
					obj2 = ((PSObject)obj2).BaseObject;
				}
				if (!(obj2 is int))
				{
					return null;
				}
				int hashCode = (int)obj2;
				CimClassSerializationId key = new CimClassSerializationId(className, namespaceName, computerName, hashCode);
				cimClass = this._context.cimClassSerializationIdCache.GetCimClassFromCache(key);
				if (cimClass == null)
				{
					PSPropertyInfo pspropertyInfo5 = psobject.InstanceMembers["MiXml"] as PSPropertyInfo;
					if (pspropertyInfo5 == null || pspropertyInfo5.Value == null)
					{
						return null;
					}
					string s = pspropertyInfo5.Value.ToString();
					byte[] bytes = Encoding.Unicode.GetBytes(s);
					uint num = 0U;
					try
					{
						cimClass = InternalDeserializer.cimDeserializer.Value.DeserializeClass(bytes, ref num, parentClass, computerName, namespaceName);
						stack.Push(new KeyValuePair<CimClassSerializationId, CimClass>(key, cimClass));
					}
					catch (CimException)
					{
						return null;
					}
				}
			}
			foreach (KeyValuePair<CimClassSerializationId, CimClass> keyValuePair in stack)
			{
				this._context.cimClassSerializationIdCache.AddCimClassToCache(keyValuePair.Key, keyValuePair.Value);
			}
			return cimClass;
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x00107DDC File Offset: 0x00105FDC
		private PSObject RehydrateCimInstance(PSObject deserializedObject)
		{
			if (!(deserializedObject.BaseObject is PSCustomObject))
			{
				return deserializedObject;
			}
			PSPropertyInfo classMetadataProperty = deserializedObject.InstanceMembers["__ClassMetadata"] as PSPropertyInfo;
			CimClass cimClass = this.RehydrateCimClass(classMetadataProperty);
			if (cimClass == null)
			{
				return deserializedObject;
			}
			CimInstance cimInstance;
			try
			{
				cimInstance = new CimInstance(cimClass);
			}
			catch (CimException)
			{
				return deserializedObject;
			}
			PSObject psobject = PSObject.AsPSObject(cimInstance);
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			PSPropertyInfo pspropertyInfo = deserializedObject.InstanceMembers["__InstanceMetadata"] as PSPropertyInfo;
			if (pspropertyInfo != null && pspropertyInfo.Value != null)
			{
				PSObject psobject2 = PSObject.AsPSObject(pspropertyInfo.Value);
				PSPropertyInfo pspropertyInfo2 = psobject2.InstanceMembers["Modified"] as PSPropertyInfo;
				if (pspropertyInfo2 != null && pspropertyInfo2.Value != null)
				{
					string text = pspropertyInfo2.Value.ToString();
					foreach (string item in text.Split(new char[]
					{
						' '
					}))
					{
						hashSet.Add(item);
					}
				}
			}
			if (deserializedObject.adaptedMembers != null)
			{
				foreach (PSMemberInfo psmemberInfo in deserializedObject.adaptedMembers)
				{
					PSPropertyInfo pspropertyInfo3 = psmemberInfo as PSPropertyInfo;
					if (pspropertyInfo3 != null && !this.RehydrateCimInstanceProperty(cimInstance, pspropertyInfo3, hashSet))
					{
						return deserializedObject;
					}
				}
			}
			foreach (PSMemberInfo psmemberInfo2 in deserializedObject.InstanceMembers)
			{
				PSPropertyInfo pspropertyInfo4 = psmemberInfo2 as PSPropertyInfo;
				if (pspropertyInfo4 != null && (deserializedObject.adaptedMembers == null || deserializedObject.adaptedMembers[pspropertyInfo4.Name] == null) && !pspropertyInfo4.Name.Equals("__ClassMetadata", StringComparison.OrdinalIgnoreCase) && psobject.Properties[pspropertyInfo4.Name] == null)
				{
					PSNoteProperty member = new PSNoteProperty(pspropertyInfo4.Name, pspropertyInfo4.Value);
					psobject.Properties.Add(member);
				}
			}
			return psobject;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x00108014 File Offset: 0x00106214
		internal object ReadOneObject(out string streamName)
		{
			this.CheckIfStopping();
			object result;
			try
			{
				this.depthBelowTopLevel++;
				if (this.depthBelowTopLevel == 50)
				{
					throw this.NewXmlException(Serialization.DeserializationTooDeep, null, new object[0]);
				}
				bool flag;
				object obj = this.ReadOneDeserializedObject(out streamName, out flag);
				if (obj == null)
				{
					result = null;
				}
				else
				{
					if (!flag)
					{
						PSObject psobject = PSObject.AsPSObject(obj);
						if (Deserializer.IsDeserializedInstanceOfType(psobject, typeof(CimInstance)))
						{
							return this.RehydrateCimInstance(psobject);
						}
						Type targetTypeForDeserialization = psobject.GetTargetTypeForDeserialization(this._typeTable);
						if (null != targetTypeForDeserialization)
						{
							Exception ex = null;
							try
							{
								object obj2 = LanguagePrimitives.ConvertTo(obj, targetTypeForDeserialization, true, CultureInfo.InvariantCulture, this._typeTable);
								PSEtwLog.LogAnalyticVerbose(PSEventId.Serializer_RehydrationSuccess, PSOpcode.Rehydration, PSTask.Serialization, PSKeyword.Serializer, new object[]
								{
									psobject.InternalTypeNames.Key,
									targetTypeForDeserialization.FullName,
									obj2.GetType().FullName
								});
								return obj2;
							}
							catch (InvalidCastException ex2)
							{
								ex = ex2;
							}
							catch (ArgumentException ex3)
							{
								ex = ex3;
							}
							PSEtwLog.LogAnalyticError(PSEventId.Serializer_RehydrationFailure, PSOpcode.Rehydration, PSTask.Serialization, PSKeyword.Serializer, new object[]
							{
								psobject.InternalTypeNames.Key,
								targetTypeForDeserialization.FullName,
								ex.ToString(),
								(ex.InnerException == null) ? string.Empty : ex.InnerException.ToString()
							});
						}
					}
					result = obj;
				}
			}
			finally
			{
				this.depthBelowTopLevel--;
			}
			return result;
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x001081E4 File Offset: 0x001063E4
		private object ReadOneObject()
		{
			string text;
			return this.ReadOneObject(out text);
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x001081FC File Offset: 0x001063FC
		private PSObject ReadPSObject()
		{
			PSObject psobject = this.ReadAttributeAndCreatePSObject();
			if (!this.ReadStartElementAndHandleEmpty("Obj"))
			{
				return psobject;
			}
			bool overrideTypeInfo = true;
			while (this._reader.NodeType == XmlNodeType.Element)
			{
				if (this.IsNextElement("TN") || this.IsNextElement("TNRef"))
				{
					this.ReadTypeNames(psobject);
					overrideTypeInfo = false;
				}
				else if (this.IsNextElement("Props"))
				{
					this.ReadProperties(psobject);
				}
				else if (this.IsNextElement("MS"))
				{
					this.ReadMemberSet(psobject.InstanceMembers);
				}
				else if (this.IsNextElement("ToString"))
				{
					psobject.ToStringFromDeserialization = this.ReadDecodedElementString("ToString");
					psobject.InstanceMembers.Add(PSObject.dotNetInstanceAdapter.GetDotNetMethod<PSMemberInfo>(psobject, "ToString"));
					PSGetMemberBinder.SetHasInstanceMember("ToString");
					psobject.TokenText = psobject.ToStringFromDeserialization;
				}
				else
				{
					object obj = null;
					ContainerType containerType = ContainerType.None;
					TypeSerializationInfo typeSerializationInfoFromItemTag = KnownTypes.GetTypeSerializationInfoFromItemTag(this._reader.LocalName);
					if (typeSerializationInfoFromItemTag != null)
					{
						InternalDeserializer._trace.WriteLine("Primitive Knowntype Element {0}", new object[]
						{
							typeSerializationInfoFromItemTag.ItemTag
						});
						obj = this.ReadPrimaryKnownType(typeSerializationInfoFromItemTag);
					}
					else if (this.IsKnownContainerTag(out containerType))
					{
						InternalDeserializer._trace.WriteLine("Found container node {0}", new object[]
						{
							containerType
						});
						obj = this.ReadKnownContainer(containerType);
					}
					else if (this.IsNextElement("Obj"))
					{
						InternalDeserializer._trace.WriteLine("Found PSObject node", new object[0]);
						obj = this.ReadOneObject();
					}
					else
					{
						InternalDeserializer._trace.WriteLine("Unknwon tag {0} encountered", new object[]
						{
							this._reader.LocalName
						});
						if (!this.UnknownTagsAllowed)
						{
							throw this.NewXmlException(Serialization.InvalidElementTag, null, new object[]
							{
								this._reader.LocalName
							});
						}
						this.Skip();
					}
					if (obj != null)
					{
						psobject.SetCoreOnDeserialization(obj, overrideTypeInfo);
					}
				}
			}
			this.ReadEndElement();
			PSObject psobject2 = psobject.ImmediateBaseObject as PSObject;
			if (psobject2 != null)
			{
				PSObject.CopyDeserializerFields(psobject2, psobject);
			}
			return psobject;
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x00108424 File Offset: 0x00106624
		private PSObject ReadAttributeAndCreatePSObject()
		{
			string attribute = this._reader.GetAttribute("RefId");
			PSObject psobject = new PSObject();
			if (attribute != null)
			{
				InternalDeserializer._trace.WriteLine("Read PSObject with refId: {0}", new object[]
				{
					attribute
				});
				this.objectRefIdHandler.SetRefId(psobject, attribute, this.DuplicateRefIdsAllowed);
			}
			return psobject;
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x0010847C File Offset: 0x0010667C
		private void ReadTypeNames(PSObject dso)
		{
			if (this.IsNextElement("TN"))
			{
				Collection<string> collection = new Collection<string>();
				string attribute = this._reader.GetAttribute("RefId");
				InternalDeserializer._trace.WriteLine("Processing TypeNamesTag with refId {0}", new object[]
				{
					attribute
				});
				if (this.ReadStartElementAndHandleEmpty("TN"))
				{
					while (this._reader.NodeType == XmlNodeType.Element)
					{
						if (!this.IsNextElement("T"))
						{
							throw this.NewXmlException(Serialization.InvalidElementTag, null, new object[]
							{
								this._reader.LocalName
							});
						}
						string text = this.ReadDecodedElementString("T");
						if (!string.IsNullOrEmpty(text))
						{
							Deserializer.AddDeserializationPrefix(ref text);
							collection.Add(text);
						}
					}
					this.ReadEndElement();
				}
				dso.InternalTypeNames = new ConsolidatedString(collection);
				if (attribute != null)
				{
					this.typeRefIdHandler.SetRefId(dso.InternalTypeNames, attribute, this.DuplicateRefIdsAllowed);
					return;
				}
			}
			else if (this.IsNextElement("TNRef"))
			{
				string attribute2 = this._reader.GetAttribute("RefId");
				InternalDeserializer._trace.WriteLine("Processing TypeNamesReferenceTag with refId {0}", new object[]
				{
					attribute2
				});
				if (attribute2 == null)
				{
					throw this.NewXmlException(Serialization.AttributeExpected, null, new object[]
					{
						"RefId"
					});
				}
				ConsolidatedString referencedObject = this.typeRefIdHandler.GetReferencedObject(attribute2);
				if (referencedObject == null)
				{
					throw this.NewXmlException(Serialization.InvalidTypeHierarchyReferenceId, null, new object[]
					{
						attribute2
					});
				}
				this._context.LogExtraMemoryUsage(referencedObject.Key.Length * 2 - 29);
				dso.InternalTypeNames = new ConsolidatedString(referencedObject);
				this.Skip();
			}
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x00108634 File Offset: 0x00106834
		private void ReadProperties(PSObject dso)
		{
			dso.isDeserialized = true;
			dso.adaptedMembers = new PSMemberInfoInternalCollection<PSPropertyInfo>();
			dso.InstanceMembers.Add(PSObject.dotNetInstanceAdapter.GetDotNetMethod<PSMemberInfo>(dso, "GetType"));
			PSGetMemberBinder.SetHasInstanceMember("GetType");
			dso.clrMembers = new PSMemberInfoInternalCollection<PSPropertyInfo>();
			if (this.ReadStartElementAndHandleEmpty("Props"))
			{
				while (this._reader.NodeType == XmlNodeType.Element)
				{
					string name = this.ReadNameAttribute();
					object serializedValue = this.ReadOneObject();
					PSProperty member = new PSProperty(name, serializedValue);
					dso.adaptedMembers.Add(member);
				}
				this.ReadEndElement();
			}
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x001086C8 File Offset: 0x001068C8
		private void ReadMemberSet(PSMemberInfoCollection<PSMemberInfo> collection)
		{
			if (this.ReadStartElementAndHandleEmpty("MS"))
			{
				while (this._reader.NodeType == XmlNodeType.Element)
				{
					if (this.IsNextElement("MS"))
					{
						string text = this.ReadNameAttribute();
						PSMemberSet psmemberSet = new PSMemberSet(text);
						collection.Add(psmemberSet);
						this.ReadMemberSet(psmemberSet.Members);
						PSGetMemberBinder.SetHasInstanceMember(text);
					}
					else
					{
						PSNoteProperty psnoteProperty = this.ReadNoteProperty();
						collection.Add(psnoteProperty);
						PSGetMemberBinder.SetHasInstanceMember(psnoteProperty.Name);
					}
				}
				this.ReadEndElement();
			}
		}

		// Token: 0x0600305A RID: 12378 RVA: 0x00108748 File Offset: 0x00106948
		private PSNoteProperty ReadNoteProperty()
		{
			string name = this.ReadNameAttribute();
			object value = this.ReadOneObject();
			return new PSNoteProperty(name, value);
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x0010876C File Offset: 0x0010696C
		private bool IsKnownContainerTag(out ContainerType ct)
		{
			if (this.IsNextElement("DCT"))
			{
				ct = ContainerType.Dictionary;
			}
			else if (this.IsNextElement("QUE"))
			{
				ct = ContainerType.Queue;
			}
			else if (this.IsNextElement("STK"))
			{
				ct = ContainerType.Stack;
			}
			else if (this.IsNextElement("LST"))
			{
				ct = ContainerType.List;
			}
			else if (this.IsNextElement("IE"))
			{
				ct = ContainerType.Enumerable;
			}
			else
			{
				ct = ContainerType.None;
			}
			return ct != ContainerType.None;
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x001087E0 File Offset: 0x001069E0
		private object ReadKnownContainer(ContainerType ct)
		{
			switch (ct)
			{
			case ContainerType.Dictionary:
				return this.ReadDictionary(ct);
			case ContainerType.Queue:
			case ContainerType.Stack:
			case ContainerType.List:
			case ContainerType.Enumerable:
				return this.ReadListContainer(ct);
			default:
				return null;
			}
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x0010881C File Offset: 0x00106A1C
		private object ReadListContainer(ContainerType ct)
		{
			ArrayList arrayList = new ArrayList();
			if (this.ReadStartElementAndHandleEmpty(this._reader.LocalName))
			{
				while (this._reader.NodeType == XmlNodeType.Element)
				{
					arrayList.Add(this.ReadOneObject());
				}
				this.ReadEndElement();
			}
			if (ct == ContainerType.Stack)
			{
				arrayList.Reverse();
				return new Stack(arrayList);
			}
			if (ct == ContainerType.Queue)
			{
				return new Queue(arrayList);
			}
			return arrayList;
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x00108884 File Offset: 0x00106A84
		private object ReadDictionary(ContainerType ct)
		{
			Hashtable hashtable = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
			int num = 0;
			if (this.ReadStartElementAndHandleEmpty("DCT"))
			{
				while (this._reader.NodeType == XmlNodeType.Element)
				{
					this.ReadStartElement("En");
					if (this._reader.NodeType != XmlNodeType.Element)
					{
						throw this.NewXmlException(Serialization.DictionaryKeyNotSpecified, null, new object[0]);
					}
					string strA = this.ReadNameAttribute();
					if (string.Compare(strA, "Key", StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw this.NewXmlException(Serialization.InvalidDictionaryKeyName, null, new object[0]);
					}
					object obj = this.ReadOneObject();
					if (obj == null)
					{
						throw this.NewXmlException(Serialization.NullAsDictionaryKey, null, new object[0]);
					}
					if (this._reader.NodeType != XmlNodeType.Element)
					{
						throw this.NewXmlException(Serialization.DictionaryValueNotSpecified, null, new object[0]);
					}
					strA = this.ReadNameAttribute();
					if (string.Compare(strA, "Value", StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw this.NewXmlException(Serialization.InvalidDictionaryValueName, null, new object[0]);
					}
					object value = this.ReadOneObject();
					if (hashtable.ContainsKey(obj) && num == 0)
					{
						num++;
						Hashtable hashtable2 = new Hashtable();
						foreach (object obj2 in hashtable)
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
							hashtable2.Add(dictionaryEntry.Key, dictionaryEntry.Value);
						}
						hashtable = hashtable2;
					}
					if (hashtable.ContainsKey(obj) && num == 1)
					{
						num++;
						IEqualityComparer equalityComparer = new ReferenceEqualityComparer();
						Hashtable hashtable3 = new Hashtable(equalityComparer);
						foreach (object obj3 in hashtable)
						{
							DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj3;
							hashtable3.Add(dictionaryEntry2.Key, dictionaryEntry2.Value);
						}
						hashtable = hashtable3;
					}
					try
					{
						hashtable.Add(obj, value);
					}
					catch (ArgumentException innerException)
					{
						throw this.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
						{
							typeof(Hashtable)
						});
					}
					this.ReadEndElement();
				}
				this.ReadEndElement();
			}
			return hashtable;
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x00108AC8 File Offset: 0x00106CC8
		internal static XmlReaderSettings XmlReaderSettingsForCliXml
		{
			get
			{
				return InternalDeserializer.xmlReaderSettingsForCliXml;
			}
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x00108AD0 File Offset: 0x00106CD0
		private static XmlReaderSettings GetXmlReaderSettingsForCliXml()
		{
			return new XmlReaderSettings
			{
				CheckCharacters = false,
				CloseInput = false,
				ConformanceLevel = ConformanceLevel.Document,
				IgnoreComments = true,
				IgnoreProcessingInstructions = true,
				IgnoreWhitespace = false,
				MaxCharactersFromEntities = 1024L,
				Schemas = null,
				ValidationFlags = XmlSchemaValidationFlags.None,
				ValidationType = ValidationType.None,
				XmlResolver = null
			};
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x00108B36 File Offset: 0x00106D36
		internal static XmlReaderSettings XmlReaderSettingsForUntrustedXmlDocument
		{
			get
			{
				return InternalDeserializer.xmlReaderSettingsForUntrustedXmlDocument;
			}
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x00108B40 File Offset: 0x00106D40
		private static XmlReaderSettings GetXmlReaderSettingsForUntrustedXmlDocument()
		{
			return new XmlReaderSettings
			{
				CheckCharacters = false,
				ConformanceLevel = ConformanceLevel.Auto,
				IgnoreComments = true,
				IgnoreProcessingInstructions = true,
				IgnoreWhitespace = true,
				MaxCharactersFromEntities = 1024L,
				MaxCharactersInDocument = 536870912L,
				DtdProcessing = DtdProcessing.Parse,
				ValidationFlags = XmlSchemaValidationFlags.None,
				ValidationType = ValidationType.None,
				XmlResolver = null
			};
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x00108BAC File Offset: 0x00106DAC
		internal static object DeserializeBoolean(InternalDeserializer deserializer)
		{
			object result;
			try
			{
				result = XmlConvert.ToBoolean(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException innerException)
			{
				throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
				{
					typeof(bool).FullName
				});
			}
			return result;
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x00108C0C File Offset: 0x00106E0C
		internal static object DeserializeByte(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToByte(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(byte).FullName
			});
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x00108C84 File Offset: 0x00106E84
		internal static object DeserializeChar(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return (char)XmlConvert.ToUInt16(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(char).FullName
			});
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x00108CFC File Offset: 0x00106EFC
		internal static object DeserializeDateTime(InternalDeserializer deserializer)
		{
			object result;
			try
			{
				result = XmlConvert.ToDateTime(deserializer._reader.ReadElementContentAsString(), XmlDateTimeSerializationMode.RoundtripKind);
			}
			catch (FormatException innerException)
			{
				throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
				{
					typeof(DateTime).FullName
				});
			}
			return result;
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x00108D5C File Offset: 0x00106F5C
		internal static object DeserializeDecimal(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToDecimal(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(decimal).FullName
			});
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x00108DD4 File Offset: 0x00106FD4
		internal static object DeserializeDouble(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToDouble(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(double).FullName
			});
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x00108E4C File Offset: 0x0010704C
		internal static object DeserializeGuid(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToGuid(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(Guid).FullName
			});
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x00108EC4 File Offset: 0x001070C4
		internal static object DeserializeVersion(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return new Version(deserializer._reader.ReadElementContentAsString());
			}
			catch (ArgumentException ex)
			{
				innerException = ex;
			}
			catch (FormatException ex2)
			{
				innerException = ex2;
			}
			catch (OverflowException ex3)
			{
				innerException = ex3;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(Version).FullName
			});
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x00108F4C File Offset: 0x0010714C
		internal static object DeserializeInt16(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToInt16(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(short).FullName
			});
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x00108FC4 File Offset: 0x001071C4
		internal static object DeserializeInt32(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToInt32(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(int).FullName
			});
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x0010903C File Offset: 0x0010723C
		internal static object DeserializeInt64(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToInt64(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(long).FullName
			});
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x001090B4 File Offset: 0x001072B4
		internal static object DeserializeSByte(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToSByte(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(sbyte).FullName
			});
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x0010912C File Offset: 0x0010732C
		internal static object DeserializeSingle(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToSingle(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(float).FullName
			});
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x001091A4 File Offset: 0x001073A4
		internal static object DeserializeScriptBlock(InternalDeserializer deserializer)
		{
			string text = deserializer.ReadDecodedElementString("SBK");
			if (DeserializationOptions.DeserializeScriptBlocks == (deserializer._context.options & DeserializationOptions.DeserializeScriptBlocks))
			{
				return ScriptBlock.Create(text);
			}
			return text;
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x001091DD File Offset: 0x001073DD
		internal static object DeserializeString(InternalDeserializer deserializer)
		{
			return deserializer.ReadDecodedElementString("S");
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x001091EC File Offset: 0x001073EC
		internal static object DeserializeTimeSpan(InternalDeserializer deserializer)
		{
			object result;
			try
			{
				result = XmlConvert.ToTimeSpan(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException innerException)
			{
				throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
				{
					typeof(TimeSpan).FullName
				});
			}
			return result;
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x0010924C File Offset: 0x0010744C
		internal static object DeserializeUInt16(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToUInt16(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(ushort).FullName
			});
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x001092C4 File Offset: 0x001074C4
		internal static object DeserializeUInt32(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToUInt32(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(uint).FullName
			});
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x0010933C File Offset: 0x0010753C
		internal static object DeserializeUInt64(InternalDeserializer deserializer)
		{
			Exception innerException = null;
			try
			{
				return XmlConvert.ToUInt64(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException ex)
			{
				innerException = ex;
			}
			catch (OverflowException ex2)
			{
				innerException = ex2;
			}
			throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
			{
				typeof(ulong).FullName
			});
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x001093B4 File Offset: 0x001075B4
		internal static object DeserializeUri(InternalDeserializer deserializer)
		{
			object result;
			try
			{
				string uriString = deserializer.ReadDecodedElementString("URI");
				result = new Uri(uriString, UriKind.RelativeOrAbsolute);
			}
			catch (UriFormatException innerException)
			{
				throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
				{
					typeof(Uri).FullName
				});
			}
			return result;
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x00109410 File Offset: 0x00107610
		internal static object DeserializeByteArray(InternalDeserializer deserializer)
		{
			object result;
			try
			{
				result = Convert.FromBase64String(deserializer._reader.ReadElementContentAsString());
			}
			catch (FormatException innerException)
			{
				throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
				{
					typeof(byte[]).FullName
				});
			}
			return result;
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x0010946C File Offset: 0x0010766C
		internal static XmlDocument LoadUnsafeXmlDocument(FileInfo xmlPath, bool preserveNonElements, int? maxCharactersInDocument)
		{
			XmlDocument result = null;
			using (Stream stream = new FileStream(xmlPath.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				result = InternalDeserializer.LoadUnsafeXmlDocument(stream, preserveNonElements, maxCharactersInDocument);
			}
			return result;
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x001094B0 File Offset: 0x001076B0
		internal static XmlDocument LoadUnsafeXmlDocument(string xmlContents, bool preserveNonElements, int? maxCharactersInDocument)
		{
			XmlDocument result;
			using (TextReader textReader = new StringReader(xmlContents))
			{
				result = InternalDeserializer.LoadUnsafeXmlDocument(textReader, preserveNonElements, maxCharactersInDocument);
			}
			return result;
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x001094EC File Offset: 0x001076EC
		internal static XmlDocument LoadUnsafeXmlDocument(Stream stream, bool preserveNonElements, int? maxCharactersInDocument)
		{
			XmlDocument result;
			using (TextReader textReader = new StreamReader(stream))
			{
				result = InternalDeserializer.LoadUnsafeXmlDocument(textReader, preserveNonElements, maxCharactersInDocument);
			}
			return result;
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x00109528 File Offset: 0x00107728
		internal static XmlDocument LoadUnsafeXmlDocument(TextReader textReader, bool preserveNonElements, int? maxCharactersInDocument)
		{
			XmlReaderSettings xmlReaderSettings;
			if (maxCharactersInDocument != null || preserveNonElements)
			{
				xmlReaderSettings = InternalDeserializer.XmlReaderSettingsForUntrustedXmlDocument.Clone();
				if (maxCharactersInDocument != null)
				{
					xmlReaderSettings.MaxCharactersInDocument = (long)maxCharactersInDocument.Value;
				}
				if (preserveNonElements)
				{
					xmlReaderSettings.IgnoreWhitespace = false;
					xmlReaderSettings.IgnoreProcessingInstructions = false;
					xmlReaderSettings.IgnoreComments = false;
				}
			}
			else
			{
				xmlReaderSettings = InternalDeserializer.XmlReaderSettingsForUntrustedXmlDocument;
			}
			XmlDocument result;
			try
			{
				XmlReader reader = XmlReader.Create(textReader, xmlReaderSettings);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.PreserveWhitespace = preserveNonElements;
				xmlDocument.Load(reader);
				result = xmlDocument;
			}
			catch (InvalidOperationException ex)
			{
				throw new XmlException(ex.Message, ex);
			}
			return result;
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x001095C4 File Offset: 0x001077C4
		internal static object DeserializeXmlDocument(InternalDeserializer deserializer)
		{
			string text = deserializer.ReadDecodedElementString("XD");
			object result;
			try
			{
				int? maxCharactersInDocument = null;
				if (deserializer._context.MaximumAllowedMemory != null)
				{
					maxCharactersInDocument = new int?(deserializer._context.MaximumAllowedMemory.Value / 2);
				}
				XmlDocument xmlDocument = InternalDeserializer.LoadUnsafeXmlDocument(text, true, maxCharactersInDocument);
				deserializer._context.LogExtraMemoryUsage((text.Length - xmlDocument.OuterXml.Length) * 2);
				result = xmlDocument;
			}
			catch (XmlException innerException)
			{
				throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
				{
					typeof(XmlDocument).FullName
				});
			}
			return result;
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x00109684 File Offset: 0x00107884
		internal static object DeserializeProgressRecord(InternalDeserializer deserializer)
		{
			deserializer.ReadStartElement("PR");
			string activity = null;
			string text = null;
			string value = null;
			string statusDescription = null;
			int activityId = 0;
			int parentActivityId = 0;
			int percentComplete = 0;
			int secondsRemaining = 0;
			Exception ex = null;
			try
			{
				activity = deserializer.ReadDecodedElementString("AV");
				activityId = int.Parse(deserializer.ReadDecodedElementString("AI"), CultureInfo.InvariantCulture);
				object obj = deserializer.ReadOneObject();
				text = ((obj == null) ? null : obj.ToString());
				parentActivityId = int.Parse(deserializer.ReadDecodedElementString("PI"), CultureInfo.InvariantCulture);
				percentComplete = int.Parse(deserializer.ReadDecodedElementString("PC"), CultureInfo.InvariantCulture);
				value = deserializer.ReadDecodedElementString("T");
				secondsRemaining = int.Parse(deserializer.ReadDecodedElementString("SR"), CultureInfo.InvariantCulture);
				statusDescription = deserializer.ReadDecodedElementString("SD");
			}
			catch (FormatException ex2)
			{
				ex = ex2;
			}
			catch (OverflowException ex3)
			{
				ex = ex3;
			}
			if (ex != null)
			{
				throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, ex, new object[]
				{
					typeof(ulong).FullName
				});
			}
			deserializer.ReadEndElement();
			ProgressRecordType recordType;
			try
			{
				recordType = (ProgressRecordType)Enum.Parse(typeof(ProgressRecordType), value, true);
			}
			catch (ArgumentException innerException)
			{
				throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException, new object[]
				{
					typeof(ProgressRecord).FullName
				});
			}
			object result;
			try
			{
				ProgressRecord progressRecord = new ProgressRecord(activityId, activity, statusDescription);
				if (!string.IsNullOrEmpty(text))
				{
					progressRecord.CurrentOperation = text;
				}
				progressRecord.ParentActivityId = parentActivityId;
				progressRecord.PercentComplete = percentComplete;
				progressRecord.RecordType = recordType;
				progressRecord.SecondsRemaining = secondsRemaining;
				result = progressRecord;
			}
			catch (ArgumentException innerException2)
			{
				throw deserializer.NewXmlException(Serialization.InvalidPrimitiveType, innerException2, new object[]
				{
					typeof(ProgressRecord).FullName
				});
			}
			return result;
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x00109880 File Offset: 0x00107A80
		internal static object DeserializeSecureString(InternalDeserializer deserializer)
		{
			return deserializer.ReadSecureString();
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x00109888 File Offset: 0x00107A88
		private bool IsNextElement(string tag)
		{
			return this._reader.LocalName == tag && ((this._context.options & DeserializationOptions.NoNamespace) != DeserializationOptions.None || this._reader.NamespaceURI == "http://schemas.microsoft.com/powershell/2004/04");
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x001098D4 File Offset: 0x00107AD4
		internal bool ReadStartElementAndHandleEmpty(string element)
		{
			bool flag = this._reader.IsEmptyElement;
			this.ReadStartElement(element);
			if (!flag && this._reader.NodeType == XmlNodeType.EndElement)
			{
				this.ReadEndElement();
				flag = true;
			}
			return !flag;
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x00109914 File Offset: 0x00107B14
		private void ReadStartElement(string element)
		{
			if (DeserializationOptions.NoNamespace == (this._context.options & DeserializationOptions.NoNamespace))
			{
				this._reader.ReadStartElement(element);
			}
			else
			{
				this._reader.ReadStartElement(element, "http://schemas.microsoft.com/powershell/2004/04");
			}
			this._reader.MoveToContent();
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x00109964 File Offset: 0x00107B64
		private void ReadEndElement()
		{
			this._reader.ReadEndElement();
			this._reader.MoveToContent();
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x00109980 File Offset: 0x00107B80
		private string ReadDecodedElementString(string element)
		{
			this.CheckIfStopping();
			string s;
			if (DeserializationOptions.NoNamespace == (this._context.options & DeserializationOptions.NoNamespace))
			{
				s = this._reader.ReadElementContentAsString(element, string.Empty);
			}
			else
			{
				s = this._reader.ReadElementContentAsString(element, "http://schemas.microsoft.com/powershell/2004/04");
			}
			this._reader.MoveToContent();
			return InternalDeserializer.DecodeString(s);
		}

		// Token: 0x06003084 RID: 12420 RVA: 0x001099E7 File Offset: 0x00107BE7
		private void Skip()
		{
			this._reader.Skip();
			this._reader.MoveToContent();
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x00109A00 File Offset: 0x00107C00
		private object ReadPrimaryKnownType(TypeSerializationInfo pktInfo)
		{
			object result = pktInfo.Deserializer(this);
			this._reader.MoveToContent();
			return result;
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x00109A28 File Offset: 0x00107C28
		private object ReadSecureString()
		{
			string text = this._reader.ReadElementContentAsString();
			object result;
			try
			{
				object obj;
				if (this._context.cryptoHelper != null)
				{
					obj = this._context.cryptoHelper.DecryptSecureString(text);
				}
				else
				{
					obj = SecureStringHelper.Unprotect(text);
				}
				this._reader.MoveToContent();
				result = obj;
			}
			catch (PSCryptoException)
			{
				throw this.NewXmlException(Serialization.DeserializeSecureStringFailed, null, new object[0]);
			}
			return result;
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x00109AA0 File Offset: 0x00107CA0
		private XmlException NewXmlException(string resourceString, Exception innerException, params object[] args)
		{
			string message = StringUtil.Format(resourceString, args);
			XmlException ex = null;
			IXmlLineInfo xmlLineInfo = this._reader as IXmlLineInfo;
			if (xmlLineInfo != null && xmlLineInfo.HasLineInfo())
			{
				ex = new XmlException(message, innerException, xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
			}
			if (ex == null)
			{
				ex = new XmlException(message, innerException);
			}
			return ex;
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x00109AF0 File Offset: 0x00107CF0
		private string ReadNameAttribute()
		{
			string attribute = this._reader.GetAttribute("N");
			if (attribute == null)
			{
				throw this.NewXmlException(Serialization.AttributeExpected, null, new object[]
				{
					"N"
				});
			}
			return InternalDeserializer.DecodeString(attribute);
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x00109B34 File Offset: 0x00107D34
		private static string DecodeString(string s)
		{
			return XmlConvert.DecodeName(s);
		}

		// Token: 0x04001A11 RID: 6673
		private const int MaxDepthBelowTopLevel = 50;

		// Token: 0x04001A12 RID: 6674
		internal const string CimInstanceMetadataProperty = "__InstanceMetadata";

		// Token: 0x04001A13 RID: 6675
		internal const string CimModifiedProperties = "Modified";

		// Token: 0x04001A14 RID: 6676
		internal const string CimClassMetadataProperty = "__ClassMetadata";

		// Token: 0x04001A15 RID: 6677
		internal const string CimClassNameProperty = "ClassName";

		// Token: 0x04001A16 RID: 6678
		internal const string CimNamespaceProperty = "Namespace";

		// Token: 0x04001A17 RID: 6679
		internal const string CimServerNameProperty = "ServerName";

		// Token: 0x04001A18 RID: 6680
		internal const string CimHashCodeProperty = "Hash";

		// Token: 0x04001A19 RID: 6681
		internal const string CimMiXmlProperty = "MiXml";

		// Token: 0x04001A1A RID: 6682
		private readonly XmlReader _reader;

		// Token: 0x04001A1B RID: 6683
		private readonly DeserializationContext _context;

		// Token: 0x04001A1C RID: 6684
		private TypeTable _typeTable;

		// Token: 0x04001A1D RID: 6685
		private int depthBelowTopLevel;

		// Token: 0x04001A1E RID: 6686
		private Version _version;

		// Token: 0x04001A1F RID: 6687
		private readonly ReferenceIdHandlerForDeserializer<object> objectRefIdHandler;

		// Token: 0x04001A20 RID: 6688
		private readonly ReferenceIdHandlerForDeserializer<ConsolidatedString> typeRefIdHandler;

		// Token: 0x04001A21 RID: 6689
		private bool isStopping;

		// Token: 0x04001A22 RID: 6690
		private static Lazy<CimDeserializer> cimDeserializer = new Lazy<CimDeserializer>(new Func<CimDeserializer>(CimDeserializer.Create));

		// Token: 0x04001A23 RID: 6691
		private static readonly XmlReaderSettings xmlReaderSettingsForCliXml = InternalDeserializer.GetXmlReaderSettingsForCliXml();

		// Token: 0x04001A24 RID: 6692
		private static readonly XmlReaderSettings xmlReaderSettingsForUntrustedXmlDocument = InternalDeserializer.GetXmlReaderSettingsForUntrustedXmlDocument();

		// Token: 0x04001A25 RID: 6693
		[TraceSource("InternalDeserializer", "InternalDeserializer class")]
		private static readonly PSTraceSource _trace = PSTraceSource.GetTracer("InternalDeserializer", "InternalDeserializer class");
	}
}
