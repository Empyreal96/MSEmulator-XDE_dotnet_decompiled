using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020003F4 RID: 1012
	internal abstract class LogProvider
	{
		// Token: 0x06002DBB RID: 11707 RVA: 0x000FC994 File Offset: 0x000FAB94
		internal LogProvider()
		{
		}

		// Token: 0x06002DBC RID: 11708
		internal abstract void LogEngineHealthEvent(LogContext logContext, int eventId, Exception exception, Dictionary<string, string> additionalInfo);

		// Token: 0x06002DBD RID: 11709
		internal abstract void LogEngineLifecycleEvent(LogContext logContext, EngineState newState, EngineState previousState);

		// Token: 0x06002DBE RID: 11710
		internal abstract void LogCommandHealthEvent(LogContext logContext, Exception exception);

		// Token: 0x06002DBF RID: 11711
		internal abstract void LogCommandLifecycleEvent(Func<LogContext> getLogContext, CommandState newState);

		// Token: 0x06002DC0 RID: 11712
		internal abstract void LogPipelineExecutionDetailEvent(LogContext logContext, List<string> pipelineExecutionDetail);

		// Token: 0x06002DC1 RID: 11713
		internal abstract void LogProviderHealthEvent(LogContext logContext, string providerName, Exception exception);

		// Token: 0x06002DC2 RID: 11714
		internal abstract void LogProviderLifecycleEvent(LogContext logContext, string providerName, ProviderState newState);

		// Token: 0x06002DC3 RID: 11715
		internal abstract void LogSettingsEvent(LogContext logContext, string variableName, string value, string previousValue);

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000FC99C File Offset: 0x000FAB9C
		internal virtual bool UseLoggingVariables()
		{
			return true;
		}
	}
}
