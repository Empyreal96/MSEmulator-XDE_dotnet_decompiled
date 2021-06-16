using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200088D RID: 2189
	internal class PSDataCollectionReader<DataStoreType, ReturnType> : ObjectReaderBase<ReturnType>
	{
		// Token: 0x060053D2 RID: 21458 RVA: 0x001BB8D8 File Offset: 0x001B9AD8
		public PSDataCollectionReader(PSDataCollectionStream<DataStoreType> stream) : base(stream)
		{
			this.enumerator = (PSDataCollectionEnumerator<DataStoreType>)stream.ObjectStore.GetEnumerator();
		}

		// Token: 0x060053D3 RID: 21459 RVA: 0x001BB8F7 File Offset: 0x001B9AF7
		public override Collection<ReturnType> Read(int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060053D4 RID: 21460 RVA: 0x001BB900 File Offset: 0x001B9B00
		public override ReturnType Read()
		{
			object inputObject = AutomationNull.Value;
			if (this.enumerator.MoveNext())
			{
				inputObject = this.enumerator.Current;
			}
			return this.ConvertToReturnType(inputObject);
		}

		// Token: 0x060053D5 RID: 21461 RVA: 0x001BB933 File Offset: 0x001B9B33
		public override Collection<ReturnType> ReadToEnd()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060053D6 RID: 21462 RVA: 0x001BB93A File Offset: 0x001B9B3A
		public override Collection<ReturnType> NonBlockingRead()
		{
			return this.NonBlockingRead(int.MaxValue);
		}

		// Token: 0x060053D7 RID: 21463 RVA: 0x001BB948 File Offset: 0x001B9B48
		public override Collection<ReturnType> NonBlockingRead(int maxRequested)
		{
			if (maxRequested < 0)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("maxRequested", maxRequested);
			}
			if (maxRequested == 0)
			{
				return new Collection<ReturnType>();
			}
			Collection<ReturnType> collection = new Collection<ReturnType>();
			while (maxRequested > 0 && this.enumerator.MoveNext(false))
			{
				collection.Add(this.ConvertToReturnType(this.enumerator.Current));
			}
			return collection;
		}

		// Token: 0x060053D8 RID: 21464 RVA: 0x001BB9A7 File Offset: 0x001B9BA7
		public override ReturnType Peek()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x001BB9AE File Offset: 0x001B9BAE
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._stream.Close();
			}
		}

		// Token: 0x060053DA RID: 21466 RVA: 0x001BB9C0 File Offset: 0x001B9BC0
		private ReturnType ConvertToReturnType(object inputObject)
		{
			Type typeFromHandle = typeof(ReturnType);
			if (typeof(PSObject).Equals(typeFromHandle) || typeof(object).Equals(typeFromHandle))
			{
				ReturnType result = default(ReturnType);
				LanguagePrimitives.TryConvertTo<ReturnType>(inputObject, out result);
				return result;
			}
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x04002B08 RID: 11016
		private PSDataCollectionEnumerator<DataStoreType> enumerator;
	}
}
