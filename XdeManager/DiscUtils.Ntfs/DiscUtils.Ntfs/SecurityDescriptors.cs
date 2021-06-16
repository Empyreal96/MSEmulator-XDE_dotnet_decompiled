using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using DiscUtils.Internal;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200004E RID: 78
	internal sealed class SecurityDescriptors : IDiagnosticTraceable
	{
		// Token: 0x0600039D RID: 925 RVA: 0x000143A0 File Offset: 0x000125A0
		public SecurityDescriptors(File file)
		{
			this._file = file;
			this._hashIndex = new IndexView<SecurityDescriptors.HashIndexKey, SecurityDescriptors.HashIndexData>(file.GetIndex("$SDH"));
			this._idIndex = new IndexView<SecurityDescriptors.IdIndexKey, SecurityDescriptors.IdIndexData>(file.GetIndex("$SII"));
			foreach (KeyValuePair<SecurityDescriptors.IdIndexKey, SecurityDescriptors.IdIndexData> keyValuePair in this._idIndex.Entries)
			{
				if (keyValuePair.Key.Id > this._nextId)
				{
					this._nextId = keyValuePair.Key.Id;
				}
				long num = keyValuePair.Value.SdsOffset + (long)keyValuePair.Value.SdsLength;
				if (num > this._nextSpace)
				{
					this._nextSpace = num;
				}
			}
			if (this._nextId == 0U)
			{
				this._nextId = 256U;
			}
			else
			{
				this._nextId += 1U;
			}
			this._nextSpace = MathUtilities.RoundUp(this._nextSpace, 16L);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000144AC File Offset: 0x000126AC
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "SECURITY DESCRIPTORS");
			using (Stream stream = this._file.OpenStream(AttributeType.Data, "$SDS", FileAccess.Read))
			{
				byte[] buffer = StreamUtilities.ReadExact(stream, (int)stream.Length);
				foreach (KeyValuePair<SecurityDescriptors.IdIndexKey, SecurityDescriptors.IdIndexData> keyValuePair in this._idIndex.Entries)
				{
					int offset = (int)keyValuePair.Value.SdsOffset;
					SecurityDescriptorRecord securityDescriptorRecord = new SecurityDescriptorRecord();
					if (!securityDescriptorRecord.Read(buffer, offset))
					{
						break;
					}
					string str = "--unknown--";
					if (securityDescriptorRecord.SecurityDescriptor[0] != 0)
					{
						str = new RawSecurityDescriptor(securityDescriptorRecord.SecurityDescriptor, 0).GetSddlForm(AccessControlSections.All);
					}
					writer.WriteLine(indent + "  SECURITY DESCRIPTOR RECORD");
					writer.WriteLine(indent + "           Hash: " + securityDescriptorRecord.Hash);
					writer.WriteLine(indent + "             Id: " + securityDescriptorRecord.Id);
					writer.WriteLine(indent + "    File Offset: " + securityDescriptorRecord.OffsetInFile);
					writer.WriteLine(indent + "           Size: " + securityDescriptorRecord.EntrySize);
					writer.WriteLine(indent + "          Value: " + str);
				}
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00014648 File Offset: 0x00012848
		public static SecurityDescriptors Initialize(File file)
		{
			file.CreateIndex("$SDH", AttributeType.None, AttributeCollationRule.SecurityHash);
			file.CreateIndex("$SII", AttributeType.None, AttributeCollationRule.UnsignedLong);
			file.CreateStream(AttributeType.Data, "$SDS");
			return new SecurityDescriptors(file);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00014680 File Offset: 0x00012880
		public RawSecurityDescriptor GetDescriptorById(uint id)
		{
			SecurityDescriptors.IdIndexData data;
			if (this._idIndex.TryGetValue(new SecurityDescriptors.IdIndexKey(id), out data))
			{
				return this.ReadDescriptor(data).Descriptor;
			}
			return null;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000146B0 File Offset: 0x000128B0
		public uint AddDescriptor(RawSecurityDescriptor newDescriptor)
		{
			SecurityDescriptor securityDescriptor = new SecurityDescriptor(newDescriptor);
			uint num = securityDescriptor.CalcHash();
			byte[] array = new byte[securityDescriptor.Size];
			securityDescriptor.WriteTo(array, 0);
			foreach (KeyValuePair<SecurityDescriptors.HashIndexKey, SecurityDescriptors.HashIndexData> keyValuePair in this._hashIndex.FindAll(new SecurityDescriptors.HashFinder(num)))
			{
				SecurityDescriptor securityDescriptor2 = this.ReadDescriptor(keyValuePair.Value);
				byte[] array2 = new byte[securityDescriptor2.Size];
				securityDescriptor2.WriteTo(array2, 0);
				if (Utilities.AreEqual(array, array2))
				{
					return keyValuePair.Value.Id;
				}
			}
			long nextSpace = this._nextSpace;
			SecurityDescriptorRecord securityDescriptorRecord = new SecurityDescriptorRecord();
			securityDescriptorRecord.SecurityDescriptor = array;
			securityDescriptorRecord.Hash = num;
			securityDescriptorRecord.Id = this._nextId;
			if ((nextSpace + (long)securityDescriptorRecord.Size) / 262144L % 2L == 1L)
			{
				this._nextSpace = MathUtilities.RoundUp(nextSpace, 524288L);
				nextSpace = this._nextSpace;
			}
			securityDescriptorRecord.OffsetInFile = nextSpace;
			byte[] array3 = new byte[securityDescriptorRecord.Size];
			securityDescriptorRecord.WriteTo(array3, 0);
			using (Stream stream = this._file.OpenStream(AttributeType.Data, "$SDS", FileAccess.ReadWrite))
			{
				stream.Position = this._nextSpace;
				stream.Write(array3, 0, array3.Length);
				stream.Position = 262144L + this._nextSpace;
				stream.Write(array3, 0, array3.Length);
			}
			this._nextSpace = MathUtilities.RoundUp(this._nextSpace + (long)array3.Length, 16L);
			this._nextId += 1U;
			SecurityDescriptors.HashIndexData hashIndexData = new SecurityDescriptors.HashIndexData();
			hashIndexData.Hash = securityDescriptorRecord.Hash;
			hashIndexData.Id = securityDescriptorRecord.Id;
			hashIndexData.SdsOffset = securityDescriptorRecord.OffsetInFile;
			hashIndexData.SdsLength = (int)securityDescriptorRecord.EntrySize;
			SecurityDescriptors.HashIndexKey hashIndexKey = new SecurityDescriptors.HashIndexKey();
			hashIndexKey.Hash = securityDescriptorRecord.Hash;
			hashIndexKey.Id = securityDescriptorRecord.Id;
			this._hashIndex[hashIndexKey] = hashIndexData;
			SecurityDescriptors.IdIndexData idIndexData = new SecurityDescriptors.IdIndexData();
			idIndexData.Hash = securityDescriptorRecord.Hash;
			idIndexData.Id = securityDescriptorRecord.Id;
			idIndexData.SdsOffset = securityDescriptorRecord.OffsetInFile;
			idIndexData.SdsLength = (int)securityDescriptorRecord.EntrySize;
			SecurityDescriptors.IdIndexKey idIndexKey = new SecurityDescriptors.IdIndexKey();
			idIndexKey.Id = securityDescriptorRecord.Id;
			this._idIndex[idIndexKey] = idIndexData;
			this._file.UpdateRecordInMft();
			return securityDescriptorRecord.Id;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00014950 File Offset: 0x00012B50
		private SecurityDescriptor ReadDescriptor(SecurityDescriptors.IndexData data)
		{
			SecurityDescriptor result;
			using (Stream stream = this._file.OpenStream(AttributeType.Data, "$SDS", FileAccess.Read))
			{
				stream.Position = data.SdsOffset;
				byte[] buffer = StreamUtilities.ReadExact(stream, data.SdsLength);
				SecurityDescriptorRecord securityDescriptorRecord = new SecurityDescriptorRecord();
				securityDescriptorRecord.Read(buffer, 0);
				result = new SecurityDescriptor(new RawSecurityDescriptor(securityDescriptorRecord.SecurityDescriptor, 0));
			}
			return result;
		}

		// Token: 0x04000174 RID: 372
		private const int BlockSize = 262144;

		// Token: 0x04000175 RID: 373
		private readonly File _file;

		// Token: 0x04000176 RID: 374
		private readonly IndexView<SecurityDescriptors.HashIndexKey, SecurityDescriptors.HashIndexData> _hashIndex;

		// Token: 0x04000177 RID: 375
		private readonly IndexView<SecurityDescriptors.IdIndexKey, SecurityDescriptors.IdIndexData> _idIndex;

		// Token: 0x04000178 RID: 376
		private uint _nextId;

		// Token: 0x04000179 RID: 377
		private long _nextSpace;

		// Token: 0x0200008C RID: 140
		internal abstract class IndexData
		{
			// Token: 0x060004DB RID: 1243 RVA: 0x00017958 File Offset: 0x00015B58
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "[Data-Hash:{0},Id:{1},SdsOffset:{2},SdsLength:{3}]", new object[]
				{
					this.Hash,
					this.Id,
					this.SdsOffset,
					this.SdsLength
				});
			}

			// Token: 0x0400025C RID: 604
			public uint Hash;

			// Token: 0x0400025D RID: 605
			public uint Id;

			// Token: 0x0400025E RID: 606
			public int SdsLength;

			// Token: 0x0400025F RID: 607
			public long SdsOffset;
		}

		// Token: 0x0200008D RID: 141
		internal sealed class HashIndexKey : IByteArraySerializable
		{
			// Token: 0x1700015A RID: 346
			// (get) Token: 0x060004DD RID: 1245 RVA: 0x000179BA File Offset: 0x00015BBA
			public int Size
			{
				get
				{
					return 8;
				}
			}

			// Token: 0x060004DE RID: 1246 RVA: 0x000179BD File Offset: 0x00015BBD
			public int ReadFrom(byte[] buffer, int offset)
			{
				this.Hash = EndianUtilities.ToUInt32LittleEndian(buffer, offset);
				this.Id = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 4);
				return 8;
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x000179DC File Offset: 0x00015BDC
			public void WriteTo(byte[] buffer, int offset)
			{
				EndianUtilities.WriteBytesLittleEndian(this.Hash, buffer, offset);
				EndianUtilities.WriteBytesLittleEndian(this.Id, buffer, offset + 4);
			}

			// Token: 0x060004E0 RID: 1248 RVA: 0x000179FA File Offset: 0x00015BFA
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "[Key-Hash:{0},Id:{1}]", new object[]
				{
					this.Hash,
					this.Id
				});
			}

			// Token: 0x04000260 RID: 608
			public uint Hash;

			// Token: 0x04000261 RID: 609
			public uint Id;
		}

		// Token: 0x0200008E RID: 142
		internal sealed class HashIndexData : SecurityDescriptors.IndexData, IByteArraySerializable
		{
			// Token: 0x1700015B RID: 347
			// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00017A35 File Offset: 0x00015C35
			public int Size
			{
				get
				{
					return 20;
				}
			}

			// Token: 0x060004E3 RID: 1251 RVA: 0x00017A39 File Offset: 0x00015C39
			public int ReadFrom(byte[] buffer, int offset)
			{
				this.Hash = EndianUtilities.ToUInt32LittleEndian(buffer, offset);
				this.Id = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 4);
				this.SdsOffset = EndianUtilities.ToInt64LittleEndian(buffer, offset + 8);
				this.SdsLength = EndianUtilities.ToInt32LittleEndian(buffer, offset + 16);
				return 20;
			}

			// Token: 0x060004E4 RID: 1252 RVA: 0x00017A78 File Offset: 0x00015C78
			public void WriteTo(byte[] buffer, int offset)
			{
				EndianUtilities.WriteBytesLittleEndian(this.Hash, buffer, offset);
				EndianUtilities.WriteBytesLittleEndian(this.Id, buffer, offset + 4);
				EndianUtilities.WriteBytesLittleEndian(this.SdsOffset, buffer, offset + 8);
				EndianUtilities.WriteBytesLittleEndian(this.SdsLength, buffer, offset + 16);
			}
		}

		// Token: 0x0200008F RID: 143
		internal sealed class IdIndexKey : IByteArraySerializable
		{
			// Token: 0x060004E6 RID: 1254 RVA: 0x00017ABD File Offset: 0x00015CBD
			public IdIndexKey()
			{
			}

			// Token: 0x060004E7 RID: 1255 RVA: 0x00017AC5 File Offset: 0x00015CC5
			public IdIndexKey(uint id)
			{
				this.Id = id;
			}

			// Token: 0x1700015C RID: 348
			// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00017AD4 File Offset: 0x00015CD4
			public int Size
			{
				get
				{
					return 4;
				}
			}

			// Token: 0x060004E9 RID: 1257 RVA: 0x00017AD7 File Offset: 0x00015CD7
			public int ReadFrom(byte[] buffer, int offset)
			{
				this.Id = EndianUtilities.ToUInt32LittleEndian(buffer, offset);
				return 4;
			}

			// Token: 0x060004EA RID: 1258 RVA: 0x00017AE7 File Offset: 0x00015CE7
			public void WriteTo(byte[] buffer, int offset)
			{
				EndianUtilities.WriteBytesLittleEndian(this.Id, buffer, offset);
			}

			// Token: 0x060004EB RID: 1259 RVA: 0x00017AF6 File Offset: 0x00015CF6
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "[Key-Id:{0}]", new object[]
				{
					this.Id
				});
			}

			// Token: 0x04000262 RID: 610
			public uint Id;
		}

		// Token: 0x02000090 RID: 144
		internal sealed class IdIndexData : SecurityDescriptors.IndexData, IByteArraySerializable
		{
			// Token: 0x1700015D RID: 349
			// (get) Token: 0x060004EC RID: 1260 RVA: 0x00017B1B File Offset: 0x00015D1B
			public int Size
			{
				get
				{
					return 20;
				}
			}

			// Token: 0x060004ED RID: 1261 RVA: 0x00017B1F File Offset: 0x00015D1F
			public int ReadFrom(byte[] buffer, int offset)
			{
				this.Hash = EndianUtilities.ToUInt32LittleEndian(buffer, offset);
				this.Id = EndianUtilities.ToUInt32LittleEndian(buffer, offset + 4);
				this.SdsOffset = EndianUtilities.ToInt64LittleEndian(buffer, offset + 8);
				this.SdsLength = EndianUtilities.ToInt32LittleEndian(buffer, offset + 16);
				return 20;
			}

			// Token: 0x060004EE RID: 1262 RVA: 0x00017B5E File Offset: 0x00015D5E
			public void WriteTo(byte[] buffer, int offset)
			{
				EndianUtilities.WriteBytesLittleEndian(this.Hash, buffer, offset);
				EndianUtilities.WriteBytesLittleEndian(this.Id, buffer, offset + 4);
				EndianUtilities.WriteBytesLittleEndian(this.SdsOffset, buffer, offset + 8);
				EndianUtilities.WriteBytesLittleEndian(this.SdsLength, buffer, offset + 16);
			}
		}

		// Token: 0x02000091 RID: 145
		private class HashFinder : IComparable<SecurityDescriptors.HashIndexKey>
		{
			// Token: 0x060004F0 RID: 1264 RVA: 0x00017BA3 File Offset: 0x00015DA3
			public HashFinder(uint toMatch)
			{
				this._toMatch = toMatch;
			}

			// Token: 0x060004F1 RID: 1265 RVA: 0x00017BB2 File Offset: 0x00015DB2
			public int CompareTo(SecurityDescriptors.HashIndexKey other)
			{
				return this.CompareTo(other.Hash);
			}

			// Token: 0x060004F2 RID: 1266 RVA: 0x00017BC0 File Offset: 0x00015DC0
			public int CompareTo(uint otherHash)
			{
				if (this._toMatch < otherHash)
				{
					return -1;
				}
				if (this._toMatch > otherHash)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x04000263 RID: 611
			private readonly uint _toMatch;
		}
	}
}
