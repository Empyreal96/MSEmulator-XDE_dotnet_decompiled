using System;
using System.Collections.Generic;
using System.Transactions;

namespace System.Management.Automation.Internal
{
	// Token: 0x0200090C RID: 2316
	internal sealed class PSTransactionManager : IDisposable
	{
		// Token: 0x060056F9 RID: 22265 RVA: 0x001C70C7 File Offset: 0x001C52C7
		internal PSTransactionManager()
		{
			this.transactionStack = new Stack<PSTransaction>();
			this.transactionStack.Push(null);
		}

		// Token: 0x060056FA RID: 22266 RVA: 0x001C70E6 File Offset: 0x001C52E6
		internal static IDisposable GetEngineProtectionScope()
		{
			if (PSTransactionManager.engineProtectionEnabled && Transaction.Current != null)
			{
				return new TransactionScope(TransactionScopeOption.Suppress);
			}
			return null;
		}

		// Token: 0x060056FB RID: 22267 RVA: 0x001C7104 File Offset: 0x001C5304
		internal static void EnableEngineProtection()
		{
			PSTransactionManager.engineProtectionEnabled = true;
		}

		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x060056FC RID: 22268 RVA: 0x001C710C File Offset: 0x001C530C
		internal RollbackSeverity RollbackPreference
		{
			get
			{
				PSTransaction pstransaction = this.transactionStack.Peek();
				if (pstransaction == null)
				{
					string noTransactionActive = TransactionStrings.NoTransactionActive;
					throw new InvalidOperationException(noTransactionActive);
				}
				return pstransaction.RollbackPreference;
			}
		}

		// Token: 0x060056FD RID: 22269 RVA: 0x001C713B File Offset: 0x001C533B
		internal void CreateOrJoin()
		{
			this.CreateOrJoin(RollbackSeverity.Error, TimeSpan.FromMinutes(1.0));
		}

		// Token: 0x060056FE RID: 22270 RVA: 0x001C7154 File Offset: 0x001C5354
		internal void CreateOrJoin(RollbackSeverity rollbackPreference, TimeSpan timeout)
		{
			PSTransaction pstransaction = this.transactionStack.Peek();
			if (pstransaction == null)
			{
				this.transactionStack.Push(new PSTransaction(rollbackPreference, timeout));
				return;
			}
			if (pstransaction.IsRolledBack || pstransaction.IsCommitted)
			{
				this.transactionStack.Pop().Dispose();
				this.transactionStack.Push(new PSTransaction(rollbackPreference, timeout));
				return;
			}
			pstransaction.SubscriberCount++;
		}

		// Token: 0x060056FF RID: 22271 RVA: 0x001C71C4 File Offset: 0x001C53C4
		internal void CreateNew()
		{
			this.CreateNew(RollbackSeverity.Error, TimeSpan.FromMinutes(1.0));
		}

		// Token: 0x06005700 RID: 22272 RVA: 0x001C71DB File Offset: 0x001C53DB
		internal void CreateNew(RollbackSeverity rollbackPreference, TimeSpan timeout)
		{
			this.transactionStack.Push(new PSTransaction(rollbackPreference, timeout));
		}

		// Token: 0x06005701 RID: 22273 RVA: 0x001C71F0 File Offset: 0x001C53F0
		internal void Commit()
		{
			PSTransaction pstransaction = this.transactionStack.Peek();
			if (pstransaction == null)
			{
				string noTransactionActiveForCommit = TransactionStrings.NoTransactionActiveForCommit;
				throw new InvalidOperationException(noTransactionActiveForCommit);
			}
			if (pstransaction.IsRolledBack)
			{
				string transactionRolledBackForCommit = TransactionStrings.TransactionRolledBackForCommit;
				throw new TransactionAbortedException(transactionRolledBackForCommit);
			}
			if (pstransaction.IsCommitted)
			{
				string committedTransactionForCommit = TransactionStrings.CommittedTransactionForCommit;
				throw new InvalidOperationException(committedTransactionForCommit);
			}
			if (pstransaction.SubscriberCount == 1)
			{
				pstransaction.Commit();
				pstransaction.SubscriberCount = 0;
			}
			else
			{
				pstransaction.SubscriberCount--;
			}
			while (this.transactionStack.Count > 2 && (this.transactionStack.Peek().IsRolledBack || this.transactionStack.Peek().IsCommitted))
			{
				this.transactionStack.Pop().Dispose();
			}
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x001C72AD File Offset: 0x001C54AD
		internal void Rollback()
		{
			this.Rollback(false);
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x001C72B8 File Offset: 0x001C54B8
		internal void Rollback(bool suppressErrors)
		{
			PSTransaction pstransaction = this.transactionStack.Peek();
			if (pstransaction == null)
			{
				string noTransactionActiveForRollback = TransactionStrings.NoTransactionActiveForRollback;
				throw new InvalidOperationException(noTransactionActiveForRollback);
			}
			if (pstransaction.IsRolledBack && !suppressErrors)
			{
				string transactionRolledBackForRollback = TransactionStrings.TransactionRolledBackForRollback;
				throw new TransactionAbortedException(transactionRolledBackForRollback);
			}
			if (pstransaction.IsCommitted && !suppressErrors)
			{
				string committedTransactionForRollback = TransactionStrings.CommittedTransactionForRollback;
				throw new InvalidOperationException(committedTransactionForRollback);
			}
			pstransaction.SubscriberCount = 0;
			pstransaction.Rollback();
			while (this.transactionStack.Count > 2 && (this.transactionStack.Peek().IsRolledBack || this.transactionStack.Peek().IsCommitted))
			{
				this.transactionStack.Pop().Dispose();
			}
		}

		// Token: 0x06005704 RID: 22276 RVA: 0x001C7364 File Offset: 0x001C5564
		internal void SetBaseTransaction(CommittableTransaction transaction, RollbackSeverity severity)
		{
			if (this.HasTransaction)
			{
				throw new InvalidOperationException(TransactionStrings.BaseTransactionMustBeFirst);
			}
			this.transactionStack.Peek();
			while (this.transactionStack.Peek() != null && (this.transactionStack.Peek().IsRolledBack || this.transactionStack.Peek().IsCommitted))
			{
				this.transactionStack.Pop().Dispose();
			}
			this.baseTransaction = new PSTransaction(transaction, severity);
			this.transactionStack.Push(this.baseTransaction);
		}

		// Token: 0x06005705 RID: 22277 RVA: 0x001C73F4 File Offset: 0x001C55F4
		internal void ClearBaseTransaction()
		{
			if (this.baseTransaction == null)
			{
				throw new InvalidOperationException(TransactionStrings.BaseTransactionNotSet);
			}
			if (this.transactionStack.Peek() != this.baseTransaction)
			{
				throw new InvalidOperationException(TransactionStrings.BaseTransactionNotActive);
			}
			this.transactionStack.Pop().Dispose();
			this.baseTransaction = null;
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x001C7449 File Offset: 0x001C5649
		internal PSTransaction GetCurrent()
		{
			return this.transactionStack.Peek();
		}

		// Token: 0x06005707 RID: 22279 RVA: 0x001C7458 File Offset: 0x001C5658
		internal void SetActive()
		{
			PSTransactionManager.EnableEngineProtection();
			PSTransaction pstransaction = this.transactionStack.Peek();
			if (pstransaction == null)
			{
				string noTransactionForActivation = TransactionStrings.NoTransactionForActivation;
				throw new InvalidOperationException(noTransactionForActivation);
			}
			if (pstransaction.IsRolledBack)
			{
				string noTransactionForActivationBecauseRollback = TransactionStrings.NoTransactionForActivationBecauseRollback;
				throw new TransactionAbortedException(noTransactionForActivationBecauseRollback);
			}
			this.previousActiveTransaction = Transaction.Current;
			pstransaction.Activate();
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x001C74AC File Offset: 0x001C56AC
		internal void ResetActive()
		{
			Transaction.Current = this.previousActiveTransaction;
			this.previousActiveTransaction = null;
		}

		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x06005709 RID: 22281 RVA: 0x001C74C0 File Offset: 0x001C56C0
		internal bool HasTransaction
		{
			get
			{
				PSTransaction pstransaction = this.transactionStack.Peek();
				return pstransaction != null && !pstransaction.IsCommitted && !pstransaction.IsRolledBack;
			}
		}

		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x0600570A RID: 22282 RVA: 0x001C74F0 File Offset: 0x001C56F0
		internal bool IsLastTransactionCommitted
		{
			get
			{
				PSTransaction pstransaction = this.transactionStack.Peek();
				return pstransaction != null && pstransaction.IsCommitted;
			}
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x0600570B RID: 22283 RVA: 0x001C7514 File Offset: 0x001C5714
		internal bool IsLastTransactionRolledBack
		{
			get
			{
				PSTransaction pstransaction = this.transactionStack.Peek();
				return pstransaction != null && pstransaction.IsRolledBack;
			}
		}

		// Token: 0x0600570C RID: 22284 RVA: 0x001C7538 File Offset: 0x001C5738
		~PSTransactionManager()
		{
			this.Dispose(false);
		}

		// Token: 0x0600570D RID: 22285 RVA: 0x001C7568 File Offset: 0x001C5768
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600570E RID: 22286 RVA: 0x001C7578 File Offset: 0x001C5778
		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.ResetActive();
				while (this.transactionStack.Peek() != null)
				{
					PSTransaction pstransaction = this.transactionStack.Pop();
					if (pstransaction != this.baseTransaction)
					{
						pstransaction.Dispose();
					}
				}
			}
		}

		// Token: 0x04002E67 RID: 11879
		private static bool engineProtectionEnabled;

		// Token: 0x04002E68 RID: 11880
		private Stack<PSTransaction> transactionStack;

		// Token: 0x04002E69 RID: 11881
		private PSTransaction baseTransaction;

		// Token: 0x04002E6A RID: 11882
		private Transaction previousActiveTransaction;
	}
}
