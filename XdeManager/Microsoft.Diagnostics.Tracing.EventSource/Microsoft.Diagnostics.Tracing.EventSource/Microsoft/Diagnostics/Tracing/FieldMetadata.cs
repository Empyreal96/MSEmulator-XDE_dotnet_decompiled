using System;
using System.Text;
using Microsoft.Diagnostics.Tracing.Internal;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000039 RID: 57
	internal class FieldMetadata
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x0000D59A File Offset: 0x0000B79A
		public FieldMetadata(string name, TraceLoggingDataType type, EventFieldTags tags, bool variableCount) : this(name, type, tags, variableCount ? 64 : 0, 0, null)
		{
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000D5B0 File Offset: 0x0000B7B0
		public FieldMetadata(string name, TraceLoggingDataType type, EventFieldTags tags, ushort fixedCount) : this(name, type, tags, 32, fixedCount, null)
		{
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000D5C0 File Offset: 0x0000B7C0
		public FieldMetadata(string name, TraceLoggingDataType type, EventFieldTags tags, byte[] custom) : this(name, type, tags, 96, checked((ushort)((custom == null) ? 0 : custom.Length)), custom)
		{
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000D5DC File Offset: 0x0000B7DC
		private FieldMetadata(string name, TraceLoggingDataType dataType, EventFieldTags tags, byte countFlags, ushort fixedCount = 0, byte[] custom = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "This usually means that the object passed to Write is of a type that does not support being used as the top-level object in an event, e.g. a primitive or built-in type.");
			}
			Statics.CheckName(name);
			int num = (int)(dataType & (TraceLoggingDataType)31);
			this.name = name;
			checked
			{
				this.nameSize = Encoding.UTF8.GetByteCount(this.name) + 1;
				this.inType = (byte)(num | (int)countFlags);
				this.outType = (byte)(dataType >> 8 & (TraceLoggingDataType)127);
				this.tags = tags;
				this.fixedCount = fixedCount;
				this.custom = custom;
				if (countFlags != 0)
				{
					if (num == 0)
					{
						throw new NotSupportedException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NotSupportedArrayOfNil", new object[0]));
					}
					if (num == 14)
					{
						throw new NotSupportedException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NotSupportedArrayOfBinary", new object[0]));
					}
					if (num == 1 || num == 2)
					{
						throw new NotSupportedException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NotSupportedArrayOfNullTerminatedString", new object[0]));
					}
				}
				if ((this.tags & (EventFieldTags)268435455) != EventFieldTags.None)
				{
					this.outType |= 128;
				}
				if (this.outType != 0)
				{
					this.inType |= 128;
				}
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000D6F0 File Offset: 0x0000B8F0
		public void IncrementStructFieldCount()
		{
			this.inType |= 128;
			checked
			{
				this.outType += 1;
				if ((this.outType & 127) == 0)
				{
					throw new NotSupportedException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_TooManyFields", new object[0]));
				}
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000D740 File Offset: 0x0000B940
		public void Encode(ref int pos, byte[] metadata)
		{
			if (metadata != null)
			{
				Encoding.UTF8.GetBytes(this.name, 0, this.name.Length, metadata, pos);
			}
			checked
			{
				pos += this.nameSize;
				if (metadata != null)
				{
					metadata[pos] = this.inType;
				}
				pos++;
				if ((this.inType & 128) != 0)
				{
					if (metadata != null)
					{
						metadata[pos] = this.outType;
					}
					pos++;
					if ((this.outType & 128) != 0)
					{
						Statics.EncodeTags((int)this.tags, ref pos, metadata);
					}
				}
				if ((this.inType & 32) != 0)
				{
					if (metadata != null)
					{
						metadata[pos] = unchecked((byte)this.fixedCount);
						metadata[pos + 1] = (byte)(this.fixedCount >> 8);
					}
					pos += 2;
					if (96 == (this.inType & 96) && this.fixedCount != 0)
					{
						if (metadata != null)
						{
							Buffer.BlockCopy(this.custom, 0, metadata, pos, (int)this.fixedCount);
						}
						pos += (int)this.fixedCount;
					}
				}
			}
		}

		// Token: 0x0400011E RID: 286
		private readonly string name;

		// Token: 0x0400011F RID: 287
		private readonly int nameSize;

		// Token: 0x04000120 RID: 288
		private readonly EventFieldTags tags;

		// Token: 0x04000121 RID: 289
		private readonly byte[] custom;

		// Token: 0x04000122 RID: 290
		private readonly ushort fixedCount;

		// Token: 0x04000123 RID: 291
		private byte inType;

		// Token: 0x04000124 RID: 292
		private byte outType;
	}
}
