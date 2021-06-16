using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Reflection;
using System.Security;
using System.Xml;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000166 RID: 358
	public sealed class TypeTable
	{
		// Token: 0x06001232 RID: 4658 RVA: 0x00064430 File Offset: 0x00062630
		private static void AddMember(Collection<string> errors, string typeName, PSMemberInfo member, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			if (PSMemberInfoCollection<PSMemberInfo>.IsReservedName(member.name))
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.ReservedNameMember, new object[]
				{
					member.name
				});
				return;
			}
			if (membersCollection[member.name] != null && !isOverride)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.DuplicateMember, new object[]
				{
					member.name
				});
				return;
			}
			member.IsInstance = false;
			if (membersCollection[member.name] == null)
			{
				membersCollection.Add(member);
				return;
			}
			membersCollection.Replace(member);
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x000644BC File Offset: 0x000626BC
		private static bool GetCheckNote(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> members, string noteName, Type noteType, out PSNoteProperty note)
		{
			note = null;
			PSMemberInfo psmemberInfo = null;
			for (int i = 0; i < members.Count; i++)
			{
				PSMemberInfo psmemberInfo2 = members[i];
				if (string.Compare(psmemberInfo2.Name, noteName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					psmemberInfo = psmemberInfo2;
				}
			}
			if (psmemberInfo == null)
			{
				return true;
			}
			note = (psmemberInfo as PSNoteProperty);
			if (note == null)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.MemberShouldBeNote, new object[]
				{
					psmemberInfo.Name
				});
				return false;
			}
			object value = note.Value;
			if (noteType.GetTypeCode().Equals(TypeCode.Boolean))
			{
				string text = value as string;
				if (text != null)
				{
					if (text.Length == 0)
					{
						note.noteValue = true;
					}
					else
					{
						note.noteValue = (string.Compare(text, "false", StringComparison.OrdinalIgnoreCase) != 0);
					}
					return true;
				}
			}
			try
			{
				note.noteValue = LanguagePrimitives.ConvertTo(value, noteType, CultureInfo.InvariantCulture);
			}
			catch (PSInvalidCastException ex)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.ErrorConvertingNote, new object[]
				{
					note.Name,
					ex.Message
				});
				return false;
			}
			return true;
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x000645F8 File Offset: 0x000627F8
		private static bool EnsureNotPresent(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> members, string memberName)
		{
			for (int i = 0; i < members.Count; i++)
			{
				PSMemberInfo psmemberInfo = members[i];
				if (string.Compare(psmemberInfo.Name, memberName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					TypeTable.AddError(errors, typeName, TypesXmlStrings.MemberShouldNotBePresent, new object[]
					{
						psmemberInfo.Name
					});
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00064650 File Offset: 0x00062850
		private static bool GetCheckMemberType(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> members, string noteName, Type memberType, out PSMemberInfo member)
		{
			member = null;
			for (int i = 0; i < members.Count; i++)
			{
				PSMemberInfo psmemberInfo = members[i];
				if (string.Compare(psmemberInfo.Name, noteName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					member = psmemberInfo;
				}
			}
			if (member == null)
			{
				return true;
			}
			if (memberType.IsInstanceOfType(member))
			{
				return true;
			}
			TypeTable.AddError(errors, typeName, TypesXmlStrings.MemberShouldHaveType, new object[]
			{
				member.Name,
				memberType.Name
			});
			member = null;
			return false;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x000646D0 File Offset: 0x000628D0
		private static bool CheckStandardMembers(Collection<string> errors, string typeName, PSMemberInfoInternalCollection<PSMemberInfo> members)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < members.Count; i++)
			{
				bool flag = false;
				string name = members[i].Name;
				for (int j = 0; j < TypeTable.standardMembers.Length; j++)
				{
					if (string.Equals(name, TypeTable.standardMembers[j], StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(name);
					TypeTable.AddError(errors, typeName, TypesXmlStrings.NotAStandardMember, new object[]
					{
						name
					});
				}
			}
			foreach (string name2 in list)
			{
				members.Remove(name2);
			}
			PSNoteProperty psnoteProperty;
			bool flag2 = TypeTable.GetCheckNote(errors, typeName, members, "SerializationMethod", typeof(SerializationMethod), out psnoteProperty);
			if (flag2)
			{
				SerializationMethod serializationMethod = SerializationMethod.AllPublicProperties;
				if (psnoteProperty != null)
				{
					serializationMethod = (SerializationMethod)psnoteProperty.Value;
				}
				if (serializationMethod == SerializationMethod.String)
				{
					flag2 = TypeTable.EnsureNotPresent(errors, typeName, members, "InheritPropertySerializationSet");
					if (!flag2)
					{
						goto IL_26E;
					}
					flag2 = TypeTable.EnsureNotPresent(errors, typeName, members, "PropertySerializationSet");
					if (!flag2)
					{
						goto IL_26E;
					}
					flag2 = TypeTable.EnsureNotPresent(errors, typeName, members, "SerializationDepth");
					if (!flag2)
					{
						goto IL_26E;
					}
				}
				else if (serializationMethod == SerializationMethod.SpecificProperties)
				{
					PSNoteProperty psnoteProperty2;
					flag2 = TypeTable.GetCheckNote(errors, typeName, members, "InheritPropertySerializationSet", typeof(bool), out psnoteProperty2);
					if (!flag2)
					{
						goto IL_26E;
					}
					PSMemberInfo psmemberInfo;
					flag2 = TypeTable.GetCheckMemberType(errors, typeName, members, "PropertySerializationSet", typeof(PSPropertySet), out psmemberInfo);
					if (!flag2)
					{
						goto IL_26E;
					}
					if (psnoteProperty2 != null && psnoteProperty2.Value.Equals(false) && psmemberInfo == null)
					{
						TypeTable.AddError(errors, typeName, TypesXmlStrings.MemberMustBePresent, new object[]
						{
							"PropertySerializationSet",
							"SerializationMethod",
							SerializationMethod.SpecificProperties.ToString(),
							"InheritPropertySerializationSet",
							"false"
						});
						flag2 = false;
						goto IL_26E;
					}
					PSNoteProperty psnoteProperty3;
					flag2 = TypeTable.GetCheckNote(errors, typeName, members, "SerializationDepth", typeof(int), out psnoteProperty3);
					if (!flag2)
					{
						goto IL_26E;
					}
				}
				else if (serializationMethod == SerializationMethod.AllPublicProperties)
				{
					flag2 = TypeTable.EnsureNotPresent(errors, typeName, members, "InheritPropertySerializationSet");
					if (!flag2)
					{
						goto IL_26E;
					}
					flag2 = TypeTable.EnsureNotPresent(errors, typeName, members, "PropertySerializationSet");
					if (!flag2)
					{
						goto IL_26E;
					}
					PSNoteProperty psnoteProperty4;
					flag2 = TypeTable.GetCheckNote(errors, typeName, members, "SerializationDepth", typeof(int), out psnoteProperty4);
					if (!flag2)
					{
						goto IL_26E;
					}
				}
				PSMemberInfo psmemberInfo2;
				flag2 = TypeTable.GetCheckMemberType(errors, typeName, members, "StringSerializationSource", typeof(PSPropertyInfo), out psmemberInfo2);
			}
			IL_26E:
			if (!flag2)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.SerializationSettingsIgnored, new object[0]);
				members.Remove("InheritPropertySerializationSet");
				members.Remove("SerializationMethod");
				members.Remove("StringSerializationSource");
				members.Remove("PropertySerializationSet");
				members.Remove("SerializationDepth");
			}
			PSMemberInfo psmemberInfo3;
			if (!TypeTable.GetCheckMemberType(errors, typeName, members, "DefaultDisplayPropertySet", typeof(PSPropertySet), out psmemberInfo3))
			{
				members.Remove("DefaultDisplayPropertySet");
			}
			if (!TypeTable.GetCheckMemberType(errors, typeName, members, "DefaultKeyPropertySet", typeof(PSPropertySet), out psmemberInfo3))
			{
				members.Remove("DefaultKeyPropertySet");
			}
			PSNoteProperty psnoteProperty5;
			if (!TypeTable.GetCheckNote(errors, typeName, members, "DefaultDisplayProperty", typeof(string), out psnoteProperty5))
			{
				members.Remove("DefaultDisplayProperty");
			}
			PSNoteProperty psnoteProperty6;
			if (!TypeTable.GetCheckNote(errors, typeName, members, "TargetTypeForDeserialization", typeof(Type), out psnoteProperty6))
			{
				members.Remove("TargetTypeForDeserialization");
			}
			else if (psnoteProperty6 != null)
			{
				members.Remove("TargetTypeForDeserialization");
				members.Add(psnoteProperty6, true);
			}
			return flag2;
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00064A5C File Offset: 0x00062C5C
		private static bool CreateInstance(Collection<string> errors, string typeName, Type type, string errorFormatString, out object instance)
		{
			instance = null;
			Exception ex = null;
			try
			{
				instance = Activator.CreateInstance(type);
			}
			catch (TargetInvocationException ex2)
			{
				ex = (ex2.InnerException ?? ex2);
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				ex = ex3;
			}
			if (ex != null)
			{
				TypeTable.AddError(errors, typeName, errorFormatString, new object[]
				{
					type.FullName,
					ex.Message
				});
				return false;
			}
			return true;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00064AD8 File Offset: 0x00062CD8
		private static void AddError(Collection<string> errors, string typeName, string resourceString, params object[] formatArguments)
		{
			string o = StringUtil.Format(resourceString, formatArguments);
			string item = StringUtil.Format(TypesXmlStrings.TypeDataTypeError, typeName, o);
			errors.Add(item);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00064B04 File Offset: 0x00062D04
		private static void ProcessMembersData(Collection<string> errors, string typeName, IEnumerable<TypeMemberData> membersData, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			foreach (TypeMemberData typeMemberData in membersData)
			{
				typeMemberData.Process(errors, typeName, membersCollection, isOverride);
			}
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00064B50 File Offset: 0x00062D50
		internal static void ProcessNoteData(Collection<string> errors, string typeName, NotePropertyData nodeData, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			PSNoteProperty member = new PSNoteProperty(nodeData.Name, nodeData.Value)
			{
				IsHidden = nodeData.IsHidden
			};
			TypeTable.AddMember(errors, typeName, member, membersCollection, isOverride);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00064B88 File Offset: 0x00062D88
		internal static void ProcessAliasData(Collection<string> errors, string typeName, AliasPropertyData aliasData, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			if (string.IsNullOrEmpty(aliasData.ReferencedMemberName))
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeDataShouldHaveValue, new object[]
				{
					"AliasPropertyData",
					"ReferencedMemberName"
				});
				return;
			}
			PSAliasProperty member = new PSAliasProperty(aliasData.Name, aliasData.ReferencedMemberName, aliasData.MemberType)
			{
				IsHidden = aliasData.IsHidden
			};
			TypeTable.AddMember(errors, typeName, member, membersCollection, isOverride);
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00064BF8 File Offset: 0x00062DF8
		internal static void ProcessScriptPropertyData(Collection<string> errors, string typeName, ScriptPropertyData scriptPropertyData, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			ScriptBlock getScriptBlock = scriptPropertyData.GetScriptBlock;
			ScriptBlock setScriptBlock = scriptPropertyData.SetScriptBlock;
			if (setScriptBlock == null && getScriptBlock == null)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.ScriptPropertyShouldHaveGetterOrSetter, new object[0]);
				return;
			}
			PSScriptProperty member = new PSScriptProperty(scriptPropertyData.Name, getScriptBlock, setScriptBlock, true)
			{
				IsHidden = scriptPropertyData.IsHidden
			};
			TypeTable.AddMember(errors, typeName, member, membersCollection, isOverride);
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00064C54 File Offset: 0x00062E54
		internal static void ProcessCodePropertyData(Collection<string> errors, string typeName, CodePropertyData codePropertyData, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			if (codePropertyData.GetCodeReference == null && codePropertyData.SetCodeReference == null)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.CodePropertyShouldHaveGetterOrSetter, new object[0]);
				return;
			}
			PSCodeProperty pscodeProperty;
			try
			{
				pscodeProperty = new PSCodeProperty(codePropertyData.Name, codePropertyData.GetCodeReference, codePropertyData.SetCodeReference);
			}
			catch (ExtendedTypeSystemException ex)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.Exception, new object[]
				{
					ex.Message
				});
				return;
			}
			pscodeProperty.IsHidden = codePropertyData.IsHidden;
			TypeTable.AddMember(errors, typeName, pscodeProperty, membersCollection, isOverride);
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x00064CF4 File Offset: 0x00062EF4
		internal static void ProcessScriptMethodData(Collection<string> errors, string typeName, ScriptMethodData scriptMethodData, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			if (scriptMethodData.Script == null)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeDataShouldHaveValue, new object[]
				{
					"ScriptMethodData",
					"Script"
				});
				return;
			}
			PSScriptMethod member = new PSScriptMethod(scriptMethodData.Name, scriptMethodData.Script, true);
			TypeTable.AddMember(errors, typeName, member, membersCollection, isOverride);
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x00064D4C File Offset: 0x00062F4C
		internal static void ProcessCodeMethodData(Collection<string> errors, string typeName, CodeMethodData codeMethodData, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			if (codeMethodData.CodeReference == null)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeDataShouldHaveValue, new object[]
				{
					"CodeMethodData",
					"CodeReference"
				});
				return;
			}
			PSCodeMethod member;
			try
			{
				member = new PSCodeMethod(codeMethodData.Name, codeMethodData.CodeReference);
			}
			catch (ExtendedTypeSystemException ex)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.Exception, new object[]
				{
					ex.Message
				});
				return;
			}
			TypeTable.AddMember(errors, typeName, member, membersCollection, isOverride);
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00064DDC File Offset: 0x00062FDC
		internal static void ProcessPropertySetData(Collection<string> errors, string typeName, PropertySetData propertySetData, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			if (propertySetData.ReferencedProperties == null || propertySetData.ReferencedProperties.Count == 0)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeDataShouldHaveValue, new object[]
				{
					"PropertySetData",
					"ReferencedProperties"
				});
				return;
			}
			Collection<string> collection = new Collection<string>();
			foreach (string text in propertySetData.ReferencedProperties)
			{
				if (string.IsNullOrEmpty(text))
				{
					TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeDataShouldNotBeNullOrEmpty, new object[]
					{
						"PropertySetData",
						"ReferencedProperties"
					});
				}
				else
				{
					collection.Add(text);
				}
			}
			if (collection.Count == 0)
			{
				return;
			}
			PSPropertySet member = new PSPropertySet(propertySetData.Name, collection)
			{
				IsHidden = propertySetData.IsHidden
			};
			TypeTable.AddMember(errors, typeName, member, membersCollection, isOverride);
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00064ED4 File Offset: 0x000630D4
		internal static void ProcessMemberSetData(Collection<string> errors, string typeName, MemberSetData memberSetData, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			PSMemberInfoInternalCollection<PSMemberInfo> membersCollection2 = new PSMemberInfoInternalCollection<PSMemberInfo>();
			foreach (TypeMemberData typeMemberData in memberSetData.Members)
			{
				typeMemberData.Process(errors, typeName, membersCollection2, isOverride);
			}
			PSMemberSet member = new PSMemberSet(memberSetData.Name, membersCollection2)
			{
				IsHidden = memberSetData.IsHidden,
				inheritMembers = memberSetData.InheritMembers
			};
			TypeTable.AddMember(errors, typeName, member, membersCollection, isOverride);
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00064F64 File Offset: 0x00063164
		private static void ProcessStandardMembers(Collection<string> errors, string typeName, IEnumerable<TypeMemberData> standardMembers, IEnumerable<PropertySetData> propertySets, PSMemberInfoInternalCollection<PSMemberInfo> membersCollection, bool isOverride)
		{
			if (membersCollection["PSStandardMembers"] == null)
			{
				PSMemberInfoInternalCollection<PSMemberInfo> membersCollection2 = new PSMemberInfoInternalCollection<PSMemberInfo>();
				TypeTable.ProcessMembersData(errors, typeName, standardMembers, membersCollection2, false);
				foreach (PropertySetData propertySetData in propertySets)
				{
					TypeTable.ProcessPropertySetData(errors, typeName, propertySetData, membersCollection2, false);
				}
				TypeTable.CheckStandardMembers(errors, typeName, membersCollection2);
				PSMemberSet member = new PSMemberSet("PSStandardMembers", membersCollection2)
				{
					inheritMembers = true,
					IsHidden = true,
					ShouldSerialize = false
				};
				TypeTable.AddMember(errors, typeName, member, membersCollection, false);
				return;
			}
			PSMemberSet psmemberSet = (PSMemberSet)membersCollection["PSStandardMembers"];
			PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<PSMemberInfo>();
			PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection2 = new PSMemberInfoInternalCollection<PSMemberInfo>();
			foreach (PSMemberInfo psmemberInfo in psmemberSet.InternalMembers)
			{
				psmemberInfoInternalCollection.Add(psmemberInfo.Copy());
				psmemberInfoInternalCollection2.Add(psmemberInfo.Copy());
			}
			TypeTable.ProcessMembersData(errors, typeName, standardMembers, psmemberInfoInternalCollection, isOverride);
			foreach (PropertySetData propertySetData2 in propertySets)
			{
				TypeTable.ProcessPropertySetData(errors, typeName, propertySetData2, psmemberInfoInternalCollection, isOverride);
			}
			if (TypeTable.CheckStandardMembers(errors, typeName, psmemberInfoInternalCollection))
			{
				PSMemberSet member2 = new PSMemberSet("PSStandardMembers", psmemberInfoInternalCollection)
				{
					inheritMembers = true,
					IsHidden = true,
					ShouldSerialize = false
				};
				TypeTable.AddMember(errors, typeName, member2, membersCollection, true);
				return;
			}
			foreach (PSMemberInfo psmemberInfo2 in psmemberInfoInternalCollection)
			{
				if (psmemberInfoInternalCollection2[psmemberInfo2.name] == null)
				{
					psmemberInfoInternalCollection2.Add(psmemberInfo2);
				}
			}
			PSMemberSet member3 = new PSMemberSet("PSStandardMembers", psmemberInfoInternalCollection2)
			{
				inheritMembers = true,
				IsHidden = true,
				ShouldSerialize = false
			};
			TypeTable.AddMember(errors, typeName, member3, membersCollection, true);
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0006519C File Offset: 0x0006339C
		private void ProcessTypeDataToAdd(Collection<string> errors, TypeData typeData)
		{
			string typeName = typeData.TypeName;
			Collection<PropertySetData> collection = new Collection<PropertySetData>();
			if (typeData.DefaultDisplayPropertySet != null)
			{
				collection.Add(typeData.DefaultDisplayPropertySet);
			}
			if (typeData.DefaultKeyPropertySet != null)
			{
				collection.Add(typeData.DefaultKeyPropertySet);
			}
			if (typeData.PropertySerializationSet != null)
			{
				collection.Add(typeData.PropertySerializationSet);
			}
			if (typeData.Members.Count == 0 && typeData.StandardMembers.Count == 0 && typeData.TypeConverter == null && typeData.TypeAdapter == null && collection.Count == 0 && !typeData.fromTypesXmlFile)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeDataShouldNotBeEmpty, new object[0]);
				return;
			}
			if (typeData.Members.Count > 0)
			{
				PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection;
				if (!this.members.TryGetValue(typeName, out psmemberInfoInternalCollection))
				{
					psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<PSMemberInfo>();
					this.members[typeName] = psmemberInfoInternalCollection;
				}
				TypeTable.ProcessMembersData(errors, typeName, typeData.Members.Values, psmemberInfoInternalCollection, typeData.IsOverride);
				foreach (string memberName in typeData.Members.Keys)
				{
					PSGetMemberBinder.TypeTableMemberAdded(memberName);
				}
			}
			if (typeData.StandardMembers.Count > 0 || collection.Count > 0)
			{
				PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection2;
				if (!this.members.TryGetValue(typeName, out psmemberInfoInternalCollection2))
				{
					psmemberInfoInternalCollection2 = new PSMemberInfoInternalCollection<PSMemberInfo>();
					this.members[typeName] = psmemberInfoInternalCollection2;
				}
				TypeTable.ProcessStandardMembers(errors, typeName, typeData.StandardMembers.Values, collection, psmemberInfoInternalCollection2, typeData.IsOverride);
			}
			if (typeData.TypeConverter != null)
			{
				if (this.typeConverters.ContainsKey(typeName) && !typeData.IsOverride)
				{
					TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeConverterAlreadyPresent, new object[0]);
				}
				object obj;
				if (TypeTable.CreateInstance(errors, typeName, typeData.TypeConverter, TypesXmlStrings.UnableToInstantiateTypeConverter, out obj))
				{
					if (obj is TypeConverter || obj is PSTypeConverter)
					{
						this.typeConverters[typeName] = obj;
						LanguagePrimitives.UpdateTypeConvertFromTypeTable(typeName);
					}
					else
					{
						TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeIsNotTypeConverter, new object[]
						{
							typeData.TypeConverter.FullName
						});
					}
				}
			}
			if (typeData.TypeAdapter != null)
			{
				if (this.typeAdapters.ContainsKey(typeName) && !typeData.IsOverride)
				{
					TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeAdapterAlreadyPresent, new object[0]);
				}
				object obj2;
				if (TypeTable.CreateInstance(errors, typeName, typeData.TypeAdapter, TypesXmlStrings.UnableToInstantiateTypeAdapter, out obj2))
				{
					PSPropertyAdapter pspropertyAdapter = obj2 as PSPropertyAdapter;
					Type adaptedType;
					if (pspropertyAdapter == null)
					{
						TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeIsNotTypeAdapter, new object[]
						{
							typeData.TypeAdapter.FullName
						});
					}
					else if (LanguagePrimitives.TryConvertTo<Type>(typeName, out adaptedType))
					{
						this.typeAdapters[typeName] = PSObject.CreateThirdPartyAdapterSet(adaptedType, pspropertyAdapter);
					}
					else
					{
						TypeTable.AddError(errors, typeName, TypesXmlStrings.InvalidAdaptedType, new object[]
						{
							typeName
						});
					}
				}
			}
			this.typesInfo.Add(new SessionStateTypeEntry(typeData, false));
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x000654A4 File Offset: 0x000636A4
		private void ProcessTypeDataToRemove(Collection<string> errors, TypeData typeData)
		{
			string typeName = typeData.TypeName;
			bool flag = false;
			PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection;
			if (this.members.TryGetValue(typeName, out psmemberInfoInternalCollection))
			{
				flag = true;
				this.members.Remove(typeName);
				foreach (PSMemberInfo psmemberInfo in psmemberInfoInternalCollection)
				{
					PSGetMemberBinder.TypeTableMemberPossiblyUpdated(psmemberInfo.Name);
				}
			}
			if (this.typeConverters.ContainsKey(typeName))
			{
				flag = true;
				this.typeConverters.Remove(typeName);
				LanguagePrimitives.UpdateTypeConvertFromTypeTable(typeName);
			}
			if (this.typeAdapters.ContainsKey(typeName))
			{
				flag = true;
				this.typeAdapters.Remove(typeName);
			}
			if (!flag)
			{
				TypeTable.AddError(errors, typeName, TypesXmlStrings.TypeNotFound, new object[]
				{
					typeName
				});
				return;
			}
			this.typesInfo.Add(new SessionStateTypeEntry(typeData, true));
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00065590 File Offset: 0x00063790
		private void Update(LoadContext context)
		{
			try
			{
				TypesPs1xmlReader typesPs1xmlReader = new TypesPs1xmlReader(context);
				IEnumerable<TypeData> enumerable = typesPs1xmlReader.Read();
				if (enumerable != null)
				{
					foreach (TypeData typeData in enumerable)
					{
						this.ProcessTypeDataToAdd(context.errors, typeData);
					}
				}
			}
			catch (Exception ex)
			{
				context.AddError(TypesXmlStrings.Exception, new object[]
				{
					ex.Message
				});
			}
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00065624 File Offset: 0x00063824
		private void Update(Collection<string> errors, TypeData typeData, bool isRemove)
		{
			if (!isRemove)
			{
				this.ProcessTypeDataToAdd(errors, typeData);
				return;
			}
			this.ProcessTypeDataToRemove(errors, typeData);
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0006563C File Offset: 0x0006383C
		static TypeTable()
		{
			foreach (string memberName in TypeTable.standardMembers)
			{
				PSGetMemberBinder.TypeTableMemberAdded(memberName);
			}
			PSGetMemberBinder.TypeTableMemberAdded("PSStandardMembers");
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x000656C7 File Offset: 0x000638C7
		internal TypeTable() : this(false)
		{
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x000656D0 File Offset: 0x000638D0
		internal TypeTable(bool isShared)
		{
			this.consolidatedMembers = new Dictionary<string, PSMemberInfoInternalCollection<PSMemberInfo>>(256, StringComparer.OrdinalIgnoreCase);
			this.consolidatedSpecificProperties = new Dictionary<string, Collection<string>>(StringComparer.OrdinalIgnoreCase);
			this.members = new Dictionary<string, PSMemberInfoInternalCollection<PSMemberInfo>>(StringComparer.OrdinalIgnoreCase);
			this.typeConverters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			this.typeAdapters = new Dictionary<string, PSObject.AdapterSet>(StringComparer.OrdinalIgnoreCase);
			this.typesInfo = new InitialSessionStateEntryCollection<SessionStateTypeEntry>();
			base..ctor();
			this.isShared = isShared;
			this.typeFileList = new List<string>();
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00065755 File Offset: 0x00063955
		public TypeTable(IEnumerable<string> typeFiles) : this(typeFiles, null, null)
		{
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00065760 File Offset: 0x00063960
		public static TypeTable LoadDefaultTypeFiles()
		{
			return new TypeTable(TypeTable.GetDefaultTypeFiles());
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0006576C File Offset: 0x0006396C
		public static List<string> GetDefaultTypeFiles()
		{
			string item = string.Empty;
			string item2 = string.Empty;
			string defaultPowerShellShellID = Utils.DefaultPowerShellShellID;
			string applicationBase = Utils.GetApplicationBase(defaultPowerShellShellID);
			if (!string.IsNullOrEmpty(applicationBase))
			{
				item = Path.Combine(applicationBase, "types.ps1xml");
				item2 = Path.Combine(applicationBase, "typesv3.ps1xml");
			}
			return new List<string>
			{
				item,
				item2
			};
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x000657CC File Offset: 0x000639CC
		internal TypeTable(IEnumerable<string> typeFiles, AuthorizationManager authorizationManager, PSHost host)
		{
			this.consolidatedMembers = new Dictionary<string, PSMemberInfoInternalCollection<PSMemberInfo>>(256, StringComparer.OrdinalIgnoreCase);
			this.consolidatedSpecificProperties = new Dictionary<string, Collection<string>>(StringComparer.OrdinalIgnoreCase);
			this.members = new Dictionary<string, PSMemberInfoInternalCollection<PSMemberInfo>>(StringComparer.OrdinalIgnoreCase);
			this.typeConverters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
			this.typeAdapters = new Dictionary<string, PSObject.AdapterSet>(StringComparer.OrdinalIgnoreCase);
			this.typesInfo = new InitialSessionStateEntryCollection<SessionStateTypeEntry>();
			base..ctor();
			if (typeFiles == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeFiles");
			}
			this.isShared = true;
			this.typeFileList = new List<string>();
			Collection<string> collection = new Collection<string>();
			foreach (string text in typeFiles)
			{
				if (string.IsNullOrEmpty(text) || !Path.IsPathRooted(text))
				{
					throw PSTraceSource.NewArgumentException("typeFile", TypesXmlStrings.TypeFileNotRooted, new object[]
					{
						text
					});
				}
				bool flag;
				this.Initialize(string.Empty, text, collection, authorizationManager, host, out flag);
				this.typeFileList.Add(text);
			}
			if (collection.Count > 0)
			{
				throw new TypeTableLoadException(collection);
			}
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x000658F8 File Offset: 0x00063AF8
		internal Collection<string> GetSpecificProperties(ConsolidatedString types)
		{
			Collection<string> result;
			lock (this.members)
			{
				if (types == null || string.IsNullOrEmpty(types.Key))
				{
					result = new Collection<string>();
				}
				else
				{
					Collection<string> collection;
					if (!this.consolidatedSpecificProperties.TryGetValue(types.Key, out collection))
					{
						HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
						foreach (string key in types)
						{
							PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection;
							if (this.members.TryGetValue(key, out psmemberInfoInternalCollection))
							{
								PSMemberSet psmemberSet = psmemberInfoInternalCollection["PSStandardMembers"] as PSMemberSet;
								if (psmemberSet != null)
								{
									PSPropertySet pspropertySet = psmemberSet.Members["PropertySerializationSet"] as PSPropertySet;
									if (pspropertySet != null)
									{
										foreach (string item in pspropertySet.ReferencedPropertyNames)
										{
											hashSet.Add(item);
										}
										bool flag2 = (bool)PSObject.GetNoteSettingValue(psmemberSet, "InheritPropertySerializationSet", true, typeof(bool), false, null);
										if (!flag2)
										{
											break;
										}
									}
								}
							}
						}
						collection = new Collection<string>();
						foreach (string item2 in hashSet)
						{
							collection.Add(item2);
						}
						this.consolidatedSpecificProperties[types.Key] = collection;
					}
					result = collection;
				}
			}
			return result;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00065AF0 File Offset: 0x00063CF0
		internal PSMemberInfoInternalCollection<T> GetMembers<T>(ConsolidatedString types) where T : PSMemberInfo
		{
			return PSObject.TransformMemberInfoCollection<PSMemberInfo, T>(this.GetMembers(types));
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00065B00 File Offset: 0x00063D00
		private PSMemberInfoInternalCollection<PSMemberInfo> GetMembers(ConsolidatedString types)
		{
			PSMemberInfoInternalCollection<PSMemberInfo> result;
			lock (this.members)
			{
				if (this.members.Count == 0 || types == null || string.IsNullOrEmpty(types.Key))
				{
					result = new PSMemberInfoInternalCollection<PSMemberInfo>();
				}
				else
				{
					PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection;
					if (!this.consolidatedMembers.TryGetValue(types.Key, out psmemberInfoInternalCollection))
					{
						psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<PSMemberInfo>();
						for (int i = types.Count - 1; i >= 0; i--)
						{
							PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection2;
							if (this.members.TryGetValue(types[i], out psmemberInfoInternalCollection2))
							{
								foreach (PSMemberInfo psmemberInfo in psmemberInfoInternalCollection2)
								{
									PSMemberInfo psmemberInfo2 = psmemberInfoInternalCollection[psmemberInfo.Name];
									if (psmemberInfo2 == null)
									{
										psmemberInfoInternalCollection.Add(psmemberInfo.Copy());
									}
									else
									{
										PSMemberSet psmemberSet = psmemberInfo2 as PSMemberSet;
										PSMemberSet psmemberSet2 = psmemberInfo as PSMemberSet;
										if (psmemberSet == null || psmemberSet2 == null || !psmemberSet2.InheritMembers)
										{
											psmemberInfoInternalCollection.Remove(psmemberInfo.Name);
											psmemberInfoInternalCollection.Add(psmemberInfo.Copy());
										}
										else
										{
											foreach (PSMemberInfo psmemberInfo3 in psmemberSet2.Members)
											{
												if (psmemberSet.Members[psmemberInfo3.Name] == null)
												{
													((PSMemberInfoIntegratingCollection<PSMemberInfo>)psmemberSet.Members).AddToTypesXmlCache(psmemberInfo3, false);
												}
												else
												{
													psmemberSet.InternalMembers.Replace(psmemberInfo3);
												}
											}
										}
									}
								}
							}
						}
						this.consolidatedMembers[types.Key] = psmemberInfoInternalCollection;
					}
					result = psmemberInfoInternalCollection;
				}
			}
			return result;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00065D04 File Offset: 0x00063F04
		internal object GetTypeConverter(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				return null;
			}
			object result;
			lock (this.members)
			{
				object obj2;
				this.typeConverters.TryGetValue(typeName, out obj2);
				result = obj2;
			}
			return result;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00065D5C File Offset: 0x00063F5C
		internal PSObject.AdapterSet GetTypeAdapter(Type type)
		{
			if (type == null)
			{
				return null;
			}
			PSObject.AdapterSet result;
			lock (this.members)
			{
				PSObject.AdapterSet adapterSet;
				this.typeAdapters.TryGetValue(type.FullName, out adapterSet);
				result = adapterSet;
			}
			return result;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00065DB8 File Offset: 0x00063FB8
		private TypeMemberData GetTypeMemberDataFromPSMemberInfo(PSMemberInfo member)
		{
			PSNoteProperty psnoteProperty = member as PSNoteProperty;
			if (psnoteProperty != null)
			{
				return new NotePropertyData(psnoteProperty.Name, psnoteProperty.Value);
			}
			PSAliasProperty psaliasProperty = member as PSAliasProperty;
			if (psaliasProperty != null)
			{
				return new AliasPropertyData(psaliasProperty.Name, psaliasProperty.ReferencedMemberName);
			}
			PSScriptProperty psscriptProperty = member as PSScriptProperty;
			if (psscriptProperty != null)
			{
				ScriptBlock getScriptBlock = psscriptProperty.IsGettable ? psscriptProperty.GetterScript : null;
				ScriptBlock setScriptBlock = psscriptProperty.IsSettable ? psscriptProperty.SetterScript : null;
				return new ScriptPropertyData(psscriptProperty.Name, getScriptBlock, setScriptBlock);
			}
			PSCodeProperty pscodeProperty = member as PSCodeProperty;
			if (pscodeProperty != null)
			{
				MethodInfo getMethod = pscodeProperty.IsGettable ? pscodeProperty.GetterCodeReference : null;
				MethodInfo setMethod = pscodeProperty.IsSettable ? pscodeProperty.SetterCodeReference : null;
				return new CodePropertyData(pscodeProperty.Name, getMethod, setMethod);
			}
			PSScriptMethod psscriptMethod = member as PSScriptMethod;
			if (psscriptMethod != null)
			{
				return new ScriptMethodData(psscriptMethod.Name, psscriptMethod.Script);
			}
			PSCodeMethod pscodeMethod = member as PSCodeMethod;
			if (pscodeMethod != null)
			{
				return new CodeMethodData(pscodeMethod.Name, pscodeMethod.CodeReference);
			}
			PSMemberSet psmemberSet = member as PSMemberSet;
			if (psmemberSet == null)
			{
				return null;
			}
			if (psmemberSet.Name.Equals("PSStandardMembers", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			Collection<TypeMemberData> collection = new Collection<TypeMemberData>();
			foreach (PSMemberInfo member2 in psmemberSet.Members)
			{
				collection.Add(this.GetTypeMemberDataFromPSMemberInfo(member2));
			}
			return new MemberSetData(psmemberSet.Name, collection);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00065F4C File Offset: 0x0006414C
		private void LoadMembersToTypeData(PSMemberInfo member, TypeData typeData)
		{
			TypeMemberData typeMemberDataFromPSMemberInfo = this.GetTypeMemberDataFromPSMemberInfo(member);
			if (typeMemberDataFromPSMemberInfo != null)
			{
				typeData.Members.Add(typeMemberDataFromPSMemberInfo.Name, typeMemberDataFromPSMemberInfo);
				return;
			}
			PSMemberSet psmemberSet = member as PSMemberSet;
			if (psmemberSet != null && psmemberSet.Name.Equals("PSStandardMembers", StringComparison.OrdinalIgnoreCase))
			{
				this.LoadStandardMembersToTypeData(psmemberSet, typeData);
			}
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00065F9C File Offset: 0x0006419C
		private static T GetParameterType<T>(object sourceValue)
		{
			return (T)((object)LanguagePrimitives.ConvertTo(sourceValue, typeof(T), CultureInfo.InvariantCulture));
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00065FB8 File Offset: 0x000641B8
		private void LoadStandardMembersToTypeData(PSMemberSet memberSet, TypeData typeData)
		{
			foreach (PSMemberInfo psmemberInfo in memberSet.InternalMembers)
			{
				PSMemberInfo psmemberInfo2 = psmemberInfo.Copy();
				string name = psmemberInfo2.Name;
				if (name.Equals("SerializationMethod", StringComparison.OrdinalIgnoreCase))
				{
					typeData.SerializationMethod = TypeTable.GetParameterType<string>(((PSNoteProperty)psmemberInfo2).Value);
				}
				else if (name.Equals("TargetTypeForDeserialization", StringComparison.OrdinalIgnoreCase))
				{
					typeData.TargetTypeForDeserialization = TypeTable.GetParameterType<Type>(((PSNoteProperty)psmemberInfo2).Value);
				}
				else if (name.Equals("SerializationDepth", StringComparison.OrdinalIgnoreCase))
				{
					typeData.SerializationDepth = TypeTable.GetParameterType<uint>(((PSNoteProperty)psmemberInfo2).Value);
				}
				else if (name.Equals("DefaultDisplayProperty", StringComparison.OrdinalIgnoreCase))
				{
					typeData.DefaultDisplayProperty = TypeTable.GetParameterType<string>(((PSNoteProperty)psmemberInfo2).Value);
				}
				else if (name.Equals("InheritPropertySerializationSet", StringComparison.OrdinalIgnoreCase))
				{
					typeData.InheritPropertySerializationSet = TypeTable.GetParameterType<bool>(((PSNoteProperty)psmemberInfo2).Value);
				}
				else if (name.Equals("StringSerializationSource", StringComparison.OrdinalIgnoreCase))
				{
					PSAliasProperty psaliasProperty = psmemberInfo2 as PSAliasProperty;
					if (psaliasProperty != null)
					{
						typeData.StringSerializationSource = psaliasProperty.ReferencedMemberName;
					}
					else
					{
						typeData.StringSerializationSourceProperty = this.GetTypeMemberDataFromPSMemberInfo(psmemberInfo2);
					}
				}
				else if (name.Equals("DefaultDisplayPropertySet", StringComparison.OrdinalIgnoreCase))
				{
					typeData.DefaultDisplayPropertySet = new PropertySetData(((PSPropertySet)psmemberInfo2).ReferencedPropertyNames);
				}
				else if (name.Equals("DefaultKeyPropertySet", StringComparison.OrdinalIgnoreCase))
				{
					typeData.DefaultKeyPropertySet = new PropertySetData(((PSPropertySet)psmemberInfo2).ReferencedPropertyNames);
				}
				else if (name.Equals("PropertySerializationSet", StringComparison.OrdinalIgnoreCase))
				{
					typeData.PropertySerializationSet = new PropertySetData(((PSPropertySet)psmemberInfo2).ReferencedPropertyNames);
				}
			}
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00066198 File Offset: 0x00064398
		internal Dictionary<string, TypeData> GetAllTypeData()
		{
			Dictionary<string, TypeData> result;
			lock (this.members)
			{
				Dictionary<string, TypeData> dictionary = new Dictionary<string, TypeData>();
				foreach (string text in this.members.Keys)
				{
					if (!dictionary.ContainsKey(text))
					{
						TypeData typeData = new TypeData(text);
						bool flag2 = false;
						flag2 |= this.RetrieveMembersToTypeData(typeData);
						flag2 |= this.RetrieveConverterToTypeData(typeData);
						flag2 |= this.RetrieveAdapterToTypeData(typeData);
						if (flag2)
						{
							dictionary.Add(text, typeData);
						}
					}
				}
				foreach (string text2 in this.typeConverters.Keys)
				{
					if (!dictionary.ContainsKey(text2))
					{
						TypeData typeData2 = new TypeData(text2);
						bool flag3 = false;
						flag3 |= this.RetrieveConverterToTypeData(typeData2);
						flag3 |= this.RetrieveAdapterToTypeData(typeData2);
						if (flag3)
						{
							dictionary.Add(text2, typeData2);
						}
					}
				}
				foreach (string text3 in this.typeAdapters.Keys)
				{
					if (!dictionary.ContainsKey(text3))
					{
						TypeData typeData3 = new TypeData(text3);
						if (this.RetrieveAdapterToTypeData(typeData3))
						{
							dictionary.Add(text3, typeData3);
						}
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00066378 File Offset: 0x00064578
		private bool RetrieveMembersToTypeData(TypeData typeData)
		{
			string typeName = typeData.TypeName;
			PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection;
			if (this.members.TryGetValue(typeName, out psmemberInfoInternalCollection))
			{
				foreach (PSMemberInfo psmemberInfo in psmemberInfoInternalCollection)
				{
					PSMemberInfo member = psmemberInfo.Copy();
					this.LoadMembersToTypeData(member, typeData);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x000663E8 File Offset: 0x000645E8
		private bool RetrieveConverterToTypeData(TypeData typeData)
		{
			string typeName = typeData.TypeName;
			object obj;
			if (this.typeConverters.TryGetValue(typeName, out obj))
			{
				typeData.TypeConverter = obj.GetType();
				return true;
			}
			return false;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0006641C File Offset: 0x0006461C
		private bool RetrieveAdapterToTypeData(TypeData typeData)
		{
			string typeName = typeData.TypeName;
			PSObject.AdapterSet adapterSet;
			if (this.typeAdapters.TryGetValue(typeName, out adapterSet))
			{
				typeData.TypeAdapter = ((ThirdPartyAdapter)adapterSet.OriginalAdapter).ExternalAdapterType;
				return true;
			}
			return false;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0006645C File Offset: 0x0006465C
		public TypeTable Clone(bool unshared)
		{
			TypeTable typeTable = unshared ? new TypeTable() : new TypeTable(this.isShared);
			foreach (KeyValuePair<string, PSMemberInfoInternalCollection<PSMemberInfo>> keyValuePair in this.members)
			{
				PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<PSMemberInfo>();
				foreach (PSMemberInfo psmemberInfo in keyValuePair.Value)
				{
					psmemberInfoInternalCollection.Add(psmemberInfo.Copy());
				}
				typeTable.members.Add(keyValuePair.Key, psmemberInfoInternalCollection);
			}
			foreach (KeyValuePair<string, PSObject.AdapterSet> keyValuePair2 in this.typeAdapters)
			{
				typeTable.typeAdapters.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
			foreach (KeyValuePair<string, object> keyValuePair3 in this.typeConverters)
			{
				typeTable.typeConverters.Add(keyValuePair3.Key, keyValuePair3.Value);
			}
			typeTable.typesInfo.Add(this.typesInfo);
			return typeTable;
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x000665E0 File Offset: 0x000647E0
		internal void Clear(bool fullClear)
		{
			lock (this.members)
			{
				if (fullClear)
				{
					foreach (string typeName in this.typeConverters.Keys)
					{
						LanguagePrimitives.UpdateTypeConvertFromTypeTable(typeName);
					}
					foreach (PSMemberInfoInternalCollection<PSMemberInfo> psmemberInfoInternalCollection in this.members.Values)
					{
						foreach (PSMemberInfo psmemberInfo in psmemberInfoInternalCollection)
						{
							PSGetMemberBinder.TypeTableMemberPossiblyUpdated(psmemberInfo.Name);
						}
					}
					TypeTable.StandardMembersUpdated();
					this.members.Clear();
					this.typeConverters.Clear();
					this.typeAdapters.Clear();
					this.typesInfo.Clear();
				}
				this.consolidatedMembers.Clear();
				this.consolidatedSpecificProperties.Clear();
			}
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00066764 File Offset: 0x00064964
		private static void StandardMembersUpdated()
		{
			foreach (string memberName in TypeTable.standardMembers)
			{
				PSGetMemberBinder.TypeTableMemberPossiblyUpdated(memberName);
			}
			PSGetMemberBinder.TypeTableMemberPossiblyUpdated("PSStandardMembers");
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0006679C File Offset: 0x0006499C
		internal void Initialize(string snapinName, string fileToLoad, Collection<string> errors, AuthorizationManager authorizationManager, PSHost host, out bool failToLoadFile)
		{
			if (this.ProcessIsBuiltIn(fileToLoad, errors, out failToLoadFile))
			{
				return;
			}
			bool isFullyTrusted;
			string moduleContents = this.GetModuleContents(snapinName, fileToLoad, errors, authorizationManager, host, out isFullyTrusted, out failToLoadFile);
			if (moduleContents == null)
			{
				return;
			}
			this.UpdateWithModuleContents(moduleContents, snapinName, fileToLoad, isFullyTrusted, errors);
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x000667D8 File Offset: 0x000649D8
		private string GetModuleContents(string moduleName, string fileToLoad, Collection<string> errors, AuthorizationManager authorizationManager, PSHost host, out bool isFullyTrusted, out bool failToLoadFile)
		{
			isFullyTrusted = false;
			ExternalScriptInfo externalScriptInfo;
			string scriptContents;
			try
			{
				externalScriptInfo = new ExternalScriptInfo(fileToLoad, fileToLoad);
				scriptContents = externalScriptInfo.ScriptContents;
				if (externalScriptInfo.DefiningLanguageMode == PSLanguageMode.FullLanguage)
				{
					isFullyTrusted = true;
				}
			}
			catch (SecurityException ex)
			{
				string text = StringUtil.Format(TypesXmlStrings.Exception, ex.Message);
				string item = StringUtil.Format(TypesXmlStrings.FileError, new object[]
				{
					moduleName,
					fileToLoad,
					text
				});
				errors.Add(item);
				failToLoadFile = true;
				return null;
			}
			if (authorizationManager != null)
			{
				try
				{
					authorizationManager.ShouldRunInternal(externalScriptInfo, CommandOrigin.Internal, host);
				}
				catch (PSSecurityException ex2)
				{
					string item2 = StringUtil.Format(TypesXmlStrings.ValidationException, new object[]
					{
						moduleName,
						fileToLoad,
						ex2.Message
					});
					errors.Add(item2);
					failToLoadFile = true;
					return null;
				}
			}
			failToLoadFile = false;
			return scriptContents;
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x000668D8 File Offset: 0x00064AD8
		private void UpdateWithModuleContents(string fileContents, string moduleName, string fileToLoad, bool isFullyTrusted, Collection<string> errors)
		{
			this.typesInfo.Add(new SessionStateTypeEntry(fileToLoad));
			LoadContext loadContext = new LoadContext(moduleName, fileToLoad, errors)
			{
				IsFullyTrusted = isFullyTrusted
			};
			using (StringReader stringReader = new StringReader(fileContents))
			{
				XmlReader xmlReader = new XmlTextReader(stringReader)
				{
					WhitespaceHandling = WhitespaceHandling.Significant
				};
				loadContext.reader = xmlReader;
				this.Update(loadContext);
				xmlReader.Dispose();
			}
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00066954 File Offset: 0x00064B54
		internal void Remove(string typeFile)
		{
			lock (this.typeFileList)
			{
				this.typeFileList.Remove(typeFile);
				this.typesInfo.Remove(typeFile, null);
			}
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x000669A8 File Offset: 0x00064BA8
		public void AddType(TypeData typeData)
		{
			if (typeData == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeData");
			}
			Collection<string> errors = new Collection<string>();
			lock (this.members)
			{
				this.consolidatedMembers.Clear();
				this.consolidatedSpecificProperties.Clear();
				this.Update(errors, typeData, false);
				TypeTable.StandardMembersUpdated();
			}
			FormatAndTypeDataHelper.ThrowExceptionOnError("ErrorsUpdatingTypes", errors, RunspaceConfigurationCategory.Types);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00066A28 File Offset: 0x00064C28
		public void RemoveType(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				throw PSTraceSource.NewArgumentNullException("typeName");
			}
			TypeData typeData = new TypeData(typeName);
			Collection<string> errors = new Collection<string>();
			lock (this.members)
			{
				this.consolidatedMembers.Clear();
				this.consolidatedSpecificProperties.Clear();
				this.Update(errors, typeData, true);
				TypeTable.StandardMembersUpdated();
			}
			FormatAndTypeDataHelper.ThrowExceptionOnError("ErrorsUpdatingTypes", errors, RunspaceConfigurationCategory.Types);
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00066AB4 File Offset: 0x00064CB4
		internal void Update(Collection<PSSnapInTypeAndFormatErrors> psSnapinTypes, AuthorizationManager authorizationManager, PSHost host)
		{
			if (this.isShared)
			{
				throw PSTraceSource.NewInvalidOperationException(TypesXmlStrings.SharedTypeTableCannotBeUpdated, new object[0]);
			}
			lock (this.members)
			{
				this.Clear(true);
				foreach (PSSnapInTypeAndFormatErrors pssnapInTypeAndFormatErrors in psSnapinTypes)
				{
					if (pssnapInTypeAndFormatErrors.FullPath != null)
					{
						this.Initialize(pssnapInTypeAndFormatErrors.PSSnapinName, pssnapInTypeAndFormatErrors.FullPath, pssnapInTypeAndFormatErrors.Errors, authorizationManager, host, out pssnapInTypeAndFormatErrors.FailToLoadFile);
					}
					else
					{
						this.Update(pssnapInTypeAndFormatErrors.Errors, pssnapInTypeAndFormatErrors.TypeData, pssnapInTypeAndFormatErrors.IsRemove);
					}
				}
			}
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00066B80 File Offset: 0x00064D80
		internal void Update(string filePath, Collection<string> errors, bool clearTable, AuthorizationManager authorizationManager, PSHost host, out bool failToLoadFile)
		{
			this.Update(filePath, filePath, errors, clearTable, authorizationManager, host, out failToLoadFile);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00066B94 File Offset: 0x00064D94
		internal void Update(string moduleName, string filePath, Collection<string> errors, bool clearTable, AuthorizationManager authorizationManager, PSHost host, out bool failToLoadFile)
		{
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}
			if (errors == null)
			{
				throw new ArgumentNullException("errors");
			}
			if (this.isShared)
			{
				throw PSTraceSource.NewInvalidOperationException(TypesXmlStrings.SharedTypeTableCannotBeUpdated, new object[0]);
			}
			this.Clear(clearTable);
			if (this.ProcessIsBuiltIn(filePath, errors, out failToLoadFile))
			{
				return;
			}
			bool isFullyTrusted;
			string moduleContents = this.GetModuleContents(moduleName, filePath, errors, authorizationManager, host, out isFullyTrusted, out failToLoadFile);
			if (moduleContents == null)
			{
				return;
			}
			lock (this.members)
			{
				this.UpdateWithModuleContents(moduleContents, moduleName, filePath, isFullyTrusted, errors);
			}
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00066C3C File Offset: 0x00064E3C
		private void ProcessTypeData(string filePath, Collection<string> errors, IEnumerable<TypeData> types)
		{
			lock (this.members)
			{
				this.typesInfo.Add(new SessionStateTypeEntry(filePath));
				foreach (TypeData typeData in types)
				{
					this.ProcessTypeDataToAdd(errors, typeData);
				}
			}
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00066CC0 File Offset: 0x00064EC0
		private bool ProcessIsBuiltIn(string filePath, Collection<string> errors, out bool failToLoadFile)
		{
			if (InternalTestHooks.ReadEngineTypesXmlFiles)
			{
				failToLoadFile = false;
				return false;
			}
			bool result = false;
			int count = errors.Count;
			string applicationBase = Utils.GetApplicationBase(Utils.DefaultPowerShellShellID);
			if (string.Equals(Path.Combine(applicationBase, "types.ps1xml"), filePath, StringComparison.OrdinalIgnoreCase))
			{
				this.ProcessTypeData(filePath, errors, Types_Ps1Xml.Get());
				result = true;
			}
			else if (string.Equals(Path.Combine(applicationBase, "typesv3.ps1xml"), filePath, StringComparison.OrdinalIgnoreCase))
			{
				this.ProcessTypeData(filePath, errors, TypesV3_Ps1Xml.Get());
				result = true;
			}
			else if (string.Equals(Path.Combine(applicationBase, "GetEvent.types.ps1xml"), filePath, StringComparison.OrdinalIgnoreCase))
			{
				this.ProcessTypeData(filePath, errors, GetEvent_Types_Ps1Xml.Get());
				result = true;
			}
			failToLoadFile = (count < errors.Count);
			return result;
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00066D68 File Offset: 0x00064F68
		internal void Update(TypeData type, Collection<string> errors, bool isRemove, bool clearTable)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (errors == null)
			{
				throw new ArgumentNullException("errors");
			}
			if (this.isShared)
			{
				throw PSTraceSource.NewInvalidOperationException(TypesXmlStrings.SharedTypeTableCannotBeUpdated, new object[0]);
			}
			lock (this.members)
			{
				this.Clear(clearTable);
				this.Update(errors, type, isRemove);
				TypeTable.StandardMembersUpdated();
			}
		}

		// Token: 0x040007CB RID: 1995
		internal const string PSStandardMembers = "PSStandardMembers";

		// Token: 0x040007CC RID: 1996
		internal const string SerializationDepth = "SerializationDepth";

		// Token: 0x040007CD RID: 1997
		internal const string StringSerializationSource = "StringSerializationSource";

		// Token: 0x040007CE RID: 1998
		internal const string SerializationMethodNode = "SerializationMethod";

		// Token: 0x040007CF RID: 1999
		internal const string TargetTypeForDeserialization = "TargetTypeForDeserialization";

		// Token: 0x040007D0 RID: 2000
		internal const string PropertySerializationSet = "PropertySerializationSet";

		// Token: 0x040007D1 RID: 2001
		internal const string InheritPropertySerializationSet = "InheritPropertySerializationSet";

		// Token: 0x040007D2 RID: 2002
		internal const string Types = "Types";

		// Token: 0x040007D3 RID: 2003
		internal const string Type = "Type";

		// Token: 0x040007D4 RID: 2004
		internal const string DefaultDisplayPropertySet = "DefaultDisplayPropertySet";

		// Token: 0x040007D5 RID: 2005
		internal const string DefaultKeyPropertySet = "DefaultKeyPropertySet";

		// Token: 0x040007D6 RID: 2006
		internal const string DefaultDisplayProperty = "DefaultDisplayProperty";

		// Token: 0x040007D7 RID: 2007
		internal const string IsHiddenAttribute = "IsHidden";

		// Token: 0x040007D8 RID: 2008
		internal const SerializationMethod defaultSerializationMethod = SerializationMethod.AllPublicProperties;

		// Token: 0x040007D9 RID: 2009
		internal const bool defaultInheritPropertySerializationSet = true;

		// Token: 0x040007DA RID: 2010
		private readonly Dictionary<string, PSMemberInfoInternalCollection<PSMemberInfo>> consolidatedMembers;

		// Token: 0x040007DB RID: 2011
		private readonly Dictionary<string, Collection<string>> consolidatedSpecificProperties;

		// Token: 0x040007DC RID: 2012
		private readonly Dictionary<string, PSMemberInfoInternalCollection<PSMemberInfo>> members;

		// Token: 0x040007DD RID: 2013
		private readonly Dictionary<string, object> typeConverters;

		// Token: 0x040007DE RID: 2014
		private readonly Dictionary<string, PSObject.AdapterSet> typeAdapters;

		// Token: 0x040007DF RID: 2015
		internal readonly bool isShared;

		// Token: 0x040007E0 RID: 2016
		private List<string> typeFileList;

		// Token: 0x040007E1 RID: 2017
		internal InitialSessionStateEntryCollection<SessionStateTypeEntry> typesInfo;

		// Token: 0x040007E2 RID: 2018
		private static readonly string[] standardMembers = new string[]
		{
			"DefaultDisplayProperty",
			"DefaultDisplayPropertySet",
			"DefaultKeyPropertySet",
			"SerializationMethod",
			"SerializationDepth",
			"StringSerializationSource",
			"PropertySerializationSet",
			"InheritPropertySerializationSet",
			"TargetTypeForDeserialization"
		};
	}
}
