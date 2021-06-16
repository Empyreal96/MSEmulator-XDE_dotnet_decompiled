using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x0200000B RID: 11
	public abstract class PSCmdlet : Cmdlet
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003D07 File Offset: 0x00001F07
		internal bool HasDynamicParameters
		{
			get
			{
				return this is IDynamicParameters;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003D14 File Offset: 0x00001F14
		public string ParameterSetName
		{
			get
			{
				string parameterSetName;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					parameterSetName = base._ParameterSetName;
				}
				return parameterSetName;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003D4C File Offset: 0x00001F4C
		public new InvocationInfo MyInvocation
		{
			get
			{
				InvocationInfo myInvocation;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					myInvocation = base.MyInvocation;
				}
				return myInvocation;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003D84 File Offset: 0x00001F84
		public PagingParameters PagingParameters
		{
			get
			{
				PagingParameters result;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					if (!base.CommandInfo.CommandMetadata.SupportsPaging)
					{
						result = null;
					}
					else
					{
						if (this.pagingParameters == null)
						{
							MshCommandRuntime mshCommandRuntime = base.CommandRuntime as MshCommandRuntime;
							if (mshCommandRuntime != null)
							{
								this.pagingParameters = (mshCommandRuntime.PagingParameters ?? new PagingParameters(mshCommandRuntime));
							}
						}
						result = this.pagingParameters;
					}
				}
				return result;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003E00 File Offset: 0x00002000
		public CommandInvocationIntrinsics InvokeCommand
		{
			get
			{
				CommandInvocationIntrinsics invokeCommand;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					if (this._invokeCommand == null)
					{
						this._invokeCommand = new CommandInvocationIntrinsics(base.Context, this);
					}
					invokeCommand = this._invokeCommand;
				}
				return invokeCommand;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00003E54 File Offset: 0x00002054
		public PSHost Host
		{
			get
			{
				PSHost pshostInternal;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					pshostInternal = base.PSHostInternal;
				}
				return pshostInternal;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003E8C File Offset: 0x0000208C
		public SessionState SessionState
		{
			get
			{
				SessionState internalState;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					internalState = base.InternalState;
				}
				return internalState;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00003EC4 File Offset: 0x000020C4
		public PSEventManager Events
		{
			get
			{
				PSEventManager events;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					events = base.Context.Events;
				}
				return events;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00003F00 File Offset: 0x00002100
		public JobRepository JobRepository
		{
			get
			{
				JobRepository jobRepository;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					jobRepository = ((LocalRunspace)base.Context.CurrentRunspace).JobRepository;
				}
				return jobRepository;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003F48 File Offset: 0x00002148
		public JobManager JobManager
		{
			get
			{
				JobManager jobManager;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					jobManager = ((LocalRunspace)base.Context.CurrentRunspace).JobManager;
				}
				return jobManager;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003F90 File Offset: 0x00002190
		internal RunspaceRepository RunspaceRepository
		{
			get
			{
				return ((LocalRunspace)base.Context.CurrentRunspace).RunspaceRepository;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003FA8 File Offset: 0x000021A8
		public ProviderIntrinsics InvokeProvider
		{
			get
			{
				ProviderIntrinsics result;
				using (PSTransactionManager.GetEngineProtectionScope())
				{
					if (this.invokeProvider == null)
					{
						this.invokeProvider = new ProviderIntrinsics(this);
					}
					result = this.invokeProvider;
				}
				return result;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003FF4 File Offset: 0x000021F4
		public PathInfo CurrentProviderLocation(string providerId)
		{
			PathInfo result;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				if (providerId == null)
				{
					throw PSTraceSource.NewArgumentNullException("providerId");
				}
				PathInfo pathInfo = this.SessionState.Path.CurrentProviderLocation(providerId);
				result = pathInfo;
			}
			return result;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004048 File Offset: 0x00002248
		public string GetUnresolvedProviderPathFromPSPath(string path)
		{
			string unresolvedProviderPathFromPSPath;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				unresolvedProviderPathFromPSPath = this.SessionState.Path.GetUnresolvedProviderPathFromPSPath(path);
			}
			return unresolvedProviderPathFromPSPath;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000408C File Offset: 0x0000228C
		public Collection<string> GetResolvedProviderPathFromPSPath(string path, out ProviderInfo provider)
		{
			Collection<string> resolvedProviderPathFromPSPath;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				resolvedProviderPathFromPSPath = this.SessionState.Path.GetResolvedProviderPathFromPSPath(path, out provider);
			}
			return resolvedProviderPathFromPSPath;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000040D8 File Offset: 0x000022D8
		public object GetVariableValue(string name)
		{
			object value;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				value = this.SessionState.PSVariable.GetValue(name);
			}
			return value;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000411C File Offset: 0x0000231C
		public object GetVariableValue(string name, object defaultValue)
		{
			object value;
			using (PSTransactionManager.GetEngineProtectionScope())
			{
				value = this.SessionState.PSVariable.GetValue(name, defaultValue);
			}
			return value;
		}

		// Token: 0x04000026 RID: 38
		private PagingParameters pagingParameters;

		// Token: 0x04000027 RID: 39
		private CommandInvocationIntrinsics _invokeCommand;

		// Token: 0x04000028 RID: 40
		private ProviderIntrinsics invokeProvider;
	}
}
