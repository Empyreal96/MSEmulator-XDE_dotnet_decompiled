using System;
using System.Xml;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200038D RID: 909
	internal class PriorityReceiveDataCollection : IDisposable
	{
		// Token: 0x06002C01 RID: 11265 RVA: 0x000F3670 File Offset: 0x000F1870
		internal PriorityReceiveDataCollection(Fragmentor defragmentor, bool createdByClientTM)
		{
			this.defragmentor = defragmentor;
			string[] names = Enum.GetNames(typeof(DataPriorityType));
			this.recvdData = new ReceiveDataCollection[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				this.recvdData[i] = new ReceiveDataCollection(defragmentor, createdByClientTM);
			}
			this.isCreateByClientTM = createdByClientTM;
		}

		// Token: 0x17000A73 RID: 2675
		// (set) Token: 0x06002C02 RID: 11266 RVA: 0x000F36CC File Offset: 0x000F18CC
		internal int? MaximumReceivedDataSize
		{
			set
			{
				this.defragmentor.DeserializationContext.MaximumAllowedMemory = value;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (set) Token: 0x06002C03 RID: 11267 RVA: 0x000F36E0 File Offset: 0x000F18E0
		internal int? MaximumReceivedObjectSize
		{
			set
			{
				foreach (ReceiveDataCollection receiveDataCollection in this.recvdData)
				{
					receiveDataCollection.MaximumReceivedObjectSize = value;
				}
			}
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x000F3710 File Offset: 0x000F1910
		internal void PrepareForStreamConnect()
		{
			for (int i = 0; i < this.recvdData.Length; i++)
			{
				this.recvdData[i].PrepareForStreamConnect();
			}
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x000F3740 File Offset: 0x000F1940
		internal void AllowTwoThreadsToProcessRawData()
		{
			for (int i = 0; i < this.recvdData.Length; i++)
			{
				this.recvdData[i].AllowTwoThreadsToProcessRawData();
			}
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x000F3770 File Offset: 0x000F1970
		internal void ProcessRawData(byte[] data, DataPriorityType priorityType, ReceiveDataCollection.OnDataAvailableCallback callback)
		{
			try
			{
				this.defragmentor.DeserializationContext.LogExtraMemoryUsage(data.Length);
			}
			catch (XmlException)
			{
				PSRemotingTransportException ex;
				if (this.isCreateByClientTM)
				{
					ex = new PSRemotingTransportException(PSRemotingErrorId.ReceivedDataSizeExceededMaximumClient, RemotingErrorIdStrings.ReceivedDataSizeExceededMaximumClient, new object[]
					{
						this.defragmentor.DeserializationContext.MaximumAllowedMemory.Value
					});
				}
				else
				{
					ex = new PSRemotingTransportException(PSRemotingErrorId.ReceivedDataSizeExceededMaximumServer, RemotingErrorIdStrings.ReceivedDataSizeExceededMaximumServer, new object[]
					{
						this.defragmentor.DeserializationContext.MaximumAllowedMemory.Value
					});
				}
				throw ex;
			}
			this.recvdData[(int)priorityType].ProcessRawData(data, callback);
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x000F3834 File Offset: 0x000F1A34
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000F3844 File Offset: 0x000F1A44
		internal virtual void Dispose(bool isDisposing)
		{
			if (this.recvdData != null)
			{
				for (int i = 0; i < this.recvdData.Length; i++)
				{
					this.recvdData[i].Dispose();
				}
			}
		}

		// Token: 0x04001620 RID: 5664
		private Fragmentor defragmentor;

		// Token: 0x04001621 RID: 5665
		private ReceiveDataCollection[] recvdData;

		// Token: 0x04001622 RID: 5666
		private bool isCreateByClientTM;
	}
}
