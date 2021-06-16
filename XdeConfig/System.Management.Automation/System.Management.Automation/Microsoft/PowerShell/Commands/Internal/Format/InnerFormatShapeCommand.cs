using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004CF RID: 1231
	internal class InnerFormatShapeCommand : InnerFormatShapeCommandBase
	{
		// Token: 0x060035C5 RID: 13765 RVA: 0x001244A5 File Offset: 0x001226A5
		internal InnerFormatShapeCommand(FormatShape shape)
		{
			this.shape = shape;
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x001244C8 File Offset: 0x001226C8
		internal static int FormatEnumerationLimit()
		{
			object obj = null;
			try
			{
				if (LocalPipeline.GetExecutionContextFromTLS() != null)
				{
					obj = LocalPipeline.GetExecutionContextFromTLS().SessionState.PSVariable.GetValue("global:FormatEnumerationLimit");
				}
			}
			catch (ProviderNotFoundException)
			{
			}
			catch (ProviderInvocationException)
			{
			}
			if (!(obj is int))
			{
				return 4;
			}
			return (int)obj;
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x0012452C File Offset: 0x0012272C
		internal override void BeginProcessing()
		{
			base.BeginProcessing();
			this.enumerationLimit = InnerFormatShapeCommand.FormatEnumerationLimit();
			this.expressionFactory = new MshExpressionFactory();
			this.formatObjectDeserializer = new FormatObjectDeserializer(base.TerminatingErrorContext);
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x0012455C File Offset: 0x0012275C
		internal override void ProcessRecord()
		{
			this._typeInfoDataBase = this.OuterCmdlet().Context.FormatDBManager.GetTypeInfoDataBase();
			PSObject psobject = this.ReadObject();
			if (psobject == null || psobject == AutomationNull.Value)
			{
				return;
			}
			IEnumerable enumerable = PSObjectHelper.GetEnumerable(psobject);
			if (enumerable == null)
			{
				this.ProcessObject(psobject);
				return;
			}
			switch (this.GetExpansionState(psobject))
			{
			case EnumerableExpansion.EnumOnly:
				using (IEnumerator enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						this.ProcessObject(PSObjectHelper.AsPSObject(obj));
					}
					return;
				}
				break;
			case EnumerableExpansion.Both:
				break;
			default:
				goto IL_126;
			}
			int num = 0;
			foreach (object obj2 in enumerable)
			{
				num++;
			}
			this.ProcessCoreOutOfBand(psobject, num);
			using (IEnumerator enumerator3 = enumerable.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					object obj3 = enumerator3.Current;
					this.ProcessObject(PSObjectHelper.AsPSObject(obj3));
				}
				return;
			}
			IL_126:
			this.ProcessObject(psobject);
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x001246C0 File Offset: 0x001228C0
		private EnumerableExpansion GetExpansionState(PSObject so)
		{
			if (this.parameters != null && this.parameters.expansion != null)
			{
				return this.parameters.expansion.Value;
			}
			ConsolidatedString internalTypeNames = so.InternalTypeNames;
			return DisplayDataQuery.GetEnumerableExpansionFromType(this.expressionFactory, this._typeInfoDataBase, internalTypeNames);
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x00124714 File Offset: 0x00122914
		private void ProcessCoreOutOfBand(PSObject so, int count)
		{
			string msg = StringUtil.Format(FormatAndOut_format_xxx.IEnum_Header, new object[0]);
			this.SendCommentOutOfBand(msg);
			this.ProcessOutOfBand(so);
			switch (count)
			{
			case 0:
				msg = StringUtil.Format(FormatAndOut_format_xxx.IEnum_NoObjects, new object[0]);
				break;
			case 1:
				msg = StringUtil.Format(FormatAndOut_format_xxx.IEnum_OneObject, new object[0]);
				break;
			default:
				msg = StringUtil.Format(FormatAndOut_format_xxx.IEnum_ManyObjects, count);
				break;
			}
			this.SendCommentOutOfBand(msg);
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x00124794 File Offset: 0x00122994
		private void SendCommentOutOfBand(string msg)
		{
			FormatEntryData formatEntryData = OutOfBandFormatViewManager.GenerateOutOfBandObjectAsToString(PSObjectHelper.AsPSObject(msg));
			if (formatEntryData != null)
			{
				this.WriteObject(formatEntryData);
			}
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x001247B8 File Offset: 0x001229B8
		private void ProcessObject(PSObject so)
		{
			if (this.formatObjectDeserializer.IsFormatInfoData(so))
			{
				this.WriteObject(so);
				return;
			}
			if (this.ProcessOutOfBandObjectOutsideDocumentSequence(so))
			{
				return;
			}
			InnerFormatShapeCommandBase.FormattingContext formattingContext = this.contextManager.Peek();
			if (formattingContext.state == InnerFormatShapeCommandBase.FormattingContext.State.none)
			{
				this.viewManager.Initialize(base.TerminatingErrorContext, this.expressionFactory, this._typeInfoDataBase, so, this.shape, this.parameters);
				this.WriteFormatStartData(so);
				this.contextManager.Push(new InnerFormatShapeCommandBase.FormattingContext(InnerFormatShapeCommandBase.FormattingContext.State.document));
			}
			if (this.ProcessOutOfBandObjectInsideDocumentSequence(so))
			{
				return;
			}
			InnerFormatShapeCommand.GroupTransition groupTransition = this.ComputeGroupTransition(so);
			if (groupTransition == InnerFormatShapeCommand.GroupTransition.enter)
			{
				this.PushGroup(so);
				this.WritePayloadObject(so);
				return;
			}
			if (groupTransition == InnerFormatShapeCommand.GroupTransition.exit)
			{
				this.WritePayloadObject(so);
				this.PopGroup();
				return;
			}
			if (groupTransition == InnerFormatShapeCommand.GroupTransition.startNew)
			{
				this.PopGroup();
				this.PushGroup(so);
				this.WritePayloadObject(so);
				return;
			}
			this.WritePayloadObject(so);
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x060035CD RID: 13773 RVA: 0x00124892 File Offset: 0x00122A92
		private bool ShouldProcessOutOfBand
		{
			get
			{
				return this.shape == FormatShape.Undefined || this.parameters == null || !this.parameters.forceFormattingAlsoOnOutOfBand;
			}
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x001248B8 File Offset: 0x00122AB8
		private bool ProcessOutOfBandObjectOutsideDocumentSequence(PSObject so)
		{
			if (!this.ShouldProcessOutOfBand)
			{
				return false;
			}
			if (so.InternalTypeNames.Count == 0)
			{
				return false;
			}
			List<ErrorRecord> errorRecordList;
			FormatEntryData formatEntryData = OutOfBandFormatViewManager.GenerateOutOfBandData(base.TerminatingErrorContext, this.expressionFactory, this._typeInfoDataBase, so, this.enumerationLimit, false, out errorRecordList);
			this.WriteErrorRecords(errorRecordList);
			if (formatEntryData != null)
			{
				this.WriteObject(formatEntryData);
				return true;
			}
			return false;
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x00124918 File Offset: 0x00122B18
		private bool ProcessOutOfBandObjectInsideDocumentSequence(PSObject so)
		{
			if (!this.ShouldProcessOutOfBand)
			{
				return false;
			}
			ConsolidatedString internalTypeNames = so.InternalTypeNames;
			return !this.viewManager.ViewGenerator.IsObjectApplicable(internalTypeNames) && this.ProcessOutOfBand(so);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x00124952 File Offset: 0x00122B52
		private bool ProcessOutOfBand(PSObject so)
		{
			return this.ProcessOutOfBand(so, false);
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x0012495C File Offset: 0x00122B5C
		private bool ProcessOutOfBand(PSObject so, bool isProcessingError)
		{
			List<ErrorRecord> errorRecordList;
			FormatEntryData formatEntryData = OutOfBandFormatViewManager.GenerateOutOfBandData(base.TerminatingErrorContext, this.expressionFactory, this._typeInfoDataBase, so, this.enumerationLimit, true, out errorRecordList);
			if (!isProcessingError)
			{
				this.WriteErrorRecords(errorRecordList);
			}
			if (formatEntryData != null)
			{
				this.WriteObject(formatEntryData);
				return true;
			}
			return false;
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x001249A4 File Offset: 0x00122BA4
		protected void WriteInternalErrorMessage(string message)
		{
			FormatEntryData formatEntryData = new FormatEntryData();
			formatEntryData.outOfBand = true;
			ComplexViewEntry complexViewEntry = new ComplexViewEntry();
			FormatEntry formatEntry = new FormatEntry();
			complexViewEntry.formatValueList.Add(formatEntry);
			formatEntry.formatValueList.Add(new FormatNewLine());
			FormatTextField formatTextField = new FormatTextField();
			formatTextField.text = message;
			formatEntry.formatValueList.Add(formatTextField);
			formatEntry.formatValueList.Add(new FormatNewLine());
			formatEntryData.formatEntryInfo = complexViewEntry;
			this.WriteObject(formatEntryData);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x00124A20 File Offset: 0x00122C20
		private void WriteErrorRecords(List<ErrorRecord> errorRecordList)
		{
			if (errorRecordList == null)
			{
				return;
			}
			foreach (ErrorRecord obj in errorRecordList)
			{
				this.ProcessOutOfBand(PSObjectHelper.AsPSObject(obj), true);
			}
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x00124A7C File Offset: 0x00122C7C
		internal override void EndProcessing()
		{
			for (;;)
			{
				InnerFormatShapeCommandBase.FormattingContext formattingContext = this.contextManager.Peek();
				if (formattingContext.state == InnerFormatShapeCommandBase.FormattingContext.State.none)
				{
					break;
				}
				if (formattingContext.state == InnerFormatShapeCommandBase.FormattingContext.State.group)
				{
					this.PopGroup();
				}
				else if (formattingContext.state == InnerFormatShapeCommandBase.FormattingContext.State.document)
				{
					FormatEndData o = new FormatEndData();
					this.WriteObject(o);
					this.contextManager.Pop();
				}
			}
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x00124AD2 File Offset: 0x00122CD2
		internal void SetCommandLineParameters(FormattingCommandLineParameters commandLineParameters)
		{
			this.parameters = commandLineParameters;
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x00124ADC File Offset: 0x00122CDC
		private InnerFormatShapeCommand.GroupTransition ComputeGroupTransition(PSObject so)
		{
			InnerFormatShapeCommandBase.FormattingContext formattingContext = this.contextManager.Peek();
			if (formattingContext.state == InnerFormatShapeCommandBase.FormattingContext.State.document)
			{
				this.viewManager.ViewGenerator.UpdateGroupingKeyValue(so);
				return InnerFormatShapeCommand.GroupTransition.enter;
			}
			if (!this.viewManager.ViewGenerator.UpdateGroupingKeyValue(so))
			{
				return InnerFormatShapeCommand.GroupTransition.none;
			}
			return InnerFormatShapeCommand.GroupTransition.startNew;
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x00124B28 File Offset: 0x00122D28
		private void WriteFormatStartData(PSObject so)
		{
			FormatStartData o = this.viewManager.ViewGenerator.GenerateStartData(so);
			this.WriteObject(o);
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x00124B50 File Offset: 0x00122D50
		private void WritePayloadObject(PSObject so)
		{
			FormatEntryData formatEntryData = this.viewManager.ViewGenerator.GeneratePayload(so, this.enumerationLimit);
			formatEntryData.SetStreamTypeFromPSObject(so);
			this.WriteObject(formatEntryData);
			List<ErrorRecord> errorRecordList = this.viewManager.ViewGenerator.ErrorManager.DrainFailedResultList();
			this.WriteErrorRecords(errorRecordList);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x00124BA0 File Offset: 0x00122DA0
		private void PushGroup(PSObject firstObjectInGroup)
		{
			GroupStartData o = this.viewManager.ViewGenerator.GenerateGroupStartData(firstObjectInGroup, this.enumerationLimit);
			this.WriteObject(o);
			this.contextManager.Push(new InnerFormatShapeCommandBase.FormattingContext(InnerFormatShapeCommandBase.FormattingContext.State.group));
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x00124BE0 File Offset: 0x00122DE0
		private void PopGroup()
		{
			GroupEndData o = this.viewManager.ViewGenerator.GenerateGroupEndData();
			this.WriteObject(o);
			this.contextManager.Pop();
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x00124C14 File Offset: 0x00122E14
		internal ScriptBlock CreateScriptBlock(string scriptText)
		{
			ScriptBlock scriptBlock = this.OuterCmdlet().InvokeCommand.NewScriptBlock(scriptText);
			scriptBlock.DebuggerStepThrough = true;
			return scriptBlock;
		}

		// Token: 0x04001B76 RID: 7030
		private FormatShape shape;

		// Token: 0x04001B77 RID: 7031
		private MshExpressionFactory expressionFactory;

		// Token: 0x04001B78 RID: 7032
		private FormatObjectDeserializer formatObjectDeserializer;

		// Token: 0x04001B79 RID: 7033
		private TypeInfoDataBase _typeInfoDataBase;

		// Token: 0x04001B7A RID: 7034
		private FormattingCommandLineParameters parameters;

		// Token: 0x04001B7B RID: 7035
		private FormatViewManager viewManager = new FormatViewManager();

		// Token: 0x04001B7C RID: 7036
		private int enumerationLimit = 4;

		// Token: 0x020004D0 RID: 1232
		private enum GroupTransition
		{
			// Token: 0x04001B7E RID: 7038
			none,
			// Token: 0x04001B7F RID: 7039
			enter,
			// Token: 0x04001B80 RID: 7040
			exit,
			// Token: 0x04001B81 RID: 7041
			startNew
		}
	}
}
