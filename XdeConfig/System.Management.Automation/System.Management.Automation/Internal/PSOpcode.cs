using System;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200025A RID: 602
	internal enum PSOpcode : byte
	{
		// Token: 0x04000BCF RID: 3023
		WinStart = 1,
		// Token: 0x04000BD0 RID: 3024
		WinStop,
		// Token: 0x04000BD1 RID: 3025
		Open = 10,
		// Token: 0x04000BD2 RID: 3026
		Close,
		// Token: 0x04000BD3 RID: 3027
		Connect,
		// Token: 0x04000BD4 RID: 3028
		Disconnect,
		// Token: 0x04000BD5 RID: 3029
		Negotiate,
		// Token: 0x04000BD6 RID: 3030
		Create,
		// Token: 0x04000BD7 RID: 3031
		Constructor,
		// Token: 0x04000BD8 RID: 3032
		Dispose,
		// Token: 0x04000BD9 RID: 3033
		EventHandler,
		// Token: 0x04000BDA RID: 3034
		Exception,
		// Token: 0x04000BDB RID: 3035
		Method,
		// Token: 0x04000BDC RID: 3036
		Send,
		// Token: 0x04000BDD RID: 3037
		Receive,
		// Token: 0x04000BDE RID: 3038
		Rehydration,
		// Token: 0x04000BDF RID: 3039
		SerializationSettings,
		// Token: 0x04000BE0 RID: 3040
		ShuttingDown
	}
}
