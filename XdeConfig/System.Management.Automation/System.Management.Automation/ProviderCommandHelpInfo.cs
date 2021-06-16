using System;

namespace System.Management.Automation
{
	// Token: 0x020001AA RID: 426
	internal class ProviderCommandHelpInfo : HelpInfo
	{
		// Token: 0x060013EB RID: 5099 RVA: 0x00079FFC File Offset: 0x000781FC
		internal ProviderCommandHelpInfo(HelpInfo genericHelpInfo, ProviderContext providerContext)
		{
			base.ForwardHelpCategory = HelpCategory.None;
			MamlCommandHelpInfo providerSpecificHelpInfo = providerContext.GetProviderSpecificHelpInfo(genericHelpInfo.Name);
			if (providerSpecificHelpInfo == null)
			{
				this._helpInfo = genericHelpInfo;
				return;
			}
			providerSpecificHelpInfo.OverrideProviderSpecificHelpWithGenericHelp(genericHelpInfo);
			this._helpInfo = providerSpecificHelpInfo;
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0007A03C File Offset: 0x0007823C
		internal override PSObject[] GetParameter(string pattern)
		{
			return this._helpInfo.GetParameter(pattern);
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0007A04A File Offset: 0x0007824A
		internal override Uri GetUriForOnlineHelp()
		{
			return this._helpInfo.GetUriForOnlineHelp();
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x0007A057 File Offset: 0x00078257
		internal override string Name
		{
			get
			{
				return this._helpInfo.Name;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x0007A064 File Offset: 0x00078264
		internal override string Synopsis
		{
			get
			{
				return this._helpInfo.Synopsis;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060013F0 RID: 5104 RVA: 0x0007A071 File Offset: 0x00078271
		internal override HelpCategory HelpCategory
		{
			get
			{
				return this._helpInfo.HelpCategory;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x0007A07E File Offset: 0x0007827E
		internal override PSObject FullHelp
		{
			get
			{
				return this._helpInfo.FullHelp;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060013F2 RID: 5106 RVA: 0x0007A08B File Offset: 0x0007828B
		internal override string Component
		{
			get
			{
				return this._helpInfo.Component;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x0007A098 File Offset: 0x00078298
		internal override string Role
		{
			get
			{
				return this._helpInfo.Role;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060013F4 RID: 5108 RVA: 0x0007A0A5 File Offset: 0x000782A5
		internal override string Functionality
		{
			get
			{
				return this._helpInfo.Functionality;
			}
		}

		// Token: 0x04000892 RID: 2194
		private HelpInfo _helpInfo;
	}
}
