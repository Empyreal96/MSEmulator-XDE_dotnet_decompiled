using System;
using System.Collections.Generic;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Remoting.Server
{
	// Token: 0x02000386 RID: 902
	internal class OutOfProcessServerSessionTransportManager : AbstractServerSessionTransportManager
	{
		// Token: 0x06002BD7 RID: 11223 RVA: 0x000F2953 File Offset: 0x000F0B53
		internal OutOfProcessServerSessionTransportManager(OutOfProcessTextWriter outWriter, OutOfProcessTextWriter errWriter) : base(32768, new PSRemotingCryptoHelperServer())
		{
			this.stdOutWriter = outWriter;
			this.stdErrWriter = errWriter;
			this.cmdTransportManagers = new Dictionary<Guid, OutOfProcessServerTransportManager>();
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000F2989 File Offset: 0x000F0B89
		internal override void ProcessRawData(byte[] data, string stream)
		{
			base.ProcessRawData(data, stream);
			this.stdOutWriter.WriteLine(OutOfProcessUtils.CreateDataAckPacket(Guid.Empty));
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000F29A8 File Offset: 0x000F0BA8
		internal override void Prepare()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000F29AF File Offset: 0x000F0BAF
		protected override void SendDataToClient(byte[] data, bool flush, bool reportAsPending, bool reportAsDataBoundary)
		{
			this.stdOutWriter.WriteLine(OutOfProcessUtils.CreateDataPacket(data, DataPriorityType.Default, Guid.Empty));
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000F29C8 File Offset: 0x000F0BC8
		internal override void ReportExecutionStatusAsRunning()
		{
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x000F29CC File Offset: 0x000F0BCC
		internal void CreateCommandTransportManager(Guid powerShellCmdId)
		{
			OutOfProcessServerTransportManager outOfProcessServerTransportManager = new OutOfProcessServerTransportManager(this.stdOutWriter, this.stdErrWriter, powerShellCmdId, base.TypeTable, base.Fragmentor.FragmentSize, base.CryptoHelper);
			outOfProcessServerTransportManager.MigrateDataReadyEventHandlers(this);
			lock (this.syncObject)
			{
				this.cmdTransportManagers.Add(powerShellCmdId, outOfProcessServerTransportManager);
			}
			this.stdOutWriter.WriteLine(OutOfProcessUtils.CreateCommandAckPacket(powerShellCmdId));
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000F2A58 File Offset: 0x000F0C58
		internal override AbstractServerTransportManager GetCommandTransportManager(Guid powerShellCmdId)
		{
			AbstractServerTransportManager result;
			lock (this.syncObject)
			{
				OutOfProcessServerTransportManager outOfProcessServerTransportManager = null;
				this.cmdTransportManagers.TryGetValue(powerShellCmdId, out outOfProcessServerTransportManager);
				result = outOfProcessServerTransportManager;
			}
			return result;
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000F2AA8 File Offset: 0x000F0CA8
		internal override void RemoveCommandTransportManager(Guid powerShellCmdId)
		{
			lock (this.syncObject)
			{
				if (this.cmdTransportManagers.ContainsKey(powerShellCmdId))
				{
					this.cmdTransportManagers.Remove(powerShellCmdId);
				}
			}
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000F2B00 File Offset: 0x000F0D00
		internal override void Close(Exception reasonForClose)
		{
			base.RaiseClosingEvent();
		}

		// Token: 0x04001600 RID: 5632
		private OutOfProcessTextWriter stdOutWriter;

		// Token: 0x04001601 RID: 5633
		private OutOfProcessTextWriter stdErrWriter;

		// Token: 0x04001602 RID: 5634
		private Dictionary<Guid, OutOfProcessServerTransportManager> cmdTransportManagers;

		// Token: 0x04001603 RID: 5635
		private object syncObject = new object();
	}
}
