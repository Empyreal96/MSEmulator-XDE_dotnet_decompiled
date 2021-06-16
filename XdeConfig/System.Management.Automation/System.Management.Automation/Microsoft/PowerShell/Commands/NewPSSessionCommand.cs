using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Threading;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000328 RID: 808
	[Cmdlet("New", "PSSession", DefaultParameterSetName = "ComputerName", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=135237", RemotingCapability = RemotingCapability.OwnedByCommand)]
	[OutputType(new Type[]
	{
		typeof(PSSession)
	})]
	public class NewPSSessionCommand : PSRemotingBaseCmdlet, IDisposable
	{
		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x000DA0B7 File Offset: 0x000D82B7
		// (set) Token: 0x060026F2 RID: 9970 RVA: 0x000DA0BF File Offset: 0x000D82BF
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Alias(new string[]
		{
			"Cn"
		})]
		public override string[] ComputerName
		{
			get
			{
				return this.computerNames;
			}
			set
			{
				this.computerNames = value;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x000DA0C8 File Offset: 0x000D82C8
		// (set) Token: 0x060026F4 RID: 9972 RVA: 0x000DA0D0 File Offset: 0x000D82D0
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "Uri")]
		[Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "ComputerName")]
		[Credential]
		public override PSCredential Credential
		{
			get
			{
				return base.Credential;
			}
			set
			{
				base.Credential = value;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x000DA0D9 File Offset: 0x000D82D9
		// (set) Token: 0x060026F6 RID: 9974 RVA: 0x000DA0E1 File Offset: 0x000D82E1
		[Parameter(Position = 0, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, ParameterSetName = "Session")]
		[ValidateNotNullOrEmpty]
		public override PSSession[] Session
		{
			get
			{
				return this.remoteRunspaceInfos;
			}
			set
			{
				this.remoteRunspaceInfos = value;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x060026F7 RID: 9975 RVA: 0x000DA0EA File Offset: 0x000D82EA
		// (set) Token: 0x060026F8 RID: 9976 RVA: 0x000DA0F2 File Offset: 0x000D82F2
		[Parameter]
		public string[] Name
		{
			get
			{
				return this.names;
			}
			set
			{
				this.names = value;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x060026F9 RID: 9977 RVA: 0x000DA0FB File Offset: 0x000D82FB
		// (set) Token: 0x060026FA RID: 9978 RVA: 0x000DA103 File Offset: 0x000D8303
		[Parameter]
		public SwitchParameter EnableNetworkAccess
		{
			get
			{
				return this.enableNetworkAccess;
			}
			set
			{
				this.enableNetworkAccess = value;
			}
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000DA10C File Offset: 0x000D830C
		protected override void BeginProcessing()
		{
			base.BeginProcessing();
			this.operationsComplete.Reset();
			this.throttleManager.ThrottleLimit = this.ThrottleLimit;
			this.throttleManager.ThrottleComplete += this.HandleThrottleComplete;
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x000DA148 File Offset: 0x000D8348
		protected override void ProcessRecord()
		{
			List<IThrottleOperation> list = new List<IThrottleOperation>();
			string parameterSetName;
			List<RemoteRunspace> list2;
			if ((parameterSetName = base.ParameterSetName) != null)
			{
				if (parameterSetName == "Session")
				{
					list2 = this.CreateRunspacesWhenRunspaceParameterSpecified();
					goto IL_60;
				}
				if (parameterSetName == "Uri")
				{
					list2 = this.CreateRunspacesWhenUriParameterSpecified();
					goto IL_60;
				}
				if (parameterSetName == "ComputerName")
				{
					list2 = this.CreateRunspacesWhenComputerNameParameterSpecified();
					goto IL_60;
				}
			}
			list2 = new List<RemoteRunspace>();
			IL_60:
			foreach (RemoteRunspace remoteRunspace in list2)
			{
				remoteRunspace.Events.ReceivedEvents.PSEventReceived += this.OnRunspacePSEventReceived;
				OpenRunspaceOperation openRunspaceOperation = new OpenRunspaceOperation(remoteRunspace);
				openRunspaceOperation.OperationComplete += this.HandleRunspaceStateChanged;
				remoteRunspace.URIRedirectionReported += this.HandleURIDirectionReported;
				list.Add(openRunspaceOperation);
			}
			this.throttleManager.SubmitOperations(list);
			this.allOperations.Add(list);
			Collection<object> collection = this.stream.ObjectReader.NonBlockingRead();
			foreach (object obj in collection)
			{
				base.WriteStreamObject((Action<Cmdlet>)obj);
			}
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x000DA2AC File Offset: 0x000D84AC
		protected override void EndProcessing()
		{
			this.throttleManager.EndSubmitOperations();
			for (;;)
			{
				this.stream.ObjectReader.WaitHandle.WaitOne();
				if (this.stream.ObjectReader.EndOfPipeline)
				{
					break;
				}
				object obj = this.stream.ObjectReader.Read();
				base.WriteStreamObject((Action<Cmdlet>)obj);
			}
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x000DA30B File Offset: 0x000D850B
		protected override void StopProcessing()
		{
			this.stream.ObjectWriter.Close();
			this.throttleManager.StopAllOperations();
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x000DA328 File Offset: 0x000D8528
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x000DA337 File Offset: 0x000D8537
		private void OnRunspacePSEventReceived(object sender, PSEventArgs e)
		{
			if (base.Events != null)
			{
				base.Events.AddForwardedEvent(e);
			}
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000DA364 File Offset: 0x000D8564
		private void HandleURIDirectionReported(object sender, RemoteDataEventArgs<Uri> eventArgs)
		{
			string message = StringUtil.Format(RemotingErrorIdStrings.URIRedirectWarningToHost, eventArgs.Data.OriginalString);
			Action<Cmdlet> value = delegate(Cmdlet cmdlet)
			{
				cmdlet.WriteWarning(message);
			};
			this.stream.Write(value);
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000DA404 File Offset: 0x000D8604
		private void HandleRunspaceStateChanged(object sender, OperationStateEventArgs stateEventArgs)
		{
			if (sender == null)
			{
				throw PSTraceSource.NewArgumentNullException("sender");
			}
			if (stateEventArgs == null)
			{
				throw PSTraceSource.NewArgumentNullException("stateEventArgs");
			}
			RunspaceStateEventArgs runspaceStateEventArgs = stateEventArgs.BaseEvent as RunspaceStateEventArgs;
			RunspaceStateInfo runspaceStateInfo = runspaceStateEventArgs.RunspaceStateInfo;
			RunspaceState state = runspaceStateInfo.State;
			OpenRunspaceOperation openRunspaceOperation = sender as OpenRunspaceOperation;
			RemoteRunspace operatedRunspace = openRunspaceOperation.OperatedRunspace;
			if (operatedRunspace != null)
			{
				operatedRunspace.URIRedirectionReported -= this.HandleURIDirectionReported;
			}
			PipelineWriter objectWriter = this.stream.ObjectWriter;
			Exception ex = runspaceStateEventArgs.RunspaceStateInfo.Reason;
			switch (state)
			{
			case RunspaceState.Opened:
			{
				PSSession remoteRunspaceInfo = new PSSession(operatedRunspace);
				base.RunspaceRepository.Add(remoteRunspaceInfo);
				Action<Cmdlet> obj = delegate(Cmdlet cmdlet)
				{
					cmdlet.WriteObject(remoteRunspaceInfo);
				};
				if (objectWriter.IsOpen)
				{
					objectWriter.Write(obj);
					return;
				}
				break;
			}
			case RunspaceState.Closed:
			{
				Uri uri = WSManConnectionInfo.ExtractPropertyAsWsManConnectionInfo<Uri>(operatedRunspace.ConnectionInfo, "ConnectionUri", null);
				string message = base.GetMessage(RemotingErrorIdStrings.RemoteRunspaceClosed, new object[]
				{
					(uri != null) ? uri.AbsoluteUri : string.Empty
				});
				Action<Cmdlet> obj2 = delegate(Cmdlet cmdlet)
				{
					cmdlet.WriteVerbose(message);
				};
				if (objectWriter.IsOpen)
				{
					objectWriter.Write(obj2);
				}
				if (ex != null)
				{
					ErrorRecord errorRecord = new ErrorRecord(ex, "PSSessionStateClosed", ErrorCategory.OpenError, operatedRunspace);
					Action<Cmdlet> obj3 = delegate(Cmdlet cmdlet)
					{
						cmdlet.WriteError(errorRecord);
					};
					if (objectWriter.IsOpen)
					{
						objectWriter.Write(obj3);
					}
				}
				break;
			}
			case RunspaceState.Closing:
				break;
			case RunspaceState.Broken:
			{
				PSRemotingTransportException ex2 = ex as PSRemotingTransportException;
				string text = null;
				if (ex2 != null)
				{
					OpenRunspaceOperation openRunspaceOperation2 = sender as OpenRunspaceOperation;
					if (openRunspaceOperation2 != null)
					{
						string computerName = openRunspaceOperation2.OperatedRunspace.ConnectionInfo.ComputerName;
						if (ex2.ErrorCode == -2144108135)
						{
							string str = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.URIRedirectionReported, new object[]
							{
								ex2.Message,
								"MaximumConnectionRedirectionCount",
								"PSSessionOption",
								"AllowRedirection"
							});
							text = "[" + computerName + "] " + str;
						}
						else
						{
							text = "[" + computerName + "] ";
							if (!string.IsNullOrEmpty(ex2.Message))
							{
								text += ex2.Message;
							}
							else if (!string.IsNullOrEmpty(ex2.TransportMessage))
							{
								text += ex2.TransportMessage;
							}
						}
					}
				}
				PSRemotingDataStructureException ex3 = ex as PSRemotingDataStructureException;
				if (ex3 != null)
				{
					OpenRunspaceOperation openRunspaceOperation3 = sender as OpenRunspaceOperation;
					if (openRunspaceOperation3 != null)
					{
						string computerName2 = openRunspaceOperation3.OperatedRunspace.ConnectionInfo.ComputerName;
						text = "[" + computerName2 + "] " + ex3.Message;
					}
				}
				if (ex == null)
				{
					ex = new RuntimeException(base.GetMessage(RemotingErrorIdStrings.RemoteRunspaceOpenUnknownState, new object[]
					{
						state
					}));
				}
				string fqeidfromTransportError = WSManTransportManagerUtils.GetFQEIDFromTransportError((ex2 != null) ? ex2.ErrorCode : 0, this._defaultFQEID);
				ErrorRecord errorRecord = new ErrorRecord(ex, operatedRunspace, fqeidfromTransportError, ErrorCategory.OpenError, null, null, null, null, null, text, null);
				Action<Cmdlet> obj4 = delegate(Cmdlet cmdlet)
				{
					cmdlet.WriteError(errorRecord);
				};
				if (objectWriter.IsOpen)
				{
					objectWriter.Write(obj4);
				}
				this.toDispose.Add(operatedRunspace);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000DA78C File Offset: 0x000D898C
		private List<RemoteRunspace> CreateRunspacesWhenRunspaceParameterSpecified()
		{
			List<RemoteRunspace> list = new List<RemoteRunspace>();
			base.ValidateRemoteRunspacesSpecified();
			int num = 0;
			foreach (PSSession pssession in this.remoteRunspaceInfos)
			{
				if (pssession == null || pssession.Runspace == null)
				{
					base.ThrowTerminatingError(new ErrorRecord(new ArgumentNullException("PSSession"), "PSSessionArgumentNull", ErrorCategory.InvalidArgument, null));
				}
				else
				{
					try
					{
						RemoteRunspace remoteRunspace = (RemoteRunspace)pssession.Runspace;
						WSManConnectionInfo wsmanConnectionInfo = remoteRunspace.ConnectionInfo as WSManConnectionInfo;
						WSManConnectionInfo wsmanConnectionInfo2;
						if (wsmanConnectionInfo != null)
						{
							wsmanConnectionInfo2 = wsmanConnectionInfo.Copy();
							wsmanConnectionInfo2.EnableNetworkAccess = (wsmanConnectionInfo2.EnableNetworkAccess || this.EnableNetworkAccess);
						}
						else
						{
							Uri uri = WSManConnectionInfo.ExtractPropertyAsWsManConnectionInfo<Uri>(remoteRunspace.ConnectionInfo, "ConnectionUri", null);
							string shellUri = WSManConnectionInfo.ExtractPropertyAsWsManConnectionInfo<string>(remoteRunspace.ConnectionInfo, "ShellUri", string.Empty);
							wsmanConnectionInfo2 = new WSManConnectionInfo(uri, shellUri, remoteRunspace.ConnectionInfo.Credential);
							base.UpdateConnectionInfo(wsmanConnectionInfo2);
							wsmanConnectionInfo2.EnableNetworkAccess = this.EnableNetworkAccess;
						}
						RemoteRunspacePoolInternal remoteRunspacePoolInternal = remoteRunspace.RunspacePool.RemoteRunspacePoolInternal;
						TypeTable typeTable = null;
						if (remoteRunspacePoolInternal != null && remoteRunspacePoolInternal.DataStructureHandler != null && remoteRunspacePoolInternal.DataStructureHandler.TransportManager != null)
						{
							typeTable = remoteRunspacePoolInternal.DataStructureHandler.TransportManager.Fragmentor.TypeTable;
						}
						int id;
						string runspaceName = this.GetRunspaceName(num, out id);
						RemoteRunspace item = new RemoteRunspace(typeTable, wsmanConnectionInfo2, base.Host, this.SessionOption.ApplicationArguments, runspaceName, id);
						list.Add(item);
					}
					catch (UriFormatException exception)
					{
						PipelineWriter objectWriter = this.stream.ObjectWriter;
						ErrorRecord errorRecord = new ErrorRecord(exception, "CreateRemoteRunspaceFailed", ErrorCategory.InvalidArgument, pssession);
						Action<Cmdlet> obj = delegate(Cmdlet cmdlet)
						{
							cmdlet.WriteError(errorRecord);
						};
						objectWriter.Write(obj);
					}
				}
				num++;
			}
			return list;
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000DA980 File Offset: 0x000D8B80
		private List<RemoteRunspace> CreateRunspacesWhenUriParameterSpecified()
		{
			List<RemoteRunspace> list = new List<RemoteRunspace>();
			for (int i = 0; i < this.ConnectionUri.Length; i++)
			{
				try
				{
					WSManConnectionInfo wsmanConnectionInfo = new WSManConnectionInfo();
					wsmanConnectionInfo.ConnectionUri = this.ConnectionUri[i];
					wsmanConnectionInfo.ShellUri = this.ConfigurationName;
					if (this.CertificateThumbprint != null)
					{
						wsmanConnectionInfo.CertificateThumbprint = this.CertificateThumbprint;
					}
					else
					{
						wsmanConnectionInfo.Credential = this.Credential;
					}
					wsmanConnectionInfo.AuthenticationMechanism = this.Authentication;
					base.UpdateConnectionInfo(wsmanConnectionInfo);
					wsmanConnectionInfo.EnableNetworkAccess = this.EnableNetworkAccess;
					int id;
					string runspaceName = this.GetRunspaceName(i, out id);
					RemoteRunspace item = new RemoteRunspace(Utils.GetTypeTableFromExecutionContextTLS(), wsmanConnectionInfo, base.Host, this.SessionOption.ApplicationArguments, runspaceName, id);
					list.Add(item);
				}
				catch (UriFormatException e)
				{
					this.WriteErrorCreateRemoteRunspaceFailed(e, this.ConnectionUri[i]);
				}
				catch (InvalidOperationException e2)
				{
					this.WriteErrorCreateRemoteRunspaceFailed(e2, this.ConnectionUri[i]);
				}
				catch (ArgumentException e3)
				{
					this.WriteErrorCreateRemoteRunspaceFailed(e3, this.ConnectionUri[i]);
				}
				catch (NotSupportedException e4)
				{
					this.WriteErrorCreateRemoteRunspaceFailed(e4, this.ConnectionUri[i]);
				}
			}
			return list;
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000DAAE8 File Offset: 0x000D8CE8
		private List<RemoteRunspace> CreateRunspacesWhenComputerNameParameterSpecified()
		{
			List<RemoteRunspace> list = new List<RemoteRunspace>();
			string[] array;
			base.ResolveComputerNames(this.ComputerName, out array);
			base.ValidateComputerName(array);
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					WSManConnectionInfo wsmanConnectionInfo = new WSManConnectionInfo();
					string scheme = this.UseSSL.IsPresent ? "https" : "http";
					wsmanConnectionInfo.ComputerName = array[i];
					wsmanConnectionInfo.Port = this.Port;
					wsmanConnectionInfo.AppName = this.ApplicationName;
					wsmanConnectionInfo.ShellUri = this.ConfigurationName;
					wsmanConnectionInfo.Scheme = scheme;
					if (this.CertificateThumbprint != null)
					{
						wsmanConnectionInfo.CertificateThumbprint = this.CertificateThumbprint;
					}
					else
					{
						wsmanConnectionInfo.Credential = this.Credential;
					}
					wsmanConnectionInfo.AuthenticationMechanism = this.Authentication;
					base.UpdateConnectionInfo(wsmanConnectionInfo);
					wsmanConnectionInfo.EnableNetworkAccess = this.EnableNetworkAccess;
					int id;
					string runspaceName = this.GetRunspaceName(i, out id);
					RemoteRunspace item = new RemoteRunspace(Utils.GetTypeTableFromExecutionContextTLS(), wsmanConnectionInfo, base.Host, this.SessionOption.ApplicationArguments, runspaceName, id);
					list.Add(item);
				}
				catch (UriFormatException exception)
				{
					PipelineWriter objectWriter = this.stream.ObjectWriter;
					ErrorRecord errorRecord = new ErrorRecord(exception, "CreateRemoteRunspaceFailed", ErrorCategory.InvalidArgument, array[i]);
					Action<Cmdlet> obj = delegate(Cmdlet cmdlet)
					{
						cmdlet.WriteError(errorRecord);
					};
					objectWriter.Write(obj);
				}
			}
			return list;
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000DAC58 File Offset: 0x000D8E58
		private string GetRunspaceName(int rsIndex, out int rsId)
		{
			string result = PSSession.GenerateRunspaceName(out rsId);
			if (this.names != null && rsIndex < this.names.Length)
			{
				result = this.names[rsIndex];
			}
			return result;
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000DAC8C File Offset: 0x000D8E8C
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.throttleManager.Dispose();
				this.operationsComplete.WaitOne();
				this.operationsComplete.Dispose();
				this.throttleManager.ThrottleComplete -= this.HandleThrottleComplete;
				this.throttleManager = null;
				foreach (RemoteRunspace remoteRunspace in this.toDispose)
				{
					remoteRunspace.Dispose();
				}
				foreach (List<IThrottleOperation> list in this.allOperations)
				{
					foreach (IThrottleOperation throttleOperation in list)
					{
						OpenRunspaceOperation openRunspaceOperation = (OpenRunspaceOperation)throttleOperation;
						openRunspaceOperation.Dispose();
					}
				}
				this.stream.Dispose();
			}
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000DADAC File Offset: 0x000D8FAC
		private void HandleThrottleComplete(object sender, EventArgs eventArgs)
		{
			this.stream.ObjectWriter.Close();
			this.operationsComplete.Set();
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000DADE0 File Offset: 0x000D8FE0
		private void WriteErrorCreateRemoteRunspaceFailed(Exception e, Uri uri)
		{
			PipelineWriter objectWriter = this.stream.ObjectWriter;
			ErrorRecord errorRecord = new ErrorRecord(e, "CreateRemoteRunspaceFailed", ErrorCategory.InvalidArgument, uri);
			Action<Cmdlet> obj = delegate(Cmdlet cmdlet)
			{
				cmdlet.WriteError(errorRecord);
			};
			objectWriter.Write(obj);
		}

		// Token: 0x04001341 RID: 4929
		private string[] computerNames;

		// Token: 0x04001342 RID: 4930
		private PSSession[] remoteRunspaceInfos;

		// Token: 0x04001343 RID: 4931
		private string[] names;

		// Token: 0x04001344 RID: 4932
		private SwitchParameter enableNetworkAccess;

		// Token: 0x04001345 RID: 4933
		private ThrottleManager throttleManager = new ThrottleManager();

		// Token: 0x04001346 RID: 4934
		private ObjectStream stream = new ObjectStream();

		// Token: 0x04001347 RID: 4935
		private ManualResetEvent operationsComplete = new ManualResetEvent(true);

		// Token: 0x04001348 RID: 4936
		private List<RemoteRunspace> toDispose = new List<RemoteRunspace>();

		// Token: 0x04001349 RID: 4937
		private Collection<List<IThrottleOperation>> allOperations = new Collection<List<IThrottleOperation>>();

		// Token: 0x0400134A RID: 4938
		private string _defaultFQEID = "PSSessionOpenFailed";
	}
}
