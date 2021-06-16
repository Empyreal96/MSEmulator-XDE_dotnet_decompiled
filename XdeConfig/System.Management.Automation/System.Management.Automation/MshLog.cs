using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Security;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x020003F6 RID: 1014
	internal static class MshLog
	{
		// Token: 0x06002DCE RID: 11726 RVA: 0x000FC9B7 File Offset: 0x000FABB7
		static MshLog()
		{
			MshLog.ignoredCommands.Add("Out-Lineoutput");
			MshLog.ignoredCommands.Add("Format-Default");
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000FC9F1 File Offset: 0x000FABF1
		private static IEnumerable<LogProvider> GetLogProvider(string shellId)
		{
			return MshLog._logProviders.GetOrAdd(shellId, new Func<string, Collection<LogProvider>>(MshLog.CreateLogProvider));
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000FCA0C File Offset: 0x000FAC0C
		private static IEnumerable<LogProvider> GetLogProvider(ExecutionContext executionContext)
		{
			if (executionContext == null)
			{
				throw PSTraceSource.NewArgumentNullException("executionContext");
			}
			string shellID = executionContext.ShellID;
			return MshLog.GetLogProvider(shellID);
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000FCA34 File Offset: 0x000FAC34
		private static IEnumerable<LogProvider> GetLogProvider(LogContext logContext)
		{
			return MshLog.GetLogProvider(logContext.ShellId);
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000FCA44 File Offset: 0x000FAC44
		private static Collection<LogProvider> CreateLogProvider(string shellId)
		{
			Collection<LogProvider> collection = new Collection<LogProvider>();
			try
			{
				LogProvider item = new EventLogLogProvider(shellId);
				collection.Add(item);
				item = new PSEtwLogProvider();
				collection.Add(item);
				return collection;
			}
			catch (ArgumentException)
			{
			}
			catch (InvalidOperationException)
			{
			}
			catch (SecurityException)
			{
			}
			collection.Add(new DummyLogProvider());
			return collection;
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000FCAC8 File Offset: 0x000FACC8
		internal static void SetDummyLog(string shellId)
		{
			Collection<LogProvider> providers = new Collection<LogProvider>
			{
				new DummyLogProvider()
			};
			MshLog._logProviders.AddOrUpdate(shellId, providers, (string key, Collection<LogProvider> value) => providers);
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000FCB14 File Offset: 0x000FAD14
		internal static void LogEngineHealthEvent(ExecutionContext executionContext, int eventId, Exception exception, Severity severity, Dictionary<string, string> additionalInfo, EngineState newEngineState)
		{
			if (executionContext == null)
			{
				PSTraceSource.NewArgumentNullException("executionContext");
				return;
			}
			if (exception == null)
			{
				PSTraceSource.NewArgumentNullException("exception");
				return;
			}
			InvocationInfo invocationInfo = null;
			IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				invocationInfo = containsErrorRecord.ErrorRecord.InvocationInfo;
			}
			foreach (LogProvider logProvider in MshLog.GetLogProvider(executionContext))
			{
				if (MshLog.NeedToLogEngineHealthEvent(logProvider, executionContext))
				{
					logProvider.LogEngineHealthEvent(MshLog.GetLogContext(executionContext, invocationInfo, severity), eventId, exception, additionalInfo);
				}
			}
			if (newEngineState != EngineState.None)
			{
				MshLog.LogEngineLifecycleEvent(executionContext, newEngineState, invocationInfo);
			}
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x000FCBC0 File Offset: 0x000FADC0
		internal static void LogEngineHealthEvent(ExecutionContext executionContext, int eventId, Exception exception, Severity severity)
		{
			MshLog.LogEngineHealthEvent(executionContext, eventId, exception, severity, null);
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x000FCBCC File Offset: 0x000FADCC
		internal static void LogEngineHealthEvent(ExecutionContext executionContext, Exception exception, Severity severity)
		{
			MshLog.LogEngineHealthEvent(executionContext, 100, exception, severity, null);
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000FCBD9 File Offset: 0x000FADD9
		internal static void LogEngineHealthEvent(ExecutionContext executionContext, int eventId, Exception exception, Severity severity, Dictionary<string, string> additionalInfo)
		{
			MshLog.LogEngineHealthEvent(executionContext, eventId, exception, severity, additionalInfo, EngineState.None);
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x000FCBE7 File Offset: 0x000FADE7
		internal static void LogEngineHealthEvent(ExecutionContext executionContext, int eventId, Exception exception, Severity severity, EngineState newEngineState)
		{
			MshLog.LogEngineHealthEvent(executionContext, eventId, exception, severity, null, newEngineState);
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000FCBF8 File Offset: 0x000FADF8
		internal static void LogEngineHealthEvent(LogContext logContext, int eventId, Exception exception, Dictionary<string, string> additionalInfo)
		{
			if (logContext == null)
			{
				PSTraceSource.NewArgumentNullException("logContext");
				return;
			}
			if (exception == null)
			{
				PSTraceSource.NewArgumentNullException("exception");
				return;
			}
			foreach (LogProvider logProvider in MshLog.GetLogProvider(logContext))
			{
				logProvider.LogEngineHealthEvent(logContext, eventId, exception, additionalInfo);
			}
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000FCC68 File Offset: 0x000FAE68
		internal static void LogEngineLifecycleEvent(ExecutionContext executionContext, EngineState engineState, InvocationInfo invocationInfo)
		{
			if (executionContext == null)
			{
				PSTraceSource.NewArgumentNullException("executionContext");
				return;
			}
			EngineState engineState2 = MshLog.GetEngineState(executionContext);
			if (engineState == engineState2)
			{
				return;
			}
			foreach (LogProvider logProvider in MshLog.GetLogProvider(executionContext))
			{
				if (MshLog.NeedToLogEngineLifecycleEvent(logProvider, executionContext))
				{
					logProvider.LogEngineLifecycleEvent(MshLog.GetLogContext(executionContext, invocationInfo), engineState, engineState2);
				}
			}
			MshLog.SetEngineState(executionContext, engineState);
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000FCCE8 File Offset: 0x000FAEE8
		internal static void LogEngineLifecycleEvent(ExecutionContext executionContext, EngineState engineState)
		{
			MshLog.LogEngineLifecycleEvent(executionContext, engineState, null);
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000FCCF4 File Offset: 0x000FAEF4
		internal static void LogCommandHealthEvent(ExecutionContext executionContext, Exception exception, Severity severity)
		{
			if (executionContext == null)
			{
				PSTraceSource.NewArgumentNullException("executionContext");
				return;
			}
			if (exception == null)
			{
				PSTraceSource.NewArgumentNullException("exception");
				return;
			}
			InvocationInfo invocationInfo = null;
			IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				invocationInfo = containsErrorRecord.ErrorRecord.InvocationInfo;
			}
			foreach (LogProvider logProvider in MshLog.GetLogProvider(executionContext))
			{
				if (MshLog.NeedToLogCommandHealthEvent(logProvider, executionContext))
				{
					logProvider.LogCommandHealthEvent(MshLog.GetLogContext(executionContext, invocationInfo, severity), exception);
				}
			}
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x000FCDCC File Offset: 0x000FAFCC
		internal static void LogCommandLifecycleEvent(ExecutionContext executionContext, CommandState commandState, InvocationInfo invocationInfo)
		{
			if (executionContext == null)
			{
				PSTraceSource.NewArgumentNullException("executionContext");
				return;
			}
			if (invocationInfo == null)
			{
				PSTraceSource.NewArgumentNullException("invocationInfo");
				return;
			}
			if (MshLog.ignoredCommands.Contains(invocationInfo.MyCommand.Name))
			{
				return;
			}
			LogContext logContext = null;
			foreach (LogProvider logProvider in MshLog.GetLogProvider(executionContext))
			{
				if (MshLog.NeedToLogCommandLifecycleEvent(logProvider, executionContext))
				{
					logProvider.LogCommandLifecycleEvent(delegate
					{
						LogContext result;
						if ((result = logContext) == null)
						{
							result = (logContext = MshLog.GetLogContext(executionContext, invocationInfo));
						}
						return result;
					}, commandState);
				}
			}
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000FCEDC File Offset: 0x000FB0DC
		internal static void LogCommandLifecycleEvent(ExecutionContext executionContext, CommandState commandState, string commandName)
		{
			if (executionContext == null)
			{
				PSTraceSource.NewArgumentNullException("executionContext");
				return;
			}
			LogContext logContext = null;
			foreach (LogProvider logProvider in MshLog.GetLogProvider(executionContext))
			{
				if (MshLog.NeedToLogCommandLifecycleEvent(logProvider, executionContext))
				{
					logProvider.LogCommandLifecycleEvent(delegate
					{
						if (logContext == null)
						{
							logContext = MshLog.GetLogContext(executionContext, null);
							logContext.CommandName = commandName;
						}
						return logContext;
					}, commandState);
				}
			}
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000FCF80 File Offset: 0x000FB180
		internal static void LogPipelineExecutionDetailEvent(ExecutionContext executionContext, List<string> detail, InvocationInfo invocationInfo)
		{
			if (executionContext == null)
			{
				PSTraceSource.NewArgumentNullException("executionContext");
				return;
			}
			foreach (LogProvider logProvider in MshLog.GetLogProvider(executionContext))
			{
				if (MshLog.NeedToLogPipelineExecutionDetailEvent(logProvider, executionContext))
				{
					logProvider.LogPipelineExecutionDetailEvent(MshLog.GetLogContext(executionContext, invocationInfo), detail);
				}
			}
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000FCFEC File Offset: 0x000FB1EC
		internal static void LogPipelineExecutionDetailEvent(ExecutionContext executionContext, List<string> detail, string scriptName, string commandLine)
		{
			if (executionContext == null)
			{
				PSTraceSource.NewArgumentNullException("executionContext");
				return;
			}
			LogContext logContext = MshLog.GetLogContext(executionContext, null);
			logContext.CommandLine = commandLine;
			logContext.ScriptName = scriptName;
			foreach (LogProvider logProvider in MshLog.GetLogProvider(executionContext))
			{
				if (MshLog.NeedToLogPipelineExecutionDetailEvent(logProvider, executionContext))
				{
					logProvider.LogPipelineExecutionDetailEvent(logContext, detail);
				}
			}
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000FD068 File Offset: 0x000FB268
		internal static void LogProviderHealthEvent(ExecutionContext executionContext, string providerName, Exception exception, Severity severity)
		{
			if (executionContext == null)
			{
				PSTraceSource.NewArgumentNullException("executionContext");
				return;
			}
			if (exception == null)
			{
				PSTraceSource.NewArgumentNullException("exception");
				return;
			}
			InvocationInfo invocationInfo = null;
			IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				invocationInfo = containsErrorRecord.ErrorRecord.InvocationInfo;
			}
			foreach (LogProvider logProvider in MshLog.GetLogProvider(executionContext))
			{
				if (MshLog.NeedToLogProviderHealthEvent(logProvider, executionContext))
				{
					logProvider.LogProviderHealthEvent(MshLog.GetLogContext(executionContext, invocationInfo, severity), providerName, exception);
				}
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000FD108 File Offset: 0x000FB308
		internal static void LogProviderLifecycleEvent(ExecutionContext executionContext, string providerName, ProviderState providerState)
		{
			if (executionContext == null)
			{
				PSTraceSource.NewArgumentNullException("executionContext");
				return;
			}
			foreach (LogProvider logProvider in MshLog.GetLogProvider(executionContext))
			{
				if (MshLog.NeedToLogProviderLifecycleEvent(logProvider, executionContext))
				{
					logProvider.LogProviderLifecycleEvent(MshLog.GetLogContext(executionContext, null), providerName, providerState);
				}
			}
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000FD178 File Offset: 0x000FB378
		internal static void LogSettingsEvent(ExecutionContext executionContext, string variableName, string newValue, string previousValue, InvocationInfo invocationInfo)
		{
			if (executionContext == null)
			{
				PSTraceSource.NewArgumentNullException("executionContext");
				return;
			}
			foreach (LogProvider logProvider in MshLog.GetLogProvider(executionContext))
			{
				if (MshLog.NeedToLogSettingsEvent(logProvider, executionContext))
				{
					logProvider.LogSettingsEvent(MshLog.GetLogContext(executionContext, invocationInfo), variableName, newValue, previousValue);
				}
			}
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000FD1E8 File Offset: 0x000FB3E8
		internal static void LogSettingsEvent(ExecutionContext executionContext, string variableName, string newValue, string previousValue)
		{
			MshLog.LogSettingsEvent(executionContext, variableName, newValue, previousValue, null);
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000FD1F4 File Offset: 0x000FB3F4
		private static EngineState GetEngineState(ExecutionContext executionContext)
		{
			return executionContext.EngineState;
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000FD1FC File Offset: 0x000FB3FC
		private static void SetEngineState(ExecutionContext executionContext, EngineState engineState)
		{
			executionContext.EngineState = engineState;
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000FD205 File Offset: 0x000FB405
		internal static LogContext GetLogContext(ExecutionContext executionContext, InvocationInfo invocationInfo)
		{
			return MshLog.GetLogContext(executionContext, invocationInfo, Severity.Informational);
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000FD210 File Offset: 0x000FB410
		private static LogContext GetLogContext(ExecutionContext executionContext, InvocationInfo invocationInfo, Severity severity)
		{
			if (executionContext == null)
			{
				return null;
			}
			LogContext logContext = new LogContext();
			string shellID = executionContext.ShellID;
			logContext.ExecutionContext = executionContext;
			logContext.ShellId = shellID;
			logContext.Severity = severity.ToString();
			if (executionContext.EngineHostInterface != null)
			{
				logContext.HostName = executionContext.EngineHostInterface.Name;
				logContext.HostVersion = executionContext.EngineHostInterface.Version.ToString();
				logContext.HostId = executionContext.EngineHostInterface.InstanceId.ToString();
			}
			logContext.HostApplication = string.Join(" ", Environment.GetCommandLineArgs());
			if (executionContext.CurrentRunspace != null)
			{
				logContext.EngineVersion = executionContext.CurrentRunspace.Version.ToString();
				logContext.RunspaceId = executionContext.CurrentRunspace.InstanceId.ToString();
				Pipeline currentlyRunningPipeline = ((RunspaceBase)executionContext.CurrentRunspace).GetCurrentlyRunningPipeline();
				if (currentlyRunningPipeline != null)
				{
					logContext.PipelineId = currentlyRunningPipeline.InstanceId.ToString(CultureInfo.CurrentCulture);
				}
			}
			logContext.SequenceNumber = MshLog.NextSequenceNumber;
			try
			{
				if (executionContext.LogContextCache.User == null)
				{
					logContext.User = Environment.UserDomainName + "\\" + Environment.UserName;
					executionContext.LogContextCache.User = logContext.User;
				}
				else
				{
					logContext.User = executionContext.LogContextCache.User;
				}
			}
			catch (InvalidOperationException)
			{
				logContext.User = Logging.UnknownUserName;
			}
			PSSenderInfo pssenderInfo = executionContext.SessionState.PSVariable.GetValue("PSSenderInfo") as PSSenderInfo;
			if (pssenderInfo != null)
			{
				logContext.ConnectedUser = pssenderInfo.UserInfo.Identity.Name;
			}
			logContext.Time = DateTime.Now.ToString(CultureInfo.CurrentCulture);
			if (invocationInfo == null)
			{
				return logContext;
			}
			logContext.ScriptName = invocationInfo.ScriptName;
			logContext.CommandLine = invocationInfo.Line;
			if (invocationInfo.MyCommand != null)
			{
				logContext.CommandName = invocationInfo.MyCommand.Name;
				logContext.CommandType = invocationInfo.MyCommand.CommandType.ToString();
				CommandTypes commandType = invocationInfo.MyCommand.CommandType;
				if (commandType != CommandTypes.ExternalScript)
				{
					if (commandType == CommandTypes.Application)
					{
						logContext.CommandPath = ((ApplicationInfo)invocationInfo.MyCommand).Path;
					}
				}
				else
				{
					logContext.CommandPath = ((ExternalScriptInfo)invocationInfo.MyCommand).Path;
				}
			}
			return logContext;
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000FD47C File Offset: 0x000FB67C
		private static bool NeedToLogEngineHealthEvent(LogProvider logProvider, ExecutionContext executionContext)
		{
			return !logProvider.UseLoggingVariables() || LanguagePrimitives.IsTrue(executionContext.GetVariableValue(SpecialVariables.LogEngineHealthEventVarPath, true));
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000FD49E File Offset: 0x000FB69E
		private static bool NeedToLogEngineLifecycleEvent(LogProvider logProvider, ExecutionContext executionContext)
		{
			return !logProvider.UseLoggingVariables() || LanguagePrimitives.IsTrue(executionContext.GetVariableValue(SpecialVariables.LogEngineLifecycleEventVarPath, true));
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000FD4C0 File Offset: 0x000FB6C0
		private static bool NeedToLogCommandHealthEvent(LogProvider logProvider, ExecutionContext executionContext)
		{
			return !logProvider.UseLoggingVariables() || LanguagePrimitives.IsTrue(executionContext.GetVariableValue(SpecialVariables.LogCommandHealthEventVarPath, false));
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000FD4E2 File Offset: 0x000FB6E2
		private static bool NeedToLogCommandLifecycleEvent(LogProvider logProvider, ExecutionContext executionContext)
		{
			return !logProvider.UseLoggingVariables() || LanguagePrimitives.IsTrue(executionContext.GetVariableValue(SpecialVariables.LogCommandLifecycleEventVarPath, false));
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000FD504 File Offset: 0x000FB704
		private static bool NeedToLogPipelineExecutionDetailEvent(LogProvider logProvider, ExecutionContext executionContext)
		{
			return logProvider.UseLoggingVariables() || true;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000FD511 File Offset: 0x000FB711
		private static bool NeedToLogProviderHealthEvent(LogProvider logProvider, ExecutionContext executionContext)
		{
			return !logProvider.UseLoggingVariables() || LanguagePrimitives.IsTrue(executionContext.GetVariableValue(SpecialVariables.LogProviderHealthEventVarPath, true));
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000FD533 File Offset: 0x000FB733
		private static bool NeedToLogProviderLifecycleEvent(LogProvider logProvider, ExecutionContext executionContext)
		{
			return !logProvider.UseLoggingVariables() || LanguagePrimitives.IsTrue(executionContext.GetVariableValue(SpecialVariables.LogProviderLifecycleEventVarPath, true));
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000FD555 File Offset: 0x000FB755
		private static bool NeedToLogSettingsEvent(LogProvider logProvider, ExecutionContext executionContext)
		{
			return !logProvider.UseLoggingVariables() || LanguagePrimitives.IsTrue(executionContext.GetVariableValue(SpecialVariables.LogSettingsEventVarPath, true));
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06002DF1 RID: 11761 RVA: 0x000FD577 File Offset: 0x000FB777
		private static string NextSequenceNumber
		{
			get
			{
				return Convert.ToString(Interlocked.Increment(ref MshLog._nextSequenceNumber), CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x04001803 RID: 6147
		private const string _crimsonLogProviderAssemblyName = "MshCrimsonLog";

		// Token: 0x04001804 RID: 6148
		private const string _crimsonLogProviderTypeName = "System.Management.Automation.Logging.CrimsonLogProvider";

		// Token: 0x04001805 RID: 6149
		internal const int EVENT_ID_GENERAL_HEALTH_ISSUE = 100;

		// Token: 0x04001806 RID: 6150
		internal const int EVENT_ID_RESOURCE_NOT_AVAILABLE = 101;

		// Token: 0x04001807 RID: 6151
		internal const int EVENT_ID_NETWORK_CONNECTIVITY_ISSUE = 102;

		// Token: 0x04001808 RID: 6152
		internal const int EVENT_ID_CONFIGURATION_FAILURE = 103;

		// Token: 0x04001809 RID: 6153
		internal const int EVENT_ID_PERFORMANCE_ISSUE = 104;

		// Token: 0x0400180A RID: 6154
		internal const int EVENT_ID_SECURITY_ISSUE = 105;

		// Token: 0x0400180B RID: 6155
		internal const int EVENT_ID_SYSTEM_OVERLOADED = 106;

		// Token: 0x0400180C RID: 6156
		internal const int EVENT_ID_UNEXPECTED_EXCEPTION = 195;

		// Token: 0x0400180D RID: 6157
		private static ConcurrentDictionary<string, Collection<LogProvider>> _logProviders = new ConcurrentDictionary<string, Collection<LogProvider>>();

		// Token: 0x0400180E RID: 6158
		private static Collection<string> ignoredCommands = new Collection<string>();

		// Token: 0x0400180F RID: 6159
		private static int _nextSequenceNumber = 0;
	}
}
