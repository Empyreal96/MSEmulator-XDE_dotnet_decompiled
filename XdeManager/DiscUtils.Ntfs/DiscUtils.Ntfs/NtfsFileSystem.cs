using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using DiscUtils.Internal;
using DiscUtils.Ntfs.Internals;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200003D RID: 61
	public sealed class NtfsFileSystem : DiscFileSystem, IClusterBasedFileSystem, IFileSystem, IWindowsFileSystem, IDiagnosticTraceable
	{
		// Token: 0x060002B0 RID: 688 RVA: 0x0000D8B0 File Offset: 0x0000BAB0
		public NtfsFileSystem(Stream stream) : base(new NtfsOptions())
		{
			this._context = new NtfsContext();
			this._context.RawStream = stream;
			this._context.Options = this.NtfsOptions;
			this._context.GetFileByIndex = new GetFileByIndexFn(this.GetFile);
			this._context.GetFileByRef = new GetFileByRefFn(this.GetFile);
			this._context.GetDirectoryByRef = new GetDirectoryByRefFn(this.GetDirectory);
			this._context.GetDirectoryByIndex = new GetDirectoryByIndexFn(this.GetDirectory);
			this._context.AllocateFile = new AllocateFileFn(this.AllocateFile);
			this._context.ForgetFile = new ForgetFileFn(this.ForgetFile);
			this._context.ReadOnly = !stream.CanWrite;
			this._fileCache = new ObjectCache<long, File>();
			stream.Position = 0L;
			byte[] bytes = StreamUtilities.ReadExact(stream, 512);
			this._context.BiosParameterBlock = BiosParameterBlock.FromBytes(bytes, 0);
			if (!this._context.BiosParameterBlock.IsValid(stream.Length))
			{
				throw new InvalidFileSystemException("BIOS Parameter Block is invalid for an NTFS file system");
			}
			if (this.NtfsOptions.ReadCacheEnabled)
			{
				BlockCacheSettings blockCacheSettings = new BlockCacheSettings();
				blockCacheSettings.BlockSize = this._context.BiosParameterBlock.BytesPerCluster;
				this._context.RawStream = new BlockCacheStream(SparseStream.FromStream(stream, Ownership.None), Ownership.None, blockCacheSettings);
			}
			this._context.Mft = new MasterFileTable(this._context);
			File file = new File(this._context, this._context.Mft.GetBootstrapRecord());
			this._fileCache[0L] = file;
			this._context.Mft.Initialize(file);
			File file2 = this.GetFile(3L);
			this._volumeInfo = file2.GetStream(AttributeType.VolumeInformation, null).GetContent<VolumeInformation>();
			this._context.ClusterBitmap = new ClusterBitmap(this.GetFile(6L));
			this._context.AttributeDefinitions = new AttributeDefinitions(this.GetFile(4L));
			this._context.UpperCase = new UpperCase(this.GetFile(10L));
			if (this._volumeInfo.Version >= 768)
			{
				this._context.SecurityDescriptors = new SecurityDescriptors(this.GetFile(9L));
				this._context.ObjectIds = new ObjectIds(this.GetFile(this.GetDirectoryEntry("$Extend\\$ObjId").Reference));
				this._context.ReparsePoints = new ReparsePoints(this.GetFile(this.GetDirectoryEntry("$Extend\\$Reparse").Reference));
				this._context.Quotas = new Quotas(this.GetFile(this.GetDirectoryEntry("$Extend\\$Quota").Reference));
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000DB7B File Offset: 0x0000BD7B
		private bool CreateShortNames
		{
			get
			{
				return this._context.Options.ShortNameCreation == ShortFileNameOption.Enabled || (this._context.Options.ShortNameCreation == ShortFileNameOption.UseVolumeFlag && (this._volumeInfo.Flags & VolumeInformationFlags.DisableShortNameCreation) == VolumeInformationFlags.None);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000DBBA File Offset: 0x0000BDBA
		public override string FriendlyName
		{
			get
			{
				return "Microsoft NTFS";
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000DBC1 File Offset: 0x0000BDC1
		public NtfsOptions NtfsOptions
		{
			get
			{
				return (NtfsOptions)this.Options;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000DBCE File Offset: 0x0000BDCE
		public override string VolumeLabel
		{
			get
			{
				return this.GetFile(3L).GetStream(AttributeType.VolumeName, null).GetContent<VolumeName>().Name;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000DBEA File Offset: 0x0000BDEA
		public override bool CanWrite
		{
			get
			{
				return !this._context.ReadOnly;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000DBFA File Offset: 0x0000BDFA
		public long ClusterSize
		{
			get
			{
				return (long)this._context.BiosParameterBlock.BytesPerCluster;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000DC0D File Offset: 0x0000BE0D
		public long TotalClusters
		{
			get
			{
				return MathUtilities.Ceil(this._context.BiosParameterBlock.TotalSectors64, (long)((ulong)this._context.BiosParameterBlock.SectorsPerCluster));
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000DC38 File Offset: 0x0000BE38
		public override void CopyFile(string sourceFile, string destinationFile, bool overwrite)
		{
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(Utilities.GetDirectoryFromPath(sourceFile));
				if (directoryEntry == null || !directoryEntry.IsDirectory)
				{
					throw new FileNotFoundException("No such file", sourceFile);
				}
				DirectoryEntry entryByName = this.GetDirectory(directoryEntry.Reference).GetEntryByName(Utilities.GetFileFromPath(sourceFile));
				if (entryByName == null || entryByName.IsDirectory)
				{
					throw new FileNotFoundException("No such file", sourceFile);
				}
				File file = this.GetFile(entryByName.Reference);
				DirectoryEntry directoryEntry2 = this.GetDirectoryEntry(Utilities.GetDirectoryFromPath(destinationFile));
				if (directoryEntry2 == null || !directoryEntry2.IsDirectory)
				{
					throw new FileNotFoundException("Destination directory not found", destinationFile);
				}
				Directory directory = this.GetDirectory(directoryEntry2.Reference);
				DirectoryEntry entryByName2 = directory.GetEntryByName(Utilities.GetFileFromPath(destinationFile));
				if (entryByName2 != null && !entryByName2.IsDirectory)
				{
					if (!overwrite)
					{
						throw new IOException("Destination file already exists");
					}
					if (entryByName2.Reference.MftIndex == entryByName.Reference.MftIndex)
					{
						throw new IOException("Destination file already exists and is the source file");
					}
					File file2 = this.GetFile(entryByName2.Reference);
					directory.RemoveEntry(entryByName2);
					if (file2.HardLinkCount == 0)
					{
						file2.Delete();
					}
				}
				File file3 = File.CreateNew(this._context, directory.StandardInformation.FileAttributes);
				foreach (NtfsStream ntfsStream in file.AllStreams)
				{
					NtfsStream ntfsStream2 = file3.GetStream(ntfsStream.AttributeType, ntfsStream.Name);
					AttributeType attributeType = ntfsStream.AttributeType;
					if (attributeType != AttributeType.StandardInformation)
					{
						if (attributeType != AttributeType.Data)
						{
							continue;
						}
						if (ntfsStream2 == null)
						{
							ntfsStream2 = file3.CreateStream(ntfsStream.AttributeType, ntfsStream.Name);
						}
						using (SparseStream sparseStream = ntfsStream.Open(FileAccess.Read))
						{
							using (SparseStream sparseStream2 = ntfsStream2.Open(FileAccess.Write))
							{
								byte[] array = new byte[65536L];
								int num;
								do
								{
									num = sparseStream.Read(array, 0, array.Length);
									sparseStream2.Write(array, 0, num);
								}
								while (num != 0);
								continue;
							}
						}
					}
					StandardInformation content = ntfsStream.GetContent<StandardInformation>();
					ntfsStream2.SetContent<StandardInformation>(content);
				}
				this.AddFileToDirectory(file3, directory, Utilities.GetFileFromPath(destinationFile), null);
				directoryEntry2.UpdateFrom(directory);
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000DEF4 File Offset: 0x0000C0F4
		public override void CreateDirectory(string path)
		{
			this.CreateDirectory(path, null);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000DF00 File Offset: 0x0000C100
		public override void DeleteDirectory(string path)
		{
			using (new NtfsTransaction())
			{
				if (string.IsNullOrEmpty(path))
				{
					throw new IOException("Unable to delete root directory");
				}
				string directoryFromPath = Utilities.GetDirectoryFromPath(path);
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(directoryFromPath);
				if (directoryEntry == null || !directoryEntry.IsDirectory)
				{
					throw new DirectoryNotFoundException("No such directory: " + path);
				}
				Directory directory = this.GetDirectory(directoryEntry.Reference);
				DirectoryEntry entryByName = directory.GetEntryByName(Utilities.GetFileFromPath(path));
				if (entryByName == null || !entryByName.IsDirectory)
				{
					throw new DirectoryNotFoundException("No such directory: " + path);
				}
				Directory directory2 = this.GetDirectory(entryByName.Reference);
				if (!directory2.IsEmpty)
				{
					throw new IOException("Unable to delete non-empty directory");
				}
				if ((entryByName.Details.FileAttributes & FileAttributes.ReparsePoint) != (FileAttributes)0)
				{
					this.RemoveReparsePoint(directory2);
				}
				NtfsFileSystem.RemoveFileFromDirectory(directory, directory2, Utilities.GetFileFromPath(path));
				if (directory2.HardLinkCount == 0)
				{
					directory2.Delete();
				}
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000DFFC File Offset: 0x0000C1FC
		public override void DeleteFile(string path)
		{
			using (new NtfsTransaction())
			{
				string text;
				AttributeType attributeType;
				string path2 = this.ParsePath(path, out text, out attributeType);
				string directoryFromPath = Utilities.GetDirectoryFromPath(path2);
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(directoryFromPath);
				if (directoryEntry == null || !directoryEntry.IsDirectory)
				{
					throw new FileNotFoundException("No such file", path);
				}
				Directory directory = this.GetDirectory(directoryEntry.Reference);
				DirectoryEntry entryByName = directory.GetEntryByName(Utilities.GetFileFromPath(path2));
				if (entryByName == null || entryByName.IsDirectory)
				{
					throw new FileNotFoundException("No such file", path);
				}
				File file = this.GetFile(entryByName.Reference);
				if (string.IsNullOrEmpty(text) && attributeType == AttributeType.Data)
				{
					if ((entryByName.Details.FileAttributes & FileAttributes.ReparsePoint) != (FileAttributes)0)
					{
						this.RemoveReparsePoint(file);
					}
					NtfsFileSystem.RemoveFileFromDirectory(directory, file, Utilities.GetFileFromPath(path));
					if (file.HardLinkCount == 0)
					{
						file.Delete();
					}
				}
				else
				{
					NtfsStream stream = file.GetStream(attributeType, text);
					if (stream == null)
					{
						throw new FileNotFoundException("No such attribute: " + text, path);
					}
					file.RemoveStream(stream);
				}
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000E120 File Offset: 0x0000C320
		public override bool DirectoryExists(string path)
		{
			bool result;
			using (new NtfsTransaction())
			{
				if (string.IsNullOrEmpty(path))
				{
					result = true;
				}
				else
				{
					DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
					result = (directoryEntry != null && (directoryEntry.Details.FileAttributes & FileAttributes.Directory) > (FileAttributes)0);
				}
			}
			return result;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000E17C File Offset: 0x0000C37C
		public override bool FileExists(string path)
		{
			bool result;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				result = (directoryEntry != null && (directoryEntry.Details.FileAttributes & FileAttributes.Directory) == (FileAttributes)0);
			}
			return result;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000E1CC File Offset: 0x0000C3CC
		public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			string[] result;
			using (new NtfsTransaction())
			{
				Regex regex = Utilities.ConvertWildcardsToRegEx(searchPattern);
				List<string> list = new List<string>();
				this.DoSearch(list, path, regex, searchOption == SearchOption.AllDirectories, true, false);
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000E220 File Offset: 0x0000C420
		public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			string[] result;
			using (new NtfsTransaction())
			{
				Regex regex = Utilities.ConvertWildcardsToRegEx(searchPattern);
				List<string> list = new List<string>();
				this.DoSearch(list, path, regex, searchOption == SearchOption.AllDirectories, false, true);
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000E274 File Offset: 0x0000C474
		public override string[] GetFileSystemEntries(string path)
		{
			string[] result;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "The directory '{0}' does not exist", new object[]
					{
						path
					}));
				}
				result = Utilities.Map<DirectoryEntry, string>(this.GetDirectory(directoryEntry.Reference).GetAllEntries(true), (DirectoryEntry m) => Utilities.CombinePaths(path, m.Details.FileName));
			}
			return result;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000E30C File Offset: 0x0000C50C
		public override string[] GetFileSystemEntries(string path, string searchPattern)
		{
			string[] result;
			using (new NtfsTransaction())
			{
				Regex regex = Utilities.ConvertWildcardsToRegEx(searchPattern);
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "The directory '{0}' does not exist", new object[]
					{
						path
					}));
				}
				Directory directory = this.GetDirectory(directoryEntry.Reference);
				List<string> list = new List<string>();
				foreach (DirectoryEntry directoryEntry2 in directory.GetAllEntries(true))
				{
					if (regex.IsMatch(directoryEntry2.Details.FileName))
					{
						list.Add(Utilities.CombinePaths(path, directoryEntry2.Details.FileName));
					}
				}
				result = list.ToArray();
			}
			return result;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000E3F0 File Offset: 0x0000C5F0
		public override void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName)
		{
			using (new NtfsTransaction())
			{
				using (new NtfsTransaction())
				{
					DirectoryEntry directoryEntry = this.GetDirectoryEntry(Utilities.GetDirectoryFromPath(sourceDirectoryName));
					if (directoryEntry == null || !directoryEntry.IsDirectory)
					{
						throw new DirectoryNotFoundException("No such directory: " + sourceDirectoryName);
					}
					Directory directory = this.GetDirectory(directoryEntry.Reference);
					DirectoryEntry entryByName = directory.GetEntryByName(Utilities.GetFileFromPath(sourceDirectoryName));
					if (entryByName == null || !entryByName.IsDirectory)
					{
						throw new DirectoryNotFoundException("No such directory: " + sourceDirectoryName);
					}
					File file = this.GetFile(entryByName.Reference);
					DirectoryEntry directoryEntry2 = this.GetDirectoryEntry(Utilities.GetDirectoryFromPath(destinationDirectoryName));
					if (directoryEntry2 == null || !directoryEntry2.IsDirectory)
					{
						throw new DirectoryNotFoundException("Destination directory not found: " + destinationDirectoryName);
					}
					Directory directory2 = this.GetDirectory(directoryEntry2.Reference);
					if (directory2.GetEntryByName(Utilities.GetFileFromPath(destinationDirectoryName)) != null)
					{
						throw new IOException("Destination directory already exists");
					}
					NtfsFileSystem.RemoveFileFromDirectory(directory, file, entryByName.Details.FileName);
					this.AddFileToDirectory(file, directory2, Utilities.GetFileFromPath(destinationDirectoryName), null);
				}
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000E520 File Offset: 0x0000C720
		public override void MoveFile(string sourceName, string destinationName, bool overwrite)
		{
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(Utilities.GetDirectoryFromPath(sourceName));
				if (directoryEntry == null || !directoryEntry.IsDirectory)
				{
					throw new FileNotFoundException("No such file", sourceName);
				}
				Directory directory = this.GetDirectory(directoryEntry.Reference);
				DirectoryEntry entryByName = directory.GetEntryByName(Utilities.GetFileFromPath(sourceName));
				if (entryByName == null || entryByName.IsDirectory)
				{
					throw new FileNotFoundException("No such file", sourceName);
				}
				File file = this.GetFile(entryByName.Reference);
				DirectoryEntry directoryEntry2 = this.GetDirectoryEntry(Utilities.GetDirectoryFromPath(destinationName));
				if (directoryEntry2 == null || !directoryEntry2.IsDirectory)
				{
					throw new FileNotFoundException("Destination directory not found", destinationName);
				}
				Directory directory2 = this.GetDirectory(directoryEntry2.Reference);
				DirectoryEntry entryByName2 = directory2.GetEntryByName(Utilities.GetFileFromPath(destinationName));
				if (entryByName2 != null && !entryByName2.IsDirectory)
				{
					if (!overwrite)
					{
						throw new IOException("Destination file already exists");
					}
					if (entryByName2.Reference.MftIndex == entryByName.Reference.MftIndex)
					{
						throw new IOException("Destination file already exists and is the source file");
					}
					File file2 = this.GetFile(entryByName2.Reference);
					directory2.RemoveEntry(entryByName2);
					if (file2.HardLinkCount == 0)
					{
						file2.Delete();
					}
				}
				NtfsFileSystem.RemoveFileFromDirectory(directory, file, entryByName.Details.FileName);
				this.AddFileToDirectory(file, directory2, Utilities.GetFileFromPath(destinationName), null);
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000E694 File Offset: 0x0000C894
		public override SparseStream OpenFile(string path, FileMode mode, FileAccess access)
		{
			return this.OpenFile(path, mode, access, null);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000E6A0 File Offset: 0x0000C8A0
		public override FileAttributes GetAttributes(string path)
		{
			FileAttributes fileAttributes;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				fileAttributes = directoryEntry.Details.FileAttributes;
			}
			return fileAttributes;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000E6F4 File Offset: 0x0000C8F4
		public override void SetAttributes(string path, FileAttributes newValue)
		{
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				FileAttributes fileAttributes = directoryEntry.Details.FileAttributes ^ newValue;
				if (fileAttributes != (FileAttributes)0)
				{
					if ((fileAttributes & (FileAttributes.Directory | FileAttributes.ReparsePoint | FileAttributes.Offline)) != (FileAttributes)0)
					{
						throw new ArgumentException("Attempt to change attributes that are read-only", "newValue");
					}
					File file = this.GetFile(directoryEntry.Reference);
					if ((fileAttributes & FileAttributes.SparseFile) != (FileAttributes)0)
					{
						if (directoryEntry.IsDirectory)
						{
							throw new ArgumentException("Attempt to change sparse attribute on a directory", "newValue");
						}
						if ((newValue & FileAttributes.SparseFile) == (FileAttributes)0)
						{
							throw new ArgumentException("Attempt to remove sparse attribute from file", "newValue");
						}
						NtfsAttribute attribute = file.GetAttribute(AttributeType.Data, null);
						if ((attribute.Flags & AttributeFlags.Compressed) != AttributeFlags.None)
						{
							throw new ArgumentException("Attempt to mark compressed file as sparse", "newValue");
						}
						attribute.Flags |= AttributeFlags.Sparse;
						if (attribute.IsNonResident)
						{
							attribute.CompressedDataSize = attribute.PrimaryRecord.AllocatedLength;
							attribute.CompressionUnitSize = 16;
							((NonResidentAttributeBuffer)attribute.RawBuffer).AlignVirtualClusterCount();
						}
					}
					if ((fileAttributes & FileAttributes.Compressed) != (FileAttributes)0 && !directoryEntry.IsDirectory)
					{
						if ((newValue & FileAttributes.Compressed) == (FileAttributes)0)
						{
							throw new ArgumentException("Attempt to remove compressed attribute from file", "newValue");
						}
						NtfsAttribute attribute2 = file.GetAttribute(AttributeType.Data, null);
						if ((attribute2.Flags & AttributeFlags.Sparse) != AttributeFlags.None)
						{
							throw new ArgumentException("Attempt to mark sparse file as compressed", "newValue");
						}
						attribute2.Flags |= AttributeFlags.Compressed;
						if (attribute2.IsNonResident)
						{
							attribute2.CompressedDataSize = attribute2.PrimaryRecord.AllocatedLength;
							attribute2.CompressionUnitSize = 16;
							((NonResidentAttributeBuffer)attribute2.RawBuffer).AlignVirtualClusterCount();
						}
					}
					NtfsFileSystem.UpdateStandardInformation(directoryEntry, file, delegate(StandardInformation si)
					{
						si.FileAttributes = FileNameRecord.SetAttributes(newValue, si.FileAttributes);
					});
				}
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000E908 File Offset: 0x0000CB08
		public override DateTime GetCreationTimeUtc(string path)
		{
			DateTime creationTime;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				creationTime = directoryEntry.Details.CreationTime;
			}
			return creationTime;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000E95C File Offset: 0x0000CB5C
		public override void SetCreationTimeUtc(string path, DateTime newTime)
		{
			using (new NtfsTransaction())
			{
				this.UpdateStandardInformation(path, delegate(StandardInformation si)
				{
					si.CreationTime = newTime;
				});
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000E9AC File Offset: 0x0000CBAC
		public override DateTime GetLastAccessTimeUtc(string path)
		{
			DateTime lastAccessTime;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				lastAccessTime = directoryEntry.Details.LastAccessTime;
			}
			return lastAccessTime;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000EA00 File Offset: 0x0000CC00
		public override void SetLastAccessTimeUtc(string path, DateTime newTime)
		{
			using (new NtfsTransaction())
			{
				this.UpdateStandardInformation(path, delegate(StandardInformation si)
				{
					si.LastAccessTime = newTime;
				});
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000EA50 File Offset: 0x0000CC50
		public override DateTime GetLastWriteTimeUtc(string path)
		{
			DateTime modificationTime;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				modificationTime = directoryEntry.Details.ModificationTime;
			}
			return modificationTime;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000EAA4 File Offset: 0x0000CCA4
		public override void SetLastWriteTimeUtc(string path, DateTime newTime)
		{
			using (new NtfsTransaction())
			{
				this.UpdateStandardInformation(path, delegate(StandardInformation si)
				{
					si.ModificationTime = newTime;
				});
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
		public override long GetFileLength(string path)
		{
			long result;
			using (new NtfsTransaction())
			{
				string text;
				AttributeType attributeType;
				string path2 = this.ParsePath(path, out text, out attributeType);
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path2);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				if (this.NtfsOptions.FileLengthFromDirectoryEntries && text == null && attributeType == AttributeType.Data)
				{
					result = (long)directoryEntry.Details.RealSize;
				}
				else
				{
					NtfsAttribute attribute = this.GetFile(directoryEntry.Reference).GetAttribute(attributeType, text);
					if (attribute == null)
					{
						throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "No such attribute '{0}({1})'", new object[]
						{
							text,
							attributeType
						}));
					}
					result = attribute.Length;
				}
			}
			return result;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000EBC0 File Offset: 0x0000CDC0
		public long ClusterToOffset(long cluster)
		{
			return cluster * this.ClusterSize;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000EBCA File Offset: 0x0000CDCA
		public long OffsetToCluster(long offset)
		{
			return offset / this.ClusterSize;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000EBD4 File Offset: 0x0000CDD4
		public Range<long, long>[] PathToClusters(string path)
		{
			string path2;
			string text;
			NtfsFileSystem.SplitPath(path, out path2, out text);
			DirectoryEntry directoryEntry = this.GetDirectoryEntry(path2);
			if (directoryEntry == null || directoryEntry.IsDirectory)
			{
				throw new FileNotFoundException("No such file", path);
			}
			NtfsStream stream = this.GetFile(directoryEntry.Reference).GetStream(AttributeType.Data, text);
			if (stream == null)
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "File does not contain '{0}' data attribute", new object[]
				{
					text
				}), path);
			}
			return stream.GetClusters();
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000EC4C File Offset: 0x0000CE4C
		public StreamExtent[] PathToExtents(string path)
		{
			string path2;
			string text;
			NtfsFileSystem.SplitPath(path, out path2, out text);
			DirectoryEntry directoryEntry = this.GetDirectoryEntry(path2);
			if (directoryEntry == null || directoryEntry.IsDirectory)
			{
				throw new FileNotFoundException("No such file", path);
			}
			NtfsStream stream = this.GetFile(directoryEntry.Reference).GetStream(AttributeType.Data, text);
			if (stream == null)
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "File does not contain '{0}' data attribute", new object[]
				{
					text
				}), path);
			}
			return stream.GetAbsoluteExtents();
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000ECC3 File Offset: 0x0000CEC3
		public ClusterMap BuildClusterMap()
		{
			return this._context.Mft.GetClusterMap();
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000ECD8 File Offset: 0x0000CED8
		public override byte[] ReadBootCode()
		{
			byte[] result;
			using (Stream stream = this.OpenFile("\\$Boot", FileMode.Open))
			{
				result = StreamUtilities.ReadExact(stream, (int)stream.Length);
			}
			return result;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000ED20 File Offset: 0x0000CF20
		public void Dump(TextWriter writer, string linePrefix)
		{
			writer.WriteLine(linePrefix + "NTFS File System Dump");
			writer.WriteLine(linePrefix + "=====================");
			writer.WriteLine(linePrefix);
			this._context.BiosParameterBlock.Dump(writer, linePrefix);
			if (this._context.SecurityDescriptors != null)
			{
				writer.WriteLine(linePrefix);
				this._context.SecurityDescriptors.Dump(writer, linePrefix);
			}
			if (this._context.ObjectIds != null)
			{
				writer.WriteLine(linePrefix);
				this._context.ObjectIds.Dump(writer, linePrefix);
			}
			if (this._context.ReparsePoints != null)
			{
				writer.WriteLine(linePrefix);
				this._context.ReparsePoints.Dump(writer, linePrefix);
			}
			if (this._context.Quotas != null)
			{
				writer.WriteLine(linePrefix);
				this._context.Quotas.Dump(writer, linePrefix);
			}
			writer.WriteLine(linePrefix);
			this.GetDirectory(5L).Dump(writer, linePrefix);
			writer.WriteLine(linePrefix);
			writer.WriteLine(linePrefix + "FULL FILE LISTING");
			foreach (FileRecord baseRecord in this._context.Mft.Records)
			{
				File file = new File(this._context, baseRecord);
				file.Dump(writer, linePrefix);
				foreach (NtfsStream ntfsStream in file.AllStreams)
				{
					if (ntfsStream.AttributeType == AttributeType.IndexRoot)
					{
						try
						{
							writer.WriteLine(linePrefix + "  INDEX (" + ntfsStream.Name + ")");
							file.GetIndex(ntfsStream.Name).Dump(writer, linePrefix + "    ");
						}
						catch (Exception arg)
						{
							writer.WriteLine(linePrefix + "!Exception: " + arg);
						}
					}
				}
			}
			writer.WriteLine(linePrefix);
			writer.WriteLine(linePrefix + "DIRECTORY TREE");
			writer.WriteLine(linePrefix + "\\ (5)");
			this.DumpDirectory(this.GetDirectory(5L), writer, linePrefix);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000EF6C File Offset: 0x0000D16C
		public bool HasHardLinks(string path)
		{
			return this.GetHardLinkCount(path) > 1;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000EF78 File Offset: 0x0000D178
		public RawSecurityDescriptor GetSecurity(string path)
		{
			RawSecurityDescriptor result;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				File file = this.GetFile(directoryEntry.Reference);
				result = this.DoGetSecurity(file);
			}
			return result;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000EFD4 File Offset: 0x0000D1D4
		public void SetSecurity(string path, RawSecurityDescriptor securityDescriptor)
		{
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				File file = this.GetFile(directoryEntry.Reference);
				this.DoSetSecurity(file, securityDescriptor);
				directoryEntry.UpdateFrom(file);
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000F038 File Offset: 0x0000D238
		public void SetReparsePoint(string path, ReparsePoint reparsePoint)
		{
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				File file = this.GetFile(directoryEntry.Reference);
				NtfsStream ntfsStream = file.GetStream(AttributeType.ReparsePoint, null);
				if (ntfsStream != null)
				{
					using (Stream stream = ntfsStream.Open(FileAccess.Read))
					{
						byte[] buffer = StreamUtilities.ReadExact(stream, (int)stream.Length);
						ReparsePointRecord reparsePointRecord = new ReparsePointRecord();
						reparsePointRecord.ReadFrom(buffer, 0);
						this._context.ReparsePoints.Remove(reparsePointRecord.Tag, directoryEntry.Reference);
						goto IL_9E;
					}
				}
				ntfsStream = file.CreateStream(AttributeType.ReparsePoint, null);
				IL_9E:
				ReparsePointRecord reparsePointRecord2 = new ReparsePointRecord();
				reparsePointRecord2.Tag = (uint)reparsePoint.Tag;
				reparsePointRecord2.Content = reparsePoint.Content;
				byte[] array = new byte[reparsePointRecord2.Size];
				reparsePointRecord2.WriteTo(array, 0);
				using (Stream stream2 = ntfsStream.Open(FileAccess.ReadWrite))
				{
					stream2.Write(array, 0, array.Length);
					stream2.SetLength((long)array.Length);
				}
				NtfsStream stream3 = file.GetStream(AttributeType.StandardInformation, null);
				StandardInformation content = stream3.GetContent<StandardInformation>();
				content.FileAttributes |= FileAttributeFlags.ReparsePoint;
				stream3.SetContent<StandardInformation>(content);
				directoryEntry.Details.EASizeOrReparsePointTag = reparsePointRecord2.Tag;
				directoryEntry.UpdateFrom(file);
				file.UpdateRecordInMft();
				this._context.ReparsePoints.Add(reparsePointRecord2.Tag, directoryEntry.Reference);
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000F210 File Offset: 0x0000D410
		public ReparsePoint GetReparsePoint(string path)
		{
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				NtfsStream stream = this.GetFile(directoryEntry.Reference).GetStream(AttributeType.ReparsePoint, null);
				if (stream != null)
				{
					ReparsePointRecord reparsePointRecord = new ReparsePointRecord();
					using (Stream stream2 = stream.Open(FileAccess.Read))
					{
						byte[] buffer = StreamUtilities.ReadExact(stream2, (int)stream2.Length);
						reparsePointRecord.ReadFrom(buffer, 0);
						return new ReparsePoint((int)reparsePointRecord.Tag, reparsePointRecord.Content);
					}
				}
			}
			return null;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000F2CC File Offset: 0x0000D4CC
		public void RemoveReparsePoint(string path)
		{
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				File file = this.GetFile(directoryEntry.Reference);
				this.RemoveReparsePoint(file);
				directoryEntry.UpdateFrom(file);
				file.UpdateRecordInMft();
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000F334 File Offset: 0x0000D534
		public string GetShortName(string path)
		{
			string result;
			using (new NtfsTransaction())
			{
				string directoryFromPath = Utilities.GetDirectoryFromPath(path);
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(directoryFromPath);
				if (directoryEntry == null || (directoryEntry.Details.FileAttributes & FileAttributes.Directory) == (FileAttributes)0)
				{
					throw new DirectoryNotFoundException("Parent directory not found");
				}
				Directory directory = this.GetDirectory(directoryEntry.Reference);
				if (directory == null)
				{
					throw new DirectoryNotFoundException("Parent directory not found");
				}
				DirectoryEntry entryByName = directory.GetEntryByName(Utilities.GetFileFromPath(path));
				if (entryByName == null)
				{
					throw new FileNotFoundException("Path not found", path);
				}
				if (entryByName.Details.FileNameNamespace == FileNameNamespace.Dos)
				{
					result = entryByName.Details.FileName;
				}
				else
				{
					if (entryByName.Details.FileNameNamespace == FileNameNamespace.Win32)
					{
						foreach (NtfsStream ntfsStream in this.GetFile(entryByName.Reference).GetStreams(AttributeType.FileName, null))
						{
							FileNameRecord content = ntfsStream.GetContent<FileNameRecord>();
							if (content.ParentDirectory.Equals(entryByName.Details.ParentDirectory) && content.FileNameNamespace == FileNameNamespace.Dos)
							{
								return content.FileName;
							}
						}
					}
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000F498 File Offset: 0x0000D698
		public void SetShortName(string path, string shortName)
		{
			if (!Utilities.Is8Dot3(shortName))
			{
				throw new ArgumentException("Short name is not a valid 8.3 file name", "shortName");
			}
			using (new NtfsTransaction())
			{
				string directoryFromPath = Utilities.GetDirectoryFromPath(path);
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(directoryFromPath);
				if (directoryEntry == null || (directoryEntry.Details.FileAttributes & FileAttributes.Directory) == (FileAttributes)0)
				{
					throw new DirectoryNotFoundException("Parent directory not found");
				}
				Directory directory = this.GetDirectory(directoryEntry.Reference);
				if (directory == null)
				{
					throw new DirectoryNotFoundException("Parent directory not found");
				}
				DirectoryEntry entryByName = directory.GetEntryByName(Utilities.GetFileFromPath(path));
				if (entryByName == null)
				{
					throw new FileNotFoundException("Path not found", path);
				}
				bool fileNameNamespace = entryByName.Details.FileNameNamespace != FileNameNamespace.Posix;
				File file = this.GetFile(entryByName.Reference);
				if (!fileNameNamespace && file.HasWin32OrDosName)
				{
					throw new InvalidOperationException("Cannot set a short name on hard links");
				}
				if (entryByName.Details.FileNameNamespace != FileNameNamespace.Win32)
				{
					directory.RemoveEntry(entryByName);
					directory.AddEntry(file, entryByName.Details.FileName, FileNameNamespace.Win32);
				}
				foreach (NtfsStream ntfsStream in new List<NtfsStream>(file.GetStreams(AttributeType.FileName, null)))
				{
					FileNameRecord content = ntfsStream.GetContent<FileNameRecord>();
					if (content.ParentDirectory.Equals(entryByName.Details.ParentDirectory) && content.FileNameNamespace == FileNameNamespace.Dos)
					{
						DirectoryEntry entryByName2 = directory.GetEntryByName(content.FileName);
						directory.RemoveEntry(entryByName2);
					}
				}
				directory.AddEntry(file, shortName, FileNameNamespace.Dos);
				directoryEntry.UpdateFrom(directory);
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000F660 File Offset: 0x0000D860
		public WindowsFileInformation GetFileStandardInformation(string path)
		{
			WindowsFileInformation result;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				File file = this.GetFile(directoryEntry.Reference);
				StandardInformation standardInformation = file.StandardInformation;
				result = new WindowsFileInformation
				{
					CreationTime = standardInformation.CreationTime,
					LastAccessTime = standardInformation.LastAccessTime,
					ChangeTime = standardInformation.MftChangedTime,
					LastWriteTime = standardInformation.ModificationTime,
					FileAttributes = StandardInformation.ConvertFlags(standardInformation.FileAttributes, file.IsDirectory)
				};
			}
			return result;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F70C File Offset: 0x0000D90C
		public void SetFileStandardInformation(string path, WindowsFileInformation info)
		{
			using (new NtfsTransaction())
			{
				this.UpdateStandardInformation(path, delegate(StandardInformation si)
				{
					si.CreationTime = info.CreationTime;
					si.LastAccessTime = info.LastAccessTime;
					si.MftChangedTime = info.ChangeTime;
					si.ModificationTime = info.LastWriteTime;
					si.FileAttributes = StandardInformation.SetFileAttributes(info.FileAttributes, si.FileAttributes);
				});
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000F75C File Offset: 0x0000D95C
		public long GetFileId(string path)
		{
			long value;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				value = (long)directoryEntry.Reference.Value;
			}
			return value;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F7B0 File Offset: 0x0000D9B0
		public string[] GetAlternateDataStreams(string path)
		{
			DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
			if (directoryEntry == null)
			{
				throw new FileNotFoundException("File not found", path);
			}
			File file = this.GetFile(directoryEntry.Reference);
			List<string> list = new List<string>();
			foreach (NtfsStream ntfsStream in file.AllStreams)
			{
				if (ntfsStream.AttributeType == AttributeType.Data && !string.IsNullOrEmpty(ntfsStream.Name))
				{
					list.Add(ntfsStream.Name);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F84C File Offset: 0x0000DA4C
		public static NtfsFileSystem Format(Stream stream, string label, Geometry diskGeometry, long firstSector, long sectorCount)
		{
			return new NtfsFormatter
			{
				Label = label,
				DiskGeometry = diskGeometry,
				FirstSector = firstSector,
				SectorCount = sectorCount
			}.Format(stream);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F876 File Offset: 0x0000DA76
		public static NtfsFileSystem Format(Stream stream, string label, Geometry diskGeometry, long firstSector, long sectorCount, byte[] bootCode)
		{
			return new NtfsFormatter
			{
				Label = label,
				DiskGeometry = diskGeometry,
				FirstSector = firstSector,
				SectorCount = sectorCount,
				BootCode = bootCode
			}.Format(stream);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
		public static NtfsFileSystem Format(Stream stream, string label, Geometry diskGeometry, long firstSector, long sectorCount, NtfsFormatOptions options)
		{
			return new NtfsFormatter
			{
				Label = label,
				DiskGeometry = diskGeometry,
				FirstSector = firstSector,
				SectorCount = sectorCount,
				BootCode = options.BootCode,
				ComputerAccount = options.ComputerAccount
			}.Format(stream);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F8F8 File Offset: 0x0000DAF8
		public static NtfsFileSystem Format(VolumeInfo volume, string label)
		{
			return new NtfsFormatter
			{
				Label = label,
				DiskGeometry = (volume.BiosGeometry ?? Geometry.Null),
				FirstSector = volume.PhysicalStartSector,
				SectorCount = volume.Length / 512L
			}.Format(volume.Open());
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F950 File Offset: 0x0000DB50
		public static NtfsFileSystem Format(VolumeInfo volume, string label, byte[] bootCode)
		{
			return new NtfsFormatter
			{
				Label = label,
				DiskGeometry = (volume.BiosGeometry ?? Geometry.Null),
				FirstSector = volume.PhysicalStartSector,
				SectorCount = volume.Length / 512L,
				BootCode = bootCode
			}.Format(volume.Open());
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
		public static NtfsFileSystem Format(VolumeInfo volume, string label, NtfsFormatOptions options)
		{
			return new NtfsFormatter
			{
				Label = label,
				DiskGeometry = (volume.BiosGeometry ?? Geometry.Null),
				FirstSector = volume.PhysicalStartSector,
				SectorCount = volume.Length / 512L,
				BootCode = options.BootCode,
				ComputerAccount = options.ComputerAccount
			}.Format(volume.Open());
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000FA20 File Offset: 0x0000DC20
		public static bool Detect(Stream stream)
		{
			if (stream.Length < 512L)
			{
				return false;
			}
			stream.Position = 0L;
			return BiosParameterBlock.FromBytes(StreamUtilities.ReadExact(stream, 512), 0).IsValid(stream.Length);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000FA56 File Offset: 0x0000DC56
		public MasterFileTable GetMasterFileTable()
		{
			return new MasterFileTable(this._context, this._context.Mft);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000FA70 File Offset: 0x0000DC70
		public void CreateDirectory(string path, NewFileOptions options)
		{
			using (new NtfsTransaction())
			{
				string[] array = path.Split(new char[]
				{
					'\\'
				}, StringSplitOptions.RemoveEmptyEntries);
				Directory directory = this.GetDirectory(5L);
				DirectoryEntry directoryEntry = directory.DirectoryEntry;
				int i = 0;
				while (i < array.Length)
				{
					DirectoryEntry directoryEntry2 = directory.GetEntryByName(array[i]);
					if (directoryEntry2 == null)
					{
						FileAttributeFlags fileAttributeFlags = directory.StandardInformation.FileAttributes;
						if (options != null && options.Compressed != null)
						{
							if (options.Compressed.Value)
							{
								fileAttributeFlags |= FileAttributeFlags.Compressed;
							}
							else
							{
								fileAttributeFlags &= ~FileAttributeFlags.Compressed;
							}
						}
						Directory directory2 = Directory.CreateNew(this._context, fileAttributeFlags);
						try
						{
							directoryEntry2 = this.AddFileToDirectory(directory2, directory, array[i], options);
							RawSecurityDescriptor parent = this.DoGetSecurity(directory);
							RawSecurityDescriptor securityDescriptor;
							if (options != null && options.SecurityDescriptor != null)
							{
								securityDescriptor = options.SecurityDescriptor;
							}
							else
							{
								securityDescriptor = SecurityDescriptor.CalcNewObjectDescriptor(parent, false);
							}
							this.DoSetSecurity(directory2, securityDescriptor);
							directoryEntry2.UpdateFrom(directory2);
							directoryEntry.UpdateFrom(directory);
							directory = directory2;
							goto IL_111;
						}
						finally
						{
							if (directory2.HardLinkCount == 0)
							{
								directory2.Delete();
							}
						}
						goto IL_103;
					}
					goto IL_103;
					IL_111:
					directoryEntry = directoryEntry2;
					i++;
					continue;
					IL_103:
					directory = this.GetDirectory(directoryEntry2.Reference);
					goto IL_111;
				}
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000FBE4 File Offset: 0x0000DDE4
		public SparseStream OpenFile(string path, FileMode mode, FileAccess access, NewFileOptions options)
		{
			SparseStream result;
			using (new NtfsTransaction())
			{
				string text;
				AttributeType attributeType;
				string path2 = this.ParsePath(path, out text, out attributeType);
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path2);
				if (directoryEntry == null)
				{
					if (mode == FileMode.Open)
					{
						throw new FileNotFoundException("No such file", path);
					}
					directoryEntry = this.CreateNewFile(path2, options);
				}
				else if (mode == FileMode.CreateNew)
				{
					throw new IOException("File already exists");
				}
				if ((directoryEntry.Details.FileAttributes & FileAttributes.Directory) != (FileAttributes)0 && attributeType == AttributeType.Data)
				{
					throw new IOException("Attempt to open directory as a file");
				}
				File file = this.GetFile(directoryEntry.Reference);
				if (file.GetStream(attributeType, text) == null)
				{
					if (mode != FileMode.Create && mode != FileMode.OpenOrCreate)
					{
						throw new FileNotFoundException("No such attribute on file", path);
					}
					file.CreateStream(attributeType, text);
				}
				SparseStream sparseStream = new NtfsFileStream(this, directoryEntry, attributeType, text, access);
				if (mode == FileMode.Create || mode == FileMode.Truncate)
				{
					sparseStream.SetLength(0L);
				}
				result = sparseStream;
			}
			return result;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000FCDC File Offset: 0x0000DEDC
		[Obsolete("Use OpenFile with filename:attributename:$attributetype syntax (e.g. \\FILE.TXT:STREAM:$DATA)", false)]
		public SparseStream OpenRawStream(string file, AttributeType type, string name, FileAccess access)
		{
			SparseStream result;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(file);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("No such file", file);
				}
				result = this.GetFile(directoryEntry.Reference).OpenStream(type, name, access);
			}
			return result;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000FD3C File Offset: 0x0000DF3C
		public void CreateHardLink(string sourceName, string destinationName)
		{
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(sourceName);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("Source file not found", sourceName);
				}
				string directoryFromPath = Utilities.GetDirectoryFromPath(destinationName);
				DirectoryEntry directoryEntry2 = this.GetDirectoryEntry(directoryFromPath);
				if (directoryEntry2 == null || (directoryEntry2.Details.FileAttributes & FileAttributes.Directory) == (FileAttributes)0)
				{
					throw new FileNotFoundException("Destination directory not found", directoryFromPath);
				}
				Directory directory = this.GetDirectory(directoryEntry2.Reference);
				if (directory == null)
				{
					throw new FileNotFoundException("Destination directory not found", directoryFromPath);
				}
				if (this.GetDirectoryEntry(directory, Utilities.GetFileFromPath(destinationName)) != null)
				{
					throw new IOException("A file with this name already exists: " + destinationName);
				}
				File file = this.GetFile(directoryEntry.Reference);
				directory.AddEntry(file, Utilities.GetFileFromPath(destinationName), FileNameNamespace.Posix);
				directoryEntry2.UpdateFrom(directory);
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000FE18 File Offset: 0x0000E018
		public int GetHardLinkCount(string path)
		{
			int result;
			using (new NtfsTransaction())
			{
				DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
				if (directoryEntry == null)
				{
					throw new FileNotFoundException("File not found", path);
				}
				File file = this.GetFile(directoryEntry.Reference);
				if (!this._context.Options.HideDosFileNames)
				{
					result = (int)file.HardLinkCount;
				}
				else
				{
					int num = 0;
					using (IEnumerator<NtfsStream> enumerator = file.GetStreams(AttributeType.FileName, null).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.GetContent<FileNameRecord>().FileNameNamespace != FileNameNamespace.Dos)
							{
								num++;
							}
						}
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000FEDC File Offset: 0x0000E0DC
		public void UpdateBiosGeometry(Geometry geometry)
		{
			this._context.BiosParameterBlock.SectorsPerTrack = (ushort)geometry.SectorsPerTrack;
			this._context.BiosParameterBlock.NumHeads = (ushort)geometry.HeadsPerCylinder;
			this._context.RawStream.Position = 0L;
			byte[] array = StreamUtilities.ReadExact(this._context.RawStream, 512);
			this._context.BiosParameterBlock.ToBytes(array, 0);
			this._context.RawStream.Position = 0L;
			this._context.RawStream.Write(array, 0, array.Length);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000FF78 File Offset: 0x0000E178
		internal DirectoryEntry GetDirectoryEntry(string path)
		{
			return this.GetDirectoryEntry(this.GetDirectory(5L), path);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000FF8C File Offset: 0x0000E18C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._context != null && this._context.Mft != null)
				{
					this._context.Mft.Dispose();
					this._context.Mft = null;
				}
				IDisposable disposable = this._context.Options.Compressor as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
					this._context.Options.Compressor = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00010004 File Offset: 0x0000E204
		private static void RemoveFileFromDirectory(Directory dir, File file, string name)
		{
			List<string> list = new List<string>();
			DirectoryEntry entryByName = dir.GetEntryByName(name);
			if (entryByName.Details.FileNameNamespace == FileNameNamespace.Dos || entryByName.Details.FileNameNamespace == FileNameNamespace.Win32)
			{
				using (IEnumerator<NtfsStream> enumerator = file.GetStreams(AttributeType.FileName, null).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NtfsStream ntfsStream = enumerator.Current;
						FileNameRecord content = ntfsStream.GetContent<FileNameRecord>();
						if ((content.FileNameNamespace == FileNameNamespace.Win32 || content.FileNameNamespace == FileNameNamespace.Dos) && content.ParentDirectory.Value == dir.MftReference.Value)
						{
							list.Add(content.FileName);
						}
					}
					goto IL_9C;
				}
			}
			list.Add(name);
			IL_9C:
			foreach (string name2 in list)
			{
				DirectoryEntry entryByName2 = dir.GetEntryByName(name2);
				dir.RemoveEntry(entryByName2);
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00010108 File Offset: 0x0000E308
		private static void SplitPath(string path, out string plainPath, out string attributeName)
		{
			plainPath = path;
			string fileFromPath = Utilities.GetFileFromPath(path);
			attributeName = null;
			int num = fileFromPath.IndexOf(':');
			if (num >= 0)
			{
				attributeName = fileFromPath.Substring(num + 1);
				plainPath = plainPath.Substring(0, path.Length - (fileFromPath.Length - num));
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00010154 File Offset: 0x0000E354
		private static void UpdateStandardInformation(DirectoryEntry dirEntry, File file, NtfsFileSystem.StandardInformationModifier modifier)
		{
			NtfsStream stream = file.GetStream(AttributeType.StandardInformation, null);
			StandardInformation content = stream.GetContent<StandardInformation>();
			modifier(content);
			stream.SetContent<StandardInformation>(content);
			dirEntry.UpdateFrom(file);
			file.UpdateRecordInMft();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0001018C File Offset: 0x0000E38C
		private DirectoryEntry CreateNewFile(string path, NewFileOptions options)
		{
			DirectoryEntry directoryEntry = this.GetDirectoryEntry(Utilities.GetDirectoryFromPath(path));
			Directory directory = this.GetDirectory(directoryEntry.Reference);
			FileAttributeFlags fileAttributeFlags = directory.StandardInformation.FileAttributes;
			if (options != null && options.Compressed != null)
			{
				if (options.Compressed.Value)
				{
					fileAttributeFlags |= FileAttributeFlags.Compressed;
				}
				else
				{
					fileAttributeFlags &= ~FileAttributeFlags.Compressed;
				}
			}
			File file = File.CreateNew(this._context, fileAttributeFlags);
			DirectoryEntry directoryEntry2;
			try
			{
				directoryEntry2 = this.AddFileToDirectory(file, directory, Utilities.GetFileFromPath(path), options);
				RawSecurityDescriptor parent = this.DoGetSecurity(directory);
				RawSecurityDescriptor securityDescriptor;
				if (options != null && options.SecurityDescriptor != null)
				{
					securityDescriptor = options.SecurityDescriptor;
				}
				else
				{
					securityDescriptor = SecurityDescriptor.CalcNewObjectDescriptor(parent, false);
				}
				this.DoSetSecurity(file, securityDescriptor);
				directoryEntry2.UpdateFrom(file);
				directoryEntry.UpdateFrom(directory);
			}
			finally
			{
				if (file.HardLinkCount == 0)
				{
					file.Delete();
				}
			}
			return directoryEntry2;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0001027C File Offset: 0x0000E47C
		private DirectoryEntry GetDirectoryEntry(Directory dir, string path)
		{
			string[] pathEntries = path.Split(new char[]
			{
				'\\'
			}, StringSplitOptions.RemoveEmptyEntries);
			return this.GetDirectoryEntry(dir, pathEntries, 0);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000102A8 File Offset: 0x0000E4A8
		private void DoSearch(List<string> results, string path, Regex regex, bool subFolders, bool dirs, bool files)
		{
			DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
			if (directoryEntry == null)
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "The directory '{0}' was not found", new object[]
				{
					path
				}));
			}
			Directory directory = this.GetDirectory(directoryEntry.Reference);
			if (directory == null)
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "The directory '{0}' was not found", new object[]
				{
					path
				}));
			}
			foreach (DirectoryEntry directoryEntry2 in directory.GetAllEntries(true))
			{
				bool flag = (directoryEntry2.Details.FileAttributes & FileAttributes.Directory) > (FileAttributes)0;
				if (((flag && dirs) || (!flag && files)) && regex.IsMatch(directoryEntry2.SearchName))
				{
					results.Add(Utilities.CombinePaths(path, directoryEntry2.Details.FileName));
				}
				if (subFolders && flag)
				{
					this.DoSearch(results, Utilities.CombinePaths(path, directoryEntry2.Details.FileName), regex, subFolders, dirs, files);
				}
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x000103B8 File Offset: 0x0000E5B8
		private DirectoryEntry GetDirectoryEntry(Directory dir, string[] pathEntries, int pathOffset)
		{
			if (pathEntries.Length == 0)
			{
				return dir.DirectoryEntry;
			}
			DirectoryEntry entryByName = dir.GetEntryByName(pathEntries[pathOffset]);
			if (entryByName == null)
			{
				return null;
			}
			if (pathOffset == pathEntries.Length - 1)
			{
				return entryByName;
			}
			if ((entryByName.Details.FileAttributes & FileAttributes.Directory) != (FileAttributes)0)
			{
				return this.GetDirectoryEntry(this.GetDirectory(entryByName.Reference), pathEntries, pathOffset + 1);
			}
			throw new IOException(string.Format(CultureInfo.InvariantCulture, "{0} is a file, not a directory", new object[]
			{
				pathEntries[pathOffset]
			}));
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00010430 File Offset: 0x0000E630
		private DirectoryEntry AddFileToDirectory(File file, Directory dir, string name, NewFileOptions options)
		{
			bool flag;
			if (options != null && options.CreateShortNames != null)
			{
				flag = options.CreateShortNames.Value;
			}
			else
			{
				flag = this.CreateShortNames;
			}
			DirectoryEntry result;
			if (flag)
			{
				if (Utilities.Is8Dot3(name.ToUpperInvariant()))
				{
					result = dir.AddEntry(file, name, FileNameNamespace.Win32AndDos);
				}
				else
				{
					result = dir.AddEntry(file, name, FileNameNamespace.Win32);
					dir.AddEntry(file, dir.CreateShortName(name), FileNameNamespace.Dos);
				}
			}
			else
			{
				result = dir.AddEntry(file, name, FileNameNamespace.Posix);
			}
			return result;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000104B0 File Offset: 0x0000E6B0
		private void RemoveReparsePoint(File file)
		{
			NtfsStream stream = file.GetStream(AttributeType.ReparsePoint, null);
			if (stream != null)
			{
				ReparsePointRecord reparsePointRecord = new ReparsePointRecord();
				using (Stream stream2 = stream.Open(FileAccess.Read))
				{
					byte[] buffer = StreamUtilities.ReadExact(stream2, (int)stream2.Length);
					reparsePointRecord.ReadFrom(buffer, 0);
				}
				file.RemoveStream(stream);
				NtfsStream stream3 = file.GetStream(AttributeType.StandardInformation, null);
				StandardInformation content = stream3.GetContent<StandardInformation>();
				content.FileAttributes &= ~FileAttributeFlags.ReparsePoint;
				stream3.SetContent<StandardInformation>(content);
				this._context.ReparsePoints.Remove(reparsePointRecord.Tag, file.MftReference);
			}
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0001055C File Offset: 0x0000E75C
		private RawSecurityDescriptor DoGetSecurity(File file)
		{
			NtfsStream stream = file.GetStream(AttributeType.SecurityDescriptor, null);
			if (stream != null)
			{
				return stream.GetContent<SecurityDescriptor>().Descriptor;
			}
			StandardInformation standardInformation = file.StandardInformation;
			return this._context.SecurityDescriptors.GetDescriptorById(standardInformation.SecurityId);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000105A0 File Offset: 0x0000E7A0
		private void DoSetSecurity(File file, RawSecurityDescriptor securityDescriptor)
		{
			NtfsStream stream = file.GetStream(AttributeType.SecurityDescriptor, null);
			if (stream != null)
			{
				stream.SetContent<SecurityDescriptor>(new SecurityDescriptor
				{
					Descriptor = securityDescriptor
				});
				return;
			}
			uint securityId = this._context.SecurityDescriptors.AddDescriptor(securityDescriptor);
			NtfsStream stream2 = file.GetStream(AttributeType.StandardInformation, null);
			StandardInformation content = stream2.GetContent<StandardInformation>();
			content.SecurityId = securityId;
			stream2.SetContent<StandardInformation>(content);
			file.UpdateRecordInMft();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00010604 File Offset: 0x0000E804
		private void DumpDirectory(Directory dir, TextWriter writer, string indent)
		{
			foreach (DirectoryEntry directoryEntry in dir.GetAllEntries(true))
			{
				File file = this.GetFile(directoryEntry.Reference);
				Directory directory = file as Directory;
				writer.WriteLine(string.Concat(new object[]
				{
					indent,
					"+-",
					file,
					" (",
					file.IndexInMft,
					")"
				}));
				if (directory != null && file.IndexInMft != 5U)
				{
					this.DumpDirectory(directory, writer, indent + "| ");
				}
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000106C0 File Offset: 0x0000E8C0
		private void UpdateStandardInformation(string path, NtfsFileSystem.StandardInformationModifier modifier)
		{
			DirectoryEntry directoryEntry = this.GetDirectoryEntry(path);
			if (directoryEntry == null)
			{
				throw new FileNotFoundException("File not found", path);
			}
			File file = this.GetFile(directoryEntry.Reference);
			NtfsFileSystem.UpdateStandardInformation(directoryEntry, file, modifier);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000106FC File Offset: 0x0000E8FC
		private string ParsePath(string path, out string attributeName, out AttributeType attributeType)
		{
			string text = Utilities.GetFileFromPath(path);
			attributeName = null;
			attributeType = AttributeType.Data;
			string[] array = text.Split(new char[]
			{
				':'
			}, 3);
			text = array[0];
			if (array.Length > 1)
			{
				attributeName = array[1];
				if (string.IsNullOrEmpty(attributeName))
				{
					attributeName = null;
				}
			}
			if (array.Length > 2)
			{
				string text2 = array[2];
				AttributeDefinitionRecord attributeDefinitionRecord = this._context.AttributeDefinitions.Lookup(text2);
				if (attributeDefinitionRecord == null)
				{
					throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "No such attribute type '{0}'", new object[]
					{
						text2
					}), path);
				}
				attributeType = attributeDefinitionRecord.Type;
			}
			string result;
			try
			{
				result = Utilities.CombinePaths(Utilities.GetDirectoryFromPath(path), text);
			}
			catch (ArgumentException)
			{
				throw new IOException("Invalid path: " + path);
			}
			return result;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x000107C4 File Offset: 0x0000E9C4
		internal Directory GetDirectory(long index)
		{
			return (Directory)this.GetFile(index);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000107D2 File Offset: 0x0000E9D2
		internal Directory GetDirectory(FileRecordReference fileReference)
		{
			return (Directory)this.GetFile(fileReference);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000107E0 File Offset: 0x0000E9E0
		internal File GetFile(FileRecordReference fileReference)
		{
			FileRecord record = this._context.Mft.GetRecord(fileReference);
			if (record == null)
			{
				return null;
			}
			if (record.BaseFile.Value != 0UL)
			{
				return null;
			}
			File file = this._fileCache[fileReference.MftIndex];
			if (file != null && file.MftReference.SequenceNumber != fileReference.SequenceNumber)
			{
				file = null;
			}
			if (file == null)
			{
				if ((record.Flags & FileRecordFlags.IsDirectory) != FileRecordFlags.None)
				{
					file = new Directory(this._context, record);
				}
				else
				{
					file = new File(this._context, record);
				}
				this._fileCache[fileReference.MftIndex] = file;
			}
			return file;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00010884 File Offset: 0x0000EA84
		internal File GetFile(long index)
		{
			FileRecord record = this._context.Mft.GetRecord(index, false);
			if (record == null)
			{
				return null;
			}
			if (record.BaseFile.Value != 0UL)
			{
				return null;
			}
			File file = this._fileCache[index];
			if (file != null && file.MftReference.SequenceNumber != record.SequenceNumber)
			{
				file = null;
			}
			if (file == null)
			{
				if ((record.Flags & FileRecordFlags.IsDirectory) != FileRecordFlags.None)
				{
					file = new Directory(this._context, record);
				}
				else
				{
					file = new File(this._context, record);
				}
				this._fileCache[index] = file;
			}
			return file;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0001091C File Offset: 0x0000EB1C
		internal File AllocateFile(FileRecordFlags flags)
		{
			File file;
			if ((flags & FileRecordFlags.IsDirectory) != FileRecordFlags.None)
			{
				file = new Directory(this._context, this._context.Mft.AllocateRecord(flags, false));
			}
			else
			{
				file = new File(this._context, this._context.Mft.AllocateRecord(flags, false));
			}
			this._fileCache[file.MftReference.MftIndex] = file;
			return file;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00010989 File Offset: 0x0000EB89
		internal void ForgetFile(File file)
		{
			this._fileCache.Remove((long)((ulong)file.IndexInMft));
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0001099D File Offset: 0x0000EB9D
		public override long Size
		{
			get
			{
				return this.TotalClusters * this.ClusterSize;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000306 RID: 774 RVA: 0x000109AC File Offset: 0x0000EBAC
		public override long UsedSpace
		{
			get
			{
				long num = 0L;
				Bitmap bitmap = this._context.ClusterBitmap.Bitmap;
				int bytes;
				for (long num2 = 0L; num2 < bitmap.Size; num2 += (long)bytes)
				{
					byte[] array = new byte[4096L];
					bytes = bitmap.GetBytes(num2, array, 0, array.Length);
					num += BitCounter.Count(array, 0, bytes);
				}
				return num * this.ClusterSize;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00010A10 File Offset: 0x0000EC10
		public override long AvailableSpace
		{
			get
			{
				return this.Size - this.UsedSpace;
			}
		}

		// Token: 0x04000134 RID: 308
		private const FileAttributes NonSettableFileAttributes = FileAttributes.Directory | FileAttributes.ReparsePoint | FileAttributes.Offline;

		// Token: 0x04000135 RID: 309
		private readonly NtfsContext _context;

		// Token: 0x04000136 RID: 310
		private readonly ObjectCache<long, File> _fileCache;

		// Token: 0x04000137 RID: 311
		private readonly VolumeInformation _volumeInfo;

		// Token: 0x0200007D RID: 125
		// (Invoke) Token: 0x060004A1 RID: 1185
		private delegate void StandardInformationModifier(StandardInformation si);
	}
}
