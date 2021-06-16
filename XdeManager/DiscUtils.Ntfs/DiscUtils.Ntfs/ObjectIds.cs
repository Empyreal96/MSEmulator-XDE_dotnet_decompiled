using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000046 RID: 70
	internal sealed class ObjectIds
	{
		// Token: 0x0600035E RID: 862 RVA: 0x00012D72 File Offset: 0x00010F72
		public ObjectIds(File file)
		{
			this._file = file;
			this._index = new IndexView<ObjectIds.IndexKey, ObjectIdRecord>(file.GetIndex("$O"));
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00012D97 File Offset: 0x00010F97
		internal IEnumerable<KeyValuePair<Guid, ObjectIdRecord>> All
		{
			get
			{
				foreach (KeyValuePair<ObjectIds.IndexKey, ObjectIdRecord> keyValuePair in this._index.Entries)
				{
					yield return new KeyValuePair<Guid, ObjectIdRecord>(keyValuePair.Key.Id, keyValuePair.Value);
				}
				IEnumerator<KeyValuePair<ObjectIds.IndexKey, ObjectIdRecord>> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00012DA8 File Offset: 0x00010FA8
		internal void Add(Guid objId, FileRecordReference mftRef, Guid birthId, Guid birthVolumeId, Guid birthDomainId)
		{
			ObjectIds.IndexKey indexKey = new ObjectIds.IndexKey();
			indexKey.Id = objId;
			ObjectIdRecord objectIdRecord = new ObjectIdRecord();
			objectIdRecord.MftReference = mftRef;
			objectIdRecord.BirthObjectId = birthId;
			objectIdRecord.BirthVolumeId = birthVolumeId;
			objectIdRecord.BirthDomainId = birthDomainId;
			this._index[indexKey] = objectIdRecord;
			this._file.UpdateRecordInMft();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00012E00 File Offset: 0x00011000
		internal void Remove(Guid objId)
		{
			ObjectIds.IndexKey indexKey = new ObjectIds.IndexKey();
			indexKey.Id = objId;
			this._index.Remove(indexKey);
			this._file.UpdateRecordInMft();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00012E34 File Offset: 0x00011034
		internal bool TryGetValue(Guid objId, out ObjectIdRecord value)
		{
			ObjectIds.IndexKey indexKey = new ObjectIds.IndexKey();
			indexKey.Id = objId;
			return this._index.TryGetValue(indexKey, out value);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00012E5C File Offset: 0x0001105C
		internal void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "OBJECT ID INDEX");
			foreach (KeyValuePair<ObjectIds.IndexKey, ObjectIdRecord> keyValuePair in this._index.Entries)
			{
				writer.WriteLine(indent + "  OBJECT ID INDEX ENTRY");
				writer.WriteLine(indent + "             Id: " + keyValuePair.Key.Id);
				writer.WriteLine(indent + "  MFT Reference: " + keyValuePair.Value.MftReference);
				writer.WriteLine(indent + "   Birth Volume: " + keyValuePair.Value.BirthVolumeId);
				writer.WriteLine(indent + "       Birth Id: " + keyValuePair.Value.BirthObjectId);
				writer.WriteLine(indent + "   Birth Domain: " + keyValuePair.Value.BirthDomainId);
			}
		}

		// Token: 0x0400015F RID: 351
		private readonly File _file;

		// Token: 0x04000160 RID: 352
		private readonly IndexView<ObjectIds.IndexKey, ObjectIdRecord> _index;

		// Token: 0x02000085 RID: 133
		internal sealed class IndexKey : IByteArraySerializable
		{
			// Token: 0x17000152 RID: 338
			// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00017391 File Offset: 0x00015591
			public int Size
			{
				get
				{
					return 16;
				}
			}

			// Token: 0x060004B2 RID: 1202 RVA: 0x00017395 File Offset: 0x00015595
			public int ReadFrom(byte[] buffer, int offset)
			{
				this.Id = EndianUtilities.ToGuidLittleEndian(buffer, offset);
				return 16;
			}

			// Token: 0x060004B3 RID: 1203 RVA: 0x000173A6 File Offset: 0x000155A6
			public void WriteTo(byte[] buffer, int offset)
			{
				EndianUtilities.WriteBytesLittleEndian(this.Id, buffer, offset);
			}

			// Token: 0x060004B4 RID: 1204 RVA: 0x000173B5 File Offset: 0x000155B5
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "[Key-Id:{0}]", new object[]
				{
					this.Id
				});
			}

			// Token: 0x0400024A RID: 586
			public Guid Id;
		}
	}
}
