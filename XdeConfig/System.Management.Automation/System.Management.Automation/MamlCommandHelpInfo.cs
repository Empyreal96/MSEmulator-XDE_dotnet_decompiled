using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001B7 RID: 439
	internal class MamlCommandHelpInfo : BaseCommandHelpInfo
	{
		// Token: 0x06001464 RID: 5220 RVA: 0x0007D658 File Offset: 0x0007B858
		internal MamlCommandHelpInfo(PSObject helpObject, HelpCategory helpCategory) : base(helpCategory)
		{
			this._fullHelpObject = helpObject;
			base.ForwardHelpCategory = HelpCategory.Provider;
			base.AddCommonHelpProperties();
			if (helpObject.Properties["Component"] != null)
			{
				this._component = (helpObject.Properties["Component"].Value as string);
			}
			if (helpObject.Properties["Role"] != null)
			{
				this._role = (helpObject.Properties["Role"].Value as string);
			}
			if (helpObject.Properties["Functionality"] != null)
			{
				this._functionality = (helpObject.Properties["Functionality"].Value as string);
			}
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0007D718 File Offset: 0x0007B918
		private MamlCommandHelpInfo(XmlNode xmlNode, HelpCategory helpCategory) : base(helpCategory)
		{
			MamlNode mamlNode = new MamlNode(xmlNode);
			this._fullHelpObject = mamlNode.PSObject;
			base.Errors = mamlNode.Errors;
			this._fullHelpObject.TypeNames.Clear();
			if (helpCategory == HelpCategory.DscResource)
			{
				this._fullHelpObject.TypeNames.Add("DscResourceHelpInfo");
			}
			else
			{
				this._fullHelpObject.TypeNames.Add("MamlCommandHelpInfo");
				this._fullHelpObject.TypeNames.Add("HelpInfo");
			}
			base.ForwardHelpCategory = HelpCategory.Provider;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0007D7AC File Offset: 0x0007B9AC
		internal void OverrideProviderSpecificHelpWithGenericHelp(HelpInfo genericHelpInfo)
		{
			PSObject fullHelp = genericHelpInfo.FullHelp;
			MamlUtil.OverrideName(this._fullHelpObject, fullHelp);
			MamlUtil.OverridePSTypeNames(this._fullHelpObject, fullHelp);
			MamlUtil.PrependSyntax(this._fullHelpObject, fullHelp);
			MamlUtil.PrependDetailedDescription(this._fullHelpObject, fullHelp);
			MamlUtil.OverrideParameters(this._fullHelpObject, fullHelp);
			MamlUtil.PrependNotes(this._fullHelpObject, fullHelp);
			MamlUtil.AddCommonProperties(this._fullHelpObject, fullHelp);
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001467 RID: 5223 RVA: 0x0007D814 File Offset: 0x0007BA14
		internal override PSObject FullHelp
		{
			get
			{
				return this._fullHelpObject;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x0007D81C File Offset: 0x0007BA1C
		private string Examples
		{
			get
			{
				return this.ExtractTextForHelpProperty(this.FullHelp, "Examples");
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x0007D82F File Offset: 0x0007BA2F
		private string Parameters
		{
			get
			{
				return this.ExtractTextForHelpProperty(this.FullHelp, "Parameters");
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x0007D842 File Offset: 0x0007BA42
		private string Notes
		{
			get
			{
				return this.ExtractTextForHelpProperty(this.FullHelp, "alertset");
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600146B RID: 5227 RVA: 0x0007D855 File Offset: 0x0007BA55
		internal override string Component
		{
			get
			{
				return this._component;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x0007D85D File Offset: 0x0007BA5D
		internal override string Role
		{
			get
			{
				return this._role;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x0007D865 File Offset: 0x0007BA65
		internal override string Functionality
		{
			get
			{
				return this._functionality;
			}
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0007D86D File Offset: 0x0007BA6D
		internal void SetAdditionalDataFromHelpComment(string component, string functionality, string role)
		{
			this._component = component;
			this._functionality = functionality;
			this._role = role;
			base.UpdateUserDefinedDataProperties();
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0007D88C File Offset: 0x0007BA8C
		internal void AddUserDefinedData(UserDefinedHelpData userDefinedData)
		{
			if (userDefinedData == null)
			{
				return;
			}
			if (userDefinedData.Properties.ContainsKey("component"))
			{
				this._component = userDefinedData.Properties["component"];
			}
			if (userDefinedData.Properties.ContainsKey("role"))
			{
				this._role = userDefinedData.Properties["role"];
			}
			if (userDefinedData.Properties.ContainsKey("functionality"))
			{
				this._functionality = userDefinedData.Properties["functionality"];
			}
			base.UpdateUserDefinedDataProperties();
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0007D91C File Offset: 0x0007BB1C
		internal static MamlCommandHelpInfo Load(XmlNode xmlNode, HelpCategory helpCategory)
		{
			MamlCommandHelpInfo mamlCommandHelpInfo = new MamlCommandHelpInfo(xmlNode, helpCategory);
			if (string.IsNullOrEmpty(mamlCommandHelpInfo.Name))
			{
				return null;
			}
			mamlCommandHelpInfo.AddCommonHelpProperties();
			return mamlCommandHelpInfo;
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0007D948 File Offset: 0x0007BB48
		private string ExtractTextForHelpProperty(PSObject psObject, string propertyName)
		{
			if (psObject == null)
			{
				return string.Empty;
			}
			if (psObject.Properties[propertyName] == null || psObject.Properties[propertyName].Value == null)
			{
				return string.Empty;
			}
			return this.ExtractText(PSObject.AsPSObject(psObject.Properties[propertyName].Value));
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0007D9A4 File Offset: 0x0007BBA4
		private string ExtractText(PSObject psObject)
		{
			if (psObject == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(400);
			foreach (PSPropertyInfo pspropertyInfo in psObject.Properties)
			{
				string typeNameOfValue = pspropertyInfo.TypeNameOfValue;
				string key;
				switch (key = typeNameOfValue.ToLowerInvariant())
				{
				case "system.boolean":
				case "system.int32":
				case "system.object":
				case "system.object[]":
					continue;
				case "system.string":
					stringBuilder.Append((string)LanguagePrimitives.ConvertTo(pspropertyInfo.Value, typeof(string), CultureInfo.InvariantCulture));
					continue;
				case "system.management.automation.psobject[]":
				{
					PSObject[] array = (PSObject[])LanguagePrimitives.ConvertTo(pspropertyInfo.Value, typeof(PSObject[]), CultureInfo.InvariantCulture);
					foreach (PSObject psObject2 in array)
					{
						stringBuilder.Append(this.ExtractText(psObject2));
					}
					continue;
				}
				case "system.management.automation.psobject":
					stringBuilder.Append(this.ExtractText(PSObject.AsPSObject(pspropertyInfo.Value)));
					continue;
				}
				stringBuilder.Append(this.ExtractText(PSObject.AsPSObject(pspropertyInfo.Value)));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0007DB84 File Offset: 0x0007BD84
		internal override bool MatchPatternInContent(WildcardPattern pattern)
		{
			string synopsis = this.Synopsis;
			if (!string.IsNullOrEmpty(synopsis) && pattern.IsMatch(synopsis))
			{
				return true;
			}
			string detailedDescription = base.DetailedDescription;
			if (!string.IsNullOrEmpty(detailedDescription) && pattern.IsMatch(detailedDescription))
			{
				return true;
			}
			string examples = this.Examples;
			if (!string.IsNullOrEmpty(examples) && pattern.IsMatch(examples))
			{
				return true;
			}
			string notes = this.Notes;
			if (!string.IsNullOrEmpty(notes) && pattern.IsMatch(notes))
			{
				return true;
			}
			string parameters = this.Parameters;
			return !string.IsNullOrEmpty(parameters) && pattern.IsMatch(parameters);
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0007DC18 File Offset: 0x0007BE18
		internal MamlCommandHelpInfo Copy()
		{
			return new MamlCommandHelpInfo(this._fullHelpObject.Copy(), this.HelpCategory);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0007DC40 File Offset: 0x0007BE40
		internal MamlCommandHelpInfo Copy(HelpCategory newCategoryToUse)
		{
			MamlCommandHelpInfo mamlCommandHelpInfo = new MamlCommandHelpInfo(this._fullHelpObject.Copy(), newCategoryToUse);
			mamlCommandHelpInfo.FullHelp.Properties["Category"].Value = newCategoryToUse;
			return mamlCommandHelpInfo;
		}

		// Token: 0x040008D3 RID: 2259
		private PSObject _fullHelpObject;

		// Token: 0x040008D4 RID: 2260
		private string _component;

		// Token: 0x040008D5 RID: 2261
		private string _role;

		// Token: 0x040008D6 RID: 2262
		private string _functionality;
	}
}
