using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Reflection;
using System.Resources;

namespace System.Management.Automation
{
	// Token: 0x0200000A RID: 10
	public abstract class Cmdlet : InternalCommand
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002E21 File Offset: 0x00001021
		public static HashSet<string> CommonParameters
		{
			get
			{
				return Cmdlet.commonParameters.Value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002E2D File Offset: 0x0000102D
		public static HashSet<string> OptionalCommonParameters
		{
			get
			{
				return Cmdlet.optionalCommonParameters.Value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002E3C File Offset: 0x0000103C
		public bool Stopping
		{
			get
			{
				bool isStopping;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					isStopping = base.IsStopping;
				}
				return isStopping;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002E74 File Offset: 0x00001074
		internal string _ParameterSetName
		{
			get
			{
				return this._parameterSetName;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002E7C File Offset: 0x0000107C
		internal void SetParameterSetName(string parameterSetName)
		{
			this._parameterSetName = parameterSetName;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002E88 File Offset: 0x00001088
		internal override void DoBeginProcessing()
		{
			MshCommandRuntime mshCommandRuntime = this.CommandRuntime as MshCommandRuntime;
			if (mshCommandRuntime != null && mshCommandRuntime.UseTransaction && !base.Context.TransactionManager.HasTransaction)
			{
				string message = TransactionStrings.NoTransactionStarted;
				if (base.Context.TransactionManager.IsLastTransactionCommitted)
				{
					message = TransactionStrings.NoTransactionStartedFromCommit;
				}
				else if (base.Context.TransactionManager.IsLastTransactionRolledBack)
				{
					message = TransactionStrings.NoTransactionStartedFromRollback;
				}
				throw new InvalidOperationException(message);
			}
			this.BeginProcessing();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002F08 File Offset: 0x00001108
		internal override void DoProcessRecord()
		{
			this.ProcessRecord();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002F10 File Offset: 0x00001110
		internal override void DoEndProcessing()
		{
			this.EndProcessing();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002F18 File Offset: 0x00001118
		internal override void DoStopProcessing()
		{
			this.StopProcessing();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002F34 File Offset: 0x00001134
		public virtual string GetResourceString(string baseName, string resourceId)
		{
			string result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (string.IsNullOrEmpty(baseName))
				{
					throw PSTraceSource.NewArgumentNullException("baseName");
				}
				if (string.IsNullOrEmpty(resourceId))
				{
					throw PSTraceSource.NewArgumentNullException("resourceId");
				}
				ResourceManager resourceManager = ResourceManagerCache.GetResourceManager(base.GetType().GetTypeInfo().Assembly, baseName);
				string text = null;
				try
				{
					text = resourceManager.GetString(resourceId, CultureInfo.CurrentUICulture);
				}
				catch (MissingManifestResourceException)
				{
					throw PSTraceSource.NewArgumentException("baseName", GetErrorText.ResourceBaseNameFailure, new object[]
					{
						baseName
					});
				}
				if (text == null)
				{
					throw PSTraceSource.NewArgumentException("resourceId", GetErrorText.ResourceIdFailure, new object[]
					{
						resourceId
					});
				}
				result = text;
			}
			return result;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00003004 File Offset: 0x00001204
		// (set) Token: 0x06000066 RID: 102 RVA: 0x0000303C File Offset: 0x0000123C
		public ICommandRuntime CommandRuntime
		{
			get
			{
				ICommandRuntime commandRuntime;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					commandRuntime = this.commandRuntime;
				}
				return commandRuntime;
			}
			set
			{
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					this.commandRuntime = value;
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003074 File Offset: 0x00001274
		public void WriteError(ErrorRecord errorRecord)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime == null)
				{
					throw new NotImplementedException("WriteError");
				}
				this.commandRuntime.WriteError(errorRecord);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000030C4 File Offset: 0x000012C4
		public void WriteObject(object sendToPipeline)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime == null)
				{
					throw new NotImplementedException("WriteObject");
				}
				this.commandRuntime.WriteObject(sendToPipeline);
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003114 File Offset: 0x00001314
		public void WriteObject(object sendToPipeline, bool enumerateCollection)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime == null)
				{
					throw new NotImplementedException("WriteObject");
				}
				this.commandRuntime.WriteObject(sendToPipeline, enumerateCollection);
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003168 File Offset: 0x00001368
		public void WriteVerbose(string text)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime == null)
				{
					throw new NotImplementedException("WriteVerbose");
				}
				this.commandRuntime.WriteVerbose(text);
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000031B8 File Offset: 0x000013B8
		public void WriteWarning(string text)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime == null)
				{
					throw new NotImplementedException("WriteWarning");
				}
				this.commandRuntime.WriteWarning(text);
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003208 File Offset: 0x00001408
		public void WriteCommandDetail(string text)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime == null)
				{
					throw new NotImplementedException("WriteCommandDetail");
				}
				this.commandRuntime.WriteCommandDetail(text);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003258 File Offset: 0x00001458
		public void WriteProgress(ProgressRecord progressRecord)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime == null)
				{
					throw new NotImplementedException("WriteProgress");
				}
				this.commandRuntime.WriteProgress(progressRecord);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000032A8 File Offset: 0x000014A8
		internal void WriteProgress(long sourceId, ProgressRecord progressRecord)
		{
			if (this.commandRuntime != null)
			{
				this.commandRuntime.WriteProgress(sourceId, progressRecord);
				return;
			}
			throw new NotImplementedException("WriteProgress");
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000032CC File Offset: 0x000014CC
		public void WriteDebug(string text)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime == null)
				{
					throw new NotImplementedException("WriteDebug");
				}
				this.commandRuntime.WriteDebug(text);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000331C File Offset: 0x0000151C
		public void WriteInformation(object messageData, string[] tags)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				ICommandRuntime2 commandRuntime = this.commandRuntime as ICommandRuntime2;
				if (commandRuntime == null)
				{
					throw new NotImplementedException("WriteInformation");
				}
				string text = base.MyInvocation.PSCommandPath;
				if (string.IsNullOrEmpty(text))
				{
					text = base.MyInvocation.MyCommand.Name;
				}
				InformationRecord informationRecord = new InformationRecord(messageData, text);
				if (tags != null)
				{
					informationRecord.Tags.AddRange(tags);
				}
				commandRuntime.WriteInformation(informationRecord);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000033AC File Offset: 0x000015AC
		public void WriteInformation(InformationRecord informationRecord)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				ICommandRuntime2 commandRuntime = this.commandRuntime as ICommandRuntime2;
				if (commandRuntime == null)
				{
					throw new NotImplementedException("WriteInformation");
				}
				commandRuntime.WriteInformation(informationRecord);
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003400 File Offset: 0x00001600
		public bool ShouldProcess(string target)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime != null)
				{
					result = this.commandRuntime.ShouldProcess(target);
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000344C File Offset: 0x0000164C
		public bool ShouldProcess(string target, string action)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime != null)
				{
					result = this.commandRuntime.ShouldProcess(target, action);
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003498 File Offset: 0x00001698
		public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime != null)
				{
					result = this.commandRuntime.ShouldProcess(verboseDescription, verboseWarning, caption);
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000034E4 File Offset: 0x000016E4
		public bool ShouldProcess(string verboseDescription, string verboseWarning, string caption, out ShouldProcessReason shouldProcessReason)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime != null)
				{
					result = this.commandRuntime.ShouldProcess(verboseDescription, verboseWarning, caption, out shouldProcessReason);
				}
				else
				{
					shouldProcessReason = ShouldProcessReason.None;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003538 File Offset: 0x00001738
		public bool ShouldContinue(string query, string caption)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime != null)
				{
					result = this.commandRuntime.ShouldContinue(query, caption);
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003584 File Offset: 0x00001784
		public bool ShouldContinue(string query, string caption, ref bool yesToAll, ref bool noToAll)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime != null)
				{
					result = this.commandRuntime.ShouldContinue(query, caption, ref yesToAll, ref noToAll);
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000035D4 File Offset: 0x000017D4
		public bool ShouldContinue(string query, string caption, bool hasSecurityImpact, ref bool yesToAll, ref bool noToAll)
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime != null)
				{
					ICommandRuntime2 commandRuntime = this.commandRuntime as ICommandRuntime2;
					if (commandRuntime != null)
					{
						result = commandRuntime.ShouldContinue(query, caption, hasSecurityImpact, ref yesToAll, ref noToAll);
					}
					else
					{
						result = this.commandRuntime.ShouldContinue(query, caption, ref yesToAll, ref noToAll);
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003644 File Offset: 0x00001844
		internal List<object> GetResults()
		{
			if (this is PSCmdlet)
			{
				string cannotInvokePSCmdletsDirectly = CommandBaseStrings.CannotInvokePSCmdletsDirectly;
				throw new InvalidOperationException(cannotInvokePSCmdletsDirectly);
			}
			List<object> list = new List<object>();
			if (this.commandRuntime == null)
			{
				this.CommandRuntime = new DefaultCommandRuntime(list);
			}
			this.BeginProcessing();
			this.ProcessRecord();
			this.EndProcessing();
			return list;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000383C File Offset: 0x00001A3C
		public IEnumerable Invoke()
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				List<object> data = this.GetResults();
				for (int i = 0; i < data.Count; i++)
				{
					yield return data[i];
				}
			}
			yield break;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003A0C File Offset: 0x00001C0C
		public IEnumerable<T> Invoke<T>()
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				List<object> data = this.GetResults();
				for (int i = 0; i < data.Count; i++)
				{
					yield return (T)((object)data[i]);
				}
			}
			yield break;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003A2C File Offset: 0x00001C2C
		public bool TransactionAvailable()
		{
			bool result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (this.commandRuntime == null)
				{
					throw new NotImplementedException("TransactionAvailable");
				}
				result = this.commandRuntime.TransactionAvailable();
			}
			return result;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003A7C File Offset: 0x00001C7C
		public PSTransactionContext CurrentPSTransaction
		{
			get
			{
				if (this.commandRuntime != null)
				{
					return this.commandRuntime.CurrentPSTransaction;
				}
				throw new NotImplementedException("CurrentPSTransaction");
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003A9C File Offset: 0x00001C9C
		public void ThrowTerminatingError(ErrorRecord errorRecord)
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (errorRecord == null)
				{
					throw new ArgumentNullException("errorRecord");
				}
				if (this.commandRuntime != null)
				{
					this.commandRuntime.ThrowTerminatingError(errorRecord);
				}
				else
				{
					if (errorRecord.Exception != null)
					{
						throw errorRecord.Exception;
					}
					throw new InvalidOperationException(errorRecord.ToString());
				}
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003B0C File Offset: 0x00001D0C
		protected virtual void BeginProcessing()
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003B3C File Offset: 0x00001D3C
		protected virtual void ProcessRecord()
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003B6C File Offset: 0x00001D6C
		protected virtual void EndProcessing()
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003B9C File Offset: 0x00001D9C
		protected virtual void StopProcessing()
		{
			using (PSTransactionManager.GetEngineProtectionScope())
			{
			}
		}

		// Token: 0x04000021 RID: 33
		private static Lazy<HashSet<string>> commonParameters = new Lazy<HashSet<string>>(() => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"Verbose",
			"Debug",
			"ErrorAction",
			"WarningAction",
			"InformationAction",
			"ErrorVariable",
			"WarningVariable",
			"OutVariable",
			"OutBuffer",
			"PipelineVariable",
			"InformationVariable"
		});

		// Token: 0x04000022 RID: 34
		private static Lazy<HashSet<string>> optionalCommonParameters = new Lazy<HashSet<string>>(() => new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"WhatIf",
			"Confirm",
			"UseTransaction"
		});

		// Token: 0x04000023 RID: 35
		private string _parameterSetName = "";
	}
}
