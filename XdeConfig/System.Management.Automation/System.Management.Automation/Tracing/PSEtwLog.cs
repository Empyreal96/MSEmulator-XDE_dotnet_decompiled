using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008FF RID: 2303
	internal static class PSEtwLog
	{
		// Token: 0x06005629 RID: 22057 RVA: 0x001C3C4D File Offset: 0x001C1E4D
		internal static void LogEngineHealthEvent(LogContext logContext, int eventId, Exception exception, Dictionary<string, string> additionalInfo)
		{
			PSEtwLog.provider.LogEngineHealthEvent(logContext, eventId, exception, additionalInfo);
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x001C3C5D File Offset: 0x001C1E5D
		internal static void LogEngineLifecycleEvent(LogContext logContext, EngineState newState, EngineState previousState)
		{
			PSEtwLog.provider.LogEngineLifecycleEvent(logContext, newState, previousState);
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x001C3C6C File Offset: 0x001C1E6C
		internal static void LogCommandHealthEvent(LogContext logContext, Exception exception)
		{
			PSEtwLog.provider.LogCommandHealthEvent(logContext, exception);
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x001C3C8C File Offset: 0x001C1E8C
		internal static void LogCommandLifecycleEvent(LogContext logContext, CommandState newState)
		{
			PSEtwLog.provider.LogCommandLifecycleEvent(() => logContext, newState);
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x001C3CBD File Offset: 0x001C1EBD
		internal static void LogPipelineExecutionDetailEvent(LogContext logContext, List<string> pipelineExecutionDetail)
		{
			PSEtwLog.provider.LogPipelineExecutionDetailEvent(logContext, pipelineExecutionDetail);
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x001C3CCB File Offset: 0x001C1ECB
		internal static void LogProviderHealthEvent(LogContext logContext, string providerName, Exception exception)
		{
			PSEtwLog.provider.LogProviderHealthEvent(logContext, providerName, exception);
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x001C3CDA File Offset: 0x001C1EDA
		internal static void LogProviderLifecycleEvent(LogContext logContext, string providerName, ProviderState newState)
		{
			PSEtwLog.provider.LogProviderLifecycleEvent(logContext, providerName, newState);
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x001C3CE9 File Offset: 0x001C1EE9
		internal static void LogSettingsEvent(LogContext logContext, string variableName, string value, string previousValue)
		{
			PSEtwLog.provider.LogSettingsEvent(logContext, variableName, value, previousValue);
		}

		// Token: 0x06005631 RID: 22065 RVA: 0x001C3CF9 File Offset: 0x001C1EF9
		internal static void LogOperationalInformation(PSEventId id, PSOpcode opcode, PSTask task, PSKeyword keyword, params object[] args)
		{
			PSEtwLog.provider.WriteEvent(id, PSChannel.Operational, opcode, PSLevel.Informational, task, keyword, args);
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x001C3D0E File Offset: 0x001C1F0E
		internal static void LogOperationalWarning(PSEventId id, PSOpcode opcode, PSTask task, PSKeyword keyword, params object[] args)
		{
			PSEtwLog.provider.WriteEvent(id, PSChannel.Operational, opcode, PSLevel.Warning, task, keyword, args);
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x001C3D23 File Offset: 0x001C1F23
		internal static void LogOperationalVerbose(PSEventId id, PSOpcode opcode, PSTask task, PSKeyword keyword, params object[] args)
		{
			PSEtwLog.provider.WriteEvent(id, PSChannel.Operational, opcode, PSLevel.Verbose, task, keyword, args);
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x001C3D38 File Offset: 0x001C1F38
		internal static void LogAnalyticError(PSEventId id, PSOpcode opcode, PSTask task, PSKeyword keyword, params object[] args)
		{
			PSEtwLog.provider.WriteEvent(id, PSChannel.Analytic, opcode, PSLevel.Error, task, keyword, args);
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x001C3D4D File Offset: 0x001C1F4D
		internal static void LogAnalyticWarning(PSEventId id, PSOpcode opcode, PSTask task, PSKeyword keyword, params object[] args)
		{
			PSEtwLog.provider.WriteEvent(id, PSChannel.Analytic, opcode, PSLevel.Warning, task, keyword, args);
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x001C3D64 File Offset: 0x001C1F64
		internal static void LogAnalyticVerbose(PSEventId id, PSOpcode opcode, PSTask task, PSKeyword keyword, long objectId, long fragmentId, int isStartFragment, int isEndFragment, uint fragmentLength, PSETWBinaryBlob fragmentData)
		{
			if (PSEtwLog.provider.IsEnabled(PSLevel.Verbose, keyword))
			{
				string text = BitConverter.ToString(fragmentData.blob, fragmentData.offset, fragmentData.length);
				text = string.Format(CultureInfo.InvariantCulture, "0x{0}", new object[]
				{
					text.Replace("-", "")
				});
				PSEtwLog.provider.WriteEvent(id, PSChannel.Analytic, opcode, PSLevel.Verbose, task, keyword, new object[]
				{
					objectId,
					fragmentId,
					isStartFragment,
					isEndFragment,
					fragmentLength,
					text
				});
			}
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x001C3E17 File Offset: 0x001C2017
		internal static void LogAnalyticVerbose(PSEventId id, PSOpcode opcode, PSTask task, PSKeyword keyword, params object[] args)
		{
			PSEtwLog.provider.WriteEvent(id, PSChannel.Analytic, opcode, PSLevel.Verbose, task, keyword, args);
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x001C3E2C File Offset: 0x001C202C
		internal static void LogAnalyticInformational(PSEventId id, PSOpcode opcode, PSTask task, PSKeyword keyword, params object[] args)
		{
			PSEtwLog.provider.WriteEvent(id, PSChannel.Analytic, opcode, PSLevel.Informational, task, keyword, args);
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x001C3E41 File Offset: 0x001C2041
		internal static void LogOperationalError(PSEventId id, PSOpcode opcode, PSTask task, PSKeyword keyword, params object[] args)
		{
			PSEtwLog.provider.WriteEvent(id, PSChannel.Operational, opcode, PSLevel.Error, task, keyword, args);
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x001C3E56 File Offset: 0x001C2056
		internal static void LogOperationalError(PSEventId id, PSOpcode opcode, PSTask task, LogContext logContext, string payLoad)
		{
			PSEtwLog.provider.WriteEvent(id, PSChannel.Operational, opcode, task, logContext, payLoad);
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x001C3E6A File Offset: 0x001C206A
		internal static void SetActivityIdForCurrentThread(Guid newActivityId)
		{
			PSEtwLog.provider.SetActivityIdForCurrentThread(newActivityId);
		}

		// Token: 0x0600563C RID: 22076 RVA: 0x001C3E77 File Offset: 0x001C2077
		internal static void ReplaceActivityIdForCurrentThread(Guid newActivityId, PSEventId eventForOperationalChannel, PSEventId eventForAnalyticChannel, PSKeyword keyword, PSTask task)
		{
			PSEtwLog.provider.SetActivityIdForCurrentThread(newActivityId);
			PSEtwLog.WriteTransferEvent(newActivityId, eventForOperationalChannel, eventForAnalyticChannel, keyword, task);
		}

		// Token: 0x0600563D RID: 22077 RVA: 0x001C3E90 File Offset: 0x001C2090
		internal static void WriteTransferEvent(Guid relatedActivityId, PSEventId eventForOperationalChannel, PSEventId eventForAnalyticChannel, PSKeyword keyword, PSTask task)
		{
			PSEtwLog.provider.WriteEvent(eventForOperationalChannel, PSChannel.Operational, PSOpcode.Method, PSLevel.Informational, task, PSKeyword.UseAlwaysOperational, new object[0]);
			PSEtwLog.provider.WriteEvent(eventForAnalyticChannel, PSChannel.Analytic, PSOpcode.Method, PSLevel.Informational, task, PSKeyword.UseAlwaysAnalytic, new object[0]);
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x001C3EDF File Offset: 0x001C20DF
		internal static void WriteTransferEvent(Guid parentActivityId)
		{
			PSEtwLog.provider.WriteTransferEvent(parentActivityId);
		}

		// Token: 0x04002DD6 RID: 11734
		private static PSEtwLogProvider provider = new PSEtwLogProvider();
	}
}
