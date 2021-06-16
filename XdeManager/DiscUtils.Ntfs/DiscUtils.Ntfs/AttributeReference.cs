using System;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200000C RID: 12
	internal class AttributeReference : IComparable<AttributeReference>, IEquatable<AttributeReference>
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00002CCE File Offset: 0x00000ECE
		public AttributeReference(FileRecordReference fileReference, ushort attributeId)
		{
			this._fileReference = fileReference;
			this.AttributeId = attributeId;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002CE4 File Offset: 0x00000EE4
		public ushort AttributeId { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002CEC File Offset: 0x00000EEC
		public FileRecordReference File
		{
			get
			{
				return this._fileReference;
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002CF4 File Offset: 0x00000EF4
		public int CompareTo(AttributeReference other)
		{
			int num = this._fileReference.CompareTo(other._fileReference);
			if (num != 0)
			{
				return num;
			}
			return this.AttributeId.CompareTo(other.AttributeId);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002D2C File Offset: 0x00000F2C
		public bool Equals(AttributeReference other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002D38 File Offset: 0x00000F38
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this._fileReference,
				".attr[",
				this.AttributeId,
				"]"
			});
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002D74 File Offset: 0x00000F74
		public override bool Equals(object obj)
		{
			AttributeReference attributeReference = obj as AttributeReference;
			return attributeReference != null && this.Equals(attributeReference);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002D94 File Offset: 0x00000F94
		public override int GetHashCode()
		{
			return this._fileReference.GetHashCode() ^ this.AttributeId.GetHashCode();
		}

		// Token: 0x04000025 RID: 37
		private FileRecordReference _fileReference;
	}
}
