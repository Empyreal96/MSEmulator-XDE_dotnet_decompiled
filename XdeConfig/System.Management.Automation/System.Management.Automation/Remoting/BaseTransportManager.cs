using System;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000361 RID: 865
	internal abstract class BaseTransportManager : IDisposable
	{
		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06002AB1 RID: 10929 RVA: 0x000EB4B8 File Offset: 0x000E96B8
		// (remove) Token: 0x06002AB2 RID: 10930 RVA: 0x000EB4F0 File Offset: 0x000E96F0
		internal event EventHandler<TransportErrorOccuredEventArgs> WSManTransportErrorOccured;

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06002AB3 RID: 10931 RVA: 0x000EB528 File Offset: 0x000E9728
		// (remove) Token: 0x06002AB4 RID: 10932 RVA: 0x000EB560 File Offset: 0x000E9760
		internal event EventHandler<RemoteDataEventArgs> DataReceived;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06002AB5 RID: 10933 RVA: 0x000EB598 File Offset: 0x000E9798
		// (remove) Token: 0x06002AB6 RID: 10934 RVA: 0x000EB5D0 File Offset: 0x000E97D0
		public event EventHandler PowerShellGuidObserver;

		// Token: 0x06002AB7 RID: 10935 RVA: 0x000EB608 File Offset: 0x000E9808
		protected BaseTransportManager(PSRemotingCryptoHelper cryptoHelper)
		{
			this.cryptoHelper = cryptoHelper;
			this.fragmentor = new Fragmentor(32768, cryptoHelper);
			this.recvdData = new PriorityReceiveDataCollection(this.fragmentor, this is BaseClientTransportManager);
			this.onDataAvailableCallback = new ReceiveDataCollection.OnDataAvailableCallback(this.OnDataAvailableCallback);
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06002AB8 RID: 10936 RVA: 0x000EB65F File Offset: 0x000E985F
		// (set) Token: 0x06002AB9 RID: 10937 RVA: 0x000EB667 File Offset: 0x000E9867
		internal Fragmentor Fragmentor
		{
			get
			{
				return this.fragmentor;
			}
			set
			{
				this.fragmentor = value;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06002ABA RID: 10938 RVA: 0x000EB670 File Offset: 0x000E9870
		// (set) Token: 0x06002ABB RID: 10939 RVA: 0x000EB67D File Offset: 0x000E987D
		internal TypeTable TypeTable
		{
			get
			{
				return this.fragmentor.TypeTable;
			}
			set
			{
				this.fragmentor.TypeTable = value;
			}
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000EB68C File Offset: 0x000E988C
		internal virtual void ProcessRawData(byte[] data, string stream)
		{
			try
			{
				this.ProcessRawData(data, stream, this.onDataAvailableCallback);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				BaseTransportManager.baseTracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Exception processing data. {0}", new object[]
				{
					ex.Message
				}), new object[0]);
				PSRemotingTransportException e = new PSRemotingTransportException(ex.Message, ex);
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.ReceiveShellOutputEx);
				this.RaiseErrorHandler(eventArgs);
			}
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000EB710 File Offset: 0x000E9910
		internal void ProcessRawData(byte[] data, string stream, ReceiveDataCollection.OnDataAvailableCallback dataAvailableCallback)
		{
			BaseTransportManager.baseTracer.WriteLine("Processing incoming data for stream {0}.", new object[]
			{
				stream
			});
			bool flag = false;
			DataPriorityType priorityType = DataPriorityType.Default;
			if (stream.Equals("stdin", StringComparison.OrdinalIgnoreCase) || stream.Equals("stdout", StringComparison.OrdinalIgnoreCase))
			{
				flag = true;
			}
			else if (stream.Equals("pr", StringComparison.OrdinalIgnoreCase))
			{
				priorityType = DataPriorityType.PromptResponse;
				flag = true;
			}
			if (!flag)
			{
				BaseTransportManager.baseTracer.WriteLine("{0} is not a valid stream", new object[]
				{
					stream
				});
			}
			this.recvdData.ProcessRawData(data, priorityType, dataAvailableCallback);
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000EB79C File Offset: 0x000E999C
		internal void OnDataAvailableCallback(RemoteDataObject<PSObject> remoteObject)
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.TransportReceivedObject, PSOpcode.Open, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				remoteObject.RunspacePoolId.ToString(),
				remoteObject.PowerShellId.ToString(),
				(uint)remoteObject.Destination,
				(uint)remoteObject.DataType,
				(uint)remoteObject.TargetInterface
			});
			this.PowerShellGuidObserver.SafeInvoke(remoteObject.PowerShellId, EventArgs.Empty);
			RemoteDataEventArgs eventArgs = new RemoteDataEventArgs(remoteObject);
			this.DataReceived.SafeInvoke(this, eventArgs);
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x000EB850 File Offset: 0x000E9A50
		public void MigrateDataReadyEventHandlers(BaseTransportManager transportManager)
		{
			foreach (Delegate @delegate in transportManager.DataReceived.GetInvocationList())
			{
				this.DataReceived += (EventHandler<RemoteDataEventArgs>)@delegate;
			}
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000EB887 File Offset: 0x000E9A87
		internal virtual void RaiseErrorHandler(TransportErrorOccuredEventArgs eventArgs)
		{
			this.WSManTransportErrorOccured.SafeInvoke(this, eventArgs);
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x000EB896 File Offset: 0x000E9A96
		// (set) Token: 0x06002AC2 RID: 10946 RVA: 0x000EB89E File Offset: 0x000E9A9E
		internal PSRemotingCryptoHelper CryptoHelper
		{
			get
			{
				return this.cryptoHelper;
			}
			set
			{
				this.cryptoHelper = value;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x000EB8A7 File Offset: 0x000E9AA7
		internal PriorityReceiveDataCollection ReceivedDataCollection
		{
			get
			{
				return this.recvdData;
			}
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000EB8AF File Offset: 0x000E9AAF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x000EB8BE File Offset: 0x000E9ABE
		internal virtual void Dispose(bool isDisposing)
		{
			if (isDisposing)
			{
				this.recvdData.Dispose();
			}
		}

		// Token: 0x04001530 RID: 5424
		internal const int ServerDefaultKeepAliveTimeoutMs = 240000;

		// Token: 0x04001531 RID: 5425
		internal const int ClientDefaultOperationTimeoutMs = 180000;

		// Token: 0x04001532 RID: 5426
		internal const int ClientCloseTimeoutMs = 60000;

		// Token: 0x04001533 RID: 5427
		internal const int UseServerDefaultIdleTimeout = -1;

		// Token: 0x04001534 RID: 5428
		internal const uint UseServerDefaultIdleTimeoutUInt = 4294967295U;

		// Token: 0x04001535 RID: 5429
		internal const int MinimumIdleTimeout = 60000;

		// Token: 0x04001536 RID: 5430
		internal const int DefaultFragmentSize = 32768;

		// Token: 0x04001537 RID: 5431
		internal const int MaximumReceivedDataSize = 52428800;

		// Token: 0x04001538 RID: 5432
		internal const int MaximumReceivedObjectSize = 10485760;

		// Token: 0x04001539 RID: 5433
		internal const string MAX_RECEIVED_DATA_PER_COMMAND_MB = "PSMaximumReceivedDataSizePerCommandMB";

		// Token: 0x0400153A RID: 5434
		internal const string MAX_RECEIVED_OBJECT_SIZE_MB = "PSMaximumReceivedObjectSizeMB";

		// Token: 0x0400153B RID: 5435
		[TraceSource("Transport", "Traces BaseWSManTransportManager")]
		private static PSTraceSource baseTracer = PSTraceSource.GetTracer("Transport", "Traces BaseWSManTransportManager");

		// Token: 0x0400153C RID: 5436
		private Fragmentor fragmentor;

		// Token: 0x0400153D RID: 5437
		private PriorityReceiveDataCollection recvdData;

		// Token: 0x0400153E RID: 5438
		private ReceiveDataCollection.OnDataAvailableCallback onDataAvailableCallback;

		// Token: 0x0400153F RID: 5439
		private PSRemotingCryptoHelper cryptoHelper;
	}
}
