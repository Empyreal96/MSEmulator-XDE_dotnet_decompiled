using System;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Server;
using System.Management.Automation.Runspaces;
using System.Security.Principal;
using System.Threading;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002FE RID: 766
	internal class ServerRemoteSession : RemoteSession
	{
		// Token: 0x0600240F RID: 9231 RVA: 0x000CA140 File Offset: 0x000C8340
		internal ServerRemoteSession(PSSenderInfo senderInfo, string configurationProviderId, string initializationParameters, AbstractServerSessionTransportManager transportManager)
		{
			NativeCommandProcessor.IsServerSide = true;
			this._senderInfo = senderInfo;
			this._configProviderId = configurationProviderId;
			this._initParameters = initializationParameters;
			this._cryptoHelper = (PSRemotingCryptoHelperServer)transportManager.CryptoHelper;
			this._cryptoHelper.Session = this;
			this._context = new ServerRemoteSessionContext();
			this._sessionDSHandler = new ServerRemoteSessionDSHandlerlImpl(this, transportManager);
			base.BaseSessionDataStructureHandler = this._sessionDSHandler;
			this._sessionDSHandler.CreateRunspacePoolReceived += this.HandleCreateRunspacePool;
			this._sessionDSHandler.NegotiationReceived += this.HandleNegotiationReceived;
			this._sessionDSHandler.SessionClosing += this.HandleSessionDSHandlerClosing;
			this._sessionDSHandler.PublicKeyReceived += this.HandlePublicKeyReceived;
			transportManager.Closing += this.HandleResourceClosing;
			transportManager.ReceivedDataCollection.MaximumReceivedObjectSize = new int?(10485760);
			transportManager.ReceivedDataCollection.MaximumReceivedDataSize = null;
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x000CA24C File Offset: 0x000C844C
		internal static ServerRemoteSession CreateServerRemoteSession(PSSenderInfo senderInfo, string configurationProviderId, string initializationParameters, AbstractServerSessionTransportManager transportManager)
		{
			ServerRemoteSession._trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "Finding InitialSessionState provider for id : {0}", new object[]
			{
				configurationProviderId
			}), new object[0]);
			if (string.IsNullOrEmpty(configurationProviderId))
			{
				throw PSTraceSource.NewInvalidOperationException("RemotingErrorIdStrings.NonExistentInitialSessionStateProvider", new object[]
				{
					configurationProviderId
				});
			}
			ServerRemoteSession serverRemoteSession = new ServerRemoteSession(senderInfo, configurationProviderId, initializationParameters, transportManager);
			RemoteSessionStateMachineEventArgs fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.CreateSession);
			serverRemoteSession._sessionDSHandler.StateMachine.RaiseEvent(fsmEventArg);
			return serverRemoteSession;
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x000CA2C8 File Offset: 0x000C84C8
		internal static ServerRemoteSession CreateServerRemoteSession(PSSenderInfo senderInfo, string initializationScriptForOutOfProcessRunspace, AbstractServerSessionTransportManager transportManager)
		{
			ServerRemoteSession serverRemoteSession = ServerRemoteSession.CreateServerRemoteSession(senderInfo, "Microsoft.PowerShell", "", transportManager);
			serverRemoteSession._initScriptForOutOfProcRS = initializationScriptForOutOfProcessRunspace;
			return serverRemoteSession;
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x000CA2EF File Offset: 0x000C84EF
		internal override RemotingDestination MySelf
		{
			get
			{
				return RemotingDestination.Server;
			}
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x000CA2F4 File Offset: 0x000C84F4
		internal void DispatchInputQueueData(object sender, RemoteDataEventArgs dataEventArg)
		{
			if (dataEventArg == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataEventArg");
			}
			RemoteDataObject<PSObject> receivedData = dataEventArg.ReceivedData;
			if (receivedData == null)
			{
				throw PSTraceSource.NewArgumentException("dataEventArg");
			}
			RemotingDestination destination = receivedData.Destination;
			if ((destination & this.MySelf) != this.MySelf)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.RemotingDestinationNotForMe, new object[]
				{
					this.MySelf,
					destination
				});
			}
			RemotingTargetInterface targetInterface = receivedData.TargetInterface;
			RemotingDataType dataType = receivedData.DataType;
			switch (targetInterface)
			{
			case RemotingTargetInterface.Session:
				switch (dataType)
				{
				case RemotingDataType.SessionCapability:
					this._sessionDSHandler.RaiseDataReceivedEvent(dataEventArg);
					return;
				case RemotingDataType.CloseSession:
					this._sessionDSHandler.RaiseDataReceivedEvent(dataEventArg);
					return;
				case RemotingDataType.CreateRunspacePool:
				{
					RemoteSessionStateMachineEventArgs remoteSessionStateMachineEventArgs = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.MessageReceived);
					if (this.SessionDataStructureHandler.StateMachine.CanByPassRaiseEvent(remoteSessionStateMachineEventArgs))
					{
						remoteSessionStateMachineEventArgs.RemoteData = receivedData;
						this.SessionDataStructureHandler.StateMachine.DoMessageReceived(this, remoteSessionStateMachineEventArgs);
						return;
					}
					this.SessionDataStructureHandler.StateMachine.RaiseEvent(remoteSessionStateMachineEventArgs);
					return;
				}
				case RemotingDataType.PublicKey:
					this._sessionDSHandler.RaiseDataReceivedEvent(dataEventArg);
					return;
				default:
					return;
				}
				break;
			case RemotingTargetInterface.RunspacePool:
			case RemotingTargetInterface.PowerShell:
			{
				RemoteSessionStateMachineEventArgs remoteSessionStateMachineEventArgs = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.MessageReceived);
				if (this.SessionDataStructureHandler.StateMachine.CanByPassRaiseEvent(remoteSessionStateMachineEventArgs))
				{
					remoteSessionStateMachineEventArgs.RemoteData = receivedData;
					this.SessionDataStructureHandler.StateMachine.DoMessageReceived(this, remoteSessionStateMachineEventArgs);
					return;
				}
				this.SessionDataStructureHandler.StateMachine.RaiseEvent(remoteSessionStateMachineEventArgs);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x000CA474 File Offset: 0x000C8674
		private void HandlePublicKeyReceived(object sender, RemoteDataEventArgs<string> eventArgs)
		{
			if (this.SessionDataStructureHandler.StateMachine.State == RemoteSessionState.Established || this.SessionDataStructureHandler.StateMachine.State == RemoteSessionState.EstablishedAndKeyRequested || this.SessionDataStructureHandler.StateMachine.State == RemoteSessionState.EstablishedAndKeyExchanged)
			{
				string data = eventArgs.Data;
				RemoteSessionStateMachineEventArgs fsmEventArg;
				if (!this._cryptoHelper.ImportRemotePublicKey(data))
				{
					fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeyReceiveFailed);
					this.SessionDataStructureHandler.StateMachine.RaiseEvent(fsmEventArg);
				}
				fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeyReceived);
				this.SessionDataStructureHandler.StateMachine.RaiseEvent(fsmEventArg);
			}
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x000CA508 File Offset: 0x000C8708
		internal override void StartKeyExchange()
		{
			if (this.SessionDataStructureHandler.StateMachine.State == RemoteSessionState.Established)
			{
				this.SessionDataStructureHandler.SendRequestForPublicKey();
				RemoteSessionStateMachineEventArgs fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeyRequested);
				this.SessionDataStructureHandler.StateMachine.RaiseEvent(fsmEventArg);
			}
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x000CA54D File Offset: 0x000C874D
		internal override void CompleteKeyExchange()
		{
			this._cryptoHelper.CompleteKeyExchange();
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x000CA55C File Offset: 0x000C875C
		internal void SendEncryptedSessionKey()
		{
			string encryptedSessionKey = null;
			RemoteSessionStateMachineEventArgs fsmEventArg;
			if (!this._cryptoHelper.ExportEncryptedSessionKey(out encryptedSessionKey))
			{
				fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeySendFailed);
				this.SessionDataStructureHandler.StateMachine.RaiseEvent(fsmEventArg);
			}
			this.SessionDataStructureHandler.SendEncryptedSessionKey(encryptedSessionKey);
			this.CompleteKeyExchange();
			fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.KeySent);
			this.SessionDataStructureHandler.StateMachine.RaiseEvent(fsmEventArg);
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x000CA5C2 File Offset: 0x000C87C2
		internal ServerRemoteSessionContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x000CA5CA File Offset: 0x000C87CA
		internal ServerRemoteSessionDataStructureHandler SessionDataStructureHandler
		{
			get
			{
				return this._sessionDSHandler;
			}
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x000CA5D2 File Offset: 0x000C87D2
		internal void Close(RemoteSessionStateMachineEventArgs reasonForClose)
		{
			this.Closed.SafeInvoke(this, reasonForClose);
			if (this._runspacePoolDriver != null)
			{
				ServerRunspacePoolDriver runspacePoolDriver = this._runspacePoolDriver;
				runspacePoolDriver.Closed = (EventHandler<EventArgs>)Delegate.Remove(runspacePoolDriver.Closed, new EventHandler<EventArgs>(this.HandleResourceClosing));
			}
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000CA638 File Offset: 0x000C8838
		internal void ExecuteConnect(byte[] connectData, out byte[] connectResponseData)
		{
			connectResponseData = null;
			Fragmentor fragmentor = new Fragmentor(int.MaxValue, null);
			Fragmentor defragmentor = fragmentor;
			int num = connectData.Length;
			if (num < 21)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			FragmentedRemoteObject.GetFragmentId(connectData, 0);
			bool isStartFragment = FragmentedRemoteObject.GetIsStartFragment(connectData, 0);
			bool isEndFragment = FragmentedRemoteObject.GetIsEndFragment(connectData, 0);
			int blobLength = FragmentedRemoteObject.GetBlobLength(connectData, 0);
			if (blobLength > num - 21)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			if (!isStartFragment || !isEndFragment)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			RemoteSessionState state2 = this.SessionDataStructureHandler.StateMachine.State;
			if (state2 != RemoteSessionState.Established && state2 != RemoteSessionState.EstablishedAndKeyExchanged)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnServerStateValidation);
			}
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(connectData, 21, blobLength);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			RemoteDataObject<PSObject> remoteDataObject = RemoteDataObject<PSObject>.CreateFrom(memoryStream, defragmentor);
			if (remoteDataObject == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			if (remoteDataObject.Destination != RemotingDestination.Server || remoteDataObject.DataType != RemotingDataType.SessionCapability)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			int num2 = num - 21 - blobLength;
			if (num2 < 21)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			byte[] array = new byte[num2];
			Array.Copy(connectData, 21 + blobLength, array, 0, num2);
			FragmentedRemoteObject.GetFragmentId(array, 0);
			isStartFragment = FragmentedRemoteObject.GetIsStartFragment(array, 0);
			isEndFragment = FragmentedRemoteObject.GetIsEndFragment(array, 0);
			blobLength = FragmentedRemoteObject.GetBlobLength(array, 0);
			if (blobLength != num2 - 21)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			if (!isStartFragment || !isEndFragment)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			memoryStream = new MemoryStream();
			memoryStream.Write(array, 21, blobLength);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			RemoteDataObject<PSObject> remoteDataObject2 = RemoteDataObject<PSObject>.CreateFrom(memoryStream, defragmentor);
			if (remoteDataObject2 == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnServerStateValidation);
			}
			if (remoteDataObject2.Destination != RemotingDestination.Server || remoteDataObject2.DataType != RemotingDataType.ConnectRunspacePool)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			RemoteSessionCapability sessionCapability;
			try
			{
				sessionCapability = RemotingDecoder.GetSessionCapability(remoteDataObject.Data);
			}
			catch (PSRemotingDataStructureException)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			try
			{
				this.RunServerNegotiationAlgorithm(sessionCapability, true);
			}
			catch (PSRemotingDataStructureException ex)
			{
				throw ex;
			}
			int num3 = -1;
			int num4 = -1;
			bool flag = false;
			if (remoteDataObject2.Data.Properties["MinRunspaces"] != null && remoteDataObject2.Data.Properties["MinRunspaces"] != null)
			{
				try
				{
					num3 = RemotingDecoder.GetMinRunspaces(remoteDataObject2.Data);
					num4 = RemotingDecoder.GetMaxRunspaces(remoteDataObject2.Data);
					flag = true;
				}
				catch (PSRemotingDataStructureException)
				{
					throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
				}
			}
			if (flag && (num3 == -1 || num4 == -1 || num3 > num4))
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			if (this._runspacePoolDriver == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnServerStateValidation);
			}
			if (remoteDataObject2.RunspacePoolId != this._runspacePoolDriver.InstanceId)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnInputValidation);
			}
			if (flag && this._runspacePoolDriver.RunspacePool.GetMaxRunspaces() != num4 && this._runspacePoolDriver.RunspacePool.GetMinRunspaces() != num3)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnMismatchedRunspacePoolProperties);
			}
			RemoteDataObject remoteDataObject3 = RemotingEncoder.GenerateServerSessionCapability(this._context.ServerCapability, this._runspacePoolDriver.InstanceId);
			RemoteDataObject remoteDataObject4 = RemotingEncoder.GenerateRunspacePoolInitData(this._runspacePoolDriver.InstanceId, this._runspacePoolDriver.RunspacePool.GetMaxRunspaces(), this._runspacePoolDriver.RunspacePool.GetMinRunspaces());
			SerializedDataStream serializedDataStream = new SerializedDataStream(4096);
			serializedDataStream.Enter();
			remoteDataObject3.Serialize(serializedDataStream, fragmentor);
			serializedDataStream.Exit();
			serializedDataStream.Enter();
			remoteDataObject4.Serialize(serializedDataStream, fragmentor);
			serializedDataStream.Exit();
			byte[] array2 = serializedDataStream.Read();
			serializedDataStream.Dispose();
			connectResponseData = array2;
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				RemoteSessionStateMachineEventArgs fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.ConnectSession);
				this._sessionDSHandler.StateMachine.RaiseEvent(fsmEventArg);
			});
			this._runspacePoolDriver.DataStructureHandler.ProcessConnect();
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x000CAA18 File Offset: 0x000C8C18
		internal void HandlePostConnect()
		{
			if (this._runspacePoolDriver != null)
			{
				this._runspacePoolDriver.SendApplicationPrivateDataToClient();
			}
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x000CAA44 File Offset: 0x000C8C44
		private void HandleCreateRunspacePool(object sender, RemoteDataEventArgs createRunspaceEventArg)
		{
			if (createRunspaceEventArg == null)
			{
				throw PSTraceSource.NewArgumentNullException("createRunspaceEventArg");
			}
			RemoteDataObject<PSObject> receivedData = createRunspaceEventArg.ReceivedData;
			if (this._context != null)
			{
				this._senderInfo.ClientTimeZone = this._context.ClientCapability.TimeZone;
			}
			this._senderInfo.ApplicationArguments = RemotingDecoder.GetApplicationArguments(receivedData.Data);
			ConfigurationDataFromXML configurationDataFromXML = PSSessionConfiguration.LoadEndPointConfiguration(this._configProviderId, this._initParameters);
			configurationDataFromXML.InitializationScriptForOutOfProcessRunspace = this._initScriptForOutOfProcRS;
			this.maxRecvdObjectSize = configurationDataFromXML.MaxReceivedObjectSizeMB;
			this.maxRecvdDataSizeCommand = configurationDataFromXML.MaxReceivedCommandSizeMB;
			if (string.IsNullOrEmpty(configurationDataFromXML.ConfigFilePath))
			{
				this._sessionConfigProvider = configurationDataFromXML.CreateEndPointConfigurationInstance();
			}
			else
			{
				WindowsPrincipal windowsPrincipal = new WindowsPrincipal(this._senderInfo.UserInfo.WindowsIdentity);
				if (windowsPrincipal.Identity.Name.Contains("Virtual Users"))
				{
					string[] array = this._senderInfo.UserInfo.Identity.Name.Split(new char[]
					{
						'\\'
					});
					if (array.Length == 2)
					{
						string sUserPrincipalName = array[1] + "@" + array[0];
						windowsPrincipal = new WindowsPrincipal(new WindowsIdentity(sUserPrincipalName));
					}
				}
				Func<string, bool> roleVerifier = (string role) => windowsPrincipal.IsInRole(role);
				DISCPowerShellConfiguration sessionConfigProvider = new DISCPowerShellConfiguration(configurationDataFromXML.ConfigFilePath, roleVerifier);
				this._sessionConfigProvider = sessionConfigProvider;
			}
			PSPrimitiveDictionary applicationPrivateData = this._sessionConfigProvider.GetApplicationPrivateData(this._senderInfo);
			InitialSessionState initialSessionState = null;
			if (configurationDataFromXML.SessionConfigurationData != null)
			{
				try
				{
					initialSessionState = this._sessionConfigProvider.GetInitialSessionState(configurationDataFromXML.SessionConfigurationData, this._senderInfo, this._configProviderId);
					goto IL_1C6;
				}
				catch (NotImplementedException)
				{
					initialSessionState = this._sessionConfigProvider.GetInitialSessionState(this._senderInfo);
					goto IL_1C6;
				}
			}
			initialSessionState = this._sessionConfigProvider.GetInitialSessionState(this._senderInfo);
			IL_1C6:
			if (initialSessionState == null)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.InitialSessionStateNull, new object[]
				{
					this._configProviderId
				});
			}
			initialSessionState.ThrowOnRunspaceOpenError = true;
			initialSessionState.Variables.Add(new SessionStateVariableEntry("PSSenderInfo", this._senderInfo, PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.PSSenderInfoDescription, new object[0]), ScopedItemOptions.ReadOnly));
			Version psClientVersion = null;
			if (this._senderInfo.ApplicationArguments != null && this._senderInfo.ApplicationArguments.ContainsKey("PSversionTable"))
			{
				PSPrimitiveDictionary psprimitiveDictionary = PSObject.Base(this._senderInfo.ApplicationArguments["PSversionTable"]) as PSPrimitiveDictionary;
				if (psprimitiveDictionary != null)
				{
					if (psprimitiveDictionary.ContainsKey("WSManStackVersion"))
					{
						Version version = PSObject.Base(psprimitiveDictionary["WSManStackVersion"]) as Version;
						if (version != null && version.Major < 3)
						{
							initialSessionState.Commands.Add(new SessionStateFunctionEntry("TabExpansion", "\r\n            param($line, $lastWord)\r\n            & {\r\n                function Write-Members ($sep='.')\r\n                {\r\n                    Invoke-Expression ('$_val=' + $_expression)\r\n\r\n                    $_method = [Management.Automation.PSMemberTypes] `\r\n                        'Method,CodeMethod,ScriptMethod,ParameterizedProperty'\r\n                    if ($sep -eq '.')\r\n                    {\r\n                        $params = @{view = 'extended','adapted','base'}\r\n                    }\r\n                    else\r\n                    {\r\n                        $params = @{static=$true}\r\n                    }\r\n        \r\n                    foreach ($_m in ,$_val | Get-Member @params $_pat |\r\n                        Sort-Object membertype,name)\r\n                    {\r\n                        if ($_m.MemberType -band $_method)\r\n                        {\r\n                            # Return a method...\r\n                            $_base + $_expression + $sep + $_m.name + '('\r\n                        }\r\n                        else {\r\n                            # Return a property...\r\n                            $_base + $_expression + $sep + $_m.name\r\n                        }\r\n                    }\r\n                }\r\n\r\n                # If a command name contains any of these chars, it needs to be quoted\r\n                $_charsRequiringQuotes = ('`&@''#{}()$,;|<> ' + \"`t\").ToCharArray()\r\n\r\n                # If a variable name contains any of these characters it needs to be in braces\r\n                $_varsRequiringQuotes = ('-`&@''#{}()$,;|<> .\\/' + \"`t\").ToCharArray()\r\n\r\n                switch -regex ($lastWord)\r\n                {\r\n                    # Handle property and method expansion rooted at variables...\r\n                    # e.g. $a.b.<tab>\r\n                    '(^.*)(\\$(\\w|:|\\.)+)\\.([*\\w]*)$' {\r\n                        $_base = $matches[1]\r\n                        $_expression = $matches[2]\r\n                        $_pat = $matches[4] + '*'\r\n                        Write-Members\r\n                        break;\r\n                    }\r\n\r\n                    # Handle simple property and method expansion on static members...\r\n                    # e.g. [datetime]::n<tab>\r\n                    '(^.*)(\\[(\\w|\\.|\\+)+\\])(\\:\\:|\\.){0,1}([*\\w]*)$' {\r\n                        $_base = $matches[1]\r\n                        $_expression = $matches[2]\r\n                        $_pat = $matches[5] + '*'\r\n                        Write-Members $(if (! $matches[4]) {'::'} else {$matches[4]})\r\n                        break;\r\n                    }\r\n\r\n                    # Handle complex property and method expansion on static members\r\n                    # where there are intermediate properties...\r\n                    # e.g. [datetime]::now.d<tab>\r\n                    '(^.*)(\\[(\\w|\\.|\\+)+\\](\\:\\:|\\.)(\\w+\\.)+)([*\\w]*)$' {\r\n                        $_base = $matches[1]  # everything before the expression\r\n                        $_expression = $matches[2].TrimEnd('.') # expression less trailing '.'\r\n                        $_pat = $matches[6] + '*'  # the member to look for...\r\n                        Write-Members\r\n                        break;\r\n                    }\r\n\r\n                    # Handle variable name expansion...\r\n                    '(^.*\\$)([*\\w:]+)$' {\r\n                        $_prefix = $matches[1]\r\n                        $_varName = $matches[2]\r\n                        $_colonPos = $_varname.IndexOf(':')\r\n                        if ($_colonPos -eq -1)\r\n                        {\r\n                            $_varName = 'variable:' + $_varName\r\n                            $_provider = ''\r\n                        }\r\n                        else\r\n                        {\r\n                            $_provider = $_varname.Substring(0, $_colonPos+1)\r\n                        }\r\n\r\n                        foreach ($_v in Get-ChildItem ($_varName + '*') | sort Name)\r\n                        { \r\n                            $_nameFound = $_v.name\r\n                            $(if ($_nameFound.IndexOfAny($_varsRequiringQuotes) -eq -1) {'{0}{1}{2}'}\r\n                            else {'{0}{{{1}{2}}}'}) -f $_prefix, $_provider, $_nameFound\r\n                        }\r\n                        break;\r\n                    }\r\n\r\n                    # Do completion on parameters...\r\n                    '^-([*\\w0-9]*)' {\r\n                        $_pat = $matches[1] + '*'\r\n\r\n                        # extract the command name from the string\r\n                        # first split the string into statements and pipeline elements\r\n                        # This doesn't handle strings however.\r\n                        $_command = [regex]::Split($line, '[|;=]')[-1]\r\n\r\n                        #  Extract the trailing unclosed block e.g. ls | foreach { cp\r\n                        if ($_command -match '\\{([^\\{\\}]*)$')\r\n                        {\r\n                            $_command = $matches[1]\r\n                        }\r\n\r\n                        # Extract the longest unclosed parenthetical expression...\r\n                        if ($_command -match '\\(([^()]*)$')\r\n                        {\r\n                            $_command = $matches[1]\r\n                        }\r\n\r\n                        # take the first space separated token of the remaining string\r\n                        # as the command to look up. Trim any leading or trailing spaces\r\n                        # so you don't get leading empty elements.\r\n                        $_command = $_command.TrimEnd('-')\r\n                        $_command,$_arguments = $_command.Trim().Split()\r\n\r\n                        # now get the info object for it, -ArgumentList will force aliases to be resolved\r\n                        # it also retrieves dynamic parameters\r\n                        try\r\n                        {\r\n                            $_command = @(Get-Command -type 'Alias,Cmdlet,Function,Filter,ExternalScript' `\r\n                                -Name $_command -ArgumentList $_arguments)[0]\r\n                        }\r\n                        catch\r\n                        {\r\n                            # see if the command is an alias. If so, resolve it to the real command\r\n                            if(Test-Path alias:\\$_command)\r\n                            {\r\n                                $_command = @(Get-Command -Type Alias $_command)[0].Definition\r\n                            }\r\n\r\n                            # If we were unsuccessful retrieving the command, try again without the parameters\r\n                            $_command = @(Get-Command -type 'Cmdlet,Function,Filter,ExternalScript' `\r\n                                -Name $_command)[0]\r\n                        }\r\n\r\n                        # remove errors generated by the command not being found, and break\r\n                        if(-not $_command) { $error.RemoveAt(0); break; }\r\n\r\n                        # expand the parameter sets and emit the matching elements\r\n                        # need to use psbase.Keys in case 'keys' is one of the parameters\r\n                        # to the cmdlet\r\n                        foreach ($_n in $_command.Parameters.psbase.Keys)\r\n                        {\r\n                            if ($_n -like $_pat) { '-' + $_n }\r\n                        }\r\n                        break;\r\n                    }\r\n\r\n                    # Tab complete against history either #<pattern> or #<id>\r\n                    '^#(\\w*)' {\r\n                        $_pattern = $matches[1]\r\n                        if ($_pattern -match '^[0-9]+$')\r\n                        {\r\n                            Get-History -ea SilentlyContinue -Id $_pattern | Foreach { $_.CommandLine } \r\n                        }\r\n                        else\r\n                        {\r\n                            $_pattern = '*' + $_pattern + '*'\r\n                            Get-History -Count 32767 | Sort-Object -Descending Id| Foreach { $_.CommandLine } | where { $_ -like $_pattern }\r\n                        }\r\n                        break;\r\n                    }\r\n\r\n                    # try to find a matching command...\r\n                    default {\r\n                        # parse the script...\r\n                        $_tokens = [System.Management.Automation.PSParser]::Tokenize($line,\r\n                            [ref] $null)\r\n\r\n                        if ($_tokens)\r\n                        {\r\n                            $_lastToken = $_tokens[$_tokens.count - 1]\r\n                            if ($_lastToken.Type -eq 'Command')\r\n                            {\r\n                                $_cmd = $_lastToken.Content\r\n\r\n                                # don't look for paths...\r\n                                if ($_cmd.IndexOfAny('/\\:') -eq -1)\r\n                                {\r\n                                    # handle parsing errors - the last token string should be the last\r\n                                    # string in the line...\r\n                                    if ($lastword.Length -ge $_cmd.Length -and \r\n                                        $lastword.substring($lastword.length-$_cmd.length) -eq $_cmd)\r\n                                    {\r\n                                        $_pat = $_cmd + '*'\r\n                                        $_base = $lastword.substring(0, $lastword.length-$_cmd.length)\r\n\r\n                                        # get files in current directory first, then look for commands...\r\n                                        $( try {Resolve-Path -ea SilentlyContinue -Relative $_pat } catch {} ;\r\n                                           try { $ExecutionContext.InvokeCommand.GetCommandName($_pat, $true, $false) |\r\n                                               Sort-Object -Unique } catch {} ) |\r\n                                                   # If the command contains non-word characters (space, ) ] ; ) etc.)\r\n                                                   # then it needs to be quoted and prefixed with &\r\n                                                   ForEach-Object {\r\n                                                        if ($_.IndexOfAny($_charsRequiringQuotes) -eq -1) { $_ }\r\n                                                        elseif ($_.IndexOf('''') -ge 0) {'& ''{0}''' -f $_.Replace('''','''''') }\r\n                                                        else { '& ''{0}''' -f $_ }} |\r\n                                                   ForEach-Object {'{0}{1}' -f $_base,$_ }\r\n                                    }\r\n                                }\r\n                            }\r\n                        }\r\n                    }\r\n                }\r\n            }\r\n        "));
						}
					}
					if (psprimitiveDictionary.ContainsKey("PSVersion"))
					{
						psClientVersion = (PSObject.Base(psprimitiveDictionary["PSVersion"]) as Version);
					}
				}
			}
			if (!string.IsNullOrEmpty(configurationDataFromXML.EndPointConfigurationTypeName))
			{
				this.maxRecvdObjectSize = this._sessionConfigProvider.GetMaximumReceivedObjectSize(this._senderInfo);
				this.maxRecvdDataSizeCommand = this._sessionConfigProvider.GetMaximumReceivedDataSizePerCommand(this._senderInfo);
			}
			this._sessionDSHandler.TransportManager.ReceivedDataCollection.MaximumReceivedObjectSize = this.maxRecvdObjectSize;
			Guid runspacePoolId = receivedData.RunspacePoolId;
			int minRunspaces = RemotingDecoder.GetMinRunspaces(receivedData.Data);
			int maxRunspaces = RemotingDecoder.GetMaxRunspaces(receivedData.Data);
			PSThreadOptions threadOptions = RemotingDecoder.GetThreadOptions(receivedData.Data);
			ApartmentState apartmentState = RemotingDecoder.GetApartmentState(receivedData.Data);
			HostInfo hostInfo = RemotingDecoder.GetHostInfo(receivedData.Data);
			if (this._runspacePoolDriver != null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.RunspaceAlreadyExists, new object[]
				{
					this._runspacePoolDriver.InstanceId
				});
			}
			bool isAdministrator = this._senderInfo.UserInfo.IsInRole(WindowsBuiltInRole.Administrator);
			ServerRunspacePoolDriver value = new ServerRunspacePoolDriver(runspacePoolId, minRunspaces, maxRunspaces, threadOptions, apartmentState, hostInfo, initialSessionState, applicationPrivateData, configurationDataFromXML, this.SessionDataStructureHandler.TransportManager, isAdministrator, this._context.ServerCapability, psClientVersion);
			Interlocked.Exchange<ServerRunspacePoolDriver>(ref this._runspacePoolDriver, value);
			ServerRunspacePoolDriver runspacePoolDriver = this._runspacePoolDriver;
			runspacePoolDriver.Closed = (EventHandler<EventArgs>)Delegate.Combine(runspacePoolDriver.Closed, new EventHandler<EventArgs>(this.HandleResourceClosing));
			this._runspacePoolDriver.Start();
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x000CAEA8 File Offset: 0x000C90A8
		private void HandleNegotiationReceived(object sender, RemoteSessionNegotiationEventArgs negotiationEventArg)
		{
			if (negotiationEventArg == null)
			{
				throw PSTraceSource.NewArgumentNullException("negotiationEventArg");
			}
			try
			{
				this._context.ClientCapability = negotiationEventArg.RemoteSessionCapability;
				this.RunServerNegotiationAlgorithm(negotiationEventArg.RemoteSessionCapability, false);
				RemoteSessionStateMachineEventArgs fsmEventArg = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationSending);
				this._sessionDSHandler.StateMachine.RaiseEvent(fsmEventArg);
				RemoteSessionStateMachineEventArgs fsmEventArg2 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationCompleted);
				this._sessionDSHandler.StateMachine.RaiseEvent(fsmEventArg2);
			}
			catch (PSRemotingDataStructureException reason)
			{
				RemoteSessionStateMachineEventArgs fsmEventArg3 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationSending);
				this._sessionDSHandler.StateMachine.RaiseEvent(fsmEventArg3);
				RemoteSessionStateMachineEventArgs fsmEventArg4 = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.NegotiationFailed, reason);
				this._sessionDSHandler.StateMachine.RaiseEvent(fsmEventArg4);
			}
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x000CAF5C File Offset: 0x000C915C
		private void HandleSessionDSHandlerClosing(object sender, EventArgs eventArgs)
		{
			if (this._runspacePoolDriver != null)
			{
				this._runspacePoolDriver.Close();
			}
			if (this._sessionConfigProvider != null)
			{
				this._sessionConfigProvider.Dispose();
				this._sessionConfigProvider = null;
			}
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000CAF8C File Offset: 0x000C918C
		private void HandleResourceClosing(object sender, EventArgs args)
		{
			RemoteSessionStateMachineEventArgs remoteSessionStateMachineEventArgs = new RemoteSessionStateMachineEventArgs(RemoteSessionEvent.Close);
			remoteSessionStateMachineEventArgs.RemoteData = null;
			this._sessionDSHandler.StateMachine.RaiseEvent(remoteSessionStateMachineEventArgs);
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000CAFBC File Offset: 0x000C91BC
		private bool RunServerNegotiationAlgorithm(RemoteSessionCapability clientCapability, bool onConnect)
		{
			Version protocolVersion = clientCapability.ProtocolVersion;
			Version version = this._context.ServerCapability.ProtocolVersion;
			if (onConnect)
			{
				if (protocolVersion == RemotingConstants.ProtocolVersionWin8RTM && version == RemotingConstants.ProtocolVersionWin10RTM)
				{
					version = RemotingConstants.ProtocolVersionWin8RTM;
					this._context.ServerCapability.ProtocolVersion = version;
				}
				else if (protocolVersion != version)
				{
					PSRemotingDataStructureException ex = new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerConnectFailedOnNegotiation, new object[]
					{
						"protocolversion",
						protocolVersion,
						PSVersionInfo.BuildVersion,
						RemotingConstants.ProtocolVersion
					});
					throw ex;
				}
			}
			else
			{
				if (protocolVersion == RemotingConstants.ProtocolVersionWin8RTM && version == RemotingConstants.ProtocolVersionWin10RTM)
				{
					version = RemotingConstants.ProtocolVersionWin8RTM;
					this._context.ServerCapability.ProtocolVersion = version;
				}
				if (protocolVersion == RemotingConstants.ProtocolVersionWin7RTM && (version == RemotingConstants.ProtocolVersionWin8RTM || version == RemotingConstants.ProtocolVersionWin10RTM))
				{
					version = RemotingConstants.ProtocolVersionWin7RTM;
					this._context.ServerCapability.ProtocolVersion = version;
				}
				if (protocolVersion == RemotingConstants.ProtocolVersionWin7RC && (version == RemotingConstants.ProtocolVersionWin7RTM || version == RemotingConstants.ProtocolVersionWin8RTM || version == RemotingConstants.ProtocolVersionWin10RTM))
				{
					version = RemotingConstants.ProtocolVersionWin7RC;
					this._context.ServerCapability.ProtocolVersion = version;
				}
				if (protocolVersion.Major != version.Major || protocolVersion.Minor < version.Minor)
				{
					PSRemotingDataStructureException ex2 = new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerNegotiationFailed, new object[]
					{
						"protocolversion",
						protocolVersion,
						PSVersionInfo.BuildVersion,
						RemotingConstants.ProtocolVersion
					});
					throw ex2;
				}
			}
			Version psversion = clientCapability.PSVersion;
			Version psversion2 = this._context.ServerCapability.PSVersion;
			if (psversion.Major != psversion2.Major || psversion.Minor < psversion2.Minor)
			{
				PSRemotingDataStructureException ex3 = new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerNegotiationFailed, new object[]
				{
					"PSVersion",
					psversion,
					PSVersionInfo.BuildVersion,
					RemotingConstants.ProtocolVersion
				});
				throw ex3;
			}
			Version serializationVersion = clientCapability.SerializationVersion;
			Version serializationVersion2 = this._context.ServerCapability.SerializationVersion;
			if (serializationVersion.Major != serializationVersion2.Major || serializationVersion.Minor < serializationVersion2.Minor)
			{
				PSRemotingDataStructureException ex4 = new PSRemotingDataStructureException(RemotingErrorIdStrings.ServerNegotiationFailed, new object[]
				{
					"SerializationVersion",
					serializationVersion,
					PSVersionInfo.BuildVersion,
					RemotingConstants.ProtocolVersion
				});
				throw ex4;
			}
			return true;
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x000CB253 File Offset: 0x000C9453
		internal ServerRunspacePoolDriver GetRunspacePoolDriver(Guid clientRunspacePoolId)
		{
			if (this._runspacePoolDriver == null)
			{
				return null;
			}
			if (this._runspacePoolDriver.InstanceId == clientRunspacePoolId)
			{
				return this._runspacePoolDriver;
			}
			return null;
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x000CB27A File Offset: 0x000C947A
		internal void ApplyQuotaOnCommandTransportManager(AbstractServerTransportManager cmdTransportManager)
		{
			cmdTransportManager.ReceivedDataCollection.MaximumReceivedDataSize = this.maxRecvdDataSizeCommand;
			cmdTransportManager.ReceivedDataCollection.MaximumReceivedObjectSize = this.maxRecvdObjectSize;
		}

		// Token: 0x040011C3 RID: 4547
		[TraceSource("ServerRemoteSession", "ServerRemoteSession")]
		private static PSTraceSource _trace = PSTraceSource.GetTracer("ServerRemoteSession", "ServerRemoteSession");

		// Token: 0x040011C4 RID: 4548
		private ServerRemoteSessionContext _context;

		// Token: 0x040011C5 RID: 4549
		private ServerRemoteSessionDataStructureHandler _sessionDSHandler;

		// Token: 0x040011C6 RID: 4550
		private PSSenderInfo _senderInfo;

		// Token: 0x040011C7 RID: 4551
		private string _configProviderId;

		// Token: 0x040011C8 RID: 4552
		private string _initParameters;

		// Token: 0x040011C9 RID: 4553
		private string _initScriptForOutOfProcRS;

		// Token: 0x040011CA RID: 4554
		private PSSessionConfiguration _sessionConfigProvider;

		// Token: 0x040011CB RID: 4555
		private int? maxRecvdObjectSize;

		// Token: 0x040011CC RID: 4556
		private int? maxRecvdDataSizeCommand;

		// Token: 0x040011CD RID: 4557
		private ServerRunspacePoolDriver _runspacePoolDriver;

		// Token: 0x040011CE RID: 4558
		private PSRemotingCryptoHelperServer _cryptoHelper;

		// Token: 0x040011CF RID: 4559
		internal EventHandler<RemoteSessionStateMachineEventArgs> Closed;
	}
}
