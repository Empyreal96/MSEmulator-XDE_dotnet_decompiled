using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001C6 RID: 454
	internal class FaqHelpInfo : HelpInfo
	{
		// Token: 0x06001508 RID: 5384 RVA: 0x00083600 File Offset: 0x00081800
		protected FaqHelpInfo(XmlNode xmlNode)
		{
			MamlNode mamlNode = new MamlNode(xmlNode);
			this._fullHelpObject = mamlNode.PSObject;
			base.Errors = mamlNode.Errors;
			this._fullHelpObject.TypeNames.Clear();
			this._fullHelpObject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "FaqHelpInfo#{0}", new object[]
			{
				this.Name
			}));
			this._fullHelpObject.TypeNames.Add("FaqHelpInfo");
			this._fullHelpObject.TypeNames.Add("HelpInfo");
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x0008369C File Offset: 0x0008189C
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

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x00083728 File Offset: 0x00081928
		internal override string Synopsis
		{
			get
			{
				if (this._fullHelpObject == null)
				{
					return "";
				}
				if (this._fullHelpObject.Properties["question"] == null)
				{
					return "";
				}
				if (this._fullHelpObject.Properties["question"].Value == null)
				{
					return "";
				}
				string text = this._fullHelpObject.Properties["question"].Value.ToString();
				if (text == null)
				{
					return "";
				}
				return text.Trim();
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x000837B1 File Offset: 0x000819B1
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.FAQ;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x000837B5 File Offset: 0x000819B5
		internal override PSObject FullHelp
		{
			get
			{
				return this._fullHelpObject;
			}
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x000837C0 File Offset: 0x000819C0
		internal override bool MatchPatternInContent(WildcardPattern pattern)
		{
			string text = this.Synopsis;
			string input = this.Answers;
			if (text == null)
			{
				text = string.Empty;
			}
			if (this.Answers == null)
			{
				input = string.Empty;
			}
			return pattern.IsMatch(text) || pattern.IsMatch(input);
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x00083804 File Offset: 0x00081A04
		private string Answers
		{
			get
			{
				if (this.FullHelp == null)
				{
					return "";
				}
				if (this.FullHelp.Properties["answer"] == null || this.FullHelp.Properties["answer"].Value == null)
				{
					return "";
				}
				IList list = this.FullHelp.Properties["answer"].Value as IList;
				if (list == null || list.Count == 0)
				{
					return "";
				}
				StringBuilder stringBuilder = new StringBuilder();
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

		// Token: 0x0600150F RID: 5391 RVA: 0x00083940 File Offset: 0x00081B40
		internal static FaqHelpInfo Load(XmlNode xmlNode)
		{
			FaqHelpInfo faqHelpInfo = new FaqHelpInfo(xmlNode);
			if (string.IsNullOrEmpty(faqHelpInfo.Name))
			{
				return null;
			}
			faqHelpInfo.AddCommonHelpProperties();
			return faqHelpInfo;
		}

		// Token: 0x040008F3 RID: 2291
		private PSObject _fullHelpObject;
	}
}
