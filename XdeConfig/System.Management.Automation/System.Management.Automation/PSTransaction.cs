using System;
using System.Transactions;

namespace System.Management.Automation
{
	// Token: 0x02000909 RID: 2313
	public sealed class PSTransaction : IDisposable
	{
		// Token: 0x060056E5 RID: 22245 RVA: 0x001C6EFC File Offset: 0x001C50FC
		internal PSTransaction(RollbackSeverity rollbackPreference, TimeSpan timeout)
		{
			this.transaction = new CommittableTransaction(timeout);
			this.rollbackPreference = rollbackPreference;
			this.subscriberCount = 1;
		}

		// Token: 0x060056E6 RID: 22246 RVA: 0x001C6F1E File Offset: 0x001C511E
		internal PSTransaction(CommittableTransaction transaction, RollbackSeverity severity)
		{
			this.transaction = transaction;
			this.rollbackPreference = severity;
			this.subscriberCount = 1;
		}

		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x060056E7 RID: 22247 RVA: 0x001C6F3B File Offset: 0x001C513B
		public RollbackSeverity RollbackPreference
		{
			get
			{
				return this.rollbackPreference;
			}
		}

		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x060056E8 RID: 22248 RVA: 0x001C6F43 File Offset: 0x001C5143
		// (set) Token: 0x060056E9 RID: 22249 RVA: 0x001C6F5A File Offset: 0x001C515A
		public int SubscriberCount
		{
			get
			{
				if (this.IsRolledBack)
				{
					this.SubscriberCount = 0;
				}
				return this.subscriberCount;
			}
			set
			{
				this.subscriberCount = value;
			}
		}

		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x060056EA RID: 22250 RVA: 0x001C6F63 File Offset: 0x001C5163
		public PSTransactionStatus Status
		{
			get
			{
				if (this.IsRolledBack)
				{
					return PSTransactionStatus.RolledBack;
				}
				if (this.IsCommitted)
				{
					return PSTransactionStatus.Committed;
				}
				return PSTransactionStatus.Active;
			}
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x001C6F7A File Offset: 0x001C517A
		internal void Activate()
		{
			Transaction.Current = this.transaction;
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x001C6F87 File Offset: 0x001C5187
		internal void Commit()
		{
			this.transaction.Commit();
			this.isCommitted = true;
		}

		// Token: 0x060056ED RID: 22253 RVA: 0x001C6F9B File Offset: 0x001C519B
		internal void Rollback()
		{
			this.transaction.Rollback();
			this.isRolledBack = true;
		}

		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x060056EE RID: 22254 RVA: 0x001C6FAF File Offset: 0x001C51AF
		// (set) Token: 0x060056EF RID: 22255 RVA: 0x001C6FE7 File Offset: 0x001C51E7
		internal bool IsRolledBack
		{
			get
			{
				if (!this.isRolledBack && this.transaction != null && this.transaction.TransactionInformation.Status == TransactionStatus.Aborted)
				{
					this.isRolledBack = true;
				}
				return this.isRolledBack;
			}
			set
			{
				this.isRolledBack = value;
			}
		}

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x060056F0 RID: 22256 RVA: 0x001C6FF0 File Offset: 0x001C51F0
		// (set) Token: 0x060056F1 RID: 22257 RVA: 0x001C6FF8 File Offset: 0x001C51F8
		internal bool IsCommitted
		{
			get
			{
				return this.isCommitted;
			}
			set
			{
				this.isCommitted = value;
			}
		}

		// Token: 0x060056F2 RID: 22258 RVA: 0x001C7004 File Offset: 0x001C5204
		~PSTransaction()
		{
			this.Dispose(false);
		}

		// Token: 0x060056F3 RID: 22259 RVA: 0x001C7034 File Offset: 0x001C5234
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x001C7043 File Offset: 0x001C5243
		public void Dispose(bool disposing)
		{
			if (disposing && this.transaction != null)
			{
				this.transaction.Dispose();
			}
		}

		// Token: 0x04002E5D RID: 11869
		private CommittableTransaction transaction;

		// Token: 0x04002E5E RID: 11870
		private RollbackSeverity rollbackPreference;

		// Token: 0x04002E5F RID: 11871
		private int subscriberCount;

		// Token: 0x04002E60 RID: 11872
		private bool isRolledBack;

		// Token: 0x04002E61 RID: 11873
		private bool isCommitted;
	}
}
