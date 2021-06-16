using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x020001A9 RID: 425
	internal abstract class HelpInfo
	{
		// Token: 0x060013D7 RID: 5079 RVA: 0x00079D25 File Offset: 0x00077F25
		internal HelpInfo()
		{
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060013D8 RID: 5080
		internal abstract string Name { get; }

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060013D9 RID: 5081
		internal abstract string Synopsis { get; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x00079D38 File Offset: 0x00077F38
		internal virtual string Component
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060013DB RID: 5083 RVA: 0x00079D3F File Offset: 0x00077F3F
		internal virtual string Role
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x00079D46 File Offset: 0x00077F46
		internal virtual string Functionality
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060013DD RID: 5085
		internal abstract HelpCategory HelpCategory { get; }

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x00079D4D File Offset: 0x00077F4D
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x00079D55 File Offset: 0x00077F55
		internal HelpCategory ForwardHelpCategory
		{
			get
			{
				return this._forwardHelpCategory;
			}
			set
			{
				this._forwardHelpCategory = value;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x00079D5E File Offset: 0x00077F5E
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x00079D66 File Offset: 0x00077F66
		internal string ForwardTarget
		{
			get
			{
				return this._forwardTarget;
			}
			set
			{
				this._forwardTarget = value;
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060013E2 RID: 5090
		internal abstract PSObject FullHelp { get; }

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x00079D70 File Offset: 0x00077F70
		internal PSObject ShortHelp
		{
			get
			{
				if (this.FullHelp == null)
				{
					return null;
				}
				PSObject psobject = new PSObject(this.FullHelp);
				psobject.TypeNames.Clear();
				psobject.TypeNames.Add("HelpInfoShort");
				return psobject;
			}
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00079DAF File Offset: 0x00077FAF
		internal virtual PSObject[] GetParameter(string pattern)
		{
			return new PSObject[0];
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00079DB7 File Offset: 0x00077FB7
		internal virtual Uri GetUriForOnlineHelp()
		{
			return null;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00079DBA File Offset: 0x00077FBA
		internal virtual bool MatchPatternInContent(WildcardPattern pattern)
		{
			return false;
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00079DC0 File Offset: 0x00077FC0
		protected void AddCommonHelpProperties()
		{
			if (this.FullHelp == null)
			{
				return;
			}
			if (this.FullHelp.Properties["Name"] == null)
			{
				this.FullHelp.Properties.Add(new PSNoteProperty("Name", this.Name.ToString()));
			}
			if (this.FullHelp.Properties["Category"] == null)
			{
				this.FullHelp.Properties.Add(new PSNoteProperty("Category", this.HelpCategory.ToString()));
			}
			if (this.FullHelp.Properties["Synopsis"] == null)
			{
				this.FullHelp.Properties.Add(new PSNoteProperty("Synopsis", this.Synopsis.ToString()));
			}
			if (this.FullHelp.Properties["Component"] == null)
			{
				this.FullHelp.Properties.Add(new PSNoteProperty("Component", this.Component));
			}
			if (this.FullHelp.Properties["Role"] == null)
			{
				this.FullHelp.Properties.Add(new PSNoteProperty("Role", this.Role));
			}
			if (this.FullHelp.Properties["Functionality"] == null)
			{
				this.FullHelp.Properties.Add(new PSNoteProperty("Functionality", this.Functionality));
			}
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00079F34 File Offset: 0x00078134
		protected void UpdateUserDefinedDataProperties()
		{
			if (this.FullHelp == null)
			{
				return;
			}
			this.FullHelp.Properties.Remove("Component");
			this.FullHelp.Properties.Add(new PSNoteProperty("Component", this.Component));
			this.FullHelp.Properties.Remove("Role");
			this.FullHelp.Properties.Add(new PSNoteProperty("Role", this.Role));
			this.FullHelp.Properties.Remove("Functionality");
			this.FullHelp.Properties.Add(new PSNoteProperty("Functionality", this.Functionality));
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00079FE9 File Offset: 0x000781E9
		// (set) Token: 0x060013EA RID: 5098 RVA: 0x00079FF1 File Offset: 0x000781F1
		internal Collection<ErrorRecord> Errors
		{
			get
			{
				return this._errors;
			}
			set
			{
				this._errors = value;
			}
		}

		// Token: 0x0400088F RID: 2191
		private HelpCategory _forwardHelpCategory;

		// Token: 0x04000890 RID: 2192
		private string _forwardTarget = "";

		// Token: 0x04000891 RID: 2193
		private Collection<ErrorRecord> _errors;
	}
}
