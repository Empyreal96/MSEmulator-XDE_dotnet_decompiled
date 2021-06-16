using System;
using System.Management.Automation.Remoting.Server;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002FC RID: 764
	internal class ServerMethodExecutor
	{
		// Token: 0x06002401 RID: 9217 RVA: 0x000C9FA4 File Offset: 0x000C81A4
		internal ServerMethodExecutor(Guid clientRunspacePoolId, Guid clientPowerShellId, AbstractServerTransportManager transportManager)
		{
			this._clientRunspacePoolId = clientRunspacePoolId;
			this._clientPowerShellId = clientPowerShellId;
			this._transportManager = transportManager;
			this._remoteHostCallDataType = ((clientPowerShellId == Guid.Empty) ? RemotingDataType.RemoteHostCallUsingRunspaceHost : RemotingDataType.RemoteHostCallUsingPowerShellHost);
			this._serverDispatchTable = new ServerDispatchTable();
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000C9FF6 File Offset: 0x000C81F6
		internal void HandleRemoteHostResponseFromClient(RemoteHostResponse remoteHostResponse)
		{
			this._serverDispatchTable.SetResponse(remoteHostResponse.CallId, remoteHostResponse);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000CA00A File Offset: 0x000C820A
		internal void AbortAllCalls()
		{
			this._serverDispatchTable.AbortAllCalls();
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000CA017 File Offset: 0x000C8217
		internal void ExecuteVoidMethod(RemoteHostMethodId methodId)
		{
			this.ExecuteVoidMethod(methodId, new object[0]);
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000CA028 File Offset: 0x000C8228
		internal void ExecuteVoidMethod(RemoteHostMethodId methodId, object[] parameters)
		{
			long callId = -100L;
			RemoteHostCall remoteHostCall = new RemoteHostCall(callId, methodId, parameters);
			RemoteDataObject<PSObject> data = RemoteDataObject<PSObject>.CreateFrom(RemotingDestination.Client, this._remoteHostCallDataType, this._clientRunspacePoolId, this._clientPowerShellId, remoteHostCall.Encode());
			this._transportManager.SendDataToClient<PSObject>(data, false, false);
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000CA06F File Offset: 0x000C826F
		internal T ExecuteMethod<T>(RemoteHostMethodId methodId)
		{
			return this.ExecuteMethod<T>(methodId, new object[0]);
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000CA080 File Offset: 0x000C8280
		internal T ExecuteMethod<T>(RemoteHostMethodId methodId, object[] parameters)
		{
			long callId = this._serverDispatchTable.CreateNewCallId();
			RemoteHostCall remoteHostCall = new RemoteHostCall(callId, methodId, parameters);
			RemoteDataObject<PSObject> data = RemoteDataObject<PSObject>.CreateFrom(RemotingDestination.Client, this._remoteHostCallDataType, this._clientRunspacePoolId, this._clientPowerShellId, remoteHostCall.Encode());
			this._transportManager.SendDataToClient<PSObject>(data, false, true);
			RemoteHostResponse response = this._serverDispatchTable.GetResponse(callId, null);
			if (response == null)
			{
				throw RemoteHostExceptions.NewRemoteHostCallFailedException(methodId);
			}
			response.SimulateExecution();
			return (T)((object)response.SimulateExecution());
		}

		// Token: 0x040011BA RID: 4538
		private const long DefaultClientPipelineId = -1L;

		// Token: 0x040011BB RID: 4539
		private Guid _clientRunspacePoolId;

		// Token: 0x040011BC RID: 4540
		private Guid _clientPowerShellId;

		// Token: 0x040011BD RID: 4541
		private ServerDispatchTable _serverDispatchTable;

		// Token: 0x040011BE RID: 4542
		private RemotingDataType _remoteHostCallDataType;

		// Token: 0x040011BF RID: 4543
		private AbstractServerTransportManager _transportManager;
	}
}
