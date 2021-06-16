using System;
using System.Management.Automation;
using System.Management.Automation.Internal;
using Microsoft.PowerShell.Commands.Internal.Format;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200050E RID: 1294
	[Cmdlet("Out", "LineOutput")]
	public class OutLineOutputCommand : FrontEndCommandBase
	{
		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x060036D7 RID: 14039 RVA: 0x0012870E File Offset: 0x0012690E
		// (set) Token: 0x060036D8 RID: 14040 RVA: 0x00128716 File Offset: 0x00126916
		[Parameter(Mandatory = true, Position = 0)]
		public object LineOutput
		{
			get
			{
				return this.lineOutput;
			}
			set
			{
				this.lineOutput = value;
			}
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x0012871F File Offset: 0x0012691F
		public OutLineOutputCommand()
		{
			this.implementation = new OutCommandInner();
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x00128734 File Offset: 0x00126934
		protected override void BeginProcessing()
		{
			if (this.lineOutput == null)
			{
				this.ProcessNullLineOutput();
			}
			LineOutput lineOutput = this.lineOutput as LineOutput;
			if (lineOutput == null)
			{
				this.ProcessWrongTypeLineOutput(this.lineOutput);
			}
			((OutCommandInner)this.implementation).LineOutput = lineOutput;
			base.BeginProcessing();
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x00128784 File Offset: 0x00126984
		private void ProcessNullLineOutput()
		{
			string message = StringUtil.Format(FormatAndOut_out_xxx.OutLineOutput_NullLineOutputParameter, new object[0]);
			base.ThrowTerminatingError(new ErrorRecord(PSTraceSource.NewArgumentNullException("LineOutput"), "OutLineOutputNullLineOutputParameter", ErrorCategory.InvalidArgument, null)
			{
				ErrorDetails = new ErrorDetails(message)
			});
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x001287CC File Offset: 0x001269CC
		private void ProcessWrongTypeLineOutput(object obj)
		{
			string message = StringUtil.Format(FormatAndOut_out_xxx.OutLineOutput_InvalidLineOutputParameterType, obj.GetType().FullName, typeof(LineOutput).FullName);
			base.ThrowTerminatingError(new ErrorRecord(new InvalidCastException(), "OutLineOutputInvalidLineOutputParameterType", ErrorCategory.InvalidArgument, null)
			{
				ErrorDetails = new ErrorDetails(message)
			});
		}

		// Token: 0x04001C0E RID: 7182
		private object lineOutput;
	}
}
