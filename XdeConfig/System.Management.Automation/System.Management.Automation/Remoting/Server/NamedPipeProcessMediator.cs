using System;
using System.Security;
using System.Security.Principal;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x020002F7 RID: 759
	internal sealed class NamedPipeProcessMediator : OutOfProcessMediatorBase
	{
		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x060023F1 RID: 9201 RVA: 0x000C9CCC File Offset: 0x000C7ECC
		internal bool IsDisposed
		{
			get
			{
				return this._namedPipeServer.IsDisposed;
			}
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000C9CD9 File Offset: 0x000C7ED9
		private NamedPipeProcessMediator() : base(false)
		{
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000C9CE4 File Offset: 0x000C7EE4
		private NamedPipeProcessMediator(RemoteSessionNamedPipeServer namedPipeServer) : base(false)
		{
			if (namedPipeServer == null)
			{
				throw new PSArgumentNullException("namedPipeServer");
			}
			this._namedPipeServer = namedPipeServer;
			this.originalStdIn = namedPipeServer.TextReader;
			this.originalStdOut = new OutOfProcessTextWriter(namedPipeServer.TextWriter);
			this.originalStdErr = new NamedPipeErrorTextWriter(namedPipeServer.TextWriter);
			WindowsIdentity windowsIdentity = null;
			try
			{
				windowsIdentity = WindowsIdentity.GetCurrent();
			}
			catch (SecurityException)
			{
			}
			this._windowsIdentityToImpersonate = ((windowsIdentity != null && windowsIdentity.ImpersonationLevel == TokenImpersonationLevel.Impersonation) ? windowsIdentity : null);
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x000C9D70 File Offset: 0x000C7F70
		internal static void Run(string initialCommand, RemoteSessionNamedPipeServer namedPipeServer)
		{
			lock (OutOfProcessMediatorBase.SyncObject)
			{
				if (NamedPipeProcessMediator.SingletonInstance != null && !NamedPipeProcessMediator.SingletonInstance.IsDisposed)
				{
					return;
				}
				NamedPipeProcessMediator.SingletonInstance = new NamedPipeProcessMediator(namedPipeServer);
			}
			AppDomain.CurrentDomain.UnhandledException += OutOfProcessMediatorBase.AppDomainUnhandledException;
			NamedPipeProcessMediator.SingletonInstance.Start(initialCommand);
		}

		// Token: 0x040011B1 RID: 4529
		private static NamedPipeProcessMediator SingletonInstance;

		// Token: 0x040011B2 RID: 4530
		private readonly RemoteSessionNamedPipeServer _namedPipeServer;
	}
}
