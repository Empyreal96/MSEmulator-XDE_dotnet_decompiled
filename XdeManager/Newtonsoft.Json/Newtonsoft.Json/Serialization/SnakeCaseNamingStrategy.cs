using System;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200009A RID: 154
	public class SnakeCaseNamingStrategy : NamingStrategy
	{
		// Token: 0x06000825 RID: 2085 RVA: 0x00024022 File Offset: 0x00022222
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
		{
			base.ProcessDictionaryKeys = processDictionaryKeys;
			base.OverrideSpecifiedNames = overrideSpecifiedNames;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00024038 File Offset: 0x00022238
		public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames, bool processExtensionDataNames) : this(processDictionaryKeys, overrideSpecifiedNames)
		{
			base.ProcessExtensionDataNames = processExtensionDataNames;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00024049 File Offset: 0x00022249
		public SnakeCaseNamingStrategy()
		{
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00024051 File Offset: 0x00022251
		protected override string ResolvePropertyName(string name)
		{
			return StringUtils.ToSnakeCase(name);
		}
	}
}
