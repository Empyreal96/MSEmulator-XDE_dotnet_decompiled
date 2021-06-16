using System;

namespace MS.WindowsAPICodePack.Internal
{
	// Token: 0x02000002 RID: 2
	internal enum WindowMessage
	{
		// Token: 0x04000002 RID: 2
		Null,
		// Token: 0x04000003 RID: 3
		Create,
		// Token: 0x04000004 RID: 4
		Destroy,
		// Token: 0x04000005 RID: 5
		Move,
		// Token: 0x04000006 RID: 6
		Size = 5,
		// Token: 0x04000007 RID: 7
		Activate,
		// Token: 0x04000008 RID: 8
		SetFocus,
		// Token: 0x04000009 RID: 9
		KillFocus,
		// Token: 0x0400000A RID: 10
		Enable = 10,
		// Token: 0x0400000B RID: 11
		SetRedraw,
		// Token: 0x0400000C RID: 12
		SetText,
		// Token: 0x0400000D RID: 13
		GetText,
		// Token: 0x0400000E RID: 14
		GetTextLength,
		// Token: 0x0400000F RID: 15
		Paint,
		// Token: 0x04000010 RID: 16
		Close,
		// Token: 0x04000011 RID: 17
		QueryEndSession,
		// Token: 0x04000012 RID: 18
		Quit,
		// Token: 0x04000013 RID: 19
		QueryOpen,
		// Token: 0x04000014 RID: 20
		EraseBackground,
		// Token: 0x04000015 RID: 21
		SystemColorChange,
		// Token: 0x04000016 RID: 22
		EndSession,
		// Token: 0x04000017 RID: 23
		SystemError,
		// Token: 0x04000018 RID: 24
		ShowWindow,
		// Token: 0x04000019 RID: 25
		ControlColor,
		// Token: 0x0400001A RID: 26
		WinIniChange,
		// Token: 0x0400001B RID: 27
		SettingChange = 26,
		// Token: 0x0400001C RID: 28
		DevModeChange,
		// Token: 0x0400001D RID: 29
		ActivateApplication,
		// Token: 0x0400001E RID: 30
		FontChange,
		// Token: 0x0400001F RID: 31
		TimeChange,
		// Token: 0x04000020 RID: 32
		CancelMode,
		// Token: 0x04000021 RID: 33
		SetCursor,
		// Token: 0x04000022 RID: 34
		MouseActivate,
		// Token: 0x04000023 RID: 35
		ChildActivate,
		// Token: 0x04000024 RID: 36
		QueueSync,
		// Token: 0x04000025 RID: 37
		GetMinMaxInfo,
		// Token: 0x04000026 RID: 38
		PaintIcon = 38,
		// Token: 0x04000027 RID: 39
		IconEraseBackground,
		// Token: 0x04000028 RID: 40
		NextDialogControl,
		// Token: 0x04000029 RID: 41
		SpoolerStatus = 42,
		// Token: 0x0400002A RID: 42
		DrawItem,
		// Token: 0x0400002B RID: 43
		MeasureItem,
		// Token: 0x0400002C RID: 44
		DeleteItem,
		// Token: 0x0400002D RID: 45
		VKeyToItem,
		// Token: 0x0400002E RID: 46
		CharToItem,
		// Token: 0x0400002F RID: 47
		SetFont,
		// Token: 0x04000030 RID: 48
		GetFont,
		// Token: 0x04000031 RID: 49
		SetHotkey,
		// Token: 0x04000032 RID: 50
		GetHotkey,
		// Token: 0x04000033 RID: 51
		QueryDragIcon = 55,
		// Token: 0x04000034 RID: 52
		CompareItem = 57,
		// Token: 0x04000035 RID: 53
		Compacting = 65,
		// Token: 0x04000036 RID: 54
		WindowPositionChanging = 70,
		// Token: 0x04000037 RID: 55
		WindowPositionChanged,
		// Token: 0x04000038 RID: 56
		Power,
		// Token: 0x04000039 RID: 57
		CopyData = 74,
		// Token: 0x0400003A RID: 58
		CancelJournal,
		// Token: 0x0400003B RID: 59
		Notify = 78,
		// Token: 0x0400003C RID: 60
		InputLanguageChangeRequest = 80,
		// Token: 0x0400003D RID: 61
		InputLanguageChange,
		// Token: 0x0400003E RID: 62
		TCard,
		// Token: 0x0400003F RID: 63
		Help,
		// Token: 0x04000040 RID: 64
		UserChanged,
		// Token: 0x04000041 RID: 65
		NotifyFormat,
		// Token: 0x04000042 RID: 66
		ContextMenu = 123,
		// Token: 0x04000043 RID: 67
		StyleChanging,
		// Token: 0x04000044 RID: 68
		StyleChanged,
		// Token: 0x04000045 RID: 69
		DisplayChange,
		// Token: 0x04000046 RID: 70
		GetIcon,
		// Token: 0x04000047 RID: 71
		SetIcon,
		// Token: 0x04000048 RID: 72
		NCCreate,
		// Token: 0x04000049 RID: 73
		NCDestroy,
		// Token: 0x0400004A RID: 74
		NCCalculateSize,
		// Token: 0x0400004B RID: 75
		NCHitTest,
		// Token: 0x0400004C RID: 76
		NCPaint,
		// Token: 0x0400004D RID: 77
		NCActivate,
		// Token: 0x0400004E RID: 78
		GetDialogCode,
		// Token: 0x0400004F RID: 79
		NCMouseMove = 160,
		// Token: 0x04000050 RID: 80
		NCLeftButtonDown,
		// Token: 0x04000051 RID: 81
		NCLeftButtonUp,
		// Token: 0x04000052 RID: 82
		NCLeftButtonDoubleClick,
		// Token: 0x04000053 RID: 83
		NCRightButtonDown,
		// Token: 0x04000054 RID: 84
		NCRightButtonUp,
		// Token: 0x04000055 RID: 85
		NCRightButtonDoubleClick,
		// Token: 0x04000056 RID: 86
		NCMiddleButtonDown,
		// Token: 0x04000057 RID: 87
		NCMiddleButtonUp,
		// Token: 0x04000058 RID: 88
		NCMiddleButtonDoubleClick,
		// Token: 0x04000059 RID: 89
		KeyFirst = 256,
		// Token: 0x0400005A RID: 90
		KeyDown = 256,
		// Token: 0x0400005B RID: 91
		KeyUp,
		// Token: 0x0400005C RID: 92
		Char,
		// Token: 0x0400005D RID: 93
		DeadChar,
		// Token: 0x0400005E RID: 94
		SystemKeyDown,
		// Token: 0x0400005F RID: 95
		SystemKeyUp,
		// Token: 0x04000060 RID: 96
		SystemChar,
		// Token: 0x04000061 RID: 97
		SystemDeadChar,
		// Token: 0x04000062 RID: 98
		KeyLast,
		// Token: 0x04000063 RID: 99
		IMEStartComposition = 269,
		// Token: 0x04000064 RID: 100
		IMEEndComposition,
		// Token: 0x04000065 RID: 101
		IMEComposition,
		// Token: 0x04000066 RID: 102
		IMEKeyLast = 271,
		// Token: 0x04000067 RID: 103
		InitializeDialog,
		// Token: 0x04000068 RID: 104
		Command,
		// Token: 0x04000069 RID: 105
		SystemCommand,
		// Token: 0x0400006A RID: 106
		Timer,
		// Token: 0x0400006B RID: 107
		HorizontalScroll,
		// Token: 0x0400006C RID: 108
		VerticalScroll,
		// Token: 0x0400006D RID: 109
		InitializeMenu,
		// Token: 0x0400006E RID: 110
		InitializeMenuPopup,
		// Token: 0x0400006F RID: 111
		MenuSelect = 287,
		// Token: 0x04000070 RID: 112
		MenuChar,
		// Token: 0x04000071 RID: 113
		EnterIdle,
		// Token: 0x04000072 RID: 114
		CTLColorMessageBox = 306,
		// Token: 0x04000073 RID: 115
		CTLColorEdit,
		// Token: 0x04000074 RID: 116
		CTLColorListbox,
		// Token: 0x04000075 RID: 117
		CTLColorButton,
		// Token: 0x04000076 RID: 118
		CTLColorDialog,
		// Token: 0x04000077 RID: 119
		CTLColorScrollBar,
		// Token: 0x04000078 RID: 120
		CTLColorStatic,
		// Token: 0x04000079 RID: 121
		MouseFirst = 512,
		// Token: 0x0400007A RID: 122
		MouseMove = 512,
		// Token: 0x0400007B RID: 123
		LeftButtonDown,
		// Token: 0x0400007C RID: 124
		LeftButtonUp,
		// Token: 0x0400007D RID: 125
		LeftButtonDoubleClick,
		// Token: 0x0400007E RID: 126
		RightButtonDown,
		// Token: 0x0400007F RID: 127
		RightButtonUp,
		// Token: 0x04000080 RID: 128
		RightButtonDoubleClick,
		// Token: 0x04000081 RID: 129
		MiddleButtonDown,
		// Token: 0x04000082 RID: 130
		MiddleButtonUp,
		// Token: 0x04000083 RID: 131
		MiddleButtonDoubleClick,
		// Token: 0x04000084 RID: 132
		MouseWheel,
		// Token: 0x04000085 RID: 133
		MouseHorizontalWheel = 526,
		// Token: 0x04000086 RID: 134
		ParentNotify = 528,
		// Token: 0x04000087 RID: 135
		EnterMenuLoop,
		// Token: 0x04000088 RID: 136
		ExitMenuLoop,
		// Token: 0x04000089 RID: 137
		NextMenu,
		// Token: 0x0400008A RID: 138
		Sizing,
		// Token: 0x0400008B RID: 139
		CaptureChanged,
		// Token: 0x0400008C RID: 140
		Moving,
		// Token: 0x0400008D RID: 141
		PowerBroadcast = 536,
		// Token: 0x0400008E RID: 142
		DeviceChange,
		// Token: 0x0400008F RID: 143
		MDICreate = 544,
		// Token: 0x04000090 RID: 144
		MDIDestroy,
		// Token: 0x04000091 RID: 145
		MDIActivate,
		// Token: 0x04000092 RID: 146
		MDIRestore,
		// Token: 0x04000093 RID: 147
		MDINext,
		// Token: 0x04000094 RID: 148
		MDIMaximize,
		// Token: 0x04000095 RID: 149
		MDITile,
		// Token: 0x04000096 RID: 150
		MDICascade,
		// Token: 0x04000097 RID: 151
		MDIIconArrange,
		// Token: 0x04000098 RID: 152
		MDIGetActive,
		// Token: 0x04000099 RID: 153
		MDISetMenu = 560,
		// Token: 0x0400009A RID: 154
		EnterSizeMove,
		// Token: 0x0400009B RID: 155
		ExitSizeMove,
		// Token: 0x0400009C RID: 156
		DropFiles,
		// Token: 0x0400009D RID: 157
		MDIRefreshMenu,
		// Token: 0x0400009E RID: 158
		IMESetContext = 641,
		// Token: 0x0400009F RID: 159
		IMENotify,
		// Token: 0x040000A0 RID: 160
		IMEControl,
		// Token: 0x040000A1 RID: 161
		IMECompositionFull,
		// Token: 0x040000A2 RID: 162
		IMESelect,
		// Token: 0x040000A3 RID: 163
		IMEChar,
		// Token: 0x040000A4 RID: 164
		IMEKeyDown = 656,
		// Token: 0x040000A5 RID: 165
		IMEKeyUp,
		// Token: 0x040000A6 RID: 166
		MouseHover = 673,
		// Token: 0x040000A7 RID: 167
		NCMouseLeave,
		// Token: 0x040000A8 RID: 168
		MouseLeave,
		// Token: 0x040000A9 RID: 169
		Cut = 768,
		// Token: 0x040000AA RID: 170
		Copy,
		// Token: 0x040000AB RID: 171
		Paste,
		// Token: 0x040000AC RID: 172
		Clear,
		// Token: 0x040000AD RID: 173
		Undo,
		// Token: 0x040000AE RID: 174
		RenderFormat,
		// Token: 0x040000AF RID: 175
		RenderAllFormats,
		// Token: 0x040000B0 RID: 176
		DestroyClipboard,
		// Token: 0x040000B1 RID: 177
		DrawClipbard,
		// Token: 0x040000B2 RID: 178
		PaintClipbard,
		// Token: 0x040000B3 RID: 179
		VerticalScrollClipBoard,
		// Token: 0x040000B4 RID: 180
		SizeClipbard,
		// Token: 0x040000B5 RID: 181
		AskClipboardFormatname,
		// Token: 0x040000B6 RID: 182
		ChangeClipboardChain,
		// Token: 0x040000B7 RID: 183
		HorizontalScrollClipboard,
		// Token: 0x040000B8 RID: 184
		QueryNewPalette,
		// Token: 0x040000B9 RID: 185
		PaletteIsChanging,
		// Token: 0x040000BA RID: 186
		PaletteChanged,
		// Token: 0x040000BB RID: 187
		Hotkey,
		// Token: 0x040000BC RID: 188
		Print = 791,
		// Token: 0x040000BD RID: 189
		PrintClient,
		// Token: 0x040000BE RID: 190
		HandHeldFirst = 856,
		// Token: 0x040000BF RID: 191
		HandHeldlast = 863,
		// Token: 0x040000C0 RID: 192
		PenWinFirst = 896,
		// Token: 0x040000C1 RID: 193
		PenWinLast = 911,
		// Token: 0x040000C2 RID: 194
		CoalesceFirst,
		// Token: 0x040000C3 RID: 195
		CoalesceLast = 927,
		// Token: 0x040000C4 RID: 196
		DDE_First = 992,
		// Token: 0x040000C5 RID: 197
		DDE_Initiate = 992,
		// Token: 0x040000C6 RID: 198
		DDE_Terminate,
		// Token: 0x040000C7 RID: 199
		DDE_Advise,
		// Token: 0x040000C8 RID: 200
		DDE_Unadvise,
		// Token: 0x040000C9 RID: 201
		DDE_Ack,
		// Token: 0x040000CA RID: 202
		DDE_Data,
		// Token: 0x040000CB RID: 203
		DDE_Request,
		// Token: 0x040000CC RID: 204
		DDE_Poke,
		// Token: 0x040000CD RID: 205
		DDE_Execute,
		// Token: 0x040000CE RID: 206
		DDE_Last = 1000,
		// Token: 0x040000CF RID: 207
		User = 1024,
		// Token: 0x040000D0 RID: 208
		App = 32768
	}
}
