using System;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;
using System.Management.Automation.Tracing;
using System.Threading;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020002DC RID: 732
	internal static class RemotingEncoder
	{
		// Token: 0x060022C4 RID: 8900 RVA: 0x000C3C44 File Offset: 0x000C1E44
		internal static void AddNoteProperty<T>(PSObject pso, string propertyName, RemotingEncoder.ValueGetterDelegate<T> valueGetter)
		{
			T t = default(T);
			try
			{
				t = valueGetter();
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_PropertyGetterFailed, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
				{
					propertyName,
					(valueGetter.Target == null) ? string.Empty : valueGetter.Target.GetType().FullName,
					ex.ToString(),
					(ex.InnerException == null) ? string.Empty : ex.InnerException.ToString()
				});
			}
			try
			{
				pso.Properties.Add(new PSNoteProperty(propertyName, t));
			}
			catch (ExtendedTypeSystemException)
			{
				object value = pso.Properties[propertyName].Value;
			}
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x000C3D20 File Offset: 0x000C1F20
		internal static PSObject CreateEmptyPSObject()
		{
			return new PSObject
			{
				InternalTypeNames = ConsolidatedString.Empty
			};
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x000C3D3F File Offset: 0x000C1F3F
		private static PSNoteProperty CreateHostInfoProperty(HostInfo hostInfo)
		{
			return new PSNoteProperty("HostInfo", RemoteHostEncoder.EncodeObject(hostInfo));
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x000C3D54 File Offset: 0x000C1F54
		internal static RemoteDataObject GenerateCreateRunspacePool(Guid clientRunspacePoolId, int minRunspaces, int maxRunspaces, RemoteRunspacePoolInternal runspacePool, PSHost host, PSPrimitiveDictionary applicationArguments)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("MinRunspaces", minRunspaces));
			psobject.Properties.Add(new PSNoteProperty("MaxRunspaces", maxRunspaces));
			psobject.Properties.Add(new PSNoteProperty("PSThreadOptions", runspacePool.ThreadOptions));
			ApartmentState apartmentState = runspacePool.ApartmentState;
			psobject.Properties.Add(new PSNoteProperty("ApartmentState", apartmentState));
			psobject.Properties.Add(new PSNoteProperty("ApplicationArguments", applicationArguments));
			psobject.Properties.Add(RemotingEncoder.CreateHostInfoProperty(new HostInfo(host)));
			return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.CreateRunspacePool, clientRunspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000C3E20 File Offset: 0x000C2020
		internal static RemoteDataObject GenerateConnectRunspacePool(Guid clientRunspacePoolId, int minRunspaces, int maxRunspaces)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			int num = 0;
			if (minRunspaces != -1)
			{
				psobject.Properties.Add(new PSNoteProperty("MinRunspaces", minRunspaces));
				num++;
			}
			if (maxRunspaces != -1)
			{
				psobject.Properties.Add(new PSNoteProperty("MaxRunspaces", maxRunspaces));
				num++;
			}
			if (num > 0)
			{
				return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.ConnectRunspacePool, clientRunspacePoolId, Guid.Empty, psobject);
			}
			return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.ConnectRunspacePool, clientRunspacePoolId, Guid.Empty, string.Empty);
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000C3EA8 File Offset: 0x000C20A8
		internal static RemoteDataObject GenerateRunspacePoolInitData(Guid runspacePoolId, int minRunspaces, int maxRunspaces)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("MinRunspaces", minRunspaces));
			psobject.Properties.Add(new PSNoteProperty("MaxRunspaces", maxRunspaces));
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.RunspacePoolInitData, runspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000C3F04 File Offset: 0x000C2104
		internal static RemoteDataObject GenerateSetMaxRunspaces(Guid clientRunspacePoolId, int maxRunspaces, long callId)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("MaxRunspaces", maxRunspaces));
			psobject.Properties.Add(new PSNoteProperty("ci", callId));
			return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.SetMaxRunspaces, clientRunspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000C3F60 File Offset: 0x000C2160
		internal static RemoteDataObject GenerateSetMinRunspaces(Guid clientRunspacePoolId, int minRunspaces, long callId)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("MinRunspaces", minRunspaces));
			psobject.Properties.Add(new PSNoteProperty("ci", callId));
			return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.SetMinRunspaces, clientRunspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000C3FBC File Offset: 0x000C21BC
		internal static RemoteDataObject GenerateRunspacePoolOperationResponse(Guid clientRunspacePoolId, object response, long callId)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("SetMinMaxRunspacesResponse", response));
			psobject.Properties.Add(new PSNoteProperty("ci", callId));
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.RunspacePoolOperationResponse, clientRunspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000C4014 File Offset: 0x000C2214
		internal static RemoteDataObject GenerateGetAvailableRunspaces(Guid clientRunspacePoolId, long callId)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("ci", callId));
			return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.AvailableRunspaces, clientRunspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000C4054 File Offset: 0x000C2254
		internal static RemoteDataObject GenerateMyPublicKey(Guid runspacePoolId, string publicKey, RemotingDestination destination)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("PublicKey", publicKey));
			return RemoteDataObject.CreateFrom(destination, RemotingDataType.PublicKey, runspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000C408F File Offset: 0x000C228F
		internal static RemoteDataObject GeneratePublicKeyRequest(Guid runspacePoolId)
		{
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.PublicKeyRequest, runspacePoolId, Guid.Empty, string.Empty);
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x000C40A8 File Offset: 0x000C22A8
		internal static RemoteDataObject GenerateEncryptedSessionKeyResponse(Guid runspacePoolId, string encryptedSessionKey)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("EncryptedSessionKey", encryptedSessionKey));
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.EncryptedSessionKey, runspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x000C40E4 File Offset: 0x000C22E4
		internal static RemoteDataObject GenerateGetCommandMetadata(ClientRemotePowerShell shell)
		{
			Command command = null;
			foreach (Command command2 in shell.PowerShell.Commands.Commands)
			{
				if (command2.CommandText.Equals("Get-Command", StringComparison.OrdinalIgnoreCase))
				{
					command = command2;
					break;
				}
			}
			string[] value = null;
			CommandTypes commandTypes = CommandTypes.Alias | CommandTypes.Function | CommandTypes.Filter | CommandTypes.Cmdlet | CommandTypes.Configuration;
			string[] value2 = null;
			ModuleSpecification[] value3 = null;
			object[] value4 = null;
			foreach (CommandParameter commandParameter in command.Parameters)
			{
				if (commandParameter.Name.Equals("Name", StringComparison.OrdinalIgnoreCase))
				{
					value = (string[])LanguagePrimitives.ConvertTo(commandParameter.Value, typeof(string[]), CultureInfo.InvariantCulture);
				}
				else if (commandParameter.Name.Equals("CommandType", StringComparison.OrdinalIgnoreCase))
				{
					commandTypes = (CommandTypes)LanguagePrimitives.ConvertTo(commandParameter.Value, typeof(CommandTypes), CultureInfo.InvariantCulture);
				}
				else if (commandParameter.Name.Equals("Module", StringComparison.OrdinalIgnoreCase))
				{
					value2 = (string[])LanguagePrimitives.ConvertTo(commandParameter.Value, typeof(string[]), CultureInfo.InvariantCulture);
				}
				else if (commandParameter.Name.Equals("FullyQualifiedModule", StringComparison.OrdinalIgnoreCase))
				{
					value3 = (ModuleSpecification[])LanguagePrimitives.ConvertTo(commandParameter.Value, typeof(ModuleSpecification[]), CultureInfo.InvariantCulture);
				}
				else if (commandParameter.Name.Equals("ArgumentList", StringComparison.OrdinalIgnoreCase))
				{
					value4 = (object[])LanguagePrimitives.ConvertTo(commandParameter.Value, typeof(object[]), CultureInfo.InvariantCulture);
				}
			}
			RunspacePool runspacePool = shell.PowerShell.GetRunspaceConnection() as RunspacePool;
			Guid instanceId = runspacePool.InstanceId;
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("Name", value));
			psobject.Properties.Add(new PSNoteProperty("CommandType", commandTypes));
			psobject.Properties.Add(new PSNoteProperty("Namespace", value2));
			psobject.Properties.Add(new PSNoteProperty("FullyQualifiedModule", value3));
			psobject.Properties.Add(new PSNoteProperty("ArgumentList", value4));
			return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.GetCommandMetadata, instanceId, shell.InstanceId, psobject);
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x000C4388 File Offset: 0x000C2588
		internal static RemoteDataObject GenerateCreatePowerShell(ClientRemotePowerShell shell)
		{
			PowerShell powerShell = shell.PowerShell;
			PSInvocationSettings settings = shell.Settings;
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			Guid runspacePoolId = Guid.Empty;
			RunspacePool runspacePool = powerShell.GetRunspaceConnection() as RunspacePool;
			runspacePoolId = runspacePool.InstanceId;
			psobject.Properties.Add(new PSNoteProperty("PowerShell", powerShell.ToPSObjectForRemoting()));
			psobject.Properties.Add(new PSNoteProperty("NoInput", shell.NoInput));
			HostInfo hostInfo;
			if (settings == null)
			{
				hostInfo = new HostInfo(null);
				hostInfo.UseRunspaceHost = true;
				ApartmentState apartmentState = runspacePool.ApartmentState;
				psobject.Properties.Add(new PSNoteProperty("ApartmentState", apartmentState));
				psobject.Properties.Add(new PSNoteProperty("RemoteStreamOptions", RemoteStreamOptions.AddInvocationInfo));
				psobject.Properties.Add(new PSNoteProperty("AddToHistory", false));
			}
			else
			{
				hostInfo = new HostInfo(settings.Host);
				if (settings.Host == null)
				{
					hostInfo.UseRunspaceHost = true;
				}
				ApartmentState apartmentState2 = settings.ApartmentState;
				psobject.Properties.Add(new PSNoteProperty("ApartmentState", apartmentState2));
				psobject.Properties.Add(new PSNoteProperty("RemoteStreamOptions", settings.RemoteStreamOptions));
				psobject.Properties.Add(new PSNoteProperty("AddToHistory", settings.AddToHistory));
			}
			PSNoteProperty member = RemotingEncoder.CreateHostInfoProperty(hostInfo);
			psobject.Properties.Add(member);
			psobject.Properties.Add(new PSNoteProperty("IsNested", shell.PowerShell.IsNested));
			return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.CreatePowerShell, runspacePoolId, shell.InstanceId, psobject);
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x000C4544 File Offset: 0x000C2744
		internal static RemoteDataObject GenerateApplicationPrivateData(Guid clientRunspacePoolId, PSPrimitiveDictionary applicationPrivateData)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("ApplicationPrivateData", applicationPrivateData));
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.ApplicationPrivateData, clientRunspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x000C4580 File Offset: 0x000C2780
		internal static RemoteDataObject GenerateRunspacePoolStateInfo(Guid clientRunspacePoolId, RunspacePoolStateInfo stateInfo)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			PSNoteProperty member = new PSNoteProperty("RunspaceState", (int)stateInfo.State);
			psobject.Properties.Add(member);
			if (stateInfo.Reason != null)
			{
				string errorId = "RemoteRunspaceStateInfoReason";
				PSNoteProperty exceptionProperty = RemotingEncoder.GetExceptionProperty(stateInfo.Reason, errorId, ErrorCategory.NotSpecified);
				psobject.Properties.Add(exceptionProperty);
			}
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.RunspacePoolStateInfo, clientRunspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x000C45F0 File Offset: 0x000C27F0
		internal static RemoteDataObject GeneratePSEventArgs(Guid clientRunspacePoolId, PSEventArgs e)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("PSEventArgs.EventIdentifier", e.EventIdentifier));
			psobject.Properties.Add(new PSNoteProperty("PSEventArgs.SourceIdentifier", e.SourceIdentifier));
			psobject.Properties.Add(new PSNoteProperty("PSEventArgs.TimeGenerated", e.TimeGenerated));
			psobject.Properties.Add(new PSNoteProperty("PSEventArgs.Sender", e.Sender));
			psobject.Properties.Add(new PSNoteProperty("PSEventArgs.SourceArgs", e.SourceArgs));
			psobject.Properties.Add(new PSNoteProperty("PSEventArgs.MessageData", e.MessageData));
			psobject.Properties.Add(new PSNoteProperty("PSEventArgs.ComputerName", e.ComputerName));
			psobject.Properties.Add(new PSNoteProperty("PSEventArgs.RunspaceId", e.RunspaceId));
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.PSEventArgs, clientRunspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x000C46FC File Offset: 0x000C28FC
		internal static RemoteDataObject GenerateResetRunspaceState(Guid clientRunspacePoolId, long callId)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("ci", callId));
			return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.ResetRunspaceState, clientRunspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x000C473C File Offset: 0x000C293C
		internal static Version GetPSRemotingProtocolVersion(RunspacePool rsPool)
		{
			if (rsPool == null || rsPool.RemoteRunspacePoolInternal == null)
			{
				return null;
			}
			return rsPool.RemoteRunspacePoolInternal.PSRemotingProtocolVersion;
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x000C4756 File Offset: 0x000C2956
		internal static RemoteDataObject GeneratePowerShellInput(object data, Guid clientRemoteRunspacePoolId, Guid clientPowerShellId)
		{
			return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.PowerShellInput, clientRemoteRunspacePoolId, clientPowerShellId, data);
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x000C4766 File Offset: 0x000C2966
		internal static RemoteDataObject GeneratePowerShellInputEnd(Guid clientRemoteRunspacePoolId, Guid clientPowerShellId)
		{
			return RemoteDataObject.CreateFrom(RemotingDestination.Server, RemotingDataType.PowerShellInputEnd, clientRemoteRunspacePoolId, clientPowerShellId, null);
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x000C4776 File Offset: 0x000C2976
		internal static RemoteDataObject GeneratePowerShellOutput(PSObject data, Guid clientPowerShellId, Guid clientRunspacePoolId)
		{
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.PowerShellOutput, clientRunspacePoolId, clientPowerShellId, data);
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x000C4786 File Offset: 0x000C2986
		internal static RemoteDataObject GeneratePowerShellInformational(object data, Guid clientRunspacePoolId, Guid clientPowerShellId, RemotingDataType dataType)
		{
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, dataType, clientRunspacePoolId, clientPowerShellId, PSObject.AsPSObject(data));
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x000C4797 File Offset: 0x000C2997
		internal static RemoteDataObject GeneratePowerShellInformational(ProgressRecord progressRecord, Guid clientRunspacePoolId, Guid clientPowerShellId)
		{
			if (progressRecord == null)
			{
				throw PSTraceSource.NewArgumentNullException("progressRecord");
			}
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.PowerShellProgress, clientRunspacePoolId, clientPowerShellId, progressRecord.ToPSObjectForRemoting());
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000C47BA File Offset: 0x000C29BA
		internal static RemoteDataObject GeneratePowerShellInformational(InformationRecord informationRecord, Guid clientRunspacePoolId, Guid clientPowerShellId)
		{
			if (informationRecord == null)
			{
				throw PSTraceSource.NewArgumentNullException("informationRecord");
			}
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.PowerShellInformationStream, clientRunspacePoolId, clientPowerShellId, informationRecord.ToPSObjectForRemoting());
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000C47DD File Offset: 0x000C29DD
		internal static RemoteDataObject GeneratePowerShellError(object errorRecord, Guid clientRunspacePoolId, Guid clientPowerShellId)
		{
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.PowerShellErrorRecord, clientRunspacePoolId, clientPowerShellId, PSObject.AsPSObject(errorRecord));
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000C47F4 File Offset: 0x000C29F4
		internal static RemoteDataObject GeneratePowerShellStateInfo(PSInvocationStateInfo stateInfo, Guid clientPowerShellId, Guid clientRunspacePoolId)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			PSNoteProperty member = new PSNoteProperty("PipelineState", (int)stateInfo.State);
			psobject.Properties.Add(member);
			if (stateInfo.Reason != null)
			{
				string errorId = "RemotePSInvocationStateInfoReason";
				PSNoteProperty exceptionProperty = RemotingEncoder.GetExceptionProperty(stateInfo.Reason, errorId, ErrorCategory.NotSpecified);
				psobject.Properties.Add(exceptionProperty);
			}
			return RemoteDataObject.CreateFrom(RemotingDestination.Client, RemotingDataType.PowerShellStateInfo, clientRunspacePoolId, clientPowerShellId, psobject);
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000C4860 File Offset: 0x000C2A60
		internal static ErrorRecord GetErrorRecordFromException(Exception exception)
		{
			ErrorRecord errorRecord = null;
			IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
			if (containsErrorRecord != null)
			{
				errorRecord = containsErrorRecord.ErrorRecord;
				errorRecord = new ErrorRecord(errorRecord, exception);
			}
			return errorRecord;
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000C488C File Offset: 0x000C2A8C
		private static PSNoteProperty GetExceptionProperty(Exception exception, string errorId, ErrorCategory category)
		{
			ErrorRecord errorRecord = RemotingEncoder.GetErrorRecordFromException(exception);
			if (errorRecord == null)
			{
				errorRecord = new ErrorRecord(exception, errorId, category, null);
			}
			return new PSNoteProperty("ExceptionAsErrorRecord", errorRecord);
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x000C48B8 File Offset: 0x000C2AB8
		internal static RemoteDataObject GenerateClientSessionCapability(RemoteSessionCapability capability, Guid runspacePoolId)
		{
			PSObject psobject = RemotingEncoder.GenerateSessionCapability(capability);
			psobject.Properties.Add(new PSNoteProperty("TimeZone", RemoteSessionCapability.GetCurrentTimeZoneInByteFormat()));
			return RemoteDataObject.CreateFrom(capability.RemotingDestination, RemotingDataType.SessionCapability, runspacePoolId, Guid.Empty, psobject);
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x000C4900 File Offset: 0x000C2B00
		internal static RemoteDataObject GenerateServerSessionCapability(RemoteSessionCapability capability, Guid runspacePoolId)
		{
			PSObject data = RemotingEncoder.GenerateSessionCapability(capability);
			return RemoteDataObject.CreateFrom(capability.RemotingDestination, RemotingDataType.SessionCapability, runspacePoolId, Guid.Empty, data);
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x000C492C File Offset: 0x000C2B2C
		private static PSObject GenerateSessionCapability(RemoteSessionCapability capability)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("protocolversion", capability.ProtocolVersion));
			psobject.Properties.Add(new PSNoteProperty("PSVersion", capability.PSVersion));
			psobject.Properties.Add(new PSNoteProperty("SerializationVersion", capability.SerializationVersion));
			return psobject;
		}

		// Token: 0x020002DD RID: 733
		// (Invoke) Token: 0x060022E6 RID: 8934
		internal delegate T ValueGetterDelegate<T>();
	}
}
