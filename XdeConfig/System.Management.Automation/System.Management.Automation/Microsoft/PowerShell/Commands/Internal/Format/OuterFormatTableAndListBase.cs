using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004D3 RID: 1235
	public class OuterFormatTableAndListBase : OuterFormatShapeCommandBase
	{
		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x060035FB RID: 13819 RVA: 0x00124F43 File Offset: 0x00123143
		// (set) Token: 0x060035FC RID: 13820 RVA: 0x00124F4B File Offset: 0x0012314B
		[Parameter(Position = 0)]
		public object[] Property
		{
			get
			{
				return this.props;
			}
			set
			{
				this.props = value;
			}
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x00124F54 File Offset: 0x00123154
		internal override FormattingCommandLineParameters GetCommandLineParameters()
		{
			FormattingCommandLineParameters formattingCommandLineParameters = new FormattingCommandLineParameters();
			this.GetCommandLineProperties(formattingCommandLineParameters, false);
			formattingCommandLineParameters.groupByParameter = base.ProcessGroupByParameter();
			formattingCommandLineParameters.forceFormattingAlsoOnOutOfBand = base.Force;
			if (this.showErrorsAsMessages != null)
			{
				formattingCommandLineParameters.showErrorsAsMessages = this.showErrorsAsMessages;
			}
			if (this.showErrorsInFormattedOutput != null)
			{
				formattingCommandLineParameters.showErrorsInFormattedOutput = this.showErrorsInFormattedOutput;
			}
			formattingCommandLineParameters.expansion = base.ProcessExpandParameter();
			return formattingCommandLineParameters;
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x00124FCC File Offset: 0x001231CC
		internal void GetCommandLineProperties(FormattingCommandLineParameters parameters, bool isTable)
		{
			if (this.props != null)
			{
				CommandParameterDefinition p;
				if (isTable)
				{
					p = new FormatTableParameterDefinition();
				}
				else
				{
					p = new FormatListParameterDefinition();
				}
				ParameterProcessor parameterProcessor = new ParameterProcessor(p);
				TerminatingErrorContext invocationContext = new TerminatingErrorContext(this);
				parameters.mshParameterList = parameterProcessor.ProcessParameters(this.props, invocationContext);
			}
			if (!string.IsNullOrEmpty(base.View))
			{
				if (parameters.mshParameterList.Count != 0)
				{
					base.ReportCannotSpecifyViewAndProperty();
				}
				parameters.viewName = base.View;
			}
		}

		// Token: 0x04001B8B RID: 7051
		private object[] props;
	}
}
