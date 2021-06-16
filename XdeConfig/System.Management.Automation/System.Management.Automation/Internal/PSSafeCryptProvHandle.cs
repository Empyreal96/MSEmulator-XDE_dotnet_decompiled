using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008B9 RID: 2233
	internal class PSSafeCryptProvHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060054D3 RID: 21715 RVA: 0x001C083E File Offset: 0x001BEA3E
		internal PSSafeCryptProvHandle() : base(true)
		{
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x001C0847 File Offset: 0x001BEA47
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			return PSCryptoNativeUtils.CryptReleaseContext(this.handle, 0U);
		}
	}
}
