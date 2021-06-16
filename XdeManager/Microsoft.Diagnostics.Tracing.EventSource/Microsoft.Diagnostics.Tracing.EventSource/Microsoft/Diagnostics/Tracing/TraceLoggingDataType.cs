using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000077 RID: 119
	internal enum TraceLoggingDataType
	{
		// Token: 0x04000154 RID: 340
		Nil,
		// Token: 0x04000155 RID: 341
		Utf16String,
		// Token: 0x04000156 RID: 342
		MbcsString,
		// Token: 0x04000157 RID: 343
		Int8,
		// Token: 0x04000158 RID: 344
		UInt8,
		// Token: 0x04000159 RID: 345
		Int16,
		// Token: 0x0400015A RID: 346
		UInt16,
		// Token: 0x0400015B RID: 347
		Int32,
		// Token: 0x0400015C RID: 348
		UInt32,
		// Token: 0x0400015D RID: 349
		Int64,
		// Token: 0x0400015E RID: 350
		UInt64,
		// Token: 0x0400015F RID: 351
		Float,
		// Token: 0x04000160 RID: 352
		Double,
		// Token: 0x04000161 RID: 353
		Boolean32,
		// Token: 0x04000162 RID: 354
		Binary,
		// Token: 0x04000163 RID: 355
		Guid,
		// Token: 0x04000164 RID: 356
		FileTime = 17,
		// Token: 0x04000165 RID: 357
		SystemTime,
		// Token: 0x04000166 RID: 358
		HexInt32 = 20,
		// Token: 0x04000167 RID: 359
		HexInt64,
		// Token: 0x04000168 RID: 360
		CountedUtf16String,
		// Token: 0x04000169 RID: 361
		CountedMbcsString,
		// Token: 0x0400016A RID: 362
		Struct,
		// Token: 0x0400016B RID: 363
		Char16 = 518,
		// Token: 0x0400016C RID: 364
		Char8 = 516,
		// Token: 0x0400016D RID: 365
		Boolean8 = 772,
		// Token: 0x0400016E RID: 366
		HexInt8 = 1028,
		// Token: 0x0400016F RID: 367
		HexInt16 = 1030,
		// Token: 0x04000170 RID: 368
		Utf16Xml = 2817,
		// Token: 0x04000171 RID: 369
		MbcsXml,
		// Token: 0x04000172 RID: 370
		CountedUtf16Xml = 2838,
		// Token: 0x04000173 RID: 371
		CountedMbcsXml,
		// Token: 0x04000174 RID: 372
		Utf16Json = 3073,
		// Token: 0x04000175 RID: 373
		MbcsJson,
		// Token: 0x04000176 RID: 374
		CountedUtf16Json = 3094,
		// Token: 0x04000177 RID: 375
		CountedMbcsJson,
		// Token: 0x04000178 RID: 376
		HResult = 3847
	}
}
