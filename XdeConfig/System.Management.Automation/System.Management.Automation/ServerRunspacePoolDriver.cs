using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Remoting.Server;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Security;
using System.Threading;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x0200030B RID: 779
	internal class ServerRunspacePoolDriver : IRSPDriverInvoke
	{
		// Token: 0x060024E1 RID: 9441 RVA: 0x000CE350 File Offset: 0x000CC550
		internal ServerRunspacePoolDriver(Guid clientRunspacePoolId, int minRunspaces, int maxRunspaces, PSThreadOptions threadOptions, ApartmentState apartmentState, HostInfo hostInfo, InitialSessionState initialSessionState, PSPrimitiveDictionary applicationPrivateData, ConfigurationDataFromXML configData, AbstractServerSessionTransportManager transportManager, bool isAdministrator, RemoteSessionCapability serverCapability, Version psClientVersion)
		{
			this.serverCapability = serverCapability;
			this.clientPSVersion = psClientVersion;
			this.remoteHost = new ServerDriverRemoteHost(clientRunspacePoolId, Guid.Empty, hostInfo, transportManager, null);
			this.configData = configData;
			this.applicationPrivateData = applicationPrivateData;
			this.localRunspacePool = RunspaceFactory.CreateRunspacePool(minRunspaces, maxRunspaces, initialSessionState, this.remoteHost);
			PSThreadOptions psthreadOptions = (configData.ShellThreadOptions != null) ? configData.ShellThreadOptions.Value : PSThreadOptions.UseCurrentThread;
			if (threadOptions == PSThreadOptions.Default || threadOptions == psthreadOptions)
			{
				this.localRunspacePool.ThreadOptions = psthreadOptions;
			}
			else
			{
				if (!isAdministrator)
				{
					throw new InvalidOperationException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.MustBeAdminToOverrideThreadOptions, new object[0]));
				}
				this.localRunspacePool.ThreadOptions = threadOptions;
			}
			ApartmentState apartmentState2 = (configData.ShellThreadApartmentState != null) ? configData.ShellThreadApartmentState.Value : ApartmentState.Unknown;
			if (apartmentState == ApartmentState.Unknown || apartmentState == apartmentState2)
			{
				this.localRunspacePool.ApartmentState = apartmentState2;
			}
			else
			{
				this.localRunspacePool.ApartmentState = apartmentState;
			}
			if (maxRunspaces == 1 && (this.localRunspacePool.ThreadOptions == PSThreadOptions.Default || this.localRunspacePool.ThreadOptions == PSThreadOptions.UseCurrentThread))
			{
				this.driverNestedInvoker = new ServerRunspacePoolDriver.PowerShellDriverInvoker();
			}
			this.clientRunspacePoolId = clientRunspacePoolId;
			this.dsHandler = new ServerRunspacePoolDataStructureHandler(this, transportManager);
			this.localRunspacePool.StateChanged += this.HandleRunspacePoolStateChanged;
			this.localRunspacePool.ForwardEvent += this.HandleRunspacePoolForwardEvent;
			this.localRunspacePool.RunspaceCreated += this.HandleRunspaceCreated;
			this.dsHandler.CreateAndInvokePowerShell += this.HandleCreateAndInvokePowerShell;
			this.dsHandler.GetCommandMetadata += this.HandleGetCommandMetadata;
			this.dsHandler.HostResponseReceived += this.HandleHostResponseReceived;
			this.dsHandler.SetMaxRunspacesReceived += this.HandleSetMaxRunspacesReceived;
			this.dsHandler.SetMinRunspacesReceived += this.HandleSetMinRunspacesReceived;
			this.dsHandler.GetAvailableRunspacesReceived += this.HandleGetAvailalbeRunspacesReceived;
			this.dsHandler.ResetRunspaceState += this.HandleResetRunspaceState;
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x060024E2 RID: 9442 RVA: 0x000CE593 File Offset: 0x000CC793
		internal ServerRunspacePoolDataStructureHandler DataStructureHandler
		{
			get
			{
				return this.dsHandler;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060024E3 RID: 9443 RVA: 0x000CE59B File Offset: 0x000CC79B
		internal ServerRemoteHost ServerRemoteHost
		{
			get
			{
				return this.remoteHost;
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060024E4 RID: 9444 RVA: 0x000CE5A3 File Offset: 0x000CC7A3
		internal Guid InstanceId
		{
			get
			{
				return this.clientRunspacePoolId;
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060024E5 RID: 9445 RVA: 0x000CE5AB File Offset: 0x000CC7AB
		internal RunspacePool RunspacePool
		{
			get
			{
				return this.localRunspacePool;
			}
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000CE5B3 File Offset: 0x000CC7B3
		internal void Start()
		{
			this.localRunspacePool.Open();
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000CE5C0 File Offset: 0x000CC7C0
		internal void SendApplicationPrivateDataToClient()
		{
			if (this.applicationPrivateData == null)
			{
				this.applicationPrivateData = new PSPrimitiveDictionary();
			}
			if (this.serverRemoteDebugger != null)
			{
				DebugModes debugMode = this.serverRemoteDebugger.DebugMode;
				if (this.applicationPrivateData.ContainsKey("DebugMode"))
				{
					this.applicationPrivateData["DebugMode"] = (int)debugMode;
				}
				else
				{
					this.applicationPrivateData.Add("DebugMode", (int)debugMode);
				}
				bool inBreakpoint = this.serverRemoteDebugger.InBreakpoint;
				if (this.applicationPrivateData.ContainsKey("DebugStop"))
				{
					this.applicationPrivateData["DebugStop"] = inBreakpoint;
				}
				else
				{
					this.applicationPrivateData.Add("DebugStop", inBreakpoint);
				}
				int breakpointCount = this.serverRemoteDebugger.GetBreakpointCount();
				if (this.applicationPrivateData.ContainsKey("DebugBreakpointCount"))
				{
					this.applicationPrivateData["DebugBreakpointCount"] = breakpointCount;
				}
				else
				{
					this.applicationPrivateData.Add("DebugBreakpointCount", breakpointCount);
				}
				bool isDebuggerSteppingEnabled = this.serverRemoteDebugger.IsDebuggerSteppingEnabled;
				if (this.applicationPrivateData.ContainsKey("BreakAll"))
				{
					this.applicationPrivateData["BreakAll"] = isDebuggerSteppingEnabled;
				}
				else
				{
					this.applicationPrivateData.Add("BreakAll", isDebuggerSteppingEnabled);
				}
				UnhandledBreakpointProcessingMode unhandledBreakpointMode = this.serverRemoteDebugger.UnhandledBreakpointMode;
				if (this.applicationPrivateData.ContainsKey("UnhandledBreakpointMode"))
				{
					this.applicationPrivateData["UnhandledBreakpointMode"] = (int)unhandledBreakpointMode;
				}
				else
				{
					this.applicationPrivateData.Add("UnhandledBreakpointMode", (int)unhandledBreakpointMode);
				}
			}
			this.dsHandler.SendApplicationPrivateDataToClient(this.applicationPrivateData, this.serverCapability);
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000CE768 File Offset: 0x000CC968
		internal void Close()
		{
			if (!this.isClosed)
			{
				this.isClosed = true;
				this.DisposeRemoteDebugger();
				this.localRunspacePool.Close();
				this.localRunspacePool.StateChanged -= this.HandleRunspacePoolStateChanged;
				this.localRunspacePool.ForwardEvent -= this.HandleRunspacePoolForwardEvent;
				this.localRunspacePool.Dispose();
				this.localRunspacePool = null;
				if (this.rsToUseForSteppablePipeline != null)
				{
					this.rsToUseForSteppablePipeline.Close();
					this.rsToUseForSteppablePipeline.Dispose();
					this.rsToUseForSteppablePipeline = null;
				}
				this.Closed.SafeInvoke(this, EventArgs.Empty);
			}
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000CE80E File Offset: 0x000CCA0E
		public void EnterNestedPipeline()
		{
			if (this.driverNestedInvoker == null)
			{
				throw new PSNotSupportedException(RemotingErrorIdStrings.NestedPipelineNotSupported);
			}
			this.driverNestedInvoker.PushInvoker();
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000CE82E File Offset: 0x000CCA2E
		public void ExitNestedPipeline()
		{
			if (this.driverNestedInvoker == null)
			{
				throw new PSNotSupportedException(RemotingErrorIdStrings.NestedPipelineNotSupported);
			}
			this.driverNestedInvoker.PopInvoker();
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000CE84E File Offset: 0x000CCA4E
		public bool HandleStopSignal()
		{
			return this.serverRemoteDebugger != null && this.serverRemoteDebugger.HandleStopSignal();
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x000CE865 File Offset: 0x000CCA65
		private void HandleRunspaceCreatedForTypeTable(object sender, RunspaceCreatedEventArgs args)
		{
			this.dsHandler.TypeTable = args.Runspace.ExecutionContext.TypeTable;
			this.rsToUseForSteppablePipeline = args.Runspace;
			this.SetupRemoteDebugger(this.rsToUseForSteppablePipeline);
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000CE89C File Offset: 0x000CCA9C
		private void SetupRemoteDebugger(Runspace runspace)
		{
			CmdletInfo cmdlet = runspace.ExecutionContext.SessionState.InvokeCommand.GetCmdlet("Set-PSBreakpoint");
			if (cmdlet == null)
			{
				if (runspace.ExecutionContext.LanguageMode != PSLanguageMode.FullLanguage && !runspace.ExecutionContext.UseFullLanguageModeInDebugger)
				{
					return;
				}
			}
			else if (cmdlet.Visibility != SessionStateEntryVisibility.Public)
			{
				return;
			}
			if (this.driverNestedInvoker != null && this.clientPSVersion != null && this.clientPSVersion >= PSVersionInfo.PSV4Version && runspace != null && runspace.Debugger != null)
			{
				this.serverRemoteDebugger = new ServerRemoteDebugger(this, runspace, runspace.Debugger);
				this.remoteHost.ServerDebugger = this.serverRemoteDebugger;
			}
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x000CE942 File Offset: 0x000CCB42
		private void DisposeRemoteDebugger()
		{
			if (this.serverRemoteDebugger != null)
			{
				this.serverRemoteDebugger.Dispose();
			}
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000CE958 File Offset: 0x000CCB58
		private PSDataCollection<PSObject> InvokeScript(Command cmdToRun, RunspaceCreatedEventArgs args)
		{
			cmdToRun.CommandOrigin = CommandOrigin.Internal;
			cmdToRun.MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
			PowerShell powerShell = PowerShell.Create();
			powerShell.AddCommand(cmdToRun).AddCommand("out-default");
			return this.InvokePowerShell(powerShell, args);
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x000CE994 File Offset: 0x000CCB94
		private PSDataCollection<PSObject> InvokePowerShell(PowerShell powershell, RunspaceCreatedEventArgs args)
		{
			HostInfo hostInfo = this.remoteHost.HostInfo;
			ServerPowerShellDriver serverPowerShellDriver = new ServerPowerShellDriver(powershell, null, true, Guid.Empty, this.InstanceId, this, args.Runspace.ApartmentState, hostInfo, RemoteStreamOptions.AddInvocationInfo, false, args.Runspace);
			IAsyncResult asyncResult = serverPowerShellDriver.Start();
			PSDataCollection<PSObject> result = powershell.EndInvoke(asyncResult);
			ArrayList arrayList = (ArrayList)powershell.Runspace.GetExecutionContext.DollarErrorVariable;
			if (arrayList.Count > 0)
			{
				ErrorRecord errorRecord = arrayList[0] as ErrorRecord;
				string text;
				if (errorRecord != null)
				{
					text = errorRecord.ToString();
				}
				else
				{
					Exception ex = arrayList[0] as Exception;
					if (ex != null)
					{
						text = ((ex.Message != null) ? ex.Message : string.Empty);
					}
					else
					{
						text = string.Empty;
					}
				}
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.StartupScriptThrewTerminatingError, new object[]
				{
					text
				});
			}
			return result;
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x000CEA78 File Offset: 0x000CCC78
		private void HandleRunspaceCreated(object sender, RunspaceCreatedEventArgs args)
		{
			this.ServerRemoteHost.Runspace = args.Runspace;
			if (SystemPolicy.GetSystemLockdownPolicy() == SystemEnforcementMode.Enforce && args.Runspace.ExecutionContext.LanguageMode != PSLanguageMode.NoLanguage)
			{
				args.Runspace.ExecutionContext.LanguageMode = PSLanguageMode.ConstrainedLanguage;
			}
			try
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				args.Runspace.ExecutionContext.EngineSessionState.SetLocation(folderPath);
			}
			catch (ArgumentException)
			{
			}
			catch (ProviderNotFoundException)
			{
			}
			catch (DriveNotFoundException)
			{
			}
			catch (ProviderInvocationException)
			{
			}
			this.InvokeStartupScripts(args);
			this.HandleRunspaceCreatedForTypeTable(sender, args);
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x000CEB34 File Offset: 0x000CCD34
		private void InvokeStartupScripts(RunspaceCreatedEventArgs args)
		{
			Command command = null;
			if (!string.IsNullOrEmpty(this.configData.StartupScript))
			{
				command = new Command(this.configData.StartupScript, false, false);
			}
			else if (!string.IsNullOrEmpty(this.configData.InitializationScriptForOutOfProcessRunspace))
			{
				command = new Command(this.configData.InitializationScriptForOutOfProcessRunspace, true, false);
			}
			if (command != null)
			{
				this.InvokeScript(command, args);
				if (this.localRunspacePool.RunspacePoolStateInfo.State == RunspacePoolState.Opening)
				{
					object value = args.Runspace.SessionStateProxy.PSVariable.GetValue("global:PSApplicationPrivateData");
					if (value != null)
					{
						this.applicationPrivateData = (PSPrimitiveDictionary)LanguagePrimitives.ConvertTo(value, typeof(PSPrimitiveDictionary), true, CultureInfo.InvariantCulture, null);
					}
				}
			}
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000CEBF0 File Offset: 0x000CCDF0
		private void HandleRunspacePoolStateChanged(object sender, RunspacePoolStateChangedEventArgs eventArgs)
		{
			RunspacePoolState state = eventArgs.RunspacePoolStateInfo.State;
			Exception reason = eventArgs.RunspacePoolStateInfo.Reason;
			switch (state)
			{
			case RunspacePoolState.Opened:
				this.SendApplicationPrivateDataToClient();
				this.dsHandler.SendStateInfoToClient(new RunspacePoolStateInfo(state, reason));
				return;
			case RunspacePoolState.Closed:
			case RunspacePoolState.Closing:
			case RunspacePoolState.Broken:
				this.dsHandler.SendStateInfoToClient(new RunspacePoolStateInfo(state, reason));
				return;
			default:
				return;
			}
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000CEC5B File Offset: 0x000CCE5B
		private void HandleRunspacePoolForwardEvent(object sender, PSEventArgs e)
		{
			if (e.ForwardEvent)
			{
				this.dsHandler.SendPSEventArgsToClient(e);
			}
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000CEC74 File Offset: 0x000CCE74
		private void HandleCreateAndInvokePowerShell(object sender, RemoteDataEventArgs<RemoteDataObject<PSObject>> eventArgs)
		{
			RemoteDataObject<PSObject> data = eventArgs.Data;
			HostInfo hostInfo = RemotingDecoder.GetHostInfo(data.Data);
			ApartmentState apartmentState = RemotingDecoder.GetApartmentState(data.Data);
			RemoteStreamOptions remoteStreamOptions = RemotingDecoder.GetRemoteStreamOptions(data.Data);
			PowerShell powerShell = RemotingDecoder.GetPowerShell(data.Data);
			bool noInput = RemotingDecoder.GetNoInput(data.Data);
			bool addToHistory = RemotingDecoder.GetAddToHistory(data.Data);
			bool flag = false;
			if (this.serverCapability.ProtocolVersion >= RemotingConstants.ProtocolVersionWin8RTM)
			{
				flag = RemotingDecoder.GetIsNested(data.Data);
			}
			if (this.serverRemoteDebugger != null)
			{
				bool flag2 = false;
				ServerRunspacePoolDriver.DebuggerCommandArgument debuggerCommandArgument;
				switch (ServerRunspacePoolDriver.PreProcessDebuggerCommand(powerShell.Commands, this.serverRemoteDebugger.IsActive, this.serverRemoteDebugger.IsRemote, out debuggerCommandArgument))
				{
				case ServerRunspacePoolDriver.PreProcessCommandResult.ValidNotProcessed:
					flag2 = true;
					break;
				case ServerRunspacePoolDriver.PreProcessCommandResult.SetDebuggerAction:
					this.serverRemoteDebugger.SetDebuggerAction(debuggerCommandArgument.ResumeAction.Value);
					flag2 = true;
					break;
				case ServerRunspacePoolDriver.PreProcessCommandResult.SetDebugMode:
					this.serverRemoteDebugger.SetDebugMode(debuggerCommandArgument.Mode.Value);
					flag2 = true;
					break;
				case ServerRunspacePoolDriver.PreProcessCommandResult.SetDebuggerStepMode:
					this.serverRemoteDebugger.SetDebuggerStepMode(debuggerCommandArgument.DebuggerStepEnabled.Value);
					flag2 = true;
					break;
				case ServerRunspacePoolDriver.PreProcessCommandResult.SetPreserveUnhandledBreakpointMode:
					this.serverRemoteDebugger.UnhandledBreakpointMode = debuggerCommandArgument.UnhandledBreakpointMode.Value;
					flag2 = true;
					break;
				}
				if (flag2)
				{
					ServerPowerShellDriver serverPowerShellDriver = new ServerPowerShellDriver(powerShell, null, noInput, data.PowerShellId, data.RunspacePoolId, this, apartmentState, hostInfo, remoteStreamOptions, addToHistory, null);
					serverPowerShellDriver.RunNoOpCommand();
					return;
				}
			}
			if (this.remoteHost.IsRunspacePushed)
			{
				if (this.serverRemoteDebugger != null)
				{
					this.serverRemoteDebugger.CheckDebuggerState();
				}
				this.StartPowerShellCommandOnPushedRunspace(powerShell, data.PowerShellId, data.RunspacePoolId, hostInfo, remoteStreamOptions, addToHistory);
				return;
			}
			if (flag)
			{
				if (this.localRunspacePool.GetMaxRunspaces() == 1)
				{
					if (this.driverNestedInvoker != null && this.driverNestedInvoker.IsActive)
					{
						if (!this.driverNestedInvoker.IsAvailable)
						{
							throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.CannotInvokeNestedCommandNestedCommandRunning, new object[0]));
						}
						powerShell.SetIsNested(true);
						ServerPowerShellDriver serverPowerShellDriver2 = new ServerPowerShellDriver(powerShell, null, noInput, data.PowerShellId, data.RunspacePoolId, this, apartmentState, hostInfo, remoteStreamOptions, addToHistory, this.rsToUseForSteppablePipeline);
						this.inputCollection = serverPowerShellDriver2.InputCollection;
						this.driverNestedInvoker.InvokeDriverAsync(serverPowerShellDriver2);
						return;
					}
					else
					{
						if (this.serverRemoteDebugger != null && this.serverRemoteDebugger.InBreakpoint && this.serverRemoteDebugger.IsPushed)
						{
							this.serverRemoteDebugger.StartPowerShellCommand(powerShell, data.PowerShellId, data.RunspacePoolId, this, apartmentState, this.remoteHost, hostInfo, remoteStreamOptions, addToHistory);
							return;
						}
						if (powerShell.Commands.Commands.Count == 1 && !powerShell.Commands.Commands[0].IsScript && (powerShell.Commands.Commands[0].CommandText.IndexOf("Get-PSDebuggerStopArgs", StringComparison.OrdinalIgnoreCase) != -1 || powerShell.Commands.Commands[0].CommandText.IndexOf("Set-PSDebuggerAction", StringComparison.OrdinalIgnoreCase) != -1))
						{
							throw new PSInvalidOperationException();
						}
						ServerPowerShellDataStructureHandler powerShellDataStructureHandler = this.dsHandler.GetPowerShellDataStructureHandler();
						if (powerShellDataStructureHandler != null)
						{
							powerShell.SetIsNested(false);
							ServerSteppablePipelineDriver serverSteppablePipelineDriver = new ServerSteppablePipelineDriver(powerShell, noInput, data.PowerShellId, data.RunspacePoolId, this, apartmentState, hostInfo, remoteStreamOptions, addToHistory, this.rsToUseForSteppablePipeline, this.eventSubscriber, this.inputCollection);
							serverSteppablePipelineDriver.Start();
							return;
						}
					}
				}
				powerShell.SetIsNested(false);
			}
			if (this.serverRemoteDebugger != null)
			{
				this.serverRemoteDebugger.CheckDebuggerState();
			}
			ServerPowerShellDriver serverPowerShellDriver3 = new ServerPowerShellDriver(powerShell, null, noInput, data.PowerShellId, data.RunspacePoolId, this, apartmentState, hostInfo, remoteStreamOptions, addToHistory, null);
			this.inputCollection = serverPowerShellDriver3.InputCollection;
			serverPowerShellDriver3.Start();
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000CF048 File Offset: 0x000CD248
		private bool DoesInitialSessionStateIncludeGetCommandWithListImportedSwitch()
		{
			if (this._initialSessionStateIncludesGetCommandWithListImportedSwitch == null)
			{
				lock (this._initialSessionStateIncludesGetCommandWithListImportedSwitchLock)
				{
					if (this._initialSessionStateIncludesGetCommandWithListImportedSwitch == null)
					{
						bool value = false;
						InitialSessionState initialSessionState = this.RunspacePool.InitialSessionState;
						if (initialSessionState != null)
						{
							IEnumerable<SessionStateCommandEntry> source = from entry in initialSessionState.Commands["Get-Command"]
							where entry.Visibility == SessionStateEntryVisibility.Public
							select entry;
							SessionStateFunctionEntry sessionStateFunctionEntry = source.OfType<SessionStateFunctionEntry>().FirstOrDefault<SessionStateFunctionEntry>();
							if (sessionStateFunctionEntry != null)
							{
								if (sessionStateFunctionEntry.ScriptBlock.ParameterMetadata.BindableParameters.ContainsKey("ListImported"))
								{
									value = true;
								}
							}
							else
							{
								SessionStateCmdletEntry sessionStateCmdletEntry = source.OfType<SessionStateCmdletEntry>().FirstOrDefault<SessionStateCmdletEntry>();
								if (sessionStateCmdletEntry != null && sessionStateCmdletEntry.ImplementingType.Equals(typeof(GetCommandCommand)))
								{
									value = true;
								}
							}
						}
						this._initialSessionStateIncludesGetCommandWithListImportedSwitch = new bool?(value);
					}
				}
			}
			return this._initialSessionStateIncludesGetCommandWithListImportedSwitch.Value;
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000CF160 File Offset: 0x000CD360
		private void HandleGetCommandMetadata(object sender, RemoteDataEventArgs<RemoteDataObject<PSObject>> eventArgs)
		{
			RemoteDataObject<PSObject> data = eventArgs.Data;
			PowerShell commandDiscoveryPipeline = RemotingDecoder.GetCommandDiscoveryPipeline(data.Data);
			if (this.DoesInitialSessionStateIncludeGetCommandWithListImportedSwitch())
			{
				commandDiscoveryPipeline.AddParameter("ListImported", true);
			}
			commandDiscoveryPipeline.AddParameter("ErrorAction", "SilentlyContinue").AddCommand("Measure-Object").AddCommand("Select-Object").AddParameter("Property", "Count");
			PowerShell commandDiscoveryPipeline2 = RemotingDecoder.GetCommandDiscoveryPipeline(data.Data);
			if (this.DoesInitialSessionStateIncludeGetCommandWithListImportedSwitch())
			{
				commandDiscoveryPipeline2.AddParameter("ListImported", true);
			}
			commandDiscoveryPipeline2.AddCommand("Select-Object").AddParameter("Property", new string[]
			{
				"Name",
				"Namespace",
				"HelpUri",
				"CommandType",
				"ResolvedCommandName",
				"OutputType",
				"Parameters"
			});
			HostInfo hostInfo = new HostInfo(null);
			hostInfo.UseRunspaceHost = true;
			ServerPowerShellDriver serverPowerShellDriver = new ServerPowerShellDriver(commandDiscoveryPipeline, commandDiscoveryPipeline2, true, data.PowerShellId, data.RunspacePoolId, this, ApartmentState.Unknown, hostInfo, (RemoteStreamOptions)0, false, null);
			serverPowerShellDriver.Start();
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000CF285 File Offset: 0x000CD485
		private void HandleHostResponseReceived(object sender, RemoteDataEventArgs<RemoteHostResponse> eventArgs)
		{
			this.remoteHost.ServerMethodExecutor.HandleRemoteHostResponseFromClient(eventArgs.Data);
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000CF2A0 File Offset: 0x000CD4A0
		private void HandleSetMaxRunspacesReceived(object sender, RemoteDataEventArgs<PSObject> eventArgs)
		{
			PSObject data = eventArgs.Data;
			int maxRunspaces = (int)((PSNoteProperty)data.Properties["MaxRunspaces"]).Value;
			long callId = (long)((PSNoteProperty)data.Properties["ci"]).Value;
			bool flag = this.localRunspacePool.SetMaxRunspaces(maxRunspaces);
			this.dsHandler.SendResponseToClient(callId, flag);
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x000CF314 File Offset: 0x000CD514
		private void HandleSetMinRunspacesReceived(object sender, RemoteDataEventArgs<PSObject> eventArgs)
		{
			PSObject data = eventArgs.Data;
			int minRunspaces = (int)((PSNoteProperty)data.Properties["MinRunspaces"]).Value;
			long callId = (long)((PSNoteProperty)data.Properties["ci"]).Value;
			bool flag = this.localRunspacePool.SetMinRunspaces(minRunspaces);
			this.dsHandler.SendResponseToClient(callId, flag);
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x000CF388 File Offset: 0x000CD588
		private void HandleGetAvailalbeRunspacesReceived(object sender, RemoteDataEventArgs<PSObject> eventArgs)
		{
			PSObject data = eventArgs.Data;
			long callId = (long)((PSNoteProperty)data.Properties["ci"]).Value;
			int availableRunspaces = this.localRunspacePool.GetAvailableRunspaces();
			this.dsHandler.SendResponseToClient(callId, availableRunspaces);
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000CF3DC File Offset: 0x000CD5DC
		private void HandleResetRunspaceState(object sender, RemoteDataEventArgs<PSObject> eventArgs)
		{
			long callId = (long)((PSNoteProperty)eventArgs.Data.Properties["ci"]).Value;
			bool flag = this.ResetRunspaceState();
			this.dsHandler.SendResponseToClient(callId, flag);
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000CF428 File Offset: 0x000CD628
		private bool ResetRunspaceState()
		{
			LocalRunspace localRunspace = this.rsToUseForSteppablePipeline as LocalRunspace;
			if (localRunspace == null || this.localRunspacePool.GetMaxRunspaces() > 1)
			{
				return false;
			}
			try
			{
				localRunspace.ResetRunspaceState();
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				return false;
			}
			return true;
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000CF47C File Offset: 0x000CD67C
		private void StartPowerShellCommandOnPushedRunspace(PowerShell powershell, Guid powershellId, Guid runspacePoolId, HostInfo hostInfo, RemoteStreamOptions streamOptions, bool addToHistory)
		{
			Runspace pushedRunspace = this.remoteHost.PushedRunspace;
			ServerPowerShellDriver serverPowerShellDriver = new ServerPowerShellDriver(powershell, null, true, powershellId, runspacePoolId, this, ApartmentState.MTA, hostInfo, streamOptions, addToHistory, pushedRunspace);
			try
			{
				serverPowerShellDriver.Start();
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				this.remoteHost.PopRunspace();
				throw;
			}
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000CF4D8 File Offset: 0x000CD6D8
		private static ServerRunspacePoolDriver.PreProcessCommandResult PreProcessDebuggerCommand(PSCommand commands, bool isDebuggerActive, bool isDebuggerRemote, out ServerRunspacePoolDriver.DebuggerCommandArgument commandArgument)
		{
			commandArgument = new ServerRunspacePoolDriver.DebuggerCommandArgument();
			ServerRunspacePoolDriver.PreProcessCommandResult result = ServerRunspacePoolDriver.PreProcessCommandResult.None;
			if (commands.Commands.Count == 0 || commands.Commands[0].IsScript)
			{
				return result;
			}
			Command command = commands.Commands[0];
			string commandText = command.CommandText;
			if (commandText.Equals("__Get-PSDebuggerStopArgs", StringComparison.OrdinalIgnoreCase))
			{
				if (!isDebuggerActive)
				{
					return ServerRunspacePoolDriver.PreProcessCommandResult.ValidNotProcessed;
				}
				ScriptBlock scriptBlock = ScriptBlock.Create("$host.Runspace.Debugger.GetDebuggerStopArgs()");
				scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
				commands.Clear();
				commands.AddCommand("Invoke-Command").AddParameter("ScriptBlock", scriptBlock).AddParameter("NoNewScope", true);
				result = ServerRunspacePoolDriver.PreProcessCommandResult.GetDebuggerStopArgs;
			}
			else if (commandText.Equals("__Set-PSDebuggerAction", StringComparison.OrdinalIgnoreCase))
			{
				if (!isDebuggerActive)
				{
					return ServerRunspacePoolDriver.PreProcessCommandResult.ValidNotProcessed;
				}
				if (command.Parameters == null || command.Parameters.Count == 0 || !command.Parameters[0].Name.Equals("ResumeAction", StringComparison.OrdinalIgnoreCase))
				{
					throw new PSArgumentException("ResumeAction");
				}
				DebuggerResumeAction? resumeAction = null;
				PSObject psobject = command.Parameters[0].Value as PSObject;
				if (psobject != null)
				{
					try
					{
						resumeAction = new DebuggerResumeAction?((DebuggerResumeAction)psobject.BaseObject);
					}
					catch (InvalidCastException)
					{
					}
				}
				if (resumeAction == null)
				{
					throw new PSArgumentException("ResumeAction");
				}
				commandArgument.ResumeAction = resumeAction;
				result = ServerRunspacePoolDriver.PreProcessCommandResult.SetDebuggerAction;
			}
			else if (commandText.Equals("__Set-PSDebugMode", StringComparison.OrdinalIgnoreCase))
			{
				if (command.Parameters == null || command.Parameters.Count == 0 || !command.Parameters[0].Name.Equals("Mode", StringComparison.OrdinalIgnoreCase))
				{
					throw new PSArgumentException("Mode");
				}
				DebugModes? mode = null;
				PSObject psobject2 = command.Parameters[0].Value as PSObject;
				if (psobject2 != null)
				{
					try
					{
						mode = new DebugModes?((DebugModes)psobject2.BaseObject);
					}
					catch (InvalidCastException)
					{
					}
				}
				if (mode == null)
				{
					throw new PSArgumentException("Mode");
				}
				commandArgument.Mode = mode;
				result = ServerRunspacePoolDriver.PreProcessCommandResult.SetDebugMode;
			}
			else if (commandText.Equals("__Set-PSDebuggerStepMode", StringComparison.OrdinalIgnoreCase))
			{
				if (command.Parameters == null || command.Parameters.Count == 0 || !command.Parameters[0].Name.Equals("Enabled", StringComparison.OrdinalIgnoreCase))
				{
					throw new PSArgumentException("Enabled");
				}
				bool value = (bool)command.Parameters[0].Value;
				commandArgument.DebuggerStepEnabled = new bool?(value);
				result = ServerRunspacePoolDriver.PreProcessCommandResult.SetDebuggerStepMode;
			}
			else if (commandText.Equals("__Set-PSUnhandledBreakpointMode", StringComparison.OrdinalIgnoreCase))
			{
				if (command.Parameters == null || command.Parameters.Count == 0 || !command.Parameters[0].Name.Equals("UnhandledBreakpointMode", StringComparison.OrdinalIgnoreCase))
				{
					throw new PSArgumentException("UnhandledBreakpointMode");
				}
				UnhandledBreakpointProcessingMode? unhandledBreakpointMode = null;
				PSObject psobject3 = command.Parameters[0].Value as PSObject;
				if (psobject3 != null)
				{
					try
					{
						unhandledBreakpointMode = new UnhandledBreakpointProcessingMode?((UnhandledBreakpointProcessingMode)psobject3.BaseObject);
					}
					catch (InvalidCastException)
					{
					}
				}
				if (unhandledBreakpointMode == null)
				{
					throw new PSArgumentException("Mode");
				}
				commandArgument.UnhandledBreakpointMode = unhandledBreakpointMode;
				result = ServerRunspacePoolDriver.PreProcessCommandResult.SetPreserveUnhandledBreakpointMode;
			}
			return result;
		}

		// Token: 0x0400120E RID: 4622
		private RunspacePool localRunspacePool;

		// Token: 0x0400120F RID: 4623
		private ConfigurationDataFromXML configData;

		// Token: 0x04001210 RID: 4624
		private PSPrimitiveDictionary applicationPrivateData;

		// Token: 0x04001211 RID: 4625
		private Guid clientRunspacePoolId;

		// Token: 0x04001212 RID: 4626
		private ServerRunspacePoolDataStructureHandler dsHandler;

		// Token: 0x04001213 RID: 4627
		private Dictionary<Guid, ServerPowerShellDriver> associatedShells = new Dictionary<Guid, ServerPowerShellDriver>();

		// Token: 0x04001214 RID: 4628
		private ServerDriverRemoteHost remoteHost;

		// Token: 0x04001215 RID: 4629
		private bool isClosed;

		// Token: 0x04001216 RID: 4630
		private RemoteSessionCapability serverCapability;

		// Token: 0x04001217 RID: 4631
		private Runspace rsToUseForSteppablePipeline;

		// Token: 0x04001218 RID: 4632
		private ServerSteppablePipelineSubscriber eventSubscriber = new ServerSteppablePipelineSubscriber();

		// Token: 0x04001219 RID: 4633
		private PSDataCollection<object> inputCollection;

		// Token: 0x0400121A RID: 4634
		private ServerRunspacePoolDriver.PowerShellDriverInvoker driverNestedInvoker;

		// Token: 0x0400121B RID: 4635
		private ServerRemoteDebugger serverRemoteDebugger;

		// Token: 0x0400121C RID: 4636
		private Version clientPSVersion;

		// Token: 0x0400121D RID: 4637
		internal EventHandler<EventArgs> Closed;

		// Token: 0x0400121E RID: 4638
		private bool? _initialSessionStateIncludesGetCommandWithListImportedSwitch;

		// Token: 0x0400121F RID: 4639
		private object _initialSessionStateIncludesGetCommandWithListImportedSwitchLock = new object();

		// Token: 0x0200030C RID: 780
		private enum PreProcessCommandResult
		{
			// Token: 0x04001222 RID: 4642
			None,
			// Token: 0x04001223 RID: 4643
			ValidNotProcessed,
			// Token: 0x04001224 RID: 4644
			GetDebuggerStopArgs,
			// Token: 0x04001225 RID: 4645
			SetDebuggerAction,
			// Token: 0x04001226 RID: 4646
			SetDebugMode,
			// Token: 0x04001227 RID: 4647
			SetDebuggerStepMode,
			// Token: 0x04001228 RID: 4648
			SetPreserveUnhandledBreakpointMode
		}

		// Token: 0x0200030D RID: 781
		private class DebuggerCommandArgument
		{
			// Token: 0x170008B5 RID: 2229
			// (get) Token: 0x06002501 RID: 9473 RVA: 0x000CF834 File Offset: 0x000CDA34
			// (set) Token: 0x06002502 RID: 9474 RVA: 0x000CF83C File Offset: 0x000CDA3C
			public DebugModes? Mode { get; set; }

			// Token: 0x170008B6 RID: 2230
			// (get) Token: 0x06002503 RID: 9475 RVA: 0x000CF845 File Offset: 0x000CDA45
			// (set) Token: 0x06002504 RID: 9476 RVA: 0x000CF84D File Offset: 0x000CDA4D
			public DebuggerResumeAction? ResumeAction { get; set; }

			// Token: 0x170008B7 RID: 2231
			// (get) Token: 0x06002505 RID: 9477 RVA: 0x000CF856 File Offset: 0x000CDA56
			// (set) Token: 0x06002506 RID: 9478 RVA: 0x000CF85E File Offset: 0x000CDA5E
			public bool? DebuggerStepEnabled { get; set; }

			// Token: 0x170008B8 RID: 2232
			// (get) Token: 0x06002507 RID: 9479 RVA: 0x000CF867 File Offset: 0x000CDA67
			// (set) Token: 0x06002508 RID: 9480 RVA: 0x000CF86F File Offset: 0x000CDA6F
			public UnhandledBreakpointProcessingMode? UnhandledBreakpointMode { get; set; }
		}

		// Token: 0x0200030E RID: 782
		private sealed class PowerShellDriverInvoker
		{
			// Token: 0x0600250A RID: 9482 RVA: 0x000CF880 File Offset: 0x000CDA80
			public PowerShellDriverInvoker()
			{
				this._invokePumpStack = new ConcurrentStack<ServerRunspacePoolDriver.PowerShellDriverInvoker.InvokePump>();
			}

			// Token: 0x170008B9 RID: 2233
			// (get) Token: 0x0600250B RID: 9483 RVA: 0x000CF893 File Offset: 0x000CDA93
			public bool IsActive
			{
				get
				{
					return !this._invokePumpStack.IsEmpty;
				}
			}

			// Token: 0x170008BA RID: 2234
			// (get) Token: 0x0600250C RID: 9484 RVA: 0x000CF8A4 File Offset: 0x000CDAA4
			public bool IsAvailable
			{
				get
				{
					ServerRunspacePoolDriver.PowerShellDriverInvoker.InvokePump invokePump;
					if (!this._invokePumpStack.TryPeek(out invokePump))
					{
						invokePump = null;
					}
					return invokePump != null && !invokePump.IsBusy;
				}
			}

			// Token: 0x0600250D RID: 9485 RVA: 0x000CF8D0 File Offset: 0x000CDAD0
			public void InvokeDriverAsync(ServerPowerShellDriver driver)
			{
				ServerRunspacePoolDriver.PowerShellDriverInvoker.InvokePump invokePump;
				if (!this._invokePumpStack.TryPeek(out invokePump))
				{
					throw new PSInvalidOperationException(RemotingErrorIdStrings.PowerShellInvokerInvalidState);
				}
				invokePump.Dispatch(driver);
			}

			// Token: 0x0600250E RID: 9486 RVA: 0x000CF900 File Offset: 0x000CDB00
			public void PushInvoker()
			{
				ServerRunspacePoolDriver.PowerShellDriverInvoker.InvokePump invokePump = new ServerRunspacePoolDriver.PowerShellDriverInvoker.InvokePump();
				this._invokePumpStack.Push(invokePump);
				invokePump.Start();
			}

			// Token: 0x0600250F RID: 9487 RVA: 0x000CF928 File Offset: 0x000CDB28
			public void PopInvoker()
			{
				ServerRunspacePoolDriver.PowerShellDriverInvoker.InvokePump invokePump;
				if (this._invokePumpStack.TryPop(out invokePump))
				{
					invokePump.Stop();
					return;
				}
				throw new PSInvalidOperationException(RemotingErrorIdStrings.CannotExitNestedPipeline);
			}

			// Token: 0x0400122D RID: 4653
			private ConcurrentStack<ServerRunspacePoolDriver.PowerShellDriverInvoker.InvokePump> _invokePumpStack;

			// Token: 0x0200030F RID: 783
			private sealed class InvokePump
			{
				// Token: 0x06002510 RID: 9488 RVA: 0x000CF955 File Offset: 0x000CDB55
				public InvokePump()
				{
					this._driverInvokeQueue = new Queue<ServerPowerShellDriver>();
					this._processDrivers = new ManualResetEvent(false);
					this._syncObject = new object();
				}

				// Token: 0x06002511 RID: 9489 RVA: 0x000CF980 File Offset: 0x000CDB80
				public void Start()
				{
					try
					{
						for (;;)
						{
							this._processDrivers.WaitOne();
							ServerPowerShellDriver serverPowerShellDriver = null;
							lock (this._syncObject)
							{
								if (this._stopPump)
								{
									break;
								}
								if (this._driverInvokeQueue.Count > 0)
								{
									serverPowerShellDriver = this._driverInvokeQueue.Dequeue();
								}
								if (this._driverInvokeQueue.Count == 0)
								{
									this._processDrivers.Reset();
								}
							}
							if (serverPowerShellDriver != null)
							{
								try
								{
									try
									{
										this._busy = true;
										serverPowerShellDriver.InvokeMain();
									}
									catch (Exception e)
									{
										CommandProcessorBase.CheckForSevereException(e);
									}
									continue;
								}
								finally
								{
									this._busy = false;
								}
								break;
							}
						}
					}
					finally
					{
						this._isDisposed = true;
						this._processDrivers.Dispose();
					}
				}

				// Token: 0x06002512 RID: 9490 RVA: 0x000CFA68 File Offset: 0x000CDC68
				public void Dispatch(ServerPowerShellDriver driver)
				{
					this.CheckDisposed();
					lock (this._syncObject)
					{
						this._driverInvokeQueue.Enqueue(driver);
						this._processDrivers.Set();
					}
				}

				// Token: 0x06002513 RID: 9491 RVA: 0x000CFAC0 File Offset: 0x000CDCC0
				public void Stop()
				{
					this.CheckDisposed();
					lock (this._syncObject)
					{
						this._stopPump = true;
						this._processDrivers.Set();
					}
				}

				// Token: 0x170008BB RID: 2235
				// (get) Token: 0x06002514 RID: 9492 RVA: 0x000CFB14 File Offset: 0x000CDD14
				public bool IsBusy
				{
					get
					{
						return this._busy;
					}
				}

				// Token: 0x06002515 RID: 9493 RVA: 0x000CFB1C File Offset: 0x000CDD1C
				private void CheckDisposed()
				{
					if (this._isDisposed)
					{
						throw new ObjectDisposedException("InvokePump");
					}
				}

				// Token: 0x0400122E RID: 4654
				private Queue<ServerPowerShellDriver> _driverInvokeQueue;

				// Token: 0x0400122F RID: 4655
				private ManualResetEvent _processDrivers;

				// Token: 0x04001230 RID: 4656
				private object _syncObject;

				// Token: 0x04001231 RID: 4657
				private bool _stopPump;

				// Token: 0x04001232 RID: 4658
				private bool _busy;

				// Token: 0x04001233 RID: 4659
				private bool _isDisposed;
			}
		}
	}
}
