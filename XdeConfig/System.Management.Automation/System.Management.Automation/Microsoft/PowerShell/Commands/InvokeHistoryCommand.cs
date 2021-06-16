using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000201 RID: 513
	[Cmdlet("Invoke", "History", SupportsShouldProcess = true, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113344")]
	public class InvokeHistoryCommand : PSCmdlet
	{
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x000933E7 File Offset: 0x000915E7
		// (set) Token: 0x060017D3 RID: 6099 RVA: 0x000933EF File Offset: 0x000915EF
		[Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
		public string Id
		{
			get
			{
				return this._id;
			}
			set
			{
				if (this._id != null)
				{
					this._multipleIdProvided = true;
				}
				this._id = value;
			}
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00093504 File Offset: 0x00091704
		protected override void EndProcessing()
		{
			if (this._multipleIdProvided)
			{
				Exception exception = new ArgumentException(StringUtil.Format(HistoryStrings.InvokeHistoryMultipleCommandsError, new object[0]));
				base.ThrowTerminatingError(new ErrorRecord(exception, "InvokeHistoryMultipleCommandsError", ErrorCategory.InvalidArgument, null));
			}
			History history = ((LocalRunspace)base.Context.CurrentRunspace).History;
			HistoryInfo historyEntryToInvoke = this.GetHistoryEntryToInvoke(history);
			LocalPipeline localPipeline = (LocalPipeline)((LocalRunspace)base.Context.CurrentRunspace).GetCurrentlyRunningPipeline();
			if (!localPipeline.PresentInInvokeHistoryEntryList(historyEntryToInvoke))
			{
				localPipeline.AddToInvokeHistoryEntryList(historyEntryToInvoke);
			}
			else
			{
				Exception exception2 = new InvalidOperationException(StringUtil.Format(HistoryStrings.InvokeHistoryLoopDetected, new object[0]));
				base.ThrowTerminatingError(new ErrorRecord(exception2, "InvokeHistoryLoopDetected", ErrorCategory.InvalidOperation, null));
			}
			this.ReplaceHistoryString(historyEntryToInvoke);
			string commandLine = historyEntryToInvoke.CommandLine;
			if (!base.ShouldProcess(commandLine))
			{
				return;
			}
			try
			{
				base.Host.UI.WriteLine(commandLine);
			}
			catch (HostException)
			{
			}
			using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
			{
				powerShell.AddScript(commandLine);
				EventHandler<DataAddedEventArgs> value = delegate(object sender, DataAddedEventArgs e)
				{
					DebugRecord debugRecord = ((PSDataCollection<DebugRecord>)sender)[e.Index];
					base.WriteDebug(debugRecord.Message);
				};
				EventHandler<DataAddedEventArgs> value2 = delegate(object sender, DataAddedEventArgs e)
				{
					ErrorRecord errorRecord = ((PSDataCollection<ErrorRecord>)sender)[e.Index];
					base.WriteError(errorRecord);
				};
				EventHandler<DataAddedEventArgs> value3 = delegate(object sender, DataAddedEventArgs e)
				{
					InformationRecord informationRecord = ((PSDataCollection<InformationRecord>)sender)[e.Index];
					base.WriteInformation(informationRecord);
				};
				EventHandler<DataAddedEventArgs> value4 = delegate(object sender, DataAddedEventArgs e)
				{
					ProgressRecord progressRecord = ((PSDataCollection<ProgressRecord>)sender)[e.Index];
					base.WriteProgress(progressRecord);
				};
				EventHandler<DataAddedEventArgs> value5 = delegate(object sender, DataAddedEventArgs e)
				{
					VerboseRecord verboseRecord = ((PSDataCollection<VerboseRecord>)sender)[e.Index];
					base.WriteVerbose(verboseRecord.Message);
				};
				EventHandler<DataAddedEventArgs> value6 = delegate(object sender, DataAddedEventArgs e)
				{
					WarningRecord warningRecord = ((PSDataCollection<WarningRecord>)sender)[e.Index];
					base.WriteWarning(warningRecord.Message);
				};
				powerShell.Streams.Debug.DataAdded += value;
				powerShell.Streams.Error.DataAdded += value2;
				powerShell.Streams.Information.DataAdded += value3;
				powerShell.Streams.Progress.DataAdded += value4;
				powerShell.Streams.Verbose.DataAdded += value5;
				powerShell.Streams.Warning.DataAdded += value6;
				try
				{
					Collection<PSObject> collection = powerShell.Invoke();
					if (collection.Count > 0)
					{
						base.WriteObject(collection, true);
					}
					localPipeline.RemoveFromInvokeHistoryEntryList(historyEntryToInvoke);
				}
				finally
				{
					powerShell.Streams.Debug.DataAdded -= value;
					powerShell.Streams.Error.DataAdded -= value2;
					powerShell.Streams.Information.DataAdded -= value3;
					powerShell.Streams.Progress.DataAdded -= value4;
					powerShell.Streams.Verbose.DataAdded -= value5;
					powerShell.Streams.Warning.DataAdded -= value6;
				}
			}
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x000937F8 File Offset: 0x000919F8
		private HistoryInfo GetHistoryEntryToInvoke(History history)
		{
			HistoryInfo historyInfo = null;
			if (this._id == null)
			{
				HistoryInfo[] entries = history.GetEntries(0L, 1L, true);
				if (entries.Length == 1)
				{
					historyInfo = entries[0];
				}
				else
				{
					Exception exception = new InvalidOperationException(StringUtil.Format(HistoryStrings.NoLastHistoryEntryFound, new object[0]));
					base.ThrowTerminatingError(new ErrorRecord(exception, "InvokeHistoryNoLastHistoryEntryFound", ErrorCategory.InvalidOperation, null));
				}
			}
			else
			{
				this.PopulateIdAndCommandLine();
				if (this._commandLine != null)
				{
					HistoryInfo[] entries2 = history.GetEntries(0L, -1L, false);
					for (int i = entries2.Length - 1; i >= 0; i--)
					{
						if (entries2[i].CommandLine.StartsWith(this._commandLine, StringComparison.CurrentCulture))
						{
							historyInfo = entries2[i];
							break;
						}
					}
					if (historyInfo == null)
					{
						Exception exception2 = new ArgumentException(StringUtil.Format(HistoryStrings.NoHistoryForCommandline, this._commandLine));
						base.ThrowTerminatingError(new ErrorRecord(exception2, "InvokeHistoryNoHistoryForCommandline", ErrorCategory.ObjectNotFound, this._commandLine));
					}
				}
				else if (this._historyId <= 0L)
				{
					Exception exception3 = new ArgumentOutOfRangeException("Id", StringUtil.Format(HistoryStrings.InvalidIdGetHistory, this._historyId));
					base.ThrowTerminatingError(new ErrorRecord(exception3, "InvokeHistoryInvalidIdGetHistory", ErrorCategory.InvalidArgument, this._historyId));
				}
				else
				{
					historyInfo = history.GetEntry(this._historyId);
					if (historyInfo == null || historyInfo.Id != this._historyId)
					{
						Exception exception4 = new ArgumentException(StringUtil.Format(HistoryStrings.NoHistoryForId, this._historyId));
						base.ThrowTerminatingError(new ErrorRecord(exception4, "InvokeHistoryNoHistoryForId", ErrorCategory.ObjectNotFound, this._historyId));
					}
				}
			}
			return historyInfo;
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00093990 File Offset: 0x00091B90
		private void PopulateIdAndCommandLine()
		{
			if (this._id == null)
			{
				return;
			}
			try
			{
				this._historyId = (long)LanguagePrimitives.ConvertTo(this._id, typeof(long), CultureInfo.InvariantCulture);
			}
			catch (PSInvalidCastException)
			{
				this._commandLine = this._id;
			}
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x000939EC File Offset: 0x00091BEC
		private void ReplaceHistoryString(HistoryInfo entry)
		{
			LocalPipeline localPipeline = (LocalPipeline)((LocalRunspace)base.Context.CurrentRunspace).GetCurrentlyRunningPipeline();
			if (localPipeline.AddToHistory)
			{
				localPipeline.HistoryString = entry.CommandLine;
			}
		}

		// Token: 0x04000A0E RID: 2574
		private bool _multipleIdProvided;

		// Token: 0x04000A0F RID: 2575
		private string _id;

		// Token: 0x04000A10 RID: 2576
		private long _historyId = -1L;

		// Token: 0x04000A11 RID: 2577
		private string _commandLine;
	}
}
