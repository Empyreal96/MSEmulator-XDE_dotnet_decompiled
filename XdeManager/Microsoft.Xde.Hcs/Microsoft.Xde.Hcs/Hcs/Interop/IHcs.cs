using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using HCS.Schema.Responses.System;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Xde.Hcs.Interop
{
	// Token: 0x02000014 RID: 20
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
	public interface IHcs
	{
		// Token: 0x0600008E RID: 142
		string EnumerateComputeSystems(string query);

		// Token: 0x0600008F RID: 143
		Task<SafeHcsSystemHandle> CreateComputeSystemAsync(string id, string configuration, SafeAccessTokenHandle identity);

		// Token: 0x06000090 RID: 144
		SafeHcsSystemHandle OpenComputeSystem(string id);

		// Token: 0x06000091 RID: 145
		bool CloseComputeSystem(IntPtr computeSystem);

		// Token: 0x06000092 RID: 146
		Task StartComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options);

		// Token: 0x06000093 RID: 147
		Task ShutdownComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options);

		// Token: 0x06000094 RID: 148
		Task TerminateComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options);

		// Token: 0x06000095 RID: 149
		Task WaitOnComputeSystemExitAsync(SafeHcsSystemHandle computeSystem);

		// Token: 0x06000096 RID: 150
		Task PauseComputeSystemAsync(SafeHcsSystemHandle computesystem, string options);

		// Token: 0x06000097 RID: 151
		Task ResumeComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options);

		// Token: 0x06000098 RID: 152
		Task SaveComputeSystemAsync(SafeHcsSystemHandle computeSystem, string options);

		// Token: 0x06000099 RID: 153
		string GetComputeSystemProperties(SafeHcsSystemHandle computeSystem, string propertyQuery);

		// Token: 0x0600009A RID: 154
		bool ModifyComputeSystem(SafeHcsSystemHandle computeSystem, string configuration);

		// Token: 0x0600009B RID: 155
		bool AddComputeSystemResource(SafeHcsSystemHandle computeSystem, string location, string resource);

		// Token: 0x0600009C RID: 156
		bool ModifyComputeSystemResource(SafeHcsSystemHandle computeSystem, string location, string resource);

		// Token: 0x0600009D RID: 157
		bool RemoveComputeSystemResource(SafeHcsSystemHandle computeSystem, string location);

		// Token: 0x0600009E RID: 158
		SafeHcsProcessHandle CreateProcess(SafeHcsSystemHandle computeSystem, string processParameters);

		// Token: 0x0600009F RID: 159
		SafeHcsProcessHandle OpenProcess(SafeHcsSystemHandle computeSystem, uint processId);

		// Token: 0x060000A0 RID: 160
		bool CloseProcess(IntPtr process);

		// Token: 0x060000A1 RID: 161
		Task<ProcessStatus> TerminateProcessAsync(SafeHcsProcessHandle process);

		// Token: 0x060000A2 RID: 162
		bool SignalProcess(SafeHcsProcessHandle process, string options);

		// Token: 0x060000A3 RID: 163
		Task<ProcessStatus> WaitOnProcessExitAsync(SafeHcsProcessHandle process);

		// Token: 0x060000A4 RID: 164
		string GetProcessProperties(SafeHcsProcessHandle process);

		// Token: 0x060000A5 RID: 165
		bool ModifyProcess(SafeHcsProcessHandle process, string settings);
	}
}
