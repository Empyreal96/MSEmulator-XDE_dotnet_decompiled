using System;
using System.ComponentModel;
using System.Management.Automation;
using System.Security;
using System.Transactions;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.PowerShell.Commands.Internal
{
	// Token: 0x020007AE RID: 1966
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeTransactionHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06004D3D RID: 19773 RVA: 0x001971AE File Offset: 0x001953AE
		private SafeTransactionHandle(IntPtr handle) : base(true)
		{
			this.handle = handle;
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x001971BE File Offset: 0x001953BE
		internal static SafeTransactionHandle Create()
		{
			return SafeTransactionHandle.Create(Transaction.Current);
		}

		// Token: 0x06004D3F RID: 19775 RVA: 0x001971CC File Offset: 0x001953CC
		internal static SafeTransactionHandle Create(Transaction managedTransaction)
		{
			if (managedTransaction == null)
			{
				throw new InvalidOperationException(RegistryProviderStrings.InvalidOperation_NeedTransaction);
			}
			if (RemotingCommandUtil.IsWinPEHost() || PsUtils.IsRunningOnProcessorArchitectureARM())
			{
				throw new NotSupportedException(RegistryProviderStrings.NotSupported_KernelTransactions);
			}
			IDtcTransaction dtcTransaction = TransactionInterop.GetDtcTransaction(managedTransaction);
			IKernelTransaction kernelTransaction = dtcTransaction as IKernelTransaction;
			if (kernelTransaction == null)
			{
				throw new NotSupportedException(RegistryProviderStrings.NotSupported_KernelTransactions);
			}
			IntPtr handle2;
			int handle = kernelTransaction.GetHandle(out handle2);
			SafeTransactionHandle.HandleError(handle);
			return new SafeTransactionHandle(handle2);
		}

		// Token: 0x06004D40 RID: 19776 RVA: 0x00197237 File Offset: 0x00195437
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x00197244 File Offset: 0x00195444
		private static void HandleError(int error)
		{
			if (error != 0)
			{
				throw new Win32Exception(error);
			}
		}

		// Token: 0x0400266F RID: 9839
		private const string resBaseName = "RegistryProviderStrings";
	}
}
