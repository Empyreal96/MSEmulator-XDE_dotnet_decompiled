using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008BB RID: 2235
	internal class PSCryptoNativeUtils
	{
		// Token: 0x060054D9 RID: 21721
		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptGenKey(PSSafeCryptProvHandle hProv, uint Algid, uint dwFlags, ref PSSafeCryptKey phKey);

		// Token: 0x060054DA RID: 21722
		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptDestroyKey(IntPtr hKey);

		// Token: 0x060054DB RID: 21723
		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptAcquireContext(ref PSSafeCryptProvHandle phProv, [MarshalAs(UnmanagedType.LPWStr)] [In] string szContainer, [MarshalAs(UnmanagedType.LPWStr)] [In] string szProvider, uint dwProvType, uint dwFlags);

		// Token: 0x060054DC RID: 21724
		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);

		// Token: 0x060054DD RID: 21725
		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptEncrypt(PSSafeCryptKey hKey, IntPtr hHash, [MarshalAs(UnmanagedType.Bool)] bool Final, uint dwFlags, byte[] pbData, ref int pdwDataLen, int dwBufLen);

		// Token: 0x060054DE RID: 21726
		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptDecrypt(PSSafeCryptKey hKey, IntPtr hHash, [MarshalAs(UnmanagedType.Bool)] bool Final, uint dwFlags, byte[] pbData, ref int pdwDataLen);

		// Token: 0x060054DF RID: 21727
		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptExportKey(PSSafeCryptKey hKey, PSSafeCryptKey hExpKey, uint dwBlobType, uint dwFlags, byte[] pbData, ref uint pdwDataLen);

		// Token: 0x060054E0 RID: 21728
		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptImportKey(PSSafeCryptProvHandle hProv, byte[] pbData, int dwDataLen, PSSafeCryptKey hPubKey, uint dwFlags, ref PSSafeCryptKey phKey);

		// Token: 0x060054E1 RID: 21729
		[DllImport("advapi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CryptDuplicateKey(PSSafeCryptKey hKey, ref uint pdwReserved, uint dwFlags, ref PSSafeCryptKey phKey);

		// Token: 0x060054E2 RID: 21730
		[DllImport("kernel32.dll")]
		public static extern uint GetLastError();

		// Token: 0x04002BFF RID: 11263
		public const uint CRYPT_VERIFYCONTEXT = 4026531840U;

		// Token: 0x04002C00 RID: 11264
		public const uint CRYPT_EXPORTABLE = 1U;

		// Token: 0x04002C01 RID: 11265
		public const int CRYPT_CREATE_SALT = 4;

		// Token: 0x04002C02 RID: 11266
		public const int PROV_RSA_FULL = 1;

		// Token: 0x04002C03 RID: 11267
		public const int PROV_RSA_AES = 24;

		// Token: 0x04002C04 RID: 11268
		public const int AT_KEYEXCHANGE = 1;

		// Token: 0x04002C05 RID: 11269
		public const int CALG_RSA_KEYX = 41984;

		// Token: 0x04002C06 RID: 11270
		public const int ALG_CLASS_KEY_EXCHANGE = 40960;

		// Token: 0x04002C07 RID: 11271
		public const int ALG_TYPE_RSA = 1024;

		// Token: 0x04002C08 RID: 11272
		public const int ALG_SID_RSA_ANY = 0;

		// Token: 0x04002C09 RID: 11273
		public const int PUBLICKEYBLOB = 6;

		// Token: 0x04002C0A RID: 11274
		public const int SIMPLEBLOB = 1;

		// Token: 0x04002C0B RID: 11275
		public const int CALG_AES_256 = 26128;

		// Token: 0x04002C0C RID: 11276
		public const int ALG_CLASS_DATA_ENCRYPT = 24576;

		// Token: 0x04002C0D RID: 11277
		public const int ALG_TYPE_BLOCK = 1536;

		// Token: 0x04002C0E RID: 11278
		public const int ALG_SID_AES_256 = 16;

		// Token: 0x04002C0F RID: 11279
		public const int CALG_AES_128 = 26126;

		// Token: 0x04002C10 RID: 11280
		public const int ALG_SID_AES_128 = 14;
	}
}
