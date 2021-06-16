using System;
using System.Globalization;

namespace System.Management.Automation
{
	// Token: 0x020001BA RID: 442
	internal class AliasHelpInfo : HelpInfo
	{
		// Token: 0x06001496 RID: 5270 RVA: 0x0007F964 File Offset: 0x0007DB64
		private AliasHelpInfo(AliasInfo aliasInfo)
		{
			this._fullHelpObject = new PSObject();
			string text = (aliasInfo.ResolvedCommand == null) ? aliasInfo.UnresolvedCommandName : aliasInfo.ResolvedCommand.Name;
			base.ForwardTarget = text;
			base.ForwardHelpCategory = (HelpCategory.Cmdlet | HelpCategory.ScriptCommand | HelpCategory.Function | HelpCategory.Filter | HelpCategory.ExternalScript | HelpCategory.Workflow);
			if (!string.IsNullOrEmpty(aliasInfo.Name))
			{
				this._name = aliasInfo.Name.Trim();
			}
			if (!string.IsNullOrEmpty(text))
			{
				this._synopsis = text.Trim();
			}
			this._fullHelpObject.TypeNames.Clear();
			this._fullHelpObject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "AliasHelpInfo#{0}", new object[]
			{
				this.Name
			}));
			this._fullHelpObject.TypeNames.Add("AliasHelpInfo");
			this._fullHelpObject.TypeNames.Add("HelpInfo");
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x0007FA62 File Offset: 0x0007DC62
		internal override string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x0007FA6A File Offset: 0x0007DC6A
		internal override string Synopsis
		{
			get
			{
				return this._synopsis;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x0007FA72 File Offset: 0x0007DC72
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Alias;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x0007FA75 File Offset: 0x0007DC75
		internal override PSObject FullHelp
		{
			get
			{
				return this._fullHelpObject;
			}
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0007FA80 File Offset: 0x0007DC80
		internal static AliasHelpInfo GetHelpInfo(AliasInfo aliasInfo)
		{
			if (aliasInfo == null)
			{
				return null;
			}
			if (aliasInfo.ResolvedCommand == null && aliasInfo.UnresolvedCommandName == null)
			{
				return null;
			}
			AliasHelpInfo aliasHelpInfo = new AliasHelpInfo(aliasInfo);
			if (string.IsNullOrEmpty(aliasHelpInfo.Name))
			{
				return null;
			}
			aliasHelpInfo.AddCommonHelpProperties();
			return aliasHelpInfo;
		}

		// Token: 0x040008DD RID: 2269
		private string _name = "";

		// Token: 0x040008DE RID: 2270
		private string _synopsis = "";

		// Token: 0x040008DF RID: 2271
		private PSObject _fullHelpObject;
	}
}
