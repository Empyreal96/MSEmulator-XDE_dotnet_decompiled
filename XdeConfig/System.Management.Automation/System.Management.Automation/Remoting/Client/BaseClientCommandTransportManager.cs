using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Text;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000365 RID: 869
	internal abstract class BaseClientCommandTransportManager : BaseClientTransportManager, IDisposable
	{
		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06002AFE RID: 11006 RVA: 0x000EC42D File Offset: 0x000EA62D
		protected Guid PowershellInstanceId
		{
			get
			{
				return this.powershellInstanceId;
			}
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000EC438 File Offset: 0x000EA638
		protected BaseClientCommandTransportManager(ClientRemotePowerShell shell, PSRemotingCryptoHelper cryptoHelper, BaseClientSessionTransportManager sessnTM) : base(sessnTM.RunspacePoolInstanceId, cryptoHelper)
		{
			base.Fragmentor.FragmentSize = sessnTM.Fragmentor.FragmentSize;
			base.Fragmentor.TypeTable = sessnTM.Fragmentor.TypeTable;
			this.dataToBeSent.Fragmentor = base.Fragmentor;
			this.powershellInstanceId = shell.PowerShell.InstanceId;
			this.cmdText = new StringBuilder();
			foreach (Command command in shell.PowerShell.Commands.Commands)
			{
				this.cmdText.Append(command.CommandText);
				this.cmdText.Append(" | ");
			}
			this.cmdText.Remove(this.cmdText.Length - 3, 3);
			RemoteDataObject obj;
			if (shell.PowerShell.IsGetCommandMetadataSpecialPipeline)
			{
				obj = RemotingEncoder.GenerateGetCommandMetadata(shell);
			}
			else
			{
				obj = RemotingEncoder.GenerateCreatePowerShell(shell);
			}
			this.serializedPipeline = new SerializedDataStream(base.Fragmentor.FragmentSize);
			base.Fragmentor.Fragment<object>(obj, this.serializedPipeline);
		}

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06002B00 RID: 11008 RVA: 0x000EC570 File Offset: 0x000EA770
		// (remove) Token: 0x06002B01 RID: 11009 RVA: 0x000EC5A8 File Offset: 0x000EA7A8
		internal event EventHandler<EventArgs> SignalCompleted;

		// Token: 0x06002B02 RID: 11010 RVA: 0x000EC5DD File Offset: 0x000EA7DD
		internal void RaiseSignalCompleted()
		{
			this.SignalCompleted.SafeInvoke(this, EventArgs.Empty);
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000EC5F0 File Offset: 0x000EA7F0
		internal override void Dispose(bool isDisposing)
		{
			base.Dispose(isDisposing);
			if (isDisposing)
			{
				this.serializedPipeline.Dispose();
			}
		}

		// Token: 0x06002B04 RID: 11012 RVA: 0x000EC607 File Offset: 0x000EA807
		internal virtual void ReconnectAsync()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002B05 RID: 11013 RVA: 0x000EC60E File Offset: 0x000EA80E
		internal virtual void SendStopSignal()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04001559 RID: 5465
		protected StringBuilder cmdText;

		// Token: 0x0400155A RID: 5466
		protected SerializedDataStream serializedPipeline;

		// Token: 0x0400155B RID: 5467
		protected Guid powershellInstanceId;

		// Token: 0x0400155C RID: 5468
		internal bool startInDisconnectedMode;
	}
}
