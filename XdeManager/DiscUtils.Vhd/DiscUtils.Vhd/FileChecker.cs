using System;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Vhd
{
	// Token: 0x0200000B RID: 11
	public class FileChecker
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00004A2B File Offset: 0x00002C2B
		public FileChecker(Stream stream)
		{
			this._fileStream = stream;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004A44 File Offset: 0x00002C44
		public bool Check(TextWriter reportOutput, ReportLevels levels)
		{
			this._report = reportOutput;
			this._reportLevels = levels;
			this._levelsDetected = ReportLevels.None;
			try
			{
				this.DoCheck();
			}
			catch (FileChecker.AbortException arg)
			{
				this.ReportError("File system check aborted: " + arg, new object[0]);
				return false;
			}
			catch (Exception arg2)
			{
				this.ReportError("File system check aborted with exception: " + arg2, new object[0]);
				return false;
			}
			return (this._levelsDetected & this._levelsConsideredFail) == ReportLevels.None;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004AD4 File Offset: 0x00002CD4
		private static void Abort()
		{
			throw new FileChecker.AbortException();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004ADC File Offset: 0x00002CDC
		private void DoCheck()
		{
			this.CheckFooter();
			if (this._footer == null || this._footer.DiskType != FileType.Fixed)
			{
				this.CheckHeader();
			}
			if (this._footer == null)
			{
				this.ReportError("Unable to continue - no valid header or footer", new object[0]);
				FileChecker.Abort();
			}
			this.CheckFooterFields();
			if (this._footer.DiskType != FileType.Fixed)
			{
				this.CheckDynamicHeader();
				this.CheckBat();
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004B4C File Offset: 0x00002D4C
		private void CheckBat()
		{
			int num = MathUtilities.RoundUp(this._dynamicHeader.MaxTableEntries * 4, 512);
			if (this._dynamicHeader.TableOffset > this._fileStream.Length - (long)num)
			{
				this.ReportError("BAT: BAT extends beyond end of file", new object[0]);
				return;
			}
			this._fileStream.Position = this._dynamicHeader.TableOffset;
			byte[] buffer = StreamUtilities.ReadExact(this._fileStream, num);
			uint[] array = new uint[num / 4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = EndianUtilities.ToUInt32BigEndian(buffer, i * 4);
			}
			for (int j = this._dynamicHeader.MaxTableEntries; j < array.Length; j++)
			{
				if (array[j] != 4294967295U)
				{
					this.ReportError("BAT: Padding record '" + j + "' should be 0xFFFFFFFF", new object[0]);
				}
			}
			uint num2 = uint.MaxValue;
			for (int k = 0; k < this._dynamicHeader.MaxTableEntries; k++)
			{
				if (array[k] < num2)
				{
					num2 = array[k];
				}
			}
			if (num2 == 4294967295U)
			{
				return;
			}
			long num3 = (long)((ulong)num2 * 512UL);
			uint num4 = (uint)MathUtilities.RoundUp((long)((ulong)(this._dynamicHeader.BlockSize / 512U / 8U)), 512L);
			uint num5 = this._dynamicHeader.BlockSize + num4;
			bool[] array2 = new bool[this._dynamicHeader.MaxTableEntries];
			for (int l = 0; l < this._dynamicHeader.MaxTableEntries; l++)
			{
				if (array[l] != 4294967295U)
				{
					long num6 = (long)((ulong)array[l] * 512UL);
					if (num6 + (long)((ulong)num5) > this._fileStream.Length)
					{
						this.ReportError("BAT: block stored beyond end of stream", new object[0]);
					}
					if ((num6 - num3) % (long)((ulong)num5) != 0L)
					{
						this.ReportError("BAT: block stored at invalid start sector (not a multiple of size of a stored block)", new object[0]);
					}
					uint num7 = (uint)((num6 - num3) / (long)((ulong)num5));
					if (array2[(int)num7])
					{
						this.ReportError("BAT: multiple blocks occupying same file space", new object[0]);
					}
					array2[(int)num7] = true;
				}
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004D4C File Offset: 0x00002F4C
		private void CheckDynamicHeader()
		{
			long num = this._footer.DataOffset + 512L;
			Header header;
			for (long dataOffset = this._footer.DataOffset; dataOffset != -1L; dataOffset = header.DataOffset)
			{
				if (dataOffset % 512L != 0L)
				{
					this.ReportError("DynHeader: Unaligned header @{0}", new object[]
					{
						dataOffset
					});
				}
				this._fileStream.Position = dataOffset;
				header = Header.FromStream(this._fileStream);
				if (header.Cookie == "cxsparse")
				{
					if (this._dynamicHeader != null)
					{
						this.ReportError("DynHeader: Duplicate dynamic header found", new object[0]);
					}
					this._fileStream.Position = dataOffset;
					this._dynamicHeader = DynamicHeader.FromStream(this._fileStream);
					if (dataOffset + 1024L > num)
					{
						num = dataOffset + 1024L;
					}
				}
				else
				{
					this.ReportWarning("DynHeader: Undocumented header found, with cookie '" + header.Cookie + "'", new object[0]);
					if (dataOffset + 512L > num)
					{
						num = dataOffset + 1024L;
					}
				}
			}
			if (this._dynamicHeader == null)
			{
				this.ReportError("DynHeader: No dynamic header found", new object[0]);
				return;
			}
			if (this._dynamicHeader.TableOffset < num)
			{
				this.ReportError("DynHeader: BAT offset is before last header", new object[0]);
			}
			if (this._dynamicHeader.TableOffset % 512L != 0L)
			{
				this.ReportError("DynHeader: BAT offset is not sector aligned", new object[0]);
			}
			if (this._dynamicHeader.HeaderVersion != 65536U)
			{
				this.ReportError("DynHeader: Unrecognized header version", new object[0]);
			}
			if ((long)this._dynamicHeader.MaxTableEntries != MathUtilities.Ceil(this._footer.CurrentSize, (long)((ulong)this._dynamicHeader.BlockSize)))
			{
				this.ReportError("DynHeader: Max table entries is invalid", new object[0]);
			}
			if ((ulong)this._dynamicHeader.BlockSize != 2097152UL && (ulong)this._dynamicHeader.BlockSize != 524288UL)
			{
				this.ReportWarning("DynHeader: Using non-standard block size '" + this._dynamicHeader.BlockSize + "'", new object[0]);
			}
			if (!Utilities.IsPowerOfTwo(this._dynamicHeader.BlockSize))
			{
				this.ReportError("DynHeader: Block size is not a power of 2", new object[0]);
			}
			if (!this._dynamicHeader.IsChecksumValid())
			{
				this.ReportError("DynHeader: Invalid checksum", new object[0]);
			}
			if (this._footer.DiskType == FileType.Dynamic && this._dynamicHeader.ParentUniqueId != Guid.Empty)
			{
				this.ReportWarning("DynHeader: Parent Id is not null for dynamic disk", new object[0]);
			}
			else if (this._footer.DiskType == FileType.Differencing && this._dynamicHeader.ParentUniqueId == Guid.Empty)
			{
				this.ReportError("DynHeader: Parent Id is null for differencing disk", new object[0]);
			}
			if (this._footer.DiskType == FileType.Differencing && this._dynamicHeader.ParentTimestamp > DateTime.UtcNow)
			{
				this.ReportWarning("DynHeader: Parent timestamp is greater than current time", new object[0]);
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005050 File Offset: 0x00003250
		private void CheckFooterFields()
		{
			if (this._footer.Cookie != "conectix")
			{
				this.ReportError("Footer: Invalid VHD cookie - should be 'connectix'", new object[0]);
			}
			if (((ulong)this._footer.Features & 18446744073709551614UL) != 2UL)
			{
				this.ReportError("Footer: Invalid VHD features - should be 0x2 or 0x3", new object[0]);
			}
			if (this._footer.FileFormatVersion != 65536U)
			{
				this.ReportError("Footer: Unrecognized VHD file version", new object[0]);
			}
			if (this._footer.DiskType == FileType.Fixed && this._footer.DataOffset != -1L)
			{
				this.ReportError("Footer: Invalid data offset - should be 0xFFFFFFFF for fixed disks", new object[0]);
			}
			else if (this._footer.DiskType != FileType.Fixed && (this._footer.DataOffset == 0L || this._footer.DataOffset == -1L))
			{
				this.ReportError("Footer: Invalid data offset - should not be 0x0 or 0xFFFFFFFF for non-fixed disks", new object[0]);
			}
			if (this._footer.Timestamp > DateTime.UtcNow)
			{
				this.ReportError("Footer: Invalid timestamp - creation time in file is greater than current time", new object[0]);
			}
			if (this._footer.CreatorHostOS != "Wi2k" && this._footer.CreatorHostOS != "Mac ")
			{
				this.ReportWarning("Footer: Creator Host OS is not a documented value ('Wi2K' or 'Mac '), is '" + this._footer.CreatorHostOS + "'", new object[0]);
			}
			if (this._footer.OriginalSize != this._footer.CurrentSize)
			{
				this.ReportInfo("Footer: Current size of the disk doesn't match the original size", new object[0]);
			}
			if (this._footer.CurrentSize == 0L)
			{
				this.ReportError("Footer: Current size of the disk is 0 bytes", new object[0]);
			}
			if (!this._footer.Geometry.Equals(Geometry.FromCapacity(this._footer.CurrentSize)))
			{
				this.ReportWarning("Footer: Disk Geometry does not match documented Microsoft geometry for this capacity", new object[0]);
			}
			if (this._footer.DiskType != FileType.Fixed && this._footer.DiskType != FileType.Dynamic && this._footer.DiskType != FileType.Differencing)
			{
				this.ReportError("Footer: Undocumented disk type, not Fixed, Dynamic or Differencing", new object[0]);
			}
			if (!this._footer.IsChecksumValid())
			{
				this.ReportError("Footer: Invalid footer checksum", new object[0]);
			}
			if (this._footer.UniqueId == Guid.Empty)
			{
				this.ReportWarning("Footer: Unique Id is null", new object[0]);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000052B0 File Offset: 0x000034B0
		private void CheckFooter()
		{
			this._fileStream.Position = this._fileStream.Length - 512L;
			byte[] buffer = StreamUtilities.ReadExact(this._fileStream, 512);
			this._footer = Footer.FromBytes(buffer, 0);
			if (!this._footer.IsValid())
			{
				this.ReportError("Invalid VHD footer at end of file", new object[0]);
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005318 File Offset: 0x00003518
		private void CheckHeader()
		{
			this._fileStream.Position = 0L;
			byte[] array = StreamUtilities.ReadExact(this._fileStream, 512);
			Footer footer = Footer.FromBytes(array, 0);
			if (!footer.IsValid())
			{
				this.ReportError("Invalid VHD footer at start of file", new object[0]);
			}
			this._fileStream.Position = this._fileStream.Length - 512L;
			if (!Utilities.AreEqual(StreamUtilities.ReadExact(this._fileStream, 512), array))
			{
				this.ReportError("Header and footer are different", new object[0]);
			}
			if (this._footer == null || !this._footer.IsValid())
			{
				this._footer = footer;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000053C6 File Offset: 0x000035C6
		private void ReportInfo(string str, params object[] args)
		{
			this._levelsDetected |= ReportLevels.Information;
			if ((this._reportLevels & ReportLevels.Information) != ReportLevels.None)
			{
				this._report.WriteLine("INFO: " + str, args);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000053F7 File Offset: 0x000035F7
		private void ReportWarning(string str, params object[] args)
		{
			this._levelsDetected |= ReportLevels.Warnings;
			if ((this._reportLevels & ReportLevels.Warnings) != ReportLevels.None)
			{
				this._report.WriteLine("WARNING: " + str, args);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005428 File Offset: 0x00003628
		private void ReportError(string str, params object[] args)
		{
			this._levelsDetected |= ReportLevels.Errors;
			if ((this._reportLevels & ReportLevels.Errors) != ReportLevels.None)
			{
				this._report.WriteLine("ERROR: " + str, args);
			}
		}

		// Token: 0x0400002D RID: 45
		private readonly Stream _fileStream;

		// Token: 0x0400002E RID: 46
		private Footer _footer;

		// Token: 0x0400002F RID: 47
		private DynamicHeader _dynamicHeader;

		// Token: 0x04000030 RID: 48
		private TextWriter _report;

		// Token: 0x04000031 RID: 49
		private ReportLevels _reportLevels;

		// Token: 0x04000032 RID: 50
		private ReportLevels _levelsDetected;

		// Token: 0x04000033 RID: 51
		private readonly ReportLevels _levelsConsideredFail = ReportLevels.Errors;

		// Token: 0x02000016 RID: 22
		[Serializable]
		private sealed class AbortException : InvalidFileSystemException
		{
		}
	}
}
