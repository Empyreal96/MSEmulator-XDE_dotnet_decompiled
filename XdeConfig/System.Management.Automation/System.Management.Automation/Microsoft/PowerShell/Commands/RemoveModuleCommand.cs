using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Reflection;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020000BF RID: 191
	[Cmdlet("Remove", "Module", SupportsShouldProcess = true, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=141556")]
	public sealed class RemoveModuleCommand : ModuleCmdletBase
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0003E7AE File Offset: 0x0003C9AE
		// (set) Token: 0x06000A4D RID: 2637 RVA: 0x0003E7A5 File Offset: 0x0003C9A5
		[Parameter(Mandatory = true, ParameterSetName = "name", ValueFromPipeline = true, Position = 0)]
		public string[] Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0003E7B6 File Offset: 0x0003C9B6
		// (set) Token: 0x06000A50 RID: 2640 RVA: 0x0003E7BE File Offset: 0x0003C9BE
		[Parameter(Mandatory = true, ParameterSetName = "FullyQualifiedName", ValueFromPipeline = true, Position = 0)]
		public ModuleSpecification[] FullyQualifiedName { get; set; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0003E7D0 File Offset: 0x0003C9D0
		// (set) Token: 0x06000A51 RID: 2641 RVA: 0x0003E7C7 File Offset: 0x0003C9C7
		[Parameter(Mandatory = true, ParameterSetName = "ModuleInfo", ValueFromPipeline = true, Position = 0)]
		public PSModuleInfo[] ModuleInfo
		{
			get
			{
				return this._moduleInfo;
			}
			set
			{
				this._moduleInfo = value;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0003E7D8 File Offset: 0x0003C9D8
		// (set) Token: 0x06000A54 RID: 2644 RVA: 0x0003E7E5 File Offset: 0x0003C9E5
		[Parameter]
		public SwitchParameter Force
		{
			get
			{
				return base.BaseForce;
			}
			set
			{
				base.BaseForce = value;
			}
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0003E7F4 File Offset: 0x0003C9F4
		protected override void ProcessRecord()
		{
			Dictionary<PSModuleInfo, List<PSModuleInfo>> dictionary = new Dictionary<PSModuleInfo, List<PSModuleInfo>>();
			foreach (PSModuleInfo psmoduleInfo in base.Context.Modules.GetModules(this._name, false))
			{
				dictionary.Add(psmoduleInfo, new List<PSModuleInfo>
				{
					psmoduleInfo
				});
			}
			if (this.FullyQualifiedName != null)
			{
				foreach (PSModuleInfo psmoduleInfo2 in base.Context.Modules.GetModules(this.FullyQualifiedName, false))
				{
					dictionary.Add(psmoduleInfo2, new List<PSModuleInfo>
					{
						psmoduleInfo2
					});
				}
			}
			foreach (PSModuleInfo psmoduleInfo3 in this._moduleInfo)
			{
				dictionary.Add(psmoduleInfo3, new List<PSModuleInfo>
				{
					psmoduleInfo3
				});
			}
			Dictionary<PSModuleInfo, List<PSModuleInfo>> dictionary2 = new Dictionary<PSModuleInfo, List<PSModuleInfo>>();
			foreach (KeyValuePair<PSModuleInfo, List<PSModuleInfo>> keyValuePair in dictionary)
			{
				PSModuleInfo key = keyValuePair.Key;
				if (key.NestedModules != null && key.NestedModules.Count > 0)
				{
					List<PSModuleInfo> value = new List<PSModuleInfo>();
					this.GetAllNestedModules(key, ref value);
					dictionary2.Add(key, value);
				}
			}
			HashSet<PSModuleInfo> hashSet = new HashSet<PSModuleInfo>(new PSModuleInfoComparer());
			if (dictionary2.Count > 0)
			{
				foreach (KeyValuePair<PSModuleInfo, List<PSModuleInfo>> keyValuePair2 in dictionary2)
				{
					List<PSModuleInfo> list = null;
					if (dictionary.TryGetValue(keyValuePair2.Key, out list))
					{
						foreach (PSModuleInfo item in keyValuePair2.Value)
						{
							if (!hashSet.Contains(item))
							{
								list.Add(item);
								hashSet.Add(item);
							}
						}
					}
				}
			}
			Dictionary<PSModuleInfo, List<PSModuleInfo>> dictionary3 = new Dictionary<PSModuleInfo, List<PSModuleInfo>>();
			foreach (KeyValuePair<PSModuleInfo, List<PSModuleInfo>> keyValuePair3 in dictionary)
			{
				List<PSModuleInfo> list2 = new List<PSModuleInfo>();
				for (int j = keyValuePair3.Value.Count - 1; j >= 0; j--)
				{
					PSModuleInfo psmoduleInfo4 = keyValuePair3.Value[j];
					if (psmoduleInfo4.AccessMode == ModuleAccessMode.Constant)
					{
						string message = StringUtil.Format(Modules.ModuleIsConstant, psmoduleInfo4.Name);
						InvalidOperationException exception = new InvalidOperationException(message);
						ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_ModuleIsConstant", ErrorCategory.PermissionDenied, psmoduleInfo4);
						base.WriteError(errorRecord);
					}
					else if (psmoduleInfo4.AccessMode == ModuleAccessMode.ReadOnly && !base.BaseForce)
					{
						string text = StringUtil.Format(Modules.ModuleIsReadOnly, psmoduleInfo4.Name);
						if (InitialSessionState.IsConstantEngineModule(psmoduleInfo4.Name))
						{
							base.WriteWarning(text);
						}
						else
						{
							InvalidOperationException exception2 = new InvalidOperationException(text);
							ErrorRecord errorRecord2 = new ErrorRecord(exception2, "Modules_ModuleIsReadOnly", ErrorCategory.PermissionDenied, psmoduleInfo4);
							base.WriteError(errorRecord2);
						}
					}
					else if (base.ShouldProcess(StringUtil.Format(Modules.ConfirmRemoveModule, psmoduleInfo4.Name, psmoduleInfo4.Path)))
					{
						if (this.ModuleProvidesCurrentSessionDrive(psmoduleInfo4))
						{
							if (!InitialSessionState.IsEngineModule(psmoduleInfo4.Name))
							{
								string text2 = (this._name.Length == 1) ? this._name[0] : psmoduleInfo4.Name;
								PSInvalidOperationException ex = PSTraceSource.NewInvalidOperationException(Modules.ModuleDriveInUse, new object[]
								{
									text2
								});
								throw ex;
							}
							if (!base.BaseForce)
							{
								string text3 = StringUtil.Format(Modules.CoreModuleCannotBeRemoved, psmoduleInfo4.Name);
								base.WriteWarning(text3);
							}
						}
						else
						{
							list2.Add(psmoduleInfo4);
						}
					}
				}
				dictionary3[keyValuePair3.Key] = list2;
			}
			Dictionary<PSModuleInfo, List<PSModuleInfo>> requiredDependencies = this.GetRequiredDependencies();
			foreach (KeyValuePair<PSModuleInfo, List<PSModuleInfo>> keyValuePair4 in dictionary3)
			{
				foreach (PSModuleInfo psmoduleInfo5 in keyValuePair4.Value)
				{
					if (!base.BaseForce)
					{
						List<PSModuleInfo> list3 = null;
						if (requiredDependencies.TryGetValue(psmoduleInfo5, out list3))
						{
							for (int k = list3.Count - 1; k >= 0; k--)
							{
								if (dictionary3.ContainsKey(list3[k]))
								{
									list3.RemoveAt(k);
								}
							}
							if (list3.Count > 0)
							{
								string message2 = StringUtil.Format(Modules.ModuleIsRequired, psmoduleInfo5.Name, list3[0].Name);
								InvalidOperationException exception3 = new InvalidOperationException(message2);
								ErrorRecord errorRecord3 = new ErrorRecord(exception3, "Modules_ModuleIsRequired", ErrorCategory.PermissionDenied, psmoduleInfo5);
								base.WriteError(errorRecord3);
								continue;
							}
						}
					}
					this._numberRemoved++;
					base.RemoveModule(psmoduleInfo5, keyValuePair4.Key.Name);
				}
			}
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0003EDE4 File Offset: 0x0003CFE4
		private bool ModuleProvidesCurrentSessionDrive(PSModuleInfo module)
		{
			if (module.ModuleType == ModuleType.Binary)
			{
				Dictionary<string, List<ProviderInfo>> providers = base.Context.TopLevelSessionState.Providers;
				foreach (KeyValuePair<string, List<ProviderInfo>> keyValuePair in providers)
				{
					foreach (ProviderInfo providerInfo in keyValuePair.Value)
					{
						string assemblyLocation = ClrFacade.GetAssemblyLocation(providerInfo.ImplementingType.GetTypeInfo().Assembly);
						if (assemblyLocation.Equals(module.Path, StringComparison.OrdinalIgnoreCase))
						{
							foreach (PSDriveInfo drive in base.Context.TopLevelSessionState.GetDrivesForProvider(providerInfo.FullName))
							{
								if (drive == base.SessionState.Drive.Current)
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0003EF24 File Offset: 0x0003D124
		private void GetAllNestedModules(PSModuleInfo module, ref List<PSModuleInfo> nestedModulesWithNoCircularReference)
		{
			List<PSModuleInfo> list = new List<PSModuleInfo>();
			if (module.NestedModules != null && module.NestedModules.Count > 0)
			{
				foreach (PSModuleInfo item in module.NestedModules)
				{
					if (!nestedModulesWithNoCircularReference.Contains(item))
					{
						nestedModulesWithNoCircularReference.Add(item);
						list.Add(item);
					}
				}
				foreach (PSModuleInfo module2 in list)
				{
					this.GetAllNestedModules(module2, ref nestedModulesWithNoCircularReference);
				}
			}
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0003EFE4 File Offset: 0x0003D1E4
		private Dictionary<PSModuleInfo, List<PSModuleInfo>> GetRequiredDependencies()
		{
			Dictionary<PSModuleInfo, List<PSModuleInfo>> dictionary = new Dictionary<PSModuleInfo, List<PSModuleInfo>>();
			foreach (PSModuleInfo psmoduleInfo in base.Context.Modules.GetModules(new string[]
			{
				"*"
			}, false))
			{
				foreach (PSModuleInfo key in psmoduleInfo.RequiredModules)
				{
					List<PSModuleInfo> list = null;
					if (!dictionary.TryGetValue(key, out list))
					{
						dictionary.Add(key, list = new List<PSModuleInfo>());
					}
					list.Add(psmoduleInfo);
				}
			}
			return dictionary;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0003F0C4 File Offset: 0x0003D2C4
		protected override void EndProcessing()
		{
			if (this._numberRemoved == 0 && !base.MyInvocation.BoundParameters.ContainsKey("WhatIf"))
			{
				bool flag = true;
				bool flag2 = true;
				foreach (string text in this._name)
				{
					if (!InitialSessionState.IsEngineModule(text))
					{
						flag2 = false;
					}
					if (!WildcardPattern.ContainsWildcardCharacters(text))
					{
						flag = false;
					}
				}
				if (this.FullyQualifiedName != null)
				{
					if (this.FullyQualifiedName.Any((ModuleSpecification moduleSpec) => !InitialSessionState.IsEngineModule(moduleSpec.Name)))
					{
						flag2 = false;
					}
				}
				if (!flag2 && (!flag || this._moduleInfo.Length != 0 || (this.FullyQualifiedName != null && this.FullyQualifiedName.Length != 0)))
				{
					string message = StringUtil.Format(Modules.NoModulesRemoved, new object[0]);
					InvalidOperationException exception = new InvalidOperationException(message);
					ErrorRecord errorRecord = new ErrorRecord(exception, "Modules_NoModulesRemoved", ErrorCategory.ResourceUnavailable, null);
					base.WriteError(errorRecord);
				}
			}
		}

		// Token: 0x040004AD RID: 1197
		private string[] _name = new string[0];

		// Token: 0x040004AE RID: 1198
		private PSModuleInfo[] _moduleInfo = new PSModuleInfo[0];

		// Token: 0x040004AF RID: 1199
		private int _numberRemoved;
	}
}
