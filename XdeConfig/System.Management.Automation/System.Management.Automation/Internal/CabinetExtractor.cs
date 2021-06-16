using System;
using System.Runtime.InteropServices;

namespace System.Management.Automation.Internal
{
	// Token: 0x02000196 RID: 406
	internal class CabinetExtractor : ICabinetExtractor
	{
		// Token: 0x0600138F RID: 5007 RVA: 0x00078FD0 File Offset: 0x000771D0
		internal CabinetExtractor()
		{
			CabinetNativeApi.FdiERF erf = new CabinetNativeApi.FdiERF();
			this.populateDelegates();
			this.fdiContext = CabinetNativeApi.FDICreate(Marshal.GetFunctionPointerForDelegate(this.allocDelegate), Marshal.GetFunctionPointerForDelegate(this.freeDelegate), Marshal.GetFunctionPointerForDelegate(this.openDelegate), Marshal.GetFunctionPointerForDelegate(this.readDelegate), Marshal.GetFunctionPointerForDelegate(this.writeDelegate), Marshal.GetFunctionPointerForDelegate(this.closeDelegate), Marshal.GetFunctionPointerForDelegate(this.seekDelegate), CabinetNativeApi.FdiCreateCpuType.Cpu80386, erf);
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00079049 File Offset: 0x00077249
		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (this.fdiContext != null)
			{
				this.fdiContext.Dispose();
			}
			this.CleanUpDelegates();
			this.disposed = true;
			base.Dispose(disposing);
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0007907C File Offset: 0x0007727C
		~CabinetExtractor()
		{
			this.Dispose(false);
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x000790AC File Offset: 0x000772AC
		internal override bool Extract(string cabinetName, string srcPath, string destPath)
		{
			IntPtr intPtr = Marshal.StringToHGlobalAnsi(destPath);
			bool result = CabinetNativeApi.FDICopy(this.fdiContext, cabinetName, srcPath, 0, Marshal.GetFunctionPointerForDelegate(this.notifyDelegate), IntPtr.Zero, intPtr);
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x000790E8 File Offset: 0x000772E8
		private void populateDelegates()
		{
			this.allocDelegate = new CabinetNativeApi.FdiAllocDelegate(CabinetNativeApi.FdiAlloc);
			this.fdiAllocHandle = GCHandle.Alloc(this.allocDelegate);
			this.freeDelegate = new CabinetNativeApi.FdiFreeDelegate(CabinetNativeApi.FdiFree);
			this.fdiFreeHandle = GCHandle.Alloc(this.freeDelegate);
			this.openDelegate = new CabinetNativeApi.FdiOpenDelegate(CabinetNativeApi.FdiOpen);
			this.fdiOpenHandle = GCHandle.Alloc(this.openDelegate);
			this.readDelegate = new CabinetNativeApi.FdiReadDelegate(CabinetNativeApi.FdiRead);
			this.fdiReadHandle = GCHandle.Alloc(this.readDelegate);
			this.writeDelegate = new CabinetNativeApi.FdiWriteDelegate(CabinetNativeApi.FdiWrite);
			this.fdiWriteHandle = GCHandle.Alloc(this.writeDelegate);
			this.closeDelegate = new CabinetNativeApi.FdiCloseDelegate(CabinetNativeApi.FdiClose);
			this.fdiCloseHandle = GCHandle.Alloc(this.closeDelegate);
			this.seekDelegate = new CabinetNativeApi.FdiSeekDelegate(CabinetNativeApi.FdiSeek);
			this.fdiSeekHandle = GCHandle.Alloc(this.seekDelegate);
			this.notifyDelegate = new CabinetNativeApi.FdiNotifyDelegate(CabinetNativeApi.FdiNotify);
			this.fdiNotifyHandle = GCHandle.Alloc(this.notifyDelegate);
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00079210 File Offset: 0x00077410
		private void CleanUpDelegates()
		{
			this.fdiAllocHandle.Free();
			this.fdiFreeHandle.Free();
			this.fdiOpenHandle.Free();
			this.fdiReadHandle.Free();
			this.fdiWriteHandle.Free();
			this.fdiCloseHandle.Free();
			this.fdiSeekHandle.Free();
			this.fdiNotifyHandle.Free();
		}

		// Token: 0x04000853 RID: 2131
		private CabinetNativeApi.FdiAllocDelegate allocDelegate;

		// Token: 0x04000854 RID: 2132
		private GCHandle fdiAllocHandle;

		// Token: 0x04000855 RID: 2133
		private CabinetNativeApi.FdiFreeDelegate freeDelegate;

		// Token: 0x04000856 RID: 2134
		private GCHandle fdiFreeHandle;

		// Token: 0x04000857 RID: 2135
		private CabinetNativeApi.FdiOpenDelegate openDelegate;

		// Token: 0x04000858 RID: 2136
		private GCHandle fdiOpenHandle;

		// Token: 0x04000859 RID: 2137
		private CabinetNativeApi.FdiReadDelegate readDelegate;

		// Token: 0x0400085A RID: 2138
		private GCHandle fdiReadHandle;

		// Token: 0x0400085B RID: 2139
		private CabinetNativeApi.FdiWriteDelegate writeDelegate;

		// Token: 0x0400085C RID: 2140
		private GCHandle fdiWriteHandle;

		// Token: 0x0400085D RID: 2141
		private CabinetNativeApi.FdiCloseDelegate closeDelegate;

		// Token: 0x0400085E RID: 2142
		private GCHandle fdiCloseHandle;

		// Token: 0x0400085F RID: 2143
		private CabinetNativeApi.FdiSeekDelegate seekDelegate;

		// Token: 0x04000860 RID: 2144
		private GCHandle fdiSeekHandle;

		// Token: 0x04000861 RID: 2145
		private CabinetNativeApi.FdiNotifyDelegate notifyDelegate;

		// Token: 0x04000862 RID: 2146
		private GCHandle fdiNotifyHandle;

		// Token: 0x04000863 RID: 2147
		internal CabinetNativeApi.FdiContextHandle fdiContext;

		// Token: 0x04000864 RID: 2148
		private bool disposed;
	}
}
