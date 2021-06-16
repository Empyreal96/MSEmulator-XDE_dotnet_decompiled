using System;
using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Host;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000532 RID: 1330
	[Cmdlet("Out", "Default", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113362", RemotingCapability = RemotingCapability.None)]
	public class OutDefaultCommand : FrontEndCommandBase
	{
		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06003764 RID: 14180 RVA: 0x0012A327 File Offset: 0x00128527
		// (set) Token: 0x06003765 RID: 14181 RVA: 0x0012A32F File Offset: 0x0012852F
		[Parameter]
		public SwitchParameter Transcript { get; set; }

		// Token: 0x06003766 RID: 14182 RVA: 0x0012A338 File Offset: 0x00128538
		public OutDefaultCommand()
		{
			this.implementation = new OutputManagerInner();
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x0012A34C File Offset: 0x0012854C
		protected override void BeginProcessing()
		{
			PSHostUserInterface ui = base.Host.UI;
			ConsoleLineOutput lineOutput = new ConsoleLineOutput(ui, false, new TerminatingErrorContext(this));
			((OutputManagerInner)this.implementation).LineOutput = lineOutput;
			MshCommandRuntime mshCommandRuntime = base.CommandRuntime as MshCommandRuntime;
			if (mshCommandRuntime != null)
			{
				mshCommandRuntime.MergeUnclaimedPreviousErrorResults = true;
			}
			this.savedTranscribeOnly = base.Host.UI.TranscribeOnly;
			if (this.Transcript)
			{
				base.Host.UI.TranscribeOnly = true;
			}
			base.BeginProcessing();
			if (base.Context.CurrentCommandProcessor.CommandRuntime.OutVarList != null)
			{
				this.outVarResults = new ArrayList();
			}
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x0012A3F8 File Offset: 0x001285F8
		protected override void ProcessRecord()
		{
			if (this.Transcript)
			{
				base.WriteObject(base.InputObject);
			}
			if (this.outVarResults != null)
			{
				object obj = PSObject.Base(base.InputObject);
				if (obj != null && !(obj is ErrorRecord) && !obj.GetType().FullName.StartsWith("Microsoft.PowerShell.Commands.Internal.Format", StringComparison.OrdinalIgnoreCase))
				{
					this.outVarResults.Add(base.InputObject);
				}
			}
			base.ProcessRecord();
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x0012A470 File Offset: 0x00128670
		protected override void EndProcessing()
		{
			if (this.outVarResults != null && this.outVarResults.Count > 0)
			{
				base.Context.CurrentCommandProcessor.CommandRuntime.OutVarList.Clear();
				foreach (object value in this.outVarResults)
				{
					base.Context.CurrentCommandProcessor.CommandRuntime.OutVarList.Add(value);
				}
				this.outVarResults = null;
			}
			base.EndProcessing();
			if (this.Transcript)
			{
				base.Host.UI.TranscribeOnly = this.savedTranscribeOnly;
			}
		}

		// Token: 0x04001C63 RID: 7267
		private ArrayList outVarResults;

		// Token: 0x04001C64 RID: 7268
		private bool savedTranscribeOnly;
	}
}
