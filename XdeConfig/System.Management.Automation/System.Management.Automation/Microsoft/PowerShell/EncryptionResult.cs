using System;

namespace Microsoft.PowerShell
{
	// Token: 0x02000814 RID: 2068
	internal class EncryptionResult
	{
		// Token: 0x06004FA9 RID: 20393 RVA: 0x001A6DF4 File Offset: 0x001A4FF4
		internal EncryptionResult(string encrypted, string IV)
		{
			this.encryptedData = encrypted;
			this.iv = IV;
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x06004FAA RID: 20394 RVA: 0x001A6E0A File Offset: 0x001A500A
		internal string EncryptedData
		{
			get
			{
				return this.encryptedData;
			}
		}

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x06004FAB RID: 20395 RVA: 0x001A6E12 File Offset: 0x001A5012
		internal string IV
		{
			get
			{
				return this.iv;
			}
		}

		// Token: 0x040028B7 RID: 10423
		private string encryptedData;

		// Token: 0x040028B8 RID: 10424
		private string iv;
	}
}
