using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000036 RID: 54
	internal struct ManifestEnvelope
	{
		// Token: 0x04000115 RID: 277
		public const int MaxChunkSize = 65280;

		// Token: 0x04000116 RID: 278
		public ManifestEnvelope.ManifestFormats Format;

		// Token: 0x04000117 RID: 279
		public byte MajorVersion;

		// Token: 0x04000118 RID: 280
		public byte MinorVersion;

		// Token: 0x04000119 RID: 281
		public byte Magic;

		// Token: 0x0400011A RID: 282
		public ushort TotalChunks;

		// Token: 0x0400011B RID: 283
		public ushort ChunkNumber;

		// Token: 0x02000037 RID: 55
		public enum ManifestFormats : byte
		{
			// Token: 0x0400011D RID: 285
			SimpleXmlFormat = 1
		}
	}
}
