using System;

namespace System.Management.Automation
{
	// Token: 0x020001B3 RID: 435
	[Flags]
	internal enum HelpCategory
	{
		// Token: 0x040008BC RID: 2236
		None = 0,
		// Token: 0x040008BD RID: 2237
		Alias = 1,
		// Token: 0x040008BE RID: 2238
		Cmdlet = 2,
		// Token: 0x040008BF RID: 2239
		Provider = 4,
		// Token: 0x040008C0 RID: 2240
		General = 16,
		// Token: 0x040008C1 RID: 2241
		FAQ = 32,
		// Token: 0x040008C2 RID: 2242
		Glossary = 64,
		// Token: 0x040008C3 RID: 2243
		HelpFile = 128,
		// Token: 0x040008C4 RID: 2244
		ScriptCommand = 256,
		// Token: 0x040008C5 RID: 2245
		Function = 512,
		// Token: 0x040008C6 RID: 2246
		Filter = 1024,
		// Token: 0x040008C7 RID: 2247
		ExternalScript = 2048,
		// Token: 0x040008C8 RID: 2248
		All = 1048575,
		// Token: 0x040008C9 RID: 2249
		DefaultHelp = 4096,
		// Token: 0x040008CA RID: 2250
		Workflow = 8192,
		// Token: 0x040008CB RID: 2251
		Configuration = 16384,
		// Token: 0x040008CC RID: 2252
		DscResource = 32768,
		// Token: 0x040008CD RID: 2253
		Class = 65536
	}
}
