using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000009 RID: 9
	internal class AttributeList : IByteArraySerializable, IDiagnosticTraceable, ICollection<AttributeListRecord>, IEnumerable<AttributeListRecord>, IEnumerable
	{
		// Token: 0x06000011 RID: 17 RVA: 0x0000254B File Offset: 0x0000074B
		public AttributeList()
		{
			this._records = new List<AttributeListRecord>();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002560 File Offset: 0x00000760
		public int Size
		{
			get
			{
				int num = 0;
				foreach (AttributeListRecord attributeListRecord in this._records)
				{
					num += attributeListRecord.Size;
				}
				return num;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025B8 File Offset: 0x000007B8
		public int ReadFrom(byte[] buffer, int offset)
		{
			this._records.Clear();
			int i = 0;
			while (i < buffer.Length)
			{
				AttributeListRecord attributeListRecord = new AttributeListRecord();
				i += attributeListRecord.ReadFrom(buffer, offset + i);
				this._records.Add(attributeListRecord);
			}
			return i;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025FC File Offset: 0x000007FC
		public void WriteTo(byte[] buffer, int offset)
		{
			int num = offset;
			foreach (AttributeListRecord attributeListRecord in this._records)
			{
				attributeListRecord.WriteTo(buffer, offset + num);
				num += attributeListRecord.Size;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002660 File Offset: 0x00000860
		public int Count
		{
			get
			{
				return this._records.Count;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000266D File Offset: 0x0000086D
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002670 File Offset: 0x00000870
		public void Add(AttributeListRecord item)
		{
			this._records.Add(item);
			this._records.Sort();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002689 File Offset: 0x00000889
		public void Clear()
		{
			this._records.Clear();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002696 File Offset: 0x00000896
		public bool Contains(AttributeListRecord item)
		{
			return this._records.Contains(item);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000026A4 File Offset: 0x000008A4
		public void CopyTo(AttributeListRecord[] array, int arrayIndex)
		{
			this._records.CopyTo(array, arrayIndex);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000026B3 File Offset: 0x000008B3
		public bool Remove(AttributeListRecord item)
		{
			return this._records.Remove(item);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000026C1 File Offset: 0x000008C1
		public IEnumerator<AttributeListRecord> GetEnumerator()
		{
			return this._records.GetEnumerator();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000026D3 File Offset: 0x000008D3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._records.GetEnumerator();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026E8 File Offset: 0x000008E8
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "ATTRIBUTE LIST RECORDS");
			foreach (AttributeListRecord attributeListRecord in this._records)
			{
				attributeListRecord.Dump(writer, indent + "  ");
			}
		}

		// Token: 0x04000017 RID: 23
		private readonly List<AttributeListRecord> _records;
	}
}
