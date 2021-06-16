using System;
using System.Globalization;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	// Token: 0x0200003C RID: 60
	public class RestartSettings
	{
		// Token: 0x06000210 RID: 528 RVA: 0x00005D00 File Offset: 0x00003F00
		public RestartSettings(string command, RestartRestrictions restrictions)
		{
			this.command = command;
			this.restrictions = restrictions;
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00005D16 File Offset: 0x00003F16
		public string Command
		{
			get
			{
				return this.command;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00005D1E File Offset: 0x00003F1E
		public RestartRestrictions Restrictions
		{
			get
			{
				return this.restrictions;
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00005D26 File Offset: 0x00003F26
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, LocalizedMessages.RestartSettingsFormatString, this.command, this.restrictions.ToString());
		}

		// Token: 0x040001A7 RID: 423
		private string command;

		// Token: 0x040001A8 RID: 424
		private RestartRestrictions restrictions;
	}
}
