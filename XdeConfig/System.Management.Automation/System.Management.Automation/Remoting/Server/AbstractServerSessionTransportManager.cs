using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x02000367 RID: 871
	internal abstract class AbstractServerSessionTransportManager : AbstractServerTransportManager
	{
		// Token: 0x06002B12 RID: 11026 RVA: 0x000EC9B6 File Offset: 0x000EABB6
		protected AbstractServerSessionTransportManager(int fragmentSize, PSRemotingCryptoHelper cryptoHelper) : base(fragmentSize, cryptoHelper)
		{
		}

		// Token: 0x06002B13 RID: 11027
		internal abstract AbstractServerTransportManager GetCommandTransportManager(Guid powerShellCmdId);

		// Token: 0x06002B14 RID: 11028
		internal abstract void RemoveCommandTransportManager(Guid powerShellCmdId);
	}
}
