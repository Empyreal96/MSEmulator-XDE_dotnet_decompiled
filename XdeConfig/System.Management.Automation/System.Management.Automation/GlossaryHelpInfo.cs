using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace System.Management.Automation
{
	// Token: 0x020001C8 RID: 456
	internal class GlossaryHelpInfo : HelpInfo
	{
		// Token: 0x06001518 RID: 5400 RVA: 0x00083DC8 File Offset: 0x00081FC8
		protected GlossaryHelpInfo(XmlNode xmlNode)
		{
			MamlNode mamlNode = new MamlNode(xmlNode);
			this._fullHelpObject = mamlNode.PSObject;
			base.Errors = mamlNode.Errors;
			this._name = this.GetTerm();
			this._fullHelpObject.TypeNames.Clear();
			this._fullHelpObject.TypeNames.Add(string.Format(CultureInfo.InvariantCulture, "GlossaryHelpInfo#{0}", new object[]
			{
				this.Name
			}));
			this._fullHelpObject.TypeNames.Add("GlossaryHelpInfo");
			this._fullHelpObject.TypeNames.Add("HelpInfo");
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001519 RID: 5401 RVA: 0x00083E7B File Offset: 0x0008207B
		internal override string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00083E84 File Offset: 0x00082084
		private string GetTerm()
		{
			if (this._fullHelpObject == null)
			{
				return "";
			}
			if (this._fullHelpObject.Properties["Terms"] == null)
			{
				return "";
			}
			if (this._fullHelpObject.Properties["Terms"].Value == null)
			{
				return "";
			}
			PSObject psobject = (PSObject)this._fullHelpObject.Properties["Terms"].Value;
			if (psobject.Properties["Term"] == null)
			{
				return "";
			}
			if (psobject.Properties["Term"].Value == null)
			{
				return "";
			}
			if (psobject.Properties["Term"].Value.GetType().Equals(typeof(PSObject)))
			{
				PSObject psobject2 = (PSObject)psobject.Properties["Term"].Value;
				return psobject2.ToString();
			}
			if (psobject.Properties["Term"].Value.GetType().Equals(typeof(PSObject[])))
			{
				PSObject[] array = (PSObject[])psobject.Properties["Term"].Value;
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i].ToString();
					if (text != null)
					{
						text = text.Trim();
						if (!string.IsNullOrEmpty(text))
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append(text);
						}
					}
				}
				return stringBuilder.ToString();
			}
			return "";
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x0600151B RID: 5403 RVA: 0x00084029 File Offset: 0x00082229
		internal override string Synopsis
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x00084030 File Offset: 0x00082230
		internal override HelpCategory HelpCategory
		{
			get
			{
				return HelpCategory.Glossary;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x0600151D RID: 5405 RVA: 0x00084034 File Offset: 0x00082234
		internal override PSObject FullHelp
		{
			get
			{
				return this._fullHelpObject;
			}
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0008403C File Offset: 0x0008223C
		internal static GlossaryHelpInfo Load(XmlNode xmlNode)
		{
			GlossaryHelpInfo glossaryHelpInfo = new GlossaryHelpInfo(xmlNode);
			if (string.IsNullOrEmpty(glossaryHelpInfo.Name))
			{
				return null;
			}
			glossaryHelpInfo.AddCommonHelpProperties();
			return glossaryHelpInfo;
		}

		// Token: 0x040008F5 RID: 2293
		private string _name = "";

		// Token: 0x040008F6 RID: 2294
		private PSObject _fullHelpObject;
	}
}
