using System;
using System.IO;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000031 RID: 49
	internal interface INtfsContext
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001E0 RID: 480
		AllocateFileFn AllocateFile { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001E1 RID: 481
		AttributeDefinitions AttributeDefinitions { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001E2 RID: 482
		BiosParameterBlock BiosParameterBlock { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001E3 RID: 483
		ClusterBitmap ClusterBitmap { get; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001E4 RID: 484
		ForgetFileFn ForgetFile { get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001E5 RID: 485
		GetDirectoryByIndexFn GetDirectoryByIndex { get; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001E6 RID: 486
		GetDirectoryByRefFn GetDirectoryByRef { get; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001E7 RID: 487
		GetFileByIndexFn GetFileByIndex { get; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001E8 RID: 488
		GetFileByRefFn GetFileByRef { get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001E9 RID: 489
		MasterFileTable Mft { get; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001EA RID: 490
		ObjectIds ObjectIds { get; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001EB RID: 491
		NtfsOptions Options { get; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001EC RID: 492
		Quotas Quotas { get; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001ED RID: 493
		Stream RawStream { get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001EE RID: 494
		bool ReadOnly { get; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001EF RID: 495
		ReparsePoints ReparsePoints { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001F0 RID: 496
		SecurityDescriptors SecurityDescriptors { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001F1 RID: 497
		UpperCase UpperCase { get; }
	}
}
