using System;
using System.Security;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008C0 RID: 2240
	internal class PSRemotingCryptoHelperClient : PSRemotingCryptoHelper
	{
		// Token: 0x06005510 RID: 21776 RVA: 0x001C11C7 File Offset: 0x001BF3C7
		internal PSRemotingCryptoHelperClient()
		{
			this._rsaCryptoProvider = PSRSACryptoServiceProvider.GetRSACryptoServiceProviderForClient();
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x001C11DA File Offset: 0x001BF3DA
		internal override string EncryptSecureString(SecureString secureString)
		{
			base.RunKeyExchangeIfRequired();
			return base.EncryptSecureStringCore(secureString);
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x001C11E9 File Offset: 0x001BF3E9
		internal override SecureString DecryptSecureString(string encryptedString)
		{
			base.RunKeyExchangeIfRequired();
			return base.DecryptSecureStringCore(encryptedString);
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x001C11F8 File Offset: 0x001BF3F8
		internal bool ExportLocalPublicKey(out string publicKeyAsString)
		{
			try
			{
				this._rsaCryptoProvider.GenerateKeyPair();
			}
			catch (PSCryptoException)
			{
				throw;
			}
			try
			{
				publicKeyAsString = this._rsaCryptoProvider.GetPublicKeyAsBase64EncodedString();
			}
			catch (PSCryptoException)
			{
				publicKeyAsString = string.Empty;
				return false;
			}
			return true;
		}

		// Token: 0x06005514 RID: 21780 RVA: 0x001C1250 File Offset: 0x001BF450
		internal bool ImportEncryptedSessionKey(string encryptedSessionKey)
		{
			try
			{
				this._rsaCryptoProvider.ImportSessionKeyFromBase64EncodedString(encryptedSessionKey);
			}
			catch (PSCryptoException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x06005515 RID: 21781 RVA: 0x001C1284 File Offset: 0x001BF484
		// (set) Token: 0x06005516 RID: 21782 RVA: 0x001C128C File Offset: 0x001BF48C
		internal override RemoteSession Session
		{
			get
			{
				return this._session;
			}
			set
			{
				this._session = value;
			}
		}

		// Token: 0x06005517 RID: 21783 RVA: 0x001C1298 File Offset: 0x001BF498
		internal static PSRemotingCryptoHelperClient GetTestRemotingCryptHelperClient()
		{
			return new PSRemotingCryptoHelperClient
			{
				Session = new TestHelperSession()
			};
		}

		// Token: 0x04002C20 RID: 11296
		private RemoteSession _session;
	}
}
