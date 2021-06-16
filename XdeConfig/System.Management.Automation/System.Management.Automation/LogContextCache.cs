using System;

namespace System.Management.Automation
{
	// Token: 0x020003F7 RID: 1015
	internal class LogContextCache
	{
		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06002DF2 RID: 11762 RVA: 0x000FD58D File Offset: 0x000FB78D
		// (set) Token: 0x06002DF3 RID: 11763 RVA: 0x000FD595 File Offset: 0x000FB795
		internal string User
		{
			get
			{
				return this.user;
			}
			set
			{
				this.user = value;
			}
		}

		// Token: 0x04001810 RID: 6160
		private string user;
	}
}
