using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Text;

namespace System.Management.Automation.Help
{
	// Token: 0x020001D8 RID: 472
	internal class UpdatableHelpInfo
	{
		// Token: 0x060015C3 RID: 5571 RVA: 0x0008A315 File Offset: 0x00088515
		internal UpdatableHelpInfo(string unresolvedUri, CultureSpecificUpdatableHelp[] cultures)
		{
			this._unresolvedUri = unresolvedUri;
			this._helpContentUriCollection = new Collection<UpdatableHelpUri>();
			this._updatableHelpItems = cultures;
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x0008A336 File Offset: 0x00088536
		internal string UnresolvedUri
		{
			get
			{
				return this._unresolvedUri;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060015C5 RID: 5573 RVA: 0x0008A33E File Offset: 0x0008853E
		internal Collection<UpdatableHelpUri> HelpContentUriCollection
		{
			get
			{
				return this._helpContentUriCollection;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x0008A346 File Offset: 0x00088546
		internal CultureSpecificUpdatableHelp[] UpdatableHelpItems
		{
			get
			{
				return this._updatableHelpItems;
			}
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0008A350 File Offset: 0x00088550
		internal bool IsNewerVersion(UpdatableHelpInfo helpInfo, CultureInfo culture)
		{
			Version cultureVersion = helpInfo.GetCultureVersion(culture);
			Version cultureVersion2 = this.GetCultureVersion(culture);
			return cultureVersion2 == null || cultureVersion > cultureVersion2;
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0008A380 File Offset: 0x00088580
		internal bool IsCultureSupported(CultureInfo culture)
		{
			foreach (CultureSpecificUpdatableHelp cultureSpecificUpdatableHelp in this._updatableHelpItems)
			{
				if (string.Compare(cultureSpecificUpdatableHelp.Culture.Name, culture.Name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x0008A3C8 File Offset: 0x000885C8
		internal string GetSupportedCultures()
		{
			if (this._updatableHelpItems.Length == 0)
			{
				return StringUtil.Format(HelpDisplayStrings.None, new object[0]);
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this._updatableHelpItems.Length; i++)
			{
				stringBuilder.Append(this._updatableHelpItems[i].Culture.Name);
				if (i != this._updatableHelpItems.Length - 1)
				{
					stringBuilder.Append(" | ");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0008A444 File Offset: 0x00088644
		internal Version GetCultureVersion(CultureInfo culture)
		{
			foreach (CultureSpecificUpdatableHelp cultureSpecificUpdatableHelp in this._updatableHelpItems)
			{
				if (string.Compare(cultureSpecificUpdatableHelp.Culture.Name, culture.Name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return cultureSpecificUpdatableHelp.Version;
				}
			}
			return null;
		}

		// Token: 0x04000938 RID: 2360
		private string _unresolvedUri;

		// Token: 0x04000939 RID: 2361
		private Collection<UpdatableHelpUri> _helpContentUriCollection;

		// Token: 0x0400093A RID: 2362
		private CultureSpecificUpdatableHelp[] _updatableHelpItems;
	}
}
