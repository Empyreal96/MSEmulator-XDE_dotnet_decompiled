using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000891 RID: 2193
	internal sealed class PSDataCollectionStream<T> : ObjectStreamBase
	{
		// Token: 0x06005417 RID: 21527 RVA: 0x001BC834 File Offset: 0x001BAA34
		internal PSDataCollectionStream(Guid psInstanceId, PSDataCollection<T> storeToUse)
		{
			if (storeToUse == null)
			{
				throw PSTraceSource.NewArgumentNullException("storeToUse");
			}
			this._objects = storeToUse;
			this.psInstanceId = psInstanceId;
			this.isOpen = true;
			storeToUse.AddRef();
			storeToUse.DataAdded += this.HandleDataAdded;
			storeToUse.Completed += this.HandleClosed;
		}

		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x06005418 RID: 21528 RVA: 0x001BC89F File Offset: 0x001BAA9F
		internal PSDataCollection<T> ObjectStore
		{
			get
			{
				return this._objects;
			}
		}

		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x06005419 RID: 21529 RVA: 0x001BC8A7 File Offset: 0x001BAAA7
		internal override int Count
		{
			get
			{
				return this._objects.Count;
			}
		}

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x0600541A RID: 21530 RVA: 0x001BC8B4 File Offset: 0x001BAAB4
		internal override bool EndOfPipeline
		{
			get
			{
				bool result = true;
				lock (this._syncObject)
				{
					result = (this._objects.Count == 0 && !this.isOpen);
				}
				return result;
			}
		}

		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x0600541B RID: 21531 RVA: 0x001BC90C File Offset: 0x001BAB0C
		internal override bool IsOpen
		{
			get
			{
				return this.isOpen && this._objects.IsOpen;
			}
		}

		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x0600541C RID: 21532 RVA: 0x001BC923 File Offset: 0x001BAB23
		internal override int MaxCapacity
		{
			get
			{
				throw PSTraceSource.NewNotSupportedException();
			}
		}

		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x0600541D RID: 21533 RVA: 0x001BC92C File Offset: 0x001BAB2C
		internal override PipelineReader<object> ObjectReader
		{
			get
			{
				if (this._objectReader == null)
				{
					lock (this._syncObject)
					{
						if (this._objectReader == null)
						{
							this._objectReader = new PSDataCollectionReader<T, object>(this);
						}
					}
				}
				return this._objectReader;
			}
		}

		// Token: 0x0600541E RID: 21534 RVA: 0x001BC988 File Offset: 0x001BAB88
		internal PipelineReader<object> GetObjectReaderForPipeline(string computerName, Guid runspaceId)
		{
			if (this._objectReaderForPipeline == null)
			{
				lock (this._syncObject)
				{
					if (this._objectReaderForPipeline == null)
					{
						this._objectReaderForPipeline = new PSDataCollectionPipelineReader<T, object>(this, computerName, runspaceId);
					}
				}
			}
			return this._objectReaderForPipeline;
		}

		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x0600541F RID: 21535 RVA: 0x001BC9E8 File Offset: 0x001BABE8
		internal override PipelineReader<PSObject> PSObjectReader
		{
			get
			{
				if (this._psobjectReader == null)
				{
					lock (this._syncObject)
					{
						if (this._psobjectReader == null)
						{
							this._psobjectReader = new PSDataCollectionReader<T, PSObject>(this);
						}
					}
				}
				return this._psobjectReader;
			}
		}

		// Token: 0x06005420 RID: 21536 RVA: 0x001BCA44 File Offset: 0x001BAC44
		internal PipelineReader<PSObject> GetPSObjectReaderForPipeline(string computerName, Guid runspaceId)
		{
			if (this._psobjectReaderForPipeline == null)
			{
				lock (this._syncObject)
				{
					if (this._psobjectReaderForPipeline == null)
					{
						this._psobjectReaderForPipeline = new PSDataCollectionPipelineReader<T, PSObject>(this, computerName, runspaceId);
					}
				}
			}
			return this._psobjectReaderForPipeline;
		}

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x06005421 RID: 21537 RVA: 0x001BCAA4 File Offset: 0x001BACA4
		internal override PipelineWriter ObjectWriter
		{
			get
			{
				if (this._writer == null)
				{
					lock (this._syncObject)
					{
						if (this._writer == null)
						{
							this._writer = new PSDataCollectionWriter<T>(this);
						}
					}
				}
				return this._writer;
			}
		}

		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x06005422 RID: 21538 RVA: 0x001BCB00 File Offset: 0x001BAD00
		internal override WaitHandle ReadHandle
		{
			get
			{
				return this._objects.WaitHandle;
			}
		}

		// Token: 0x06005423 RID: 21539 RVA: 0x001BCB10 File Offset: 0x001BAD10
		internal override int Write(object obj, bool enumerateCollection)
		{
			if (obj == AutomationNull.Value)
			{
				return 0;
			}
			if (!this.IsOpen)
			{
				string writeToClosedBuffer = PSDataBufferStrings.WriteToClosedBuffer;
				Exception ex = new PipelineClosedException(writeToClosedBuffer);
				throw ex;
			}
			Collection<T> collection = new Collection<T>();
			IEnumerable enumerable = null;
			if (enumerateCollection)
			{
				enumerable = LanguagePrimitives.GetEnumerable(obj);
			}
			if (enumerable == null)
			{
				collection.Add((T)((object)LanguagePrimitives.ConvertTo(obj, typeof(T), CultureInfo.InvariantCulture)));
			}
			else
			{
				foreach (object obj2 in enumerable)
				{
					if (AutomationNull.Value != obj2)
					{
						collection.Add((T)((object)LanguagePrimitives.ConvertTo(obj, typeof(T), CultureInfo.InvariantCulture)));
					}
				}
			}
			this._objects.InternalAddRange(this.psInstanceId, collection);
			return collection.Count;
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x001BCBFC File Offset: 0x001BADFC
		internal override void Close()
		{
			bool flag = false;
			lock (this._syncObject)
			{
				if (this.isOpen)
				{
					this._objects.DecrementRef();
					this._objects.DataAdded -= this.HandleDataAdded;
					this._objects.Completed -= this.HandleClosed;
					flag = true;
					this.isOpen = false;
				}
			}
			if (flag)
			{
				base.FireDataReadyEvent(this, EventArgs.Empty);
			}
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x001BCC94 File Offset: 0x001BAE94
		private void HandleClosed(object sender, EventArgs e)
		{
			this.Close();
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x001BCC9C File Offset: 0x001BAE9C
		private void HandleDataAdded(object sender, DataAddedEventArgs e)
		{
			base.FireDataReadyEvent(this, EventArgs.Empty);
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x001BCCAC File Offset: 0x001BAEAC
		protected override void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			lock (this._syncObject)
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
			}
			if (disposing)
			{
				this._objects.Dispose();
				this.Close();
				if (this._objectReaderForPipeline != null)
				{
					((PSDataCollectionPipelineReader<T, object>)this._objectReaderForPipeline).Dispose();
				}
				if (this._psobjectReaderForPipeline != null)
				{
					((PSDataCollectionPipelineReader<T, PSObject>)this._psobjectReaderForPipeline).Dispose();
				}
			}
		}

		// Token: 0x04002B1B RID: 11035
		private PSDataCollection<T> _objects;

		// Token: 0x04002B1C RID: 11036
		private Guid psInstanceId;

		// Token: 0x04002B1D RID: 11037
		private bool isOpen;

		// Token: 0x04002B1E RID: 11038
		private PipelineWriter _writer;

		// Token: 0x04002B1F RID: 11039
		private PipelineReader<object> _objectReader;

		// Token: 0x04002B20 RID: 11040
		private PipelineReader<PSObject> _psobjectReader;

		// Token: 0x04002B21 RID: 11041
		private PipelineReader<object> _objectReaderForPipeline;

		// Token: 0x04002B22 RID: 11042
		private PipelineReader<PSObject> _psobjectReaderForPipeline;

		// Token: 0x04002B23 RID: 11043
		private object _syncObject = new object();

		// Token: 0x04002B24 RID: 11044
		private bool _disposed;
	}
}
