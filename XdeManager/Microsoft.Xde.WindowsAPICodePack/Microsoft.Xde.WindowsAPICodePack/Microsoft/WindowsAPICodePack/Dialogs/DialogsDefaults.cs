using System;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x0200001E RID: 30
	internal static class DialogsDefaults
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00003AE2 File Offset: 0x00001CE2
		internal static string Caption
		{
			get
			{
				return LocalizedMessages.DialogDefaultCaption;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00003AE9 File Offset: 0x00001CE9
		internal static string MainInstruction
		{
			get
			{
				return LocalizedMessages.DialogDefaultMainInstruction;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00003AF0 File Offset: 0x00001CF0
		internal static string Content
		{
			get
			{
				return LocalizedMessages.DialogDefaultContent;
			}
		}

		// Token: 0x04000124 RID: 292
		internal const int ProgressBarStartingValue = 0;

		// Token: 0x04000125 RID: 293
		internal const int ProgressBarMinimumValue = 0;

		// Token: 0x04000126 RID: 294
		internal const int ProgressBarMaximumValue = 100;

		// Token: 0x04000127 RID: 295
		internal const int IdealWidth = 0;

		// Token: 0x04000128 RID: 296
		internal const int MinimumDialogControlId = 9;
	}
}
