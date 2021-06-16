using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Host;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;

namespace System.Management.Automation.Internal.Host
{
	// Token: 0x0200020F RID: 527
	internal class InternalHostUserInterface : PSHostUserInterface, IHostUISupportsMultipleChoiceSelection
	{
		// Token: 0x0600189B RID: 6299 RVA: 0x00095F64 File Offset: 0x00094164
		internal InternalHostUserInterface(PSHostUserInterface externalUI, InternalHost parentHost)
		{
			this.externalUI = externalUI;
			if (parentHost == null)
			{
				throw PSTraceSource.NewArgumentNullException("parentHost");
			}
			this.parent = parentHost;
			PSHostRawUserInterface externalRawUI = null;
			if (externalUI != null)
			{
				externalRawUI = externalUI.RawUI;
			}
			this.internalRawUI = new InternalHostRawUserInterface(externalRawUI, this.parent);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x00095FB1 File Offset: 0x000941B1
		private void ThrowNotInteractive()
		{
			this.internalRawUI.ThrowNotInteractive();
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00095FC0 File Offset: 0x000941C0
		private void ThrowPromptNotInteractive(string promptMessage)
		{
			string message = StringUtil.Format(HostInterfaceExceptionsStrings.HostFunctionPromptNotImplemented, promptMessage);
			HostException ex = new HostException(message, null, "HostFunctionNotImplemented", ErrorCategory.NotImplemented);
			throw ex;
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600189E RID: 6302 RVA: 0x00095FE9 File Offset: 0x000941E9
		public override PSHostRawUserInterface RawUI
		{
			get
			{
				return this.internalRawUI;
			}
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00095FF4 File Offset: 0x000941F4
		public override string ReadLine()
		{
			if (this.externalUI == null)
			{
				this.ThrowNotInteractive();
			}
			string result = null;
			try
			{
				result = this.externalUI.ReadLine();
			}
			catch (PipelineStoppedException)
			{
				LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)this.parent.Context.CurrentRunspace).GetCurrentlyRunningPipeline();
				if (localPipeline == null)
				{
					throw;
				}
				localPipeline.Stopper.Stop();
			}
			return result;
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00096064 File Offset: 0x00094264
		public override SecureString ReadLineAsSecureString()
		{
			if (this.externalUI == null)
			{
				this.ThrowNotInteractive();
			}
			SecureString result = null;
			try
			{
				result = this.externalUI.ReadLineAsSecureString();
			}
			catch (PipelineStoppedException)
			{
				LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)this.parent.Context.CurrentRunspace).GetCurrentlyRunningPipeline();
				if (localPipeline == null)
				{
					throw;
				}
				localPipeline.Stopper.Stop();
			}
			return result;
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x000960D4 File Offset: 0x000942D4
		public override void Write(string value)
		{
			if (value == null)
			{
				return;
			}
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.Write(value);
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x000960EF File Offset: 0x000942EF
		public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
		{
			if (value == null)
			{
				return;
			}
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.Write(foregroundColor, backgroundColor, value);
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0009610C File Offset: 0x0009430C
		public override void WriteLine()
		{
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.WriteLine();
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x00096122 File Offset: 0x00094322
		public override void WriteLine(string value)
		{
			if (value == null)
			{
				return;
			}
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.WriteLine(value);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0009613D File Offset: 0x0009433D
		public override void WriteErrorLine(string value)
		{
			if (value == null)
			{
				return;
			}
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.WriteErrorLine(value);
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x00096158 File Offset: 0x00094358
		public override void WriteLine(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
		{
			if (value == null)
			{
				return;
			}
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.WriteLine(foregroundColor, backgroundColor, value);
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x00096175 File Offset: 0x00094375
		public override void WriteDebugLine(string message)
		{
			this.WriteDebugLineHelper(message);
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x0009617E File Offset: 0x0009437E
		internal void WriteDebugRecord(DebugRecord record)
		{
			this.WriteDebugInfoBuffers(record);
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.WriteDebugLine(record.Message);
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x000961A1 File Offset: 0x000943A1
		internal void WriteDebugInfoBuffers(DebugRecord record)
		{
			if (this.informationalBuffers != null)
			{
				this.informationalBuffers.AddDebug(record);
			}
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x000961B8 File Offset: 0x000943B8
		internal void WriteDebugLine(string message, ref ActionPreference preference)
		{
			switch (preference)
			{
			case ActionPreference.SilentlyContinue:
			case ActionPreference.Ignore:
				return;
			case ActionPreference.Stop:
			{
				this.WriteDebugLineHelper(message);
				string writeDebugLineStoppedError = InternalHostUserInterfaceStrings.WriteDebugLineStoppedError;
				ErrorRecord error = new ErrorRecord(new ParentContainsErrorRecordException(writeDebugLineStoppedError), "ActionPreferenceStop", ErrorCategory.OperationStopped, null);
				ActionPreferenceStopException ex = new ActionPreferenceStopException(error);
				throw ex;
			}
			case ActionPreference.Continue:
				this.WriteDebugLineHelper(message);
				return;
			case ActionPreference.Inquire:
				if (!this.DebugShouldContinue(message, ref preference))
				{
					string writeDebugLineStoppedError = InternalHostUserInterfaceStrings.WriteDebugLineStoppedError;
					ErrorRecord error = new ErrorRecord(new ParentContainsErrorRecordException(writeDebugLineStoppedError), "UserStopRequest", ErrorCategory.OperationStopped, null);
					ActionPreferenceStopException ex2 = new ActionPreferenceStopException(error);
					throw ex2;
				}
				this.WriteDebugLineHelper(message);
				return;
			default:
				throw PSTraceSource.NewArgumentException("preference", InternalHostUserInterfaceStrings.UnsupportedPreferenceError, new object[]
				{
					preference
				});
			}
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x00096276 File Offset: 0x00094476
		internal void SetInformationalMessageBuffers(PSInformationalBuffers informationalBuffers)
		{
			this.informationalBuffers = informationalBuffers;
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x0009627F File Offset: 0x0009447F
		internal PSInformationalBuffers GetInformationalMessageBuffers()
		{
			return this.informationalBuffers;
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x00096287 File Offset: 0x00094487
		private void WriteDebugLineHelper(string message)
		{
			if (message == null)
			{
				return;
			}
			this.WriteDebugRecord(new DebugRecord(message));
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x0009629C File Offset: 0x0009449C
		private bool DebugShouldContinue(string message, ref ActionPreference actionPreference)
		{
			bool result = false;
			Collection<ChoiceDescription> collection = new Collection<ChoiceDescription>();
			collection.Add(new ChoiceDescription(InternalHostUserInterfaceStrings.ShouldContinueYesLabel, InternalHostUserInterfaceStrings.ShouldContinueYesHelp));
			collection.Add(new ChoiceDescription(InternalHostUserInterfaceStrings.ShouldContinueYesToAllLabel, InternalHostUserInterfaceStrings.ShouldContinueYesToAllHelp));
			collection.Add(new ChoiceDescription(InternalHostUserInterfaceStrings.ShouldContinueNoLabel, InternalHostUserInterfaceStrings.ShouldContinueNoHelp));
			collection.Add(new ChoiceDescription(InternalHostUserInterfaceStrings.ShouldContinueNoToAllLabel, InternalHostUserInterfaceStrings.ShouldContinueNoToAllHelp));
			collection.Add(new ChoiceDescription(InternalHostUserInterfaceStrings.ShouldContinueSuspendLabel, InternalHostUserInterfaceStrings.ShouldContinueSuspendHelp));
			bool flag;
			do
			{
				flag = true;
				switch (this.PromptForChoice(InternalHostUserInterfaceStrings.ShouldContinuePromptMessage, message, collection, 0))
				{
				case 0:
					result = true;
					break;
				case 1:
					actionPreference = ActionPreference.Continue;
					result = true;
					break;
				case 2:
					result = false;
					break;
				case 3:
					actionPreference = ActionPreference.Stop;
					result = false;
					break;
				case 4:
					this.parent.EnterNestedPrompt();
					flag = false;
					break;
				}
			}
			while (!flag);
			return result;
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00096370 File Offset: 0x00094570
		public override void WriteProgress(long sourceId, ProgressRecord record)
		{
			if (record == null)
			{
				throw PSTraceSource.NewArgumentNullException("record");
			}
			if (this.informationalBuffers != null)
			{
				this.informationalBuffers.AddProgress(record);
			}
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.WriteProgress(sourceId, record);
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x000963AA File Offset: 0x000945AA
		public override void WriteVerboseLine(string message)
		{
			if (message == null)
			{
				return;
			}
			this.WriteVerboseRecord(new VerboseRecord(message));
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x000963BC File Offset: 0x000945BC
		internal void WriteVerboseRecord(VerboseRecord record)
		{
			this.WriteVerboseInfoBuffers(record);
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.WriteVerboseLine(record.Message);
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x000963DF File Offset: 0x000945DF
		internal void WriteVerboseInfoBuffers(VerboseRecord record)
		{
			if (this.informationalBuffers != null)
			{
				this.informationalBuffers.AddVerbose(record);
			}
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x000963F5 File Offset: 0x000945F5
		public override void WriteWarningLine(string message)
		{
			if (message == null)
			{
				return;
			}
			this.WriteWarningRecord(new WarningRecord(message));
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x00096407 File Offset: 0x00094607
		internal void WriteWarningRecord(WarningRecord record)
		{
			this.WriteWarningInfoBuffers(record);
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.WriteWarningLine(record.Message);
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x0009642A File Offset: 0x0009462A
		internal void WriteWarningInfoBuffers(WarningRecord record)
		{
			if (this.informationalBuffers != null)
			{
				this.informationalBuffers.AddWarning(record);
			}
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x00096440 File Offset: 0x00094640
		internal void WriteInformationRecord(InformationRecord record)
		{
			this.WriteInformationInfoBuffers(record);
			if (this.externalUI == null)
			{
				return;
			}
			this.externalUI.WriteInformation(record);
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0009645E File Offset: 0x0009465E
		internal void WriteInformationInfoBuffers(InformationRecord record)
		{
			if (this.informationalBuffers != null)
			{
				this.informationalBuffers.AddInformation(record);
			}
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x00096474 File Offset: 0x00094674
		internal static Type GetFieldType(FieldDescription field)
		{
			Type result;
			if (TypeResolver.TryResolveType(field.ParameterAssemblyFullName, out result) || TypeResolver.TryResolveType(field.ParameterTypeFullName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x000964A2 File Offset: 0x000946A2
		internal static bool IsSecuritySensitiveType(string typeName)
		{
			return typeName.Equals(typeof(PSCredential).Name, StringComparison.OrdinalIgnoreCase) || typeName.Equals(typeof(SecureString).Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x000964DC File Offset: 0x000946DC
		public override Dictionary<string, PSObject> Prompt(string caption, string message, Collection<FieldDescription> descriptions)
		{
			if (descriptions == null)
			{
				throw PSTraceSource.NewArgumentNullException("descriptions");
			}
			if (descriptions.Count < 1)
			{
				throw PSTraceSource.NewArgumentException("descriptions", InternalHostUserInterfaceStrings.PromptEmptyDescriptionsError, new object[]
				{
					"descriptions"
				});
			}
			if (this.externalUI == null)
			{
				this.ThrowPromptNotInteractive(message);
			}
			Dictionary<string, PSObject> result = null;
			try
			{
				result = this.externalUI.Prompt(caption, message, descriptions);
			}
			catch (PipelineStoppedException)
			{
				LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)this.parent.Context.CurrentRunspace).GetCurrentlyRunningPipeline();
				if (localPipeline == null)
				{
					throw;
				}
				localPipeline.Stopper.Stop();
			}
			return result;
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00096588 File Offset: 0x00094788
		public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice)
		{
			if (this.externalUI == null)
			{
				this.ThrowPromptNotInteractive(message);
			}
			int result = -1;
			try
			{
				result = this.externalUI.PromptForChoice(caption, message, choices, defaultChoice);
			}
			catch (PipelineStoppedException)
			{
				LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)this.parent.Context.CurrentRunspace).GetCurrentlyRunningPipeline();
				if (localPipeline == null)
				{
					throw;
				}
				localPipeline.Stopper.Stop();
			}
			return result;
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00096600 File Offset: 0x00094800
		public Collection<int> PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, IEnumerable<int> defaultChoices)
		{
			if (this.externalUI == null)
			{
				this.ThrowPromptNotInteractive(message);
			}
			IHostUISupportsMultipleChoiceSelection hostUISupportsMultipleChoiceSelection = this.externalUI as IHostUISupportsMultipleChoiceSelection;
			Collection<int> result = null;
			try
			{
				if (hostUISupportsMultipleChoiceSelection == null)
				{
					result = this.EmulatePromptForMultipleChoice(caption, message, choices, defaultChoices);
				}
				else
				{
					result = hostUISupportsMultipleChoiceSelection.PromptForChoice(caption, message, choices, defaultChoices);
				}
			}
			catch (PipelineStoppedException)
			{
				LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)this.parent.Context.CurrentRunspace).GetCurrentlyRunningPipeline();
				if (localPipeline == null)
				{
					throw;
				}
				localPipeline.Stopper.Stop();
			}
			return result;
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x00096690 File Offset: 0x00094890
		private Collection<int> EmulatePromptForMultipleChoice(string caption, string message, Collection<ChoiceDescription> choices, IEnumerable<int> defaultChoices)
		{
			if (choices == null)
			{
				throw PSTraceSource.NewArgumentNullException("choices");
			}
			if (choices.Count == 0)
			{
				throw PSTraceSource.NewArgumentException("choices", InternalHostUserInterfaceStrings.EmptyChoicesError, new object[]
				{
					"choices"
				});
			}
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			if (defaultChoices != null)
			{
				foreach (int num in defaultChoices)
				{
					if (num < 0 || num >= choices.Count)
					{
						throw PSTraceSource.NewArgumentOutOfRangeException("defaultChoice", num, InternalHostUserInterfaceStrings.InvalidDefaultChoiceForMultipleSelection, new object[]
						{
							"defaultChoice",
							"choices",
							num
						});
					}
					if (!dictionary.ContainsKey(num))
					{
						dictionary.Add(num, true);
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			char c = '\n';
			if (!string.IsNullOrEmpty(caption))
			{
				stringBuilder.Append(caption);
				stringBuilder.Append(c);
			}
			if (!string.IsNullOrEmpty(message))
			{
				stringBuilder.Append(message);
				stringBuilder.Append(c);
			}
			string[,] array = null;
			HostUIHelperMethods.BuildHotkeysAndPlainLabels(choices, out array);
			string format = "[{0}] {1}  ";
			for (int i = 0; i < array.GetLength(1); i++)
			{
				string value = string.Format(CultureInfo.InvariantCulture, format, new object[]
				{
					array[0, i],
					array[1, i]
				});
				stringBuilder.Append(value);
				stringBuilder.Append(c);
			}
			string arg = "";
			if (dictionary.Count > 0)
			{
				string text = "";
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (int num2 in dictionary.Keys)
				{
					string text2 = array[0, num2];
					if (string.IsNullOrEmpty(text2))
					{
						text2 = array[1, num2];
					}
					stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "{0}{1}", new object[]
					{
						text,
						text2
					}));
					text = ",";
				}
				string o = stringBuilder2.ToString();
				if (dictionary.Count == 1)
				{
					arg = StringUtil.Format(InternalHostUserInterfaceStrings.DefaultChoice, o);
				}
				else
				{
					arg = StringUtil.Format(InternalHostUserInterfaceStrings.DefaultChoicesForMultipleChoices, o);
				}
			}
			string text3 = stringBuilder.ToString() + arg + c;
			Collection<int> collection = new Collection<int>();
			int num3 = 0;
			for (;;)
			{
				string str = StringUtil.Format(InternalHostUserInterfaceStrings.ChoiceMessage, num3);
				text3 += str;
				this.externalUI.WriteLine(text3);
				string text4 = this.externalUI.ReadLine();
				if (text4.Length == 0)
				{
					if (collection.Count != 0 || dictionary.Keys.Count < 0)
					{
						break;
					}
					using (Dictionary<int, bool>.KeyCollection.Enumerator enumerator3 = dictionary.Keys.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							int item = enumerator3.Current;
							collection.Add(item);
						}
						break;
					}
				}
				int num4 = HostUIHelperMethods.DetermineChoicePicked(text4.Trim(), choices, array);
				if (num4 >= 0)
				{
					collection.Add(num4);
					num3++;
				}
				text3 = "";
			}
			return collection;
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x000969F4 File Offset: 0x00094BF4
		public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
		{
			return this.PromptForCredential(caption, message, userName, targetName, PSCredentialTypes.Default, PSCredentialUIOptions.Default);
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x00096A04 File Offset: 0x00094C04
		public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
		{
			if (this.externalUI == null)
			{
				this.ThrowPromptNotInteractive(message);
			}
			PSCredential result = null;
			try
			{
				result = this.externalUI.PromptForCredential(caption, message, userName, targetName, allowedCredentialTypes, options);
			}
			catch (PipelineStoppedException)
			{
				LocalPipeline localPipeline = (LocalPipeline)((RunspaceBase)this.parent.Context.CurrentRunspace).GetCurrentlyRunningPipeline();
				if (localPipeline == null)
				{
					throw;
				}
				localPipeline.Stopper.Stop();
			}
			return result;
		}

		// Token: 0x04000A37 RID: 2615
		private PSHostUserInterface externalUI;

		// Token: 0x04000A38 RID: 2616
		private InternalHostRawUserInterface internalRawUI;

		// Token: 0x04000A39 RID: 2617
		private InternalHost parent;

		// Token: 0x04000A3A RID: 2618
		private PSInformationalBuffers informationalBuffers;
	}
}
