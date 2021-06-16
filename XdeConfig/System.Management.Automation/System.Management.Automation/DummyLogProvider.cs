using System;
using System.Collections.Generic;

namespace System.Management.Automation
{
	// Token: 0x020003F5 RID: 1013
	internal class DummyLogProvider : LogProvider
	{
		// Token: 0x06002DC5 RID: 11717 RVA: 0x000FC99F File Offset: 0x000FAB9F
		internal DummyLogProvider()
		{
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000FC9A7 File Offset: 0x000FABA7
		internal override void LogEngineHealthEvent(LogContext logContext, int eventId, Exception exception, Dictionary<string, string> additionalInfo)
		{
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000FC9A9 File Offset: 0x000FABA9
		internal override void LogEngineLifecycleEvent(LogContext logContext, EngineState newState, EngineState previousState)
		{
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000FC9AB File Offset: 0x000FABAB
		internal override void LogCommandHealthEvent(LogContext logContext, Exception exception)
		{
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000FC9AD File Offset: 0x000FABAD
		internal override void LogCommandLifecycleEvent(Func<LogContext> getLogContext, CommandState newState)
		{
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000FC9AF File Offset: 0x000FABAF
		internal override void LogPipelineExecutionDetailEvent(LogContext logContext, List<string> pipelineExecutionDetail)
		{
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000FC9B1 File Offset: 0x000FABB1
		internal override void LogProviderHealthEvent(LogContext logContext, string providerName, Exception exception)
		{
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000FC9B3 File Offset: 0x000FABB3
		internal override void LogProviderLifecycleEvent(LogContext logContext, string providerName, ProviderState newState)
		{
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000FC9B5 File Offset: 0x000FABB5
		internal override void LogSettingsEvent(LogContext logContext, string variableName, string value, string previousValue)
		{
		}
	}
}
