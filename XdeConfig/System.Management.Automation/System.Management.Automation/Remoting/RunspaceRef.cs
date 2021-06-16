using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Runspaces.Internal;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002AA RID: 682
	internal class RunspaceRef
	{
		// Token: 0x0600210B RID: 8459 RVA: 0x000BED54 File Offset: 0x000BCF54
		internal RunspaceRef(Runspace runspace)
		{
			this._runspaceRef = new ObjectRef<Runspace>(runspace);
			this._stopInvoke = false;
			this._localSyncObject = new object();
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x000BED7C File Offset: 0x000BCF7C
		internal void Revert()
		{
			this._runspaceRef.Revert();
			lock (this._localSyncObject)
			{
				this._stopInvoke = true;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x0600210D RID: 8461 RVA: 0x000BEDC8 File Offset: 0x000BCFC8
		internal Runspace Runspace
		{
			get
			{
				return this._runspaceRef.Value;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x000BEDD5 File Offset: 0x000BCFD5
		internal Runspace OldRunspace
		{
			get
			{
				return this._runspaceRef.OldValue;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x0600210F RID: 8463 RVA: 0x000BEDE2 File Offset: 0x000BCFE2
		internal bool IsRunspaceOverridden
		{
			get
			{
				return this._runspaceRef.IsOverridden;
			}
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000BEDF0 File Offset: 0x000BCFF0
		private PSCommand ParsePsCommandUsingScriptBlock(string line, bool? useLocalScope)
		{
			try
			{
				Runspace oldValue = this._runspaceRef.OldValue;
				ExecutionContext executionContext = oldValue.ExecutionContext;
				bool isTrustedInput = oldValue.ExecutionContext.LanguageMode == PSLanguageMode.FullLanguage;
				ScriptBlock scriptBlock = ScriptBlock.Create(executionContext, line);
				PowerShell powerShell = scriptBlock.GetPowerShell(executionContext, isTrustedInput, useLocalScope, null);
				return powerShell.Commands;
			}
			catch (ScriptBlockToPowerShellNotSupportedException e)
			{
				CommandProcessorBase.CheckForSevereException(e);
			}
			catch (RuntimeException e2)
			{
				CommandProcessorBase.CheckForSevereException(e2);
			}
			return null;
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000BEE74 File Offset: 0x000BD074
		internal PSCommand CreatePsCommand(string line, bool isScript, bool? useNewScope)
		{
			if (!this.IsRunspaceOverridden)
			{
				return this.CreatePsCommandNotOverriden(line, isScript, useNewScope);
			}
			PSCommand pscommand = this.ParsePsCommandUsingScriptBlock(line, useNewScope);
			if (pscommand == null)
			{
				return this.CreatePsCommandNotOverriden(line, isScript, useNewScope);
			}
			return pscommand;
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000BEEAC File Offset: 0x000BD0AC
		private PSCommand CreatePsCommandNotOverriden(string line, bool isScript, bool? useNewScope)
		{
			PSCommand pscommand = new PSCommand();
			if (isScript)
			{
				if (useNewScope != null)
				{
					pscommand.AddScript(line, useNewScope.Value);
				}
				else
				{
					pscommand.AddScript(line);
				}
			}
			else if (useNewScope != null)
			{
				pscommand.AddCommand(line, useNewScope.Value);
			}
			else
			{
				pscommand.AddCommand(line);
			}
			return pscommand;
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000BEFAC File Offset: 0x000BD1AC
		internal Pipeline CreatePipeline(string line, bool addToHistory, bool useNestedPipelines)
		{
			Pipeline pipeline = null;
			if (this.IsRunspaceOverridden)
			{
				if (this._runspaceRef.Value is RemoteRunspace && !string.IsNullOrEmpty(line) && string.Equals(line.Trim(), "exit", StringComparison.OrdinalIgnoreCase))
				{
					line = "Exit-PSSession";
				}
				PSCommand pscommand = this.ParsePsCommandUsingScriptBlock(line, null);
				if (pscommand != null)
				{
					pipeline = (useNestedPipelines ? this._runspaceRef.Value.CreateNestedPipeline(pscommand.Commands[0].CommandText, addToHistory) : this._runspaceRef.Value.CreatePipeline(pscommand.Commands[0].CommandText, addToHistory));
					pipeline.Commands.Clear();
					foreach (Command item in pscommand.Commands)
					{
						pipeline.Commands.Add(item);
					}
				}
			}
			if (pipeline == null)
			{
				pipeline = (useNestedPipelines ? this._runspaceRef.Value.CreateNestedPipeline(line, addToHistory) : this._runspaceRef.Value.CreatePipeline(line, addToHistory));
			}
			RemotePipeline remotePipeline = pipeline as RemotePipeline;
			if (this.IsRunspaceOverridden && remotePipeline != null)
			{
				PowerShell powerShell = remotePipeline.PowerShell;
				if (powerShell.RemotePowerShell != null)
				{
					powerShell.RemotePowerShell.RCConnectionNotification += this.HandleRCConnectionNotification;
				}
				powerShell.ErrorBuffer.DataAdded += delegate(object sender, DataAddedEventArgs eventArgs)
				{
					RemoteRunspace remoteRunspace = this._runspaceRef.Value as RemoteRunspace;
					PSDataCollection<ErrorRecord> psdataCollection = sender as PSDataCollection<ErrorRecord>;
					if (remoteRunspace != null && psdataCollection != null && remoteRunspace.RunspacePool.RemoteRunspacePoolInternal.Host != null)
					{
						Collection<ErrorRecord> collection = psdataCollection.ReadAll();
						foreach (ErrorRecord errorRecord in collection)
						{
							remoteRunspace.RunspacePool.RemoteRunspacePoolInternal.Host.UI.WriteErrorLine(errorRecord.ToString());
						}
					}
				};
			}
			pipeline.SetHistoryString(line);
			return pipeline;
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000BF144 File Offset: 0x000BD344
		internal Pipeline CreatePipeline()
		{
			return this._runspaceRef.Value.CreatePipeline();
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000BF156 File Offset: 0x000BD356
		internal Pipeline CreateNestedPipeline()
		{
			return this._runspaceRef.Value.CreateNestedPipeline();
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x000BF168 File Offset: 0x000BD368
		internal void Override(RemoteRunspace remoteRunspace)
		{
			bool flag = false;
			this.Override(remoteRunspace, null, out flag);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x000BF184 File Offset: 0x000BD384
		internal void Override(RemoteRunspace remoteRunspace, object syncObject, out bool isRunspacePushed)
		{
			lock (this._localSyncObject)
			{
				this._stopInvoke = false;
			}
			try
			{
				if (syncObject != null)
				{
					lock (syncObject)
					{
						this._runspaceRef.Override(remoteRunspace);
						isRunspacePushed = true;
						goto IL_63;
					}
				}
				this._runspaceRef.Override(remoteRunspace);
				isRunspacePushed = true;
				IL_63:
				if (remoteRunspace.GetCurrentlyRunningPipeline() == null)
				{
					using (PowerShell powerShell = PowerShell.Create())
					{
						powerShell.AddCommand("Get-Command");
						powerShell.AddParameter("Name", new string[]
						{
							"Out-Default",
							"Exit-PSSession"
						});
						powerShell.Runspace = this._runspaceRef.Value;
						bool flag3 = this._runspaceRef.Value.GetRemoteProtocolVersion() == RemotingConstants.ProtocolVersionWin7RC;
						powerShell.IsGetCommandMetadataSpecialPipeline = !flag3;
						int num = flag3 ? 2 : 3;
						powerShell.RemotePowerShell.HostCallReceived += this.HandleHostCall;
						IAsyncResult asyncResult = powerShell.BeginInvoke();
						PSDataCollection<PSObject> psdataCollection = new PSDataCollection<PSObject>();
						while (!this._stopInvoke)
						{
							asyncResult.AsyncWaitHandle.WaitOne(1000);
							if (asyncResult.IsCompleted)
							{
								psdataCollection = powerShell.EndInvoke(asyncResult);
								break;
							}
						}
						if (powerShell.Streams.Error.Count > 0 || psdataCollection.Count < num)
						{
							throw RemoteHostExceptions.NewRemoteRunspaceDoesNotSupportPushRunspaceException();
						}
					}
				}
			}
			catch (Exception)
			{
				this._runspaceRef.Revert();
				isRunspacePushed = false;
				throw;
			}
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000BF378 File Offset: 0x000BD578
		private void HandleHostCall(object sender, RemoteDataEventArgs<RemoteHostCall> eventArgs)
		{
			ClientRemotePowerShell.ExitHandler(sender, eventArgs);
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000BF384 File Offset: 0x000BD584
		private void HandleRCConnectionNotification(object sender, PSConnectionRetryStatusEventArgs e)
		{
			switch (e.Notification)
			{
			case PSConnectionRetryStatus.NetworkFailureDetected:
				this.StartProgressBar((long)sender.GetHashCode(), e.ComputerName, e.MaxRetryConnectionTime / 1000);
				return;
			case PSConnectionRetryStatus.ConnectionRetryAttempt:
				break;
			case PSConnectionRetryStatus.ConnectionRetrySucceeded:
			case PSConnectionRetryStatus.AutoDisconnectStarting:
				this.StopProgressBar((long)sender.GetHashCode());
				return;
			case PSConnectionRetryStatus.AutoDisconnectSucceeded:
			case PSConnectionRetryStatus.InternalErrorAbort:
				this.WriteRCFailedError();
				this.StopProgressBar((long)sender.GetHashCode());
				break;
			default:
				return;
			}
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000BF3FC File Offset: 0x000BD5FC
		private void WriteRCFailedError()
		{
			RemoteRunspace remoteRunspace = this._runspaceRef.Value as RemoteRunspace;
			if (remoteRunspace != null && remoteRunspace.RunspacePool.RemoteRunspacePoolInternal.Host != null)
			{
				remoteRunspace.RunspacePool.RemoteRunspacePoolInternal.Host.UI.WriteErrorLine(StringUtil.Format(RemotingErrorIdStrings.RCAutoDisconnectingError, remoteRunspace.ConnectionInfo.ComputerName));
			}
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000BF460 File Offset: 0x000BD660
		private void StartProgressBar(long sourceId, string computerName, int totalSeconds)
		{
			RemoteRunspace remoteRunspace = this._runspaceRef.Value as RemoteRunspace;
			if (remoteRunspace != null)
			{
				RunspaceRef.RCProgress.StartProgress(sourceId, computerName, totalSeconds, remoteRunspace.RunspacePool.RemoteRunspacePoolInternal.Host);
			}
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000BF49E File Offset: 0x000BD69E
		private void StopProgressBar(long sourceId)
		{
			RunspaceRef.RCProgress.StopProgress(sourceId);
		}

		// Token: 0x04000EA1 RID: 3745
		private ObjectRef<Runspace> _runspaceRef;

		// Token: 0x04000EA2 RID: 3746
		private bool _stopInvoke;

		// Token: 0x04000EA3 RID: 3747
		private object _localSyncObject;

		// Token: 0x04000EA4 RID: 3748
		private static RobustConnectionProgress RCProgress = new RobustConnectionProgress();
	}
}
