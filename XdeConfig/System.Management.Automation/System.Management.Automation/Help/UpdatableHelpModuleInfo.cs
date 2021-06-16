using System;
using System.Globalization;

namespace System.Management.Automation.Help
{
	// Token: 0x020001D9 RID: 473
	internal class UpdatableHelpModuleInfo
	{
		// Token: 0x060015CB RID: 5579 RVA: 0x0008A48F File Offset: 0x0008868F
		internal UpdatableHelpModuleInfo(string name, Guid guid, string path, string uri)
		{
			this._moduleName = name;
			this._moduleGuid = guid;
			this._moduleBase = path;
			this._helpInfoUri = uri;
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x0008A4B4 File Offset: 0x000886B4
		internal string ModuleName
		{
			get
			{
				return this._moduleName;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x0008A4BC File Offset: 0x000886BC
		internal Guid ModuleGuid
		{
			get
			{
				return this._moduleGuid;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x0008A4C4 File Offset: 0x000886C4
		internal string ModuleBase
		{
			get
			{
				return this._moduleBase;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x0008A4CC File Offset: 0x000886CC
		internal string HelpInfoUri
		{
			get
			{
				return this._helpInfoUri;
			}
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0008A4D4 File Offset: 0x000886D4
		internal string GetHelpContentName(CultureInfo culture)
		{
			return string.Concat(new string[]
			{
				this._moduleName,
				"_",
				this._moduleGuid.ToString(),
				"_",
				culture.Name,
				"_",
				UpdatableHelpModuleInfo.HelpContentZipName
			});
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0008A534 File Offset: 0x00088734
		internal string GetHelpInfoName()
		{
			return string.Concat(new string[]
			{
				this._moduleName,
				"_",
				this._moduleGuid.ToString(),
				"_",
				UpdatableHelpModuleInfo.HelpIntoXmlName
			});
		}

		// Token: 0x0400093B RID: 2363
		internal static readonly string HelpContentZipName = "HelpContent.cab";

		// Token: 0x0400093C RID: 2364
		internal static readonly string HelpIntoXmlName = "HelpInfo.xml";

		// Token: 0x0400093D RID: 2365
		private string _moduleName;

		// Token: 0x0400093E RID: 2366
		private Guid _moduleGuid;

		// Token: 0x0400093F RID: 2367
		private string _moduleBase;

		// Token: 0x04000940 RID: 2368
		private string _helpInfoUri;
	}
}
