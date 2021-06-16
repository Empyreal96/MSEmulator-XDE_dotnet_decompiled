using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000034 RID: 52
	internal class MasterFileTable : IDiagnosticTraceable, IDisposable
	{
		// Token: 0x06000204 RID: 516 RVA: 0x0000AA30 File Offset: 0x00008C30
		public MasterFileTable(INtfsContext context)
		{
			BiosParameterBlock biosParameterBlock = context.BiosParameterBlock;
			this._recordCache = new ObjectCache<long, FileRecord>();
			this.RecordSize = biosParameterBlock.MftRecordSize;
			this._bytesPerSector = (int)biosParameterBlock.BytesPerSector;
			this._recordStream = new SubStream(context.RawStream, biosParameterBlock.MftCluster * (long)((ulong)biosParameterBlock.SectorsPerCluster) * (long)((ulong)biosParameterBlock.BytesPerSector), (long)(24 * this.RecordSize));
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000AA9E File Offset: 0x00008C9E
		public IEnumerable<FileRecord> Records
		{
			get
			{
				using (Stream mftStream = this._self.OpenStream(AttributeType.Data, null, FileAccess.Read))
				{
					uint index = 0U;
					while (mftStream.Position < mftStream.Length)
					{
						byte[] array = StreamUtilities.ReadExact(mftStream, this.RecordSize);
						if (!(EndianUtilities.BytesToString(array, 0, 4) != "FILE"))
						{
							FileRecord fileRecord = new FileRecord(this._bytesPerSector);
							fileRecord.FromBytes(array, 0);
							fileRecord.LoadedIndex = index;
							yield return fileRecord;
							uint num = index;
							index = num + 1U;
						}
					}
				}
				Stream mftStream = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000AAAE File Offset: 0x00008CAE
		// (set) Token: 0x06000207 RID: 519 RVA: 0x0000AAB6 File Offset: 0x00008CB6
		public int RecordSize { get; private set; }

		// Token: 0x06000208 RID: 520 RVA: 0x0000AAC0 File Offset: 0x00008CC0
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "MASTER FILE TABLE");
			writer.WriteLine(indent + "  Record Length: " + this.RecordSize);
			foreach (FileRecord fileRecord in this.Records)
			{
				fileRecord.Dump(writer, indent + "  ");
				foreach (AttributeRecord attributeRecord in fileRecord.Attributes)
				{
					attributeRecord.Dump(writer, indent + "     ");
				}
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000AB90 File Offset: 0x00008D90
		public void Dispose()
		{
			if (this._recordStream != null)
			{
				this._recordStream.Dispose();
				this._recordStream = null;
			}
			if (this._bitmap != null)
			{
				this._bitmap.Dispose();
				this._bitmap = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000ABCC File Offset: 0x00008DCC
		public FileRecord GetBootstrapRecord()
		{
			this._recordStream.Position = 0L;
			byte[] buffer = StreamUtilities.ReadExact(this._recordStream, this.RecordSize);
			FileRecord fileRecord = new FileRecord(this._bytesPerSector);
			fileRecord.FromBytes(buffer, 0);
			this._recordCache[0L] = fileRecord;
			return fileRecord;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000AC1C File Offset: 0x00008E1C
		public void Initialize(File file)
		{
			this._self = file;
			if (this._recordStream != null)
			{
				this._recordStream.Dispose();
			}
			NtfsStream stream = this._self.GetStream(AttributeType.Bitmap, null);
			this._bitmap = new Bitmap(stream.Open(FileAccess.ReadWrite), long.MaxValue);
			NtfsStream stream2 = this._self.GetStream(AttributeType.Data, null);
			this._recordStream = stream2.Open(FileAccess.ReadWrite);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000AC90 File Offset: 0x00008E90
		public File InitializeNew(INtfsContext context, long firstBitmapCluster, ulong numBitmapClusters, long firstRecordsCluster, ulong numRecordsClusters)
		{
			BiosParameterBlock biosParameterBlock = context.BiosParameterBlock;
			FileRecord fileRecord = new FileRecord((int)biosParameterBlock.BytesPerSector, biosParameterBlock.MftRecordSize, 0U);
			fileRecord.Flags = FileRecordFlags.InUse;
			fileRecord.SequenceNumber = 1;
			this._recordCache[0L] = fileRecord;
			this._self = new File(context, fileRecord);
			StandardInformation.InitializeNewFile(this._self, FileAttributeFlags.Hidden | FileAttributeFlags.System);
			NtfsStream ntfsStream = this._self.CreateStream(AttributeType.Data, null, firstRecordsCluster, numRecordsClusters, (uint)biosParameterBlock.BytesPerCluster);
			this._recordStream = ntfsStream.Open(FileAccess.ReadWrite);
			MasterFileTable.Wipe(this._recordStream);
			using (Stream stream = this._self.CreateStream(AttributeType.Bitmap, null, firstBitmapCluster, numBitmapClusters, (uint)biosParameterBlock.BytesPerCluster).Open(FileAccess.ReadWrite))
			{
				MasterFileTable.Wipe(stream);
				stream.SetLength(8L);
				this._bitmap = new Bitmap(stream, long.MaxValue);
			}
			this.RecordSize = context.BiosParameterBlock.MftRecordSize;
			this._bytesPerSector = (int)context.BiosParameterBlock.BytesPerSector;
			this._bitmap.MarkPresentRange(0L, 1L);
			byte[] buffer = new byte[this.RecordSize];
			fileRecord.ToBytes(buffer, 0);
			this._recordStream.Position = 0L;
			this._recordStream.Write(buffer, 0, this.RecordSize);
			this._recordStream.Flush();
			return this._self;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000ADFC File Offset: 0x00008FFC
		public FileRecord AllocateRecord(FileRecordFlags flags, bool isMft)
		{
			if (isMft)
			{
				for (int i = 15; i > 11; i--)
				{
					FileRecord record = this.GetRecord((long)i, false);
					if (record.BaseFile.SequenceNumber == 0)
					{
						record.Reset();
						record.Flags |= FileRecordFlags.InUse;
						this.WriteRecord(record);
						return record;
					}
				}
				throw new IOException("MFT too fragmented - unable to allocate MFT overflow record");
			}
			long num = this._bitmap.AllocateFirstAvailable(24L);
			if (num * (long)this.RecordSize >= this._recordStream.Length)
			{
				long num2 = MathUtilities.RoundUp(num + 1L, 64L);
				this._recordStream.SetLength(num2 * (long)this.RecordSize);
				for (long num3 = num; num3 < num2; num3 += 1L)
				{
					FileRecord record2 = new FileRecord(this._bytesPerSector, this.RecordSize, (uint)num3);
					this.WriteRecord(record2);
				}
			}
			FileRecord record3 = this.GetRecord(num, true);
			record3.ReInitialize(this._bytesPerSector, this.RecordSize, (uint)num);
			this._recordCache[num] = record3;
			record3.Flags = (FileRecordFlags.InUse | flags);
			this.WriteRecord(record3);
			this._self.UpdateRecordInMft();
			return record3;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000AF1C File Offset: 0x0000911C
		public FileRecord AllocateRecord(long index, FileRecordFlags flags)
		{
			this._bitmap.MarkPresent(index);
			FileRecord fileRecord = new FileRecord(this._bytesPerSector, this.RecordSize, (uint)index);
			this._recordCache[index] = fileRecord;
			fileRecord.Flags = (FileRecordFlags.InUse | flags);
			this.WriteRecord(fileRecord);
			this._self.UpdateRecordInMft();
			return fileRecord;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000AF74 File Offset: 0x00009174
		public void RemoveRecord(FileRecordReference fileRef)
		{
			FileRecord record = this.GetRecord(fileRef.MftIndex, false);
			record.Reset();
			this.WriteRecord(record);
			this._recordCache.Remove(fileRef.MftIndex);
			this._bitmap.MarkAbsent(fileRef.MftIndex);
			this._self.UpdateRecordInMft();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000AFCC File Offset: 0x000091CC
		public FileRecord GetRecord(FileRecordReference fileReference)
		{
			FileRecord record = this.GetRecord(fileReference.MftIndex, false);
			if (record != null && fileReference.SequenceNumber != 0 && record.SequenceNumber != 0 && fileReference.SequenceNumber != record.SequenceNumber)
			{
				throw new IOException("Attempt to get an MFT record with an old reference");
			}
			return record;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000B017 File Offset: 0x00009217
		public FileRecord GetRecord(long index, bool ignoreMagic)
		{
			return this.GetRecord(index, ignoreMagic, false);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000B024 File Offset: 0x00009224
		public FileRecord GetRecord(long index, bool ignoreMagic, bool ignoreBitmap)
		{
			if (!ignoreBitmap && this._bitmap != null && !this._bitmap.IsPresent(index))
			{
				return null;
			}
			FileRecord fileRecord = this._recordCache[index];
			if (fileRecord != null)
			{
				return fileRecord;
			}
			if ((index + 1L) * (long)this.RecordSize <= this._recordStream.Length)
			{
				this._recordStream.Position = index * (long)this.RecordSize;
				byte[] buffer = StreamUtilities.ReadExact(this._recordStream, this.RecordSize);
				fileRecord = new FileRecord(this._bytesPerSector);
				fileRecord.FromBytes(buffer, 0, ignoreMagic);
				fileRecord.LoadedIndex = (uint)index;
			}
			else
			{
				fileRecord = new FileRecord(this._bytesPerSector, this.RecordSize, (uint)index);
			}
			this._recordCache[index] = fileRecord;
			return fileRecord;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000B0E4 File Offset: 0x000092E4
		public void WriteRecord(FileRecord record)
		{
			if (record.Size > this.RecordSize)
			{
				throw new IOException("Attempting to write over-sized MFT record");
			}
			byte[] buffer = new byte[this.RecordSize];
			record.ToBytes(buffer, 0);
			this._recordStream.Position = (long)((ulong)record.MasterFileTableIndex * (ulong)((long)this.RecordSize));
			this._recordStream.Write(buffer, 0, this.RecordSize);
			this._recordStream.Flush();
			if (this._self.MftRecordIsDirty)
			{
				DirectoryEntry directoryEntry = this._self.DirectoryEntry;
				if (directoryEntry != null)
				{
					directoryEntry.UpdateFrom(this._self);
				}
				this._self.UpdateRecordInMft();
			}
			if (record.MasterFileTableIndex < 4U && this._self.Context.GetFileByIndex != null)
			{
				File file = this._self.Context.GetFileByIndex(1L);
				if (file != null)
				{
					using (Stream stream = file.OpenStream(AttributeType.Data, null, FileAccess.ReadWrite))
					{
						stream.Position = (long)((ulong)record.MasterFileTableIndex * (ulong)((long)this.RecordSize));
						stream.Write(buffer, 0, this.RecordSize);
					}
				}
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000B20C File Offset: 0x0000940C
		public long GetRecordOffset(FileRecordReference fileReference)
		{
			return fileReference.MftIndex * (long)this.RecordSize;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000B220 File Offset: 0x00009420
		public ClusterMap GetClusterMap()
		{
			int num = (int)MathUtilities.Ceil(this._self.Context.BiosParameterBlock.TotalSectors64, (long)((ulong)this._self.Context.BiosParameterBlock.SectorsPerCluster));
			ClusterRoles[] array = new ClusterRoles[num];
			object[] array2 = new object[num];
			Dictionary<object, string[]> dictionary = new Dictionary<object, string[]>();
			for (int i = 0; i < num; i++)
			{
				array[i] = ClusterRoles.Free;
			}
			foreach (FileRecord fileRecord in this.Records)
			{
				if (fileRecord.BaseFile.Value == 0UL && (fileRecord.Flags & FileRecordFlags.InUse) != FileRecordFlags.None)
				{
					File file = new File(this._self.Context, fileRecord);
					using (IEnumerator<NtfsStream> enumerator2 = file.AllStreams.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							NtfsStream stream = enumerator2.Current;
							string text;
							if (stream.AttributeType == AttributeType.Data && !string.IsNullOrEmpty(stream.Name))
							{
								text = file.IndexInMft.ToString(CultureInfo.InvariantCulture) + ":" + stream.Name;
								dictionary[text] = Utilities.Map<string, string>(file.Names, (string n) => n + ":" + stream.Name);
							}
							else
							{
								text = file.IndexInMft.ToString(CultureInfo.InvariantCulture);
								dictionary[text] = file.Names.ToArray();
							}
							ClusterRoles clusterRoles = ClusterRoles.None;
							if (file.IndexInMft < 24U)
							{
								clusterRoles |= ClusterRoles.SystemFile;
								if ((ulong)file.IndexInMft == 7UL)
								{
									clusterRoles |= ClusterRoles.BootArea;
								}
							}
							else
							{
								clusterRoles |= ClusterRoles.DataFile;
							}
							if (stream.AttributeType != AttributeType.Data)
							{
								clusterRoles |= ClusterRoles.Metadata;
							}
							foreach (Range<long, long> range in stream.GetClusters())
							{
								for (long num2 = range.Offset; num2 < range.Offset + range.Count; num2 += 1L)
								{
									checked
									{
										array[(int)((IntPtr)num2)] = clusterRoles;
										array2[(int)((IntPtr)num2)] = text;
									}
								}
							}
						}
					}
				}
			}
			return new ClusterMap(array, array2, dictionary);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000B4B8 File Offset: 0x000096B8
		private static void Wipe(Stream s)
		{
			s.Position = 0L;
			byte[] array = new byte[65536L];
			int num = 0;
			while ((long)num < s.Length)
			{
				int num2 = (int)Math.Min((long)array.Length, s.Length - s.Position);
				s.Write(array, 0, num2);
				num += num2;
			}
		}

		// Token: 0x040000EF RID: 239
		public const long MftIndex = 0L;

		// Token: 0x040000F0 RID: 240
		public const long MftMirrorIndex = 1L;

		// Token: 0x040000F1 RID: 241
		public const long LogFileIndex = 2L;

		// Token: 0x040000F2 RID: 242
		public const long VolumeIndex = 3L;

		// Token: 0x040000F3 RID: 243
		public const long AttrDefIndex = 4L;

		// Token: 0x040000F4 RID: 244
		public const long RootDirIndex = 5L;

		// Token: 0x040000F5 RID: 245
		public const long BitmapIndex = 6L;

		// Token: 0x040000F6 RID: 246
		public const long BootIndex = 7L;

		// Token: 0x040000F7 RID: 247
		public const long BadClusIndex = 8L;

		// Token: 0x040000F8 RID: 248
		public const long SecureIndex = 9L;

		// Token: 0x040000F9 RID: 249
		public const long UpCaseIndex = 10L;

		// Token: 0x040000FA RID: 250
		public const long ExtendIndex = 11L;

		// Token: 0x040000FB RID: 251
		private const uint FirstAvailableMftIndex = 24U;

		// Token: 0x040000FC RID: 252
		private Bitmap _bitmap;

		// Token: 0x040000FD RID: 253
		private int _bytesPerSector;

		// Token: 0x040000FE RID: 254
		private readonly ObjectCache<long, FileRecord> _recordCache;

		// Token: 0x040000FF RID: 255
		private Stream _recordStream;

		// Token: 0x04000100 RID: 256
		private File _self;
	}
}
