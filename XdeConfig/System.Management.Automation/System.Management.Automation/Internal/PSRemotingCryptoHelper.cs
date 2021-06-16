using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Management.Automation.Internal
{
	// Token: 0x020008BE RID: 2238
	internal abstract class PSRemotingCryptoHelper : IDisposable
	{
		// Token: 0x060054FD RID: 21757 RVA: 0x001C0E58 File Offset: 0x001BF058
		protected void RunKeyExchangeIfRequired()
		{
			if (!this._rsaCryptoProvider.CanEncrypt)
			{
				try
				{
					lock (this.syncObject)
					{
						if (!this._rsaCryptoProvider.CanEncrypt && !this.keyExchangeStarted)
						{
							this.keyExchangeStarted = true;
							this._keyExchangeCompleted.Reset();
							this.Session.StartKeyExchange();
						}
					}
				}
				finally
				{
					this._keyExchangeCompleted.WaitOne();
				}
			}
		}

		// Token: 0x060054FE RID: 21758 RVA: 0x001C0EF0 File Offset: 0x001BF0F0
		protected string EncryptSecureStringCore(SecureString secureString)
		{
			string result = null;
			if (this._rsaCryptoProvider.CanEncrypt)
			{
				IntPtr intPtr = ClrFacade.SecureStringToCoTaskMemUnicode(secureString);
				if (intPtr != IntPtr.Zero)
				{
					byte[] array = new byte[secureString.Length * 2];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = Marshal.ReadByte(intPtr, i);
					}
					ClrFacade.ZeroFreeCoTaskMemUnicode(intPtr);
					try
					{
						byte[] inArray = this._rsaCryptoProvider.EncryptWithSessionKey(array);
						result = Convert.ToBase64String(inArray);
					}
					finally
					{
						for (int j = 0; j < array.Length; j++)
						{
							array[j] = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060054FF RID: 21759 RVA: 0x001C0F90 File Offset: 0x001BF190
		protected SecureString DecryptSecureStringCore(string encryptedString)
		{
			SecureString secureString = null;
			if (this._rsaCryptoProvider.CanEncrypt)
			{
				byte[] array = null;
				try
				{
					array = Convert.FromBase64String(encryptedString);
				}
				catch (FormatException)
				{
					throw new PSCryptoException();
				}
				if (array != null)
				{
					byte[] array2 = this._rsaCryptoProvider.DecryptWithSessionKey(array);
					secureString = new SecureString();
					try
					{
						for (int i = 0; i < array2.Length; i += 2)
						{
							ushort c = (ushort)array2[i] + (ushort)(array2[i + 1] << 8);
							secureString.AppendChar((char)c);
						}
					}
					finally
					{
						for (int j = 0; j < array2.Length; j += 2)
						{
							array2[j] = 0;
							array2[j + 1] = 0;
						}
					}
				}
			}
			return secureString;
		}

		// Token: 0x06005500 RID: 21760
		internal abstract string EncryptSecureString(SecureString secureString);

		// Token: 0x06005501 RID: 21761
		internal abstract SecureString DecryptSecureString(string encryptedString);

		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x06005502 RID: 21762
		// (set) Token: 0x06005503 RID: 21763
		internal abstract RemoteSession Session { get; set; }

		// Token: 0x06005504 RID: 21764 RVA: 0x001C1044 File Offset: 0x001BF244
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005505 RID: 21765 RVA: 0x001C1053 File Offset: 0x001BF253
		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._rsaCryptoProvider != null)
				{
					this._rsaCryptoProvider.Dispose();
				}
				this._rsaCryptoProvider = null;
				this._keyExchangeCompleted.Dispose();
			}
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x001C107D File Offset: 0x001BF27D
		internal void CompleteKeyExchange()
		{
			this._keyExchangeCompleted.Set();
		}

		// Token: 0x04002C1B RID: 11291
		protected PSRSACryptoServiceProvider _rsaCryptoProvider;

		// Token: 0x04002C1C RID: 11292
		protected ManualResetEvent _keyExchangeCompleted = new ManualResetEvent(false);

		// Token: 0x04002C1D RID: 11293
		protected object syncObject = new object();

		// Token: 0x04002C1E RID: 11294
		private bool keyExchangeStarted;
	}
}
