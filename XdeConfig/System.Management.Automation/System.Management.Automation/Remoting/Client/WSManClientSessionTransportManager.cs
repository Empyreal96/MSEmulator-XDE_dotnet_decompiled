using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Server;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Management.Automation.Tracing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Xml;

namespace System.Management.Automation.Remoting.Client
{
	// Token: 0x02000390 RID: 912
	internal sealed class WSManClientSessionTransportManager : BaseClientSessionTransportManager
	{
		// Token: 0x06002C0D RID: 11277 RVA: 0x000F3CE0 File Offset: 0x000F1EE0
		private void ProcessShellData(string data)
		{
			try
			{
				XmlReaderSettings xmlReaderSettings = InternalDeserializer.XmlReaderSettingsForUntrustedXmlDocument.Clone();
				xmlReaderSettings.MaxCharactersFromEntities = 1024L;
				xmlReaderSettings.MaxCharactersInDocument = 30720L;
				xmlReaderSettings.DtdProcessing = DtdProcessing.Prohibit;
				using (XmlReader xmlReader = XmlReader.Create(new StringReader(data), xmlReaderSettings))
				{
					while (xmlReader.Read())
					{
						if (xmlReader.NodeType == XmlNodeType.Element)
						{
							if (xmlReader.LocalName.Equals("IdleTimeOut", StringComparison.OrdinalIgnoreCase) || xmlReader.LocalName.Equals("MaxIdleTimeOut", StringComparison.OrdinalIgnoreCase))
							{
								bool flag = !xmlReader.LocalName.Equals("MaxIdleTimeOut", StringComparison.OrdinalIgnoreCase);
								string text = xmlReader.ReadElementContentAsString();
								int num = text.IndexOf('.');
								try
								{
									int num2 = Convert.ToInt32(text.Substring(2, num - 2), NumberFormatInfo.InvariantInfo) * 1000 + Convert.ToInt32(text.Substring(num + 1, 3), NumberFormatInfo.InvariantInfo);
									if (flag)
									{
										this._connectionInfo.IdleTimeout = num2;
									}
									else
									{
										this._connectionInfo.MaxIdleTimeout = num2;
									}
									continue;
								}
								catch (InvalidCastException)
								{
									continue;
								}
							}
							if (xmlReader.LocalName.Equals("BufferMode", StringComparison.OrdinalIgnoreCase))
							{
								string text2 = xmlReader.ReadElementContentAsString();
								if (text2.Equals("Block", StringComparison.OrdinalIgnoreCase))
								{
									this._connectionInfo.OutputBufferingMode = OutputBufferingMode.Block;
								}
								else if (text2.Equals("Drop", StringComparison.OrdinalIgnoreCase))
								{
									this._connectionInfo.OutputBufferingMode = OutputBufferingMode.Drop;
								}
							}
						}
					}
				}
			}
			catch (XmlException)
			{
			}
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000F3E94 File Offset: 0x000F2094
		private static long GetNextSessionTMHandleId()
		{
			return Interlocked.Increment(ref WSManClientSessionTransportManager.SessionTMSeed);
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000F3EA0 File Offset: 0x000F20A0
		private static void AddSessionTransportManager(long sessnTMId, WSManClientSessionTransportManager sessnTransportManager)
		{
			lock (WSManClientSessionTransportManager.SessionTMHandles)
			{
				WSManClientSessionTransportManager.SessionTMHandles.Add(sessnTMId, sessnTransportManager);
			}
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x000F3EE8 File Offset: 0x000F20E8
		private static void RemoveSessionTransportManager(long sessnTMId)
		{
			lock (WSManClientSessionTransportManager.SessionTMHandles)
			{
				if (WSManClientSessionTransportManager.SessionTMHandles.ContainsKey(sessnTMId))
				{
					WSManClientSessionTransportManager.SessionTMHandles[sessnTMId] = null;
					WSManClientSessionTransportManager.SessionTMHandles.Remove(sessnTMId);
				}
			}
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x000F3F48 File Offset: 0x000F2148
		private static bool TryGetSessionTransportManager(IntPtr operationContext, out WSManClientSessionTransportManager sessnTransportManager, out long sessnTMId)
		{
			sessnTMId = operationContext.ToInt64();
			sessnTransportManager = null;
			bool result;
			lock (WSManClientSessionTransportManager.SessionTMHandles)
			{
				result = WSManClientSessionTransportManager.SessionTMHandles.TryGetValue(sessnTMId, out sessnTransportManager);
			}
			return result;
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000F3F9C File Offset: 0x000F219C
		static WSManClientSessionTransportManager()
		{
			WSManNativeApi.WSManShellCompletionFunction callback = new WSManNativeApi.WSManShellCompletionFunction(WSManClientSessionTransportManager.OnCreateSessionCallback);
			WSManClientSessionTransportManager.sessionCreateCallback = new WSManNativeApi.WSManShellAsyncCallback(callback);
			WSManNativeApi.WSManShellCompletionFunction callback2 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientSessionTransportManager.OnCloseSessionCompleted);
			WSManClientSessionTransportManager.sessionCloseCallback = new WSManNativeApi.WSManShellAsyncCallback(callback2);
			WSManNativeApi.WSManShellCompletionFunction callback3 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientSessionTransportManager.OnRemoteSessionDataReceived);
			WSManClientSessionTransportManager.sessionReceiveCallback = new WSManNativeApi.WSManShellAsyncCallback(callback3);
			WSManNativeApi.WSManShellCompletionFunction callback4 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientSessionTransportManager.OnRemoteSessionSendCompleted);
			WSManClientSessionTransportManager.sessionSendCallback = new WSManNativeApi.WSManShellAsyncCallback(callback4);
			WSManNativeApi.WSManShellCompletionFunction callback5 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientSessionTransportManager.OnRemoteSessionDisconnectCompleted);
			WSManClientSessionTransportManager.sessionDisconnectCallback = new WSManNativeApi.WSManShellAsyncCallback(callback5);
			WSManNativeApi.WSManShellCompletionFunction callback6 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientSessionTransportManager.OnRemoteSessionReconnectCompleted);
			WSManClientSessionTransportManager.sessionReconnectCallback = new WSManNativeApi.WSManShellAsyncCallback(callback6);
			WSManNativeApi.WSManShellCompletionFunction callback7 = new WSManNativeApi.WSManShellCompletionFunction(WSManClientSessionTransportManager.OnRemoteSessionConnectCallback);
			WSManClientSessionTransportManager.sessionConnectCallback = new WSManNativeApi.WSManShellAsyncCallback(callback7);
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x000F4070 File Offset: 0x000F2270
		internal WSManClientSessionTransportManager(Guid runspacePoolInstanceId, WSManConnectionInfo connectionInfo, PSRemotingCryptoHelper cryptoHelper, string sessionName) : base(runspacePoolInstanceId, cryptoHelper)
		{
			this.wsManApiData = new WSManClientSessionTransportManager.WSManAPIDataCommon();
			if (this.wsManApiData.WSManAPIHandle == IntPtr.Zero)
			{
				throw new PSRemotingTransportException(StringUtil.Format(RemotingErrorIdStrings.WSManInitFailed, this.wsManApiData.ErrorCode));
			}
			base.CryptoHelper = cryptoHelper;
			this.dataToBeSent.Fragmentor = base.Fragmentor;
			this.sessionName = sessionName;
			base.ReceivedDataCollection.MaximumReceivedDataSize = null;
			base.ReceivedDataCollection.MaximumReceivedObjectSize = connectionInfo.MaximumReceivedObjectSize;
			this.onDataAvailableToSendCallback = new PrioritySendDataCollection.OnDataAvailableCallback(this.OnDataAvailableCallback);
			this.Initialize(connectionInfo.ConnectionUri, connectionInfo);
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x000F4134 File Offset: 0x000F2334
		internal void SetDefaultTimeOut(int milliseconds)
		{
			using (BaseClientTransportManager.tracer.TraceMethod("Setting Default timeout: {0} milliseconds", new object[]
			{
				milliseconds
			}))
			{
				int num = WSManNativeApi.WSManSetSessionOption(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_DEFAULT_OPERATION_TIMEOUTMS, new WSManNativeApi.WSManDataDWord(milliseconds));
				if (num != 0)
				{
					string message = WSManNativeApi.WSManGetErrorMessage(this.wsManApiData.WSManAPIHandle, num);
					PSInvalidOperationException ex = new PSInvalidOperationException(message);
					throw ex;
				}
			}
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000F41B4 File Offset: 0x000F23B4
		internal void SetConnectTimeOut(int milliseconds)
		{
			using (BaseClientTransportManager.tracer.TraceMethod("Setting CreateShell timeout: {0} milliseconds", new object[]
			{
				milliseconds
			}))
			{
				int num = WSManNativeApi.WSManSetSessionOption(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_TIMEOUTMS_CREATE_SHELL, new WSManNativeApi.WSManDataDWord(milliseconds));
				if (num != 0)
				{
					string message = WSManNativeApi.WSManGetErrorMessage(this.wsManApiData.WSManAPIHandle, num);
					PSInvalidOperationException ex = new PSInvalidOperationException(message);
					throw ex;
				}
			}
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000F4234 File Offset: 0x000F2434
		internal void SetCloseTimeOut(int milliseconds)
		{
			using (BaseClientTransportManager.tracer.TraceMethod("Setting CloseShell timeout: {0} milliseconds", new object[]
			{
				milliseconds
			}))
			{
				int num = WSManNativeApi.WSManSetSessionOption(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_TIMEOUTMS_CLOSE_SHELL_OPERATION, new WSManNativeApi.WSManDataDWord(milliseconds));
				if (num != 0)
				{
					string message = WSManNativeApi.WSManGetErrorMessage(this.wsManApiData.WSManAPIHandle, num);
					PSInvalidOperationException ex = new PSInvalidOperationException(message);
					throw ex;
				}
			}
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000F42B4 File Offset: 0x000F24B4
		internal void SetSendTimeOut(int milliseconds)
		{
			using (BaseClientTransportManager.tracer.TraceMethod("Setting SendShellInput timeout: {0} milliseconds", new object[]
			{
				milliseconds
			}))
			{
				int num = WSManNativeApi.WSManSetSessionOption(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_TIMEOUTMS_SEND_SHELL_INPUT, new WSManNativeApi.WSManDataDWord(milliseconds));
				if (num != 0)
				{
					string message = WSManNativeApi.WSManGetErrorMessage(this.wsManApiData.WSManAPIHandle, num);
					PSInvalidOperationException ex = new PSInvalidOperationException(message);
					throw ex;
				}
			}
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x000F4334 File Offset: 0x000F2534
		internal void SetReceiveTimeOut(int milliseconds)
		{
			using (BaseClientTransportManager.tracer.TraceMethod("Setting ReceiveShellOutput timeout: {0} milliseconds", new object[]
			{
				milliseconds
			}))
			{
				int num = WSManNativeApi.WSManSetSessionOption(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_TIMEOUTMS_RECEIVE_SHELL_OUTPUT, new WSManNativeApi.WSManDataDWord(milliseconds));
				if (num != 0)
				{
					string message = WSManNativeApi.WSManGetErrorMessage(this.wsManApiData.WSManAPIHandle, num);
					PSInvalidOperationException ex = new PSInvalidOperationException(message);
					throw ex;
				}
			}
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x000F43B4 File Offset: 0x000F25B4
		internal void SetSignalTimeOut(int milliseconds)
		{
			using (BaseClientTransportManager.tracer.TraceMethod("Setting SignalShell timeout: {0} milliseconds", new object[]
			{
				milliseconds
			}))
			{
				int num = WSManNativeApi.WSManSetSessionOption(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_TIMEOUTMS_SIGNAL_SHELL, new WSManNativeApi.WSManDataDWord(milliseconds));
				if (num != 0)
				{
					string message = WSManNativeApi.WSManGetErrorMessage(this.wsManApiData.WSManAPIHandle, num);
					PSInvalidOperationException ex = new PSInvalidOperationException(message);
					throw ex;
				}
			}
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000F4434 File Offset: 0x000F2634
		internal void SetWSManSessionOption(WSManNativeApi.WSManSessionOption option, int dwordData)
		{
			int num = WSManNativeApi.WSManSetSessionOption(this.wsManSessionHandle, option, new WSManNativeApi.WSManDataDWord(dwordData));
			if (num != 0)
			{
				string message = WSManNativeApi.WSManGetErrorMessage(this.wsManApiData.WSManAPIHandle, num);
				PSInvalidOperationException ex = new PSInvalidOperationException(message);
				throw ex;
			}
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000F4474 File Offset: 0x000F2674
		internal void SetWSManSessionOption(WSManNativeApi.WSManSessionOption option, string stringData)
		{
			using (WSManNativeApi.WSManData_ManToUn wsmanData_ManToUn = new WSManNativeApi.WSManData_ManToUn(stringData))
			{
				int num = WSManNativeApi.WSManSetSessionOption(this.wsManSessionHandle, option, wsmanData_ManToUn);
				if (num != 0)
				{
					string message = WSManNativeApi.WSManGetErrorMessage(this.wsManApiData.WSManAPIHandle, num);
					PSInvalidOperationException ex = new PSInvalidOperationException(message);
					throw ex;
				}
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06002C1C RID: 11292 RVA: 0x000F44D8 File Offset: 0x000F26D8
		internal WSManClientSessionTransportManager.WSManAPIDataCommon WSManAPIData
		{
			get
			{
				return this.wsManApiData;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06002C1D RID: 11293 RVA: 0x000F44E0 File Offset: 0x000F26E0
		internal bool SupportsDisconnect
		{
			get
			{
				return this.supportsDisconnect;
			}
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000F44E8 File Offset: 0x000F26E8
		internal override void DisconnectAsync()
		{
			uint serverIdleTimeOut = (uint)((this._connectionInfo.IdleTimeout > 0) ? this._connectionInfo.IdleTimeout : -1);
			WSManNativeApi.WSManShellDisconnectInfo disconnectInfo = new WSManNativeApi.WSManShellDisconnectInfo(serverIdleTimeOut);
			this.disconnectSessionCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.sessionContextID), WSManClientSessionTransportManager.sessionDisconnectCallback);
			try
			{
				lock (this.syncObject)
				{
					if (!this.isClosed)
					{
						int num = 0;
						num |= ((this._connectionInfo.OutputBufferingMode == OutputBufferingMode.Block) ? 8 : 0);
						num |= ((this._connectionInfo.OutputBufferingMode == OutputBufferingMode.Drop) ? 4 : 0);
						WSManNativeApi.WSManDisconnectShellEx(this.wsManShellOperationHandle, num, disconnectInfo, this.disconnectSessionCompleted);
					}
				}
			}
			finally
			{
				disconnectInfo.Dispose();
			}
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000F45D0 File Offset: 0x000F27D0
		internal override void ReconnectAsync()
		{
			base.ReceivedDataCollection.PrepareForStreamConnect();
			this.reconnectSessionCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.sessionContextID), WSManClientSessionTransportManager.sessionReconnectCallback);
			lock (this.syncObject)
			{
				if (!this.isClosed)
				{
					int num = 0;
					num |= ((this._connectionInfo.OutputBufferingMode == OutputBufferingMode.Block) ? 8 : 0);
					num |= ((this._connectionInfo.OutputBufferingMode == OutputBufferingMode.Drop) ? 4 : 0);
					WSManNativeApi.WSManReconnectShellEx(this.wsManShellOperationHandle, num, this.reconnectSessionCompleted);
				}
			}
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000F4680 File Offset: 0x000F2880
		internal override void ConnectAsync()
		{
			base.ReceivedDataCollection.PrepareForStreamConnect();
			if (this.openContent == null)
			{
				DataPriorityType dataPriorityType;
				byte[] array = this.dataToBeSent.ReadOrRegisterCallback(null, out dataPriorityType);
				if (array != null)
				{
					string data = string.Format(CultureInfo.InvariantCulture, "<{0} xmlns=\"{1}\">{2}</{0}>", new object[]
					{
						"connectXml",
						"http://schemas.microsoft.com/powershell",
						Convert.ToBase64String(array)
					});
					this.openContent = new WSManNativeApi.WSManData_ManToUn(data);
				}
				array = this.dataToBeSent.ReadOrRegisterCallback(null, out dataPriorityType);
				if (array != null)
				{
					return;
				}
			}
			this.sessionContextID = WSManClientSessionTransportManager.GetNextSessionTMHandleId();
			WSManClientSessionTransportManager.AddSessionTransportManager(this.sessionContextID, this);
			this.supportsDisconnect = true;
			this.connectSessionCallback = new WSManNativeApi.WSManShellAsync(new IntPtr(this.sessionContextID), WSManClientSessionTransportManager.sessionConnectCallback);
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					return;
				}
				this.startMode = WSManTransportManagerUtils.tmStartModes.Connect;
				int num = 0;
				num |= ((this._connectionInfo.OutputBufferingMode == OutputBufferingMode.Block) ? 8 : 0);
				num |= ((this._connectionInfo.OutputBufferingMode == OutputBufferingMode.Drop) ? 4 : 0);
				WSManNativeApi.WSManConnectShellEx(this.wsManSessionHandle, num, this._connectionInfo.ShellUri, base.RunspacePoolInstanceId.ToString().ToUpperInvariant(), IntPtr.Zero, this.openContent, this.connectSessionCallback, ref this.wsManShellOperationHandle);
			}
			if (this.wsManShellOperationHandle == IntPtr.Zero)
			{
				TransportErrorOccuredEventArgs eventArgs = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(this.wsManApiData.WSManAPIHandle, this, default(WSManNativeApi.WSManError), TransportMethodEnum.ConnectShellEx, RemotingErrorIdStrings.ConnectExFailed, new object[]
				{
					this.ConnectionInfo.ComputerName
				});
				this.ProcessWSManTransportError(eventArgs);
			}
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000F485C File Offset: 0x000F2A5C
		internal override void StartReceivingData()
		{
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
				}
				else if (this.receiveDataInitiated)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: ReceiveData has already been called.", new object[0]);
				}
				else
				{
					this.receiveDataInitiated = true;
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: Placing Receive request using WSManReceiveShellOutputEx", new object[0]);
					PSEtwLog.LogAnalyticInformational(PSEventId.WSManReceiveShellOutputEx, PSOpcode.Receive, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
					{
						base.RunspacePoolInstanceId.ToString(),
						Guid.Empty.ToString()
					});
					this.receivedFromRemote = new WSManNativeApi.WSManShellAsync(new IntPtr(this.sessionContextID), WSManClientSessionTransportManager.sessionReceiveCallback);
					WSManNativeApi.WSManReceiveShellOutputEx(this.wsManShellOperationHandle, IntPtr.Zero, 0, this.wsManApiData.OutputStreamSet, this.receivedFromRemote, ref this.wsManRecieveOperationHandle);
				}
			}
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x000F499C File Offset: 0x000F2B9C
		internal override void CreateAsync()
		{
			List<WSManNativeApi.WSManOption> list = new List<WSManNativeApi.WSManOption>(this.wsManApiData.CommonOptionSet);
			if (WSManClientSessionTransportManager.protocolVersionRedirect != null)
			{
				string value = (string)WSManClientSessionTransportManager.protocolVersionRedirect.DynamicInvoke(new object[0]);
				list.Clear();
				list.Add(new WSManNativeApi.WSManOption
				{
					name = "protocolversion",
					value = value,
					mustComply = true
				});
			}
			uint serverIdleTimeOut = (uint)((this._connectionInfo.IdleTimeout > 0) ? this._connectionInfo.IdleTimeout : -1);
			WSManNativeApi.WSManShellStartupInfo_ManToUn startupInfo = new WSManNativeApi.WSManShellStartupInfo_ManToUn(this.wsManApiData.InputStreamSet, this.wsManApiData.OutputStreamSet, serverIdleTimeOut, this.sessionName);
			if (this.openContent == null)
			{
				DataPriorityType dataPriorityType;
				byte[] array = this.dataToBeSent.ReadOrRegisterCallback(null, out dataPriorityType);
				bool flag = true;
				if (WSManClientSessionTransportManager.sessionSendRedirect != null)
				{
					object[] array2 = new object[]
					{
						null,
						array
					};
					flag = (bool)WSManClientSessionTransportManager.sessionSendRedirect.DynamicInvoke(array2);
					array = (byte[])array2[0];
				}
				if (!flag)
				{
					return;
				}
				if (array != null)
				{
					string data = string.Format(CultureInfo.InvariantCulture, "<{0} xmlns=\"{1}\">{2}</{0}>", new object[]
					{
						"creationXml",
						"http://schemas.microsoft.com/powershell",
						Convert.ToBase64String(array)
					});
					this.openContent = new WSManNativeApi.WSManData_ManToUn(data);
				}
			}
			if (this.sessionContextID == 0L)
			{
				this.sessionContextID = WSManClientSessionTransportManager.GetNextSessionTMHandleId();
				WSManClientSessionTransportManager.AddSessionTransportManager(this.sessionContextID, this);
				this.createSessionCallback = new WSManNativeApi.WSManShellAsync(new IntPtr(this.sessionContextID), WSManClientSessionTransportManager.sessionCreateCallback);
				this.createSessionCallbackGCHandle = GCHandle.Alloc(this.createSessionCallback);
			}
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManCreateShell, PSOpcode.Connect, PSTask.CreateRunspace, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString()
			});
			try
			{
				lock (this.syncObject)
				{
					if (this.isClosed)
					{
						return;
					}
					this.startMode = WSManTransportManagerUtils.tmStartModes.Create;
					if (this.noMachineProfile)
					{
						list.Add(new WSManNativeApi.WSManOption
						{
							name = "WINRS_NOPROFILE",
							mustComply = true,
							value = "1"
						});
					}
					int num = this.noCompression ? 1 : 0;
					num |= ((this._connectionInfo.OutputBufferingMode == OutputBufferingMode.Block) ? 8 : 0);
					num |= ((this._connectionInfo.OutputBufferingMode == OutputBufferingMode.Drop) ? 4 : 0);
					using (WSManNativeApi.WSManOptionSet optionSet = new WSManNativeApi.WSManOptionSet(list.ToArray()))
					{
						WSManNativeApi.WSManCreateShellEx(this.wsManSessionHandle, num, this._connectionInfo.ShellUri, base.RunspacePoolInstanceId.ToString().ToUpperInvariant(), startupInfo, optionSet, this.openContent, this.createSessionCallback, ref this.wsManShellOperationHandle);
					}
				}
				if (this.wsManShellOperationHandle == IntPtr.Zero)
				{
					TransportErrorOccuredEventArgs eventArgs = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(this.wsManApiData.WSManAPIHandle, this, default(WSManNativeApi.WSManError), TransportMethodEnum.CreateShellEx, RemotingErrorIdStrings.ConnectExFailed, new object[]
					{
						this.ConnectionInfo.ComputerName
					});
					this.ProcessWSManTransportError(eventArgs);
				}
			}
			finally
			{
				startupInfo.Dispose();
			}
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000F4D34 File Offset: 0x000F2F34
		internal override void CloseAsync()
		{
			bool flag = false;
			lock (this.syncObject)
			{
				if (this.isClosed)
				{
					return;
				}
				if (this.startMode == WSManTransportManagerUtils.tmStartModes.None)
				{
					flag = true;
				}
				else if ((this.startMode == WSManTransportManagerUtils.tmStartModes.Create || this.startMode == WSManTransportManagerUtils.tmStartModes.Connect) && IntPtr.Zero == this.wsManShellOperationHandle)
				{
					flag = true;
				}
				this.isClosed = true;
			}
			base.CloseAsync();
			if (!flag)
			{
				PSEtwLog.LogAnalyticInformational(PSEventId.WSManCloseShell, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					base.RunspacePoolInstanceId.ToString()
				});
				this.closeSessionCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.sessionContextID), WSManClientSessionTransportManager.sessionCloseCallback);
				WSManNativeApi.WSManCloseShell(this.wsManShellOperationHandle, 0, this.closeSessionCompleted);
				return;
			}
			try
			{
				base.RaiseCloseCompleted();
			}
			finally
			{
				WSManClientSessionTransportManager.RemoveSessionTransportManager(this.sessionContextID);
			}
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x000F4E48 File Offset: 0x000F3048
		internal void AdjustForProtocolVariations(Version serverProtocolVersion)
		{
			if (serverProtocolVersion <= RemotingConstants.ProtocolVersionWin7RTM)
			{
				int num;
				WSManNativeApi.WSManGetSessionOptionAsDword(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_MAX_ENVELOPE_SIZE_KB, out num);
				if (num == 500)
				{
					int num2 = WSManNativeApi.WSManSetSessionOption(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_MAX_ENVELOPE_SIZE_KB, new WSManNativeApi.WSManDataDWord(150));
					if (num2 != 0)
					{
						string message = WSManNativeApi.WSManGetErrorMessage(this.wsManApiData.WSManAPIHandle, num2);
						PSInvalidOperationException ex = new PSInvalidOperationException(message);
						throw ex;
					}
					int num3;
					WSManNativeApi.WSManGetSessionOptionAsDword(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_SHELL_MAX_DATA_SIZE_PER_MESSAGE_KB, out num3);
					base.Fragmentor.FragmentSize = num3 << 10;
				}
			}
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x000F4ECE File Offset: 0x000F30CE
		internal override void PrepareForRedirection()
		{
			this.closeSessionCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.sessionContextID), WSManClientSessionTransportManager.sessionCloseCallback);
			WSManNativeApi.WSManCloseShell(this.wsManShellOperationHandle, 0, this.closeSessionCompleted);
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000F4F04 File Offset: 0x000F3104
		internal override void Redirect(Uri newUri, RunspaceConnectionInfo connectionInfo)
		{
			this.CloseSessionAndClearResources();
			BaseClientTransportManager.tracer.WriteLine("Redirecting to URI: {0}", new object[]
			{
				newUri
			});
			PSEtwLog.LogAnalyticInformational(PSEventId.URIRedirection, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				newUri.ToString()
			});
			this.Initialize(newUri, (WSManConnectionInfo)connectionInfo);
			this.startMode = WSManTransportManagerUtils.tmStartModes.None;
			this.CreateAsync();
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x000F4F8C File Offset: 0x000F318C
		internal override BaseClientCommandTransportManager CreateClientCommandTransportManager(RunspaceConnectionInfo connectionInfo, ClientRemotePowerShell cmd, bool noInput)
		{
			WSManConnectionInfo connectionInfo2 = connectionInfo as WSManConnectionInfo;
			return new WSManClientCommandTransportManager(connectionInfo2, this.wsManShellOperationHandle, cmd, noInput, this);
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x000F4FB4 File Offset: 0x000F31B4
		private void Initialize(Uri connectionUri, WSManConnectionInfo connectionInfo)
		{
			this._connectionInfo = connectionInfo;
			bool flag = false;
			string text = connectionUri.OriginalString;
			if (connectionUri == connectionInfo.ConnectionUri && connectionInfo.UseDefaultWSManPort)
			{
				text = WSManConnectionInfo.GetConnectionString(connectionInfo.ConnectionUri, out flag);
			}
			string text2 = string.Empty;
			if (PSSessionConfigurationData.IsServerManager)
			{
				text2 = ";MSP=7a83d074-bb86-4e52-aa3e-6cc73cc066c8";
			}
			if (string.IsNullOrEmpty(connectionUri.Query))
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0}?PSVersion={1}{2}", new object[]
				{
					text.TrimEnd(new char[]
					{
						'/'
					}),
					PSVersionInfo.PSVersion,
					text2
				});
			}
			else
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0};PSVersion={1}{2}", new object[]
				{
					text,
					PSVersionInfo.PSVersion,
					text2
				});
			}
			WSManNativeApi.BaseWSManAuthenticationCredentials baseWSManAuthenticationCredentials;
			if (connectionInfo.CertificateThumbprint != null)
			{
				baseWSManAuthenticationCredentials = new WSManNativeApi.WSManCertificateThumbprintCredentials(connectionInfo.CertificateThumbprint);
			}
			else
			{
				string name = null;
				SecureString pwd = null;
				if (connectionInfo.Credential != null && !string.IsNullOrEmpty(connectionInfo.Credential.UserName))
				{
					name = connectionInfo.Credential.UserName;
					pwd = connectionInfo.Credential.Password;
				}
				WSManNativeApi.WSManUserNameAuthenticationCredentials wsmanUserNameAuthenticationCredentials = new WSManNativeApi.WSManUserNameAuthenticationCredentials(name, pwd, connectionInfo.WSManAuthenticationMechanism);
				baseWSManAuthenticationCredentials = wsmanUserNameAuthenticationCredentials;
			}
			WSManNativeApi.WSManUserNameAuthenticationCredentials wsmanUserNameAuthenticationCredentials2 = null;
			if (connectionInfo.ProxyCredential != null)
			{
				WSManNativeApi.WSManAuthenticationMechanism authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_NEGOTIATE;
				string name2 = null;
				SecureString pwd2 = null;
				switch (connectionInfo.ProxyAuthentication)
				{
				case AuthenticationMechanism.Basic:
					authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_BASIC;
					break;
				case AuthenticationMechanism.Negotiate:
					authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_NEGOTIATE;
					break;
				case AuthenticationMechanism.Digest:
					authMechanism = WSManNativeApi.WSManAuthenticationMechanism.WSMAN_FLAG_AUTH_DIGEST;
					break;
				}
				if (!string.IsNullOrEmpty(connectionInfo.ProxyCredential.UserName))
				{
					name2 = connectionInfo.ProxyCredential.UserName;
					pwd2 = connectionInfo.ProxyCredential.Password;
				}
				wsmanUserNameAuthenticationCredentials2 = new WSManNativeApi.WSManUserNameAuthenticationCredentials(name2, pwd2, authMechanism);
			}
			WSManNativeApi.WSManProxyInfo wsmanProxyInfo = (connectionInfo.ProxyAccessType == ProxyAccessType.None) ? null : new WSManNativeApi.WSManProxyInfo(connectionInfo.ProxyAccessType, wsmanUserNameAuthenticationCredentials2);
			int num = 0;
			try
			{
				num = WSManNativeApi.WSManCreateSession(this.wsManApiData.WSManAPIHandle, text, 0, baseWSManAuthenticationCredentials.GetMarshalledObject(), (wsmanProxyInfo == null) ? IntPtr.Zero : wsmanProxyInfo, ref this.wsManSessionHandle);
			}
			finally
			{
				if (wsmanUserNameAuthenticationCredentials2 != null)
				{
					wsmanUserNameAuthenticationCredentials2.Dispose();
				}
				if (wsmanProxyInfo != null)
				{
					wsmanProxyInfo.Dispose();
				}
				if (baseWSManAuthenticationCredentials != null)
				{
					baseWSManAuthenticationCredentials.Dispose();
				}
			}
			if (num != 0)
			{
				string message = WSManNativeApi.WSManGetErrorMessage(this.wsManApiData.WSManAPIHandle, num);
				PSInvalidOperationException ex = new PSInvalidOperationException(message);
				throw ex;
			}
			int num2;
			WSManNativeApi.WSManGetSessionOptionAsDword(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_SHELL_MAX_DATA_SIZE_PER_MESSAGE_KB, out num2);
			base.Fragmentor.FragmentSize = num2 << 10;
			WSManNativeApi.WSManGetSessionOptionAsDword(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_MAX_RETRY_TIME, out this.maxRetryTime);
			this.dataToBeSent.Fragmentor = base.Fragmentor;
			this.noCompression = !connectionInfo.UseCompression;
			this.noMachineProfile = connectionInfo.NoMachineProfile;
			if (flag)
			{
				this.SetWSManSessionOption(WSManNativeApi.WSManSessionOption.WSMAN_OPTION_USE_SSL, 1);
			}
			if (connectionInfo.NoEncryption)
			{
				this.SetWSManSessionOption(WSManNativeApi.WSManSessionOption.WSMAN_OPTION_UNENCRYPTED_MESSAGES, 1);
			}
			if (connectionInfo.AllowImplicitCredentialForNegotiate)
			{
				num = WSManNativeApi.WSManSetSessionOption(this.wsManSessionHandle, WSManNativeApi.WSManSessionOption.WSMAN_OPTION_ALLOW_NEGOTIATE_IMPLICIT_CREDENTIALS, new WSManNativeApi.WSManDataDWord(1));
			}
			if (connectionInfo.UseUTF16)
			{
				this.SetWSManSessionOption(WSManNativeApi.WSManSessionOption.WSMAN_OPTION_UTF16, 1);
			}
			if (connectionInfo.SkipCACheck)
			{
				this.SetWSManSessionOption(WSManNativeApi.WSManSessionOption.WSMAN_OPTION_SKIP_CA_CHECK, 1);
			}
			if (connectionInfo.SkipCNCheck)
			{
				this.SetWSManSessionOption(WSManNativeApi.WSManSessionOption.WSMAN_OPTION_SKIP_CN_CHECK, 1);
			}
			if (connectionInfo.SkipRevocationCheck)
			{
				this.SetWSManSessionOption(WSManNativeApi.WSManSessionOption.WSMAN_OPTION_SKIP_REVOCATION_CHECK, 1);
			}
			if (connectionInfo.IncludePortInSPN)
			{
				this.SetWSManSessionOption(WSManNativeApi.WSManSessionOption.WSMAN_OPTION_ENABLE_SPN_SERVER_PORT, 1);
			}
			this.SetWSManSessionOption(WSManNativeApi.WSManSessionOption.WSMAN_OPTION_USE_INTERACTIVE_TOKEN, connectionInfo.EnableNetworkAccess ? 1 : 0);
			string name3 = connectionInfo.UICulture.Name;
			if (!string.IsNullOrEmpty(name3))
			{
				this.SetWSManSessionOption(WSManNativeApi.WSManSessionOption.WSMAN_OPTION_UI_LANGUAGE, name3);
			}
			string name4 = connectionInfo.Culture.Name;
			if (!string.IsNullOrEmpty(name4))
			{
				this.SetWSManSessionOption(WSManNativeApi.WSManSessionOption.WSMAN_OPTION_LOCALE, name4);
			}
			this.SetDefaultTimeOut(connectionInfo.OperationTimeout);
			this.SetConnectTimeOut(connectionInfo.OpenTimeout);
			this.SetCloseTimeOut(connectionInfo.CancelTimeout);
			this.SetSignalTimeOut(connectionInfo.CancelTimeout);
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x000F5388 File Offset: 0x000F3588
		internal void ProcessWSManTransportError(TransportErrorOccuredEventArgs eventArgs)
		{
			base.EnqueueAndStartProcessingThread(null, eventArgs, null);
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x000F5394 File Offset: 0x000F3594
		internal override void RaiseErrorHandler(TransportErrorOccuredEventArgs eventArgs)
		{
			string text;
			if (!string.IsNullOrEmpty(eventArgs.Exception.StackTrace))
			{
				text = eventArgs.Exception.StackTrace;
			}
			else if (eventArgs.Exception.InnerException != null && !string.IsNullOrEmpty(eventArgs.Exception.InnerException.StackTrace))
			{
				text = eventArgs.Exception.InnerException.StackTrace;
			}
			else
			{
				text = string.Empty;
			}
			PSEtwLog.LogOperationalError(PSEventId.TransportError, PSOpcode.Open, PSTask.None, PSKeyword.UseAlwaysOperational, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				Guid.Empty.ToString(),
				eventArgs.Exception.ErrorCode.ToString(CultureInfo.InvariantCulture),
				eventArgs.Exception.Message,
				text
			});
			PSEtwLog.LogAnalyticError(PSEventId.TransportError_Analytic, PSOpcode.Open, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				base.RunspacePoolInstanceId.ToString(),
				Guid.Empty.ToString(),
				eventArgs.Exception.ErrorCode.ToString(CultureInfo.InvariantCulture),
				eventArgs.Exception.Message,
				text
			});
			base.RaiseErrorHandler(eventArgs);
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000F5500 File Offset: 0x000F3700
		internal void ClearReceiveOrSendResources(int flags, bool shouldClearSend)
		{
			if (shouldClearSend)
			{
				if (this.sendToRemoteCompleted != null)
				{
					this.sendToRemoteCompleted.Dispose();
					this.sendToRemoteCompleted = null;
				}
				if (IntPtr.Zero != this.wsManSendOperationHandle)
				{
					WSManNativeApi.WSManCloseOperation(this.wsManSendOperationHandle, 0);
					this.wsManSendOperationHandle = IntPtr.Zero;
					return;
				}
			}
			else if (flags == 1)
			{
				if (IntPtr.Zero != this.wsManRecieveOperationHandle)
				{
					WSManNativeApi.WSManCloseOperation(this.wsManRecieveOperationHandle, 0);
					this.wsManRecieveOperationHandle = IntPtr.Zero;
				}
				if (this.receivedFromRemote != null)
				{
					this.receivedFromRemote.Dispose();
					this.receivedFromRemote = null;
				}
			}
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000F559C File Offset: 0x000F379C
		internal override void ProcessPrivateData(object privateData)
		{
			ConnectionStatusEventArgs connectionStatusEventArgs = privateData as ConnectionStatusEventArgs;
			if (connectionStatusEventArgs != null)
			{
				base.RaiseRobustConnectionNotification(connectionStatusEventArgs);
				return;
			}
			WSManClientSessionTransportManager.CompletionEventArgs completionEventArgs = privateData as WSManClientSessionTransportManager.CompletionEventArgs;
			if (completionEventArgs != null)
			{
				WSManClientSessionTransportManager.CompletionNotification notification = completionEventArgs.Notification;
				if (notification != WSManClientSessionTransportManager.CompletionNotification.DisconnectCompleted)
				{
					return;
				}
				base.RaiseDisconnectCompleted();
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06002C2D RID: 11309 RVA: 0x000F55D7 File Offset: 0x000F37D7
		internal int MaxRetryConnectionTime
		{
			get
			{
				return this.maxRetryTime;
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06002C2E RID: 11310 RVA: 0x000F55DF File Offset: 0x000F37DF
		internal IntPtr SessionHandle
		{
			get
			{
				return this.wsManSessionHandle;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06002C2F RID: 11311 RVA: 0x000F55E7 File Offset: 0x000F37E7
		internal WSManConnectionInfo ConnectionInfo
		{
			get
			{
				return this._connectionInfo;
			}
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x000F55F0 File Offset: 0x000F37F0
		private bool RetrySessionCreation(int sessionCreateErrorCode)
		{
			if (this._connectionRetryCount >= this._connectionInfo.MaxConnectionRetryCount)
			{
				return false;
			}
			int num = sessionCreateErrorCode;
			if (num <= -2144108176)
			{
				if (num != -2144108526)
				{
					switch (num)
					{
					case -2144108270:
					case -2144108269:
						break;
					default:
						if (num != -2144108176)
						{
							goto IL_73;
						}
						break;
					}
				}
			}
			else if (num <= -2144108080)
			{
				if (num != -2144108090 && num != -2144108080)
				{
					goto IL_73;
				}
			}
			else if (num != 995 && num != 1722)
			{
				goto IL_73;
			}
			bool flag = true;
			goto IL_75;
			IL_73:
			flag = false;
			IL_75:
			if (flag)
			{
				this._connectionRetryCount++;
				string format = StringUtil.Format("Attempting session creation retry {0} for error code {1} on session Id {2}", new object[]
				{
					this._connectionRetryCount,
					sessionCreateErrorCode,
					base.RunspacePoolInstanceId
				});
				BaseClientTransportManager.tracer.WriteLine(format, new object[0]);
				PSEtwLog.LogOperationalInformation(PSEventId.RetrySessionCreation, PSOpcode.Open, PSTask.None, PSKeyword.UseAlwaysOperational, new object[]
				{
					this._connectionRetryCount.ToString(CultureInfo.InvariantCulture),
					sessionCreateErrorCode.ToString(CultureInfo.InvariantCulture),
					base.RunspacePoolInstanceId.ToString()
				});
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.StartCreateRetry));
			}
			return flag;
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000F573F File Offset: 0x000F393F
		private void StartCreateRetry(object state)
		{
			this.startMode = WSManTransportManagerUtils.tmStartModes.None;
			this.CreateAsync();
		}

		// Token: 0x06002C32 RID: 11314 RVA: 0x000F5750 File Offset: 0x000F3950
		private static void OnCreateSessionCallback(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("Client Session TM: CreateShell callback received", new object[0]);
			long num = 0L;
			WSManClientSessionTransportManager wsmanClientSessionTransportManager = null;
			if (!WSManClientSessionTransportManager.TryGetSessionTransportManager(operationContext, out wsmanClientSessionTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to find a transport manager for context {0}.", new object[]
				{
					num
				}), new object[0]);
				return;
			}
			if (WSManClientSessionTransportManager.HandleRobustConnectionCallback(flags, wsmanClientSessionTransportManager))
			{
				return;
			}
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManCreateShellCallbackReceived, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				wsmanClientSessionTransportManager.RunspacePoolInstanceId.ToString()
			});
			wsmanClientSessionTransportManager.wsManShellOperationHandle = shellOperationHandle;
			lock (wsmanClientSessionTransportManager.syncObject)
			{
				if (wsmanClientSessionTransportManager.isClosed)
				{
					return;
				}
			}
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Got error with error code {0}. Message {1}", new object[]
					{
						errorStruct.errorCode,
						errorStruct.errorDetail
					}), new object[0]);
					if (wsmanClientSessionTransportManager.RetrySessionCreation(errorStruct.errorCode))
					{
						return;
					}
					TransportErrorOccuredEventArgs eventArgs = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientSessionTransportManager.wsManApiData.WSManAPIHandle, wsmanClientSessionTransportManager, errorStruct, TransportMethodEnum.CreateShellEx, RemotingErrorIdStrings.ConnectExCallBackError, new object[]
					{
						wsmanClientSessionTransportManager.ConnectionInfo.ComputerName,
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientSessionTransportManager.ProcessWSManTransportError(eventArgs);
					return;
				}
			}
			wsmanClientSessionTransportManager.supportsDisconnect = ((flags & 32) != 0);
			if (wsmanClientSessionTransportManager.openContent != null)
			{
				wsmanClientSessionTransportManager.openContent.Dispose();
				wsmanClientSessionTransportManager.openContent = null;
			}
			if (data != IntPtr.Zero)
			{
				WSManNativeApi.WSManCreateShellDataResult wsmanCreateShellDataResult = WSManNativeApi.WSManCreateShellDataResult.UnMarshal(data);
				if (wsmanCreateShellDataResult.data != null)
				{
					string data2 = wsmanCreateShellDataResult.data;
					wsmanClientSessionTransportManager.ProcessShellData(data2);
				}
			}
			lock (wsmanClientSessionTransportManager.syncObject)
			{
				if (wsmanClientSessionTransportManager.isClosed)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
					return;
				}
				wsmanClientSessionTransportManager.RaiseCreateCompleted(new CreateCompleteEventArgs(wsmanClientSessionTransportManager.ConnectionInfo.Copy()));
				wsmanClientSessionTransportManager.StartReceivingData();
			}
			wsmanClientSessionTransportManager.SendOneItem();
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x000F59CC File Offset: 0x000F3BCC
		private static void OnCloseSessionCompleted(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("Client Session TM: CloseShell callback received", new object[0]);
			long num = 0L;
			WSManClientSessionTransportManager wsmanClientSessionTransportManager = null;
			if (!WSManClientSessionTransportManager.TryGetSessionTransportManager(operationContext, out wsmanClientSessionTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to find a transport manager for context {0}.", new object[]
				{
					num
				}), new object[0]);
				return;
			}
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManCloseShellCallbackReceived, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				wsmanClientSessionTransportManager.RunspacePoolInstanceId.ToString()
			});
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Got error with error code {0}. Message {1}", new object[]
					{
						errorStruct.errorCode,
						errorStruct.errorDetail
					}), new object[0]);
					TransportErrorOccuredEventArgs eventArgs = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientSessionTransportManager.wsManApiData.WSManAPIHandle, wsmanClientSessionTransportManager, errorStruct, TransportMethodEnum.CloseShellOperationEx, RemotingErrorIdStrings.CloseExCallBackError, new object[]
					{
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientSessionTransportManager.RaiseErrorHandler(eventArgs);
					return;
				}
			}
			wsmanClientSessionTransportManager.RaiseCloseCompleted();
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x000F5B14 File Offset: 0x000F3D14
		private static void OnRemoteSessionDisconnectCompleted(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("Client Session TM: CreateShell callback received", new object[0]);
			long num = 0L;
			WSManClientSessionTransportManager wsmanClientSessionTransportManager = null;
			if (!WSManClientSessionTransportManager.TryGetSessionTransportManager(operationContext, out wsmanClientSessionTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to find a transport manager for context {0}.", new object[]
				{
					num
				}), new object[0]);
				return;
			}
			if (wsmanClientSessionTransportManager.disconnectSessionCompleted != null)
			{
				wsmanClientSessionTransportManager.disconnectSessionCompleted.Dispose();
				wsmanClientSessionTransportManager.disconnectSessionCompleted = null;
			}
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Got error with error code {0}. Message {1}", new object[]
					{
						errorStruct.errorCode,
						errorStruct.errorDetail
					}), new object[0]);
					TransportErrorOccuredEventArgs eventArgs = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientSessionTransportManager.wsManApiData.WSManAPIHandle, wsmanClientSessionTransportManager, errorStruct, TransportMethodEnum.DisconnectShellEx, RemotingErrorIdStrings.DisconnectShellExFailed, new object[]
					{
						wsmanClientSessionTransportManager.ConnectionInfo.ComputerName,
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientSessionTransportManager.ProcessWSManTransportError(eventArgs);
					return;
				}
			}
			lock (wsmanClientSessionTransportManager.syncObject)
			{
				if (wsmanClientSessionTransportManager.isClosed)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
				}
				else
				{
					wsmanClientSessionTransportManager.EnqueueAndStartProcessingThread(null, null, new WSManClientSessionTransportManager.CompletionEventArgs(WSManClientSessionTransportManager.CompletionNotification.DisconnectCompleted));
				}
			}
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x000F5CA4 File Offset: 0x000F3EA4
		private static void OnRemoteSessionReconnectCompleted(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("Client Session TM: CreateShell callback received", new object[0]);
			long num = 0L;
			WSManClientSessionTransportManager wsmanClientSessionTransportManager = null;
			if (!WSManClientSessionTransportManager.TryGetSessionTransportManager(operationContext, out wsmanClientSessionTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to find a transport manager for context {0}.", new object[]
				{
					num
				}), new object[0]);
				return;
			}
			if (wsmanClientSessionTransportManager.reconnectSessionCompleted != null)
			{
				wsmanClientSessionTransportManager.reconnectSessionCompleted.Dispose();
				wsmanClientSessionTransportManager.reconnectSessionCompleted = null;
			}
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Got error with error code {0}. Message {1}", new object[]
					{
						errorStruct.errorCode,
						errorStruct.errorDetail
					}), new object[0]);
					TransportErrorOccuredEventArgs eventArgs = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientSessionTransportManager.wsManApiData.WSManAPIHandle, wsmanClientSessionTransportManager, errorStruct, TransportMethodEnum.ReconnectShellEx, RemotingErrorIdStrings.ReconnectShellExCallBackErrr, new object[]
					{
						wsmanClientSessionTransportManager.ConnectionInfo.ComputerName,
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientSessionTransportManager.ProcessWSManTransportError(eventArgs);
					return;
				}
			}
			lock (wsmanClientSessionTransportManager.syncObject)
			{
				if (wsmanClientSessionTransportManager.isClosed)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
				}
				else
				{
					wsmanClientSessionTransportManager.RaiseReconnectCompleted();
				}
			}
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x000F5E2C File Offset: 0x000F402C
		private static bool HandleRobustConnectionCallback(int flags, WSManClientSessionTransportManager sessionTM)
		{
			if (flags != 64 && flags != 256 && flags != 512 && flags != 1024 && flags != 2048 && flags != 4096)
			{
				return false;
			}
			if (flags == 256)
			{
				try
				{
					sessionTM.RobustConnectionsInitiated.SafeInvoke(sessionTM, EventArgs.Empty);
				}
				catch (ObjectDisposedException)
				{
				}
			}
			sessionTM.QueueRobustConnectionNotification(flags);
			if (flags != 64 && flags != 1024)
			{
				if (flags != 4096)
				{
					return true;
				}
			}
			try
			{
				sessionTM.RobustConnectionsCompleted.SafeInvoke(sessionTM, EventArgs.Empty);
			}
			catch (ObjectDisposedException)
			{
			}
			return true;
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x000F5ED8 File Offset: 0x000F40D8
		private static void OnRemoteSessionConnectCallback(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("Client Session TM: Connect callback received", new object[0]);
			long num = 0L;
			WSManClientSessionTransportManager wsmanClientSessionTransportManager = null;
			if (!WSManClientSessionTransportManager.TryGetSessionTransportManager(operationContext, out wsmanClientSessionTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to find a transport manager for context {0}.", new object[]
				{
					num
				}), new object[0]);
				return;
			}
			if (WSManClientSessionTransportManager.HandleRobustConnectionCallback(flags, wsmanClientSessionTransportManager))
			{
				return;
			}
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Got error with error code {0}. Message {1}", new object[]
					{
						errorStruct.errorCode,
						errorStruct.errorDetail
					}), new object[0]);
					TransportErrorOccuredEventArgs eventArgs = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientSessionTransportManager.wsManApiData.WSManAPIHandle, wsmanClientSessionTransportManager, errorStruct, TransportMethodEnum.ConnectShellEx, RemotingErrorIdStrings.ConnectExCallBackError, new object[]
					{
						wsmanClientSessionTransportManager.ConnectionInfo.ComputerName,
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientSessionTransportManager.ProcessWSManTransportError(eventArgs);
					return;
				}
			}
			if (wsmanClientSessionTransportManager.openContent != null)
			{
				wsmanClientSessionTransportManager.openContent.Dispose();
				wsmanClientSessionTransportManager.openContent = null;
			}
			lock (wsmanClientSessionTransportManager.syncObject)
			{
				if (wsmanClientSessionTransportManager.isClosed)
				{
					BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
					return;
				}
			}
			WSManNativeApi.WSManConnectDataResult wsmanConnectDataResult = WSManNativeApi.WSManConnectDataResult.UnMarshal(data);
			if (wsmanConnectDataResult.data != null)
			{
				byte[] data2 = ServerOperationHelpers.ExtractEncodedXmlElement(wsmanConnectDataResult.data, "connectResponseXml");
				wsmanClientSessionTransportManager.ProcessRawData(data2, "stdout");
			}
			wsmanClientSessionTransportManager.SendOneItem();
			wsmanClientSessionTransportManager.RaiseConnectCompleted();
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x000F60A4 File Offset: 0x000F42A4
		private static void OnRemoteSessionSendCompleted(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("Client Session TM: SendComplete callback received", new object[0]);
			long num = 0L;
			WSManClientSessionTransportManager wsmanClientSessionTransportManager = null;
			if (!WSManClientSessionTransportManager.TryGetSessionTransportManager(operationContext, out wsmanClientSessionTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to find a transport manager for context {0}.", new object[]
				{
					num
				}), new object[0]);
				return;
			}
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManSendShellInputExCallbackReceived, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
			{
				wsmanClientSessionTransportManager.RunspacePoolInstanceId.ToString(),
				Guid.Empty.ToString()
			});
			if (!shellOperationHandle.Equals(wsmanClientSessionTransportManager.wsManShellOperationHandle))
			{
				PSRemotingTransportException e = new PSRemotingTransportException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.SendExFailed, new object[]
				{
					wsmanClientSessionTransportManager.ConnectionInfo.ComputerName
				}));
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.SendShellInputEx);
				wsmanClientSessionTransportManager.ProcessWSManTransportError(eventArgs);
				return;
			}
			wsmanClientSessionTransportManager.ClearReceiveOrSendResources(flags, true);
			if (wsmanClientSessionTransportManager.isClosed)
			{
				BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
				return;
			}
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0 && errorStruct.errorCode != 995)
				{
					BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Got error with error code {0}. Message {1}", new object[]
					{
						errorStruct.errorCode,
						errorStruct.errorDetail
					}), new object[0]);
					TransportErrorOccuredEventArgs eventArgs2 = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientSessionTransportManager.wsManApiData.WSManAPIHandle, wsmanClientSessionTransportManager, errorStruct, TransportMethodEnum.SendShellInputEx, RemotingErrorIdStrings.SendExCallBackError, new object[]
					{
						wsmanClientSessionTransportManager.ConnectionInfo.ComputerName,
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientSessionTransportManager.ProcessWSManTransportError(eventArgs2);
					return;
				}
			}
			wsmanClientSessionTransportManager.SendOneItem();
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x000F62A4 File Offset: 0x000F44A4
		private static void OnRemoteSessionDataReceived(IntPtr operationContext, int flags, IntPtr error, IntPtr shellOperationHandle, IntPtr commandOperationHandle, IntPtr operationHandle, IntPtr data)
		{
			BaseClientTransportManager.tracer.WriteLine("Client Session TM: OnRemoteDataReceived callback.", new object[0]);
			long num = 0L;
			WSManClientSessionTransportManager wsmanClientSessionTransportManager = null;
			if (!WSManClientSessionTransportManager.TryGetSessionTransportManager(operationContext, out wsmanClientSessionTransportManager, out num))
			{
				BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Unable to find a transport manager for context {0}.", new object[]
				{
					num
				}), new object[0]);
				return;
			}
			wsmanClientSessionTransportManager.ClearReceiveOrSendResources(flags, false);
			if (wsmanClientSessionTransportManager.isClosed)
			{
				BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
				return;
			}
			if (!shellOperationHandle.Equals(wsmanClientSessionTransportManager.wsManShellOperationHandle))
			{
				PSRemotingTransportException e = new PSRemotingTransportException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.ReceiveExFailed, new object[]
				{
					wsmanClientSessionTransportManager.ConnectionInfo.ComputerName
				}));
				TransportErrorOccuredEventArgs eventArgs = new TransportErrorOccuredEventArgs(e, TransportMethodEnum.ReceiveShellOutputEx);
				wsmanClientSessionTransportManager.ProcessWSManTransportError(eventArgs);
				return;
			}
			if (IntPtr.Zero != error)
			{
				WSManNativeApi.WSManError errorStruct = WSManNativeApi.WSManError.UnMarshal(error);
				if (errorStruct.errorCode != 0)
				{
					BaseClientTransportManager.tracer.WriteLine(string.Format(CultureInfo.InvariantCulture, "Got error with error code {0}. Message {1}", new object[]
					{
						errorStruct.errorCode,
						errorStruct.errorDetail
					}), new object[0]);
					TransportErrorOccuredEventArgs eventArgs2 = WSManTransportManagerUtils.ConstructTransportErrorEventArgs(wsmanClientSessionTransportManager.wsManApiData.WSManAPIHandle, wsmanClientSessionTransportManager, errorStruct, TransportMethodEnum.ReceiveShellOutputEx, RemotingErrorIdStrings.ReceiveExCallBackError, new object[]
					{
						wsmanClientSessionTransportManager.ConnectionInfo.ComputerName,
						WSManTransportManagerUtils.ParseEscapeWSManErrorMessage(errorStruct.errorDetail)
					});
					wsmanClientSessionTransportManager.ProcessWSManTransportError(eventArgs2);
					return;
				}
			}
			WSManNativeApi.WSManReceiveDataResult wsmanReceiveDataResult = WSManNativeApi.WSManReceiveDataResult.UnMarshal(data);
			if (wsmanReceiveDataResult.data != null)
			{
				BaseClientTransportManager.tracer.WriteLine("Session Received Data : {0}", new object[]
				{
					wsmanReceiveDataResult.data.Length
				});
				PSEtwLog.LogAnalyticInformational(PSEventId.WSManReceiveShellOutputExCallbackReceived, PSOpcode.Receive, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					wsmanClientSessionTransportManager.RunspacePoolInstanceId.ToString(),
					Guid.Empty.ToString(),
					wsmanReceiveDataResult.data.Length.ToString(CultureInfo.InvariantCulture)
				});
				wsmanClientSessionTransportManager.ProcessRawData(wsmanReceiveDataResult.data, wsmanReceiveDataResult.stream);
			}
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x000F64FC File Offset: 0x000F46FC
		private void SendOneItem()
		{
			DataPriorityType priorityType;
			byte[] array = this.dataToBeSent.ReadOrRegisterCallback(this.onDataAvailableToSendCallback, out priorityType);
			if (array != null)
			{
				this.SendData(array, priorityType);
			}
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x000F6528 File Offset: 0x000F4728
		private void OnDataAvailableCallback(byte[] data, DataPriorityType priorityType)
		{
			BaseClientTransportManager.tracer.WriteLine("Received data to be sent from the callback.", new object[0]);
			this.SendData(data, priorityType);
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x000F6548 File Offset: 0x000F4748
		private void SendData(byte[] data, DataPriorityType priorityType)
		{
			BaseClientTransportManager.tracer.WriteLine("Session sending data of size : {0}", new object[]
			{
				data.Length
			});
			byte[] array = data;
			bool flag = true;
			if (WSManClientSessionTransportManager.sessionSendRedirect != null)
			{
				object[] array2 = new object[]
				{
					null,
					array
				};
				flag = (bool)WSManClientSessionTransportManager.sessionSendRedirect.DynamicInvoke(array2);
				array = (byte[])array2[0];
			}
			if (!flag)
			{
				return;
			}
			using (WSManNativeApi.WSManData_ManToUn wsmanData_ManToUn = new WSManNativeApi.WSManData_ManToUn(array))
			{
				PSEtwLog.LogAnalyticInformational(PSEventId.WSManSendShellInputEx, PSOpcode.Send, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					base.RunspacePoolInstanceId.ToString(),
					Guid.Empty.ToString(),
					wsmanData_ManToUn.BufferLength.ToString(CultureInfo.InvariantCulture)
				});
				lock (this.syncObject)
				{
					if (this.isClosed)
					{
						BaseClientTransportManager.tracer.WriteLine("Client Session TM: Transport manager is closed. So returning", new object[0]);
					}
					else
					{
						this.sendToRemoteCompleted = new WSManNativeApi.WSManShellAsync(new IntPtr(this.sessionContextID), WSManClientSessionTransportManager.sessionSendCallback);
						WSManNativeApi.WSManSendShellInputEx(this.wsManShellOperationHandle, IntPtr.Zero, 0, (priorityType == DataPriorityType.Default) ? "stdin" : "pr", wsmanData_ManToUn, this.sendToRemoteCompleted, ref this.wsManSendOperationHandle);
					}
				}
			}
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x000F66D8 File Offset: 0x000F48D8
		internal override void Dispose(bool isDisposing)
		{
			BaseClientTransportManager.tracer.WriteLine("Disposing session with session context: {0} Operation Context: {1}", new object[]
			{
				this.sessionContextID,
				this.wsManShellOperationHandle
			});
			this.CloseSessionAndClearResources();
			this.DisposeWSManAPIDataAsync();
			if (isDisposing && this.openContent != null)
			{
				this.openContent.Dispose();
				this.openContent = null;
			}
			base.Dispose(isDisposing);
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x000F6770 File Offset: 0x000F4970
		private void CloseSessionAndClearResources()
		{
			BaseClientTransportManager.tracer.WriteLine("Clearing session with session context: {0} Operation Context: {1}", new object[]
			{
				this.sessionContextID,
				this.wsManShellOperationHandle
			});
			IntPtr intPtr = this.wsManSessionHandle;
			this.wsManSessionHandle = IntPtr.Zero;
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				IntPtr value = (IntPtr)state;
				if (value != IntPtr.Zero)
				{
					WSManNativeApi.WSManCloseSession(value, 0);
				}
			}, intPtr);
			WSManClientSessionTransportManager.RemoveSessionTransportManager(this.sessionContextID);
			if (this.closeSessionCompleted != null)
			{
				this.closeSessionCompleted.Dispose();
				this.closeSessionCompleted = null;
			}
			if (this.createSessionCallback != null)
			{
				this.createSessionCallbackGCHandle.Free();
				this.createSessionCallback.Dispose();
				this.createSessionCallback = null;
			}
			if (this.connectSessionCallback != null)
			{
				this.connectSessionCallback.Dispose();
				this.connectSessionCallback = null;
			}
			this.sessionContextID = 0L;
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x000F686C File Offset: 0x000F4A6C
		private void DisposeWSManAPIDataAsync()
		{
			WSManClientSessionTransportManager.WSManAPIDataCommon tempWSManApiData = this.wsManApiData;
			if (tempWSManApiData == null)
			{
				return;
			}
			this.wsManApiData = null;
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				tempWSManApiData.Dispose();
			});
		}

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06002C40 RID: 11328 RVA: 0x000F68B0 File Offset: 0x000F4AB0
		// (remove) Token: 0x06002C41 RID: 11329 RVA: 0x000F68E8 File Offset: 0x000F4AE8
		internal event EventHandler<EventArgs> RobustConnectionsInitiated;

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06002C42 RID: 11330 RVA: 0x000F6920 File Offset: 0x000F4B20
		// (remove) Token: 0x06002C43 RID: 11331 RVA: 0x000F6958 File Offset: 0x000F4B58
		internal event EventHandler<EventArgs> RobustConnectionsCompleted;

		// Token: 0x04001628 RID: 5672
		internal const string MAX_URI_REDIRECTION_COUNT_VARIABLE = "WSManMaxRedirectionCount";

		// Token: 0x04001629 RID: 5673
		internal const int MAX_URI_REDIRECTION_COUNT = 5;

		// Token: 0x0400162A RID: 5674
		private const string resBaseName = "remotingerroridstrings";

		// Token: 0x0400162B RID: 5675
		private IntPtr wsManSessionHandle;

		// Token: 0x0400162C RID: 5676
		private IntPtr wsManShellOperationHandle;

		// Token: 0x0400162D RID: 5677
		private IntPtr wsManRecieveOperationHandle;

		// Token: 0x0400162E RID: 5678
		private IntPtr wsManSendOperationHandle;

		// Token: 0x0400162F RID: 5679
		private long sessionContextID;

		// Token: 0x04001630 RID: 5680
		private WSManTransportManagerUtils.tmStartModes startMode = WSManTransportManagerUtils.tmStartModes.None;

		// Token: 0x04001631 RID: 5681
		private string sessionName;

		// Token: 0x04001632 RID: 5682
		private bool supportsDisconnect;

		// Token: 0x04001633 RID: 5683
		private PrioritySendDataCollection.OnDataAvailableCallback onDataAvailableToSendCallback;

		// Token: 0x04001634 RID: 5684
		private WSManNativeApi.WSManShellAsync createSessionCallback;

		// Token: 0x04001635 RID: 5685
		private WSManNativeApi.WSManShellAsync receivedFromRemote;

		// Token: 0x04001636 RID: 5686
		private WSManNativeApi.WSManShellAsync sendToRemoteCompleted;

		// Token: 0x04001637 RID: 5687
		private WSManNativeApi.WSManShellAsync disconnectSessionCompleted;

		// Token: 0x04001638 RID: 5688
		private WSManNativeApi.WSManShellAsync reconnectSessionCompleted;

		// Token: 0x04001639 RID: 5689
		private WSManNativeApi.WSManShellAsync connectSessionCallback;

		// Token: 0x0400163A RID: 5690
		private GCHandle createSessionCallbackGCHandle;

		// Token: 0x0400163B RID: 5691
		private WSManNativeApi.WSManShellAsync closeSessionCompleted;

		// Token: 0x0400163C RID: 5692
		private WSManNativeApi.WSManData_ManToUn openContent;

		// Token: 0x0400163D RID: 5693
		private bool noCompression;

		// Token: 0x0400163E RID: 5694
		private bool noMachineProfile;

		// Token: 0x0400163F RID: 5695
		private WSManConnectionInfo _connectionInfo;

		// Token: 0x04001640 RID: 5696
		private int _connectionRetryCount;

		// Token: 0x04001641 RID: 5697
		private int maxRetryTime;

		// Token: 0x04001642 RID: 5698
		private WSManClientSessionTransportManager.WSManAPIDataCommon wsManApiData;

		// Token: 0x04001643 RID: 5699
		private static WSManNativeApi.WSManShellAsyncCallback sessionCreateCallback;

		// Token: 0x04001644 RID: 5700
		private static WSManNativeApi.WSManShellAsyncCallback sessionCloseCallback;

		// Token: 0x04001645 RID: 5701
		private static WSManNativeApi.WSManShellAsyncCallback sessionReceiveCallback;

		// Token: 0x04001646 RID: 5702
		private static WSManNativeApi.WSManShellAsyncCallback sessionSendCallback;

		// Token: 0x04001647 RID: 5703
		private static WSManNativeApi.WSManShellAsyncCallback sessionDisconnectCallback;

		// Token: 0x04001648 RID: 5704
		private static WSManNativeApi.WSManShellAsyncCallback sessionReconnectCallback;

		// Token: 0x04001649 RID: 5705
		private static WSManNativeApi.WSManShellAsyncCallback sessionConnectCallback;

		// Token: 0x0400164A RID: 5706
		private static Dictionary<long, WSManClientSessionTransportManager> SessionTMHandles = new Dictionary<long, WSManClientSessionTransportManager>();

		// Token: 0x0400164B RID: 5707
		private static long SessionTMSeed;

		// Token: 0x0400164C RID: 5708
		private static Delegate sessionSendRedirect = null;

		// Token: 0x0400164D RID: 5709
		private static Delegate protocolVersionRedirect = null;

		// Token: 0x02000391 RID: 913
		private enum CompletionNotification
		{
			// Token: 0x04001652 RID: 5714
			DisconnectCompleted
		}

		// Token: 0x02000392 RID: 914
		private class CompletionEventArgs : EventArgs
		{
			// Token: 0x06002C45 RID: 11333 RVA: 0x000F698D File Offset: 0x000F4B8D
			internal CompletionEventArgs(WSManClientSessionTransportManager.CompletionNotification notification)
			{
				this._notification = notification;
			}

			// Token: 0x17000A7A RID: 2682
			// (get) Token: 0x06002C46 RID: 11334 RVA: 0x000F699C File Offset: 0x000F4B9C
			internal WSManClientSessionTransportManager.CompletionNotification Notification
			{
				get
				{
					return this._notification;
				}
			}

			// Token: 0x04001653 RID: 5715
			private WSManClientSessionTransportManager.CompletionNotification _notification;
		}

		// Token: 0x02000393 RID: 915
		internal class WSManAPIDataCommon : IDisposable
		{
			// Token: 0x06002C47 RID: 11335 RVA: 0x000F69A4 File Offset: 0x000F4BA4
			internal WSManAPIDataCommon()
			{
				this._identityToImpersonate = WindowsIdentity.GetCurrent();
				this._identityToImpersonate = ((this._identityToImpersonate.ImpersonationLevel == TokenImpersonationLevel.Impersonation) ? this._identityToImpersonate : null);
				this.handle = IntPtr.Zero;
				this.errorCode = WSManNativeApi.WSManInitialize(1, ref this.handle);
				this.inputStreamSet = new WSManNativeApi.WSManStreamIDSet_ManToUn(new string[]
				{
					"stdin",
					"pr"
				});
				this.outputStreamSet = new WSManNativeApi.WSManStreamIDSet_ManToUn(new string[]
				{
					"stdout"
				});
				WSManNativeApi.WSManOption item = default(WSManNativeApi.WSManOption);
				item.name = "protocolversion";
				item.value = RemotingConstants.ProtocolVersion.ToString();
				item.mustComply = true;
				this.commonOptionSet = new List<WSManNativeApi.WSManOption>();
				this.commonOptionSet.Add(item);
			}

			// Token: 0x17000A7B RID: 2683
			// (get) Token: 0x06002C48 RID: 11336 RVA: 0x000F6A89 File Offset: 0x000F4C89
			internal int ErrorCode
			{
				get
				{
					return this.errorCode;
				}
			}

			// Token: 0x17000A7C RID: 2684
			// (get) Token: 0x06002C49 RID: 11337 RVA: 0x000F6A91 File Offset: 0x000F4C91
			internal WSManNativeApi.WSManStreamIDSet_ManToUn InputStreamSet
			{
				get
				{
					return this.inputStreamSet;
				}
			}

			// Token: 0x17000A7D RID: 2685
			// (get) Token: 0x06002C4A RID: 11338 RVA: 0x000F6A99 File Offset: 0x000F4C99
			internal WSManNativeApi.WSManStreamIDSet_ManToUn OutputStreamSet
			{
				get
				{
					return this.outputStreamSet;
				}
			}

			// Token: 0x17000A7E RID: 2686
			// (get) Token: 0x06002C4B RID: 11339 RVA: 0x000F6AA1 File Offset: 0x000F4CA1
			internal List<WSManNativeApi.WSManOption> CommonOptionSet
			{
				get
				{
					return this.commonOptionSet;
				}
			}

			// Token: 0x17000A7F RID: 2687
			// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000F6AA9 File Offset: 0x000F4CA9
			internal IntPtr WSManAPIHandle
			{
				get
				{
					return this.handle;
				}
			}

			// Token: 0x06002C4D RID: 11341 RVA: 0x000F6AB4 File Offset: 0x000F4CB4
			public void Dispose()
			{
				lock (this.syncObject)
				{
					if (this.isDisposed)
					{
						return;
					}
					this.isDisposed = true;
				}
				this.inputStreamSet.Dispose();
				this.outputStreamSet.Dispose();
				if (IntPtr.Zero != this.handle)
				{
					WindowsImpersonationContext windowsImpersonationContext = (this._identityToImpersonate != null) ? this._identityToImpersonate.Impersonate() : null;
					try
					{
						WSManNativeApi.WSManDeinitialize(this.handle, 0);
					}
					finally
					{
						if (windowsImpersonationContext != null)
						{
							try
							{
								windowsImpersonationContext.Undo();
								windowsImpersonationContext.Dispose();
							}
							catch (SecurityException)
							{
							}
						}
					}
					this.handle = IntPtr.Zero;
				}
			}

			// Token: 0x04001654 RID: 5716
			private IntPtr handle;

			// Token: 0x04001655 RID: 5717
			private int errorCode;

			// Token: 0x04001656 RID: 5718
			private WSManNativeApi.WSManStreamIDSet_ManToUn inputStreamSet;

			// Token: 0x04001657 RID: 5719
			private WSManNativeApi.WSManStreamIDSet_ManToUn outputStreamSet;

			// Token: 0x04001658 RID: 5720
			private List<WSManNativeApi.WSManOption> commonOptionSet;

			// Token: 0x04001659 RID: 5721
			private bool isDisposed;

			// Token: 0x0400165A RID: 5722
			private object syncObject = new object();

			// Token: 0x0400165B RID: 5723
			private WindowsIdentity _identityToImpersonate;
		}
	}
}
