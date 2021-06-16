using System;
using System.Management.Automation.Remoting;
using System.Security;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008BF RID: 2239
	internal class PSRemotingCryptoHelperServer : PSRemotingCryptoHelper
	{
		// Token: 0x06005508 RID: 21768 RVA: 0x001C10AA File Offset: 0x001BF2AA
		internal PSRemotingCryptoHelperServer()
		{
			this._rsaCryptoProvider = PSRSACryptoServiceProvider.GetRSACryptoServiceProviderForServer();
		}

		// Token: 0x06005509 RID: 21769 RVA: 0x001C10C0 File Offset: 0x001BF2C0
		internal override string EncryptSecureString(SecureString secureString)
		{
			ServerRemoteSession serverRemoteSession = this.Session as ServerRemoteSession;
			if (serverRemoteSession != null && serverRemoteSession.Context.ClientCapability.ProtocolVersion >= RemotingConstants.ProtocolVersionWin8RTM)
			{
				this._rsaCryptoProvider.GenerateSessionKey();
			}
			else
			{
				base.RunKeyExchangeIfRequired();
			}
			return base.EncryptSecureStringCore(secureString);
		}

		// Token: 0x0600550A RID: 21770 RVA: 0x001C1112 File Offset: 0x001BF312
		internal override SecureString DecryptSecureString(string encryptedString)
		{
			base.RunKeyExchangeIfRequired();
			return base.DecryptSecureStringCore(encryptedString);
		}

		// Token: 0x0600550B RID: 21771 RVA: 0x001C1124 File Offset: 0x001BF324
		internal bool ImportRemotePublicKey(string publicKeyAsString)
		{
			try
			{
				this._rsaCryptoProvider.ImportPublicKeyFromBase64EncodedString(publicKeyAsString);
			}
			catch (PSCryptoException)
			{
				return false;
			}
			return true;
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x0600550C RID: 21772 RVA: 0x001C1158 File Offset: 0x001BF358
		// (set) Token: 0x0600550D RID: 21773 RVA: 0x001C1160 File Offset: 0x001BF360
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

		// Token: 0x0600550E RID: 21774 RVA: 0x001C116C File Offset: 0x001BF36C
		internal bool ExportEncryptedSessionKey(out string encryptedSessionKey)
		{
			try
			{
				encryptedSessionKey = this._rsaCryptoProvider.SafeExportSessionKey();
			}
			catch (PSCryptoException)
			{
				encryptedSessionKey = string.Empty;
				return false;
			}
			return true;
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x001C11A8 File Offset: 0x001BF3A8
		internal static PSRemotingCryptoHelperServer GetTestRemotingCryptHelperServer()
		{
			return new PSRemotingCryptoHelperServer
			{
				Session = new TestHelperSession()
			};
		}

		// Token: 0x04002C1F RID: 11295
		private RemoteSession _session;
	}
}
