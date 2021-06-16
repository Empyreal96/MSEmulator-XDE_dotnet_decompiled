using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x02000024 RID: 36
	public sealed class TaskDialog : IDialogControlHost, IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000164 RID: 356 RVA: 0x00004548 File Offset: 0x00002748
		// (remove) Token: 0x06000165 RID: 357 RVA: 0x00004580 File Offset: 0x00002780
		public event EventHandler<TaskDialogTickEventArgs> Tick;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000166 RID: 358 RVA: 0x000045B8 File Offset: 0x000027B8
		// (remove) Token: 0x06000167 RID: 359 RVA: 0x000045F0 File Offset: 0x000027F0
		public event EventHandler<TaskDialogHyperlinkClickedEventArgs> HyperlinkClick;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000168 RID: 360 RVA: 0x00004628 File Offset: 0x00002828
		// (remove) Token: 0x06000169 RID: 361 RVA: 0x00004660 File Offset: 0x00002860
		public event EventHandler<TaskDialogClosingEventArgs> Closing;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600016A RID: 362 RVA: 0x00004698 File Offset: 0x00002898
		// (remove) Token: 0x0600016B RID: 363 RVA: 0x000046D0 File Offset: 0x000028D0
		public event EventHandler HelpInvoked;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600016C RID: 364 RVA: 0x00004708 File Offset: 0x00002908
		// (remove) Token: 0x0600016D RID: 365 RVA: 0x00004740 File Offset: 0x00002940
		public event EventHandler Opened;

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00004775 File Offset: 0x00002975
		// (set) Token: 0x0600016F RID: 367 RVA: 0x0000477D File Offset: 0x0000297D
		public IntPtr OwnerWindowHandle
		{
			get
			{
				return this.ownerWindow;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.OwnerCannotBeChanged);
				this.ownerWindow = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00004791 File Offset: 0x00002991
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00004799 File Offset: 0x00002999
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
				if (this.NativeDialogShowing)
				{
					this.nativeDialog.UpdateText(this.text);
				}
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000172 RID: 370 RVA: 0x000047BB File Offset: 0x000029BB
		// (set) Token: 0x06000173 RID: 371 RVA: 0x000047C3 File Offset: 0x000029C3
		public string InstructionText
		{
			get
			{
				return this.instructionText;
			}
			set
			{
				this.instructionText = value;
				if (this.NativeDialogShowing)
				{
					this.nativeDialog.UpdateInstruction(this.instructionText);
				}
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000047E5 File Offset: 0x000029E5
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000047ED File Offset: 0x000029ED
		public string Caption
		{
			get
			{
				return this.caption;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.CaptionCannotBeChanged);
				this.caption = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00004801 File Offset: 0x00002A01
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00004809 File Offset: 0x00002A09
		public string FooterText
		{
			get
			{
				return this.footerText;
			}
			set
			{
				this.footerText = value;
				if (this.NativeDialogShowing)
				{
					this.nativeDialog.UpdateFooterText(this.footerText);
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000482B File Offset: 0x00002A2B
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00004833 File Offset: 0x00002A33
		public string FooterCheckBoxText
		{
			get
			{
				return this.checkBoxText;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.CheckBoxCannotBeChanged);
				this.checkBoxText = value;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00004847 File Offset: 0x00002A47
		// (set) Token: 0x0600017B RID: 379 RVA: 0x0000484F File Offset: 0x00002A4F
		public string DetailsExpandedText
		{
			get
			{
				return this.detailsExpandedText;
			}
			set
			{
				this.detailsExpandedText = value;
				if (this.NativeDialogShowing)
				{
					this.nativeDialog.UpdateExpandedText(this.detailsExpandedText);
				}
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00004871 File Offset: 0x00002A71
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00004879 File Offset: 0x00002A79
		public bool DetailsExpanded
		{
			get
			{
				return this.detailsExpanded;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.ExpandingStateCannotBeChanged);
				this.detailsExpanded = value;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000488D File Offset: 0x00002A8D
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00004895 File Offset: 0x00002A95
		public string DetailsExpandedLabel
		{
			get
			{
				return this.detailsExpandedLabel;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.ExpandedLabelCannotBeChanged);
				this.detailsExpandedLabel = value;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000048A9 File Offset: 0x00002AA9
		// (set) Token: 0x06000181 RID: 385 RVA: 0x000048B1 File Offset: 0x00002AB1
		public string DetailsCollapsedLabel
		{
			get
			{
				return this.detailsCollapsedLabel;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.CollapsedTextCannotBeChanged);
				this.detailsCollapsedLabel = value;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000182 RID: 386 RVA: 0x000048C5 File Offset: 0x00002AC5
		// (set) Token: 0x06000183 RID: 387 RVA: 0x000048CD File Offset: 0x00002ACD
		public bool Cancelable
		{
			get
			{
				return this.cancelable;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.CancelableCannotBeChanged);
				this.cancelable = value;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000184 RID: 388 RVA: 0x000048E1 File Offset: 0x00002AE1
		// (set) Token: 0x06000185 RID: 389 RVA: 0x000048E9 File Offset: 0x00002AE9
		public TaskDialogStandardIcon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
				if (this.NativeDialogShowing)
				{
					this.nativeDialog.UpdateMainIcon(this.icon);
				}
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000490B File Offset: 0x00002B0B
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00004913 File Offset: 0x00002B13
		public TaskDialogStandardIcon FooterIcon
		{
			get
			{
				return this.footerIcon;
			}
			set
			{
				this.footerIcon = value;
				if (this.NativeDialogShowing)
				{
					this.nativeDialog.UpdateFooterIcon(this.footerIcon);
				}
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00004935 File Offset: 0x00002B35
		// (set) Token: 0x06000189 RID: 393 RVA: 0x0000493D File Offset: 0x00002B3D
		public TaskDialogStandardButtons StandardButtons
		{
			get
			{
				return this.standardButtons;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.StandardButtonsCannotBeChanged);
				this.standardButtons = value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00004951 File Offset: 0x00002B51
		public DialogControlCollection<TaskDialogControl> Controls
		{
			get
			{
				return this.controls;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00004959 File Offset: 0x00002B59
		// (set) Token: 0x0600018C RID: 396 RVA: 0x00004961 File Offset: 0x00002B61
		public bool HyperlinksEnabled
		{
			get
			{
				return this.hyperlinksEnabled;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.HyperlinksCannotBetSet);
				this.hyperlinksEnabled = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00004975 File Offset: 0x00002B75
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00004988 File Offset: 0x00002B88
		public bool? FooterCheckBoxChecked
		{
			get
			{
				return new bool?(this.footerCheckBoxChecked.GetValueOrDefault(false));
			}
			set
			{
				this.footerCheckBoxChecked = value;
				if (this.NativeDialogShowing)
				{
					this.nativeDialog.UpdateCheckBoxChecked(this.footerCheckBoxChecked.Value);
				}
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000049AF File Offset: 0x00002BAF
		// (set) Token: 0x06000190 RID: 400 RVA: 0x000049B7 File Offset: 0x00002BB7
		public TaskDialogExpandedDetailsLocation ExpansionMode
		{
			get
			{
				return this.expansionMode;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.ExpandedDetailsCannotBeChanged);
				this.expansionMode = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000049CB File Offset: 0x00002BCB
		// (set) Token: 0x06000192 RID: 402 RVA: 0x000049D3 File Offset: 0x00002BD3
		public TaskDialogStartupLocation StartupLocation
		{
			get
			{
				return this.startupLocation;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.StartupLocationCannotBeChanged);
				this.startupLocation = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000193 RID: 403 RVA: 0x000049E7 File Offset: 0x00002BE7
		// (set) Token: 0x06000194 RID: 404 RVA: 0x000049EF File Offset: 0x00002BEF
		public TaskDialogProgressBar ProgressBar
		{
			get
			{
				return this.progressBar;
			}
			set
			{
				this.ThrowIfDialogShowing(LocalizedMessages.ProgressBarCannotBeChanged);
				if (value != null)
				{
					if (value.HostingDialog != null)
					{
						throw new InvalidOperationException(LocalizedMessages.ProgressBarCannotBeHostedInMultipleDialogs);
					}
					value.HostingDialog = this;
				}
				this.progressBar = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00004A20 File Offset: 0x00002C20
		// (set) Token: 0x06000196 RID: 406 RVA: 0x00004A28 File Offset: 0x00002C28
		public bool SizeToContent
		{
			get
			{
				return this.sizeToContent;
			}
			set
			{
				this.sizeToContent = value;
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00004A31 File Offset: 0x00002C31
		public TaskDialog()
		{
			CoreHelpers.ThrowIfNotVista();
			this.controls = new DialogControlCollection<TaskDialogControl>(this);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00004A6B File Offset: 0x00002C6B
		public static TaskDialogResult Show(string text)
		{
			return TaskDialog.ShowCoreStatic(text, TaskDialogDefaults.MainInstruction, TaskDialogDefaults.Caption);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00004A7D File Offset: 0x00002C7D
		public static TaskDialogResult Show(string text, string instructionText)
		{
			return TaskDialog.ShowCoreStatic(text, instructionText, TaskDialogDefaults.Caption);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00004A8B File Offset: 0x00002C8B
		public static TaskDialogResult Show(string text, string instructionText, string caption)
		{
			return TaskDialog.ShowCoreStatic(text, instructionText, caption);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00004A95 File Offset: 0x00002C95
		public TaskDialogResult Show()
		{
			return this.ShowCore();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00004AA0 File Offset: 0x00002CA0
		private static TaskDialogResult ShowCoreStatic(string text, string instructionText, string caption)
		{
			CoreHelpers.ThrowIfNotVista();
			if (TaskDialog.staticDialog == null)
			{
				TaskDialog.staticDialog = new TaskDialog();
			}
			TaskDialog.staticDialog.text = text;
			TaskDialog.staticDialog.instructionText = instructionText;
			TaskDialog.staticDialog.caption = caption;
			return TaskDialog.staticDialog.Show();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00004AF0 File Offset: 0x00002CF0
		private TaskDialogResult ShowCore()
		{
			TaskDialogResult result;
			try
			{
				this.SortDialogControls();
				this.ValidateCurrentDialogSettings();
				NativeTaskDialogSettings settings = new NativeTaskDialogSettings();
				this.ApplyCoreSettings(settings);
				this.ApplySupplementalSettings(settings);
				this.nativeDialog = new NativeTaskDialog(settings, this);
				this.nativeDialog.NativeShow();
				result = TaskDialog.ConstructDialogResult(this.nativeDialog);
				this.footerCheckBoxChecked = new bool?(this.nativeDialog.CheckBoxChecked);
			}
			finally
			{
				this.CleanUp();
				this.nativeDialog = null;
			}
			return result;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00004B78 File Offset: 0x00002D78
		private void ValidateCurrentDialogSettings()
		{
			if (this.footerCheckBoxChecked != null && this.footerCheckBoxChecked.Value && string.IsNullOrEmpty(this.checkBoxText))
			{
				throw new InvalidOperationException(LocalizedMessages.TaskDialogCheckBoxTextRequiredToEnableCheckBox);
			}
			if (this.progressBar != null && !this.progressBar.HasValidValues)
			{
				throw new InvalidOperationException(LocalizedMessages.TaskDialogProgressBarValueInRange);
			}
			if (this.buttons.Count > 0 && this.commandLinks.Count > 0)
			{
				throw new NotSupportedException(LocalizedMessages.TaskDialogSupportedButtonsAndLinks);
			}
			if (this.buttons.Count > 0 && this.standardButtons != TaskDialogStandardButtons.None)
			{
				throw new NotSupportedException(LocalizedMessages.TaskDialogSupportedButtonsAndButtons);
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00004C20 File Offset: 0x00002E20
		private static TaskDialogResult ConstructDialogResult(NativeTaskDialog native)
		{
			TaskDialogStandardButtons taskDialogStandardButtons = TaskDialog.MapButtonIdToStandardButton(native.SelectedButtonId);
			TaskDialogResult result;
			if (taskDialogStandardButtons == TaskDialogStandardButtons.None)
			{
				result = TaskDialogResult.CustomButtonClicked;
			}
			else
			{
				result = (TaskDialogResult)taskDialogStandardButtons;
			}
			return result;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00004C49 File Offset: 0x00002E49
		public void Close()
		{
			if (!this.NativeDialogShowing)
			{
				throw new InvalidOperationException(LocalizedMessages.TaskDialogCloseNonShowing);
			}
			this.nativeDialog.NativeClose(TaskDialogResult.Cancel);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00004C6A File Offset: 0x00002E6A
		public void Close(TaskDialogResult closingResult)
		{
			if (!this.NativeDialogShowing)
			{
				throw new InvalidOperationException(LocalizedMessages.TaskDialogCloseNonShowing);
			}
			this.nativeDialog.NativeClose(closingResult);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00004C8B File Offset: 0x00002E8B
		private void ApplyCoreSettings(NativeTaskDialogSettings settings)
		{
			this.ApplyGeneralNativeConfiguration(settings.NativeConfiguration);
			this.ApplyTextConfiguration(settings.NativeConfiguration);
			this.ApplyOptionConfiguration(settings.NativeConfiguration);
			this.ApplyControlConfiguration(settings);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00004CB8 File Offset: 0x00002EB8
		private void ApplyGeneralNativeConfiguration(TaskDialogNativeMethods.TaskDialogConfiguration dialogConfig)
		{
			if (this.ownerWindow != IntPtr.Zero)
			{
				dialogConfig.parentHandle = this.ownerWindow;
			}
			dialogConfig.mainIcon = new TaskDialogNativeMethods.IconUnion((int)this.icon);
			dialogConfig.footerIcon = new TaskDialogNativeMethods.IconUnion((int)this.footerIcon);
			dialogConfig.commonButtons = (TaskDialogNativeMethods.TaskDialogCommonButtons)this.standardButtons;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00004D14 File Offset: 0x00002F14
		private void ApplyTextConfiguration(TaskDialogNativeMethods.TaskDialogConfiguration dialogConfig)
		{
			dialogConfig.content = this.text;
			dialogConfig.windowTitle = this.caption;
			dialogConfig.mainInstruction = this.instructionText;
			dialogConfig.expandedInformation = this.detailsExpandedText;
			dialogConfig.expandedControlText = this.detailsExpandedLabel;
			dialogConfig.collapsedControlText = this.detailsCollapsedLabel;
			dialogConfig.footerText = this.footerText;
			dialogConfig.verificationText = this.checkBoxText;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00004D84 File Offset: 0x00002F84
		private void ApplyOptionConfiguration(TaskDialogNativeMethods.TaskDialogConfiguration dialogConfig)
		{
			TaskDialogNativeMethods.TaskDialogOptions taskDialogOptions = TaskDialogNativeMethods.TaskDialogOptions.None;
			if (this.cancelable)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.AllowCancel;
			}
			if (this.footerCheckBoxChecked != null && this.footerCheckBoxChecked.Value)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.CheckVerificationFlag;
			}
			if (this.hyperlinksEnabled)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.EnableHyperlinks;
			}
			if (this.detailsExpanded)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.ExpandedByDefault;
			}
			if (this.Tick != null)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.UseCallbackTimer;
			}
			if (this.startupLocation == TaskDialogStartupLocation.CenterOwner)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.PositionRelativeToWindow;
			}
			if (this.sizeToContent)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.SizeToContent;
			}
			if (this.expansionMode == TaskDialogExpandedDetailsLocation.ExpandFooter)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.ExpandFooterArea;
			}
			dialogConfig.taskDialogFlags = taskDialogOptions;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00004E24 File Offset: 0x00003024
		private void ApplyControlConfiguration(NativeTaskDialogSettings settings)
		{
			if (this.progressBar != null)
			{
				if (this.progressBar.State == TaskDialogProgressBarState.Marquee)
				{
					settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.ShowMarqueeProgressBar;
				}
				else
				{
					settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.ShowProgressBar;
				}
			}
			if (this.buttons.Count > 0 || this.commandLinks.Count > 0)
			{
				List<TaskDialogButtonBase> list = (this.buttons.Count > 0) ? this.buttons : this.commandLinks;
				settings.Buttons = TaskDialog.BuildButtonStructArray(list);
				if (this.commandLinks.Count > 0)
				{
					settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.UseCommandLinks;
				}
				settings.NativeConfiguration.defaultButtonIndex = TaskDialog.FindDefaultButtonId(list);
				TaskDialog.ApplyElevatedIcons(settings, list);
			}
			if (this.radioButtons.Count > 0)
			{
				settings.RadioButtons = TaskDialog.BuildButtonStructArray(this.radioButtons);
				int num = TaskDialog.FindDefaultButtonId(this.radioButtons);
				settings.NativeConfiguration.defaultRadioButtonIndex = num;
				if (num == 0)
				{
					settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.NoDefaultRadioButton;
				}
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00004F48 File Offset: 0x00003148
		private static TaskDialogNativeMethods.TaskDialogButton[] BuildButtonStructArray(List<TaskDialogButtonBase> controls)
		{
			int count = controls.Count;
			TaskDialogNativeMethods.TaskDialogButton[] array = new TaskDialogNativeMethods.TaskDialogButton[count];
			for (int i = 0; i < count; i++)
			{
				TaskDialogButtonBase taskDialogButtonBase = controls[i];
				array[i] = new TaskDialogNativeMethods.TaskDialogButton(taskDialogButtonBase.Id, taskDialogButtonBase.ToString());
			}
			return array;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00004F90 File Offset: 0x00003190
		private static int FindDefaultButtonId(List<TaskDialogButtonBase> controls)
		{
			List<TaskDialogButtonBase> list = controls.FindAll((TaskDialogButtonBase control) => control.Default);
			if (list.Count == 1)
			{
				return list[0].Id;
			}
			if (list.Count > 1)
			{
				throw new InvalidOperationException(LocalizedMessages.TaskDialogOnlyOneDefaultControl);
			}
			return 0;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00004FF0 File Offset: 0x000031F0
		private static void ApplyElevatedIcons(NativeTaskDialogSettings settings, List<TaskDialogButtonBase> controls)
		{
			foreach (TaskDialogButtonBase taskDialogButtonBase in controls)
			{
				TaskDialogButton taskDialogButton = (TaskDialogButton)taskDialogButtonBase;
				if (taskDialogButton.UseElevationIcon)
				{
					if (settings.ElevatedButtons == null)
					{
						settings.ElevatedButtons = new List<int>();
					}
					settings.ElevatedButtons.Add(taskDialogButton.Id);
				}
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00005068 File Offset: 0x00003268
		private void ApplySupplementalSettings(NativeTaskDialogSettings settings)
		{
			if (this.progressBar != null && this.progressBar.State != TaskDialogProgressBarState.Marquee)
			{
				settings.ProgressBarMinimum = this.progressBar.Minimum;
				settings.ProgressBarMaximum = this.progressBar.Maximum;
				settings.ProgressBarValue = this.progressBar.Value;
				settings.ProgressBarState = this.progressBar.State;
			}
			if (this.HelpInvoked != null)
			{
				settings.InvokeHelp = true;
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000050E0 File Offset: 0x000032E0
		private void SortDialogControls()
		{
			foreach (TaskDialogControl taskDialogControl in this.controls)
			{
				TaskDialogButtonBase taskDialogButtonBase = taskDialogControl as TaskDialogButtonBase;
				TaskDialogCommandLink taskDialogCommandLink = taskDialogControl as TaskDialogCommandLink;
				if (taskDialogButtonBase != null && string.IsNullOrEmpty(taskDialogButtonBase.Text) && taskDialogCommandLink != null && string.IsNullOrEmpty(taskDialogCommandLink.Instruction))
				{
					throw new InvalidOperationException(LocalizedMessages.TaskDialogButtonTextEmpty);
				}
				TaskDialogRadioButton item;
				if (taskDialogCommandLink != null)
				{
					this.commandLinks.Add(taskDialogCommandLink);
				}
				else if ((item = (taskDialogControl as TaskDialogRadioButton)) != null)
				{
					if (this.radioButtons == null)
					{
						this.radioButtons = new List<TaskDialogButtonBase>();
					}
					this.radioButtons.Add(item);
				}
				else if (taskDialogButtonBase != null)
				{
					if (this.buttons == null)
					{
						this.buttons = new List<TaskDialogButtonBase>();
					}
					this.buttons.Add(taskDialogButtonBase);
				}
				else
				{
					TaskDialogProgressBar taskDialogProgressBar;
					if ((taskDialogProgressBar = (taskDialogControl as TaskDialogProgressBar)) == null)
					{
						throw new InvalidOperationException(LocalizedMessages.TaskDialogUnkownControl);
					}
					this.progressBar = taskDialogProgressBar;
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000051E8 File Offset: 0x000033E8
		private static TaskDialogStandardButtons MapButtonIdToStandardButton(int id)
		{
			switch (id)
			{
			case 1:
				return TaskDialogStandardButtons.Ok;
			case 2:
				return TaskDialogStandardButtons.Cancel;
			case 3:
				return TaskDialogStandardButtons.None;
			case 4:
				return TaskDialogStandardButtons.Retry;
			case 5:
				return TaskDialogStandardButtons.None;
			case 6:
				return TaskDialogStandardButtons.Yes;
			case 7:
				return TaskDialogStandardButtons.No;
			case 8:
				return TaskDialogStandardButtons.Close;
			default:
				return TaskDialogStandardButtons.None;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00005234 File Offset: 0x00003434
		private void ThrowIfDialogShowing(string message)
		{
			if (this.NativeDialogShowing)
			{
				throw new NotSupportedException(message);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00005245 File Offset: 0x00003445
		private bool NativeDialogShowing
		{
			get
			{
				return this.nativeDialog != null && (this.nativeDialog.ShowState == DialogShowState.Showing || this.nativeDialog.ShowState == DialogShowState.Closing);
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000526F File Offset: 0x0000346F
		private TaskDialogButtonBase GetButtonForId(int id)
		{
			return (TaskDialogButtonBase)this.controls.GetControlbyId(id);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00005282 File Offset: 0x00003482
		bool IDialogControlHost.IsCollectionChangeAllowed()
		{
			return !this.NativeDialogShowing;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000407F File Offset: 0x0000227F
		void IDialogControlHost.ApplyCollectionChanged()
		{
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00005290 File Offset: 0x00003490
		bool IDialogControlHost.IsControlPropertyChangeAllowed(string propertyName, DialogControl control)
		{
			bool result = false;
			if (!this.NativeDialogShowing)
			{
				result = !(propertyName == "Enabled");
			}
			else if (!(propertyName == "Text") && !(propertyName == "Default"))
			{
				if (propertyName == "ShowElevationIcon" || propertyName == "Enabled")
				{
					result = true;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000052FC File Offset: 0x000034FC
		void IDialogControlHost.ApplyControlPropertyChange(string propertyName, DialogControl control)
		{
			if (this.NativeDialogShowing)
			{
				TaskDialogButton taskDialogButton;
				TaskDialogRadioButton taskDialogRadioButton;
				if (control is TaskDialogProgressBar)
				{
					if (!this.progressBar.HasValidValues)
					{
						throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarValueInRange);
					}
					if (propertyName == "State")
					{
						this.nativeDialog.UpdateProgressBarState(this.progressBar.State);
						return;
					}
					if (propertyName == "Value")
					{
						this.nativeDialog.UpdateProgressBarValue(this.progressBar.Value);
						return;
					}
					if (!(propertyName == "Minimum") && !(propertyName == "Maximum"))
					{
						return;
					}
					this.nativeDialog.UpdateProgressBarRange();
					return;
				}
				else if ((taskDialogButton = (control as TaskDialogButton)) != null)
				{
					if (propertyName == "ShowElevationIcon")
					{
						this.nativeDialog.UpdateElevationIcon(taskDialogButton.Id, taskDialogButton.UseElevationIcon);
						return;
					}
					if (!(propertyName == "Enabled"))
					{
						return;
					}
					this.nativeDialog.UpdateButtonEnabled(taskDialogButton.Id, taskDialogButton.Enabled);
					return;
				}
				else if ((taskDialogRadioButton = (control as TaskDialogRadioButton)) != null && propertyName == "Enabled")
				{
					this.nativeDialog.UpdateRadioButtonEnabled(taskDialogRadioButton.Id, taskDialogRadioButton.Enabled);
				}
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000542C File Offset: 0x0000362C
		internal void RaiseButtonClickEvent(int id)
		{
			TaskDialogButtonBase buttonForId = this.GetButtonForId(id);
			if (buttonForId != null)
			{
				buttonForId.RaiseClickEvent();
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000544C File Offset: 0x0000364C
		internal void RaiseHyperlinkClickEvent(string link)
		{
			EventHandler<TaskDialogHyperlinkClickedEventArgs> hyperlinkClick = this.HyperlinkClick;
			if (hyperlinkClick != null)
			{
				hyperlinkClick(this, new TaskDialogHyperlinkClickedEventArgs(link));
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00005470 File Offset: 0x00003670
		internal int RaiseClosingEvent(int id)
		{
			EventHandler<TaskDialogClosingEventArgs> closing = this.Closing;
			if (closing != null)
			{
				TaskDialogClosingEventArgs taskDialogClosingEventArgs = new TaskDialogClosingEventArgs();
				TaskDialogStandardButtons taskDialogStandardButtons = TaskDialog.MapButtonIdToStandardButton(id);
				if (taskDialogStandardButtons == TaskDialogStandardButtons.None)
				{
					TaskDialogButtonBase buttonForId = this.GetButtonForId(id);
					if (buttonForId == null)
					{
						throw new InvalidOperationException(LocalizedMessages.TaskDialogBadButtonId);
					}
					taskDialogClosingEventArgs.CustomButton = buttonForId.Name;
					taskDialogClosingEventArgs.TaskDialogResult = TaskDialogResult.CustomButtonClicked;
				}
				else
				{
					taskDialogClosingEventArgs.TaskDialogResult = (TaskDialogResult)taskDialogStandardButtons;
				}
				closing(this, taskDialogClosingEventArgs);
				if (taskDialogClosingEventArgs.Cancel)
				{
					return 1;
				}
			}
			return 0;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000054E2 File Offset: 0x000036E2
		internal void RaiseHelpInvokedEvent()
		{
			if (this.HelpInvoked != null)
			{
				this.HelpInvoked(this, EventArgs.Empty);
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000054FD File Offset: 0x000036FD
		internal void RaiseOpenedEvent()
		{
			if (this.Opened != null)
			{
				this.Opened(this, EventArgs.Empty);
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00005518 File Offset: 0x00003718
		internal void RaiseTickEvent(int ticks)
		{
			if (this.Tick != null)
			{
				this.Tick(this, new TaskDialogTickEventArgs(ticks));
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00005534 File Offset: 0x00003734
		private void CleanUp()
		{
			if (this.progressBar != null)
			{
				this.progressBar.Reset();
			}
			if (this.buttons != null)
			{
				this.buttons.Clear();
			}
			if (this.commandLinks != null)
			{
				this.commandLinks.Clear();
			}
			if (this.radioButtons != null)
			{
				this.radioButtons.Clear();
			}
			this.progressBar = null;
			if (this.nativeDialog != null)
			{
				this.nativeDialog.Dispose();
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000055A7 File Offset: 0x000037A7
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000055B8 File Offset: 0x000037B8
		~TaskDialog()
		{
			this.Dispose(false);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000055E8 File Offset: 0x000037E8
		public void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				if (disposing)
				{
					if (this.nativeDialog != null && this.nativeDialog.ShowState == DialogShowState.Showing)
					{
						this.nativeDialog.NativeClose(TaskDialogResult.Cancel);
					}
					this.buttons = null;
					this.radioButtons = null;
					this.commandLinks = null;
				}
				if (this.nativeDialog != null)
				{
					this.nativeDialog.Dispose();
					this.nativeDialog = null;
				}
				if (TaskDialog.staticDialog != null)
				{
					TaskDialog.staticDialog.Dispose();
					TaskDialog.staticDialog = null;
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000566F File Offset: 0x0000386F
		public static bool IsPlatformSupported
		{
			get
			{
				return CoreHelpers.RunningOnVista;
			}
		}

		// Token: 0x04000147 RID: 327
		private static TaskDialog staticDialog;

		// Token: 0x04000148 RID: 328
		private NativeTaskDialog nativeDialog;

		// Token: 0x04000149 RID: 329
		private List<TaskDialogButtonBase> buttons = new List<TaskDialogButtonBase>();

		// Token: 0x0400014A RID: 330
		private List<TaskDialogButtonBase> radioButtons = new List<TaskDialogButtonBase>();

		// Token: 0x0400014B RID: 331
		private List<TaskDialogButtonBase> commandLinks = new List<TaskDialogButtonBase>();

		// Token: 0x0400014C RID: 332
		private IntPtr ownerWindow;

		// Token: 0x04000152 RID: 338
		private string text;

		// Token: 0x04000153 RID: 339
		private string instructionText;

		// Token: 0x04000154 RID: 340
		private string caption;

		// Token: 0x04000155 RID: 341
		private string footerText;

		// Token: 0x04000156 RID: 342
		private string checkBoxText;

		// Token: 0x04000157 RID: 343
		private string detailsExpandedText;

		// Token: 0x04000158 RID: 344
		private bool detailsExpanded;

		// Token: 0x04000159 RID: 345
		private string detailsExpandedLabel;

		// Token: 0x0400015A RID: 346
		private string detailsCollapsedLabel;

		// Token: 0x0400015B RID: 347
		private bool cancelable;

		// Token: 0x0400015C RID: 348
		private TaskDialogStandardIcon icon;

		// Token: 0x0400015D RID: 349
		private TaskDialogStandardIcon footerIcon;

		// Token: 0x0400015E RID: 350
		private TaskDialogStandardButtons standardButtons;

		// Token: 0x0400015F RID: 351
		private DialogControlCollection<TaskDialogControl> controls;

		// Token: 0x04000160 RID: 352
		private bool hyperlinksEnabled;

		// Token: 0x04000161 RID: 353
		private bool? footerCheckBoxChecked;

		// Token: 0x04000162 RID: 354
		private TaskDialogExpandedDetailsLocation expansionMode;

		// Token: 0x04000163 RID: 355
		private TaskDialogStartupLocation startupLocation;

		// Token: 0x04000164 RID: 356
		private TaskDialogProgressBar progressBar;

		// Token: 0x04000165 RID: 357
		private bool sizeToContent;

		// Token: 0x04000166 RID: 358
		private bool disposed;
	}
}
