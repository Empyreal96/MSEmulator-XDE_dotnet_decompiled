using System;
using System.CodeDom.Compiler;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x02000034 RID: 52
	[GeneratedCode("MarsComp", "")]
	[Flags]
	public enum Plan9ShareFlags
	{
		// Token: 0x040000FA RID: 250
		None = 0,
		// Token: 0x040000FB RID: 251
		ReadOnly = 1,
		// Token: 0x040000FC RID: 252
		LinuxMetadata = 4,
		// Token: 0x040000FD RID: 253
		CaseSensitive = 8,
		// Token: 0x040000FE RID: 254
		UseShareRootIdentity = 16,
		// Token: 0x040000FF RID: 255
		AllowOptions = 32,
		// Token: 0x04000100 RID: 256
		AllowSubPaths = 64,
		// Token: 0x04000101 RID: 257
		RestrictFileAccess = 128
	}
}
