using System;
using System.ComponentModel;
using System.Text;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008BD RID: 2237
	internal class PSRSACryptoServiceProvider : IDisposable
	{
		// Token: 0x060054EB RID: 21739 RVA: 0x001C08F0 File Offset: 0x001BEAF0
		private PSRSACryptoServiceProvider(bool serverMode)
		{
			if (serverMode)
			{
				this.hProv = new PSSafeCryptProvHandle();
				bool value = PSCryptoNativeUtils.CryptAcquireContext(ref this.hProv, null, null, 24U, 4026531840U);
				this.CheckStatus(value);
				this.hRSAKey = new PSSafeCryptKey();
			}
			this.hSessionKey = new PSSafeCryptKey();
		}

		// Token: 0x060054EC RID: 21740 RVA: 0x001C0944 File Offset: 0x001BEB44
		internal string GetPublicKeyAsBase64EncodedString()
		{
			uint num = 0U;
			bool value = PSCryptoNativeUtils.CryptExportKey(this.hRSAKey, PSSafeCryptKey.Zero, 6U, 0U, null, ref num);
			this.CheckStatus(value);
			byte[] array = new byte[num];
			value = PSCryptoNativeUtils.CryptExportKey(this.hRSAKey, PSSafeCryptKey.Zero, 6U, 0U, array, ref num);
			this.CheckStatus(value);
			return Convert.ToBase64String(array);
		}

		// Token: 0x060054ED RID: 21741 RVA: 0x001C09A0 File Offset: 0x001BEBA0
		internal void GenerateSessionKey()
		{
			if (this.sessionKeyGenerated)
			{
				return;
			}
			lock (PSRSACryptoServiceProvider.syncObject)
			{
				if (!this.sessionKeyGenerated)
				{
					bool value = PSCryptoNativeUtils.CryptGenKey(this.hProv, 26128U, 16777221U, ref this.hSessionKey);
					this.CheckStatus(value);
					this.sessionKeyGenerated = true;
					this.canEncrypt = true;
				}
			}
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x001C0A1C File Offset: 0x001BEC1C
		internal string SafeExportSessionKey()
		{
			this.GenerateSessionKey();
			uint num = 0U;
			bool value = PSCryptoNativeUtils.CryptExportKey(this.hSessionKey, this.hRSAKey, 1U, 0U, null, ref num);
			this.CheckStatus(value);
			byte[] array = new byte[num];
			value = PSCryptoNativeUtils.CryptExportKey(this.hSessionKey, this.hRSAKey, 1U, 0U, array, ref num);
			this.CheckStatus(value);
			this.canEncrypt = true;
			return Convert.ToBase64String(array);
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x001C0A84 File Offset: 0x001BEC84
		internal void ImportPublicKeyFromBase64EncodedString(string publicKey)
		{
			byte[] array = Convert.FromBase64String(publicKey);
			bool value = PSCryptoNativeUtils.CryptImportKey(this.hProv, array, array.Length, PSSafeCryptKey.Zero, 0U, ref this.hRSAKey);
			this.CheckStatus(value);
		}

		// Token: 0x060054F0 RID: 21744 RVA: 0x001C0ABC File Offset: 0x001BECBC
		internal void ImportSessionKeyFromBase64EncodedString(string sessionKey)
		{
			byte[] array = Convert.FromBase64String(sessionKey);
			bool value = PSCryptoNativeUtils.CryptImportKey(this.hProv, array, array.Length, this.hRSAKey, 0U, ref this.hSessionKey);
			this.CheckStatus(value);
			this.canEncrypt = true;
		}

		// Token: 0x060054F1 RID: 21745 RVA: 0x001C0AFC File Offset: 0x001BECFC
		internal byte[] EncryptWithSessionKey(byte[] data)
		{
			byte[] array = new byte[data.Length];
			Array.Copy(data, 0, array, 0, data.Length);
			int num = array.Length;
			if (!PSCryptoNativeUtils.CryptEncrypt(this.hSessionKey, IntPtr.Zero, true, 0U, array, ref num, data.Length))
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 0;
				}
				array = new byte[num];
				Array.Copy(data, 0, array, 0, data.Length);
				num = data.Length;
				bool value = PSCryptoNativeUtils.CryptEncrypt(this.hSessionKey, IntPtr.Zero, true, 0U, array, ref num, array.Length);
				this.CheckStatus(value);
			}
			byte[] array2 = new byte[num];
			Array.Copy(array, 0, array2, 0, num);
			return array2;
		}

		// Token: 0x060054F2 RID: 21746 RVA: 0x001C0B9C File Offset: 0x001BED9C
		internal byte[] DecryptWithSessionKey(byte[] data)
		{
			byte[] array = new byte[data.Length];
			Array.Copy(data, 0, array, 0, data.Length);
			int num = array.Length;
			if (!PSCryptoNativeUtils.CryptDecrypt(this.hSessionKey, IntPtr.Zero, true, 0U, array, ref num))
			{
				array = new byte[num];
				Array.Copy(data, 0, array, 0, data.Length);
				bool value = PSCryptoNativeUtils.CryptDecrypt(this.hSessionKey, IntPtr.Zero, true, 0U, array, ref num);
				this.CheckStatus(value);
			}
			byte[] array2 = new byte[num];
			Array.Copy(array, 0, array2, 0, num);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			return array2;
		}

		// Token: 0x060054F3 RID: 21747 RVA: 0x001C0C34 File Offset: 0x001BEE34
		internal void GenerateKeyPair()
		{
			if (!PSRSACryptoServiceProvider.keyPairGenerated)
			{
				lock (PSRSACryptoServiceProvider.syncObject)
				{
					if (!PSRSACryptoServiceProvider.keyPairGenerated)
					{
						PSRSACryptoServiceProvider._hStaticProv = new PSSafeCryptProvHandle();
						bool value = PSCryptoNativeUtils.CryptAcquireContext(ref PSRSACryptoServiceProvider._hStaticProv, null, null, 24U, 4026531840U);
						this.CheckStatus(value);
						PSRSACryptoServiceProvider._hStaticRSAKey = new PSSafeCryptKey();
						value = PSCryptoNativeUtils.CryptGenKey(PSRSACryptoServiceProvider._hStaticProv, 1U, 134217729U, ref PSRSACryptoServiceProvider._hStaticRSAKey);
						this.CheckStatus(value);
						PSRSACryptoServiceProvider.keyPairGenerated = true;
					}
				}
			}
			this.hProv = PSRSACryptoServiceProvider._hStaticProv;
			this.hRSAKey = PSRSACryptoServiceProvider._hStaticRSAKey;
		}

		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x060054F4 RID: 21748 RVA: 0x001C0CE4 File Offset: 0x001BEEE4
		// (set) Token: 0x060054F5 RID: 21749 RVA: 0x001C0CEC File Offset: 0x001BEEEC
		internal bool CanEncrypt
		{
			get
			{
				return this.canEncrypt;
			}
			set
			{
				this.canEncrypt = value;
			}
		}

		// Token: 0x060054F6 RID: 21750 RVA: 0x001C0CF8 File Offset: 0x001BEEF8
		internal static PSRSACryptoServiceProvider GetRSACryptoServiceProviderForClient()
		{
			return new PSRSACryptoServiceProvider(false)
			{
				hProv = PSRSACryptoServiceProvider._hStaticProv,
				hRSAKey = PSRSACryptoServiceProvider._hStaticRSAKey
			};
		}

		// Token: 0x060054F7 RID: 21751 RVA: 0x001C0D24 File Offset: 0x001BEF24
		internal static PSRSACryptoServiceProvider GetRSACryptoServiceProviderForServer()
		{
			return new PSRSACryptoServiceProvider(true);
		}

		// Token: 0x060054F8 RID: 21752 RVA: 0x001C0D3C File Offset: 0x001BEF3C
		private void CheckStatus(bool value)
		{
			if (value)
			{
				return;
			}
			uint lastError = PSCryptoNativeUtils.GetLastError();
			StringBuilder message = new StringBuilder(new Win32Exception((int)lastError).Message);
			throw new PSCryptoException(lastError, message);
		}

		// Token: 0x060054F9 RID: 21753 RVA: 0x001C0D6B File Offset: 0x001BEF6B
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060054FA RID: 21754 RVA: 0x001C0D7C File Offset: 0x001BEF7C
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.hSessionKey != null)
				{
					if (!this.hSessionKey.IsInvalid)
					{
						this.hSessionKey.Dispose();
					}
					this.hSessionKey = null;
				}
				if (PSRSACryptoServiceProvider._hStaticRSAKey == null && this.hRSAKey != null)
				{
					if (!this.hRSAKey.IsInvalid)
					{
						this.hRSAKey.Dispose();
					}
					this.hRSAKey = null;
				}
				if (PSRSACryptoServiceProvider._hStaticProv == null && this.hProv != null)
				{
					if (!this.hProv.IsInvalid)
					{
						this.hProv.Dispose();
					}
					this.hProv = null;
				}
			}
		}

		// Token: 0x060054FB RID: 21755 RVA: 0x001C0E14 File Offset: 0x001BF014
		~PSRSACryptoServiceProvider()
		{
			this.Dispose(true);
		}

		// Token: 0x04002C12 RID: 11282
		private PSSafeCryptProvHandle hProv;

		// Token: 0x04002C13 RID: 11283
		private bool canEncrypt;

		// Token: 0x04002C14 RID: 11284
		private PSSafeCryptKey hRSAKey;

		// Token: 0x04002C15 RID: 11285
		private PSSafeCryptKey hSessionKey;

		// Token: 0x04002C16 RID: 11286
		private bool sessionKeyGenerated;

		// Token: 0x04002C17 RID: 11287
		private static PSSafeCryptProvHandle _hStaticProv;

		// Token: 0x04002C18 RID: 11288
		private static PSSafeCryptKey _hStaticRSAKey;

		// Token: 0x04002C19 RID: 11289
		private static bool keyPairGenerated = false;

		// Token: 0x04002C1A RID: 11290
		private static object syncObject = new object();
	}
}
