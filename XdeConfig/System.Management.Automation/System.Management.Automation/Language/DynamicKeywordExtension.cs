using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerShell.DesiredStateConfiguration.Internal;

namespace System.Management.Automation.Language
{
	// Token: 0x020005DE RID: 1502
	internal static class DynamicKeywordExtension
	{
		// Token: 0x06004027 RID: 16423 RVA: 0x001521B4 File Offset: 0x001503B4
		internal static bool IsMetaDSCResource(this DynamicKeyword keyword)
		{
			string implementingModule = keyword.ImplementingModule;
			return implementingModule != null && implementingModule.Equals(DscClassCache.DefaultModuleInfoForMetaConfigResource.Item1, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06004028 RID: 16424 RVA: 0x001521DE File Offset: 0x001503DE
		internal static bool IsCompatibleWithConfigurationType(this DynamicKeyword keyword, ConfigurationType ConfigurationType)
		{
			return (ConfigurationType == ConfigurationType.Meta && keyword.IsMetaDSCResource()) || (ConfigurationType != ConfigurationType.Meta && !keyword.IsMetaDSCResource());
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x0015221C File Offset: 0x0015041C
		internal static IEnumerable<DynamicKeyword> GetAllowedKeywords(this DynamicKeyword keyword, IEnumerable<DynamicKeyword> allowedKeywords)
		{
			string keyword2 = keyword.Keyword;
			if (string.Compare(keyword2, "Node", StringComparison.OrdinalIgnoreCase) != 0)
			{
				return null;
			}
			if (DynamicKeywordExtension._excludeKeywords.ContainsKey(keyword2))
			{
				List<string> excludeKeywods = DynamicKeywordExtension._excludeKeywords[keyword2];
				return from k in allowedKeywords
				where !excludeKeywods.Contains(k.Keyword)
				select k;
			}
			return allowedKeywords;
		}

		// Token: 0x0400204D RID: 8269
		private static Dictionary<string, List<string>> _excludeKeywords = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
		{
			{
				"Node",
				new List<string>
				{
					"Node"
				}
			}
		};
	}
}
