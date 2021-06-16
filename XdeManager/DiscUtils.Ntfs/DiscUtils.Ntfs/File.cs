using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000019 RID: 25
	internal class File
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00005560 File Offset: 0x00003760
		public File(INtfsContext context, FileRecord baseRecord)
		{
			this._context = context;
			this._mft = this._context.Mft;
			this._records = new List<FileRecord>();
			this._records.Add(baseRecord);
			this._indexCache = new ObjectCache<string, Index>();
			this._attributes = new List<NtfsAttribute>();
			this.LoadAttributes();
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000055BE File Offset: 0x000037BE
		internal IEnumerable<NtfsAttribute> AllAttributes
		{
			get
			{
				return this._attributes;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000055C6 File Offset: 0x000037C6
		public IEnumerable<NtfsStream> AllStreams
		{
			get
			{
				foreach (NtfsAttribute attr in this._attributes)
				{
					yield return new NtfsStream(this, attr);
				}
				List<NtfsAttribute>.Enumerator enumerator = default(List<NtfsAttribute>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000055D8 File Offset: 0x000037D8
		public string BestName
		{
			get
			{
				NtfsAttribute[] attributes = this.GetAttributes(AttributeType.FileName);
				string text = null;
				if (attributes != null && attributes.Length != 0)
				{
					text = attributes[0].ToString();
					for (int i = 1; i < attributes.Length; i++)
					{
						string text2 = attributes[i].ToString();
						if (Utilities.Is8Dot3(text))
						{
							text = text2;
						}
					}
				}
				return text;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005622 File Offset: 0x00003822
		internal INtfsContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000562C File Offset: 0x0000382C
		public DirectoryEntry DirectoryEntry
		{
			get
			{
				if (this._context.GetDirectoryByRef == null)
				{
					return null;
				}
				NtfsStream stream = this.GetStream(AttributeType.FileName, null);
				if (stream == null)
				{
					return null;
				}
				FileNameRecord content = stream.GetContent<FileNameRecord>();
				if ((ulong)this._records[0].MasterFileTableIndex == 5UL)
				{
					content.Flags |= FileAttributeFlags.Directory;
				}
				return new DirectoryEntry(this._context.GetDirectoryByRef(content.ParentDirectory), this.MftReference, content);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000056A8 File Offset: 0x000038A8
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x000056BB File Offset: 0x000038BB
		public ushort HardLinkCount
		{
			get
			{
				return this._records[0].HardLinkCount;
			}
			set
			{
				this._records[0].HardLinkCount = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000056D0 File Offset: 0x000038D0
		public bool HasWin32OrDosName
		{
			get
			{
				NtfsAttribute[] attributes = this.GetAttributes(AttributeType.FileName);
				for (int i = 0; i < attributes.Length; i++)
				{
					if (((StructuredNtfsAttribute<FileNameRecord>)attributes[i]).Content.FileNameNamespace != FileNameNamespace.Posix)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000570B File Offset: 0x0000390B
		public uint IndexInMft
		{
			get
			{
				return this._records[0].MasterFileTableIndex;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000571E File Offset: 0x0000391E
		public bool IsDirectory
		{
			get
			{
				return (this._records[0].Flags & FileRecordFlags.IsDirectory) > FileRecordFlags.None;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00005736 File Offset: 0x00003936
		public uint MaxMftRecordSize
		{
			get
			{
				return this._records[0].AllocatedSize;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005749 File Offset: 0x00003949
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00005751 File Offset: 0x00003951
		public bool MftRecordIsDirty { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000575A File Offset: 0x0000395A
		public FileRecordReference MftReference
		{
			get
			{
				return this._records[0].Reference;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00005770 File Offset: 0x00003970
		public List<string> Names
		{
			get
			{
				List<string> list = new List<string>();
				if ((ulong)this.IndexInMft == 5UL)
				{
					list.Add(string.Empty);
				}
				else
				{
					foreach (StructuredNtfsAttribute<FileNameRecord> structuredNtfsAttribute in this.GetAttributes(AttributeType.FileName))
					{
						string fileName = structuredNtfsAttribute.Content.FileName;
						Directory directory = this._context.GetDirectoryByRef(structuredNtfsAttribute.Content.ParentDirectory);
						if (directory != null)
						{
							foreach (string a in directory.Names)
							{
								list.Add(Utilities.CombinePaths(a, fileName));
							}
						}
					}
				}
				return list;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005844 File Offset: 0x00003A44
		public StandardInformation StandardInformation
		{
			get
			{
				return this.GetStream(AttributeType.StandardInformation, null).GetContent<StandardInformation>();
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005854 File Offset: 0x00003A54
		public static File CreateNew(INtfsContext context, FileAttributeFlags dirFlags)
		{
			return File.CreateNew(context, FileRecordFlags.None, dirFlags);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005860 File Offset: 0x00003A60
		public static File CreateNew(INtfsContext context, FileRecordFlags flags, FileAttributeFlags dirFlags)
		{
			File file = context.AllocateFile(flags);
			FileAttributeFlags flags2 = FileAttributeFlags.Archive | FileRecord.ConvertFlags(flags) | (dirFlags & FileAttributeFlags.Compressed);
			AttributeFlags attributeFlags = AttributeFlags.None;
			if ((dirFlags & FileAttributeFlags.Compressed) != FileAttributeFlags.None)
			{
				attributeFlags |= AttributeFlags.Compressed;
			}
			StandardInformation.InitializeNewFile(file, flags2);
			if (context.ObjectIds != null)
			{
				Guid guid = File.CreateNewGuid(context);
				file.CreateStream(AttributeType.ObjectId, null).SetContent<ObjectId>(new ObjectId
				{
					Id = guid
				});
				context.ObjectIds.Add(guid, file.MftReference, guid, Guid.Empty, Guid.Empty);
			}
			file.CreateAttribute(AttributeType.Data, attributeFlags);
			file.UpdateRecordInMft();
			return file;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005904 File Offset: 0x00003B04
		public int MftRecordFreeSpace(AttributeType attrType, string attrName)
		{
			foreach (FileRecord fileRecord in this._records)
			{
				if (fileRecord.GetAttribute(attrType, attrName) != null)
				{
					return this._mft.RecordSize - fileRecord.Size;
				}
			}
			throw new IOException("Attempt to determine free space for non-existent attribute");
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000597C File Offset: 0x00003B7C
		public void Modified()
		{
			DateTime utcNow = DateTime.UtcNow;
			NtfsStream stream = this.GetStream(AttributeType.StandardInformation, null);
			StandardInformation content = stream.GetContent<StandardInformation>();
			content.LastAccessTime = utcNow;
			content.ModificationTime = utcNow;
			stream.SetContent<StandardInformation>(content);
			this.MarkMftRecordDirty();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000059BC File Offset: 0x00003BBC
		public void Accessed()
		{
			DateTime utcNow = DateTime.UtcNow;
			NtfsStream stream = this.GetStream(AttributeType.StandardInformation, null);
			StandardInformation content = stream.GetContent<StandardInformation>();
			content.LastAccessTime = utcNow;
			stream.SetContent<StandardInformation>(content);
			this.MarkMftRecordDirty();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000059F2 File Offset: 0x00003BF2
		public void MarkMftRecordDirty()
		{
			this.MftRecordIsDirty = true;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000059FC File Offset: 0x00003BFC
		public void UpdateRecordInMft()
		{
			if (this.MftRecordIsDirty)
			{
				if (NtfsTransaction.Current != null)
				{
					NtfsStream stream = this.GetStream(AttributeType.StandardInformation, null);
					StandardInformation content = stream.GetContent<StandardInformation>();
					content.MftChangedTime = NtfsTransaction.Current.Timestamp;
					stream.SetContent<StandardInformation>(content);
				}
				bool flag = true;
				while (flag)
				{
					flag = false;
					for (int i = 0; i < this._records.Count; i++)
					{
						FileRecord fileRecord = this._records[i];
						bool flag2 = true;
						while (fileRecord.Size > this._mft.RecordSize && flag2)
						{
							flag2 = false;
							if (!flag2 && !fileRecord.IsMftRecord)
							{
								foreach (AttributeRecord attributeRecord in fileRecord.Attributes)
								{
									if (!attributeRecord.IsNonResident && !this._context.AttributeDefinitions.MustBeResident(attributeRecord.AttributeType))
									{
										this.MakeAttributeNonResident(new AttributeReference(fileRecord.Reference, attributeRecord.AttributeId), (int)attributeRecord.DataLength);
										flag2 = true;
										break;
									}
								}
							}
							if (!flag2)
							{
								foreach (AttributeRecord attributeRecord2 in fileRecord.Attributes)
								{
									if (attributeRecord2.AttributeType == AttributeType.IndexRoot && this.ShrinkIndexRoot(attributeRecord2.Name))
									{
										flag2 = true;
										break;
									}
								}
							}
							if (!flag2)
							{
								if (fileRecord.Attributes.Count == 1)
								{
									flag2 = this.SplitAttribute(fileRecord);
								}
								else
								{
									if (this._records.Count == 1)
									{
										this.CreateAttributeList();
									}
									flag2 = this.ExpelAttribute(fileRecord);
								}
							}
							flag = (flag || flag2);
						}
					}
				}
				this.MftRecordIsDirty = false;
				foreach (FileRecord record in this._records)
				{
					this._mft.WriteRecord(record);
				}
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00005C2C File Offset: 0x00003E2C
		public Index CreateIndex(string name, AttributeType attrType, AttributeCollationRule collRule)
		{
			Index.Create(attrType, collRule, this, name);
			return this.GetIndex(name);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005C40 File Offset: 0x00003E40
		public Index GetIndex(string name)
		{
			Index index = this._indexCache[name];
			if (index == null)
			{
				index = new Index(this, name, this._context.BiosParameterBlock, this._context.UpperCase);
				this._indexCache[name] = index;
			}
			return index;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005C8C File Offset: 0x00003E8C
		public void Delete()
		{
			if (this._records[0].HardLinkCount != 0)
			{
				throw new InvalidOperationException("Attempt to delete in-use file: " + this.ToString());
			}
			this._context.ForgetFile(this);
			NtfsStream stream = this.GetStream(AttributeType.ObjectId, null);
			if (stream != null)
			{
				ObjectId content = stream.GetContent<ObjectId>();
				this.Context.ObjectIds.Remove(content.Id);
			}
			List<NtfsAttribute> list = new List<NtfsAttribute>(this._attributes.Count);
			foreach (NtfsAttribute ntfsAttribute in this._attributes)
			{
				if (ntfsAttribute.Type != AttributeType.AttributeList)
				{
					list.Add(ntfsAttribute);
				}
			}
			foreach (NtfsAttribute ntfsAttribute2 in list)
			{
				ntfsAttribute2.GetDataBuffer().SetCapacity(0L);
			}
			NtfsAttribute attribute = this.GetAttribute(AttributeType.AttributeList, null);
			if (attribute != null)
			{
				attribute.GetDataBuffer().SetCapacity(0L);
			}
			foreach (FileRecord fileRecord in this._records)
			{
				this._context.Mft.RemoveRecord(fileRecord.Reference);
			}
			this._attributes.Clear();
			this._records.Clear();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005E28 File Offset: 0x00004028
		public bool StreamExists(AttributeType attrType, string name)
		{
			return this.GetStream(attrType, name) != null;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005E38 File Offset: 0x00004038
		public NtfsStream GetStream(AttributeType attrType, string name)
		{
			using (IEnumerator<NtfsStream> enumerator = this.GetStreams(attrType, name).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005E88 File Offset: 0x00004088
		public IEnumerable<NtfsStream> GetStreams(AttributeType attrType, string name)
		{
			foreach (NtfsAttribute ntfsAttribute in this._attributes)
			{
				if (ntfsAttribute.Type == attrType && ntfsAttribute.Name == name)
				{
					yield return new NtfsStream(this, ntfsAttribute);
				}
			}
			List<NtfsAttribute>.Enumerator enumerator = default(List<NtfsAttribute>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00005EA6 File Offset: 0x000040A6
		public NtfsStream CreateStream(AttributeType attrType, string name)
		{
			return new NtfsStream(this, this.CreateAttribute(attrType, name, AttributeFlags.None));
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005EB7 File Offset: 0x000040B7
		public NtfsStream CreateStream(AttributeType attrType, string name, long firstCluster, ulong numClusters, uint bytesPerCluster)
		{
			return new NtfsStream(this, this.CreateAttribute(attrType, name, AttributeFlags.None, firstCluster, numClusters, bytesPerCluster));
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005ED0 File Offset: 0x000040D0
		public SparseStream OpenStream(AttributeType attrType, string name, FileAccess access)
		{
			NtfsAttribute attribute = this.GetAttribute(attrType, name);
			if (attribute != null)
			{
				return new File.FileStream(this, attribute, access);
			}
			return null;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005EF3 File Offset: 0x000040F3
		public void RemoveStream(NtfsStream stream)
		{
			this.RemoveAttribute(stream.Attribute);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005F04 File Offset: 0x00004104
		public FileNameRecord GetFileNameRecord(string name, bool freshened)
		{
			NtfsAttribute[] attributes = this.GetAttributes(AttributeType.FileName);
			StructuredNtfsAttribute<FileNameRecord> structuredNtfsAttribute = null;
			if (string.IsNullOrEmpty(name))
			{
				if (attributes.Length != 0)
				{
					structuredNtfsAttribute = (StructuredNtfsAttribute<FileNameRecord>)attributes[0];
				}
			}
			else
			{
				foreach (StructuredNtfsAttribute<FileNameRecord> structuredNtfsAttribute2 in attributes)
				{
					if (this._context.UpperCase.Compare(structuredNtfsAttribute2.Content.FileName, name) == 0)
					{
						structuredNtfsAttribute = structuredNtfsAttribute2;
					}
				}
				if (structuredNtfsAttribute == null)
				{
					throw new FileNotFoundException("File name not found on file", name);
				}
			}
			FileNameRecord fileNameRecord = (structuredNtfsAttribute == null) ? new FileNameRecord() : new FileNameRecord(structuredNtfsAttribute.Content);
			if (freshened)
			{
				this.FreshenFileName(fileNameRecord, false);
			}
			return fileNameRecord;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005FA8 File Offset: 0x000041A8
		public virtual void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "FILE (" + this.ToString() + ")");
			writer.WriteLine(indent + "  File Number: " + this._records[0].MasterFileTableIndex);
			this._records[0].Dump(writer, indent + "  ");
			foreach (AttributeRecord record in this._records[0].Attributes)
			{
				NtfsAttribute.FromRecord(this, this.MftReference, record).Dump(writer, indent + "  ");
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00006080 File Offset: 0x00004280
		public override string ToString()
		{
			string bestName = this.BestName;
			if (bestName == null)
			{
				return "?????";
			}
			return bestName;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000060A0 File Offset: 0x000042A0
		internal void RemoveAttributeExtents(NtfsAttribute attr)
		{
			attr.GetDataBuffer().SetCapacity(0L);
			foreach (AttributeReference extentRef in attr.Extents.Keys)
			{
				this.RemoveAttributeExtent(extentRef);
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006100 File Offset: 0x00004300
		internal void RemoveAttributeExtent(AttributeReference extentRef)
		{
			FileRecord fileRecord = this.GetFileRecord(extentRef.File);
			if (fileRecord != null)
			{
				fileRecord.RemoveAttribute(extentRef.AttributeId);
				if (fileRecord.Attributes.Count == 0 && fileRecord.BaseFile.Value != 0UL)
				{
					this.RemoveFileRecord(extentRef.File);
				}
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006154 File Offset: 0x00004354
		internal NtfsAttribute GetAttribute(AttributeReference attrRef)
		{
			foreach (NtfsAttribute ntfsAttribute in this._attributes)
			{
				if (ntfsAttribute.Reference.Equals(attrRef))
				{
					return ntfsAttribute;
				}
			}
			return null;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000061B8 File Offset: 0x000043B8
		internal NtfsAttribute GetAttribute(AttributeType type, string name)
		{
			foreach (NtfsAttribute ntfsAttribute in this._attributes)
			{
				if (ntfsAttribute.PrimaryRecord.AttributeType == type && ntfsAttribute.Name == name)
				{
					return ntfsAttribute;
				}
			}
			return null;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006228 File Offset: 0x00004428
		internal NtfsAttribute[] GetAttributes(AttributeType type)
		{
			List<NtfsAttribute> list = new List<NtfsAttribute>();
			foreach (NtfsAttribute ntfsAttribute in this._attributes)
			{
				if (ntfsAttribute.PrimaryRecord.AttributeType == type && string.IsNullOrEmpty(ntfsAttribute.Name))
				{
					list.Add(ntfsAttribute);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000062A4 File Offset: 0x000044A4
		internal void MakeAttributeNonResident(AttributeReference attrRef, int maxData)
		{
			NtfsAttribute attribute = this.GetAttribute(attrRef);
			if (attribute.IsNonResident)
			{
				throw new InvalidOperationException("Attribute is already non-resident");
			}
			ushort id = this._records[0].CreateNonResidentAttribute(attribute.Type, attribute.Name, attribute.Flags);
			AttributeRecord attribute2 = this._records[0].GetAttribute(id);
			IBuffer dataBuffer = attribute.GetDataBuffer();
			byte[] array = StreamUtilities.ReadExact(dataBuffer, 0L, (int)Math.Min((long)maxData, dataBuffer.Capacity));
			this.RemoveAttributeExtents(attribute);
			attribute.SetExtent(this._records[0].Reference, attribute2);
			attribute.GetDataBuffer().Write(0L, array, 0, array.Length);
			this.UpdateAttributeList();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000635C File Offset: 0x0000455C
		internal void FreshenFileName(FileNameRecord fileName, bool updateMftRecord)
		{
			StandardInformation standardInformation = this.StandardInformation;
			NtfsAttribute attribute = this.GetAttribute(AttributeType.Data, null);
			fileName.CreationTime = standardInformation.CreationTime;
			fileName.ModificationTime = standardInformation.ModificationTime;
			fileName.MftChangedTime = standardInformation.MftChangedTime;
			fileName.LastAccessTime = standardInformation.LastAccessTime;
			fileName.Flags = standardInformation.FileAttributes;
			if (this.MftRecordIsDirty && NtfsTransaction.Current != null)
			{
				fileName.MftChangedTime = NtfsTransaction.Current.Timestamp;
			}
			if ((this._records[0].Flags & FileRecordFlags.IsDirectory) != FileRecordFlags.None)
			{
				fileName.Flags |= FileAttributeFlags.Directory;
			}
			if (attribute != null)
			{
				fileName.RealSize = (ulong)attribute.PrimaryRecord.DataLength;
				fileName.AllocatedSize = (ulong)attribute.PrimaryRecord.AllocatedLength;
			}
			if (updateMftRecord)
			{
				foreach (NtfsStream ntfsStream in this.GetStreams(AttributeType.FileName, null))
				{
					FileNameRecord fileNameRecord = ntfsStream.GetContent<FileNameRecord>();
					if (fileNameRecord.Equals(fileName))
					{
						fileNameRecord = new FileNameRecord(fileName);
						fileNameRecord.Flags &= ~FileAttributeFlags.ReparsePoint;
						ntfsStream.SetContent<FileNameRecord>(fileNameRecord);
					}
				}
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00006498 File Offset: 0x00004698
		internal long GetAttributeOffset(AttributeReference attrRef)
		{
			long recordOffset = this._mft.GetRecordOffset(attrRef.File);
			FileRecord fileRecord = this.GetFileRecord(attrRef.File);
			return recordOffset + fileRecord.GetAttributeOffset(attrRef.AttributeId);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000064D0 File Offset: 0x000046D0
		private static Guid CreateNewGuid(INtfsContext context)
		{
			Random randomNumberGenerator = context.Options.RandomNumberGenerator;
			if (randomNumberGenerator != null)
			{
				byte[] array = new byte[16];
				randomNumberGenerator.NextBytes(array);
				return new Guid(array);
			}
			return Guid.NewGuid();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006508 File Offset: 0x00004708
		private void LoadAttributes()
		{
			Dictionary<long, FileRecord> dictionary = new Dictionary<long, FileRecord>();
			AttributeRecord attribute = this._records[0].GetAttribute(AttributeType.AttributeList);
			if (attribute != null)
			{
				NtfsAttribute ntfsAttribute = null;
				StructuredNtfsAttribute<AttributeList> structuredNtfsAttribute = (StructuredNtfsAttribute<AttributeList>)NtfsAttribute.FromRecord(this, this.MftReference, attribute);
				AttributeList content = structuredNtfsAttribute.Content;
				this._attributes.Add(structuredNtfsAttribute);
				foreach (AttributeListRecord attributeListRecord in content)
				{
					FileRecord fileRecord = this._records[0];
					if (attributeListRecord.BaseFileReference.MftIndex != (long)((ulong)this._records[0].MasterFileTableIndex) && !dictionary.TryGetValue(attributeListRecord.BaseFileReference.MftIndex, out fileRecord))
					{
						fileRecord = this._context.Mft.GetRecord(attributeListRecord.BaseFileReference);
						if (fileRecord != null)
						{
							dictionary[(long)((ulong)fileRecord.MasterFileTableIndex)] = fileRecord;
						}
					}
					if (fileRecord != null)
					{
						AttributeRecord attribute2 = fileRecord.GetAttribute(attributeListRecord.AttributeId);
						if (attribute2 != null)
						{
							if (attributeListRecord.StartVcn == 0UL)
							{
								ntfsAttribute = NtfsAttribute.FromRecord(this, attributeListRecord.BaseFileReference, attribute2);
								this._attributes.Add(ntfsAttribute);
							}
							else
							{
								ntfsAttribute.AddExtent(attributeListRecord.BaseFileReference, attribute2);
							}
						}
					}
				}
				using (Dictionary<long, FileRecord>.Enumerator enumerator2 = dictionary.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<long, FileRecord> keyValuePair = enumerator2.Current;
						this._records.Add(keyValuePair.Value);
					}
					return;
				}
			}
			foreach (AttributeRecord record in this._records[0].Attributes)
			{
				this._attributes.Add(NtfsAttribute.FromRecord(this, this.MftReference, record));
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006708 File Offset: 0x00004908
		private bool SplitAttribute(FileRecord record)
		{
			if (record.Attributes.Count != 1)
			{
				throw new InvalidOperationException("Attempting to split attribute in MFT record containing multiple attributes");
			}
			return this.SplitAttribute(record, (NonResidentAttributeRecord)record.FirstAttribute, false);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006738 File Offset: 0x00004938
		private bool SplitAttribute(FileRecord record, NonResidentAttributeRecord targetAttr, bool atStart)
		{
			if (targetAttr.DataRuns.Count <= 1)
			{
				return false;
			}
			int num = 1;
			if (!atStart)
			{
				List<DataRun> dataRuns = targetAttr.DataRuns;
				num = dataRuns.Count - 1;
				int num2 = dataRuns[num].Size;
				while (num > 1 && (long)(record.Size - num2) > (long)((ulong)record.AllocatedSize))
				{
					num--;
					num2 += dataRuns[num].Size;
				}
			}
			AttributeRecord attributeRecord = targetAttr.Split(num);
			FileRecord fileRecord = null;
			foreach (FileRecord fileRecord2 in this._records)
			{
				if (!fileRecord2.IsMftRecord && this._mft.RecordSize - fileRecord2.Size >= attributeRecord.Size)
				{
					fileRecord2.AddAttribute(attributeRecord);
					fileRecord = fileRecord2;
				}
			}
			if (fileRecord == null)
			{
				fileRecord = this._mft.AllocateRecord(this._records[0].Flags & ~FileRecordFlags.InUse, record.IsMftRecord);
				fileRecord.BaseFile = (record.BaseFile.IsNull ? record.Reference : record.BaseFile);
				this._records.Add(fileRecord);
				fileRecord.AddAttribute(attributeRecord);
			}
			bool flag = false;
			foreach (NtfsAttribute ntfsAttribute in this._attributes)
			{
				foreach (KeyValuePair<AttributeReference, AttributeRecord> keyValuePair in ntfsAttribute.Extents)
				{
					if (keyValuePair.Key.File == record.Reference && keyValuePair.Key.AttributeId == targetAttr.AttributeId)
					{
						ntfsAttribute.AddExtent(fileRecord.Reference, attributeRecord);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			this.UpdateAttributeList();
			return true;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006954 File Offset: 0x00004B54
		private bool ExpelAttribute(FileRecord record)
		{
			if ((ulong)record.MasterFileTableIndex == 0UL)
			{
				List<AttributeRecord> attributes = record.Attributes;
				for (int i = attributes.Count - 1; i >= 0; i--)
				{
					AttributeRecord attributeRecord = attributes[i];
					if (attributeRecord.AttributeType == AttributeType.Data && this.SplitAttribute(record, (NonResidentAttributeRecord)attributeRecord, true))
					{
						return true;
					}
				}
			}
			else
			{
				List<AttributeRecord> attributes2 = record.Attributes;
				for (int j = attributes2.Count - 1; j >= 0; j--)
				{
					AttributeRecord attributeRecord2 = attributes2[j];
					if (attributeRecord2.AttributeType > AttributeType.AttributeList)
					{
						foreach (FileRecord fileRecord in this._records)
						{
							if (this._mft.RecordSize - fileRecord.Size >= attributeRecord2.Size)
							{
								this.MoveAttribute(record, attributeRecord2, fileRecord);
								return true;
							}
						}
						FileRecord fileRecord2 = this._mft.AllocateRecord(FileRecordFlags.None, record.IsMftRecord);
						fileRecord2.BaseFile = record.Reference;
						this._records.Add(fileRecord2);
						this.MoveAttribute(record, attributeRecord2, fileRecord2);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006A98 File Offset: 0x00004C98
		private void MoveAttribute(FileRecord record, AttributeRecord attrRec, FileRecord targetRecord)
		{
			AttributeReference oldRef = new AttributeReference(record.Reference, attrRec.AttributeId);
			record.RemoveAttribute(attrRec.AttributeId);
			targetRecord.AddAttribute(attrRec);
			AttributeReference newRef = new AttributeReference(targetRecord.Reference, attrRec.AttributeId);
			foreach (NtfsAttribute ntfsAttribute in this._attributes)
			{
				ntfsAttribute.ReplaceExtent(oldRef, newRef, attrRec);
			}
			this.UpdateAttributeList();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006B2C File Offset: 0x00004D2C
		private void CreateAttributeList()
		{
			ushort id = this._records[0].CreateAttribute(AttributeType.AttributeList, null, false, AttributeFlags.None);
			StructuredNtfsAttribute<AttributeList> item = (StructuredNtfsAttribute<AttributeList>)NtfsAttribute.FromRecord(this, this.MftReference, this._records[0].GetAttribute(id));
			this._attributes.Add(item);
			this.UpdateAttributeList();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006B88 File Offset: 0x00004D88
		private void UpdateAttributeList()
		{
			if (this._records.Count > 1)
			{
				AttributeList attributeList = new AttributeList();
				foreach (NtfsAttribute ntfsAttribute in this._attributes)
				{
					if (ntfsAttribute.Type != AttributeType.AttributeList)
					{
						foreach (KeyValuePair<AttributeReference, AttributeRecord> keyValuePair in ntfsAttribute.Extents)
						{
							attributeList.Add(AttributeListRecord.FromAttribute(keyValuePair.Value, keyValuePair.Key.File));
						}
					}
				}
				StructuredNtfsAttribute<AttributeList> structuredNtfsAttribute = (StructuredNtfsAttribute<AttributeList>)this.GetAttribute(AttributeType.AttributeList, null);
				structuredNtfsAttribute.Content = attributeList;
				structuredNtfsAttribute.Save();
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006C64 File Offset: 0x00004E64
		private NtfsAttribute CreateAttribute(AttributeType type, AttributeFlags flags)
		{
			return this.CreateAttribute(type, null, flags);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006C70 File Offset: 0x00004E70
		private NtfsAttribute CreateAttribute(AttributeType type, string name, AttributeFlags flags)
		{
			bool indexed = this._context.AttributeDefinitions.IsIndexed(type);
			ushort id = this._records[0].CreateAttribute(type, name, indexed, flags);
			AttributeRecord attribute = this._records[0].GetAttribute(id);
			NtfsAttribute ntfsAttribute = NtfsAttribute.FromRecord(this, this.MftReference, attribute);
			this._attributes.Add(ntfsAttribute);
			this.UpdateAttributeList();
			this.MarkMftRecordDirty();
			return ntfsAttribute;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006CE0 File Offset: 0x00004EE0
		private NtfsAttribute CreateAttribute(AttributeType type, string name, AttributeFlags flags, long firstCluster, ulong numClusters, uint bytesPerCluster)
		{
			this._context.AttributeDefinitions.IsIndexed(type);
			ushort id = this._records[0].CreateNonResidentAttribute(type, name, flags, firstCluster, numClusters, bytesPerCluster);
			AttributeRecord attribute = this._records[0].GetAttribute(id);
			NtfsAttribute ntfsAttribute = NtfsAttribute.FromRecord(this, this.MftReference, attribute);
			this._attributes.Add(ntfsAttribute);
			this.UpdateAttributeList();
			this.MarkMftRecordDirty();
			return ntfsAttribute;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006D54 File Offset: 0x00004F54
		private void RemoveAttribute(NtfsAttribute attr)
		{
			if (attr != null)
			{
				if (attr.PrimaryRecord.AttributeType == AttributeType.IndexRoot)
				{
					this._indexCache.Remove(attr.PrimaryRecord.Name);
				}
				this.RemoveAttributeExtents(attr);
				this._attributes.Remove(attr);
				this.UpdateAttributeList();
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00006DA6 File Offset: 0x00004FA6
		private bool ShrinkIndexRoot(string indexName)
		{
			return this.GetAttribute(AttributeType.IndexRoot, indexName).Length > 40L && this.GetIndex(indexName).ShrinkRoot();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006DCC File Offset: 0x00004FCC
		private void MakeAttributeResident(AttributeReference attrRef, int maxData)
		{
			NtfsAttribute attribute = this.GetAttribute(attrRef);
			if (!attribute.IsNonResident)
			{
				throw new InvalidOperationException("Attribute is already resident");
			}
			ushort id = this._records[0].CreateAttribute(attribute.Type, attribute.Name, this._context.AttributeDefinitions.IsIndexed(attribute.Type), attribute.Flags);
			AttributeRecord attribute2 = this._records[0].GetAttribute(id);
			IBuffer dataBuffer = attribute.GetDataBuffer();
			byte[] array = StreamUtilities.ReadExact(dataBuffer, 0L, (int)Math.Min((long)maxData, dataBuffer.Capacity));
			this.RemoveAttributeExtents(attribute);
			attribute.SetExtent(this._records[0].Reference, attribute2);
			attribute.GetDataBuffer().Write(0L, array, 0, array.Length);
			this.UpdateAttributeList();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006E9C File Offset: 0x0000509C
		private FileRecord GetFileRecord(FileRecordReference fileReference)
		{
			foreach (FileRecord fileRecord in this._records)
			{
				if ((ulong)fileRecord.MasterFileTableIndex == (ulong)fileReference.MftIndex)
				{
					return fileRecord;
				}
			}
			return null;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006F00 File Offset: 0x00005100
		private void RemoveFileRecord(FileRecordReference fileReference)
		{
			for (int i = 0; i < this._records.Count; i++)
			{
				if ((ulong)this._records[i].MasterFileTableIndex == (ulong)fileReference.MftIndex)
				{
					FileRecord fileRecord = this._records[i];
					if (fileRecord.Attributes.Count > 0)
					{
						throw new IOException("Attempting to remove non-empty MFT record");
					}
					this._context.Mft.RemoveRecord(fileReference);
					this._records.Remove(fileRecord);
					if (this._records.Count == 1)
					{
						NtfsAttribute attribute = this.GetAttribute(AttributeType.AttributeList, null);
						if (attribute != null)
						{
							this.RemoveAttribute(attribute);
						}
					}
				}
			}
		}

		// Token: 0x04000077 RID: 119
		private readonly List<NtfsAttribute> _attributes;

		// Token: 0x04000078 RID: 120
		protected INtfsContext _context;

		// Token: 0x04000079 RID: 121
		private readonly ObjectCache<string, Index> _indexCache;

		// Token: 0x0400007A RID: 122
		private readonly MasterFileTable _mft;

		// Token: 0x0400007B RID: 123
		private readonly List<FileRecord> _records;

		// Token: 0x0200006B RID: 107
		private class FileStream : SparseStream
		{
			// Token: 0x06000425 RID: 1061 RVA: 0x00015B67 File Offset: 0x00013D67
			public FileStream(File file, NtfsAttribute attr, FileAccess access)
			{
				this._file = file;
				this._attr = attr;
				this._wrapped = attr.Open(access);
			}

			// Token: 0x17000138 RID: 312
			// (get) Token: 0x06000426 RID: 1062 RVA: 0x00015B8A File Offset: 0x00013D8A
			public override bool CanRead
			{
				get
				{
					return this._wrapped.CanRead;
				}
			}

			// Token: 0x17000139 RID: 313
			// (get) Token: 0x06000427 RID: 1063 RVA: 0x00015B97 File Offset: 0x00013D97
			public override bool CanSeek
			{
				get
				{
					return this._wrapped.CanSeek;
				}
			}

			// Token: 0x1700013A RID: 314
			// (get) Token: 0x06000428 RID: 1064 RVA: 0x00015BA4 File Offset: 0x00013DA4
			public override bool CanWrite
			{
				get
				{
					return this._wrapped.CanWrite;
				}
			}

			// Token: 0x1700013B RID: 315
			// (get) Token: 0x06000429 RID: 1065 RVA: 0x00015BB1 File Offset: 0x00013DB1
			public override IEnumerable<StreamExtent> Extents
			{
				get
				{
					return this._wrapped.Extents;
				}
			}

			// Token: 0x1700013C RID: 316
			// (get) Token: 0x0600042A RID: 1066 RVA: 0x00015BBE File Offset: 0x00013DBE
			public override long Length
			{
				get
				{
					return this._wrapped.Length;
				}
			}

			// Token: 0x1700013D RID: 317
			// (get) Token: 0x0600042B RID: 1067 RVA: 0x00015BCB File Offset: 0x00013DCB
			// (set) Token: 0x0600042C RID: 1068 RVA: 0x00015BD8 File Offset: 0x00013DD8
			public override long Position
			{
				get
				{
					return this._wrapped.Position;
				}
				set
				{
					this._wrapped.Position = value;
				}
			}

			// Token: 0x0600042D RID: 1069 RVA: 0x00015BE6 File Offset: 0x00013DE6
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
				this._wrapped.Dispose();
			}

			// Token: 0x0600042E RID: 1070 RVA: 0x00015BFA File Offset: 0x00013DFA
			public override void Flush()
			{
				this._wrapped.Flush();
			}

			// Token: 0x0600042F RID: 1071 RVA: 0x00015C07 File Offset: 0x00013E07
			public override int Read(byte[] buffer, int offset, int count)
			{
				return this._wrapped.Read(buffer, offset, count);
			}

			// Token: 0x06000430 RID: 1072 RVA: 0x00015C17 File Offset: 0x00013E17
			public override long Seek(long offset, SeekOrigin origin)
			{
				return this._wrapped.Seek(offset, origin);
			}

			// Token: 0x06000431 RID: 1073 RVA: 0x00015C26 File Offset: 0x00013E26
			public override void SetLength(long value)
			{
				this.ChangeAttributeResidencyByLength(value);
				this._wrapped.SetLength(value);
			}

			// Token: 0x06000432 RID: 1074 RVA: 0x00015C3B File Offset: 0x00013E3B
			public override void Write(byte[] buffer, int offset, int count)
			{
				if (this._wrapped.Position + (long)count > this.Length)
				{
					this.ChangeAttributeResidencyByLength(this._wrapped.Position + (long)count);
				}
				this._wrapped.Write(buffer, offset, count);
			}

			// Token: 0x06000433 RID: 1075 RVA: 0x00015C75 File Offset: 0x00013E75
			public override void Clear(int count)
			{
				if (this._wrapped.Position + (long)count > this.Length)
				{
					this.ChangeAttributeResidencyByLength(this._wrapped.Position + (long)count);
				}
				this._wrapped.Clear(count);
			}

			// Token: 0x06000434 RID: 1076 RVA: 0x00015CAD File Offset: 0x00013EAD
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					this._file,
					".attr[",
					this._attr.Id,
					"]"
				});
			}

			// Token: 0x06000435 RID: 1077 RVA: 0x00015CE8 File Offset: 0x00013EE8
			private void ChangeAttributeResidencyByLength(long value)
			{
				if ((ulong)this._file._records[0].MasterFileTableIndex == 6UL)
				{
					return;
				}
				if (!this._attr.IsNonResident && value >= (long)((ulong)this._file.MaxMftRecordSize))
				{
					this._file.MakeAttributeNonResident(this._attr.Reference, (int)Math.Min(value, this._wrapped.Length));
					return;
				}
				if (this._attr.IsNonResident && value <= (long)((ulong)(this._file.MaxMftRecordSize / 4U)))
				{
					this._file.MakeAttributeResident(this._attr.Reference, (int)value);
				}
			}

			// Token: 0x040001F5 RID: 501
			private readonly NtfsAttribute _attr;

			// Token: 0x040001F6 RID: 502
			private readonly File _file;

			// Token: 0x040001F7 RID: 503
			private readonly SparseStream _wrapped;
		}
	}
}
