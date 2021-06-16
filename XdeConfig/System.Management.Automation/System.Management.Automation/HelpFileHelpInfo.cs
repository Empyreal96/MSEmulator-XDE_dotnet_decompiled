using System;
using System.IO;

namespace System.Management.Automation
{
	// Token: 0x020001BC RID: 444
	internal class HelpFileHelpInfo : HelpInfo
	{
		// Token: 0x060014A4 RID: 5284 RVA: 0x000803B4 File Offset: 0x0007E5B4
		private HelpFileHelpInfo(string name, string text, string filename)
		{
			this._fullHelpObject = PSObject.AsPSObject(text);
			this._name = name;
			this._synopsis = HelpFileHelpInfo.GetLine(text, 5);
			if (this._synopsis != null)
			{
				this._synopsis = this._synopsis.Trim();
			}
			else
			{
				this._synopsis = "";
			}
			this._filename = filename;
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x00080435 File Offset: 0x0007E635
		internal override string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060014A6 RID: 5286 RVA: 0x0008043D File Offset: 0x0007E63D
		internal override string Synopsis
		{
			get
			{
				return this._synopsis;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x00080445 File Offset: 0x0007E645
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.HelpFile;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x0008044C File Offset: 0x0007E64C
		internal override PSObject FullHelp
		{
			get
			{
				return this._fullHelpObject;
			}
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00080454 File Offset: 0x0007E654
		internal static HelpFileHelpInfo GetHelpInfo(string name, string text, string filename)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			HelpFileHelpInfo helpFileHelpInfo = new HelpFileHelpInfo(name, text, filename);
			if (string.IsNullOrEmpty(helpFileHelpInfo.Name))
			{
				return null;
			}
			helpFileHelpInfo.AddCommonHelpProperties();
			return helpFileHelpInfo;
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0008048C File Offset: 0x0007E68C
		private static string GetLine(string text, int line)
		{
			StringReader stringReader = new StringReader(text);
			string text2 = null;
			for (int i = 0; i < line; i++)
			{
				text2 = stringReader.ReadLine();
				if (text2 == null)
				{
					return null;
				}
			}
			return text2;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x000804BC File Offset: 0x0007E6BC
		internal override bool MatchPatternInContent(WildcardPattern pattern)
		{
			string empty = string.Empty;
			LanguagePrimitives.TryConvertTo<string>(this.FullHelp, out empty);
			return pattern.IsMatch(empty);
		}

		// Token: 0x040008E3 RID: 2275
		private string _name = "";

		// Token: 0x040008E4 RID: 2276
		private string _filename = "";

		// Token: 0x040008E5 RID: 2277
		private string _synopsis = "";

		// Token: 0x040008E6 RID: 2278
		private PSObject _fullHelpObject;
	}
}
