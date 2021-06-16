using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000364 RID: 868
	internal abstract class BaseClientSessionTransportManager : BaseClientTransportManager, IDisposable
	{
		// Token: 0x06002AF7 RID: 10999 RVA: 0x000EC3FE File Offset: 0x000EA5FE
		protected BaseClientSessionTransportManager(Guid runspaceId, PSRemotingCryptoHelper cryptoHelper) : base(runspaceId, cryptoHelper)
		{
		}

		// Token: 0x06002AF8 RID: 11000 RVA: 0x000EC408 File Offset: 0x000EA608
		internal virtual BaseClientCommandTransportManager CreateClientCommandTransportManager(RunspaceConnectionInfo connectionInfo, ClientRemotePowerShell cmd, bool noInput)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000EC40F File Offset: 0x000EA60F
		internal virtual void RemoveCommandTransportManager(Guid powerShellCmdId)
		{
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000EC411 File Offset: 0x000EA611
		internal virtual void DisconnectAsync()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000EC418 File Offset: 0x000EA618
		internal virtual void ReconnectAsync()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x000EC41F File Offset: 0x000EA61F
		internal virtual void Redirect(Uri newUri, RunspaceConnectionInfo connectionInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x000EC426 File Offset: 0x000EA626
		internal virtual void PrepareForRedirection()
		{
			throw new NotImplementedException();
		}
	}
}
