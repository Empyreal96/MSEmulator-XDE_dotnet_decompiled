using System;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces.Internal;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002A9 RID: 681
	internal class ClientMethodExecutor
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06002102 RID: 8450 RVA: 0x000BEA67 File Offset: 0x000BCC67
		internal RemoteHostCall RemoteHostCall
		{
			get
			{
				return this._remoteHostCall;
			}
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000BEA6F File Offset: 0x000BCC6F
		private ClientMethodExecutor(BaseClientTransportManager transportManager, PSHost clientHost, Guid clientRunspacePoolId, Guid clientPowerShellId, RemoteHostCall remoteHostCall)
		{
			this._transportManager = transportManager;
			this._remoteHostCall = remoteHostCall;
			this._clientHost = clientHost;
			this._clientRunspacePoolId = clientRunspacePoolId;
			this._clientPowerShellId = clientPowerShellId;
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000BEA9C File Offset: 0x000BCC9C
		internal static void Dispatch(BaseClientTransportManager transportManager, PSHost clientHost, PSDataCollectionStream<ErrorRecord> errorStream, ObjectStream methodExecutorStream, bool isMethodExecutorStreamEnabled, RemoteRunspacePoolInternal runspacePool, Guid clientPowerShellId, RemoteHostCall remoteHostCall)
		{
			ClientMethodExecutor clientMethodExecutor = new ClientMethodExecutor(transportManager, clientHost, runspacePool.InstanceId, clientPowerShellId, remoteHostCall);
			if (clientPowerShellId == Guid.Empty)
			{
				clientMethodExecutor.Execute(errorStream);
				return;
			}
			bool flag = false;
			if (clientHost != null)
			{
				PSObject privateData = clientHost.PrivateData;
				if (privateData != null)
				{
					PSNoteProperty psnoteProperty = privateData.Properties["AllowSetShouldExitFromRemote"] as PSNoteProperty;
					flag = (psnoteProperty != null && psnoteProperty.Value is bool && (bool)psnoteProperty.Value);
				}
			}
			if (remoteHostCall.IsSetShouldExit && isMethodExecutorStreamEnabled && !flag)
			{
				runspacePool.Close();
				return;
			}
			if (isMethodExecutorStreamEnabled)
			{
				methodExecutorStream.Write(clientMethodExecutor);
				return;
			}
			clientMethodExecutor.Execute(errorStream);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000BEB44 File Offset: 0x000BCD44
		private bool IsRunspacePushed(PSHost host)
		{
			IHostSupportsInteractiveSession hostSupportsInteractiveSession = host as IHostSupportsInteractiveSession;
			if (hostSupportsInteractiveSession == null)
			{
				return false;
			}
			try
			{
				return hostSupportsInteractiveSession.IsRunspacePushed;
			}
			catch (PSNotImplementedException)
			{
			}
			return false;
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000BEBE0 File Offset: 0x000BCDE0
		internal void Execute(PSDataCollectionStream<ErrorRecord> errorStream)
		{
			Action<ErrorRecord> writeErrorAction;
			if (errorStream == null || this.IsRunspacePushed(this._clientHost))
			{
				writeErrorAction = delegate(ErrorRecord errorRecord)
				{
					try
					{
						if (this._clientHost.UI != null)
						{
							this._clientHost.UI.WriteErrorLine(errorRecord.ToString());
						}
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
				};
			}
			else
			{
				writeErrorAction = delegate(ErrorRecord errorRecord)
				{
					errorStream.Write(errorRecord);
				};
			}
			this.Execute(writeErrorAction);
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000BEC4A File Offset: 0x000BCE4A
		internal void Execute(Cmdlet cmdlet)
		{
			this.Execute(new Action<ErrorRecord>(cmdlet.WriteError));
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x000BEC60 File Offset: 0x000BCE60
		internal void Execute(Action<ErrorRecord> writeErrorAction)
		{
			if (this._remoteHostCall.IsVoidMethod)
			{
				this.ExecuteVoid(writeErrorAction);
				return;
			}
			RemotingDataType dataType = (this._clientPowerShellId == Guid.Empty) ? RemotingDataType.RemoteRunspaceHostResponseData : RemotingDataType.RemotePowerShellHostResponseData;
			RemoteHostResponse remoteHostResponse = this._remoteHostCall.ExecuteNonVoidMethod(this._clientHost);
			RemoteDataObject<PSObject> data = RemoteDataObject<PSObject>.CreateFrom(RemotingDestination.Server, dataType, this._clientRunspacePoolId, this._clientPowerShellId, remoteHostResponse.Encode());
			this._transportManager.DataToBeSentCollection.Add<PSObject>(data, DataPriorityType.PromptResponse);
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x000BECE0 File Offset: 0x000BCEE0
		internal void ExecuteVoid(Action<ErrorRecord> writeErrorAction)
		{
			try
			{
				this._remoteHostCall.ExecuteVoidMethod(this._clientHost);
			}
			catch (Exception innerException)
			{
				CommandProcessorBase.CheckForSevereException(innerException);
				if (innerException.InnerException != null)
				{
					innerException = innerException.InnerException;
				}
				ErrorRecord obj = new ErrorRecord(innerException, PSRemotingErrorId.RemoteHostCallFailed.ToString(), ErrorCategory.InvalidArgument, this._remoteHostCall.MethodName);
				writeErrorAction(obj);
			}
		}

		// Token: 0x04000E9C RID: 3740
		private BaseClientTransportManager _transportManager;

		// Token: 0x04000E9D RID: 3741
		private PSHost _clientHost;

		// Token: 0x04000E9E RID: 3742
		private Guid _clientRunspacePoolId;

		// Token: 0x04000E9F RID: 3743
		private Guid _clientPowerShellId;

		// Token: 0x04000EA0 RID: 3744
		private RemoteHostCall _remoteHostCall;
	}
}
