using System;
using System.Collections.Generic;
using System.Management.Automation.Remoting;

namespace System.Management.Automation
{
	// Token: 0x02000334 RID: 820
	public abstract class Repository<T> where T : class
	{
		// Token: 0x060027BB RID: 10171 RVA: 0x000DEBF4 File Offset: 0x000DCDF4
		public void Add(T item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(this.identifier);
			}
			lock (this.syncObject)
			{
				Guid key = this.GetKey(item);
				if (this.repository.ContainsKey(key))
				{
					throw new ArgumentException(this.identifier);
				}
				this.repository.Add(key, item);
			}
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x000DEC74 File Offset: 0x000DCE74
		public void Remove(T item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(this.identifier);
			}
			lock (this.syncObject)
			{
				Guid key = this.GetKey(item);
				if (!this.repository.ContainsKey(key))
				{
					string message = PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.ItemNotFoundInRepository, new object[]
					{
						"Job repository",
						key.ToString()
					});
					throw new ArgumentException(message);
				}
				this.repository.Remove(key);
			}
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000DED20 File Offset: 0x000DCF20
		public List<T> GetItems()
		{
			return this.Items;
		}

		// Token: 0x060027BE RID: 10174
		protected abstract Guid GetKey(T item);

		// Token: 0x060027BF RID: 10175 RVA: 0x000DED28 File Offset: 0x000DCF28
		protected Repository(string identifier)
		{
			this.identifier = identifier;
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x000DED50 File Offset: 0x000DCF50
		internal List<T> Items
		{
			get
			{
				List<T> result;
				lock (this.syncObject)
				{
					result = new List<T>(this.repository.Values);
				}
				return result;
			}
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x000DED9C File Offset: 0x000DCF9C
		public T GetItem(Guid instanceId)
		{
			T result;
			lock (this.syncObject)
			{
				if (this.repository.ContainsKey(instanceId))
				{
					result = this.repository[instanceId];
				}
				else
				{
					result = default(T);
				}
			}
			return result;
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x060027C2 RID: 10178 RVA: 0x000DEE00 File Offset: 0x000DD000
		internal Dictionary<Guid, T> Dictionary
		{
			get
			{
				return this.repository;
			}
		}

		// Token: 0x040013AE RID: 5038
		private Dictionary<Guid, T> repository = new Dictionary<Guid, T>();

		// Token: 0x040013AF RID: 5039
		private object syncObject = new object();

		// Token: 0x040013B0 RID: 5040
		private string identifier;
	}
}
