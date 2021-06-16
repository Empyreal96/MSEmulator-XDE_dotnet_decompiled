using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.Management.Automation.Internal;
using System.Text;

namespace System.Management.Automation.Tracing
{
	// Token: 0x02000900 RID: 2304
	internal class PSEtwLogProvider : LogProvider
	{
		// Token: 0x0600563F RID: 22079 RVA: 0x001C3EEC File Offset: 0x001C20EC
		static PSEtwLogProvider()
		{
			PSEtwLogProvider.etwProvider = new EventProvider(new Guid(PSEtwLogProvider.PowerShellEventProviderGuid));
			PSEtwLogProvider.LogContextSeverity = EtwLoggingStrings.LogContextSeverity;
			PSEtwLogProvider.LogContextHostName = EtwLoggingStrings.LogContextHostName;
			PSEtwLogProvider.LogContextHostVersion = EtwLoggingStrings.LogContextHostVersion;
			PSEtwLogProvider.LogContextHostId = EtwLoggingStrings.LogContextHostId;
			PSEtwLogProvider.LogContextHostApplication = EtwLoggingStrings.LogContextHostApplication;
			PSEtwLogProvider.LogContextEngineVersion = EtwLoggingStrings.LogContextEngineVersion;
			PSEtwLogProvider.LogContextRunspaceId = EtwLoggingStrings.LogContextRunspaceId;
			PSEtwLogProvider.LogContextPipelineId = EtwLoggingStrings.LogContextPipelineId;
			PSEtwLogProvider.LogContextCommandName = EtwLoggingStrings.LogContextCommandName;
			PSEtwLogProvider.LogContextCommandType = EtwLoggingStrings.LogContextCommandType;
			PSEtwLogProvider.LogContextScriptName = EtwLoggingStrings.LogContextScriptName;
			PSEtwLogProvider.LogContextCommandPath = EtwLoggingStrings.LogContextCommandPath;
			PSEtwLogProvider.LogContextSequenceNumber = EtwLoggingStrings.LogContextSequenceNumber;
			PSEtwLogProvider.LogContextUser = EtwLoggingStrings.LogContextUser;
			PSEtwLogProvider.LogContextConnectedUser = EtwLoggingStrings.LogContextConnectedUser;
			PSEtwLogProvider.LogContextTime = EtwLoggingStrings.LogContextTime;
			PSEtwLogProvider.LogContextShellId = EtwLoggingStrings.LogContextShellId;
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x001C3FE0 File Offset: 0x001C21E0
		internal bool IsEnabled(PSLevel level, PSKeyword keywords)
		{
			return PSEtwLogProvider.etwProvider.IsEnabled((byte)level, (long)keywords);
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x001C3FF0 File Offset: 0x001C21F0
		internal override void LogEngineHealthEvent(LogContext logContext, int eventId, Exception exception, Dictionary<string, string> additionalInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			PSEtwLogProvider.AppendException(stringBuilder, exception);
			stringBuilder.AppendLine();
			PSEtwLogProvider.AppendAdditionalInfo(stringBuilder, additionalInfo);
			this.WriteEvent(PSEventId.Engine_Health, PSChannel.Operational, PSOpcode.Exception, PSTask.ExecutePipeline, logContext, stringBuilder.ToString());
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x001C4034 File Offset: 0x001C2234
		internal override void LogEngineLifecycleEvent(LogContext logContext, EngineState newState, EngineState previousState)
		{
			if (this.IsEnabled(PSLevel.Informational, (PSKeyword)4611686018427387936UL))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.EngineStateChange, previousState.ToString(), newState.ToString()));
				PSTask task = PSTask.EngineStart;
				if (newState == EngineState.Stopped || newState == EngineState.OutOfService || newState == EngineState.None || newState == EngineState.Degraded)
				{
					task = PSTask.EngineStop;
				}
				this.WriteEvent(PSEventId.Engine_Lifecycle, PSChannel.Analytic, PSOpcode.Method, task, logContext, stringBuilder.ToString());
			}
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x001C40AC File Offset: 0x001C22AC
		internal override void LogCommandHealthEvent(LogContext logContext, Exception exception)
		{
			StringBuilder stringBuilder = new StringBuilder();
			PSEtwLogProvider.AppendException(stringBuilder, exception);
			this.WriteEvent(PSEventId.Command_Health, PSChannel.Operational, PSOpcode.Exception, PSTask.ExecutePipeline, logContext, stringBuilder.ToString());
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x001C40E0 File Offset: 0x001C22E0
		internal override void LogCommandLifecycleEvent(Func<LogContext> getLogContext, CommandState newState)
		{
			if (this.IsEnabled(PSLevel.Informational, (PSKeyword)4611686018427387936UL))
			{
				LogContext logContext = getLogContext();
				StringBuilder stringBuilder = new StringBuilder();
				if (logContext.CommandType != null)
				{
					if (logContext.CommandType.Equals("SCRIPT", StringComparison.OrdinalIgnoreCase))
					{
						stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.ScriptStateChange, newState.ToString()));
					}
					else
					{
						stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.CommandStateChange, logContext.CommandName, newState.ToString()));
					}
				}
				PSTask task = PSTask.CommandStart;
				if (newState == CommandState.Stopped || newState == CommandState.Terminated)
				{
					task = PSTask.CommandStop;
				}
				this.WriteEvent(PSEventId.Command_Lifecycle, PSChannel.Analytic, PSOpcode.Method, task, logContext, stringBuilder.ToString());
			}
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x001C4190 File Offset: 0x001C2390
		internal override void LogPipelineExecutionDetailEvent(LogContext logContext, List<string> pipelineExecutionDetail)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (pipelineExecutionDetail != null)
			{
				foreach (string value in pipelineExecutionDetail)
				{
					stringBuilder.AppendLine(value);
				}
			}
			this.WriteEvent(PSEventId.Pipeline_Detail, PSChannel.Operational, PSOpcode.Method, PSTask.ExecutePipeline, logContext, stringBuilder.ToString());
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x001C4200 File Offset: 0x001C2400
		internal override void LogProviderHealthEvent(LogContext logContext, string providerName, Exception exception)
		{
			StringBuilder stringBuilder = new StringBuilder();
			PSEtwLogProvider.AppendException(stringBuilder, exception);
			stringBuilder.AppendLine();
			PSEtwLogProvider.AppendAdditionalInfo(stringBuilder, new Dictionary<string, string>
			{
				{
					EtwLoggingStrings.ProviderNameString,
					providerName
				}
			});
			this.WriteEvent(PSEventId.Provider_Health, PSChannel.Operational, PSOpcode.Exception, PSTask.ExecutePipeline, logContext, stringBuilder.ToString());
		}

		// Token: 0x06005647 RID: 22087 RVA: 0x001C4254 File Offset: 0x001C2454
		internal override void LogProviderLifecycleEvent(LogContext logContext, string providerName, ProviderState newState)
		{
			if (this.IsEnabled(PSLevel.Informational, (PSKeyword)4611686018427387936UL))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.ProviderStateChange, providerName, newState.ToString()));
				PSTask task = PSTask.ProviderStart;
				if (newState == ProviderState.Stopped)
				{
					task = PSTask.ProviderStop;
				}
				this.WriteEvent(PSEventId.Provider_Lifecycle, PSChannel.Analytic, PSOpcode.Method, task, logContext, stringBuilder.ToString());
			}
		}

		// Token: 0x06005648 RID: 22088 RVA: 0x001C42B8 File Offset: 0x001C24B8
		internal override void LogSettingsEvent(LogContext logContext, string variableName, string value, string previousValue)
		{
			if (this.IsEnabled(PSLevel.Informational, (PSKeyword)4611686018427387936UL))
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (previousValue == null)
				{
					stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.SettingChangeNoPrevious, variableName, value));
				}
				else
				{
					stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.SettingChange, new object[]
					{
						variableName,
						previousValue,
						value
					}));
				}
				this.WriteEvent(PSEventId.Settings, PSChannel.Analytic, PSOpcode.Method, PSTask.ExecutePipeline, logContext, stringBuilder.ToString());
			}
		}

		// Token: 0x06005649 RID: 22089 RVA: 0x001C4334 File Offset: 0x001C2534
		internal override bool UseLoggingVariables()
		{
			return false;
		}

		// Token: 0x0600564A RID: 22090 RVA: 0x001C4338 File Offset: 0x001C2538
		private static string GetPSLogUserData(ExecutionContext context)
		{
			if (context == null)
			{
				return string.Empty;
			}
			object variableValue = context.GetVariableValue(SpecialVariables.PSLogUserDataPath);
			if (variableValue == null)
			{
				return string.Empty;
			}
			return variableValue.ToString();
		}

		// Token: 0x0600564B RID: 22091 RVA: 0x001C436C File Offset: 0x001C256C
		internal static void AppendException(StringBuilder sb, Exception except)
		{
			sb.AppendLine(StringUtil.Format(EtwLoggingStrings.ErrorRecordMessage, except.Message));
			IContainsErrorRecord containsErrorRecord = except as IContainsErrorRecord;
			if (containsErrorRecord != null)
			{
				ErrorRecord errorRecord = containsErrorRecord.ErrorRecord;
				if (errorRecord != null)
				{
					sb.AppendLine(StringUtil.Format(EtwLoggingStrings.ErrorRecordId, errorRecord.FullyQualifiedErrorId));
					ErrorDetails errorDetails = errorRecord.ErrorDetails;
					if (errorDetails != null)
					{
						sb.AppendLine(StringUtil.Format(EtwLoggingStrings.ErrorRecordRecommendedAction, errorDetails.RecommendedAction));
					}
				}
			}
		}

		// Token: 0x0600564C RID: 22092 RVA: 0x001C43DC File Offset: 0x001C25DC
		private static void AppendAdditionalInfo(StringBuilder sb, Dictionary<string, string> additionalInfo)
		{
			if (additionalInfo != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in additionalInfo)
				{
					sb.AppendLine(StringUtil.Format("{0} = {1}", keyValuePair.Key, keyValuePair.Value));
				}
			}
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x001C4448 File Offset: 0x001C2648
		private static PSLevel GetPSLevelFromSeverity(string severity)
		{
			if (severity != null)
			{
				if (severity == "Critical" || severity == "Error")
				{
					return PSLevel.Error;
				}
				if (severity == "Warning")
				{
					return PSLevel.Warning;
				}
			}
			return PSLevel.Informational;
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x001C4488 File Offset: 0x001C2688
		private static string LogContextToString(LogContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(PSEtwLogProvider.LogContextSeverity);
			stringBuilder.AppendLine(context.Severity);
			stringBuilder.Append(PSEtwLogProvider.LogContextHostName);
			stringBuilder.AppendLine(context.HostName);
			stringBuilder.Append(PSEtwLogProvider.LogContextHostVersion);
			stringBuilder.AppendLine(context.HostVersion);
			stringBuilder.Append(PSEtwLogProvider.LogContextHostId);
			stringBuilder.AppendLine(context.HostId);
			stringBuilder.Append(PSEtwLogProvider.LogContextHostApplication);
			stringBuilder.AppendLine(context.HostApplication);
			stringBuilder.Append(PSEtwLogProvider.LogContextEngineVersion);
			stringBuilder.AppendLine(context.EngineVersion);
			stringBuilder.Append(PSEtwLogProvider.LogContextRunspaceId);
			stringBuilder.AppendLine(context.RunspaceId);
			stringBuilder.Append(PSEtwLogProvider.LogContextPipelineId);
			stringBuilder.AppendLine(context.PipelineId);
			stringBuilder.Append(PSEtwLogProvider.LogContextCommandName);
			stringBuilder.AppendLine(context.CommandName);
			stringBuilder.Append(PSEtwLogProvider.LogContextCommandType);
			stringBuilder.AppendLine(context.CommandType);
			stringBuilder.Append(PSEtwLogProvider.LogContextScriptName);
			stringBuilder.AppendLine(context.ScriptName);
			stringBuilder.Append(PSEtwLogProvider.LogContextCommandPath);
			stringBuilder.AppendLine(context.CommandPath);
			stringBuilder.Append(PSEtwLogProvider.LogContextSequenceNumber);
			stringBuilder.AppendLine(context.SequenceNumber);
			stringBuilder.Append(PSEtwLogProvider.LogContextUser);
			stringBuilder.AppendLine(context.User);
			stringBuilder.Append(PSEtwLogProvider.LogContextConnectedUser);
			stringBuilder.AppendLine(context.ConnectedUser);
			stringBuilder.Append(PSEtwLogProvider.LogContextShellId);
			stringBuilder.AppendLine(context.ShellId);
			return stringBuilder.ToString();
		}

		// Token: 0x0600564F RID: 22095 RVA: 0x001C4634 File Offset: 0x001C2834
		internal void WriteEvent(PSEventId id, PSChannel channel, PSOpcode opcode, PSTask task, LogContext logContext, string payLoad)
		{
			this.WriteEvent(id, channel, opcode, PSEtwLogProvider.GetPSLevelFromSeverity(logContext.Severity), task, (PSKeyword)0UL, new object[]
			{
				PSEtwLogProvider.LogContextToString(logContext),
				PSEtwLogProvider.GetPSLogUserData(logContext.ExecutionContext),
				payLoad
			});
		}

		// Token: 0x06005650 RID: 22096 RVA: 0x001C4680 File Offset: 0x001C2880
		internal void WriteEvent(PSEventId id, PSChannel channel, PSOpcode opcode, PSLevel level, PSTask task, PSKeyword keyword, params object[] args)
		{
			long keywords;
			if (keyword == PSKeyword.UseAlwaysAnalytic || keyword == PSKeyword.UseAlwaysOperational)
			{
				keywords = 0L;
			}
			else
			{
				keywords = (long)keyword;
			}
			EventDescriptor eventDescriptor = new EventDescriptor((int)id, 1, (byte)channel, (byte)level, (byte)opcode, (int)task, keywords);
			PSEtwLogProvider.etwProvider.WriteEvent(ref eventDescriptor, args);
		}

		// Token: 0x06005651 RID: 22097 RVA: 0x001C46D8 File Offset: 0x001C28D8
		internal void WriteTransferEvent(Guid parentActivityId)
		{
			PSEtwLogProvider.etwProvider.WriteTransferEvent(ref PSEtwLogProvider._xferEventDescriptor, parentActivityId, new object[]
			{
				EtwActivity.GetActivityId(),
				parentActivityId
			});
		}

		// Token: 0x06005652 RID: 22098 RVA: 0x001C4714 File Offset: 0x001C2914
		internal void SetActivityIdForCurrentThread(Guid newActivityId)
		{
			Guid guid = newActivityId;
			EventProvider.SetActivityId(ref guid);
		}

		// Token: 0x04002DD7 RID: 11735
		private static EventProvider etwProvider;

		// Token: 0x04002DD8 RID: 11736
		private static readonly string PowerShellEventProviderGuid = "A0C1853B-5C40-4b15-8766-3CF1C58F985A";

		// Token: 0x04002DD9 RID: 11737
		private static EventDescriptor _xferEventDescriptor = new EventDescriptor(7941, 1, 17, 5, 20, 0, 4611686018427387904L);

		// Token: 0x04002DDA RID: 11738
		private static readonly string LogContextSeverity;

		// Token: 0x04002DDB RID: 11739
		private static readonly string LogContextHostName;

		// Token: 0x04002DDC RID: 11740
		private static readonly string LogContextHostVersion;

		// Token: 0x04002DDD RID: 11741
		private static readonly string LogContextHostId;

		// Token: 0x04002DDE RID: 11742
		private static readonly string LogContextHostApplication;

		// Token: 0x04002DDF RID: 11743
		private static readonly string LogContextEngineVersion;

		// Token: 0x04002DE0 RID: 11744
		private static readonly string LogContextRunspaceId;

		// Token: 0x04002DE1 RID: 11745
		private static readonly string LogContextPipelineId;

		// Token: 0x04002DE2 RID: 11746
		private static readonly string LogContextCommandName;

		// Token: 0x04002DE3 RID: 11747
		private static readonly string LogContextCommandType;

		// Token: 0x04002DE4 RID: 11748
		private static readonly string LogContextScriptName;

		// Token: 0x04002DE5 RID: 11749
		private static readonly string LogContextCommandPath;

		// Token: 0x04002DE6 RID: 11750
		private static readonly string LogContextSequenceNumber;

		// Token: 0x04002DE7 RID: 11751
		private static readonly string LogContextUser;

		// Token: 0x04002DE8 RID: 11752
		private static readonly string LogContextConnectedUser;

		// Token: 0x04002DE9 RID: 11753
		private static readonly string LogContextTime;

		// Token: 0x04002DEA RID: 11754
		private static readonly string LogContextShellId;
	}
}
