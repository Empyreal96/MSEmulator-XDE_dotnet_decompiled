using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000021 RID: 33
	internal abstract class FixupRecordBase
	{
		// Token: 0x06000142 RID: 322 RVA: 0x00008001 File Offset: 0x00006201
		public FixupRecordBase(string magic, int sectorSize)
		{
			this.Magic = magic;
			this._sectorSize = sectorSize;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00008017 File Offset: 0x00006217
		public FixupRecordBase(string magic, int sectorSize, int recordLength)
		{
			this.Initialize(magic, sectorSize, recordLength);
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00008028 File Offset: 0x00006228
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00008030 File Offset: 0x00006230
		public string Magic { get; private set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00008039 File Offset: 0x00006239
		public int Size
		{
			get
			{
				return this.CalcSize();
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00008041 File Offset: 0x00006241
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00008049 File Offset: 0x00006249
		public ushort UpdateSequenceCount { get; private set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00008052 File Offset: 0x00006252
		// (set) Token: 0x0600014A RID: 330 RVA: 0x0000805A File Offset: 0x0000625A
		public ushort UpdateSequenceNumber { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00008063 File Offset: 0x00006263
		// (set) Token: 0x0600014C RID: 332 RVA: 0x0000806B File Offset: 0x0000626B
		public ushort UpdateSequenceOffset { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00008074 File Offset: 0x00006274
		public int UpdateSequenceSize
		{
			get
			{
				return (int)(this.UpdateSequenceCount * 2);
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000807E File Offset: 0x0000627E
		public void FromBytes(byte[] buffer, int offset)
		{
			this.FromBytes(buffer, offset, false);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000808C File Offset: 0x0000628C
		public void FromBytes(byte[] buffer, int offset, bool ignoreMagic)
		{
			string text = EndianUtilities.BytesToString(buffer, offset, 4);
			if (this.Magic == null)
			{
				this.Magic = text;
			}
			else
			{
				if (text != this.Magic && ignoreMagic)
				{
					return;
				}
				if (text != this.Magic)
				{
					throw new IOException("Corrupt record");
				}
			}
			this.UpdateSequenceOffset = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 4);
			this.UpdateSequenceCount = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 6);
			this.UpdateSequenceNumber = EndianUtilities.ToUInt16LittleEndian(buffer, offset + (int)this.UpdateSequenceOffset);
			this._updateSequenceArray = new ushort[(int)(this.UpdateSequenceCount - 1)];
			for (int i = 0; i < this._updateSequenceArray.Length; i++)
			{
				this._updateSequenceArray[i] = EndianUtilities.ToUInt16LittleEndian(buffer, offset + (int)this.UpdateSequenceOffset + 2 * (i + 1));
			}
			this.UnprotectBuffer(buffer, offset);
			this.Read(buffer, offset);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00008164 File Offset: 0x00006364
		public void ToBytes(byte[] buffer, int offset)
		{
			this.UpdateSequenceOffset = this.Write(buffer, offset);
			this.ProtectBuffer(buffer, offset);
			EndianUtilities.StringToBytes(this.Magic, buffer, offset, 4);
			EndianUtilities.WriteBytesLittleEndian(this.UpdateSequenceOffset, buffer, offset + 4);
			EndianUtilities.WriteBytesLittleEndian(this.UpdateSequenceCount, buffer, offset + 6);
			EndianUtilities.WriteBytesLittleEndian(this.UpdateSequenceNumber, buffer, offset + (int)this.UpdateSequenceOffset);
			for (int i = 0; i < this._updateSequenceArray.Length; i++)
			{
				EndianUtilities.WriteBytesLittleEndian(this._updateSequenceArray[i], buffer, offset + (int)this.UpdateSequenceOffset + 2 * (i + 1));
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000081F6 File Offset: 0x000063F6
		protected void Initialize(string magic, int sectorSize, int recordLength)
		{
			this.Magic = magic;
			this._sectorSize = sectorSize;
			this.UpdateSequenceCount = (ushort)(1 + MathUtilities.Ceil(recordLength, 512));
			this.UpdateSequenceNumber = 1;
			this._updateSequenceArray = new ushort[(int)(this.UpdateSequenceCount - 1)];
		}

		// Token: 0x06000152 RID: 338
		protected abstract void Read(byte[] buffer, int offset);

		// Token: 0x06000153 RID: 339
		protected abstract ushort Write(byte[] buffer, int offset);

		// Token: 0x06000154 RID: 340
		protected abstract int CalcSize();

		// Token: 0x06000155 RID: 341 RVA: 0x00008234 File Offset: 0x00006434
		private void UnprotectBuffer(byte[] buffer, int offset)
		{
			for (int i = 0; i < this._updateSequenceArray.Length; i++)
			{
				if (this.UpdateSequenceNumber != EndianUtilities.ToUInt16LittleEndian(buffer, offset + 512 * (i + 1) - 2))
				{
					throw new IOException("Corrupt file system record found");
				}
			}
			for (int j = 0; j < this._updateSequenceArray.Length; j++)
			{
				EndianUtilities.WriteBytesLittleEndian(this._updateSequenceArray[j], buffer, offset + 512 * (j + 1) - 2);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000082A8 File Offset: 0x000064A8
		private void ProtectBuffer(byte[] buffer, int offset)
		{
			ushort updateSequenceNumber = this.UpdateSequenceNumber;
			this.UpdateSequenceNumber = updateSequenceNumber + 1;
			for (int i = 0; i < this._updateSequenceArray.Length; i++)
			{
				this._updateSequenceArray[i] = EndianUtilities.ToUInt16LittleEndian(buffer, offset + 512 * (i + 1) - 2);
			}
			for (int j = 0; j < this._updateSequenceArray.Length; j++)
			{
				EndianUtilities.WriteBytesLittleEndian(this.UpdateSequenceNumber, buffer, offset + 512 * (j + 1) - 2);
			}
		}

		// Token: 0x040000B2 RID: 178
		private int _sectorSize;

		// Token: 0x040000B3 RID: 179
		private ushort[] _updateSequenceArray;
	}
}
