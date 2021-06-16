using System;
using System.Text;
using System.Transactions;

namespace Microsoft.PowerShell.Commands.Management
{
	// Token: 0x0200090D RID: 2317
	public class TransactedString : IEnlistmentNotification
	{
		// Token: 0x06005710 RID: 22288 RVA: 0x001C75BA File Offset: 0x001C57BA
		public TransactedString() : this("")
		{
		}

		// Token: 0x06005711 RID: 22289 RVA: 0x001C75C7 File Offset: 0x001C57C7
		public TransactedString(string value)
		{
			this.m_Value = new StringBuilder(value);
			this.m_TemporaryValue = null;
		}

		// Token: 0x06005712 RID: 22290 RVA: 0x001C75E2 File Offset: 0x001C57E2
		void IEnlistmentNotification.Commit(Enlistment enlistment)
		{
			this.m_Value = new StringBuilder(this.m_TemporaryValue.ToString());
			this.m_TemporaryValue = null;
			this.enlistedTransaction = null;
			enlistment.Done();
		}

		// Token: 0x06005713 RID: 22291 RVA: 0x001C760E File Offset: 0x001C580E
		void IEnlistmentNotification.Rollback(Enlistment enlistment)
		{
			this.m_TemporaryValue = null;
			this.enlistedTransaction = null;
			enlistment.Done();
		}

		// Token: 0x06005714 RID: 22292 RVA: 0x001C7624 File Offset: 0x001C5824
		void IEnlistmentNotification.InDoubt(Enlistment enlistment)
		{
			enlistment.Done();
		}

		// Token: 0x06005715 RID: 22293 RVA: 0x001C762C File Offset: 0x001C582C
		void IEnlistmentNotification.Prepare(PreparingEnlistment preparingEnlistment)
		{
			preparingEnlistment.Prepared();
		}

		// Token: 0x06005716 RID: 22294 RVA: 0x001C7634 File Offset: 0x001C5834
		public void Append(string text)
		{
			this.ValidateTransactionOrEnlist();
			if (this.enlistedTransaction != null)
			{
				this.m_TemporaryValue.Append(text);
				return;
			}
			this.m_Value.Append(text);
		}

		// Token: 0x06005717 RID: 22295 RVA: 0x001C7665 File Offset: 0x001C5865
		public void Remove(int startIndex, int length)
		{
			this.ValidateTransactionOrEnlist();
			if (this.enlistedTransaction != null)
			{
				this.m_TemporaryValue.Remove(startIndex, length);
				return;
			}
			this.m_Value.Remove(startIndex, length);
		}

		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x06005718 RID: 22296 RVA: 0x001C7698 File Offset: 0x001C5898
		public int Length
		{
			get
			{
				if (Transaction.Current == null || this.enlistedTransaction != Transaction.Current)
				{
					return this.m_Value.Length;
				}
				return this.m_TemporaryValue.Length;
			}
		}

		// Token: 0x06005719 RID: 22297 RVA: 0x001C76D0 File Offset: 0x001C58D0
		public override string ToString()
		{
			if (Transaction.Current == null || this.enlistedTransaction != Transaction.Current)
			{
				return this.m_Value.ToString();
			}
			return this.m_TemporaryValue.ToString();
		}

		// Token: 0x0600571A RID: 22298 RVA: 0x001C7708 File Offset: 0x001C5908
		private void ValidateTransactionOrEnlist()
		{
			if (Transaction.Current != null)
			{
				if (this.enlistedTransaction == null)
				{
					Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
					this.enlistedTransaction = Transaction.Current;
					this.m_TemporaryValue = new StringBuilder(this.m_Value.ToString());
					return;
				}
				if (Transaction.Current != this.enlistedTransaction)
				{
					throw new InvalidOperationException("Cannot modify string. It has been modified by another transaction.");
				}
			}
			else if (this.enlistedTransaction != null)
			{
				throw new InvalidOperationException("Cannot modify string. It has been modified by another transaction.");
			}
		}

		// Token: 0x04002E6B RID: 11883
		private StringBuilder m_Value;

		// Token: 0x04002E6C RID: 11884
		private StringBuilder m_TemporaryValue;

		// Token: 0x04002E6D RID: 11885
		private Transaction enlistedTransaction;
	}
}
