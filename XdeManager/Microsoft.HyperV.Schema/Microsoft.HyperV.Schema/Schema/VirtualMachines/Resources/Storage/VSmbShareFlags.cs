using System;
using System.CodeDom.Compiler;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x02000033 RID: 51
	[GeneratedCode("MarsComp", "")]
	[Flags]
	public enum VSmbShareFlags
	{
		// Token: 0x040000E3 RID: 227
		None = 0,
		// Token: 0x040000E4 RID: 228
		ReadOnly = 1,
		// Token: 0x040000E5 RID: 229
		ShareRead = 2,
		// Token: 0x040000E6 RID: 230
		CacheIO = 4,
		// Token: 0x040000E7 RID: 231
		NoOplocks = 8,
		// Token: 0x040000E8 RID: 232
		TakeBackupPrivilege = 16,
		// Token: 0x040000E9 RID: 233
		UseShareRootIdentity = 32,
		// Token: 0x040000EA RID: 234
		NoDirectmap = 64,
		// Token: 0x040000EB RID: 235
		NoLocks = 128,
		// Token: 0x040000EC RID: 236
		NoDirnotify = 256,
		// Token: 0x040000ED RID: 237
		Test = 512,
		// Token: 0x040000EE RID: 238
		VmSharedMemory = 1024,
		// Token: 0x040000EF RID: 239
		RestrictFileAccess = 2048,
		// Token: 0x040000F0 RID: 240
		ForceLevelIIOplocks = 4096,
		// Token: 0x040000F1 RID: 241
		ReparseBaseLayer = 8192,
		// Token: 0x040000F2 RID: 242
		PseudoOplocks = 16384,
		// Token: 0x040000F3 RID: 243
		NonCacheIO = 32768,
		// Token: 0x040000F4 RID: 244
		PseudoDirnotify = 65536,
		// Token: 0x040000F5 RID: 245
		DisableIndexing = 131072,
		// Token: 0x040000F6 RID: 246
		HideAlternateDataStreams = 262144,
		// Token: 0x040000F7 RID: 247
		EnableFsctlFiltering = 524288,
		// Token: 0x040000F8 RID: 248
		AllowNewCreates = 1048576
	}
}
