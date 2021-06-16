using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004CC RID: 1228
	internal class InnerFormatShapeCommandBase : ImplementationCommandBase
	{
		// Token: 0x060035C3 RID: 13763 RVA: 0x00124472 File Offset: 0x00122672
		internal InnerFormatShapeCommandBase()
		{
			this.contextManager.Push(new InnerFormatShapeCommandBase.FormattingContext(InnerFormatShapeCommandBase.FormattingContext.State.none));
		}

		// Token: 0x04001B70 RID: 7024
		protected Stack<InnerFormatShapeCommandBase.FormattingContext> contextManager = new Stack<InnerFormatShapeCommandBase.FormattingContext>();

		// Token: 0x020004CD RID: 1229
		protected class FormattingContext
		{
			// Token: 0x060035C4 RID: 13764 RVA: 0x00124496 File Offset: 0x00122696
			internal FormattingContext(InnerFormatShapeCommandBase.FormattingContext.State s)
			{
				this.state = s;
			}

			// Token: 0x04001B71 RID: 7025
			internal InnerFormatShapeCommandBase.FormattingContext.State state;

			// Token: 0x020004CE RID: 1230
			internal enum State
			{
				// Token: 0x04001B73 RID: 7027
				none,
				// Token: 0x04001B74 RID: 7028
				document,
				// Token: 0x04001B75 RID: 7029
				group
			}
		}
	}
}
