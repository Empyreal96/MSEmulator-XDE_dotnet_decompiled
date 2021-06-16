using System;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.PowerShell
{
	// Token: 0x02000813 RID: 2067
	internal static class SecureStringHelper
	{
		// Token: 0x06004F9D RID: 20381 RVA: 0x001A69E4 File Offset: 0x001A4BE4
		private static SecureString New(byte[] data)
		{
			if (data.Length % 2 != 0)
			{
				string invalidKey = Serialization.InvalidKey;
				throw new PSArgumentException(invalidKey);
			}
			SecureString secureString = new SecureString();
			int num = data.Length / 2;
			for (int i = 0; i < num; i++)
			{
				char c = (char)((int)data[2 * i + 1] * 256 + (int)data[2 * i]);
				secureString.AppendChar(c);
				data[2 * i] = 0;
				data[2 * i + 1] = 0;
			}
			return secureString;
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x001A6A50 File Offset: 0x001A4C50
		[ArchitectureSensitive]
		internal static byte[] GetData(SecureString s)
		{
			byte[] array = new byte[s.Length * 2];
			if (s.Length > 0)
			{
				IntPtr intPtr = ClrFacade.SecureStringToCoTaskMemUnicode(s);
				try
				{
					Marshal.Copy(intPtr, array, 0, array.Length);
				}
				finally
				{
					ClrFacade.ZeroFreeCoTaskMemUnicode(intPtr);
				}
			}
			return array;
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x001A6AA0 File Offset: 0x001A4CA0
		internal static string ByteArrayToString(byte[] data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				stringBuilder.Append(data[i].ToString("x2", CultureInfo.InvariantCulture));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x001A6AE4 File Offset: 0x001A4CE4
		internal static byte[] ByteArrayFromString(string s)
		{
			int num = s.Length / 2;
			byte[] array = new byte[num];
			if (s.Length > 0)
			{
				for (int i = 0; i < num; i++)
				{
					array[i] = byte.Parse(s.Substring(2 * i, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				}
			}
			return array;
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x001A6B34 File Offset: 0x001A4D34
		internal static string Protect(SecureString input)
		{
			Utils.CheckSecureStringArg(input, "input");
			byte[] data = SecureStringHelper.GetData(input);
			byte[] data2 = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = 0;
			}
			return SecureStringHelper.ByteArrayToString(data2);
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x001A6B80 File Offset: 0x001A4D80
		internal static SecureString Unprotect(string input)
		{
			Utils.CheckArgForNullOrEmpty(input, "input");
			if (input.Length % 2 != 0)
			{
				throw PSTraceSource.NewArgumentException("input", Serialization.InvalidEncryptedString, new object[]
				{
					input
				});
			}
			byte[] encryptedData = SecureStringHelper.ByteArrayFromString(input);
			byte[] data = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
			return SecureStringHelper.New(data);
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x001A6BDC File Offset: 0x001A4DDC
		internal static EncryptionResult Encrypt(SecureString input, SecureString key)
		{
			byte[] data = SecureStringHelper.GetData(key);
			EncryptionResult result = SecureStringHelper.Encrypt(input, data);
			Array.Clear(data, 0, data.Length);
			return result;
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x001A6C05 File Offset: 0x001A4E05
		internal static EncryptionResult Encrypt(SecureString input, byte[] key)
		{
			return SecureStringHelper.Encrypt(input, key, null);
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x001A6C10 File Offset: 0x001A4E10
		internal static EncryptionResult Encrypt(SecureString input, byte[] key, byte[] iv)
		{
			Utils.CheckSecureStringArg(input, "input");
			Utils.CheckKeyArg(key, "key");
			Aes aes = Aes.Create();
			if (iv == null)
			{
				iv = aes.IV;
			}
			ICryptoTransform transform = aes.CreateEncryptor(key, iv);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream2;
			CryptoStream cryptoStream = cryptoStream2 = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			EncryptionResult result;
			try
			{
				byte[] data = SecureStringHelper.GetData(input);
				cryptoStream.Write(data, 0, data.Length);
				cryptoStream.FlushFinalBlock();
				Array.Clear(data, 0, data.Length);
				byte[] data2 = memoryStream.ToArray();
				EncryptionResult encryptionResult = new EncryptionResult(SecureStringHelper.ByteArrayToString(data2), Convert.ToBase64String(iv));
				result = encryptionResult;
			}
			finally
			{
				if (cryptoStream2 != null)
				{
					((IDisposable)cryptoStream2).Dispose();
				}
			}
			return result;
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x001A6CD0 File Offset: 0x001A4ED0
		internal static SecureString Decrypt(string input, SecureString key, byte[] IV)
		{
			byte[] data = SecureStringHelper.GetData(key);
			SecureString result = SecureStringHelper.Decrypt(input, data, IV);
			Array.Clear(data, 0, data.Length);
			return result;
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x001A6CFC File Offset: 0x001A4EFC
		internal static SecureString Decrypt(string input, byte[] key, byte[] IV)
		{
			Utils.CheckArgForNullOrEmpty(input, "input");
			Utils.CheckKeyArg(key, "key");
			Aes aes = Aes.Create();
			byte[] array = SecureStringHelper.ByteArrayFromString(input);
			ICryptoTransform transform;
			if (IV != null)
			{
				transform = aes.CreateDecryptor(key, IV);
			}
			else
			{
				transform = aes.CreateDecryptor(key, aes.IV);
			}
			MemoryStream stream = new MemoryStream(array);
			SecureString result;
			using (CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read))
			{
				byte[] array2 = new byte[array.Length];
				int num = cryptoStream.Read(array2, 0, array2.Length);
				byte[] array3 = new byte[num];
				for (int i = 0; i < num; i++)
				{
					array3[i] = array2[i];
				}
				SecureString secureString = SecureStringHelper.New(array3);
				Array.Clear(array3, 0, array3.Length);
				Array.Clear(array2, 0, array2.Length);
				result = secureString;
			}
			return result;
		}

		// Token: 0x040028B6 RID: 10422
		internal static string SecureStringExportHeader = "76492d1116743f0423413b16050a5345";
	}
}
