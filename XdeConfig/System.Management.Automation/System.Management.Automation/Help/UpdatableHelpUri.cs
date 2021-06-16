using System;
using System.Globalization;

namespace System.Management.Automation.Help
{
	// Token: 0x020001E0 RID: 480
	internal class UpdatableHelpUri
	{
		// Token: 0x0600160A RID: 5642 RVA: 0x0008C2EE File Offset: 0x0008A4EE
		internal UpdatableHelpUri(string moduleName, Guid moduleGuid, CultureInfo culture, string resolvedUri)
		{
			this._moduleName = moduleName;
			this._moduleGuid = moduleGuid;
			this._culture = culture;
			this._resolvedUri = resolvedUri;
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600160B RID: 5643 RVA: 0x0008C313 File Offset: 0x0008A513
		internal string ModuleName
		{
			get
			{
				return this._moduleName;
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x0008C31B File Offset: 0x0008A51B
		internal Guid ModuleGuid
		{
			get
			{
				return this._moduleGuid;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x0600160D RID: 5645 RVA: 0x0008C323 File Offset: 0x0008A523
		internal CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x0008C32B File Offset: 0x0008A52B
		internal string ResolvedUri
		{
			get
			{
				return this._resolvedUri;
			}
		}

		// Token: 0x04000964 RID: 2404
		private string _moduleName;

		// Token: 0x04000965 RID: 2405
		private Guid _moduleGuid;

		// Token: 0x04000966 RID: 2406
		private CultureInfo _culture;

		// Token: 0x04000967 RID: 2407
		private string _resolvedUri;
	}
}
