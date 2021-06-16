using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Language;
using System.Text;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x02000088 RID: 136
	public sealed class ParameterMetadata
	{
		// Token: 0x060006E3 RID: 1763 RVA: 0x00020F24 File Offset: 0x0001F124
		public ParameterMetadata(string name) : this(name, null)
		{
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00020F30 File Offset: 0x0001F130
		public ParameterMetadata(string name, Type parameterType)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentNullException("name");
			}
			this.name = name;
			this.parameterType = parameterType;
			this.attributes = new Collection<Attribute>();
			this.aliases = new Collection<string>();
			this.parameterSets = new Dictionary<string, ParameterSetMetadata>();
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00020F88 File Offset: 0x0001F188
		public ParameterMetadata(ParameterMetadata other)
		{
			if (other == null)
			{
				throw PSTraceSource.NewArgumentNullException("other");
			}
			this.isDynamic = other.isDynamic;
			this.name = other.name;
			this.parameterType = other.parameterType;
			this.aliases = new Collection<string>(new List<string>(other.aliases.Count));
			foreach (string item in other.aliases)
			{
				this.aliases.Add(item);
			}
			if (other.attributes == null)
			{
				this.attributes = null;
			}
			else
			{
				this.attributes = new Collection<Attribute>(new List<Attribute>(other.attributes.Count));
				foreach (Attribute item2 in other.attributes)
				{
					this.attributes.Add(item2);
				}
			}
			this.parameterSets = null;
			if (other.parameterSets == null)
			{
				this.parameterSets = null;
				return;
			}
			this.parameterSets = new Dictionary<string, ParameterSetMetadata>(other.parameterSets.Count);
			foreach (KeyValuePair<string, ParameterSetMetadata> keyValuePair in other.parameterSets)
			{
				this.parameterSets.Add(keyValuePair.Key, new ParameterSetMetadata(keyValuePair.Value));
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00021128 File Offset: 0x0001F328
		internal ParameterMetadata(CompiledCommandParameter cmdParameterMD)
		{
			this.Initialize(cmdParameterMD);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00021137 File Offset: 0x0001F337
		internal ParameterMetadata(Collection<string> aliases, bool isDynamic, string name, Dictionary<string, ParameterSetMetadata> parameterSets, Type parameterType)
		{
			this.aliases = aliases;
			this.isDynamic = isDynamic;
			this.name = name;
			this.parameterSets = parameterSets;
			this.parameterType = parameterType;
			this.attributes = new Collection<Attribute>();
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0002116F File Offset: 0x0001F36F
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x00021177 File Offset: 0x0001F377
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw PSTraceSource.NewArgumentNullException("Name");
				}
				this.name = value;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x00021193 File Offset: 0x0001F393
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x0002119B File Offset: 0x0001F39B
		public Type ParameterType
		{
			get
			{
				return this.parameterType;
			}
			set
			{
				this.parameterType = value;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x000211A4 File Offset: 0x0001F3A4
		public Dictionary<string, ParameterSetMetadata> ParameterSets
		{
			get
			{
				return this.parameterSets;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x000211AC File Offset: 0x0001F3AC
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x000211B4 File Offset: 0x0001F3B4
		public bool IsDynamic
		{
			get
			{
				return this.isDynamic;
			}
			set
			{
				this.isDynamic = value;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x000211BD File Offset: 0x0001F3BD
		public Collection<string> Aliases
		{
			get
			{
				return this.aliases;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x000211C5 File Offset: 0x0001F3C5
		public Collection<Attribute> Attributes
		{
			get
			{
				return this.attributes;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x000211CD File Offset: 0x0001F3CD
		public bool SwitchParameter
		{
			get
			{
				return this.parameterType != null && this.parameterType.Equals(typeof(SwitchParameter));
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x000211F4 File Offset: 0x0001F3F4
		public static Dictionary<string, ParameterMetadata> GetParameterMetadata(Type type)
		{
			if (null == type)
			{
				throw PSTraceSource.NewArgumentNullException("type");
			}
			CommandMetadata commandMetadata = new CommandMetadata(type);
			return commandMetadata.Parameters;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00021228 File Offset: 0x0001F428
		internal void Initialize(CompiledCommandParameter compiledParameterMD)
		{
			this.name = compiledParameterMD.Name;
			this.parameterType = compiledParameterMD.Type;
			this.isDynamic = compiledParameterMD.IsDynamic;
			this.parameterSets = new Dictionary<string, ParameterSetMetadata>(StringComparer.OrdinalIgnoreCase);
			foreach (string key in compiledParameterMD.ParameterSetData.Keys)
			{
				ParameterSetSpecificMetadata psMD = compiledParameterMD.ParameterSetData[key];
				this.parameterSets.Add(key, new ParameterSetMetadata(psMD));
			}
			this.aliases = new Collection<string>();
			foreach (string item in compiledParameterMD.Aliases)
			{
				this.aliases.Add(item);
			}
			this.attributes = new Collection<Attribute>();
			foreach (Attribute item2 in compiledParameterMD.CompiledAttributes)
			{
				this.attributes.Add(item2);
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00021370 File Offset: 0x0001F570
		internal static Dictionary<string, ParameterMetadata> GetParameterMetadata(MergedCommandParameterMetadata cmdParameterMetadata)
		{
			Dictionary<string, ParameterMetadata> dictionary = new Dictionary<string, ParameterMetadata>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, MergedCompiledCommandParameter> keyValuePair in cmdParameterMetadata.BindableParameters)
			{
				string key = keyValuePair.Key;
				MergedCompiledCommandParameter value = keyValuePair.Value;
				ParameterMetadata value2 = new ParameterMetadata(value.Parameter);
				dictionary.Add(key, value2);
			}
			return dictionary;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000213F0 File Offset: 0x0001F5F0
		internal bool IsMatchingType(PSTypeName psTypeName)
		{
			Type type = psTypeName.Type;
			if (type != null)
			{
				bool flag = LanguagePrimitives.FigureConversion(typeof(object), this.ParameterType).Rank >= ConversionRank.AssignableS2A;
				if (type.Equals(typeof(object)))
				{
					return flag;
				}
				if (flag)
				{
					return psTypeName.Type != null && psTypeName.Type.Equals(typeof(object));
				}
				LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(type, this.ParameterType);
				return conversionData != null && conversionData.Rank >= ConversionRank.NumericImplicitS2A;
			}
			else
			{
				WildcardPattern wildcardPattern = new WildcardPattern("*" + (psTypeName.Name ?? ""), WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
				if (wildcardPattern.IsMatch(this.ParameterType.FullName))
				{
					return true;
				}
				if (this.ParameterType.IsArray && wildcardPattern.IsMatch(this.ParameterType.GetElementType().FullName))
				{
					return true;
				}
				if (this.Attributes != null)
				{
					PSTypeNameAttribute pstypeNameAttribute = this.Attributes.OfType<PSTypeNameAttribute>().FirstOrDefault<PSTypeNameAttribute>();
					if (pstypeNameAttribute != null && wildcardPattern.IsMatch(pstypeNameAttribute.PSTypeName))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00021520 File Offset: 0x0001F720
		internal string GetProxyParameterData(string prefix, string paramNameOverride, bool isProxyForCmdlet)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.parameterSets != null && isProxyForCmdlet)
			{
				foreach (string text in this.parameterSets.Keys)
				{
					ParameterSetMetadata parameterSetMetadata = this.parameterSets[text];
					string proxyParameterData = parameterSetMetadata.GetProxyParameterData();
					if (!string.IsNullOrEmpty(proxyParameterData) || !text.Equals("__AllParameterSets"))
					{
						string value = "";
						stringBuilder.Append(prefix);
						stringBuilder.Append("[Parameter(");
						if (!text.Equals("__AllParameterSets"))
						{
							stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "ParameterSetName='{0}'", new object[]
							{
								CodeGeneration.EscapeSingleQuotedStringContent(text)
							});
							value = ", ";
						}
						if (!string.IsNullOrEmpty(proxyParameterData))
						{
							stringBuilder.Append(value);
							stringBuilder.Append(proxyParameterData);
						}
						stringBuilder.Append(")]");
					}
				}
			}
			if (this.aliases != null && this.aliases.Count > 0)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				string text2 = "";
				foreach (string value2 in this.aliases)
				{
					stringBuilder2.AppendFormat(CultureInfo.InvariantCulture, "{0}'{1}'", new object[]
					{
						text2,
						CodeGeneration.EscapeSingleQuotedStringContent(value2)
					});
					text2 = ",";
				}
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}[Alias({1})]", new object[]
				{
					prefix,
					stringBuilder2.ToString()
				});
			}
			if (this.attributes != null && this.attributes.Count > 0)
			{
				foreach (Attribute attrib in this.attributes)
				{
					string proxyAttributeData = this.GetProxyAttributeData(attrib, prefix);
					if (!string.IsNullOrEmpty(proxyAttributeData))
					{
						stringBuilder.Append(proxyAttributeData);
					}
				}
			}
			if (this.SwitchParameter)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}[{1}]", new object[]
				{
					prefix,
					"switch"
				});
			}
			else if (this.parameterType != null)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}[{1}]", new object[]
				{
					prefix,
					ToStringCodeMethods.Type(this.parameterType, false)
				});
			}
			CredentialAttribute credentialAttribute = this.attributes.OfType<CredentialAttribute>().FirstOrDefault<CredentialAttribute>();
			if (credentialAttribute != null)
			{
				string value3 = string.Format(CultureInfo.InvariantCulture, "{0}[System.Management.Automation.CredentialAttribute()]", new object[]
				{
					prefix
				});
				if (!string.IsNullOrEmpty(value3))
				{
					stringBuilder.Append(value3);
				}
			}
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}${{{1}}}", new object[]
			{
				prefix,
				CodeGeneration.EscapeVariableName(string.IsNullOrEmpty(paramNameOverride) ? this.name : paramNameOverride)
			});
			return stringBuilder.ToString();
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002185C File Offset: 0x0001FA5C
		private string GetProxyAttributeData(Attribute attrib, string prefix)
		{
			ValidateLengthAttribute validateLengthAttribute = attrib as ValidateLengthAttribute;
			if (validateLengthAttribute != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}[ValidateLength({1}, {2})]", new object[]
				{
					prefix,
					validateLengthAttribute.MinLength,
					validateLengthAttribute.MaxLength
				});
			}
			ValidateRangeAttribute validateRangeAttribute = attrib as ValidateRangeAttribute;
			if (validateRangeAttribute != null)
			{
				Type type = validateRangeAttribute.MinRange.GetType();
				string format;
				if (type == typeof(float) || type == typeof(double))
				{
					format = "{0}[ValidateRange({1:R}, {2:R})]";
				}
				else
				{
					format = "{0}[ValidateRange({1}, {2})]";
				}
				return string.Format(CultureInfo.InvariantCulture, format, new object[]
				{
					prefix,
					validateRangeAttribute.MinRange,
					validateRangeAttribute.MaxRange
				});
			}
			AllowNullAttribute allowNullAttribute = attrib as AllowNullAttribute;
			if (allowNullAttribute != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}[AllowNull()]", new object[]
				{
					prefix
				});
			}
			AllowEmptyStringAttribute allowEmptyStringAttribute = attrib as AllowEmptyStringAttribute;
			if (allowEmptyStringAttribute != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}[AllowEmptyString()]", new object[]
				{
					prefix
				});
			}
			AllowEmptyCollectionAttribute allowEmptyCollectionAttribute = attrib as AllowEmptyCollectionAttribute;
			if (allowEmptyCollectionAttribute != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}[AllowEmptyCollection()]", new object[]
				{
					prefix
				});
			}
			ValidatePatternAttribute validatePatternAttribute = attrib as ValidatePatternAttribute;
			if (validatePatternAttribute != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}[ValidatePattern('{1}')]", new object[]
				{
					prefix,
					CodeGeneration.EscapeSingleQuotedStringContent(validatePatternAttribute.RegexPattern)
				});
			}
			ValidateCountAttribute validateCountAttribute = attrib as ValidateCountAttribute;
			if (validateCountAttribute != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}[ValidateCount({1}, {2})]", new object[]
				{
					prefix,
					validateCountAttribute.MinLength,
					validateCountAttribute.MaxLength
				});
			}
			ValidateNotNullAttribute validateNotNullAttribute = attrib as ValidateNotNullAttribute;
			if (validateNotNullAttribute != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}[ValidateNotNull()]", new object[]
				{
					prefix
				});
			}
			ValidateNotNullOrEmptyAttribute validateNotNullOrEmptyAttribute = attrib as ValidateNotNullOrEmptyAttribute;
			if (validateNotNullOrEmptyAttribute != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}[ValidateNotNullOrEmpty()]", new object[]
				{
					prefix
				});
			}
			ValidateSetAttribute validateSetAttribute = attrib as ValidateSetAttribute;
			if (validateSetAttribute != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = "";
				foreach (string value in validateSetAttribute.ValidValues)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}'{1}'", new object[]
					{
						text,
						CodeGeneration.EscapeSingleQuotedStringContent(value)
					});
					text = ",";
				}
				return string.Format(CultureInfo.InvariantCulture, "{0}[ValidateSet({1})]", new object[]
				{
					prefix,
					stringBuilder.ToString()
				});
			}
			ValidateScriptAttribute validateScriptAttribute = attrib as ValidateScriptAttribute;
			if (validateScriptAttribute != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}[ValidateScript({{ {1} }})]", new object[]
				{
					prefix,
					validateScriptAttribute.ScriptBlock.ToString()
				});
			}
			PSTypeNameAttribute pstypeNameAttribute = attrib as PSTypeNameAttribute;
			if (pstypeNameAttribute != null)
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}[PSTypeName('{1}')]", new object[]
				{
					prefix,
					CodeGeneration.EscapeSingleQuotedStringContent(pstypeNameAttribute.PSTypeName)
				});
			}
			ObsoleteAttribute obsoleteAttribute = attrib as ObsoleteAttribute;
			if (obsoleteAttribute != null)
			{
				string text2 = string.Empty;
				if (obsoleteAttribute.IsError)
				{
					string str = "'" + CodeGeneration.EscapeSingleQuotedStringContent(obsoleteAttribute.Message) + "'";
					text2 = str + ", $true";
				}
				else if (obsoleteAttribute.Message != null)
				{
					text2 = "'" + CodeGeneration.EscapeSingleQuotedStringContent(obsoleteAttribute.Message) + "'";
				}
				return string.Format(CultureInfo.InvariantCulture, "{0}[Obsolete({1})]", new object[]
				{
					prefix,
					text2
				});
			}
			return null;
		}

		// Token: 0x040002DC RID: 732
		private const string ParameterNameFormat = "{0}${{{1}}}";

		// Token: 0x040002DD RID: 733
		private const string ParameterTypeFormat = "{0}[{1}]";

		// Token: 0x040002DE RID: 734
		private const string ParameterSetNameFormat = "ParameterSetName='{0}'";

		// Token: 0x040002DF RID: 735
		private const string AliasesFormat = "{0}[Alias({1})]";

		// Token: 0x040002E0 RID: 736
		private const string ValidateLengthFormat = "{0}[ValidateLength({1}, {2})]";

		// Token: 0x040002E1 RID: 737
		private const string ValidateRangeFloatFormat = "{0}[ValidateRange({1:R}, {2:R})]";

		// Token: 0x040002E2 RID: 738
		private const string ValidateRangeFormat = "{0}[ValidateRange({1}, {2})]";

		// Token: 0x040002E3 RID: 739
		private const string ValidatePatternFormat = "{0}[ValidatePattern('{1}')]";

		// Token: 0x040002E4 RID: 740
		private const string ValidateScriptFormat = "{0}[ValidateScript({{ {1} }})]";

		// Token: 0x040002E5 RID: 741
		private const string ValidateCountFormat = "{0}[ValidateCount({1}, {2})]";

		// Token: 0x040002E6 RID: 742
		private const string ValidateSetFormat = "{0}[ValidateSet({1})]";

		// Token: 0x040002E7 RID: 743
		private const string ValidateNotNullFormat = "{0}[ValidateNotNull()]";

		// Token: 0x040002E8 RID: 744
		private const string ValidateNotNullOrEmptyFormat = "{0}[ValidateNotNullOrEmpty()]";

		// Token: 0x040002E9 RID: 745
		private const string AllowNullFormat = "{0}[AllowNull()]";

		// Token: 0x040002EA RID: 746
		private const string AllowEmptyStringFormat = "{0}[AllowEmptyString()]";

		// Token: 0x040002EB RID: 747
		private const string AllowEmptyCollectionFormat = "{0}[AllowEmptyCollection()]";

		// Token: 0x040002EC RID: 748
		private const string PSTypeNameFormat = "{0}[PSTypeName('{1}')]";

		// Token: 0x040002ED RID: 749
		private const string ObsoleteFormat = "{0}[Obsolete({1})]";

		// Token: 0x040002EE RID: 750
		private const string CredentialAttributeFormat = "{0}[System.Management.Automation.CredentialAttribute()]";

		// Token: 0x040002EF RID: 751
		private string name;

		// Token: 0x040002F0 RID: 752
		private Type parameterType;

		// Token: 0x040002F1 RID: 753
		private bool isDynamic;

		// Token: 0x040002F2 RID: 754
		private Dictionary<string, ParameterSetMetadata> parameterSets;

		// Token: 0x040002F3 RID: 755
		private Collection<string> aliases;

		// Token: 0x040002F4 RID: 756
		private Collection<Attribute> attributes;
	}
}
