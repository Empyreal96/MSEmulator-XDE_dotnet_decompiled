using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Provider;

namespace System.Management.Automation
{
	// Token: 0x02000466 RID: 1126
	internal sealed class CmdletProviderContext
	{
		// Token: 0x060031FB RID: 12795 RVA: 0x0010EF20 File Offset: 0x0010D120
		internal CmdletProviderContext(ExecutionContext executionContext)
		{
			if (executionContext == null)
			{
				throw PSTraceSource.NewArgumentNullException("executionContext");
			}
			this.executionContext = executionContext;
			this._origin = CommandOrigin.Internal;
			this.drive = executionContext.EngineSessionState.CurrentDrive;
			if (executionContext.CurrentCommandProcessor != null && executionContext.CurrentCommandProcessor.Command is Cmdlet)
			{
				this.command = (Cmdlet)executionContext.CurrentCommandProcessor.Command;
			}
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x0010EFC4 File Offset: 0x0010D1C4
		internal CmdletProviderContext(ExecutionContext executionContext, CommandOrigin origin)
		{
			if (executionContext == null)
			{
				throw PSTraceSource.NewArgumentNullException("executionContext");
			}
			this.executionContext = executionContext;
			this._origin = origin;
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x0010F028 File Offset: 0x0010D228
		internal CmdletProviderContext(PSCmdlet command, PSCredential credentials, PSDriveInfo drive)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			this.command = command;
			this._origin = command.CommandOrigin;
			if (credentials != null)
			{
				this.credentials = credentials;
			}
			this.drive = drive;
			if (command.Host == null)
			{
				throw PSTraceSource.NewArgumentException("command.Host");
			}
			if (command.Context == null)
			{
				throw PSTraceSource.NewArgumentException("command.Context");
			}
			this.executionContext = command.Context;
			this.streamObjects = true;
			this.streamErrors = true;
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x0010F0E0 File Offset: 0x0010D2E0
		internal CmdletProviderContext(PSCmdlet command, PSCredential credentials)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			this.command = command;
			this._origin = command.CommandOrigin;
			if (credentials != null)
			{
				this.credentials = credentials;
			}
			if (command.Host == null)
			{
				throw PSTraceSource.NewArgumentException("command.Host");
			}
			if (command.Context == null)
			{
				throw PSTraceSource.NewArgumentException("command.Context");
			}
			this.executionContext = command.Context;
			this.streamObjects = true;
			this.streamErrors = true;
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x0010F194 File Offset: 0x0010D394
		internal CmdletProviderContext(Cmdlet command)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			this.command = command;
			this._origin = command.CommandOrigin;
			if (command.Context == null)
			{
				throw PSTraceSource.NewArgumentException("command.Context");
			}
			this.executionContext = command.Context;
			this.streamObjects = true;
			this.streamErrors = true;
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x0010F228 File Offset: 0x0010D428
		internal CmdletProviderContext(CmdletProviderContext contextToCopyFrom)
		{
			if (contextToCopyFrom == null)
			{
				throw PSTraceSource.NewArgumentNullException("contextToCopyFrom");
			}
			this.executionContext = contextToCopyFrom.ExecutionContext;
			this.command = contextToCopyFrom.command;
			if (contextToCopyFrom.Credential != null)
			{
				this.credentials = contextToCopyFrom.Credential;
			}
			this.drive = contextToCopyFrom.Drive;
			this.force = contextToCopyFrom.Force;
			this.CopyFilters(contextToCopyFrom);
			this.suppressWildcardExpansion = contextToCopyFrom.SuppressWildcardExpansion;
			this.dynamicParameters = contextToCopyFrom.DynamicParameters;
			this._origin = contextToCopyFrom._origin;
			this.stopping = contextToCopyFrom.Stopping;
			contextToCopyFrom.StopReferrals.Add(this);
			this.copiedContext = contextToCopyFrom;
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x0010F30F File Offset: 0x0010D50F
		internal CommandOrigin Origin
		{
			get
			{
				return this._origin;
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06003202 RID: 12802 RVA: 0x0010F317 File Offset: 0x0010D517
		internal ExecutionContext ExecutionContext
		{
			get
			{
				return this.executionContext;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x0010F31F File Offset: 0x0010D51F
		// (set) Token: 0x06003204 RID: 12804 RVA: 0x0010F327 File Offset: 0x0010D527
		internal CmdletProvider ProviderInstance
		{
			get
			{
				return this.providerInstance;
			}
			set
			{
				this.providerInstance = value;
			}
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x0010F330 File Offset: 0x0010D530
		private void CopyFilters(CmdletProviderContext context)
		{
			this._include = context.Include;
			this._exclude = context.Exclude;
			this._filter = context.Filter;
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x0010F356 File Offset: 0x0010D556
		internal void RemoveStopReferral()
		{
			if (this.copiedContext != null)
			{
				this.copiedContext.StopReferrals.Remove(this);
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06003207 RID: 12807 RVA: 0x0010F372 File Offset: 0x0010D572
		// (set) Token: 0x06003208 RID: 12808 RVA: 0x0010F37A File Offset: 0x0010D57A
		internal object DynamicParameters
		{
			get
			{
				return this.dynamicParameters;
			}
			set
			{
				this.dynamicParameters = value;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06003209 RID: 12809 RVA: 0x0010F383 File Offset: 0x0010D583
		internal InvocationInfo MyInvocation
		{
			get
			{
				if (this.command != null)
				{
					return this.command.MyInvocation;
				}
				return null;
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x0600320A RID: 12810 RVA: 0x0010F39A File Offset: 0x0010D59A
		// (set) Token: 0x0600320B RID: 12811 RVA: 0x0010F3A2 File Offset: 0x0010D5A2
		internal bool PassThru
		{
			get
			{
				return this.streamObjects;
			}
			set
			{
				this.streamObjects = value;
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x0010F3AB File Offset: 0x0010D5AB
		// (set) Token: 0x0600320D RID: 12813 RVA: 0x0010F3B3 File Offset: 0x0010D5B3
		internal PSDriveInfo Drive
		{
			get
			{
				return this.drive;
			}
			set
			{
				this.drive = value;
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x0600320E RID: 12814 RVA: 0x0010F3BC File Offset: 0x0010D5BC
		internal PSCredential Credential
		{
			get
			{
				PSCredential credential = this.credentials;
				if (this.credentials == null && this.drive != null)
				{
					credential = this.drive.Credential;
				}
				return credential;
			}
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x0600320F RID: 12815 RVA: 0x0010F3F4 File Offset: 0x0010D5F4
		internal bool UseTransaction
		{
			get
			{
				if (this.command != null && this.command.CommandRuntime != null)
				{
					MshCommandRuntime mshCommandRuntime = this.command.CommandRuntime as MshCommandRuntime;
					if (mshCommandRuntime != null)
					{
						return mshCommandRuntime.UseTransaction;
					}
				}
				return false;
			}
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x0010F437 File Offset: 0x0010D637
		public bool TransactionAvailable()
		{
			return this.command != null && this.command.TransactionAvailable();
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06003211 RID: 12817 RVA: 0x0010F44E File Offset: 0x0010D64E
		public PSTransactionContext CurrentPSTransaction
		{
			get
			{
				if (this.command != null)
				{
					return this.command.CurrentPSTransaction;
				}
				return null;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06003212 RID: 12818 RVA: 0x0010F465 File Offset: 0x0010D665
		// (set) Token: 0x06003213 RID: 12819 RVA: 0x0010F472 File Offset: 0x0010D672
		internal SwitchParameter Force
		{
			get
			{
				return this.force;
			}
			set
			{
				this.force = value;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06003214 RID: 12820 RVA: 0x0010F480 File Offset: 0x0010D680
		// (set) Token: 0x06003215 RID: 12821 RVA: 0x0010F488 File Offset: 0x0010D688
		internal string Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06003216 RID: 12822 RVA: 0x0010F491 File Offset: 0x0010D691
		internal Collection<string> Include
		{
			get
			{
				return this._include;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06003217 RID: 12823 RVA: 0x0010F499 File Offset: 0x0010D699
		internal Collection<string> Exclude
		{
			get
			{
				return this._exclude;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06003218 RID: 12824 RVA: 0x0010F4A1 File Offset: 0x0010D6A1
		// (set) Token: 0x06003219 RID: 12825 RVA: 0x0010F4A9 File Offset: 0x0010D6A9
		public bool SuppressWildcardExpansion
		{
			get
			{
				return this.suppressWildcardExpansion;
			}
			internal set
			{
				this.suppressWildcardExpansion = value;
			}
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x0010F4B4 File Offset: 0x0010D6B4
		internal bool ShouldProcess(string target)
		{
			bool flag = true;
			if (this.command != null)
			{
				flag = this.command.ShouldProcess(target);
			}
			CmdletProviderContext.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x0010F4FC File Offset: 0x0010D6FC
		internal bool ShouldProcess(string target, string action)
		{
			bool flag = true;
			if (this.command != null)
			{
				flag = this.command.ShouldProcess(target, action);
			}
			CmdletProviderContext.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x0010F544 File Offset: 0x0010D744
		internal bool ShouldProcess(string verboseDescription, string verboseWarning, string caption)
		{
			bool flag = true;
			if (this.command != null)
			{
				flag = this.command.ShouldProcess(verboseDescription, verboseWarning, caption);
			}
			CmdletProviderContext.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x0010F58C File Offset: 0x0010D78C
		internal bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
		{
			bool flag = true;
			if (this.command != null)
			{
				flag = this.command.ShouldProcess(verboseDescription, verboseWarning, caption, out shouldProcessReason);
			}
			else
			{
				shouldProcessReason = ShouldProcessReason.None;
			}
			CmdletProviderContext.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x0010F5DC File Offset: 0x0010D7DC
		internal bool ShouldContinue(string query, string caption)
		{
			bool flag = true;
			if (this.command != null)
			{
				flag = this.command.ShouldContinue(query, caption);
			}
			CmdletProviderContext.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x0010F624 File Offset: 0x0010D824
		internal bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll)
		{
			bool flag = true;
			if (this.command != null)
			{
				flag = this.command.ShouldContinue(query, caption, ref yesToAll, ref noToAll);
			}
			else
			{
				yesToAll = false;
				noToAll = false;
			}
			CmdletProviderContext.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x0010F676 File Offset: 0x0010D876
		internal void WriteVerbose(string text)
		{
			if (this.command != null)
			{
				this.command.WriteVerbose(text);
			}
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x0010F68C File Offset: 0x0010D88C
		internal void WriteWarning(string text)
		{
			if (this.command != null)
			{
				this.command.WriteWarning(text);
			}
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x0010F6A2 File Offset: 0x0010D8A2
		internal void WriteProgress(ProgressRecord record)
		{
			if (this.command != null)
			{
				this.command.WriteProgress(record);
			}
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x0010F6B8 File Offset: 0x0010D8B8
		internal void WriteDebug(string text)
		{
			if (this.command != null)
			{
				this.command.WriteDebug(text);
			}
		}

		// Token: 0x06003224 RID: 12836 RVA: 0x0010F6CE File Offset: 0x0010D8CE
		internal void WriteInformation(InformationRecord record)
		{
			if (this.command != null)
			{
				this.command.WriteInformation(record);
			}
		}

		// Token: 0x06003225 RID: 12837 RVA: 0x0010F6E4 File Offset: 0x0010D8E4
		internal void WriteInformation(object messageData, string[] tags)
		{
			if (this.command != null)
			{
				this.command.WriteInformation(messageData, tags);
			}
		}

		// Token: 0x06003226 RID: 12838 RVA: 0x0010F6FB File Offset: 0x0010D8FB
		internal void SetFilters(Collection<string> include, Collection<string> exclude, string filter)
		{
			this._include = include;
			this._exclude = exclude;
			this._filter = filter;
		}

		// Token: 0x06003227 RID: 12839 RVA: 0x0010F714 File Offset: 0x0010D914
		internal Collection<PSObject> GetAccumulatedObjects()
		{
			Collection<PSObject> result = this.accumulatedObjects;
			this.accumulatedObjects = new Collection<PSObject>();
			return result;
		}

		// Token: 0x06003228 RID: 12840 RVA: 0x0010F734 File Offset: 0x0010D934
		internal Collection<ErrorRecord> GetAccumulatedErrorObjects()
		{
			Collection<ErrorRecord> result = this.accumulatedErrorObjects;
			this.accumulatedErrorObjects = new Collection<ErrorRecord>();
			return result;
		}

		// Token: 0x06003229 RID: 12841 RVA: 0x0010F754 File Offset: 0x0010D954
		internal void ThrowFirstErrorOrDoNothing()
		{
			this.ThrowFirstErrorOrDoNothing(true);
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x0010F760 File Offset: 0x0010D960
		internal void ThrowFirstErrorOrDoNothing(bool wrapExceptionInProviderException)
		{
			if (this.HasErrors())
			{
				Collection<ErrorRecord> collection = this.GetAccumulatedErrorObjects();
				if (collection != null && collection.Count > 0)
				{
					if (wrapExceptionInProviderException)
					{
						ProviderInfo providerInfo = null;
						if (this.ProviderInstance != null)
						{
							providerInfo = this.ProviderInstance.ProviderInfo;
						}
						ProviderInvocationException ex = new ProviderInvocationException(providerInfo, collection[0]);
						MshLog.LogProviderHealthEvent(this.ExecutionContext, (providerInfo != null) ? providerInfo.Name : "unknown provider", ex, Severity.Warning);
						throw ex;
					}
					throw collection[0].Exception;
				}
			}
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x0010F7DC File Offset: 0x0010D9DC
		internal void WriteErrorsToContext(CmdletProviderContext errorContext)
		{
			if (errorContext == null)
			{
				throw PSTraceSource.NewArgumentNullException("errorContext");
			}
			if (this.HasErrors())
			{
				foreach (ErrorRecord errorRecord in this.GetAccumulatedErrorObjects())
				{
					errorContext.WriteError(errorRecord);
				}
			}
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x0010F840 File Offset: 0x0010DA40
		internal void WriteObject(object obj)
		{
			if (this.Stopping)
			{
				PipelineStoppedException ex = new PipelineStoppedException();
				throw ex;
			}
			if (!this.streamObjects)
			{
				CmdletProviderContext.tracer.WriteLine("Writing to accumulated objects", new object[0]);
				PSObject item = PSObject.AsPSObject(obj);
				this.accumulatedObjects.Add(item);
				return;
			}
			if (this.command != null)
			{
				CmdletProviderContext.tracer.WriteLine("Writing to command pipeline", new object[0]);
				this.command.WriteObject(obj);
				return;
			}
			InvalidOperationException ex2 = PSTraceSource.NewInvalidOperationException(SessionStateStrings.OutputStreamingNotEnabled, new object[0]);
			throw ex2;
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x0010F8CC File Offset: 0x0010DACC
		internal void WriteError(ErrorRecord errorRecord)
		{
			if (this.Stopping)
			{
				PipelineStoppedException ex = new PipelineStoppedException();
				throw ex;
			}
			if (!this.streamErrors)
			{
				this.accumulatedErrorObjects.Add(errorRecord);
				if (errorRecord.ErrorDetails != null && errorRecord.ErrorDetails.TextLookupError != null)
				{
					Exception textLookupError = errorRecord.ErrorDetails.TextLookupError;
					errorRecord.ErrorDetails.TextLookupError = null;
					MshLog.LogProviderHealthEvent(this.ExecutionContext, this.ProviderInstance.ProviderInfo.Name, textLookupError, Severity.Warning);
				}
				return;
			}
			if (this.command != null)
			{
				CmdletProviderContext.tracer.WriteLine("Writing error package to command error pipe", new object[0]);
				this.command.WriteError(errorRecord);
				return;
			}
			InvalidOperationException ex2 = PSTraceSource.NewInvalidOperationException(SessionStateStrings.ErrorStreamingNotEnabled, new object[0]);
			throw ex2;
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x0010F984 File Offset: 0x0010DB84
		internal bool HasErrors()
		{
			bool flag = false;
			if (this.accumulatedErrorObjects != null && this.accumulatedErrorObjects.Count > 0)
			{
				flag = true;
			}
			CmdletProviderContext.tracer.WriteLine("result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x0010F9CC File Offset: 0x0010DBCC
		internal void StopProcessing()
		{
			this.stopping = true;
			if (this.providerInstance != null)
			{
				this.providerInstance.StopProcessing();
			}
			foreach (CmdletProviderContext cmdletProviderContext in this.StopReferrals)
			{
				cmdletProviderContext.StopProcessing();
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x0010FA34 File Offset: 0x0010DC34
		internal bool Stopping
		{
			get
			{
				return this.stopping;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06003231 RID: 12849 RVA: 0x0010FA3C File Offset: 0x0010DC3C
		internal Collection<CmdletProviderContext> StopReferrals
		{
			get
			{
				return this.stopReferrals;
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06003232 RID: 12850 RVA: 0x0010FA44 File Offset: 0x0010DC44
		internal bool HasIncludeOrExclude
		{
			get
			{
				return (this.Include != null && this.Include.Count > 0) || (this.Exclude != null && this.Exclude.Count > 0);
			}
		}

		// Token: 0x04001A5D RID: 6749
		[TraceSource("CmdletProviderContext", "The context under which a core command is being run.")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("CmdletProviderContext", "The context under which a core command is being run.");

		// Token: 0x04001A5E RID: 6750
		private CmdletProviderContext copiedContext;

		// Token: 0x04001A5F RID: 6751
		private ExecutionContext executionContext;

		// Token: 0x04001A60 RID: 6752
		private PSCredential credentials = PSCredential.Empty;

		// Token: 0x04001A61 RID: 6753
		private PSDriveInfo drive;

		// Token: 0x04001A62 RID: 6754
		private bool force;

		// Token: 0x04001A63 RID: 6755
		private string _filter;

		// Token: 0x04001A64 RID: 6756
		private Collection<string> _include;

		// Token: 0x04001A65 RID: 6757
		private Collection<string> _exclude;

		// Token: 0x04001A66 RID: 6758
		private bool suppressWildcardExpansion;

		// Token: 0x04001A67 RID: 6759
		private Cmdlet command;

		// Token: 0x04001A68 RID: 6760
		private CommandOrigin _origin = CommandOrigin.Internal;

		// Token: 0x04001A69 RID: 6761
		private bool streamObjects;

		// Token: 0x04001A6A RID: 6762
		private bool streamErrors;

		// Token: 0x04001A6B RID: 6763
		private Collection<PSObject> accumulatedObjects = new Collection<PSObject>();

		// Token: 0x04001A6C RID: 6764
		private Collection<ErrorRecord> accumulatedErrorObjects = new Collection<ErrorRecord>();

		// Token: 0x04001A6D RID: 6765
		private CmdletProvider providerInstance;

		// Token: 0x04001A6E RID: 6766
		private object dynamicParameters;

		// Token: 0x04001A6F RID: 6767
		private bool stopping;

		// Token: 0x04001A70 RID: 6768
		private Collection<CmdletProviderContext> stopReferrals = new Collection<CmdletProviderContext>();
	}
}
