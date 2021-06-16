using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Remoting.Server;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003EF RID: 1007
	internal class WSManPluginShellSession : WSManPluginServerSession
	{
		// Token: 0x06002D74 RID: 11636 RVA: 0x000FBB14 File Offset: 0x000F9D14
		internal WSManPluginShellSession(WSManNativeApi.WSManPluginRequest creationRequestDetails, WSManPluginServerTransportManager trnsprtMgr, ServerRemoteSession remoteSession, WSManPluginOperationShutdownContext shutDownContext) : base(creationRequestDetails, trnsprtMgr)
		{
			this.remoteSession = remoteSession;
			ServerRemoteSession serverRemoteSession = this.remoteSession;
			serverRemoteSession.Closed = (EventHandler<RemoteSessionStateMachineEventArgs>)Delegate.Combine(serverRemoteSession.Closed, new EventHandler<RemoteSessionStateMachineEventArgs>(this.HandleServerRemoteSessionClosed));
			this.activeCommandSessions = new Dictionary<IntPtr, WSManPluginCommandSession>();
			this.shellSyncObject = new object();
			this.shutDownContext = shutDownContext;
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x000FBB78 File Offset: 0x000F9D78
		internal override void ExecuteConnect(WSManNativeApi.WSManPluginRequest requestDetails, int flags, WSManNativeApi.WSManData_UnToMan inboundConnectInformation)
		{
			if (inboundConnectInformation == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullInvalidInput, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullInvalidInput, "inboundConnectInformation", "WSManPluginShellConnect"));
				return;
			}
			IntPtr zero = IntPtr.Zero;
			try
			{
				byte[] connectData = ServerOperationHelpers.ExtractEncodedXmlElement(inboundConnectInformation.Text, "connectXml");
				try
				{
					byte[] inArray;
					this.remoteSession.ExecuteConnect(connectData, out inArray);
					string errorMessage = string.Format(CultureInfo.InvariantCulture, "<{0} xmlns=\"{1}\">{2}</{0}>", new object[]
					{
						"connectResponseXml",
						"http://schemas.microsoft.com/powershell",
						Convert.ToBase64String(inArray)
					});
					WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NoError, errorMessage);
				}
				catch (PSRemotingDataStructureException ex)
				{
					WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.PluginConnectOperationFailed, ex.Message);
				}
			}
			catch (OutOfMemoryException)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.OutOfMemory);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(zero);
				}
			}
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x000FBC74 File Offset: 0x000F9E74
		internal void CreateCommand(IntPtr pluginContext, WSManNativeApi.WSManPluginRequest requestDetails, int flags, string commandLine, WSManNativeApi.WSManCommandArgSet arguments)
		{
			try
			{
				WSManPluginCommandTransportManager wsmanPluginCommandTransportManager = new WSManPluginCommandTransportManager(this.transportMgr);
				wsmanPluginCommandTransportManager.Initialize();
				this.remoteSession.ApplyQuotaOnCommandTransportManager(wsmanPluginCommandTransportManager);
				WSManPluginCommandSession wsmanPluginCommandSession = new WSManPluginCommandSession(requestDetails, wsmanPluginCommandTransportManager, this.remoteSession);
				this.AddToActiveCmdSessions(wsmanPluginCommandSession);
				wsmanPluginCommandSession.SessionClosed += this.HandleCommandSessionClosed;
				wsmanPluginCommandSession.shutDownContext = new WSManPluginOperationShutdownContext(pluginContext, this.creationRequestDetails.unmanagedHandle, wsmanPluginCommandSession.creationRequestDetails.unmanagedHandle, false);
				if (!wsmanPluginCommandSession.ProcessArguments(arguments))
				{
					WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidArgSet, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidArgSet, "WSManPluginCommand"));
				}
				else
				{
					wsmanPluginCommandSession.ReportContext();
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.ManagedException, StringUtil.Format(RemotingErrorIdStrings.WSManPluginManagedException, ex.Message));
			}
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x000FBD4C File Offset: 0x000F9F4C
		internal void CloseCommandOperation(WSManPluginOperationShutdownContext context)
		{
			WSManPluginCommandSession commandSession = this.GetCommandSession(context.commandContext);
			if (commandSession == null)
			{
				return;
			}
			if (!context.isReceiveOperation)
			{
				this.DeleteFromActiveCmdSessions(commandSession.creationRequestDetails.unmanagedHandle);
			}
			commandSession.CloseOperation(context, null);
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x000FBD8C File Offset: 0x000F9F8C
		private void AddToActiveCmdSessions(WSManPluginCommandSession newCmdSession)
		{
			lock (this.shellSyncObject)
			{
				if (!this.isClosed)
				{
					IntPtr unmanagedHandle = newCmdSession.creationRequestDetails.unmanagedHandle;
					if (!this.activeCommandSessions.ContainsKey(unmanagedHandle))
					{
						this.activeCommandSessions.Add(unmanagedHandle, newCmdSession);
					}
				}
			}
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x000FBDFC File Offset: 0x000F9FFC
		private void DeleteFromActiveCmdSessions(IntPtr keyToDelete)
		{
			lock (this.shellSyncObject)
			{
				if (!this.isClosed)
				{
					if (this.activeCommandSessions.ContainsKey(keyToDelete))
					{
						this.activeCommandSessions.Remove(keyToDelete);
					}
				}
			}
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x000FBE5C File Offset: 0x000FA05C
		private void CloseAndClearCommandSessions(Exception reasonForClose)
		{
			Collection<WSManPluginCommandSession> collection = new Collection<WSManPluginCommandSession>();
			lock (this.shellSyncObject)
			{
				Dictionary<IntPtr, WSManPluginCommandSession>.Enumerator enumerator = this.activeCommandSessions.GetEnumerator();
				while (enumerator.MoveNext())
				{
					Collection<WSManPluginCommandSession> collection2 = collection;
					KeyValuePair<IntPtr, WSManPluginCommandSession> keyValuePair = enumerator.Current;
					collection2.Add(keyValuePair.Value);
				}
				this.activeCommandSessions.Clear();
			}
			foreach (WSManPluginCommandSession wsmanPluginCommandSession in collection)
			{
				wsmanPluginCommandSession.SessionClosed -= this.HandleCommandSessionClosed;
				wsmanPluginCommandSession.Close(reasonForClose);
			}
			collection.Clear();
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x000FBF10 File Offset: 0x000FA110
		internal WSManPluginCommandSession GetCommandSession(IntPtr cmdContext)
		{
			WSManPluginCommandSession result;
			lock (this.shellSyncObject)
			{
				WSManPluginCommandSession wsmanPluginCommandSession = null;
				this.activeCommandSessions.TryGetValue(cmdContext, out wsmanPluginCommandSession);
				result = wsmanPluginCommandSession;
			}
			return result;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x000FBF60 File Offset: 0x000FA160
		private void HandleServerRemoteSessionClosed(object sender, RemoteSessionStateMachineEventArgs eventArgs)
		{
			Exception reasonForClose = null;
			if (eventArgs != null)
			{
				reasonForClose = eventArgs.Reason;
			}
			base.Close(reasonForClose);
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x000FBF80 File Offset: 0x000FA180
		private void HandleCommandSessionClosed(object source, EventArgs e)
		{
			this.DeleteFromActiveCmdSessions((IntPtr)source);
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x000FBF90 File Offset: 0x000FA190
		internal override void CloseOperation(WSManPluginOperationShutdownContext context, Exception reasonForClose)
		{
			lock (this.shellSyncObject)
			{
				if (this.isClosed)
				{
					return;
				}
				if (!context.isReceiveOperation)
				{
					this.isClosed = true;
				}
			}
			bool isShuttingDown = context.isShuttingDown && context.isReceiveOperation;
			bool isReceiveOperation = context.isReceiveOperation;
			bool isShuttingDown2 = context.isShuttingDown;
			base.ReportSendOperationComplete();
			this.transportMgr.DoClose(isShuttingDown, reasonForClose);
			if (!isReceiveOperation)
			{
				this.CloseAndClearCommandSessions(reasonForClose);
				base.SafeInvokeSessionClosed(this.creationRequestDetails.unmanagedHandle, EventArgs.Empty);
				WSManPluginInstance.ReportWSManOperationComplete(this.creationRequestDetails, reasonForClose);
				base.Close(isShuttingDown2);
			}
		}

		// Token: 0x040017DF RID: 6111
		private Dictionary<IntPtr, WSManPluginCommandSession> activeCommandSessions;

		// Token: 0x040017E0 RID: 6112
		private ServerRemoteSession remoteSession;

		// Token: 0x040017E1 RID: 6113
		internal object shellSyncObject;
	}
}
