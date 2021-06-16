using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.Spaces.Diskstream
{
	// Token: 0x02000002 RID: 2
	public abstract class Disk : Stream
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected Disk(IntPtr handle)
		{
			bool flag = handle == IntPtr.Zero || handle == Disk.InvalidHandleValue;
			if (flag)
			{
				throw new ArgumentException("The handle is invalid.");
			}
			this.Handle = handle;
			this.IsDisposed = false;
			this.Position = 0L;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x000020AC File Offset: 0x000002AC
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020D4 File Offset: 0x000002D4
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020E8 File Offset: 0x000002E8
		public override long Length
		{
			get
			{
				return (long)Disk.DiskHandleGetLength(this.GetHandle());
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002108 File Offset: 0x00000308
		public long Cylinders
		{
			get
			{
				long num = this.Length / (long)this.BytesPerSector;
				long num2 = (long)(this.TracksPerCylinder * this.SectorsPerTrack);
				return num / num2;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002140 File Offset: 0x00000340
		public int TracksPerCylinder
		{
			get
			{
				return 255;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002158 File Offset: 0x00000358
		public int SectorsPerTrack
		{
			get
			{
				return 63;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000216C File Offset: 0x0000036C
		public int BytesPerSector
		{
			get
			{
				return (int)Disk.DiskHandleGetBytesPerSector(this.GetHandle());
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000218C File Offset: 0x0000038C
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000021A4 File Offset: 0x000003A4
		public override long Position
		{
			get
			{
				return this.position;
			}
			set
			{
				bool flag = this.position < 0L;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				bool flag2 = this.position > this.Length;
				if (flag2)
				{
					throw new EndOfStreamException();
				}
				this.position = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000021EC File Offset: 0x000003EC
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000021F4 File Offset: 0x000003F4
		protected bool IsDisposed { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000021FD File Offset: 0x000003FD
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002205 File Offset: 0x00000405
		protected IntPtr Handle { get; set; }

		// Token: 0x06000010 RID: 16 RVA: 0x00002210 File Offset: 0x00000410
		public override void Flush()
		{
			bool flag = Disk.DiskHandleFlush(this.GetHandle());
			bool flag2 = !flag;
			if (flag2)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000223C File Offset: 0x0000043C
		public override int Read(byte[] buffer, int offset, int count)
		{
			bool flag = buffer == null;
			if (flag)
			{
				throw new ArgumentNullException("buffer");
			}
			bool flag2 = offset + count > buffer.Length;
			if (flag2)
			{
				throw new ArgumentException("The sum of offset and count is greater than the buffer length.");
			}
			bool flag3 = offset < 0;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			bool flag4 = count < 0;
			if (flag4)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			bool flag5 = Disk.DiskHandleRead(this.GetHandle(), buffer, (uint)buffer.Length, (uint)offset, (ulong)this.Position, (uint)count);
			bool flag6 = !flag5;
			if (flag6)
			{
				throw new Win32Exception();
			}
			this.Position += (long)count;
			return count;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022E0 File Offset: 0x000004E0
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool flag = buffer == null;
			if (flag)
			{
				throw new ArgumentNullException("buffer");
			}
			bool flag2 = offset + count > buffer.Length;
			if (flag2)
			{
				throw new ArgumentException("The sum of offset and count is greater than the buffer length.");
			}
			bool flag3 = offset < 0;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			bool flag4 = count < 0;
			if (flag4)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			bool flag5 = Disk.DiskHandleWrite(this.GetHandle(), buffer, (uint)buffer.Length, (uint)offset, (ulong)this.Position, (uint)count);
			bool flag6 = !flag5;
			if (flag6)
			{
				throw new Win32Exception();
			}
			this.Position += (long)count;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002380 File Offset: 0x00000580
		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
			case SeekOrigin.Begin:
				this.Position = offset;
				break;
			case SeekOrigin.Current:
				this.Position += offset;
				break;
			case SeekOrigin.End:
				this.Position = this.Length + offset;
				break;
			}
			return this.Position;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023DC File Offset: 0x000005DC
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023E4 File Offset: 0x000005E4
		internal IntPtr GetHandle()
		{
			bool isDisposed = this.IsDisposed;
			if (isDisposed)
			{
				throw new ObjectDisposedException("Handle");
			}
			return this.Handle;
		}

		// Token: 0x06000016 RID: 22
		[DllImport("diskhandle.dll")]
		private static extern ulong DiskHandleGetLength(IntPtr diskHandle);

		// Token: 0x06000017 RID: 23
		[DllImport("diskhandle.dll")]
		private static extern uint DiskHandleGetBytesPerSector(IntPtr diskHandle);

		// Token: 0x06000018 RID: 24
		[DllImport("diskhandle.dll", SetLastError = true)]
		private static extern bool DiskHandleFlush(IntPtr diskHandle);

		// Token: 0x06000019 RID: 25
		[DllImport("diskhandle.dll", SetLastError = true)]
		private static extern bool DiskHandleRead(IntPtr diskHandle, byte[] buffer, uint length, uint offset, ulong position, uint count);

		// Token: 0x0600001A RID: 26
		[DllImport("diskhandle.dll", SetLastError = true)]
		private static extern bool DiskHandleWrite(IntPtr diskHandle, byte[] buffer, uint length, uint offset, ulong position, uint count);

		// Token: 0x04000001 RID: 1
		protected static readonly IntPtr InvalidHandleValue = new IntPtr(-1);

		// Token: 0x04000002 RID: 2
		private long position;
	}
}
