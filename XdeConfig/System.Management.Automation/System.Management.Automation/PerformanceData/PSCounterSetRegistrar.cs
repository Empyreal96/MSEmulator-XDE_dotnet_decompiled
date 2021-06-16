using System;
using System.Diagnostics.PerformanceData;

namespace System.Management.Automation.PerformanceData
{
	// Token: 0x02000906 RID: 2310
	public class PSCounterSetRegistrar : CounterSetRegistrarBase
	{
		// Token: 0x060056D1 RID: 22225 RVA: 0x001C68C2 File Offset: 0x001C4AC2
		public PSCounterSetRegistrar(Guid providerId, Guid counterSetId, CounterSetInstanceType counterSetInstType, CounterInfo[] counterInfoArray, string counterSetName = null) : base(providerId, counterSetId, counterSetInstType, counterInfoArray, counterSetName)
		{
		}

		// Token: 0x060056D2 RID: 22226 RVA: 0x001C68D1 File Offset: 0x001C4AD1
		public PSCounterSetRegistrar(PSCounterSetRegistrar srcPSCounterSetRegistrar) : base(srcPSCounterSetRegistrar)
		{
			if (srcPSCounterSetRegistrar == null)
			{
				throw new ArgumentNullException("srcPSCounterSetRegistrar");
			}
		}

		// Token: 0x060056D3 RID: 22227 RVA: 0x001C68E8 File Offset: 0x001C4AE8
		protected override CounterSetInstanceBase CreateCounterSetInstance()
		{
			return new PSCounterSetInstance(this);
		}

		// Token: 0x060056D4 RID: 22228 RVA: 0x001C68F0 File Offset: 0x001C4AF0
		public override void DisposeCounterSetInstance()
		{
			this._counterSetInstanceBase.Dispose();
		}
	}
}
