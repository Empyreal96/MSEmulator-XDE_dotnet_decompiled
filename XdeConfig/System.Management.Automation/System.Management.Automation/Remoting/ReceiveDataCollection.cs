using System;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Tracing;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200038B RID: 907
	internal class ReceiveDataCollection : IDisposable
	{
		// Token: 0x06002BF3 RID: 11251 RVA: 0x000F2E74 File Offset: 0x000F1074
		internal ReceiveDataCollection(Fragmentor defragmentor, bool createdByClientTM)
		{
			this.pendingDataStream = new MemoryStream();
			this.syncObject = new object();
			this.defragmentor = defragmentor;
			this.isCreateByClientTM = createdByClientTM;
		}

		// Token: 0x17000A72 RID: 2674
		// (set) Token: 0x06002BF4 RID: 11252 RVA: 0x000F2EA7 File Offset: 0x000F10A7
		internal int? MaximumReceivedObjectSize
		{
			set
			{
				this.maxReceivedObjectSize = value;
			}
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x000F2EB0 File Offset: 0x000F10B0
		internal void AllowTwoThreadsToProcessRawData()
		{
			this.maxNumberOfThreadsToAllowForProcessing = 2;
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x000F2EB9 File Offset: 0x000F10B9
		internal void PrepareForStreamConnect()
		{
			this.canIgnoreOffSyncFragments = true;
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x000F2EC4 File Offset: 0x000F10C4
		internal void ProcessRawData(byte[] data, ReceiveDataCollection.OnDataAvailableCallback callback)
		{
			lock (this.syncObject)
			{
				if (this.isDisposed)
				{
					return;
				}
				this.numberOfThreadsProcessing++;
				int num = this.numberOfThreadsProcessing;
				int num2 = this.maxNumberOfThreadsToAllowForProcessing;
			}
			try
			{
				this.pendingDataStream.Write(data, 0, data.Length);
				while (this.pendingDataStream.Length > 21L)
				{
					byte[] fragmentBytes = this.pendingDataStream.ToArray();
					long objectId = FragmentedRemoteObject.GetObjectId(fragmentBytes, 0);
					if (objectId <= 0L)
					{
						throw new PSRemotingTransportException(RemotingErrorIdStrings.ObjectIdCannotBeLessThanZero);
					}
					long fragmentId = FragmentedRemoteObject.GetFragmentId(fragmentBytes, 0);
					bool isStartFragment = FragmentedRemoteObject.GetIsStartFragment(fragmentBytes, 0);
					bool isEndFragment = FragmentedRemoteObject.GetIsEndFragment(fragmentBytes, 0);
					int blobLength = FragmentedRemoteObject.GetBlobLength(fragmentBytes, 0);
					ReceiveDataCollection.baseTracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Object Id: {0}", new object[]
					{
						objectId
					}), new object[0]);
					ReceiveDataCollection.baseTracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Fragment Id: {0}", new object[]
					{
						fragmentId
					}), new object[0]);
					ReceiveDataCollection.baseTracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Start Flag: {0}", new object[]
					{
						isStartFragment
					}), new object[0]);
					ReceiveDataCollection.baseTracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "End Flag: {0}", new object[]
					{
						isEndFragment
					}), new object[0]);
					ReceiveDataCollection.baseTracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Blob Length: {0}", new object[]
					{
						blobLength
					}), new object[0]);
					int num3 = 0;
					try
					{
						num3 = checked(21 + blobLength);
					}
					catch (OverflowException)
					{
						ReceiveDataCollection.baseTracer.WriteLine("Fragement too big.", new object[0]);
						this.ResetRecieveData();
						PSRemotingTransportException ex = new PSRemotingTransportException(RemotingErrorIdStrings.ObjectIsTooBig);
						throw ex;
					}
					if (this.pendingDataStream.Length < (long)num3)
					{
						ReceiveDataCollection.baseTracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Not enough data to process packet. Data is less than expected blob length. Data length {0}. Expected Length {1}.", new object[]
						{
							this.pendingDataStream.Length,
							num3
						}), new object[0]);
						return;
					}
					if (this.maxReceivedObjectSize != null)
					{
						this.totalReceivedObjectSizeSoFar += num3;
						if (this.totalReceivedObjectSizeSoFar < 0 || this.totalReceivedObjectSizeSoFar > this.maxReceivedObjectSize.Value)
						{
							ReceiveDataCollection.baseTracer.WriteLine("ObjectSize > MaxReceivedObjectSize. ObjectSize is {0}. MaxReceivedObjectSize is {1}", new object[]
							{
								this.totalReceivedObjectSizeSoFar,
								this.maxReceivedObjectSize
							});
							PSRemotingTransportException ex2;
							if (this.isCreateByClientTM)
							{
								ex2 = new PSRemotingTransportException(PSRemotingErrorId.ReceivedObjectSizeExceededMaximumClient, RemotingErrorIdStrings.ReceivedObjectSizeExceededMaximumClient, new object[]
								{
									this.totalReceivedObjectSizeSoFar,
									this.maxReceivedObjectSize
								});
							}
							else
							{
								ex2 = new PSRemotingTransportException(PSRemotingErrorId.ReceivedObjectSizeExceededMaximumServer, RemotingErrorIdStrings.ReceivedObjectSizeExceededMaximumServer, new object[]
								{
									this.totalReceivedObjectSizeSoFar,
									this.maxReceivedObjectSize
								});
							}
							this.ResetRecieveData();
							throw ex2;
						}
					}
					this.pendingDataStream.Seek(0L, SeekOrigin.Begin);
					byte[] array = new byte[num3];
					this.pendingDataStream.Read(array, 0, num3);
					PSEtwLog.LogAnalyticVerbose(PSEventId.ReceivedRemotingFragment, PSOpcode.Receive, PSTask.None, (PSKeyword)4611686018427387912UL, objectId, fragmentId, isStartFragment ? 1 : 0, isEndFragment ? 1 : 0, (uint)blobLength, new PSETWBinaryBlob(array, 21, blobLength));
					byte[] array2 = null;
					if ((long)num3 < this.pendingDataStream.Length)
					{
						array2 = new byte[this.pendingDataStream.Length - (long)num3];
						this.pendingDataStream.Read(array2, 0, (int)(this.pendingDataStream.Length - (long)num3));
					}
					this.pendingDataStream.Dispose();
					this.pendingDataStream = new MemoryStream();
					if (array2 != null)
					{
						this.pendingDataStream.Write(array2, 0, array2.Length);
					}
					if (isStartFragment)
					{
						this.canIgnoreOffSyncFragments = false;
						this.currentObjectId = objectId;
						this.dataToProcessStream = new MemoryStream();
					}
					else if (objectId != this.currentObjectId)
					{
						ReceiveDataCollection.baseTracer.WriteLine("ObjectId != CurrentObjectId", new object[0]);
						this.ResetRecieveData();
						if (!this.canIgnoreOffSyncFragments)
						{
							PSRemotingTransportException ex3 = new PSRemotingTransportException(RemotingErrorIdStrings.ObjectIdsNotMatching);
							throw ex3;
						}
						ReceiveDataCollection.baseTracer.WriteLine("Ignoring ObjectId != CurrentObjectId", new object[0]);
						continue;
					}
					else if (fragmentId != this.currentFrgId + 1L)
					{
						ReceiveDataCollection.baseTracer.WriteLine("Fragment Id is not in sequence.", new object[0]);
						this.ResetRecieveData();
						if (!this.canIgnoreOffSyncFragments)
						{
							PSRemotingTransportException ex4 = new PSRemotingTransportException(RemotingErrorIdStrings.FragmetIdsNotInSequence);
							throw ex4;
						}
						ReceiveDataCollection.baseTracer.WriteLine("Ignoring Fragment Id is not in sequence.", new object[0]);
						continue;
					}
					this.currentFrgId = fragmentId;
					this.dataToProcessStream.Write(array, 21, blobLength);
					if (isEndFragment)
					{
						try
						{
							this.dataToProcessStream.Seek(0L, SeekOrigin.Begin);
							RemoteDataObject<PSObject> remoteDataObject = RemoteDataObject<PSObject>.CreateFrom(this.dataToProcessStream, this.defragmentor);
							ReceiveDataCollection.baseTracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Runspace Id: {0}", new object[]
							{
								remoteDataObject.RunspacePoolId
							}), new object[0]);
							ReceiveDataCollection.baseTracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "PowerShell Id: {0}", new object[]
							{
								remoteDataObject.PowerShellId
							}), new object[0]);
							callback(remoteDataObject);
						}
						finally
						{
							this.ResetRecieveData();
						}
						if (this.isDisposed)
						{
							return;
						}
					}
				}
				ReceiveDataCollection.baseTracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Not enough data to process. Data is less than header length. Data length is {0}. Header Length {1}.", new object[]
				{
					this.pendingDataStream.Length,
					21
				}), new object[0]);
			}
			finally
			{
				lock (this.syncObject)
				{
					if (this.isDisposed && this.numberOfThreadsProcessing == 1)
					{
						this.ReleaseResources();
					}
					this.numberOfThreadsProcessing--;
				}
			}
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x000F3594 File Offset: 0x000F1794
		private void ResetRecieveData()
		{
			if (this.dataToProcessStream != null)
			{
				this.dataToProcessStream.Dispose();
			}
			this.currentObjectId = 0L;
			this.currentFrgId = 0L;
			this.totalReceivedObjectSizeSoFar = 0;
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x000F35C0 File Offset: 0x000F17C0
		private void ReleaseResources()
		{
			if (this.pendingDataStream != null)
			{
				this.pendingDataStream.Dispose();
				this.pendingDataStream = null;
			}
			if (this.dataToProcessStream != null)
			{
				this.dataToProcessStream.Dispose();
				this.dataToProcessStream = null;
			}
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x000F35F6 File Offset: 0x000F17F6
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x000F3608 File Offset: 0x000F1808
		internal virtual void Dispose(bool isDisposing)
		{
			lock (this.syncObject)
			{
				this.isDisposed = true;
				if (this.numberOfThreadsProcessing == 0)
				{
					this.ReleaseResources();
				}
			}
		}

		// Token: 0x04001612 RID: 5650
		[TraceSource("Transport", "Traces BaseWSManTransportManager")]
		private static PSTraceSource baseTracer = PSTraceSource.GetTracer("Transport", "Traces BaseWSManTransportManager");

		// Token: 0x04001613 RID: 5651
		private Fragmentor defragmentor;

		// Token: 0x04001614 RID: 5652
		private MemoryStream pendingDataStream;

		// Token: 0x04001615 RID: 5653
		private MemoryStream dataToProcessStream;

		// Token: 0x04001616 RID: 5654
		private long currentObjectId;

		// Token: 0x04001617 RID: 5655
		private long currentFrgId;

		// Token: 0x04001618 RID: 5656
		private int? maxReceivedObjectSize;

		// Token: 0x04001619 RID: 5657
		private int totalReceivedObjectSizeSoFar;

		// Token: 0x0400161A RID: 5658
		private bool isCreateByClientTM;

		// Token: 0x0400161B RID: 5659
		private bool canIgnoreOffSyncFragments;

		// Token: 0x0400161C RID: 5660
		private object syncObject;

		// Token: 0x0400161D RID: 5661
		private bool isDisposed;

		// Token: 0x0400161E RID: 5662
		private int numberOfThreadsProcessing;

		// Token: 0x0400161F RID: 5663
		private int maxNumberOfThreadsToAllowForProcessing = 1;

		// Token: 0x0200038C RID: 908
		// (Invoke) Token: 0x06002BFE RID: 11262
		internal delegate void OnDataAvailableCallback(RemoteDataObject<PSObject> data);
	}
}
