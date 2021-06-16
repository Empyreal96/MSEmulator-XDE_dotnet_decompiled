using System;
using System.Globalization;

namespace System.Management.Automation
{
	// Token: 0x02000085 RID: 133
	internal class PSSnapinQualifiedName
	{
		// Token: 0x060006C5 RID: 1733 RVA: 0x00020A1C File Offset: 0x0001EC1C
		private PSSnapinQualifiedName(string[] splitName)
		{
			if (splitName.Length == 1)
			{
				this._shortName = splitName[0];
			}
			else
			{
				if (splitName.Length != 2)
				{
					throw PSTraceSource.NewArgumentException("name");
				}
				if (!string.IsNullOrEmpty(splitName[0]))
				{
					this._psSnapinName = splitName[0];
				}
				this._shortName = splitName[1];
			}
			if (!string.IsNullOrEmpty(this._psSnapinName))
			{
				this._fullName = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", new object[]
				{
					this._psSnapinName,
					this._shortName
				});
				return;
			}
			this._fullName = this._shortName;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00020AB8 File Offset: 0x0001ECB8
		internal static PSSnapinQualifiedName GetInstance(string name)
		{
			if (name == null)
			{
				return null;
			}
			string[] array = name.Split(new char[]
			{
				'\\'
			});
			if (array.Length < 0 || array.Length > 2)
			{
				return null;
			}
			PSSnapinQualifiedName pssnapinQualifiedName = new PSSnapinQualifiedName(array);
			if (string.IsNullOrEmpty(pssnapinQualifiedName.ShortName))
			{
				return null;
			}
			return pssnapinQualifiedName;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00020B05 File Offset: 0x0001ED05
		internal string FullName
		{
			get
			{
				return this._fullName;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00020B0D File Offset: 0x0001ED0D
		internal string PSSnapInName
		{
			get
			{
				return this._psSnapinName;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00020B15 File Offset: 0x0001ED15
		internal string ShortName
		{
			get
			{
				return this._shortName;
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00020B1D File Offset: 0x0001ED1D
		public override string ToString()
		{
			return this._fullName;
		}

		// Token: 0x040002C6 RID: 710
		private string _fullName;

		// Token: 0x040002C7 RID: 711
		private string _psSnapinName;

		// Token: 0x040002C8 RID: 712
		private string _shortName;
	}
}
