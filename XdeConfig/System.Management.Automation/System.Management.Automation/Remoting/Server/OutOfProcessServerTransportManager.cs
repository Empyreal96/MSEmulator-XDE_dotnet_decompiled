using System;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x02000387 RID: 903
	internal class OutOfProcessServerTransportManager : AbstractServerTransportManager
	{
		// Token: 0x06002BE0 RID: 11232 RVA: 0x000F2B08 File Offset: 0x000F0D08
		internal OutOfProcessServerTransportManager(OutOfProcessTextWriter stdOutWriter, OutOfProcessTextWriter stdErrWriter, Guid powershellInstanceId, TypeTable typeTableToUse, int fragmentSize, PSRemotingCryptoHelper cryptoHelper) : base(fragmentSize, cryptoHelper)
		{
			this.stdOutWriter = stdOutWriter;
			this.stdErrWriter = stdErrWriter;
			this.powershellInstanceId = powershellInstanceId;
			base.TypeTable = typeTableToUse;
			base.WSManTransportErrorOccured += this.HandleWSManTransportError;
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000F2B43 File Offset: 0x000F0D43
		private void HandleWSManTransportError(object sender, TransportErrorOccuredEventArgs e)
		{
			this.stdErrWriter.WriteLine(StringUtil.Format(RemotingErrorIdStrings.RemoteTransportError, e.Exception.TransportMessage));
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000F2B65 File Offset: 0x000F0D65
		internal override void ProcessRawData(byte[] data, string stream)
		{
			this.isDataAckSendPending = true;
			base.ProcessRawData(data, stream);
			if (this.isDataAckSendPending)
			{
				this.isDataAckSendPending = false;
				this.stdOutWriter.WriteLine(OutOfProcessUtils.CreateDataAckPacket(this.powershellInstanceId));
			}
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000F2B9B File Offset: 0x000F0D9B
		internal override void ReportExecutionStatusAsRunning()
		{
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000F2B9D File Offset: 0x000F0D9D
		protected override void SendDataToClient(byte[] data, bool flush, bool reportAsPending, bool reportAsDataBoundary)
		{
			this.stdOutWriter.WriteLine(OutOfProcessUtils.CreateDataPacket(data, DataPriorityType.Default, this.powershellInstanceId));
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000F2BB7 File Offset: 0x000F0DB7
		internal override void Prepare()
		{
			if (this.isDataAckSendPending)
			{
				this.isDataAckSendPending = false;
				base.Prepare();
				this.stdOutWriter.WriteLine(OutOfProcessUtils.CreateDataAckPacket(this.powershellInstanceId));
			}
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000F2BE4 File Offset: 0x000F0DE4
		internal override void Close(Exception reasonForClose)
		{
			base.RaiseClosingEvent();
		}

		// Token: 0x04001604 RID: 5636
		private OutOfProcessTextWriter stdOutWriter;

		// Token: 0x04001605 RID: 5637
		private OutOfProcessTextWriter stdErrWriter;

		// Token: 0x04001606 RID: 5638
		private Guid powershellInstanceId;

		// Token: 0x04001607 RID: 5639
		private bool isDataAckSendPending;
	}
}
