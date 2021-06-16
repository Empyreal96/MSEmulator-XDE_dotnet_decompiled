using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Help;
using System.Management.Automation.Internal;
using System.Runtime.InteropServices;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x020001AC RID: 428
	[Cmdlet("Get", "Help", DefaultParameterSetName = "AllUsersView", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=113316")]
	public sealed class GetHelpCommand : PSCmdlet
	{
		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x0007A242 File Offset: 0x00078442
		// (set) Token: 0x060013FA RID: 5114 RVA: 0x0007A24A File Offset: 0x0007844A
		[Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
		public string Name
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

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x0007A253 File Offset: 0x00078453
		// (set) Token: 0x060013FC RID: 5116 RVA: 0x0007A25B File Offset: 0x0007845B
		[Parameter]
		public string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x0007A264 File Offset: 0x00078464
		// (set) Token: 0x060013FE RID: 5118 RVA: 0x0007A26C File Offset: 0x0007846C
		[Parameter]
		[ValidateSet(new string[]
		{
			"Alias",
			"Cmdlet",
			"Provider",
			"General",
			"FAQ",
			"Glossary",
			"HelpFile",
			"ScriptCommand",
			"Function",
			"Filter",
			"ExternalScript",
			"All",
			"DefaultHelp",
			"Workflow",
			"DscResource",
			"Class",
			"Configuration"
		}, IgnoreCase = true)]
		public string[] Category
		{
			get
			{
				return this._category;
			}
			set
			{
				this._category = value;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x0007A275 File Offset: 0x00078475
		// (set) Token: 0x06001400 RID: 5120 RVA: 0x0007A27D File Offset: 0x0007847D
		[Parameter]
		public string[] Component
		{
			get
			{
				return this._component;
			}
			set
			{
				this._component = value;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x0007A286 File Offset: 0x00078486
		// (set) Token: 0x06001402 RID: 5122 RVA: 0x0007A28E File Offset: 0x0007848E
		[Parameter]
		public string[] Functionality
		{
			get
			{
				return this._functionality;
			}
			set
			{
				this._functionality = value;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x0007A297 File Offset: 0x00078497
		// (set) Token: 0x06001404 RID: 5124 RVA: 0x0007A29F File Offset: 0x0007849F
		[Parameter]
		public string[] Role
		{
			get
			{
				return this._role;
			}
			set
			{
				this._role = value;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (set) Token: 0x06001405 RID: 5125 RVA: 0x0007A2A8 File Offset: 0x000784A8
		[Parameter(ParameterSetName = "DetailedView", Mandatory = true)]
		public SwitchParameter Detailed
		{
			set
			{
				if (value.ToBool())
				{
					this._viewTokenToAdd = GetHelpCommand.HelpView.DetailedView;
				}
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (set) Token: 0x06001406 RID: 5126 RVA: 0x0007A2BA File Offset: 0x000784BA
		[Parameter(ParameterSetName = "AllUsersView")]
		public SwitchParameter Full
		{
			set
			{
				if (value.ToBool())
				{
					this._viewTokenToAdd = GetHelpCommand.HelpView.FullView;
				}
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (set) Token: 0x06001407 RID: 5127 RVA: 0x0007A2CC File Offset: 0x000784CC
		[Parameter(ParameterSetName = "Examples", Mandatory = true)]
		public SwitchParameter Examples
		{
			set
			{
				if (value.ToBool())
				{
					this._viewTokenToAdd = GetHelpCommand.HelpView.ExamplesView;
				}
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x0007A2E7 File Offset: 0x000784E7
		// (set) Token: 0x06001408 RID: 5128 RVA: 0x0007A2DE File Offset: 0x000784DE
		[Parameter(ParameterSetName = "Parameters", Mandatory = true)]
		public string Parameter
		{
			get
			{
				return this._parameter;
			}
			set
			{
				this._parameter = value;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x0007A310 File Offset: 0x00078510
		// (set) Token: 0x0600140A RID: 5130 RVA: 0x0007A2EF File Offset: 0x000784EF
		[Parameter(ParameterSetName = "Online", Mandatory = true)]
		public SwitchParameter Online
		{
			get
			{
				return this.showOnlineHelp;
			}
			set
			{
				this.showOnlineHelp = value;
				if (this.showOnlineHelp)
				{
					GetHelpCommand.VerifyParameterForbiddenInRemoteRunspace(this, "Online");
				}
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x0007A31D File Offset: 0x0007851D
		// (set) Token: 0x0600140D RID: 5133 RVA: 0x0007A32A File Offset: 0x0007852A
		[Parameter(ParameterSetName = "ShowWindow", Mandatory = true)]
		public SwitchParameter ShowWindow
		{
			get
			{
				return this.showWindow;
			}
			set
			{
				this.showWindow = value;
				if (this.showWindow)
				{
					GetHelpCommand.VerifyParameterForbiddenInRemoteRunspace(this, "ShowWindow");
				}
			}
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0007A34C File Offset: 0x0007854C
		protected override void BeginProcessing()
		{
			if (!this.Online.IsPresent && UpdatableHelpSystem.ShouldPromptToUpdateHelp() && HostUtilities.IsProcessInteractive(base.MyInvocation) && this.HasInternetConnection())
			{
				if (base.ShouldContinue(HelpDisplayStrings.UpdateHelpPromptBody, HelpDisplayStrings.UpdateHelpPromptTitle))
				{
					PowerShell.Create(RunspaceMode.CurrentRunspace).AddCommand("Update-Help").Invoke();
				}
				UpdatableHelpSystem.SetDisablePromptToUpdateHelp();
			}
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0007A3B4 File Offset: 0x000785B4
		protected override void ProcessRecord()
		{
			try
			{
				if (this.ShowWindow)
				{
					this.graphicalHostReflectionWrapper = GraphicalHostReflectionWrapper.GetGraphicalHostReflectionWrapper(this, "Microsoft.PowerShell.Commands.Internal.HelpWindowHelper");
				}
				base.Context.HelpSystem.OnProgress += this.HelpSystem_OnProgress;
				bool flag = false;
				HelpCategory helpCategory = this.ToHelpCategory(this._category, ref flag);
				if (!flag)
				{
					this.ValidateAndThrowIfError(helpCategory);
					HelpRequest helpRequest = new HelpRequest(this.Name, helpCategory);
					helpRequest.Provider = this._provider;
					helpRequest.Component = this._component;
					helpRequest.Role = this._role;
					helpRequest.Functionality = this._functionality;
					helpRequest.ProviderContext = new ProviderContext(this.Path, base.Context.Engine.Context, base.SessionState.Path);
					helpRequest.CommandOrigin = base.MyInvocation.CommandOrigin;
					IEnumerable<HelpInfo> help = base.Context.HelpSystem.GetHelp(helpRequest);
					HelpInfo helpInfo = null;
					int num = 0;
					foreach (HelpInfo helpInfo2 in help)
					{
						if (base.IsStopping)
						{
							return;
						}
						if (num == 0)
						{
							helpInfo = helpInfo2;
						}
						else
						{
							if (helpInfo != null)
							{
								this.WriteObjectsOrShowOnlineHelp(helpInfo, false);
								helpInfo = null;
							}
							this.WriteObjectsOrShowOnlineHelp(helpInfo2, false);
						}
						num++;
					}
					if (1 == num)
					{
						this.WriteObjectsOrShowOnlineHelp(helpInfo, true);
					}
					else if (this.showOnlineHelp && num > 1)
					{
						throw PSTraceSource.NewInvalidOperationException(HelpErrors.MultipleOnlineTopicsNotSupported, new object[]
						{
							"Online"
						});
					}
					if (((num == 0 && !WildcardPattern.ContainsWildcardCharacters(helpRequest.Target)) || base.Context.HelpSystem.VerboseHelpErrors) && base.Context.HelpSystem.LastErrors.Count > 0)
					{
						foreach (ErrorRecord errorRecord in base.Context.HelpSystem.LastErrors)
						{
							base.WriteError(errorRecord);
						}
					}
				}
			}
			finally
			{
				base.Context.HelpSystem.OnProgress -= this.HelpSystem_OnProgress;
				base.Context.HelpSystem.ClearScriptBlockTokenCache();
			}
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0007A640 File Offset: 0x00078840
		private HelpCategory ToHelpCategory(string[] category, ref bool failed)
		{
			if (category == null || category.Length == 0)
			{
				return HelpCategory.None;
			}
			HelpCategory helpCategory = HelpCategory.None;
			failed = false;
			for (int i = 0; i < category.Length; i++)
			{
				try
				{
					HelpCategory helpCategory2 = (HelpCategory)Enum.Parse(typeof(HelpCategory), category[i], true);
					helpCategory |= helpCategory2;
				}
				catch (ArgumentException innerException)
				{
					Exception exception = new HelpCategoryInvalidException(category[i], innerException);
					ErrorRecord errorRecord = new ErrorRecord(exception, "InvalidHelpCategory", ErrorCategory.InvalidArgument, null);
					base.WriteError(errorRecord);
					failed = true;
				}
			}
			return helpCategory;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0007A6C4 File Offset: 0x000788C4
		private PSObject TransformView(PSObject originalHelpObject)
		{
			if (this._viewTokenToAdd == GetHelpCommand.HelpView.Default)
			{
				GetHelpCommand.tracer.WriteLine("Detailed, Full, Examples are not selected. Constructing default view.", new object[0]);
				return originalHelpObject;
			}
			string text = this._viewTokenToAdd.ToString();
			PSObject psobject = originalHelpObject.Copy();
			psobject.TypeNames.Clear();
			if (originalHelpObject.TypeNames.Count == 0)
			{
				string item = string.Format(CultureInfo.InvariantCulture, "HelpInfo#{0}", new object[]
				{
					text
				});
				psobject.TypeNames.Add(item);
			}
			else
			{
				foreach (string text2 in originalHelpObject.TypeNames)
				{
					if (!text2.ToLowerInvariant().Equals("system.string") && !text2.ToLowerInvariant().Equals("system.object"))
					{
						string text3 = string.Format(CultureInfo.InvariantCulture, "{0}#{1}", new object[]
						{
							text2,
							text
						});
						GetHelpCommand.tracer.WriteLine("Adding type {0}", new object[]
						{
							text3
						});
						psobject.TypeNames.Add(text3);
					}
				}
				foreach (string text4 in originalHelpObject.TypeNames)
				{
					GetHelpCommand.tracer.WriteLine("Adding type {0}", new object[]
					{
						text4
					});
					psobject.TypeNames.Add(text4);
				}
			}
			return psobject;
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0007A870 File Offset: 0x00078A70
		private void GetAndWriteParameterInfo(HelpInfo helpInfo)
		{
			GetHelpCommand.tracer.WriteLine("Searching parameters for {0}", new object[]
			{
				helpInfo.Name
			});
			PSObject[] parameter = helpInfo.GetParameter(this._parameter);
			if (parameter == null || parameter.Length == 0)
			{
				Exception exception = PSTraceSource.NewArgumentException("Parameter", HelpErrors.NoParmsFound, new object[]
				{
					this._parameter
				});
				base.WriteError(new ErrorRecord(exception, "NoParmsFound", ErrorCategory.InvalidArgument, helpInfo));
				return;
			}
			foreach (PSObject sendToPipeline in parameter)
			{
				base.WriteObject(sendToPipeline);
			}
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0007A910 File Offset: 0x00078B10
		private void ValidateAndThrowIfError(HelpCategory cat)
		{
			if (cat == HelpCategory.None)
			{
				return;
			}
			HelpCategory helpCategory = HelpCategory.Alias | HelpCategory.Cmdlet | HelpCategory.ScriptCommand | HelpCategory.Function | HelpCategory.Filter | HelpCategory.ExternalScript | HelpCategory.Workflow;
			if ((cat & helpCategory) == HelpCategory.None)
			{
				if (!string.IsNullOrEmpty(this._parameter))
				{
					throw PSTraceSource.NewArgumentException("Parameter", HelpErrors.ParamNotSupported, new object[]
					{
						"-Parameter"
					});
				}
				if (this._component != null)
				{
					throw PSTraceSource.NewArgumentException("Component", HelpErrors.ParamNotSupported, new object[]
					{
						"-Component"
					});
				}
				if (this._role != null)
				{
					throw PSTraceSource.NewArgumentException("Role", HelpErrors.ParamNotSupported, new object[]
					{
						"-Role"
					});
				}
				if (this._functionality != null)
				{
					throw PSTraceSource.NewArgumentException("Functionality", HelpErrors.ParamNotSupported, new object[]
					{
						"-Functionality"
					});
				}
			}
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0007A9D8 File Offset: 0x00078BD8
		private void WriteObjectsOrShowOnlineHelp(HelpInfo helpInfo, bool showFullHelp)
		{
			if (helpInfo != null)
			{
				if (showFullHelp && this.showOnlineHelp)
				{
					bool flag = false;
					GetHelpCommand.tracer.WriteLine("Preparing to show help online.", new object[0]);
					Uri uriForOnlineHelp = helpInfo.GetUriForOnlineHelp();
					if (null != uriForOnlineHelp)
					{
						this.LaunchOnlineHelp(uriForOnlineHelp);
						return;
					}
					if (!flag)
					{
						throw PSTraceSource.NewInvalidOperationException(HelpErrors.NoURIFound, new object[0]);
					}
				}
				else
				{
					if (showFullHelp && this.ShowWindow)
					{
						this.graphicalHostReflectionWrapper.CallStaticMethod("ShowHelpWindow", new object[]
						{
							helpInfo.FullHelp,
							this
						});
						return;
					}
					if (showFullHelp)
					{
						if (!string.IsNullOrEmpty(this._parameter))
						{
							this.GetAndWriteParameterInfo(helpInfo);
							return;
						}
						PSObject psobject = this.TransformView(helpInfo.FullHelp);
						psobject.IsHelpObject = true;
						base.WriteObject(psobject);
						return;
					}
					else
					{
						if (!string.IsNullOrEmpty(this._parameter))
						{
							PSObject[] parameter = helpInfo.GetParameter(this._parameter);
							if (parameter == null || parameter.Length == 0)
							{
								return;
							}
						}
						base.WriteObject(helpInfo.ShortHelp);
					}
				}
			}
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0007AADC File Offset: 0x00078CDC
		private void LaunchOnlineHelp(Uri uriToLaunch)
		{
			if (!uriToLaunch.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) && !uriToLaunch.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
			{
				throw PSTraceSource.NewInvalidOperationException(HelpErrors.ProtocolNotSupported, new object[]
				{
					uriToLaunch.ToString(),
					"http",
					"https"
				});
			}
			Exception ex = null;
			try
			{
				base.WriteVerbose(string.Format(CultureInfo.InvariantCulture, HelpDisplayStrings.OnlineHelpUri, new object[]
				{
					uriToLaunch.OriginalString
				}));
				new Process
				{
					StartInfo = 
					{
						UseShellExecute = true,
						FileName = uriToLaunch.OriginalString
					}
				}.Start();
			}
			catch (InvalidOperationException ex2)
			{
				ex = ex2;
			}
			catch (Win32Exception ex3)
			{
				ex = ex3;
			}
			if (ex != null)
			{
				throw PSTraceSource.NewInvalidOperationException(ex, HelpErrors.CannotLaunchURI, new object[]
				{
					uriToLaunch.OriginalString
				});
			}
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0007ABE0 File Offset: 0x00078DE0
		private void HelpSystem_OnProgress(object sender, HelpProgressInfo arg)
		{
			base.WriteProgress(new ProgressRecord(0, base.CommandInfo.Name, arg.Activity)
			{
				PercentComplete = arg.PercentComplete
			});
		}

		// Token: 0x06001417 RID: 5143
		[DllImport("wininet.dll")]
		private static extern bool InternetGetConnectedState(out int desc, int reserved);

		// Token: 0x06001418 RID: 5144 RVA: 0x0007AC18 File Offset: 0x00078E18
		private bool HasInternetConnection()
		{
			int num;
			return GetHelpCommand.InternetGetConnectedState(out num, 0);
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0007AC30 File Offset: 0x00078E30
		internal static void VerifyParameterForbiddenInRemoteRunspace(Cmdlet cmdlet, string parameterName)
		{
			if (NativeCommandProcessor.IsServerSide)
			{
				string message = StringUtil.Format(CommandBaseStrings.ParameterNotValidInRemoteRunspace, cmdlet.MyInvocation.InvocationName, parameterName);
				Exception exception = new InvalidOperationException(message);
				ErrorRecord errorRecord = new ErrorRecord(exception, "ParameterNotValidInRemoteRunspace", ErrorCategory.InvalidArgument, null);
				cmdlet.ThrowTerminatingError(errorRecord);
			}
		}

		// Token: 0x04000896 RID: 2198
		private string _name = "";

		// Token: 0x04000897 RID: 2199
		private string _path;

		// Token: 0x04000898 RID: 2200
		private string[] _category;

		// Token: 0x04000899 RID: 2201
		private string[] _component;

		// Token: 0x0400089A RID: 2202
		private string[] _functionality;

		// Token: 0x0400089B RID: 2203
		private string[] _role;

		// Token: 0x0400089C RID: 2204
		private string _provider = "";

		// Token: 0x0400089D RID: 2205
		private string _parameter;

		// Token: 0x0400089E RID: 2206
		private bool showOnlineHelp;

		// Token: 0x0400089F RID: 2207
		private bool showWindow;

		// Token: 0x040008A0 RID: 2208
		private GetHelpCommand.HelpView _viewTokenToAdd;

		// Token: 0x040008A1 RID: 2209
		private GraphicalHostReflectionWrapper graphicalHostReflectionWrapper;

		// Token: 0x040008A2 RID: 2210
		[TraceSource("GetHelpCommand ", "GetHelpCommand ")]
		private static PSTraceSource tracer = PSTraceSource.GetTracer("GetHelpCommand ", "GetHelpCommand ");

		// Token: 0x020001AD RID: 429
		internal enum HelpView
		{
			// Token: 0x040008A4 RID: 2212
			Default,
			// Token: 0x040008A5 RID: 2213
			DetailedView,
			// Token: 0x040008A6 RID: 2214
			FullView,
			// Token: 0x040008A7 RID: 2215
			ExamplesView
		}
	}
}
