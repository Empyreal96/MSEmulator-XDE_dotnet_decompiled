using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200088E RID: 2190
	internal class PSDataCollectionPipelineReader<DataStoreType, ReturnType> : ObjectReaderBase<ReturnType>
	{
		// Token: 0x060053DB RID: 21467 RVA: 0x001BBA14 File Offset: 0x001B9C14
		internal PSDataCollectionPipelineReader(PSDataCollectionStream<DataStoreType> stream, string computerName, Guid runspaceId) : base(stream)
		{
			this.datastore = stream.ObjectStore;
			this.computerName = computerName;
			this.runspaceId = runspaceId;
		}

		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x060053DC RID: 21468 RVA: 0x001BBA37 File Offset: 0x001B9C37
		internal string ComputerName
		{
			get
			{
				return this.computerName;
			}
		}

		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x060053DD RID: 21469 RVA: 0x001BBA3F File Offset: 0x001B9C3F
		internal Guid RunspaceId
		{
			get
			{
				return this.runspaceId;
			}
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x001BBA47 File Offset: 0x001B9C47
		public override Collection<ReturnType> Read(int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x001BBA50 File Offset: 0x001B9C50
		public override ReturnType Read()
		{
			object inputObject = AutomationNull.Value;
			if (this.datastore.Count > 0)
			{
				Collection<DataStoreType> collection = this.datastore.ReadAndRemove(1);
				if (collection.Count == 1)
				{
					inputObject = collection[0];
				}
			}
			return this.ConvertToReturnType(inputObject);
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x001BBA9B File Offset: 0x001B9C9B
		public override Collection<ReturnType> ReadToEnd()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x001BBAA2 File Offset: 0x001B9CA2
		public override Collection<ReturnType> NonBlockingRead()
		{
			return this.NonBlockingRead(int.MaxValue);
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x001BBAB0 File Offset: 0x001B9CB0
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
			int num = maxRequested;
			while (num > 0 && this.datastore.Count > 0)
			{
				collection.Add(this.ConvertToReturnType(this.datastore.ReadAndRemove(1)[0]));
				num--;
			}
			return collection;
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x001BBB1F File Offset: 0x001B9D1F
		public override ReturnType Peek()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x001BBB28 File Offset: 0x001B9D28
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

		// Token: 0x060053E5 RID: 21477 RVA: 0x001BBB7C File Offset: 0x001B9D7C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.datastore.Dispose();
			}
		}

		// Token: 0x04002B09 RID: 11017
		private PSDataCollection<DataStoreType> datastore;

		// Token: 0x04002B0A RID: 11018
		private string computerName;

		// Token: 0x04002B0B RID: 11019
		private Guid runspaceId;
	}
}
