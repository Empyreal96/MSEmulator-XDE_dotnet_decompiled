using System;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001E7 RID: 487
	internal class MamlClassHelpInfo : HelpInfo
	{
		// Token: 0x06001647 RID: 5703 RVA: 0x0008EE8A File Offset: 0x0008D08A
		internal MamlClassHelpInfo(PSObject helpObject, HelpCategory helpCategory)
		{
			this._helpCategory = helpCategory;
			this._fullHelpObject = helpObject;
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x0008EEA0 File Offset: 0x0008D0A0
		private MamlClassHelpInfo(XmlNode xmlNode, HelpCategory helpCategory)
		{
			this._helpCategory = helpCategory;
			MamlNode mamlNode = new MamlNode(xmlNode);
			this._fullHelpObject = mamlNode.PSObject;
			base.Errors = mamlNode.Errors;
			this._fullHelpObject.TypeNames.Clear();
			this._fullHelpObject.TypeNames.Add("PSClassHelpInfo");
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0008EF00 File Offset: 0x0008D100
		internal static MamlClassHelpInfo Load(XmlNode xmlNode, HelpCategory helpCategory)
		{
			MamlClassHelpInfo mamlClassHelpInfo = new MamlClassHelpInfo(xmlNode, helpCategory);
			if (string.IsNullOrEmpty(mamlClassHelpInfo.Name))
			{
				return null;
			}
			mamlClassHelpInfo.AddCommonHelpProperties();
			return mamlClassHelpInfo;
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x0008EF2C File Offset: 0x0008D12C
		internal MamlClassHelpInfo Copy()
		{
			return new MamlClassHelpInfo(this._fullHelpObject.Copy(), this.HelpCategory);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0008EF54 File Offset: 0x0008D154
		internal MamlClassHelpInfo Copy(HelpCategory newCategoryToUse)
		{
			MamlClassHelpInfo mamlClassHelpInfo = new MamlClassHelpInfo(this._fullHelpObject.Copy(), newCategoryToUse);
			mamlClassHelpInfo.FullHelp.Properties["Category"].Value = newCategoryToUse;
			return mamlClassHelpInfo;
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x0600164C RID: 5708 RVA: 0x0008EF94 File Offset: 0x0008D194
		internal override string Name
		{
			get
			{
				string result = string.Empty;
				PSPropertyInfo pspropertyInfo = this._fullHelpObject.Properties["title"];
				if (pspropertyInfo != null && pspropertyInfo.Value != null)
				{
					result = pspropertyInfo.Value.ToString();
				}
				return result;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x0600164D RID: 5709 RVA: 0x0008EFD8 File Offset: 0x0008D1D8
		internal override string Synopsis
		{
			get
			{
				string result = string.Empty;
				PSPropertyInfo pspropertyInfo = this._fullHelpObject.Properties["introduction"];
				if (pspropertyInfo != null && pspropertyInfo.Value != null)
				{
					result = pspropertyInfo.Value.ToString();
				}
				return result;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x0600164E RID: 5710 RVA: 0x0008F019 File Offset: 0x0008D219
		internal override HelpCategory HelpCategory
		{
			get
			{
				return this._helpCategory;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x0600164F RID: 5711 RVA: 0x0008F021 File Offset: 0x0008D221
		internal override PSObject FullHelp
		{
			get
			{
				return this._fullHelpObject;
			}
		}

		// Token: 0x0400097A RID: 2426
		private HelpCategory _helpCategory;

		// Token: 0x0400097B RID: 2427
		private PSObject _fullHelpObject;
	}
}
