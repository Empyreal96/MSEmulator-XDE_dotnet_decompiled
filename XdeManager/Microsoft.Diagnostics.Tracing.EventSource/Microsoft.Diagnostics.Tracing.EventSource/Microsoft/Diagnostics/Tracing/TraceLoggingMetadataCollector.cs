using System;
using System.Collections.Generic;
using Microsoft.Diagnostics.Tracing.Internal;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200007E RID: 126
	internal class TraceLoggingMetadataCollector
	{
		// Token: 0x06000313 RID: 787 RVA: 0x0000FA80 File Offset: 0x0000DC80
		internal TraceLoggingMetadataCollector()
		{
			this.impl = new TraceLoggingMetadataCollector.Impl();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000FA9E File Offset: 0x0000DC9E
		private TraceLoggingMetadataCollector(TraceLoggingMetadataCollector other, FieldMetadata group)
		{
			this.impl = other.impl;
			this.currentGroup = group;
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000FAC4 File Offset: 0x0000DCC4
		// (set) Token: 0x06000316 RID: 790 RVA: 0x0000FACC File Offset: 0x0000DCCC
		internal EventFieldTags Tags { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000FAD5 File Offset: 0x0000DCD5
		internal int ScratchSize
		{
			get
			{
				return (int)this.impl.scratchSize;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000FAE2 File Offset: 0x0000DCE2
		internal int DataCount
		{
			get
			{
				return (int)this.impl.dataCount;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000FAEF File Offset: 0x0000DCEF
		internal int PinCount
		{
			get
			{
				return (int)this.impl.pinCount;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000FAFC File Offset: 0x0000DCFC
		private bool BeginningBufferedArray
		{
			get
			{
				return this.bufferedArrayFieldCount == 0;
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000FB08 File Offset: 0x0000DD08
		public TraceLoggingMetadataCollector AddGroup(string name)
		{
			TraceLoggingMetadataCollector result = this;
			if (name != null || this.BeginningBufferedArray)
			{
				FieldMetadata fieldMetadata = new FieldMetadata(name, TraceLoggingDataType.Struct, EventFieldTags.None, this.BeginningBufferedArray);
				this.AddField(fieldMetadata);
				result = new TraceLoggingMetadataCollector(this, fieldMetadata);
			}
			return result;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000FB44 File Offset: 0x0000DD44
		public void AddScalar(string name, TraceLoggingDataType type)
		{
			TraceLoggingDataType traceLoggingDataType = type & (TraceLoggingDataType)31;
			int size;
			switch (traceLoggingDataType)
			{
			case TraceLoggingDataType.Int8:
			case TraceLoggingDataType.UInt8:
				break;
			case TraceLoggingDataType.Int16:
			case TraceLoggingDataType.UInt16:
				goto IL_77;
			case TraceLoggingDataType.Int32:
			case TraceLoggingDataType.UInt32:
			case TraceLoggingDataType.Float:
			case TraceLoggingDataType.Boolean32:
			case TraceLoggingDataType.HexInt32:
				size = 4;
				goto IL_93;
			case TraceLoggingDataType.Int64:
			case TraceLoggingDataType.UInt64:
			case TraceLoggingDataType.Double:
			case TraceLoggingDataType.FileTime:
			case TraceLoggingDataType.HexInt64:
				size = 8;
				goto IL_93;
			case TraceLoggingDataType.Binary:
			case (TraceLoggingDataType)16:
			case (TraceLoggingDataType)19:
				goto IL_88;
			case TraceLoggingDataType.Guid:
			case TraceLoggingDataType.SystemTime:
				size = 16;
				goto IL_93;
			default:
				switch (traceLoggingDataType)
				{
				case TraceLoggingDataType.Char8:
					break;
				case (TraceLoggingDataType)517:
					goto IL_88;
				case TraceLoggingDataType.Char16:
					goto IL_77;
				default:
					goto IL_88;
				}
				break;
			}
			size = 1;
			goto IL_93;
			IL_77:
			size = 2;
			goto IL_93;
			IL_88:
			throw new ArgumentOutOfRangeException("type");
			IL_93:
			this.impl.AddScalar(size);
			this.AddField(new FieldMetadata(name, type, this.Tags, this.BeginningBufferedArray));
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000FC0C File Offset: 0x0000DE0C
		public void AddBinary(string name, TraceLoggingDataType type)
		{
			TraceLoggingDataType traceLoggingDataType = type & (TraceLoggingDataType)31;
			if (traceLoggingDataType != TraceLoggingDataType.Binary)
			{
				switch (traceLoggingDataType)
				{
				case TraceLoggingDataType.CountedUtf16String:
				case TraceLoggingDataType.CountedMbcsString:
					break;
				default:
					throw new ArgumentOutOfRangeException("type");
				}
			}
			this.impl.AddScalar(2);
			this.impl.AddNonscalar();
			this.AddField(new FieldMetadata(name, type, this.Tags, this.BeginningBufferedArray));
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000FC70 File Offset: 0x0000DE70
		public void AddArray(string name, TraceLoggingDataType type)
		{
			TraceLoggingDataType traceLoggingDataType = type & (TraceLoggingDataType)31;
			switch (traceLoggingDataType)
			{
			case TraceLoggingDataType.Utf16String:
			case TraceLoggingDataType.MbcsString:
			case TraceLoggingDataType.Int8:
			case TraceLoggingDataType.UInt8:
			case TraceLoggingDataType.Int16:
			case TraceLoggingDataType.UInt16:
			case TraceLoggingDataType.Int32:
			case TraceLoggingDataType.UInt32:
			case TraceLoggingDataType.Int64:
			case TraceLoggingDataType.UInt64:
			case TraceLoggingDataType.Float:
			case TraceLoggingDataType.Double:
			case TraceLoggingDataType.Boolean32:
			case TraceLoggingDataType.Guid:
			case TraceLoggingDataType.FileTime:
			case TraceLoggingDataType.HexInt32:
			case TraceLoggingDataType.HexInt64:
				goto IL_84;
			case TraceLoggingDataType.Binary:
			case (TraceLoggingDataType)16:
			case TraceLoggingDataType.SystemTime:
			case (TraceLoggingDataType)19:
				break;
			default:
				switch (traceLoggingDataType)
				{
				case TraceLoggingDataType.Char8:
				case TraceLoggingDataType.Char16:
					goto IL_84;
				}
				break;
			}
			throw new ArgumentOutOfRangeException("type");
			IL_84:
			if (this.BeginningBufferedArray)
			{
				throw new NotSupportedException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NotSupportedNestedArraysEnums", new object[0]));
			}
			this.impl.AddScalar(2);
			this.impl.AddNonscalar();
			this.AddField(new FieldMetadata(name, type, this.Tags, true));
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000FD4A File Offset: 0x0000DF4A
		public void BeginBufferedArray()
		{
			if (this.bufferedArrayFieldCount >= 0)
			{
				throw new NotSupportedException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NotSupportedNestedArraysEnums", new object[0]));
			}
			this.bufferedArrayFieldCount = 0;
			this.impl.BeginBuffered();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000FD7D File Offset: 0x0000DF7D
		public void EndBufferedArray()
		{
			if (this.bufferedArrayFieldCount != 1)
			{
				throw new InvalidOperationException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_IncorrentlyAuthoredTypeInfo", new object[0]));
			}
			this.bufferedArrayFieldCount = int.MinValue;
			this.impl.EndBuffered();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000FDB4 File Offset: 0x0000DFB4
		public void AddCustom(string name, TraceLoggingDataType type, byte[] metadata)
		{
			if (this.BeginningBufferedArray)
			{
				throw new NotSupportedException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_NotSupportedCustomSerializedData", new object[0]));
			}
			this.impl.AddScalar(2);
			this.impl.AddNonscalar();
			this.AddField(new FieldMetadata(name, type, this.Tags, metadata));
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000FE0C File Offset: 0x0000E00C
		internal byte[] GetMetadata()
		{
			int num = this.impl.Encode(null);
			byte[] array = new byte[num];
			this.impl.Encode(array);
			return array;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000FE3B File Offset: 0x0000E03B
		private void AddField(FieldMetadata fieldMetadata)
		{
			this.Tags = EventFieldTags.None;
			checked
			{
				this.bufferedArrayFieldCount++;
				this.impl.fields.Add(fieldMetadata);
				if (this.currentGroup != null)
				{
					this.currentGroup.IncrementStructFieldCount();
				}
			}
		}

		// Token: 0x0400018D RID: 397
		private readonly TraceLoggingMetadataCollector.Impl impl;

		// Token: 0x0400018E RID: 398
		private readonly FieldMetadata currentGroup;

		// Token: 0x0400018F RID: 399
		private int bufferedArrayFieldCount = int.MinValue;

		// Token: 0x0200007F RID: 127
		private class Impl
		{
			// Token: 0x06000324 RID: 804 RVA: 0x0000FE76 File Offset: 0x0000E076
			public void AddScalar(int size)
			{
				checked
				{
					if (this.bufferNesting == 0)
					{
						if (!this.scalar)
						{
							this.dataCount += 1;
						}
						this.scalar = true;
						this.scratchSize = (short)((int)this.scratchSize + size);
					}
				}
			}

			// Token: 0x06000325 RID: 805 RVA: 0x0000FEAD File Offset: 0x0000E0AD
			public void AddNonscalar()
			{
				checked
				{
					if (this.bufferNesting == 0)
					{
						this.scalar = false;
						this.pinCount += 1;
						this.dataCount += 1;
					}
				}
			}

			// Token: 0x06000326 RID: 806 RVA: 0x0000FEDC File Offset: 0x0000E0DC
			public void BeginBuffered()
			{
				if (this.bufferNesting == 0)
				{
					this.AddNonscalar();
				}
				checked
				{
					this.bufferNesting++;
				}
			}

			// Token: 0x06000327 RID: 807 RVA: 0x0000FEFA File Offset: 0x0000E0FA
			public void EndBuffered()
			{
				checked
				{
					this.bufferNesting--;
				}
			}

			// Token: 0x06000328 RID: 808 RVA: 0x0000FF0C File Offset: 0x0000E10C
			public int Encode(byte[] metadata)
			{
				int result = 0;
				foreach (FieldMetadata fieldMetadata in this.fields)
				{
					fieldMetadata.Encode(ref result, metadata);
				}
				return result;
			}

			// Token: 0x04000191 RID: 401
			internal readonly List<FieldMetadata> fields = new List<FieldMetadata>();

			// Token: 0x04000192 RID: 402
			internal short scratchSize;

			// Token: 0x04000193 RID: 403
			internal sbyte dataCount;

			// Token: 0x04000194 RID: 404
			internal sbyte pinCount;

			// Token: 0x04000195 RID: 405
			private int bufferNesting;

			// Token: 0x04000196 RID: 406
			private bool scalar;
		}
	}
}
