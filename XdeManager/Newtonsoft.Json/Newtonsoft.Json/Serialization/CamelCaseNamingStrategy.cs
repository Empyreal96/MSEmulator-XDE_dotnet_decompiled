using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200006B RID: 107
	public class CamelCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x060005F9 RID: 1529 RVA: 0x00019B09 File Offset: 0x00017D09
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00019B1F File Offset: 0x00017D1F
		public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames) : this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00019B30 File Offset: 0x00017D30
		public CamelCaseNamingStrategy()
		{
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00019B38 File Offset: 0x00017D38
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToCamelCase(name);
		}
	}
}
