using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200003E RID: 62
	public sealed class NtfsFileSystemChecker : DiscFileSystemChecker
	{
		// Token: 0x06000308 RID: 776 RVA: 0x00010A20 File Offset: 0x0000EC20
		public NtfsFileSystemChecker(Stream diskData)
		{
			SnapshotStream snapshotStream = new SnapshotStream(diskData, Ownership.None);
			snapshotStream.Snapshot();
			snapshotStream.Freeze();
			this._target = snapshotStream;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00010A58 File Offset: 0x0000EC58
		public override bool Check(TextWriter reportOutput, ReportLevels levels)
		{
			this._context = new NtfsContext();
			this._context.RawStream = this._target;
			this._context.Options = new NtfsOptions();
			this._report = reportOutput;
			this._reportLevels = levels;
			this._levelsDetected = ReportLevels.None;
			try
			{
				this.DoCheck();
			}
			catch (NtfsFileSystemChecker.AbortException arg)
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

		// Token: 0x0600030A RID: 778 RVA: 0x00010B14 File Offset: 0x0000ED14
		public ClusterMap BuildClusterMap()
		{
			this._context = new NtfsContext();
			this._context.RawStream = this._target;
			this._context.Options = new NtfsOptions();
			this._context.RawStream.Position = 0L;
			byte[] bytes = StreamUtilities.ReadExact(this._context.RawStream, 512);
			this._context.BiosParameterBlock = BiosParameterBlock.FromBytes(bytes, 0);
			this._context.Mft = new MasterFileTable(this._context);
			File file = new File(this._context, this._context.Mft.GetBootstrapRecord());
			this._context.Mft.Initialize(file);
			return this._context.Mft.GetClusterMap();
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00010BDA File Offset: 0x0000EDDA
		private static void Abort()
		{
			throw new NtfsFileSystemChecker.AbortException();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00010BE4 File Offset: 0x0000EDE4
		private void DoCheck()
		{
			this._context.RawStream.Position = 0L;
			byte[] bytes = StreamUtilities.ReadExact(this._context.RawStream, 512);
			this._context.BiosParameterBlock = BiosParameterBlock.FromBytes(bytes, 0);
			this._context.Mft = new MasterFileTable(this._context);
			File file = new File(this._context, this._context.Mft.GetBootstrapRecord());
			this.PreVerifyMft(file);
			this._context.Mft.Initialize(file);
			this.VerifyMft();
			this._context.Mft.Dump(this._report, "INFO: ");
			File file2 = new File(this._context, this._context.Mft.GetRecord(10L, false));
			this._context.UpperCase = new UpperCase(file2);
			this.SelfCheckIndexes();
			this.VerifyDirectories();
			this.VerifyWellKnownFilesExist();
			this.VerifyObjectIds();
			using (NtfsFileSystem ntfsFileSystem = new NtfsFileSystem(this._context.RawStream))
			{
				if ((this._reportLevels & ReportLevels.Information) != ReportLevels.None)
				{
					this.ReportDump(ntfsFileSystem);
				}
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00010D20 File Offset: 0x0000EF20
		private void VerifyWellKnownFilesExist()
		{
			Directory directory = new Directory(this._context, this._context.Mft.GetRecord(5L, false));
			DirectoryEntry entryByName = directory.GetEntryByName("$Extend");
			if (entryByName == null)
			{
				this.ReportError("$Extend does not exist in root directory", new object[0]);
				NtfsFileSystemChecker.Abort();
			}
			DirectoryEntry entryByName2 = new Directory(this._context, this._context.Mft.GetRecord(entryByName.Reference)).GetEntryByName("$ObjId");
			if (entryByName2 == null)
			{
				this.ReportError("$ObjId does not exist in $Extend directory", new object[0]);
				NtfsFileSystemChecker.Abort();
			}
			this._context.ObjectIds = new ObjectIds(new File(this._context, this._context.Mft.GetRecord(entryByName2.Reference)));
			if (directory.GetEntryByName("System Volume Information") == null)
			{
				this.ReportError("'System Volume Information' does not exist in root directory", new object[0]);
				NtfsFileSystemChecker.Abort();
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00010E0C File Offset: 0x0000F00C
		private void VerifyObjectIds()
		{
			foreach (FileRecord fileRecord in this._context.Mft.Records)
			{
				if (fileRecord.BaseFile.Value != 0UL)
				{
					File file = new File(this._context, fileRecord);
					foreach (NtfsStream ntfsStream in file.AllStreams)
					{
						if (ntfsStream.AttributeType == AttributeType.ObjectId)
						{
							ObjectId content = ntfsStream.GetContent<ObjectId>();
							ObjectIdRecord objectIdRecord;
							if (!this._context.ObjectIds.TryGetValue(content.Id, out objectIdRecord))
							{
								this.ReportError("ObjectId {0} for file {1} is not indexed", new object[]
								{
									content.Id,
									file.BestName
								});
							}
							else if (objectIdRecord.MftReference != file.MftReference)
							{
								this.ReportError("ObjectId {0} for file {1} points to {2}", new object[]
								{
									content.Id,
									file.BestName,
									objectIdRecord.MftReference
								});
							}
						}
					}
				}
			}
			foreach (KeyValuePair<Guid, ObjectIdRecord> keyValuePair in this._context.ObjectIds.All)
			{
				if (this._context.Mft.GetRecord(keyValuePair.Value.MftReference) == null)
				{
					this.ReportError("ObjectId {0} refers to non-existant file {1}", new object[]
					{
						keyValuePair.Key,
						keyValuePair.Value.MftReference
					});
				}
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00011028 File Offset: 0x0000F228
		private void VerifyDirectories()
		{
			foreach (FileRecord fileRecord in this._context.Mft.Records)
			{
				if (fileRecord.BaseFile.Value == 0UL)
				{
					File file = new File(this._context, fileRecord);
					foreach (NtfsStream ntfsStream in file.AllStreams)
					{
						if (ntfsStream.AttributeType == AttributeType.IndexRoot && ntfsStream.Name == "$I30")
						{
							foreach (KeyValuePair<FileNameRecord, FileRecordReference> keyValuePair in new IndexView<FileNameRecord, FileRecordReference>(file.GetIndex("$I30")).Entries)
							{
								FileRecord record = this._context.Mft.GetRecord(keyValuePair.Value);
								if (record == null)
								{
									this.ReportError("Directory {0} references non-existent file {1}", new object[]
									{
										file,
										keyValuePair.Key
									});
								}
								StandardInformation standardInformation = new File(this._context, record).StandardInformation;
								if (standardInformation.CreationTime != keyValuePair.Key.CreationTime || standardInformation.MftChangedTime != keyValuePair.Key.MftChangedTime || standardInformation.ModificationTime != keyValuePair.Key.ModificationTime)
								{
									this.ReportInfo("Directory entry {0} in {1} is out of date", new object[]
									{
										keyValuePair.Key,
										file
									});
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00011238 File Offset: 0x0000F438
		private void SelfCheckIndexes()
		{
			foreach (FileRecord baseRecord in this._context.Mft.Records)
			{
				File file = new File(this._context, baseRecord);
				foreach (NtfsStream ntfsStream in file.AllStreams)
				{
					if (ntfsStream.AttributeType == AttributeType.IndexRoot)
					{
						this.SelfCheckIndex(file, ntfsStream.Name);
					}
				}
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000112E8 File Offset: 0x0000F4E8
		private void SelfCheckIndex(File file, string name)
		{
			this.ReportInfo("About to self-check index {0} in file {1} (MFT:{2})", new object[]
			{
				name,
				file.BestName,
				file.IndexInMft
			});
			IndexRoot content = file.GetStream(AttributeType.IndexRoot, name).GetContent<IndexRoot>();
			byte[] buffer;
			using (Stream stream = file.OpenStream(AttributeType.IndexRoot, name, FileAccess.Read))
			{
				buffer = StreamUtilities.ReadExact(stream, (int)stream.Length);
			}
			Bitmap bitmap = null;
			if (file.GetStream(AttributeType.Bitmap, name) != null)
			{
				bitmap = new Bitmap(file.OpenStream(AttributeType.Bitmap, name, FileAccess.Read), long.MaxValue);
			}
			if (!this.SelfCheckIndexNode(buffer, 16, bitmap, content, file.BestName, name))
			{
				this.ReportError("Index {0} in file {1} (MFT:{2}) has corrupt IndexRoot attribute", new object[]
				{
					name,
					file.BestName,
					file.IndexInMft
				});
				return;
			}
			this.ReportInfo("Self-check of index {0} in file {1} (MFT:{2}) complete", new object[]
			{
				name,
				file.BestName,
				file.IndexInMft
			});
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00011404 File Offset: 0x0000F604
		private bool SelfCheckIndexNode(byte[] buffer, int offset, Bitmap bitmap, IndexRoot root, string fileName, string indexName)
		{
			bool result = true;
			IndexHeader indexHeader = new IndexHeader(buffer, offset);
			IndexEntry indexEntry = null;
			IComparer<byte[]> collator = root.GetCollator(this._context.UpperCase);
			int num = (int)indexHeader.OffsetToFirstEntry;
			while ((long)num < (long)((ulong)indexHeader.TotalSizeOfEntries))
			{
				IndexEntry indexEntry2 = new IndexEntry(indexName == "$I30");
				indexEntry2.Read(buffer, offset + num);
				num += indexEntry2.Size;
				if ((indexEntry2.Flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
				{
					long num2 = indexEntry2.ChildrenVirtualCluster / MathUtilities.Ceil((long)((ulong)root.IndexAllocationSize), (long)((ushort)this._context.BiosParameterBlock.SectorsPerCluster * this._context.BiosParameterBlock.BytesPerSector));
					if (!bitmap.IsPresent(num2))
					{
						this.ReportError("Index entry {0} is non-leaf, but child vcn {1} is not in bitmap at index {2}", new object[]
						{
							Index.EntryAsString(indexEntry2, fileName, indexName),
							indexEntry2.ChildrenVirtualCluster,
							num2
						});
					}
				}
				if ((indexEntry2.Flags & IndexEntryFlags.End) != IndexEntryFlags.None && (long)num != (long)((ulong)indexHeader.TotalSizeOfEntries))
				{
					this.ReportError("Found END index entry {0}, but not at end of node", new object[]
					{
						Index.EntryAsString(indexEntry2, fileName, indexName)
					});
					result = false;
				}
				if (indexEntry != null && collator.Compare(indexEntry.KeyBuffer, indexEntry2.KeyBuffer) >= 0)
				{
					this.ReportError("Found entries out of order {0} was before {1}", new object[]
					{
						Index.EntryAsString(indexEntry, fileName, indexName),
						Index.EntryAsString(indexEntry2, fileName, indexName)
					});
					result = false;
				}
				indexEntry = indexEntry2;
			}
			return result;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00011580 File Offset: 0x0000F780
		private void PreVerifyMft(File file)
		{
			int mftRecordSize = this._context.BiosParameterBlock.MftRecordSize;
			int bytesPerSector = (int)this._context.BiosParameterBlock.BytesPerSector;
			foreach (Range<long, long> range in file.GetAttribute(AttributeType.Data, null).GetClusters())
			{
				if (!this.VerifyClusterRange(range))
				{
					this.ReportError("Corrupt cluster range in MFT data attribute {0}", new object[]
					{
						range.ToString()
					});
					NtfsFileSystemChecker.Abort();
				}
			}
			foreach (Range<long, long> range2 in file.GetAttribute(AttributeType.Bitmap, null).GetClusters())
			{
				if (!this.VerifyClusterRange(range2))
				{
					this.ReportError("Corrupt cluster range in MFT bitmap attribute {0}", new object[]
					{
						range2.ToString()
					});
					NtfsFileSystemChecker.Abort();
				}
			}
			using (Stream stream = file.OpenStream(AttributeType.Data, null, FileAccess.Read))
			{
				using (Stream stream2 = file.OpenStream(AttributeType.Bitmap, null, FileAccess.Read))
				{
					Bitmap bitmap = new Bitmap(stream2, long.MaxValue);
					long num = 0L;
					while (stream.Position < stream.Length)
					{
						byte[] array = StreamUtilities.ReadExact(stream, mftRecordSize);
						string text = EndianUtilities.BytesToString(array, 0, 4);
						if (text != "FILE")
						{
							if (bitmap.IsPresent(num))
							{
								this.ReportError("Invalid MFT record magic at index {0} - was ({2},{3},{4},{5}) \"{1}\"", new object[]
								{
									num,
									text.Trim(new char[1]),
									(int)text[0],
									(int)text[1],
									(int)text[2],
									(int)text[3]
								});
							}
						}
						else if (!this.VerifyMftRecord(array, bitmap.IsPresent(num), bytesPerSector))
						{
							this.ReportError("Invalid MFT record at index {0}", new object[]
							{
								num
							});
							StringBuilder stringBuilder = new StringBuilder();
							for (int j = 0; j < array.Length; j++)
							{
								stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0:X2}", new object[]
								{
									array[j]
								}));
							}
							this.ReportInfo("MFT record binary data for index {0}:{1}", new object[]
							{
								num,
								stringBuilder.ToString()
							});
						}
						num += 1L;
					}
				}
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0001182C File Offset: 0x0000FA2C
		private void VerifyMft()
		{
			Dictionary<long, string> dictionary = new Dictionary<long, string>();
			foreach (FileRecord fileRecord in this._context.Mft.Records)
			{
				if ((fileRecord.Flags & FileRecordFlags.InUse) != FileRecordFlags.None)
				{
					foreach (NtfsAttribute ntfsAttribute in new File(this._context, fileRecord).AllAttributes)
					{
						string text = fileRecord.MasterFileTableIndex + ":" + ntfsAttribute.Id;
						foreach (Range<long, long> range in ntfsAttribute.GetClusters())
						{
							if (!this.VerifyClusterRange(range))
							{
								this.ReportError("Attribute {0} contains bad cluster range {1}", new object[]
								{
									text,
									range
								});
							}
							for (long num = range.Offset; num < range.Offset + range.Count; num += 1L)
							{
								string text2;
								if (dictionary.TryGetValue(num, out text2))
								{
									this.ReportError("Two attributes referencing cluster {0} (0x{0:X16}) - {1} and {2} (as MftIndex:AttrId)", new object[]
									{
										num,
										text2,
										text
									});
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000119BC File Offset: 0x0000FBBC
		private bool VerifyMftRecord(byte[] recordData, bool presentInBitmap, int bytesPerSector)
		{
			bool result = true;
			byte[] array = new byte[recordData.Length];
			Array.Copy(recordData, array, array.Length);
			GenericFixupRecord genericFixupRecord = new GenericFixupRecord(bytesPerSector);
			genericFixupRecord.FromBytes(array, 0);
			int num = (int)EndianUtilities.ToUInt16LittleEndian(genericFixupRecord.Content, 20);
			while (EndianUtilities.ToUInt32LittleEndian(genericFixupRecord.Content, num) != 4294967295U)
			{
				int num2;
				try
				{
					AttributeRecord attributeRecord = AttributeRecord.FromBytes(genericFixupRecord.Content, num, out num2);
					if (num2 != attributeRecord.Size)
					{
						this.ReportError("Attribute size is different to calculated size.  AttrId={0}", new object[]
						{
							attributeRecord.AttributeId
						});
						result = false;
					}
					if (attributeRecord.IsNonResident)
					{
						NonResidentAttributeRecord nonResidentAttributeRecord = (NonResidentAttributeRecord)attributeRecord;
						if (nonResidentAttributeRecord.DataRuns.Count > 0)
						{
							long num3 = 0L;
							foreach (DataRun dataRun in nonResidentAttributeRecord.DataRuns)
							{
								num3 += dataRun.RunLength;
							}
							if (num3 != nonResidentAttributeRecord.LastVcn - nonResidentAttributeRecord.StartVcn + 1L)
							{
								this.ReportError("Declared VCNs doesn't match data runs.  AttrId={0}", new object[]
								{
									attributeRecord.AttributeId
								});
								result = false;
							}
						}
					}
				}
				catch
				{
					this.ReportError("Failure parsing attribute at pos={0}", new object[]
					{
						num
					});
					return false;
				}
				num += num2;
			}
			FileRecord fileRecord = new FileRecord(bytesPerSector);
			fileRecord.FromBytes(recordData, 0);
			bool flag = (fileRecord.Flags & FileRecordFlags.InUse) > FileRecordFlags.None;
			if (flag != presentInBitmap)
			{
				this.ReportError("MFT bitmap and record in-use flag don't agree.  Mft={0}, Record={1}", new object[]
				{
					presentInBitmap ? "InUse" : "Free",
					flag ? "InUse" : "Free"
				});
				result = false;
			}
			if ((long)fileRecord.Size != (long)((ulong)fileRecord.RealSize))
			{
				this.ReportError("MFT record real size is different to calculated size.  Stored in MFT={0}, Calculated={1}", new object[]
				{
					fileRecord.RealSize,
					fileRecord.Size
				});
				result = false;
			}
			if (EndianUtilities.ToUInt32LittleEndian(recordData, (int)(fileRecord.RealSize - 8U)) != 4294967295U)
			{
				this.ReportError("MFT record is not correctly terminated with 0xFFFFFFFF", new object[0]);
				result = false;
			}
			return result;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00011BFC File Offset: 0x0000FDFC
		private bool VerifyClusterRange(Range<long, long> range)
		{
			bool result = true;
			if (range.Offset < 0L)
			{
				this.ReportError("Invalid cluster range {0} - negative start", new object[]
				{
					range
				});
				result = false;
			}
			if (range.Count <= 0L)
			{
				this.ReportError("Invalid cluster range {0} - negative/zero count", new object[]
				{
					range
				});
				result = false;
			}
			if ((range.Offset + range.Count) * (long)this._context.BiosParameterBlock.BytesPerCluster > this._context.RawStream.Length)
			{
				this.ReportError("Invalid cluster range {0} - beyond end of disk", new object[]
				{
					range
				});
				result = false;
			}
			return result;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00011C96 File Offset: 0x0000FE96
		private void ReportDump(IDiagnosticTraceable toDump)
		{
			this._levelsDetected |= ReportLevels.Information;
			if ((this._reportLevels & ReportLevels.Information) != ReportLevels.None)
			{
				toDump.Dump(this._report, "INFO: ");
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00011CC1 File Offset: 0x0000FEC1
		private void ReportInfo(string str, params object[] args)
		{
			this._levelsDetected |= ReportLevels.Information;
			if ((this._reportLevels & ReportLevels.Information) != ReportLevels.None)
			{
				this._report.WriteLine("INFO: " + str, args);
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00011CF2 File Offset: 0x0000FEF2
		private void ReportError(string str, params object[] args)
		{
			this._levelsDetected |= ReportLevels.Errors;
			if ((this._reportLevels & ReportLevels.Errors) != ReportLevels.None)
			{
				this._report.WriteLine("ERROR: " + str, args);
			}
		}

		// Token: 0x04000138 RID: 312
		private readonly Stream _target;

		// Token: 0x04000139 RID: 313
		private NtfsContext _context;

		// Token: 0x0400013A RID: 314
		private TextWriter _report;

		// Token: 0x0400013B RID: 315
		private ReportLevels _reportLevels;

		// Token: 0x0400013C RID: 316
		private ReportLevels _levelsDetected;

		// Token: 0x0400013D RID: 317
		private readonly ReportLevels _levelsConsideredFail = ReportLevels.Errors;

		// Token: 0x02000084 RID: 132
		[Serializable]
		private sealed class AbortException : InvalidFileSystemException
		{
		}
	}
}
