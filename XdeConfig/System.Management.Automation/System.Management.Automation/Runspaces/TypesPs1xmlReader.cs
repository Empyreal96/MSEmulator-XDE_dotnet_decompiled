using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Language;
using System.Reflection;
using System.Xml;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000158 RID: 344
	internal class TypesPs1xmlReader
	{
		// Token: 0x06001183 RID: 4483 RVA: 0x00060B4B File Offset: 0x0005ED4B
		public TypesPs1xmlReader(LoadContext context)
		{
			this._context = context;
			this._reader = context.reader;
			this._readerLineInfo = (IXmlLineInfo)context.reader;
			this.InitIDs();
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00060B80 File Offset: 0x0005ED80
		private string ReadElementString(string nodeName)
		{
			string text = string.Empty;
			if (this._reader.MoveToContent() != XmlNodeType.Element)
			{
				this._context.AddError(this._readerLineInfo.LineNumber, TypesXmlStrings.NodeShouldHaveInnerText, new object[]
				{
					nodeName
				});
				return null;
			}
			if (!this._reader.IsEmptyElement)
			{
				this._reader.Read();
				while (this._reader.NodeType == XmlNodeType.Text)
				{
					text += this._reader.Value;
					if (!this._reader.Read())
					{
						break;
					}
				}
				if (this._reader.NodeType != XmlNodeType.EndElement)
				{
					this._context.AddError(this._readerLineInfo.LineNumber, TypesXmlStrings.NodeShouldHaveInnerText, new object[]
					{
						nodeName
					});
					return null;
				}
				text = text.Trim();
				this._reader.Read();
			}
			else
			{
				this._reader.Read();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this._context.AddError(this._readerLineInfo.LineNumber, TypesXmlStrings.NodeShouldHaveInnerText, new object[]
				{
					nodeName
				});
				return null;
			}
			return text;
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00060CA0 File Offset: 0x0005EEA0
		private bool? ReadIsHiddenAttribute()
		{
			bool? result = null;
			while (this._reader.MoveToNextAttribute())
			{
				if (result == null && this._reader.LocalName == this._idIsHidden)
				{
					result = new bool?(this.ToBoolean(this._reader.Value));
				}
			}
			return result;
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00060CFC File Offset: 0x0005EEFC
		private void ReadIsHiddenAttributeNotSupported(string node)
		{
			if (this.ReadIsHiddenAttribute() != null)
			{
				this._context.AddError(TypesXmlStrings.IsHiddenNotSupported, new object[]
				{
					node,
					this._idIsHidden
				});
			}
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00060D40 File Offset: 0x0005EF40
		private void ReadEndElement()
		{
			while (this._reader.NodeType == XmlNodeType.Whitespace)
			{
				this._reader.Skip();
			}
			if (this._reader.NodeType == XmlNodeType.None)
			{
				this._reader.Skip();
				return;
			}
			this._reader.ReadEndElement();
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00060D90 File Offset: 0x0005EF90
		private void UnknownNode(string node, string expectedNodes)
		{
			if (this._reader.NodeType == XmlNodeType.Text)
			{
				this._context.AddError(this._readerLineInfo.LineNumber, TypesXmlStrings.NodeShouldNotHaveInnerText, new object[]
				{
					node
				});
				this._reader.Read();
				return;
			}
			this._context.AddError(this._readerLineInfo.LineNumber, TypesXmlStrings.UnknownNode, new object[]
			{
				this._reader.LocalName,
				expectedNodes
			});
			this.SkipUntillNodeEnd(this._reader.LocalName);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x00060E28 File Offset: 0x0005F028
		private void SkipUntillNodeEnd(string nodeName)
		{
			while (this._reader.Read())
			{
				if (this._reader.IsStartElement() && this._reader.LocalName.Equals(nodeName))
				{
					this.SkipUntillNodeEnd(nodeName);
				}
				else if (this._reader.NodeType == XmlNodeType.EndElement && this._reader.LocalName.Equals(nodeName))
				{
					return;
				}
			}
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00060E90 File Offset: 0x0005F090
		private void NotMoreThanOnce(string node, string parent)
		{
			this._context.AddError(this._readerLineInfo.LineNumber, TypesXmlStrings.NotMoreThanOnceOne, new object[]
			{
				node,
				parent
			});
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x00060EC8 File Offset: 0x0005F0C8
		private void NodeNotFound(int lineNumber, string node, string parent)
		{
			this._context.AddError(lineNumber, TypesXmlStrings.NodeNotFoundOnce, new object[]
			{
				node,
				parent
			});
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00060EF8 File Offset: 0x0005F0F8
		private ScriptBlock GetScriptBlock(string text, int initialLine)
		{
			if (text == null)
			{
				return null;
			}
			ScriptBlock scriptBlock;
			try
			{
				scriptBlock = ScriptBlock.Create(text);
			}
			catch (ParseException ex)
			{
				this._context.AddError(ex.Errors[0].Extent.StartLineNumber + initialLine - 1, ex.Errors[0].Message, new object[0]);
				return null;
			}
			if (scriptBlock != null && this._context.IsFullyTrusted)
			{
				scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
			}
			return scriptBlock;
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00060F7C File Offset: 0x0005F17C
		private Type ResolveType(string typeName, int line)
		{
			Exception ex;
			Type type = TypeResolver.ResolveType(typeName, out ex);
			if (ex != null)
			{
				this._context.AddError(line, ex.Message, new object[0]);
			}
			else if (type == null)
			{
				this._context.AddError(line, ParserStrings.TypeNotFound, new object[]
				{
					typeName
				});
			}
			return type;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x00060FD8 File Offset: 0x0005F1D8
		private bool ToBoolean(string value)
		{
			value = value.Trim();
			if (string.Equals(value, "true", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (string.Equals(value, "false", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			this._context.AddError(this._readerLineInfo.LineNumber, TypesXmlStrings.ValueShouldBeTrueOrFalse, new object[]
			{
				value
			});
			return false;
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00061038 File Offset: 0x0005F238
		private T Converter<T>(object value, string name)
		{
			T result;
			if (!LanguagePrimitives.TryConvertTo<T>(value, out result))
			{
				this._context.AddError(TypesXmlStrings.ErrorConvertingNote, new object[]
				{
					name,
					typeof(T)
				});
			}
			return result;
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0006107C File Offset: 0x0005F27C
		private bool BoolConverter(object value, string name)
		{
			string text = value as string;
			if (text != null)
			{
				return this.ToBoolean(text);
			}
			return this.Converter<bool>(value, name);
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x000610A4 File Offset: 0x0005F2A4
		private void CheckStandardNote<T>(TypeMemberData member, TypeData typeData, Action<TypeData, T> setter, Func<object, string, T> converter)
		{
			NotePropertyData notePropertyData = member as NotePropertyData;
			if (notePropertyData != null)
			{
				T arg;
				if (notePropertyData.Value.GetType() != typeof(T))
				{
					arg = converter(notePropertyData.Value, notePropertyData.Name);
				}
				else
				{
					arg = (T)((object)notePropertyData.Value);
				}
				setter(typeData, arg);
				return;
			}
			this._context.AddError(TypesXmlStrings.MemberShouldBeNote, new object[]
			{
				member.Name
			});
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00061124 File Offset: 0x0005F324
		private bool CheckStandardPropertySet(TypeMemberData member, TypeData typeData, Action<TypeData, PropertySetData> setter)
		{
			PropertySetData propertySetData = member as PropertySetData;
			if (propertySetData != null)
			{
				setter(typeData, propertySetData);
				return true;
			}
			return false;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x00061148 File Offset: 0x0005F348
		public IEnumerable<TypeData> Read()
		{
			IEnumerable<TypeData> enumerable = null;
			this._reader.MoveToContent();
			if (this._reader.NodeType == XmlNodeType.Element && this._reader.LocalName == this._idTypes)
			{
				enumerable = this.Read_Types();
			}
			if (enumerable == null)
			{
				this.NodeNotFound(0, this._idTypes, "Document");
			}
			return enumerable;
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x000613AC File Offset: 0x0005F5AC
		private IEnumerable<TypeData> Read_Types()
		{
			this.ReadIsHiddenAttributeNotSupported(this._idTypes);
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idType)
						{
							TypeData p = this.Read_Type();
							if (p != null)
							{
								yield return p;
							}
						}
						else
						{
							this.UnknownNode(this._idTypes, "Type");
						}
					}
					else
					{
						this.UnknownNode(this._idTypes, "Type");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			yield break;
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00061414 File Offset: 0x0005F614
		private TypeData Read_Type()
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			MemberSetData memberSetData = null;
			string text = null;
			Collection<TypeMemberData> collection = null;
			Type type = null;
			Type type2 = null;
			this.ReadIsHiddenAttributeNotSupported(this._idType);
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idName, this._idType);
							}
							text = this.ReadElementString(this._idName);
						}
						else if (this._reader.LocalName == this._idMembers)
						{
							if (collection != null)
							{
								this.NotMoreThanOnce(this._idMembers, this._idType);
							}
							collection = this.Read_Members(out memberSetData);
						}
						else if (this._reader.LocalName == this._idTypeConverter)
						{
							if (type != null)
							{
								this.NotMoreThanOnce(this._idTypeConverter, this._idType);
							}
							type = this.Read_TypeX(this._idTypeConverter);
						}
						else if (this._reader.LocalName == this._idTypeAdapter)
						{
							if (type2 != null)
							{
								this.NotMoreThanOnce(this._idTypeAdapter, this._idType);
							}
							type2 = this.Read_TypeX(this._idTypeAdapter);
						}
						else
						{
							this.UnknownNode(this._idType, "Name,Members,TypeConverter,TypeAdapter");
						}
					}
					else
					{
						this.UnknownNode(this._idType, "Name,Members,TypeConverter,TypeAdapter");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NodeNotFound(lineNumber, this._idName, this._idType);
			}
			else if (collection == null && type == null && type2 == null)
			{
				this._context.AddError(text, lineNumber, TypesXmlStrings.TypeNodeShouldHaveMembersOrTypeConverters, new object[0]);
			}
			if (this._context.errors.Count != count)
			{
				return null;
			}
			TypeData typeData = new TypeData(text, true);
			if (collection != null)
			{
				foreach (TypeMemberData typeMemberData in collection)
				{
					typeData.Members.Add(typeMemberData.Name, typeMemberData);
				}
			}
			typeData.TypeAdapter = type2;
			typeData.TypeConverter = type;
			if (memberSetData != null)
			{
				foreach (TypeMemberData typeMemberData2 in memberSetData.Members)
				{
					if (typeMemberData2.Name.Equals("DefaultDisplayProperty", StringComparison.OrdinalIgnoreCase))
					{
						this.CheckStandardNote<string>(typeMemberData2, typeData, delegate(TypeData t, string v)
						{
							t.DefaultDisplayProperty = v;
						}, new Func<object, string, string>(this.Converter<string>));
					}
					else if (typeMemberData2.Name.Equals("DefaultDisplayPropertySet", StringComparison.OrdinalIgnoreCase))
					{
						if (!this.CheckStandardPropertySet(typeMemberData2, typeData, delegate(TypeData t, PropertySetData p)
						{
							t.DefaultDisplayPropertySet = p;
						}))
						{
							this._context.AddError(TypesXmlStrings.MemberShouldHaveType, new object[]
							{
								"DefaultDisplayPropertySet",
								this._idPropertySet
							});
						}
					}
					else if (typeMemberData2.Name.Equals("DefaultKeyPropertySet", StringComparison.OrdinalIgnoreCase))
					{
						if (!this.CheckStandardPropertySet(typeMemberData2, typeData, delegate(TypeData t, PropertySetData p)
						{
							t.DefaultKeyPropertySet = p;
						}))
						{
							this._context.AddError(TypesXmlStrings.MemberShouldHaveType, new object[]
							{
								"DefaultKeyPropertySet",
								this._idPropertySet
							});
						}
					}
					else if (typeMemberData2.Name.Equals("SerializationMethod", StringComparison.OrdinalIgnoreCase))
					{
						this.CheckStandardNote<string>(typeMemberData2, typeData, delegate(TypeData t, string v)
						{
							t.SerializationMethod = v;
						}, new Func<object, string, string>(this.Converter<string>));
					}
					else if (typeMemberData2.Name.Equals("SerializationDepth", StringComparison.OrdinalIgnoreCase))
					{
						this.CheckStandardNote<uint>(typeMemberData2, typeData, delegate(TypeData t, uint v)
						{
							t.SerializationDepth = v;
						}, new Func<object, string, uint>(this.Converter<uint>));
					}
					else if (typeMemberData2.Name.Equals("StringSerializationSource", StringComparison.OrdinalIgnoreCase))
					{
						AliasPropertyData aliasPropertyData = typeMemberData2 as AliasPropertyData;
						if (aliasPropertyData != null)
						{
							typeData.StringSerializationSource = aliasPropertyData.ReferencedMemberName;
						}
						else
						{
							typeData.StringSerializationSourceProperty = typeMemberData2;
						}
					}
					else if (typeMemberData2.Name.Equals("PropertySerializationSet", StringComparison.OrdinalIgnoreCase))
					{
						if (!this.CheckStandardPropertySet(typeMemberData2, typeData, delegate(TypeData t, PropertySetData p)
						{
							t.PropertySerializationSet = p;
						}))
						{
							this._context.AddError(TypesXmlStrings.MemberShouldHaveType, new object[]
							{
								"PropertySerializationSet",
								this._idPropertySet
							});
						}
					}
					else if (typeMemberData2.Name.Equals("InheritPropertySerializationSet", StringComparison.OrdinalIgnoreCase))
					{
						this.CheckStandardNote<bool>(typeMemberData2, typeData, delegate(TypeData t, bool v)
						{
							t.InheritPropertySerializationSet = v;
						}, new Func<object, string, bool>(this.BoolConverter));
					}
					else if (typeMemberData2.Name.Equals("TargetTypeForDeserialization", StringComparison.OrdinalIgnoreCase))
					{
						this.CheckStandardNote<Type>(typeMemberData2, typeData, delegate(TypeData t, Type v)
						{
							t.TargetTypeForDeserialization = v;
						}, new Func<object, string, Type>(this.Converter<Type>));
					}
					else
					{
						this._context.AddError(TypesXmlStrings.NotAStandardMember, new object[]
						{
							typeMemberData2.Name
						});
					}
				}
			}
			return typeData;
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00061A58 File Offset: 0x0005FC58
		private Type Read_TypeX(string elementName)
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			int line = 0;
			string text = null;
			this.ReadIsHiddenAttributeNotSupported(elementName);
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idTypeName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idTypeName, elementName);
							}
							line = this._readerLineInfo.LineNumber;
							text = this.ReadElementString(this._idTypeName);
						}
						else
						{
							this.UnknownNode(elementName, "TypeName");
						}
					}
					else
					{
						this.UnknownNode(elementName, "TypeName");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NodeNotFound(lineNumber, this._idTypeName, elementName);
			}
			if (this._context.errors.Count != count)
			{
				return null;
			}
			return this.ResolveType(text, line);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00061B9C File Offset: 0x0005FD9C
		private Collection<TypeMemberData> Read_Members(out MemberSetData standardMembers)
		{
			int count = this._context.errors.Count;
			standardMembers = null;
			Collection<TypeMemberData> collection = new Collection<TypeMemberData>();
			this.ReadIsHiddenAttributeNotSupported(this._idMembers);
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idNoteProperty)
						{
							NotePropertyData notePropertyData = this.Read_NoteProperty();
							if (notePropertyData != null)
							{
								collection.Add(notePropertyData);
							}
						}
						else if (this._reader.LocalName == this._idAliasProperty)
						{
							AliasPropertyData aliasPropertyData = this.Read_AliasProperty();
							if (aliasPropertyData != null)
							{
								collection.Add(aliasPropertyData);
							}
						}
						else if (this._reader.LocalName == this._idScriptProperty)
						{
							ScriptPropertyData scriptPropertyData = this.Read_ScriptProperty();
							if (scriptPropertyData != null)
							{
								collection.Add(scriptPropertyData);
							}
						}
						else if (this._reader.LocalName == this._idCodeProperty)
						{
							CodePropertyData codePropertyData = this.Read_CodeProperty();
							if (codePropertyData != null)
							{
								collection.Add(codePropertyData);
							}
						}
						else if (this._reader.LocalName == this._idScriptMethod)
						{
							ScriptMethodData scriptMethodData = this.Read_ScriptMethod();
							if (scriptMethodData != null)
							{
								collection.Add(scriptMethodData);
							}
						}
						else if (this._reader.LocalName == this._idCodeMethod)
						{
							CodeMethodData codeMethodData = this.Read_CodeMethod();
							if (codeMethodData != null)
							{
								collection.Add(codeMethodData);
							}
						}
						else if (this._reader.LocalName == this._idPropertySet)
						{
							PropertySetData propertySetData = this.Read_PropertySet();
							if (propertySetData != null)
							{
								collection.Add(propertySetData);
							}
						}
						else if (this._reader.LocalName == this._idMemberSet)
						{
							MemberSetData memberSetData = this.Read_MemberSet();
							if (memberSetData != null)
							{
								if (memberSetData.Name.Equals("PSStandardMembers", StringComparison.OrdinalIgnoreCase))
								{
									standardMembers = memberSetData;
								}
								else
								{
									collection.Add(memberSetData);
								}
							}
						}
						else
						{
							this.UnknownNode(this._idMembers, "NoteProperty,AliasProperty,ScriptProperty,CodeProperty,ScriptMethod,CodeMethod,PropertySet,MemberSet");
						}
					}
					else
					{
						this.UnknownNode(this._idMembers, "NoteProperty,AliasProperty,ScriptProperty,CodeProperty,ScriptMethod,CodeMethod,PropertySet,MemberSet");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (this._context.errors.Count != count)
			{
				return null;
			}
			return collection;
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00061E10 File Offset: 0x00060010
		private MemberSetData Read_MemberSet()
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			string text = null;
			Collection<TypeMemberData> collection = null;
			bool? flag = null;
			bool? flag2 = this.ReadIsHiddenAttribute();
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idName, this._idMemberSet);
							}
							text = this.ReadElementString(this._idName);
						}
						else if (this._reader.LocalName == this._idInheritMembers)
						{
							if (flag != null)
							{
								this.NotMoreThanOnce(this._idInheritMembers, this._idMemberSet);
							}
							flag = new bool?(this.ToBoolean(this.ReadElementString(this._idMemberSet)));
						}
						else if (this._reader.LocalName == this._idMembers)
						{
							if (collection != null)
							{
								this.NotMoreThanOnce(this._idMembers, this._idMemberSet);
							}
							MemberSetData memberSetData;
							collection = this.Read_Members(out memberSetData);
							if (memberSetData != null)
							{
								collection.Add(memberSetData);
							}
						}
						else
						{
							this.UnknownNode(this._idMemberSet, "Name,InheritMembers,Members");
						}
					}
					else
					{
						this.UnknownNode(this._idMemberSet, "Name,InheritMembers,Members");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NodeNotFound(lineNumber, this._idName, this._idMemberSet);
			}
			if (collection == null)
			{
				collection = new Collection<TypeMemberData>();
			}
			if (this._context.errors.Count != count)
			{
				return null;
			}
			MemberSetData memberSetData2 = new MemberSetData(text, collection)
			{
				IsHidden = flag2.GetValueOrDefault()
			};
			if (flag != null)
			{
				memberSetData2.InheritMembers = flag.Value;
			}
			return memberSetData2;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00062028 File Offset: 0x00060228
		private PropertySetData Read_PropertySet()
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			string text = null;
			List<string> list = null;
			bool? flag = this.ReadIsHiddenAttribute();
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idName, this._idPropertySet);
							}
							text = this.ReadElementString(this._idName);
						}
						else if (this._reader.LocalName == this._idReferencedProperties)
						{
							if (this._reader.IsEmptyElement)
							{
								this._reader.Skip();
							}
							else
							{
								this._reader.ReadStartElement();
								this._reader.MoveToContent();
								while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
								{
									if (this._reader.NodeType == XmlNodeType.Element)
									{
										if (this._reader.LocalName == this._idName)
										{
											if (list == null)
											{
												list = new List<string>(8);
											}
											list.Add(this.ReadElementString(this._idName));
										}
										else
										{
											this.UnknownNode(this._idPropertySet, "Name");
										}
									}
									else
									{
										this.UnknownNode(this._idPropertySet, "Name");
									}
									this._reader.MoveToContent();
								}
								this.ReadEndElement();
							}
						}
						else
						{
							this.UnknownNode(this._idPropertySet, "Name,ReferencedProperties");
						}
					}
					else
					{
						this.UnknownNode(this._idPropertySet, "Name,ReferencedProperties");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NodeNotFound(lineNumber, this._idName, this._idPropertySet);
			}
			if (list == null)
			{
				this.NodeNotFound(lineNumber, this._idReferencedProperties, this._idPropertySet);
			}
			if (this._context.errors.Count != count)
			{
				return null;
			}
			return new PropertySetData(list)
			{
				Name = text,
				IsHidden = flag.GetValueOrDefault()
			};
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0006228C File Offset: 0x0006048C
		private CodeMethodData Read_CodeMethod()
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			int num = 0;
			string text = null;
			MethodInfo methodInfo = null;
			this.ReadIsHiddenAttributeNotSupported(this._idCodeMethod);
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idName, this._idCodeMethod);
							}
							text = this.ReadElementString(this._idName);
						}
						else if (this._reader.LocalName == this._idCodeReference)
						{
							if (methodInfo != null)
							{
								this.NotMoreThanOnce(this._idCodeReference, this._idCodeMethod);
							}
							num = this._readerLineInfo.LineNumber;
							methodInfo = this.Read_CodeReference();
							if (methodInfo == null)
							{
								this._context.AddError(num, ExtendedTypeSystem.CodeMethodMethodFormat, new object[0]);
							}
						}
						else
						{
							this.UnknownNode(this._idCodeMethod, "Name,CodeReference");
						}
					}
					else
					{
						this.UnknownNode(this._idCodeMethod, "Name,CodeReference");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NodeNotFound(lineNumber, this._idName, this._idCodeMethod);
			}
			if (methodInfo == null && num == 0)
			{
				this.NodeNotFound(lineNumber, this._idCodeReference, this._idCodeMethod);
			}
			if (methodInfo != null && !PSCodeMethod.CheckMethodInfo(methodInfo))
			{
				this._context.AddError(num, ExtendedTypeSystem.CodeMethodMethodFormat, new object[0]);
			}
			if (this._context.errors.Count != count)
			{
				return null;
			}
			return new CodeMethodData(text, methodInfo);
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00062498 File Offset: 0x00060698
		private MethodInfo Read_CodeReference()
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			int line = 0;
			int errorLineNumber = 0;
			string text = null;
			string text2 = null;
			this.ReadIsHiddenAttributeNotSupported(this._idCodeReference);
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idTypeName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idTypeName, this._idCodeReference);
							}
							line = this._readerLineInfo.LineNumber;
							text = this.ReadElementString(this._idTypeName);
						}
						else if (this._reader.LocalName == this._idMethodName)
						{
							if (text2 != null)
							{
								this.NotMoreThanOnce(this._idMethodName, this._idCodeReference);
							}
							errorLineNumber = this._readerLineInfo.LineNumber;
							text2 = this.ReadElementString(this._idMethodName);
						}
						else
						{
							this.UnknownNode(this._idCodeReference, "TypeName,MethodName");
						}
					}
					else
					{
						this.UnknownNode(this._idCodeReference, "TypeName,MethodName");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NodeNotFound(lineNumber, this._idTypeName, this._idCodeReference);
			}
			if (string.IsNullOrWhiteSpace(text2))
			{
				this.NodeNotFound(lineNumber, this._idMethodName, this._idCodeReference);
			}
			if (this._context.errors.Count != count)
			{
				return null;
			}
			MethodInfo result = null;
			Type type = this.ResolveType(text, line);
			if (type != null)
			{
				try
				{
					result = type.GetMethod(text2, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
				}
				catch (AmbiguousMatchException ex)
				{
					this._context.AddError(errorLineNumber, ex.Message, new object[0]);
				}
			}
			return result;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x000626B0 File Offset: 0x000608B0
		private ScriptMethodData Read_ScriptMethod()
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			string text = null;
			string text2 = null;
			int initialLine = 0;
			this.ReadIsHiddenAttributeNotSupported(this._idScriptMethod);
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idName, this._idScriptMethod);
							}
							text = this.ReadElementString(this._idName);
						}
						else if (this._reader.LocalName == this._idScript)
						{
							if (text2 != null)
							{
								this.NotMoreThanOnce(this._idScript, this._idScriptMethod);
							}
							initialLine = this._readerLineInfo.LineNumber;
							text2 = this.ReadElementString(this._idScript);
						}
						else
						{
							this.UnknownNode(this._idScriptMethod, "Name,Script");
						}
					}
					else
					{
						this.UnknownNode(this._idScriptMethod, "Name,Script");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NodeNotFound(lineNumber, this._idName, this._idScriptMethod);
			}
			if (string.IsNullOrWhiteSpace(text2))
			{
				this.NodeNotFound(lineNumber, this._idScript, this._idScriptMethod);
			}
			ScriptBlock scriptBlock = this.GetScriptBlock(text2, initialLine);
			if (this._context.errors.Count != count)
			{
				return null;
			}
			return new ScriptMethodData(text, scriptBlock);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00062874 File Offset: 0x00060A74
		private CodePropertyData Read_CodeProperty()
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			int num = 0;
			int num2 = 0;
			string text = null;
			MethodInfo methodInfo = null;
			MethodInfo methodInfo2 = null;
			bool? flag = this.ReadIsHiddenAttribute();
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idName, this._idCodeProperty);
							}
							text = this.ReadElementString(this._idName);
						}
						else if (this._reader.LocalName == this._idGetCodeReference)
						{
							if (methodInfo != null)
							{
								this.NotMoreThanOnce(this._idGetCodeReference, this._idCodeProperty);
							}
							num = this._readerLineInfo.LineNumber;
							methodInfo = this.Read_CodeReference();
							if (methodInfo == null)
							{
								this._context.AddError(num, ExtendedTypeSystem.CodePropertyGetterAndSetterNull, new object[0]);
							}
						}
						else if (this._reader.LocalName == this._idSetCodeReference)
						{
							if (methodInfo2 != null)
							{
								this.NotMoreThanOnce(this._idSetCodeReference, this._idCodeProperty);
							}
							num2 = this._readerLineInfo.LineNumber;
							methodInfo2 = this.Read_CodeReference();
							if (methodInfo2 == null)
							{
								this._context.AddError(num2, ExtendedTypeSystem.CodePropertyGetterAndSetterNull, new object[0]);
							}
						}
						else
						{
							this.UnknownNode(this._idCodeProperty, "Name,GetCodeReference,SetCodeReference");
						}
					}
					else
					{
						this.UnknownNode(this._idCodeProperty, "Name,GetCodeReference,SetCodeReference");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrEmpty(text))
			{
				this.NodeNotFound(lineNumber, this._idName, this._idCodeProperty);
			}
			if (methodInfo == null && methodInfo2 == null && num == 0 && num2 == 0)
			{
				this._context.AddError(lineNumber, TypesXmlStrings.CodePropertyShouldHaveGetterOrSetter, new object[0]);
			}
			if (methodInfo != null && !PSCodeProperty.CheckGetterMethodInfo(methodInfo))
			{
				this._context.AddError(num, ExtendedTypeSystem.CodePropertyGetterFormat, new object[0]);
			}
			if (methodInfo2 != null && !PSCodeProperty.CheckSetterMethodInfo(methodInfo2, methodInfo))
			{
				this._context.AddError(num2, ExtendedTypeSystem.CodePropertySetterFormat, new object[0]);
			}
			if (this._context.errors.Count != count)
			{
				return null;
			}
			return new CodePropertyData(text, methodInfo, methodInfo2)
			{
				IsHidden = flag.GetValueOrDefault()
			};
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00062B44 File Offset: 0x00060D44
		private ScriptPropertyData Read_ScriptProperty()
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			int initialLine = 0;
			int initialLine2 = 0;
			string text = null;
			string text2 = null;
			string text3 = null;
			bool? flag = this.ReadIsHiddenAttribute();
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idName, this._idScriptProperty);
							}
							text = this.ReadElementString(this._idName);
						}
						else if (this._reader.LocalName == this._idGetScriptBlock)
						{
							if (text2 != null)
							{
								this.NotMoreThanOnce(this._idGetScriptBlock, this._idScriptProperty);
							}
							initialLine = this._readerLineInfo.LineNumber;
							text2 = this.ReadElementString(this._idGetScriptBlock);
						}
						else if (this._reader.LocalName == this._idSetScriptBlock)
						{
							if (text3 != null)
							{
								this.NotMoreThanOnce(this._idSetScriptBlock, this._idScriptProperty);
							}
							initialLine2 = this._readerLineInfo.LineNumber;
							text3 = this.ReadElementString(this._idSetScriptBlock);
						}
						else
						{
							this.UnknownNode(this._idScriptProperty, "Name,GetScriptBlock,SetScriptBlock");
						}
					}
					else
					{
						this.UnknownNode(this._idScriptProperty, "Name,GetScriptBlock,SetScriptBlock");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NodeNotFound(lineNumber, this._idName, this._idScriptProperty);
			}
			if (string.IsNullOrWhiteSpace(text2) && string.IsNullOrWhiteSpace(text3))
			{
				this._context.AddError(lineNumber, TypesXmlStrings.ScriptPropertyShouldHaveGetterOrSetter, new object[0]);
			}
			ScriptBlock scriptBlock = this.GetScriptBlock(text2, initialLine);
			ScriptBlock scriptBlock2 = this.GetScriptBlock(text3, initialLine2);
			if (this._context.errors.Count != count)
			{
				return null;
			}
			return new ScriptPropertyData(text, scriptBlock, scriptBlock2)
			{
				IsHidden = flag.GetValueOrDefault()
			};
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00062D84 File Offset: 0x00060F84
		private AliasPropertyData Read_AliasProperty()
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			int line = 0;
			string text = null;
			string text2 = null;
			string text3 = null;
			bool? flag = this.ReadIsHiddenAttribute();
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idName, this._idAliasProperty);
							}
							text = this.ReadElementString(this._idAliasProperty);
						}
						else if (this._reader.LocalName == this._idReferencedMemberName)
						{
							if (text2 != null)
							{
								this.NotMoreThanOnce(this._idReferencedMemberName, this._idAliasProperty);
							}
							text2 = this.ReadElementString(this._idReferencedMemberName);
						}
						else if (this._reader.LocalName == this._idTypeName)
						{
							if (text3 != null)
							{
								this.NotMoreThanOnce(this._idTypeName, this._idAliasProperty);
							}
							line = this._readerLineInfo.LineNumber;
							text3 = this.ReadElementString(this._idTypeName);
						}
						else
						{
							this.UnknownNode(this._idAliasProperty, "Name,ReferencedMemberName,TypeName");
						}
					}
					else
					{
						this.UnknownNode(this._idAliasProperty, "Name,ReferencedMemberName,TypeName");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NodeNotFound(lineNumber, this._idName, this._idAliasProperty);
			}
			if (string.IsNullOrWhiteSpace(text2))
			{
				this.NodeNotFound(lineNumber, this._idReferencedMemberName, this._idAliasProperty);
			}
			Type type = (text3 != null) ? this.ResolveType(text3, line) : null;
			if (this._context.errors.Count != count)
			{
				return null;
			}
			return new AliasPropertyData(text, text2, type)
			{
				IsHidden = flag.GetValueOrDefault()
			};
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00062FA0 File Offset: 0x000611A0
		private NotePropertyData Read_NoteProperty()
		{
			int count = this._context.errors.Count;
			int lineNumber = this._readerLineInfo.LineNumber;
			int num = 0;
			string text = null;
			string text2 = null;
			string text3 = null;
			bool? flag = this.ReadIsHiddenAttribute();
			this._reader.MoveToElement();
			if (this._reader.IsEmptyElement)
			{
				this._reader.Skip();
			}
			else
			{
				this._reader.ReadStartElement();
				this._reader.MoveToContent();
				while (this._reader.NodeType != XmlNodeType.EndElement && this._reader.NodeType != XmlNodeType.None)
				{
					if (this._reader.NodeType == XmlNodeType.Element)
					{
						if (this._reader.LocalName == this._idName)
						{
							if (text != null)
							{
								this.NotMoreThanOnce(this._idName, this._idNoteProperty);
							}
							text = this.ReadElementString(this._idName);
						}
						else if (this._reader.LocalName == this._idValue)
						{
							if (text2 != null)
							{
								this.NotMoreThanOnce(this._idValue, this._idNoteProperty);
							}
							text2 = this.ReadElementString(this._idValue);
						}
						else if (this._reader.LocalName == this._idTypeName)
						{
							if (text3 != null)
							{
								this.NotMoreThanOnce(this._idTypeName, this._idNoteProperty);
							}
							num = this._readerLineInfo.LineNumber;
							text3 = this.ReadElementString(this._idTypeName);
						}
						else
						{
							this.UnknownNode(this._idNoteProperty, "Name,Value,TypeName");
						}
					}
					else
					{
						this.UnknownNode(this._idNoteProperty, "Name,Value,TypeName");
					}
					this._reader.MoveToContent();
				}
				this.ReadEndElement();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				this.NodeNotFound(lineNumber, this._idName, this._idNoteProperty);
			}
			if (string.IsNullOrWhiteSpace(text2))
			{
				this.NodeNotFound(lineNumber, this._idValue, this._idNoteProperty);
			}
			object obj = text2;
			Type type = (text3 != null) ? this.ResolveType(text3, num) : null;
			if (type != null)
			{
				try
				{
					obj = LanguagePrimitives.ConvertTo(obj, type, CultureInfo.InvariantCulture);
				}
				catch (PSInvalidCastException ex)
				{
					this._context.AddError(num, ex.Message, new object[0]);
				}
			}
			if (this._context.errors.Count != count)
			{
				return null;
			}
			return new NotePropertyData(text, obj)
			{
				IsHidden = flag.GetValueOrDefault()
			};
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00063204 File Offset: 0x00061404
		protected void InitIDs()
		{
			XmlNameTable nameTable = this._reader.NameTable;
			this._idTypeName = nameTable.Add("TypeName");
			this._idSetCodeReference = nameTable.Add("SetCodeReference");
			this._idScriptMethod = nameTable.Add("ScriptMethod");
			this._idValue = nameTable.Add("Value");
			this._idScriptProperty = nameTable.Add("ScriptProperty");
			this._idIsHidden = nameTable.Add("IsHidden");
			this._idScript = nameTable.Add("Script");
			this._idCodeMethod = nameTable.Add("CodeMethod");
			this._idMemberSet = nameTable.Add("MemberSet");
			this._idCodeProperty = nameTable.Add("CodeProperty");
			this._idMembers = nameTable.Add("Members");
			this._idName = nameTable.Add("Name");
			this._idGetCodeReference = nameTable.Add("GetCodeReference");
			this._idMethodName = nameTable.Add("MethodName");
			this._idInheritMembers = nameTable.Add("InheritMembers");
			this._idNoteProperty = nameTable.Add("NoteProperty");
			this._idTypeAdapter = nameTable.Add("TypeAdapter");
			this._idSetScriptBlock = nameTable.Add("SetScriptBlock");
			this._idPropertySet = nameTable.Add("PropertySet");
			this._idTypeConverter = nameTable.Add("TypeConverter");
			this._idAliasProperty = nameTable.Add("AliasProperty");
			this._idType = nameTable.Add("Type");
			this._idCodeReference = nameTable.Add("CodeReference");
			this._idTypes = nameTable.Add("Types");
			this._idReferencedProperties = nameTable.Add("ReferencedProperties");
			this._idGetScriptBlock = nameTable.Add("GetScriptBlock");
			this._idReferencedMemberName = nameTable.Add("ReferencedMemberName");
		}

		// Token: 0x04000771 RID: 1905
		private readonly LoadContext _context;

		// Token: 0x04000772 RID: 1906
		private readonly XmlReader _reader;

		// Token: 0x04000773 RID: 1907
		private readonly IXmlLineInfo _readerLineInfo;

		// Token: 0x04000774 RID: 1908
		private string _idTypeName;

		// Token: 0x04000775 RID: 1909
		private string _idSetCodeReference;

		// Token: 0x04000776 RID: 1910
		private string _idScriptMethod;

		// Token: 0x04000777 RID: 1911
		private string _idValue;

		// Token: 0x04000778 RID: 1912
		private string _idScriptProperty;

		// Token: 0x04000779 RID: 1913
		private string _idIsHidden;

		// Token: 0x0400077A RID: 1914
		private string _idScript;

		// Token: 0x0400077B RID: 1915
		private string _idCodeMethod;

		// Token: 0x0400077C RID: 1916
		private string _idMemberSet;

		// Token: 0x0400077D RID: 1917
		private string _idCodeProperty;

		// Token: 0x0400077E RID: 1918
		private string _idMembers;

		// Token: 0x0400077F RID: 1919
		private string _idName;

		// Token: 0x04000780 RID: 1920
		private string _idGetCodeReference;

		// Token: 0x04000781 RID: 1921
		private string _idMethodName;

		// Token: 0x04000782 RID: 1922
		private string _idInheritMembers;

		// Token: 0x04000783 RID: 1923
		private string _idNoteProperty;

		// Token: 0x04000784 RID: 1924
		private string _idTypeAdapter;

		// Token: 0x04000785 RID: 1925
		private string _idSetScriptBlock;

		// Token: 0x04000786 RID: 1926
		private string _idPropertySet;

		// Token: 0x04000787 RID: 1927
		private string _idTypeConverter;

		// Token: 0x04000788 RID: 1928
		private string _idAliasProperty;

		// Token: 0x04000789 RID: 1929
		private string _idType;

		// Token: 0x0400078A RID: 1930
		private string _idCodeReference;

		// Token: 0x0400078B RID: 1931
		private string _idTypes;

		// Token: 0x0400078C RID: 1932
		private string _idReferencedProperties;

		// Token: 0x0400078D RID: 1933
		private string _idGetScriptBlock;

		// Token: 0x0400078E RID: 1934
		private string _idReferencedMemberName;
	}
}
