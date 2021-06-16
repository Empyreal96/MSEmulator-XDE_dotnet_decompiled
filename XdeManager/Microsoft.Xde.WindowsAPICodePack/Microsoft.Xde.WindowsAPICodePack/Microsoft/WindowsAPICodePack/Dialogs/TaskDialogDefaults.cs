using System;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x0200002B RID: 43
	internal static class TaskDialogDefaults
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00005901 File Offset: 0x00003B01
		public static string Caption
		{
			get
			{
				return LocalizedMessages.TaskDialogDefaultCaption;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00005908 File Offset: 0x00003B08
		public static string MainInstruction
		{
			get
			{
				return LocalizedMessages.TaskDialogDefaultMainInstruction;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000590F File Offset: 0x00003B0F
		public static string Content
		{
			get
			{
				return LocalizedMessages.TaskDialogDefaultContent;
			}
		}

		// Token: 0x04000170 RID: 368
		public const int ProgressBarMinimumValue = 0;

		// Token: 0x04000171 RID: 369
		public const int ProgressBarMaximumValue = 100;

		// Token: 0x04000172 RID: 370
		public const int IdealWidth = 0;

		// Token: 0x04000173 RID: 371
		public const int MinimumDialogControlId = 9;
	}
}
