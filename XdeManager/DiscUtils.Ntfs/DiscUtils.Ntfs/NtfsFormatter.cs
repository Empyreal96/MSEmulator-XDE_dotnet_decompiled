using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000040 RID: 64
	internal class NtfsFormatter
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00011D4D File Offset: 0x0000FF4D
		// (set) Token: 0x06000320 RID: 800 RVA: 0x00011D55 File Offset: 0x0000FF55
		public byte[] BootCode { get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00011D5E File Offset: 0x0000FF5E
		// (set) Token: 0x06000322 RID: 802 RVA: 0x00011D66 File Offset: 0x0000FF66
		public SecurityIdentifier ComputerAccount { get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00011D6F File Offset: 0x0000FF6F
		// (set) Token: 0x06000324 RID: 804 RVA: 0x00011D77 File Offset: 0x0000FF77
		public Geometry DiskGeometry { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000325 RID: 805 RVA: 0x00011D80 File Offset: 0x0000FF80
		// (set) Token: 0x06000326 RID: 806 RVA: 0x00011D88 File Offset: 0x0000FF88
		public long FirstSector { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00011D91 File Offset: 0x0000FF91
		// (set) Token: 0x06000328 RID: 808 RVA: 0x00011D99 File Offset: 0x0000FF99
		public string Label { get; set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00011DA2 File Offset: 0x0000FFA2
		// (set) Token: 0x0600032A RID: 810 RVA: 0x00011DAA File Offset: 0x0000FFAA
		public long SectorCount { get; set; }

		// Token: 0x0600032B RID: 811 RVA: 0x00011DB4 File Offset: 0x0000FFB4
		public NtfsFileSystem Format(Stream stream)
		{
			this._context = new NtfsContext();
			this._context.Options = new NtfsOptions();
			this._context.RawStream = stream;
			this._context.AttributeDefinitions = new AttributeDefinitions();
			string str = (this.ComputerAccount == null) ? "LA" : new SecurityIdentifier(WellKnownSidType.AccountAdministratorSid, this.ComputerAccount).ToString();
			using (new NtfsTransaction())
			{
				this._clusterSize = 4096;
				this._mftRecordSize = 1024;
				this._indexBufferSize = 4096;
				long num = (this.SectorCount - 1L) * 512L / (long)this._clusterSize;
				int num2 = MathUtilities.Ceil(Math.Max(8192, (this.BootCode == null) ? 0 : this.BootCode.Length), this._clusterSize);
				this._mftMirrorCluster = num / 2L;
				uint num3 = 1U;
				this._bitmapCluster = this._mftMirrorCluster + 13L;
				int num4 = (int)MathUtilities.Ceil(num / 8L, (long)this._clusterSize);
				long num5 = Math.Max(3L + num / 10L, (long)num2);
				int num6 = 1;
				this._mftCluster = num5 + (long)num6;
				int num7 = 8;
				if (this._mftCluster + (long)num7 > this._mftMirrorCluster || this._bitmapCluster + (long)num4 >= num)
				{
					throw new IOException("Unable to determine initial layout of NTFS metadata - disk may be too small");
				}
				this.CreateBiosParameterBlock(stream, num2 * this._clusterSize);
				this._context.Mft = new MasterFileTable(this._context);
				File file = this._context.Mft.InitializeNew(this._context, num5, (ulong)((long)num6), this._mftCluster, (ulong)((long)num7));
				File file2 = this.CreateFixedSystemFile(6L, this._bitmapCluster, (ulong)((long)num4), true);
				this._context.ClusterBitmap = new ClusterBitmap(file2);
				this._context.ClusterBitmap.MarkAllocated(0L, (long)num2);
				this._context.ClusterBitmap.MarkAllocated(this._bitmapCluster, (long)num4);
				this._context.ClusterBitmap.MarkAllocated(num5, (long)num6);
				this._context.ClusterBitmap.MarkAllocated(this._mftCluster, (long)num7);
				this._context.ClusterBitmap.SetTotalClusters(num);
				file2.UpdateRecordInMft();
				File file3 = this.CreateFixedSystemFile(1L, this._mftMirrorCluster, (ulong)num3, true);
				File file4 = this.CreateSystemFile(2L);
				using (Stream stream2 = file4.OpenStream(AttributeType.Data, null, FileAccess.ReadWrite))
				{
					stream2.SetLength(Math.Min(Math.Max(2097152L, num / 500L * (long)this._clusterSize), 67108864L));
					byte[] array = new byte[1048576];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = byte.MaxValue;
					}
					int num9;
					for (long num8 = 0L; num8 < stream2.Length; num8 += (long)num9)
					{
						num9 = (int)Math.Min(stream2.Length - num8, (long)array.Length);
						stream2.Write(array, 0, num9);
					}
				}
				File file5 = this.CreateSystemFile(3L);
				file5.CreateStream(AttributeType.VolumeName, null).SetContent<VolumeName>(new VolumeName(this.Label ?? "New Volume"));
				file5.CreateStream(AttributeType.VolumeInformation, null).SetContent<VolumeInformation>(new VolumeInformation(3, 1, VolumeInformationFlags.None));
				NtfsFormatter.SetSecurityAttribute(file5, "O:" + str + "G:BAD:(A;;0x12019f;;;SY)(A;;0x12019f;;;BA)");
				file5.UpdateRecordInMft();
				this._context.GetFileByIndex = ((long index) => new File(this._context, this._context.Mft.GetRecord(index, false)));
				this._context.AllocateFile = ((FileRecordFlags frf) => new File(this._context, this._context.Mft.AllocateRecord(frf, false)));
				File file6 = this.CreateSystemFile(4L);
				this._context.AttributeDefinitions.WriteTo(file6);
				NtfsFormatter.SetSecurityAttribute(file6, "O:" + str + "G:BAD:(A;;FR;;;SY)(A;;FR;;;BA)");
				file6.UpdateRecordInMft();
				File file7 = this.CreateFixedSystemFile(7L, 0L, (ulong)num2, false);
				NtfsFormatter.SetSecurityAttribute(file7, "O:" + str + "G:BAD:(A;;FR;;;SY)(A;;FR;;;BA)");
				file7.UpdateRecordInMft();
				File file8 = this.CreateSystemFile(8L);
				file8.CreateStream(AttributeType.Data, "$Bad");
				file8.UpdateRecordInMft();
				File file9 = this.CreateSystemFile(9L, FileRecordFlags.HasViewIndex);
				file9.RemoveStream(file9.GetStream(AttributeType.Data, null));
				this._context.SecurityDescriptors = SecurityDescriptors.Initialize(file9);
				file9.UpdateRecordInMft();
				File file10 = this.CreateSystemFile(10L);
				this._context.UpperCase = UpperCase.Initialize(file10);
				file10.UpdateRecordInMft();
				File file11 = File.CreateNew(this._context, FileRecordFlags.IsMetaFile | FileRecordFlags.HasViewIndex, FileAttributeFlags.None);
				file11.RemoveStream(file11.GetStream(AttributeType.Data, null));
				file11.CreateIndex("$O", AttributeType.None, AttributeCollationRule.MultipleUnsignedLongs);
				file11.UpdateRecordInMft();
				File file12 = File.CreateNew(this._context, FileRecordFlags.IsMetaFile | FileRecordFlags.HasViewIndex, FileAttributeFlags.None);
				file12.CreateIndex("$R", AttributeType.None, AttributeCollationRule.MultipleUnsignedLongs);
				file12.UpdateRecordInMft();
				File file13 = File.CreateNew(this._context, FileRecordFlags.IsMetaFile | FileRecordFlags.HasViewIndex, FileAttributeFlags.None);
				Quotas.Initialize(file13);
				Directory directory = this.CreateSystemDirectory(11L);
				directory.AddEntry(file11, "$ObjId", FileNameNamespace.Win32AndDos);
				directory.AddEntry(file12, "$Reparse", FileNameNamespace.Win32AndDos);
				directory.AddEntry(file13, "$Quota", FileNameNamespace.Win32AndDos);
				directory.UpdateRecordInMft();
				Directory directory2 = this.CreateSystemDirectory(5L);
				directory2.AddEntry(file, "$MFT", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(file3, "$MFTMirr", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(file4, "$LogFile", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(file5, "$Volume", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(file6, "$AttrDef", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(directory2, ".", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(file2, "$Bitmap", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(file7, "$Boot", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(file8, "$BadClus", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(file9, "$Secure", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(file10, "$UpCase", FileNameNamespace.Win32AndDos);
				directory2.AddEntry(directory, "$Extend", FileNameNamespace.Win32AndDos);
				NtfsFormatter.SetSecurityAttribute(directory2, "O:" + str + "G:BUD:(A;OICI;FA;;;BA)(A;OICI;FA;;;SY)(A;OICIIO;GA;;;CO)(A;OICI;0x1200a9;;;BU)(A;CI;LC;;;BU)(A;CIIO;DC;;;BU)(A;;0x1200a9;;;WD)");
				directory2.UpdateRecordInMft();
				for (long num10 = 12L; num10 <= 15L; num10 += 1L)
				{
					File file14 = this.CreateSystemFile(num10);
					NtfsFormatter.SetSecurityAttribute(file14, "O:S-1-5-21-1708537768-746137067-1060284298-1003G:BAD:(A;;0x12019f;;;SY)(A;;0x12019f;;;BA)");
					file14.UpdateRecordInMft();
				}
			}
			NtfsFileSystem ntfsFileSystem = new NtfsFileSystem(stream);
			ntfsFileSystem.SetSecurity("$MFT", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;FR;;;SY)(A;;FR;;;BA)"));
			ntfsFileSystem.SetSecurity("$MFTMirr", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;FR;;;SY)(A;;FR;;;BA)"));
			ntfsFileSystem.SetSecurity("$LogFile", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;FR;;;SY)(A;;FR;;;BA)"));
			ntfsFileSystem.SetSecurity("$Bitmap", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;FR;;;SY)(A;;FR;;;BA)"));
			ntfsFileSystem.SetSecurity("$BadClus", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;FR;;;SY)(A;;FR;;;BA)"));
			ntfsFileSystem.SetSecurity("$UpCase", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;FR;;;SY)(A;;FR;;;BA)"));
			ntfsFileSystem.SetSecurity("$Secure", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;0x12019f;;;SY)(A;;0x12019f;;;BA)"));
			ntfsFileSystem.SetSecurity("$Extend", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;0x12019f;;;SY)(A;;0x12019f;;;BA)"));
			ntfsFileSystem.SetSecurity("$Extend\\$Quota", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;0x12019f;;;SY)(A;;0x12019f;;;BA)"));
			ntfsFileSystem.SetSecurity("$Extend\\$ObjId", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;0x12019f;;;SY)(A;;0x12019f;;;BA)"));
			ntfsFileSystem.SetSecurity("$Extend\\$Reparse", new RawSecurityDescriptor("O:" + str + "G:BAD:(A;;0x12019f;;;SY)(A;;0x12019f;;;BA)"));
			ntfsFileSystem.CreateDirectory("System Volume Information");
			ntfsFileSystem.SetAttributes("System Volume Information", FileAttributes.Hidden | FileAttributes.System | FileAttributes.Directory);
			ntfsFileSystem.SetSecurity("System Volume Information", new RawSecurityDescriptor("O:BAG:SYD:(A;OICI;FA;;;SY)"));
			using (ntfsFileSystem.OpenFile("System Volume Information\\MountPointManagerRemoteDatabase", FileMode.Create))
			{
			}
			ntfsFileSystem.SetAttributes("System Volume Information\\MountPointManagerRemoteDatabase", FileAttributes.Hidden | FileAttributes.System | FileAttributes.Archive);
			ntfsFileSystem.SetSecurity("System Volume Information\\MountPointManagerRemoteDatabase", new RawSecurityDescriptor("O:BAG:SYD:(A;;FA;;;SY)"));
			return ntfsFileSystem;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00012620 File Offset: 0x00010820
		private static void SetSecurityAttribute(File file, string secDesc)
		{
			file.CreateStream(AttributeType.SecurityDescriptor, null).SetContent<SecurityDescriptor>(new SecurityDescriptor
			{
				Descriptor = new RawSecurityDescriptor(secDesc)
			});
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00012650 File Offset: 0x00010850
		private File CreateFixedSystemFile(long mftIndex, long firstCluster, ulong numClusters, bool wipe)
		{
			BiosParameterBlock biosParameterBlock = this._context.BiosParameterBlock;
			if (wipe)
			{
				byte[] array = new byte[biosParameterBlock.BytesPerCluster];
				this._context.RawStream.Position = firstCluster * (long)biosParameterBlock.BytesPerCluster;
				for (ulong num = 0UL; num < numClusters; num += 1UL)
				{
					this._context.RawStream.Write(array, 0, array.Length);
				}
			}
			FileRecord fileRecord = this._context.Mft.AllocateRecord((long)((ulong)((uint)mftIndex)), FileRecordFlags.None);
			fileRecord.Flags = FileRecordFlags.InUse;
			fileRecord.SequenceNumber = (ushort)mftIndex;
			File file = new File(this._context, fileRecord);
			StandardInformation.InitializeNewFile(file, FileAttributeFlags.Hidden | FileAttributeFlags.System);
			file.CreateStream(AttributeType.Data, null, firstCluster, numClusters, (uint)biosParameterBlock.BytesPerCluster);
			file.UpdateRecordInMft();
			if (this._context.ClusterBitmap != null)
			{
				this._context.ClusterBitmap.MarkAllocated(firstCluster, (long)numClusters);
			}
			return file;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00012727 File Offset: 0x00010927
		private File CreateSystemFile(long mftIndex)
		{
			return this.CreateSystemFile(mftIndex, FileRecordFlags.None);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00012734 File Offset: 0x00010934
		private File CreateSystemFile(long mftIndex, FileRecordFlags flags)
		{
			FileRecord fileRecord = this._context.Mft.AllocateRecord((long)((ulong)((uint)mftIndex)), flags);
			fileRecord.SequenceNumber = (ushort)mftIndex;
			File file = new File(this._context, fileRecord);
			StandardInformation.InitializeNewFile(file, FileAttributeFlags.Hidden | FileAttributeFlags.System | FileRecord.ConvertFlags(flags));
			file.CreateStream(AttributeType.Data, null);
			file.UpdateRecordInMft();
			return file;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0001278C File Offset: 0x0001098C
		private Directory CreateSystemDirectory(long mftIndex)
		{
			FileRecord fileRecord = this._context.Mft.AllocateRecord((long)((ulong)((uint)mftIndex)), FileRecordFlags.None);
			fileRecord.Flags = (FileRecordFlags.InUse | FileRecordFlags.IsDirectory);
			fileRecord.SequenceNumber = (ushort)mftIndex;
			Directory directory = new Directory(this._context, fileRecord);
			StandardInformation.InitializeNewFile(directory, FileAttributeFlags.Hidden | FileAttributeFlags.System);
			directory.CreateIndex("$I30", AttributeType.FileName, AttributeCollationRule.Filename);
			directory.UpdateRecordInMft();
			return directory;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000127E8 File Offset: 0x000109E8
		private void CreateBiosParameterBlock(Stream stream, int bootFileSize)
		{
			byte[] array = new byte[bootFileSize];
			if (this.BootCode != null)
			{
				Array.Copy(this.BootCode, 0, array, 0, this.BootCode.Length);
			}
			BiosParameterBlock biosParameterBlock = BiosParameterBlock.Initialized(this.DiskGeometry, this._clusterSize, (uint)this.FirstSector, this.SectorCount, this._mftRecordSize, this._indexBufferSize);
			biosParameterBlock.MftCluster = this._mftCluster;
			biosParameterBlock.MftMirrorCluster = this._mftMirrorCluster;
			biosParameterBlock.ToBytes(array, 0);
			stream.Position = 0L;
			stream.Write(array, 0, array.Length);
			stream.Position = (this.SectorCount - 1L) * 512L;
			stream.Write(array, 0, 512);
			this._context.BiosParameterBlock = biosParameterBlock;
		}

		// Token: 0x04000140 RID: 320
		private long _bitmapCluster;

		// Token: 0x04000141 RID: 321
		private int _clusterSize;

		// Token: 0x04000142 RID: 322
		private NtfsContext _context;

		// Token: 0x04000143 RID: 323
		private int _indexBufferSize;

		// Token: 0x04000144 RID: 324
		private long _mftCluster;

		// Token: 0x04000145 RID: 325
		private long _mftMirrorCluster;

		// Token: 0x04000146 RID: 326
		private int _mftRecordSize;
	}
}
