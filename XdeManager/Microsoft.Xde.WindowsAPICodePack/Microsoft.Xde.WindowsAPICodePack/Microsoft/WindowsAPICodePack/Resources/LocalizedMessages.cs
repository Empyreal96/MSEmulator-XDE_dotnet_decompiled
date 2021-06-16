using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.WindowsAPICodePack.Resources
{
	// Token: 0x0200000C RID: 12
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class LocalizedMessages
	{
		// Token: 0x06000051 RID: 81 RVA: 0x00002530 File Offset: 0x00000730
		internal LocalizedMessages()
		{
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002EE6 File Offset: 0x000010E6
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (LocalizedMessages.resourceMan == null)
				{
					LocalizedMessages.resourceMan = new ResourceManager("Microsoft.WindowsAPICodePack.Resources.LocalizedMessages", typeof(LocalizedMessages).Assembly);
				}
				return LocalizedMessages.resourceMan;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00002F12 File Offset: 0x00001112
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00002F19 File Offset: 0x00001119
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return LocalizedMessages.resourceCulture;
			}
			set
			{
				LocalizedMessages.resourceCulture = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00002F21 File Offset: 0x00001121
		internal static string ApplicationRecoverFailedToRegisterForRestartBadParameters
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ApplicationRecoverFailedToRegisterForRestartBadParameters", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002F37 File Offset: 0x00001137
		internal static string ApplicationRecoveryBadParameters
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ApplicationRecoveryBadParameters", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00002F4D File Offset: 0x0000114D
		internal static string ApplicationRecoveryFailedToRegister
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ApplicationRecoveryFailedToRegister", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002F63 File Offset: 0x00001163
		internal static string ApplicationRecoveryFailedToRegisterForRestart
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ApplicationRecoveryFailedToRegisterForRestart", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00002F79 File Offset: 0x00001179
		internal static string ApplicationRecoveryFailedToUnregister
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ApplicationRecoveryFailedToUnregister", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002F8F File Offset: 0x0000118F
		internal static string ApplicationRecoveryFailedToUnregisterForRestart
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ApplicationRecoveryFailedToUnregisterForRestart", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002FA5 File Offset: 0x000011A5
		internal static string ApplicationRecoveryMustBeCalledFromCallback
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ApplicationRecoveryMustBeCalledFromCallback", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002FBB File Offset: 0x000011BB
		internal static string BatteryStateStringRepresentation
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("BatteryStateStringRepresentation", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00002FD1 File Offset: 0x000011D1
		internal static string CancelableCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("CancelableCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002FE7 File Offset: 0x000011E7
		internal static string CaptionCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("CaptionCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002FFD File Offset: 0x000011FD
		internal static string CheckBoxCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("CheckBoxCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003013 File Offset: 0x00001213
		internal static string CollapsedTextCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("CollapsedTextCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003029 File Offset: 0x00001229
		internal static string CoreHelpersRunningOn7
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("CoreHelpersRunningOn7", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000062 RID: 98 RVA: 0x0000303F File Offset: 0x0000123F
		internal static string CoreHelpersRunningOnVista
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("CoreHelpersRunningOnVista", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003055 File Offset: 0x00001255
		internal static string CoreHelpersRunningOnXp
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("CoreHelpersRunningOnXp", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000306B File Offset: 0x0000126B
		internal static string DialogCollectionCannotHaveDuplicateNames
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("DialogCollectionCannotHaveDuplicateNames", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00003081 File Offset: 0x00001281
		internal static string DialogCollectionControlAlreadyHosted
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("DialogCollectionControlAlreadyHosted", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003097 File Offset: 0x00001297
		internal static string DialogCollectionControlNameNull
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("DialogCollectionControlNameNull", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000030AD File Offset: 0x000012AD
		internal static string DialogCollectionModifyShowingDialog
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("DialogCollectionModifyShowingDialog", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000030C3 File Offset: 0x000012C3
		internal static string DialogControlNameCannotBeEmpty
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("DialogControlNameCannotBeEmpty", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000030D9 File Offset: 0x000012D9
		internal static string DialogControlsCannotBeRenamed
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("DialogControlsCannotBeRenamed", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000030EF File Offset: 0x000012EF
		internal static string DialogDefaultCaption
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("DialogDefaultCaption", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003105 File Offset: 0x00001305
		internal static string DialogDefaultContent
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("DialogDefaultContent", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0000311B File Offset: 0x0000131B
		internal static string DialogDefaultMainInstruction
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("DialogDefaultMainInstruction", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003131 File Offset: 0x00001331
		internal static string ExpandedDetailsCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ExpandedDetailsCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003147 File Offset: 0x00001347
		internal static string ExpandedLabelCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ExpandedLabelCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006F RID: 111 RVA: 0x0000315D File Offset: 0x0000135D
		internal static string ExpandingStateCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ExpandingStateCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003173 File Offset: 0x00001373
		internal static string HyperlinksCannotBetSet
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("HyperlinksCannotBetSet", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003189 File Offset: 0x00001389
		internal static string InvalidReferencePath
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("InvalidReferencePath", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000072 RID: 114 RVA: 0x0000319F File Offset: 0x0000139F
		internal static string MessageManagerHandlerNotRegistered
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("MessageManagerHandlerNotRegistered", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000031B5 File Offset: 0x000013B5
		internal static string NativeTaskDialogConfigurationError
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("NativeTaskDialogConfigurationError", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000031CB File Offset: 0x000013CB
		internal static string NativeTaskDialogInternalErrorArgs
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("NativeTaskDialogInternalErrorArgs", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000031E1 File Offset: 0x000013E1
		internal static string NativeTaskDialogInternalErrorComplex
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("NativeTaskDialogInternalErrorComplex", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000031F7 File Offset: 0x000013F7
		internal static string NativeTaskDialogInternalErrorUnexpected
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("NativeTaskDialogInternalErrorUnexpected", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000320D File Offset: 0x0000140D
		internal static string NativeTaskDialogVersionError
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("NativeTaskDialogVersionError", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003223 File Offset: 0x00001423
		internal static string OwnerCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("OwnerCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003239 File Offset: 0x00001439
		internal static string PowerExecutionStateFailed
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PowerExecutionStateFailed", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600007A RID: 122 RVA: 0x0000324F File Offset: 0x0000144F
		internal static string PowerInsufficientAccessBatteryState
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PowerInsufficientAccessBatteryState", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003265 File Offset: 0x00001465
		internal static string PowerInsufficientAccessCapabilities
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PowerInsufficientAccessCapabilities", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007C RID: 124 RVA: 0x0000327B File Offset: 0x0000147B
		internal static string PowerManagerActiveSchemeFailed
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PowerManagerActiveSchemeFailed", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003291 File Offset: 0x00001491
		internal static string PowerManagerBatteryNotPresent
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PowerManagerBatteryNotPresent", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000032A7 File Offset: 0x000014A7
		internal static string ProgressBarCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ProgressBarCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000032BD File Offset: 0x000014BD
		internal static string ProgressBarCannotBeHostedInMultipleDialogs
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("ProgressBarCannotBeHostedInMultipleDialogs", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000032D3 File Offset: 0x000014D3
		internal static string PropertyKeyFormatString
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PropertyKeyFormatString", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000081 RID: 129 RVA: 0x000032E9 File Offset: 0x000014E9
		internal static string PropVariantInitializationError
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PropVariantInitializationError", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000032FF File Offset: 0x000014FF
		internal static string PropVariantMultiDimArray
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PropVariantMultiDimArray", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003315 File Offset: 0x00001515
		internal static string PropVariantNullString
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PropVariantNullString", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000084 RID: 132 RVA: 0x0000332B File Offset: 0x0000152B
		internal static string PropVariantTypeNotSupported
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PropVariantTypeNotSupported", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003341 File Offset: 0x00001541
		internal static string PropVariantUnsupportedType
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("PropVariantUnsupportedType", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003357 File Offset: 0x00001557
		internal static string RecoverySettingsFormatString
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("RecoverySettingsFormatString", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000336D File Offset: 0x0000156D
		internal static string RestartSettingsFormatString
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("RestartSettingsFormatString", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003383 File Offset: 0x00001583
		internal static string StandardButtonsCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("StandardButtonsCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003399 File Offset: 0x00001599
		internal static string StartupLocationCannotBeChanged
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("StartupLocationCannotBeChanged", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600008A RID: 138 RVA: 0x000033AF File Offset: 0x000015AF
		internal static string TaskDialogBadButtonId
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogBadButtonId", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000033C5 File Offset: 0x000015C5
		internal static string TaskDialogButtonTextEmpty
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogButtonTextEmpty", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000033DB File Offset: 0x000015DB
		internal static string TaskDialogCheckBoxTextRequiredToEnableCheckBox
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogCheckBoxTextRequiredToEnableCheckBox", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000033F1 File Offset: 0x000015F1
		internal static string TaskDialogCloseNonShowing
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogCloseNonShowing", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003407 File Offset: 0x00001607
		internal static string TaskDialogDefaultCaption
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogDefaultCaption", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600008F RID: 143 RVA: 0x0000341D File Offset: 0x0000161D
		internal static string TaskDialogDefaultContent
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogDefaultContent", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003433 File Offset: 0x00001633
		internal static string TaskDialogDefaultMainInstruction
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogDefaultMainInstruction", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003449 File Offset: 0x00001649
		internal static string TaskDialogOnlyOneDefaultControl
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogOnlyOneDefaultControl", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000092 RID: 146 RVA: 0x0000345F File Offset: 0x0000165F
		internal static string TaskDialogProgressBarMaxValueGreaterThanMin
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogProgressBarMaxValueGreaterThanMin", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003475 File Offset: 0x00001675
		internal static string TaskDialogProgressBarMinValueGreaterThanZero
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogProgressBarMinValueGreaterThanZero", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000348B File Offset: 0x0000168B
		internal static string TaskDialogProgressBarMinValueLessThanMax
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogProgressBarMinValueLessThanMax", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000034A1 File Offset: 0x000016A1
		internal static string TaskDialogProgressBarValueInRange
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogProgressBarValueInRange", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000034B7 File Offset: 0x000016B7
		internal static string TaskDialogSupportedButtonsAndButtons
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogSupportedButtonsAndButtons", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000034CD File Offset: 0x000016CD
		internal static string TaskDialogSupportedButtonsAndLinks
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogSupportedButtonsAndLinks", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000034E3 File Offset: 0x000016E3
		internal static string TaskDialogUnkownControl
		{
			get
			{
				return LocalizedMessages.ResourceManager.GetString("TaskDialogUnkownControl", LocalizedMessages.resourceCulture);
			}
		}

		// Token: 0x04000100 RID: 256
		private static ResourceManager resourceMan;

		// Token: 0x04000101 RID: 257
		private static CultureInfo resourceCulture;
	}
}
