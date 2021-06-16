using System;
using System.Management.Automation.Host;
using System.Management.Automation.Internal.Host;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002E6 RID: 742
	internal class HostInfo
	{
		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x000C74C8 File Offset: 0x000C56C8
		internal HostDefaultData HostDefaultData
		{
			get
			{
				return this._hostDefaultData;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x000C74D0 File Offset: 0x000C56D0
		internal bool IsHostNull
		{
			get
			{
				return this._isHostNull;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x0600236C RID: 9068 RVA: 0x000C74D8 File Offset: 0x000C56D8
		internal bool IsHostUINull
		{
			get
			{
				return this._isHostUINull;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x0600236D RID: 9069 RVA: 0x000C74E0 File Offset: 0x000C56E0
		internal bool IsHostRawUINull
		{
			get
			{
				return this._isHostRawUINull;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x0600236E RID: 9070 RVA: 0x000C74E8 File Offset: 0x000C56E8
		// (set) Token: 0x0600236F RID: 9071 RVA: 0x000C74F0 File Offset: 0x000C56F0
		internal bool UseRunspaceHost
		{
			get
			{
				return this._useRunspaceHost;
			}
			set
			{
				this._useRunspaceHost = value;
			}
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x000C74FC File Offset: 0x000C56FC
		internal HostInfo(PSHost host)
		{
			HostInfo.CheckHostChain(host, ref this._isHostNull, ref this._isHostUINull, ref this._isHostRawUINull);
			if (!this._isHostUINull && !this._isHostRawUINull)
			{
				this._hostDefaultData = HostDefaultData.Create(host.UI.RawUI);
			}
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x000C754D File Offset: 0x000C574D
		internal HostInfo()
		{
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000C7558 File Offset: 0x000C5758
		private static void CheckHostChain(PSHost host, ref bool isHostNull, ref bool isHostUINull, ref bool isHostRawUINull)
		{
			isHostNull = true;
			isHostUINull = true;
			isHostRawUINull = true;
			if (host == null)
			{
				return;
			}
			if (host is InternalHost)
			{
				host = ((InternalHost)host).ExternalHost;
			}
			isHostNull = false;
			if (host.UI == null)
			{
				return;
			}
			isHostUINull = false;
			if (host.UI.RawUI == null)
			{
				return;
			}
			isHostRawUINull = false;
		}

		// Token: 0x04001137 RID: 4407
		private HostDefaultData _hostDefaultData;

		// Token: 0x04001138 RID: 4408
		private bool _isHostNull;

		// Token: 0x04001139 RID: 4409
		private bool _isHostUINull;

		// Token: 0x0400113A RID: 4410
		private bool _isHostRawUINull;

		// Token: 0x0400113B RID: 4411
		private bool _useRunspaceHost;
	}
}
