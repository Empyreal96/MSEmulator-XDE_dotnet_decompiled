using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004D2 RID: 1234
	public class OuterFormatShapeCommandBase : FrontEndCommandBase
	{
		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x060035E9 RID: 13801 RVA: 0x00124D3C File Offset: 0x00122F3C
		// (set) Token: 0x060035EA RID: 13802 RVA: 0x00124D44 File Offset: 0x00122F44
		[Parameter]
		public object GroupBy
		{
			get
			{
				return this.groupByParameter;
			}
			set
			{
				this.groupByParameter = value;
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x060035EB RID: 13803 RVA: 0x00124D4D File Offset: 0x00122F4D
		// (set) Token: 0x060035EC RID: 13804 RVA: 0x00124D55 File Offset: 0x00122F55
		[Parameter]
		public string View
		{
			get
			{
				return this.viewName;
			}
			set
			{
				this.viewName = value;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x060035ED RID: 13805 RVA: 0x00124D5E File Offset: 0x00122F5E
		// (set) Token: 0x060035EE RID: 13806 RVA: 0x00124D84 File Offset: 0x00122F84
		[Parameter]
		public SwitchParameter ShowError
		{
			get
			{
				if (this.showErrorsAsMessages != null)
				{
					return this.showErrorsAsMessages.Value;
				}
				return false;
			}
			set
			{
				this.showErrorsAsMessages = new bool?(value);
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x060035EF RID: 13807 RVA: 0x00124D97 File Offset: 0x00122F97
		// (set) Token: 0x060035F0 RID: 13808 RVA: 0x00124DBD File Offset: 0x00122FBD
		[Parameter]
		public SwitchParameter DisplayError
		{
			get
			{
				if (this.showErrorsInFormattedOutput != null)
				{
					return this.showErrorsInFormattedOutput.Value;
				}
				return false;
			}
			set
			{
				this.showErrorsInFormattedOutput = new bool?(value);
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x060035F1 RID: 13809 RVA: 0x00124DD0 File Offset: 0x00122FD0
		// (set) Token: 0x060035F2 RID: 13810 RVA: 0x00124DDD File Offset: 0x00122FDD
		[Parameter]
		public SwitchParameter Force
		{
			get
			{
				return this.forceFormattingAlsoOnOutOfBand;
			}
			set
			{
				this.forceFormattingAlsoOnOutOfBand = value;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x060035F3 RID: 13811 RVA: 0x00124DEB File Offset: 0x00122FEB
		// (set) Token: 0x060035F4 RID: 13812 RVA: 0x00124DF3 File Offset: 0x00122FF3
		[ValidateSet(new string[]
		{
			"CoreOnly",
			"EnumOnly",
			"Both"
		}, IgnoreCase = true)]
		[Parameter]
		public string Expand
		{
			get
			{
				return this.expansionString;
			}
			set
			{
				this.expansionString = value;
			}
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x00124DFC File Offset: 0x00122FFC
		internal EnumerableExpansion? ProcessExpandParameter()
		{
			EnumerableExpansion? result = null;
			if (string.IsNullOrEmpty(this.expansionString))
			{
				return result;
			}
			EnumerableExpansion value;
			if (!EnumerableExpansionConversion.Convert(this.expansionString, out value))
			{
				throw PSTraceSource.NewArgumentException("Expand", FormatAndOut_MshParameter.IllegalEnumerableExpansionValue, new object[0]);
			}
			result = new EnumerableExpansion?(value);
			return result;
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x00124E50 File Offset: 0x00123050
		internal MshParameter ProcessGroupByParameter()
		{
			if (this.groupByParameter != null)
			{
				TerminatingErrorContext invocationContext = new TerminatingErrorContext(this);
				ParameterProcessor parameterProcessor = new ParameterProcessor(new FormatGroupByParameterDefinition());
				List<MshParameter> list = parameterProcessor.ProcessParameters(new object[]
				{
					this.groupByParameter
				}, invocationContext);
				if (list.Count != 0)
				{
					return list[0];
				}
			}
			return null;
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x00124EA4 File Offset: 0x001230A4
		protected override void BeginProcessing()
		{
			InnerFormatShapeCommand innerFormatShapeCommand = (InnerFormatShapeCommand)this.implementation;
			FormattingCommandLineParameters commandLineParameters = this.GetCommandLineParameters();
			innerFormatShapeCommand.SetCommandLineParameters(commandLineParameters);
			base.BeginProcessing();
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x00124ED1 File Offset: 0x001230D1
		internal virtual FormattingCommandLineParameters GetCommandLineParameters()
		{
			return null;
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x00124ED4 File Offset: 0x001230D4
		internal void ReportCannotSpecifyViewAndProperty()
		{
			string message = StringUtil.Format(FormatAndOut_format_xxx.CannotSpecifyViewAndPropertyError, new object[0]);
			base.ThrowTerminatingError(new ErrorRecord(new InvalidDataException(), "FormatCannotSpecifyViewAndProperty", ErrorCategory.InvalidArgument, null)
			{
				ErrorDetails = new ErrorDetails(message)
			});
		}

		// Token: 0x04001B84 RID: 7044
		private object groupByParameter;

		// Token: 0x04001B85 RID: 7045
		private string viewName;

		// Token: 0x04001B86 RID: 7046
		internal bool? showErrorsAsMessages = null;

		// Token: 0x04001B87 RID: 7047
		internal bool? showErrorsInFormattedOutput = null;

		// Token: 0x04001B88 RID: 7048
		private bool forceFormattingAlsoOnOutOfBand;

		// Token: 0x04001B89 RID: 7049
		private string expansionString;

		// Token: 0x04001B8A RID: 7050
		internal EnumerableExpansion? expansion = null;
	}
}
