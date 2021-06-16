using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x020003FB RID: 1019
	internal class EventLogLogProvider : LogProvider
	{
		// Token: 0x06002DF5 RID: 11765 RVA: 0x000FD5A8 File Offset: 0x000FB7A8
		internal EventLogLogProvider(string shellId)
		{
			string source = this.SetupEventSource(shellId);
			this._eventLog = new EventLog();
			this._eventLog.Source = source;
			this._resourceManager = new ResourceManager("Logging", Assembly.GetExecutingAssembly());
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000FD5F0 File Offset: 0x000FB7F0
		internal string SetupEventSource(string shellId)
		{
			string text;
			if (string.IsNullOrEmpty(shellId))
			{
				text = "Default";
			}
			else
			{
				int num = shellId.LastIndexOf('.');
				if (num < 0)
				{
					text = shellId;
				}
				else
				{
					text = shellId.Substring(num + 1);
				}
				if (string.IsNullOrEmpty(text))
				{
					text = "Default";
				}
			}
			if (EventLog.SourceExists(text))
			{
				return text;
			}
			string message = string.Format(Thread.CurrentThread.CurrentCulture, "Event source '{0}' is not registered", new object[]
			{
				text
			});
			throw new InvalidOperationException(message);
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000FD668 File Offset: 0x000FB868
		internal override void LogEngineHealthEvent(LogContext logContext, int eventId, Exception exception, Dictionary<string, string> additionalInfo)
		{
			Hashtable hashtable = new Hashtable();
			IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				hashtable["ExceptionClass"] = exception.GetType().Name;
				hashtable["ErrorCategory"] = containsErrorRecord.ErrorRecord.CategoryInfo.Category;
				hashtable["ErrorId"] = containsErrorRecord.ErrorRecord.FullyQualifiedErrorId;
				if (containsErrorRecord.ErrorRecord.ErrorDetails != null)
				{
					hashtable["ErrorMessage"] = containsErrorRecord.ErrorRecord.ErrorDetails.Message;
				}
				else
				{
					hashtable["ErrorMessage"] = exception.Message;
				}
			}
			else
			{
				hashtable["ExceptionClass"] = exception.GetType().Name;
				hashtable["ErrorCategory"] = "";
				hashtable["ErrorId"] = "";
				hashtable["ErrorMessage"] = exception.Message;
			}
			EventLogLogProvider.FillEventArgs(hashtable, logContext);
			EventLogLogProvider.FillEventArgs(hashtable, additionalInfo);
			EventInstance eventInstance = new EventInstance((long)eventId, 1);
			eventInstance.EntryType = EventLogLogProvider.GetEventLogEntryType(logContext);
			string eventDetail = this.GetEventDetail("EngineHealthContext", hashtable);
			this.LogEvent(eventInstance, new object[]
			{
				hashtable["ErrorMessage"],
				eventDetail
			});
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000FD7BC File Offset: 0x000FB9BC
		private static EventLogEntryType GetEventLogEntryType(LogContext logContext)
		{
			string severity;
			if ((severity = logContext.Severity) != null)
			{
				if (severity == "Critical" || severity == "Error")
				{
					return EventLogEntryType.Error;
				}
				if (severity == "Warning")
				{
					return EventLogEntryType.Warning;
				}
			}
			return EventLogEntryType.Information;
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000FD804 File Offset: 0x000FBA04
		internal override void LogEngineLifecycleEvent(LogContext logContext, EngineState newState, EngineState previousState)
		{
			int engineLifecycleEventId = EventLogLogProvider.GetEngineLifecycleEventId(newState);
			if (engineLifecycleEventId == -1)
			{
				return;
			}
			Hashtable hashtable = new Hashtable();
			hashtable["NewEngineState"] = newState.ToString();
			hashtable["PreviousEngineState"] = previousState.ToString();
			EventLogLogProvider.FillEventArgs(hashtable, logContext);
			EventInstance eventInstance = new EventInstance((long)engineLifecycleEventId, 4);
			eventInstance.EntryType = EventLogEntryType.Information;
			string eventDetail = this.GetEventDetail("EngineLifecycleContext", hashtable);
			this.LogEvent(eventInstance, new object[]
			{
				newState,
				previousState,
				eventDetail
			});
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000FD8A0 File Offset: 0x000FBAA0
		private static int GetEngineLifecycleEventId(EngineState engineState)
		{
			switch (engineState)
			{
			case EngineState.None:
				return -1;
			case EngineState.Available:
				return 400;
			case EngineState.Degraded:
				return 401;
			case EngineState.OutOfService:
				return 402;
			case EngineState.Stopped:
				return 403;
			default:
				return -1;
			}
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x000FD8E8 File Offset: 0x000FBAE8
		internal override void LogCommandHealthEvent(LogContext logContext, Exception exception)
		{
			int num = 200;
			Hashtable hashtable = new Hashtable();
			IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				hashtable["ExceptionClass"] = exception.GetType().Name;
				hashtable["ErrorCategory"] = containsErrorRecord.ErrorRecord.CategoryInfo.Category;
				hashtable["ErrorId"] = containsErrorRecord.ErrorRecord.FullyQualifiedErrorId;
				if (containsErrorRecord.ErrorRecord.ErrorDetails != null)
				{
					hashtable["ErrorMessage"] = containsErrorRecord.ErrorRecord.ErrorDetails.Message;
				}
				else
				{
					hashtable["ErrorMessage"] = exception.Message;
				}
			}
			else
			{
				hashtable["ExceptionClass"] = exception.GetType().Name;
				hashtable["ErrorCategory"] = "";
				hashtable["ErrorId"] = "";
				hashtable["ErrorMessage"] = exception.Message;
			}
			EventLogLogProvider.FillEventArgs(hashtable, logContext);
			EventInstance eventInstance = new EventInstance((long)num, 2);
			eventInstance.EntryType = EventLogLogProvider.GetEventLogEntryType(logContext);
			string eventDetail = this.GetEventDetail("CommandHealthContext", hashtable);
			this.LogEvent(eventInstance, new object[]
			{
				hashtable["ErrorMessage"],
				eventDetail
			});
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000FDA3C File Offset: 0x000FBC3C
		internal override void LogCommandLifecycleEvent(Func<LogContext> getLogContext, CommandState newState)
		{
			LogContext logContext = getLogContext();
			int commandLifecycleEventId = EventLogLogProvider.GetCommandLifecycleEventId(newState);
			if (commandLifecycleEventId == -1)
			{
				return;
			}
			Hashtable hashtable = new Hashtable();
			hashtable["NewCommandState"] = newState.ToString();
			EventLogLogProvider.FillEventArgs(hashtable, logContext);
			EventInstance eventInstance = new EventInstance((long)commandLifecycleEventId, 5);
			eventInstance.EntryType = EventLogEntryType.Information;
			string eventDetail = this.GetEventDetail("CommandLifecycleContext", hashtable);
			this.LogEvent(eventInstance, new object[]
			{
				logContext.CommandName,
				newState,
				eventDetail
			});
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000FDAC8 File Offset: 0x000FBCC8
		private static int GetCommandLifecycleEventId(CommandState commandState)
		{
			switch (commandState)
			{
			case CommandState.Started:
				return 500;
			case CommandState.Stopped:
				return 501;
			case CommandState.Terminated:
				return 502;
			default:
				return -1;
			}
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000FDB00 File Offset: 0x000FBD00
		internal override void LogPipelineExecutionDetailEvent(LogContext logContext, List<string> pipelineExecutionDetail)
		{
			List<string> list = this.GroupMessages(pipelineExecutionDetail);
			for (int i = 0; i < list.Count; i++)
			{
				this.LogPipelineExecutionDetailEvent(logContext, list[i], i + 1, list.Count);
			}
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000FDB40 File Offset: 0x000FBD40
		private List<string> GroupMessages(List<string> messages)
		{
			List<string> list = new List<string>();
			if (messages == null || messages.Count == 0)
			{
				return list;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < messages.Count; i++)
			{
				if (stringBuilder.Length + messages[i].Length < 16000)
				{
					stringBuilder.AppendLine(messages[i]);
				}
				else
				{
					list.Add(stringBuilder.ToString());
					stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(messages[i]);
				}
			}
			list.Add(stringBuilder.ToString());
			return list;
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x000FDBD0 File Offset: 0x000FBDD0
		private void LogPipelineExecutionDetailEvent(LogContext logContext, string pipelineExecutionDetail, int detailSequence, int detailTotal)
		{
			int num = 800;
			Hashtable hashtable = new Hashtable();
			hashtable["PipelineExecutionDetail"] = pipelineExecutionDetail;
			hashtable["DetailSequence"] = detailSequence;
			hashtable["DetailTotal"] = detailTotal;
			EventLogLogProvider.FillEventArgs(hashtable, logContext);
			EventInstance eventInstance = new EventInstance((long)num, 8);
			eventInstance.EntryType = EventLogEntryType.Information;
			string eventDetail = this.GetEventDetail("PipelineExecutionDetailContext", hashtable);
			this.LogEvent(eventInstance, new object[]
			{
				logContext.CommandLine,
				eventDetail,
				pipelineExecutionDetail
			});
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000FDC64 File Offset: 0x000FBE64
		internal override void LogProviderHealthEvent(LogContext logContext, string providerName, Exception exception)
		{
			int num = 300;
			Hashtable hashtable = new Hashtable();
			hashtable["ProviderName"] = providerName;
			IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
			if (containsErrorRecord != null && containsErrorRecord.ErrorRecord != null)
			{
				hashtable["ExceptionClass"] = exception.GetType().Name;
				hashtable["ErrorCategory"] = containsErrorRecord.ErrorRecord.CategoryInfo.Category;
				hashtable["ErrorId"] = containsErrorRecord.ErrorRecord.FullyQualifiedErrorId;
				if (containsErrorRecord.ErrorRecord.ErrorDetails != null && !string.IsNullOrEmpty(containsErrorRecord.ErrorRecord.ErrorDetails.Message))
				{
					hashtable["ErrorMessage"] = containsErrorRecord.ErrorRecord.ErrorDetails.Message;
				}
				else
				{
					hashtable["ErrorMessage"] = exception.Message;
				}
			}
			else
			{
				hashtable["ExceptionClass"] = exception.GetType().Name;
				hashtable["ErrorCategory"] = "";
				hashtable["ErrorId"] = "";
				hashtable["ErrorMessage"] = exception.Message;
			}
			EventLogLogProvider.FillEventArgs(hashtable, logContext);
			EventInstance eventInstance = new EventInstance((long)num, 3);
			eventInstance.EntryType = EventLogLogProvider.GetEventLogEntryType(logContext);
			string eventDetail = this.GetEventDetail("ProviderHealthContext", hashtable);
			this.LogEvent(eventInstance, new object[]
			{
				hashtable["ErrorMessage"],
				eventDetail
			});
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000FDDD8 File Offset: 0x000FBFD8
		internal override void LogProviderLifecycleEvent(LogContext logContext, string providerName, ProviderState newState)
		{
			int providerLifecycleEventId = EventLogLogProvider.GetProviderLifecycleEventId(newState);
			if (providerLifecycleEventId == -1)
			{
				return;
			}
			Hashtable hashtable = new Hashtable();
			hashtable["ProviderName"] = providerName;
			hashtable["NewProviderState"] = newState.ToString();
			EventLogLogProvider.FillEventArgs(hashtable, logContext);
			EventInstance eventInstance = new EventInstance((long)providerLifecycleEventId, 6);
			eventInstance.EntryType = EventLogEntryType.Information;
			string eventDetail = this.GetEventDetail("ProviderLifecycleContext", hashtable);
			this.LogEvent(eventInstance, new object[]
			{
				providerName,
				newState,
				eventDetail
			});
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000FDE64 File Offset: 0x000FC064
		private static int GetProviderLifecycleEventId(ProviderState providerState)
		{
			switch (providerState)
			{
			case ProviderState.Started:
				return 600;
			case ProviderState.Stopped:
				return 601;
			default:
				return -1;
			}
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000FDE90 File Offset: 0x000FC090
		internal override void LogSettingsEvent(LogContext logContext, string variableName, string value, string previousValue)
		{
			int num = 700;
			Hashtable hashtable = new Hashtable();
			hashtable["VariableName"] = variableName;
			hashtable["NewValue"] = value;
			hashtable["PreviousValue"] = previousValue;
			EventLogLogProvider.FillEventArgs(hashtable, logContext);
			EventInstance eventInstance = new EventInstance((long)num, 7);
			eventInstance.EntryType = EventLogEntryType.Information;
			string eventDetail = this.GetEventDetail("SettingsContext", hashtable);
			this.LogEvent(eventInstance, new object[]
			{
				variableName,
				value,
				previousValue,
				eventDetail
			});
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000FDF18 File Offset: 0x000FC118
		private void LogEvent(EventInstance entry, params object[] args)
		{
			try
			{
				this._eventLog.WriteEvent(entry, args);
			}
			catch (ArgumentException)
			{
			}
			catch (InvalidOperationException)
			{
			}
			catch (Win32Exception)
			{
			}
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000FDF68 File Offset: 0x000FC168
		private static void FillEventArgs(Hashtable mapArgs, LogContext logContext)
		{
			mapArgs["Severity"] = logContext.Severity;
			mapArgs["SequenceNumber"] = logContext.SequenceNumber;
			mapArgs["HostName"] = logContext.HostName;
			mapArgs["HostVersion"] = logContext.HostVersion;
			mapArgs["HostId"] = logContext.HostId;
			mapArgs["HostApplication"] = logContext.HostApplication;
			mapArgs["EngineVersion"] = logContext.EngineVersion;
			mapArgs["RunspaceId"] = logContext.RunspaceId;
			mapArgs["PipelineId"] = logContext.PipelineId;
			mapArgs["CommandName"] = logContext.CommandName;
			mapArgs["CommandType"] = logContext.CommandType;
			mapArgs["ScriptName"] = logContext.ScriptName;
			mapArgs["CommandPath"] = logContext.CommandPath;
			mapArgs["CommandLine"] = logContext.CommandLine;
			mapArgs["User"] = logContext.User;
			mapArgs["Time"] = logContext.Time;
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000FE088 File Offset: 0x000FC288
		private static void FillEventArgs(Hashtable mapArgs, Dictionary<string, string> additionalInfo)
		{
			if (additionalInfo == null)
			{
				for (int i = 0; i < 3; i++)
				{
					string str = (i + 1).ToString("d1", CultureInfo.CurrentCulture);
					mapArgs["AdditionalInfo_Name" + str] = "";
					mapArgs["AdditionalInfo_Value" + str] = "";
				}
				return;
			}
			string[] array = new string[additionalInfo.Count];
			string[] array2 = new string[additionalInfo.Count];
			additionalInfo.Keys.CopyTo(array, 0);
			additionalInfo.Values.CopyTo(array2, 0);
			for (int j = 0; j < 3; j++)
			{
				string str2 = (j + 1).ToString("d1", CultureInfo.CurrentCulture);
				if (j < array.Length)
				{
					mapArgs["AdditionalInfo_Name" + str2] = array[j];
					mapArgs["AdditionalInfo_Value" + str2] = array2[j];
				}
				else
				{
					mapArgs["AdditionalInfo_Name" + str2] = "";
					mapArgs["AdditionalInfo_Value" + str2] = "";
				}
			}
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000FE1AC File Offset: 0x000FC3AC
		private string GetEventDetail(string contextId, Hashtable mapArgs)
		{
			return this.GetMessage(contextId, mapArgs);
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000FE1B8 File Offset: 0x000FC3B8
		private string GetMessage(string messageId, Hashtable mapArgs)
		{
			if (this._resourceManager == null)
			{
				return "";
			}
			string @string = this._resourceManager.GetString(messageId);
			if (string.IsNullOrEmpty(@string))
			{
				return "";
			}
			return EventLogLogProvider.FillMessageTemplate(@string, mapArgs);
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000FE1F8 File Offset: 0x000FC3F8
		private static string FillMessageTemplate(string messageTemplate, Hashtable mapArgs)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (;;)
			{
				int num2 = messageTemplate.IndexOf('[', num);
				if (num2 < 0)
				{
					break;
				}
				int num3 = messageTemplate.IndexOf(']', num2 + 1);
				if (num3 < 0)
				{
					goto Block_2;
				}
				stringBuilder.Append(messageTemplate.Substring(num, num2 - num));
				num = num2;
				string key = messageTemplate.Substring(num2 + 1, num3 - num2 - 1);
				if (mapArgs.Contains(key))
				{
					stringBuilder.Append(mapArgs[key]);
					num = num3 + 1;
				}
				else
				{
					stringBuilder.Append("[");
					num++;
				}
			}
			stringBuilder.Append(messageTemplate.Substring(num));
			return stringBuilder.ToString();
			Block_2:
			stringBuilder.Append(messageTemplate.Substring(num));
			return stringBuilder.ToString();
		}

		// Token: 0x0400181E RID: 6174
		private const int EngineHealthCategoryId = 1;

		// Token: 0x0400181F RID: 6175
		private const int CommandHealthCategoryId = 2;

		// Token: 0x04001820 RID: 6176
		private const int ProviderHealthCategoryId = 3;

		// Token: 0x04001821 RID: 6177
		private const int EngineLifecycleCategoryId = 4;

		// Token: 0x04001822 RID: 6178
		private const int CommandLifecycleCategoryId = 5;

		// Token: 0x04001823 RID: 6179
		private const int ProviderLifecycleCategoryId = 6;

		// Token: 0x04001824 RID: 6180
		private const int SettingsCategoryId = 7;

		// Token: 0x04001825 RID: 6181
		private const int PipelineExecutionDetailCategoryId = 8;

		// Token: 0x04001826 RID: 6182
		private const int _baseEngineLifecycleEventId = 400;

		// Token: 0x04001827 RID: 6183
		private const int _invalidEventId = -1;

		// Token: 0x04001828 RID: 6184
		private const int _commandHealthEventId = 200;

		// Token: 0x04001829 RID: 6185
		private const int _baseCommandLifecycleEventId = 500;

		// Token: 0x0400182A RID: 6186
		private const int _pipelineExecutionDetailEventId = 800;

		// Token: 0x0400182B RID: 6187
		private const int MaxLength = 16000;

		// Token: 0x0400182C RID: 6188
		private const int _providerHealthEventId = 300;

		// Token: 0x0400182D RID: 6189
		private const int _baseProviderLifecycleEventId = 600;

		// Token: 0x0400182E RID: 6190
		private const int _settingsEventId = 700;

		// Token: 0x0400182F RID: 6191
		private EventLog _eventLog;

		// Token: 0x04001830 RID: 6192
		private ResourceManager _resourceManager;
	}
}
