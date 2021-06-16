using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x02000021 RID: 33
	internal class NativeTaskDialog : IDisposable
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00003AF8 File Offset: 0x00001CF8
		internal NativeTaskDialog(NativeTaskDialogSettings settings, TaskDialog outerDialog)
		{
			this.nativeDialogConfig = settings.NativeConfiguration;
			this.settings = settings;
			this.nativeDialogConfig.callback = new TaskDialogNativeMethods.TaskDialogCallback(this.DialogProc);
			this.ShowState = DialogShowState.PreShow;
			this.outerDialog = outerDialog;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00003B66 File Offset: 0x00001D66
		// (set) Token: 0x06000125 RID: 293 RVA: 0x00003B6E File Offset: 0x00001D6E
		public DialogShowState ShowState { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00003B77 File Offset: 0x00001D77
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00003B7F File Offset: 0x00001D7F
		public int SelectedButtonId { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00003B88 File Offset: 0x00001D88
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00003B90 File Offset: 0x00001D90
		public int SelectedRadioButtonId { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00003B99 File Offset: 0x00001D99
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00003BA1 File Offset: 0x00001DA1
		public bool CheckBoxChecked { get; private set; }

		// Token: 0x0600012C RID: 300 RVA: 0x00003BAC File Offset: 0x00001DAC
		internal void NativeShow()
		{
			if (this.settings == null)
			{
				throw new InvalidOperationException(LocalizedMessages.NativeTaskDialogConfigurationError);
			}
			this.MarshalDialogControlStructs();
			try
			{
				this.ShowState = DialogShowState.Showing;
				int selectedButtonId;
				int selectedRadioButtonId;
				bool checkBoxChecked;
				HResult hresult = TaskDialogNativeMethods.TaskDialogIndirect(this.nativeDialogConfig, out selectedButtonId, out selectedRadioButtonId, out checkBoxChecked);
				if (CoreErrorHelper.Failed(hresult))
				{
					string message;
					if (hresult != HResult.OutOfMemory)
					{
						if (hresult == HResult.InvalidArguments)
						{
							message = LocalizedMessages.NativeTaskDialogInternalErrorArgs;
						}
						else
						{
							message = string.Format(CultureInfo.InvariantCulture, LocalizedMessages.NativeTaskDialogInternalErrorUnexpected, hresult);
						}
					}
					else
					{
						message = LocalizedMessages.NativeTaskDialogInternalErrorComplex;
					}
					Exception exceptionForHR = Marshal.GetExceptionForHR((int)hresult);
					throw new Win32Exception(message, exceptionForHR);
				}
				this.SelectedButtonId = selectedButtonId;
				this.SelectedRadioButtonId = selectedRadioButtonId;
				this.CheckBoxChecked = checkBoxChecked;
			}
			catch (EntryPointNotFoundException innerException)
			{
				throw new NotSupportedException(LocalizedMessages.NativeTaskDialogVersionError, innerException);
			}
			finally
			{
				this.ShowState = DialogShowState.Closed;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00003C88 File Offset: 0x00001E88
		internal void NativeClose(TaskDialogResult result)
		{
			this.ShowState = DialogShowState.Closing;
			int wparam;
			if (result <= TaskDialogResult.Retry)
			{
				switch (result)
				{
				case TaskDialogResult.Ok:
					wparam = 1;
					goto IL_55;
				case TaskDialogResult.Yes:
					wparam = 6;
					goto IL_55;
				case (TaskDialogResult)3:
					break;
				case TaskDialogResult.No:
					wparam = 7;
					goto IL_55;
				default:
					if (result == TaskDialogResult.Retry)
					{
						wparam = 4;
						goto IL_55;
					}
					break;
				}
			}
			else
			{
				if (result == TaskDialogResult.Close)
				{
					wparam = 8;
					goto IL_55;
				}
				if (result == TaskDialogResult.CustomButtonClicked)
				{
					wparam = 9;
					goto IL_55;
				}
			}
			wparam = 2;
			IL_55:
			this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.ClickButton, wparam, 0L);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00003CFC File Offset: 0x00001EFC
		private int DialogProc(IntPtr windowHandle, uint message, IntPtr wparam, IntPtr lparam, IntPtr referenceData)
		{
			this.hWndDialog = windowHandle;
			switch (message)
			{
			case 0U:
			{
				int result = this.PerformDialogInitialization();
				this.outerDialog.RaiseOpenedEvent();
				return result;
			}
			case 2U:
				return this.HandleButtonClick((int)wparam);
			case 3U:
				return this.HandleHyperlinkClick(lparam);
			case 4U:
				return this.HandleTick((int)wparam);
			case 5U:
				return this.PerformDialogCleanup();
			case 6U:
				return this.HandleRadioButtonClick((int)wparam);
			case 9U:
				return this.HandleHelpInvocation();
			}
			return 0;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00003D94 File Offset: 0x00001F94
		private int PerformDialogInitialization()
		{
			if (this.IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.ShowProgressBar))
			{
				this.UpdateProgressBarRange();
				this.UpdateProgressBarState(this.settings.ProgressBarState);
				this.UpdateProgressBarValue(this.settings.ProgressBarValue);
				this.UpdateProgressBarValue(this.settings.ProgressBarValue);
			}
			else if (this.IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.ShowMarqueeProgressBar))
			{
				this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarMarquee, 1, 0L);
				this.UpdateProgressBarState(this.settings.ProgressBarState);
			}
			if (this.settings.ElevatedButtons != null && this.settings.ElevatedButtons.Count > 0)
			{
				foreach (int buttonId in this.settings.ElevatedButtons)
				{
					this.UpdateElevationIcon(buttonId, true);
				}
			}
			return 0;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00003E84 File Offset: 0x00002084
		private int HandleButtonClick(int id)
		{
			if (this.ShowState != DialogShowState.Closing)
			{
				this.outerDialog.RaiseButtonClickEvent(id);
			}
			if (id < 9)
			{
				return this.outerDialog.RaiseClosingEvent(id);
			}
			return 1;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00003EAE File Offset: 0x000020AE
		private int HandleRadioButtonClick(int id)
		{
			if (this.firstRadioButtonClicked && !this.IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.NoDefaultRadioButton))
			{
				this.firstRadioButtonClicked = false;
			}
			else
			{
				this.outerDialog.RaiseButtonClickEvent(id);
			}
			return 0;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00003EDC File Offset: 0x000020DC
		private int HandleHyperlinkClick(IntPtr href)
		{
			string link = Marshal.PtrToStringUni(href);
			this.outerDialog.RaiseHyperlinkClickEvent(link);
			return 0;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00003EFD File Offset: 0x000020FD
		private int HandleTick(int ticks)
		{
			this.outerDialog.RaiseTickEvent(ticks);
			return 0;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00003F0C File Offset: 0x0000210C
		private int HandleHelpInvocation()
		{
			this.outerDialog.RaiseHelpInvokedEvent();
			return 0;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00003F1A File Offset: 0x0000211A
		private int PerformDialogCleanup()
		{
			this.firstRadioButtonClicked = true;
			return 0;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00003F24 File Offset: 0x00002124
		internal void UpdateProgressBarValue(int i)
		{
			this.AssertCurrentlyShowing();
			this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarPosition, i, 0L);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00003F3C File Offset: 0x0000213C
		internal void UpdateProgressBarRange()
		{
			this.AssertCurrentlyShowing();
			long lparam = NativeTaskDialog.MakeLongLParam(this.settings.ProgressBarMaximum, this.settings.ProgressBarMinimum);
			this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarRange, 0, lparam);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00003F79 File Offset: 0x00002179
		internal void UpdateProgressBarState(TaskDialogProgressBarState state)
		{
			this.AssertCurrentlyShowing();
			this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarState, (int)state, 0L);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00003F90 File Offset: 0x00002190
		internal void UpdateText(string text)
		{
			this.UpdateTextCore(text, TaskDialogNativeMethods.TaskDialogElements.Content);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00003F9A File Offset: 0x0000219A
		internal void UpdateInstruction(string instruction)
		{
			this.UpdateTextCore(instruction, TaskDialogNativeMethods.TaskDialogElements.MainInstruction);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00003FA4 File Offset: 0x000021A4
		internal void UpdateFooterText(string footerText)
		{
			this.UpdateTextCore(footerText, TaskDialogNativeMethods.TaskDialogElements.Footer);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00003FAE File Offset: 0x000021AE
		internal void UpdateExpandedText(string expandedText)
		{
			this.UpdateTextCore(expandedText, TaskDialogNativeMethods.TaskDialogElements.ExpandedInformation);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00003FB8 File Offset: 0x000021B8
		private void UpdateTextCore(string s, TaskDialogNativeMethods.TaskDialogElements element)
		{
			this.AssertCurrentlyShowing();
			this.FreeOldString(element);
			this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetElementText, (int)element, (long)this.MakeNewString(s, element));
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00003FE1 File Offset: 0x000021E1
		internal void UpdateMainIcon(TaskDialogStandardIcon mainIcon)
		{
			this.UpdateIconCore(mainIcon, TaskDialogNativeMethods.TaskDialogIconElement.Main);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00003FEB File Offset: 0x000021EB
		internal void UpdateFooterIcon(TaskDialogStandardIcon footerIcon)
		{
			this.UpdateIconCore(footerIcon, TaskDialogNativeMethods.TaskDialogIconElement.Footer);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00003FF5 File Offset: 0x000021F5
		private void UpdateIconCore(TaskDialogStandardIcon icon, TaskDialogNativeMethods.TaskDialogIconElement element)
		{
			this.AssertCurrentlyShowing();
			this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.UpdateIcon, (int)element, (long)icon);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000400C File Offset: 0x0000220C
		internal void UpdateCheckBoxChecked(bool cbc)
		{
			this.AssertCurrentlyShowing();
			this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.ClickVerification, cbc ? 1 : 0, 1L);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00004029 File Offset: 0x00002229
		internal void UpdateElevationIcon(int buttonId, bool showIcon)
		{
			this.AssertCurrentlyShowing();
			this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetButtonElevationRequiredState, buttonId, (long)Convert.ToInt32(showIcon));
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00004045 File Offset: 0x00002245
		internal void UpdateButtonEnabled(int buttonID, bool enabled)
		{
			this.AssertCurrentlyShowing();
			this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.EnableButton, buttonID, enabled ? 1L : 0L);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00004062 File Offset: 0x00002262
		internal void UpdateRadioButtonEnabled(int buttonID, bool enabled)
		{
			this.AssertCurrentlyShowing();
			this.SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.EnableRadioButton, buttonID, enabled ? 1L : 0L);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000407F File Offset: 0x0000227F
		internal void AssertCurrentlyShowing()
		{
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00004081 File Offset: 0x00002281
		private int SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages message, int wparam, long lparam)
		{
			return (int)CoreNativeMethods.SendMessage(this.hWndDialog, (uint)message, (IntPtr)wparam, new IntPtr(lparam));
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000040A0 File Offset: 0x000022A0
		private bool IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions flag)
		{
			return (this.nativeDialogConfig.taskDialogFlags & flag) == flag;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000040B4 File Offset: 0x000022B4
		private IntPtr MakeNewString(string text, TaskDialogNativeMethods.TaskDialogElements element)
		{
			IntPtr intPtr = Marshal.StringToHGlobalUni(text);
			this.updatedStrings[(int)element] = intPtr;
			return intPtr;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000040D4 File Offset: 0x000022D4
		private void FreeOldString(TaskDialogNativeMethods.TaskDialogElements element)
		{
			if (this.updatedStrings[(int)element] != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.updatedStrings[(int)element]);
				this.updatedStrings[(int)element] = IntPtr.Zero;
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00004111 File Offset: 0x00002311
		private static long MakeLongLParam(int a, int b)
		{
			return (long)((a << 16) + b);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000411C File Offset: 0x0000231C
		private void MarshalDialogControlStructs()
		{
			if (this.settings.Buttons != null && this.settings.Buttons.Length != 0)
			{
				this.buttonArray = NativeTaskDialog.AllocateAndMarshalButtons(this.settings.Buttons);
				this.settings.NativeConfiguration.buttons = this.buttonArray;
				this.settings.NativeConfiguration.buttonCount = (uint)this.settings.Buttons.Length;
			}
			if (this.settings.RadioButtons != null && this.settings.RadioButtons.Length != 0)
			{
				this.radioButtonArray = NativeTaskDialog.AllocateAndMarshalButtons(this.settings.RadioButtons);
				this.settings.NativeConfiguration.radioButtons = this.radioButtonArray;
				this.settings.NativeConfiguration.radioButtonCount = (uint)this.settings.RadioButtons.Length;
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000041F4 File Offset: 0x000023F4
		private static IntPtr AllocateAndMarshalButtons(TaskDialogNativeMethods.TaskDialogButton[] structs)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TaskDialogNativeMethods.TaskDialogButton)) * structs.Length);
			IntPtr intPtr2 = intPtr;
			foreach (TaskDialogNativeMethods.TaskDialogButton structure in structs)
			{
				Marshal.StructureToPtr<TaskDialogNativeMethods.TaskDialogButton>(structure, intPtr2, false);
				intPtr2 = IntPtr.Add(intPtr2, Marshal.SizeOf<TaskDialogNativeMethods.TaskDialogButton>(structure));
			}
			return intPtr;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000424D File Offset: 0x0000244D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000425C File Offset: 0x0000245C
		~NativeTaskDialog()
		{
			this.Dispose(false);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000428C File Offset: 0x0000248C
		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				if (this.ShowState == DialogShowState.Showing)
				{
					this.NativeClose(TaskDialogResult.Cancel);
				}
				if (this.updatedStrings != null)
				{
					for (int i = 0; i < this.updatedStrings.Length; i++)
					{
						if (this.updatedStrings[i] != IntPtr.Zero)
						{
							Marshal.FreeHGlobal(this.updatedStrings[i]);
							this.updatedStrings[i] = IntPtr.Zero;
						}
					}
				}
				if (this.buttonArray != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.buttonArray);
					this.buttonArray = IntPtr.Zero;
				}
				if (this.radioButtonArray != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.radioButtonArray);
					this.radioButtonArray = IntPtr.Zero;
				}
			}
		}

		// Token: 0x0400012E RID: 302
		private TaskDialogNativeMethods.TaskDialogConfiguration nativeDialogConfig;

		// Token: 0x0400012F RID: 303
		private NativeTaskDialogSettings settings;

		// Token: 0x04000130 RID: 304
		private IntPtr hWndDialog;

		// Token: 0x04000131 RID: 305
		private TaskDialog outerDialog;

		// Token: 0x04000132 RID: 306
		private IntPtr[] updatedStrings = new IntPtr[Enum.GetNames(typeof(TaskDialogNativeMethods.TaskDialogElements)).Length];

		// Token: 0x04000133 RID: 307
		private IntPtr buttonArray;

		// Token: 0x04000134 RID: 308
		private IntPtr radioButtonArray;

		// Token: 0x04000135 RID: 309
		private bool firstRadioButtonClicked = true;

		// Token: 0x0400013A RID: 314
		private bool disposed;
	}
}
