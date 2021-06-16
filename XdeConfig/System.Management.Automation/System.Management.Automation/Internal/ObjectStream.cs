using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000890 RID: 2192
	internal sealed class ObjectStream : ObjectStreamBase, IDisposable
	{
		// Token: 0x060053FE RID: 21502 RVA: 0x001BBC6F File Offset: 0x001B9E6F
		internal ObjectStream() : this(int.MaxValue)
		{
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x001BBC7C File Offset: 0x001B9E7C
		internal ObjectStream(int capacity)
		{
			if (capacity <= 0 || capacity > 2147483647)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("capacity", capacity);
			}
			this._capacity = capacity;
			this._readHandle = new AutoResetEvent(false);
			this._writeHandle = new AutoResetEvent(true);
			this._readClosedHandle = new ManualResetEvent(false);
			this._writeClosedHandle = new ManualResetEvent(false);
			this._objects = new List<object>();
			this._isOpen = true;
		}

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x06005400 RID: 21504 RVA: 0x001BBD0B File Offset: 0x001B9F0B
		internal override int MaxCapacity
		{
			get
			{
				return this._capacity;
			}
		}

		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x06005401 RID: 21505 RVA: 0x001BBD14 File Offset: 0x001B9F14
		internal override WaitHandle ReadHandle
		{
			get
			{
				WaitHandle result = null;
				lock (this._monitorObject)
				{
					if (this._readWaitHandle == null)
					{
						this._readWaitHandle = new ManualResetEvent(this._objects.Count > 0 || !this._isOpen);
					}
					result = this._readWaitHandle;
				}
				return result;
			}
		}

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x06005402 RID: 21506 RVA: 0x001BBD88 File Offset: 0x001B9F88
		internal override WaitHandle WriteHandle
		{
			get
			{
				WaitHandle result = null;
				lock (this._monitorObject)
				{
					if (this._writeWaitHandle == null)
					{
						this._writeWaitHandle = new ManualResetEvent(this._objects.Count < this._capacity || !this._isOpen);
					}
					result = this._writeWaitHandle;
				}
				return result;
			}
		}

		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x06005403 RID: 21507 RVA: 0x001BBE00 File Offset: 0x001BA000
		internal override PipelineReader<object> ObjectReader
		{
			get
			{
				PipelineReader<object> result = null;
				lock (this._monitorObject)
				{
					if (this._reader == null)
					{
						this._reader = new ObjectReader(this);
					}
					result = this._reader;
				}
				return result;
			}
		}

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x06005404 RID: 21508 RVA: 0x001BBE58 File Offset: 0x001BA058
		internal override PipelineReader<PSObject> PSObjectReader
		{
			get
			{
				PipelineReader<PSObject> result = null;
				lock (this._monitorObject)
				{
					if (this._mshreader == null)
					{
						this._mshreader = new PSObjectReader(this);
					}
					result = this._mshreader;
				}
				return result;
			}
		}

		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x06005405 RID: 21509 RVA: 0x001BBEB0 File Offset: 0x001BA0B0
		internal override PipelineWriter ObjectWriter
		{
			get
			{
				PipelineWriter result = null;
				lock (this._monitorObject)
				{
					if (this._writer == null)
					{
						this._writer = new ObjectWriter(this);
					}
					result = this._writer;
				}
				return result;
			}
		}

		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x06005406 RID: 21510 RVA: 0x001BBF08 File Offset: 0x001BA108
		internal override bool EndOfPipeline
		{
			get
			{
				bool result = true;
				lock (this._monitorObject)
				{
					result = (this._objects.Count == 0 && !this._isOpen);
				}
				return result;
			}
		}

		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x06005407 RID: 21511 RVA: 0x001BBF60 File Offset: 0x001BA160
		internal override bool IsOpen
		{
			get
			{
				bool result = true;
				lock (this._monitorObject)
				{
					result = this._isOpen;
				}
				return result;
			}
		}

		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x06005408 RID: 21512 RVA: 0x001BBFA4 File Offset: 0x001BA1A4
		internal override int Count
		{
			get
			{
				int result = 0;
				lock (this._monitorObject)
				{
					result = this._objects.Count;
				}
				return result;
			}
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x001BBFF0 File Offset: 0x001BA1F0
		private bool WaitRead()
		{
			if (!this.EndOfPipeline)
			{
				try
				{
					WaitHandle[] waitHandles = new WaitHandle[]
					{
						this._readHandle,
						this._readClosedHandle
					};
					WaitHandle.WaitAny(waitHandles);
				}
				catch (ObjectDisposedException)
				{
				}
			}
			return !this.EndOfPipeline;
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x001BC048 File Offset: 0x001BA248
		private bool WaitWrite()
		{
			if (this.IsOpen)
			{
				try
				{
					WaitHandle[] waitHandles = new WaitHandle[]
					{
						this._writeHandle,
						this._writeClosedHandle
					};
					WaitHandle.WaitAny(waitHandles);
				}
				catch (ObjectDisposedException)
				{
				}
			}
			return this.IsOpen;
		}

		// Token: 0x0600540B RID: 21515 RVA: 0x001BC09C File Offset: 0x001BA29C
		private void RaiseEvents()
		{
			bool flag = true;
			bool flag2 = true;
			bool flag3 = false;
			try
			{
				lock (this._monitorObject)
				{
					flag = (!this._isOpen || this._objects.Count > 0);
					flag2 = (!this._isOpen || this._objects.Count < this._capacity);
					flag3 = (!this._isOpen && this._objects.Count == 0);
					if (this._readWaitHandle != null)
					{
						try
						{
							if (flag)
							{
								this._readWaitHandle.Set();
							}
							else
							{
								this._readWaitHandle.Reset();
							}
						}
						catch (ObjectDisposedException)
						{
						}
					}
					if (this._writeWaitHandle != null)
					{
						try
						{
							if (flag2)
							{
								this._writeWaitHandle.Set();
							}
							else
							{
								this._writeWaitHandle.Reset();
							}
						}
						catch (ObjectDisposedException)
						{
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					try
					{
						this._readHandle.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				}
				if (flag2)
				{
					try
					{
						this._writeHandle.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				}
				if (flag3)
				{
					try
					{
						this._readClosedHandle.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}
			if (flag)
			{
				base.FireDataReadyEvent(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600540C RID: 21516 RVA: 0x001BC21C File Offset: 0x001BA41C
		internal override void Flush()
		{
			bool flag = false;
			try
			{
				lock (this._monitorObject)
				{
					if (this._objects.Count > 0)
					{
						flag = true;
						this._objects.Clear();
					}
				}
			}
			finally
			{
				if (flag)
				{
					this.RaiseEvents();
				}
			}
		}

		// Token: 0x0600540D RID: 21517 RVA: 0x001BC28C File Offset: 0x001BA48C
		internal override void Close()
		{
			bool flag = false;
			try
			{
				lock (this._monitorObject)
				{
					if (this._isOpen)
					{
						flag = true;
						this._isOpen = false;
					}
				}
			}
			finally
			{
				if (flag)
				{
					try
					{
						this._writeClosedHandle.Set();
					}
					catch (ObjectDisposedException)
					{
					}
					this.RaiseEvents();
				}
			}
		}

		// Token: 0x0600540E RID: 21518 RVA: 0x001BC310 File Offset: 0x001BA510
		internal override object Read()
		{
			Collection<object> collection = this.Read(1);
			if (collection.Count == 1)
			{
				return collection[0];
			}
			return AutomationNull.Value;
		}

		// Token: 0x0600540F RID: 21519 RVA: 0x001BC33C File Offset: 0x001BA53C
		internal override Collection<object> Read(int count)
		{
			if (count < 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("count", count);
			}
			if (count == 0)
			{
				return new Collection<object>();
			}
			Collection<object> collection = new Collection<object>();
			bool flag = false;
			while (count > 0 && this.WaitRead())
			{
				try
				{
					lock (this._monitorObject)
					{
						if (this._objects.Count != 0)
						{
							flag = true;
							int num = 0;
							foreach (object item in this._objects)
							{
								collection.Add(item);
								num++;
								if (--count <= 0)
								{
									break;
								}
							}
							this._objects.RemoveRange(0, num);
						}
					}
				}
				finally
				{
					if (flag)
					{
						this.RaiseEvents();
					}
				}
			}
			return collection;
		}

		// Token: 0x06005410 RID: 21520 RVA: 0x001BC440 File Offset: 0x001BA640
		internal override Collection<object> ReadToEnd()
		{
			return this.Read(int.MaxValue);
		}

		// Token: 0x06005411 RID: 21521 RVA: 0x001BC450 File Offset: 0x001BA650
		internal override Collection<object> NonBlockingRead(int maxRequested)
		{
			Collection<object> collection = null;
			bool flag = false;
			if (maxRequested == 0)
			{
				return new Collection<object>();
			}
			if (maxRequested < 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("maxRequested", maxRequested);
			}
			try
			{
				lock (this._monitorObject)
				{
					int num = this._objects.Count;
					if (num > maxRequested)
					{
						num = maxRequested;
					}
					if (num > 0)
					{
						collection = new Collection<object>();
						for (int i = 0; i < num; i++)
						{
							collection.Add(this._objects[i]);
						}
						flag = true;
						this._objects.RemoveRange(0, num);
					}
				}
			}
			finally
			{
				if (flag)
				{
					this.RaiseEvents();
				}
			}
			if (collection == null)
			{
				collection = new Collection<object>();
			}
			return collection;
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x001BC51C File Offset: 0x001BA71C
		internal override object Peek()
		{
			object result = null;
			lock (this._monitorObject)
			{
				if (this.EndOfPipeline || this._objects.Count == 0)
				{
					result = AutomationNull.Value;
				}
				else
				{
					result = this._objects[0];
				}
			}
			return result;
		}

		// Token: 0x06005413 RID: 21523 RVA: 0x001BC584 File Offset: 0x001BA784
		internal override int Write(object obj, bool enumerateCollection)
		{
			if (obj == AutomationNull.Value)
			{
				return 0;
			}
			if (!this.IsOpen)
			{
				string writeToClosedPipeline = PipelineStrings.WriteToClosedPipeline;
				Exception ex = new PipelineClosedException(writeToClosedPipeline);
				throw ex;
			}
			List<object> list = new List<object>();
			IEnumerable enumerable = null;
			if (enumerateCollection)
			{
				enumerable = LanguagePrimitives.GetEnumerable(obj);
			}
			if (enumerable == null)
			{
				list.Add(obj);
			}
			else
			{
				foreach (object obj2 in enumerable)
				{
					if (AutomationNull.Value != obj2)
					{
						list.Add(obj2);
					}
				}
			}
			int num = 0;
			int i = list.Count;
			while (i > 0)
			{
				bool flag = false;
				if (!this.WaitWrite())
				{
					break;
				}
				try
				{
					lock (this._monitorObject)
					{
						if (!this.IsOpen)
						{
							break;
						}
						int num2 = this._capacity - this._objects.Count;
						if (0 < num2)
						{
							int num3 = i;
							if (num3 > num2)
							{
								num3 = num2;
							}
							try
							{
								if (num3 == list.Count)
								{
									this._objects.AddRange(list);
									num += num3;
									i -= num3;
								}
								else
								{
									List<object> range = list.GetRange(num, num3);
									this._objects.AddRange(range);
									num += num3;
									i -= num3;
								}
							}
							finally
							{
								flag = true;
							}
						}
					}
				}
				finally
				{
					if (flag)
					{
						this.RaiseEvents();
					}
				}
			}
			return num;
		}

		// Token: 0x06005414 RID: 21524 RVA: 0x001BC728 File Offset: 0x001BA928
		private void DFT_AddHandler_OnDataReady(EventHandler eventHandler)
		{
			base.DataReady += eventHandler;
		}

		// Token: 0x06005415 RID: 21525 RVA: 0x001BC731 File Offset: 0x001BA931
		private void DFT_RemoveHandler_OnDataReady(EventHandler eventHandler)
		{
			base.DataReady -= eventHandler;
		}

		// Token: 0x06005416 RID: 21526 RVA: 0x001BC73C File Offset: 0x001BA93C
		protected override void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			lock (this._monitorObject)
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
			}
			if (disposing)
			{
				this._readHandle.Dispose();
				this._writeHandle.Dispose();
				this._writeClosedHandle.Dispose();
				this._readClosedHandle.Dispose();
				if (this._readWaitHandle != null)
				{
					this._readWaitHandle.Dispose();
				}
				if (this._writeWaitHandle != null)
				{
					this._writeWaitHandle.Dispose();
				}
				if (this._reader != null)
				{
					this._reader.Close();
					this._reader.WaitHandle.Dispose();
				}
				if (this._writer != null)
				{
					this._writer.Close();
					this._writer.WaitHandle.Dispose();
				}
			}
		}

		// Token: 0x04002B0D RID: 11021
		private List<object> _objects;

		// Token: 0x04002B0E RID: 11022
		private bool _isOpen;

		// Token: 0x04002B0F RID: 11023
		private AutoResetEvent _readHandle;

		// Token: 0x04002B10 RID: 11024
		private ManualResetEvent _readWaitHandle;

		// Token: 0x04002B11 RID: 11025
		private ManualResetEvent _readClosedHandle;

		// Token: 0x04002B12 RID: 11026
		private AutoResetEvent _writeHandle;

		// Token: 0x04002B13 RID: 11027
		private ManualResetEvent _writeWaitHandle;

		// Token: 0x04002B14 RID: 11028
		private ManualResetEvent _writeClosedHandle;

		// Token: 0x04002B15 RID: 11029
		private PipelineReader<object> _reader;

		// Token: 0x04002B16 RID: 11030
		private PipelineReader<PSObject> _mshreader;

		// Token: 0x04002B17 RID: 11031
		private PipelineWriter _writer;

		// Token: 0x04002B18 RID: 11032
		private int _capacity = int.MaxValue;

		// Token: 0x04002B19 RID: 11033
		private object _monitorObject = new object();

		// Token: 0x04002B1A RID: 11034
		private bool _disposed;
	}
}
