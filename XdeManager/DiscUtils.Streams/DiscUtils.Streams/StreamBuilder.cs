using System;
using System.Collections.Generic;
using System.IO;

namespace DiscUtils.Streams
{
	// Token: 0x02000016 RID: 22
	public abstract class StreamBuilder
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00003584 File Offset: 0x00001784
		public virtual SparseStream Build()
		{
			long length;
			List<BuilderExtent> extents = this.FixExtents(out length);
			return new BuiltStream(length, extents);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000035A4 File Offset: 0x000017A4
		public void Build(Stream output)
		{
			using (Stream stream = this.Build())
			{
				byte[] array = new byte[65536];
				for (int count = stream.Read(array, 0, array.Length); count != 0; count = stream.Read(array, 0, array.Length))
				{
					output.Write(array, 0, count);
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003608 File Offset: 0x00001808
		public void Build(string outputFile)
		{
			using (FileStream fileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
			{
				this.Build(fileStream);
			}
		}

		// Token: 0x060000A7 RID: 167
		protected abstract List<BuilderExtent> FixExtents(out long totalLength);
	}
}
