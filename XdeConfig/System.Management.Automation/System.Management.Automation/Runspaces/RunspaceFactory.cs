using System;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Tracing;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020001FA RID: 506
	public static class RunspaceFactory
	{
		// Token: 0x0600175E RID: 5982 RVA: 0x00092024 File Offset: 0x00090224
		static RunspaceFactory()
		{
			Guid activityId = EtwActivity.GetActivityId();
			if (activityId == Guid.Empty)
			{
				EtwActivity.SetActivityId(EtwActivity.CreateActivityId());
			}
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x00092050 File Offset: 0x00090250
		public static Runspace CreateRunspace()
		{
			PSHost host = new DefaultHost(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture);
			return RunspaceFactory.CreateRunspace(host);
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x00092073 File Offset: 0x00090273
		public static Runspace CreateRunspace(PSHost host)
		{
			if (host == null)
			{
				throw PSTraceSource.NewArgumentNullException("host");
			}
			return RunspaceFactory.CreateRunspace(host, RunspaceConfiguration.Create());
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00092090 File Offset: 0x00090290
		public static Runspace CreateRunspace(RunspaceConfiguration runspaceConfiguration)
		{
			if (runspaceConfiguration == null)
			{
				throw PSTraceSource.NewArgumentNullException("runspaceConfiguration");
			}
			PSHost host = new DefaultHost(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture);
			return RunspaceFactory.CreateRunspace(host, runspaceConfiguration);
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x000920C2 File Offset: 0x000902C2
		public static Runspace CreateRunspace(PSHost host, RunspaceConfiguration runspaceConfiguration)
		{
			if (host == null)
			{
				throw PSTraceSource.NewArgumentNullException("host");
			}
			if (runspaceConfiguration == null)
			{
				throw PSTraceSource.NewArgumentNullException("runspaceConfiguration");
			}
			return new LocalRunspace(host, runspaceConfiguration);
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x000920E8 File Offset: 0x000902E8
		public static Runspace CreateRunspace(InitialSessionState initialSessionState)
		{
			if (initialSessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("initialSessionState");
			}
			PSHost host = new DefaultHost(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture);
			return RunspaceFactory.CreateRunspace(host, initialSessionState);
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x0009211A File Offset: 0x0009031A
		public static Runspace CreateRunspace(PSHost host, InitialSessionState initialSessionState)
		{
			if (host == null)
			{
				throw PSTraceSource.NewArgumentNullException("host");
			}
			if (initialSessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("initialSessionState");
			}
			return new LocalRunspace(host, initialSessionState);
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x0009213F File Offset: 0x0009033F
		internal static Runspace CreateRunspaceFromSessionStateNoClone(PSHost host, InitialSessionState initialSessionState)
		{
			if (host == null)
			{
				throw PSTraceSource.NewArgumentNullException("host");
			}
			if (initialSessionState == null)
			{
				throw PSTraceSource.NewArgumentNullException("initialSessionState");
			}
			return new LocalRunspace(host, initialSessionState, true);
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00092165 File Offset: 0x00090365
		public static RunspacePool CreateRunspacePool()
		{
			return RunspaceFactory.CreateRunspacePool(1, 1, RunspaceConfiguration.Create(), new DefaultHost(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture));
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x00092182 File Offset: 0x00090382
		public static RunspacePool CreateRunspacePool(int minRunspaces, int maxRunspaces)
		{
			return RunspaceFactory.CreateRunspacePool(minRunspaces, maxRunspaces, RunspaceConfiguration.Create(), new DefaultHost(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture));
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x0009219F File Offset: 0x0009039F
		public static RunspacePool CreateRunspacePool(InitialSessionState initialSessionState)
		{
			return RunspaceFactory.CreateRunspacePool(1, 1, initialSessionState, new DefaultHost(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture));
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x000921B8 File Offset: 0x000903B8
		public static RunspacePool CreateRunspacePool(int minRunspaces, int maxRunspaces, PSHost host)
		{
			return RunspaceFactory.CreateRunspacePool(minRunspaces, maxRunspaces, RunspaceConfiguration.Create(), host);
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x000921C7 File Offset: 0x000903C7
		private static RunspacePool CreateRunspacePool(int minRunspaces, int maxRunspaces, RunspaceConfiguration runspaceConfiguration, PSHost host)
		{
			return new RunspacePool(minRunspaces, maxRunspaces, runspaceConfiguration, host);
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x000921D2 File Offset: 0x000903D2
		public static RunspacePool CreateRunspacePool(int minRunspaces, int maxRunspaces, InitialSessionState initialSessionState, PSHost host)
		{
			return new RunspacePool(minRunspaces, maxRunspaces, initialSessionState, host);
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x000921DD File Offset: 0x000903DD
		public static RunspacePool CreateRunspacePool(int minRunspaces, int maxRunspaces, RunspaceConnectionInfo connectionInfo)
		{
			return RunspaceFactory.CreateRunspacePool(minRunspaces, maxRunspaces, connectionInfo, null);
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x000921E8 File Offset: 0x000903E8
		public static RunspacePool CreateRunspacePool(int minRunspaces, int maxRunspaces, RunspaceConnectionInfo connectionInfo, PSHost host)
		{
			return RunspaceFactory.CreateRunspacePool(minRunspaces, maxRunspaces, connectionInfo, host, null);
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x000921F4 File Offset: 0x000903F4
		public static RunspacePool CreateRunspacePool(int minRunspaces, int maxRunspaces, RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable)
		{
			return RunspaceFactory.CreateRunspacePool(minRunspaces, maxRunspaces, connectionInfo, host, typeTable, null);
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00092204 File Offset: 0x00090404
		public static RunspacePool CreateRunspacePool(int minRunspaces, int maxRunspaces, RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable, PSPrimitiveDictionary applicationArguments)
		{
			if (!(connectionInfo is WSManConnectionInfo) && !(connectionInfo is NewProcessConnectionInfo) && !(connectionInfo is NamedPipeConnectionInfo) && !(connectionInfo is VMConnectionInfo) && !(connectionInfo is ContainerConnectionInfo))
			{
				throw new NotSupportedException();
			}
			if (connectionInfo is WSManConnectionInfo)
			{
				RemotingCommandUtil.CheckHostRemotingPrerequisites();
			}
			return new RunspacePool(minRunspaces, maxRunspaces, typeTable, host, applicationArguments, connectionInfo, null);
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x0009225A File Offset: 0x0009045A
		public static Runspace CreateRunspace(RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable)
		{
			return RunspaceFactory.CreateRunspace(connectionInfo, host, typeTable, null, null);
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00092266 File Offset: 0x00090466
		public static Runspace CreateRunspace(RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable, PSPrimitiveDictionary applicationArguments)
		{
			return RunspaceFactory.CreateRunspace(connectionInfo, host, typeTable, applicationArguments, null);
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00092274 File Offset: 0x00090474
		public static Runspace CreateRunspace(RunspaceConnectionInfo connectionInfo, PSHost host, TypeTable typeTable, PSPrimitiveDictionary applicationArguments, string name)
		{
			if (!(connectionInfo is WSManConnectionInfo) && !(connectionInfo is NewProcessConnectionInfo) && !(connectionInfo is NamedPipeConnectionInfo) && !(connectionInfo is VMConnectionInfo) && !(connectionInfo is ContainerConnectionInfo))
			{
				throw new NotSupportedException();
			}
			if (connectionInfo is WSManConnectionInfo)
			{
				RemotingCommandUtil.CheckHostRemotingPrerequisites();
			}
			return new RemoteRunspace(typeTable, connectionInfo, host, applicationArguments, name, -1);
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x000922C8 File Offset: 0x000904C8
		public static Runspace CreateRunspace(PSHost host, RunspaceConnectionInfo connectionInfo)
		{
			return RunspaceFactory.CreateRunspace(connectionInfo, host, null);
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x000922D2 File Offset: 0x000904D2
		public static Runspace CreateRunspace(RunspaceConnectionInfo connectionInfo)
		{
			return RunspaceFactory.CreateRunspace(null, connectionInfo);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x000922DC File Offset: 0x000904DC
		public static Runspace CreateOutOfProcessRunspace(TypeTable typeTable)
		{
			NewProcessConnectionInfo connectionInfo = new NewProcessConnectionInfo(null);
			return RunspaceFactory.CreateRunspace(connectionInfo, null, typeTable);
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x000922F8 File Offset: 0x000904F8
		public static Runspace CreateOutOfProcessRunspace(TypeTable typeTable, PowerShellProcessInstance processInstance)
		{
			NewProcessConnectionInfo connectionInfo = new NewProcessConnectionInfo(null)
			{
				Process = processInstance
			};
			return RunspaceFactory.CreateRunspace(connectionInfo, null, typeTable);
		}
	}
}
