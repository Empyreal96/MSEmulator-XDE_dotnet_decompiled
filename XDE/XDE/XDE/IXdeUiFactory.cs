using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Xde.Common;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000025 RID: 37
	public interface IXdeUiFactory : IDisposable
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000218 RID: 536
		// (remove) Token: 0x06000219 RID: 537
		event EventHandler<TaskDialogArgs> ShowingTaskDialog;

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600021A RID: 538
		// (set) Token: 0x0600021B RID: 539
		bool SilentMode { get; set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600021C RID: 540
		// (set) Token: 0x0600021D RID: 541
		bool IsShuttingDown { get; set; }

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600021E RID: 542
		// (set) Token: 0x0600021F RID: 543
		IWin32Window DefaultOwner { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000220 RID: 544
		// (set) Token: 0x06000221 RID: 545
		IXdeSku Sku { get; set; }

		// Token: 0x06000222 RID: 546
		void ShowElevationRequiredForHyperV();

		// Token: 0x06000223 RID: 547
		ElevationRequiredDlg ShowElevationRequiredForEtwLogging();

		// Token: 0x06000224 RID: 548
		void ShowUsage();

		// Token: 0x06000225 RID: 549
		XdeUiFactory.ZoomUiResult DisplayZoomUi(IWin32Window owner, int scale);

		// Token: 0x06000226 RID: 550
		void DisplayToolsUi(IWin32Window owner, Point ownerLocation, Point possibleToolsLeftCornerLocation, Point possibleToolsRightCornerLocation);

		// Token: 0x06000227 RID: 551
		void ShowErrorMessageForException(IWin32Window owner, string messageFormat, Exception exception, string telemetryIdentifier);

		// Token: 0x06000228 RID: 552
		void ShowErrorMessage(IWin32Window owner, string message);

		// Token: 0x06000229 RID: 553
		void ShowErrorMessage(IWin32Window owner, string instruction, string message);

		// Token: 0x0600022A RID: 554
		void ShowErrorMessageFormat(IWin32Window owner, string instruction, string message, params string[] items);

		// Token: 0x0600022B RID: 555
		TaskDialogResult ShowTaskDialog(IWin32Window owner, string instruction, string text, TaskDialogStandardButtons buttons, TaskDialogStandardIcon icon);

		// Token: 0x0600022C RID: 556
		TaskDialogResult ShowTaskDialog(IWin32Window owner, string instruction, string text, TaskDialogStandardButtons buttons, TaskDialogResult resultWhenSilent, TaskDialogStandardIcon icon);

		// Token: 0x0600022D RID: 557
		string ShowTaskDialogWithInput(string prompt, string title, string defaultResponse);

		// Token: 0x0600022E RID: 558
		TaskDialog CreatedLinkEnabledTaskDialog();

		// Token: 0x0600022F RID: 559
		void DisplayHelp();

		// Token: 0x06000230 RID: 560
		void DisplayWebPage(string url);

		// Token: 0x06000231 RID: 561
		void DisplayWebPage(string url, string arguments);
	}
}
