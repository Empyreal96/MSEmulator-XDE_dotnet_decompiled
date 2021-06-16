using System;
using System.Collections;
using System.Text;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001BF RID: 447
	internal class ProviderHelpInfo : HelpInfo
	{
		// Token: 0x060014B8 RID: 5304 RVA: 0x00080DE4 File Offset: 0x0007EFE4
		private ProviderHelpInfo(XmlNode xmlNode)
		{
			MamlNode mamlNode = new MamlNode(xmlNode);
			this._fullHelpObject = mamlNode.PSObject;
			base.Errors = mamlNode.Errors;
			this._fullHelpObject.TypeNames.Clear();
			this._fullHelpObject.TypeNames.Add("ProviderHelpInfo");
			this._fullHelpObject.TypeNames.Add("HelpInfo");
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x00080E50 File Offset: 0x0007F050
		internal override string Name
		{
			get
			{
				if (this._fullHelpObject == null)
				{
					return "";
				}
				if (this._fullHelpObject.Properties["Name"] == null)
				{
					return "";
				}
				if (this._fullHelpObject.Properties["Name"].Value == null)
				{
					return "";
				}
				string text = this._fullHelpObject.Properties["Name"].Value.ToString();
				if (text == null)
				{
					return "";
				}
				return text.Trim();
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x00080EDC File Offset: 0x0007F0DC
		internal override string Synopsis
		{
			get
			{
				if (this._fullHelpObject == null)
				{
					return "";
				}
				if (this._fullHelpObject.Properties["Synopsis"] == null)
				{
					return "";
				}
				if (this._fullHelpObject.Properties["Synopsis"].Value == null)
				{
					return "";
				}
				string text = this._fullHelpObject.Properties["Synopsis"].Value.ToString();
				if (text == null)
				{
					return "";
				}
				return text.Trim();
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x00080F68 File Offset: 0x0007F168
		internal string DetailedDescription
		{
			get
			{
				if (this.FullHelp == null)
				{
					return "";
				}
				if (this.FullHelp.Properties["DetailedDescription"] == null || this.FullHelp.Properties["DetailedDescription"].Value == null)
				{
					return "";
				}
				IList list = this.FullHelp.Properties["DetailedDescription"].Value as IList;
				if (list == null || list.Count == 0)
				{
					return "";
				}
				StringBuilder stringBuilder = new StringBuilder(400);
				foreach (object obj in list)
				{
					PSObject psobject = PSObject.AsPSObject(obj);
					if (psobject != null && psobject.Properties["Text"] != null && psobject.Properties["Text"].Value != null)
					{
						string value = psobject.Properties["Text"].Value.ToString();
						stringBuilder.Append(value);
						stringBuilder.Append(Environment.NewLine);
					}
				}
				return stringBuilder.ToString().Trim();
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060014BC RID: 5308 RVA: 0x000810AC File Offset: 0x0007F2AC
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Provider;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x000810AF File Offset: 0x0007F2AF
		internal override PSObject FullHelp
		{
			get
			{
				return this._fullHelpObject;
			}
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x000810B8 File Offset: 0x0007F2B8
		internal override bool MatchPatternInContent(WildcardPattern pattern)
		{
			string text = this.Synopsis;
			string text2 = this.DetailedDescription;
			if (text == null)
			{
				text = string.Empty;
			}
			if (text2 == null)
			{
				text2 = string.Empty;
			}
			return pattern.IsMatch(text) || pattern.IsMatch(text2);
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x000810F8 File Offset: 0x0007F2F8
		internal static ProviderHelpInfo Load(XmlNode xmlNode)
		{
			ProviderHelpInfo providerHelpInfo = new ProviderHelpInfo(xmlNode);
			if (string.IsNullOrEmpty(providerHelpInfo.Name))
			{
				return null;
			}
			providerHelpInfo.AddCommonHelpProperties();
			return providerHelpInfo;
		}

		// Token: 0x040008E8 RID: 2280
		private PSObject _fullHelpObject;
	}
}
