using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000495 RID: 1173
	[Cmdlet("Set", "PSDebug", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113398")]
	public sealed class SetPSDebugCommand : PSCmdlet
	{
		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x060034B5 RID: 13493 RVA: 0x0011EA03 File Offset: 0x0011CC03
		// (set) Token: 0x060034B4 RID: 13492 RVA: 0x0011E9FA File Offset: 0x0011CBFA
		[Parameter(ParameterSetName = "on")]
		[ValidateRange(0, 2)]
		public int Trace
		{
			get
			{
				return this.trace;
			}
			set
			{
				this.trace = value;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x060034B7 RID: 13495 RVA: 0x0011EA1E File Offset: 0x0011CC1E
		// (set) Token: 0x060034B6 RID: 13494 RVA: 0x0011EA0B File Offset: 0x0011CC0B
		[Parameter(ParameterSetName = "on")]
		public SwitchParameter Step
		{
			get
			{
				return this.step.Value;
			}
			set
			{
				this.step = new bool?(value);
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x060034B9 RID: 13497 RVA: 0x0011EA43 File Offset: 0x0011CC43
		// (set) Token: 0x060034B8 RID: 13496 RVA: 0x0011EA30 File Offset: 0x0011CC30
		[Parameter(ParameterSetName = "on")]
		public SwitchParameter Strict
		{
			get
			{
				return this.strict.Value;
			}
			set
			{
				this.strict = new bool?(value);
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x060034BA RID: 13498 RVA: 0x0011EA55 File Offset: 0x0011CC55
		// (set) Token: 0x060034BB RID: 13499 RVA: 0x0011EA62 File Offset: 0x0011CC62
		[Parameter(ParameterSetName = "off")]
		public SwitchParameter Off
		{
			get
			{
				return this.off;
			}
			set
			{
				this.off = value;
			}
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x0011EA70 File Offset: 0x0011CC70
		protected override void BeginProcessing()
		{
			if (this.off)
			{
				base.Context.Debugger.DisableTracing();
				base.Context.EngineSessionState.GlobalScope.StrictModeVersion = null;
				return;
			}
			if (this.trace >= 0 || this.step != null)
			{
				base.Context.Debugger.EnableTracing(this.trace, this.step);
			}
			if (this.strict != null)
			{
				base.Context.EngineSessionState.GlobalScope.StrictModeVersion = new Version(this.strict.Value ? 1 : 0, 0);
			}
		}

		// Token: 0x04001AF4 RID: 6900
		private int trace = -1;

		// Token: 0x04001AF5 RID: 6901
		private bool? step;

		// Token: 0x04001AF6 RID: 6902
		private bool? strict;

		// Token: 0x04001AF7 RID: 6903
		private bool off;
	}
}
