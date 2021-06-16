using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000139 RID: 313
	public class PSAliasProperty : PSPropertyInfo
	{
		// Token: 0x0600106C RID: 4204 RVA: 0x0005C940 File Offset: 0x0005AB40
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.Name);
			stringBuilder.Append(" = ");
			if (this.conversionType != null)
			{
				stringBuilder.Append("(");
				stringBuilder.Append(this.conversionType);
				stringBuilder.Append(")");
			}
			stringBuilder.Append(this.referencedMemberName);
			return stringBuilder.ToString();
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0005C9B2 File Offset: 0x0005ABB2
		public PSAliasProperty(string name, string referencedMemberName)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (string.IsNullOrEmpty(referencedMemberName))
			{
				throw PSTraceSource.NewArgumentException("referencedMemberName");
			}
			this.referencedMemberName = referencedMemberName;
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0005C9F0 File Offset: 0x0005ABF0
		public PSAliasProperty(string name, string referencedMemberName, Type conversionType)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			this.name = name;
			if (string.IsNullOrEmpty(referencedMemberName))
			{
				throw PSTraceSource.NewArgumentException("referencedMemberName");
			}
			this.referencedMemberName = referencedMemberName;
			this.conversionType = conversionType;
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x0005CA3E File Offset: 0x0005AC3E
		public string ReferencedMemberName
		{
			get
			{
				return this.referencedMemberName;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x0005CA46 File Offset: 0x0005AC46
		internal PSMemberInfo ReferencedMember
		{
			get
			{
				return this.LookupMember(this.referencedMemberName);
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0005CA54 File Offset: 0x0005AC54
		public Type ConversionType
		{
			get
			{
				return this.conversionType;
			}
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0005CA5C File Offset: 0x0005AC5C
		public override PSMemberInfo Copy()
		{
			PSAliasProperty psaliasProperty = new PSAliasProperty(this.name, this.referencedMemberName);
			psaliasProperty.conversionType = this.conversionType;
			base.CloneBaseProperties(psaliasProperty);
			return psaliasProperty;
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x0005CA8F File Offset: 0x0005AC8F
		public override PSMemberTypes MemberType
		{
			get
			{
				return PSMemberTypes.AliasProperty;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x0005CA92 File Offset: 0x0005AC92
		public override string TypeNameOfValue
		{
			get
			{
				if (this.conversionType != null)
				{
					return this.conversionType.FullName;
				}
				return this.ReferencedMember.TypeNameOfValue;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x0005CABC File Offset: 0x0005ACBC
		public override bool IsSettable
		{
			get
			{
				PSPropertyInfo pspropertyInfo = this.ReferencedMember as PSPropertyInfo;
				return pspropertyInfo != null && pspropertyInfo.IsSettable;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x0005CAE0 File Offset: 0x0005ACE0
		public override bool IsGettable
		{
			get
			{
				PSPropertyInfo pspropertyInfo = this.ReferencedMember as PSPropertyInfo;
				return pspropertyInfo != null && pspropertyInfo.IsGettable;
			}
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0005CB04 File Offset: 0x0005AD04
		private PSMemberInfo LookupMember(string name)
		{
			PSMemberInfo result;
			bool flag;
			this.LookupMember(name, new HashSet<string>(StringComparer.OrdinalIgnoreCase), out result, out flag);
			if (flag)
			{
				throw new ExtendedTypeSystemException("CycleInAliasLookup", null, ExtendedTypeSystem.CycleInAlias, new object[]
				{
					base.Name
				});
			}
			return result;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0005CB4C File Offset: 0x0005AD4C
		private void LookupMember(string name, HashSet<string> visitedAliases, out PSMemberInfo returnedMember, out bool hasCycle)
		{
			returnedMember = null;
			if (this.instance == null)
			{
				throw new ExtendedTypeSystemException("AliasLookupMemberOutsidePSObject", null, ExtendedTypeSystem.AccessMemberOutsidePSObject, new object[]
				{
					name
				});
			}
			PSMemberInfo psmemberInfo = PSObject.AsPSObject(this.instance).Properties[name];
			if (psmemberInfo == null)
			{
				throw new ExtendedTypeSystemException("AliasLookupMemberNotPresent", null, ExtendedTypeSystem.MemberNotPresent, new object[]
				{
					name
				});
			}
			PSAliasProperty psaliasProperty = psmemberInfo as PSAliasProperty;
			if (psaliasProperty == null)
			{
				hasCycle = false;
				returnedMember = psmemberInfo;
				return;
			}
			if (visitedAliases.Contains(name))
			{
				hasCycle = true;
				return;
			}
			visitedAliases.Add(name);
			this.LookupMember(psaliasProperty.ReferencedMemberName, visitedAliases, out returnedMember, out hasCycle);
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x0005CBF0 File Offset: 0x0005ADF0
		// (set) Token: 0x0600107A RID: 4218 RVA: 0x0005CC2A File Offset: 0x0005AE2A
		public override object Value
		{
			get
			{
				object obj = this.ReferencedMember.Value;
				if (this.conversionType != null)
				{
					obj = LanguagePrimitives.ConvertTo(obj, this.conversionType, CultureInfo.InvariantCulture);
				}
				return obj;
			}
			set
			{
				this.ReferencedMember.Value = value;
			}
		}

		// Token: 0x0400072E RID: 1838
		private string referencedMemberName;

		// Token: 0x0400072F RID: 1839
		private Type conversionType;
	}
}
