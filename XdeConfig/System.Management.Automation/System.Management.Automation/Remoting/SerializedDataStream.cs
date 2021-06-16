using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Tracing;
using System.Threading;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002B2 RID: 690
	internal class SerializedDataStream : Stream, IDisposable
	{
		// Token: 0x06002160 RID: 8544 RVA: 0x000C029C File Offset: 0x000BE49C
		internal SerializedDataStream(int fragmentSize)
		{
			SerializedDataStream._trace.WriteLine("Creating SerializedDataStream with fragmentsize : {0}", new object[]
			{
				fragmentSize
			});
			this.syncObject = new object();
			this.currentFragment = new FragmentedRemoteObject();
			this.queuedStreams = new Queue<MemoryStream>();
			this.fragmentSize = fragmentSize;
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000C02F7 File Offset: 0x000BE4F7
		internal SerializedDataStream(int fragmentSize, SerializedDataStream.OnDataAvailableCallback callbackToNotify) : this(fragmentSize)
		{
			if (callbackToNotify != null)
			{
				this.notifyOnWriteFragmentImmediately = true;
				this.onDataAvailableCallback = callbackToNotify;
			}
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000C0314 File Offset: 0x000BE514
		internal void Enter()
		{
			this.isEntered = true;
			this.fragmentId = 0L;
			this.currentFragment.ObjectId = SerializedDataStream.GetObjectId();
			this.currentFragment.FragmentId = this.fragmentId;
			this.currentFragment.IsStartFragment = true;
			this.currentFragment.BlobLength = 0;
			this.currentFragment.Blob = new byte[this.fragmentSize];
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000C037F File Offset: 0x000BE57F
		internal void Exit()
		{
			this.isEntered = false;
			if (this.currentFragment.BlobLength > 0)
			{
				this.currentFragment.IsEndFragment = true;
				this.WriteCurrentFragmentAndReset();
			}
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x000C03A8 File Offset: 0x000BE5A8
		public override void Write(byte[] buffer, int offset, int count)
		{
			int num = offset;
			int i = count;
			while (i > 0)
			{
				int num2 = this.fragmentSize - 21 - this.currentFragment.BlobLength;
				if (num2 > 0)
				{
					int num3 = (i > num2) ? num2 : i;
					i -= num3;
					Array.Copy(buffer, num, this.currentFragment.Blob, this.currentFragment.BlobLength, num3);
					this.currentFragment.BlobLength += num3;
					num += num3;
					if (i > 0)
					{
						this.WriteCurrentFragmentAndReset();
					}
				}
				else
				{
					this.WriteCurrentFragmentAndReset();
				}
			}
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x000C0430 File Offset: 0x000BE630
		public override void WriteByte(byte value)
		{
			this.Write(new byte[]
			{
				value
			}, 0, 1);
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x000C0454 File Offset: 0x000BE654
		internal byte[] ReadOrRegisterCallback(SerializedDataStream.OnDataAvailableCallback callback)
		{
			byte[] result;
			lock (this.syncObject)
			{
				if (this.length <= 0L)
				{
					this.onDataAvailableCallback = callback;
					result = null;
				}
				else
				{
					int num = (this.length > (long)this.fragmentSize) ? this.fragmentSize : ((int)this.length);
					byte[] array = new byte[num];
					this.Read(array, 0, num);
					result = array;
				}
			}
			return result;
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x000C04D8 File Offset: 0x000BE6D8
		internal byte[] Read()
		{
			byte[] result;
			lock (this.syncObject)
			{
				if (this.isDisposed)
				{
					result = null;
				}
				else
				{
					int num = (this.length > (long)this.fragmentSize) ? this.fragmentSize : ((int)this.length);
					if (num > 0)
					{
						byte[] array = new byte[num];
						this.Read(array, 0, num);
						result = array;
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000C055C File Offset: 0x000BE75C
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = offset;
			int i = 0;
			Collection<MemoryStream> collection = new Collection<MemoryStream>();
			MemoryStream memoryStream = null;
			lock (this.syncObject)
			{
				if (this.isDisposed)
				{
					return 0;
				}
				while (i < count)
				{
					if (this.readStream == null)
					{
						if (this.queuedStreams.Count > 0)
						{
							this.readStream = this.queuedStreams.Dequeue();
							if (!this.readStream.CanRead || memoryStream == this.readStream)
							{
								this.readStream = null;
								continue;
							}
						}
						else
						{
							this.readStream = this.writeStream;
						}
						this.readOffSet = 0;
					}
					this.readStream.Position = (long)this.readOffSet;
					int num2 = this.readStream.Read(buffer, num, count - i);
					SerializedDataStream._trace.WriteLine("Read {0} data from readstream: {1}", new object[]
					{
						num2,
						this.readStream.GetHashCode()
					});
					i += num2;
					num += num2;
					this.readOffSet += num2;
					this.length -= (long)num2;
					if (this.readStream.Capacity == this.readOffSet && this.readStream != this.writeStream)
					{
						SerializedDataStream._trace.WriteLine("Adding readstream {0} to dispose collection.", new object[]
						{
							this.readStream.GetHashCode()
						});
						collection.Add(this.readStream);
						memoryStream = this.readStream;
						this.readStream = null;
					}
				}
			}
			foreach (MemoryStream memoryStream2 in collection)
			{
				SerializedDataStream._trace.WriteLine("Disposing stream: {0}", new object[]
				{
					memoryStream2.GetHashCode()
				});
				memoryStream2.Dispose();
			}
			return i;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000C078C File Offset: 0x000BE98C
		private void WriteCurrentFragmentAndReset()
		{
			PSEtwLog.LogAnalyticVerbose(PSEventId.SentRemotingFragment, PSOpcode.Send, PSTask.None, (PSKeyword)4611686018427387912UL, this.currentFragment.ObjectId, this.currentFragment.FragmentId, this.currentFragment.IsStartFragment ? 1 : 0, this.currentFragment.IsEndFragment ? 1 : 0, (uint)this.currentFragment.BlobLength, new PSETWBinaryBlob(this.currentFragment.Blob, 0, this.currentFragment.BlobLength));
			byte[] bytes = this.currentFragment.GetBytes();
			int i = bytes.Length;
			int num = 0;
			if (!this.notifyOnWriteFragmentImmediately)
			{
				lock (this.syncObject)
				{
					if (this.isDisposed)
					{
						return;
					}
					if (this.writeStream == null)
					{
						this.writeStream = new MemoryStream(this.fragmentSize);
						SerializedDataStream._trace.WriteLine("Created write stream: {0}", new object[]
						{
							this.writeStream.GetHashCode()
						});
						this.writeOffset = 0;
					}
					while (i > 0)
					{
						int num2 = this.writeStream.Capacity - this.writeOffset;
						if (num2 == 0)
						{
							this.EnqueueWriteStream();
							num2 = this.writeStream.Capacity - this.writeOffset;
						}
						int num3 = (i > num2) ? num2 : i;
						i -= num3;
						this.writeStream.Position = (long)this.writeOffset;
						this.writeStream.Write(bytes, num, num3);
						num += num3;
						this.writeOffset += num3;
						this.length += (long)num3;
					}
				}
			}
			if (this.onDataAvailableCallback != null)
			{
				this.onDataAvailableCallback(bytes, this.currentFragment.IsEndFragment);
			}
			this.currentFragment.FragmentId = (this.fragmentId += 1L);
			this.currentFragment.IsStartFragment = false;
			this.currentFragment.IsEndFragment = false;
			this.currentFragment.BlobLength = 0;
			this.currentFragment.Blob = new byte[this.fragmentSize];
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x000C09C0 File Offset: 0x000BEBC0
		private void EnqueueWriteStream()
		{
			SerializedDataStream._trace.WriteLine("Queuing write stream: {0} Length: {1} Capacity: {2}", new object[]
			{
				this.writeStream.GetHashCode(),
				this.writeStream.Length,
				this.writeStream.Capacity
			});
			this.queuedStreams.Enqueue(this.writeStream);
			this.writeStream = new MemoryStream(this.fragmentSize);
			this.writeOffset = 0;
			SerializedDataStream._trace.WriteLine("Created write stream: {0}", new object[]
			{
				this.writeStream.GetHashCode()
			});
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x000C0A70 File Offset: 0x000BEC70
		private static long GetObjectId()
		{
			return Interlocked.Increment(ref SerializedDataStream._objectIdSequenceNumber);
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x000C0A7C File Offset: 0x000BEC7C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this.syncObject)
				{
					foreach (MemoryStream memoryStream in this.queuedStreams)
					{
						if (memoryStream.CanRead)
						{
							memoryStream.Dispose();
						}
					}
					if (this.readStream != null && this.readStream.CanRead)
					{
						this.readStream.Dispose();
					}
					if (this.writeStream != null && this.writeStream.CanRead)
					{
						this.writeStream.Dispose();
					}
					this.isDisposed = true;
				}
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x0600216D RID: 8557 RVA: 0x000C0B4C File Offset: 0x000BED4C
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x000C0B4F File Offset: 0x000BED4F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x0600216F RID: 8559 RVA: 0x000C0B52 File Offset: 0x000BED52
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x000C0B55 File Offset: 0x000BED55
		public override long Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002171 RID: 8561 RVA: 0x000C0B5D File Offset: 0x000BED5D
		// (set) Token: 0x06002172 RID: 8562 RVA: 0x000C0B64 File Offset: 0x000BED64
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000C0B6B File Offset: 0x000BED6B
		public override void Flush()
		{
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000C0B6D File Offset: 0x000BED6D
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x000C0B74 File Offset: 0x000BED74
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000C0B7B File Offset: 0x000BED7B
		public new void Dispose()
		{
			if (!this.disposed)
			{
				GC.SuppressFinalize(this);
				this.disposed = true;
			}
			base.Dispose();
		}

		// Token: 0x04000ECA RID: 3786
		[TraceSource("SerializedDataStream", "SerializedDataStream")]
		private static PSTraceSource _trace = PSTraceSource.GetTracer("SerializedDataStream", "SerializedDataStream");

		// Token: 0x04000ECB RID: 3787
		private static long _objectIdSequenceNumber = 0L;

		// Token: 0x04000ECC RID: 3788
		private bool isEntered;

		// Token: 0x04000ECD RID: 3789
		private FragmentedRemoteObject currentFragment;

		// Token: 0x04000ECE RID: 3790
		private long fragmentId;

		// Token: 0x04000ECF RID: 3791
		private int fragmentSize;

		// Token: 0x04000ED0 RID: 3792
		private object syncObject;

		// Token: 0x04000ED1 RID: 3793
		private bool isDisposed;

		// Token: 0x04000ED2 RID: 3794
		private bool notifyOnWriteFragmentImmediately;

		// Token: 0x04000ED3 RID: 3795
		private Queue<MemoryStream> queuedStreams;

		// Token: 0x04000ED4 RID: 3796
		private MemoryStream writeStream;

		// Token: 0x04000ED5 RID: 3797
		private MemoryStream readStream;

		// Token: 0x04000ED6 RID: 3798
		private int writeOffset;

		// Token: 0x04000ED7 RID: 3799
		private int readOffSet;

		// Token: 0x04000ED8 RID: 3800
		private long length;

		// Token: 0x04000ED9 RID: 3801
		private SerializedDataStream.OnDataAvailableCallback onDataAvailableCallback;

		// Token: 0x04000EDA RID: 3802
		private bool disposed;

		// Token: 0x020002B3 RID: 691
		// (Invoke) Token: 0x06002179 RID: 8569
		internal delegate void OnDataAvailableCallback(byte[] data, bool isEndFragment);
	}
}
