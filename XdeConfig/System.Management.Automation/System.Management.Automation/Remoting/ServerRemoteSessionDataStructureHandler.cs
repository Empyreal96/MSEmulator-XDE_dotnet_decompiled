using System;
using System.Management.Automation.Remoting.Server;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000300 RID: 768
	internal abstract class ServerRemoteSessionDataStructureHandler : BaseSessionDataStructureHandler
	{
		// Token: 0x06002442 RID: 9282 RVA: 0x000CC42F File Offset: 0x000CA62F
		internal ServerRemoteSessionDataStructureHandler()
		{
		}

		// Token: 0x06002443 RID: 9283
		internal abstract void ConnectAsync();

		// Token: 0x06002444 RID: 9284
		internal abstract void SendNegotiationAsync();

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06002445 RID: 9285
		// (remove) Token: 0x06002446 RID: 9286
		internal abstract event EventHandler<RemoteSessionNegotiationEventArgs> NegotiationReceived;

		// Token: 0x06002447 RID: 9287
		internal abstract void CloseConnectionAsync(Exception reasonForClose);

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06002448 RID: 9288
		// (remove) Token: 0x06002449 RID: 9289
		internal abstract event EventHandler<EventArgs> SessionClosing;

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x0600244A RID: 9290
		// (remove) Token: 0x0600244B RID: 9291
		internal abstract event EventHandler<RemoteDataEventArgs> CreateRunspacePoolReceived;

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x0600244C RID: 9292
		internal abstract ServerRemoteSessionDSHandlerStateMachine StateMachine { get; }

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x0600244D RID: 9293
		internal abstract AbstractServerSessionTransportManager TransportManager { get; }

		// Token: 0x0600244E RID: 9294
		internal abstract void RaiseDataReceivedEvent(RemoteDataEventArgs arg);

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x0600244F RID: 9295
		// (remove) Token: 0x06002450 RID: 9296
		internal abstract event EventHandler<RemoteDataEventArgs<string>> PublicKeyReceived;

		// Token: 0x06002451 RID: 9297
		internal abstract void SendRequestForPublicKey();

		// Token: 0x06002452 RID: 9298
		internal abstract void SendEncryptedSessionKey(string encryptedSessionKey);
	}
}
