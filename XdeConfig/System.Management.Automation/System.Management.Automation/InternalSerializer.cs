using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Security;
using System.Text;
using System.Xml;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Serialization;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000451 RID: 1105
	internal class InternalSerializer
	{
		// Token: 0x06003003 RID: 12291 RVA: 0x00105CF8 File Offset: 0x00103EF8
		internal InternalSerializer(XmlWriter writer, SerializationContext context)
		{
			this._writer = writer;
			this._context = context;
			IDictionary<object, ulong> dictionary = null;
			if ((this._context.options & SerializationOptions.NoObjectRefIds) == SerializationOptions.None)
			{
				dictionary = new WeakReferenceDictionary<ulong>();
			}
			this.objectRefIdHandler = new ReferenceIdHandlerForSerializer<object>(dictionary);
			this.typeRefIdHandler = new ReferenceIdHandlerForSerializer<ConsolidatedString>(new Dictionary<ConsolidatedString, ulong>(new InternalSerializer.ConsolidatedStringEqualityComparer()));
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06003004 RID: 12292 RVA: 0x00105D51 File Offset: 0x00103F51
		// (set) Token: 0x06003005 RID: 12293 RVA: 0x00105D59 File Offset: 0x00103F59
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

		// Token: 0x06003006 RID: 12294 RVA: 0x00105D62 File Offset: 0x00103F62
		internal void Start()
		{
			if (SerializationOptions.NoRootElement != (this._context.options & SerializationOptions.NoRootElement))
			{
				this.WriteStartElement("Objs");
				this.WriteAttribute("Version", "1.1.0.1");
			}
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x00105D8F File Offset: 0x00103F8F
		internal void End()
		{
			if (SerializationOptions.NoRootElement != (this._context.options & SerializationOptions.NoRootElement))
			{
				this._writer.WriteEndElement();
			}
			this._writer.Flush();
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x00105DB7 File Offset: 0x00103FB7
		internal void Stop()
		{
			this.isStopping = true;
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x00105DC0 File Offset: 0x00103FC0
		private void CheckIfStopping()
		{
			if (this.isStopping)
			{
				throw PSTraceSource.NewInvalidOperationException(Serialization.Stopping, new object[0]);
			}
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x00105DDC File Offset: 0x00103FDC
		internal static bool IsPrimitiveKnownType(Type input)
		{
			TypeSerializationInfo typeSerializationInfo = KnownTypes.GetTypeSerializationInfo(input);
			return typeSerializationInfo != null;
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x00105DF7 File Offset: 0x00103FF7
		internal void WriteOneTopLevelObject(object source, string streamName)
		{
			this.WriteOneObject(source, streamName, null, this._context.depth);
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x00105E10 File Offset: 0x00104010
		private void WriteOneObject(object source, string streamName, string property, int depth)
		{
			this.CheckIfStopping();
			if (source == null)
			{
				this.WriteNull(streamName, property);
				return;
			}
			try
			{
				this.depthBelowTopLevel++;
				if (!this.HandleMaxDepth(source, streamName, property))
				{
					depth = this.GetDepthOfSerialization(source, depth);
					if (!this.HandlePrimitiveKnownTypeByConvertingToPSObject(source, streamName, property, depth))
					{
						string refId = this.objectRefIdHandler.GetRefId(source);
						if (refId != null)
						{
							this.WritePSObjectReference(streamName, property, refId);
						}
						else if (!this.HandlePrimitiveKnownTypePSObject(source, streamName, property, depth))
						{
							if (!this.HandleKnownContainerTypes(source, streamName, property, depth))
							{
								PSObject source2 = PSObject.AsPSObject(source);
								if (depth == 0 || this.SerializeAsString(source2))
								{
									this.HandlePSObjectAsString(source2, streamName, property, depth);
								}
								else
								{
									this.HandleComplexTypePSObject(source, streamName, property, depth);
								}
							}
						}
					}
				}
			}
			finally
			{
				this.depthBelowTopLevel--;
			}
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x00105EF0 File Offset: 0x001040F0
		private bool HandleMaxDepth(object source, string streamName, string property)
		{
			if (this.depthBelowTopLevel == 50)
			{
				PSEtwLog.LogAnalyticError(PSEventId.Serializer_MaxDepthWhenSerializing, PSOpcode.Exception, PSTask.Serialization, PSKeyword.Serializer, new object[]
				{
					source.GetType().AssemblyQualifiedName,
					property,
					this.depthBelowTopLevel
				});
				string deserializationTooDeep = Serialization.DeserializationTooDeep;
				this.HandlePrimitiveKnownType(deserializationTooDeep, streamName, property);
				return true;
			}
			return false;
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x00105F54 File Offset: 0x00104154
		private bool HandlePrimitiveKnownType(object source, string streamName, string property)
		{
			TypeSerializationInfo typeSerializationInfo = KnownTypes.GetTypeSerializationInfo(source.GetType());
			if (typeSerializationInfo != null)
			{
				InternalSerializer.WriteOnePrimitiveKnownType(this, streamName, property, source, typeSerializationInfo);
				return true;
			}
			return false;
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x00105F80 File Offset: 0x00104180
		private bool HandlePrimitiveKnownTypeByConvertingToPSObject(object source, string streamName, string property, int depth)
		{
			TypeSerializationInfo typeSerializationInfo = KnownTypes.GetTypeSerializationInfo(source.GetType());
			if (typeSerializationInfo != null)
			{
				PSObject source2 = PSObject.AsPSObject(source);
				return this.HandlePrimitiveKnownTypePSObject(source2, streamName, property, depth);
			}
			return false;
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x00105FB0 File Offset: 0x001041B0
		private bool HandleSecureString(object source, string streamName, string property)
		{
			SecureString secureString = source as SecureString;
			PSObject psobject;
			if (secureString != null)
			{
				psobject = PSObject.AsPSObject(secureString);
			}
			else
			{
				psobject = (source as PSObject);
			}
			if (psobject != null && !psobject.immediateBaseObjectIsEmpty)
			{
				secureString = (psobject.ImmediateBaseObject as SecureString);
				if (secureString != null)
				{
					try
					{
						string text;
						if (this._context.cryptoHelper != null)
						{
							text = this._context.cryptoHelper.EncryptSecureString(secureString);
						}
						else
						{
							text = SecureStringHelper.Protect(secureString);
						}
						if (property != null)
						{
							this.WriteStartElement("SS");
							this.WriteNameAttribute(property);
						}
						else
						{
							this.WriteStartElement("SS");
						}
						if (streamName != null)
						{
							this.WriteAttribute("S", streamName);
						}
						this._writer.WriteString(text);
						this._writer.WriteEndElement();
						return true;
					}
					catch (PSCryptoException)
					{
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x00106084 File Offset: 0x00104284
		private bool HandlePrimitiveKnownTypePSObject(object source, string streamName, string property, int depth)
		{
			bool result = false;
			PSObject psobject = source as PSObject;
			if (psobject != null && !psobject.immediateBaseObjectIsEmpty)
			{
				object immediateBaseObject = psobject.ImmediateBaseObject;
				TypeSerializationInfo typeSerializationInfo = KnownTypes.GetTypeSerializationInfo(immediateBaseObject.GetType());
				if (typeSerializationInfo != null)
				{
					this.WritePrimitiveTypePSObject(psobject, immediateBaseObject, typeSerializationInfo, streamName, property, depth);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x001060CC File Offset: 0x001042CC
		private bool HandleKnownContainerTypes(object source, string streamName, string property, int depth)
		{
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
			string refId = this.objectRefIdHandler.SetRefId(source);
			this.WriteStartOfPSObject(psobject ?? PSObject.AsPSObject(source), streamName, property, refId, true, null);
			switch (containerType)
			{
			case ContainerType.Dictionary:
				this.WriteDictionary(dictionary, "DCT", depth);
				break;
			case ContainerType.Queue:
				this.WriteEnumerable(enumerable, "QUE", depth);
				break;
			case ContainerType.Stack:
				this.WriteEnumerable(enumerable, "STK", depth);
				break;
			case ContainerType.List:
				this.WriteEnumerable(enumerable, "LST", depth);
				break;
			case ContainerType.Enumerable:
				this.WriteEnumerable(enumerable, "IE", depth);
				break;
			}
			if (depth != 0)
			{
				if (containerType == ContainerType.Enumerable || (psobject != null && psobject.isDeserialized))
				{
					PSObject source2 = PSObject.AsPSObject(source);
					PSMemberInfoInternalCollection<PSPropertyInfo> specificPropertiesToSerialize = SerializationUtilities.GetSpecificPropertiesToSerialize(source2, this.AllPropertiesCollection, this._typeTable);
					this.WritePSObjectProperties(source2, depth, specificPropertiesToSerialize);
					this.SerializeExtendedProperties(source2, depth, specificPropertiesToSerialize);
				}
				else if (psobject != null)
				{
					this.SerializeInstanceProperties(psobject, depth);
				}
			}
			this._writer.WriteEndElement();
			return true;
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x00106205 File Offset: 0x00104405
		private void WritePSObjectReference(string streamName, string property, string refId)
		{
			this.WriteStartElement("Ref");
			if (streamName != null)
			{
				this.WriteAttribute("S", streamName);
			}
			if (property != null)
			{
				this.WriteNameAttribute(property);
			}
			this.WriteAttribute("RefId", refId);
			this._writer.WriteEndElement();
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x00106244 File Offset: 0x00104444
		private static bool PSObjectHasModifiedTypesCollection(PSObject pso)
		{
			ConsolidatedString internalTypeNames = pso.InternalTypeNames;
			Collection<string> collection = pso.InternalAdapter.BaseGetTypeNameHierarchy(pso.ImmediateBaseObject);
			if (internalTypeNames.Count != collection.Count)
			{
				return true;
			}
			IEnumerator<string> enumerator = internalTypeNames.GetEnumerator();
			IEnumerator<string> enumerator2 = collection.GetEnumerator();
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				if (!enumerator.Current.Equals(enumerator2.Current, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x001062B4 File Offset: 0x001044B4
		private void WritePrimitiveTypePSObject(PSObject source, object primitive, TypeSerializationInfo pktInfo, string streamName, string property, int depth)
		{
			string toStringForPrimitiveObject = SerializationUtilities.GetToStringForPrimitiveObject(source);
			bool flag = InternalSerializer.PSObjectHasModifiedTypesCollection(source);
			bool flag2 = InternalSerializer.PSObjectHasNotes(source);
			bool flag3 = toStringForPrimitiveObject != null;
			if (flag2 || flag || flag3)
			{
				this.WritePrimitiveTypePSObjectWithNotes(source, primitive, flag, toStringForPrimitiveObject, pktInfo, streamName, property, depth);
				return;
			}
			if (primitive != null)
			{
				InternalSerializer.WriteOnePrimitiveKnownType(this, streamName, property, primitive, pktInfo);
				return;
			}
			this.WriteNull(streamName, property);
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x00106314 File Offset: 0x00104514
		private void WritePrimitiveTypePSObjectWithNotes(PSObject source, object primitive, bool hasModifiedTypesCollection, string toStringValue, TypeSerializationInfo pktInfo, string streamName, string property, int depth)
		{
			string refId = this.objectRefIdHandler.SetRefId(source);
			this.WriteStartOfPSObject(source, streamName, property, refId, hasModifiedTypesCollection, toStringValue);
			if (pktInfo != null)
			{
				InternalSerializer.WriteOnePrimitiveKnownType(this, streamName, null, primitive, pktInfo);
			}
			this.SerializeInstanceProperties(source, depth);
			this._writer.WriteEndElement();
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x00106364 File Offset: 0x00104564
		private void HandleComplexTypePSObject(object source, string streamName, string property, int depth)
		{
			PSObject psobject = PSObject.AsPSObject(source);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			if (!psobject.immediateBaseObjectIsEmpty)
			{
				CimInstance cimInstance = psobject.ImmediateBaseObject as CimInstance;
				if (cimInstance != null)
				{
					flag5 = true;
				}
				else
				{
					ErrorRecord errorRecord = psobject.ImmediateBaseObject as ErrorRecord;
					if (errorRecord != null)
					{
						errorRecord.ToPSObjectForRemoting(psobject);
						flag = true;
					}
					else
					{
						InformationalRecord informationalRecord = psobject.ImmediateBaseObject as InformationalRecord;
						if (informationalRecord != null)
						{
							informationalRecord.ToPSObjectForRemoting(psobject);
							flag2 = true;
						}
						else
						{
							flag3 = (psobject.ImmediateBaseObject is Enum);
							flag4 = (psobject.ImmediateBaseObject is PSObject);
						}
					}
				}
			}
			bool flag6 = true;
			if (psobject.ToStringFromDeserialization == null && psobject.immediateBaseObjectIsEmpty)
			{
				flag6 = false;
			}
			string refId = this.objectRefIdHandler.SetRefId(source);
			this.WriteStartOfPSObject(psobject, streamName, property, refId, true, flag6 ? SerializationUtilities.GetToString(psobject) : null);
			PSMemberInfoInternalCollection<PSPropertyInfo> specificPropertiesToSerialize = SerializationUtilities.GetSpecificPropertiesToSerialize(psobject, this.AllPropertiesCollection, this._typeTable);
			if (flag3)
			{
				object immediateBaseObject = psobject.ImmediateBaseObject;
				this.WriteOneObject(Convert.ChangeType(immediateBaseObject, Enum.GetUnderlyingType(immediateBaseObject.GetType()), CultureInfo.InvariantCulture), null, null, depth);
			}
			else if (flag4)
			{
				this.WriteOneObject(psobject.ImmediateBaseObject, null, null, depth);
			}
			else if (!flag && !flag2)
			{
				this.WritePSObjectProperties(psobject, depth, specificPropertiesToSerialize);
			}
			if (flag5)
			{
				CimInstance cimInstance2 = psobject.ImmediateBaseObject as CimInstance;
				this.PrepareCimInstanceForSerialization(psobject, cimInstance2);
			}
			this.SerializeExtendedProperties(psobject, depth, specificPropertiesToSerialize);
			this._writer.WriteEndElement();
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x001064E4 File Offset: 0x001046E4
		private void PrepareCimInstanceForSerialization(PSObject psObject, CimInstance cimInstance)
		{
			Queue<CimClassSerializationId> queue = new Queue<CimClassSerializationId>();
			ArrayList arrayList = new ArrayList();
			for (CimClass cimClass = cimInstance.CimClass; cimClass != null; cimClass = cimClass.CimSuperClass)
			{
				PSObject psobject = new PSObject();
				psobject.TypeNames.Clear();
				arrayList.Add(psobject);
				psobject.Properties.Add(new PSNoteProperty("ClassName", cimClass.CimSystemProperties.ClassName));
				psobject.Properties.Add(new PSNoteProperty("Namespace", cimClass.CimSystemProperties.Namespace));
				psobject.Properties.Add(new PSNoteProperty("ServerName", cimClass.CimSystemProperties.ServerName));
				psobject.Properties.Add(new PSNoteProperty("Hash", cimClass.GetHashCode()));
				CimClassSerializationId cimClassSerializationId = new CimClassSerializationId(cimClass.CimSystemProperties.ClassName, cimClass.CimSystemProperties.Namespace, cimClass.CimSystemProperties.ServerName, cimClass.GetHashCode());
				if (this._context.cimClassSerializationIdCache.DoesDeserializerAlreadyHaveCimClass(cimClassSerializationId))
				{
					break;
				}
				queue.Enqueue(cimClassSerializationId);
				byte[] array = InternalSerializer.cimSerializer.Value.Serialize(cimClass, ClassSerializationOptions.None);
				string @string = Encoding.Unicode.GetString(array, 0, array.Length);
				psobject.Properties.Add(new PSNoteProperty("MiXml", @string));
			}
			arrayList.Reverse();
			foreach (CimClassSerializationId key in queue)
			{
				this._context.cimClassSerializationIdCache.AddClassToCache(key);
			}
			PSPropertyInfo pspropertyInfo = psObject.Properties["__ClassMetadata"];
			if (pspropertyInfo != null)
			{
				pspropertyInfo.Value = arrayList;
			}
			else
			{
				PSNoteProperty psnoteProperty = new PSNoteProperty("__ClassMetadata", arrayList);
				psnoteProperty.IsHidden = true;
				psObject.Properties.Add(psnoteProperty);
			}
			List<string> list = (from p in cimInstance.CimInstanceProperties
			where p.IsValueModified
			select p.Name).ToList<string>();
			if (list.Count != 0)
			{
				PSObject psobject2 = new PSObject();
				PSPropertyInfo pspropertyInfo2 = psObject.Properties["__InstanceMetadata"];
				if (pspropertyInfo2 != null)
				{
					pspropertyInfo2.Value = psobject2;
				}
				else
				{
					PSNoteProperty psnoteProperty2 = new PSNoteProperty("__InstanceMetadata", psobject2);
					psnoteProperty2.IsHidden = true;
					psObject.Properties.Add(psnoteProperty2);
				}
				psobject2.InternalTypeNames = ConsolidatedString.Empty;
				psobject2.Properties.Add(new PSNoteProperty("Modified", string.Join(" ", list)));
			}
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x001067A0 File Offset: 0x001049A0
		private void WriteStartOfPSObject(PSObject mshObject, string streamName, string property, string refId, bool writeTypeNames, string toStringValue)
		{
			this.WriteStartElement("Obj");
			if (streamName != null)
			{
				this.WriteAttribute("S", streamName);
			}
			if (property != null)
			{
				this.WriteNameAttribute(property);
			}
			if (refId != null)
			{
				this.WriteAttribute("RefId", refId);
			}
			if (writeTypeNames)
			{
				ConsolidatedString internalTypeNames = mshObject.InternalTypeNames;
				if (internalTypeNames.Count > 0)
				{
					string refId2 = this.typeRefIdHandler.GetRefId(internalTypeNames);
					if (refId2 == null)
					{
						this.WriteStartElement("TN");
						string value = this.typeRefIdHandler.SetRefId(internalTypeNames);
						this.WriteAttribute("RefId", value);
						foreach (string value2 in internalTypeNames)
						{
							this.WriteEncodedElementString("T", value2);
						}
						this._writer.WriteEndElement();
					}
					else
					{
						this.WriteStartElement("TNRef");
						this.WriteAttribute("RefId", refId2);
						this._writer.WriteEndElement();
					}
				}
			}
			if (toStringValue != null)
			{
				this.WriteEncodedElementString("ToString", toStringValue);
			}
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x001068B8 File Offset: 0x00104AB8
		private static bool PSObjectHasNotes(PSObject source)
		{
			return source.InstanceMembers != null && source.InstanceMembers.Count > 0;
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x001068D3 File Offset: 0x00104AD3
		private bool CanUseDefaultRunspaceInThreadSafeManner
		{
			get
			{
				if (this.canUseDefaultRunspaceInThreadSafeManner == null)
				{
					this.canUseDefaultRunspaceInThreadSafeManner = new bool?(Runspace.CanUseDefaultRunspace);
				}
				return this.canUseDefaultRunspaceInThreadSafeManner.Value;
			}
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x00106900 File Offset: 0x00104B00
		private void WriteMemberInfoCollection(IEnumerable<PSMemberInfo> me, int depth, bool writeEnclosingMemberSetElementTag)
		{
			bool flag = false;
			foreach (PSMemberInfo psmemberInfo in me)
			{
				if (psmemberInfo.ShouldSerialize)
				{
					int depth2 = psmemberInfo.IsInstance ? depth : (depth - 1);
					if (psmemberInfo.MemberType == (psmemberInfo.MemberType & PSMemberTypes.Properties))
					{
						bool flag2;
						object propertyValueInThreadSafeManner = SerializationUtilities.GetPropertyValueInThreadSafeManner((PSPropertyInfo)psmemberInfo, this.CanUseDefaultRunspaceInThreadSafeManner, out flag2);
						if (flag2)
						{
							if (writeEnclosingMemberSetElementTag && !flag)
							{
								flag = true;
								this.WriteStartElement("MS");
							}
							this.WriteOneObject(propertyValueInThreadSafeManner, null, psmemberInfo.Name, depth2);
						}
					}
					else if (psmemberInfo.MemberType == PSMemberTypes.MemberSet)
					{
						if (writeEnclosingMemberSetElementTag && !flag)
						{
							flag = true;
							this.WriteStartElement("MS");
						}
						this.WriteMemberSet((PSMemberSet)psmemberInfo, depth2);
					}
				}
			}
			if (flag)
			{
				this._writer.WriteEndElement();
			}
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x001069F4 File Offset: 0x00104BF4
		private void WriteMemberSet(PSMemberSet set, int depth)
		{
			if (!set.ShouldSerialize)
			{
				return;
			}
			this.WriteStartElement("MS");
			this.WriteNameAttribute(set.Name);
			this.WriteMemberInfoCollection(set.Members, depth, false);
			this._writer.WriteEndElement();
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x00106A30 File Offset: 0x00104C30
		private void WritePSObjectProperties(PSObject source, int depth, IEnumerable<PSPropertyInfo> specificPropertiesToSerialize)
		{
			depth--;
			if (specificPropertiesToSerialize != null)
			{
				this.SerializeProperties(specificPropertiesToSerialize, "Props", depth);
				return;
			}
			if (source.ShouldSerializeAdapter())
			{
				IEnumerable<PSPropertyInfo> adaptedProperties = source.GetAdaptedProperties();
				if (adaptedProperties != null)
				{
					this.SerializeProperties(adaptedProperties, "Props", depth);
				}
			}
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x00106A74 File Offset: 0x00104C74
		private void SerializeInstanceProperties(PSObject source, int depth)
		{
			PSMemberInfoCollection<PSMemberInfo> instanceMembers = source.InstanceMembers;
			if (instanceMembers != null)
			{
				this.WriteMemberInfoCollection(instanceMembers, depth, true);
			}
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06003020 RID: 12320 RVA: 0x00106A94 File Offset: 0x00104C94
		private Collection<CollectionEntry<PSMemberInfo>> ExtendedMembersCollection
		{
			get
			{
				if (this.extendedMembersCollection == null)
				{
					this.extendedMembersCollection = PSObject.GetMemberCollection(PSMemberViewTypes.Extended, this._typeTable);
				}
				return this.extendedMembersCollection;
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x00106AB6 File Offset: 0x00104CB6
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

		// Token: 0x06003022 RID: 12322 RVA: 0x00106AD8 File Offset: 0x00104CD8
		private void SerializeExtendedProperties(PSObject source, int depth, IEnumerable<PSPropertyInfo> specificPropertiesToSerialize)
		{
			IEnumerable<PSMemberInfo> enumerable = null;
			if (specificPropertiesToSerialize == null)
			{
				PSMemberInfoIntegratingCollection<PSMemberInfo> psmemberInfoIntegratingCollection = new PSMemberInfoIntegratingCollection<PSMemberInfo>(source, this.ExtendedMembersCollection);
				enumerable = psmemberInfoIntegratingCollection.Match("*", PSMemberTypes.AliasProperty | PSMemberTypes.CodeProperty | PSMemberTypes.Property | PSMemberTypes.NoteProperty | PSMemberTypes.ScriptProperty | PSMemberTypes.PropertySet | PSMemberTypes.MemberSet, MshMemberMatchOptions.IncludeHidden | MshMemberMatchOptions.OnlySerializable);
			}
			else
			{
				List<PSMemberInfo> list = new List<PSMemberInfo>(source.InstanceMembers);
				enumerable = list;
				foreach (PSMemberInfo psmemberInfo in specificPropertiesToSerialize)
				{
					if (!psmemberInfo.IsInstance && !(psmemberInfo is PSProperty))
					{
						list.Add(psmemberInfo);
					}
				}
			}
			if (enumerable != null)
			{
				this.WriteMemberInfoCollection(enumerable, depth, true);
			}
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x00106B78 File Offset: 0x00104D78
		private void SerializeProperties(IEnumerable<PSPropertyInfo> propertyCollection, string name, int depth)
		{
			bool flag = false;
			foreach (PSMemberInfo psmemberInfo in propertyCollection)
			{
				PSProperty psproperty = psmemberInfo as PSProperty;
				if (psproperty != null)
				{
					if (!flag)
					{
						this.WriteStartElement(name);
						flag = true;
					}
					bool flag2;
					object propertyValueInThreadSafeManner = SerializationUtilities.GetPropertyValueInThreadSafeManner(psproperty, this.CanUseDefaultRunspaceInThreadSafeManner, out flag2);
					if (flag2)
					{
						this.WriteOneObject(propertyValueInThreadSafeManner, null, psproperty.Name, depth);
					}
				}
			}
			if (flag)
			{
				this._writer.WriteEndElement();
			}
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x00106C08 File Offset: 0x00104E08
		private void WriteEnumerable(IEnumerable enumerable, string tag, int depth)
		{
			this.WriteStartElement(tag);
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
					this.WriteOneObject(source, null, null, depth);
				}
			}
			this._writer.WriteEndElement();
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x00106D10 File Offset: 0x00104F10
		private void WriteDictionary(IDictionary dictionary, string tag, int depth)
		{
			this.WriteStartElement(tag);
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
					object source = null;
					try
					{
						if (!dictionaryEnumerator.MoveNext())
						{
							break;
						}
						obj = dictionaryEnumerator.Key;
						source = dictionaryEnumerator.Value;
					}
					catch (Exception ex2)
					{
						CommandProcessorBase.CheckForSevereException(ex2);
						PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_EnumerationFailed, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
						{
							dictionary.GetType().AssemblyQualifiedName,
							ex2.ToString()
						});
						break;
					}
					if (obj == null)
					{
						break;
					}
					this.WriteStartElement("En");
					this.WriteOneObject(obj, null, "Key", depth);
					this.WriteOneObject(source, null, "Value", depth);
					this._writer.WriteEndElement();
				}
			}
			this._writer.WriteEndElement();
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x00106E40 File Offset: 0x00105040
		private void HandlePSObjectAsString(PSObject source, string streamName, string property, int depth)
		{
			string serializationString = this.GetSerializationString(source);
			TypeSerializationInfo pktInfo = null;
			if (serializationString != null)
			{
				pktInfo = KnownTypes.GetTypeSerializationInfo(serializationString.GetType());
			}
			this.WritePrimitiveTypePSObject(source, serializationString, pktInfo, streamName, property, depth);
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x00106E74 File Offset: 0x00105074
		private string GetSerializationString(PSObject source)
		{
			PSPropertyInfo pspropertyInfo = null;
			try
			{
				pspropertyInfo = source.GetStringSerializationSource(this._typeTable);
			}
			catch (ExtendedTypeSystemException ex)
			{
				PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_ToStringFailed, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
				{
					source.GetType().AssemblyQualifiedName,
					(ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString()
				});
			}
			string result = null;
			if (pspropertyInfo != null)
			{
				bool flag;
				object propertyValueInThreadSafeManner = SerializationUtilities.GetPropertyValueInThreadSafeManner(pspropertyInfo, this.CanUseDefaultRunspaceInThreadSafeManner, out flag);
				if (flag && propertyValueInThreadSafeManner != null)
				{
					result = SerializationUtilities.GetToString(propertyValueInThreadSafeManner);
				}
			}
			else
			{
				result = SerializationUtilities.GetToString(source);
			}
			return result;
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x00106F24 File Offset: 0x00105124
		private bool SerializeAsString(PSObject source)
		{
			SerializationMethod serializationMethod = source.GetSerializationMethod(this._typeTable);
			if (serializationMethod == SerializationMethod.String)
			{
				PSEtwLog.LogAnalyticVerbose(PSEventId.Serializer_ModeOverride, PSOpcode.SerializationSettings, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
				{
					source.InternalTypeNames.Key,
					1U
				});
				return true;
			}
			return false;
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x00106F7C File Offset: 0x0010517C
		private int GetDepthOfSerialization(object source, int depth)
		{
			PSObject psobject = PSObject.AsPSObject(source);
			if (psobject == null)
			{
				return depth;
			}
			if (psobject.BaseObject is CimInstance)
			{
				return 1;
			}
			if (psobject.BaseObject is PSCredential)
			{
				return 1;
			}
			if (psobject.BaseObject is PSSenderInfo)
			{
				return 4;
			}
			if (psobject.BaseObject is SwitchParameter)
			{
				return 1;
			}
			if ((this._context.options & SerializationOptions.UseDepthFromTypes) != SerializationOptions.None)
			{
				int serializationDepth = psobject.GetSerializationDepth(this._typeTable);
				if (serializationDepth > 0 && serializationDepth != depth)
				{
					PSEtwLog.LogAnalyticVerbose(PSEventId.Serializer_DepthOverride, PSOpcode.SerializationSettings, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
					{
						psobject.InternalTypeNames.Key,
						depth,
						serializationDepth,
						this.depthBelowTopLevel
					});
					return serializationDepth;
				}
			}
			if ((this._context.options & SerializationOptions.PreserveSerializationSettingOfOriginal) != SerializationOptions.None && psobject.isDeserialized && depth <= 0)
			{
				return 1;
			}
			return depth;
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x00107062 File Offset: 0x00105262
		private void WriteNull(string streamName, string property)
		{
			this.WriteStartElement("Nil");
			if (streamName != null)
			{
				this.WriteAttribute("S", streamName);
			}
			if (property != null)
			{
				this.WriteNameAttribute(property);
			}
			this._writer.WriteEndElement();
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x00107094 File Offset: 0x00105294
		private static void WriteRawString(InternalSerializer serializer, string streamName, string property, string raw, TypeSerializationInfo entry)
		{
			if (property != null)
			{
				serializer.WriteStartElement(entry.PropertyTag);
				serializer.WriteNameAttribute(property);
			}
			else
			{
				serializer.WriteStartElement(entry.ItemTag);
			}
			if (streamName != null)
			{
				serializer.WriteAttribute("S", streamName);
			}
			serializer._writer.WriteRaw(raw);
			serializer._writer.WriteEndElement();
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x001070F0 File Offset: 0x001052F0
		private static void WriteOnePrimitiveKnownType(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			if (entry.Serializer == null)
			{
				string raw = Convert.ToString(source, CultureInfo.InvariantCulture);
				InternalSerializer.WriteRawString(serializer, streamName, property, raw, entry);
				return;
			}
			entry.Serializer(serializer, streamName, property, source, entry);
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x00107130 File Offset: 0x00105330
		internal static void WriteDateTime(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			InternalSerializer.WriteRawString(serializer, streamName, property, XmlConvert.ToString((DateTime)source, XmlDateTimeSerializationMode.RoundtripKind), entry);
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x00107148 File Offset: 0x00105348
		internal static void WriteVersion(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			InternalSerializer.WriteRawString(serializer, streamName, property, Convert.ToString(source, CultureInfo.InvariantCulture), entry);
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x0010715F File Offset: 0x0010535F
		internal static void WriteScriptBlock(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			InternalSerializer.WriteEncodedString(serializer, streamName, property, Convert.ToString(source, CultureInfo.InvariantCulture), entry);
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x00107176 File Offset: 0x00105376
		internal static void WriteUri(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			InternalSerializer.WriteEncodedString(serializer, streamName, property, Convert.ToString(source, CultureInfo.InvariantCulture), entry);
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x00107190 File Offset: 0x00105390
		internal static void WriteEncodedString(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			if (property != null)
			{
				serializer.WriteStartElement(entry.PropertyTag);
				serializer.WriteNameAttribute(property);
			}
			else
			{
				serializer.WriteStartElement(entry.ItemTag);
			}
			if (streamName != null)
			{
				serializer.WriteAttribute("S", streamName);
			}
			string s = (string)source;
			string text = InternalSerializer.EncodeString(s);
			serializer._writer.WriteString(text);
			serializer._writer.WriteEndElement();
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x001071F7 File Offset: 0x001053F7
		internal static void WriteDouble(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			InternalSerializer.WriteRawString(serializer, streamName, property, XmlConvert.ToString((double)source), entry);
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x0010720E File Offset: 0x0010540E
		internal static void WriteChar(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			InternalSerializer.WriteRawString(serializer, streamName, property, XmlConvert.ToString((ushort)((char)source)), entry);
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x00107225 File Offset: 0x00105425
		internal static void WriteBoolean(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			InternalSerializer.WriteRawString(serializer, streamName, property, XmlConvert.ToString((bool)source), entry);
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x0010723C File Offset: 0x0010543C
		internal static void WriteSingle(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			InternalSerializer.WriteRawString(serializer, streamName, property, XmlConvert.ToString((float)source), entry);
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x00107253 File Offset: 0x00105453
		internal static void WriteTimeSpan(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			InternalSerializer.WriteRawString(serializer, streamName, property, XmlConvert.ToString((TimeSpan)source), entry);
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x0010726C File Offset: 0x0010546C
		internal static void WriteByteArray(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			byte[] array = (byte[])source;
			if (property != null)
			{
				serializer.WriteStartElement(entry.PropertyTag);
				serializer.WriteNameAttribute(property);
			}
			else
			{
				serializer.WriteStartElement(entry.ItemTag);
			}
			if (streamName != null)
			{
				serializer.WriteAttribute("S", streamName);
			}
			serializer._writer.WriteBase64(array, 0, array.Length);
			serializer._writer.WriteEndElement();
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x001072D0 File Offset: 0x001054D0
		internal static void WriteXmlDocument(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			string outerXml = ((XmlDocument)source).OuterXml;
			InternalSerializer.WriteEncodedString(serializer, streamName, property, outerXml, entry);
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x001072F4 File Offset: 0x001054F4
		internal static void WriteProgressRecord(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			ProgressRecord progressRecord = (ProgressRecord)source;
			serializer.WriteStartElement(entry.PropertyTag);
			if (property != null)
			{
				serializer.WriteNameAttribute(property);
			}
			if (streamName != null)
			{
				serializer.WriteAttribute("S", streamName);
			}
			serializer.WriteEncodedElementString("AV", progressRecord.Activity);
			serializer.WriteEncodedElementString("AI", progressRecord.ActivityId.ToString(CultureInfo.InvariantCulture));
			serializer.WriteOneObject(progressRecord.CurrentOperation, null, null, 1);
			serializer.WriteEncodedElementString("PI", progressRecord.ParentActivityId.ToString(CultureInfo.InvariantCulture));
			serializer.WriteEncodedElementString("PC", progressRecord.PercentComplete.ToString(CultureInfo.InvariantCulture));
			serializer.WriteEncodedElementString("T", progressRecord.RecordType.ToString());
			serializer.WriteEncodedElementString("SR", progressRecord.SecondsRemaining.ToString(CultureInfo.InvariantCulture));
			serializer.WriteEncodedElementString("SD", progressRecord.StatusDescription);
			serializer._writer.WriteEndElement();
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x001073FE File Offset: 0x001055FE
		internal static void WriteSecureString(InternalSerializer serializer, string streamName, string property, object source, TypeSerializationInfo entry)
		{
			serializer.HandleSecureString(source, streamName, property);
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x0010740A File Offset: 0x0010560A
		private void WriteStartElement(string elementTag)
		{
			if (SerializationOptions.NoNamespace == (this._context.options & SerializationOptions.NoNamespace))
			{
				this._writer.WriteStartElement(elementTag);
				return;
			}
			this._writer.WriteStartElement(elementTag, "http://schemas.microsoft.com/powershell/2004/04");
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x0010743A File Offset: 0x0010563A
		private void WriteAttribute(string name, string value)
		{
			this._writer.WriteAttributeString(name, value);
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x00107449 File Offset: 0x00105649
		private void WriteNameAttribute(string value)
		{
			this.WriteAttribute("N", InternalSerializer.EncodeString(value));
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x0010745C File Offset: 0x0010565C
		internal static string EncodeString(string s)
		{
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if (c <= '\u001f' || (c >= '\u007f' && c <= '\u009f') || (c >= '\ud800' && c <= '\udfff') || (c == '_' && i + 1 < length && (s[i + 1] == 'x' || s[i + 1] == 'X')))
				{
					return InternalSerializer.EncodeString(s, i);
				}
			}
			return s;
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x001074D4 File Offset: 0x001056D4
		private static string EncodeString(string s, int indexOfFirstEncodableCharacter)
		{
			int length = s.Length;
			char[] array = new char[indexOfFirstEncodableCharacter + (length - indexOfFirstEncodableCharacter) * 7];
			s.CopyTo(0, array, 0, indexOfFirstEncodableCharacter);
			int num = indexOfFirstEncodableCharacter;
			for (int i = indexOfFirstEncodableCharacter; i < length; i++)
			{
				char c = s[i];
				if (c > '\u001f' && (c < '\u007f' || c > '\u009f') && (c < '\ud800' || c > '\udfff') && (c != '_' || i + 1 >= length || (s[i + 1] != 'x' && s[i + 1] != 'X')))
				{
					array[num++] = c;
				}
				else if (c == '_')
				{
					array[num] = '_';
					array[num + 1] = 'x';
					array[num + 2] = '0';
					array[num + 3] = '0';
					array[num + 4] = '5';
					array[num + 5] = 'F';
					array[num + 6] = '_';
					num += 7;
				}
				else
				{
					array[num] = '_';
					array[num + 1] = 'x';
					array[num + 2 + 3] = InternalSerializer.hexlookup[(int)(c & '\u000f')];
					c >>= 4;
					array[num + 2 + 2] = InternalSerializer.hexlookup[(int)(c & '\u000f')];
					c >>= 4;
					array[num + 2 + 1] = InternalSerializer.hexlookup[(int)(c & '\u000f')];
					c >>= 4;
					array[num + 2] = InternalSerializer.hexlookup[(int)(c & '\u000f')];
					array[num + 6] = '_';
					num += 7;
				}
			}
			return new string(array, 0, num);
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x0010762C File Offset: 0x0010582C
		private void WriteEncodedElementString(string name, string value)
		{
			this.CheckIfStopping();
			value = InternalSerializer.EncodeString(value);
			if (SerializationOptions.NoNamespace == (this._context.options & SerializationOptions.NoNamespace))
			{
				this._writer.WriteElementString(name, value);
				return;
			}
			this._writer.WriteElementString(name, "http://schemas.microsoft.com/powershell/2004/04", value);
		}

		// Token: 0x04001A01 RID: 6657
		internal const string DefaultVersion = "1.1.0.1";

		// Token: 0x04001A02 RID: 6658
		private const int MaxDepthBelowTopLevel = 50;

		// Token: 0x04001A03 RID: 6659
		private readonly XmlWriter _writer;

		// Token: 0x04001A04 RID: 6660
		private readonly SerializationContext _context;

		// Token: 0x04001A05 RID: 6661
		private TypeTable _typeTable;

		// Token: 0x04001A06 RID: 6662
		private int depthBelowTopLevel;

		// Token: 0x04001A07 RID: 6663
		private readonly ReferenceIdHandlerForSerializer<object> objectRefIdHandler;

		// Token: 0x04001A08 RID: 6664
		private readonly ReferenceIdHandlerForSerializer<ConsolidatedString> typeRefIdHandler;

		// Token: 0x04001A09 RID: 6665
		private bool isStopping;

		// Token: 0x04001A0A RID: 6666
		private static Lazy<CimSerializer> cimSerializer = new Lazy<CimSerializer>(new Func<CimSerializer>(CimSerializer.Create));

		// Token: 0x04001A0B RID: 6667
		private bool? canUseDefaultRunspaceInThreadSafeManner;

		// Token: 0x04001A0C RID: 6668
		private Collection<CollectionEntry<PSMemberInfo>> extendedMembersCollection;

		// Token: 0x04001A0D RID: 6669
		private Collection<CollectionEntry<PSPropertyInfo>> allPropertiesCollection;

		// Token: 0x04001A0E RID: 6670
		private static readonly char[] hexlookup = new char[]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F'
		};

		// Token: 0x02000452 RID: 1106
		private class ConsolidatedStringEqualityComparer : IEqualityComparer<ConsolidatedString>
		{
			// Token: 0x06003044 RID: 12356 RVA: 0x001076BF File Offset: 0x001058BF
			bool IEqualityComparer<ConsolidatedString>.Equals(ConsolidatedString x, ConsolidatedString y)
			{
				return x.Key.Equals(y.Key, StringComparison.Ordinal);
			}

			// Token: 0x06003045 RID: 12357 RVA: 0x001076D3 File Offset: 0x001058D3
			int IEqualityComparer<ConsolidatedString>.GetHashCode(ConsolidatedString obj)
			{
				return obj.Key.GetHashCode();
			}
		}
	}
}
