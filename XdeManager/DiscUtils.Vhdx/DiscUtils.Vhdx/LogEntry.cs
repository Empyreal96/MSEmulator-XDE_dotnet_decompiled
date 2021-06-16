using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000012 RID: 18
	internal sealed class LogEntry
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00004A97 File Offset: 0x00002C97
		private LogEntry(long position, LogEntryHeader header, List<LogEntry.Descriptor> descriptors)
		{
			this.Position = position;
			this._header = header;
			this._descriptors = descriptors;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004ABF File Offset: 0x00002CBF
		public ulong FlushedFileOffset
		{
			get
			{
				return this._header.FlushedFileOffset;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004ACC File Offset: 0x00002CCC
		public bool IsEmpty
		{
			get
			{
				return this._descriptors.Count == 0;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004ADC File Offset: 0x00002CDC
		public ulong LastFileOffset
		{
			get
			{
				return this._header.LastFileOffset;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004AE9 File Offset: 0x00002CE9
		public Guid LogGuid
		{
			get
			{
				return this._header.LogGuid;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004AF6 File Offset: 0x00002CF6
		public IEnumerable<Range<ulong, ulong>> ModifiedExtents
		{
			get
			{
				foreach (LogEntry.Descriptor descriptor in this._descriptors)
				{
					yield return new Range<ulong, ulong>(descriptor.FileOffset, descriptor.FileLength);
				}
				List<LogEntry.Descriptor>.Enumerator enumerator = default(List<LogEntry.Descriptor>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004B06 File Offset: 0x00002D06
		public long Position { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004B0E File Offset: 0x00002D0E
		public ulong SequenceNumber
		{
			get
			{
				return this._header.SequenceNumber;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004B1B File Offset: 0x00002D1B
		public uint Tail
		{
			get
			{
				return this._header.Tail;
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004B28 File Offset: 0x00002D28
		public void Replay(Stream target)
		{
			if (this.IsEmpty)
			{
				return;
			}
			foreach (LogEntry.Descriptor descriptor in this._descriptors)
			{
				descriptor.WriteData(target);
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004B84 File Offset: 0x00002D84
		public static bool TryRead(Stream logStream, out LogEntry entry)
		{
			long position = logStream.Position;
			byte[] array = new byte[4096];
			if (StreamUtilities.ReadMaximum(logStream, array, 0, array.Length) != array.Length)
			{
				entry = null;
				return false;
			}
			if (EndianUtilities.ToUInt32LittleEndian(array, 0) != 1701277548U)
			{
				entry = null;
				return false;
			}
			LogEntryHeader logEntryHeader = new LogEntryHeader();
			logEntryHeader.ReadFrom(array, 0);
			if (!logEntryHeader.IsValid || (ulong)logEntryHeader.EntryLength > (ulong)logStream.Length)
			{
				entry = null;
				return false;
			}
			byte[] array2 = new byte[logEntryHeader.EntryLength];
			Array.Copy(array, array2, 4096);
			StreamUtilities.ReadExact(logStream, array2, 4096, array2.Length - 4096);
			EndianUtilities.WriteBytesLittleEndian(0, array2, 4);
			if (logEntryHeader.Checksum != Crc32LittleEndian.Compute(Crc32Algorithm.Castagnoli, array2, 0, (int)logEntryHeader.EntryLength))
			{
				entry = null;
				return false;
			}
			int num = MathUtilities.RoundUp((int)(logEntryHeader.DescriptorCount * 32U + 64U), 4096);
			List<LogEntry.Descriptor> list = new List<LogEntry.Descriptor>();
			int num2 = 0;
			while ((long)num2 < (long)((ulong)logEntryHeader.DescriptorCount))
			{
				int offset = num2 * 32 + 64;
				uint num3 = EndianUtilities.ToUInt32LittleEndian(array2, offset);
				LogEntry.Descriptor descriptor;
				if (num3 != 1668506980U)
				{
					if (num3 != 1869768058U)
					{
						entry = null;
						return false;
					}
					descriptor = new LogEntry.ZeroDescriptor();
				}
				else
				{
					descriptor = new LogEntry.DataDescriptor(array2, num);
					num += 4096;
				}
				descriptor.ReadFrom(array2, offset);
				if (!descriptor.IsValid(logEntryHeader.SequenceNumber))
				{
					entry = null;
					return false;
				}
				list.Add(descriptor);
				num2++;
			}
			entry = new LogEntry(position, logEntryHeader, list);
			return true;
		}

		// Token: 0x04000044 RID: 68
		public const int LogSectorSize = 4096;

		// Token: 0x04000045 RID: 69
		private readonly List<LogEntry.Descriptor> _descriptors = new List<LogEntry.Descriptor>();

		// Token: 0x04000046 RID: 70
		private readonly LogEntryHeader _header;

		// Token: 0x0200002B RID: 43
		private abstract class Descriptor : IByteArraySerializable
		{
			// Token: 0x17000082 RID: 130
			// (get) Token: 0x06000156 RID: 342
			public abstract ulong FileLength { get; }

			// Token: 0x17000083 RID: 131
			// (get) Token: 0x06000157 RID: 343 RVA: 0x00006E1F File Offset: 0x0000501F
			public int Size
			{
				get
				{
					return 32;
				}
			}

			// Token: 0x06000158 RID: 344
			public abstract int ReadFrom(byte[] buffer, int offset);

			// Token: 0x06000159 RID: 345
			public abstract void WriteTo(byte[] buffer, int offset);

			// Token: 0x0600015A RID: 346
			public abstract bool IsValid(ulong sequenceNumber);

			// Token: 0x0600015B RID: 347
			public abstract void WriteData(Stream target);

			// Token: 0x040000C6 RID: 198
			public const uint ZeroDescriptorSignature = 1869768058U;

			// Token: 0x040000C7 RID: 199
			public const uint DataDescriptorSignature = 1668506980U;

			// Token: 0x040000C8 RID: 200
			public const uint DataSectorSignature = 1635017060U;

			// Token: 0x040000C9 RID: 201
			public ulong FileOffset;

			// Token: 0x040000CA RID: 202
			public ulong SequenceNumber;
		}

		// Token: 0x0200002C RID: 44
		private sealed class ZeroDescriptor : LogEntry.Descriptor
		{
			// Token: 0x17000084 RID: 132
			// (get) Token: 0x0600015D RID: 349 RVA: 0x00006E2B File Offset: 0x0000502B
			public override ulong FileLength
			{
				get
				{
					return this.ZeroLength;
				}
			}

			// Token: 0x0600015E RID: 350 RVA: 0x00006E33 File Offset: 0x00005033
			public override int ReadFrom(byte[] buffer, int offset)
			{
				this.ZeroLength = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 8);
				this.FileOffset = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 16);
				this.SequenceNumber = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 24);
				return 32;
			}

			// Token: 0x0600015F RID: 351 RVA: 0x00006E66 File Offset: 0x00005066
			public override void WriteTo(byte[] buffer, int offset)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000160 RID: 352 RVA: 0x00006E6D File Offset: 0x0000506D
			public override bool IsValid(ulong sequenceNumber)
			{
				return this.SequenceNumber == sequenceNumber;
			}

			// Token: 0x06000161 RID: 353 RVA: 0x00006E78 File Offset: 0x00005078
			public override void WriteData(Stream target)
			{
				target.Seek((long)this.FileOffset, SeekOrigin.Begin);
				byte[] array = new byte[4096L];
				int num2;
				for (ulong num = this.ZeroLength; num > 0UL; num -= (ulong)num2)
				{
					num2 = array.Length;
					if (num < (ulong)num2)
					{
						num2 = (int)num;
					}
					target.Write(array, 0, num2);
				}
			}

			// Token: 0x040000CB RID: 203
			public ulong ZeroLength;
		}

		// Token: 0x0200002D RID: 45
		private sealed class DataDescriptor : LogEntry.Descriptor
		{
			// Token: 0x06000163 RID: 355 RVA: 0x00006ED0 File Offset: 0x000050D0
			public DataDescriptor(byte[] data, int offset)
			{
				this._data = data;
				this._offset = offset;
			}

			// Token: 0x17000085 RID: 133
			// (get) Token: 0x06000164 RID: 356 RVA: 0x00006EE6 File Offset: 0x000050E6
			public override ulong FileLength
			{
				get
				{
					return 4096UL;
				}
			}

			// Token: 0x06000165 RID: 357 RVA: 0x00006EF0 File Offset: 0x000050F0
			public override int ReadFrom(byte[] buffer, int offset)
			{
				this.TrailingBytes = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 4);
				this.LeadingBytes = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 8);
				this.FileOffset = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 16);
				this.SequenceNumber = EndianUtilities.ToUInt64LittleEndian(buffer, offset + 24);
				this.DataSignature = EndianUtilities.ToUInt32LittleEndian(this._data, this._offset);
				return 32;
			}

			// Token: 0x06000166 RID: 358 RVA: 0x00006F54 File Offset: 0x00005154
			public override void WriteTo(byte[] buffer, int offset)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06000167 RID: 359 RVA: 0x00006F5C File Offset: 0x0000515C
			public override bool IsValid(ulong sequenceNumber)
			{
				return this.SequenceNumber == sequenceNumber && this._offset + 4096 <= this._data.Length && (ulong)EndianUtilities.ToUInt32LittleEndian(this._data, this._offset + 4096 - 4) == (sequenceNumber & (ulong)-1) && (ulong)EndianUtilities.ToUInt32LittleEndian(this._data, this._offset + 4) == (sequenceNumber >> 32 & (ulong)-1) && this.DataSignature == 1635017060U;
			}

			// Token: 0x06000168 RID: 360 RVA: 0x00006FD4 File Offset: 0x000051D4
			public override void WriteData(Stream target)
			{
				target.Seek((long)this.FileOffset, SeekOrigin.Begin);
				byte[] array = new byte[8];
				EndianUtilities.WriteBytesLittleEndian(this.LeadingBytes, array, 0);
				byte[] array2 = new byte[4];
				EndianUtilities.WriteBytesLittleEndian(this.TrailingBytes, array2, 0);
				target.Write(array, 0, array.Length);
				target.Write(this._data, this._offset + 8, 4084);
				target.Write(array2, 0, array2.Length);
			}

			// Token: 0x040000CC RID: 204
			private readonly byte[] _data;

			// Token: 0x040000CD RID: 205
			private readonly int _offset;

			// Token: 0x040000CE RID: 206
			public ulong LeadingBytes;

			// Token: 0x040000CF RID: 207
			public uint TrailingBytes;

			// Token: 0x040000D0 RID: 208
			public uint DataSignature;
		}
	}
}
