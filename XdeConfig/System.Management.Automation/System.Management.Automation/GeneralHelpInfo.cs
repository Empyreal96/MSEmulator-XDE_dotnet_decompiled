using System;
using System.Globalization;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001CA RID: 458
	internal class GeneralHelpInfo : HelpInfo
	{
		// Token: 0x06001525 RID: 5413 RVA: 0x000843BC File Offset: 0x000825BC
		protected GeneralHelpInfo(XmlNode xmlNode)
		{
			MamlNode mamlNode = new MamlNode(xmlNode);
			this._fullHelpObject = mamlNode.PSObject;
			base.Errors = mamlNode.Errors;
			this._fullHelpObject.TypeNames.Clear();
			this._fullHelpObject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "GeneralHelpInfo#{0}", new object[]
			{
				this.Name
			}));
			this._fullHelpObject.TypeNames.Add("GeneralHelpInfo");
			this._fullHelpObject.TypeNames.Add("HelpInfo");
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x00084458 File Offset: 0x00082658
		internal override string Name
		{
			get
			{
				if (this._fullHelpObject == null)
				{
					return "";
				}
				if (this._fullHelpObject.Properties["Title"] == null)
				{
					return "";
				}
				if (this._fullHelpObject.Properties["Title"].Value == null)
				{
					return "";
				}
				string text = this._fullHelpObject.Properties["Title"].Value.ToString();
				if (text == null)
				{
					return "";
				}
				return text.Trim();
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x000844E1 File Offset: 0x000826E1
		internal override string Synopsis
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001528 RID: 5416 RVA: 0x000844E8 File Offset: 0x000826E8
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.General;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x000844EC File Offset: 0x000826EC
		internal override PSObject FullHelp
		{
			get
			{
				return this._fullHelpObject;
			}
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x000844F4 File Offset: 0x000826F4
		internal static GeneralHelpInfo Load(XmlNode xmlNode)
		{
			GeneralHelpInfo generalHelpInfo = new GeneralHelpInfo(xmlNode);
			if (string.IsNullOrEmpty(generalHelpInfo.Name))
			{
				return null;
			}
			generalHelpInfo.AddCommonHelpProperties();
			return generalHelpInfo;
		}

		// Token: 0x040008F8 RID: 2296
		private PSObject _fullHelpObject;
	}
}
