using System;

namespace System.Management.Automation
{
	// Token: 0x020001D3 RID: 467
	internal class SyntaxHelpInfo : BaseCommandHelpInfo
	{
		// Token: 0x06001581 RID: 5505 RVA: 0x00087138 File Offset: 0x00085338
		private SyntaxHelpInfo(string name, string text, HelpCategory category) : base(category)
		{
			this._fullHelpObject = PSObject.AsPSObject(text);
			this._name = name;
			this._synopsis = text;
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x00087171 File Offset: 0x00085371
		internal override string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001583 RID: 5507 RVA: 0x00087179 File Offset: 0x00085379
		internal override string Synopsis
		{
			get
			{
				return this._synopsis;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x00087181 File Offset: 0x00085381
		internal override PSObject FullHelp
		{
			get
			{
				return this._fullHelpObject;
			}
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0008718C File Offset: 0x0008538C
		internal static SyntaxHelpInfo GetHelpInfo(string name, string text, HelpCategory category)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			SyntaxHelpInfo syntaxHelpInfo = new SyntaxHelpInfo(name, text, category);
			if (string.IsNullOrEmpty(syntaxHelpInfo.Name))
			{
				return null;
			}
			syntaxHelpInfo.AddCommonHelpProperties();
			return syntaxHelpInfo;
		}

		// Token: 0x04000925 RID: 2341
		private string _name = "";

		// Token: 0x04000926 RID: 2342
		private string _synopsis = "";

		// Token: 0x04000927 RID: 2343
		private PSObject _fullHelpObject;
	}
}
