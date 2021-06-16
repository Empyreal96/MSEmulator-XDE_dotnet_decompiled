using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x0200040C RID: 1036
	internal class PSSnapInTypeAndFormatErrors
	{
		// Token: 0x06002E7E RID: 11902 RVA: 0x000FFBAF File Offset: 0x000FDDAF
		internal PSSnapInTypeAndFormatErrors(string psSnapinName, string fullPath)
		{
			this.psSnapinName = psSnapinName;
			this.fullPath = fullPath;
			this.errors = new Collection<string>();
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000FFBD0 File Offset: 0x000FDDD0
		internal PSSnapInTypeAndFormatErrors(string psSnapinName, FormatTable formatTable)
		{
			this.psSnapinName = psSnapinName;
			this.formatTable = formatTable;
			this.errors = new Collection<string>();
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000FFBF1 File Offset: 0x000FDDF1
		internal PSSnapInTypeAndFormatErrors(string psSnapinName, TypeData typeData, bool isRemove)
		{
			this.psSnapinName = psSnapinName;
			this.typeData = typeData;
			this.isRemove = isRemove;
			this.errors = new Collection<string>();
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000FFC19 File Offset: 0x000FDE19
		internal PSSnapInTypeAndFormatErrors(string psSnapinName, ExtendedTypeDefinition typeDefinition)
		{
			this.psSnapinName = psSnapinName;
			this.typeDefinition = typeDefinition;
			this.errors = new Collection<string>();
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06002E82 RID: 11906 RVA: 0x000FFC3A File Offset: 0x000FDE3A
		internal ExtendedTypeDefinition FormatData
		{
			get
			{
				return this.typeDefinition;
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06002E83 RID: 11907 RVA: 0x000FFC42 File Offset: 0x000FDE42
		internal TypeData TypeData
		{
			get
			{
				return this.typeData;
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06002E84 RID: 11908 RVA: 0x000FFC4A File Offset: 0x000FDE4A
		internal bool IsRemove
		{
			get
			{
				return this.isRemove;
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06002E85 RID: 11909 RVA: 0x000FFC52 File Offset: 0x000FDE52
		internal string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06002E86 RID: 11910 RVA: 0x000FFC5A File Offset: 0x000FDE5A
		internal FormatTable FormatTable
		{
			get
			{
				return this.formatTable;
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06002E87 RID: 11911 RVA: 0x000FFC62 File Offset: 0x000FDE62
		// (set) Token: 0x06002E88 RID: 11912 RVA: 0x000FFC6A File Offset: 0x000FDE6A
		internal Collection<string> Errors
		{
			get
			{
				return this.errors;
			}
			set
			{
				this.errors = value;
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06002E89 RID: 11913 RVA: 0x000FFC73 File Offset: 0x000FDE73
		internal string PSSnapinName
		{
			get
			{
				return this.psSnapinName;
			}
		}

		// Token: 0x04001870 RID: 6256
		public string psSnapinName;

		// Token: 0x04001871 RID: 6257
		private string fullPath;

		// Token: 0x04001872 RID: 6258
		private FormatTable formatTable;

		// Token: 0x04001873 RID: 6259
		private TypeData typeData;

		// Token: 0x04001874 RID: 6260
		private bool isRemove;

		// Token: 0x04001875 RID: 6261
		private ExtendedTypeDefinition typeDefinition;

		// Token: 0x04001876 RID: 6262
		private Collection<string> errors;

		// Token: 0x04001877 RID: 6263
		internal bool FailToLoadFile;
	}
}
