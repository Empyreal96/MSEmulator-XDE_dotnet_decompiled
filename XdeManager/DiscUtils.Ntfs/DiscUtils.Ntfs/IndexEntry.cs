using System;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200002A RID: 42
	internal class IndexEntry
	{
		// Token: 0x06000196 RID: 406 RVA: 0x0000900A File Offset: 0x0000720A
		public IndexEntry(bool isFileIndexEntry)
		{
			this.IsFileIndexEntry = isFileIndexEntry;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00009019 File Offset: 0x00007219
		public IndexEntry(IndexEntry toCopy, byte[] newKey, byte[] newData)
		{
			this.IsFileIndexEntry = toCopy.IsFileIndexEntry;
			this._flags = toCopy._flags;
			this._vcn = toCopy._vcn;
			this._keyBuffer = newKey;
			this._dataBuffer = newData;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00009053 File Offset: 0x00007253
		public IndexEntry(byte[] key, byte[] data, bool isFileIndexEntry)
		{
			this.IsFileIndexEntry = isFileIndexEntry;
			this._flags = IndexEntryFlags.None;
			this._keyBuffer = key;
			this._dataBuffer = data;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00009077 File Offset: 0x00007277
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000907F File Offset: 0x0000727F
		public long ChildrenVirtualCluster
		{
			get
			{
				return this._vcn;
			}
			set
			{
				this._vcn = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00009088 File Offset: 0x00007288
		// (set) Token: 0x0600019C RID: 412 RVA: 0x00009090 File Offset: 0x00007290
		public byte[] DataBuffer
		{
			get
			{
				return this._dataBuffer;
			}
			set
			{
				this._dataBuffer = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00009099 File Offset: 0x00007299
		// (set) Token: 0x0600019E RID: 414 RVA: 0x000090A1 File Offset: 0x000072A1
		public IndexEntryFlags Flags
		{
			get
			{
				return this._flags;
			}
			set
			{
				this._flags = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000090AA File Offset: 0x000072AA
		protected bool IsFileIndexEntry { get; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000090B2 File Offset: 0x000072B2
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x000090BA File Offset: 0x000072BA
		public byte[] KeyBuffer
		{
			get
			{
				return this._keyBuffer;
			}
			set
			{
				this._keyBuffer = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x000090C4 File Offset: 0x000072C4
		public virtual int Size
		{
			get
			{
				int num = 16;
				if ((this._flags & IndexEntryFlags.End) == IndexEntryFlags.None)
				{
					num += this._keyBuffer.Length;
					num += (this.IsFileIndexEntry ? 0 : this._dataBuffer.Length);
				}
				num = MathUtilities.RoundUp(num, 8);
				if ((this._flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
				{
					num += 8;
				}
				return num;
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00009118 File Offset: 0x00007318
		public virtual void Read(byte[] buffer, int offset)
		{
			EndianUtilities.ToUInt16LittleEndian(buffer, offset);
			ushort num = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 2);
			ushort num2 = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 8);
			ushort num3 = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 10);
			this._flags = (IndexEntryFlags)EndianUtilities.ToUInt16LittleEndian(buffer, offset + 12);
			if ((this._flags & IndexEntryFlags.End) == IndexEntryFlags.None)
			{
				this._keyBuffer = new byte[(int)num3];
				Array.Copy(buffer, offset + 16, this._keyBuffer, 0, (int)num3);
				if (this.IsFileIndexEntry)
				{
					this._dataBuffer = new byte[8];
					Array.Copy(buffer, offset, this._dataBuffer, 0, 8);
				}
				else
				{
					this._dataBuffer = new byte[(int)num];
					Array.Copy(buffer, offset + 16 + (int)num3, this._dataBuffer, 0, (int)num);
				}
			}
			if ((this._flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
			{
				this._vcn = EndianUtilities.ToInt64LittleEndian(buffer, offset + (int)num2 - 8);
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000091E4 File Offset: 0x000073E4
		public virtual void WriteTo(byte[] buffer, int offset)
		{
			ushort num = (ushort)this.Size;
			if ((this._flags & IndexEntryFlags.End) == IndexEntryFlags.None)
			{
				ushort num2 = (ushort)this._keyBuffer.Length;
				if (this.IsFileIndexEntry)
				{
					Array.Copy(this._dataBuffer, 0, buffer, offset, 8);
				}
				else
				{
					ushort num3 = this.IsFileIndexEntry ? 0 : (16 + num2);
					ushort val = (ushort)this._dataBuffer.Length;
					EndianUtilities.WriteBytesLittleEndian(num3, buffer, offset);
					EndianUtilities.WriteBytesLittleEndian(val, buffer, offset + 2);
					Array.Copy(this._dataBuffer, 0, buffer, offset + (int)num3, this._dataBuffer.Length);
				}
				EndianUtilities.WriteBytesLittleEndian(num2, buffer, offset + 10);
				Array.Copy(this._keyBuffer, 0, buffer, offset + 16, this._keyBuffer.Length);
			}
			else
			{
				EndianUtilities.WriteBytesLittleEndian(0, buffer, offset);
				EndianUtilities.WriteBytesLittleEndian(0, buffer, offset + 2);
				EndianUtilities.WriteBytesLittleEndian(0, buffer, offset + 10);
			}
			EndianUtilities.WriteBytesLittleEndian(num, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian((ushort)this._flags, buffer, offset + 12);
			if ((this._flags & IndexEntryFlags.Node) != IndexEntryFlags.None)
			{
				EndianUtilities.WriteBytesLittleEndian(this._vcn, buffer, offset + (int)num - 8);
			}
		}

		// Token: 0x040000CB RID: 203
		public const int EndNodeSize = 24;

		// Token: 0x040000CC RID: 204
		protected byte[] _dataBuffer;

		// Token: 0x040000CD RID: 205
		protected IndexEntryFlags _flags;

		// Token: 0x040000CE RID: 206
		protected byte[] _keyBuffer;

		// Token: 0x040000CF RID: 207
		protected long _vcn;
	}
}
