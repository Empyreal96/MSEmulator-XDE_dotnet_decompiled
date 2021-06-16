using System;
using System.Globalization;

namespace System.Management.Automation.Help
{
	// Token: 0x020001D7 RID: 471
	internal class CultureSpecificUpdatableHelp
	{
		// Token: 0x060015BE RID: 5566 RVA: 0x0008A2DD File Offset: 0x000884DD
		internal CultureSpecificUpdatableHelp(CultureInfo culture, Version version)
		{
			this._culture = culture;
			this._version = version;
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060015BF RID: 5567 RVA: 0x0008A2F3 File Offset: 0x000884F3
		// (set) Token: 0x060015C0 RID: 5568 RVA: 0x0008A2FB File Offset: 0x000884FB
		internal Version Version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x0008A304 File Offset: 0x00088504
		// (set) Token: 0x060015C2 RID: 5570 RVA: 0x0008A30C File Offset: 0x0008850C
		internal CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				this._culture = value;
			}
		}

		// Token: 0x04000936 RID: 2358
		private Version _version;

		// Token: 0x04000937 RID: 2359
		private CultureInfo _culture;
	}
}
