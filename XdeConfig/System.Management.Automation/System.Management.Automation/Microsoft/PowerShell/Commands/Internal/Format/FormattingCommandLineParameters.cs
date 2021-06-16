using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004B5 RID: 1205
	internal sealed class FormattingCommandLineParameters
	{
		// Token: 0x04001B4E RID: 6990
		internal List<MshParameter> mshParameterList = new List<MshParameter>();

		// Token: 0x04001B4F RID: 6991
		internal MshParameter groupByParameter;

		// Token: 0x04001B50 RID: 6992
		internal string viewName;

		// Token: 0x04001B51 RID: 6993
		internal bool forceFormattingAlsoOnOutOfBand;

		// Token: 0x04001B52 RID: 6994
		internal bool? autosize = null;

		// Token: 0x04001B53 RID: 6995
		internal bool? showErrorsAsMessages = null;

		// Token: 0x04001B54 RID: 6996
		internal bool? showErrorsInFormattedOutput = null;

		// Token: 0x04001B55 RID: 6997
		internal EnumerableExpansion? expansion = null;

		// Token: 0x04001B56 RID: 6998
		internal ShapeSpecificParameters shapeParameters;
	}
}
