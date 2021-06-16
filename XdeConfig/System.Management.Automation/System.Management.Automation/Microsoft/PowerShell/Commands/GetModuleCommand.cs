using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Threading;
using Microsoft.Management.Infrastructure;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000A9 RID: 169
	[Cmdlet("Get", "Module", DefaultParameterSetName = "Loaded", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=141552")]
	[OutputType(new Type[]
	{
		typeof(PSModuleInfo)
	})]
	public sealed class GetModuleCommand : ModuleCmdletBase, IDisposable
	{
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x000321DD File Offset: 0x000303DD
		// (set) Token: 0x06000856 RID: 2134 RVA: 0x000321E5 File Offset: 0x000303E5
		[Parameter(ParameterSetName = "CimSession", ValueFromPipeline = true, Position = 0)]
		[ValidateNotNullOrEmpty]
		[Parameter(ParameterSetName = "Available", ValueFromPipeline = true, Position = 0)]
		[Parameter(ParameterSetName = "PsSession", ValueFromPipeline = true, Position = 0)]
		[Parameter(ParameterSetName = "Loaded", ValueFromPipeline = true, Position = 0)]
		public string[] Name { get; set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x000321EE File Offset: 0x000303EE
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x000321F6 File Offset: 0x000303F6
		[Parameter(ParameterSetName = "CimSession", ValueFromPipelineByPropertyName = true)]
		[Parameter(ParameterSetName = "Loaded", ValueFromPipelineByPropertyName = true)]
		[Parameter(ParameterSetName = "Available", ValueFromPipelineByPropertyName = true)]
		[Parameter(ParameterSetName = "PsSession", ValueFromPipelineByPropertyName = true)]
		public ModuleSpecification[] FullyQualifiedName { get; set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x000321FF File Offset: 0x000303FF
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x00032207 File Offset: 0x00030407
		[Parameter(ParameterSetName = "Loaded")]
		[Parameter(ParameterSetName = "Available")]
		public SwitchParameter All { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x00032210 File Offset: 0x00030410
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x00032218 File Offset: 0x00030418
		[Parameter(ParameterSetName = "Available", Mandatory = true)]
		[Parameter(ParameterSetName = "PsSession")]
		[Parameter(ParameterSetName = "CimSession")]
		public SwitchParameter ListAvailable { get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00032221 File Offset: 0x00030421
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x00032229 File Offset: 0x00030429
		[Parameter(ParameterSetName = "Available")]
		[Parameter(ParameterSetName = "CimSession")]
		[Parameter(ParameterSetName = "PsSession")]
		public SwitchParameter Refresh { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00032232 File Offset: 0x00030432
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x0003223A File Offset: 0x0003043A
		[Parameter(ParameterSetName = "PsSession", Mandatory = true)]
		[ValidateNotNull]
		public PSSession PSSession { get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x00032243 File Offset: 0x00030443
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x0003224B File Offset: 0x0003044B
		[ValidateNotNull]
		[Parameter(ParameterSetName = "CimSession", Mandatory = true)]
		public CimSession CimSession { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x00032254 File Offset: 0x00030454
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x0003225C File Offset: 0x0003045C
		[Parameter(ParameterSetName = "CimSession", Mandatory = false)]
		[ValidateNotNull]
		public Uri CimResourceUri { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x00032265 File Offset: 0x00030465
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x0003226D File Offset: 0x0003046D
		[Parameter(ParameterSetName = "CimSession", Mandatory = false)]
		[ValidateNotNullOrEmpty]
		public string CimNamespace { get; set; }

		// Token: 0x06000867 RID: 2151 RVA: 0x00032558 File Offset: 0x00030758
		private IEnumerable<PSModuleInfo> GetAvailableViaPsrpSessionCore(string[] moduleNames, Runspace remoteRunspace)
		{
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.Runspace = remoteRunspace;
				powerShell.AddCommand("Get-Module");
				powerShell.AddParameter("ListAvailable", true);
				if (this.Refresh.IsPresent)
				{
					powerShell.AddParameter("Refresh", true);
				}
				if (moduleNames != null)
				{
					powerShell.AddParameter("Name", moduleNames);
				}
				string errorMessageTemplate = string.Format(CultureInfo.InvariantCulture, Modules.RemoteDiscoveryRemotePsrpCommandFailed, new object[]
				{
					"Get-Module"
				});
				foreach (PSObject outputObject in RemoteDiscoveryHelper.InvokePowerShell(powerShell, this.CancellationToken, this, errorMessageTemplate))
				{
					PSModuleInfo moduleInfo = RemoteDiscoveryHelper.RehydratePSModuleInfo(outputObject);
					yield return moduleInfo;
				}
			}
			yield break;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00032583 File Offset: 0x00030783
		private PSModuleInfo GetModuleInfoForRemoteModuleWithoutManifest(RemoteDiscoveryHelper.CimModule cimModule)
		{
			return new PSModuleInfo(cimModule.ModuleName, null, null);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00032594 File Offset: 0x00030794
		private PSModuleInfo ConvertCimModuleInfoToPSModuleInfo(RemoteDiscoveryHelper.CimModule cimModule, string computerName)
		{
			PSModuleInfo result;
			try
			{
				bool flag = false;
				if (cimModule.MainManifest == null)
				{
					result = this.GetModuleInfoForRemoteModuleWithoutManifest(cimModule);
				}
				else
				{
					string text = Path.Combine(RemoteDiscoveryHelper.GetModulePath(cimModule.ModuleName, null, computerName, base.Context.CurrentRunspace), Path.GetFileName(cimModule.ModuleName));
					Hashtable hashtable = null;
					if (!flag)
					{
						hashtable = RemoteDiscoveryHelper.ConvertCimModuleFileToManifestHashtable(cimModule.MainManifest, text, this, ref flag);
						if (hashtable == null)
						{
							return this.GetModuleInfoForRemoteModuleWithoutManifest(cimModule);
						}
					}
					if (!flag)
					{
						hashtable = RemoteDiscoveryHelper.RewriteManifest(hashtable);
					}
					Hashtable localizedData = hashtable;
					PSModuleInfo psmoduleInfo = null;
					if (!flag)
					{
						ModuleCmdletBase.ImportModuleOptions importModuleOptions = default(ModuleCmdletBase.ImportModuleOptions);
						psmoduleInfo = base.LoadModuleManifest(text, null, hashtable, localizedData, (ModuleCmdletBase.ManifestProcessingFlags)0, base.BaseMinimumVersion, base.BaseMaximumVersion, base.BaseRequiredVersion, base.BaseGuid, ref importModuleOptions, ref flag);
					}
					if (psmoduleInfo == null || flag)
					{
						psmoduleInfo = this.GetModuleInfoForRemoteModuleWithoutManifest(cimModule);
					}
					result = psmoduleInfo;
				}
			}
			catch (Exception innerException)
			{
				ErrorRecord errorRecordForProcessingOfCimModule = RemoteDiscoveryHelper.GetErrorRecordForProcessingOfCimModule(innerException, cimModule.ModuleName);
				base.WriteError(errorRecordForProcessingOfCimModule);
				result = null;
			}
			return result;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x000326BC File Offset: 0x000308BC
		private IEnumerable<PSModuleInfo> GetAvailableViaCimSessionCore(IEnumerable<string> moduleNames, CimSession cimSession, Uri resourceUri, string cimNamespace)
		{
			IEnumerable<RemoteDiscoveryHelper.CimModule> cimModules = RemoteDiscoveryHelper.GetCimModules(cimSession, resourceUri, cimNamespace, moduleNames, true, this, this.CancellationToken);
			return from cimModule in cimModules
			select this.ConvertCimModuleInfoToPSModuleInfo(cimModule, cimSession.ComputerName) into moduleInfo
			where moduleInfo != null
			select moduleInfo;
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x0003272B File Offset: 0x0003092B
		private CancellationToken CancellationToken
		{
			get
			{
				return this._cancellationTokenSource.Token;
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00032738 File Offset: 0x00030938
		protected override void StopProcessing()
		{
			this._cancellationTokenSource.Cancel();
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00032745 File Offset: 0x00030945
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00032754 File Offset: 0x00030954
		private void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing)
			{
				this._cancellationTokenSource.Dispose();
			}
			this._disposed = true;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00032774 File Offset: 0x00030974
		private void AssertListAvailableMode()
		{
			if (!this.ListAvailable.IsPresent)
			{
				string remoteDiscoveryWorksOnlyInListAvailableMode = Modules.RemoteDiscoveryWorksOnlyInListAvailableMode;
				ArgumentException exception = new ArgumentException(remoteDiscoveryWorksOnlyInListAvailableMode);
				ErrorRecord errorRecord = new ErrorRecord(exception, "RemoteDiscoveryWorksOnlyInListAvailableMode", ErrorCategory.InvalidArgument, null);
				base.ThrowTerminatingError(errorRecord);
			}
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000327C4 File Offset: 0x000309C4
		protected override void ProcessRecord()
		{
			if (this.Name != null && this.FullyQualifiedName != null)
			{
				string message = StringUtil.Format(SessionStateStrings.GetContent_TailAndHeadCannotCoexist, "Name", "FullyQualifiedName");
				ErrorRecord errorRecord = new ErrorRecord(new InvalidOperationException(message), "NameAndFullyQualifiedNameCannotBeSpecifiedTogether", ErrorCategory.InvalidOperation, null);
				base.ThrowTerminatingError(errorRecord);
			}
			List<string> list = new List<string>();
			if (this.Name != null)
			{
				list.AddRange(this.Name);
			}
			Dictionary<string, ModuleSpecification> moduleSpecTable = new Dictionary<string, ModuleSpecification>(StringComparer.OrdinalIgnoreCase);
			if (this.FullyQualifiedName != null)
			{
				moduleSpecTable = this.FullyQualifiedName.ToDictionary((ModuleSpecification moduleSpecification) => moduleSpecification.Name, StringComparer.OrdinalIgnoreCase);
				list.AddRange(from spec in this.FullyQualifiedName
				select spec.Name);
			}
			string[] names = (list.Count > 0) ? list.ToArray() : null;
			if (base.ParameterSetName.Equals("Loaded", StringComparison.OrdinalIgnoreCase))
			{
				this.AssertNameDoesNotResolveToAPath(names, Modules.ModuleDiscoveryForLoadedModulesWorksOnlyForUnQualifiedNames, "ModuleDiscoveryForLoadedModulesWorksOnlyForUnQualifiedNames");
				this.GetLoadedModules(names, moduleSpecTable, this.All);
				return;
			}
			if (base.ParameterSetName.Equals("Available", StringComparison.OrdinalIgnoreCase))
			{
				if (this.ListAvailable.IsPresent)
				{
					this.GetAvailableLocallyModules(names, moduleSpecTable, this.All);
					return;
				}
				this.AssertNameDoesNotResolveToAPath(names, Modules.ModuleDiscoveryForLoadedModulesWorksOnlyForUnQualifiedNames, "ModuleDiscoveryForLoadedModulesWorksOnlyForUnQualifiedNames");
				this.GetLoadedModules(names, moduleSpecTable, this.All);
				return;
			}
			else
			{
				if (base.ParameterSetName.Equals("PsSession", StringComparison.OrdinalIgnoreCase))
				{
					this.AssertListAvailableMode();
					this.AssertNameDoesNotResolveToAPath(names, Modules.RemoteDiscoveryWorksOnlyForUnQualifiedNames, "RemoteDiscoveryWorksOnlyForUnQualifiedNames");
					this.GetAvailableViaPsrpSession(names, moduleSpecTable, this.PSSession);
					return;
				}
				if (base.ParameterSetName.Equals("CimSession", StringComparison.OrdinalIgnoreCase))
				{
					this.AssertListAvailableMode();
					this.AssertNameDoesNotResolveToAPath(names, Modules.RemoteDiscoveryWorksOnlyForUnQualifiedNames, "RemoteDiscoveryWorksOnlyForUnQualifiedNames");
					this.GetAvailableViaCimSession(names, moduleSpecTable, this.CimSession, this.CimResourceUri, this.CimNamespace);
				}
				return;
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x000329C8 File Offset: 0x00030BC8
		private void AssertNameDoesNotResolveToAPath(string[] names, string stringFormat, string resourceId)
		{
			if (names != null)
			{
				foreach (string text in names)
				{
					if (text.IndexOf('\\') != -1 || text.IndexOf('/') != -1)
					{
						string message = StringUtil.Format(stringFormat, text);
						ArgumentException exception = new ArgumentException(message);
						ErrorRecord errorRecord = new ErrorRecord(exception, resourceId, ErrorCategory.InvalidArgument, text);
						base.ThrowTerminatingError(errorRecord);
					}
				}
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00032A2A File Offset: 0x00030C2A
		private static bool ModuleMatch(PSModuleInfo moduleInfo, IDictionary<string, ModuleSpecification> moduleSpecTable)
		{
			return !moduleSpecTable.ContainsKey(moduleInfo.Name) || (moduleSpecTable.ContainsKey(moduleInfo.Name) && ModuleIntrinsics.IsModuleMatchingModuleSpec(moduleInfo, moduleSpecTable[moduleInfo.Name]));
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00032A78 File Offset: 0x00030C78
		private void GetAvailableViaCimSession(IEnumerable<string> names, IDictionary<string, ModuleSpecification> moduleSpecTable, CimSession cimSession, Uri resourceUri, string cimNamespace)
		{
			IEnumerable<PSModuleInfo> availableViaCimSessionCore = this.GetAvailableViaCimSessionCore(names, cimSession, resourceUri, cimNamespace);
			foreach (PSModuleInfo psmoduleInfo in from remoteModule in availableViaCimSessionCore
			where GetModuleCommand.ModuleMatch(remoteModule, moduleSpecTable)
			select remoteModule)
			{
				RemoteDiscoveryHelper.AssociatePSModuleInfoWithSession(psmoduleInfo, cimSession, resourceUri, cimNamespace);
				base.WriteObject(psmoduleInfo);
			}
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00032B10 File Offset: 0x00030D10
		private void GetAvailableViaPsrpSession(string[] names, IDictionary<string, ModuleSpecification> moduleSpecTable, PSSession session)
		{
			IEnumerable<PSModuleInfo> availableViaPsrpSessionCore = this.GetAvailableViaPsrpSessionCore(names, session.Runspace);
			foreach (PSModuleInfo psmoduleInfo in from remoteModule in availableViaPsrpSessionCore
			where GetModuleCommand.ModuleMatch(remoteModule, moduleSpecTable)
			select remoteModule)
			{
				RemoteDiscoveryHelper.AssociatePSModuleInfoWithSession(psmoduleInfo, session);
				base.WriteObject(psmoduleInfo);
			}
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00032BAC File Offset: 0x00030DAC
		private void GetAvailableLocallyModules(string[] names, IDictionary<string, ModuleSpecification> moduleSpecTable, bool all)
		{
			bool isPresent = this.Refresh.IsPresent;
			List<PSModuleInfo> module2 = base.GetModule(names, all, isPresent);
			foreach (PSObject psobject in from module in module2
			where GetModuleCommand.ModuleMatch(module, moduleSpecTable)
			select new PSObject(module))
			{
				psobject.TypeNames.Insert(0, "ModuleInfoGrouping");
				base.WriteObject(psobject);
			}
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00032C7C File Offset: 0x00030E7C
		private void GetLoadedModules(string[] names, IDictionary<string, ModuleSpecification> moduleSpecTable, bool all)
		{
			List<PSModuleInfo> modules = base.Context.Modules.GetModules(names, all);
			foreach (PSModuleInfo sendToPipeline in from moduleInfo in modules
			where GetModuleCommand.ModuleMatch(moduleInfo, moduleSpecTable)
			select moduleInfo)
			{
				base.WriteObject(sendToPipeline);
			}
		}

		// Token: 0x040003CC RID: 972
		private const string ParameterSet_Loaded = "Loaded";

		// Token: 0x040003CD RID: 973
		private const string ParameterSet_AvailableLocally = "Available";

		// Token: 0x040003CE RID: 974
		private const string ParameterSet_AvailableInPsrpSession = "PsSession";

		// Token: 0x040003CF RID: 975
		private const string ParameterSet_AvailableInCimSession = "CimSession";

		// Token: 0x040003D0 RID: 976
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x040003D1 RID: 977
		private bool _disposed;
	}
}
