using System;
using System.Diagnostics.PerformanceData;

namespace System.Management.Automation.PerformanceData
{
	// Token: 0x02000905 RID: 2309
	public abstract class CounterSetRegistrarBase
	{
		// Token: 0x060056C7 RID: 22215
		protected abstract CounterSetInstanceBase CreateCounterSetInstance();

		// Token: 0x060056C8 RID: 22216 RVA: 0x001C6728 File Offset: 0x001C4928
		protected CounterSetRegistrarBase(Guid providerId, Guid counterSetId, CounterSetInstanceType counterSetInstType, CounterInfo[] counterInfoArray, string counterSetName = null)
		{
			this._providerId = providerId;
			this._counterSetId = counterSetId;
			this._counterSetInstanceType = counterSetInstType;
			this._counterSetName = counterSetName;
			if (counterInfoArray == null || counterInfoArray.Length == 0)
			{
				throw new ArgumentNullException("counterInfoArray");
			}
			this._counterInfoArray = new CounterInfo[counterInfoArray.Length];
			for (int i = 0; i < counterInfoArray.Length; i++)
			{
				this._counterInfoArray[i] = new CounterInfo(counterInfoArray[i].Id, counterInfoArray[i].Type, counterInfoArray[i].Name);
			}
			this._counterSetInstanceBase = null;
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x001C67D0 File Offset: 0x001C49D0
		protected CounterSetRegistrarBase(CounterSetRegistrarBase srcCounterSetRegistrarBase)
		{
			if (srcCounterSetRegistrarBase == null)
			{
				throw new ArgumentNullException("srcCounterSetRegistrarBase");
			}
			this._providerId = srcCounterSetRegistrarBase._providerId;
			this._counterSetId = srcCounterSetRegistrarBase._counterSetId;
			this._counterSetInstanceType = srcCounterSetRegistrarBase._counterSetInstanceType;
			this._counterSetName = srcCounterSetRegistrarBase._counterSetName;
			CounterInfo[] counterInfoArray = srcCounterSetRegistrarBase._counterInfoArray;
			this._counterInfoArray = new CounterInfo[counterInfoArray.Length];
			for (int i = 0; i < counterInfoArray.Length; i++)
			{
				this._counterInfoArray[i] = new CounterInfo(counterInfoArray[i].Id, counterInfoArray[i].Type, counterInfoArray[i].Name);
			}
		}

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x060056CA RID: 22218 RVA: 0x001C687E File Offset: 0x001C4A7E
		public Guid ProviderId
		{
			get
			{
				return this._providerId;
			}
		}

		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x060056CB RID: 22219 RVA: 0x001C6886 File Offset: 0x001C4A86
		public Guid CounterSetId
		{
			get
			{
				return this._counterSetId;
			}
		}

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x060056CC RID: 22220 RVA: 0x001C688E File Offset: 0x001C4A8E
		public string CounterSetName
		{
			get
			{
				return this._counterSetName;
			}
		}

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x060056CD RID: 22221 RVA: 0x001C6896 File Offset: 0x001C4A96
		public CounterSetInstanceType CounterSetInstType
		{
			get
			{
				return this._counterSetInstanceType;
			}
		}

		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x060056CE RID: 22222 RVA: 0x001C689E File Offset: 0x001C4A9E
		public CounterInfo[] CounterInfoArray
		{
			get
			{
				return this._counterInfoArray;
			}
		}

		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x060056CF RID: 22223 RVA: 0x001C68A6 File Offset: 0x001C4AA6
		public CounterSetInstanceBase CounterSetInstance
		{
			get
			{
				if (this._counterSetInstanceBase == null)
				{
					this._counterSetInstanceBase = this.CreateCounterSetInstance();
				}
				return this._counterSetInstanceBase;
			}
		}

		// Token: 0x060056D0 RID: 22224
		public abstract void DisposeCounterSetInstance();

		// Token: 0x04002E4F RID: 11855
		private readonly Guid _providerId;

		// Token: 0x04002E50 RID: 11856
		private readonly Guid _counterSetId;

		// Token: 0x04002E51 RID: 11857
		private readonly string _counterSetName;

		// Token: 0x04002E52 RID: 11858
		private readonly CounterSetInstanceType _counterSetInstanceType;

		// Token: 0x04002E53 RID: 11859
		private readonly CounterInfo[] _counterInfoArray;

		// Token: 0x04002E54 RID: 11860
		protected CounterSetInstanceBase _counterSetInstanceBase;
	}
}
