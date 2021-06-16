using System;
using System.Collections.Generic;

namespace DiscUtils.Streams
{
	// Token: 0x02000026 RID: 38
	public sealed class SparseMemoryBuffer : Buffer
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00004B2D File Offset: 0x00002D2D
		public SparseMemoryBuffer(int chunkSize)
		{
			this.ChunkSize = chunkSize;
			this._buffers = new Dictionary<int, byte[]>();
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00004B47 File Offset: 0x00002D47
		public IEnumerable<int> AllocatedChunks
		{
			get
			{
				List<int> list = new List<int>(this._buffers.Keys);
				list.Sort();
				return list;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00004B5F File Offset: 0x00002D5F
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00004B62 File Offset: 0x00002D62
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00004B65 File Offset: 0x00002D65
		public override long Capacity
		{
			get
			{
				return this._capacity;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00004B6D File Offset: 0x00002D6D
		public int ChunkSize { get; }

		// Token: 0x17000057 RID: 87
		public byte this[long pos]
		{
			get
			{
				byte[] array = new byte[1];
				if (this.Read(pos, array, 0, 1) != 0)
				{
					return array[0];
				}
				return 0;
			}
			set
			{
				this.Write(pos, new byte[]
				{
					value
				}, 0, 1);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00004BC4 File Offset: 0x00002DC4
		public override int Read(long pos, byte[] buffer, int offset, int count)
		{
			int num = 0;
			while (count > 0 && pos < this._capacity)
			{
				int key = (int)(pos / (long)this.ChunkSize);
				int num2 = (int)(pos % (long)this.ChunkSize);
				int num3 = (int)Math.Min(Math.Min((long)(this.ChunkSize - num2), this._capacity - pos), (long)count);
				byte[] sourceArray;
				if (!this._buffers.TryGetValue(key, out sourceArray))
				{
					Array.Clear(buffer, offset, num3);
				}
				else
				{
					Array.Copy(sourceArray, num2, buffer, offset, num3);
				}
				num += num3;
				offset += num3;
				count -= num3;
				pos += (long)num3;
			}
			return num;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00004C54 File Offset: 0x00002E54
		public override void Write(long pos, byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				int key = (int)(pos / (long)this.ChunkSize);
				int num = (int)(pos % (long)this.ChunkSize);
				int num2 = Math.Min(this.ChunkSize - num, count);
				byte[] array;
				if (!this._buffers.TryGetValue(key, out array))
				{
					array = new byte[this.ChunkSize];
					this._buffers[key] = array;
				}
				Array.Copy(buffer, offset, array, num, num2);
				offset += num2;
				count -= num2;
				pos += (long)num2;
			}
			this._capacity = Math.Max(this._capacity, pos);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004CE4 File Offset: 0x00002EE4
		public override void Clear(long pos, int count)
		{
			while (count > 0)
			{
				int key = (int)(pos / (long)this.ChunkSize);
				int num = (int)(pos % (long)this.ChunkSize);
				int num2 = Math.Min(this.ChunkSize - num, count);
				byte[] array;
				if (this._buffers.TryGetValue(key, out array))
				{
					if (num == 0 && num2 == this.ChunkSize)
					{
						this._buffers.Remove(key);
					}
					else
					{
						Array.Clear(array, num, num2);
					}
				}
				count -= num2;
				pos += (long)num2;
			}
			this._capacity = Math.Max(this._capacity, pos);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004D6C File Offset: 0x00002F6C
		public override void SetCapacity(long value)
		{
			this._capacity = value;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00004D75 File Offset: 0x00002F75
		public override IEnumerable<StreamExtent> GetExtentsInRange(long start, long count)
		{
			long end = start + count;
			foreach (int num in this.AllocatedChunks)
			{
				long num2 = (long)num * (long)this.ChunkSize;
				long num3 = num2 + (long)this.ChunkSize;
				if (num3 > start && num2 < end)
				{
					long num4 = Math.Max(start, num2);
					yield return new StreamExtent(num4, Math.Min(num3, end) - num4);
				}
			}
			IEnumerator<int> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000058 RID: 88
		private readonly Dictionary<int, byte[]> _buffers;

		// Token: 0x04000059 RID: 89
		private long _capacity;
	}
}
