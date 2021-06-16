using System;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces.Internal;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000295 RID: 661
	internal abstract class ClientRemoteSessionDataStructureHandler : BaseSessionDataStructureHandler
	{
		// Token: 0x06001FA1 RID: 8097
		internal abstract void CreateAsync();

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06001FA2 RID: 8098
		// (remove) Token: 0x06001FA3 RID: 8099
		internal abstract event EventHandler<RemoteSessionStateEventArgs> ConnectionStateChanged;

		// Token: 0x06001FA4 RID: 8100
		internal abstract void SendNegotiationAsync(RemoteSessionState sessionState);

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06001FA5 RID: 8101
		// (remove) Token: 0x06001FA6 RID: 8102
		internal abstract event EventHandler<RemoteSessionNegotiationEventArgs> NegotiationReceived;

		// Token: 0x06001FA7 RID: 8103
		internal abstract void CloseConnectionAsync();

		// Token: 0x06001FA8 RID: 8104
		internal abstract void DisconnectAsync();

		// Token: 0x06001FA9 RID: 8105
		internal abstract void ReconnectAsync();

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06001FAA RID: 8106
		internal abstract ClientRemoteSessionDSHandlerStateMachine StateMachine { get; }

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06001FAB RID: 8107
		internal abstract BaseClientSessionTransportManager TransportManager { get; }

		// Token: 0x06001FAC RID: 8108
		internal abstract BaseClientCommandTransportManager CreateClientCommandTransportManager(ClientRemotePowerShell cmd, bool noInput);

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06001FAD RID: 8109
		// (remove) Token: 0x06001FAE RID: 8110
		internal abstract event EventHandler<RemoteDataEventArgs<string>> EncryptedSessionKeyReceived;

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06001FAF RID: 8111
		// (remove) Token: 0x06001FB0 RID: 8112
		internal abstract event EventHandler<RemoteDataEventArgs<string>> PublicKeyRequestReceived;

		// Token: 0x06001FB1 RID: 8113
		internal abstract void SendPublicKeyAsync(string localPublicKey);
	}
}
