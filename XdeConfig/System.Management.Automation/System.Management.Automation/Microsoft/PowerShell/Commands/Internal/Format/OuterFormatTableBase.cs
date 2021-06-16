using System;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004D4 RID: 1236
	public class OuterFormatTableBase : OuterFormatTableAndListBase
	{
		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06003600 RID: 13824 RVA: 0x00125047 File Offset: 0x00123247
		// (set) Token: 0x06003601 RID: 13825 RVA: 0x0012506D File Offset: 0x0012326D
		[Parameter]
		public SwitchParameter AutoSize
		{
			get
			{
				if (this.autosize != null)
				{
					return this.autosize.Value;
				}
				return false;
			}
			set
			{
				this.autosize = new bool?(value);
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06003602 RID: 13826 RVA: 0x00125080 File Offset: 0x00123280
		// (set) Token: 0x06003603 RID: 13827 RVA: 0x001250A6 File Offset: 0x001232A6
		[Parameter]
		public SwitchParameter HideTableHeaders
		{
			get
			{
				if (this.hideHeaders != null)
				{
					return this.hideHeaders.Value;
				}
				return false;
			}
			set
			{
				this.hideHeaders = new bool?(value);
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06003604 RID: 13828 RVA: 0x001250B9 File Offset: 0x001232B9
		// (set) Token: 0x06003605 RID: 13829 RVA: 0x001250DF File Offset: 0x001232DF
		[Parameter]
		public SwitchParameter Wrap
		{
			get
			{
				if (this.multiLine != null)
				{
					return this.multiLine.Value;
				}
				return false;
			}
			set
			{
				this.multiLine = new bool?(value);
			}
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x001250F4 File Offset: 0x001232F4
		internal override FormattingCommandLineParameters GetCommandLineParameters()
		{
			FormattingCommandLineParameters formattingCommandLineParameters = new FormattingCommandLineParameters();
			base.GetCommandLineProperties(formattingCommandLineParameters, true);
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
			if (this.autosize != null)
			{
				formattingCommandLineParameters.autosize = new bool?(this.autosize.Value);
			}
			formattingCommandLineParameters.groupByParameter = base.ProcessGroupByParameter();
			TableSpecificParameters tableSpecificParameters = new TableSpecificParameters();
			formattingCommandLineParameters.shapeParameters = tableSpecificParameters;
			if (this.hideHeaders != null)
			{
				tableSpecificParameters.hideHeaders = new bool?(this.hideHeaders.Value);
			}
			if (this.multiLine != null)
			{
				tableSpecificParameters.multiLine = new bool?(this.multiLine.Value);
			}
			return formattingCommandLineParameters;
		}

		// Token: 0x04001B8C RID: 7052
		private bool? autosize = null;

		// Token: 0x04001B8D RID: 7053
		private bool? hideHeaders = null;

		// Token: 0x04001B8E RID: 7054
		private bool? multiLine = null;
	}
}
