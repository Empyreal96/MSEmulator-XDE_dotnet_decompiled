using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Transactions;
using Microsoft.Win32;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x020007AC RID: 1964
	[ComVisible(true)]
	public sealed class TransactedRegistryKey : MarshalByRefObject, IDisposable
	{
		// Token: 0x06004CF0 RID: 19696 RVA: 0x00195A2C File Offset: 0x00193C2C
		private int RegOpenKeyTransactedWrapper(SafeRegistryHandle hKey, string lpSubKey, int ulOptions, int samDesired, out SafeRegistryHandle hkResult, SafeTransactionHandle hTransaction, IntPtr pExtendedParameter)
		{
			SafeRegistryHandle safeRegistryHandle = null;
			int num = Win32Native.RegOpenKeyTransacted(this.hkey, lpSubKey, ulOptions, samDesired, out safeRegistryHandle, hTransaction, pExtendedParameter);
			if (num == 0 && !safeRegistryHandle.IsInvalid)
			{
				int num2 = 0;
				int num3 = 0;
				num = Win32Native.RegQueryInfoKey(safeRegistryHandle, null, null, Win32Native.NULL, ref num2, null, null, ref num3, null, null, null, null);
				if (6700 == num)
				{
					SafeRegistryHandle safeRegistryHandle2 = null;
					SafeRegistryHandle safeRegistryHandle3 = null;
					num = Win32Native.RegOpenKeyEx(this.hkey, lpSubKey, ulOptions, samDesired, out safeRegistryHandle2);
					if (num == 0)
					{
						num = Win32Native.RegOpenKeyTransacted(safeRegistryHandle2, null, ulOptions, samDesired, out safeRegistryHandle3, hTransaction, pExtendedParameter);
						if (num == 0)
						{
							safeRegistryHandle.Dispose();
							safeRegistryHandle = safeRegistryHandle3;
						}
						safeRegistryHandle2.Dispose();
						safeRegistryHandle2 = null;
					}
				}
			}
			hkResult = safeRegistryHandle;
			return num;
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x00195ACC File Offset: 0x00193CCC
		private TransactedRegistryKey(SafeRegistryHandle hkey, bool writable, bool systemkey, Transaction transaction, SafeTransactionHandle txHandle)
		{
			this.hkey = hkey;
			this.keyName = "";
			if (systemkey)
			{
				this.state |= 2;
			}
			if (writable)
			{
				this.state |= 4;
			}
			if (null != transaction)
			{
				this.myTransaction = transaction.Clone();
				this.myTransactionHandle = txHandle;
				return;
			}
			this.myTransaction = null;
			this.myTransactionHandle = null;
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x00195B44 File Offset: 0x00193D44
		private SafeTransactionHandle GetTransactionHandle()
		{
			SafeTransactionHandle result;
			if (null != this.myTransaction)
			{
				if (!this.myTransaction.Equals(Transaction.Current))
				{
					throw new InvalidOperationException(RegistryProviderStrings.InvalidOperation_MustUseSameTransaction);
				}
				result = this.myTransactionHandle;
			}
			else
			{
				result = SafeTransactionHandle.Create();
			}
			return result;
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x00195B8E File Offset: 0x00193D8E
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x00195B98 File Offset: 0x00193D98
		private void Dispose(bool disposing)
		{
			if (this.hkey != null && !this.IsSystemKey())
			{
				try
				{
					this.hkey.Dispose();
				}
				catch (IOException)
				{
				}
				finally
				{
					this.hkey = null;
				}
			}
			if (null != this.myTransaction)
			{
				try
				{
					this.myTransaction.Dispose();
				}
				catch (TransactionException)
				{
				}
				finally
				{
					this.myTransaction = null;
				}
			}
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x00195C2C File Offset: 0x00193E2C
		public void Flush()
		{
			this.VerifyTransaction();
			if (this.hkey != null && this.IsDirty())
			{
				int num = Win32Native.RegFlushKey(this.hkey);
				if (num != 0)
				{
					throw new IOException(Win32Native.GetMessage(num), num);
				}
			}
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x00195C6B File Offset: 0x00193E6B
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x00195C74 File Offset: 0x00193E74
		public TransactedRegistryKey CreateSubKey(string subkey)
		{
			return this.CreateSubKey(subkey, this.checkMode);
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x00195C83 File Offset: 0x00193E83
		[ComVisible(false)]
		public TransactedRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
		{
			return this.CreateSubKeyInternal(subkey, permissionCheck, null);
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x00195C8E File Offset: 0x00193E8E
		[ComVisible(false)]
		public TransactedRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, TransactedRegistrySecurity registrySecurity)
		{
			return this.CreateSubKeyInternal(subkey, permissionCheck, registrySecurity);
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x00195C9C File Offset: 0x00193E9C
		[ComVisible(false)]
		private unsafe TransactedRegistryKey CreateSubKeyInternal(string subkey, RegistryKeyPermissionCheck permissionCheck, object registrySecurityObj)
		{
			TransactedRegistryKey.ValidateKeyName(subkey);
			if (string.Empty == subkey)
			{
				throw new ArgumentException(RegistryProviderStrings.Arg_RegKeyStrEmpty);
			}
			TransactedRegistryKey.ValidateKeyMode(permissionCheck);
			this.EnsureWriteable();
			subkey = TransactedRegistryKey.FixupName(subkey);
			TransactedRegistryKey transactedRegistryKey = this.InternalOpenSubKey(subkey, permissionCheck != RegistryKeyPermissionCheck.ReadSubTree);
			if (transactedRegistryKey != null)
			{
				this.CheckSubKeyWritePermission(subkey);
				this.CheckSubTreePermission(subkey, permissionCheck);
				transactedRegistryKey.checkMode = permissionCheck;
				return transactedRegistryKey;
			}
			this.CheckSubKeyCreatePermission(subkey);
			Win32Native.SECURITY_ATTRIBUTES security_ATTRIBUTES = null;
			TransactedRegistrySecurity transactedRegistrySecurity = registrySecurityObj as TransactedRegistrySecurity;
			if (transactedRegistrySecurity != null)
			{
				security_ATTRIBUTES = new Win32Native.SECURITY_ATTRIBUTES();
				security_ATTRIBUTES.nLength = Marshal.SizeOf(security_ATTRIBUTES);
				byte[] securityDescriptorBinaryForm = transactedRegistrySecurity.GetSecurityDescriptorBinaryForm();
				byte* ptr = stackalloc byte[(UIntPtr)securityDescriptorBinaryForm.Length];
				Buffer.memcpy(securityDescriptorBinaryForm, 0, ptr, 0, securityDescriptorBinaryForm.Length);
				security_ATTRIBUTES.pSecurityDescriptor = ptr;
			}
			int num = 0;
			SafeRegistryHandle safeRegistryHandle = null;
			SafeTransactionHandle transactionHandle = this.GetTransactionHandle();
			int num2 = Win32Native.RegCreateKeyTransacted(this.hkey, subkey, 0, null, 0, TransactedRegistryKey.GetRegistryKeyAccess(permissionCheck != RegistryKeyPermissionCheck.ReadSubTree), security_ATTRIBUTES, out safeRegistryHandle, out num, transactionHandle, IntPtr.Zero);
			if (num2 == 0 && !safeRegistryHandle.IsInvalid)
			{
				TransactedRegistryKey transactedRegistryKey2 = new TransactedRegistryKey(safeRegistryHandle, permissionCheck != RegistryKeyPermissionCheck.ReadSubTree, false, Transaction.Current, transactionHandle);
				this.CheckSubTreePermission(subkey, permissionCheck);
				transactedRegistryKey2.checkMode = permissionCheck;
				if (subkey.Length == 0)
				{
					transactedRegistryKey2.keyName = this.keyName;
				}
				else
				{
					transactedRegistryKey2.keyName = this.keyName + "\\" + subkey;
				}
				return transactedRegistryKey2;
			}
			if (num2 != 0)
			{
				this.Win32Error(num2, this.keyName + "\\" + subkey);
			}
			return null;
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x00195E0D File Offset: 0x0019400D
		public void DeleteSubKey(string subkey)
		{
			this.DeleteSubKey(subkey, true);
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x00195E18 File Offset: 0x00194018
		public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
		{
			TransactedRegistryKey.ValidateKeyName(subkey);
			this.EnsureWriteable();
			subkey = TransactedRegistryKey.FixupName(subkey);
			this.CheckSubKeyWritePermission(subkey);
			TransactedRegistryKey transactedRegistryKey = this.InternalOpenSubKey(subkey, false);
			if (transactedRegistryKey != null)
			{
				try
				{
					if (transactedRegistryKey.InternalSubKeyCount() > 0)
					{
						throw new InvalidOperationException(RegistryProviderStrings.InvalidOperation_RegRemoveSubKey);
					}
				}
				finally
				{
					transactedRegistryKey.Close();
				}
				SafeTransactionHandle transactionHandle = this.GetTransactionHandle();
				int num = Win32Native.RegDeleteKeyTransacted(this.hkey, subkey, 0, 0U, transactionHandle, IntPtr.Zero);
				if (num != 0)
				{
					if (num != 2)
					{
						this.Win32Error(num, null);
						return;
					}
					if (throwOnMissingSubKey)
					{
						throw new ArgumentException(RegistryProviderStrings.ArgumentException_RegSubKeyAbsent);
					}
				}
			}
			else if (throwOnMissingSubKey)
			{
				throw new ArgumentException(RegistryProviderStrings.ArgumentException_RegSubKeyAbsent);
			}
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x00195EC4 File Offset: 0x001940C4
		public void DeleteSubKeyTree(string subkey)
		{
			TransactedRegistryKey.ValidateKeyName(subkey);
			if ((string.IsNullOrEmpty(subkey) || subkey.Length == 0) && this.IsSystemKey())
			{
				throw new ArgumentException(RegistryProviderStrings.ArgRegKeyDelHive);
			}
			this.EnsureWriteable();
			SafeTransactionHandle transactionHandle = this.GetTransactionHandle();
			subkey = TransactedRegistryKey.FixupName(subkey);
			this.CheckSubTreeWritePermission(subkey);
			TransactedRegistryKey transactedRegistryKey = this.InternalOpenSubKey(subkey, true);
			if (transactedRegistryKey == null)
			{
				throw new ArgumentException(RegistryProviderStrings.Arg_RegSubKeyAbsent);
			}
			try
			{
				if (transactedRegistryKey.InternalSubKeyCount() > 0)
				{
					string[] array = transactedRegistryKey.InternalGetSubKeyNames();
					for (int i = 0; i < array.Length; i++)
					{
						transactedRegistryKey.DeleteSubKeyTreeInternal(array[i]);
					}
				}
			}
			finally
			{
				transactedRegistryKey.Close();
			}
			int num = Win32Native.RegDeleteKeyTransacted(this.hkey, subkey, 0, 0U, transactionHandle, IntPtr.Zero);
			if (num != 0)
			{
				this.Win32Error(num, null);
				return;
			}
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x00195F98 File Offset: 0x00194198
		private void DeleteSubKeyTreeInternal(string subkey)
		{
			SafeTransactionHandle transactionHandle = this.GetTransactionHandle();
			TransactedRegistryKey transactedRegistryKey = this.InternalOpenSubKey(subkey, true);
			if (transactedRegistryKey == null)
			{
				throw new ArgumentException(RegistryProviderStrings.Arg_RegSubKeyAbsent);
			}
			try
			{
				if (transactedRegistryKey.InternalSubKeyCount() > 0)
				{
					string[] array = transactedRegistryKey.InternalGetSubKeyNames();
					for (int i = 0; i < array.Length; i++)
					{
						transactedRegistryKey.DeleteSubKeyTreeInternal(array[i]);
					}
				}
			}
			finally
			{
				transactedRegistryKey.Close();
			}
			int num = Win32Native.RegDeleteKeyTransacted(this.hkey, subkey, 0, 0U, transactionHandle, IntPtr.Zero);
			if (num != 0)
			{
				this.Win32Error(num, null);
				return;
			}
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x0019602C File Offset: 0x0019422C
		public void DeleteValue(string name)
		{
			this.DeleteValue(name, true);
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x00196038 File Offset: 0x00194238
		public void DeleteValue(string name, bool throwOnMissingValue)
		{
			this.EnsureWriteable();
			this.CheckValueWritePermission(name);
			this.VerifyTransaction();
			int num = Win32Native.RegDeleteValue(this.hkey, name);
			if (num == 2 || num == 206)
			{
				if (throwOnMissingValue)
				{
					throw new ArgumentException(RegistryProviderStrings.Arg_RegSubKeyValueAbsent);
				}
				num = 0;
			}
			if (num != 0)
			{
				this.Win32Error(num, null);
			}
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x0019608C File Offset: 0x0019428C
		internal static TransactedRegistryKey GetBaseKey(IntPtr hKey)
		{
			int num = (int)hKey & 268435455;
			SafeRegistryHandle safeRegistryHandle = new SafeRegistryHandle(hKey, false);
			return new TransactedRegistryKey(safeRegistryHandle, true, true, null, null)
			{
				checkMode = RegistryKeyPermissionCheck.Default,
				keyName = TransactedRegistryKey.hkeyNames[num]
			};
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x001960D0 File Offset: 0x001942D0
		public TransactedRegistryKey OpenSubKey(string name, bool writable)
		{
			TransactedRegistryKey.ValidateKeyName(name);
			this.EnsureNotDisposed();
			name = TransactedRegistryKey.FixupName(name);
			this.CheckOpenSubKeyPermission(name, writable);
			SafeRegistryHandle safeRegistryHandle = null;
			SafeTransactionHandle transactionHandle = this.GetTransactionHandle();
			int num = this.RegOpenKeyTransactedWrapper(this.hkey, name, 0, TransactedRegistryKey.GetRegistryKeyAccess(writable), out safeRegistryHandle, transactionHandle, IntPtr.Zero);
			if (num == 0 && !safeRegistryHandle.IsInvalid)
			{
				return new TransactedRegistryKey(safeRegistryHandle, writable, false, Transaction.Current, transactionHandle)
				{
					checkMode = this.GetSubKeyPermissonCheck(writable),
					keyName = this.keyName + "\\" + name
				};
			}
			if (num == 5 || num == 1346)
			{
				throw new SecurityException(RegistryProviderStrings.Security_RegistryPermission);
			}
			return null;
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x00196179 File Offset: 0x00194379
		[ComVisible(false)]
		public TransactedRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
		{
			TransactedRegistryKey.ValidateKeyMode(permissionCheck);
			return this.InternalOpenSubKey(name, permissionCheck, TransactedRegistryKey.GetRegistryKeyAccess(permissionCheck));
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x0019618F File Offset: 0x0019438F
		[ComVisible(false)]
		public TransactedRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
		{
			return this.InternalOpenSubKey(name, permissionCheck, (int)rights);
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x0019619C File Offset: 0x0019439C
		private TransactedRegistryKey InternalOpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, int rights)
		{
			TransactedRegistryKey.ValidateKeyName(name);
			TransactedRegistryKey.ValidateKeyMode(permissionCheck);
			TransactedRegistryKey.ValidateKeyRights(rights);
			this.EnsureNotDisposed();
			name = TransactedRegistryKey.FixupName(name);
			this.CheckOpenSubKeyPermission(name, permissionCheck);
			SafeRegistryHandle safeRegistryHandle = null;
			SafeTransactionHandle transactionHandle = this.GetTransactionHandle();
			int num = this.RegOpenKeyTransactedWrapper(this.hkey, name, 0, rights, out safeRegistryHandle, transactionHandle, IntPtr.Zero);
			if (num == 0 && !safeRegistryHandle.IsInvalid)
			{
				return new TransactedRegistryKey(safeRegistryHandle, permissionCheck == RegistryKeyPermissionCheck.ReadWriteSubTree, false, Transaction.Current, transactionHandle)
				{
					keyName = this.keyName + "\\" + name,
					checkMode = permissionCheck
				};
			}
			if (num == 5 || num == 1346)
			{
				throw new SecurityException(RegistryProviderStrings.Security_RegistryPermission);
			}
			return null;
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x0019624C File Offset: 0x0019444C
		internal TransactedRegistryKey InternalOpenSubKey(string name, bool writable)
		{
			TransactedRegistryKey.ValidateKeyName(name);
			this.EnsureNotDisposed();
			int registryKeyAccess = TransactedRegistryKey.GetRegistryKeyAccess(writable);
			SafeRegistryHandle safeRegistryHandle = null;
			SafeTransactionHandle transactionHandle = this.GetTransactionHandle();
			if (this.RegOpenKeyTransactedWrapper(this.hkey, name, 0, registryKeyAccess, out safeRegistryHandle, transactionHandle, IntPtr.Zero) == 0 && !safeRegistryHandle.IsInvalid)
			{
				return new TransactedRegistryKey(safeRegistryHandle, writable, false, Transaction.Current, transactionHandle)
				{
					keyName = this.keyName + "\\" + name
				};
			}
			return null;
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x001962C6 File Offset: 0x001944C6
		public TransactedRegistryKey OpenSubKey(string name)
		{
			return this.OpenSubKey(name, false);
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06004D08 RID: 19720 RVA: 0x001962D0 File Offset: 0x001944D0
		public int SubKeyCount
		{
			get
			{
				this.CheckKeyReadPermission();
				return this.InternalSubKeyCount();
			}
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x001962E0 File Offset: 0x001944E0
		internal int InternalSubKeyCount()
		{
			this.EnsureNotDisposed();
			int result = 0;
			int num = 0;
			int num2 = Win32Native.RegQueryInfoKey(this.hkey, null, null, Win32Native.NULL, ref result, null, null, ref num, null, null, null, null);
			if (num2 != 0)
			{
				this.Win32Error(num2, null);
			}
			return result;
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x00196320 File Offset: 0x00194520
		public string[] GetSubKeyNames()
		{
			this.CheckKeyReadPermission();
			return this.InternalGetSubKeyNames();
		}

		// Token: 0x06004D0B RID: 19723 RVA: 0x00196330 File Offset: 0x00194530
		internal string[] InternalGetSubKeyNames()
		{
			this.EnsureNotDisposed();
			int num = this.InternalSubKeyCount();
			string[] array = new string[num];
			if (num > 0)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				for (int i = 0; i < num; i++)
				{
					int capacity = stringBuilder.Capacity;
					int num2 = Win32Native.RegEnumKeyEx(this.hkey, i, stringBuilder, out capacity, null, null, null, null);
					if (num2 != 0)
					{
						this.Win32Error(num2, null);
					}
					array[i] = stringBuilder.ToString();
				}
			}
			return array;
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x06004D0C RID: 19724 RVA: 0x001963A5 File Offset: 0x001945A5
		public int ValueCount
		{
			get
			{
				this.CheckKeyReadPermission();
				return this.InternalValueCount();
			}
		}

		// Token: 0x06004D0D RID: 19725 RVA: 0x001963B4 File Offset: 0x001945B4
		internal int InternalValueCount()
		{
			this.EnsureNotDisposed();
			int result = 0;
			int num = 0;
			int num2 = Win32Native.RegQueryInfoKey(this.hkey, null, null, Win32Native.NULL, ref num, null, null, ref result, null, null, null, null);
			if (num2 != 0)
			{
				this.Win32Error(num2, null);
			}
			return result;
		}

		// Token: 0x06004D0E RID: 19726 RVA: 0x001963F4 File Offset: 0x001945F4
		public string[] GetValueNames()
		{
			this.CheckKeyReadPermission();
			this.EnsureNotDisposed();
			int num = this.InternalValueCount();
			string[] array = new string[num];
			if (num > 0)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				for (int i = 0; i < num; i++)
				{
					int num2 = stringBuilder.Capacity;
					int num3 = 234;
					while (234 == num3)
					{
						int num4 = num2;
						num3 = Win32Native.RegEnumValue(this.hkey, i, stringBuilder, ref num4, Win32Native.NULL, null, null, null);
						if (num3 != 0)
						{
							if (num3 != 234)
							{
								this.Win32Error(num3, null);
							}
							if (16383 == num2)
							{
								this.Win32Error(num3, null);
							}
							num2 *= 2;
							if (16383 < num2)
							{
								num2 = 16383;
							}
							stringBuilder = new StringBuilder(num2);
						}
					}
					array[i] = stringBuilder.ToString();
				}
			}
			return array;
		}

		// Token: 0x06004D0F RID: 19727 RVA: 0x001964CB File Offset: 0x001946CB
		public object GetValue(string name)
		{
			this.CheckValueReadPermission(name);
			return this.InternalGetValue(name, null, false, true);
		}

		// Token: 0x06004D10 RID: 19728 RVA: 0x001964DE File Offset: 0x001946DE
		public object GetValue(string name, object defaultValue)
		{
			this.CheckValueReadPermission(name);
			return this.InternalGetValue(name, defaultValue, false, true);
		}

		// Token: 0x06004D11 RID: 19729 RVA: 0x001964F4 File Offset: 0x001946F4
		[ComVisible(false)]
		public object GetValue(string name, object defaultValue, RegistryValueOptions options)
		{
			if (options < RegistryValueOptions.None || options > RegistryValueOptions.DoNotExpandEnvironmentNames)
			{
				string arg_EnumIllegalVal = RegistryProviderStrings.Arg_EnumIllegalVal;
				string message = string.Format(CultureInfo.CurrentCulture, arg_EnumIllegalVal, new object[]
				{
					options.ToString()
				});
				throw new ArgumentException(message);
			}
			bool doNotExpand = options == RegistryValueOptions.DoNotExpandEnvironmentNames;
			this.CheckValueReadPermission(name);
			return this.InternalGetValue(name, defaultValue, doNotExpand, true);
		}

		// Token: 0x06004D12 RID: 19730 RVA: 0x00196550 File Offset: 0x00194750
		internal object InternalGetValue(string name, object defaultValue, bool doNotExpand, bool checkSecurity)
		{
			if (checkSecurity)
			{
				this.EnsureNotDisposed();
			}
			object obj = defaultValue;
			int num = 0;
			int num2 = 0;
			int num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, null, ref num2);
			if (num3 != 0 && num3 != 234)
			{
				return obj;
			}
			switch (num)
			{
			case 0:
			case 6:
			case 8:
			case 9:
			case 10:
				return obj;
			case 1:
			{
				StringBuilder stringBuilder = new StringBuilder(num2 / 2);
				num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, stringBuilder, ref num2);
				return stringBuilder.ToString();
			}
			case 2:
			{
				StringBuilder stringBuilder2 = new StringBuilder(num2 / 2);
				num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, stringBuilder2, ref num2);
				if (doNotExpand)
				{
					return stringBuilder2.ToString();
				}
				return Environment.ExpandEnvironmentVariables(stringBuilder2.ToString());
			}
			case 3:
			case 5:
				break;
			case 4:
				if (num2 <= 4)
				{
					int num4 = 0;
					num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, ref num4, ref num2);
					return num4;
				}
				goto IL_93;
			case 7:
			{
				IList<string> list = new List<string>();
				char[] array = new char[num2 / 2];
				num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, array, ref num2);
				int num5 = 0;
				int num6 = array.Length;
				while (num3 == 0 && num5 < num6)
				{
					int num7 = num5;
					while (num7 < num6 && array[num7] != '\0')
					{
						num7++;
					}
					if (num7 < num6)
					{
						if (num7 - num5 > 0)
						{
							list.Add(new string(array, num5, num7 - num5));
						}
						else if (num7 != num6 - 1)
						{
							list.Add(string.Empty);
						}
					}
					else
					{
						list.Add(new string(array, num5, num6 - num5));
					}
					num5 = num7 + 1;
				}
				obj = new string[list.Count];
				list.CopyTo((string[])obj, 0);
				return obj;
			}
			case 11:
				goto IL_93;
			default:
				return obj;
			}
			IL_6F:
			byte[] array2 = new byte[num2];
			num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, array2, ref num2);
			return array2;
			IL_93:
			if (num2 > 8)
			{
				goto IL_6F;
			}
			long num8 = 0L;
			num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, ref num8, ref num2);
			obj = num8;
			return obj;
		}

		// Token: 0x06004D13 RID: 19731 RVA: 0x00196770 File Offset: 0x00194970
		[ComVisible(false)]
		public RegistryValueKind GetValueKind(string name)
		{
			this.CheckValueReadPermission(name);
			this.EnsureNotDisposed();
			int num = 0;
			int num2 = 0;
			int num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, null, ref num2);
			if (num3 != 0)
			{
				this.Win32Error(num3, null);
			}
			if (!Enum.IsDefined(typeof(RegistryValueKind), num))
			{
				return RegistryValueKind.Unknown;
			}
			return (RegistryValueKind)num;
		}

		// Token: 0x06004D14 RID: 19732 RVA: 0x001967C6 File Offset: 0x001949C6
		private bool IsDirty()
		{
			return (this.state & 1) != 0;
		}

		// Token: 0x06004D15 RID: 19733 RVA: 0x001967D6 File Offset: 0x001949D6
		private bool IsSystemKey()
		{
			return (this.state & 2) != 0;
		}

		// Token: 0x06004D16 RID: 19734 RVA: 0x001967E6 File Offset: 0x001949E6
		private bool IsWritable()
		{
			return (this.state & 4) != 0;
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x06004D17 RID: 19735 RVA: 0x001967F6 File Offset: 0x001949F6
		public string Name
		{
			get
			{
				this.EnsureNotDisposed();
				return this.keyName;
			}
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x00196804 File Offset: 0x00194A04
		private void SetDirty()
		{
			this.state |= 1;
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x00196814 File Offset: 0x00194A14
		public void SetValue(string name, object value)
		{
			this.SetValue(name, value, RegistryValueKind.Unknown);
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x00196820 File Offset: 0x00194A20
		[ComVisible(false)]
		public void SetValue(string name, object value, RegistryValueKind valueKind)
		{
			if (value == null)
			{
				throw new ArgumentNullException(RegistryProviderStrings.Arg_Value);
			}
			if (name != null && name.Length > 16383)
			{
				throw new ArgumentException(RegistryProviderStrings.Arg_RegValueNameStrLenBug);
			}
			if (!Enum.IsDefined(typeof(RegistryValueKind), valueKind))
			{
				throw new ArgumentException(RegistryProviderStrings.Arg_RegBadKeyKind);
			}
			this.EnsureWriteable();
			this.VerifyTransaction();
			if (this.ContainsRegistryValue(name))
			{
				this.CheckValueWritePermission(name);
			}
			else
			{
				this.CheckValueCreatePermission(name);
			}
			if (valueKind == RegistryValueKind.Unknown)
			{
				valueKind = this.CalculateValueKind(value);
			}
			int num = 0;
			try
			{
				switch (valueKind)
				{
				case RegistryValueKind.String:
				case RegistryValueKind.ExpandString:
				{
					string text = value.ToString();
					if (524288 < text.Length)
					{
						throw new ArgumentException(RegistryProviderStrings.Arg_ValueDataLenBug);
					}
					num = Win32Native.RegSetValueEx(this.hkey, name, 0, valueKind, text, text.Length * 2 + 2);
					break;
				}
				case RegistryValueKind.Binary:
				{
					byte[] array = (byte[])value;
					if (1048576 < array.Length)
					{
						throw new ArgumentException(RegistryProviderStrings.Arg_ValueDataLenBug);
					}
					num = Win32Native.RegSetValueEx(this.hkey, name, 0, RegistryValueKind.Binary, array, array.Length);
					break;
				}
				case RegistryValueKind.DWord:
				{
					int num2 = Convert.ToInt32(value, CultureInfo.InvariantCulture);
					num = Win32Native.RegSetValueEx(this.hkey, name, 0, RegistryValueKind.DWord, ref num2, 4);
					break;
				}
				case RegistryValueKind.MultiString:
				{
					string[] array2 = (string[])((string[])value).Clone();
					int num3 = 0;
					for (int i = 0; i < array2.Length; i++)
					{
						if (array2[i] == null)
						{
							throw new ArgumentException(RegistryProviderStrings.Arg_RegSetStrArrNull);
						}
						num3 += (array2[i].Length + 1) * 2;
					}
					num3 += 2;
					if (1048576 < num3)
					{
						throw new ArgumentException(RegistryProviderStrings.Arg_ValueDataLenBug);
					}
					byte[] array3 = new byte[num3];
					byte[] array4;
					if ((array4 = array3) != null)
					{
						int num4 = array4.Length;
					}
					int num5 = 0;
					for (int j = 0; j < array2.Length; j++)
					{
						int bytes = Encoding.Unicode.GetBytes(array2[j], 0, array2[j].Length, array3, num5);
						num5 += bytes;
						array3[num5] = 0;
						array3[num5 + 1] = 0;
						num5 += 2;
					}
					num = Win32Native.RegSetValueEx(this.hkey, name, 0, RegistryValueKind.MultiString, array3, num3);
					break;
				}
				case RegistryValueKind.QWord:
				{
					long num6 = Convert.ToInt64(value, CultureInfo.InvariantCulture);
					num = Win32Native.RegSetValueEx(this.hkey, name, 0, RegistryValueKind.QWord, ref num6, 8);
					break;
				}
				}
			}
			catch (OverflowException)
			{
				throw new ArgumentException(RegistryProviderStrings.Arg_RegSetMismatchedKind);
			}
			catch (InvalidOperationException)
			{
				throw new ArgumentException(RegistryProviderStrings.Arg_RegSetMismatchedKind);
			}
			catch (FormatException)
			{
				throw new ArgumentException(RegistryProviderStrings.Arg_RegSetMismatchedKind);
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(RegistryProviderStrings.Arg_RegSetMismatchedKind);
			}
			if (num == 0)
			{
				this.SetDirty();
				return;
			}
			this.Win32Error(num, null);
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x00196B1C File Offset: 0x00194D1C
		private RegistryValueKind CalculateValueKind(object value)
		{
			if (value is int)
			{
				return RegistryValueKind.DWord;
			}
			if (!(value is Array))
			{
				return RegistryValueKind.String;
			}
			if (value is byte[])
			{
				return RegistryValueKind.Binary;
			}
			if (value is string[])
			{
				return RegistryValueKind.MultiString;
			}
			string arg_RegSetBadArrType = RegistryProviderStrings.Arg_RegSetBadArrType;
			string message = string.Format(CultureInfo.CurrentCulture, arg_RegSetBadArrType, new object[]
			{
				value.GetType().Name
			});
			throw new ArgumentException(message);
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x00196B7F File Offset: 0x00194D7F
		public override string ToString()
		{
			this.EnsureNotDisposed();
			return this.keyName;
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x00196B8D File Offset: 0x00194D8D
		public TransactedRegistrySecurity GetAccessControl()
		{
			return this.GetAccessControl(AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group);
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x00196B97 File Offset: 0x00194D97
		public TransactedRegistrySecurity GetAccessControl(AccessControlSections includeSections)
		{
			this.EnsureNotDisposed();
			return new TransactedRegistrySecurity(this.hkey, this.keyName, includeSections);
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x00196BB1 File Offset: 0x00194DB1
		public void SetAccessControl(TransactedRegistrySecurity registrySecurity)
		{
			this.EnsureWriteable();
			if (registrySecurity == null)
			{
				throw new ArgumentNullException("registrySecurity");
			}
			this.VerifyTransaction();
			registrySecurity.Persist(this.hkey, this.keyName);
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x00196BE0 File Offset: 0x00194DE0
		internal void Win32Error(int errorCode, string str)
		{
			switch (errorCode)
			{
			case 2:
			{
				string arg_RegKeyNotFound = RegistryProviderStrings.Arg_RegKeyNotFound;
				string message = string.Format(CultureInfo.CurrentCulture, arg_RegKeyNotFound, new object[]
				{
					errorCode.ToString(CultureInfo.InvariantCulture)
				});
				throw new IOException(message);
			}
			case 5:
				if (str != null)
				{
					string unauthorizedAccess_RegistryKeyGeneric_Key = RegistryProviderStrings.UnauthorizedAccess_RegistryKeyGeneric_Key;
					string message2 = string.Format(CultureInfo.CurrentCulture, unauthorizedAccess_RegistryKeyGeneric_Key, new object[]
					{
						str
					});
					throw new UnauthorizedAccessException(message2);
				}
				throw new UnauthorizedAccessException();
			case 6:
				this.hkey.SetHandleAsInvalid();
				this.hkey = null;
				break;
			}
			throw new IOException(Win32Native.GetMessage(errorCode), errorCode);
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x00196C94 File Offset: 0x00194E94
		internal static void Win32ErrorStatic(int errorCode, string str)
		{
			if (errorCode != 5)
			{
				throw new IOException(Win32Native.GetMessage(errorCode), errorCode);
			}
			if (str != null)
			{
				string unauthorizedAccess_RegistryKeyGeneric_Key = RegistryProviderStrings.UnauthorizedAccess_RegistryKeyGeneric_Key;
				string message = string.Format(CultureInfo.CurrentCulture, unauthorizedAccess_RegistryKeyGeneric_Key, new object[]
				{
					str
				});
				throw new UnauthorizedAccessException(message);
			}
			throw new UnauthorizedAccessException();
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x00196CE4 File Offset: 0x00194EE4
		internal static string FixupName(string name)
		{
			if (name.IndexOf('\\') == -1)
			{
				return name;
			}
			StringBuilder stringBuilder = new StringBuilder(name);
			TransactedRegistryKey.FixupPath(stringBuilder);
			int num = stringBuilder.Length - 1;
			if (stringBuilder[num] == '\\')
			{
				stringBuilder.Length = num;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x00196D2C File Offset: 0x00194F2C
		private static void FixupPath(StringBuilder path)
		{
			int length = path.Length;
			bool flag = false;
			char maxValue = char.MaxValue;
			for (int i = 1; i < length - 1; i++)
			{
				if (path[i] == '\\')
				{
					i++;
					while (i < length && path[i] == '\\')
					{
						path[i] = maxValue;
						i++;
						flag = true;
					}
				}
			}
			if (flag)
			{
				int i = 0;
				int num = 0;
				while (i < length)
				{
					if (path[i] == maxValue)
					{
						i++;
					}
					else
					{
						path[num] = path[i];
						i++;
						num++;
					}
				}
				path.Length += num - i;
			}
		}

		// Token: 0x06004D24 RID: 19748 RVA: 0x00196DCC File Offset: 0x00194FCC
		private void CheckOpenSubKeyPermission(string subkeyName, bool subKeyWritable)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				this.CheckSubKeyReadPermission(subkeyName);
			}
			if (subKeyWritable && this.checkMode == RegistryKeyPermissionCheck.ReadSubTree)
			{
				this.CheckSubTreeReadWritePermission(subkeyName);
			}
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x00196DF0 File Offset: 0x00194FF0
		private void CheckOpenSubKeyPermission(string subkeyName, RegistryKeyPermissionCheck subKeyCheck)
		{
			if (subKeyCheck == RegistryKeyPermissionCheck.Default && this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				this.CheckSubKeyReadPermission(subkeyName);
			}
			this.CheckSubTreePermission(subkeyName, subKeyCheck);
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x00196E0C File Offset: 0x0019500C
		private void CheckSubTreePermission(string subkeyName, RegistryKeyPermissionCheck subKeyCheck)
		{
			if (subKeyCheck == RegistryKeyPermissionCheck.ReadSubTree)
			{
				if (this.checkMode == RegistryKeyPermissionCheck.Default)
				{
					this.CheckSubTreeReadPermission(subkeyName);
					return;
				}
			}
			else if (subKeyCheck == RegistryKeyPermissionCheck.ReadWriteSubTree && this.checkMode != RegistryKeyPermissionCheck.ReadWriteSubTree)
			{
				this.CheckSubTreeReadWritePermission(subkeyName);
			}
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x00196E36 File Offset: 0x00195036
		private void CheckSubKeyWritePermission(string subkeyName)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Write, this.keyName + "\\" + subkeyName + "\\.").Demand();
			}
		}

		// Token: 0x06004D28 RID: 19752 RVA: 0x00196E61 File Offset: 0x00195061
		private void CheckSubKeyReadPermission(string subkeyName)
		{
			new RegistryPermission(RegistryPermissionAccess.Read, this.keyName + "\\" + subkeyName + "\\.").Demand();
		}

		// Token: 0x06004D29 RID: 19753 RVA: 0x00196E84 File Offset: 0x00195084
		private void CheckSubKeyCreatePermission(string subkeyName)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Create, this.keyName + "\\" + subkeyName + "\\.").Demand();
			}
		}

		// Token: 0x06004D2A RID: 19754 RVA: 0x00196EAF File Offset: 0x001950AF
		private void CheckSubTreeReadPermission(string subkeyName)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Read, this.keyName + "\\" + subkeyName + "\\").Demand();
			}
		}

		// Token: 0x06004D2B RID: 19755 RVA: 0x00196EDA File Offset: 0x001950DA
		private void CheckSubTreeWritePermission(string subkeyName)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Write, this.keyName + "\\" + subkeyName + "\\").Demand();
			}
		}

		// Token: 0x06004D2C RID: 19756 RVA: 0x00196F05 File Offset: 0x00195105
		private void CheckSubTreeReadWritePermission(string subkeyName)
		{
			new RegistryPermission(RegistryPermissionAccess.Read | RegistryPermissionAccess.Write, this.keyName + "\\" + subkeyName).Demand();
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x00196F23 File Offset: 0x00195123
		private void CheckValueWritePermission(string valueName)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Write, this.keyName + "\\" + valueName).Demand();
			}
		}

		// Token: 0x06004D2E RID: 19758 RVA: 0x00196F49 File Offset: 0x00195149
		private void CheckValueCreatePermission(string valueName)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Create, this.keyName + "\\" + valueName).Demand();
			}
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x00196F6F File Offset: 0x0019516F
		private void CheckValueReadPermission(string valueName)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Read, this.keyName + "\\" + valueName).Demand();
			}
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x00196F95 File Offset: 0x00195195
		private void CheckKeyReadPermission()
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				new RegistryPermission(RegistryPermissionAccess.Read, this.keyName + "\\.").Demand();
			}
		}

		// Token: 0x06004D31 RID: 19761 RVA: 0x00196FBC File Offset: 0x001951BC
		private bool ContainsRegistryValue(string name)
		{
			int num = 0;
			int num2 = 0;
			int num3 = Win32Native.RegQueryValueEx(this.hkey, name, null, ref num, null, ref num2);
			return num3 == 0;
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x00196FE4 File Offset: 0x001951E4
		private void EnsureNotDisposed()
		{
			if (this.hkey == null)
			{
				throw new ObjectDisposedException(this.keyName, RegistryProviderStrings.ObjectDisposed_RegKeyClosed);
			}
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x00196FFF File Offset: 0x001951FF
		private void EnsureWriteable()
		{
			this.EnsureNotDisposed();
			if (!this.IsWritable())
			{
				throw new UnauthorizedAccessException(RegistryProviderStrings.UnauthorizedAccess_RegistryNoWrite);
			}
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x0019701C File Offset: 0x0019521C
		private static int GetRegistryKeyAccess(bool isWritable)
		{
			int result;
			if (!isWritable)
			{
				result = 131097;
			}
			else
			{
				result = 131103;
			}
			return result;
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x0019703C File Offset: 0x0019523C
		private static int GetRegistryKeyAccess(RegistryKeyPermissionCheck mode)
		{
			int result = 0;
			switch (mode)
			{
			case RegistryKeyPermissionCheck.Default:
			case RegistryKeyPermissionCheck.ReadSubTree:
				result = 131097;
				break;
			case RegistryKeyPermissionCheck.ReadWriteSubTree:
				result = 131103;
				break;
			}
			return result;
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x00197070 File Offset: 0x00195270
		private RegistryKeyPermissionCheck GetSubKeyPermissonCheck(bool subkeyWritable)
		{
			if (this.checkMode == RegistryKeyPermissionCheck.Default)
			{
				return this.checkMode;
			}
			if (subkeyWritable)
			{
				return RegistryKeyPermissionCheck.ReadWriteSubTree;
			}
			return RegistryKeyPermissionCheck.ReadSubTree;
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x00197088 File Offset: 0x00195288
		private static void ValidateKeyName(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException(RegistryProviderStrings.Arg_Name);
			}
			int num = name.IndexOf("\\", StringComparison.OrdinalIgnoreCase);
			int num2 = 0;
			while (num != -1)
			{
				if (num - num2 > 255)
				{
					throw new ArgumentException(RegistryProviderStrings.Arg_RegKeyStrLenBug);
				}
				num2 = num + 1;
				num = name.IndexOf("\\", num2, StringComparison.OrdinalIgnoreCase);
			}
			if (name.Length - num2 > 255)
			{
				throw new ArgumentException(RegistryProviderStrings.Arg_RegKeyStrLenBug);
			}
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x001970F9 File Offset: 0x001952F9
		private static void ValidateKeyMode(RegistryKeyPermissionCheck mode)
		{
			if (mode < RegistryKeyPermissionCheck.Default || mode > RegistryKeyPermissionCheck.ReadWriteSubTree)
			{
				throw new ArgumentException(RegistryProviderStrings.Argument_InvalidRegistryKeyPermissionCheck);
			}
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x0019710E File Offset: 0x0019530E
		private static void ValidateKeyRights(int rights)
		{
			if ((rights & -983104) != 0)
			{
				throw new SecurityException(RegistryProviderStrings.Security_RegistryPermission);
			}
		}

		// Token: 0x06004D3A RID: 19770 RVA: 0x00197124 File Offset: 0x00195324
		private void VerifyTransaction()
		{
			if (null == this.myTransaction)
			{
				throw new InvalidOperationException(RegistryProviderStrings.InvalidOperation_NotAssociatedWithTransaction);
			}
			if (!this.myTransaction.Equals(Transaction.Current))
			{
				throw new InvalidOperationException(RegistryProviderStrings.InvalidOperation_MustUseSameTransaction);
			}
		}

		// Token: 0x0400265E RID: 9822
		private const string resBaseName = "RegistryProviderStrings";

		// Token: 0x0400265F RID: 9823
		private const int STATE_DIRTY = 1;

		// Token: 0x04002660 RID: 9824
		private const int STATE_SYSTEMKEY = 2;

		// Token: 0x04002661 RID: 9825
		private const int STATE_WRITEACCESS = 4;

		// Token: 0x04002662 RID: 9826
		private const int MaxKeyLength = 255;

		// Token: 0x04002663 RID: 9827
		private const int MaxValueNameLength = 16383;

		// Token: 0x04002664 RID: 9828
		private const int MaxValueDataLength = 1048576;

		// Token: 0x04002665 RID: 9829
		private const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04002666 RID: 9830
		private const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04002667 RID: 9831
		private const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;

		// Token: 0x04002668 RID: 9832
		private static readonly string[] hkeyNames = new string[]
		{
			"HKEY_CLASSES_ROOT",
			"HKEY_CURRENT_USER",
			"HKEY_LOCAL_MACHINE",
			"HKEY_USERS",
			"HKEY_PERFORMANCE_DATA",
			"HKEY_CURRENT_CONFIG",
			"HKEY_DYN_DATA"
		};

		// Token: 0x04002669 RID: 9833
		private SafeRegistryHandle hkey;

		// Token: 0x0400266A RID: 9834
		private int state;

		// Token: 0x0400266B RID: 9835
		private string keyName;

		// Token: 0x0400266C RID: 9836
		private RegistryKeyPermissionCheck checkMode;

		// Token: 0x0400266D RID: 9837
		private Transaction myTransaction;

		// Token: 0x0400266E RID: 9838
		private SafeTransactionHandle myTransactionHandle;
	}
}
