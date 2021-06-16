using System;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation.Tracing
{
	// Token: 0x020008E1 RID: 2273
	public sealed class PowerShellTraceSource : IDisposable
	{
		// Token: 0x06005598 RID: 21912 RVA: 0x001C2448 File Offset: 0x001C0648
		internal PowerShellTraceSource(PowerShellTraceTask task, PowerShellTraceKeywords keywords)
		{
			if (this.IsEtwSupported)
			{
				this.debugChannel = new PowerShellChannelWriter(PowerShellTraceChannel.Debug, keywords | PowerShellTraceKeywords.UseAlwaysDebug);
				this.analyticChannel = new PowerShellChannelWriter(PowerShellTraceChannel.Analytic, keywords | PowerShellTraceKeywords.UseAlwaysAnalytic);
				this.operationsChannel = new PowerShellChannelWriter(PowerShellTraceChannel.Operational, keywords | PowerShellTraceKeywords.UseAlwaysOperational);
				this.task = task;
				this.keywords = keywords;
				return;
			}
			this.debugChannel = NullWriter.Instance;
			this.analyticChannel = NullWriter.Instance;
			this.operationsChannel = NullWriter.Instance;
		}

		// Token: 0x06005599 RID: 21913 RVA: 0x001C24DB File Offset: 0x001C06DB
		public void Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				GC.SuppressFinalize(this);
				this.debugChannel.Dispose();
				this.analyticChannel.Dispose();
				this.operationsChannel.Dispose();
			}
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x0600559A RID: 21914 RVA: 0x001C2513 File Offset: 0x001C0713
		public PowerShellTraceKeywords Keywords
		{
			get
			{
				return this.keywords;
			}
		}

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x0600559B RID: 21915 RVA: 0x001C251B File Offset: 0x001C071B
		// (set) Token: 0x0600559C RID: 21916 RVA: 0x001C2523 File Offset: 0x001C0723
		public PowerShellTraceTask Task
		{
			get
			{
				return this.task;
			}
			set
			{
				this.task = value;
			}
		}

		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x0600559D RID: 21917 RVA: 0x001C252C File Offset: 0x001C072C
		private bool IsEtwSupported
		{
			get
			{
				return Environment.OSVersion.Version.Major >= 6;
			}
		}

		// Token: 0x0600559E RID: 21918 RVA: 0x001C2544 File Offset: 0x001C0744
		public bool TraceErrorRecord(ErrorRecord errorRecord)
		{
			if (errorRecord != null)
			{
				Exception exception = errorRecord.Exception;
				string text = "None";
				if (exception.InnerException != null)
				{
					text = exception.InnerException.Message;
				}
				ErrorCategoryInfo categoryInfo = errorRecord.CategoryInfo;
				string text2 = "None";
				if (errorRecord.ErrorDetails != null)
				{
					text2 = errorRecord.ErrorDetails.Message;
				}
				return this.debugChannel.TraceError(PowerShellTraceEvent.ErrorRecord, PowerShellTraceOperationCode.Exception, PowerShellTraceTask.None, new object[]
				{
					text2,
					categoryInfo.Category.ToString(),
					categoryInfo.Reason,
					categoryInfo.TargetName,
					errorRecord.FullyQualifiedErrorId,
					exception.Message,
					exception.StackTrace,
					text
				});
			}
			return this.debugChannel.TraceError(PowerShellTraceEvent.ErrorRecord, PowerShellTraceOperationCode.Exception, PowerShellTraceTask.None, new object[]
			{
				"NULL errorRecord"
			});
		}

		// Token: 0x0600559F RID: 21919 RVA: 0x001C2630 File Offset: 0x001C0830
		public bool TraceException(Exception exception)
		{
			if (exception != null)
			{
				string text = "None";
				if (exception.InnerException != null)
				{
					text = exception.InnerException.Message;
				}
				return this.debugChannel.TraceError(PowerShellTraceEvent.Exception, PowerShellTraceOperationCode.Exception, PowerShellTraceTask.None, new object[]
				{
					exception.Message,
					exception.StackTrace,
					text
				});
			}
			return this.debugChannel.TraceError(PowerShellTraceEvent.Exception, PowerShellTraceOperationCode.Exception, PowerShellTraceTask.None, new object[]
			{
				"NULL exception"
			});
		}

		// Token: 0x060055A0 RID: 21920 RVA: 0x001C26AF File Offset: 0x001C08AF
		public bool TracePowerShellObject(PSObject powerShellObject)
		{
			return this.debugChannel.TraceDebug(PowerShellTraceEvent.PowerShellObject, PowerShellTraceOperationCode.Method, PowerShellTraceTask.None, new object[0]);
		}

		// Token: 0x060055A1 RID: 21921 RVA: 0x001C26CC File Offset: 0x001C08CC
		public bool TraceJob(Job job)
		{
			if (job != null)
			{
				return this.debugChannel.TraceDebug(PowerShellTraceEvent.Job, PowerShellTraceOperationCode.Method, PowerShellTraceTask.None, new object[]
				{
					job.Id.ToString(CultureInfo.InvariantCulture),
					job.InstanceId.ToString(),
					job.Name,
					job.Location,
					job.JobStateInfo.State.ToString(),
					job.Command
				});
			}
			return this.debugChannel.TraceDebug(PowerShellTraceEvent.Job, PowerShellTraceOperationCode.Method, PowerShellTraceTask.None, new object[]
			{
				job.Id.ToString(CultureInfo.InvariantCulture),
				job.InstanceId.ToString(),
				"NULL job"
			});
		}

		// Token: 0x060055A2 RID: 21922 RVA: 0x001C27AC File Offset: 0x001C09AC
		public bool WriteMessage(string message)
		{
			return this.debugChannel.TraceInformational(PowerShellTraceEvent.TraceMessage, PowerShellTraceOperationCode.None, PowerShellTraceTask.None, new object[]
			{
				message
			});
		}

		// Token: 0x060055A3 RID: 21923 RVA: 0x001C27D8 File Offset: 0x001C09D8
		public bool WriteMessage(string message1, string message2)
		{
			return this.debugChannel.TraceInformational(PowerShellTraceEvent.TraceMessage2, PowerShellTraceOperationCode.None, PowerShellTraceTask.None, new object[]
			{
				message1,
				message2
			});
		}

		// Token: 0x060055A4 RID: 21924 RVA: 0x001C2808 File Offset: 0x001C0A08
		public bool WriteMessage(string message, Guid instanceId)
		{
			return this.debugChannel.TraceInformational(PowerShellTraceEvent.TraceMessageGuid, PowerShellTraceOperationCode.None, PowerShellTraceTask.None, new object[]
			{
				message,
				instanceId
			});
		}

		// Token: 0x060055A5 RID: 21925 RVA: 0x001C283C File Offset: 0x001C0A3C
		public void WriteMessage(string className, string methodName, Guid workflowId, string message, params string[] parameters)
		{
			PSEtwLog.LogAnalyticVerbose(PSEventId.Engine_Trace, PSOpcode.Method, PSTask.None, PSKeyword.UseAlwaysAnalytic, new object[]
			{
				className,
				methodName,
				workflowId.ToString(),
				(parameters == null) ? message : StringUtil.Format(message, parameters),
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty
			});
		}

		// Token: 0x060055A6 RID: 21926 RVA: 0x001C28B4 File Offset: 0x001C0AB4
		public void WriteMessage(string className, string methodName, Guid workflowId, Job job, string message, params string[] parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (job != null)
			{
				try
				{
					stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.JobName, job.Name));
					stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.JobId, job.Id.ToString(CultureInfo.InvariantCulture)));
					stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.JobInstanceId, job.InstanceId.ToString()));
					stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.JobLocation, job.Location));
					stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.JobState, job.JobStateInfo.State.ToString()));
					stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.JobCommand, job.Command));
					goto IL_10D;
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					this.TraceException(ex);
					stringBuilder.Clear();
					stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.JobName, EtwLoggingStrings.NullJobName));
					goto IL_10D;
				}
			}
			stringBuilder.AppendLine(StringUtil.Format(EtwLoggingStrings.JobName, EtwLoggingStrings.NullJobName));
			IL_10D:
			PSEtwLog.LogAnalyticVerbose(PSEventId.Engine_Trace, PSOpcode.Method, PSTask.None, PSKeyword.UseAlwaysAnalytic, new object[]
			{
				className,
				methodName,
				workflowId.ToString(),
				(parameters == null) ? message : StringUtil.Format(message, parameters),
				stringBuilder.ToString(),
				string.Empty,
				string.Empty,
				string.Empty
			});
		}

		// Token: 0x060055A7 RID: 21927 RVA: 0x001C2A54 File Offset: 0x001C0C54
		public void WriteScheduledJobStartEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ScheduledJob_Start, PSOpcode.Method, PSTask.ScheduledJob, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055A8 RID: 21928 RVA: 0x001C2A6E File Offset: 0x001C0C6E
		public void WriteScheduledJobCompleteEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ScheduledJob_Complete, PSOpcode.Method, PSTask.ScheduledJob, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055A9 RID: 21929 RVA: 0x001C2A88 File Offset: 0x001C0C88
		public void WriteScheduledJobErrorEvent(params object[] args)
		{
			PSEtwLog.LogOperationalError(PSEventId.ScheduledJob_Error, PSOpcode.Exception, PSTask.ScheduledJob, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055AA RID: 21930 RVA: 0x001C2AA2 File Offset: 0x001C0CA2
		public void WriteISEExecuteScriptEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEExecuteScript, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055AB RID: 21931 RVA: 0x001C2ABC File Offset: 0x001C0CBC
		public void WriteISEExecuteSelectionEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEExecuteSelection, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055AC RID: 21932 RVA: 0x001C2AD6 File Offset: 0x001C0CD6
		public void WriteISEStopCommandEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEStopCommand, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055AD RID: 21933 RVA: 0x001C2AF0 File Offset: 0x001C0CF0
		public void WriteISEResumeDebuggerEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEResumeDebugger, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055AE RID: 21934 RVA: 0x001C2B0A File Offset: 0x001C0D0A
		public void WriteISEStopDebuggerEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEStopDebugger, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055AF RID: 21935 RVA: 0x001C2B24 File Offset: 0x001C0D24
		public void WriteISEDebuggerStepIntoEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEDebuggerStepInto, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055B0 RID: 21936 RVA: 0x001C2B3E File Offset: 0x001C0D3E
		public void WriteISEDebuggerStepOverEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEDebuggerStepOver, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055B1 RID: 21937 RVA: 0x001C2B58 File Offset: 0x001C0D58
		public void WriteISEDebuggerStepOutEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEDebuggerStepOut, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055B2 RID: 21938 RVA: 0x001C2B72 File Offset: 0x001C0D72
		public void WriteISEEnableAllBreakpointsEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEEnableAllBreakpoints, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055B3 RID: 21939 RVA: 0x001C2B8C File Offset: 0x001C0D8C
		public void WriteISEDisableAllBreakpointsEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEDisableAllBreakpoints, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055B4 RID: 21940 RVA: 0x001C2BA6 File Offset: 0x001C0DA6
		public void WriteISERemoveAllBreakpointsEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISERemoveAllBreakpoints, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055B5 RID: 21941 RVA: 0x001C2BC0 File Offset: 0x001C0DC0
		public void WriteISESetBreakpointEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISESetBreakpoint, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055B6 RID: 21942 RVA: 0x001C2BDA File Offset: 0x001C0DDA
		public void WriteISERemoveBreakpointEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISERemoveBreakpoint, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055B7 RID: 21943 RVA: 0x001C2BF4 File Offset: 0x001C0DF4
		public void WriteISEEnableBreakpointEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEEnableBreakpoint, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055B8 RID: 21944 RVA: 0x001C2C0E File Offset: 0x001C0E0E
		public void WriteISEDisableBreakpointEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEDisableBreakpoint, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055B9 RID: 21945 RVA: 0x001C2C28 File Offset: 0x001C0E28
		public void WriteISEHitBreakpointEvent(params object[] args)
		{
			PSEtwLog.LogOperationalInformation(PSEventId.ISEHitBreakpoint, PSOpcode.Method, PSTask.ISEOperation, PSKeyword.UseAlwaysOperational, args);
		}

		// Token: 0x060055BA RID: 21946 RVA: 0x001C2C44 File Offset: 0x001C0E44
		public void WriteMessage(string className, string methodName, Guid workflowId, string activityName, Guid activityId, string message, params string[] parameters)
		{
			PSEtwLog.LogAnalyticVerbose(PSEventId.Engine_Trace, PSOpcode.Method, PSTask.None, PSKeyword.UseAlwaysAnalytic, new object[]
			{
				className,
				methodName,
				workflowId.ToString(),
				(parameters == null) ? message : StringUtil.Format(message, parameters),
				string.Empty,
				activityName,
				activityId.ToString(),
				string.Empty
			});
		}

		// Token: 0x060055BB RID: 21947 RVA: 0x001C2CC0 File Offset: 0x001C0EC0
		public bool TraceWSManConnectionInfo(WSManConnectionInfo connectionInfo)
		{
			return true;
		}

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x060055BC RID: 21948 RVA: 0x001C2CC3 File Offset: 0x001C0EC3
		public BaseChannelWriter DebugChannel
		{
			get
			{
				return this.debugChannel;
			}
		}

		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x060055BD RID: 21949 RVA: 0x001C2CCB File Offset: 0x001C0ECB
		public BaseChannelWriter AnalyticChannel
		{
			get
			{
				return this.analyticChannel;
			}
		}

		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x060055BE RID: 21950 RVA: 0x001C2CD3 File Offset: 0x001C0ED3
		public BaseChannelWriter OperationalChannel
		{
			get
			{
				return this.operationsChannel;
			}
		}

		// Token: 0x04002D4F RID: 11599
		private PowerShellTraceKeywords keywords;

		// Token: 0x04002D50 RID: 11600
		private PowerShellTraceTask task;

		// Token: 0x04002D51 RID: 11601
		private readonly BaseChannelWriter debugChannel;

		// Token: 0x04002D52 RID: 11602
		private readonly BaseChannelWriter analyticChannel;

		// Token: 0x04002D53 RID: 11603
		private readonly BaseChannelWriter operationsChannel;

		// Token: 0x04002D54 RID: 11604
		private bool disposed;
	}
}
