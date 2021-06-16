using System;
using System.Collections.Generic;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200015C RID: 348
	public sealed class TypeData
	{
		// Token: 0x060011C4 RID: 4548 RVA: 0x000637CD File Offset: 0x000619CD
		private TypeData()
		{
			this.StandardMembers = new Dictionary<string, TypeMemberData>(StringComparer.OrdinalIgnoreCase);
			this.Members = new Dictionary<string, TypeMemberData>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x000637F5 File Offset: 0x000619F5
		public TypeData(string typeName) : this()
		{
			if (string.IsNullOrWhiteSpace(typeName))
			{
				throw PSTraceSource.NewArgumentNullException("typeName");
			}
			this.TypeName = typeName;
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00063817 File Offset: 0x00061A17
		internal TypeData(string typeName, bool typesXml) : this()
		{
			this.fromTypesXmlFile = typesXml;
			this.TypeName = typeName;
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0006382D File Offset: 0x00061A2D
		public TypeData(Type type) : this()
		{
			if (type == null)
			{
				throw PSTraceSource.NewArgumentNullException("type");
			}
			this.TypeName = type.FullName;
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x00063855 File Offset: 0x00061A55
		// (set) Token: 0x060011C9 RID: 4553 RVA: 0x0006385D File Offset: 0x00061A5D
		internal bool fromTypesXmlFile { get; private set; }

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x00063866 File Offset: 0x00061A66
		// (set) Token: 0x060011CB RID: 4555 RVA: 0x0006386E File Offset: 0x00061A6E
		public string TypeName { get; private set; }

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x00063877 File Offset: 0x00061A77
		// (set) Token: 0x060011CD RID: 4557 RVA: 0x0006387F File Offset: 0x00061A7F
		public Dictionary<string, TypeMemberData> Members { get; private set; }

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x00063888 File Offset: 0x00061A88
		// (set) Token: 0x060011CF RID: 4559 RVA: 0x00063890 File Offset: 0x00061A90
		public Type TypeConverter { get; set; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00063899 File Offset: 0x00061A99
		// (set) Token: 0x060011D1 RID: 4561 RVA: 0x000638A1 File Offset: 0x00061AA1
		public Type TypeAdapter { get; set; }

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x000638AA File Offset: 0x00061AAA
		// (set) Token: 0x060011D3 RID: 4563 RVA: 0x000638B2 File Offset: 0x00061AB2
		public bool IsOverride { get; set; }

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x000638BB File Offset: 0x00061ABB
		// (set) Token: 0x060011D5 RID: 4565 RVA: 0x000638C3 File Offset: 0x00061AC3
		internal Dictionary<string, TypeMemberData> StandardMembers { get; private set; }

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x000638CC File Offset: 0x00061ACC
		// (set) Token: 0x060011D7 RID: 4567 RVA: 0x000638D4 File Offset: 0x00061AD4
		public string SerializationMethod
		{
			get
			{
				return this._serializationMethod;
			}
			set
			{
				this._serializationMethod = value;
				if (this._serializationMethod == null)
				{
					if (this.StandardMembers.ContainsKey("SerializationMethod"))
					{
						this.StandardMembers.Remove("SerializationMethod");
					}
					return;
				}
				TypeMemberData typeMemberData;
				if (this.StandardMembers.TryGetValue("SerializationMethod", out typeMemberData))
				{
					((NotePropertyData)typeMemberData).Value = this._serializationMethod;
					return;
				}
				NotePropertyData value2 = new NotePropertyData("SerializationMethod", this._serializationMethod);
				this.StandardMembers.Add("SerializationMethod", value2);
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x0006395C File Offset: 0x00061B5C
		// (set) Token: 0x060011D9 RID: 4569 RVA: 0x00063964 File Offset: 0x00061B64
		public Type TargetTypeForDeserialization
		{
			get
			{
				return this._targetTypeForDeserialization;
			}
			set
			{
				this._targetTypeForDeserialization = value;
				if (this._targetTypeForDeserialization == null)
				{
					if (this.StandardMembers.ContainsKey("TargetTypeForDeserialization"))
					{
						this.StandardMembers.Remove("TargetTypeForDeserialization");
					}
					return;
				}
				TypeMemberData typeMemberData;
				if (this.StandardMembers.TryGetValue("TargetTypeForDeserialization", out typeMemberData))
				{
					((NotePropertyData)typeMemberData).Value = this._targetTypeForDeserialization;
					return;
				}
				NotePropertyData value2 = new NotePropertyData("TargetTypeForDeserialization", this._targetTypeForDeserialization);
				this.StandardMembers.Add("TargetTypeForDeserialization", value2);
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x000639F2 File Offset: 0x00061BF2
		// (set) Token: 0x060011DB RID: 4571 RVA: 0x000639FC File Offset: 0x00061BFC
		public uint SerializationDepth
		{
			get
			{
				return this._serializationDepth;
			}
			set
			{
				this._serializationDepth = value;
				TypeMemberData typeMemberData;
				if (this.StandardMembers.TryGetValue("SerializationDepth", out typeMemberData))
				{
					((NotePropertyData)typeMemberData).Value = this._serializationDepth;
					return;
				}
				NotePropertyData value2 = new NotePropertyData("SerializationDepth", this._serializationDepth);
				this.StandardMembers.Add("SerializationDepth", value2);
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x00063A62 File Offset: 0x00061C62
		// (set) Token: 0x060011DD RID: 4573 RVA: 0x00063A6C File Offset: 0x00061C6C
		public string DefaultDisplayProperty
		{
			get
			{
				return this._defaultDisplayProperty;
			}
			set
			{
				this._defaultDisplayProperty = value;
				if (this._defaultDisplayProperty == null)
				{
					if (this.StandardMembers.ContainsKey("DefaultDisplayProperty"))
					{
						this.StandardMembers.Remove("DefaultDisplayProperty");
					}
					return;
				}
				TypeMemberData typeMemberData;
				if (this.StandardMembers.TryGetValue("DefaultDisplayProperty", out typeMemberData))
				{
					((NotePropertyData)typeMemberData).Value = this._defaultDisplayProperty;
					return;
				}
				NotePropertyData value2 = new NotePropertyData("DefaultDisplayProperty", this._defaultDisplayProperty);
				this.StandardMembers.Add("DefaultDisplayProperty", value2);
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x00063AF4 File Offset: 0x00061CF4
		// (set) Token: 0x060011DF RID: 4575 RVA: 0x00063AFC File Offset: 0x00061CFC
		public bool InheritPropertySerializationSet
		{
			get
			{
				return this._inheritPropertySerializationSet;
			}
			set
			{
				this._inheritPropertySerializationSet = value;
				TypeMemberData typeMemberData;
				if (this.StandardMembers.TryGetValue("InheritPropertySerializationSet", out typeMemberData))
				{
					((NotePropertyData)typeMemberData).Value = this._inheritPropertySerializationSet;
					return;
				}
				NotePropertyData value2 = new NotePropertyData("InheritPropertySerializationSet", this._inheritPropertySerializationSet);
				this.StandardMembers.Add("InheritPropertySerializationSet", value2);
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x00063B62 File Offset: 0x00061D62
		// (set) Token: 0x060011E1 RID: 4577 RVA: 0x00063B6C File Offset: 0x00061D6C
		public string StringSerializationSource
		{
			get
			{
				return this._stringSerializationSource;
			}
			set
			{
				if (value == null)
				{
					if (this._stringSerializationSource != null && this.StandardMembers.ContainsKey("StringSerializationSource"))
					{
						this.StandardMembers.Remove("StringSerializationSource");
					}
					this._stringSerializationSource = null;
					return;
				}
				this._stringSerializationSource = value;
				if (this._stringSerializationSourceProperty != null)
				{
					this.StandardMembers.Remove("StringSerializationSource");
					this._stringSerializationSourceProperty = null;
				}
				TypeMemberData typeMemberData;
				if (this.StandardMembers.TryGetValue("StringSerializationSource", out typeMemberData))
				{
					((AliasPropertyData)typeMemberData).ReferencedMemberName = this._stringSerializationSource;
					return;
				}
				AliasPropertyData value2 = new AliasPropertyData("StringSerializationSource", this._stringSerializationSource);
				this.StandardMembers.Add("StringSerializationSource", value2);
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x00063C1E File Offset: 0x00061E1E
		// (set) Token: 0x060011E3 RID: 4579 RVA: 0x00063C28 File Offset: 0x00061E28
		public TypeMemberData StringSerializationSourceProperty
		{
			get
			{
				return this._stringSerializationSourceProperty;
			}
			set
			{
				if (value == null)
				{
					if (this._stringSerializationSourceProperty != null && this.StandardMembers.ContainsKey("StringSerializationSource"))
					{
						this.StandardMembers.Remove("StringSerializationSource");
					}
					this._stringSerializationSourceProperty = null;
					return;
				}
				if (!(value is NotePropertyData) && !(value is ScriptPropertyData) && !(value is CodePropertyData))
				{
					throw PSTraceSource.NewArgumentException("value");
				}
				if (this.StandardMembers.ContainsKey("StringSerializationSource"))
				{
					this.StandardMembers.Remove("StringSerializationSource");
				}
				this._stringSerializationSourceProperty = value;
				this._stringSerializationSource = null;
				this.StandardMembers.Add("StringSerializationSource", value);
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x00063CD0 File Offset: 0x00061ED0
		// (set) Token: 0x060011E5 RID: 4581 RVA: 0x00063CD8 File Offset: 0x00061ED8
		public PropertySetData DefaultDisplayPropertySet
		{
			get
			{
				return this._defaultDisplayPropertySet;
			}
			set
			{
				this._defaultDisplayPropertySet = value;
				if (this._defaultDisplayPropertySet != null)
				{
					this._defaultDisplayPropertySet.Name = "DefaultDisplayPropertySet";
				}
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x00063CF9 File Offset: 0x00061EF9
		// (set) Token: 0x060011E7 RID: 4583 RVA: 0x00063D01 File Offset: 0x00061F01
		public PropertySetData DefaultKeyPropertySet
		{
			get
			{
				return this._defaultKeyPropertySet;
			}
			set
			{
				this._defaultKeyPropertySet = value;
				if (this._defaultKeyPropertySet != null)
				{
					this._defaultKeyPropertySet.Name = "DefaultKeyPropertySet";
				}
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00063D22 File Offset: 0x00061F22
		// (set) Token: 0x060011E9 RID: 4585 RVA: 0x00063D2A File Offset: 0x00061F2A
		public PropertySetData PropertySerializationSet
		{
			get
			{
				return this._propertySerializationSet;
			}
			set
			{
				this._propertySerializationSet = value;
				if (this._propertySerializationSet != null)
				{
					this._propertySerializationSet.Name = "PropertySerializationSet";
				}
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00063D4C File Offset: 0x00061F4C
		public TypeData Copy()
		{
			TypeData typeData = new TypeData(this.TypeName);
			foreach (KeyValuePair<string, TypeMemberData> keyValuePair in this.Members)
			{
				typeData.Members.Add(keyValuePair.Key, keyValuePair.Value.Copy());
			}
			typeData.TypeConverter = this.TypeConverter;
			typeData.TypeAdapter = this.TypeAdapter;
			typeData.IsOverride = this.IsOverride;
			foreach (KeyValuePair<string, TypeMemberData> keyValuePair2 in this.StandardMembers)
			{
				string key;
				switch (key = keyValuePair2.Key)
				{
				case "SerializationMethod":
					typeData.SerializationMethod = this.SerializationMethod;
					break;
				case "TargetTypeForDeserialization":
					typeData.TargetTypeForDeserialization = this.TargetTypeForDeserialization;
					break;
				case "SerializationDepth":
					typeData.SerializationDepth = this.SerializationDepth;
					break;
				case "DefaultDisplayProperty":
					typeData.DefaultDisplayProperty = this.DefaultDisplayProperty;
					break;
				case "InheritPropertySerializationSet":
					typeData.InheritPropertySerializationSet = this.InheritPropertySerializationSet;
					break;
				case "StringSerializationSource":
					typeData.StringSerializationSource = this.StringSerializationSource;
					break;
				}
			}
			typeData.DefaultDisplayPropertySet = ((this.DefaultDisplayPropertySet == null) ? null : ((PropertySetData)this.DefaultDisplayPropertySet.Copy()));
			typeData.DefaultKeyPropertySet = ((this.DefaultKeyPropertySet == null) ? null : ((PropertySetData)this.DefaultKeyPropertySet.Copy()));
			typeData.PropertySerializationSet = ((this.PropertySerializationSet == null) ? null : ((PropertySetData)this.PropertySerializationSet.Copy()));
			return typeData;
		}

		// Token: 0x0400079F RID: 1951
		internal const string NoteProperty = "NoteProperty";

		// Token: 0x040007A0 RID: 1952
		internal const string AliasProperty = "AliasProperty";

		// Token: 0x040007A1 RID: 1953
		internal const string ScriptProperty = "ScriptProperty";

		// Token: 0x040007A2 RID: 1954
		internal const string CodeProperty = "CodeProperty";

		// Token: 0x040007A3 RID: 1955
		internal const string ScriptMethod = "ScriptMethod";

		// Token: 0x040007A4 RID: 1956
		internal const string CodeMethod = "CodeMethod";

		// Token: 0x040007A5 RID: 1957
		internal const string PropertySet = "PropertySet";

		// Token: 0x040007A6 RID: 1958
		internal const string MemberSet = "MemberSet";

		// Token: 0x040007A7 RID: 1959
		private string _serializationMethod;

		// Token: 0x040007A8 RID: 1960
		private Type _targetTypeForDeserialization;

		// Token: 0x040007A9 RID: 1961
		private uint _serializationDepth;

		// Token: 0x040007AA RID: 1962
		private string _defaultDisplayProperty;

		// Token: 0x040007AB RID: 1963
		private bool _inheritPropertySerializationSet;

		// Token: 0x040007AC RID: 1964
		private string _stringSerializationSource;

		// Token: 0x040007AD RID: 1965
		private TypeMemberData _stringSerializationSourceProperty;

		// Token: 0x040007AE RID: 1966
		private PropertySetData _defaultDisplayPropertySet;

		// Token: 0x040007AF RID: 1967
		private PropertySetData _defaultKeyPropertySet;

		// Token: 0x040007B0 RID: 1968
		private PropertySetData _propertySerializationSet;
	}
}
