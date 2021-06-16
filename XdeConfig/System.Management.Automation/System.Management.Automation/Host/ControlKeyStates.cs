using System;

namespace System.Management.Automation.Host
{
	// Token: 0x02000221 RID: 545
	[Flags]
	public enum ControlKeyStates
	{
		// Token: 0x04000A8E RID: 2702
		RightAltPressed = 1,
		// Token: 0x04000A8F RID: 2703
		LeftAltPressed = 2,
		// Token: 0x04000A90 RID: 2704
		RightCtrlPressed = 4,
		// Token: 0x04000A91 RID: 2705
		LeftCtrlPressed = 8,
		// Token: 0x04000A92 RID: 2706
		ShiftPressed = 16,
		// Token: 0x04000A93 RID: 2707
		NumLockOn = 32,
		// Token: 0x04000A94 RID: 2708
		ScrollLockOn = 64,
		// Token: 0x04000A95 RID: 2709
		CapsLockOn = 128,
		// Token: 0x04000A96 RID: 2710
		EnhancedKey = 256
	}
}
