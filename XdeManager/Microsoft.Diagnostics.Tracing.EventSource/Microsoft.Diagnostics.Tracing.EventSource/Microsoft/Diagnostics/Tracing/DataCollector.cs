using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Diagnostics.Tracing.Internal;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200001B RID: 27
	[SecurityCritical]
	internal struct DataCollector
	{
		// Token: 0x06000107 RID: 263 RVA: 0x00009830 File Offset: 0x00007A30
		internal unsafe void Enable(byte* scratch, int scratchSize, EventSource.EventData* datas, int dataCount, GCHandle* pins, int pinCount)
		{
			this.datasStart = datas;
			checked
			{
				this.scratchEnd = scratch + scratchSize;
				this.datasEnd = datas + dataCount;
				this.pinsEnd = pins + pinCount;
				this.scratch = scratch;
				this.datas = datas;
				this.pins = pins;
				this.writingScalars = false;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000988F File Offset: 0x00007A8F
		internal void Disable()
		{
			this = default(DataCollector);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00009898 File Offset: 0x00007A98
		internal unsafe EventSource.EventData* Finish()
		{
			this.ScalarsEnd();
			return this.datas;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000098A8 File Offset: 0x00007AA8
		internal unsafe void AddScalar(void* value, int size)
		{
			checked
			{
				if (this.bufferNesting != 0)
				{
					int num = this.bufferPos;
					this.bufferPos += size;
					this.EnsureBuffer();
					int num2 = 0;
					while (num2 != size)
					{
						this.buffer[num] = unchecked(((byte*)value)[num2]);
						num2++;
						num++;
					}
					return;
				}
				byte* ptr = this.scratch;
				byte* ptr2 = ptr + size;
				if (this.scratchEnd < ptr2)
				{
					throw new IndexOutOfRangeException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_AddScalarOutOfRange", new object[0]));
				}
				this.ScalarsBegin();
				this.scratch = ptr2;
				for (int num3 = 0; num3 != size; num3++)
				{
					unchecked
					{
						ptr[num3] = ((byte*)value)[num3];
					}
				}
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000994C File Offset: 0x00007B4C
		internal unsafe void AddBinary(string value, int size)
		{
			if (size > 65535)
			{
				size = 65534;
			}
			if (this.bufferNesting != 0)
			{
				this.EnsureBuffer(checked(size + 2));
			}
			this.AddScalar((void*)(&size), 2);
			checked
			{
				if (size != 0)
				{
					if (this.bufferNesting == 0)
					{
						this.ScalarsEnd();
						this.PinArray(value, size);
						return;
					}
					int startIndex = this.bufferPos;
					this.bufferPos += size;
					this.EnsureBuffer();
					fixed (void* value2 = value)
					{
						Marshal.Copy((IntPtr)value2, this.buffer, startIndex, size);
					}
				}
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000099DB File Offset: 0x00007BDB
		internal void AddBinary(Array value, int size)
		{
			this.AddArray(value, size, 1);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000099E8 File Offset: 0x00007BE8
		internal unsafe void AddArray(Array value, int length, int itemSize)
		{
			if (length > 65535)
			{
				length = 65535;
			}
			checked
			{
				int num = length * itemSize;
				if (this.bufferNesting != 0)
				{
					this.EnsureBuffer(num + 2);
				}
				this.AddScalar(unchecked((void*)(&length)), 2);
				if (length != 0)
				{
					if (this.bufferNesting == 0)
					{
						this.ScalarsEnd();
						this.PinArray(value, num);
						return;
					}
					int dstOffset = this.bufferPos;
					this.bufferPos += num;
					this.EnsureBuffer();
					Buffer.BlockCopy(value, 0, this.buffer, dstOffset, num);
				}
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00009A67 File Offset: 0x00007C67
		internal int BeginBufferedArray()
		{
			this.BeginBuffered();
			checked
			{
				this.bufferPos += 2;
				return this.bufferPos;
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00009A83 File Offset: 0x00007C83
		internal void EndBufferedArray(int bookmark, int count)
		{
			this.EnsureBuffer();
			this.buffer[checked(bookmark - 2)] = (byte)count;
			this.buffer[checked(bookmark - 1)] = (byte)(count >> 8);
			this.EndBuffered();
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00009AAB File Offset: 0x00007CAB
		internal void BeginBuffered()
		{
			this.ScalarsEnd();
			checked
			{
				this.bufferNesting++;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00009AC1 File Offset: 0x00007CC1
		internal void EndBuffered()
		{
			checked
			{
				this.bufferNesting--;
				if (this.bufferNesting == 0)
				{
					this.EnsureBuffer();
					this.PinArray(this.buffer, this.bufferPos);
					this.buffer = null;
					this.bufferPos = 0;
				}
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00009B00 File Offset: 0x00007D00
		private void EnsureBuffer()
		{
			int num = this.bufferPos;
			if (this.buffer == null || this.buffer.Length < num)
			{
				this.GrowBuffer(num);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00009B30 File Offset: 0x00007D30
		private void EnsureBuffer(int additionalSize)
		{
			int num = checked(this.bufferPos + additionalSize);
			if (this.buffer == null || this.buffer.Length < num)
			{
				this.GrowBuffer(num);
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00009B60 File Offset: 0x00007D60
		private void GrowBuffer(int required)
		{
			int num = (this.buffer == null) ? 64 : this.buffer.Length;
			checked
			{
				do
				{
					num *= 2;
				}
				while (num < required);
				Array.Resize<byte>(ref this.buffer, num);
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00009B98 File Offset: 0x00007D98
		private unsafe void PinArray(object value, int size)
		{
			GCHandle* ptr = this.pins;
			if (this.pinsEnd == ptr)
			{
				throw new IndexOutOfRangeException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_PinArrayOutOfRange", new object[0]));
			}
			EventSource.EventData* ptr2 = this.datas;
			if (this.datasEnd == ptr2)
			{
				throw new IndexOutOfRangeException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_DataDescriptorsOutOfRange", new object[0]));
			}
			checked
			{
				this.pins = ptr + 1;
				this.datas = ptr2 + 1;
				*ptr = GCHandle.Alloc(value, GCHandleType.Pinned);
				ptr2->m_Ptr = (long)((ulong)((UIntPtr)((void*)ptr->AddrOfPinnedObject())));
				ptr2->m_Size = size;
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00009C40 File Offset: 0x00007E40
		private unsafe void ScalarsBegin()
		{
			if (!this.writingScalars)
			{
				EventSource.EventData* ptr = this.datas;
				if (this.datasEnd == ptr)
				{
					throw new IndexOutOfRangeException(Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("EventSource_DataDescriptorsOutOfRange", new object[0]));
				}
				ptr->m_Ptr = checked((long)((ulong)((UIntPtr)((void*)this.scratch))));
				this.writingScalars = true;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00009C9C File Offset: 0x00007E9C
		private unsafe void ScalarsEnd()
		{
			if (this.writingScalars)
			{
				EventSource.EventData* ptr = this.datas;
				ptr->m_Size = (this.scratch - checked((UIntPtr)ptr->m_Ptr)) / 1;
				this.datas = checked(ptr + 1);
				this.writingScalars = false;
			}
		}

		// Token: 0x04000089 RID: 137
		[ThreadStatic]
		internal static DataCollector ThreadInstance;

		// Token: 0x0400008A RID: 138
		private unsafe byte* scratchEnd;

		// Token: 0x0400008B RID: 139
		private unsafe EventSource.EventData* datasEnd;

		// Token: 0x0400008C RID: 140
		private unsafe GCHandle* pinsEnd;

		// Token: 0x0400008D RID: 141
		private unsafe EventSource.EventData* datasStart;

		// Token: 0x0400008E RID: 142
		private unsafe byte* scratch;

		// Token: 0x0400008F RID: 143
		private unsafe EventSource.EventData* datas;

		// Token: 0x04000090 RID: 144
		private unsafe GCHandle* pins;

		// Token: 0x04000091 RID: 145
		private byte[] buffer;

		// Token: 0x04000092 RID: 146
		private int bufferPos;

		// Token: 0x04000093 RID: 147
		private int bufferNesting;

		// Token: 0x04000094 RID: 148
		private bool writingScalars;
	}
}
