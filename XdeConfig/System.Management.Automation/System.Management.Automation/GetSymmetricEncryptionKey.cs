using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020007B4 RID: 1972
	// (Invoke) Token: 0x06004D62 RID: 19810
	public delegate bool GetSymmetricEncryptionKey(StreamingContext context, out byte[] key, out byte[] iv);
}
