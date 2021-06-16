using System;
using System.Collections.Generic;
using System.Globalization;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000010 RID: 16
	internal sealed class FreeSpaceTable
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x000047B1 File Offset: 0x000029B1
		public FreeSpaceTable(long fileSize)
		{
			this._freeExtents = new List<StreamExtent>();
			this._freeExtents.Add(new StreamExtent(0L, fileSize));
			this._fileSize = fileSize;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000047E0 File Offset: 0x000029E0
		public void ExtendTo(long fileSize, bool isFree)
		{
			if (fileSize % 1048576L != 0L)
			{
				throw new ArgumentException("VHDX space must be allocated on 1MB boundaries", "fileSize");
			}
			if (fileSize < this._fileSize)
			{
				throw new ArgumentOutOfRangeException("fileSize", "Attempt to extend file to smaller size", fileSize.ToString(CultureInfo.InvariantCulture));
			}
			this._fileSize = fileSize;
			if (isFree)
			{
				this._freeExtents = new List<StreamExtent>(StreamExtent.Union(this._freeExtents, new StreamExtent(this._fileSize, fileSize - this._fileSize)));
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000485F File Offset: 0x00002A5F
		public void Release(long start, long length)
		{
			this.ValidateRange(start, length, "release");
			this._freeExtents = new List<StreamExtent>(StreamExtent.Union(this._freeExtents, new StreamExtent(start, length)));
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000488B File Offset: 0x00002A8B
		public void Reserve(long start, long length)
		{
			this.ValidateRange(start, length, "reserve");
			this._freeExtents = new List<StreamExtent>(StreamExtent.Subtract(this._freeExtents, new StreamExtent(start, length)));
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000048B7 File Offset: 0x00002AB7
		public void Reserve(IEnumerable<StreamExtent> extents)
		{
			this._freeExtents = new List<StreamExtent>(StreamExtent.Subtract(this._freeExtents, extents));
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000048D0 File Offset: 0x00002AD0
		public bool TryAllocate(long length, out long start)
		{
			if (length % 1048576L != 0L)
			{
				throw new ArgumentException("VHDX free space must be managed on 1MB boundaries", "length");
			}
			for (int i = 0; i < this._freeExtents.Count; i++)
			{
				StreamExtent streamExtent = this._freeExtents[i];
				if (streamExtent.Length == length)
				{
					this._freeExtents.RemoveAt(i);
					start = streamExtent.Start;
					return true;
				}
				if (streamExtent.Length > length)
				{
					this._freeExtents[i] = new StreamExtent(streamExtent.Start + length, streamExtent.Length - length);
					start = streamExtent.Start;
					return true;
				}
			}
			start = 0L;
			return false;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004974 File Offset: 0x00002B74
		private void ValidateRange(long start, long length, string method)
		{
			if (start % 1048576L != 0L)
			{
				throw new ArgumentException("VHDX free space must be managed on 1MB boundaries", "start");
			}
			if (length % 1048576L != 0L)
			{
				throw new ArgumentException("VHDX free space must be managed on 1MB boundaries", "length");
			}
			if (start < 0L || start > this._fileSize || length > this._fileSize - start)
			{
				throw new ArgumentOutOfRangeException("Attempt to " + method + " space outside of file range");
			}
		}

		// Token: 0x04000041 RID: 65
		private long _fileSize;

		// Token: 0x04000042 RID: 66
		private List<StreamExtent> _freeExtents;
	}
}
