using System;
using System.Net;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography;
using Microsoft.PowerShell;

namespace System.Management.Automation
{
	// Token: 0x020007B5 RID: 1973
	[Serializable]
	public sealed class PSCredential : ISerializable
	{
		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x06004D65 RID: 19813 RVA: 0x001974BF File Offset: 0x001956BF
		// (set) Token: 0x06004D66 RID: 19814 RVA: 0x001974C6 File Offset: 0x001956C6
		public static GetSymmetricEncryptionKey GetSymmetricEncryptionKeyDelegate
		{
			get
			{
				return PSCredential._delegate;
			}
			set
			{
				PSCredential._delegate = value;
			}
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x001974D0 File Offset: 0x001956D0
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				return;
			}
			string value = string.Empty;
			if (this._password != null && this._password.Length > 0)
			{
				byte[] key;
				byte[] iv;
				if (PSCredential._delegate != null && PSCredential._delegate(context, out key, out iv))
				{
					value = SecureStringHelper.Encrypt(this._password, key, iv).EncryptedData;
				}
				else
				{
					try
					{
						value = SecureStringHelper.Protect(this._password);
					}
					catch (CryptographicException innerException)
					{
						throw PSTraceSource.NewInvalidOperationException(innerException, Credential.CredentialDisallowed, new object[0]);
					}
				}
			}
			info.AddValue("UserName", this._userName);
			info.AddValue("Password", value);
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x00197578 File Offset: 0x00195778
		private PSCredential(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				return;
			}
			this._userName = (string)info.GetValue("UserName", typeof(string));
			string text = (string)info.GetValue("Password", typeof(string));
			if (text == string.Empty)
			{
				this._password = new SecureString();
				return;
			}
			byte[] key;
			byte[] iv;
			if (PSCredential._delegate != null && PSCredential._delegate(context, out key, out iv))
			{
				this._password = SecureStringHelper.Decrypt(text, key, iv);
				return;
			}
			this._password = SecureStringHelper.Unprotect(text);
		}

		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x06004D69 RID: 19817 RVA: 0x00197616 File Offset: 0x00195816
		public string UserName
		{
			get
			{
				return this._userName;
			}
		}

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x06004D6A RID: 19818 RVA: 0x0019761E File Offset: 0x0019581E
		public SecureString Password
		{
			get
			{
				return this._password;
			}
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x00197626 File Offset: 0x00195826
		public PSCredential(string userName, SecureString password)
		{
			Utils.CheckArgForNullOrEmpty(userName, "userName");
			Utils.CheckArgForNull(password, "password");
			this._userName = userName;
			this._password = password;
		}

		// Token: 0x06004D6C RID: 19820 RVA: 0x00197654 File Offset: 0x00195854
		public PSCredential(PSObject pso)
		{
			if (pso == null)
			{
				throw PSTraceSource.NewArgumentNullException("pso");
			}
			if (pso.Properties["UserName"] != null)
			{
				this._userName = (string)pso.Properties["UserName"].Value;
				if (pso.Properties["Password"] != null)
				{
					this._password = (SecureString)pso.Properties["Password"].Value;
				}
			}
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x001976D9 File Offset: 0x001958D9
		private PSCredential()
		{
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x001976E4 File Offset: 0x001958E4
		public NetworkCredential GetNetworkCredential()
		{
			if (this._netCred == null)
			{
				string userName = null;
				string domain = null;
				if (PSCredential.IsValidUserName(this._userName, out userName, out domain))
				{
					this._netCred = new NetworkCredential(userName, this._password, domain);
				}
			}
			return this._netCred;
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x00197727 File Offset: 0x00195927
		public static explicit operator NetworkCredential(PSCredential credential)
		{
			if (credential == null)
			{
				throw PSTraceSource.NewArgumentNullException("credential");
			}
			return credential.GetNetworkCredential();
		}

		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x06004D70 RID: 19824 RVA: 0x0019773D File Offset: 0x0019593D
		public static PSCredential Empty
		{
			get
			{
				return PSCredential.empty;
			}
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x00197744 File Offset: 0x00195944
		private static bool IsValidUserName(string input, out string user, out string domain)
		{
			PSCredential.SplitUserDomain(input, out user, out domain);
			if (user == null || domain == null || user.Length == 0)
			{
				throw PSTraceSource.NewArgumentException("UserName", Credential.InvalidUserNameFormat, new object[0]);
			}
			return true;
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x00197778 File Offset: 0x00195978
		private static void SplitUserDomain(string input, out string user, out string domain)
		{
			user = null;
			domain = null;
			int num;
			if ((num = input.IndexOf('\\')) >= 0)
			{
				user = input.Substring(num + 1);
				domain = input.Substring(0, num);
				return;
			}
			num = input.LastIndexOf('@');
			if (num >= 0 && (input.LastIndexOf('.') < num || input.IndexOf('@') != num))
			{
				domain = input.Substring(num + 1);
				user = input.Substring(0, num);
				return;
			}
			user = input;
			domain = "";
		}

		// Token: 0x0400267B RID: 9851
		private static GetSymmetricEncryptionKey _delegate = null;

		// Token: 0x0400267C RID: 9852
		private string _userName;

		// Token: 0x0400267D RID: 9853
		private SecureString _password;

		// Token: 0x0400267E RID: 9854
		private NetworkCredential _netCred;

		// Token: 0x0400267F RID: 9855
		private static readonly PSCredential empty = new PSCredential();
	}
}
