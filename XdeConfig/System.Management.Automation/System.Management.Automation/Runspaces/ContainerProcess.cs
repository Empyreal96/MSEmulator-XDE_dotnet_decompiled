using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Management.Automation.Internal;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x020002D3 RID: 723
	internal class ContainerProcess
	{
		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002298 RID: 8856 RVA: 0x000C3177 File Offset: 0x000C1377
		// (set) Token: 0x06002299 RID: 8857 RVA: 0x000C317F File Offset: 0x000C137F
		public Guid RuntimeId
		{
			get
			{
				return this.runtimeId;
			}
			set
			{
				this.runtimeId = value;
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x0600229A RID: 8858 RVA: 0x000C3188 File Offset: 0x000C1388
		// (set) Token: 0x0600229B RID: 8859 RVA: 0x000C3190 File Offset: 0x000C1390
		public string ContainerId
		{
			get
			{
				return this.containerId;
			}
			set
			{
				this.containerId = value;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x000C3199 File Offset: 0x000C1399
		// (set) Token: 0x0600229D RID: 8861 RVA: 0x000C31A1 File Offset: 0x000C13A1
		public string ContainerName
		{
			get
			{
				return this.containerName;
			}
			set
			{
				this.containerName = value;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x000C31AA File Offset: 0x000C13AA
		// (set) Token: 0x0600229F RID: 8863 RVA: 0x000C31B2 File Offset: 0x000C13B2
		internal int ProcessId
		{
			get
			{
				return this.processId;
			}
			set
			{
				this.processId = value;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x000C31BB File Offset: 0x000C13BB
		// (set) Token: 0x060022A1 RID: 8865 RVA: 0x000C31C3 File Offset: 0x000C13C3
		internal bool RunAsAdmin
		{
			get
			{
				return this.runAsAdmin;
			}
			set
			{
				this.runAsAdmin = value;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x060022A2 RID: 8866 RVA: 0x000C31CC File Offset: 0x000C13CC
		// (set) Token: 0x060022A3 RID: 8867 RVA: 0x000C31D4 File Offset: 0x000C13D4
		internal bool ProcessTerminated
		{
			get
			{
				return this.processTerminated;
			}
			set
			{
				this.processTerminated = value;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x000C31DD File Offset: 0x000C13DD
		// (set) Token: 0x060022A5 RID: 8869 RVA: 0x000C31E5 File Offset: 0x000C13E5
		internal uint ErrorCode
		{
			get
			{
				return this.errorCode;
			}
			set
			{
				this.errorCode = value;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x060022A6 RID: 8870 RVA: 0x000C31EE File Offset: 0x000C13EE
		// (set) Token: 0x060022A7 RID: 8871 RVA: 0x000C31F6 File Offset: 0x000C13F6
		internal string ErrorMessage
		{
			get
			{
				return this.errorMessage;
			}
			set
			{
				this.errorMessage = value;
			}
		}

		// Token: 0x060022A8 RID: 8872
		[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint CreateProcessInComputeSystem(string id, string ProcessParameters, ref int processId);

		// Token: 0x060022A9 RID: 8873
		[DllImport("vmcompute.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint TerminateProcessInComputeSystem(string id, int processId);

		// Token: 0x060022AA RID: 8874 RVA: 0x000C3200 File Offset: 0x000C1400
		public ContainerProcess(string containerId, string containerName, int processId, bool runAsAdmin)
		{
			this.ContainerId = containerId;
			this.ContainerName = containerName;
			this.ProcessId = processId;
			this.RunAsAdmin = runAsAdmin;
			if (!string.IsNullOrEmpty(containerId))
			{
				if (string.IsNullOrEmpty(containerName))
				{
					this.GetContainerNameFromContainerId();
					return;
				}
			}
			else if (!string.IsNullOrEmpty(containerName))
			{
				this.GetContainerIdFromContainerName();
			}
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x000C3260 File Offset: 0x000C1460
		public void CreateContainerProcess()
		{
			this.RunOnMTAThread(new ThreadStart(this.CreateContainerProcessInternal));
			uint num = this.ErrorCode;
			switch (num)
			{
			case 0U:
				return;
			case 1U:
				throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.InvalidContainerId, this.ContainerId));
			case 2U:
				throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.ContainersFeatureNotEnabled, new object[0]));
			default:
				if (num != 9999U)
				{
					throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.CannotCreateProcessInContainer, this.ContainerId));
				}
				throw new PSInvalidOperationException(this.ErrorMessage);
			}
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x000C32F0 File Offset: 0x000C14F0
		public bool TerminateContainerProcess()
		{
			this.RunOnMTAThread(new ThreadStart(this.TerminateContainerProcessInternal));
			return this.ProcessTerminated;
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x000C330C File Offset: 0x000C150C
		public void GetContainerNameFromContainerId()
		{
			this.RunOnMTAThread(new ThreadStart(this.GetContainerNameFromContainerIdInternal));
			uint num = this.ErrorCode;
			switch (num)
			{
			case 0U:
			case 1U:
				return;
			case 2U:
				throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.ContainersFeatureNotEnabled, new object[0]));
			default:
				if (num != 9999U)
				{
					return;
				}
				throw new PSInvalidOperationException(this.ErrorMessage);
			}
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x000C3370 File Offset: 0x000C1570
		public void GetContainerIdFromContainerName()
		{
			this.RunOnMTAThread(new ThreadStart(this.GetContainerIdFromContainerNameInternal));
			uint num = this.ErrorCode;
			switch (num)
			{
			case 0U:
			case 1U:
				return;
			case 2U:
				throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.ContainersFeatureNotEnabled, new object[0]));
			case 3U:
				throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.InvalidContainerNameMultiple, this.ContainerName));
			case 4U:
				throw new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.InvalidContainerNameNotExist, this.ContainerName));
			default:
				if (num != 9999U)
				{
					return;
				}
				throw new PSInvalidOperationException(this.ErrorMessage);
			}
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x000C3408 File Offset: 0x000C1608
		private static void GetHostComputeInteropTypes(out Type computeSystemPropertiesType, out Type hostComputeInteropType)
		{
			Assembly assembly = Assembly.Load(new AssemblyName("Microsoft.HyperV.Schema, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"));
			computeSystemPropertiesType = assembly.GetType("Microsoft.HyperV.Schema.Compute.System.Properties");
			Assembly assembly2 = Assembly.Load(new AssemblyName("Microsoft.HostCompute.Interop, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"));
			hostComputeInteropType = assembly2.GetType("Microsoft.HostCompute.Interop.HostComputeInterop");
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x000C3450 File Offset: 0x000C1650
		private void CreateContainerProcessInternal()
		{
			int num = 0;
			uint num2 = 0U;
			try
			{
				Type type;
				Type type2;
				ContainerProcess.GetHostComputeInteropTypes(out type, out type2);
				MethodInfo method = type2.GetMethod("ComputeSystemExists");
				if (!(bool)method.Invoke(null, new object[]
				{
					this.ContainerId
				}))
				{
					num = 0;
					num2 = 1U;
				}
				else
				{
					string processParameters = string.Format(CultureInfo.InvariantCulture, "{{\"CommandLine\": \"powershell.exe {0} -NoLogo\",\"RestrictedToken\": {1}}}", new object[]
					{
						(this.RuntimeId != Guid.Empty) ? "-so -NoProfile" : "-NamedPipeServerMode",
						this.RunAsAdmin ? "false" : "true"
					});
					uint num3 = ContainerProcess.CreateProcessInComputeSystem(this.ContainerId, processParameters, ref num);
					if (num3 != 0U)
					{
						num = 0;
						num2 = num3;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is FileNotFoundException || ex is FileLoadException)
				{
					this.ProcessId = 0;
					this.ErrorCode = 2U;
					return;
				}
				this.ProcessId = 0;
				this.ErrorCode = 9999U;
				this.ErrorMessage = this.GetErrorMessageFromException(ex);
				return;
			}
			this.ProcessId = num;
			this.ErrorCode = num2;
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x000C3574 File Offset: 0x000C1774
		private void TerminateContainerProcessInternal()
		{
			this.ProcessTerminated = (ContainerProcess.TerminateProcessInComputeSystem(this.ContainerId, this.ProcessId) == 0U);
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000C3590 File Offset: 0x000C1790
		private void GetContainerNameFromContainerIdInternal()
		{
			try
			{
				Type type;
				Type type2;
				ContainerProcess.GetHostComputeInteropTypes(out type, out type2);
				MethodInfo method = type2.GetMethod("GetComputeSystemProperties");
				object obj = method.Invoke(null, new object[]
				{
					this.ContainerId
				});
				this.ContainerName = (string)type.GetProperty("Name").GetValue(obj);
				this.RuntimeId = (Guid)type.GetProperty("RuntimeId").GetValue(obj);
			}
			catch (FileNotFoundException)
			{
				this.ErrorCode = 2U;
			}
			catch (FileLoadException)
			{
				this.ErrorCode = 2U;
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null && StringComparer.Ordinal.Equals(ex.InnerException.GetType().FullName, "Microsoft.HostCompute.Interop.ObjectNotFoundException"))
				{
					this.ErrorCode = 1U;
				}
				else
				{
					this.ErrorCode = 9999U;
					this.ErrorMessage = this.GetErrorMessageFromException(ex);
				}
			}
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000C3698 File Offset: 0x000C1898
		private void GetContainerIdFromContainerNameInternal()
		{
			try
			{
				object obj = null;
				int num = 0;
				WildcardPattern wildcardPattern = new WildcardPattern(this.ContainerName, WildcardOptions.IgnoreCase);
				Type type;
				Type type2;
				ContainerProcess.GetHostComputeInteropTypes(out type, out type2);
				MethodInfo method = type2.GetMethod("EnumerateComputeSystems");
				MethodBase methodBase = method;
				object obj2 = null;
				object[] parameters = new object[4];
				IEnumerable enumerable = (IEnumerable)methodBase.Invoke(obj2, parameters);
				foreach (object obj3 in enumerable)
				{
					string input = (string)type.GetProperty("Name").GetValue(obj3);
					if (wildcardPattern.IsMatch(input))
					{
						obj = obj3;
						num++;
						if (num > 1)
						{
							this.ErrorCode = 3U;
							return;
						}
					}
				}
				if (obj == null)
				{
					this.ErrorCode = 4U;
				}
				else
				{
					this.ContainerId = (string)type.GetProperty("Id").GetValue(obj);
					this.ContainerName = (string)type.GetProperty("Name").GetValue(obj);
					this.RuntimeId = (Guid)type.GetProperty("RuntimeId").GetValue(obj);
				}
			}
			catch (FileNotFoundException)
			{
				this.ErrorCode = 2U;
			}
			catch (FileLoadException)
			{
				this.ErrorCode = 2U;
			}
			catch (Exception e)
			{
				this.ErrorCode = 9999U;
				this.ErrorMessage = this.GetErrorMessageFromException(e);
			}
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000C3850 File Offset: 0x000C1A50
		private void RunOnMTAThread(ThreadStart threadProc)
		{
			if (Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA)
			{
				threadProc();
				return;
			}
			Thread thread = new Thread(new ThreadStart(threadProc.Invoke));
			thread.SetApartmentState(ApartmentState.MTA);
			thread.Start();
			thread.Join();
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x000C3898 File Offset: 0x000C1A98
		private string GetErrorMessageFromException(Exception e)
		{
			string text = e.Message;
			if (e.InnerException != null)
			{
				text = text + " " + e.InnerException.Message;
			}
			return text;
		}

		// Token: 0x04001073 RID: 4211
		private const uint NoError = 0U;

		// Token: 0x04001074 RID: 4212
		private const uint InvalidContainerId = 1U;

		// Token: 0x04001075 RID: 4213
		private const uint ContainersFeatureNotEnabled = 2U;

		// Token: 0x04001076 RID: 4214
		private const uint InvalidContainerNameMultiple = 3U;

		// Token: 0x04001077 RID: 4215
		private const uint InvalidContainerNameNotExist = 4U;

		// Token: 0x04001078 RID: 4216
		private const uint OtherError = 9999U;

		// Token: 0x04001079 RID: 4217
		private Guid runtimeId;

		// Token: 0x0400107A RID: 4218
		private string containerId;

		// Token: 0x0400107B RID: 4219
		private string containerName;

		// Token: 0x0400107C RID: 4220
		private int processId;

		// Token: 0x0400107D RID: 4221
		private bool runAsAdmin;

		// Token: 0x0400107E RID: 4222
		private bool processTerminated;

		// Token: 0x0400107F RID: 4223
		private uint errorCode;

		// Token: 0x04001080 RID: 4224
		private string errorMessage = string.Empty;
	}
}
