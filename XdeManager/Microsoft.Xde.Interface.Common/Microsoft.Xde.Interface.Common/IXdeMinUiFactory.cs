using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000010 RID: 16
	[ComVisible(false)]
	public interface IXdeMinUiFactory : IDisposable
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600005A RID: 90
		bool SilentMode { get; }

		// Token: 0x0600005B RID: 91
		void ShowErrorMessageForException(string messageFormat, Exception exception, string telemetryIdentifier);

		// Token: 0x0600005C RID: 92
		void ShowErrorMessage(string message);

		// Token: 0x0600005D RID: 93
		void ShowErrorMessage(string instruction, string message);

		// Token: 0x0600005E RID: 94
		void ShowErrorMessageFormat(string instruction, string message, params string[] items);

		// Token: 0x0600005F RID: 95
		TaskDialogResult ShowTaskDialog(string instruction, string text, TaskDialogStandardButtons buttons, TaskDialogStandardIcon icon);

		// Token: 0x06000060 RID: 96
		TaskDialogResult ShowTaskDialog(string instruction, string text, TaskDialogStandardButtons buttons, TaskDialogResult resultWhenSilent, TaskDialogStandardIcon icon);

		// Token: 0x06000061 RID: 97
		string ShowTaskDialogWithInput(string prompt, string title, string defaultResponse);

		// Token: 0x06000062 RID: 98
		void DisplayWebPage(string url, string arguments = null);

		// Token: 0x06000063 RID: 99
		bool ShowElevationDialog(string instruction, string text, bool enableRestart);
	}
}
