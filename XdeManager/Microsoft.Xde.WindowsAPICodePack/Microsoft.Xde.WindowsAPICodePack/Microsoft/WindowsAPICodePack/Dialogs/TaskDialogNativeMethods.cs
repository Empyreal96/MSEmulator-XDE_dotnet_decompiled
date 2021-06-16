using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	// Token: 0x02000023 RID: 35
	internal static class TaskDialogNativeMethods
	{
		// Token: 0x06000163 RID: 355
		[DllImport("Comctl32.dll", SetLastError = true)]
		internal static extern HResult TaskDialogIndirect([In] TaskDialogNativeMethods.TaskDialogConfiguration taskConfig, out int button, out int radioButton, [MarshalAs(UnmanagedType.Bool)] out bool verificationFlagChecked);

		// Token: 0x04000144 RID: 324
		internal const int TaskDialogIdealWidth = 0;

		// Token: 0x04000145 RID: 325
		internal const int TaskDialogButtonShieldIcon = 1;

		// Token: 0x04000146 RID: 326
		internal const int NoDefaultButtonSpecified = 0;

		// Token: 0x02000052 RID: 82
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		internal class TaskDialogConfiguration
		{
			// Token: 0x040001FA RID: 506
			internal uint size;

			// Token: 0x040001FB RID: 507
			internal IntPtr parentHandle;

			// Token: 0x040001FC RID: 508
			internal IntPtr instance;

			// Token: 0x040001FD RID: 509
			internal TaskDialogNativeMethods.TaskDialogOptions taskDialogFlags;

			// Token: 0x040001FE RID: 510
			internal TaskDialogNativeMethods.TaskDialogCommonButtons commonButtons;

			// Token: 0x040001FF RID: 511
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string windowTitle;

			// Token: 0x04000200 RID: 512
			internal TaskDialogNativeMethods.IconUnion mainIcon;

			// Token: 0x04000201 RID: 513
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string mainInstruction;

			// Token: 0x04000202 RID: 514
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string content;

			// Token: 0x04000203 RID: 515
			internal uint buttonCount;

			// Token: 0x04000204 RID: 516
			internal IntPtr buttons;

			// Token: 0x04000205 RID: 517
			internal int defaultButtonIndex;

			// Token: 0x04000206 RID: 518
			internal uint radioButtonCount;

			// Token: 0x04000207 RID: 519
			internal IntPtr radioButtons;

			// Token: 0x04000208 RID: 520
			internal int defaultRadioButtonIndex;

			// Token: 0x04000209 RID: 521
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string verificationText;

			// Token: 0x0400020A RID: 522
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string expandedInformation;

			// Token: 0x0400020B RID: 523
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string expandedControlText;

			// Token: 0x0400020C RID: 524
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string collapsedControlText;

			// Token: 0x0400020D RID: 525
			internal TaskDialogNativeMethods.IconUnion footerIcon;

			// Token: 0x0400020E RID: 526
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string footerText;

			// Token: 0x0400020F RID: 527
			internal TaskDialogNativeMethods.TaskDialogCallback callback;

			// Token: 0x04000210 RID: 528
			internal IntPtr callbackData;

			// Token: 0x04000211 RID: 529
			internal uint width;
		}

		// Token: 0x02000053 RID: 83
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
		internal struct IconUnion
		{
			// Token: 0x06000291 RID: 657 RVA: 0x00006D1E File Offset: 0x00004F1E
			internal IconUnion(int i)
			{
				this.spacer = IntPtr.Zero;
				this.mainIcon = i;
			}

			// Token: 0x170000CA RID: 202
			// (get) Token: 0x06000292 RID: 658 RVA: 0x00006D32 File Offset: 0x00004F32
			public int MainIcon
			{
				get
				{
					return this.mainIcon;
				}
			}

			// Token: 0x04000212 RID: 530
			[FieldOffset(0)]
			private int mainIcon;

			// Token: 0x04000213 RID: 531
			[FieldOffset(0)]
			private IntPtr spacer;
		}

		// Token: 0x02000054 RID: 84
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		internal struct TaskDialogButton
		{
			// Token: 0x06000293 RID: 659 RVA: 0x00006D3A File Offset: 0x00004F3A
			public TaskDialogButton(int buttonId, string text)
			{
				this.buttonId = buttonId;
				this.buttonText = text;
			}

			// Token: 0x04000214 RID: 532
			internal int buttonId;

			// Token: 0x04000215 RID: 533
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string buttonText;
		}

		// Token: 0x02000055 RID: 85
		[Flags]
		internal enum TaskDialogCommonButtons
		{
			// Token: 0x04000217 RID: 535
			Ok = 1,
			// Token: 0x04000218 RID: 536
			Yes = 2,
			// Token: 0x04000219 RID: 537
			No = 4,
			// Token: 0x0400021A RID: 538
			Cancel = 8,
			// Token: 0x0400021B RID: 539
			Retry = 16,
			// Token: 0x0400021C RID: 540
			Close = 32
		}

		// Token: 0x02000056 RID: 86
		internal enum TaskDialogCommonButtonReturnIds
		{
			// Token: 0x0400021E RID: 542
			Ok = 1,
			// Token: 0x0400021F RID: 543
			Cancel,
			// Token: 0x04000220 RID: 544
			Abort,
			// Token: 0x04000221 RID: 545
			Retry,
			// Token: 0x04000222 RID: 546
			Ignore,
			// Token: 0x04000223 RID: 547
			Yes,
			// Token: 0x04000224 RID: 548
			No,
			// Token: 0x04000225 RID: 549
			Close
		}

		// Token: 0x02000057 RID: 87
		internal enum TaskDialogElements
		{
			// Token: 0x04000227 RID: 551
			Content,
			// Token: 0x04000228 RID: 552
			ExpandedInformation,
			// Token: 0x04000229 RID: 553
			Footer,
			// Token: 0x0400022A RID: 554
			MainInstruction
		}

		// Token: 0x02000058 RID: 88
		internal enum TaskDialogIconElement
		{
			// Token: 0x0400022C RID: 556
			Main,
			// Token: 0x0400022D RID: 557
			Footer
		}

		// Token: 0x02000059 RID: 89
		[Flags]
		internal enum TaskDialogOptions
		{
			// Token: 0x0400022F RID: 559
			None = 0,
			// Token: 0x04000230 RID: 560
			EnableHyperlinks = 1,
			// Token: 0x04000231 RID: 561
			UseMainIcon = 2,
			// Token: 0x04000232 RID: 562
			UseFooterIcon = 4,
			// Token: 0x04000233 RID: 563
			AllowCancel = 8,
			// Token: 0x04000234 RID: 564
			UseCommandLinks = 16,
			// Token: 0x04000235 RID: 565
			UseNoIconCommandLinks = 32,
			// Token: 0x04000236 RID: 566
			ExpandFooterArea = 64,
			// Token: 0x04000237 RID: 567
			ExpandedByDefault = 128,
			// Token: 0x04000238 RID: 568
			CheckVerificationFlag = 256,
			// Token: 0x04000239 RID: 569
			ShowProgressBar = 512,
			// Token: 0x0400023A RID: 570
			ShowMarqueeProgressBar = 1024,
			// Token: 0x0400023B RID: 571
			UseCallbackTimer = 2048,
			// Token: 0x0400023C RID: 572
			PositionRelativeToWindow = 4096,
			// Token: 0x0400023D RID: 573
			RightToLeftLayout = 8192,
			// Token: 0x0400023E RID: 574
			NoDefaultRadioButton = 16384,
			// Token: 0x0400023F RID: 575
			SizeToContent = 16777216
		}

		// Token: 0x0200005A RID: 90
		internal enum TaskDialogMessages
		{
			// Token: 0x04000241 RID: 577
			NavigatePage = 1125,
			// Token: 0x04000242 RID: 578
			ClickButton,
			// Token: 0x04000243 RID: 579
			SetMarqueeProgressBar,
			// Token: 0x04000244 RID: 580
			SetProgressBarState,
			// Token: 0x04000245 RID: 581
			SetProgressBarRange,
			// Token: 0x04000246 RID: 582
			SetProgressBarPosition,
			// Token: 0x04000247 RID: 583
			SetProgressBarMarquee,
			// Token: 0x04000248 RID: 584
			SetElementText,
			// Token: 0x04000249 RID: 585
			ClickRadioButton = 1134,
			// Token: 0x0400024A RID: 586
			EnableButton,
			// Token: 0x0400024B RID: 587
			EnableRadioButton,
			// Token: 0x0400024C RID: 588
			ClickVerification,
			// Token: 0x0400024D RID: 589
			UpdateElementText,
			// Token: 0x0400024E RID: 590
			SetButtonElevationRequiredState,
			// Token: 0x0400024F RID: 591
			UpdateIcon
		}

		// Token: 0x0200005B RID: 91
		internal enum TaskDialogNotifications
		{
			// Token: 0x04000251 RID: 593
			Created,
			// Token: 0x04000252 RID: 594
			Navigated,
			// Token: 0x04000253 RID: 595
			ButtonClicked,
			// Token: 0x04000254 RID: 596
			HyperlinkClicked,
			// Token: 0x04000255 RID: 597
			Timer,
			// Token: 0x04000256 RID: 598
			Destroyed,
			// Token: 0x04000257 RID: 599
			RadioButtonClicked,
			// Token: 0x04000258 RID: 600
			Constructed,
			// Token: 0x04000259 RID: 601
			VerificationClicked,
			// Token: 0x0400025A RID: 602
			Help,
			// Token: 0x0400025B RID: 603
			ExpandButtonClicked
		}

		// Token: 0x0200005C RID: 92
		// (Invoke) Token: 0x06000295 RID: 661
		internal delegate int TaskDialogCallback(IntPtr hwnd, uint message, IntPtr wparam, IntPtr lparam, IntPtr referenceData);

		// Token: 0x0200005D RID: 93
		internal enum ProgressBarState
		{
			// Token: 0x0400025D RID: 605
			Normal = 1,
			// Token: 0x0400025E RID: 606
			Error,
			// Token: 0x0400025F RID: 607
			Paused
		}

		// Token: 0x0200005E RID: 94
		internal enum TaskDialogIcons
		{
			// Token: 0x04000261 RID: 609
			Warning = 65535,
			// Token: 0x04000262 RID: 610
			Error = 65534,
			// Token: 0x04000263 RID: 611
			Information = 65533,
			// Token: 0x04000264 RID: 612
			Shield = 65532
		}
	}
}
