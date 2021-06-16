using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Management.Automation
{
	// Token: 0x02000454 RID: 1108
	internal class ReferenceIdHandlerForSerializer<T> where T : class
	{
		// Token: 0x0600308B RID: 12427 RVA: 0x00109B7C File Offset: 0x00107D7C
		private ulong GetNewReferenceId()
		{
			ulong result;
			this.seed = (result = this.seed) + 1UL;
			return result;
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x00109B9D File Offset: 0x00107D9D
		internal ReferenceIdHandlerForSerializer(IDictionary<T, ulong> dictionary)
		{
			this.object2refId = dictionary;
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x00109BAC File Offset: 0x00107DAC
		internal string SetRefId(T t)
		{
			if (this.object2refId != null)
			{
				ulong newReferenceId = this.GetNewReferenceId();
				this.object2refId.Add(t, newReferenceId);
				return newReferenceId.ToString(CultureInfo.InvariantCulture);
			}
			return null;
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x00109BE4 File Offset: 0x00107DE4
		internal string GetRefId(T t)
		{
			ulong num;
			if (this.object2refId != null && this.object2refId.TryGetValue(t, out num))
			{
				return num.ToString(CultureInfo.InvariantCulture);
			}
			return null;
		}

		// Token: 0x04001A26 RID: 6694
		private ulong seed;

		// Token: 0x04001A27 RID: 6695
		private readonly IDictionary<T, ulong> object2refId;
	}
}
