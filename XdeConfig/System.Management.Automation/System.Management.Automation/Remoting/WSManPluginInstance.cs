using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Remoting.Server;
using System.Management.Automation.Remoting.WSMan;
using System.Management.Automation.Tracing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020003DE RID: 990
	internal class WSManPluginInstance
	{
		// Token: 0x06002CF8 RID: 11512 RVA: 0x000F9B6F File Offset: 0x000F7D6F
		internal WSManPluginInstance()
		{
			this.activeShellSessions = new Dictionary<IntPtr, WSManPluginShellSession>();
			this.syncObject = new object();
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000F9B90 File Offset: 0x000F7D90
		static WSManPluginInstance()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += WSManPluginInstance.UnhandledExceptionHandler;
			WindowsErrorReporting.RegisterWindowsErrorReporting(true);
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x000F9BD0 File Offset: 0x000F7DD0
		internal void CreateShell(IntPtr pluginContext, WSManNativeApi.WSManPluginRequest requestDetails, int flags, string extraInfo, WSManNativeApi.WSManShellStartupInfo_UnToMan startupInfo, WSManNativeApi.WSManData_UnToMan inboundShellInformation)
		{
			if (requestDetails == null)
			{
				PSEtwLog.LogAnalyticInformational(PSEventId.ReportOperationComplete, PSOpcode.Close, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
				{
					"null",
					Convert.ToString(WSManPluginErrorCodes.NullInvalidInput, CultureInfo.InvariantCulture),
					StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullInvalidInput, "requestDetails", "WSManPluginShell"),
					string.Empty
				});
				return;
			}
			if (requestDetails.senderDetails == null || requestDetails.operationInfo == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullInvalidInput, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullInvalidInput, "requestDetails", "WSManPluginShell"));
				return;
			}
			if (startupInfo == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullInvalidInput, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullInvalidInput, "startupInfo", "WSManPluginShell"));
				return;
			}
			if (startupInfo.inputStreamSet.streamIDsCount == 0 || startupInfo.outputStreamSet.streamIDsCount == 0)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullInvalidStreamSets, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullInvalidStreamSet, "stdin", "stdout"));
				return;
			}
			if (string.IsNullOrEmpty(extraInfo))
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullInvalidInput, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullInvalidInput, "extraInfo", "WSManPluginShell"));
				return;
			}
			WSManPluginInstance.SetThreadProperties(requestDetails);
			if (!this.EnsureOptionsComply(requestDetails))
			{
				return;
			}
			byte[] array = null;
			WSManPluginOperationShutdownContext wsmanPluginOperationShutdownContext;
			WSManPluginShellSession wsmanPluginShellSession;
			try
			{
				PSSenderInfo pssenderInfo = this.GetPSSenderInfo(requestDetails.senderDetails);
				WSManPluginServerTransportManager wsmanPluginServerTransportManager = new WSManPluginServerTransportManager(32768, new PSRemotingCryptoHelperServer());
				PSEtwLog.LogAnalyticInformational(PSEventId.ServerCreateRemoteSession, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
				{
					requestDetails.ToString(),
					pssenderInfo.UserInfo.Identity.Name,
					requestDetails.resourceUri
				});
				ServerRemoteSession serverRemoteSession = ServerRemoteSession.CreateServerRemoteSession(pssenderInfo, requestDetails.resourceUri, extraInfo, wsmanPluginServerTransportManager);
				if (serverRemoteSession == null)
				{
					WSManPluginInstance.ReportWSManOperationComplete(requestDetails, WSManPluginErrorCodes.SessionCreationFailed);
					return;
				}
				wsmanPluginOperationShutdownContext = new WSManPluginOperationShutdownContext(pluginContext, requestDetails.unmanagedHandle, IntPtr.Zero, false);
				if (wsmanPluginOperationShutdownContext == null)
				{
					WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.OutOfMemory);
					return;
				}
				wsmanPluginShellSession = new WSManPluginShellSession(requestDetails, wsmanPluginServerTransportManager, serverRemoteSession, wsmanPluginOperationShutdownContext);
				this.AddToActiveShellSessions(wsmanPluginShellSession);
				wsmanPluginShellSession.SessionClosed += this.HandleShellSessionClosed;
				if (inboundShellInformation != null)
				{
					if (1U != inboundShellInformation.Type)
					{
						WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidInputDatatype, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidInputDataType, "WSMAN_DATA_TYPE_TEXT"));
						this.DeleteFromActiveShellSessions(requestDetails.unmanagedHandle);
						return;
					}
					array = ServerOperationHelpers.ExtractEncodedXmlElement(inboundShellInformation.Text, "creationXml");
				}
				PSEtwLog.LogAnalyticInformational(PSEventId.ReportContext, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
				{
					requestDetails.ToString(),
					requestDetails.ToString()
				});
				int num = WSManPluginInstance.wsmanPinvokeStatic.WSManPluginReportContext(requestDetails.unmanagedHandle, 0, requestDetails.unmanagedHandle);
				if (num != 0)
				{
					WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.ReportContextFailed, StringUtil.Format(RemotingErrorIdStrings.WSManPluginReportContextFailed, new object[0]));
					this.DeleteFromActiveShellSessions(requestDetails.unmanagedHandle);
					return;
				}
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				PSEtwLog.LogOperationalError(PSEventId.TransportError, PSOpcode.Connect, PSTask.None, PSKeyword.UseAlwaysOperational, new object[]
				{
					"00000000-0000-0000-0000-000000000000",
					"00000000-0000-0000-0000-000000000000",
					Convert.ToString(WSManPluginErrorCodes.ManagedException, CultureInfo.InvariantCulture),
					ex.Message,
					ex.StackTrace
				});
				this.DeleteFromActiveShellSessions(requestDetails.unmanagedHandle);
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.ManagedException, StringUtil.Format(RemotingErrorIdStrings.WSManPluginManagedException, ex.Message));
				return;
			}
			bool flag = true;
			lock (wsmanPluginShellSession.shellSyncObject)
			{
				wsmanPluginShellSession.registeredShutdownNotification = 1;
				SafeWaitHandle value = new SafeWaitHandle(requestDetails.shutdownNotificationHandle, false);
				EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
				ClrFacade.SetSafeWaitHandle(eventWaitHandle, value);
				wsmanPluginShellSession.registeredShutDownWaitHandle = ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, new WaitOrTimerCallback(WSManPluginManagedEntryWrapper.PSPluginOperationShutdownCallback), wsmanPluginOperationShutdownContext, -1, true);
				if (wsmanPluginShellSession.registeredShutDownWaitHandle == null)
				{
					flag = false;
				}
			}
			if (flag)
			{
				try
				{
					if (array != null)
					{
						wsmanPluginShellSession.SendOneItemToSessionHelper(array, "stdin");
					}
				}
				catch (Exception ex2)
				{
					CommandProcessorBase.CheckForSevereException(ex2);
					PSEtwLog.LogOperationalError(PSEventId.TransportError, PSOpcode.Connect, PSTask.None, PSKeyword.UseAlwaysOperational, new object[]
					{
						"00000000-0000-0000-0000-000000000000",
						"00000000-0000-0000-0000-000000000000",
						Convert.ToString(WSManPluginErrorCodes.ManagedException, CultureInfo.InvariantCulture),
						ex2.Message,
						ex2.StackTrace
					});
					if (Interlocked.Exchange(ref wsmanPluginShellSession.registeredShutdownNotification, 0) == 1)
					{
						wsmanPluginShellSession.registeredShutDownWaitHandle.Unregister(null);
						wsmanPluginShellSession.registeredShutDownWaitHandle = null;
						WSManPluginInstance.PerformCloseOperation(wsmanPluginOperationShutdownContext);
					}
					return;
				}
				return;
			}
			wsmanPluginShellSession.registeredShutdownNotification = 0;
			WSManPluginInstance.ReportWSManOperationComplete(requestDetails, WSManPluginErrorCodes.ShutdownRegistrationFailed);
			this.DeleteFromActiveShellSessions(requestDetails.unmanagedHandle);
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x000FA0D8 File Offset: 0x000F82D8
		internal void CloseShellOperation(WSManPluginOperationShutdownContext context)
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.ServerCloseOperation, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
			{
				context.shellContext.ToString(),
				context.commandContext.ToString(),
				context.isReceiveOperation.ToString()
			});
			WSManPluginShellSession fromActiveShellSessions = this.GetFromActiveShellSessions(context.shellContext);
			if (fromActiveShellSessions == null)
			{
				return;
			}
			WSManPluginInstance.SetThreadProperties(fromActiveShellSessions.creationRequestDetails);
			if (!context.isReceiveOperation)
			{
				this.DeleteFromActiveShellSessions(context.shellContext);
			}
			string message = StringUtil.Format(RemotingErrorIdStrings.WSManPluginOperationClose, new object[0]);
			Exception reasonForClose = new Exception(message);
			fromActiveShellSessions.CloseOperation(context, reasonForClose);
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x000FA18C File Offset: 0x000F838C
		internal void CloseCommandOperation(WSManPluginOperationShutdownContext context)
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.ServerCloseOperation, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
			{
				context.shellContext.ToString(),
				context.commandContext.ToString(),
				context.isReceiveOperation.ToString()
			});
			WSManPluginShellSession fromActiveShellSessions = this.GetFromActiveShellSessions(context.shellContext);
			if (fromActiveShellSessions == null)
			{
				return;
			}
			WSManPluginInstance.SetThreadProperties(fromActiveShellSessions.creationRequestDetails);
			fromActiveShellSessions.CloseCommandOperation(context);
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x000FA210 File Offset: 0x000F8410
		private void AddToActiveShellSessions(WSManPluginShellSession newShellSession)
		{
			int num = -1;
			lock (this.syncObject)
			{
				IntPtr unmanagedHandle = newShellSession.creationRequestDetails.unmanagedHandle;
				if (!this.activeShellSessions.ContainsKey(unmanagedHandle))
				{
					this.activeShellSessions.Add(unmanagedHandle, newShellSession);
					num = this.activeShellSessions.Count;
				}
			}
			if (-1 != num)
			{
				WSManServerChannelEvents.RaiseActiveSessionsChangedEvent(new ActiveSessionsChangedEventArgs(num));
			}
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x000FA290 File Offset: 0x000F8490
		private WSManPluginShellSession GetFromActiveShellSessions(IntPtr key)
		{
			WSManPluginShellSession result;
			lock (this.syncObject)
			{
				WSManPluginShellSession wsmanPluginShellSession;
				this.activeShellSessions.TryGetValue(key, out wsmanPluginShellSession);
				result = wsmanPluginShellSession;
			}
			return result;
		}

		// Token: 0x06002CFF RID: 11519 RVA: 0x000FA2DC File Offset: 0x000F84DC
		private void DeleteFromActiveShellSessions(IntPtr keyToDelete)
		{
			int num = -1;
			lock (this.syncObject)
			{
				if (this.activeShellSessions.ContainsKey(keyToDelete))
				{
					this.activeShellSessions.Remove(keyToDelete);
					num = this.activeShellSessions.Count;
				}
			}
			if (-1 != num)
			{
				WSManServerChannelEvents.RaiseActiveSessionsChangedEvent(new ActiveSessionsChangedEventArgs(num));
			}
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x000FA350 File Offset: 0x000F8550
		private void HandleShellSessionClosed(object source, EventArgs e)
		{
			this.DeleteFromActiveShellSessions((IntPtr)source);
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x000FA360 File Offset: 0x000F8560
		private bool validateIncomingContexts(WSManNativeApi.WSManPluginRequest requestDetails, IntPtr shellContext, string inputFunctionName)
		{
			if (requestDetails == null)
			{
				PSEtwLog.LogAnalyticInformational(PSEventId.ReportOperationComplete, PSOpcode.Close, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
				{
					"null",
					Convert.ToString(WSManPluginErrorCodes.NullInvalidInput, CultureInfo.InvariantCulture),
					StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullInvalidInput, "requestDetails", inputFunctionName),
					string.Empty
				});
				return false;
			}
			if (IntPtr.Zero == shellContext)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.NullShellContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullShellContext, "ShellContext", inputFunctionName));
				return false;
			}
			return true;
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x000FA3F8 File Offset: 0x000F85F8
		internal void CreateCommand(IntPtr pluginContext, WSManNativeApi.WSManPluginRequest requestDetails, int flags, IntPtr shellContext, string commandLine, WSManNativeApi.WSManCommandArgSet arguments)
		{
			if (!this.validateIncomingContexts(requestDetails, shellContext, "WSManRunShellCommandEx"))
			{
				return;
			}
			WSManPluginInstance.SetThreadProperties(requestDetails);
			PSEtwLog.LogAnalyticInformational(PSEventId.ServerCreateCommandSession, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
			{
				shellContext.ToString(),
				requestDetails.ToString()
			});
			WSManPluginShellSession fromActiveShellSessions = this.GetFromActiveShellSessions(shellContext);
			if (fromActiveShellSessions == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidShellContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidShellContext, new object[0]));
				return;
			}
			fromActiveShellSessions.CreateCommand(pluginContext, requestDetails, flags, commandLine, arguments);
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x000FA488 File Offset: 0x000F8688
		internal void StopCommand(WSManNativeApi.WSManPluginRequest requestDetails, IntPtr shellContext, IntPtr commandContext)
		{
			if (requestDetails == null)
			{
				PSEtwLog.LogAnalyticInformational(PSEventId.ReportOperationComplete, PSOpcode.Close, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
				{
					"null",
					Convert.ToString(WSManPluginErrorCodes.NullInvalidInput, CultureInfo.InvariantCulture),
					StringUtil.Format(RemotingErrorIdStrings.WSManPluginNullInvalidInput, "requestDetails", "StopCommand"),
					string.Empty
				});
				return;
			}
			WSManPluginInstance.SetThreadProperties(requestDetails);
			PSEtwLog.LogAnalyticInformational(PSEventId.ServerStopCommand, PSOpcode.Disconnect, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
			{
				shellContext.ToString(),
				commandContext.ToString(),
				requestDetails.ToString()
			});
			WSManPluginShellSession fromActiveShellSessions = this.GetFromActiveShellSessions(shellContext);
			if (fromActiveShellSessions == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidShellContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidShellContext, new object[0]));
				return;
			}
			WSManPluginCommandSession commandSession = fromActiveShellSessions.GetCommandSession(commandContext);
			if (commandSession == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidCommandContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidCommandContext, new object[0]));
				return;
			}
			commandSession.Stop(requestDetails);
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x000FA597 File Offset: 0x000F8797
		internal void Shutdown()
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.WSManPluginShutdown, PSOpcode.ShuttingDown, PSTask.None, (PSKeyword)4611686018427388160UL, new object[0]);
			WSManServerChannelEvents.RaiseShuttingDownEvent();
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x000FA5BC File Offset: 0x000F87BC
		internal void ConnectShellOrCommand(WSManNativeApi.WSManPluginRequest requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, WSManNativeApi.WSManData_UnToMan inboundConnectInformation)
		{
			if (!this.validateIncomingContexts(requestDetails, shellContext, "ConnectShellOrCommand"))
			{
				return;
			}
			WSManPluginInstance.SetThreadProperties(requestDetails);
			WSManPluginShellSession fromActiveShellSessions = this.GetFromActiveShellSessions(shellContext);
			if (fromActiveShellSessions == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidShellContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidShellContext, new object[0]));
				return;
			}
			if (IntPtr.Zero == commandContext)
			{
				fromActiveShellSessions.ExecuteConnect(requestDetails, flags, inboundConnectInformation);
				return;
			}
			WSManPluginCommandSession commandSession = fromActiveShellSessions.GetCommandSession(commandContext);
			if (commandSession == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidCommandContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidCommandContext, new object[0]));
				return;
			}
			commandSession.ExecuteConnect(requestDetails, flags, inboundConnectInformation);
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000FA654 File Offset: 0x000F8854
		internal void SendOneItemToShellOrCommand(WSManNativeApi.WSManPluginRequest requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, string stream, WSManNativeApi.WSManData_UnToMan inboundData)
		{
			if (!this.validateIncomingContexts(requestDetails, shellContext, "SendOneItemToShellOrCommand"))
			{
				return;
			}
			WSManPluginInstance.SetThreadProperties(requestDetails);
			PSEtwLog.LogAnalyticInformational(PSEventId.ServerReceivedData, PSOpcode.Open, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
			{
				shellContext.ToString(),
				commandContext.ToString(),
				requestDetails.ToString()
			});
			WSManPluginShellSession fromActiveShellSessions = this.GetFromActiveShellSessions(shellContext);
			if (fromActiveShellSessions == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidShellContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidShellContext, new object[0]));
				return;
			}
			if (IntPtr.Zero == commandContext)
			{
				fromActiveShellSessions.SendOneItemToSession(requestDetails, flags, stream, inboundData);
				return;
			}
			WSManPluginCommandSession commandSession = fromActiveShellSessions.GetCommandSession(commandContext);
			if (commandSession == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidCommandContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidCommandContext, new object[0]));
				return;
			}
			commandSession.SendOneItemToSession(requestDetails, flags, stream, inboundData);
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x000FA734 File Offset: 0x000F8934
		internal void EnableShellOrCommandToSendDataToClient(IntPtr pluginContext, WSManNativeApi.WSManPluginRequest requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, WSManNativeApi.WSManStreamIDSet_UnToMan streamSet)
		{
			if (!this.validateIncomingContexts(requestDetails, shellContext, "EnableShellOrCommandToSendDataToClient"))
			{
				return;
			}
			WSManPluginInstance.SetThreadProperties(requestDetails);
			PSEtwLog.LogAnalyticInformational(PSEventId.ServerClientReceiveRequest, PSOpcode.Open, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
			{
				shellContext.ToString(),
				commandContext.ToString(),
				requestDetails.ToString()
			});
			WSManPluginShellSession fromActiveShellSessions = this.GetFromActiveShellSessions(shellContext);
			if (fromActiveShellSessions == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidShellContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidShellContext, new object[0]));
				return;
			}
			WSManPluginOperationShutdownContext wsmanPluginOperationShutdownContext = new WSManPluginOperationShutdownContext(pluginContext, shellContext, IntPtr.Zero, true);
			if (wsmanPluginOperationShutdownContext == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.OutOfMemory);
				return;
			}
			if (IntPtr.Zero == commandContext)
			{
				if (fromActiveShellSessions.EnableSessionToSendDataToClient(requestDetails, flags, streamSet, wsmanPluginOperationShutdownContext))
				{
					return;
				}
			}
			else
			{
				wsmanPluginOperationShutdownContext.commandContext = commandContext;
				WSManPluginCommandSession commandSession = fromActiveShellSessions.GetCommandSession(commandContext);
				if (commandSession == null)
				{
					WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.InvalidCommandContext, StringUtil.Format(RemotingErrorIdStrings.WSManPluginInvalidCommandContext, new object[0]));
					return;
				}
				commandSession.EnableSessionToSendDataToClient(requestDetails, flags, streamSet, wsmanPluginOperationShutdownContext);
			}
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000FA840 File Offset: 0x000F8A40
		private PSSenderInfo GetPSSenderInfo(WSManNativeApi.WSManSenderDetails senderDetails)
		{
			PSCertificateDetails cert = null;
			if (senderDetails.certificateDetails != null)
			{
				cert = new PSCertificateDetails(senderDetails.certificateDetails.subject, senderDetails.certificateDetails.issuerName, senderDetails.certificateDetails.issuerThumbprint);
			}
			PSIdentity identity = new PSIdentity(senderDetails.authenticationMechanism, true, senderDetails.senderName, cert);
			WindowsIdentity windowsIdentity = null;
			if (IntPtr.Zero != senderDetails.clientToken)
			{
				try
				{
					windowsIdentity = new WindowsIdentity(senderDetails.clientToken, senderDetails.authenticationMechanism);
				}
				catch (ArgumentException)
				{
				}
				catch (SecurityException)
				{
				}
			}
			PSPrincipal userPrincipal = new PSPrincipal(identity, windowsIdentity);
			return new PSSenderInfo(userPrincipal, senderDetails.httpUrl);
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000FA8F4 File Offset: 0x000F8AF4
		protected internal bool EnsureOptionsComply(WSManNativeApi.WSManPluginRequest requestDetails)
		{
			WSManNativeApi.WSManOption[] options = requestDetails.operationInfo.optionSet.options;
			bool flag = false;
			foreach (WSManNativeApi.WSManOption wsmanOption in options)
			{
				if (string.Equals(wsmanOption.name, "protocolversion", StringComparison.Ordinal))
				{
					if (!this.EnsureProtocolVersionComplies(requestDetails, wsmanOption.value))
					{
						return false;
					}
					flag = true;
				}
				if (string.Compare(wsmanOption.name, 0, "PS_", 0, "PS_".Length, StringComparison.Ordinal) == 0 && wsmanOption.mustComply)
				{
					WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.OptionNotUnderstood, StringUtil.Format(RemotingErrorIdStrings.WSManPluginOptionNotUnderstood, new object[]
					{
						wsmanOption.name,
						PSVersionInfo.BuildVersion,
						"2.0"
					}));
					return false;
				}
			}
			if (!flag)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.ProtocolVersionNotFound, StringUtil.Format(RemotingErrorIdStrings.WSManPluginProtocolVersionNotFound, new object[]
				{
					"protocolversion",
					PSVersionInfo.BuildVersion,
					"2.0"
				}));
				return false;
			}
			return true;
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000FAA04 File Offset: 0x000F8C04
		protected internal bool EnsureProtocolVersionComplies(WSManNativeApi.WSManPluginRequest requestDetails, string clientVersionString)
		{
			if (string.Equals(clientVersionString, "2.0", StringComparison.Ordinal))
			{
				return true;
			}
			Version version = Utils.StringToVersion(clientVersionString);
			Version version2 = Utils.StringToVersion("2.0");
			if (null != version && null != version2 && version.Major == version2.Major && version.Minor >= version2.Minor)
			{
				return true;
			}
			WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.ProtocolVersionNotMatch, StringUtil.Format(RemotingErrorIdStrings.WSManPluginProtocolVersionNotMatch, new object[]
			{
				"2.0",
				PSVersionInfo.BuildVersion,
				clientVersionString
			}));
			return false;
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000FAA98 File Offset: 0x000F8C98
		internal static void PerformWSManPluginShell(IntPtr pluginContext, IntPtr requestDetails, int flags, string extraInfo, IntPtr startupInfo, IntPtr inboundShellInformation)
		{
			WSManPluginInstance wsmanPluginInstance = WSManPluginInstance.GetFromActivePlugins(pluginContext);
			if (wsmanPluginInstance == null)
			{
				lock (WSManPluginInstance.activePlugins)
				{
					wsmanPluginInstance = WSManPluginInstance.GetFromActivePlugins(pluginContext);
					if (wsmanPluginInstance == null)
					{
						WSManPluginInstance wsmanPluginInstance2 = new WSManPluginInstance();
						WSManPluginInstance.AddToActivePlugins(pluginContext, wsmanPluginInstance2);
						wsmanPluginInstance = wsmanPluginInstance2;
					}
				}
			}
			WSManNativeApi.WSManPluginRequest requestDetails2 = WSManNativeApi.WSManPluginRequest.UnMarshal(requestDetails);
			WSManNativeApi.WSManShellStartupInfo_UnToMan startupInfo2 = WSManNativeApi.WSManShellStartupInfo_UnToMan.UnMarshal(startupInfo);
			WSManNativeApi.WSManData_UnToMan inboundShellInformation2 = WSManNativeApi.WSManData_UnToMan.UnMarshal(inboundShellInformation);
			wsmanPluginInstance.CreateShell(pluginContext, requestDetails2, flags, extraInfo, startupInfo2, inboundShellInformation2);
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000FAB20 File Offset: 0x000F8D20
		internal static void PerformWSManPluginCommand(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, [MarshalAs(UnmanagedType.LPWStr)] string commandLine, IntPtr arguments)
		{
			WSManPluginInstance fromActivePlugins = WSManPluginInstance.GetFromActivePlugins(pluginContext);
			if (fromActivePlugins == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.PluginContextNotFound, StringUtil.Format(RemotingErrorIdStrings.WSManPluginContextNotFound, new object[0]));
				return;
			}
			WSManNativeApi.WSManPluginRequest requestDetails2 = WSManNativeApi.WSManPluginRequest.UnMarshal(requestDetails);
			WSManNativeApi.WSManCommandArgSet arguments2 = WSManNativeApi.WSManCommandArgSet.UnMarshal(arguments);
			fromActivePlugins.CreateCommand(pluginContext, requestDetails2, flags, shellContext, commandLine, arguments2);
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000FAB70 File Offset: 0x000F8D70
		internal static void PerformWSManPluginConnect(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, IntPtr inboundConnectInformation)
		{
			WSManPluginInstance fromActivePlugins = WSManPluginInstance.GetFromActivePlugins(pluginContext);
			if (fromActivePlugins == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.PluginContextNotFound, StringUtil.Format(RemotingErrorIdStrings.WSManPluginContextNotFound, new object[0]));
				return;
			}
			WSManNativeApi.WSManPluginRequest requestDetails2 = WSManNativeApi.WSManPluginRequest.UnMarshal(requestDetails);
			WSManNativeApi.WSManData_UnToMan inboundConnectInformation2 = WSManNativeApi.WSManData_UnToMan.UnMarshal(inboundConnectInformation);
			fromActivePlugins.ConnectShellOrCommand(requestDetails2, flags, shellContext, commandContext, inboundConnectInformation2);
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000FABC0 File Offset: 0x000F8DC0
		internal static void PerformWSManPluginSend(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, string stream, IntPtr inboundData)
		{
			WSManPluginInstance fromActivePlugins = WSManPluginInstance.GetFromActivePlugins(pluginContext);
			if (fromActivePlugins == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.PluginContextNotFound, StringUtil.Format(RemotingErrorIdStrings.WSManPluginContextNotFound, new object[0]));
				return;
			}
			WSManNativeApi.WSManPluginRequest requestDetails2 = WSManNativeApi.WSManPluginRequest.UnMarshal(requestDetails);
			WSManNativeApi.WSManData_UnToMan inboundData2 = WSManNativeApi.WSManData_UnToMan.UnMarshal(inboundData);
			fromActivePlugins.SendOneItemToShellOrCommand(requestDetails2, flags, shellContext, commandContext, stream, inboundData2);
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000FAC10 File Offset: 0x000F8E10
		internal static void PerformWSManPluginReceive(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, IntPtr streamSet)
		{
			WSManPluginInstance fromActivePlugins = WSManPluginInstance.GetFromActivePlugins(pluginContext);
			if (fromActivePlugins == null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.PluginContextNotFound, StringUtil.Format(RemotingErrorIdStrings.WSManPluginContextNotFound, new object[0]));
				return;
			}
			WSManNativeApi.WSManPluginRequest requestDetails2 = WSManNativeApi.WSManPluginRequest.UnMarshal(requestDetails);
			WSManNativeApi.WSManStreamIDSet_UnToMan streamSet2 = WSManNativeApi.WSManStreamIDSet_UnToMan.UnMarshal(streamSet);
			fromActivePlugins.EnableShellOrCommandToSendDataToClient(pluginContext, requestDetails2, flags, shellContext, commandContext, streamSet2);
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000FAC60 File Offset: 0x000F8E60
		internal static void PerformWSManPluginSignal(IntPtr pluginContext, IntPtr requestDetails, int flags, IntPtr shellContext, IntPtr commandContext, string code)
		{
			WSManNativeApi.WSManPluginRequest requestDetails2 = WSManNativeApi.WSManPluginRequest.UnMarshal(requestDetails);
			if (IntPtr.Zero != commandContext)
			{
				if (!string.Equals(code, "powershell/signal/crtl_c", StringComparison.Ordinal))
				{
					WSManPluginOperationShutdownContext wsmanPluginOperationShutdownContext = new WSManPluginOperationShutdownContext(pluginContext, shellContext, commandContext, false);
					if (wsmanPluginOperationShutdownContext == null)
					{
						WSManPluginInstance.ReportOperationComplete(requestDetails2, WSManPluginErrorCodes.OutOfMemory);
						return;
					}
					WSManPluginInstance.PerformCloseOperation(wsmanPluginOperationShutdownContext);
				}
				else
				{
					WSManPluginInstance fromActivePlugins = WSManPluginInstance.GetFromActivePlugins(pluginContext);
					if (fromActivePlugins == null)
					{
						WSManPluginInstance.ReportOperationComplete(requestDetails2, WSManPluginErrorCodes.PluginContextNotFound, StringUtil.Format(RemotingErrorIdStrings.WSManPluginContextNotFound, new object[0]));
						return;
					}
					fromActivePlugins.StopCommand(requestDetails2, shellContext, commandContext);
					return;
				}
			}
			WSManPluginInstance.ReportOperationComplete(requestDetails2, WSManPluginErrorCodes.NoError);
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x000FACEC File Offset: 0x000F8EEC
		internal static void PerformCloseOperation(WSManPluginOperationShutdownContext context)
		{
			WSManPluginInstance fromActivePlugins = WSManPluginInstance.GetFromActivePlugins(context.pluginContext);
			if (fromActivePlugins == null)
			{
				return;
			}
			if (IntPtr.Zero == context.commandContext)
			{
				fromActivePlugins.CloseShellOperation(context);
				return;
			}
			fromActivePlugins.CloseCommandOperation(context);
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000FAD2C File Offset: 0x000F8F2C
		internal static void PerformShutdown(IntPtr pluginContext)
		{
			WSManPluginInstance fromActivePlugins = WSManPluginInstance.GetFromActivePlugins(pluginContext);
			if (fromActivePlugins == null)
			{
				return;
			}
			fromActivePlugins.Shutdown();
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000FAD4C File Offset: 0x000F8F4C
		private static WSManPluginInstance GetFromActivePlugins(IntPtr pluginContext)
		{
			WSManPluginInstance result;
			lock (WSManPluginInstance.activePlugins)
			{
				WSManPluginInstance wsmanPluginInstance = null;
				WSManPluginInstance.activePlugins.TryGetValue(pluginContext, out wsmanPluginInstance);
				result = wsmanPluginInstance;
			}
			return result;
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000FAD98 File Offset: 0x000F8F98
		private static void AddToActivePlugins(IntPtr pluginContext, WSManPluginInstance plugin)
		{
			lock (WSManPluginInstance.activePlugins)
			{
				if (!WSManPluginInstance.activePlugins.ContainsKey(pluginContext))
				{
					WSManPluginInstance.activePlugins.Add(pluginContext, plugin);
				}
			}
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000FADEC File Offset: 0x000F8FEC
		internal static void ReportWSManOperationComplete(WSManNativeApi.WSManPluginRequest requestDetails, WSManPluginErrorCodes errorCode)
		{
			PSEtwLog.LogAnalyticInformational(PSEventId.ReportOperationComplete, PSOpcode.Close, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
			{
				requestDetails.unmanagedHandle.ToString(),
				Convert.ToString(errorCode, CultureInfo.InvariantCulture),
				string.Empty,
				string.Empty
			});
			WSManPluginInstance.ReportOperationComplete(requestDetails.unmanagedHandle, errorCode, "");
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000FAE60 File Offset: 0x000F9060
		internal static void ReportWSManOperationComplete(WSManNativeApi.WSManPluginRequest requestDetails, Exception reasonForClose)
		{
			WSManPluginErrorCodes wsmanPluginErrorCodes = WSManPluginErrorCodes.NoError;
			string text = string.Empty;
			string text2 = string.Empty;
			if (reasonForClose != null)
			{
				wsmanPluginErrorCodes = WSManPluginErrorCodes.ManagedException;
				text = reasonForClose.Message;
				text2 = reasonForClose.StackTrace;
			}
			PSEtwLog.LogAnalyticInformational(PSEventId.ReportOperationComplete, PSOpcode.Close, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
			{
				requestDetails.ToString(),
				Convert.ToString(wsmanPluginErrorCodes, CultureInfo.InvariantCulture),
				text,
				text2
			});
			if (reasonForClose != null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails, WSManPluginErrorCodes.ManagedException, StringUtil.Format(RemotingErrorIdStrings.WSManPluginManagedException, reasonForClose.Message));
				return;
			}
			WSManPluginInstance.ReportOperationComplete(requestDetails.unmanagedHandle, WSManPluginErrorCodes.NoError, "");
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000FAF04 File Offset: 0x000F9104
		internal static void SetThreadProperties(WSManNativeApi.WSManPluginRequest requestDetails)
		{
			WSManNativeApi.WSManDataStruct wsmanDataStruct = new WSManNativeApi.WSManDataStruct();
			int num = WSManPluginInstance.wsmanPinvokeStatic.WSManPluginGetOperationParameters(requestDetails.unmanagedHandle, 5, wsmanDataStruct);
			bool flag = 0 == num;
			WSManNativeApi.WSManData_UnToMan wsmanData_UnToMan = WSManNativeApi.WSManData_UnToMan.UnMarshal(wsmanDataStruct);
			num = WSManPluginInstance.wsmanPinvokeStatic.WSManPluginGetOperationParameters(requestDetails.unmanagedHandle, 6, wsmanDataStruct);
			bool flag2 = 0 == num;
			WSManNativeApi.WSManData_UnToMan wsmanData_UnToMan2 = WSManNativeApi.WSManData_UnToMan.UnMarshal(wsmanDataStruct);
			try
			{
				if (flag && 1U == wsmanData_UnToMan.Type)
				{
					CultureInfo currentUICulture = new CultureInfo(wsmanData_UnToMan.Text);
					Thread.CurrentThread.CurrentUICulture = currentUICulture;
				}
			}
			catch (ArgumentException)
			{
			}
			try
			{
				if (flag2 && 1U == wsmanData_UnToMan2.Type)
				{
					CultureInfo currentCulture = new CultureInfo(wsmanData_UnToMan2.Text);
					Thread.CurrentThread.CurrentCulture = currentCulture;
				}
			}
			catch (ArgumentException)
			{
			}
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000FAFCC File Offset: 0x000F91CC
		internal static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception ex = (Exception)args.ExceptionObject;
			PSEtwLog.LogOperationalError(PSEventId.AppDomainUnhandledException, PSOpcode.Close, PSTask.None, PSKeyword.UseAlwaysOperational, new object[]
			{
				ex.GetType().ToString(),
				ex.Message,
				ex.StackTrace
			});
			PSEtwLog.LogAnalyticError(PSEventId.AppDomainUnhandledException_Analytic, PSOpcode.Close, PSTask.None, (PSKeyword)4611686018427388160UL, new object[]
			{
				ex.GetType().ToString(),
				ex.Message,
				ex.StackTrace
			});
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000FB061 File Offset: 0x000F9261
		internal static void ReportOperationComplete(WSManNativeApi.WSManPluginRequest requestDetails, WSManPluginErrorCodes errorCode, string errorMessage)
		{
			if (requestDetails != null)
			{
				WSManPluginInstance.ReportOperationComplete(requestDetails.unmanagedHandle, errorCode, errorMessage);
			}
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000FB073 File Offset: 0x000F9273
		internal static void ReportOperationComplete(WSManNativeApi.WSManPluginRequest requestDetails, WSManPluginErrorCodes errorCode)
		{
			if (requestDetails != null && IntPtr.Zero != requestDetails.unmanagedHandle)
			{
				WSManPluginInstance.wsmanPinvokeStatic.WSManPluginOperationComplete(requestDetails.unmanagedHandle, 0, (int)errorCode, null);
			}
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x000FB09E File Offset: 0x000F929E
		internal static void ReportOperationComplete(IntPtr requestDetails, WSManPluginErrorCodes errorCode, string errorMessage = "")
		{
			if (IntPtr.Zero == requestDetails)
			{
				return;
			}
			WSManPluginInstance.wsmanPinvokeStatic.WSManPluginOperationComplete(requestDetails, 0, (int)errorCode, errorMessage);
		}

		// Token: 0x040017B8 RID: 6072
		private Dictionary<IntPtr, WSManPluginShellSession> activeShellSessions;

		// Token: 0x040017B9 RID: 6073
		private object syncObject;

		// Token: 0x040017BA RID: 6074
		private static Dictionary<IntPtr, WSManPluginInstance> activePlugins = new Dictionary<IntPtr, WSManPluginInstance>();

		// Token: 0x040017BB RID: 6075
		internal static IWSManNativeApiFacade wsmanPinvokeStatic = new WSManNativeApiFacade();
	}
}
