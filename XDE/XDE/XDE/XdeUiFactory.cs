using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Xde.Common;
using Microsoft.Xde.Telemetry;
using Microsoft.Xde.Tools;

namespace Microsoft.Xde.Client
{
	// Token: 0x0200002B RID: 43
	public sealed class XdeUiFactory : IXdeUiFactory, IDisposable, IXdeMinUiFactory
	{
		// Token: 0x060002D9 RID: 729 RVA: 0x0000AFB4 File Offset: 0x000091B4
		public XdeUiFactory()
		{
			XdeUiFactory.Instance = this;
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060002DA RID: 730 RVA: 0x0000AFC4 File Offset: 0x000091C4
		// (remove) Token: 0x060002DB RID: 731 RVA: 0x0000AFFC File Offset: 0x000091FC
		public event EventHandler<TaskDialogArgs> ShowingTaskDialog;

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000B031 File Offset: 0x00009231
		// (set) Token: 0x060002DD RID: 733 RVA: 0x0000B038 File Offset: 0x00009238
		public static IXdeUiFactory Instance { get; private set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000B040 File Offset: 0x00009240
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000B048 File Offset: 0x00009248
		public bool SilentMode { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000B051 File Offset: 0x00009251
		// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000B059 File Offset: 0x00009259
		public bool IsShuttingDown { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000B062 File Offset: 0x00009262
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x0000B06A File Offset: 0x0000926A
		public IWin32Window DefaultOwner { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000B073 File Offset: 0x00009273
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000B07B File Offset: 0x0000927B
		public IXdeSku Sku { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000B084 File Offset: 0x00009284
		private string BrandingName
		{
			get
			{
				if (this.Sku != null && this.Sku.Branding != null)
				{
					return this.Sku.Branding.DisplayName;
				}
				return null;
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000B0AD File Offset: 0x000092AD
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000B0C8 File Offset: 0x000092C8
		public void ShowElevationRequiredForHyperV()
		{
			using (ElevationRequiredDlg elevationRequiredDlg = new ElevationRequiredDlg(this))
			{
				elevationRequiredDlg.TaskDlg.FooterCheckBoxText = Resources.AddMeToHyperVAdminsText;
				elevationRequiredDlg.TaskDlg.FooterText = Resources.AddMeToHyperVAdminsText2;
				elevationRequiredDlg.TaskDlg.FooterCheckBoxChecked = new bool?(true);
				elevationRequiredDlg.RetryXdeAsElevatedCallback = new ElevationRequiredDlg.RetryXdeAsElevatedCallbackType(this.RetryXdeAsElevatedCallbackForHyperV);
				elevationRequiredDlg.ShowDialog(true);
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000B144 File Offset: 0x00009344
		public ElevationRequiredDlg ShowElevationRequiredForEtwLogging()
		{
			ElevationRequiredDlg result;
			using (ElevationRequiredDlg elevationRequiredDlg = new ElevationRequiredDlg(this))
			{
				elevationRequiredDlg.TaskDlg.InstructionText = Resources.CouldntStartLogging;
				elevationRequiredDlg.TaskDlg.FooterCheckBoxText = Resources.AddMeToPerfLogUsersGroupText;
				elevationRequiredDlg.TaskDlg.FooterText = Resources.AddMeToPerfLogUsersGroupText2;
				elevationRequiredDlg.TaskDlg.FooterCheckBoxChecked = new bool?(true);
				elevationRequiredDlg.RetryXdeAsElevatedCallback = new ElevationRequiredDlg.RetryXdeAsElevatedCallbackType(this.RetryXdeAsElevatedCallbackForEtwLogging);
				elevationRequiredDlg.ShowDialog(false);
				if (elevationRequiredDlg.ShouldRetry)
				{
					Process.GetCurrentProcess().CloseMainWindow();
				}
				result = elevationRequiredDlg;
			}
			return result;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000B1E4 File Offset: 0x000093E4
		public void ShowUsage()
		{
			string text = StringUtilities.CurrentCultureFormat(Resources.UsageFormat, new object[]
			{
				"Default Emulator",
				512,
				"480x800",
				Microsoft.Xde.Common.Globals.XdeVersion.ToString()
			});
			this.ShowTaskDialog(null, null, text, TaskDialogStandardButtons.Ok, TaskDialogResult.Ok, TaskDialogStandardIcon.None);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000B23C File Offset: 0x0000943C
		public XdeUiFactory.ZoomUiResult DisplayZoomUi(IWin32Window owner, int scale)
		{
			XdeUiFactory.ZoomUiResult result;
			using (ScaleForm scaleForm = new ScaleForm())
			{
				scaleForm.DisplayScale = scale;
				XdeUiFactory.ZoomUiResult zoomUiResult = new XdeUiFactory.ZoomUiResult();
				XdeUiFactory.EnableOwnersWindows(owner, false);
				zoomUiResult.Result = scaleForm.ShowDialog(owner);
				XdeUiFactory.EnableOwnersWindows(owner, true);
				if (zoomUiResult.Result == DialogResult.OK)
				{
					zoomUiResult.Scale = scaleForm.DisplayScale;
				}
				result = zoomUiResult;
			}
			return result;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000B2AC File Offset: 0x000094AC
		public void DisplayToolsUi(IWin32Window owner, System.Drawing.Point ownerLocation, System.Drawing.Point possibleToolsLeftCornerLocation, System.Drawing.Point possibleToolsRightCornerLocation)
		{
			if (this.toolsWindow == null)
			{
				this.toolsWindow = new ToolsWindow(this.Sku);
			}
			if (this.toolsWindow.Visibility != Visibility.Visible)
			{
				this.toolsWindow.Left = (double)possibleToolsLeftCornerLocation.X;
				this.toolsWindow.Top = (double)possibleToolsLeftCornerLocation.Y;
				this.toolsWindow.Owner = (Window)owner;
				this.toolsWindow.Visibility = Visibility.Visible;
				System.Drawing.Size childSize = new System.Drawing.Size((int)this.toolsWindow.Width, (int)this.toolsWindow.Height);
				System.Drawing.Point possibleLocation = possibleToolsLeftCornerLocation;
				System.Drawing.Point possibleLocation2 = new System.Drawing.Point(possibleToolsRightCornerLocation.X - (int)this.toolsWindow.Width, possibleToolsRightCornerLocation.Y);
				XdeUiFactory.ChildWindowBoundsPotential childWindowBoundsPotential = XdeUiFactory.ChildWindowBoundsPotential.GetChildWindowBoundsPotential(ownerLocation, possibleLocation, childSize);
				XdeUiFactory.ChildWindowBoundsPotential childWindowBoundsPotential2 = XdeUiFactory.ChildWindowBoundsPotential.GetChildWindowBoundsPotential(ownerLocation, possibleLocation2, childSize);
				System.Drawing.Point fixedPossibleLocation = childWindowBoundsPotential.FixedPossibleLocation;
				if ((childWindowBoundsPotential2.Area > childWindowBoundsPotential.Area || (childWindowBoundsPotential2.SameScreenAsOwner && !childWindowBoundsPotential.SameScreenAsOwner)) && childWindowBoundsPotential2.FixedPossibleLocation.X >= 0)
				{
					fixedPossibleLocation = childWindowBoundsPotential2.FixedPossibleLocation;
				}
				this.toolsWindow.Left = (double)fixedPossibleLocation.X;
				this.toolsWindow.Top = (double)fixedPossibleLocation.Y;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000B3E8 File Offset: 0x000095E8
		public void ShowErrorMessageForException(IWin32Window owner, string messageFormat, Exception exception, string telemetryIdentifier)
		{
			Logger.Instance.LogException(telemetryIdentifier, exception, this.IsShuttingDown || this.disposed);
			string message = StringUtilities.CurrentCultureFormat(messageFormat, new object[]
			{
				exception.Message
			});
			this.ShowErrorMessage(owner, message);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000B431 File Offset: 0x00009631
		public void ShowErrorMessage(IWin32Window owner, string message)
		{
			this.ShowErrorMessage(owner, null, message);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000B43C File Offset: 0x0000963C
		public void ShowErrorMessage(IWin32Window owner, string instruction, string message)
		{
			if (!this.IsShuttingDown && !this.disposed)
			{
				this.ShowTaskDialog(owner, instruction, message, TaskDialogStandardButtons.Close, TaskDialogResult.Close, TaskDialogStandardIcon.Error);
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000B464 File Offset: 0x00009664
		public void ShowErrorMessageFormat(IWin32Window owner, string instruction, string messageFormat, params string[] items)
		{
			this.ShowErrorMessage(owner, instruction, StringUtilities.CurrentCultureFormat(messageFormat, items));
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000B483 File Offset: 0x00009683
		public TaskDialogResult ShowTaskDialog(IWin32Window owner, string instruction, string text, TaskDialogStandardButtons buttons, TaskDialogStandardIcon icon)
		{
			return this.ShowTaskDialog(owner, instruction, text, buttons, TaskDialogResult.Ok, icon);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000B494 File Offset: 0x00009694
		public TaskDialogResult ShowTaskDialog(IWin32Window owner, string instruction, string text, TaskDialogStandardButtons buttons, TaskDialogResult resultWhenSilent, TaskDialogStandardIcon icon)
		{
			if (this.SilentMode)
			{
				return resultWhenSilent;
			}
			TaskDialogArgs taskDialogArgs = new TaskDialogArgs();
			taskDialogArgs.CancelDialog = false;
			taskDialogArgs.Text = text;
			taskDialogArgs.Instruction = instruction;
			taskDialogArgs.Buttons = buttons;
			taskDialogArgs.Icon = icon;
			if (this.ShowingTaskDialog != null)
			{
				this.ShowingTaskDialog(this, taskDialogArgs);
				if (taskDialogArgs.CancelDialog)
				{
					return taskDialogArgs.Result;
				}
			}
			Form form = owner as Form;
			TaskDialogResult result = TaskDialogResult.Ok;
			if (form != null && form.InvokeRequired)
			{
				form.Invoke(new MethodInvoker(delegate()
				{
					result = this.ActualShowTaskDialog(owner, instruction, text, buttons, icon);
				}));
			}
			else
			{
				result = this.ActualShowTaskDialog(owner, instruction, text, buttons, icon);
			}
			return result;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000B5A4 File Offset: 0x000097A4
		public string ShowTaskDialogWithInput(string prompt, string title, string defaultResponse)
		{
			if (this.SilentMode)
			{
				return defaultResponse;
			}
			TaskDialogArgs taskDialogArgs = new TaskDialogArgs
			{
				CancelDialog = false,
				Text = prompt,
				Instruction = title,
				Icon = TaskDialogStandardIcon.None,
				Buttons = (TaskDialogStandardButtons.Ok | TaskDialogStandardButtons.Cancel)
			};
			if (this.ShowingTaskDialog != null)
			{
				this.ShowingTaskDialog(this, taskDialogArgs);
				if (taskDialogArgs.CancelDialog)
				{
					return string.Empty;
				}
			}
			return Interaction.InputBox(prompt, title, defaultResponse, -1, -1);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000B612 File Offset: 0x00009812
		public TaskDialog CreatedLinkEnabledTaskDialog()
		{
			TaskDialog taskDialog = new TaskDialog();
			taskDialog.HyperlinksEnabled = true;
			taskDialog.HyperlinkClick += this.TaskDialog_HyperlinkClick;
			return taskDialog;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000B634 File Offset: 0x00009834
		public bool ShowElevationDialog(string instruction, string text, bool enableRestart)
		{
			bool result = false;
			using (ElevationRequiredDlg elevationRequiredDlg = new ElevationRequiredDlg(this))
			{
				elevationRequiredDlg.TaskDlg.InstructionText = instruction;
				elevationRequiredDlg.TaskDlg.Text = text;
				elevationRequiredDlg.ShowDialog(enableRestart);
				result = elevationRequiredDlg.ShouldRetry;
			}
			return result;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000B690 File Offset: 0x00009890
		public void DisplayHelp()
		{
			this.DisplayWebPage("http://go.microsoft.com/fwlink/?LinkId=532763");
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000B69D File Offset: 0x0000989D
		public void DisplayWebPage(string url)
		{
			this.DisplayWebPage(url, null);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000B6A7 File Offset: 0x000098A7
		public void DisplayWebPage(string url, string arguments)
		{
			Process.Start(url, arguments);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000B6B1 File Offset: 0x000098B1
		void IXdeMinUiFactory.ShowErrorMessage(string instruction, string message)
		{
			this.ShowErrorMessage(this.DefaultOwner, instruction, message);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000B6C1 File Offset: 0x000098C1
		void IXdeMinUiFactory.ShowErrorMessage(string message)
		{
			this.ShowErrorMessage(this.DefaultOwner, message);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000B6D0 File Offset: 0x000098D0
		void IXdeMinUiFactory.ShowErrorMessageForException(string messageFormat, Exception exception, string telemetryIdentifier)
		{
			this.ShowErrorMessageForException(this.DefaultOwner, messageFormat, exception, telemetryIdentifier);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000B6E1 File Offset: 0x000098E1
		void IXdeMinUiFactory.ShowErrorMessageFormat(string instruction, string message, params string[] items)
		{
			this.ShowErrorMessageFormat(this.DefaultOwner, instruction, message, items);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000B6F2 File Offset: 0x000098F2
		TaskDialogResult IXdeMinUiFactory.ShowTaskDialog(string instruction, string text, TaskDialogStandardButtons buttons, TaskDialogResult resultWhenSilent, TaskDialogStandardIcon icon)
		{
			return this.ShowTaskDialog(this.DefaultOwner, instruction, text, buttons, resultWhenSilent, icon);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000B707 File Offset: 0x00009907
		TaskDialogResult IXdeMinUiFactory.ShowTaskDialog(string instruction, string text, TaskDialogStandardButtons buttons, TaskDialogStandardIcon icon)
		{
			return this.ShowTaskDialog(this.DefaultOwner, instruction, text, buttons, icon);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000B71C File Offset: 0x0000991C
		private static void EnableOwnersWindows(IWin32Window owner, bool enable)
		{
			Form form = owner as Form;
			if (form != null && form.Owner != null)
			{
				form.Owner.Enabled = enable;
				foreach (Form form2 in form.Owner.OwnedForms)
				{
					if (form2 != owner)
					{
						form2.Enabled = enable;
					}
				}
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000B770 File Offset: 0x00009970
		private void TaskDialog_HyperlinkClick(object sender, TaskDialogHyperlinkClickedEventArgs e)
		{
			if (e.LinkText.StartsWith("http:", StringComparison.OrdinalIgnoreCase))
			{
				Process.Start(e.LinkText, null);
				return;
			}
			foreach (string text in Environment.ExpandEnvironmentVariables(e.LinkText).Split(new char[]
			{
				';'
			}))
			{
				if (File.Exists(text))
				{
					Process.Start(text);
					return;
				}
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000B7DC File Offset: 0x000099DC
		private TaskDialogResult ActualShowTaskDialog(IWin32Window owner, string instruction, string text, TaskDialogStandardButtons buttons, TaskDialogStandardIcon icon)
		{
			return new XdeUiFactory.TaskDialogInstance(this).ShowTaskDialog(owner, instruction, text, buttons, icon);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000B7F0 File Offset: 0x000099F0
		private void RetryXdeAsElevatedCallbackForHyperV(ElevationRequiredDlg dlg)
		{
			if (dlg.TaskDlg.FooterCheckBoxChecked.Value)
			{
				WindowsIdentity current = WindowsIdentity.GetCurrent();
				dlg.RestartElevatedArgs = StringUtilities.InvariantCultureFormat("/addUserToHyperVAdmins \"{0}\"", new object[]
				{
					current.Name
				});
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000B838 File Offset: 0x00009A38
		private void RetryXdeAsElevatedCallbackForEtwLogging(ElevationRequiredDlg dlg)
		{
			if (dlg.TaskDlg.FooterCheckBoxChecked.Value)
			{
				WindowsIdentity current = WindowsIdentity.GetCurrent();
				dlg.RestartElevatedArgs = StringUtilities.InvariantCultureFormat("/addUserToPerformanceLogUsersGroup \"{0}\"", new object[]
				{
					current.Name
				});
			}
		}

		// Token: 0x040000FF RID: 255
		private ToolsWindow toolsWindow;

		// Token: 0x04000100 RID: 256
		private bool disposed;

		// Token: 0x02000047 RID: 71
		public class ZoomUiResult
		{
			// Token: 0x170001C8 RID: 456
			// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00011A77 File Offset: 0x0000FC77
			// (set) Token: 0x060004A6 RID: 1190 RVA: 0x00011A7F File Offset: 0x0000FC7F
			public DialogResult Result { get; set; }

			// Token: 0x170001C9 RID: 457
			// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00011A88 File Offset: 0x0000FC88
			// (set) Token: 0x060004A8 RID: 1192 RVA: 0x00011A90 File Offset: 0x0000FC90
			public int Scale { get; set; }
		}

		// Token: 0x02000048 RID: 72
		private class ChildWindowBoundsPotential
		{
			// Token: 0x170001CA RID: 458
			// (get) Token: 0x060004AA RID: 1194 RVA: 0x00011AA1 File Offset: 0x0000FCA1
			// (set) Token: 0x060004AB RID: 1195 RVA: 0x00011AA9 File Offset: 0x0000FCA9
			public int Area { get; private set; }

			// Token: 0x170001CB RID: 459
			// (get) Token: 0x060004AC RID: 1196 RVA: 0x00011AB2 File Offset: 0x0000FCB2
			// (set) Token: 0x060004AD RID: 1197 RVA: 0x00011ABA File Offset: 0x0000FCBA
			public System.Drawing.Point FixedPossibleLocation { get; private set; }

			// Token: 0x170001CC RID: 460
			// (get) Token: 0x060004AE RID: 1198 RVA: 0x00011AC3 File Offset: 0x0000FCC3
			// (set) Token: 0x060004AF RID: 1199 RVA: 0x00011ACB File Offset: 0x0000FCCB
			public bool SameScreenAsOwner { get; private set; }

			// Token: 0x060004B0 RID: 1200 RVA: 0x00011AD4 File Offset: 0x0000FCD4
			public static XdeUiFactory.ChildWindowBoundsPotential GetChildWindowBoundsPotential(System.Drawing.Point ownerLocation, System.Drawing.Point possibleLocation, System.Drawing.Size childSize)
			{
				XdeUiFactory.ChildWindowBoundsPotential childWindowBoundsPotential = new XdeUiFactory.ChildWindowBoundsPotential
				{
					FixedPossibleLocation = possibleLocation
				};
				Screen screen = Screen.FromPoint(ownerLocation);
				if (childWindowBoundsPotential.FixedPossibleLocation.Y < screen.Bounds.Y)
				{
					childWindowBoundsPotential.FixedPossibleLocation = new System.Drawing.Point(childWindowBoundsPotential.FixedPossibleLocation.X, screen.Bounds.Y);
				}
				Screen screen2 = Screen.FromPoint(childWindowBoundsPotential.FixedPossibleLocation);
				Rectangle rectangle = new Rectangle(possibleLocation, childSize);
				rectangle.Intersect(screen2.Bounds);
				childWindowBoundsPotential.Area = rectangle.Width * rectangle.Height;
				childWindowBoundsPotential.SameScreenAsOwner = (screen.DeviceName == screen2.DeviceName);
				return childWindowBoundsPotential;
			}
		}

		// Token: 0x02000049 RID: 73
		private class TaskDialogInstance
		{
			// Token: 0x060004B2 RID: 1202 RVA: 0x00011B95 File Offset: 0x0000FD95
			public TaskDialogInstance(XdeUiFactory factory)
			{
				this.factory = factory;
			}

			// Token: 0x060004B3 RID: 1203 RVA: 0x00011BA4 File Offset: 0x0000FDA4
			public TaskDialogResult ShowTaskDialog(IWin32Window owner, string instruction, string text, TaskDialogStandardButtons buttons, TaskDialogStandardIcon icon)
			{
				XdeUiFactory.EnableOwnersWindows(owner, false);
				this.result = TaskDialogResult.None;
				using (TaskDialog taskDialog = this.factory.CreatedLinkEnabledTaskDialog())
				{
					taskDialog.Caption = this.factory.BrandingName;
					taskDialog.Icon = icon;
					taskDialog.InstructionText = instruction;
					taskDialog.Text = text;
					taskDialog.SizeToContent = XdeUiFactory.TaskDialogInstance.DoesContainLongWord(text);
					foreach (object obj in Enum.GetValues(typeof(TaskDialogStandardButtons)))
					{
						TaskDialogStandardButtons taskDialogStandardButtons = (TaskDialogStandardButtons)obj;
						if (taskDialogStandardButtons != TaskDialogStandardButtons.None && (buttons & taskDialogStandardButtons) == taskDialogStandardButtons)
						{
							string @string = Resources.ResourceManager.GetString("ButtonText_" + taskDialogStandardButtons.ToString(), Resources.Culture);
							TaskDialogButton taskDialogButton = new TaskDialogButton(taskDialogStandardButtons.ToString(), @string);
							taskDialogButton.Click += this.Button_Click;
							taskDialog.Controls.Add(taskDialogButton);
						}
					}
					if (owner != null)
					{
						try
						{
							taskDialog.OwnerWindowHandle = owner.Handle;
						}
						catch (ObjectDisposedException)
						{
						}
					}
					taskDialog.StartupLocation = TaskDialogStartupLocation.CenterOwner;
					this.currentDlg = taskDialog;
					taskDialog.Show();
				}
				XdeUiFactory.EnableOwnersWindows(owner, true);
				return this.result;
			}

			// Token: 0x060004B4 RID: 1204 RVA: 0x00011D34 File Offset: 0x0000FF34
			private static string RemoveHrefs(string input)
			{
				return XdeUiFactory.TaskDialogInstance.HrefRegEx.Replace(input, string.Empty);
			}

			// Token: 0x060004B5 RID: 1205 RVA: 0x00011D48 File Offset: 0x0000FF48
			private static bool DoesContainLongWord(string text)
			{
				string input = XdeUiFactory.TaskDialogInstance.RemoveHrefs(text);
				return XdeUiFactory.TaskDialogInstance.LongWordRegEx.IsMatch(input);
			}

			// Token: 0x060004B6 RID: 1206 RVA: 0x00011D68 File Offset: 0x0000FF68
			private void Button_Click(object sender, EventArgs e)
			{
				TaskDialogButton taskDialogButton = (TaskDialogButton)sender;
				this.result = (TaskDialogResult)Enum.Parse(typeof(TaskDialogResult), taskDialogButton.Name);
				this.currentDlg.Close();
			}

			// Token: 0x040001C9 RID: 457
			private static readonly Regex LongWordRegEx = new Regex("\\S{20,}");

			// Token: 0x040001CA RID: 458
			private static readonly Regex HrefRegEx = new Regex("href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))", RegexOptions.IgnoreCase);

			// Token: 0x040001CB RID: 459
			private TaskDialog currentDlg;

			// Token: 0x040001CC RID: 460
			private TaskDialogResult result;

			// Token: 0x040001CD RID: 461
			private XdeUiFactory factory;
		}
	}
}
