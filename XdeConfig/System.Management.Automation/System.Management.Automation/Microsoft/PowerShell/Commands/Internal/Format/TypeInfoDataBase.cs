using System;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200091F RID: 2335
	internal sealed class TypeInfoDataBase
	{
		// Token: 0x04002E9C RID: 11932
		internal DefaultSettingsSection defaultSettingsSection = new DefaultSettingsSection();

		// Token: 0x04002E9D RID: 11933
		internal TypeGroupsSection typeGroupSection = new TypeGroupsSection();

		// Token: 0x04002E9E RID: 11934
		internal ViewDefinitionsSection viewDefinitionsSection = new ViewDefinitionsSection();

		// Token: 0x04002E9F RID: 11935
		internal FormatControlDefinitionHolder formatControlDefinitionHolder = new FormatControlDefinitionHolder();

		// Token: 0x04002EA0 RID: 11936
		internal DisplayResourceManagerCache displayResourceManagerCache = new DisplayResourceManagerCache();
	}
}
