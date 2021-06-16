using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.Management.Automation.Language;
using System.Net;
using System.Net.Mail;
using System.Numerics;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Management.Infrastructure;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020005E8 RID: 1512
	internal static class CoreTypes
	{
		// Token: 0x060040C7 RID: 16583 RVA: 0x001578C0 File Offset: 0x00155AC0
		internal static bool Contains(Type inputType)
		{
			if (CoreTypes.Items.Value.ContainsKey(inputType))
			{
				return true;
			}
			TypeInfo typeInfo = inputType.GetTypeInfo();
			if (typeInfo.IsEnum)
			{
				return true;
			}
			if (typeInfo.IsGenericType)
			{
				Type genericTypeDefinition = typeInfo.GetGenericTypeDefinition();
				return genericTypeDefinition == typeof(Nullable<>) || genericTypeDefinition == typeof(FlagsExpression<>);
			}
			return inputType.IsArray && CoreTypes.Contains(inputType.GetElementType());
		}

		// Token: 0x0400208A RID: 8330
		internal static Lazy<Dictionary<Type, string[]>> Items = new Lazy<Dictionary<Type, string[]>>(() => new Dictionary<Type, string[]>
		{
			{
				typeof(AliasAttribute),
				new string[]
				{
					"Alias"
				}
			},
			{
				typeof(AllowEmptyCollectionAttribute),
				new string[]
				{
					"AllowEmptyCollection"
				}
			},
			{
				typeof(AllowEmptyStringAttribute),
				new string[]
				{
					"AllowEmptyString"
				}
			},
			{
				typeof(AllowNullAttribute),
				new string[]
				{
					"AllowNull"
				}
			},
			{
				typeof(ArgumentCompleterAttribute),
				new string[]
				{
					"ArgumentCompleter"
				}
			},
			{
				typeof(Array),
				new string[]
				{
					"array"
				}
			},
			{
				typeof(bool),
				new string[]
				{
					"bool"
				}
			},
			{
				typeof(byte),
				new string[]
				{
					"byte"
				}
			},
			{
				typeof(char),
				new string[]
				{
					"char"
				}
			},
			{
				typeof(CmdletBindingAttribute),
				new string[]
				{
					"CmdletBinding"
				}
			},
			{
				typeof(DateTime),
				new string[]
				{
					"datetime"
				}
			},
			{
				typeof(decimal),
				new string[]
				{
					"decimal"
				}
			},
			{
				typeof(double),
				new string[]
				{
					"double"
				}
			},
			{
				typeof(DscResourceAttribute),
				new string[]
				{
					"DscResource"
				}
			},
			{
				typeof(float),
				new string[]
				{
					"float",
					"single"
				}
			},
			{
				typeof(Guid),
				new string[]
				{
					"guid"
				}
			},
			{
				typeof(Hashtable),
				new string[]
				{
					"hashtable"
				}
			},
			{
				typeof(int),
				new string[]
				{
					"int",
					"int32"
				}
			},
			{
				typeof(short),
				new string[]
				{
					"int16"
				}
			},
			{
				typeof(long),
				new string[]
				{
					"long",
					"int64"
				}
			},
			{
				typeof(CimInstance),
				new string[]
				{
					"ciminstance"
				}
			},
			{
				typeof(CimClass),
				new string[]
				{
					"cimclass"
				}
			},
			{
				typeof(CimType),
				new string[]
				{
					"cimtype"
				}
			},
			{
				typeof(CimConverter),
				new string[]
				{
					"cimconverter"
				}
			},
			{
				typeof(ModuleSpecification),
				null
			},
			{
				typeof(NullString),
				new string[]
				{
					"NullString"
				}
			},
			{
				typeof(OutputTypeAttribute),
				new string[]
				{
					"OutputType"
				}
			},
			{
				typeof(object[]),
				null
			},
			{
				typeof(ParameterAttribute),
				new string[]
				{
					"Parameter"
				}
			},
			{
				typeof(PSCredential),
				new string[]
				{
					"pscredential"
				}
			},
			{
				typeof(PSDefaultValueAttribute),
				new string[]
				{
					"PSDefaultValue"
				}
			},
			{
				typeof(PSListModifier),
				new string[]
				{
					"pslistmodifier"
				}
			},
			{
				typeof(PSObject),
				new string[]
				{
					"psobject",
					"pscustomobject"
				}
			},
			{
				typeof(PSPrimitiveDictionary),
				new string[]
				{
					"psprimitivedictionary"
				}
			},
			{
				typeof(PSReference),
				new string[]
				{
					"ref"
				}
			},
			{
				typeof(PSTypeNameAttribute),
				new string[]
				{
					"PSTypeNameAttribute"
				}
			},
			{
				typeof(Regex),
				new string[]
				{
					"regex"
				}
			},
			{
				typeof(DscPropertyAttribute),
				new string[]
				{
					"DscProperty"
				}
			},
			{
				typeof(sbyte),
				new string[]
				{
					"sbyte"
				}
			},
			{
				typeof(string),
				new string[]
				{
					"string"
				}
			},
			{
				typeof(SupportsWildcardsAttribute),
				new string[]
				{
					"SupportsWildcards"
				}
			},
			{
				typeof(SwitchParameter),
				new string[]
				{
					"switch"
				}
			},
			{
				typeof(CultureInfo),
				new string[]
				{
					"cultureinfo"
				}
			},
			{
				typeof(BigInteger),
				new string[]
				{
					"bigint"
				}
			},
			{
				typeof(SecureString),
				new string[]
				{
					"securestring"
				}
			},
			{
				typeof(TimeSpan),
				new string[]
				{
					"timespan"
				}
			},
			{
				typeof(ushort),
				new string[]
				{
					"uint16"
				}
			},
			{
				typeof(uint),
				new string[]
				{
					"uint32"
				}
			},
			{
				typeof(ulong),
				new string[]
				{
					"uint64"
				}
			},
			{
				typeof(Uri),
				new string[]
				{
					"uri"
				}
			},
			{
				typeof(ValidateCountAttribute),
				new string[]
				{
					"ValidateCount"
				}
			},
			{
				typeof(ValidateLengthAttribute),
				new string[]
				{
					"ValidateLength"
				}
			},
			{
				typeof(ValidateNotNullAttribute),
				new string[]
				{
					"ValidateNotNull"
				}
			},
			{
				typeof(ValidateNotNullOrEmptyAttribute),
				new string[]
				{
					"ValidateNotNullOrEmpty"
				}
			},
			{
				typeof(ValidatePatternAttribute),
				new string[]
				{
					"ValidatePattern"
				}
			},
			{
				typeof(ValidateRangeAttribute),
				new string[]
				{
					"ValidateRange"
				}
			},
			{
				typeof(ValidateScriptAttribute),
				new string[]
				{
					"ValidateScript"
				}
			},
			{
				typeof(ValidateSetAttribute),
				new string[]
				{
					"ValidateSet"
				}
			},
			{
				typeof(Version),
				new string[]
				{
					"version"
				}
			},
			{
				typeof(void),
				new string[]
				{
					"void"
				}
			},
			{
				typeof(IPAddress),
				new string[]
				{
					"ipaddress"
				}
			},
			{
				typeof(DscLocalConfigurationManagerAttribute),
				new string[]
				{
					"DscLocalConfigurationManager"
				}
			},
			{
				typeof(XmlDocument),
				new string[]
				{
					"xml"
				}
			},
			{
				typeof(DirectoryEntry),
				new string[]
				{
					"adsi"
				}
			},
			{
				typeof(DirectorySearcher),
				new string[]
				{
					"adsisearcher"
				}
			},
			{
				typeof(ManagementClass),
				new string[]
				{
					"wmiclass"
				}
			},
			{
				typeof(ManagementObject),
				new string[]
				{
					"wmi"
				}
			},
			{
				typeof(ManagementObjectSearcher),
				new string[]
				{
					"wmisearcher"
				}
			},
			{
				typeof(MailAddress),
				new string[]
				{
					"mailaddress"
				}
			}
		});
	}
}
