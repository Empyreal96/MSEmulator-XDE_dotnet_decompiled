using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x02000022 RID: 34
	internal class NativeTaskDialogSettings
	{
		// Token: 0x06000150 RID: 336 RVA: 0x00004358 File Offset: 0x00002558
		internal NativeTaskDialogSettings()
		{
			this.NativeConfiguration = new TaskDialogNativeMethods.TaskDialogConfiguration();
			this.NativeConfiguration.size = (uint)Marshal.SizeOf<TaskDialogNativeMethods.TaskDialogConfiguration>(this.NativeConfiguration);
			this.NativeConfiguration.parentHandle = IntPtr.Zero;
			this.NativeConfiguration.instance = IntPtr.Zero;
			this.NativeConfiguration.taskDialogFlags = TaskDialogNativeMethods.TaskDialogOptions.AllowCancel;
			this.NativeConfiguration.commonButtons = TaskDialogNativeMethods.TaskDialogCommonButtons.Ok;
			this.NativeConfiguration.mainIcon = new TaskDialogNativeMethods.IconUnion(0);
			this.NativeConfiguration.footerIcon = new TaskDialogNativeMethods.IconUnion(0);
			this.NativeConfiguration.width = 0U;
			this.NativeConfiguration.buttonCount = 0U;
			this.NativeConfiguration.radioButtonCount = 0U;
			this.NativeConfiguration.buttons = IntPtr.Zero;
			this.NativeConfiguration.radioButtons = IntPtr.Zero;
			this.NativeConfiguration.defaultButtonIndex = 0;
			this.NativeConfiguration.defaultRadioButtonIndex = 0;
			this.NativeConfiguration.windowTitle = TaskDialogDefaults.Caption;
			this.NativeConfiguration.mainInstruction = TaskDialogDefaults.MainInstruction;
			this.NativeConfiguration.content = TaskDialogDefaults.Content;
			this.NativeConfiguration.verificationText = null;
			this.NativeConfiguration.expandedInformation = null;
			this.NativeConfiguration.expandedControlText = null;
			this.NativeConfiguration.collapsedControlText = null;
			this.NativeConfiguration.footerText = null;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000151 RID: 337 RVA: 0x000044AE File Offset: 0x000026AE
		// (set) Token: 0x06000152 RID: 338 RVA: 0x000044B6 File Offset: 0x000026B6
		public int ProgressBarMinimum { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000044BF File Offset: 0x000026BF
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000044C7 File Offset: 0x000026C7
		public int ProgressBarMaximum { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000044D0 File Offset: 0x000026D0
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000044D8 File Offset: 0x000026D8
		public int ProgressBarValue { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000044E1 File Offset: 0x000026E1
		// (set) Token: 0x06000158 RID: 344 RVA: 0x000044E9 File Offset: 0x000026E9
		public TaskDialogProgressBarState ProgressBarState { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000044F2 File Offset: 0x000026F2
		// (set) Token: 0x0600015A RID: 346 RVA: 0x000044FA File Offset: 0x000026FA
		public bool InvokeHelp { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00004503 File Offset: 0x00002703
		// (set) Token: 0x0600015C RID: 348 RVA: 0x0000450B File Offset: 0x0000270B
		public TaskDialogNativeMethods.TaskDialogConfiguration NativeConfiguration { get; private set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00004514 File Offset: 0x00002714
		// (set) Token: 0x0600015E RID: 350 RVA: 0x0000451C File Offset: 0x0000271C
		public TaskDialogNativeMethods.TaskDialogButton[] Buttons { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00004525 File Offset: 0x00002725
		// (set) Token: 0x06000160 RID: 352 RVA: 0x0000452D File Offset: 0x0000272D
		public TaskDialogNativeMethods.TaskDialogButton[] RadioButtons { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00004536 File Offset: 0x00002736
		// (set) Token: 0x06000162 RID: 354 RVA: 0x0000453E File Offset: 0x0000273E
		public List<int> ElevatedButtons { get; set; }
	}
}
