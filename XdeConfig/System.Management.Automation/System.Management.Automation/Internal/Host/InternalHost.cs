using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Internal.Host
{
	// Token: 0x02000209 RID: 521
	internal class InternalHost : PSHost, IHostSupportsInteractiveSession
	{
		// Token: 0x06001812 RID: 6162 RVA: 0x000943AC File Offset: 0x000925AC
		internal InternalHost(PSHost externalHost, ExecutionContext executionContext)
		{
			this.externalHostRef = new ObjectRef<PSHost>(externalHost);
			this.executionContext = executionContext;
			PSHostUserInterface ui = externalHost.UI;
			this.internalUIRef = new ObjectRef<InternalHostUserInterface>(new InternalHostUserInterface(ui, this));
			this.zeroGuid = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			this.idResult = this.zeroGuid;
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001813 RID: 6163 RVA: 0x0009441F File Offset: 0x0009261F
		public override string Name
		{
			get
			{
				if (string.IsNullOrEmpty(this.nameResult))
				{
					this.nameResult = this.externalHostRef.Value.Name;
					if (string.IsNullOrEmpty(this.nameResult))
					{
						throw PSTraceSource.NewNotImplementedException();
					}
				}
				return this.nameResult;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x0009445D File Offset: 0x0009265D
		public override Version Version
		{
			get
			{
				if (this.versionResult == null)
				{
					this.versionResult = this.externalHostRef.Value.Version;
					if (this.versionResult == null)
					{
						throw PSTraceSource.NewNotImplementedException();
					}
				}
				return this.versionResult;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001815 RID: 6165 RVA: 0x000944A0 File Offset: 0x000926A0
		public override Guid InstanceId
		{
			get
			{
				if (this.idResult == this.zeroGuid)
				{
					this.idResult = this.externalHostRef.Value.InstanceId;
					if (this.idResult == this.zeroGuid)
					{
						throw PSTraceSource.NewNotImplementedException();
					}
				}
				return this.idResult;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001816 RID: 6166 RVA: 0x000944F5 File Offset: 0x000926F5
		public override PSHostUserInterface UI
		{
			get
			{
				return this.internalUIRef.Value;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001817 RID: 6167 RVA: 0x00094502 File Offset: 0x00092702
		internal InternalHostUserInterface InternalUI
		{
			get
			{
				return this.internalUIRef.Value;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001818 RID: 6168 RVA: 0x00094510 File Offset: 0x00092710
		public override CultureInfo CurrentCulture
		{
			get
			{
				CultureInfo cultureInfo = this.externalHostRef.Value.CurrentCulture;
				if (cultureInfo == null)
				{
					cultureInfo = CultureInfo.InvariantCulture;
				}
				return cultureInfo;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001819 RID: 6169 RVA: 0x00094538 File Offset: 0x00092738
		public override CultureInfo CurrentUICulture
		{
			get
			{
				return this.externalHostRef.Value.CurrentUICulture ?? CultureInfo.InstalledUICulture;
			}
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x00094560 File Offset: 0x00092760
		public override void SetShouldExit(int exitCode)
		{
			this.externalHostRef.Value.SetShouldExit(exitCode);
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x00094573 File Offset: 0x00092773
		public override void EnterNestedPrompt()
		{
			this.EnterNestedPrompt(null);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0009457C File Offset: 0x0009277C
		internal void EnterNestedPrompt(InternalCommand callingCommand)
		{
			LocalRunspace localRunspace = null;
			try
			{
				localRunspace = (this.Runspace as LocalRunspace);
			}
			catch (PSNotImplementedException)
			{
			}
			if (localRunspace != null)
			{
				Pipeline currentlyRunningPipeline = this.Runspace.GetCurrentlyRunningPipeline();
				if (currentlyRunningPipeline != null && currentlyRunningPipeline == localRunspace.PulsePipeline)
				{
					throw new InvalidOperationException();
				}
			}
			if (this.nestedPromptCount < 0)
			{
				throw PSTraceSource.NewInvalidOperationException(InternalHostStrings.EnterExitNestedPromptOutOfSync, new object[0]);
			}
			this.nestedPromptCount++;
			this.executionContext.SetVariable(SpecialVariables.NestedPromptCounterVarPath, this.nestedPromptCount);
			InternalHost.PromptContextData item = default(InternalHost.PromptContextData);
			item.SavedContextData = this.executionContext.SaveContextData();
			item.SavedCurrentlyExecutingCommandVarValue = this.executionContext.GetVariableValue(SpecialVariables.CurrentlyExecutingCommandVarPath);
			item.SavedPSBoundParametersVarValue = this.executionContext.GetVariableValue(SpecialVariables.PSBoundParametersVarPath);
			item.RunspaceAvailability = this.Context.CurrentRunspace.RunspaceAvailability;
			item.LanguageMode = this.executionContext.LanguageMode;
			PSPropertyInfo pspropertyInfo = null;
			PSPropertyInfo pspropertyInfo2 = null;
			object value = null;
			object value2 = null;
			if (callingCommand != null)
			{
				PSObject psobject = PSObject.AsPSObject(callingCommand);
				pspropertyInfo = psobject.Properties["CommandInfo"];
				if (pspropertyInfo == null)
				{
					psobject.Properties.Add(new PSNoteProperty("CommandInfo", callingCommand.CommandInfo));
				}
				else
				{
					value = pspropertyInfo.Value;
					pspropertyInfo.Value = callingCommand.CommandInfo;
				}
				pspropertyInfo2 = psobject.Properties["StackTrace"];
				if (pspropertyInfo2 == null)
				{
					psobject.Properties.Add(new PSNoteProperty("StackTrace", new StackTrace()));
				}
				else
				{
					value2 = pspropertyInfo2.Value;
					pspropertyInfo2.Value = new StackTrace();
				}
				this.executionContext.SetVariable(SpecialVariables.CurrentlyExecutingCommandVarPath, psobject);
			}
			this.contextStack.Push(item);
			this.executionContext.PSDebugTraceStep = false;
			this.executionContext.PSDebugTraceLevel = 0;
			this.executionContext.ResetShellFunctionErrorOutputPipe();
			if (this.executionContext.HasRunspaceEverUsedConstrainedLanguageMode)
			{
				this.executionContext.LanguageMode = PSLanguageMode.ConstrainedLanguage;
			}
			this.Context.CurrentRunspace.UpdateRunspaceAvailability(RunspaceAvailability.AvailableForNestedCommand, true);
			try
			{
				this.externalHostRef.Value.EnterNestedPrompt();
			}
			catch
			{
				this.ExitNestedPromptHelper();
				throw;
			}
			finally
			{
				if (pspropertyInfo != null)
				{
					pspropertyInfo.Value = value;
				}
				if (pspropertyInfo2 != null)
				{
					pspropertyInfo2.Value = value2;
				}
			}
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x000947E4 File Offset: 0x000929E4
		private void ExitNestedPromptHelper()
		{
			this.nestedPromptCount--;
			this.executionContext.SetVariable(SpecialVariables.NestedPromptCounterVarPath, this.nestedPromptCount);
			if (this.contextStack.Count > 0)
			{
				InternalHost.PromptContextData promptContextData = this.contextStack.Pop();
				promptContextData.SavedContextData.RestoreContextData(this.executionContext);
				this.executionContext.LanguageMode = promptContextData.LanguageMode;
				this.executionContext.SetVariable(SpecialVariables.CurrentlyExecutingCommandVarPath, promptContextData.SavedCurrentlyExecutingCommandVarValue);
				this.executionContext.SetVariable(SpecialVariables.PSBoundParametersVarPath, promptContextData.SavedPSBoundParametersVarValue);
				this.Context.CurrentRunspace.UpdateRunspaceAvailability(promptContextData.RunspaceAvailability, true);
			}
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x000948A0 File Offset: 0x00092AA0
		public override void ExitNestedPrompt()
		{
			if (this.nestedPromptCount == 0)
			{
				return;
			}
			try
			{
				this.externalHostRef.Value.ExitNestedPrompt();
			}
			finally
			{
				this.ExitNestedPromptHelper();
			}
			ExitNestedPromptException ex = new ExitNestedPromptException();
			throw ex;
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x0600181F RID: 6175 RVA: 0x000948E8 File Offset: 0x00092AE8
		public override PSObject PrivateData
		{
			get
			{
				return this.externalHostRef.Value.PrivateData;
			}
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00094907 File Offset: 0x00092B07
		public override void NotifyBeginApplication()
		{
			this.externalHostRef.Value.NotifyBeginApplication();
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00094919 File Offset: 0x00092B19
		public override void NotifyEndApplication()
		{
			this.externalHostRef.Value.NotifyEndApplication();
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001822 RID: 6178 RVA: 0x0009492B File Offset: 0x00092B2B
		// (set) Token: 0x06001823 RID: 6179 RVA: 0x00094933 File Offset: 0x00092B33
		public override bool DebuggerEnabled
		{
			get
			{
				return this.isDebuggingEnabled;
			}
			set
			{
				this.isDebuggingEnabled = value;
			}
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x0009493C File Offset: 0x00092B3C
		private IHostSupportsInteractiveSession GetIHostSupportsInteractiveSession()
		{
			IHostSupportsInteractiveSession hostSupportsInteractiveSession = this.externalHostRef.Value as IHostSupportsInteractiveSession;
			if (hostSupportsInteractiveSession == null)
			{
				throw new PSNotImplementedException();
			}
			return hostSupportsInteractiveSession;
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x00094964 File Offset: 0x00092B64
		public void PushRunspace(Runspace runspace)
		{
			IHostSupportsInteractiveSession ihostSupportsInteractiveSession = this.GetIHostSupportsInteractiveSession();
			ihostSupportsInteractiveSession.PushRunspace(runspace);
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x00094980 File Offset: 0x00092B80
		public void PopRunspace()
		{
			IHostSupportsInteractiveSession ihostSupportsInteractiveSession = this.GetIHostSupportsInteractiveSession();
			ihostSupportsInteractiveSession.PopRunspace();
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001827 RID: 6183 RVA: 0x0009499C File Offset: 0x00092B9C
		public bool IsRunspacePushed
		{
			get
			{
				IHostSupportsInteractiveSession ihostSupportsInteractiveSession = this.GetIHostSupportsInteractiveSession();
				return ihostSupportsInteractiveSession.IsRunspacePushed;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x000949B8 File Offset: 0x00092BB8
		public Runspace Runspace
		{
			get
			{
				IHostSupportsInteractiveSession ihostSupportsInteractiveSession = this.GetIHostSupportsInteractiveSession();
				return ihostSupportsInteractiveSession.Runspace;
			}
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x000949D2 File Offset: 0x00092BD2
		internal bool HostInNestedPrompt()
		{
			return this.nestedPromptCount > 0;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x000949E0 File Offset: 0x00092BE0
		internal void SetHostRef(PSHost psHost)
		{
			this.externalHostRef.Override(psHost);
			this.internalUIRef.Override(new InternalHostUserInterface(psHost.UI, this));
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x00094A05 File Offset: 0x00092C05
		internal void RevertHostRef()
		{
			if (!this.IsHostRefSet)
			{
				return;
			}
			this.externalHostRef.Revert();
			this.internalUIRef.Revert();
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600182C RID: 6188 RVA: 0x00094A26 File Offset: 0x00092C26
		internal bool IsHostRefSet
		{
			get
			{
				return this.externalHostRef.IsOverridden;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600182D RID: 6189 RVA: 0x00094A33 File Offset: 0x00092C33
		internal ExecutionContext Context
		{
			get
			{
				return this.executionContext;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x0600182E RID: 6190 RVA: 0x00094A3B File Offset: 0x00092C3B
		internal PSHost ExternalHost
		{
			get
			{
				return this.externalHostRef.Value;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x0600182F RID: 6191 RVA: 0x00094A48 File Offset: 0x00092C48
		internal int NestedPromptCount
		{
			get
			{
				return this.nestedPromptCount;
			}
		}

		// Token: 0x04000A22 RID: 2594
		private ObjectRef<PSHost> externalHostRef;

		// Token: 0x04000A23 RID: 2595
		private ObjectRef<InternalHostUserInterface> internalUIRef;

		// Token: 0x04000A24 RID: 2596
		private ExecutionContext executionContext;

		// Token: 0x04000A25 RID: 2597
		private string nameResult;

		// Token: 0x04000A26 RID: 2598
		private Version versionResult;

		// Token: 0x04000A27 RID: 2599
		private Guid idResult;

		// Token: 0x04000A28 RID: 2600
		private int nestedPromptCount;

		// Token: 0x04000A29 RID: 2601
		private Stack<InternalHost.PromptContextData> contextStack = new Stack<InternalHost.PromptContextData>();

		// Token: 0x04000A2A RID: 2602
		private bool isDebuggingEnabled = true;

		// Token: 0x04000A2B RID: 2603
		private readonly Guid zeroGuid;

		// Token: 0x0200020A RID: 522
		private struct PromptContextData
		{
			// Token: 0x04000A2C RID: 2604
			public object SavedCurrentlyExecutingCommandVarValue;

			// Token: 0x04000A2D RID: 2605
			public object SavedPSBoundParametersVarValue;

			// Token: 0x04000A2E RID: 2606
			public ExecutionContext.SavedContextData SavedContextData;

			// Token: 0x04000A2F RID: 2607
			public RunspaceAvailability RunspaceAvailability;

			// Token: 0x04000A30 RID: 2608
			public PSLanguageMode LanguageMode;
		}
	}
}
