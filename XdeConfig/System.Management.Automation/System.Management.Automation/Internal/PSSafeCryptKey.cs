using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008BA RID: 2234
	internal class PSSafeCryptKey : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060054D5 RID: 21717 RVA: 0x001C0855 File Offset: 0x001BEA55
		internal PSSafeCryptKey() : base(true)
		{
		}

		// Token: 0x060054D6 RID: 21718 RVA: 0x001C085E File Offset: 0x001BEA5E
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return PSCryptoNativeUtils.CryptDestroyKey(this.handle);
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x060054D7 RID: 21719 RVA: 0x001C086B File Offset: 0x001BEA6B
		internal static PSSafeCryptKey Zero
		{
			get
			{
				return PSSafeCryptKey._zero;
			}
		}

		// Token: 0x04002BFE RID: 11262
		private static PSSafeCryptKey _zero = new PSSafeCryptKey();
	}
}
